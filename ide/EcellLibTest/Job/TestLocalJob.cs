namespace Ecell.Job
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestLocalJob
    {

        private LocalJob _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new LocalJob();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorLocalJob()
        {
            LocalJob testLocalJob = new LocalJob();
            Assert.IsNotNull(testLocalJob, "Constructor of type, LocalJob failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void Testretry()
        {
            _unitUnderTest.retry();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void Testrun()
        {
            _unitUnderTest.run();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void Teststop()
        {
            _unitUnderTest.stop();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestUpdate()
        {
            _unitUnderTest.Update();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetStdOut()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdOut();
            Assert.AreEqual(expectedString, resultString, "GetStdOut method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetStdErr()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetStdErr();
            Assert.AreEqual(expectedString, resultString, "GetStdErr method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestPrepareProcess()
        {
            _unitUnderTest.PrepareProcess();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetLogData()
        {
            string key = null;
            System.Collections.Generic.Dictionary<System.Double, System.Double> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.Double, System.Double> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetLogData(key);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetLogData method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetDefaultScript()
        {
            string expectedString = null;
            string resultString = null;
            resultString = LocalJob.GetDefaultScript();
            Assert.AreEqual(expectedString, resultString, "GetDefaultScript method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
