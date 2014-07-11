using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetsuo.Core.Common;
using Tetsuo.Common;

namespace Tetsuo.Core.Contracts
{
    public interface IGatewayManagerService
    {
        void Startup();
        bool Resolve(string Url,ReslovedEnpointTypes endpointType);
        string[] GetGateways(string machineAddress);
        string[] GetHubs(string machineAddress, string gateway);
        void ShutdownGateway(string machineAddress, string gateway);
        void Shutdown(string machineAddress);
        bool StopService(string serviceName);
        bool StartService(string serviceName);
    }
}
