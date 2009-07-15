namespace Ecell.IDE.Plugins.PathwayWindow.Graphics
{
    using System;
    using NUnit.Framework;
    using System.Drawing;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestBrushManager
    {

        private BrushManager _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new BrushManager();
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
        public void TestGetBrushNameList()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            resultList = BrushManager.GetBrushNameList();
            Assert.AreEqual(expectedList, resultList, "GetBrushNameList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetBrushImageList()
        {
            System.Windows.Forms.ImageList expectedImageList = null;
            System.Windows.Forms.ImageList resultImageList = null;
            resultImageList = BrushManager.GetBrushImageList();
            Assert.AreEqual(expectedImageList, resultImageList, "GetBrushImageList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParseStringToBrush()
        {
            string color = null;
            System.Drawing.Brush expectedBrush = null;
            System.Drawing.Brush resultBrush = null;
            resultBrush = BrushManager.ParseStringToBrush(color);
            Assert.AreEqual(expectedBrush, resultBrush, "ParseStringToBrush method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParseBrushToString()
        {
            System.Drawing.Brush brush = null;
            string expectedString = null;
            string resultString = null;
            resultString = BrushManager.ParseBrushToString(brush);
            Assert.AreEqual(expectedString, resultString, "ParseBrushToString method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParseBrushToColor()
        {
            System.Drawing.Brush brush = null;
            System.Drawing.Color expectedColor = Color.AliceBlue;
            System.Drawing.Color resultColor = Color.AliceBlue;
            resultColor = BrushManager.ParseBrushToColor(brush);
            Assert.AreEqual(expectedColor, resultColor, "ParseBrushToColor method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateGradientBrush()
        {
            System.Drawing.Drawing2D.GraphicsPath path = null;
            System.Drawing.Brush centerBrush = null;
            System.Drawing.Brush fillBrush = null;
            System.Drawing.Drawing2D.PathGradientBrush expectedPathGradientBrush = null;
            System.Drawing.Drawing2D.PathGradientBrush resultPathGradientBrush = null;
            resultPathGradientBrush = BrushManager.CreateGradientBrush(path, centerBrush, fillBrush);
            Assert.AreEqual(expectedPathGradientBrush, resultPathGradientBrush, "CreateGradientBrush method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateBrushDictionary()
        {
            System.Collections.Generic.Dictionary<System.String, System.Drawing.Brush> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Drawing.Brush> resultDictionary = null;
            resultDictionary = BrushManager.CreateBrushDictionary();
            Assert.AreEqual(expectedDictionary, resultDictionary, "CreateBrushDictionary method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateColorDictionary()
        {
            System.Collections.Generic.Dictionary<System.String, System.Drawing.Color> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Drawing.Color> resultDictionary = null;
            resultDictionary = BrushManager.CreateColorDictionary();
            Assert.AreEqual(expectedDictionary, resultDictionary, "CreateColorDictionary method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
