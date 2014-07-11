﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Tetsuo.Core.Contracts
{
    [ServiceContract]
    public interface IServiceSpoke : IBaseServiceContract
    {
        string Name { get; set; }
        string EndpointName { get; set; }
        string ContractName { get; set; }
        bool CanConnect { get; set; }
        bool IsActive { get; set; }
    }
}
