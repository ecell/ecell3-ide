namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestProgressReportEventArgs
    {

        private ProgressReportEventArgs _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            int perc = 1;
            _unitUnderTest = new ProgressReportEventArgs(perc);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorProgressReportEventArgs()
        {
            int perc = 14;
            ProgressReportEventArgs testProgressReportEventArgs = new ProgressReportEventArgs(perc);
            Assert.IsNotNull(testProgressReportEventArgs, "Constructor of type, ProgressReportEventArgs failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
