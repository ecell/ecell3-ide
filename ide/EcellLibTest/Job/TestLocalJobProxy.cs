namespace Ecell.Job
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestLocalJobProxy
    {

        private LocalJobProxy _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new LocalJobProxy();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorLocalJobProxy()
        {
            LocalJobProxy testLocalJobProxy = new LocalJobProxy();
            Assert.IsNotNull(testLocalJobProxy, "Constructor of type, LocalJobProxy failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestCreateJob()
        {
            Ecell.Job.Job expectedJob = null;
            Ecell.Job.Job resultJob = null;
            resultJob = _unitUnderTest.CreateJob();
            Assert.AreEqual(expectedJob, resultJob, "CreateJob method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestUpdate()
        {
            _unitUnderTest.Update();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestIsIDE()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsIDE();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsIDE method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> list = null;
            _unitUnderTest.SetProperty(list);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetDefaultProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Object> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetDefaultProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetDefaultProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetProperty()
        {
            System.Collections.Generic.Dictionary<System.String, System.Object> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Object> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetDefaultScript()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetDefaultScript();
            Assert.AreEqual(expectedString, resultString, "GetDefaultScript method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
