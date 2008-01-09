namespace EcellLib.MainWindow
{
    using System;
    using NUnit.Framework;
    using System.IO;
    using System.Windows.Forms;


    [TestFixture()]
    public class TestDockWindowSerializer
    {

        private DockWindowSerializer _unitUnderTest;
        private string _modelFile;
        [SetUp()]
        public void SetUp()
        {
            string foldername = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "E-Cell IDE");
            _modelFile = Path.Combine(foldername, "TestModel.xml");
            _unitUnderTest = new DockWindowSerializer();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestSaveAsXML()
        {
            EcellLib.MainWindow.MainWindow window = new MainWindow();
            DockWindowSerializer.SaveAsXML(window, _modelFile);
            Assert.IsNotNull(window);
        }

        [Test()]
        public void TestLoadFromXML()
        {
            EcellLib.MainWindow.MainWindow window = new MainWindow();
            DockWindowSerializer.LoadFromXML(window, _modelFile);
            Assert.IsNotNull(window);
        }
    }
}
