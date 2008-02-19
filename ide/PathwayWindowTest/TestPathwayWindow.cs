namespace EcellLib.PathwayWindow
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;

    /// <summary>
    /// Auto Generated TestCase of PathwayWindow
    /// </summary>
    [TestFixture()]
    public class TestPathwayWindow
    {

        private PathwayWindow _unitUnderTest;

        [TestFixtureSetUp()]
        public void TestFixtureSetUp()
        {
            _unitUnderTest = new PathwayWindow();
        }

        [TestFixtureTearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorPathwayWindow()
        {
            Assert.IsNotNull(_unitUnderTest, "Constructor of type, PathwayWindow failed to create instance.");
        }

        [Test()]
        public void TestGetData()
        {
            string modelID = "project";
            List<EcellLib.EcellObject> expectedList = null;
            List<EcellLib.EcellObject> resultList = null;
            resultList = _unitUnderTest.GetData(modelID);
            Assert.AreEqual(expectedList, resultList, "GetData method returned unexpected result.");
        }

        [Test()]
        public void TestGetEcellObject()
        {
            string modelID = "";
            string key = "";
            string type = "";
            EcellObject expectedEcellObject = null;
            EcellObject resultEcellObject = _unitUnderTest.GetEcellObject(modelID, key, type);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "GetEcellObject method returned unexpected result.");

        }

        [Test()]
        public void TestNotifyDataAdd()
        {
            System.Collections.Generic.List<EcellLib.EcellObject> list = null;
            bool isAnchor = false;
            _unitUnderTest.NotifyDataAdd(list, isAnchor);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNotifyLoggerAdd()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string entityPath = null;
            _unitUnderTest.NotifyLoggerAdd(modelID, key, type, entityPath);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNotifyDataChanged()
        {
            string oldKey = null;
            EcellLib.EcellObject eo = null;
            bool isRecorded = false;
            bool isAnchor = false;
            _unitUnderTest.NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNotifyDataDelete()
        {
            string modelID = null;
            string key = null;
            string type = null;
            bool isAnchor = false;
            _unitUnderTest.NotifyDataDelete(modelID, key, type, isAnchor);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNotifyDataMerge()
        {
            string modelID = null;
            string key = null;
            _unitUnderTest.NotifyDataMerge(modelID, key);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNotifySelectChanged()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.NotifySelectChanged(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestNotifyAddSelect()
        {
            string modelID = null;
            string key = null;
            string type = null;
            bool isSelected = false;
            _unitUnderTest.NotifyAddSelect(modelID, key, type, isSelected);
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
        public void TestClear()
        {
            _unitUnderTest.Clear();
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
        public void TestIsEnablePrint()
        {
            List<string> expectedList = null;
            List<string> resultList = null;
            resultList = _unitUnderTest.GetEnablePrintNames();
            Assert.AreEqual(expectedList, resultList, "IsEnablePrint method returned unexpected result.");
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
        public void TestLogData()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string propName = null;
            System.Collections.Generic.List<EcellLib.LogData> log = null;
            _unitUnderTest.LogData(modelID, key, type, propName, log);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestLoggerAdd()
        {
            string modelID = null;
            string type = null;
            string key = null;
            string path = null;
            _unitUnderTest.LoggerAdd(modelID, type, key, path);
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
        public void TestPrint()
        {
            System.Drawing.Bitmap expectedBitmap = null;
            System.Drawing.Bitmap resultBitmap = null;
            resultBitmap = _unitUnderTest.Print("");
            Assert.AreEqual(expectedBitmap, resultBitmap, "Print method returned unexpected result.");
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
        public void TestGetPluginName()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetPluginName();
            Assert.AreEqual(expectedString, resultString, "GetPluginName method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetTemporaryID()
        {
            string modelID = null;
            string type = null;
            string systemID = null;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetTemporaryID method returned unexpected result.");
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
        public void TestSetPosition()
        {
            EcellLib.EcellObject data = null;
            _unitUnderTest.SetPosition(data);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetLayoutAlgorithms()
        {
            System.Collections.Generic.List<EcellLib.ILayoutAlgorithm> expectedList = null;
            System.Collections.Generic.List<EcellLib.ILayoutAlgorithm> resultList = null;
            resultList = _unitUnderTest.GetLayoutAlgorithms();
            Assert.AreEqual(expectedList, resultList, "GetLayoutAlgorithms method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}