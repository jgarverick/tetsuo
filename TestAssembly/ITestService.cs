using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TestAssembly
{
    [ServiceContract(Namespace="TestAssembly")]
    public interface ITestService
    {
        [OperationContract]
        void DoSomething();
    }
}
