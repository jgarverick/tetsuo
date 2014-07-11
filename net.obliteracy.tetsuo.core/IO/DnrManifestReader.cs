using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tetsuo.Core.IO
{
    /// <summary>
    /// Handles any IO related activities with DnrManifest files.
    /// </summary>
    public class DnrManifestReader
    {
        List<DnrManifest> ManifestCollection;

        public DnrManifestReader()
        {
            ManifestCollection = new List<DnrManifest>();
        }

        public DnrManifestReader(string filePath)
        {
            BinaryFormatter bf = new BinaryFormatter();
            if (!(filePath == string.Empty))
            {
                Stream sr = File.Open(filePath, FileMode.Open);
                ManifestCollection = (List<DnrManifest>)bf.Deserialize(sr);

            }
        }

        public DnrManifest this[int index]
        {
            get { return ManifestCollection[index]; }
        }

        public int ManifestCount
        {
            get { return ManifestCollection.Count; }
        }

        public bool Deploy(string targetDirectory)
        {
            bool retval = true;
            foreach (DnrManifest dnr in ManifestCollection)
            {
                if (string.IsNullOrEmpty(targetDirectory))
                    dnr.Extract(Environment.CurrentDirectory);
                else
                {
                    if (!Directory.Exists(targetDirectory))
                        Directory.CreateDirectory(targetDirectory);
                    dnr.Extract(targetDirectory);
                }


            }
            return retval;
        }
    }


}
