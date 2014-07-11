using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Tetsuo.Core.IO
{
    public class CommandLineMonitor
    {
        public TextBoxBase TextControl { get; set; }
        public delegate void OnStreamUpdated(string value);
        public event OnStreamUpdated StreamUpdated;
        protected BackgroundWorker outputWorker;
        protected BackgroundWorker errorWorker;

        Process p = new Process();
        ProcessStartInfo si;
        StreamWriter sw;
        StreamReader sr;
        StreamReader err;

        public CommandLineMonitor()
        {
            outputWorker = new BackgroundWorker();
            errorWorker = new BackgroundWorker();
            outputWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
            errorWorker.DoWork += new DoWorkEventHandler(errorWorker_DoWork);
            errorWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            outputWorker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            outputWorker.WorkerReportsProgress = true;
            errorWorker.WorkerReportsProgress = true;
        }

        public void Initialize(string exe = "cmd",string args="")
        {
            si = new ProcessStartInfo(exe);
            if (!string.IsNullOrEmpty(args))
                si.Arguments = args;
            si.UseShellExecute = false;
            si.RedirectStandardInput = true;
            si.RedirectStandardOutput = true;
            si.RedirectStandardError = true;
            si.CreateNoWindow = true;
            p.StartInfo = si;
            p.Start();
            sw = p.StandardInput;
            sr = p.StandardOutput;
            err = p.StandardError;

            sw.AutoFlush = true;
            outputWorker.RunWorkerAsync();
            errorWorker.RunWorkerAsync();
        }

        void errorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!err.EndOfStream)
            {
                errorWorker.ReportProgress(1, err.ReadLine() + Environment.NewLine);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!(TextControl == null))
            {
                TextControl.AppendText(e.UserState.ToString());
                TextControl.SelectionStart = TextControl.TextLength;
            }
            if (!(StreamUpdated == null))
                StreamUpdated(e.UserState.ToString());

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!sr.EndOfStream)
            {
                outputWorker.ReportProgress(0, sr.ReadLine() + Environment.NewLine);
            }

        }

        public void SendCommand(string command, string executable = "",string args="")
        {
            if (!string.IsNullOrEmpty(executable))
            {
                if (!string.IsNullOrEmpty(args))
                    si = new ProcessStartInfo(executable, args);
                else
                    si = new ProcessStartInfo(executable);
                p.StartInfo = si;
                p.Start();
            }
            else
                sw.WriteLine(command);
        }
    }
}
