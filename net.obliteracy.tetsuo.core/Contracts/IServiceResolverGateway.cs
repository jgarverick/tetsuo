using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Reflection;
using Tetsuo.Core.Common;

namespace Tetsuo.Core.Contracts
{
    [ServiceContract]
    public interface IServiceResolverGateway
    {
        [OperationContract]
        void OnShutdownGateway();

        [OperationContract]
        void OnInitializeGateway(string HubContract,Uri baseAddress);

        [OperationContract]
        bool ResolveHub(string uri);

        [OperationContract]
        IServiceSpoke ResolveSpoke(string HubName, string SpokeName);

        bool IsAuthenticated { get; set; }
        bool IsAuthorized { get; set; }
        List<IServiceHub> ServiceHubs { get; set; }
        ServiceHub CurrentHub { get; set; }
        List<ServiceHost> Hosts { get; set; }
        List<Assembly> Assemblies { get; set; }
        IBaseServiceContract ResolvedContract { get; set; }
        string Name { get; set; }
        
    }
}
