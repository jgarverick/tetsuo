using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.obliteracy.tetsuo.contract.ServiceContracts;
using net.obliteracy.tetsuo.core.Common;
using net.obliteracy.tetsuo.core.services;
using System.ServiceModel;
using System.IO;
using net.obliteracy.tetsuo.io;
using System.Data.SqlClient;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Configuration;

namespace net.obliteracy.tetsuo.contractimplementation.Services
{
    public class GatewayManagerService : IGatewayManagerService
    {
        private List<ServiceResolverGateway> gateways = new List<ServiceResolverGateway>();
        private List<ServiceHost> Hosts = new List<ServiceHost>();
        FileSystemWatcher fswConfig = new FileSystemWatcher();

        public GatewayManagerService()
        {
            fswConfig.EnableRaisingEvents = true;
            fswConfig.Created += new FileSystemEventHandler(fswConfig_Created);
            fswConfig.Deleted += new FileSystemEventHandler(fswConfig_Deleted);
            fswConfig.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // watch all files
            fswConfig.Filter = "*.dnr";

        }
        #region IGatewayManagerService Members

        public void Startup()
        {
            InitializeGateways();
        }

        public bool Resolve(string Url, ReslovedEnpointTypes endpointType)
        {
            bool retval = false;
            switch (endpointType)
            {
                case ReslovedEnpointTypes.Gateway:
                    break;
                case ReslovedEnpointTypes.Hub:
                    break;
                case ReslovedEnpointTypes.Service:
                    break;
            }
            return retval;
        }

        public string[] GetGateways(string machineAddress)
        {
            string[] retval = new string[gateways.Count];
            int i = 0;
            foreach (ServiceResolverGateway gateway in gateways)
            {
                retval[i] = gateways[i].Name;
                i += 1;
            }
            return retval;
        }

        public string[] GetHubs(string machineAddress, string gateway)
        {
            throw new NotImplementedException();
        }

        public void ShutdownGateway(string machineAddress, string gateway)
        {
            throw new NotImplementedException();
        }

        public void Shutdown(string machineAddress)
        {
            throw new NotImplementedException();
        }

        public bool StopService(string serviceName)
        {
            throw new NotImplementedException();
        }

        public bool StartService(string serviceName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Initialization
        //
        private void InitializeGateways()
        {
            int i = 0;
            BindingElement bElement = new TcpTransportBindingElement();
            System.ServiceModel.Channels.Binding binding = new BasicHttpBinding();
            SqlConnection conn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["mexData"].ConnectionString);
            SqlCommand comm = new SqlCommand();
            SqlDataReader dr;
            comm.CommandText = "SELECT * from Gateways";
            comm.CommandType = System.Data.CommandType.Text;
            comm.Connection = conn;
            conn.Open();
            dr = comm.ExecuteReader();
            while (dr.Read())
            {
                ServiceResolverGateway gateway = new ServiceResolverGateway();
                Hosts.Add(new ServiceHost(typeof(ServiceResolverGateway),
                    new Uri(dr[2].ToString())));
                Hosts[i].AddServiceEndpoint("net.obliteracy.tetsuo.core.contracts.IServiceResolverGateway",
                    binding, dr[2].ToString());
                Hosts[i].Description.Behaviors.Add(new ServiceMetadataBehavior());
                Hosts[i].AddServiceEndpoint(typeof(IMetadataExchange),
                    binding, "MEX");
                gateway.OnInitializeGateway("net.obliteracy.tetsuo.core.contracts.IServiceHub",
                    new Uri(dr[2].ToString()));
                gateways.Add(gateway);
                Hosts[i].Open();
            }
            conn.Close();
        }
        //
        #endregion

        #region File System Management
        //
        private void fswConfig_Deleted(object sender, FileSystemEventArgs e)
        {
            // Check to see if the service is actually running
            Console.WriteLine("File {0} was just deleted from the main directory by {2} ({1}).", e.Name, e.FullPath, Environment.UserName);
        }

        private void fswConfig_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                DnrManifestReader dr = new DnrManifestReader(e.FullPath);
                for (int i = 0; i < dr.ManifestCount; i++)
                {
                    dr[i].Extract(fswConfig.Path);
                    // TODO: Add methods here that will insert the hub and
                    // associated spokes into the data store
                }
                dr = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("File {0} was just dropped into the main directory ({1}).", e.Name, e.FullPath);
        }
        //
        #endregion
    }
}
