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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Ecell;
using Ecell.Objects;
using Ecell.Reporting;

namespace Ecell.Job
{
    /// <summary>
    /// Management class of session.
    /// </summary>
    public class JobManager: IJobManager
    {
        #region Fields
        public event JobUpdateEventHandler JobUpdateEvent;
        private ApplicationEnvironment m_env;
        private bool m_tmpDirRemovable = false;
        private string m_tmpRootDir = null;
        private string m_tmpDir = null;
        private int m_conc = -1;
        private int m_limitRetry = 5;
        private int m_updateInterval = 5000;
        private int m_globalTimeOut = 0;
        private JobProxy m_proxy;
        private Dictionary<string, JobProxy> m_proxyList = new Dictionary<string, JobProxy>();
        private Dictionary<string, JobGroup> m_groupDic = new Dictionary<string, JobGroup>();
        private Dictionary<string, IAnalysisModule> m_analysisDic = new Dictionary<string, IAnalysisModule>();
        private Timer m_timer;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public JobManager(ApplicationEnvironment env)
        {
            m_env = env;
            m_timer = new Timer();
            m_timer.Enabled = false;
            m_timer.Interval = m_updateInterval;
            m_timer.Tick += new EventHandler(UpdateTimeFire);

            LocalJobProxy p = new LocalJobProxy();
            p.Manager = this;
            m_proxyList.Add(p.Name, p);

            string cog_home = System.Environment.GetEnvironmentVariable("COG_HOME");
            if (!string.IsNullOrEmpty(cog_home))
            {
                GlobusJobProxy g = new GlobusJobProxy();
                g.Manager = this;
                m_proxyList.Add(g.Name, g);
            }

            SetCurrentEnvironment(p.Name);
            TmpRootDir = Util.GetTmpDir();
        }

        /// <summary>
        /// get / set the concurrency of this system.
        /// </summary>
        public int Concurrency
        {
            get {
                if (m_conc <= 0) m_conc = GetDefaultConcurrency();
                return m_conc;
            }
            set { this.m_conc = value; }
        }

        /// <summary>
        /// get / set the temporary root directory to work the jobs.
        /// </summary>
        public string TmpRootDir
        {
            get { return this.m_tmpRootDir; }
            set {
                this.m_tmpRootDir = value;
                Process p = Process.GetCurrentProcess();
                this.m_tmpDir = m_tmpRootDir + "/" + p.Id;
            }
        }

        /// <summary>
        /// get the temporary directory to work the jobs.
        /// </summary>
        public string TmpDir
        {
            get { return this.m_tmpDir; }
        }

        /// <summary>
        /// get / set the flag whether the temporary directory is removable.
        /// </summary>
        public bool IsTmpDirRemovable
        {
            get { return this.m_tmpDirRemovable; }
            set { this.m_tmpDirRemovable = value; }
        }

        /// <summary>
        /// set the the limit number of retry.
        /// </summary>
        public int LimitRetry
        {
            set { this.m_limitRetry = value; }
        }

        /// <summary>
        /// get / set the interval to update the status of job.
        /// </summary>
        public int UpdateInterval
        {
            get { return this.m_updateInterval; }
            set { 
                this.m_updateInterval = value;
                m_timer.Interval = value;
            }
        }

        /// <summary>
        /// get / set the time of time out to queue the job.
        /// </summary>
        public int GlobalTimeOut
        {
            get { return this.m_globalTimeOut; }
            set { this.m_globalTimeOut = value; }
        }

        /// <summary>
        /// get / set the using proxy for system.
        /// </summary>
        public JobProxy Proxy
        {
            get { return this.m_proxy; }
            set { this.m_proxy = value; }
        }

        /// <summary>
        /// get the dictionaty the group name and job group object.
        /// </summary>
        public Dictionary<string, JobGroup> GroupDic
        {
            get { return this.m_groupDic; }
        }

        /// <summary>
        /// get the dictionary the analysis name and analysis object.
        /// </summary>
        public Dictionary<string, IAnalysisModule> AnalysisDic
        {
            get { return this.m_analysisDic; }
        }

        /// <summary>
        /// Set the environment with input name.
        /// </summary>
        /// <param name="env">the environment name.</param>
        public void SetCurrentEnvironment(string env)
        {
            if (m_proxyList.ContainsKey(env))
            {
                m_proxy = m_proxyList[env];
            }
        }

        /// <summary>
        /// Get the list of environment name.
        /// </summary>
        /// <returns>the list of string.</returns>
        public List<string> GetEnvironmentList()
        {
            List<string> tmpList = new List<string>();
            foreach (string id in m_proxyList.Keys)
            {
                tmpList.Add(id);
            }
            return tmpList;
        }

        /// <summary>
        /// Get the environment name.
        /// </summary>
        /// <returns>string.</returns>
        public string GetCurrentEnvironment()
        {
            if (m_proxy == null)
                return null;
            return m_proxy.Name;
        }

        /// <summary>
        /// Get the list of property for environment.
        /// </summary>
        /// <returns>the list of string.</returns>
        public Dictionary<string, object> GetEnvironmentProperty()
        {
            if (m_proxy == null)
                return null;
            return m_proxy.GetProperty();
        }

