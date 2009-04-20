namespace Ecell.IDE.Plugins.PathwayWindow.Graphic
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestGraphicsExporter
    {

        private GraphicsExporter _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new GraphicsExporter();
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
        public void TestExportSVG()
        {
            Ecell.IDE.Plugins.PathwayWindow.CanvasControl canvas = null;
            string filename = null;
            GraphicsExporter.ExportSVG(canvas, filename);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
