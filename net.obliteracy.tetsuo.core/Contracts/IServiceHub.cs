﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Tetsuo.Core.Contracts
{
    [ServiceContract]
    public interface IServiceHub : IBaseServiceContract
    {
        string Name { get; set; }
        string OriginAddress { get; set; }
        string DestinationAddress { get; set; }
        List<IServiceSpoke> Spokes { get; set; }
        [OperationContract]
        IServiceSpoke GetServiceSpoke();
        List<ServiceHost> Hosts { get; set; }
        void InitializeSpokes(int HubId, string origin, string destination);
    }
}
