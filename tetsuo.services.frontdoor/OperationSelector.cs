using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.MsmqIntegration;
using System.ServiceModel.Channels;

namespace tetsuo.services.frontdoor
{
    public class OperationSelector : IDispatchOperationSelector
    {
        public string SelectOperation(ref System.ServiceModel.Channels.Message message)
        {
            MsmqIntegrationMessageProperty property = MsmqIntegrationMessageProperty.Get(message);
            return property.Label;
        }
    }

    public class DynamicInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Inspect(reply.Headers.ToList());
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {

            Inspect(request.Headers.ToList());
            return null;
        }

        private void Inspect(List<MessageHeaderInfo> headers)
        {
            InspectedMessageHeaderEventArgs args = new InspectedMessageHeaderEventArgs();
            headers.ForEach(header =>
            {
                if (header.Name == "DestinationHub")
                    args.Hub = header.ToString();
                if (header.Name == "DestinationSpoke")
                    args.Spoke = header.ToString();
            });
            RouteDispatcherEventBus.InspectMessage(this, args);
        }
    }
}