        /// <summary>
        /// Get the default property of environment.
        /// </summary>
        /// <param name="env">Environment name.</param>
        /// <returns>Dictionary of default property.</returns>
        public Dictionary<string, object> GetDefaultEnvironmentProperty(string env)
        {
            if (m_proxyList.ContainsKey(env))
            {
                return m_proxyList[env].GetDefaultProperty();
            }
            return new Dictionary<string,object>();
        }

        /// <summary>
        /// Update the property of proxy.
        /// </summary>
        /// <param name="list">the list of new property.</param>
        public void SetEnvironmentProperty(Dictionary<string, object> list)
        {
            if (m_proxy == null) return;
            m_proxy.SetProperty(list);
        }

        /// <summary>
        /// get the defalut concurrency of environment.
        /// </summary>
        /// <returns></returns>
        public int GetDefaultConcurrency()
        {
            if (m_proxy == null) return 1;
            return m_proxy.Concurrency;
        }

        /// <summary>
        /// get the defalut concurrency of environment.
        /// </summary>
        /// <returns></returns>
        public int GetDefaultConcurrency(string env)
        {
            if (m_proxyList.ContainsKey(env))
            {
                return m_proxyList[env].Concurrency;
            }
            return 1;
        }

        /// <summary>
        /// Regist the jobs.
        /// </summary>
        /// <param name="script">Script file name.</param>
        /// <param name="arg">Argument of script file.</param>
        /// <param name="extFile">Extra file list of script file.</param>
        /// <returns>the status of job.</returns>
        public int RegisterJob(Job job, string script, string arg, List<string> extFile)
        {
            if (m_proxy == null)
                return -1;
            if (job == null)
                job = m_proxy.CreateJob();

            job.ScriptFile = script;
            job.Argument = arg;
            job.Manager = this;
            job.ExtraFileList = extFile;
            // search dmpath
            job.JobDirectory = TmpDir + "/" + job.JobID;
            m_groupDic[job.GroupName].Jobs.Add(job);
            OnJobUpdate(JobUpdateType.AddJob);

            return job.JobID;
        }

        /// <summary>
        /// Create the job entry when the analysis result is loaded.
        /// </summary>
        /// <param name="groupName">the group name,</param>
        /// <param name="param">the analysis parameter.</param>
        /// <returns>return jobid.</returns>
        public int CreateJobEntry(string groupName, ExecuteParameter param)
        {
            if (m_proxy == null)
                return -1;

            Job job = m_proxy.CreateJob();
            job.GroupName = groupName;
            job.Manager = this;
            job.Status = JobStatus.FINISHED;
            job.ExecParam = param;
            m_groupDic[groupName].Jobs.Add(job);
            OnJobUpdate(JobUpdateType.AddJob);

            return job.JobID;
        }

        /// <summary>
        /// Create the job entry for the saved job.
        /// </summary>
        /// <param name="jobid">job id.</param>
        /// <param name="groupName">the group name.</param>
        /// <returns>return job.</returns>
        public Job CreateJobEntry(int jobid, string groupName)
        {
            LocalJobProxy p = new LocalJobProxy();
            Job job = p.CreateJob(jobid);
            job.Manager = this;
            job.GroupName = groupName;
            job.Status = JobStatus.FINISHED;
            job.ExecParam = new ExecuteParameter();
            m_groupDic[groupName].Jobs.Add(job);

            return job;
        }

        /// <summary>
        /// Regist the session of e-cell.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        /// <param name="arg">the argument of script.</param>
        /// <param name="extFile">the list of extension file.</param>
        /// <param name="script">the script file.</param>
        /// <returns>the status of job.</returns>
        public int RegisterEcellSession(string groupName, string script, string arg, List<string> extFile)
        {
            if (m_proxy == null)
                return -1;

            Job job = m_proxy.CreateJob();
            job.ScriptFile = script;
            job.Argument = arg;
            job.Manager = this;
            job.ExtraFileList = extFile;
            // search dmpath
            job.JobDirectory = TmpDir + "/" + job.JobID;
            m_groupDic[groupName].Jobs.Add(job);
            OnJobUpdate(JobUpdateType.AddJob);

            return job.JobID;
        }

        /// <summary>
        /// Clear the files of the selected jobs. if jobID = 0, all jobs are deleted.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        /// <param name="jobID">the ID of deleted job.</param>
        public void ClearJob(string groupName, int jobID)
        {
            if (string.IsNullOrEmpty(groupName) || !m_groupDic.ContainsKey(groupName))
                return;

            if (jobID == 0)
            {
                m_groupDic[groupName].Clear();
            }
            else
            {
                m_groupDic[groupName].ClearJob(jobID);
            }
            OnJobUpdate(JobUpdateType.DeleteJob);
        }

        /// <summary>
        /// Delete the selected jobs. if jobID = 0, job group is deleted.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        /// <param name="jobID">the job id of deleted job.</param>
        public void DeleteJob(string groupName, int jobID)
        {
            if (string.IsNullOrEmpty(groupName) || !m_groupDic.ContainsKey(groupName))
                return;

            if (jobID == 0)
            {
                m_groupDic[groupName].Delete();
            }
            else
            {
                m_groupDic[groupName].DeleteJob(jobID);
            }
            OnJobUpdate(JobUpdateType.DeleteJob);
        }

