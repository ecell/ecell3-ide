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
using System.IO;

namespace Ecell.Job
{
    /// <summary>
    /// enumerable set of status for job.
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// Initial status.
        /// </summary>
        NONE,
        /// <summary>
        /// Regist the job, but not run the job.
        /// </summary>
        QUEUED,
        /// <summary>
        /// Run the job.
        /// </summary>
        RUNNING,
        /// <summary>
        /// Fail to execute the job.
        /// </summary>
        ERROR,
        /// <summary>
        /// User stop the job.
        /// </summary>
        STOPPED,
        /// <summary>
        /// Finish the job.
        /// </summary>
        FINISHED
    }

    /// <summary>
    /// SessionProxy class. This class is abstract class managed job.
    /// </summary>
    public class Job
    {
        private int m_jobId;
        private int m_processId;
        private string m_scriptFile;
        private string m_argument;
        private string m_jobDirectory;
        private string m_machine;
        private List<string> m_extraFile;
        private JobStatus m_status;
        private string m_stderr;

        static private string s_dmPath;
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
        /// get / set the machine name executed the job.
        /// </summary>
        public String Machine
        {
            get { return this.m_machine; }
            set { this.m_machine = value; }
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
        /// get / set the string of stderr.
        /// </summary>
        public String StdErr
        {
            get { return this.m_stderr; }
            set { this.m_stderr = value; }
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
        /// reset the number of job ID.
        /// </summary>
        static public void ClearJobID()
        {
            s_jobID = 0;
        }

        /// <summary>
        /// Constructor wirh no parameters.
        /// </summary>
        public Job()
        {
            this.m_argument = "";
            this.m_extraFile = new List<string>();
            this.m_jobDirectory = "";
            this.m_jobId = Job.NextJobID();
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
        public Job(string exeFile, string arg, List<string> extFile,
            string tmpDir)
        {
            this.m_scriptFile = exeFile;
            this.m_argument = arg;
            this.m_extraFile = extFile;
            this.m_jobId = Job.NextJobID();
            this.m_jobDirectory = tmpDir + "/" + m_jobId;
            this.JobID = -1;
            this.ProcessID = -1;
            this.m_status = JobStatus.NONE;
        }

        /// <summary>
        /// Retry this session.
        /// </summary>
        public virtual void retry()
        {
        }

        /// <summary>
        /// Start this session.
        /// </summary>
        public virtual void run()
        {
        }

        /// <summary>
        /// Stop this job.
        /// </summary>
        public virtual void stop()
        {
            // not implement
        }

        /// <summary>
        /// Update the status of job.
        /// </summary>
        public virtual void Update()
        {
            // not implement
        }

        /// <summary>
        /// Get the judgement of script if script is output.
        /// </summary>
        /// <param name="judgeFile">script file to judge.</param>
        /// <returns>judgement.</returns>
        public virtual int GetJudge(String judgeFile)
        {
            // not implement.
            return 0;
        }

        /// <summary>
        /// Get log data output the script.
        /// </summary>
        /// <param name="key">entry name.</param>
        /// <returns>log data list.</returns>
        public virtual Dictionary<double, double> GetLogData(string key)
        {
            return new Dictionary<double, double>();
        }

        /// <summary>
        /// Get the stream of StdOut for this process.
        /// </summary>
        /// <returns>string.</returns>
        public virtual string GetStdOut()
        {
            return null;
        }

        /// <summary>
        /// Get the stream of StdErr for this process.
        /// </summary>
        /// <returns>string.</returns>
        public virtual string GetStdErr()
        {
            return null;
        }

        /// <summary>
        /// Prepare the file before the process run.
        /// </summary>
        public virtual void PrepareProcess()
        {
            this.Status = JobStatus.QUEUED;
        }

        /// <summary>
        /// Clear the file used in this job.
        /// </summary>
        public virtual void Clear()
        {
            foreach (string name in m_extraFile)
                if (File.Exists(name))
                    File.Delete(name);
            if (Directory.Exists(m_jobDirectory))
                Directory.Delete(m_jobDirectory, true);
        }
    }
}
