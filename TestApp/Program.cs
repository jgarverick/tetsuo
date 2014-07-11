using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Resources;
using System.Globalization;
using Tetsuo.Services;
using Tetsuo.Core.IO;
using Tetsuo.Entities;
using Tetsuo.Entities.Model;
using Tetsuo.Core.Common;
using System.ServiceModel.MsmqIntegration;
using System.Messaging;

namespace TestApp
{
    
    class Program
    {
        static List<ServiceHost> Hosts =
            new List<ServiceHost>();
        static FileSystemWatcher fswConfig = new FileSystemWatcher();

        static EntityManager em = new EntityManager();

        [MTAThread()]
        static void Main(string[] args)
        {
           // fswConfig.Path = @"C:\Test\";
           // fswConfig.EnableRaisingEvents = true;
           // fswConfig.Created += new FileSystemEventHandler(fswConfig_Created);
           // fswConfig.Deleted += new FileSystemEventHandler(fswConfig_Deleted);
           // fswConfig.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           //| NotifyFilters.FileName | NotifyFilters.DirectoryName;
           // // watch all files
           // fswConfig.Filter = "*.dnr";

            //InitializeGateways();
            
            //if (gateways[0].ResolveHub(string.Format("http://{0}:9000/service/Hub1",Environment.MachineName)))
            //{
            //    ServiceHub hub = gateways[0].CurrentHub;
            //    Console.WriteLine(hub.Name + ": " + hub.DestinationAddress);
            //}
            MsmqIntegrationBinding mBind = new MsmqIntegrationBinding();
            if (!(MessageQueue.Exists(@".\private$\request")))
                MessageQueue.Create(@".\private$\request", true);
            if (!(MessageQueue.Exists(@".\private$\response")))
                MessageQueue.Create(@".\private$\response", false);

            using (ServiceHost host = new ServiceHost(new RouteDispatcher()))
            {
                host.Faulted += new EventHandler(host_Faulted);
                host.Open();
                Console.WriteLine("Services are currently running.  Press any key to exit.");
                while (Console.Read() < 1)
                {
                }
                if (!(host.State == CommunicationState.Closed))
                    host.Close();
            }

        }

        static void host_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Main routing host has faulted.");
            (sender as ServiceHost).Abort();
        }

        static void fswConfig_Deleted(object sender, FileSystemEventArgs e)
        {
            // Check to see if the service is actually running
            Console.WriteLine("File {0} was just deleted from the main directory by {2} ({1}).", e.Name, e.FullPath,Environment.UserName);
        }

        static void fswConfig_Created(object sender, FileSystemEventArgs e)
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
            Console.WriteLine("File {0} was just dropped into the main directory ({1}).", e.Name,e.FullPath);
        }
       
    }
   

}
