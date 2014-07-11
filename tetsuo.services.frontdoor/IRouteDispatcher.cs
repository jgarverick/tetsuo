using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.MsmqIntegration;
using System.ServiceModel.Description;
using net.obliteracy.tetsuo.contract.DataContracts;

namespace tetsuo.services.frontdoor
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace="tetsuo.services.frontdoor")]
    [ServiceKnownType(typeof(RoutedMessageContract))]
    public interface IRouteDispatcher:IContractBehavior
    {

        [OperationContract(IsOneWay = true, Action = "*")]
        void Route(MsmqMessage<RoutedMessageContract> value);

        //[OperationContract]
        //void DoSomeWork();
        // TODO: Add your service operations here
    }



}
