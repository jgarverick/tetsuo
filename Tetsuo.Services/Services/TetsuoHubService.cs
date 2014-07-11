using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetsuo.Core.Contracts;
using System.ServiceModel;
using Tetsuo.Common;
using System.Reflection;
using Extensionista;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.ServiceModel.MsmqIntegration;
using Tetsuo.Common.Contracts;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace Tetsuo.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TetsuoHubService : TetsuoServiceBase
    {
        public List<Assembly> Assemblies { get; set; }
        public object HostedContract { get; set; }

        public TetsuoHubService()
        {
            Assemblies = new List<Assembly>();
        }

        public override void Route(System.ServiceModel.MsmqIntegration.MsmqMessage<Common.Contracts.RoutedMessageContract> value)
        {
            if (!value.Body.IsFromGateway)
            {
                value.Body.StatusCode = Common.TransmissionStatusCodes.EOT_ERR;
                value.Body.MessageBody = "Message was sent illegally (not via gateway).";
            }
            base.Route(value);
        }

        public void SetObjectID(InstrumentationSources source, int id)
        {
            this.Source = source;
            this.ObjectID = id;
        }

        public override void Start(System.ServiceModel.MsmqIntegration.MsmqMessage<Common.Contracts.RoutedMessageContract> value)
        {
            
        }
        
        public override void Execute(System.ServiceModel.MsmqIntegration.MsmqMessage<Common.Contracts.RoutedMessageContract> value)
        {
            string paramString = string.Empty;

            Task task = new Task(() =>
            {
                try
                {
                    MethodInfo info = HostedContract.GetType()
                        .GetMethod(value.Body.Action);
                    // Strip out parameters from the message contract
                    object val = value.Body.MessageBody;
                    List<ActionParameter> parms = new List<ActionParameter>();
                    XmlSerializer serial = new XmlSerializer(typeof(List<ActionParameter>));
                    using (StringReader sr = new StringReader(value.Body.MessageBody.ToString()))
                    {
                        parms.AddRange((List<ActionParameter>)serial.Deserialize(sr));
                        parms.ForEach(p => paramString += string.Format("{0} ({1}): {2}\r\n", p.Name, p.ParamType, p.Value.ToString() ?? string.Empty));
                    }
                    // Execute the function based on action name
                    object retval = null;
                    object locker = 4;
                    lock (locker)
                    {
                        retval = info.Invoke(HostedContract, parms.Select(x => x.Value).ToArray());
                    };
                    MsmqMessage<RoutedMessageContract> msg = new MsmqMessage<RoutedMessageContract>(
                        new RoutedMessageContract()
                        {
                            StatusCode = TransmissionStatusCodes.EOT_GOOD,
                            MessageBody = retval ?? string.Format("Call to {0}::{1} successful",
                                this.HostedContract.GetType().Name, value.Body.Action),
                            IsFromGateway = true,
                            Action = "",
                            DestinationHub = "",
                            DestinationSpoke = "",
                        });
                    msg.Label = "Success";
                    msg.CorrelationId = value.Id;
                    Route(msg);
                }
                catch (Exception ex)
                {
                    string errOutput = ex.Message + "\r\n" + ex.StackTrace +
                             "\r\n" +
                             string.Format("Function: {0}.{1}",
                             HostedContract.GetType().Name, value.Body.Action) + "\r\n" +
                             "Parameter(s):" + "\r\n" + paramString;
                    Instrument(TransmissionStatusCodes.EOT_ERR, errOutput, InstrumentationSources.Spoke, this.ObjectID, value.Id);
                    MsmqMessage<RoutedMessageContract> msg = new System.ServiceModel.MsmqIntegration.MsmqMessage<Common.Contracts.RoutedMessageContract>(
                        new Common.Contracts.RoutedMessageContract()
                        {
                            StatusCode = TransmissionStatusCodes.EOT_ERR,
                            MessageBody = "An error has occurred. Please consult the application log for more details." ,
                            IsFromGateway = true,
                            Action = value.Body.Action,
                            DestinationHub = value.Body.DestinationHub,
                            DestinationSpoke = value.Body.DestinationSpoke,
                        }) { Label = "Error" };
                    msg.CorrelationId = value.Id;
                    Route(msg);
                }
            });
            task.Start();

        }
    }
}
