using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.MsmqIntegration;
using System.ServiceModel.Channels;
using Tetsuo.Common.Contracts;

namespace Tetsuo.Core.Common
{
    public class OperationSelector : IDispatchOperationSelector
    {
        public string SelectOperation(ref System.ServiceModel.Channels.Message message)
        {
            MsmqIntegrationMessageProperty property = MsmqIntegrationMessageProperty.Get(message);
            return property.Label ?? "Unknown";
        }
    }

    
}