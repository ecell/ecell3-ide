namespace Ecell.IDE.Plugins.PathwayWindow.Exceptions
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestPathwayException
    {

        private PathwayException _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new PathwayException();
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
        public void TestConstructorPathwayException()
        {
            PathwayException testPathwayException = new PathwayException();
            Assert.IsNotNull(testPathwayException, "Constructor of type, PathwayException failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorPathwayExceptionMessage()
        {
            string message = null;
            PathwayException testPathwayException = new PathwayException(message);
            Assert.IsNotNull(testPathwayException, "Constructor of type, PathwayException failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorPathwayExceptionMessageE()
        {
            string message = null;
            System.Exception e = null;
            PathwayException testPathwayException = new PathwayException(message, e);
            Assert.IsNotNull(testPathwayException, "Constructor of type, PathwayException failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
