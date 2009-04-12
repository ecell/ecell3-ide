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
        public static string SERVER_NAME = "servername";
        public static string PROVIDER_NAME = "provider";
        public static string SCRIPT_NAME = "scriptname";
        public static string TOPDIR_NAME = "topdir";
        public static string PASSWORD = "password";

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
            m_process.StartInfo.CreateNoWindow = false;
            m_process.StartInfo.RedirectStandardError = true;
            m_process.StartInfo.RedirectStandardInput = true;
            m_process.StartInfo.RedirectStandardOutput = true;

            m_process.OutputDataReceived += new DataReceivedEventHandler(m_process_OutputDataReceived);
            m_process.ErrorDataReceived += new DataReceivedEventHandler(m_process_ErrorDataReceived);

            m_process.Start();
            m_process.BeginErrorReadLine();
            m_process.BeginOutputReadLine();
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
                // grid-proxy-init
                cmd = "grid-proxy-init";
                m_process.StandardInput.WriteLine(cmd);
                m_process.StandardInput.Flush();
                m_process.StandardInput.WriteLine(Param[GlobusJob.PASSWORD].ToString());
                m_process.StandardInput.Flush();

                // 実行
                // cog-job-submit -e $script -args $ROOT/$JobID/$jobfile -p $provider -s $server
                cmd = "cog-job-submit";
                argument = " -e " + Param[GlobusJob.SCRIPT_NAME].ToString() 
                    + " -args \"" + ScriptFile + "\""
                    + " -p " + Param[GlobusJob.PROVIDER_NAME]
                    + " -s " + Param[GlobusJob.SERVER_NAME]
                    + "-target \"" + Param[GlobusJob.TOPDIR_NAME].ToString() + "/" 
                    + this.Machine + "/" + this.JobID + "\"";
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

        /// <summary>
        /// Pprepare the execution of this job.
        /// </summary>
        public override void PrepareProcess()
        {
            string cmd = "";
            string argument = "";

            try
            {
                // not implements
                // 初期化
                cmd = "grid-proxy-init";
                m_process.StandardInput.WriteLine(cmd);
                m_process.StandardInput.Flush();

                // 実行ディレクトリを作成
                // cog-job-submit -e /bin/mkdir -args $ROOT/$JobID -p $Provider -s $Server           
                cmd = "cog-job-submit";
                argument = " -e /bin/mkdir -args \"-p " + Param[GlobusJob.TOPDIR_NAME].ToString()
                    + "/" + this.Machine + "/" + this.JobID + "\"" 
                    + " -p " + Param[GlobusJob.PROVIDER_NAME].ToString()
                    + " -s " + Param[GlobusJob.SERVER_NAME];
                m_process.StandardInput.WriteLine(cmd + argument);
                m_process.StandardInput.Flush();

                string dFileName = JobID + ".ess";
                File.Copy(ScriptFile, dFileName);
                string sModelName = Path.GetFileNameWithoutExtension(ScriptFile) + ".eml";
                string dModelName = JobID + ".eml";

                // grid-ftpでサーバにスクリプトを持っていく
                // cog-file-transfer -s file://tmp/$jobfile -d gsiftp://$Server/$ROOT/$JobID
                cmd = "cog-file-transfer";
                argument = " -s file://tmp/" + dFileName + " -d gsiftp://"
                + Param[GlobusJob.SERVER_NAME].ToString() + "/"
                + this.Machine + "/" + this.JobID + "/";
                m_process.StandardInput.WriteLine(cmd + argument);
                m_process.StandardInput.Flush();

                cmd = "cog-file-transfer";
                argument = " -s file://tmp/" + dModelName + " -d gsiftp://"
                + Param[GlobusJob.SERVER_NAME].ToString() + "/"
                + this.Machine + "/" + this.JobID + "/";
                m_process.StandardInput.WriteLine(cmd + argument);
                m_process.StandardInput.Flush();

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
            }
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
                if (this.Status == JobStatus.ERROR)
                    return result;
                m_process = new Process();
                m_process.StartInfo.FileName = "cmd.exe";
                m_process.StartInfo.UseShellExecute = false;
                m_process.StartInfo.CreateNoWindow = false;
                m_process.StartInfo.RedirectStandardError = true;
                m_process.StartInfo.RedirectStandardInput = true;
                m_process.StartInfo.RedirectStandardOutput = true;

                m_process.OutputDataReceived += new DataReceivedEventHandler(m_process_OutputDataReceived);
                m_process.ErrorDataReceived += new DataReceivedEventHandler(m_process_ErrorDataReceived);

                m_process.Start();
                m_process.BeginErrorReadLine();
                m_process.BeginOutputReadLine();

                string cmd = "";
                string argument = "";
                if (key == null)
                    return result;

                string fileName = key.Replace("/", "_");
                fileName = fileName.Replace(":", "_");
                fileName = fileName + ".ecd";

                // 初期化
                // grid-proxy-init
                cmd = "grid-proxy-init";
                m_process.StandardInput.WriteLine(cmd);
                m_process.StandardInput.Flush();

                // grid-ftpでログをサーバから持ってくる
                // cog-file-transfer -s gsiftp://$Server/$ROOT/$JobID/$logfile -d $ROOT/$JobID
                cmd = "cog-file-transfer";
                argument = " -d file://tmp/tmp.log -d gsiftp://"
                + Param[GlobusJob.SERVER_NAME].ToString() + "/"
                + this.Machine + "/" + this.JobID + "/" + fileName;
                m_process.StandardInput.WriteLine(cmd);
                m_process.StandardInput.Flush();

                while (this.Status != JobStatus.ERROR && !File.Exists(fileName))
                {
                    Thread.Sleep(30 * 1000);
                }

                // Tempに移動
                File.Move("tmp.log", JobDirectory + "/" + fileName);
                //File.Move($logfile, $tmpdir/$logfile)
                // ログの読み込み
                //
                StreamReader hReader = new StreamReader(JobDirectory + "/" + fileName, Encoding.ASCII);
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
            if (this.Status == JobStatus.RUNNING && e.Data.Contains("Job Completed"))
            {
                this.Status = JobStatus.FINISHED;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
            }
            else if (this.Status == JobStatus.RUNNING && e.Data.Contains("failed"))
            {
                this.Status = JobStatus.ERROR;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
            }
        }

        private void m_process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.StdErr += e.Data + "\n";
            if (this.Status == JobStatus.RUNNING && e.Data.Contains("Job Completed"))
            {
                this.Status = JobStatus.FINISHED;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
            }
            else if (this.Status == JobStatus.RUNNING && e.Data.Contains("failed"))
            {
                this.Status = JobStatus.ERROR;
                if (m_process != null)
                {
                    m_process.Kill();
                    m_process = null;
                }
            }
        }
        #endregion
    }
}
