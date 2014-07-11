using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Tetsuo.Core.Contracts;
using System.Threading;
using System.ServiceModel.MsmqIntegration;
using Tetsuo.Common.Contracts;
using Tetsuo.Entities;
using System.Messaging;
using Tetsuo.Common;
using System.Threading.Tasks;

namespace Tetsuo.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TetsuoServiceBase:IRouteDispatcher
    {
        protected List<ServiceHost> serviceThreads = new List<ServiceHost>();
        protected MessageQueue RequestQueue { get; set; }
        protected MessageQueue ResponseQueue { get; set; }
        protected InstrumentationSources Source { get; set; }
        protected bool IsGateway { get; set; }
        protected int ObjectID { get; set; }

        protected EntityManager em = new EntityManager();

        [OperationBehavior(TransactionAutoComplete = true, TransactionScopeRequired = true)]
        public virtual void Route(System.ServiceModel.MsmqIntegration.MsmqMessage<Tetsuo.Common.Contracts.RoutedMessageContract> value)
        {
            // Scan the repository for the appropriate endpoint, then get the inbound queue for it
            Instrument(value.Body.StatusCode, "Message received.", Source, ObjectID, value.Id);
            switch (value.Body.StatusCode)
            {
                case Common.TransmissionStatusCodes.SVC_START:
                    Start(value);
                    break;
                case Common.TransmissionStatusCodes.SVC_STOP:
                    Stop(value);
                    break;
                case Common.TransmissionStatusCodes.PARK:
                    Park(value);
                    break;
                case Common.TransmissionStatusCodes.BEGIN:
                    Execute(value);
                    break;
                case Common.TransmissionStatusCodes.EOT_ERR:
                case Common.TransmissionStatusCodes.EOT_GOOD:
                    Instrument(value.Body.StatusCode, value.Body.MessageBody.ToString(), Source, ObjectID, value.Id);
                    ResponseQueue.Send(new Message(value.Body) { Label = value.Label });
                    break;
                default:
                    Console.WriteLine(value.Label);
                    break;
            }
        }

        public virtual void Start(System.ServiceModel.MsmqIntegration.MsmqMessage<Tetsuo.Common.Contracts.RoutedMessageContract> value)
        {
            RoutedMessageContract ct = value.Body;
            string serviceName = string.Format("{0}.{1}.request", ct.DestinationHub, ct.DestinationSpoke);
            string contract = em.GetSpokeContract(ct.DestinationHub, ct.DestinationSpoke);
            ServiceHost t = serviceThreads.Where(x => 
                x.Description.Endpoints.Where(y => 
                    y.Address.Uri.AbsolutePath.EndsWith(serviceName)).Any()).FirstOrDefault();
            if (!(t == null))
                if (t.State != CommunicationState.Opened)
                    try
                    {
                        t = CreateServiceHost<TetsuoHubService>(contract, serviceName);
                        Instrument(ct.StatusCode, string.Format("Host for {0} has successfully opened.", t.Description.Name), Source, ObjectID);

                        // send a success
                    }
                    catch (Exception ex)
                    {
                        // send a new EOT message with the issue
                        Console.WriteLine(ex.Message);
                    }
        }

        public virtual void Stop(System.ServiceModel.MsmqIntegration.MsmqMessage<Tetsuo.Common.Contracts.RoutedMessageContract> value)
        {
            RoutedMessageContract ct = value.Body;
            string serviceName = string.Format("{0}.{1}.request", ct.DestinationHub, ct.DestinationSpoke);
            ServiceHost t = serviceThreads.Where(x => 
                x.Description.Endpoints.Where(y => 
                    y.Address.Uri.AbsolutePath.EndsWith(serviceName)).Any()).FirstOrDefault();
            if (!(t == null))
                if (t.State == CommunicationState.Opened)
                    try
                    {
                        t.Close();
                        Instrument(ct.StatusCode, string.Format("Host for {0} has successfully closed.", t.Description.Name), Source, ObjectID);
                        // send a success
                    }
                    catch (Exception ex)
                    {
                        // send a new EOT message with the issue
                    }
        }

        public virtual void Park(System.ServiceModel.MsmqIntegration.MsmqMessage<Tetsuo.Common.Contracts.RoutedMessageContract> value)
        {
            //throw new NotImplementedException();
        }

        public virtual void AddBindingParameters(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public virtual void ApplyClientBehavior(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            //throw new NotImplementedException();
        }

        public virtual void ApplyDispatchBehavior(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {
            //throw new NotImplementedException();
        }

        public virtual void Validate(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }

        protected void host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Host for {0} has faulted.", (sender as ServiceHost).Description.Name);
            (sender as ServiceHost).Abort();
        }

        protected ServiceHost CreateServiceHost<T>(string contract, string qName, bool isGateway = false)
        where T:new()
        {
            ServiceHost host = new ServiceHost(new T());
            host.Faulted += new EventHandler(host_Faulted);
            (host.SingletonInstance as TetsuoHubService).ResponseQueue = new MessageQueue(@".\private$\response");
            Thread thread = new Thread(() =>
            {
                host.AddServiceEndpoint(contract,
                        new MsmqIntegrationBinding(MsmqIntegrationSecurityMode.None),
                        string.Format(@"msmq.formatname:DIRECT=OS:.\private$\{0}", qName));
                host.Open();
            });
            thread.Start();
            return host;
        }

        protected void Instrument(TransmissionStatusCodes status, string message, InstrumentationSources source, int objectID=0,string messageID="")
        {
            Console.WriteLine("[{0} {1}]: {2}", status, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"), message);
            if (source == InstrumentationSources.System)
            {
                em.Instrument(source.ToString(), message, status.ToString(),null,messageID);
            }
            else
            {
                em.Instrument(source.ToString(), message, status.ToString(), objectID,messageID);
            }
                    
        }


        public virtual void Execute(MsmqMessage<RoutedMessageContract> value)
        {
            //throw new NotImplementedException();
        }
    }
}
