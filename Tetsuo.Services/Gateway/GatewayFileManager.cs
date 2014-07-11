using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.Data.SqlClient;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Configuration;
using Tetsuo.Core.Contracts;
using Tetsuo.Common;
using Tetsuo.Core.IO;

namespace Tetsuo.Services
{
    public class GatewayFileManager 
    {
        FileSystemWatcher fswConfig = new FileSystemWatcher();

        public GatewayFileManager()
        {
            fswConfig.EnableRaisingEvents = true;
            fswConfig.Created += new FileSystemEventHandler(fswConfig_Created);
            fswConfig.Deleted += new FileSystemEventHandler(fswConfig_Deleted);
            fswConfig.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // watch all files
            fswConfig.Filter = "*.dnr";

        }

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
                    Console.WriteLine("Extracting {0}...", dr[i].Name);
                    dr[i].Extract(fswConfig.Path);
                    // TODO: Add methods here that will insert the hub and
                    // associated spokes into the data store

                    // If hub exists...

                    // If spoke exists...

                    // Check spoke version...
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
