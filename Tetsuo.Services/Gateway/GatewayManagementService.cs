using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetsuo.Services
{
    public class GatewayManagementService
    {
        protected GatewayFileManager fileManager;
        protected RouteDispatcher dispatcher;

        public GatewayManagementService(int gatewayId)
        {
            Task fileTask = new Task(() =>
            {
                fileManager = new GatewayFileManager();
            });

            Task dispatchTask = new Task(() =>
            {
                dispatcher = new RouteDispatcher();
            });

            fileTask.Start();
            dispatchTask.Start();
        }


    }
}
