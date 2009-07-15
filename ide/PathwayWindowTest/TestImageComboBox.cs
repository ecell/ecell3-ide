namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestImageComboBox
    {

        private ImageComboBox _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new ImageComboBox();
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
        public void TestConstructorImageComboBox()
        {
            ImageComboBox testImageComboBox = new ImageComboBox();
            Assert.IsNotNull(testImageComboBox, "Constructor of type, ImageComboBox failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
