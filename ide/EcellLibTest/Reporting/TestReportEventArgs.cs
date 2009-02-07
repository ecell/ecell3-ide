namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestReportEventArgs
    {

        private ReportEventArgs _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.Reporting.IReport report = null;
            _unitUnderTest = new ReportEventArgs(report);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorReportEventArgs()
        {
            Ecell.Reporting.IReport report = null;
            ReportEventArgs testReportEventArgs = new ReportEventArgs(report);
            Assert.IsNotNull(testReportEventArgs, "Constructor of type, ReportEventArgs failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
