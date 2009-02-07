namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestCompileReport
    {

        private CompileReport _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.MessageType type = MessageType.Information;
            string message = null;
            string group = null;
            _unitUnderTest = new CompileReport(type, message, group);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorCompileReport()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = null;
            string group = null;
            CompileReport testCompileReport = new CompileReport(type, message, group);
            Assert.IsNotNull(testCompileReport, "Constructor of type, CompileReport failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
