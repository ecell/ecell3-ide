//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

namespace Ecell.Job
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.IO;
    using Ecell.Objects;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestJobManager
    {

        private ApplicationEnvironment _env;
        private JobManager _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = new JobManager(_env);
        }
        /// <summary>
        /// 
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorJobManager()
        {
            Ecell.ApplicationEnvironment env = null;
            JobManager testJobManager = new JobManager(env);
            Assert.IsNotNull(testJobManager, "Constructor of type, JobManager failed to create instance.");

            Assert.AreEqual(1, testJobManager.Concurrency, "Concurrency is unexpected value.");
            Assert.AreEqual(env, testJobManager.Environment, "Environment is unexpected value.");
            Assert.AreEqual(0, testJobManager.GlobalTimeOut, "GlobalTimeOut is unexpected value.");
            Assert.AreEqual(false, testJobManager.IsTmpDirRemovable, "IsTmpDirRemovable is unexpected value.");
            Assert.IsEmpty(testJobManager.JobList, "JobList is unexpected value.");
            Assert.IsEmpty(testJobManager.ParameterDic, "ParameterDic is unexpected value.");
            Assert.IsNotNull(testJobManager.Proxy, "Proxy is unexpected value.");
            Assert.IsNotNull(testJobManager.TmpDir, "TmpDir is unexpected value.");
            Assert.IsNotNull(testJobManager.TmpRootDir, "TmpRootDir is unexpected value.");
            Assert.AreEqual(5, testJobManager.UpdateInterval, "UpdateInterval is unexpected value.");

            testJobManager.LimitRetry = 1;

            testJobManager.Concurrency = 2;
            Assert.AreEqual(2, testJobManager.Concurrency, "Concurrency is unexpected value.");

            testJobManager.GlobalTimeOut = 10;
            Assert.AreEqual(10, testJobManager.GlobalTimeOut, "GlobalTimeOut is unexpected value.");

            testJobManager.IsTmpDirRemovable = true;
            Assert.AreEqual(true, testJobManager.IsTmpDirRemovable, "IsTmpDirRemovable is unexpected value.");

            testJobManager.Proxy = new LocalJobProxy();
            Assert.IsNotNull(testJobManager.Proxy, "Proxy is unexpected value.");

            testJobManager.UpdateInterval = 10;
            Assert.AreEqual(10, testJobManager.UpdateInterval, "UpdateInterval is unexpected value.");

            testJobManager.ParameterDic = new Dictionary<int,ExecuteParameter>();
            Assert.AreEqual(new Dictionary<int, ExecuteParameter>(), testJobManager.ParameterDic, "UpdateInterval is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetCurrentEnvironment()
        {
            string env = "Local";
            _unitUnderTest.SetCurrentEnvironment(env);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEnvironmentList()
        {
            JobManager manager = new JobManager(_env);
            List<string> expectedList = new List<string>();
            expectedList.Add("Local");

            List<string> resultList = manager.GetEnvironmentList();
            Assert.AreEqual(expectedList, resultList, "GetEnvironmentList method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCurrentEnvironment()
        {
            JobManager manager = new JobManager(_env);

            string expectedString = "Local";
            string resultString = null;
            resultString = manager.GetCurrentEnvironment();
            Assert.AreEqual(expectedString, resultString, "GetCurrentEnvironment method returned unexpected result.");

            manager.Proxy = null;
            expectedString = null;
            resultString = manager.GetCurrentEnvironment();
            Assert.AreEqual(expectedString, resultString, "GetEnvironmentList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEnvironmentProperty()
        {
            JobManager manager = new JobManager(_env);

            Dictionary<string, object> expectedDictionary = new Dictionary<string,object>();
            Dictionary<string, object> resultDictionary = null;
            resultDictionary = manager.GetEnvironmentProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetEnvironmentProperty method returned unexpected result.");

            manager.Proxy = null;
            expectedDictionary = null;
            resultDictionary = manager.GetEnvironmentProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetEnvironmentProperty method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDefaultEnvironmentProperty()
        {
            string env = "Local";
            Dictionary<string, object> expectedDictionary = new Dictionary<string, object>();
            Dictionary<string, object> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetDefaultEnvironmentProperty(env);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetDefaultEnvironmentProperty method returned unexpected result.");

            env = "Empty";
            resultDictionary = _unitUnderTest.GetDefaultEnvironmentProperty(env);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetDefaultEnvironmentProperty method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetEnvironmentProperty()
        {
            JobManager manager = new JobManager(_env);
            Dictionary<string, object> list = new Dictionary<string, object>();
            manager.SetEnvironmentProperty(list);

            manager.Proxy = null;
            manager.SetEnvironmentProperty(list);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDefaultConcurrency()
        {
            JobManager manager = new JobManager(_env);
            int expectedInt32 = 1;
            int resultInt32 = 0;
            resultInt32 = manager.GetDefaultConcurrency();
            Assert.AreEqual(expectedInt32, resultInt32, "GetDefaultConcurrency method returned unexpected result.");

            manager.Proxy = null;
            resultInt32 = manager.GetDefaultConcurrency();
            Assert.AreEqual(expectedInt32, resultInt32, "GetDefaultConcurrency method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDefaultConcurrencyEnv()
        {
            string env = "Local";
            int expectedInt32 = 1;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetDefaultConcurrency(env);
            Assert.AreEqual(expectedInt32, resultInt32, "GetDefaultConcurrency method returned unexpected result.");

            env = "Empty";
            resultInt32 = _unitUnderTest.GetDefaultConcurrency(env);
            Assert.AreEqual(expectedInt32, resultInt32, "GetDefaultConcurrency method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRegisterJob()
        {
            JobManager manager = new JobManager(_env);

            string script = null;
            string arg = null;
            List<string> extFile = null;
            int expectedInt32 = 1;
            int resultInt32 = 0;

            LocalJob.ClearJobID();
            resultInt32 = manager.RegisterJob(null, script, arg, extFile);
            Assert.AreEqual(expectedInt32, resultInt32, "RegisterJob method returned unexpected result.");

            manager.Proxy = null;
            resultInt32 = manager.RegisterJob(null, script, arg, extFile);
            expectedInt32 = -1;
            Assert.AreEqual(expectedInt32, resultInt32, "RegisterJob method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateJobEntry()
        {
            JobManager manager = new JobManager(_env);
            LocalJob.ClearJobID();

            ExecuteParameter param = new ExecuteParameter();
            int expectedInt32 = 1;
            int resultInt32 = 0;

            resultInt32 = manager.CreateJobEntry(param);
            Assert.AreEqual(expectedInt32, resultInt32, "CreateJobEntry method returned unexpected result.");

            manager.Proxy = null;
            expectedInt32 = -1;
            resultInt32 = manager.CreateJobEntry(param);
            Assert.AreEqual(expectedInt32, resultInt32, "CreateJobEntry method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRegisterEcellSession()
        {
            JobManager manager = new JobManager(_env);
            LocalJob.ClearJobID();

            string script = null;
            string arg = null;
            List<string> extFile = null;
            int expectedInt32 = 1;
            int resultInt32 = 0;
            resultInt32 = manager.RegisterEcellSession(script, arg, extFile);
            Assert.AreEqual(expectedInt32, resultInt32, "RegisterEcellSession method returned unexpected result.");

            manager.Proxy = null;
            expectedInt32 = -1;
            resultInt32 = manager.RegisterEcellSession(script, arg, extFile);
            Assert.AreEqual(expectedInt32, resultInt32, "RegisterEcellSession method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClearJob()
        {
            JobManager manager = new JobManager(_env);
            LocalJob.ClearJobID();

            manager.CreateJobEntry(new ExecuteParameter());
            int jobID = 0;
            manager.ClearJob(jobID);

            jobID = manager.CreateJobEntry(new ExecuteParameter());
            manager.ClearJob(jobID);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClearQueuedJobs()
        {
            _unitUnderTest.ClearQueuedJobs();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClearRunningJobs()
        {
            _unitUnderTest.ClearRunningJobs();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClearErrorJobs()
        {
            _unitUnderTest.ClearErrorJobs();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClearFinishedJobs()
        {
            _unitUnderTest.CreateJobEntry(new ExecuteParameter());
            _unitUnderTest.ClearFinishedJobs();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdate()
        {
            JobManager manager = new JobManager(_env);
            manager.CreateJobEntry(new ExecuteParameter());
            foreach (Job job in manager.GetFinishedJobList())
                job.Status = JobStatus.RUNNING;
            manager.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetQueuedJobList()
        {
            List<Job> expectedList = new List<Job>();
            List<Job> resultList = null;
            resultList = _unitUnderTest.GetQueuedJobList();
            Assert.AreEqual(expectedList, resultList, "GetQueuedJobList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetRunningJobList()
        {
            List<Job> expectedList = new List<Job>();
            List<Job> resultList = null;
            resultList = _unitUnderTest.GetRunningJobList();
            Assert.AreEqual(expectedList, resultList, "GetRunningJobList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetErrorJobList()
        {
            List<Job> expectedList = new List<Job>();
            List<Job> resultList = null;
            resultList = _unitUnderTest.GetErrorJobList();
            Assert.AreEqual(expectedList, resultList, "GetErrorJobList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetFinishedJobList()
        {
            JobManager manager = new JobManager(_env);

            List<Job> expectedList = new List<Job>();
            List<Job> resultList = null;
            resultList = manager.GetFinishedJobList();
            Assert.AreEqual(expectedList, resultList, "GetFinishedJobList method returned unexpected result.");

            manager.CreateJobEntry(new ExecuteParameter());
            resultList = manager.GetFinishedJobList();
            Assert.IsNotEmpty(resultList, "GetFinishedJobList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsFinished()
        {
            JobManager manager = new JobManager(_env);

            bool expectedBoolean = true;
            bool resultBoolean = false;
            resultBoolean = manager.IsFinished();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsFinished method returned unexpected result.");

            manager.CreateJobEntry(new ExecuteParameter());
            foreach (Job job in manager.GetFinishedJobList())
                job.Status = JobStatus.RUNNING;

            expectedBoolean = false;
            resultBoolean = manager.IsFinished();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsFinished method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsError()
        {
            JobManager manager = new JobManager(_env);

            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = manager.IsError();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsError method returned unexpected result.");

            manager.CreateJobEntry(new ExecuteParameter());
            foreach (Job job in manager.GetFinishedJobList())
                job.Status = JobStatus.ERROR;

            expectedBoolean = true;
            resultBoolean = manager.IsError();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsFinished method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsRunning()
        {
            JobManager manager = new JobManager(_env);

            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsRunning();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsRunning method returned unexpected result.");

            manager.CreateJobEntry(new ExecuteParameter());
            foreach (Job job in manager.GetFinishedJobList())
                job.Status = JobStatus.RUNNING;

            expectedBoolean = true;
            resultBoolean = manager.IsRunning();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsFinished method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRun()
        {
            _unitUnderTest.Run();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRunWaitFinish()
        {
            JobManager manager = new JobManager(_env);
            manager.CreateJobEntry(new ExecuteParameter());
            //foreach (Job job in manager.GetFinishedJobList())
            //    job.Status = JobStatus.RUNNING;

            manager.RunWaitFinish();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStop()
        {
            int jobid = 0;
            _unitUnderTest.Stop(jobid);

            LocalJobProxy proxy = new LocalJobProxy();
            Job j = proxy.CreateJob();
            _unitUnderTest.RegisterJob(j, "", "", new List<string>());
            _unitUnderTest.Stop(j.JobID);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStopRunningJobs()
        {
            LocalJobProxy proxy = new LocalJobProxy();
            Job j = proxy.CreateJob();
            _unitUnderTest.RegisterJob(j, "", "", new List<string>());
            j.Status = JobStatus.RUNNING;
            _unitUnderTest.StopRunningJobs();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSessionProxy()
        {
            int jobid = 0;
            System.Collections.Generic.List<Ecell.Job.Job> expectedList = new List<Job>();
            System.Collections.Generic.List<Ecell.Job.Job> resultList = null;
            resultList = _unitUnderTest.GetSessionProxy(jobid);
            Assert.AreEqual(expectedList, resultList, "GetSessionProxy method returned unexpected result.");

            LocalJobProxy proxy = new LocalJobProxy();
            Job j = proxy.CreateJob();
            _unitUnderTest.RegisterJob(j, "", "", new List<string>());
            resultList = _unitUnderTest.GetSessionProxy(j.JobID);
            Assert.AreEqual(1, resultList.Count, "GetSessionProxy method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetJobDirectory()
        {
            int jobid = 0;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetJobDirectory(jobid);
            Assert.AreEqual(expectedString, resultString, "GetJobDirectory method returned unexpected result.");

            LocalJobProxy proxy = new LocalJobProxy();
            Job j = proxy.CreateJob();
            _unitUnderTest.RegisterJob(j, "", "", new List<string>());
            resultString = _unitUnderTest.GetJobDirectory(j.JobID);
            Assert.IsNotNull(resultString, "GetJobDirectory method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStdout()
        {
            int jobid = 1;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdout(jobid);
            Assert.AreEqual(expectedString, resultString, "GetStdout method returned unexpected result.");

            LocalJobProxy proxy = new LocalJobProxy();
            Job j = proxy.CreateJob();
            _unitUnderTest.RegisterJob(j, "", "", new List<string>());
            resultString = _unitUnderTest.GetStdout(j.JobID);
            Assert.AreEqual("", resultString, "GetStdout method returned unexpected result.");


        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStderr()
        {
            int jobid = 1;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStderr(jobid);
            Assert.AreEqual(expectedString, resultString, "GetStderr method returned unexpected result.");

            LocalJobProxy proxy = new LocalJobProxy();
            Job j = proxy.CreateJob();
            _unitUnderTest.RegisterJob(j, "", "", new List<string>());
            resultString = _unitUnderTest.GetStderr(j.JobID);
            Assert.AreEqual(expectedString, resultString, "GetStderr method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetOptionList()
        {
            string expectedString = " Test \"Test\"";
            string resultString = null;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Test", "Test");
            _unitUnderTest.Proxy.SetProperty(dic);
            resultString = _unitUnderTest.GetOptionList();
            Assert.AreEqual(expectedString, resultString, "GetOptionList method returned unexpected result.");

            _unitUnderTest.Proxy = null;
            expectedString = null;
            resultString = _unitUnderTest.GetOptionList();
            Assert.AreEqual(expectedString, resultString, "GetOptionList method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateExecuteParameter()
        {
            List<EcellParameterData> pList = new List<EcellParameterData>();
            pList.Add(new EcellParameterData("", 0.0));
            _unitUnderTest.SetParameterRange(pList);

            ExecuteParameter resultExecuteParameter = null;
            resultExecuteParameter = _unitUnderTest.CreateExecuteParameter();
            Assert.IsNotNull(resultExecuteParameter, "CreateExecuteParameter method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRunSimParameterSet()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            LocalJob.ClearJobID();
            string topDir = TestConstant.TestDirectory + "TestJob1";
            string modelName = "Drosophila";
            double count = 10;
            bool isStep = false;
            
            Dictionary<int, ExecuteParameter> setparam = new Dictionary<int, ExecuteParameter>();
            Dictionary<string, double> data = new Dictionary<string, double>();
            data.Add("Variable:/CELL/CYTOPLASM:P0:Value", 0.0);
            data.Add("System:/CELL:CYTOPLASM:Size", 1e-10);
            ExecuteParameter param = new ExecuteParameter(data);
            setparam.Add(0, param);

            Dictionary<int, ExecuteParameter> expectedDictionary = new Dictionary<int, ExecuteParameter>();
            Dictionary<int, ExecuteParameter> resultDictionary = new Dictionary<int, ExecuteParameter>();
            resultDictionary = _unitUnderTest.RunSimParameterSet(topDir, modelName, count, isStep, setparam);
            Assert.AreEqual(expectedDictionary, resultDictionary, "RunSimParameterSet method returned unexpected result.");
            //
            SetLoggerData();
            isStep = true;
            resultDictionary = _unitUnderTest.RunSimParameterSet(topDir, modelName, count, isStep, setparam);
            Assert.IsNotEmpty(resultDictionary, "RunSimParameterSet method returned unexpected result.");

            _unitUnderTest.GetJobDirectory(1);
            _unitUnderTest.GetStdout(1);
            _unitUnderTest.GetStderr(1);
            _unitUnderTest.GetSessionProxy(1);
            _unitUnderTest.GetSessionProxy(-1);
            _unitUnderTest.Stop(0);

            if (Directory.Exists(topDir))
                Directory.Delete(topDir, true);
        }

        private void SetLoggerData()
        {
            _env.PluginManager.ChangeStatus(ProjectStatus.Analysis);
            _env.DataManager.SetObservedData(new EcellObservedData("Variable:/CELL/CYTOPLASM:P0:Value", 5.0));

            List<SaveLoggerProperty> resList = new List<SaveLoggerProperty>();
            List<EcellObservedData> obsList = _env.DataManager.GetObservedData();

            foreach (EcellObservedData data in obsList)
            {
                String dir = _env.JobManager.TmpDir;
                string path = data.Key;
                double start = 0.0;
                double end = 100;

                SaveLoggerProperty p = new SaveLoggerProperty(path, start, end, dir);
                resList.Add(p);
            }
            _unitUnderTest.SetLoggerData(resList);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetParameterRange()
        {
            List<EcellParameterData> pList = new List<EcellParameterData>();
            pList.Add(new EcellParameterData("", 0.0));
            _unitUnderTest.SetParameterRange(pList);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRunSimParameterRange()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);

            string topDir = TestConstant.TestDirectory + "TestJob";
            string modelName = "Drosophila";
            double count = 10;
            bool isStep = false;
            int num = 3;

            List<EcellParameterData> list = new List<EcellParameterData>();
            list.Add(new EcellParameterData("Variable:/CELL/CYTOPLASM:M:Value", 2.0, 0.0, 0.0));
            list.Add(new EcellParameterData("System:/CELL:CYTOPLASM:Size", 1e-10, 1e-11, 4.5e-11));
            _unitUnderTest.SetParameterRange(list);

            Dictionary<int, ExecuteParameter> expectedDictionary = new Dictionary<int, ExecuteParameter>();
            Dictionary<int, ExecuteParameter> resultDictionary = null;
            resultDictionary = _unitUnderTest.RunSimParameterRange(topDir, modelName, num, count, isStep);
            Assert.AreEqual(expectedDictionary, resultDictionary, "RunSimParameterRange method returned unexpected result.");

            isStep = true;
            SetLoggerData();
            resultDictionary = _unitUnderTest.RunSimParameterRange(topDir, modelName, num, count, isStep);
            Assert.IsNotEmpty(resultDictionary, "RunSimParameterRange method returned unexpected result.");

            if (Directory.Exists(topDir))
                Directory.Delete(topDir, true);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRunSimParameterMatrix()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);

            string topDir = TestConstant.TestDirectory + "TestJob";
            string modelName = "Drosophila";
            double count = 10;
            bool isStep = false;

            if (Directory.Exists(topDir))
                Directory.Delete(topDir, true);

            Dictionary<int, ExecuteParameter> expectedDictionary = new Dictionary<int, ExecuteParameter>();
            Dictionary<int, ExecuteParameter> resultDictionary = null;
            resultDictionary = _unitUnderTest.RunSimParameterMatrix(topDir, modelName, count, isStep);
            Assert.AreEqual(expectedDictionary, resultDictionary, "RunSimParameterMatrix method returned unexpected result.");

            List<EcellParameterData> list = new List<EcellParameterData>();
            list.Add(new EcellParameterData("Variable:/CELL/CYTOPLASM:M:Value", 2.0, 0.0, 1.0));
            list.Add(new EcellParameterData("System:/CELL:CYTOPLASM:Size", 1e-10, 1e-11, 4.5e-11));
            _unitUnderTest.SetParameterRange(list);

            SetLoggerData();
            resultDictionary = _unitUnderTest.RunSimParameterMatrix(topDir, modelName, count, isStep);
            Assert.IsNotEmpty(resultDictionary, "RunSimParameterMatrix method returned unexpected result.");

            isStep = true;
            _env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
            list = new List<EcellParameterData>();
            list.Add(new EcellParameterData("System:/CELL:CYTOPLASM:Size", 1e-10, 1e-11, 4.5e-11));
            list.Add(new EcellParameterData("Variable:/CELL/CYTOPLASM:M:Value", 2.0, 0.0, 1.0));
            _unitUnderTest.SetParameterRange(list);

            resultDictionary = _unitUnderTest.RunSimParameterMatrix(topDir, modelName, count, isStep);
            Assert.IsEmpty(resultDictionary, "RunSimParameterMatrix method returned unexpected result.");

            if (Directory.Exists(topDir))
                Directory.Delete(topDir, true);

        }
    }
}