        /// <summary>
        /// Clear the job and the job group because the project is closed.
        /// </summary>
        public void Clear()
        {
            foreach (string name in m_groupDic.Keys)
                m_groupDic[name].Clear();
            m_groupDic.Clear();
            OnJobUpdate(JobUpdateType.DeleteJobGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnJobUpdate(JobUpdateType type)
        {
            if (JobUpdateEvent != null)
                JobUpdateEvent(this, new JobUpdateEventArgs(type));
        }

        /// <summary>
        /// Update the information of session.
        /// </summary>
        public void Update()
        {
            List<JobGroup> updateList = new List<JobGroup>();
            foreach (JobGroup m in m_groupDic.Values)
            {
                updateList.Add(m);
            }
            foreach (JobGroup m in updateList)
            {
                if (m.IsRunning)
                {
                    m.UpdateStatus();
                }
            }
            if (m_proxy != null)
                m_proxy.Update();

            OnJobUpdate(JobUpdateType.Update);
        }

        /// <summary>
        /// Get the list of queued jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        public List<Job> GetQueuedJobList()
        {
            return GetJobListWithStatus(JobStatus.QUEUED);
        }

        /// <summary>
        /// Get the list of running jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        public List<Job> GetRunningJobList()
        {
            return GetJobListWithStatus(JobStatus.RUNNING);
        }

        /// <summary>
        /// Get the list of finished jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        public List<Job> GetFinishedJobList()
        {
            return GetJobListWithStatus(JobStatus.FINISHED);
        }

        /// <summary>
        /// Get the list of jobs with JobStatus.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private List<Job> GetJobListWithStatus(JobStatus status)
        {
            List<Job> tmpList = new List<Job>();
            foreach (string name in m_groupDic.Keys)
            {
                foreach (Job j in m_groupDic[name].Jobs)
                {
                    if (j.Status == status)
                        tmpList.Add(j);
                }
            }
            return tmpList;
        }

        /// <summary>
        /// Check whether all jobs is finished.
        /// </summary>
        /// <param name="groupName">the group name</param>
        /// <returns>if all jobs is finished, retur true.</returns>
        public bool IsFinished(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                foreach (string name in m_groupDic.Keys)
                {
                    if (!m_groupDic[name].IsRunning) continue;
                    bool res = m_groupDic[name].IsFinished();
                    if (res == false)
                        return false;
                }
                return true;
            }

            return m_groupDic[groupName].IsFinished();
        }

        /// <summary>
        /// Check whther there are any error jobs.
        /// </summary>
        /// <param name="groupName">the group name</param>
        /// <returns>if there is error job, return true.</returns>
        public bool IsError(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                foreach (string name in m_groupDic.Keys)
                {
                    bool res = m_groupDic[name].IsError();
                    if (res == true)
                        return true;
                }
                return false;
            }

            return m_groupDic[groupName].IsError();
        }

        /// <summary>
        /// Preapre to execute the process.
        /// Ex. script file, extra file, job directory and so on.
        /// <param name="groupName"></param>
        /// <param name="isForce"></param>
        /// </summary>
        private void PrepareProcessRun(string groupName, bool isForce)
        {
            foreach (string name in m_groupDic.Keys)
            {
                if (!string.IsNullOrEmpty(groupName) &&
                    !name.Equals(groupName))
                    continue;
                foreach (Job j in m_groupDic[name].Jobs)
                {
                    if (j.Status == JobStatus.NONE || isForce)
                        j.PrepareProcess();
                }
            }
        }

        /// <summary>
        /// Run the jobs.
        /// </summary>
        public void Run(string groupName, bool isForce)
        {
            PrepareProcessRun(groupName, isForce);
            m_groupDic[groupName].Run(true);
            if (m_timer.Enabled == false)
            {
                //Update();
                m_timer.Enabled = true;
                m_timer.Start();
            }
        }

        /// <summary>
        /// Run the jobs.
        /// </summary>
        public void Run(string groupName, int jobid)
        {
            Job j = m_groupDic[groupName].GetJob(jobid);
            if (j.Status != JobStatus.QUEUED)
            {
                j.Status = JobStatus.QUEUED;
            }
            j.PrepareProcess();
            m_groupDic[groupName].Run(false);

            if (m_timer.Enabled == false)
            {
                Update();
                m_timer.Enabled = true;
                m_timer.Start();
            }
        }

        /// <summary>
        /// Run the jobs and execute this process until all SessionProxy is finished.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        public void RunWaitFinish(string groupName)
        {
            PrepareProcessRun(groupName, true);

            while (!IsFinished(null))
            {
                Update();
                System.Threading.Thread.Sleep(m_updateInterval * 1000);
            }
        }

        /// <summary>
        /// Stop the job with input ID of job. if jobid = 0, all job are stopped.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        /// <param name="jobid">stop the ID of job.</param>
        public void Stop(string groupName, int jobid)
        {
            if (jobid == 0)
            {
                foreach (Job j in m_groupDic[groupName].Jobs)
                {
                    j.stop();
                }
                m_groupDic[groupName].Stop();
            }
            else
            {
                Job j = m_groupDic[groupName].GetJob(jobid);
                if (j != null)
                    j.stop();
            }
            OnJobUpdate(JobUpdateType.Update);
        }

