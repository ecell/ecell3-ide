namespace EcellLib
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;


    [TestFixture()]
    public class TestPluginManager
    {

        private PluginManager _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new PluginManager();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorPluginManager()
        {
            PluginManager testPluginManager = new PluginManager();
            Assert.IsNotNull(testPluginManager, "Constructor of type, PluginManager failed to create instance.");
        }

        [Test()]
        public void TestFocusDataChanged()
        {
            string modelID = null;
            string key = null;
            PluginBase pbase = null;
            _unitUnderTest.FocusDataChanged(modelID, key, pbase);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestFocusClear()
        {
            _unitUnderTest.FocusClear();
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
        public void TestLogData()
        {
            string modelID = null;
            string key = null;
            string type = null;
            string propName = null;
            List<LogData> data = null;
            _unitUnderTest.LogData(modelID, key, type, propName, data);
            Assert.Fail("Create or modify test(s).");

        }

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

        [Test()]
        public void TestDataAdd()
        {
            List<EcellObject> data = null;
            _unitUnderTest.DataAdd(data);
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
            string paramID = null;
            _unitUnderTest.ParameterAdd(projectID, paramID);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestParameterDelete()
        {
            string projectID = null;
            string paramID = null;
            _unitUnderTest.ParameterDelete(projectID, paramID);
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
        public void TestLoadData()
        {
            string modelID = null;
            _unitUnderTest.LoadData(modelID);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAddPlugin()
        {
            PluginBase p = null;
            _unitUnderTest.AddPlugin(p);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestShowSelectPlugin()
        {
            _unitUnderTest.ShowSelectPlugin();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestPrint()
        {
            string pluginName = null;
            _unitUnderTest.Print(pluginName);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
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
        public void TestAdvancedTime()
        {
            double time = 0;
            _unitUnderTest.AdvancedTime(time);
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
        public void TestSaveModel()
        {
            string modelID = null;
            string path = null;
            _unitUnderTest.SaveModel(modelID, path);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestImageAdd()
        {
            string type = null;
            int imageIndex = 0;
            _unitUnderTest.ImageAdd(type, imageIndex);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetImageIndex()
        {
            string type = null;
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestLoadPlugin()
        {
            string path = null;
            string className = null;
            PluginBase expectedPluginBase = null;
            PluginBase resultPluginBase = _unitUnderTest.LoadPlugin(path, className);
            Assert.AreEqual(expectedPluginBase, resultPluginBase, "LoadPlugin method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestUnloadPlugin()
        {
            PluginBase p = null;
            _unitUnderTest.UnloadPlugin(p);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestCurrentToolBarMenu()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.CurrentToolBarMenu();
            Assert.AreEqual(expectedString, resultString, "CurrentToolBarMenu method returned unexpected result.");
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
        public void TestGetPlugin()
        {
            string name = null;
            PluginBase expectedPluginBase = null;
            PluginBase resultPluginBase = null;
            resultPluginBase = _unitUnderTest.GetPlugin(name);
            Assert.AreEqual(expectedPluginBase, resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetLayoutPlugins()
        {
            List<EcellLib.ILayoutAlgorithm> expectedList = null;
            List<EcellLib.ILayoutAlgorithm> resultList = _unitUnderTest.GetLayoutPlugins();
            Assert.AreEqual(expectedList, resultList, "GetLayoutPlugins method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetPluginVersionList()
        {
            Dictionary<System.String, System.String> expectedDictionary = null;
            Dictionary<System.String, System.String> resultDictionary = _unitUnderTest.GetPluginVersionList();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetPluginVersionList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetPluginManager()
        {
            PluginManager expectedPluginManager = PluginManager.GetPluginManager();
            PluginManager resultPluginManager = PluginManager.GetPluginManager();
            Assert.AreEqual(expectedPluginManager, resultPluginManager, "GetPluginManager method returned unexpected result.");
        }

        [Test()]
        public void TestSetPosition()
        {
            EcellObject data = null;
            _unitUnderTest.SetPosition(data);
            Assert.Fail("Create or modify test(s).");

        }
    }
}