using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tetsuo.Core.Contracts;
using Tetsuo.Common.Contracts;
using Tetsuo.Core.Common;
using Tetsuo.Entities;
using System.Threading;
using System.ServiceModel.Description;
using System.ServiceModel.MsmqIntegration;
using System.Messaging;
using Tetsuo.Entities.Model;
using System.Reflection;
using System.Threading.Tasks;

namespace Tetsuo.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RouteDispatcher : TetsuoServiceBase
    {

        public RouteDispatcher()
        {

            this.ObjectID = 1;
            this.Source = Common.InstrumentationSources.Gateway;
            Initialize();
        }

        public override void Route(MsmqMessage<RoutedMessageContract> value)
        {
            value.Body.IsFromGateway = true;
            base.Route(value);
        }

        public override void Start(MsmqMessage<RoutedMessageContract> value)
        {
            Instrument(Common.TransmissionStatusCodes.SVC_START, "Starting route dispatcher...", Common.InstrumentationSources.Hub, this.ObjectID);
            base.Start(value);
        }

        private void Initialize()
        {
            EntityManager em = new EntityManager();
            List<Hub> hubs = em.GetHubsByGateway(ObjectID);
            foreach (var hub in hubs)
            {
                List<HubService> spokes = em.GetSpokesByHub(hub.HubId);
                //ServiceHost host =
                //     CreateServiceHost<TetsuoHubService>("Tetsuo.Core.Contracts.IRouteDispatcher", hub.HubName + ".request");
                //(host.SingletonInstance as TetsuoHubService).SetObjectID(Common.InstrumentationSources.Hub, hub.HubId);
                Instrument(Common.TransmissionStatusCodes.SVC_START, "Starting hub " + hub.HubName, Common.InstrumentationSources.Hub, hub.HubId);

                spokes.ForEach(spoke =>
                {
                    string qName = string.Format(@"{0}\private$\{1}.{2}.request",Environment.MachineName,
                        hub.HubName,spoke.HubServiceName);
                    if(!(MessageQueue.Exists(qName)))
                        MessageQueue.Create(qName,true);
                    ServiceHost spokeHost =
                     CreateServiceHost<TetsuoHubService>("Tetsuo.Core.Contracts.IRouteDispatcher", hub.HubName + "." + spoke.HubServiceName + ".request");
                    //spokeHost.AddServiceEndpoint(spoke.SpokeContract, new MsmqIntegrationBinding(), @"msmq.formatname:DIRECT=OS:.\private$\" + hub.HubName + "." + spoke.SpokeName + ".request");
                    (spokeHost.SingletonInstance as TetsuoHubService).SetObjectID(Common.InstrumentationSources.Spoke, spoke.HubServiceId);
                    Assembly asm = Assembly.LoadFrom(spoke.HubServiceAssembly);
                    if (!(spokeHost.SingletonInstance as TetsuoHubService).Assemblies.Contains(asm))
                        (spokeHost.SingletonInstance as TetsuoHubService).Assemblies.Add(asm);
                    (spokeHost.SingletonInstance as TetsuoHubService).HostedContract = asm.CreateInstance(spoke.ClientClass);
                    serviceThreads.Add(spokeHost);
                    Instrument(Common.TransmissionStatusCodes.SVC_START, "Service for " + spoke.HubServiceName + " successfully started.", Common.InstrumentationSources.Spoke, spoke.HubServiceId);

                });
                //host.Open();
                //serviceThreads.Add(host);
            }
        }

    }
}
