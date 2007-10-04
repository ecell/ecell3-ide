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
        }

        /// <summary>
        /// Retry this session.
        /// </summary>
        public new void retry()
        {
            this.stop();
            this.Status = JobStatus.QUEUED;
            this.run();
        }

        /// <summary>
        /// Start this session.
        /// </summary>
        public new void run()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ScriptFile;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.Arguments = Argument;
            m_currentProcess = Process.Start(psi);
            m_currentProcess.Exited += new EventHandler(ProcessExited);
            ProcessID = m_currentProcess.Id;
            this.Status = JobStatus.RUNNING;
        }

        /// Stop this session.
        /// </summary>
        public new void stop()
        {
            if (m_currentProcess != null)
            {
                m_currentProcess.Kill();
                Status = JobStatus.STOPPED;
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
        public new void Update()
        {
            try
            {
                if (m_currentProcess != null)
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
                // nothing.
            }
        }

        /// <summary>
        /// End process event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProcessExited(object sender, EventArgs e)
        {
            if (m_currentProcess.ExitCode == 0)
            {
                this.Status = JobStatus.FINISHED;
            }
            else
            {
                this.Status = JobStatus.ERROR;
            }
        }


    }
}
