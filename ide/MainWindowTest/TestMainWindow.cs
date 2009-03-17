namespace Ecell.IDE.MainWindow
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Ecell.IDE.MainWindow;
    using Ecell.Logger;

    /// <summary>
    /// Test code for MainWindow
    /// </summary>
    [TestFixture()]
    public class TestMainWindow
    {

        private MainWindow _unitUnderTest;
        /// <summary>
        /// SetUp
        /// </summary>
        [TestFixtureSetUp()]
        public void TestFixtureSetUp()
        {
            _unitUnderTest = new MainWindow();
        }
        /// <summary>
        /// TearDown
        /// </summary>
        [TestFixtureTearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
        }
        /// <summary>
        /// 
        /// </summary>
        [NUnit.Framework.Test()]
        public void TestConstructorMainWindow()
        {
            Assert.IsNotNull(_unitUnderTest, "Constructor of type, MainWindow failed to create instance.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadDefaultWindowSetting()
        {
            _unitUnderTest.LoadDefaultWindowSetting();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDockContent()
        {
            string name = null;
            WeifenLuo.WinFormsUI.Docking.DockContent expectedDockContent = null;
            WeifenLuo.WinFormsUI.Docking.DockContent resultDockContent = null;
            resultDockContent = _unitUnderTest.GetDockContent(name);
            Assert.AreEqual(expectedDockContent, resultDockContent, "GetDockContent method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSelectChanged()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.SelectChanged(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSelect()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.AddSelect(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRemoveSelect()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.RemoveSelect(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetSelect()
        {
            _unitUnderTest.ResetSelect();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataAdd()
        {
            System.Collections.Generic.List<Ecell.Objects.EcellObject> data = null;
            _unitUnderTest.DataAdd(data);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChanged()
        {
            string modelID = null;
            string key = null;
            string type = null;
            Ecell.Objects.EcellObject data = null;
            _unitUnderTest.DataChanged(modelID, key, type, data);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoggerAdd()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string path = null;
            _unitUnderTest.LoggerAdd(new LoggerEntry(modelID, key, type, path));
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDelete()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.DataDelete(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParameterAdd()
        {
            string projectID = null;
            string parameterID = null;
            _unitUnderTest.ParameterAdd(projectID, parameterID);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParameterDelete()
        {
            string projectID = null;
            string parameterID = null;
            _unitUnderTest.ParameterDelete(projectID, parameterID);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLogData()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string propName = null;
            System.Collections.Generic.List<Ecell.LogData> data = null;
            _unitUnderTest.LogData(modelID, key, type, propName, data);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAdvancedTime()
        {
            double time = 0;
            _unitUnderTest.AdvancedTime(time);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestChangeStatus()
        {
            Ecell.ProjectStatus type = ProjectStatus.Uninitialized;
            _unitUnderTest.ChangeStatus(type);
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveModel()
        {
            string modelID = null;
            string directory = null;
            _unitUnderTest.SaveModel(modelID, directory);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPrint()
        {
            System.Drawing.Bitmap expectedBitmap = null;
            System.Drawing.Bitmap resultBitmap = null;
            resultBitmap = _unitUnderTest.Print("");
            Assert.AreEqual(expectedBitmap, resultBitmap, "Print method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetPluginName()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetPluginName();
            Assert.AreEqual(expectedString, resultString, "GetPluginName method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetVersionString()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetVersionString();
            Assert.AreEqual(expectedString, resultString, "GetVersionString method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsMessageWindow()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsMessageWindow();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsMessageWindow method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsEnablePrint()
        {
            IEnumerable<string> expectedList = null;
            IEnumerable<string> resultList = null;
            resultList = _unitUnderTest.GetEnablePrintNames();
            Assert.AreEqual(expectedList, resultList, "IsEnablePrint method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetProcessWorkingSetSize()
        {
            System.IntPtr hwnd = IntPtr.Zero;
            int min = 0;
            int max = 0;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = MainWindow.SetProcessWorkingSetSize(hwnd, min, max);
            Assert.AreEqual(expectedBoolean, resultBoolean, "SetProcessWorkingSetSize method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
