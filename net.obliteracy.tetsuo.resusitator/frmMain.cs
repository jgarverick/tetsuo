using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Tetsuo.Core.IO;
using System.Xml;
using System.IO;

namespace Tetsuo.Resusitator
{
    public partial class frmMain : Form
    {
        DnrManifestWriter writer;
        DnrManifestReader reader;
        DnrManifest SelectedManifest;

        public frmMain()
        {
            InitializeComponent();
            writer = new DnrManifestWriter();
            reader = new DnrManifestReader();
            writer.ManifestAdded += new DnrManifestWriter.OnManifestAdd(writer_ManifestAdded);
        }

        void writer_ManifestAdded(string value)
        {
            txtOutput.AppendText(value);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            tvwBrowser.MouseDown += new MouseEventHandler(tvwBrowser_MouseDown);
            tvwBrowser.AfterLabelEdit += new NodeLabelEditEventHandler(tvwBrowser_AfterLabelEdit);
            dnrParser.DoWork += new DoWorkEventHandler(dnrParser_DoWork);
        }

        void dnrParser_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument != null)
            {
                if (writer.AddManifest(e.Argument.ToString()))
                {
                    ProcessManifest(writer.Manifests[writer.Manifests.Count - 1], e.Argument.ToString());
                    
                }
                
            }
        }

        void ProcessManifest(DnrManifest manifest,string manifestName)
        {
            SelectedManifest = manifest;
            TreeNode newNode = new TreeNode();
            newNode.Text = manifestName;
            newNode.StateImageIndex = 4;
            newNode.ImageIndex = 4;
            newNode.Tag = writer.Manifests.IndexOf(manifest);
            // Add the children
            TreeNode assem = new TreeNode("Current Assembly: " +
                SelectedManifest.CurrentAssembly.Name, 5, 5);
            assem.Tag = newNode.Tag;
            // Add the interfaces
            foreach (string service in SelectedManifest.GetAvailableServices())
            {
                ProcessManifestMethods(assem, service);
            }
            newNode.Nodes.Add(assem);
            ProcessManifestDependencies(newNode);
            if (tvwBrowser.SelectedNode.ImageIndex == 3)
            {
                tvwBrowser.SelectedNode.Nodes.Add(newNode);
            }
            else
            {
                tvwBrowser.SelectedNode.FirstNode.Parent.Nodes.Add(newNode);
            }
            ProcessManifestOutput();

            ProcessNamespaces(newNode);
        }

        private void ProcessNamespaces(TreeNode newNode)
        {
            TreeNode namespaceNode = new TreeNode("Namespaces");
            namespaceNode.SelectedImageIndex = 6;
            namespaceNode.StateImageIndex = 6;
            namespaceNode.ImageIndex = 6;
            foreach (string nspace in SelectedManifest.ServiceContractNamespaces)
            {
                TreeNode newFolderNode = new TreeNode(nspace);
                newFolderNode.SelectedImageIndex = 6;
                newFolderNode.StateImageIndex = 6;
                newFolderNode.ImageIndex = 6;
                if (SelectedManifest.DataContracts.ContainsKey(nspace))
                {
                    TreeNode contractNode = new TreeNode("Data Contract");
                    contractNode.ImageIndex = 10;
                    contractNode.SelectedImageIndex = 10;
                    newFolderNode.Nodes.Add(contractNode);
                }

                if (SelectedManifest.ServiceWsdls.ContainsKey(nspace))
                {
                    TreeNode wsdlNode = new TreeNode("Service Contract (WSDL)");
                    wsdlNode.ImageIndex = 11;
                    wsdlNode.SelectedImageIndex = 11;
                    newFolderNode.Nodes.Add(wsdlNode);
                }
                namespaceNode.Nodes.Add(newFolderNode);
            }
            newNode.Nodes.Add(namespaceNode);
        }

        private void ProcessManifestDependencies(TreeNode newNode)
        {
            TreeNode dependencies = new TreeNode();
            dependencies.Text = "Dependencies";
            dependencies.SelectedImageIndex = 7;
            dependencies.StateImageIndex = 7;
            dependencies.ImageIndex = 6;
            int i = 0;
            foreach (DnrAssembly asm in SelectedManifest.Dependencies)
            {
                dependencies.Nodes.Add("dep|" + i, asm.Name, 1, 1);
                dependencies.Nodes[i].Tag = writer.Manifests.IndexOf(SelectedManifest);
                i += 1;
            }
            newNode.Nodes.Add(dependencies);

        }

        private void ProcessManifestMethods(TreeNode assem, string service)
        {
            string[] names = service.Split('.');
            TreeNode _interface = new TreeNode(names[names.GetUpperBound(0)], 8, 8);
            _interface.Tag = writer.Manifests.IndexOf(SelectedManifest);
            foreach (string method in SelectedManifest.GetMethodsFromService(service))
            {
                _interface.Nodes.Add(method, method, 9, 9);
            }
            assem.Nodes.Add(_interface);
        }

        private void ProcessManifestOutput()
        {
            txtOutput.Text = SelectedManifest.Output;
            foreach (Exception ex in SelectedManifest.AssemblyErrors)
            {
                ListViewItem item = new ListViewItem(SelectedManifest.CurrentAssembly.AssemblyName);
                item.SubItems.Add(ex.Message);
                lvwErrors.Items.Add(item);
            }
        }

        void tvwBrowser_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            switch (tvwBrowser.SelectedNode.ImageIndex)
            {
                case 3:
                    writer.Name = tvwBrowser.SelectedNode.Text;
                    break;
                case 4:
                    break;
            }
        }

        void tvwBrowser_MouseDown(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo info = tvwBrowser.HitTest(e.X, e.Y);
            tvwBrowser.SelectedNode = info.Node;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                mnuPopup.Top = e.X;
                mnuPopup.Left = e.Y;
                if (tvwBrowser.SelectedNode.ImageIndex == 3)
                {
                    addManifestToolStripMenuItem.Visible = true;
                    deleteSelectedManifestToolStripMenuItem.Visible = false;
                }
                else if (tvwBrowser.SelectedNode.ImageIndex == 4)
                {
                    addManifestToolStripMenuItem.Visible = false;
                    deleteSelectedManifestToolStripMenuItem.Visible = true;
                }
                else
                {
                    addManifestToolStripMenuItem.Visible = false;
                    deleteSelectedManifestToolStripMenuItem.Visible = false;
                }


            }
        }

        private void newDNRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvwBrowser.Enabled = true;
            tvwBrowser.Nodes.Clear();
            tvwBrowser.Nodes.Add("projRoot", "New Manifest", 3,3);
        }

        private void tvwBrowser_AfterSelect(object sender, TreeViewEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            if (e.Node.Tag != null)
            {
                SelectedManifest = writer.Manifests[Convert.ToInt32(e.Node.Tag)];
            }
            TabPage tbp = tbpProperties;
            switch (e.Node.ImageIndex)
            {
                case 1:
                    string[] dependency = e.Node.Name.ToString().Split('|');
                    pgInspector.SelectedObject = SelectedManifest.Dependencies[Int32.Parse(dependency[1])];
                    break;
                case 4:
                    pgInspector.SelectedObject = SelectedManifest;
                    break;
                case 5:
                    pgInspector.SelectedObject = SelectedManifest.CurrentAssembly;
                    break;
                case 8:
                    Assembly asm = Assembly.Load(SelectedManifest.CurrentAssembly.AssemblyStream);
                    Type mod = asm.GetType(SelectedManifest.CurrentAssembly.AssemblyName + "." + e.Node.Text);
                    pgInspector.SelectedObject = mod;
                    break;
                case 10:
                    tbp = tbpContractViewer;
                    doc.LoadXml(SelectedManifest.DataContracts[e.Node.Parent.Text]);
                    doc.Save("output.xsd");
                    wbContractViewer.Navigate(Environment.CurrentDirectory + "\\output.xsd");
                    break;
                case 11:
                    tbp = tbpContractViewer;
                    doc.LoadXml(SelectedManifest.ServiceWsdls[e.Node.Parent.Text]);
                    doc.Save("output.wsdl");
                    wbContractViewer.Navigate(Environment.CurrentDirectory + "\\output.wsdl");
                    break;
            }
            tbcOutput.SelectedTab = tbp;
        }

        private void addManifestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialog.Title = "Choose DLL to use";
            openDialog.Filter = "Application extensions|*.dll";
            openDialog.ShowDialog();
            if (openDialog.FileName != string.Empty)
            {
                DoWorkEventArgs arg = new DoWorkEventArgs(openDialog.FileName);
                this.Cursor = Cursors.WaitCursor;
                tssStatus.Text = "Processing...";
                dnrParser_DoWork(this, arg);
                this.Cursor = Cursors.Default;
                tssStatus.Text = "Ready.";
                tbcOutput.SelectedTab = tbpOutput;
            }
        }

        private void saveDNRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveDialog.ShowDialog();
            if (saveDialog.FileName != string.Empty)
            {
                // Save the archive
                writer.Output(saveDialog.FileName);
            }
        }

        private void openDNRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialog.Title = "Open DNR";
            openDialog.Filter = "DNR archive files|*.dnr";
            openDialog.ShowDialog();
            if (openDialog.FileName != string.Empty)
            {
                reader = new DnrManifestReader(openDialog.FileName);
                TreeNode thisDnr = new TreeNode(openDialog.FileName,3,3);
                tvwBrowser.Nodes.Add(thisDnr);
                tvwBrowser.SelectedNode = thisDnr;
                writer.Manifests.Clear();
                for (int i = 0; i < reader.ManifestCount; i++)
                {
                    writer.Manifests.Add(reader[i]);
                    ProcessManifest(writer.Manifests[i], writer.Manifests[i].CurrentAssembly.AssemblyName);
                }
                tvwBrowser.Enabled = true;
                tvwBrowser.SelectedNode = null;
            }
        }

        private void deleteSelectedManifestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int itemIndex = 0;
            if (int.TryParse(tvwBrowser.SelectedNode.Tag.ToString(), out itemIndex))
            {
                writer.Manifests.RemoveAt(itemIndex);
                tvwBrowser.Nodes.Remove(tvwBrowser.SelectedNode);
                tvwBrowser.Refresh();
            }
        }
    }
}
