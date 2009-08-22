//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
using System.IO;

using System.Threading;
using System.Diagnostics;

namespace Ecell.Job
{
    /// <summary>
    /// Job class for globus.
    /// </summary>
    public class GlobusJob : Job
    {
        public static string SERVER_NAME = MessageResources.NameJobServerName;
        public static string PROVIDER_NAME = MessageResources.NameJobProvider;
        public static string SCRIPT_NAME = MessageResources.NameJobScriptName;
        public static string TOPDIR_NAME = MessageResources.NameJobTopDir;
        public static string PASSWORD = MessageResources.NameJobPassword;

        private Process m_process = null;

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public GlobusJob()
            : base()
        {
            this.JobDirectory = "";
            m_process = new Process();
            m_process.StartInfo.FileName = "cmd.exe";
            m_process.StartInfo.UseShellExecute = false;
            m_process.StartInfo.CreateNoWindow = true;
            m_process.StartInfo.RedirectStandardError = true;
            m_process.StartInfo.RedirectStandardInput = true;
            m_process.StartInfo.RedirectStandardOutput = true;

            m_process.OutputDataReceived += new DataReceivedEventHandler(m_process_OutputDataReceived);
            m_process.ErrorDataReceived += new DataReceivedEventHandler(m_process_ErrorDataReceived);

            m_process.Start();
            m_process.BeginErrorReadLine();
            m_process.BeginOutputReadLine();
        }

        /// <summary>
        /// Constructors with job id.
        /// </summary>
        /// <param name="jobid">job id.</param>
        public GlobusJob(int jobid)
            : base(jobid)
        {
        }
        #endregion
        /// <summary>
        /// Retry this job.
        /// </summary>
        public override void retry()
        {
            this.stop();
            this.Status = JobStatus.QUEUED;
            this.run();
        }

        /// <summary>
        /// Run this job.
        /// </summary>
        public override void run()
        {
            string cmd = "";
            string argument = "";

            try
            {
                this.Status = JobStatus.RUNNING;

                // 初期化
                // prepareと競合してcogrunが正常に動作しない
                // grid-proxy-init
                //cmd = "grid-proxy-init";
                //m_process.StandardInput.WriteLine(cmd);
                //m_process.StandardInput.Flush();
                //m_process.StandardInput.WriteLine(Param[GlobusJob.PASSWORD].ToString());
                //Thread.Sleep(5 * 1000);
                //m_process.StandardInput.Flush();

                string cogdir = System.Environment.GetEnvironmentVariable("COG_HOME");
                string curdir = System.Environment.CurrentDirectory + "\\";
                Uri u1 = new Uri(curdir);
                Uri u2 = new Uri(cogdir);
                string relativePath = u1.MakeRelativeUri(u2).ToString();
                relativePath = relativePath.Replace('/', '\\');

                // 実行
                // cog-job-submit -e $script -args $ROOT/$JobID/$jobfile -p $provider -s $server
                cmd = relativePath + "\\bin\\cogrun";
                argument = " -e " + Param[GlobusJob.SCRIPT_NAME].ToString() 
                    + " -args " + ScriptFile
                    + " -p " + Param[GlobusJob.PROVIDER_NAME]
                    + " -s " + Param[GlobusJob.SERVER_NAME]
                    + " -d " + Param[GlobusJob.TOPDIR_NAME].ToString() + "/" 
                    + this.Machine + "/" + this.JobID;
                m_process.StandardInput.WriteLine(cmd + argument);
                m_process.StandardInput.Flush();
            }
            catch (Exception)
            {
                try
                {
                    if (m_process != null)
                        m_process.Kill();                   
                }
                catch (Exception)
                {
                }
                this.Status = JobStatus.ERROR;
                m_process = null;
                NotifyErrorMessage("cog initialize error.");
            }
        }

        /// <summary>
        /// Stop this job.
        /// </summary>
        public override void stop()
        {
            if (this.Status != JobStatus.RUNNING && this.Status != JobStatus.QUEUED &&
                this.Status != JobStatus.NONE)
                return;
            if (m_process != null)
            {
                Status = JobStatus.STOPPED;
                try
                {
                    m_process.Kill();
                }
                catch (Exception)
                {
                }
                this.StdErr = "stop ...";
                m_process = null;
            }
            if (Status == JobStatus.QUEUED || Status == JobStatus.RUNNING ||
                Status == JobStatus.NONE)
            {
                Status = JobStatus.STOPPED;
            }            
        }

