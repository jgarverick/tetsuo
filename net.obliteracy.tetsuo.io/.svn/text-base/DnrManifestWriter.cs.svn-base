using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace net.obliteracy.tetsuo.io
{
    /// <summary>
    /// Handles save functionality for DnrManifest objects
    /// </summary>
    public class DnrManifestWriter
    {
        public List<DnrManifest> Manifests { get; set; }
        public string Name { get; set; }
        public DnrManifestWriter()
        {
            Manifests = new List<DnrManifest>();
        }

        public bool AddManifest(string pathToAssembly)
        {
            bool retval = false;
            if (!(pathToAssembly == string.Empty) && !(pathToAssembly == null))
            {
                DnrManifest drm = new DnrManifest();
                if (drm.Add(pathToAssembly))
                {
                    Manifests.Add(drm);
                    retval = true;
                }
                else
                {
                    DnrManifest drmNew = new DnrManifest("",drm.CurrentAssembly.AssemblyName,"","");
                    drmNew.AssemblyErrors.AddRange(drm.AssemblyErrors);
                    Manifests.Add(drmNew);
                    retval = true;
                }
                
            }
            return retval;
        }

        public void Output(string outputPath)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream sr = File.Open(outputPath, FileMode.Create);
            bf.Serialize(sr, Manifests);
            sr.Close();
            bf = null;
            Console.WriteLine("DNR archive creation complete.\r\n");
        }
    }
}
