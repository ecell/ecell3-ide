using System;
using System.Collections.Generic;

using Ecell.Objects;

namespace Ecell.Job
{
    /// <summary>
    /// EventHandler when job status is updated.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void JobUpdateEventHandler(object o, JobUpdateEventArgs e);

    /// <summary>
    /// Interface of JobManager.
    /// </summary>
    public interface IJobManager
    {
        event JobUpdateEventHandler JobUpdateEvent;

        #region Accessors
        /// <summary>
        /// get the temporary directory.
        /// </summary>
        string TmpDir { get; }
        /// <summary>
        /// get / set the temporary root directory.
        /// </summary>
        string TmpRootDir { get; set; }
        /// <summary>
        /// get / set the interval time of update.
        /// </summary>
        int UpdateInterval { get; set; }
        /// <summary>
        /// get the ApplicationEnvironment.
        /// </summary>
        ApplicationEnvironment Environment { get; }
        /// <summary>
        /// get / set the concurrency of job.
        /// </summary>
        int Concurrency { get; set; }
        /// <summary>
        /// get / set the timeout of all jobs.
        /// </summary>
        int GlobalTimeOut { get; set; }
        /// <summary>
        /// get / set the flag whether the tmp directory is able to be deleted.
        /// </summary>
        bool IsTmpDirRemovable { get; set; }
        /// <summary>
        /// set the limit number of retry.
        /// </summary>
        int LimitRetry { set; }
        /// <summary>
        /// get / set the proxy of job.
        /// </summary>
        JobProxy Proxy { get; set; }
        ///// <summary>
        ///// get / set the dictionary of job id and job.
        ///// </summary>
        //Dictionary<int, Job> JobList { get; }
        /// <summary>
        /// get the dictionaty the group name and job group object.
        /// </summary>
        Dictionary<string, JobGroup> GroupDic { get; }
        /// <summary>
        /// get the dictionary the analysis name and analysis object.
        /// </summary>
        Dictionary<string, IAnalysisModule> AnalysisDic { get; }
        #endregion

