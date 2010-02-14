//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using Ecell;
using System.IO;

namespace Ecell.Job
{
    /// <summary>
    /// SessionProxy to execute the simulation in Local Environment.
    /// </summary>
    public class LocalJob : Job
    {
        Process m_currentProcess = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalJob()
            : base()
        {
            this.JobDirectory = "";
            this.Machine = "Local";
        }

        /// <summary>
        /// Constructors with job id.
        /// </summary>
        /// <param name="jobid">job id.</param>
        public LocalJob(int jobid)
            : base(jobid)
        {
            this.JobDirectory = "";
            this.Machine = "Local";
        }

        /// <summary>
        /// Retry this session.
        /// </summary>
        public override void retry()
        {
            this.stop();
            this.Status = JobStatus.QUEUED;
            this.run();
        }

        /// <summary>
        /// Start this session.
        /// </summary>
        public override void run()
        {
            try
            {
                string pArgument = "\"" + Util.GetAnalysisDir() + "\\idesession\" \""  + Argument + "\"";
                Process p = new Process();
                p.StartInfo.FileName = ScriptFile;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.Arguments = @pArgument;
                p.StartInfo.WorkingDirectory = Util.GetAnalysisDir();
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                //if (p.StartInfo.EnvironmentVariables.ContainsKey("IRONPYTHONSTARTUP"))
                //{
                //    p.StartInfo.EnvironmentVariables.Remove("IRONPYTHONSTARTUP");
                //}
                //p.StartInfo.EnvironmentVariables.Add("IRONPYTHONSTARTUP", Util.GetStartupFile());
                this.Status = JobStatus.RUNNING;
                p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                this.Status = JobStatus.RUNNING;
                p.Start();
                m_currentProcess = p;
                ProcessID = m_currentProcess.Id;

                p.BeginErrorReadLine();
                p.BeginOutputReadLine();

                //ProcessStartInfo psi = new ProcessStartInfo();
                //psi.FileName = ScriptFile;
                //psi.UseShellExecute = false;
                //psi.CreateNoWindow = true;
                //psi.Arguments = @Argument;
                //psi.WorkingDirectory = Util.GetAnalysisDir();
                //psi.RedirectStandardError = true;
                //psi.RedirectStandardOutput = false;
                //if (psi.EnvironmentVariables.ContainsKey("IRONPYTHONSTARTUP"))
                //{
                //    psi.EnvironmentVariables.Remove("IRONPYTHONSTARTUP");
                //}
                //psi.EnvironmentVariables.Add("IRONPYTHONSTARTUP", Util.GetStartupFile());
                //this.Status = JobStatus.RUNNING;
                //m_currentProcess = Process.Start(psi);
                //ProcessID = m_currentProcess.Id;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
                this.Status = JobStatus.ERROR;
            }
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.StdErr += e.Data + "\n";
        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.StdErr += e.Data + "\n";
        }

        /// <summary>
        /// Stop this session.
        /// </summary>
        public override void stop()
        {
            if (this.Status != JobStatus.RUNNING && this.Status != JobStatus.QUEUED &&
                this.Status != JobStatus.NONE)
                return;
            if (m_currentProcess != null)
            {
                Status = JobStatus.STOPPED;
                try
                {
                    int i = m_currentProcess.ExitCode;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    m_currentProcess.Kill();
                }
                this.StdErr = "stop ...";
                m_currentProcess = null;
            }
            if (Status == JobStatus.QUEUED || Status == JobStatus.RUNNING || 
                Status == JobStatus.NONE)
            {
                Status = JobStatus.STOPPED;
            }
        }

        /// <summary>
        /// Update the status of job.
        /// </summary>
        public override void Update()
        {
            if (m_currentProcess == null || Status != JobStatus.RUNNING ||
                !m_currentProcess.HasExited)
                return;

            try
            {
                int exitCode = m_currentProcess.ExitCode;
                if (exitCode == 0)
                {
                    this.Status = JobStatus.FINISHED;
                }
                else
                {                       
                    this.Status = JobStatus.ERROR;
                    Manager.NotifyErrorMessage(this.GroupName, this.StdErr);
                }
                m_currentProcess = null;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Get the stream of StdOut for this process.
        /// </summary>
        /// <returns>StreamReader.</returns>
        public override string GetStdOut()
        {
            return "";
        }

        /// <summary>
        /// Get the stream of StdErr for this process.
        /// </summary>
        /// <returns>StreamReader.</returns>
        public override string GetStdErr()
        {
            return this.StdErr;
        }

        /// <summary>
        /// Prepare the file before the process run.
        /// </summary>
        public override void PrepareProcess()
        {
            base.PrepareProcess();
        }

        /// <summary>
        /// Get the log data of key.
        /// </summary>
        /// <param name="key">the key of logger.</param>
        /// <returns>the list of log.</returns>
        public override Dictionary<double, double> GetLogData(string key)
        {
            Dictionary<double, double> result = new Dictionary<double, double>();
            if (key == null)
                return result;

            string fileName = Util.GetOutputFileName(key, false);

            foreach (string extFileName in ExtraFileList)
            {
                if (!extFileName.Contains(fileName))
                    continue;
                if (!System.IO.File.Exists(extFileName))
                    return result;
                StreamReader hReader = new StreamReader(extFileName, Encoding.UTF8);
                char splitter = '\t';
                string ext = Path.GetExtension(extFileName);
                if (!string.IsNullOrEmpty(ext) || ext.ToLower().Equals(".csv"))
                    splitter = ',';

                while (!hReader.EndOfStream)
                {
                    string line = hReader.ReadLine();
                    if (line.StartsWith("#")) continue;
                    string[] ele = line.Split(new char[] { splitter });
                    if (ele.Length >= 2)
                    {
                        double time = Convert.ToDouble(ele[0]);
                        double value = Convert.ToDouble(ele[1]);
                        result.Add(time, value);
                    }
                }
                hReader.Close();
            }
            return result;
        }

        /// <summary>
        /// Get the default script name.
        /// </summary>
        /// <returns>the script file name.</returns>
        static public string GetDefaultScript()
        {
            return Util.GetAnalysisDir() + "/python.exe";
        }
    }
}
