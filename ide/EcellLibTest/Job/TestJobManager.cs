namespace Ecell.Job
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestJobManager
    {

        private JobManager _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.ApplicationEnvironment env = null;
            _unitUnderTest = new JobManager(env);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorJobManager()
        {
            Ecell.ApplicationEnvironment env = null;
            JobManager testJobManager = new JobManager(env);
            Assert.IsNotNull(testJobManager, "Constructor of type, JobManager failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetCurrentEnvironment()
        {
            string env = null;
            _unitUnderTest.SetCurrentEnvironment(env);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetEnvironmentList()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            resultList = _unitUnderTest.GetEnvironmentList();
            Assert.AreEqual(expectedList, resultList, "GetEnvironmentList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetCurrentEnvironment()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetCurrentEnvironment();
            Assert.AreEqual(expectedString, resultString, "GetCurrentEnvironment method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetEnvironmentProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Object> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetEnvironmentProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetEnvironmentProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetDefaultEnvironmentProperty()
        {
            string env = null;
            System.Collections.Generic.Dictionary<System.String, System.Object> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Object> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetDefaultEnvironmentProperty(env);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetDefaultEnvironmentProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetEnvironmentProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> list = null;
            _unitUnderTest.SetEnvironmentProperty(list);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetDefaultConcurrency()
        {
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetDefaultConcurrency();
            Assert.AreEqual(expectedInt32, resultInt32, "GetDefaultConcurrency method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetDefaultConcurrencyEnv()
        {
            string env = null;
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetDefaultConcurrency(env);
            Assert.AreEqual(expectedInt32, resultInt32, "GetDefaultConcurrency method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRegisterJob()
        {
            string script = null;
            string arg = null;
            System.Collections.Generic.List<System.String> extFile = null;
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.RegisterJob(script, arg, extFile);
            Assert.AreEqual(expectedInt32, resultInt32, "RegisterJob method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestCreateJobEntry()
        {
            Ecell.Job.ExecuteParameter param = null;
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.CreateJobEntry(param);
            Assert.AreEqual(expectedInt32, resultInt32, "CreateJobEntry method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRegisterEcellSession()
        {
            string script = null;
            string arg = null;
            System.Collections.Generic.List<System.String> extFile = null;
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.RegisterEcellSession(script, arg, extFile);
            Assert.AreEqual(expectedInt32, resultInt32, "RegisterEcellSession method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClearJob()
        {
            int jobID = 0;
            _unitUnderTest.ClearJob(jobID);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClearQueuedJobs()
        {
            _unitUnderTest.ClearQueuedJobs();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClearRunningJobs()
        {
            _unitUnderTest.ClearRunningJobs();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClearErrorJobs()
        {
            _unitUnderTest.ClearErrorJobs();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClearFinishedJobs()
        {
            _unitUnderTest.ClearFinishedJobs();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestUpdate()
        {
            _unitUnderTest.Update();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetQueuedJobList()
        {
            System.Collections.Generic.List<Ecell.Job.Job> expectedList = null;
            System.Collections.Generic.List<Ecell.Job.Job> resultList = null;
            resultList = _unitUnderTest.GetQueuedJobList();
            Assert.AreEqual(expectedList, resultList, "GetQueuedJobList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetRunningJobList()
        {
            System.Collections.Generic.List<Ecell.Job.Job> expectedList = null;
            System.Collections.Generic.List<Ecell.Job.Job> resultList = null;
            resultList = _unitUnderTest.GetRunningJobList();
            Assert.AreEqual(expectedList, resultList, "GetRunningJobList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetErrorJobList()
        {
            System.Collections.Generic.List<Ecell.Job.Job> expectedList = null;
            System.Collections.Generic.List<Ecell.Job.Job> resultList = null;
            resultList = _unitUnderTest.GetErrorJobList();
            Assert.AreEqual(expectedList, resultList, "GetErrorJobList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetFinishedJobList()
        {
            System.Collections.Generic.List<Ecell.Job.Job> expectedList = null;
            System.Collections.Generic.List<Ecell.Job.Job> resultList = null;
            resultList = _unitUnderTest.GetFinishedJobList();
            Assert.AreEqual(expectedList, resultList, "GetFinishedJobList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestIsFinished()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsFinished();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsFinished method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestIsError()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsError();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsError method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestIsRunning()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsRunning();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsRunning method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRun()
        {
            _unitUnderTest.Run();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRunWaitFinish()
        {
            _unitUnderTest.RunWaitFinish();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestStop()
        {
            int jobid = 0;
            _unitUnderTest.Stop(jobid);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestStopRunningJobs()
        {
            _unitUnderTest.StopRunningJobs();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetSessionProxy()
        {
            int jobid = 0;
            System.Collections.Generic.List<Ecell.Job.Job> expectedList = null;
            System.Collections.Generic.List<Ecell.Job.Job> resultList = null;
            resultList = _unitUnderTest.GetSessionProxy(jobid);
            Assert.AreEqual(expectedList, resultList, "GetSessionProxy method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetJobDirectory()
        {
            int jobid = 0;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetJobDirectory(jobid);
            Assert.AreEqual(expectedString, resultString, "GetJobDirectory method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetStdout()
        {
            int jobid = 0;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdout(jobid);
            Assert.AreEqual(expectedString, resultString, "GetStdout method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetStderr()
        {
            int jobid = 0;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStderr(jobid);
            Assert.AreEqual(expectedString, resultString, "GetStderr method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetOptionList()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetOptionList();
            Assert.AreEqual(expectedString, resultString, "GetOptionList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetLoggerData()
        {
            System.Collections.Generic.List<Ecell.SaveLoggerProperty> sList = null;
            _unitUnderTest.SetLoggerData(sList);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetParameterRange()
        {
            System.Collections.Generic.List<Ecell.Objects.EcellParameterData> pList = null;
            _unitUnderTest.SetParameterRange(pList);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRunSimParameterSet()
        {
            string topDir = null;
            string modelName = null;
            double count = 0;
            bool isStep = false;
            System.Collections.Generic.Dictionary<System.Int32, Ecell.Job.ExecuteParameter> setparam = null;
            System.Collections.Generic.Dictionary<System.Int32, Ecell.Job.ExecuteParameter> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.Int32, Ecell.Job.ExecuteParameter> resultDictionary = null;
            resultDictionary = _unitUnderTest.RunSimParameterSet(topDir, modelName, count, isStep, setparam);
            Assert.AreEqual(expectedDictionary, resultDictionary, "RunSimParameterSet method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRunSimParameterRange()
        {
            string topDir = null;
            string modelName = null;
            int num = 0;
            double count = 0;
            bool isStep = false;
            System.Collections.Generic.Dictionary<System.Int32, Ecell.Job.ExecuteParameter> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.Int32, Ecell.Job.ExecuteParameter> resultDictionary = null;
            resultDictionary = _unitUnderTest.RunSimParameterRange(topDir, modelName, num, count, isStep);
            Assert.AreEqual(expectedDictionary, resultDictionary, "RunSimParameterRange method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestCreateExecuteParameter()
        {
            Ecell.Job.ExecuteParameter expectedExecuteParameter = null;
            Ecell.Job.ExecuteParameter resultExecuteParameter = null;
            resultExecuteParameter = _unitUnderTest.CreateExecuteParameter();
            Assert.AreEqual(expectedExecuteParameter, resultExecuteParameter, "CreateExecuteParameter method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRunSimParameterMatrix()
        {
            string topDir = null;
            string modelName = null;
            double count = 0;
            bool isStep = false;
            System.Collections.Generic.Dictionary<System.Int32, Ecell.Job.ExecuteParameter> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.Int32, Ecell.Job.ExecuteParameter> resultDictionary = null;
            resultDictionary = _unitUnderTest.RunSimParameterMatrix(topDir, modelName, count, isStep);
            Assert.AreEqual(expectedDictionary, resultDictionary, "RunSimParameterMatrix method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
