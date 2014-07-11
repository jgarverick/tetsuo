using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using net.obliteracy.tetsuo.contract;

namespace net.obliteracy.tetsuo.core
{
    public class PayrollService:IPayrollService
    {
        #region IPayrollService Members

        public void PerformAnotherAction(string input)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
