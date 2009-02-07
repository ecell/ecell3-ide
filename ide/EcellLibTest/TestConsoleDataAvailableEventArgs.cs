namespace Ecell
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestConsoleDataAvailableEventArgs
    {

        private ConsoleDataAvailableEventArgs _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            string str = null;
            _unitUnderTest = new ConsoleDataAvailableEventArgs(str);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorConsoleDataAvailableEventArgs()
        {
            string str = null;
            ConsoleDataAvailableEventArgs testConsoleDataAvailableEventArgs = new ConsoleDataAvailableEventArgs(str);
            Assert.IsNotNull(testConsoleDataAvailableEventArgs, "Constructor of type, ConsoleDataAvailableEventArgs failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