        /// <summary>
        /// Get the all list of SessionProxy or SessionProxy with jobid.
        /// </summary>
        /// <param name="groupName">the group name</param>
        /// <param name="jobid">jobid.</param>
        /// <returns>the list of SessionProxy.</returns>
        public List<Job> GetSessionProxy(string groupName, int jobid)
        {
            List<Job> tmpList = new List<Job>();
            foreach (string name in m_groupDic.Keys)
            {
                if (string.IsNullOrEmpty(groupName) ||
                    !name.Equals(groupName))
                    continue;

                foreach (Job job in m_groupDic[groupName].Jobs)
                {
                    if (jobid == 0)
                        tmpList.Add(job);
                    else if (jobid == job.JobID)
                        tmpList.Add(job);
                }
            }
            return tmpList;
        }

        /// <summary>
        /// Get the job directory of session correspond to jobID.
        /// </summary>
        /// <param name="name">the group name</param>
        /// <param name="jobid">JobID.</param>
        /// <returns>Directory path.</returns>
        public string GetJobDirectory(string name, int jobid)
        {
            foreach (Job job in m_groupDic[name].Jobs)
            {
                if (job.JobID == jobid)
                    return job.JobDirectory;
            }
            return null;
        }

        /// <summary>
        /// Get the stream of StrOut.
        /// </summary>
        /// <param name="name">the group name</param>
        /// <param name="jobid">job id.</param>
        /// <returns>string</returns>
        public string GetStdout(string name, int jobid)
        {
            string stdout = null;
            if (!m_groupDic.ContainsKey(name))
                return stdout;
            foreach (Job job in m_groupDic[name].Jobs)
            {
                if (job.JobID == jobid)
                    stdout = job.GetStdOut();
            }
            return stdout;

        }

        /// <summary>
        /// Get the stream of StdErr.
        /// </summary>
        /// <param name="name">the group name</param>
        /// <param name="jobid">job id.</param>
        /// <returns>string</returns>
        public string GetStderr(string name, int jobid)
        {
            string stderr = null;
            if (!m_groupDic.ContainsKey(name))
                return stderr;
            foreach (Job job in m_groupDic[name].Jobs)
            {
                if (job.JobID == jobid)
                    stderr = job.GetStdErr();
            }
            return stderr;
        }

        /// <summary>
        /// Get the string of option.
        /// </summary>
        /// <returns>options for SystemProxy.</returns>
        public string GetOptionList()
        {
            if (m_proxy == null)
                return null;
            Dictionary<string, object> dic = GetEnvironmentProperty();
            string result = "";
            foreach (string opt in dic.Keys)
            {
                result = result + " " + opt;
                if (dic[opt] != null)
                {
                    result = result + " \"" + dic[opt].ToString() + "\"";
                }
            }
            return result;
        }

        /// <summary>
        /// Update the status of session at intervals while program is running.
        /// </summary>
        /// <param name="sender">Timer.</param>
        /// <param name="e">EventArgs.</param>
        void UpdateTimeFire(object sender, EventArgs e)
        {
            if (IsFinished(null))
            {
                m_timer.Enabled = false;
                m_timer.Stop();
            }
            // This function should be executed before the above for loop,
            // but execute to reexecute the analysis after the for loop.
            Update();
        }

        //======================================================
        // The following member and function is in here or
        // EcellLib.AnalysisManager.
        //======================================================
        private List<EcellParameterData> m_paramList = new List<EcellParameterData>();
        private List<SaveLoggerProperty> m_logList = new List<SaveLoggerProperty>();

        /// <summary>
        /// Set the logger data to judge the result, when execute RunSimParameterRange or RunSimParameterMatrix.
        /// </summary>
        /// <param name="sList">the list of logger data.</param>
        public void SetLoggerData(List<SaveLoggerProperty> sList)
        {
            m_logList.Clear();

            foreach (SaveLoggerProperty p in sList)
            {
                m_logList.Add(p);
            }
        }

        /// <summary>
        /// Set the range of initial parameter, when execute RunSimParameterMatrix and RunSimParameterRange.
        /// If you use RunSimParameterMatrix, the number of list must be 2.
        /// </summary>
        /// <param name="pList">the list of range for initial parameters.</param>
        public void SetParameterRange(List<EcellParameterData> pList)
        {
            m_paramList.Clear();
            foreach (EcellParameterData p in pList)
            {
                m_paramList.Add(p);
            }
        }

        /// <summary>
        /// Rerun jobs after the script is created.
        /// </summary>
        /// <param name="jobid">Job id</param>
        /// <param name="groupName">group name</param>
        /// <param name="topDir">the top directory</param>
        /// <param name="modelName">the model name</param>
        /// <param name="count">simulation time or simulation step.</param>
        /// <param                            name="isStep">the flag use simulation time or simulation step.</param>
        /// <param name="paramDic">the execution parameter.</param>
        public void ReRunSimParameterSet(int jobid, string groupName, string topDir, string modelName,
            double count, bool isStep, ExecuteParameter paramDic)
        {            
            Project prj = m_env.DataManager.CurrentProject;
            ScriptWriter writer = new ScriptWriter(prj);
            List<EcellObject> sysList = m_groupDic[groupName].SystemObjectList;
            List<EcellObject> stepperList = m_groupDic[groupName].StepperObjectList;

            string dirName = topDir + "/" + jobid;
            string fileName = topDir + "/" + jobid + ".ess";
            string modelFileName = topDir + "/" + jobid + ".eml";

            CreateLocalScript(topDir, dirName, fileName, writer,
                modelName, count, isStep, sysList, stepperList, paramDic.ParamDic);
        }

