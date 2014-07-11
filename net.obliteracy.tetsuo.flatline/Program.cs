using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using Tetsuo.Core.IO;
using System.Messaging;

namespace Tetsuo.Flatline
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                switch (args[0].ToLower())
                {
                    case "-d":
                        DisplayArchive(args[1]);
                        break;
                    case "-c":
                        CreateArchive(args[1], args[2]);
                        break;
                    case "-e":
                        ExpandArchive(args[1]);
                        break;
                }
            }
            Console.Read();
            
        }

        static void CreateArchive(string asmPath, string archiveName)
        {
            if (File.Exists(asmPath))
            {
                DnrManifestWriter wr = new DnrManifestWriter();
                wr.AddManifest(asmPath);
                wr.Output(archiveName);
            }
        }

        static void DisplayArchive(string archiveName)
        {
            if (File.Exists(archiveName))
            {
                DnrManifestReader dr = new DnrManifestReader(archiveName);
                for (int i = 0; i < dr.ManifestCount;i++ )
                {
                    foreach (var item in dr[i].GetAvailableServices())
                    {
                        Console.WriteLine("Found service: " + item);
                    }
                    Console.WriteLine("Hub name: " + dr[i].HubName);
                    Console.WriteLine("Destination service endpoint name: " + dr[i].DefaultSpoke);
                    Console.WriteLine("Processing output:\r\n"+dr[i].Output);
                    if(dr[i].AssemblyErrors != null)
                        if(dr[i].AssemblyErrors.Count > 0)
                            dr[i].AssemblyErrors.ForEach(x=>Console.WriteLine(x.Message + 
                                Environment.NewLine + x.StackTrace));
                }
            }
        }

        static void ExpandArchive(string archiveName)
        {
            if (File.Exists(archiveName))
            {
                // Determine if the hub/service names are set.  If not, default them.
                DnrManifestReader dr = new DnrManifestReader(archiveName);
                for (int i = 0; i < dr.ManifestCount; i++)
                {
                    string hubDirectory = "";
                    string assemblyDirectory = "";
                    string requestQ = @".\private$\{0}.{1}.request";
                    if (string.IsNullOrEmpty(dr[i].HubName))
                        dr[i].HubName = dr[i].CurrentAssembly.Name;
                    hubDirectory = string.Format("{0}\\{1}", Path.GetDirectoryName(archiveName), dr[i].HubName);
                    assemblyDirectory = string.Format("{0}\\{1}", hubDirectory, dr[i].CurrentAssembly.AssemblyVersion);
                    foreach (var item in dr[i].GetAvailableServices())
                    {
                        //if (!(MessageQueue.Exists(string.Format(requestQ, dr[i].HubName, item))))
                        //    MessageQueue.Create(string.Format(requestQ, dr[i].HubName, item), true);
                    }
                    Console.WriteLine("Hub path: " + hubDirectory);
                    Console.WriteLine("Assembly path: " + assemblyDirectory);
                    if (!(Directory.Exists(hubDirectory)))
                        Directory.CreateDirectory(hubDirectory);
                    if (!(Directory.Exists(assemblyDirectory)))
                        Directory.CreateDirectory(assemblyDirectory);
                    dr[i].Extract(assemblyDirectory);

                }

            }
        }

        

        
    }
}
