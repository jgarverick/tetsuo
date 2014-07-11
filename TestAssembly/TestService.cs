using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestAssembly
{
    public class TestService:ITestService
    {
        public void DoSomething()
        {
            Console.WriteLine("This is a DoSomething method.");
        }
        public void DoSomethingElse()
        {
            Thread.Sleep(5000);
            Console.WriteLine("This is a DoSomethingElse method.");
        }
    }
}
