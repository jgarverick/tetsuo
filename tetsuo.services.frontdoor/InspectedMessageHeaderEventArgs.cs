using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tetsuo.services.frontdoor
{
    internal class InspectedMessageHeaderEventArgs:EventArgs
    {
        public string Hub { get; set; }
        public string Spoke { get; set; }
    }
}