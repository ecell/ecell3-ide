namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestLineHandler
    {

        private LineHandler _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.IDE.Plugins.PathwayWindow.CanvasControl canvas = null;
            _unitUnderTest = new LineHandler(canvas);
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
        public void TestConstructorLineHandler()
        {
            Ecell.IDE.Plugins.PathwayWindow.CanvasControl canvas = null;
            LineHandler testLineHandler = new LineHandler(canvas);
            Assert.IsNotNull(testLineHandler, "Constructor of type, LineHandler failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSelectedLine()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayEdge line = null;
            _unitUnderTest.AddSelectedLine(line);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetSelectedLine()
        {
            _unitUnderTest.ResetSelectedLine();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetLinePosition()
        {
            _unitUnderTest.ResetLinePosition();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetLineVisibility()
        {
            bool visible = false;
            _unitUnderTest.SetLineVisibility(visible);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
