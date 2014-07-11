using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetsuo.Core.Contracts;

namespace Tetsuo.Core.Common
{
    public class ServiceSpoke:IServiceSpoke
    {
        #region IServiceSpoke Members

        public string Name
        {
            get;
            set;
        }

        public string EndpointName
        {
            get;
            set;
        }

        public string ContractName
        {
            get;
            set;
        }

        public bool CanConnect
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        #endregion

        #region IBaseServiceContract Members

        public string GetCoreObject(params string[] values)
        {
            return string.Empty;
        }

        public Exception BaseServiceException
        {
            get;
            set;
        }

        public string BaseServiceMessage
        {
            get;
            set;
        }

        public string GetServiceMessage()
        {
            return BaseServiceMessage;
        }

        #endregion
    }
}