        /// <summary>
        /// Execute the simulation with using the set parameters.
        /// </summary>
        /// <param name="groupName">the group name</param>
        /// <param name="topDir">top directory include the script file and result data.</param>
        /// <param name="modelName">model name executed the simulation.</param>
        /// <param name="count">simulation time or simulation step.</param>
        /// <param name="isStep">the flag use simulation time or simulation step.</param>
        /// <param name="setparam">the set parameters.</param>
        /// <returns>Dictionary of jobid and the execution parameter.</returns>
        public Dictionary<int, ExecuteParameter> RunSimParameterSet(string groupName, string topDir, string modelName, 
            double count, bool isStep, Dictionary<int, ExecuteParameter> setparam)
        {
            Project prj = m_env.DataManager.CurrentProject;
            ScriptWriter writer = new ScriptWriter(prj);
            List<EcellObject> sysList = m_groupDic[groupName].SystemObjectList;
            List<EcellObject> stepperList = m_groupDic[groupName].StepperObjectList;
            Dictionary<int, ExecuteParameter> resList = new Dictionary<int, ExecuteParameter>();
            if (!Directory.Exists(topDir))
            {
                Directory.CreateDirectory(topDir);
            }
            foreach (int i in setparam.Keys)
            {
                Dictionary<string, double> paramDic = setparam[i].ParamDic;

                Job job = Proxy.CreateJob();
                job.GroupName = groupName;
                job.Manager = this;
                string dirName = topDir + "/" + job.JobID;
                string fileName = topDir + "/" + job.JobID + ".ess";
                string modelFileName = topDir + "/" + job.JobID + ".eml";

                List<string> extFileList = ExtractExtFileList(topDir + "/" + job.JobID, m_logList);
                if (m_groupDic[groupName].Status == AnalysisStatus.Stopped)
                    return new Dictionary<int, ExecuteParameter>();
                int jobid = RegisterJob(job, m_proxy.GetDefaultScript(), fileName, extFileList);

                if (this.Proxy.IsIDE() == true)
                {
                    CreateLocalScript(topDir, dirName, fileName, writer,
                        modelName, count, isStep, sysList, stepperList, paramDic);
                }
                else
                {
                    CreateUnixScript(jobid, topDir, dirName, fileName, modelFileName, writer,
                        modelName, count, isStep, sysList, stepperList, paramDic);
                }

                job.ExecParam = new ExecuteParameter(paramDic);
                resList.Add(jobid, new ExecuteParameter(paramDic));
                Application.DoEvents();
            }
            Run(groupName, false);
            OnJobUpdate(JobUpdateType.Update);
            return resList;
        }

        /// <summary>
        /// Run the simulation by using the initial parameter within the range of parameters.
        /// The number of sample is set. SetLoggerData and SetParameterRange should be called, before this function use.
        /// </summary>
        /// <param name="groupName">the group name</param>
        /// <param name="topDir">top directory include the script file and result data.</param>
        /// <param name="modelName">model name executed the simulation.</param>
        /// <param name="num">the number of sample.</param>
        /// <param name="count">simulation time or simulation step.</param>
        /// <param name="isStep">the flag use simulation time or simulation step.</param>
        /// <returns>Dictionary of jobid and the execution parameter.</returns>
        public Dictionary<int, ExecuteParameter> RunSimParameterRange(string groupName, string topDir, string modelName, int num, double count, bool isStep)
        {
            Dictionary<int, ExecuteParameter> resList = new Dictionary<int, ExecuteParameter>();
            Project prj = m_env.DataManager.CurrentProject;
            ScriptWriter writer = new ScriptWriter(prj);
            List<EcellObject> sysList = m_groupDic[groupName].SystemObjectList;
            List<EcellObject> stepperList = m_groupDic[groupName].StepperObjectList;
            Dictionary<string, double> paramDic = new Dictionary<string, double>();
            Random hRandom = new Random();
            if (!Directory.Exists(topDir))
            {
                Directory.CreateDirectory(topDir);
            }
            for (int i = 0 ; i < num ; i++ )
            {
                paramDic.Clear();
                foreach (EcellParameterData p in m_paramList)
                {
                    double data = 0.0;
                    if (p.Step <= 0.0)
                    {
                        double d = hRandom.NextDouble();
                        data = d  * (p.Max - p.Min) + p.Min;
                    }
                    else
                    {
                        data = p.Step * i + p.Min;
                    }
                    paramDic.Add(p.Key, data);
                }

                Job job = Proxy.CreateJob();
                job.GroupName = groupName;
                job.Manager = this;
                string dirName = topDir + "/" + job.JobID;
                string fileName = topDir + "/" + job.JobID + ".ess";
                string modelFileName = topDir + "/" + job.JobID + ".eml";

                List<string> extFileList = ExtractExtFileList(topDir + "/" + job.JobID, m_logList);

                if (m_groupDic[groupName].Status == AnalysisStatus.Stopped)
                    return new Dictionary<int, ExecuteParameter>();
                int jobid = RegisterJob(job, m_proxy.GetDefaultScript(), fileName, extFileList);
                if (this.Proxy.IsIDE() == true)
                {
                    CreateLocalScript(topDir, dirName, fileName, writer,
                        modelName, count, isStep, sysList, stepperList, paramDic);
                }
                else
                {
                    CreateUnixScript(jobid, topDir, dirName, fileName, modelFileName, writer,
                        modelName, count, isStep, sysList, stepperList, paramDic);
                }
                job.ExecParam = new ExecuteParameter(paramDic);
                resList.Add(jobid, new ExecuteParameter(paramDic));
                Application.DoEvents();
            }
            Run(groupName, false);
            OnJobUpdate(JobUpdateType.Update);
            return resList;
        }

