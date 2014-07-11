using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Tetsuo.Common.Contracts
{
    [MessageContract]
    [XmlInclude(typeof(ActionParameter))]
    [ServiceKnownType(typeof(ActionParameter))]
    public class RoutedMessageContract
    {
        [MessageHeader]
        public string DestinationHub { get; set; }
        [MessageHeader]
        public string DestinationSpoke { get; set; }
        [MessageHeader]
        public string Action { get; set; }
        [MessageHeader]
        public TransmissionStatusCodes StatusCode { get; set; }
        [MessageHeader]
        public bool IsFromGateway { get; set; }
        [MessageBodyMember]
        public object MessageBody { get; set; }
    }
}
