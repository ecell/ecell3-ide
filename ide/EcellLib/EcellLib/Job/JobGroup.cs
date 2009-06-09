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

using Ecell.Objects;

namespace Ecell.Job
{
    /// <summary>
    /// Class manage the jobs by the analysis unit.
    /// </summary>
    public class JobGroup
    {
        #region Fields
        /// <summary>
        /// Analysis name.
        /// </summary>
        private string m_analysisName;
        /// <summary>
        /// Date executed this job group
        /// </summary>
        private string m_date;
        /// <summary>
        /// The list of jobs that this job group is managed.
        /// </summary>
        private List<Job> m_jobs;
        /// <summary>
        /// The status of this job group.
        /// </summary>
        private AnalysisStatus m_status;
        /// <summary>
        /// The top directory of this job group.
        /// </summary>
        private string m_topDir;
        /// <summary>
        /// The flag whether this job group is saved.
        /// </summary>
        private bool m_isSaved = false;
        /// <summary>
        /// Analysis module related with this job group.
        /// </summary>
        private IAnalysisModule m_analysis;
        /// <summary>
        /// The flag whether this group is running.
        /// </summary>
        private bool m_isRunning = false;
        /// <summary>
        ///  The flag whether this group is error.
        /// </summary>
        private bool m_isGroupError = false;
        /// <summary>
        /// JobManager.
        /// </summary>
        private IJobManager m_manager;
        /// <summary>
        /// the list of system object.
        /// </summary>
        private List<EcellObject> m_sysObj;
        /// <summary>
        /// the list of stepper object.
        /// </summary>
        private List<EcellObject> m_stepperObj;
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the analysis name.
        /// if you create IAnalysis, this class should have IAnalysis.
        /// </summary>
        public string AnalysisName
        {
            get { return this.m_analysisName; }
            set { this.m_analysisName = value; }
        }

        /// <summary>
        /// get / set the group name(analysisname_date)
        /// </summary>
        public string GroupName
        {
            get { return this.m_analysisName + "_" + m_date; }
        }

        /// <summary>
        /// get / set the date string.
        /// </summary>
        public string DateString
        {
            get { return this.m_date; }
            set { this.m_date = value; }
        }

        /// <summary>
        /// get / set the topdir.
        /// </summary>
        public string TopDir
        {
            get { return this.m_topDir; }
            set { this.m_topDir = value; }
        }

        /// <summary>
        /// get / set the flag whether this group is saved.
        /// </summary>
        public bool IsSaved
        {
            get { return this.m_isSaved; }
            set { this.m_isSaved = value; }
        }

        /// <summary>
        /// get / set the analysis parameters.
        /// </summary>
        public Dictionary<string, string> AnalysisParameter
        {
            get { return m_analysis.GetAnalysisProperty(); }
            set { this.m_analysis.SetAnalysisProperty(value); }
        }

        /// <summary>
        /// get / set the job list.
        /// </summary>
        public List<Job> Jobs
        {
            get { return this.m_jobs; }
            set { this.m_jobs = value; }
        }

        /// <summary>
        /// get / set the status.
        /// </summary>
        public AnalysisStatus Status
        {
            get { return this.m_status; }
            set { this.m_status = value; }
        }

        /// <summary>
        /// get the flag whether this job group is running.
        /// </summary>
        public bool IsRunning
        {
            get { return this.m_isRunning; }
        }

        /// <summary>
        /// set the error when this job group is failed to analysis.
        /// </summary>
        public bool IsGroupError
        {
            set { 
                this.m_isGroupError = value;
                UpdateStatus();
                m_manager.Update();
            }
        }

        /// <summary>
        /// get / set the analysis module for this job group.
        /// </summary>
        public IAnalysisModule AnalysisModule
        {
            get { return m_analysis; }
            set { this.m_analysis = value; }
        }

        /// <summary>
        /// get / set the system list.
        /// </summary>
        public List<EcellObject> SystemObjectList
        {
            get { return m_sysObj; }
            set { this.m_sysObj = value; }
        }

