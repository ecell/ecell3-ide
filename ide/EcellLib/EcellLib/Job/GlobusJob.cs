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

using System.Diagnostics;

namespace Ecell.Job
{
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


        }
        #endregion

        public override void retry()
        {
            this.stop();
            this.Status = JobStatus.QUEUED;
            this.run();
        }

        public override void run()
        {
            string cmd = "";
            string argument = "";

            // not implements
            // 初期化
            // grid-proxy-init
            cmd = "grid-proxy-init";

            // 実行
            // cog-job-submit -e $script -args $ROOT/$JobID/$jobfile -p $provider -s $server
            cmd = "cog-job-submit";
            argument = " -e " + Param[GlobusJob.SCRIPT_NAME].ToString() + " -args "
            + Param[GlobusJob.TOPDIR_NAME].ToString() + "/" + this.Machine + "/"
            + this.JobID + "/" + ScriptFile
            + " -p " + Param[GlobusJob.PROVIDER_NAME]
            + " -s " + Param[GlobusJob.SERVER_NAME];
        }

        public override void stop()
        {
            // not implements
        }

        public override void Update()
        {
            // not implements
        }

        public override string GetStdOut()
        {
            // not implements
            return base.GetStdOut();
        }

        public override string GetStdErr()
        {
            // not implements
            return base.GetStdErr();
        }

        public override void PrepareProcess()
        {
            string cmd = "";
            string argument = "";

            // not implements
            // 初期化
            cmd = "grid-proxy-init";

            // 実行ディレクトリを作成
            // cog-job-submit -e /bin/mkdir -args $ROOT/$JobID -p $Provider -s $Server           
            cmd = "cog-job-submit";
            argument = "-e /bin/mkdir -args " + Param[GlobusJob.TOPDIR_NAME].ToString()
                + "/" + this.Machine + "/" + this.JobID 
                + " -p " + Param[GlobusJob.PROVIDER_NAME].ToString()
                + " -s " + Param[GlobusJob.SERVER_NAME];
            
            string dFileName = JobID + ".py";
            File.Copy(Argument, dFileName);
            // grid-ftpでサーバにスクリプトを持っていく
            // cog-file-transfer -s file://tmp/$jobfile -d gsiftp://$Server/$ROOT/$JobID
            cmd = "cog-file-transfer";
            argument = " -s file://tmp/" + dFileName + " -d gsiftp://"
            + Param[GlobusJob.SERVER_NAME].ToString() + "/"
            + this.Machine + "/" + this.JobID + "/";

            ScriptFile = dFileName;
            base.PrepareProcess();
        }

        public override Dictionary<double, double> GetLogData(string key)
        {
            string cmd = "";
            string argument = "";
            Dictionary<double, double> result = new Dictionary<double, double>();
            if (key == null)
                return result;

            string fileName = key.Replace("/", "_");
            fileName = fileName + ".csv";

            // not implements
            // 初期化
            // grid-proxy-init
            cmd = "grid-proxy-init";

            // grid-ftpでログをサーバから持ってくる
            // cog-file-transfer -s gsiftp://$Server/$ROOT/$JobID/$logfile -d $ROOT/$JobID
            cmd = "cog-file-transfer";
            argument = " -d file://tmp/tmp.log -d gsiftp://"
            + Param[GlobusJob.SERVER_NAME].ToString() + "/"
            + this.Machine + "/" + this.JobID + "/" + fileName;

          
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

            return result;
        }

        static public string GetDefaultScript()
        {
            return "";
        }
    }
}
