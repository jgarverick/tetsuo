using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TestAssembly
{
    [ServiceContract(Namespace = "TestAssembly")]
    interface IReturnService
    {
        [OperationContract]
        string Multiply(int x, int y);
    }
}
