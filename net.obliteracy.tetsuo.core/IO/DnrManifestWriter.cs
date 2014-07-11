using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Tetsuo.Core.IO
{
    /// <summary>
    /// Handles save functionality for DnrManifest objects
    /// </summary>
    [Serializable]
    public class DnrManifestWriter
    {
        public List<DnrManifest> Manifests { get; set; }
        public string Name { get; set; }
        public delegate void OnManifestAdd(string value);
        public event OnManifestAdd ManifestAdded;

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
                drm.InstrumentationChanged += new DnrManifest.OnInstrumentationChanged(drm_InstrumentationChanged);
                if (drm.Add(pathToAssembly))
                {
                    Manifests.Add(drm);
                    retval = true;
                }
                else
                {
                    DnrManifest drmNew = new DnrManifest("",drm.CurrentAssembly.AssemblyName,"","");
                    drmNew.InstrumentationChanged += new DnrManifest.OnInstrumentationChanged(drm_InstrumentationChanged);
                    drmNew.AssemblyErrors.AddRange(drm.AssemblyErrors);
                    Manifests.Add(drmNew);
                    retval = true;
                }
                
            }
            return retval;
        }

        void drm_InstrumentationChanged(string value)
        {
            if (!(ManifestAdded == null))
                ManifestAdded(value);
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
