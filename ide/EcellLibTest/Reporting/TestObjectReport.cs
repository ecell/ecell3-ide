namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestObjectReport
    {

        private ObjectReport _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.MessageType type = MessageType.Debug;
            string message = null;
            string group = null;
            Ecell.Objects.EcellObject obj = null;
            _unitUnderTest = new ObjectReport(type, message, group, obj);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorObjectReport()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = null;
            string group = null;
            Ecell.Objects.EcellObject obj = null;
            ObjectReport testObjectReport = new ObjectReport(type, message, group, obj);
            Assert.IsNotNull(testObjectReport, "Constructor of type, ObjectReport failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
