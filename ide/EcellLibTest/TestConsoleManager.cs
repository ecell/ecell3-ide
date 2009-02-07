namespace Ecell
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestConsoleManager
    {

        private ConsoleManager _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.ApplicationEnvironment env = null;
            _unitUnderTest = new ConsoleManager(env);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorConsoleManager()
        {
            Ecell.ApplicationEnvironment env = null;
            ConsoleManager testConsoleManager = new ConsoleManager(env);
            Assert.IsNotNull(testConsoleManager, "Constructor of type, ConsoleManager failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestFlush()
        {
            _unitUnderTest.Flush();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestWriteC()
        {
            char c = 'A';
            _unitUnderTest.Write(c);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestWriteBufIndexCount()
        {
            char[] buf = new char[] { };
            int index = 0;
            int count = 0;
            _unitUnderTest.Write(buf, index, count);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