        /// <summary>
        /// get / set the stepper list.
        /// </summary>
        public List<EcellObject> StepperObjectList
        {
            get { return m_stepperObj; }
            set { this.m_stepperObj = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="manager">JobManager.</param>
        /// <param name="analysisName">the analysis name.</param>
        /// <param name="sysObj">the list of system object.</param>
        /// <param name="stepperList">the list of stepper object.</param>
        public JobGroup(IJobManager manager, string analysisName, List<EcellObject> sysObj, List<EcellObject> stepperList)
        {
            m_manager = manager;
            this.m_analysisName = analysisName;           
            DateTime dt = DateTime.Now;
            string dateString = dt.ToString("yyyyMMddHHmm");
            m_date = dateString;
            this.m_jobs = new List<Job>();
            this.m_sysObj = sysObj;
            this.m_stepperObj = stepperList;
        }

        /// <summary>
        /// Constructors with the initial parameters.
        /// </summary>
        /// <param name="manager">JobManager.</param>
        /// <param name="analysisName">the analysis name.</param>
        /// <param name="date">the date</param>
        /// <param name="sysObj">the list of system object.</param>
        /// <param name="stepperList">the list of stepper object.</param>
        public JobGroup(IJobManager manager, string analysisName, string date, List<EcellObject> sysObj, List<EcellObject> stepperList)
        {
            m_manager = manager;
            this.m_analysisName = analysisName;
            this.m_date = date;
            this.m_jobs = new List<Job>();
            this.m_sysObj = sysObj;
            this.m_stepperObj = stepperList;
        }
        #endregion

        /// <summary>
        /// Start this job group.
        /// </summary>
        public void Run()
        {
            m_isRunning = true;
        }

        /// <summary>
        /// Stop this job group.
        /// </summary>
        public void Stop()
        {
            m_isRunning = false;
            UpdateStatus();
        }

        /// <summary>
        /// Update status of job in this job group.
        /// </summary>
        private void UpdateJobStatus()
        {
            foreach (Job job in m_jobs)
            {
                if (job.Status == JobStatus.QUEUED ||
                    job.Status == JobStatus.RUNNING)
                {
                    job.Update();
                }
            }
        }

        /// <summary>
        /// Update status.
        /// </summary>
        public void UpdateStatus()
        {
            foreach (Job m in m_jobs)
            {
                m.Update();
            }

            AnalysisStatus status = AnalysisStatus.Finished;
            int count = 0;
            foreach (Job m in m_jobs)
            {

                if (m.Status != JobStatus.RUNNING &&
                    m.Status != JobStatus.QUEUED)
                    count++;

                if (m.Status == JobStatus.RUNNING ||
                    (m_isRunning && m.Status == JobStatus.QUEUED))
                {
                    status = AnalysisStatus.Running;
                    break;
                }
                if (status != AnalysisStatus.Running &&
                    (!m_isRunning && m.Status == JobStatus.QUEUED))
                {
                    status = AnalysisStatus.Waiting;
                    break;
                }
                if (m.Status == JobStatus.STOPPED)
                {
                    status = AnalysisStatus.Stopped;
                }
                else if (m.Status == JobStatus.ERROR &&
                    status == AnalysisStatus.Finished)
                {
                    status = AnalysisStatus.Error;
                }
            }
            if (m_isGroupError)
                status = AnalysisStatus.Error;

            m_status = status;
            if (count != m_jobs.Count)
            {
//                m_status = AnalysisStatus.Running;
            }
            else if (m_isRunning == true)
            {
                m_isRunning = false;
                m_analysis.NotifyAnalysisFinished();
            }
        }

        /// <summary>
        /// Clear the directory of selected job.
        /// </summary>
        public void ClearJob(int jobid)
        {
            foreach (Job m in m_jobs)
            {
                if (jobid == m.JobID)
                    m.Clear();
            }
        }

        /// <summary>
        /// Clear the job directory in this job group.
        /// </summary>
        public void Clear()
        {
            if (m_isSaved) return;
            foreach (Job m in m_jobs)
                m.Clear();
            if (!string.IsNullOrEmpty(m_topDir) && Directory.Exists(m_topDir))
                Directory.Delete(m_topDir, true);                
        }

        /// <summary>
        /// Get the job from this job group.
        /// </summary>
        /// <param name="jobid">the job id of selected job.</param>
        /// <returns>job object.</returns>
        public Job GetJob(int jobid)
        {
            foreach (Job m in m_jobs)
            {
                if (m.JobID == jobid)
                    return m;
            }
            return null;
        }

        /// <summary>
        /// Delete the selected job in this job group.
        /// </summary>
        /// <param name="jobid"></param>
        public void DeleteJob(int jobid)
        {
            foreach (Job m in m_jobs)
            {
                if (m.JobID == jobid)
                {
                    m_jobs.Remove(m);
                    return;
                }
            }
        }

        /// <summary>
        /// Delete the all job in this job group.
        /// </summary>
        public void Delete()
        {
            m_jobs.Clear();
        }

        /// <summary>
        /// Check whether this job group is finished.
        /// </summary>
        /// <returns></returns>
        public bool IsFinished()
        {
            foreach (Job m in m_jobs)
            {
                if (m.Status == JobStatus.QUEUED ||
                    m.Status == JobStatus.RUNNING)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check whether this job group is error.
        /// </summary>
        /// <returns></returns>
        public bool IsError()
        {
            foreach (Job m in m_jobs)
            {
                if (m.Status == JobStatus.ERROR)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Save the job group in the selected directory.
        /// </summary>
        /// <param name="topdir">the top directory to save the job group.</param>
        public void SaveJobGroup(string topdir)
        {
            if (!Directory.Exists(topdir))
                Directory.CreateDirectory(topdir);

            string modelDir = topdir + "/" + Constants.ModelDirName;
            string logDir = topdir + "/" + Constants.LogDirName;

            if (!Directory.Exists(modelDir))
                Directory.CreateDirectory(modelDir);
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            string modelFile = modelDir + "/" + m_date + ".eml";

            List<EcellObject> writeList = new List<EcellObject>();
            writeList.AddRange(StepperObjectList);
            writeList.AddRange(SystemObjectList);
            EmlWriter.Create(modelFile, writeList, false);

            AnalysisModule.SaveAnalysisInfo(modelDir);
            SaveJobEntry(logDir);
            m_topDir = topdir;
            IsSaved = true;
        }

        private void SaveJobEntry(string topdir)
        {
            foreach (Job j in m_jobs)
            {
                string logdir = topdir + "/" + j.JobID;
                if (!Directory.Exists(logdir))
                    Directory.CreateDirectory(logdir);

                // save parameter file.
                string paramFile = logdir + "/" + GroupName + "_" + j.JobID + ".param";
                JobParameterFile f = new JobParameterFile(j, paramFile);
                f.Write();

                // save script file.
                string scriptFile = logdir + "/" + GroupName + "_" + j.JobID + ".ess";
                File.Copy(j.Argument, scriptFile);

                // save log file.
                foreach (string srcname in j.ExtraFileList)
                {
                    string filename = Path.GetFileName(srcname);
                    string dstname = logdir + "/" + filename;
                    if (!File.Exists(srcname))
                        continue;
                    File.Copy(srcname, dstname);
                }
            }                       
        }

        public void LoadJobEntry(string topdir)
        {
            string[] dirs = Directory.GetDirectories(topdir);
            for (int i = 0; i < dirs.Length; i++)
            {
                DirectoryInfo d = new DirectoryInfo(dirs[i]);
                int jobid = Int32.Parse(d.Name);
                                
                string paramFile = dirs[i] + "/" + GroupName + "_" + jobid + ".param";
                string scriptFile = dirs[i] + "/" + GroupName + "_" + jobid + ".ess";
                string[] files = Directory.GetFiles(dirs[i], "*.csv");
                List<string> extFileList = new List<string>();
                for (int j = 0; j < files.Length; j++)
                {
                    extFileList.Add(files[j]);
                }
                Job job = m_manager.CreateJobEntry(jobid, GroupName); ;
                JobParameterFile f = new JobParameterFile(job, paramFile);
                f.Read();

                job.ExtraFileList = extFileList;
                job.Argument = scriptFile;
                job.JobDirectory = dirs[i];
            }
        }
    }
}
