using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tetsuo.Common;

namespace Tetsuo.Core.Common
{
    public class InspectedMessageHeaderEventArgs:EventArgs
    {
        public string Hub { get; set; }
        public string Spoke { get; set; }
        public TransmissionStatusCodes StatusCode { get; set; }
    }
}