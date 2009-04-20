namespace Ecell.IDE.Plugins.PathwayWindow.Components
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestComponentManager
    {

        private ComponentManager _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ComponentManager();
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
        public void TestConstructorComponentManager()
        {
            ComponentManager testComponentManager = new ComponentManager();
            Assert.IsNotNull(testComponentManager, "Constructor of type, ComponentManager failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadComponentSettings()
        {
            string filename = null;
            _unitUnderTest.LoadComponentSettings(filename);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveComponentSettings()
        {
            _unitUnderTest.SaveComponentSettings();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDefaultComponentSetting()
        {
            string type = null;
            Ecell.IDE.Plugins.PathwayWindow.Components.ComponentSetting expectedComponentSetting = null;
            Ecell.IDE.Plugins.PathwayWindow.Components.ComponentSetting resultComponentSetting = null;
            resultComponentSetting = _unitUnderTest.GetDefaultComponentSetting(type);
            Assert.AreEqual(expectedComponentSetting, resultComponentSetting, "GetDefaultComponentSetting method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
