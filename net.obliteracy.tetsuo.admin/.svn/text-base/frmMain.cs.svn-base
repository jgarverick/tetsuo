using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace net.obliteracy.tetsuo.admin
{
    public partial class frmMain : Form
    {
        MyHub.ServiceResolverGatewayClient client;
        public frmMain()
        {
            InitializeComponent();
             client =
                new net.obliteracy.tetsuo.admin.MyHub.ServiceResolverGatewayClient();
            client.Open();
            client.OnInitializeGateway("net.obliteracy.tetsuo.core.contracts.IServiceHub", new Uri("net.tcp://localhost:9000/service/"));
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
