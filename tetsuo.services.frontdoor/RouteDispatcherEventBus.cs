using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tetsuo.services.frontdoor
{
    internal class RouteDispatcherEventBus
    {
        public delegate void MessageHeaderInspected(object sender,InspectedMessageHeaderEventArgs e);
        public static event MessageHeaderInspected OnMessageHeaderInspected;

        public static void InspectMessage(object sender,InspectedMessageHeaderEventArgs e)
        {
            if (!(OnMessageHeaderInspected == null))
                OnMessageHeaderInspected(sender,e);
        }
    }
}