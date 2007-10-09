//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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

namespace SessionManager
{
    public class LocalSessionProxy : SessionProxy
    {
        Process m_currentProcess = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalSessionProxy()
            : base()
        {
            this.JobDirectory = "";
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
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ScriptFile;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.Arguments = Argument;
            psi.WorkingDirectory = GetAnalysisDir();
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            this.Status = JobStatus.RUNNING;
            m_currentProcess = Process.Start(psi);
            ProcessID = m_currentProcess.Id;
        }

        /// Stop this session.
        /// </summary>
        public override void stop()
        {
            if (this.Status != JobStatus.RUNNING || this.Status != JobStatus.QUEUED)
                return;
            if (m_currentProcess != null)
            {
                Status = JobStatus.STOPPED;
                try {
                    int i = m_currentProcess.ExitCode;
                }catch (Exception ex)
                {
                    ex.ToString();
                    m_currentProcess.Kill();
                }
                m_currentProcess = null;
            }
            if (Status != JobStatus.QUEUED)
            {
                Status = JobStatus.STOPPED;
            }
        }

        /// <summary>
        /// Update the status of job.
        /// </summary>
        public override void Update()
        {
            try
            {
                if (m_currentProcess != null && Status == JobStatus.RUNNING)
                {
                    int exitCode = m_currentProcess.ExitCode;
                    if (exitCode == 0)
                    {
                        this.Status = JobStatus.FINISHED;
                    }
                    else
                    {                       
                        this.Status = JobStatus.ERROR;
                    }
                    m_currentProcess = null;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                // nothing.
            }
        }

        /// <summary>
        /// Get the stream of StrOut for this process.
        /// </summary>
        /// <returns>StreamReader.</returns>
        public override System.IO.StreamReader GetStdOut()
        {
            if (m_currentProcess == null) return null;
            return m_currentProcess.StandardOutput;
        }

        /// <summary>
        /// Get the stream of StdErr for this process.
        /// </summary>
        /// <returns>StreamReader.</returns>
        public override System.IO.StreamReader GetStdErr()
        {
            if (m_currentProcess == null) return null;
            return m_currentProcess.StandardError;
        }

        /// <summary>
        /// Prepare the file before the process run.
        /// </summary>
        public override void PrepareProcess()
        {
            base.PrepareProcess();
        }


        private string GetAnalysisDir()
        {
            string l_currentDir = null;
            Microsoft.Win32.RegistryKey l_subkey = null;
            Microsoft.Win32.RegistryKey l_key = Microsoft.Win32.Registry.LocalMachine;
            l_subkey = l_key.OpenSubKey("software\\KeioUniv\\E-Cell IDE");
            if (l_subkey != null)
            {
                l_currentDir = (string)l_subkey.GetValue("E-Cell IDE Analysis");
            }
            return l_currentDir;

        }

    }
}