        public override void Update()
        {
        }

        /// <summary>
        /// Get the string of standard output.
        /// </summary>
        /// <returns>The string of standart output.</returns>
        public override string GetStdOut()
        {
            return this.StdErr;
        }

        /// <summary>
        /// Get the string of standard error.
        /// </summary>
        /// <returns>The string of standard error.</returns>
        public override string GetStdErr()
        {
            return this.StdErr;
        }

        private bool m_isRunning = false;
        /// <summary>
        /// Pprepare the execution of this job.
        /// </summary>
        public override void PrepareProcess()
        {
            string cmd = "";
            string argument = "";

            try
            {
                string cogdir = System.Environment.GetEnvironmentVariable("COG_HOME");
                string curdir = System.Environment.CurrentDirectory + "\\";
                Uri u1 = new Uri(curdir);
                Uri u2 = new Uri(cogdir);
                string relativePath = u1.MakeRelativeUri(u2).ToString();
                relativePath = relativePath.Replace('/', '\\');
                
                // 初期化
                cmd = relativePath + "\\bin\\grid-proxy-init";
                m_process.StandardInput.WriteLine(cmd);
//                Thread.Sleep(5 * 1000);
                m_process.StandardInput.Flush();
                m_process.StandardInput.WriteLine(Param[GlobusJob.PASSWORD].ToString());
                Thread.Sleep(5 * 1000);
                m_process.StandardInput.Flush();

                cmd = relativePath + "\\bin\\cogrun.bat";
                argument = " -e /bin/mkdir -args \" -p " + Param[GlobusJob.TOPDIR_NAME].ToString()
                    + "/" + this.Machine + "/" + this.JobID + "\"" 
                    + " -p " + Param[GlobusJob.PROVIDER_NAME].ToString()
                    + " -s " + Param[GlobusJob.SERVER_NAME];
                cmd = cmd + argument;
                m_process.StandardInput.WriteLine(cmd);
                m_process.StandardInput.Flush();
                m_isRunning = true;
                while (m_isRunning)
                {
                    Thread.Sleep(1 * 1000);
                    System.Windows.Forms.Application.DoEvents();
                }

                string dFileName = JobID + ".ess";
                File.Delete(dFileName); 
                File.Copy(Argument, dFileName);
                string sModelName = Path.GetDirectoryName(Argument) + "/" +  Path.GetFileNameWithoutExtension(Argument) + ".eml";
                string dModelName = JobID + ".eml";
                File.Delete(dModelName);
                File.Copy(sModelName, dModelName);

                // grid-ftpでサーバにスクリプトを持っていく
                // cog-file-transfer -s file://tmp/$jobfile -d gsiftp://$Server/$ROOT/$JobID
                cmd = relativePath + "\\bin\\cog-file-transfer";
                argument = " -s file://tmp/" + dFileName + " -d gsiftp://"
                + Param[GlobusJob.SERVER_NAME].ToString() + "/"
                + Param[GlobusJob.TOPDIR_NAME].ToString() + "/"
                + this.Machine + "/" + this.JobID + "/" + dFileName;
                m_process.StandardInput.WriteLine(cmd + argument);
//                m_process.StandardInput.Flush();

                cmd = relativePath + "\\bin\\cog-file-transfer";
                argument = " -s file://tmp/" + dModelName + " -d gsiftp://"
                + Param[GlobusJob.SERVER_NAME].ToString() + "/"
                + Param[GlobusJob.TOPDIR_NAME].ToString() + "/"
                + this.Machine + "/" + this.JobID + "/" + dModelName;
                m_process.StandardInput.WriteLine(cmd + argument);
//                m_process.StandardInput.Flush();

                ScriptFile = dFileName;
                base.PrepareProcess();
            }
            catch (Exception)
            {
                try
                {
                    if (m_process != null)
                        m_process.Kill();
                }
                catch (Exception ex)
                {
                    System.Console.Write(ex.ToString());
                }
                this.Status = JobStatus.ERROR;
                m_process = null;
                NotifyErrorMessage("Prepare to the executing jobs.");
            }
        }

        /// <summary>
        /// Send the report to ReportManager.
        /// </summary>
        /// <param name="message">the report message.</param>
        private void NotifyErrorMessage(string message)
        {
            Manager.NotifyErrorMessage(this.GroupName, message);
        }


