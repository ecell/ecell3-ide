namespace Ecell.IDE.Plugins.PathwayWindow.Figure
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestFigureManager
    {

        private FigureManager _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new FigureManager();
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
        public void TestCreateFigure()
        {
            string type = null;
            string args = null;
            Ecell.IDE.Plugins.PathwayWindow.Figure.IFigure expectedIFigure = null;
            Ecell.IDE.Plugins.PathwayWindow.Figure.IFigure resultIFigure = null;
            resultIFigure = FigureManager.CreateFigure(type, args);
            Assert.AreEqual(expectedIFigure, resultIFigure, "CreateFigure method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetFigureList()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            resultList = FigureManager.GetFigureList();
            Assert.AreEqual(expectedList, resultList, "GetFigureList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
