namespace Ecell.Job
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestExecuteParameter
    {

        private ExecuteParameter _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ExecuteParameter();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorExecuteParameter()
        {
            ExecuteParameter testExecuteParameter = new ExecuteParameter();
            Assert.IsNotNull(testExecuteParameter, "Constructor of type, ExecuteParameter failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestConstructorExecuteParameterData()
        {
            System.Collections.Generic.Dictionary<System.String, System.Double> data = null;
            ExecuteParameter testExecuteParameter = new ExecuteParameter(data);
            Assert.IsNotNull(testExecuteParameter, "Constructor of type, ExecuteParameter failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAddParameter()
        {
            string path = null;
            double value = 0;
            _unitUnderTest.AddParameter(path, value);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetParameter()
        {
            string path = null;
            double expectedDouble = 0;
            double resultDouble = 0;
            resultDouble = _unitUnderTest.GetParameter(path);
            Assert.AreEqual(expectedDouble, resultDouble, "GetParameter method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRemoveParameter()
        {
            string path = null;
            _unitUnderTest.RemoveParameter(path);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
