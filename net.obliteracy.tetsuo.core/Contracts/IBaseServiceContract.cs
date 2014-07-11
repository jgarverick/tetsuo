using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Tetsuo.Core.Contracts
{
    [ServiceContract]
    public interface IBaseServiceContract
    {
        [OperationContract]
        string GetCoreObject(params string[] values);
        Exception BaseServiceException { get; set; }
        string BaseServiceMessage { get; set; }
        [OperationContract]
        string GetServiceMessage();
        bool IsActive { get; set; }
    }
}
