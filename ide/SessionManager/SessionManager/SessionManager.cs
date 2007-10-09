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
using EcellLib;

namespace SessionManager
{
    /// <summary>
    /// Management class of session.
    /// </summary>
    public class SessionManager
    {
        private bool m_tmpDirRemovable = false;
        private string m_module = null;
        private string m_tmpRootDir = null;
        private string m_tmpDir = null;
        private int m_conc = -1;
        private int m_limitRetry = 5;
        private int m_updateInterval = 5;
        private int m_globalTimeOut = 0;
        private SystemProxy m_proxy;
        private Dictionary<string, SystemProxy> m_proxyList = new Dictionary<string, SystemProxy>();
        private Dictionary<int, SessionProxy> m_sessionList = new Dictionary<int, SessionProxy>();

        private Timer m_timer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SessionManager()
        {
            m_timer = new Timer();
            m_timer.Enabled = false;
            m_timer.Interval = m_updateInterval;
            m_timer.Tick += new EventHandler(UpdateTimeFire);

            LocalSystemProxy p = new LocalSystemProxy();
            p.Manager = this;
            m_proxyList.Add("Local", p);
        }


        /// <summary>
        /// Constructor with the initial prameters.
        /// </summary>
        /// <param name="module">module name.</param>
        /// <param name="conc">concurrency.</param>
        /// <param name="env">environment object.</param>
        public SessionManager(string module, int conc, String env)
        {
            this.m_module = module;
            this.m_conc = conc;
            this.SetEnvironment(env);

            m_timer = new Timer();
            m_timer.Enabled = false;
            m_timer.Interval = m_updateInterval;
            m_timer.Tick += new EventHandler(UpdateTimeFire);

            LocalSystemProxy p = new LocalSystemProxy();
            p.Manager = this;
            m_proxyList.Add("Local", p);
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
                this.m_tmpDir = m_tmpDir + "/" + p.Id;
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
        /// get the list of session.
        /// </summary>
        public Dictionary<int, SessionProxy> SessionList
        {
            get { return this.m_sessionList; }
        }

        /// <summary>
        /// get / set the using proxy for system.
        /// </summary>
        public SystemProxy Proxy
        {
            get { return this.m_proxy; }
            set { this.m_proxy = value; }
        }

        /// <summary>
        /// Set the environment with input name.
        /// </summary>
        /// <param name="env">the environment name.</param>
        public void SetEnvironment(String env)
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
        public List<String> GetEnvironmentList()
        {
            List<String> tmpList = new List<string>();
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
        public String GetEnvironment()
        {
            if (m_proxy == null) return null;
            return m_proxy.GetEnvironment();
        }

        /// <summary>
        /// Get the list of property for environment.
        /// </summary>
        /// <returns>the list of string.</returns>
        public Dictionary<String, Object> GetEnvironmentProperty()
        {
            if (m_proxy == null) return null;
            return m_proxy.GetProperty();
        }

        /// <summary>
        /// Update the property of proxy.
        /// </summary>
        /// <param name="list">the list of new property.</param>
        public void SetEnvironmentProperty(Dictionary<String, Object> list)
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
            return m_proxy.DefaultConcurrency;
        }

        /// <summary>
        /// Regist the jobs.
        /// </summary>
        /// <param name="script">Script file name.</param>
        /// <param name="arg">Argument of script file.</param>
        /// <param name="extFile">Extra file list of script file.</param>
        /// <returns>the status of job.</returns>
        public int RegisterJob(string script, string arg, List<string> extFile)
        {
            if (m_proxy == null) return -1;
            SessionProxy s = m_proxy.CreateSessionProxy();
            if (s == null) return -1;

            s.ScriptFile = script;
            s.Argument = arg;
            s.ExtraFileList = extFile;
            // dmpath
            s.JobDirectory = TmpDir + "/" + s.JobID;
            m_sessionList.Add(s.JobID, s);

            return s.JobID;
        }

        /// <summary>
        /// Regist the session of e-cell.
        /// </summary>
        /// <returns>the status of job.</returns>
        public int RegisterEcellSession()
        {
            // not implement
            return 0;
        }

        /// <summary>
        /// Delete the job. if jobID = 0, all jobs are deleted.
        /// </summary>
        /// <param name="jobID">the ID of deleted job.</param>
        public void ClearJob(int jobID)
        {
            if (jobID == 0)
            {
                m_sessionList.Clear();
            }
            else
            {
                if (m_sessionList.ContainsKey(jobID))
                    m_sessionList.Remove(jobID);
            }
        }

        /// <summary>
        /// Delete the queued jobs.
        /// </summary>
        public void ClearQueuedJobs()
        {
            List<int> delList = new List<int>();
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.QUEUED)
                    delList.Add(job);
            }

            foreach (int job in delList)
            {
                m_sessionList.Remove(job);
            }
        }

