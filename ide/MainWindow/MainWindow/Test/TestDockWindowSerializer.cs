namespace EcellLib.MainWindow
{
    using System;
    using NUnit.Framework;
    using System.IO;
    using System.Windows.Forms;


    [TestFixture()]
    public class TestDockWindowSerializer
    {
        MainWindow _window;
        private DockWindowSerializer _unitUnderTest;
        private string _modelFile;

        [TestFixtureSetUp()]
        public void SetUp()
        {
            _modelFile = Path.Combine(Util.GetUserDir(), "TestModel.xml");
            _window = new MainWindow();
            _unitUnderTest = new DockWindowSerializer();
        }

        [TestFixtureTearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestSaveAsXML()
        {
            DockWindowSerializer.SaveAsXML(_window, _modelFile);
            Assert.IsNotNull(_window);
        }

        [Test()]
        public void TestLoadFromXML()
        {
            DockWindowSerializer.LoadFromXML(_window, _modelFile);
            Assert.IsNotNull(_window);
        }
    }
}
