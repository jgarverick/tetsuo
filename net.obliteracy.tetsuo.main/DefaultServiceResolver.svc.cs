using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using net.obliteracy.tetsuo.contract;
using HealthNow.Framework.Contract.ServiceContracts;
using net.obliteracy.tetsuo.core;
using net.obliteracy.tetsuo.core.contracts;
using net.obliteracy.tetsuo.core.services;

namespace net.obliteracy.tetsuo
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class DefaultServiceResolver 
    {
        ServiceHost host;
        public DefaultServiceResolver()
        {
            host = new ServiceHost(typeof(DefaultServiceResolver));
            System.ServiceModel.Channels.Binding binding = new BasicHttpBinding();
            host.AddServiceEndpoint("net.obliteracy.tetsuo.core.contracts.IServiceHub", binding,
                "http://localhost:4872/service/Port1").Name = "Hub1";
            host.AddServiceEndpoint("net.obliteracy.tetsuo.core.contracts.IServiceHub", binding,
                "http://localhost:4872/service/Port2").Name = "Hub2";
            host.AddServiceEndpoint("net.obliteracy.tetsuo.core.contracts.IServiceHub", binding,
                "http://localhost:4872/service/Port3").Name = "Hub3";
        }
        #region IServiceResolverGateway Members

        public IBaseServiceContract ResolvedContract
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void OnInitializeGateway()
        {
            throw new NotImplementedException();
        }

        public bool ResolveHub(string uri)
        {
            try
            {
                CurrentHub = new ServiceHub();
                CurrentHub.DestinationAddress = uri;
                return true;
            }
            catch (Exception ex)
            {
                //BaseServiceException = ex;
                //BaseServiceMessage = "The command failed.  Please see the BaseServiceException for details.";

            }
            return false;
        }

        public IServiceSpoke ResolveSpoke(string HubName, string SpokeName)
        {
            throw new NotImplementedException();
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

        public net.obliteracy.tetsuo.core.services.ServiceHub CurrentHub
        {
            get;
            set;
        }

        #endregion
    }

}
