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

namespace Ecell.Job
{
    /// <summary>
    /// Class manage the jobs by the analysis unit.
    /// </summary>
    public class JobGroup
    {
        #region Fields
        private string m_analysisName;
        private string m_date;
        private List<Job> m_jobs;
        private AnalysisStatus m_status;
        private string m_topDir;
        private bool m_isSaved = false;
        private IAnalysisModule m_analysis;
        private bool m_isRunning = false;
        private bool m_isGroupError = false;
        private IJobManager m_manager;
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
        /// 
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

        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="analysisName"></param>
        public JobGroup(JobManager manger, string analysisName)
        {
            m_manager = manger;
            this.m_analysisName = analysisName;           
            DateTime dt = DateTime.Now;
            string dateString = dt.ToString("yyyyMMddHHmm");
            m_date = dateString;
            this.m_jobs = new List<Job>();
        }

        /// <summary>
        /// Constructors with the initial parameters.
        /// </summary>
        /// <param name="analysisName"></param>
        /// <param name="date"></param>
        /// <param name="param"></param>
        public JobGroup(JobManager manager, string analysisName, string date)
        {
            m_manager = manager;
            this.m_analysisName = analysisName;
            this.m_date = date;
            this.m_jobs = new List<Job>();
        }
        #endregion


        public void Run()
        {
            m_isRunning = true;
        }

        public void Stop()
        {
            m_isRunning = false;
            UpdateStatus();
        }

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
        /// 
        /// </summary>
        public void ClearJob(int jobid)
        {
            foreach (Job m in m_jobs)
            {
                if (jobid == m.JobID)
                    m.Clear();
            }
        }

        public void Clear()
        {
            if (m_isSaved) return;
            foreach (Job m in m_jobs)
                m.Clear();
            if (!string.IsNullOrEmpty(m_topDir) && Directory.Exists(m_topDir))
                Directory.Delete(m_topDir, true);
                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        public Job GetJob(int jobid)
        {
            foreach (Job m in m_jobs)
            {
                if (m.JobID == jobid)
                    return m;
            }
            return null;
        }

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

        public void Delete()
        {
            m_jobs.Clear();
        }

        /// <summary>
        /// 
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
        /// 
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
    }
}
