namespace Ecell.IDE.MainWindow
{
    using System;
    using NUnit.Framework;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestSelectDirectory
    {

        private SelectDirectory _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new SelectDirectory();
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
        public void TestShowDialog()
        {
            DialogResult expectedDialogResult = DialogResult.Cancel;
            DialogResult resultDialogResult = _unitUnderTest.ShowDialog();
            Assert.AreEqual(expectedDialogResult, resultDialogResult, "ShowDialog method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
