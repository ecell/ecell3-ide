namespace Ecell.IDE.Plugins.PathwayWindow
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestDuplicateKeyException
    {

        private DuplicateKeyException _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            string message = null;
            _unitUnderTest = new DuplicateKeyException(message);
        }
        /// <summary>
        /// 
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorDuplicateKeyException()
        {
            string message = null;
            DuplicateKeyException testDuplicateKeyException = new DuplicateKeyException(message);
            Assert.IsNotNull(testDuplicateKeyException, "Constructor of type, DuplicateKeyException failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
