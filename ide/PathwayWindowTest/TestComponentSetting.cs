namespace Ecell.IDE.Plugins.PathwayWindow.Components
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestComponentSetting
    {

        private ComponentSetting _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ComponentSetting();
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
        public void TestRaisePropertyChange()
        {
            //_unitUnderTest.RaisePropertyChange();
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateTemplate()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject expectedPPathwayObject = null;
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayObject resultPPathwayObject = null;
            resultPPathwayObject = _unitUnderTest.CreateTemplate();
            Assert.AreEqual(expectedPPathwayObject, resultPPathwayObject, "CreateTemplate method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
