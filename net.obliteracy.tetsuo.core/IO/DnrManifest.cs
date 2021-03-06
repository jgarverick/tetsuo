﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Specialized;
using System.EnterpriseServices.Internal;
using System.ServiceModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;

namespace Tetsuo.Core.IO
{
    [Serializable()]
    public class AvailableService
    {
        public string FullName { get; set; }
        public StringCollection Methods { get; set; }
        public Dictionary<string, string> Interfaces { get; set; }

        public AvailableService()
        {
            Interfaces = new Dictionary<string, string>();
            Methods = new StringCollection();
        }
    }

    [Serializable()]
    public class DnrAssembly
    {
        public byte[] AssemblyStream { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }
        public string InitialAssemblyLoadPath { get; set; }
        public DnrAssemblyTypes AssemblyType { get; set; }
        public string Name { get; set; }
        public string Wsdl { get; set; }
        public string ServiceSchema { get; set; }
    }

    public enum DnrAssemblyTypes
    {
        GAC = 0,
        System = 1,
        Local = 2,
        Primary = 3
    }

    [Serializable()]
    public class DnrManifest
    {
        public DnrAssembly CurrentAssembly { get; set; }
        public List<DnrAssembly> Dependencies { get; set; }
        public List<Exception> AssemblyErrors { get; private set; }
        public string HubName { get; set; }
        public string DefaultSpoke { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> DataContracts { get; set; }
        public Dictionary<string, string> ServiceWsdls { get; set; }
        public StringCollection ServiceContractNamespaces { get; set; }
        private string currentDirectory = System.IO.Directory.GetCurrentDirectory();
        StringBuilder OutputMessages = new StringBuilder();
        StringCollection AssemblyRef = new StringCollection();
        bool hasServices = false;
        bool hasImplementations = false;
        Hashtable AvailableContracts = new Hashtable();
        List<AvailableService> AvailableImplementations = new List<AvailableService>();
        Hashtable ImplementationMethods = new Hashtable();
        StringCollection ResolvedReferences = new StringCollection();

        public delegate void OnInstrumentationChanged(string value);
        public event OnInstrumentationChanged InstrumentationChanged;

        public string Output
        {
            get
            {
                return OutputMessages.ToString();
            }
        }

        public DnrManifest()
        {
            CurrentAssembly = new DnrAssembly();
            AssemblyErrors = new List<Exception>();
            Dependencies = new List<DnrAssembly>();
            ServiceContractNamespaces = new StringCollection();
            DataContracts = new Dictionary<string, string>();
            ServiceWsdls = new Dictionary<string, string>();
            CurrentAssembly.AssemblyStream = null;
            CurrentAssembly.AssemblyName = "";
            HubName = "";
            DefaultSpoke = "";
            CurrentAssembly.AssemblyVersion = "";
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
        }

        Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        public DnrManifest(string AssemblyPath, string assemblyName, string hubName, string defaultSpoke)
        {
            Dependencies = new List<DnrAssembly>();
            AssemblyErrors = new List<Exception>();
            this.Add(AssemblyPath);
            CurrentAssembly = new DnrAssembly();
            CurrentAssembly.AssemblyName = assemblyName;
            HubName = hubName;
            DefaultSpoke = defaultSpoke;
            CurrentAssembly.AssemblyVersion = "";
        }

        byte[] ProcessBinary(string path)
        {
            byte[] retval = null;
            StreamReader sr = new StreamReader(path);
            BinaryReader br = new BinaryReader(sr.BaseStream);
            retval = br.ReadBytes((int)br.BaseStream.Length);
            br.Close();
            sr.Close();
            sr.Dispose();
            br = null;
            sr = null;
            return retval;
        }

        public ICollection GetMethodsFromService(string serviceFullName)
        {
            ICollection retval = new StringCollection();
            foreach (AvailableService serv in AvailableImplementations)
            {
                if (serv.FullName == serviceFullName)
                {
                    retval = serv.Methods;
                    break;
                }
            }
            
            return retval;
        }

        public StringCollection GetAvailableServices()
        {
            StringCollection retval = new StringCollection();
            try
            {
                foreach (AvailableService serv in AvailableImplementations)
                {
                    retval.Add(serv.FullName);
                }
            }
            catch (Exception ex)
            {
                if (!(ex.GetType() == typeof(NullReferenceException)))
                {
                    this.AssemblyErrors.Add(ex);
                }
            }
            return retval;
        }

        public void Extract(string outputPath)
        {
            
            if (!(CurrentAssembly.AssemblyStream == null))
            {
                // There are goodies in the byte array
                if (!(CurrentAssembly.AssemblyName == ""))
                {

                    string asmFileName = CurrentAssembly.AssemblyName + ".dll";
                    OutputAssembly(outputPath, asmFileName);
                    foreach (DnrAssembly dep in this.Dependencies)
                    {
                        if (dep.AssemblyType == DnrAssemblyTypes.Local)
                        {
                            OutputAssembly(outputPath, dep.AssemblyName + ".dll");
                        }
                    }
                    
                }
                else
                {

                }
            }
        }

        private void OutputAssembly(string outputPath, string asmFileName)
        {
            FileStream fs = null;
            if (!(outputPath == null) && !(outputPath == string.Empty))
            {
                fs = new FileStream(outputPath
                     + "\\" + asmFileName, FileMode.Create);
            }
            else
            {
                fs = new FileStream(Environment.CurrentDirectory
                    + "\\" + asmFileName, FileMode.Create);
            }
            fs.Write(CurrentAssembly.AssemblyStream, 0, CurrentAssembly.AssemblyStream.Length);
            fs.Close();
            fs.Dispose();
            WriteInstrumentation(string.Format("Extracting {0}...\r\n", asmFileName));
        }
        public bool Add(string inputPath)
        {
            bool retval = false;
            //bool HasValidService = false;
            if (!(inputPath == string.Empty) && !(inputPath == null) &&
                Path.GetExtension(inputPath) == ".dll")
            {
                AppDomain.CurrentDomain.SetData("APPBASE", inputPath);
                this.Name = System.IO.Path.GetFileNameWithoutExtension(inputPath);
                currentDirectory = System.IO.Path.GetDirectoryName(inputPath);
                CurrentAssembly = new DnrAssembly();
                CurrentAssembly.AssemblyType = DnrAssemblyTypes.Primary;
                CurrentAssembly.Name = System.IO.Path.GetFileName(inputPath);
                CurrentAssembly.AssemblyStream = ProcessBinary(inputPath);
                Assembly asm = Assembly.Load(CurrentAssembly.AssemblyStream);
                //_domain.Load(inputPath);
                CurrentAssembly.AssemblyName = asm.GetName().Name;
                CurrentAssembly.AssemblyVersion = asm.GetName().Version.ToString();
                WriteInstrumentation("Resolving dependencies...\r\n");
                AssemblyName[] dependencies = asm.GetReferencedAssemblies();
                foreach (AssemblyName aName in dependencies)
                {
                    ResolveDependency(aName.Name);
                }

                CheckExportedTypes(inputPath);

                foreach (string s in Directory.GetFiles(Environment.CurrentDirectory))
                {
                    if (s.ToLower().EndsWith(".xsd") || s.ToLower().EndsWith(".wsdl"))
                        File.Delete(s);
                }
                // Now check to see if you can generate the service context and wsdl
                CommandLineMonitor monitor = new CommandLineMonitor();
                monitor.StreamUpdated += new CommandLineMonitor.OnStreamUpdated(monitor_StreamUpdated);
                monitor.Initialize("svcutil.exe", inputPath + "");
                // Wait until it's done
                bool test = false;
                while (!test)
                {
                    foreach (string s in Directory.GetFiles(Environment.CurrentDirectory))
                    {
                        if (s.ToLower().EndsWith(".xsd") || s.ToLower().EndsWith(".wsdl"))
                            test = true;
                    }
                    Thread.Sleep(1000);
                }

                CaptureWsdlsAndDataContracts();

                if (AssemblyErrors.Count > 0)
                {
                    WriteInstrumentation(string.Format("Errors creating manifest--{0} error(s) found.\r\n", AssemblyErrors.Count));
                }
                else
                {
                    WriteInstrumentation("Dependencies resolved and manifest generated.\r\n");

                }
                // }
                retval = true;
            }
            return retval;
        }

        private void CaptureWsdlsAndDataContracts()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    foreach (string n in ServiceContractNamespaces)
                    {
                        if (File.Exists(n + ".wsdl") && File.Exists(n + ".xsd"))
                        {
                            if (!(ServiceWsdls.ContainsKey(n)))
                            {
                                ServiceWsdls.Add(n, File.ReadAllText(n + ".wsdl"));
                                WriteInstrumentation("WSDL for namespace " + n + " captured.");
                            }
                            if (!(DataContracts.ContainsKey(n)))
                            {
                                DataContracts.Add(n, File.ReadAllText(n + ".xsd"));
                                WriteInstrumentation("Data contract for namespace " + n + " captured.");
                            }
                        }
                    }
                    
                }
                catch { }
                Thread.Sleep(750);
            }
        }

        void monitor_StreamUpdated(string value)
        {
            WriteInstrumentation(value);
        }

        private void CheckExportedTypes(string appPath)
        {
            Assembly asm = null;
            try
            {
                foreach (string f in Directory.GetFiles(currentDirectory))
                {
                    if (f.EndsWith(".dll"))
                    {
                        asm = Assembly.LoadFrom(f);
                        try
                        {
                            foreach (var type in asm.GetTypes())
                            {
                                //Console.WriteLine(asm.Location);

                                //output.Append(string.Format("Found object {0} in {1}.\r\n", type.Name, asm.CodeBase)); 
                                if (type.IsClass)
                                {
                                    object instance = asm.CreateInstance(type.FullName);
                                    //output.Append(string.Format("Class {0} is being interrogated.\r\n", type.Name)); 
                                    Type[] interfaces = type.GetInterfaces();
                                    AvailableService service = new AvailableService();
                                    service.Interfaces = new Dictionary<string, string>();
                                    service.Methods = new StringCollection();
                                    service.FullName = type.FullName;
                                    foreach (var interf in interfaces)
                                    {
                                        //output.Append(string.Format("Implements {0}.\r\n", interf.Name)); 
                                        foreach (var prop in interf.GetCustomAttributes(true))
                                        {
                                            if (prop.GetType().IsAssignableFrom(typeof(ServiceContractAttribute)))
                                            {
                                                if (!(AvailableContracts.ContainsKey(interf.Name)))
                                                {
                                                    string refNamespace = (interf.GetCustomAttributes(typeof(ServiceContractAttribute), true).ToList().First() as ServiceContractAttribute).Namespace;
                                                    if (!(ServiceContractNamespaces.Contains(refNamespace)))
                                                        ServiceContractNamespaces.Add(refNamespace);
                                                    service.Interfaces.Add(interf.Name, refNamespace);
                                                    MethodInfo[] modInfo = interf.GetMethods();
                                                    foreach (MethodInfo method in modInfo)
                                                    {
                                                        service.Methods.Add(method.Name);
                                                    }
                                                }
                                                AvailableImplementations.Add(service);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            AssemblyErrors.Add(ex1);
                        }
                    }
                }
                asm = Assembly.LoadFile(appPath);

                foreach (var reference in AppDomain.CurrentDomain.GetAssemblies())
                {
                    ResolvedReferences.Add(reference.FullName);
                }

            }
            catch (Exception ex)
            {
                AssemblyErrors.Add(ex);
            }

        }

        private void ResolveDependency(string inputPath)
        {
            if (inputPath == string.Empty)
            {
                AssemblyErrors.Add(new FileNotFoundException(
                                string.Format("Could not resolve the dependency because no name was supplied.\r\n")));
            }
            else
            {
                if (!(IsInGAC(inputPath)))
                {
                    if (!(IsInSystem32(inputPath)))
                    {
                        if (!(IsInCurrentDirectory(inputPath)))
                        {
                            // Figure out where it is, then
                            WriteInstrumentation(inputPath);
                            AssemblyErrors.Add(new FileNotFoundException(
                                string.Format("Could not resolve the following dependency: {0}\r\n", inputPath + ".dll")));
                        }
                    }
                }
            }
        }

        private bool IsInGAC(string inputName)
        {
            bool retval = false;
            string GACPath = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "\\assembly";
            string[] match = System.IO.Directory.GetFiles(GACPath, inputName + ".dll", SearchOption.AllDirectories);
            if (match.Length > 0)
            {
                DnrAssembly dep = new DnrAssembly();
                dep.InitialAssemblyLoadPath = match[0];
                dep.Name = inputName;
                dep.AssemblyType = DnrAssemblyTypes.GAC;
                //_domain.Load(inputName);
                Dependencies.Add(dep);
                WriteInstrumentation(string.Format("** Found referenced assembly {0} (GAC).\r\n", inputName));
                retval = true;
            }
            return retval;
        }

        private bool IsInSystem32(string inputName)
        {
            bool retval = false;
            string SysPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string[] match = System.IO.Directory.GetFiles(SysPath, inputName + ".dll", SearchOption.TopDirectoryOnly);
            if (match.Length > 0)
            {
                DnrAssembly dep = new DnrAssembly();
                dep.InitialAssemblyLoadPath = match[0];
                dep.Name = inputName;
                dep.AssemblyType = DnrAssemblyTypes.System;
                //_domain.Load(inputName);
                Dependencies.Add(dep);
                WriteInstrumentation(string.Format("** Found referenced assembly {0} (System directory).\r\n", inputName));
                retval = true;
            }
            return retval;
        }

        private bool IsInCurrentDirectory(string inputName)
        {
            bool retval = false;
            string SysPath = currentDirectory;
            AppDomain.CurrentDomain.SetData("APPBASE", currentDirectory);
            string[] match = System.IO.Directory.GetFiles(SysPath, inputName + ".dll", SearchOption.TopDirectoryOnly);
            if (match.Length > 0)
            {
                WriteInstrumentation(string.Format("** Found referenced assembly {0} (Current directory).\r\n", inputName));
                foreach (string s in match)
                {
                    DnrAssembly dep = new DnrAssembly();
                    dep.InitialAssemblyLoadPath = s;
                    dep.Name = inputName;
                    dep.AssemblyStream = ProcessBinary(dep.InitialAssemblyLoadPath);
                    Assembly asm = Assembly.Load(dep.AssemblyStream);
                    dep.AssemblyName = asm.GetName().Name;
                    dep.AssemblyType = DnrAssemblyTypes.Local;
                    dep.AssemblyVersion = asm.GetName().Version.ToString();
                    AssemblyRef.Add(inputName);
                    Dependencies.Add(dep);
                    WriteInstrumentation(string.Format("** Added referenced assembly {0} to DNR.\r\n", inputName));
                }
                retval = true;
            }
            return retval;
        }

        private Assembly GetMainAssembly(string assemblyName)
        {
            Assembly retval = null;
            Assembly[] group = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in group)
            {
                if (asm.GetName().Name == assemblyName)
                {
                    retval = asm;
                    break;
                }
            }
            if (retval == null) { retval = Assembly.Load(CurrentAssembly.AssemblyStream); }
            return retval;
        }

        private bool TypeFiltering(Type t, object filterArgs)
        {
            return t.Assembly.GetName().Name == filterArgs.ToString();
        }

        private void WriteInstrumentation(string message)
        {
            Console.WriteLine(message);
            OutputMessages.Append(message);
            if (InstrumentationChanged != null)
                InstrumentationChanged(message);
        }

    }
}
