using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace net.obliteracy.tetsuo.contract.DataContracts
{
    [MessageContract]
    public class RoutedMessageContract
    {
        [MessageHeader]
        public string DestinationHub { get; set; }
        [MessageHeader]
        public string DestinationSpoke { get; set; }
        [MessageBodyMember]
        public string MessageBody { get; set; }
    }
}