        /// <summary>
        /// Set the top directory because system change the output directory each simulation.
        /// Only use RunSimParameterRange and RunSimParameterMatrix.
        /// </summary>
        /// <param name="topDir">the top directory set each simulation.</param>
        private void SetLogTopDirectory(string topDir)
        {
            string res = topDir.Replace("\\", "\\\\");
            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();
            foreach (SaveLoggerProperty s in m_logList)
            {
                s.DirName = res;
            }
        }

        /// <summary>
        /// Extract the list of file from the information of logger.
        /// </summary>
        /// <param name="topDir">top directory.</param>
        /// <param name="logList">the list of logger information.</param>
        /// <returns>the list of file.</returns>
        private static List<string> ExtractExtFileList(string topDir, List<SaveLoggerProperty> logList)
        {
            List<string> resList = new List<string>();
            foreach (SaveLoggerProperty s in logList)
            {
                string outName = Util.GetOutputFileName(s.FullPath, false);
                string fileName = topDir + "/" + outName;
                s.DirName = topDir;
                resList.Add(fileName);
            }
            return resList;
        }

        /// <summary>
        /// Create the parameter set with random of ParameterRange.
        /// </summary>
        /// <returns>the parameter set.</returns>
        public ExecuteParameter CreateExecuteParameter()
        {
            Dictionary<string, double> paramDic = new Dictionary<string, double>();
            Random hRandom = new Random();
            paramDic.Clear();
            foreach (EcellParameterData p in m_paramList)
            {
                double data = 0.0;
                double d = hRandom.NextDouble();
                data = d * (p.Max - p.Min) + p.Min;
                paramDic.Add(p.Key, data);
            }
            return new ExecuteParameter(paramDic);
        }

        /// <summary>
        /// Run the simulation by using the initial parameter according with ParameterRange object.
        /// SetLoggerData and SetParameterRange should be called, before this function use.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        /// <param name="topDir">top directory include the script file and result data.</param>
        /// <param name="modelName">model name executed the simulation.</param>
        /// <param name="count">simulation time or simulation step.</param>
        /// <param name="isStep">the flag use simulation time or simulation step.</param>
        /// <returns>Dictionary of jobid and the execution parameter.</returns>
        public Dictionary<int, ExecuteParameter> RunSimParameterMatrix(string groupName, string topDir, string modelName, double count, bool isStep)
        {
            Dictionary<int, ExecuteParameter> resList = new Dictionary<int, ExecuteParameter>();
            Project prj = m_env.DataManager.CurrentProject;
            ScriptWriter writer = new ScriptWriter(prj);
            List<EcellObject> sysList = m_groupDic[groupName].SystemObjectList;
            List<EcellObject> stepperList = m_groupDic[groupName].StepperObjectList;
            Dictionary<string, double> paramDic = new Dictionary<string, double>();
            if (m_paramList.Count != 2)
            {
                Util.ShowErrorDialog("ERROR");
                return resList; 
            }
            EcellParameterData x = m_paramList[0];
            EcellParameterData y = m_paramList[1];
            if (!Directory.Exists(topDir))
            {
                Directory.CreateDirectory(topDir);
            }
            int i = 0;
            for (double xd = x.Min ; xd <= x.Max; xd += x.Step)
            {
                int j = 0;
                for (double yd = y.Min; yd <= y.Max; yd += y.Step)
                {
                    paramDic.Clear();
                    foreach (EcellObject sysObj in sysList)
                    {
                        if (sysObj.Value != null)
                        {
                            foreach (EcellData d in sysObj.Value)
                            {
                                if (d.EntityPath.Equals(x.Key))
                                {
                                    d.Value = new EcellValue(xd);
                                    paramDic.Add(x.Key, xd);
                                }
                                else if (d.EntityPath.Equals(y.Key))
                                {
                                    d.Value = new EcellValue(yd);
                                    paramDic.Add(y.Key, yd);
                                }
                            }
                        }
                    }
                    foreach (EcellObject sysObj in sysList)
                    {
                        foreach (EcellObject obj in sysObj.Children)
                        {
                            if (obj.Value == null) continue;
                            foreach (EcellData d in obj.Value)
                            {
                                if (d.EntityPath.Equals(x.Key))
                                {
                                    d.Value = new EcellValue(xd);
                                    paramDic.Add(x.Key, xd);
                                }
                                else if (d.EntityPath.Equals(y.Key))
                                {
                                    d.Value = new EcellValue(yd);
                                    paramDic.Add(y.Key, yd);
                                }
                            }
                            Application.DoEvents();
                        }
                    }

                    Job job = Proxy.CreateJob();
                    job.GroupName = groupName;
                    job.Manager = this;
                    string dirName = topDir + "/" + job.JobID;
                    string fileName = topDir + "/" + job.JobID + ".ess";
                    string modelFileName = topDir + "/" + job.JobID + ".eml";

                    List<string> extFileList = ExtractExtFileList(topDir + "/" + job.JobID, m_logList);
                    if (m_groupDic[groupName].Status == AnalysisStatus.Stopped)
                        return new Dictionary<int, ExecuteParameter>();

                    int jobid = RegisterJob(job, m_proxy.GetDefaultScript(), fileName, extFileList);

                    if (this.Proxy.IsIDE())
                    {
                        CreateLocalScript(topDir, dirName, fileName, writer,
                            modelName, count, isStep, sysList, stepperList, paramDic);
                    }
                    else
                    {
                        CreateUnixScript(jobid, topDir, dirName, fileName, modelFileName, writer,
                            modelName, count, isStep, sysList, stepperList, paramDic);
                    }
                    job.ExecParam = new ExecuteParameter(paramDic);
                    resList.Add(jobid, new ExecuteParameter(paramDic));
                    Application.DoEvents();
                    j++;
                }
                i++;
            }
            Run(groupName, false);
            OnJobUpdate(JobUpdateType.Update);
            return resList;
        }

