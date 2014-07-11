using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using net.obliteracy.tetsuo.contract;

namespace net.obliteracy.tetsuo.core
{
    public class ActuarialService:IActuarialService
    {
        #region IActuarialService Members

        public void DoSomethingElse(string input)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IActuarialService Members


        public string GetResult()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