        /// <summary>
        /// Delete the running jobs.
        /// </summary>
        public void ClearRunningJobs()
        {
            List<int> delList = new List<int>();
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.RUNNING)
                    delList.Add(job);
            }

            foreach (int job in delList)
            {
                m_sessionList.Remove(job);
            }
        }

        /// <summary>
        /// Delete the error jobs.
        /// </summary>
        public void ClearErrorJobs()
        {
            List<int> delList = new List<int>();
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.ERROR)
                    delList.Add(job);
            }

            foreach (int job in delList)
            {
                m_sessionList.Remove(job);
            }
        }

        /// <summary>
        /// Delete the finished jobs.
        /// </summary>
        public void ClearFinishedJobs()
        {
            List<int> delList = new List<int>();
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.FINISHED)
                    delList.Add(job);
            }

            foreach (int job in delList)
            {
                m_sessionList.Remove(job);
            }
        }

        /// <summary>
        /// Update the information of session.
        /// </summary>
        public void Update()
        {
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.QUEUED ||
                    m_sessionList[job].Status == JobStatus.RUNNING)
                {
                    m_sessionList[job].Update();
                }
            }
            if (m_proxy != null) m_proxy.Update();
        }

        /// <summary>
        /// Get the list of queued jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        public List<SessionProxy> GetQueuedJobList()
        {
            List<SessionProxy> tmpList = new List<SessionProxy>();

            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.QUEUED)
                    tmpList.Add(m_sessionList[job]);
            }
            return tmpList;
        }

        /// <summary>
        /// Get the list of running jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        public List<SessionProxy> GetRunningJobList()
        {
            List<SessionProxy> tmpList = new List<SessionProxy>();

            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.RUNNING)
                    tmpList.Add(m_sessionList[job]);
            }
            return tmpList;
        }

        /// <summary>
        /// Get the list of error jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        public List<SessionProxy> GetErrorJobList()
        {
            List<SessionProxy> tmpList = new List<SessionProxy>();

            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.ERROR)
                    tmpList.Add(m_sessionList[job]);
            }
            return tmpList;

        }

        /// <summary>
        /// Get the list of finished jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        public List<SessionProxy> GetFinishedJobList()
        {
            List<SessionProxy> tmpList = new List<SessionProxy>();

            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.FINISHED)
                    tmpList.Add(m_sessionList[job]);
            }
            return tmpList;

        }

        /// <summary>
        /// Check whether all jobs is finished.
        /// </summary>
        /// <returns>if all jobs is finished, retur true.</returns>
        public bool IsFinished()
        {
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.QUEUED ||
                    m_sessionList[job].Status == JobStatus.RUNNING)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check whther there are any error jobs.
        /// </summary>
        /// <returns>if there is error job, return true.</returns>
        public bool IsError()
        {
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.ERROR)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether there are any error jobs.
        /// </summary>
        /// <returns>if there is running job, return true.</returns>
        public bool IsRunning()
        {
            foreach (int job in m_sessionList.Keys)
            {
                if (m_sessionList[job].Status == JobStatus.RUNNING)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Preapre to execute the process.
        /// Ex. script file, extra file, job directory and so on.
        /// </summary>
        private void PrepareProcessRun()
        {
            foreach (SessionProxy p in m_sessionList.Values)
            {
                if (p.Status == JobStatus.NONE)
                {
                    p.PrepareProcess();
                }
            }
        }

        /// <summary>
        /// Run the jobs.
        /// </summary>
        public void Run()
        {
            PrepareProcessRun();
            Update();
            m_timer.Enabled = true;
            m_timer.Start();
        }

        /// <summary>
        /// Run the jobs and execute this process until all SessionProxy is finished.
        /// </summary>
        public void RunWaitFinish()
        {
            PrepareProcessRun();

            while (!IsFinished())
            {
                Update();
                System.Threading.Thread.Sleep(m_updateInterval * 1000);
            }
        }

        /// <summary>
        /// Stop the job with input ID of job. if jobid = 0, all job are stopped.
        /// </summary>
        /// <param name="jobid">stop the ID of job.</param>
        public void Stop(int jobid)
        {
            if (jobid < 0) return;
            if (jobid == 0)
            {
                foreach (int id in m_sessionList.Keys)
                {
                    m_sessionList[id].stop();
                }
                m_timer.Enabled = false;
                m_timer.Stop();
            }
            else
            {
                m_sessionList[jobid].stop();
            }
        }

        /// <summary>
        /// Stop the running jobs.
        /// </summary>
        public void StopRunningJobs()
        {
            foreach (int id in m_sessionList.Keys)
            {
                if (m_sessionList[id].Status == JobStatus.QUEUED ||
                    m_sessionList[id].Status == JobStatus.RUNNING)
                {
                    m_sessionList[id].stop();
                }
            }
        }

        /// <summary>
        /// Get the all list of SessionProxy or SessionProxy with jobid.
        /// </summary>
        /// <param name="jobid">jobid.</param>
        /// <returns>the list of SessionProxy.</returns>
        public List<SessionProxy> GetSessionProxy(int jobid)
        {
            List<SessionProxy> tmpList = new List<SessionProxy>();
            if (jobid <= 0)
            {
                foreach (int job in m_sessionList.Keys)
                {
                    tmpList.Add(m_sessionList[job]);
                }
                return tmpList;
            }

            if (m_sessionList.ContainsKey(jobid))
                tmpList.Add(m_sessionList[jobid]);
            return tmpList;
        }

        /// <summary>
        /// Get the job directory of session correspond to jobID.
        /// </summary>
        /// <param name="jobid">JobID.</param>
        /// <returns>Directory path.</returns>
        public String GetJobDirectory(int jobid)
        {
            if (jobid <= 0) return null;
            if (m_sessionList.ContainsKey(jobid))
            {
                return m_sessionList[jobid].JobDirectory;
            }
            return null;
        }

        /// <summary>
        /// Get the stream of StrOut.
        /// </summary>
        /// <param name="jobid">job id.</param>
        /// <returns>StreamReader</returns>
        public System.IO.StreamReader GetStdout(int jobid)
        {
            return m_sessionList[jobid].GetStdOut();
        }

        /// <summary>
        /// Get the stream of StdErr.
        /// </summary>
        /// <param name="jobid">job id.</param>
        /// <returns>StreamReader</returns>
        public System.IO.StreamReader GetStderr(int jobid)
        {
            return m_sessionList[jobid].GetStdErr();
        }

        public void SetOptionList(List<String> optionList)
        {
            
            // not implement
        }

        public List<String> GetOptionList()
        {
            // not implement
            return null;
        }

        /// <summary>
        /// Update the status of session at intervals while program is running.
        /// </summary>
        /// <param name="sender">Timer.</param>
        /// <param name="e">EventArgs.</param>
        void UpdateTimeFire(object sender, EventArgs e)
        {
            Update();

            if (IsFinished())
            {
                m_timer.Enabled = false;
                m_timer.Stop();
            }
        }

        //======================================================
        // The following member and function is in here or
        // EcellLib.AnalysisManager.
        //======================================================
        private List<ParameterRange> m_paramList = new List<ParameterRange>();
        private List<SaveLoggerProperty> m_logList = new List<SaveLoggerProperty>();

        public void SetLoggerData(List<SaveLoggerProperty> sList)
        {
            m_logList.Clear();

            foreach (SaveLoggerProperty p in m_logList)
            {
                m_logList.Add(p);
            }
        }

        public void SetParameterRange(List<ParameterRange> pList)
        {
            m_paramList.Clear();

            foreach (ParameterRange p in pList)
            {
                m_paramList.Add(p);
            }
        }

        public void RunSimParameterRange(string topDir, string modelName, int num)
        {
            DataManager manager = DataManager.GetDataManager();
            List<EcellObject> sysList = manager.GetData(modelName, null);
            Dictionary<string, double> paramDic = new Dictionary<string, double>();
            Random hRandom = new Random();
            for (int i = 0 ; i < num ; i++ )
            {
                paramDic.Clear();
                foreach (ParameterRange p in m_paramList)
                {
                    double d = hRandom.NextDouble();
                    double data = (d - p.Min) / (p.Max - p.Min) + p.Min;
                    paramDic.Add(p.FullPath, data);
                }
                string dirName = topDir + "/" + num;
                string fileName = topDir + "/" + num + ".ess";
                Encoding enc = Encoding.GetEncoding(932);

                manager.ClearScriptInfo();
                File.WriteAllText(fileName, "", enc);
                manager.WritePrefix(fileName, enc);
                manager.WriteModelEntry(fileName, enc, modelName);
                manager.WriteModelProperty(fileName, enc, modelName);
                File.WriteAllText(fileName, "\n# System\n", enc);
                foreach (EcellObject sysObj in sysList)
                {
                    manager.WriteSystemEntry(fileName, enc, modelName, sysObj);
                    manager.WriteSystemProperty(fileName, enc, modelName, sysObj);
                }
                foreach (EcellObject sysObj in sysList)
                {
                    manager.WriteComponentEntry(fileName, enc, sysObj);
                    manager.WriteComponentProperty(fileName, enc, sysObj);
                }
                List<string> sList = new List<string>();
                foreach (SaveLoggerProperty s in m_logList)
                {
                    sList.Add(s.FullPath);
                }
                manager.WriteLoggerProperty(fileName, enc, sList); 
                manager.WriteSimulation(fileName, enc);
                manager.WriteLoggerSaveEntry(fileName, enc, m_logList);
            }
        }
        
    }

    public class ParameterRange
    {
        private string m_fullPath = "";
        private double m_min = 0.0;
        private double m_max = 0.0;

        public ParameterRange()
        {
        }

        public ParameterRange(string path, double min, double max)
        {
            m_fullPath = path;
            m_min = min;
            m_max = max;
        }

        public string FullPath
        {
            get { return this.m_fullPath; }
            set { this.m_fullPath = value; }
        }

        public double Max
        {
            get { return this.m_max; }
            set { this.m_max = value; }
        }

        public double Min
        {
            get { return this.m_min; }
            set { this.m_min = value; }
        }
    }
}
