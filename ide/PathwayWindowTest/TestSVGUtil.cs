namespace Ecell.IDE.Plugins.PathwayWindow.Graphic
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestSVGUtil
    {

        private SVGUtil _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new SVGUtil();
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
        public void TestRectangle()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            string lineBrush = null;
            string fillBrush = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Rectangle(rect, lineBrush, fillBrush);
            Assert.AreEqual(expectedString, resultString, "Rectangle method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRoundedRectangleRectLineBrushFillBrush()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            string lineBrush = null;
            string fillBrush = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.RoundedRectangle(rect, lineBrush, fillBrush);
            Assert.AreEqual(expectedString, resultString, "RoundedRectangle method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRoundedRectangleRectLineBrushFillBrushRxRy()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            string lineBrush = null;
            string fillBrush = null;
            float rx = 0;
            float ry = 0;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.RoundedRectangle(rect, lineBrush, fillBrush, rx, ry);
            Assert.AreEqual(expectedString, resultString, "RoundedRectangle method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSystemRectangle()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            string lineBrush = null;
            string fillBrush = null;
            float rOut = 0;
            float rIn = 0;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.SystemRectangle(rect, lineBrush, fillBrush, rOut, rIn);
            Assert.AreEqual(expectedString, resultString, "SystemRectangle method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestEllipse()
        {
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF();
            string lineBrush = null;
            string fillBrush = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Ellipse(rect, lineBrush, fillBrush);
            Assert.AreEqual(expectedString, resultString, "Ellipse method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLineStartEnd()
        {
            System.Drawing.PointF start = new System.Drawing.PointF();
            System.Drawing.PointF end = new System.Drawing.PointF();
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Line(start, end);
            Assert.AreEqual(expectedString, resultString, "Line method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLineStartEndBrush()
        {
            System.Drawing.PointF start = new System.Drawing.PointF();
            System.Drawing.PointF end = new System.Drawing.PointF();
            string brush = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Line(start, end, brush);
            Assert.AreEqual(expectedString, resultString, "Line method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLineStartEndBrushWidth()
        {
            System.Drawing.PointF start = new System.Drawing.PointF();
            System.Drawing.PointF end = new System.Drawing.PointF();
            string brush = null;
            string width = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Line(start, end, brush, width);
            Assert.AreEqual(expectedString, resultString, "Line method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDashedLineStartEnd()
        {
            System.Drawing.PointF start = new System.Drawing.PointF();
            System.Drawing.PointF end = new System.Drawing.PointF();
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.DashedLine(start, end);
            Assert.AreEqual(expectedString, resultString, "DashedLine method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDashedLineStartEndBrush()
        {
            System.Drawing.PointF start = new System.Drawing.PointF();
            System.Drawing.PointF end = new System.Drawing.PointF();
            string brush = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.DashedLine(start, end, brush);
            Assert.AreEqual(expectedString, resultString, "DashedLine method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDashedLineStartEndBrushWidth()
        {
            System.Drawing.PointF start = new System.Drawing.PointF();
            System.Drawing.PointF end = new System.Drawing.PointF();
            string brush = null;
            string width = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.DashedLine(start, end, brush, width);
            Assert.AreEqual(expectedString, resultString, "DashedLine method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPolygon()
        {
            System.Drawing.PointF[] points = null;
            string brush = null;
            string width = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Polygon(points, brush, width);
            Assert.AreEqual(expectedString, resultString, "Polygon method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestTextPointTextBrush()
        {
            System.Drawing.PointF point = new System.Drawing.PointF();
            string text = null;
            string brush = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Text(point, text, brush);
            Assert.AreEqual(expectedString, resultString, "Text method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestTextPointTextBrushWeightSize()
        {
            System.Drawing.PointF point = new System.Drawing.PointF();
            string text = null;
            string brush = null;
            string weight = null;
            float size = 0;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.Text(point, text, brush, weight, size);
            Assert.AreEqual(expectedString, resultString, "Text method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGradationBrush()
        {
            string name = null;
            string centerBrush = null;
            string roundBrush = null;
            string expectedString = null;
            string resultString = null;
            resultString = SVGUtil.GradationBrush(name, centerBrush, roundBrush);
            Assert.AreEqual(expectedString, resultString, "GradationBrush method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
