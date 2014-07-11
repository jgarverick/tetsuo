using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.MsmqIntegration;
using System.ServiceModel.Description;
using Tetsuo.Common.Contracts;

namespace Tetsuo.Core.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "Tetsuo.Core")]
    [ServiceKnownType(typeof(RoutedMessageContract))]
    public interface IRouteDispatcher:IContractBehavior
    {

        [OperationContract(IsOneWay = true, Action = "*")]
        void Route(MsmqMessage<RoutedMessageContract> value);

        void Start(MsmqMessage<RoutedMessageContract> value);

        void Stop(MsmqMessage<RoutedMessageContract> value);

        void Park(MsmqMessage<RoutedMessageContract> value);

        void Execute(MsmqMessage<RoutedMessageContract> value);
    }



}
