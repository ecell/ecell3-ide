namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestFigureBase
    {

        private FigureBase _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new FigureBase();
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
        public void TestConstructorFigureBase()
        {
            FigureBase testFigureBase = new FigureBase();
            Assert.IsNotNull(testFigureBase, "Constructor of type, FigureBase failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorFigureBaseVars()
        {
            float[] vars = null;
            FigureBase testFigureBase = new FigureBase(vars);
            Assert.IsNotNull(testFigureBase, "Constructor of type, FigureBase failed to create instance.");
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
        public void TestGetContactPoint()
        {
            System.Drawing.PointF outerPoint = new System.Drawing.PointF();
            System.Drawing.PointF innerPoint = new System.Drawing.PointF();
            System.Drawing.PointF expectedPointF = new System.Drawing.PointF();
            System.Drawing.PointF resultPointF = new System.Drawing.PointF();
            resultPointF = _unitUnderTest.GetContactPoint(outerPoint, innerPoint);
            Assert.AreEqual(expectedPointF, resultPointF, "GetContactPoint method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateSVGObject()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF(); ;
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