        /// <summary>
        /// Create the script for linux.
        /// </summary>
        /// <param name="jobID">the job id.</param>
        /// <param name="topDir">the top directory.</param>
        /// <param name="dirName">the execute directory.</param>
        /// <param name="fileName">the script file.</param>
        /// <param name="modelFile">the model file.</param>
        /// <param name="writer">the file writer.</param>
        /// <param name="modelName">the model name.</param>
        /// <param name="count">the simulation time.</param>
        /// <param name="isStep">the flag whetehr simulation is step.</param>
        /// <param name="sysList">the list of system object.</param>
        /// <param name="paramDic">the dictionary of parameters.</param>
        private void CreateUnixScript(int jobID, string topDir, string dirName, string fileName, string modelFile,
            ScriptWriter writer, string modelName, double count, bool isStep, List<EcellObject> sysList,
            List<EcellObject> stepperList, Dictionary<string, double> paramDic)
        {
            Encoding enc = Encoding.GetEncoding(51932);
            SetLogTopDirectory(dirName);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            //List<string> modelList = new List<string>();
            //modelList.Add(modelName);
            //m_env.DataManager.ExportModel(modelList, modelFile);

            List<EcellObject> storedList = new List<EcellObject>();
            storedList.AddRange(stepperList);
            storedList.AddRange(sysList);
            EmlWriter.Create(modelFile, storedList, false);

            writer.ClearScriptInfo();
            File.WriteAllText(fileName, "", enc);

            writer.WriteModelEntryUnix(fileName, enc, jobID);
            foreach (EcellObject sysObj in sysList)
            {
                foreach (string path in paramDic.Keys)
                {
                    if (sysObj.Value == null) continue;
                    foreach (EcellData v in sysObj.Value)
                    {
                        if (!path.Equals(v.EntityPath))
                            continue;
                        v.Value = new EcellValue(paramDic[path]);
                        writer.WriteComponentPropertyUnix(fileName, enc, sysObj, v);
                        break;
                    }
                }
            }
            Application.DoEvents();
            foreach (EcellObject sysObj in sysList)
            {
                EcellObject tmpObj = sysObj.Clone();
                foreach (string path in paramDic.Keys)
                {
                    foreach (EcellObject obj in tmpObj.Children)
                    {
                        if (obj.Value == null) continue;
                        foreach (EcellData v in obj.Value)
                        {
                            if (!path.Equals(v.EntityPath))
                                continue;
                            v.Value = new EcellValue(paramDic[path]);
                            writer.WriteComponentPropertyUnix(fileName, enc, obj, v);
                            break;
                        }
                        Application.DoEvents();
                    }
                }
            }


            Application.DoEvents();
            List<string> sList = new List<string>();
            foreach (SaveLoggerProperty s in m_logList)
            {
                sList.Add(s.FullPath);
            }
            writer.WriteLoggerPropertyUnix(fileName, enc, sList);
            if (isStep)
                writer.WriteSimulationForStepUnix(fileName, (int)(count), enc);
            else
                writer.WriteSimulationForTimeUnix(fileName, count, enc);
            writer.WriteLoggerSaveEntryUnix(fileName, enc, jobID, 
                m_logList, Proxy.GetData(GlobusJob.TOPDIR_NAME));
        }

