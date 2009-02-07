namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestObjectPropertyReport
    {

        private ObjectPropertyReport _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.MessageType type = MessageType.Warning;
            string message = null;
            string group = null;
            Ecell.Objects.EcellObject obj = null;
            string propertyName = null;
            _unitUnderTest = new ObjectPropertyReport(type, message, group, obj, propertyName);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorObjectPropertyReport()
        {
            Ecell.MessageType type = MessageType.Debug;
            string message = null;
            string group = null;
            Ecell.Objects.EcellObject obj = null;
            string propertyName = null;
            ObjectPropertyReport testObjectPropertyReport = new ObjectPropertyReport(type, message, group, obj, propertyName);
            Assert.IsNotNull(testObjectPropertyReport, "Constructor of type, ObjectPropertyReport failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
