﻿namespace Tetsuo.Frontdoor
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BrokerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.BrokerInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // BrokerProcessInstaller
            // 
            this.BrokerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.BrokerProcessInstaller.Password = null;
            this.BrokerProcessInstaller.Username = null;
            // 
            // BrokerInstaller
            // 
            this.BrokerInstaller.Description = "Implementation of the Hub and Spoke Service Infrastructure (net.obliteracy.tetsuo) methodology of " +
                "WCF spawned services.";
            this.BrokerInstaller.DisplayName = "net.obliteracy.tetsuo Hub Broker";
            this.BrokerInstaller.ServiceName = "HSBroker";
            this.BrokerInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.BrokerProcessInstaller,
            this.BrokerInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller BrokerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller BrokerInstaller;
    }
}