        /// <summary>
        /// Create the script for windows.
        /// </summary>
        /// <param name="topDir">the top directory.</param>
        /// <param name="dirName">the execute directory.</param>
        /// <param name="fileName">the script file.</param>
        /// <param name="writer">the file writer.</param>
        /// <param name="modelName">the model name.</param>
        /// <param name="count">the simulation time.</param>
        /// <param name="isStep">the flag whetehr simulation is step.</param>
        /// <param name="sysList">the list of system object.</param>
        /// <param name="paramDic">the dictionary of parameters.</param>
        private void CreateLocalScript(string topDir, string dirName, string fileName, ScriptWriter writer,
                string modelName, double count, bool isStep, List<EcellObject> sysList,
                List<EcellObject> stepperList, Dictionary<string, double> paramDic)
        {
            Encoding enc = Encoding.GetEncoding(932);
            SetLogTopDirectory(dirName);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            writer.ClearScriptInfo();
            File.WriteAllText(fileName, "", enc);
            writer.WritePrefix(fileName, enc);
            writer.WriteModelEntry(fileName, enc, modelName, stepperList);
            writer.WriteModelProperty(fileName, enc, modelName, stepperList);
            File.AppendAllText(fileName, "\n# System\n", enc);
            foreach (EcellObject sysObj in sysList)
            {
                foreach (string path in paramDic.Keys)
                {
                    if (sysObj.Value == null) continue;
                    foreach (EcellData v in sysObj.Value)
                    {
                        if (!path.Equals(v.EntityPath))
                            continue;
                        v.Value = new EcellValue(paramDic[path]);
                        break;
                    }
                }
                writer.WriteSystemEntry(fileName, enc, modelName, sysObj);
                writer.WriteSystemProperty(fileName, enc, modelName, sysObj);
            }
            Application.DoEvents();
            foreach (EcellObject sysObj in sysList)
            {
                EcellObject tmpObj = sysObj.Clone();
                foreach (string path in paramDic.Keys)
                {
                    foreach (EcellObject obj in tmpObj.Children)
                    {
                        if (obj.Value == null) continue;
                        foreach (EcellData v in obj.Value)
                        {
                            if (!path.Equals(v.EntityPath))
                                continue;
                            v.Value = new EcellValue(paramDic[path]);
                            break;
                        }
                        Application.DoEvents();
                    }
                }
                writer.WriteComponentEntry(fileName, enc, tmpObj);
                writer.WriteComponentProperty(fileName, enc, tmpObj);
            }
            Application.DoEvents();
            File.AppendAllText(fileName, "session.initialize()\n", enc);
            List<string> sList = new List<string>();
            foreach (SaveLoggerProperty s in m_logList)
            {
                sList.Add(s.FullPath);
            }
            writer.WriteLoggerProperty(fileName, enc, sList);
            if (isStep)
                writer.WriteSimulationForStep(fileName, (int)(count), enc);
            else
                writer.WriteSimulationForTime(fileName, count, enc);
            writer.WriteLoggerSaveEntry(fileName, enc, m_logList);
        }

        /// <summary>
        /// Create the job group.
        /// </summary>
        /// <param name="name">the group name.</param>
        /// <param name="sysObjList">the list of system object.</param>
        /// <param name="stepperList">the list of stepper object.</param>
        /// <returns></returns>
        public JobGroup CreateJobGroup(string name, List<EcellObject> sysObjList, List<EcellObject> stepperList)
        {
            JobGroup group = new JobGroup(this, name, sysObjList, stepperList);
            m_groupDic[group.GroupName] = group;
            OnJobUpdate(JobUpdateType.AddJobGroup);
            return group;
        }

        /// <summary>
        /// Create the job group.
        /// </summary>
        /// <param name="name">the group name.</param>
        /// <param name="date">the date</param>
        /// <param name="sysObjList">the list of system object.</param>
        /// <param name="stepperList">the list of stepper object.</param>
        /// <returns></returns>
        public JobGroup CreateJobGroup(string name, string date, List<EcellObject> sysObjList, List<EcellObject> stepperList)
        {
            JobGroup group = new JobGroup(this, name, date, sysObjList, stepperList);
            m_groupDic[group.GroupName] = group;
            OnJobUpdate(JobUpdateType.AddJobGroup);
            return group;
        }

        /// <summary>
        /// Remove the job group.
        /// </summary>
        /// <param name="name">the group name.</param>
        public void RemoveJobGroup(string name)
        {
            if (!m_groupDic.ContainsKey(name))
                return;

            m_groupDic[name].Clear();
            m_groupDic[name].Delete();
            m_groupDic.Remove(name);
            OnJobUpdate(JobUpdateType.DeleteJobGroup);
        }

        /// <summary>
        /// Save the job group.
        /// </summary>
        /// <param name="name">the group name.</param>
        public void SaveJobGroup(string name)
        {
            m_groupDic[name].IsSaved = true;
            OnJobUpdate(JobUpdateType.SaveJobGroup);
        }

        /// <summary>
        /// Notify the job that is error.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="message">Error message.</param>
        public void NotifyErrorMessage(string groupName, string message)
        {
            try
            {
                ReportingSession rs = m_env.ReportManager.GetReportingSession(Constants.groupAnalysis);
                using (rs)
                {
                    AnalysisReport rep = new AnalysisReport(MessageType.Error, message, Constants.groupAnalysis, groupName);
                    rs.Add(rep);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }
}
