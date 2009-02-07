namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestReportingSessionEventArgs
    {

        private ReportingSessionEventArgs _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.Reporting.ReportingSession session = null;
            _unitUnderTest = new ReportingSessionEventArgs(session);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorReportingSessionEventArgs()
        {
            Ecell.Reporting.ReportingSession session = null;
            ReportingSessionEventArgs testReportingSessionEventArgs = new ReportingSessionEventArgs(session);
            Assert.IsNotNull(testReportingSessionEventArgs, "Constructor of type, ReportingSessionEventArgs failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
