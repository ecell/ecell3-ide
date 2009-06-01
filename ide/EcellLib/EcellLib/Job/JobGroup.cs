﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
        private Dictionary<string, string> m_analysisParameter;
        private List<Job> m_jobs;
        private AnalysisStatus m_status;
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
        /// get / set the analysis parameters.
        /// </summary>
        public Dictionary<string, string> AnalysisParameter
        {
            get { return this.m_analysisParameter; }
            set { this.m_analysisParameter = value; }
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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="analysisName"></param>
        public JobGroup(string analysisName)
        {
            this.m_analysisName = analysisName;
            DateTime dt = DateTime.Now;
            string dateString = dt.ToString("yyyyMMddHHmm");
            m_date = dateString;
            this.m_analysisParameter = new Dictionary<string, string>();
            this.m_jobs = new List<Job>();
        }

        /// <summary>
        /// Constructors with the initial parameters.
        /// </summary>
        /// <param name="analysisName"></param>
        /// <param name="date"></param>
        /// <param name="param"></param>
        public JobGroup(string analysisName, string date, Dictionary<string, string> param)
        {
            this.m_analysisName = analysisName;
            this.m_date = date;
            this.m_analysisParameter = new Dictionary<string, string>();
            foreach (string name in param.Keys)
            {
                this.m_analysisParameter.Add(name, param[name]);
            }
            this.m_jobs = new List<Job>();
        }
        #endregion

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
            AnalysisStatus status = AnalysisStatus.Finished;
            foreach (Job m in m_jobs)
            {             
                if (m.Status == JobStatus.RUNNING)
                {
                    status = AnalysisStatus.Running;
                    break;
                }
                else if (m.Status == JobStatus.STOPPED)
                {
                    status = AnalysisStatus.Stopped;
                }
                else if (m.Status == JobStatus.ERROR ||
                    (status != AnalysisStatus.Stopped &&
                    status != AnalysisStatus.Running))
                {
                    status = AnalysisStatus.Error;
                }
                else if ((m.Status == JobStatus.NONE ||
                    m.Status == JobStatus.QUEUED) &&
                    (status == AnalysisStatus.Finished))
                {
                    status = AnalysisStatus.Waiting;
                }
            }
            m_status = status;
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
            foreach (Job m in m_jobs)
                m.Clear();
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
            m_analysisParameter.Clear();
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