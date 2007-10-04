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

namespace SessionManager
{
    /// <summary>
    /// enumerable set of status for job.
    /// </summary>
    public enum JobStatus
    {
        NONE,
        QUEUED,
        RUNNING,
        ERROR,
        STOPPED,
        FINISHED
    }

    /// <summary>
    /// SessionProxy class. This class is abstract class managed job.
    /// </summary>
    public class SessionProxy
    {
        private int m_jobId;
        private int m_processId;
        private string m_scriptFile;
        private string m_argument;
        private string m_jobDirectory;
        private List<String> m_extraFile;

        private JobStatus m_status;
        static private String s_dmPath;
        static private int s_maxCount;
        static private int s_jobID = 0;

        /// <summary>
        /// get / set status of job.
        /// </summary>
        public JobStatus Status
        {
            get { return this.m_status; }
            set { this.m_status = value; }
        }

        /// <summary>
        /// get / set ID of job. Job ID is inner ID.
        /// </summary>
        public int JobID
        {
            get { return this.m_jobId; }
            set { this.m_jobId = value; }
        }

        /// <summary>
        /// get / set ID of process. Process ID is ID under OS.
        /// </summary>
        public int ProcessID
        {
            get { return this.m_processId; }
            set { this.m_processId = value; }
        }

        /// <summary>
        /// get / set script file name.
        /// </summary>
        public String ScriptFile
        {
            get { return this.m_scriptFile; }
            set { this.m_scriptFile = value; }
        }

        /// <summary>
        /// get / set the argument of program.
        /// </summary>
        public String Argument
        {
            get { return this.m_argument; }
            set { this.m_argument = value; }
        }

        /// <summary>
        /// get / set the directory of jobs.
        /// </summary>
        public String JobDirectory
        {
            get { return this.m_jobDirectory; }
            set { this.m_jobDirectory = value; }
        }

        /// <summary>
        /// get / set the list of extra file.
        /// </summary>
        public List<String> ExtraFileList
        {
            get { return this.m_extraFile; }
            set { this.m_extraFile = value; }
        }

        /// <summary>
        /// get / set the dm directory path.
        /// </summary>
        static public String DMPATH
        {
            get { return s_dmPath; }
            set { s_dmPath = value; }
        }

        /// <summary>
        /// get / set the max count to retry the simulation.
        /// </summary>
        static public int MaxCount
        {
            get { return s_maxCount; }
            set { s_maxCount = value; }
        }

        /// <summary>
        /// get the job ID of session.
        /// </summary>
        /// <returns>job id.</returns>
        static public int NextJobID()
        {
            return ++s_jobID;
        }

        /// <summary>
        /// Constructor wirh no parameters.
        /// </summary>
        public SessionProxy()
        {
            this.m_argument = "";
            this.m_extraFile = new List<string>();
            this.m_jobDirectory = "";
            this.m_jobId = SessionProxy.NextJobID();
            this.m_processId = -1;
            this.m_scriptFile = "";
            this.m_status = JobStatus.NONE;
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="exeFile">script file name.</param>
        /// <param name="arg">argument of script file.</param>
        /// <param name="extFile">extra file of script file.</param>
        /// <param name="tmpDir">tmp directory of script file</param>
        public SessionProxy(string exeFile, string arg, List<string> extFile,
            string tmpDir)
        {
            this.m_scriptFile = exeFile;
            this.m_argument = arg;
            this.m_extraFile = extFile;
            this.m_jobId = SessionProxy.NextJobID();
            this.m_jobDirectory = tmpDir + "/" + m_jobId;
            this.JobID = -1;
            this.ProcessID = -1;
            this.m_status = JobStatus.NONE;
        }

        /// <summary>
        /// Retry this session.
        /// </summary>
        public void retry()
        {
        }

        /// <summary>
        /// Start this session.
        /// </summary>
        public void run()
        {
        }

        /// <summary>
        /// Stop this job.
        /// </summary>
        public void stop()
        {
            // not implement
        }

        /// <summary>
        /// Update the status of job.
        /// </summary>
        public void Update()
        {
            // not implement
        }

        /// <summary>
        /// Get the judgement of script if script is output.
        /// </summary>
        /// <returns>judgement.</returns>
        public int GetJudge()
        {
            // not implement.
            return 0;
        }

        /// <summary>
        /// Get log data output the script.
        /// </summary>
        /// <returns>log data.</returns>
        public string GetLogData()
        {
            // not implement.
            return "";
        }
    }
}
