using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAssembly
{
    public class ReturnService:IReturnService
    {
        public string Multiply(int x, int y)
        {
            return string.Format("The value of {0} multiplied by {1} is {2}.",
                x, y, x * y);
        }
    }
}
