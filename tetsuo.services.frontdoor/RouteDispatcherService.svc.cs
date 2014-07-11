using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using net.obliteracy.tetsuo.entities;
using net.obliteracy.tetsuo.contract.DataContracts;

namespace tetsuo.services.frontdoor
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class RouteDispatcher : IRouteDispatcher
    {
        private string destinationHub;
        private string destinationSpoke;

        public RouteDispatcher()
        {
            RouteDispatcherEventBus.OnMessageHeaderInspected += new RouteDispatcherEventBus.MessageHeaderInspected(RouteDispatcherEventBus_OnMessageHeaderInspected);
        }

        void RouteDispatcherEventBus_OnMessageHeaderInspected(object sender, InspectedMessageHeaderEventArgs e)
        {
            destinationHub = e.Hub;
            destinationSpoke = e.Spoke;
        }

        [OperationBehavior(TransactionAutoComplete=true,TransactionScopeRequired=true)]
        public void Route(System.ServiceModel.MsmqIntegration.MsmqMessage<RoutedMessageContract> value)
        {
            //throw new NotImplementedException();
            // Scan the repository for the appropriate endpoint, then get the inbound queue for it
            EntityManager em = new EntityManager();
            
        }

        public void AddBindingParameters(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyClientBehavior(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            //throw new NotImplementedException();
            clientRuntime.MessageInspectors.Add(new DynamicInspector());
        }

        public void ApplyDispatchBehavior(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.OperationSelector = new OperationSelector();
        }

        public void Validate(System.ServiceModel.Description.ContractDescription contractDescription, System.ServiceModel.Description.ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }

    }
}
