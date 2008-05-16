using System;
using System.Collections.Generic;

namespace EcellLib.Job
{
    public interface IJobManager
    {
        #region Accessors
        string TmpDir { get; }
        string TmpRootDir { get; set; }
        int UpdateInterval { get; set; }
        ApplicationEnvironment Environment { get; }
        int Concurrency { get; set; }
        int GlobalTimeOut { get; set; }
        bool IsTmpDirRemovable { get; set; }
        int LimitRetry { set; }
        JobProxy Proxy { get; set; }
        Dictionary<int, ExecuteParameter> ParameterDic { get; set; }
        Dictionary<int, Job> JobList { get; }
        #endregion

        void ClearErrorJobs();
        void ClearFinishedJobs();
        void ClearJob(int jobID);
        void ClearQueuedJobs();
        void ClearRunningJobs();
        ExecuteParameter CreateExecuteParameter();
        int GetDefaultConcurrency();
        int GetDefaultConcurrency(string env);
        Dictionary<string, object> GetDefaultEnvironmentProperty(string env);
        string GetCurrentEnvironment();
        void SetCurrentEnvironment(string env);
        List<string> GetEnvironmentList();
        Dictionary<string, object> GetEnvironmentProperty();
        List<Job> GetErrorJobList();
        List<Job> GetFinishedJobList();
        string GetJobDirectory(int jobid);
        string GetOptionList();
        List<Job> GetQueuedJobList();
        List<Job> GetRunningJobList();
        List<Job> GetSessionProxy(int jobid);
        System.IO.StreamReader GetStderr(int jobid);
        System.IO.StreamReader GetStdout(int jobid);
        bool IsError();
        bool IsFinished();
        bool IsRunning();
        int RegisterEcellSession(string script, string arg, List<string> extFile);
        int RegisterJob(string script, string arg, List<string> extFile);
        void Run();
        Dictionary<int, ExecuteParameter> RunSimParameterMatrix(string topDir, string modelName, double count, bool isStep);
        Dictionary<int, ExecuteParameter> RunSimParameterRange(string topDir, string modelName, int num, double count, bool isStep);
        Dictionary<int, ExecuteParameter> RunSimParameterSet(string topDir, string modelName, double count, bool isStep, Dictionary<int, ExecuteParameter> setparam);
        void RunWaitFinish();
        void SetEnvironmentProperty(Dictionary<string, object> list);
        void SetLoggerData(List<SaveLoggerProperty> sList);
        void SetParameterRange(List<EcellLib.Objects.EcellParameterData> pList);
        void Stop(int jobid);
        void StopRunningJobs();
        void Update();
    }
}
