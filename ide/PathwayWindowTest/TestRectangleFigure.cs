namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestRectangleFigure
    {

        private RectangleFigure _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new RectangleFigure();
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
        public void TestConstructorRectangleFigure()
        {
            RectangleFigure testRectangleFigure = new RectangleFigure();
            Assert.IsNotNull(testRectangleFigure, "Constructor of type, RectangleFigure failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorRectangleFigureXYWidthHeight()
        {
            float x = 0;
            float y = 0;
            float width = 0;
            float height = 0;
            RectangleFigure testRectangleFigure = new RectangleFigure(x, y, width, height);
            Assert.IsNotNull(testRectangleFigure, "Constructor of type, RectangleFigure failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorRectangleFigureVars()
        {
            float[] vars = null;
            RectangleFigure testRectangleFigure = new RectangleFigure(vars);
            Assert.IsNotNull(testRectangleFigure, "Constructor of type, RectangleFigure failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreatePath()
        {
            float x = 0;
            float y = 0;
            float width = 0;
            float height = 0;
            System.Drawing.Drawing2D.GraphicsPath expectedGraphicsPath = null;
            System.Drawing.Drawing2D.GraphicsPath resultGraphicsPath = null;
            resultGraphicsPath = _unitUnderTest.CreatePath(x, y, width, height);
            Assert.AreEqual(expectedGraphicsPath, resultGraphicsPath, "CreatePath method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateSVGObject()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            string lineBrush = null;
            string fillBrush = null;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.CreateSVGObject(rect, lineBrush, fillBrush);
            Assert.AreEqual(expectedString, resultString, "CreateSVGObject method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
