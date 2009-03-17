namespace Ecell.IDE.Plugins.PathwayWindow
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Ecell.Objects;
    using Ecell.Logger;
    using System.Windows.Forms;

    /// <summary>
    /// Auto Generated TestCase of PathwayWindow
    /// </summary>
    [TestFixture()]
    public class TestPathwayWindow
    {

        private PathwayWindow _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [TestFixtureSetUp()]
        public void TestFixtureSetUp()
        {
            _unitUnderTest = new PathwayWindow();
        }
        /// <summary>
        /// 
        /// </summary>
        [TestFixtureTearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorPathwayWindow()
        {
            Assert.IsNotNull(_unitUnderTest, "Constructor of type, PathwayWindow failed to create instance.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetData()
        {
            string modelID = "project";
            List<Ecell.Objects.EcellObject> expectedList = null;
            List<Ecell.Objects.EcellObject> resultList = null;
            resultList = _unitUnderTest.GetData(modelID);
            Assert.AreEqual(expectedList, resultList, "GetData method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyDataAdd()
        {
            System.Collections.Generic.List<Ecell.Objects.EcellObject> list = null;
            bool isAnchor = false;
            _unitUnderTest.NotifyDataAdd(list, isAnchor);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyDataChanged()
        {
            string oldKey = null;
            Ecell.Objects.EcellObject eo = null;
            bool isRecorded = false;
            bool isAnchor = false;
            _unitUnderTest.NotifyDataChanged(oldKey, eo, isRecorded, isAnchor);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyDataMerge()
        {
            string modelID = null;
            string key = null;
            _unitUnderTest.NotifyDataMerge(modelID, key);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifySelectChanged()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.NotifySelectChanged(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNotifyAddSelect()
        {
            string modelID = null;
            string key = null;
            string type = null;
            _unitUnderTest.NotifyAddSelect(modelID, key, type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetMenuStripItems()
        {
            System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripMenuItem> expectedList = null;
            System.Collections.Generic.IEnumerable<System.Windows.Forms.ToolStripMenuItem> resultList = null;
            resultList = _unitUnderTest.GetMenuStripItems();
            Assert.AreEqual(expectedList, resultList, "GetMenuStripItems method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetToolBarMenuStripItems()
        {
            ToolStrip expectedList = null;
            ToolStrip resultList = null;
            resultList = _unitUnderTest.GetToolBarMenuStrip();
            Assert.AreEqual(expectedList, resultList, "GetToolBarMenuStripItems method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetWindowsForms()
        {
            IEnumerable<Ecell.EcellDockContent> expectedList = null;
            IEnumerable<Ecell.EcellDockContent> resultList = null;
            resultList = _unitUnderTest.GetWindowsForms();
            Assert.AreEqual(expectedList, resultList, "GetWindowsForms method returned unexpected result.");
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
        public void TestClear()
        {
            _unitUnderTest.Clear();
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
            EcellObject data = null;
            _unitUnderTest.DataChanged(modelID, key, type, data);
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
        public void TestLoggerAdd()
        {
            string modelID = null;
            string type = null;
            string key = null;
            string path = null;
            _unitUnderTest.LoggerAdd(new LoggerEntry(modelID, key, type, path));
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
        public void TestSetPosition()
        {
            Ecell.Objects.EcellObject data = null;
            _unitUnderTest.SetPosition(data);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