        /// <summary>
        /// Get log data from server node.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Dictionary<double, double> GetLogData(string key)
        {
            Dictionary<double, double> result = new Dictionary<double, double>();
            try
            {
                string cogdir = System.Environment.GetEnvironmentVariable("COG_HOME");
                string curdir = System.Environment.CurrentDirectory + "\\";
                Uri u1 = new Uri(curdir);
                Uri u2 = new Uri(cogdir);
                string relativePath = u1.MakeRelativeUri(u2).ToString();
                relativePath = relativePath.Replace('/', '\\');

                if (this.Status == JobStatus.ERROR)
                    return result;
                m_process = new Process();
                m_process.StartInfo.FileName = "cmd.exe";
                m_process.StartInfo.UseShellExecute = false;
                m_process.StartInfo.CreateNoWindow = true;
                m_process.StartInfo.RedirectStandardError = true;
                m_process.StartInfo.RedirectStandardInput = true;
                m_process.StartInfo.RedirectStandardOutput = true;

                m_process.Start();
                m_process.BeginErrorReadLine();
                m_process.BeginOutputReadLine();

                string cmd = "";
                string argument = "";
                if (key == null)
                    return result;

                string ufileName = Util.GetOutputFileName(key, true);
                string wfileName = Util.GetOutputFileName(key, false); 

                // 初期化
                // grid-proxy-init
                cmd = "grid-proxy-init";
                m_process.StandardInput.WriteLine(cmd);
                m_process.StandardInput.Flush();
                m_process.StandardInput.WriteLine(Param[GlobusJob.PASSWORD].ToString());
                Thread.Sleep(5 * 1000);
                m_process.StandardInput.Flush();


                // grid-ftpでログをサーバから持ってくる
                // cog-file-transfer -s gsiftp://$Server/$ROOT/$JobID/$logfile -d $ROOT/$JobID
                cmd = relativePath + "\\bin\\cog-file-transfer";
                argument = " -d file://tmp/tmp.log -s gsiftp://"
                + Param[GlobusJob.SERVER_NAME].ToString() + ""
                + Param[GlobusJob.TOPDIR_NAME].ToString() + "/"
                + this.Machine + "/" + this.JobID + "/" + ufileName;
                cmd = cmd + argument;
                m_process.StandardInput.WriteLine(cmd);
                m_process.StandardInput.Flush();

                while (!File.Exists("tmp.log"))
                {
                    Thread.Sleep(1 * 1000);
                    System.Windows.Forms.Application.DoEvents();
                }

                // Tempに移動
                File.Move("tmp.log", JobDirectory + "/" + wfileName);
                //File.Move($logfile, $tmpdir/$logfile)
                // ログの読み込み
                //
                StreamReader hReader = new StreamReader(JobDirectory + "/" + wfileName, Encoding.ASCII);
                char splitter = '\t';

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
                m_process.Kill();
                m_process = null;
            }
            catch (Exception)
            {
                if (m_process != null)
                    m_process.Kill();
                m_process = null;
            }
            return result;
        }

        /// <summary>
        /// Get default script.
        /// </summary>
        /// <returns></returns>
        static public string GetDefaultScript()
        {
            return "";
        }

        #region Events
        private void m_process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.StdErr += e.Data + "\n";
            if (this.Status == JobStatus.RUNNING && (e.Data != null && e.Data.Contains("Job completed")))
            {
                this.Status = JobStatus.FINISHED;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
            }
            else if (e.Data != null && (e.Data.Contains("failed") || e.Data.Contains("Error")))
            {
                this.Status = JobStatus.ERROR;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
                NotifyErrorMessage(e.Data);
            }
            if (e.Data != null &&  
                (e.Data.Contains("Job completed") || e.Data.Contains("failed") || 
                e.Data.Contains("is valid")))
                m_isRunning = false;
        }

        private void m_process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.StdErr += e.Data + "\n";
            if (this.Status == JobStatus.RUNNING && (e.Data != null && e.Data.Contains("Job completed")))
            {
                this.Status = JobStatus.FINISHED;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
            }
            else if (e.Data != null && (e.Data.Contains("failed") || e.Data.Contains("Error")))
            {
                this.Status = JobStatus.ERROR;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
                NotifyErrorMessage(e.Data);
            }
            if (e.Data != null && 
                (e.Data.Contains("Job completed") || e.Data.Contains("failed") || 
                e.Data.Contains("is valid")))
                m_isRunning = false;
        }
        #endregion
    }
}
