using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Data.SqlClient;
using System.Configuration;
using Tetsuo.Services;

namespace Tetsuo.Frontdoor
{
    public partial class TTSBroker : ServiceBase
    {
        public TTSBroker()
        {
            InitializeComponent();
            this.AutoLog = true;
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            
        }

        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }

        
    }
}