        /// <summary>
        /// Clear the job and job group because the project is closed.
        /// </summary>
        void Clear();
        /// <summary>
        /// Clear the files of the selected jobs.
        /// </summary>
        /// <param name="jobID">the deleted job.</param>
        void ClearJob(string groupName, int jobID);
        /// <summary>
        /// Delete the selected jobs.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="jobID"></param>
        void DeleteJob(string groupName, int jobID);
        /// <summary>
        /// Create the execute parameter.
        /// </summary>
        /// <returns>the execute parameter.</returns>
        ExecuteParameter CreateExecuteParameter();
        /// <summary>
        /// Get the default concurrency of current environment.
        /// </summary>
        /// <returns>the number of concurrency.</returns>
        int GetDefaultConcurrency();
        /// <summary>
        /// Get the default concurrency of selected environment.
        /// </summary>
        /// <param name="env">the selected environment.</param>
        /// <returns>the number of concurrency.</returns>
        int GetDefaultConcurrency(string env);
        /// <summary>
        /// Get the default property list of selected environment.
        /// </summary>
        /// <param name="env">the selected environment.</param>
        /// <returns>the dictionary of property name and property value.</returns>
        Dictionary<string, object> GetDefaultEnvironmentProperty(string env);
        /// <summary>
        /// Get the name of current environment.
        /// </summary>
        /// <returns>the name of environment.</returns>
        string GetCurrentEnvironment();
        /// <summary>
        /// Set the current environment.
        /// </summary>
        /// <param name="env">the name of environment.</param>
        void SetCurrentEnvironment(string env);
        /// <summary>
        /// Get the list of environment.
        /// </summary>
        /// <returns>the list of environment name.</returns>
        List<string> GetEnvironmentList();
        /// <summary>
        /// Get the dictionary of property name and property value of the current environment.
        /// </summary>
        /// <returns>the dictionary of property name and property value.</returns>
        Dictionary<string, object> GetEnvironmentProperty();
        /// <summary>
        /// Get the working directory of selected job.
        /// </summary>
        /// <param name="jobid">the ID of selected job.</param>
        /// <returns>the path of working directory.</returns>
        string GetJobDirectory(string name, int jobid);
        /// <summary>
        /// Get the string of option.
        /// </summary>
        /// <returns>options for SystemProxy.</returns>
        string GetOptionList();
        /// <summary>
        /// Get the all list of SessionProxy or SessionProxy with jobid.
        /// </summary>
        /// <param name="jobid">jobid.</param>
        /// <returns>the list of SessionProxy.</returns>
        List<Job> GetSessionProxy(string name, int jobid);
        /// <summary>
        /// Get the stream of StdErr.
        /// </summary>
        /// <param name="jobid">job id.</param>
        /// <returns>StreamReader</returns>
        string GetStderr(string name, int jobid);
        /// <summary>
        /// Get the stream of StrOut.
        /// </summary>
        /// <param name="jobid">job id.</param>
        /// <returns>StreamReader</returns>
        string GetStdout(string name, int jobid);
        /// <summary>
        /// Get the list of finished jobs.
        /// </summary>
        /// <returns>the list of jobs.</returns>
        List<Job> GetFinishedJobList();
        /// <summary>
        /// Get the list of queued jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        List<Job> GetQueuedJobList();
        /// <summary>
        /// Get the list of running jobs.
        /// </summary>
        /// <returns>List of SessionProxy.</returns>
        List<Job> GetRunningJobList();
        /// <summary>
        /// Check whther there are any error jobs.
        /// </summary>
        /// <returns>if there is error job, return true.</returns>
        bool IsError(string name);
        /// <summary>
        /// Check whether all jobs is finished.
        /// </summary>
        /// <returns>if all jobs is finished, retur true.</returns>
        bool IsFinished(string name);
        /// <summary>
        /// Regist the session of e-cell.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="arg">the argument of script.</param>
        /// <param name="extFile">the list of extension file.</param>
        /// <param name="script">the script file.</param>
        /// <returns>the status of job.</returns>
        int RegisterEcellSession(string groupName, string script, string arg, List<string> extFile);
        /// <summary>
        /// Regist the jobs.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="script">Script file name.</param>
        /// <param name="arg">Argument of script file.</param>
        /// <param name="extFile">Extra file list of script file.</param>
        /// <returns>the status of job.</returns>
        int RegisterJob(Job job, string script, string arg, List<string> extFile);
        /// <summary>
        /// Run the jobs.
        /// <param name="groupName">the executed group name.</param>
        /// </summary>
        void Run(string groupName, bool isForce);
        /// <summary>
        /// Run the job.
        /// </summary>
        /// <param name="groupName">group name of the executed job.</param>
        /// <param name="jobid">job id of the executed job.</param>
        void Run(string groupName, int jobid);
        /// <summary>
        /// Run the simulation by using the initial parameter according with ParameterRange object.
        /// SetLoggerData and SetParameterRange should be called, before this function use.
        /// </summary>
        /// <param name="topDir">top directory include the script file and result data.</param>
        /// <param name="modelName">model name executed the simulation.</param>
        /// <param name="count">simulation time or simulation step.</param>
        /// <param name="isStep">the flag use simulation time or simulation step.</param>
        /// <returns>Dictionary of jobid and the execution parameter.</returns>
        Dictionary<int, ExecuteParameter> RunSimParameterMatrix(string groupName, string topDir, string modelName, double count, bool isStep);
        /// <summary>
        /// Run the simulation by using the initial parameter within the range of parameters.
        /// The number of sample is set. SetLoggerData and SetParameterRange should be called, before this function use.
        /// </summary>
        /// <param name="topDir">top directory include the script file and result data.</param>
        /// <param name="modelName">model name executed the simulation.</param>
        /// <param name="num">the number of sample.</param>
        /// <param name="count">simulation time or simulation step.</param>
        /// <param name="isStep">the flag use simulation time or simulation step.</param>
        /// <returns>Dictionary of jobid and the execution parameter.</returns>
        Dictionary<int, ExecuteParameter> RunSimParameterRange(string groupName, string topDir, string modelName, int num, double count, bool isStep);
        /// <summary>
        /// Execute the simulation with using the set parameters.
        /// </summary>
        /// <param name="topDir">top directory include the script file and result data.</param>
        /// <param name="modelName">model name executed the simulation.</param>
        /// <param name="count">simulation time or simulation step.</param>
        /// <param name="isStep">the flag use simulation time or simulation step.</param>
        /// <param name="setparam">the set parameters.</param>
        /// <returns>Dictionary of jobid and the execution parameter.</returns>
        Dictionary<int, ExecuteParameter> RunSimParameterSet(string groupName, string topDir, string modelName, double count, bool isStep, Dictionary<int, ExecuteParameter> setparam);
        /// <summary>
        /// Run the jobs and execute this process until all SessionProxy is finished.
        /// </summary>
        void RunWaitFinish(string groupName);
        /// <summary>
        /// Update the property of proxy.
        /// </summary>
        /// <param name="list">the list of new property.</param>
        void SetEnvironmentProperty(Dictionary<string, object> list);
        /// <summary>
        /// Set the logger data to judge the result, when execute RunSimParameterRange or RunSimParameterMatrix.
        /// </summary>
        /// <param name="sList">the list of logger data.</param>
        void SetLoggerData(List<SaveLoggerProperty> sList);
        /// <summary>
        /// Set the range of initial parameter, when execute RunSimParameterMatrix and RunSimParameterRange.
        /// If you use RunSimParameterMatrix, the number of list must be 2.
        /// </summary>
        /// <param name="pList">the list of range for initial parameters.</param>
        void SetParameterRange(List<Ecell.Objects.EcellParameterData> pList);
        /// <summary>
        /// Stop the job with input ID of job. if jobid = 0, all job are stopped.
        /// </summary>
        /// <param name="jobid">stop the ID of job.</param>
        void Stop(string name, int jobid);
        /// <summary>
        /// Update the information of session.
        /// </summary>
        void Update();
        /// <summary>
        /// Create the job entry when the analysis result is loaded.
        /// </summary>
        /// <param name="groupName">the group name.</param>
        /// <param name="param">the analysis parameter.</param>
        /// <returns>return jobid.</returns>
        int CreateJobEntry(string groupName, ExecuteParameter param);
        /// <summary>
        /// Create the job group
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        JobGroup CreateJobGroup(string name, List<EcellObject> sysObjList, List<EcellObject> stepperList);
        /// <summary>
        /// Create the job group with the initial parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        JobGroup CreateJobGroup(string name, string date, List<EcellObject> sysObjList, List<EcellObject> stepperList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        void RemoveJobGroup(string name);
    }   
}
