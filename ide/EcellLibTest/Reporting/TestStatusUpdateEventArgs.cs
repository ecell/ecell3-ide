namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestStatusUpdateEventArgs
    {

        private StatusUpdateEventArgs _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.StatusBarMessageKind type = StatusBarMessageKind.Generic;
            string text = null;
            _unitUnderTest = new StatusUpdateEventArgs(type, text);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorStatusUpdateEventArgs()
        {
            Ecell.StatusBarMessageKind type = StatusBarMessageKind.QuickInspector;
            string text = null;
            StatusUpdateEventArgs testStatusUpdateEventArgs = new StatusUpdateEventArgs(type, text);
            Assert.IsNotNull(testStatusUpdateEventArgs, "Constructor of type, StatusUpdateEventArgs failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
