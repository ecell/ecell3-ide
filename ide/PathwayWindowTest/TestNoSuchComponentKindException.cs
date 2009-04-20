namespace Ecell.IDE.Plugins.PathwayWindow.Exceptions
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestNoSuchComponentKindException
    {

        private NoSuchComponentKindException _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            string message = null;
            _unitUnderTest = new NoSuchComponentKindException(message);
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
        public void TestConstructorNoSuchComponentKindException()
        {
            string message = null;
            NoSuchComponentKindException testNoSuchComponentKindException = new NoSuchComponentKindException(message);
            Assert.IsNotNull(testNoSuchComponentKindException, "Constructor of type, NoSuchComponentKindException failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
