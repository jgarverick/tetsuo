using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using Tetsuo.Core.Contracts;
using Tetsuo.Entities;
using Tetsuo.Entities.Model;

namespace Tetsuo.Core.Common
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceHub : IServiceHub
    {
        System.ServiceModel.Channels.Binding binding;
        public ServiceHub()
        {
            Spokes = new List<IServiceSpoke>();
            Hosts = new List<ServiceHost>();

            //bElement = new TcpTransportBindingElement();
            //binding = new CustomBinding(bElement);
            binding = new BasicHttpBinding();

        }
        #region IServiceHub Members

        public string Name
        {
            get;
            set;
        }

        public string OriginAddress
        {
            get;
            set;
        }

        public string DestinationAddress
        {
            get;
            set;
        }

        public List<IServiceSpoke> Spokes
        {
            get;
            set;
        }

        public void InitializeSpokes(int HubId, string origin, string destination)
        {
            int i = 0;
            EntityManager em = new EntityManager();
            List<Spoke> spokes = em.GetSpokesByHub(HubId);
            foreach(var spoke in spokes)
            {
                IServiceSpoke currentSpoke = new ServiceSpoke();
                currentSpoke.Name = spoke.SpokeName;
                currentSpoke.EndpointName = spoke.SpokeEndpoint;
                currentSpoke.ContractName = spoke.SpokeContract;
                currentSpoke.IsActive = spoke.Active.Value;
                if (currentSpoke.IsActive)
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFile(spoke.SpokeAssembly);
                        Type spokeType = asm.GetType(spoke.SpokeClientClass);
                        Hosts.Add(new ServiceHost(spokeType, new Uri(
                            string.Format(destination + "/{0}/",
                            currentSpoke.EndpointName))));
                        binding = GetBinding(spoke.SpokeBinding);
                        Hosts[i].AddServiceEndpoint(currentSpoke.ContractName, binding,
                        destination + "/" + currentSpoke.EndpointName);
                        ServiceMetadataBehavior behavior;
                        behavior = Hosts[i].Description.Behaviors.Find<ServiceMetadataBehavior>();
                        if (behavior == null)
                        {
                            behavior = new ServiceMetadataBehavior();
                            Hosts[i].Description.Behaviors.Add(behavior);
                        }
                        behavior.HttpGetEnabled = true;
                        ServiceDebugBehavior debugBehavior;
                        debugBehavior = Hosts[i].Description.Behaviors.Find<ServiceDebugBehavior>();
                        if (debugBehavior == null)
                        {
                            debugBehavior = new ServiceDebugBehavior();
                            debugBehavior.IncludeExceptionDetailInFaults = true;
                            Hosts[i].Description.Behaviors.Add(debugBehavior);
                        }
                        //behavior.HttpGetUrl = new Uri(destination + "/" + currentSpoke.EndpointName);
                        Hosts[i].AddServiceEndpoint(typeof(IMetadataExchange), binding,
                        "MEX");
                        i++;
                        Spokes.Add(currentSpoke);
                        Console.WriteLine("ServiceHub: successfully created spoke at endpoint {0}.",
                           destination + "/" + currentSpoke.EndpointName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ServiceHub: error encountered when created spoke at endpoint {0}.\r\n{1}",
                           destination + "/" + currentSpoke.EndpointName,ex.Message);
                    }
                }
            }

        }

        private Binding GetBinding(string bindingType)
        {
            switch (bindingType)
            {
                case "basicHttpBinding":
                    return new BasicHttpBinding()
                    {
                        BypassProxyOnLocal = true,
                        TransferMode = TransferMode.Buffered,
                        Security = new BasicHttpSecurity() { Mode = BasicHttpSecurityMode.None },
                    };
                case "wsHttpBinding":
                    return new WSHttpBinding()
                    {
                        BypassProxyOnLocal = true,
                        Security = new WSHttpSecurity() { Mode = SecurityMode.Transport },
                    };
                default:
                    return new BasicHttpBinding();
            }
        }

        public List<ServiceHost> Hosts { get; set; }

        public IServiceSpoke GetServiceSpoke()
        {
            try
            {

                // TODO: Add logic here to fill in availability
                BaseServiceMessage = "OK.";
                return null;
            }
            catch (Exception ex)
            {
                BaseServiceException = ex;
                BaseServiceMessage = "The operation failed.  Please see the BaseServiceException for further details.";
            }
            return null;
        }

        #endregion

        #region IBaseServiceContract Members

        public string GetCoreObject(params string[] values)
        {
            return string.Empty;
        }

        public Exception BaseServiceException
        {
            get;
            set;
        }

        public string BaseServiceMessage
        {
            get;
            set;
        }

        public string GetServiceMessage()
        {
            return BaseServiceMessage;
        }

        public bool IsActive
        {
            get;
            set;
        }

        #endregion
    }
}
