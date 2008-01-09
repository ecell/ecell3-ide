namespace EcellLib.MainWindow
{
    using System;
    using NUnit.Framework;
    using System.Windows.Forms;


    [TestFixture()]
    public class TestSelectDirectory
    {

        private SelectDirectory _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new SelectDirectory();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

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
