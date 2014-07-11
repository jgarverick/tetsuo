using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.ServiceModel.Description;
using System.Data.SqlClient;
using System.Reflection;
using System.ServiceModel.Channels;
using Tetsuo.Core.Contracts;
using Tetsuo.Entities;
using Tetsuo.Entities.Model;
using Tetsuo.Core.Common;

namespace Tetsuo.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceResolverGateway:IServiceResolverGateway
    {
        Exception _baseException;
        string _baseMessage;
        System.ServiceModel.Channels.Binding binding;

        public ServiceResolverGateway()
        {
            ServiceHubs = new List<IServiceHub>();
            Hosts = new List<ServiceHost>();
            Assemblies = new List<Assembly>();
            
            //bElement = new TcpTransportBindingElement();
            binding = new BasicHttpBinding(); //new CustomBinding(bElement);
        }
        

        #region IServiceResolverGateway Members

        public IBaseServiceContract ResolvedContract
        {
            get;
            set;
        }

        public void OnInitializeGateway(string HubContract, Uri baseAddress)
        {
            
            // Go out, get the group config files and read them in
            // This will build out the architecture of the current
            // gateway and show what hubs/spokes can be instantiated.

            InitializeHubs(baseAddress);

            AddServices(HubContract, baseAddress.OriginalString);

            InitializeAllServices();
            

        }
        public List<ServiceHost> Hosts { get; set; }
        public List<Assembly> Assemblies { get; set; }

        private void AddServices(string HubContract, string address)
        {
            for (int i = 0; i < ServiceHubs.Count; i++)
            {
                Hosts.Add(new ServiceHost(typeof(ServiceHub), 
                    new Uri(ServiceHubs[i].DestinationAddress + "/")));
                Hosts[i].AddServiceEndpoint(HubContract, binding,
                ServiceHubs[i].DestinationAddress)
                .Name = ServiceHubs[i].Name;
                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                behavior.HttpGetEnabled = true;
                //behavior.HttpGetUrl = new Uri(ServiceHubs[i].DestinationAddress);
                Hosts[i].Description.Behaviors.Add(behavior);
                Hosts[i].AddServiceEndpoint(typeof(IMetadataExchange), binding,
                "MEX");
            }
            
        }

        private void InitializeAllServices()
        {
            // Open the hubs
            for (int j = 0; j < Hosts.Count; j++)
            {
                try
                {
                    Hosts[j].Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                    break;
                }
                Console.WriteLine("Gateway: successfully started hub at endpoint {0}.",
                    ServiceHubs[j].DestinationAddress);
                // Now open the spokes
                for (int k = 0; k < ServiceHubs[j].Hosts.Count; k++) 
                {
                    ServiceHubs[j].Hosts[k].Open();
                    Console.WriteLine("Gateway: successfully started spoke service at endpoint {0}.",
                    ServiceHubs[j].Spokes[k].EndpointName);
                }
            }
        }

        private void InitializeHubs(Uri address)
        {
            EntityManager em = new EntityManager();
            List<Hub> hubs = em.GetHubsByGateway(address);
            foreach(var hub in hubs)
            {
                IServiceHub currentHub = new ServiceHub();
                currentHub.Name = hub.HubName;
                currentHub.OriginAddress = address.OriginalString;
                currentHub.DestinationAddress = address.OriginalString + hub.HubEndpoint;
                ServiceHubs.Add(currentHub);
                Console.WriteLine("Gateway: successfully created endpoint {0}.",
                    currentHub.DestinationAddress);
                currentHub.InitializeSpokes(hub.HubId,
                    currentHub.OriginAddress, currentHub.DestinationAddress);
            }
            
        }

        public void OnResolveSpoke(string HubName, string SpokeName)
        {
           
        }

        public bool IsAuthenticated
        {
            get;
            set;
        }

        public bool IsAuthorized
        {
            get;
            set;
        }

        public List<IServiceHub> ServiceHubs
        {
            get;
            set;
        }

        public bool ResolveHub(string uri)
        {

            Console.WriteLine("Attempting to resolve ServiceHub {0}...", uri);
            try
            {
                foreach (ServiceHub hub in ServiceHubs)
                {
                    Console.WriteLine("Hub {0} has {1} for its Origin, {2} for its Destination.", hub.Name, hub.OriginAddress, hub.DestinationAddress);
                    if (hub.DestinationAddress == uri ||
                        hub.OriginAddress == uri)
                    {
                        CurrentHub = hub;
                        return true;
                    }
                }
                BaseServiceMessage = string.Format(
                    "The service hub with the address {0} could not be found. {1} total Hub(s) searched.",uri,ServiceHubs.Count);
                Console.WriteLine(BaseServiceMessage);
                return false;
            }
            catch (Exception ex)
            {
                BaseServiceException = ex;
                BaseServiceMessage = "The command failed.  Please see the BaseServiceException for details.";
                Console.WriteLine(BaseServiceMessage);
            }
            return false;
        }

        public void OnShutdownGateway()
        {
            for (int i = 0; i < Hosts.Count; i++)
            {
                for (int j = 0; j < ServiceHubs[i].Hosts.Count; j++)
                {
                    ServiceHubs[i].Hosts[j].Close();
                }
                Hosts[i].Close();
            }
        }

        #endregion
        #region IBaseServiceContract Members

        public string GetCoreObject(params string[] values)
        {
            throw new NotImplementedException();
        }

        public Exception BaseServiceException
        {
            get { return _baseException; }
            set { _baseException = value; }
        }

        public string BaseServiceMessage
        {
            get
            {
                return _baseMessage;
            }
            set
            {
                _baseMessage = value;
            }
        }

        public string GetServiceMessage()
        {
            return _baseMessage;
        }


        public IServiceSpoke ResolveSpoke(string HubName, string SpokeName)
        {
            throw new NotImplementedException();
        }

        public ServiceHub CurrentHub
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        #endregion


        Core.Common.ServiceHub IServiceResolverGateway.CurrentHub
        {
            get;
            set;
        }
    }
}
