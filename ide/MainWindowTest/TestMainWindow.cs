namespace EcellLib.MainWindow
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture()]
    public class TestMainWindow
    {

        private MainWindow _unitUnderTest;

        [TestFixtureSetUp()]
        public void TestFixtureSetUp()
        {
            _unitUnderTest = new MainWindow();
        }

        [TestFixtureTearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
        }

        [NUnit.Framework.Test()]
        public void TestConstructorMainWindow()
        {
            Assert.IsNotNull(_unitUnderTest, "Constructor of type, MainWindow failed to create instance.");
        }

        [Test()]
        public void TestLoadDefaultWindowSetting()
        {
            _unitUnderTest.LoadDefaultWindowSetting();
        }

        [Test()]
        public void TestLoadModelThread()
        {
            string modelID = null;
            _unitUnderTest.LoadModelThread(modelID);
            Assert.Fail("Create or modify test(s).");
        }

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

        [Test()]
        public void TestCheckWindowMenu()
        {
            string name = null;
            bool bChecked = false;
            _unitUnderTest.CheckWindowMenu(name, bChecked);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetMenuStripItems()
        {
            System.Collections.Generic.List<System.Windows.Forms.ToolStripMenuItem> expectedList = null;
            System.Collections.Generic.List<System.Windows.Forms.ToolStripMenuItem> resultList = null;
            resultList = _unitUnderTest.GetMenuStripItems();
            Assert.AreEqual(expectedList, resultList, "GetMenuStripItems method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetToolBarMenuStripItems()
        {
            System.Collections.Generic.List<System.Windows.Forms.ToolStripItem> expectedList = null;
            System.Collections.Generic.List<System.Windows.Forms.ToolStripItem> resultList = null;
            resultList = _unitUnderTest.GetToolBarMenuStripItems();
            Assert.AreEqual(expectedList, resultList, "GetToolBarMenuStripItems method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetWindowsForms()
        {
            System.Collections.Generic.List<EcellLib.EcellDockContent> expectedList = null;
            System.Collections.Generic.List<EcellLib.EcellDockContent> resultList = null;
            resultList = _unitUnderTest.GetWindowsForms();
            Assert.AreEqual(expectedList, resultList, "GetWindowsForms method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSelectChanged()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.SelectChanged(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAddSelect()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.AddSelect(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRemoveSelect()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.RemoveSelect(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestResetSelect()
        {
            _unitUnderTest.ResetSelect();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestDataAdd()
        {
            System.Collections.Generic.List<EcellLib.EcellObject> data = null;
            _unitUnderTest.DataAdd(data);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestDataChanged()
        {
            string modelID = null;
            string key = null;
            string type = null;
            EcellLib.EcellObject data = null;
            _unitUnderTest.DataChanged(modelID, key, type, data);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestLoggerAdd()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string path = null;
            _unitUnderTest.LoggerAdd(modelID, key, type, path);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestDataDelete()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.DataDelete(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestParameterAdd()
        {
            string projectID = null;
            string parameterID = null;
            _unitUnderTest.ParameterAdd(projectID, parameterID);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestParameterDelete()
        {
            string projectID = null;
            string parameterID = null;
            _unitUnderTest.ParameterDelete(projectID, parameterID);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestLogData()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string propName = null;
            System.Collections.Generic.List<EcellLib.LogData> data = null;
            _unitUnderTest.LogData(modelID, key, type, propName, data);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestWarnData()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string warntype = null;
            _unitUnderTest.WarnData(modelID, key, type, warntype);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestMessage()
        {
            string type = null;
            string message = null;
            _unitUnderTest.Message(type, message);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAdvancedTime()
        {
            double time = 0;
            _unitUnderTest.AdvancedTime(time);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestChangeStatus()
        {
            EcellLib.ProjectStatus type = ProjectStatus.Uninitialized;
            _unitUnderTest.ChangeStatus(type);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestChangeUndoStatus()
        {
            EcellLib.UndoStatus status = UndoStatus.NOTHING;
            _unitUnderTest.ChangeUndoStatus(status);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSaveModel()
        {
            string modelID = null;
            string directory = null;
            _unitUnderTest.SaveModel(modelID, directory);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestPrint()
        {
            System.Drawing.Bitmap expectedBitmap = null;
            System.Drawing.Bitmap resultBitmap = null;
            resultBitmap = _unitUnderTest.Print("");
            Assert.AreEqual(expectedBitmap, resultBitmap, "Print method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetPluginName()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetPluginName();
            Assert.AreEqual(expectedString, resultString, "GetPluginName method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetVersionString()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetVersionString();
            Assert.AreEqual(expectedString, resultString, "GetVersionString method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestIsMessageWindow()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsMessageWindow();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsMessageWindow method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestIsEnablePrint()
        {
            List<string> expectedList = null;
            List<string> resultList = null;
            resultList = _unitUnderTest.GetEnablePrintNames();
            Assert.AreEqual(expectedList, resultList, "IsEnablePrint method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetPosition()
        {
            EcellLib.EcellObject data = null;
            _unitUnderTest.SetPosition(data);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNewProject()
        {
            object sender = null;
            System.EventArgs e = null;
            _unitUnderTest.NewProject(sender, e);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNewProjectCancel()
        {
            object sender = null;
            System.EventArgs e = null;
            _unitUnderTest.NewProjectCancel(sender, e);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestOpenProjectCancel()
        {
            object sender = null;
            System.EventArgs e = null;
            _unitUnderTest.OpenProjectCancel(sender, e);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestOpenProject()
        {
            object sender = null;
            System.EventArgs e = null;
            _unitUnderTest.OpenProject(sender, e);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestLoadModel()
        {
            string path = null;
            _unitUnderTest.LoadModel(path);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestExportModelCancel()
        {
            object sender = null;
            System.EventArgs e = null;
            _unitUnderTest.ExportModelCancel(sender, e);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestExportModel()
        {
            object sender = null;
            System.EventArgs e = null;
            _unitUnderTest.ExportModel(sender, e);
            Assert.Fail("Create or modify test(s).");

        }

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