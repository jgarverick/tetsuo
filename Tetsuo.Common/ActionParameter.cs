using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetsuo.Common
{
    [Serializable]
    public class ActionParameter
    {
        public string Name { get; set; }
        public string ParamType { get; set; }
        public object Value { get; set; }
    }
}
