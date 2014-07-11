namespace Tetsuo.Resusitator
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.dnrParser = new System.ComponentModel.BackgroundWorker();
            this.appStatus = new System.Windows.Forms.StatusStrip();
            this.tssStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pgFileProcess = new System.Windows.Forms.ToolStripProgressBar();
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.tvwBrowser = new System.Windows.Forms.TreeView();
            this.mnuPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addManifestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedManifestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appImages = new System.Windows.Forms.ImageList(this.components);
            this.tbcOutput = new System.Windows.Forms.TabControl();
            this.tbpProperties = new System.Windows.Forms.TabPage();
            this.pgInspector = new System.Windows.Forms.PropertyGrid();
            this.tbpOutput = new System.Windows.Forms.TabPage();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.tbpErrors = new System.Windows.Forms.TabPage();
            this.lvwErrors = new System.Windows.Forms.ListView();
            this.colManifest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbpContractViewer = new System.Windows.Forms.TabPage();
            this.wbContractViewer = new System.Windows.Forms.WebBrowser();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDNRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDNRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDNRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.appStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            this.mnuPopup.SuspendLayout();
            this.tbcOutput.SuspendLayout();
            this.tbpProperties.SuspendLayout();
            this.tbpOutput.SuspendLayout();
            this.tbpErrors.SuspendLayout();
            this.tbpContractViewer.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // dnrParser
            // 
            this.dnrParser.WorkerReportsProgress = true;
            // 
            // appStatus
            // 
            this.appStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssStatus,
            this.pgFileProcess});
            this.appStatus.Location = new System.Drawing.Point(0, 359);
            this.appStatus.Name = "appStatus";
            this.appStatus.Size = new System.Drawing.Size(563, 22);
            this.appStatus.TabIndex = 0;
            this.appStatus.Text = "statusStrip1";
            // 
            // tssStatus
            // 
            this.tssStatus.Name = "tssStatus";
            this.tssStatus.Size = new System.Drawing.Size(42, 17);
            this.tssStatus.Text = "Ready.";
            // 
            // pgFileProcess
            // 
            this.pgFileProcess.Name = "pgFileProcess";
            this.pgFileProcess.Size = new System.Drawing.Size(100, 16);
            this.pgFileProcess.Visible = false;
            // 
            // splMain
            // 
            this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMain.Location = new System.Drawing.Point(0, 24);
            this.splMain.Name = "splMain";
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.tvwBrowser);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.tbcOutput);
            this.splMain.Size = new System.Drawing.Size(563, 335);
            this.splMain.SplitterDistance = 230;
            this.splMain.TabIndex = 1;
            // 
            // tvwBrowser
            // 
            this.tvwBrowser.ContextMenuStrip = this.mnuPopup;
            this.tvwBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwBrowser.Enabled = false;
            this.tvwBrowser.ImageKey = "performance.ico";
            this.tvwBrowser.ImageList = this.appImages;
            this.tvwBrowser.LabelEdit = true;
            this.tvwBrowser.Location = new System.Drawing.Point(0, 0);
            this.tvwBrowser.Name = "tvwBrowser";
            this.tvwBrowser.SelectedImageIndex = 0;
            this.tvwBrowser.Size = new System.Drawing.Size(230, 335);
            this.tvwBrowser.TabIndex = 0;
            this.tvwBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwBrowser_AfterSelect);
            // 
            // mnuPopup
            // 
            this.mnuPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addManifestToolStripMenuItem,
            this.deleteSelectedManifestToolStripMenuItem});
            this.mnuPopup.Name = "mnuPopup";
            this.mnuPopup.Size = new System.Drawing.Size(227, 70);
            // 
            // addManifestToolStripMenuItem
            // 
            this.addManifestToolStripMenuItem.Name = "addManifestToolStripMenuItem";
            this.addManifestToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.addManifestToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.addManifestToolStripMenuItem.Text = "&Add manifest";
            this.addManifestToolStripMenuItem.Click += new System.EventHandler(this.addManifestToolStripMenuItem_Click);
            // 
            // deleteSelectedManifestToolStripMenuItem
            // 
            this.deleteSelectedManifestToolStripMenuItem.Name = "deleteSelectedManifestToolStripMenuItem";
            this.deleteSelectedManifestToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteSelectedManifestToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.deleteSelectedManifestToolStripMenuItem.Text = "&Delete selected manifest";
            this.deleteSelectedManifestToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedManifestToolStripMenuItem_Click);
            // 
            // appImages
            // 
            this.appImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("appImages.ImageStream")));
            this.appImages.TransparentColor = System.Drawing.Color.Fuchsia;
            this.appImages.Images.SetKeyName(0, "error.ico");
            this.appImages.Images.SetKeyName(1, "INFO.ICO");
            this.appImages.Images.SetKeyName(2, "warning.ico");
            this.appImages.Images.SetKeyName(3, "performance.ico");
            this.appImages.Images.SetKeyName(4, "VSProject_genericproject.ico");
            this.appImages.Images.SetKeyName(5, "idr_dll.ico");
            this.appImages.Images.SetKeyName(6, "CLSDFOLD.ICO");
            this.appImages.Images.SetKeyName(7, "OPENFOLD.ICO");
            this.appImages.Images.SetKeyName(8, "interface.bmp");
            this.appImages.Images.SetKeyName(9, "method.bmp");
            this.appImages.Images.SetKeyName(10, "XSD.png");
            this.appImages.Images.SetKeyName(11, "XML.png");
            // 
            // tbcOutput
            // 
            this.tbcOutput.Controls.Add(this.tbpProperties);
            this.tbcOutput.Controls.Add(this.tbpOutput);
            this.tbcOutput.Controls.Add(this.tbpErrors);
            this.tbcOutput.Controls.Add(this.tbpContractViewer);
            this.tbcOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcOutput.Location = new System.Drawing.Point(0, 0);
            this.tbcOutput.Name = "tbcOutput";
            this.tbcOutput.SelectedIndex = 0;
            this.tbcOutput.Size = new System.Drawing.Size(329, 335);
            this.tbcOutput.TabIndex = 0;
            // 
            // tbpProperties
            // 
            this.tbpProperties.Controls.Add(this.pgInspector);
            this.tbpProperties.Location = new System.Drawing.Point(4, 22);
            this.tbpProperties.Name = "tbpProperties";
            this.tbpProperties.Size = new System.Drawing.Size(321, 309);
            this.tbpProperties.TabIndex = 2;
            this.tbpProperties.Text = "Properties";
            this.tbpProperties.UseVisualStyleBackColor = true;
            // 
            // pgInspector
            // 
            this.pgInspector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgInspector.Location = new System.Drawing.Point(0, 0);
            this.pgInspector.Name = "pgInspector";
            this.pgInspector.Size = new System.Drawing.Size(321, 309);
            this.pgInspector.TabIndex = 0;
            // 
            // tbpOutput
            // 
            this.tbpOutput.Controls.Add(this.txtOutput);
            this.tbpOutput.Location = new System.Drawing.Point(4, 22);
            this.tbpOutput.Name = "tbpOutput";
            this.tbpOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tbpOutput.Size = new System.Drawing.Size(321, 309);
            this.tbpOutput.TabIndex = 0;
            this.tbpOutput.Text = "Output";
            this.tbpOutput.UseVisualStyleBackColor = true;
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(3, 3);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(315, 303);
            this.txtOutput.TabIndex = 0;
            // 
            // tbpErrors
            // 
            this.tbpErrors.Controls.Add(this.lvwErrors);
            this.tbpErrors.Location = new System.Drawing.Point(4, 22);
            this.tbpErrors.Name = "tbpErrors";
            this.tbpErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tbpErrors.Size = new System.Drawing.Size(321, 309);
            this.tbpErrors.TabIndex = 1;
            this.tbpErrors.Text = "Errors";
            this.tbpErrors.UseVisualStyleBackColor = true;
            // 
            // lvwErrors
            // 
            this.lvwErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colManifest,
            this.colMessage});
            this.lvwErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwErrors.GridLines = true;
            this.lvwErrors.Location = new System.Drawing.Point(3, 3);
            this.lvwErrors.Name = "lvwErrors";
            this.lvwErrors.Size = new System.Drawing.Size(315, 303);
            this.lvwErrors.TabIndex = 0;
            this.lvwErrors.UseCompatibleStateImageBehavior = false;
            this.lvwErrors.View = System.Windows.Forms.View.Details;
            // 
            // colManifest
            // 
            this.colManifest.Text = "Manifest";
            this.colManifest.Width = 87;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 281;
            // 
            // tbpContractViewer
            // 
            this.tbpContractViewer.Controls.Add(this.wbContractViewer);
            this.tbpContractViewer.Location = new System.Drawing.Point(4, 22);
            this.tbpContractViewer.Name = "tbpContractViewer";
            this.tbpContractViewer.Size = new System.Drawing.Size(321, 309);
            this.tbpContractViewer.TabIndex = 3;
            this.tbpContractViewer.Text = "ContractViewer";
            this.tbpContractViewer.UseVisualStyleBackColor = true;
            // 
            // wbContractViewer
            // 
            this.wbContractViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbContractViewer.Location = new System.Drawing.Point(0, 0);
            this.wbContractViewer.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbContractViewer.Name = "wbContractViewer";
            this.wbContractViewer.Size = new System.Drawing.Size(321, 309);
            this.wbContractViewer.TabIndex = 0;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(563, 24);
            this.mnuMain.TabIndex = 2;
            this.mnuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDNRToolStripMenuItem,
            this.openDNRToolStripMenuItem,
            this.saveDNRToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newDNRToolStripMenuItem
            // 
            this.newDNRToolStripMenuItem.Name = "newDNRToolStripMenuItem";
            this.newDNRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newDNRToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.newDNRToolStripMenuItem.Text = "&New DNR";
            this.newDNRToolStripMenuItem.Click += new System.EventHandler(this.newDNRToolStripMenuItem_Click);
            // 
            // openDNRToolStripMenuItem
            // 
            this.openDNRToolStripMenuItem.Name = "openDNRToolStripMenuItem";
            this.openDNRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openDNRToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.openDNRToolStripMenuItem.Text = "&Open DNR";
            this.openDNRToolStripMenuItem.Click += new System.EventHandler(this.openDNRToolStripMenuItem_Click);
            // 
            // saveDNRToolStripMenuItem
            // 
            this.saveDNRToolStripMenuItem.Name = "saveDNRToolStripMenuItem";
            this.saveDNRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveDNRToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.saveDNRToolStripMenuItem.Text = "&Save DNR";
            this.saveDNRToolStripMenuItem.Click += new System.EventHandler(this.saveDNRToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // openDialog
            // 
            this.openDialog.Filter = "Application Extensions|*.dll";
            this.openDialog.Title = "Choose DLL to use";
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "dnr";
            this.saveDialog.Filter = "DNR Manifest Archives|*.dnr";
            this.saveDialog.Title = "Save DNR";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 381);
            this.Controls.Add(this.splMain);
            this.Controls.Add(this.appStatus);
            this.Controls.Add(this.mnuMain);
            this.Name = "frmMain";
            this.Text = "Resusitator - DNR Archive Utility";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.appStatus.ResumeLayout(false);
            this.appStatus.PerformLayout();
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            this.mnuPopup.ResumeLayout(false);
            this.tbcOutput.ResumeLayout(false);
            this.tbpProperties.ResumeLayout(false);
            this.tbpOutput.ResumeLayout(false);
            this.tbpOutput.PerformLayout();
            this.tbpErrors.ResumeLayout(false);
            this.tbpContractViewer.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker dnrParser;
        private System.Windows.Forms.StatusStrip appStatus;
        private System.Windows.Forms.ToolStripStatusLabel tssStatus;
        private System.Windows.Forms.ToolStripProgressBar pgFileProcess;
        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.TreeView tvwBrowser;
        private System.Windows.Forms.TabControl tbcOutput;
        private System.Windows.Forms.TabPage tbpProperties;
        private System.Windows.Forms.PropertyGrid pgInspector;
        private System.Windows.Forms.TabPage tbpOutput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.TabPage tbpErrors;
        private System.Windows.Forms.ListView lvwErrors;
        private System.Windows.Forms.ColumnHeader colManifest;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mnuPopup;
        private System.Windows.Forms.ToolStripMenuItem newDNRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDNRToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ImageList appImages;
        private System.Windows.Forms.ToolStripMenuItem addManifestToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.ToolStripMenuItem saveDNRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedManifestToolStripMenuItem;
        private System.Windows.Forms.TabPage tbpContractViewer;
        private System.Windows.Forms.WebBrowser wbContractViewer;
    }
}