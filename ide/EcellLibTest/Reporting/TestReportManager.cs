namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestReportManager
    {

        private ReportManager _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.ApplicationEnvironment env = null;
            _unitUnderTest = new ReportManager(env);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorReportManager()
        {
            Ecell.ApplicationEnvironment env = null;
            ReportManager testReportManager = new ReportManager(env);
            Assert.IsNotNull(testReportManager, "Constructor of type, ReportManager failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetReportingSession()
        {
            string group = null;
            Ecell.Reporting.ReportingSession expectedReportingSession = null;
            Ecell.Reporting.ReportingSession resultReportingSession = null;
            resultReportingSession = _unitUnderTest.GetReportingSession(group);
            Assert.AreEqual(expectedReportingSession, resultReportingSession, "GetReportingSession method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetStatus()
        {
            Ecell.StatusBarMessageKind type = StatusBarMessageKind.QuickInspector;
            string text = null;
            _unitUnderTest.SetStatus(type, text);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetProgress()
        {
            int value = 10;
            _unitUnderTest.SetProgress(value);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
