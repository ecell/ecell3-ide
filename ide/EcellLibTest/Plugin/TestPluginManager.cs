//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
using System;
using NUnit.Framework;
using System.Collections.Generic;
using Ecell;
using Ecell.Objects;
using Ecell.Logging;
using Ecell.Logger;
using System.IO;
using System.Diagnostics;
using System.Collections;
using Ecell.Exceptions;

using WeifenLuo.WinFormsUI.Docking;

namespace Ecell.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestPluginManager
    {
        internal class TestMainWindow : IRootMenuProvider, IDockOwner, IEcellPlugin, IDataHandler
        {
            public System.Windows.Forms.ToolStripMenuItem GetRootMenuItem(string name)
            {
                return null;
            }

            public virtual DockPanel DockPanel
            {
                get { return new DockPanel(); }
            }

            public string GetPluginName()
            {
                return "TestMainWindow";
            }

            public String GetVersionString()
            {
                return "AAA";
            }

            public System.Xml.XmlNode GetPluginStatus()
            {
                return null;
            }

            public void SetPluginStatus(System.Xml.XmlNode status)
            {
            }

            public List<IPropertyItem> GetPropertySettings()
            {
                List<IPropertyItem> result = new List<IPropertyItem>();
                return result;
            }

            public void ChangeStatus(ProjectStatus status)
            { }

            public Dictionary<string, Delegate> GetPublicDelegate()
            {
                Dictionary<string, Delegate> list = new Dictionary<string, Delegate>();
                return list;
            }

            public void Initialize()
            { }

            public virtual ApplicationEnvironment Environment
            {
                get { return null; }
                set { }
            }

            public void DataAdd(List<EcellObject> data)
            { }

            public void DataChanged(string modelID, string key, string type, EcellObject data)
            { }

            public void DataDelete(string modelID, string key, string type)
            { }

            public void ResetSelect()
            { }

            public void RemoveSelect(string modelID, string key, string type)
            { }

            public void AddSelect(string modelID, string key, string type)
            { }

            public void SelectChanged(string modelID, string key, string type)
            { }

            /// <summary>
            /// The event sequence when the simulation parameter is added.
            /// </summary>
            /// <param name="projectID">The current project ID.</param>
            /// <param name="parameterID">The added parameter ID.</param>
            public void ParameterAdd(string projectID, string parameterID)
            {
                // nothing
            }

            /// <summary>
            /// The event sequence when the simulation parameter is deleted.
            /// </summary>
            /// <param name="projectID">The current project ID.</param>
            /// <param name="parameterID">The deleted parameter ID.</param>
            public void ParameterDelete(string projectID, string parameterID)
            {
                // nothing
            }

            /// <summary>
            /// The event sequence when the simulation parameter is set.
            /// </summary>
            /// <param name="projectID">The current project ID.</param>
            /// <param name="parameterID">The deleted parameter ID.</param>
            public void ParameterSet(string projectID, string parameterID)
            {
                // nothing
            }

            public void ParameterUpdate(string projectID, string parameterID)
            { }

            public void LoggerAdd(LoggerEntry entry)
            { }

            public void AdvancedTime(double time)
            { }

            public void Clear()
            { }

            public void SaveModel(string modelID, string directory)
            { }
        }

        internal class TestMainWindow2 : IRootMenuProvider, IEcellPlugin, IDataHandler
        {
            public System.Windows.Forms.ToolStripMenuItem GetRootMenuItem(string name)
            {
                return null;
            }

            public virtual DockPanel DockPanel
            {
                get { return null; }
            }

            public string GetPluginName()
            {
                return "TestMainWindow2";
            }

            public String GetVersionString()
            {
                return "AAA";
            }

            public System.Xml.XmlNode GetPluginStatus()
            {
                return null;
            }

            public void SetPluginStatus(System.Xml.XmlNode status)
            {
            }

            public List<IPropertyItem> GetPropertySettings()
            {
                List<IPropertyItem> result = new List<IPropertyItem>();
                return result;
            }

            public void ChangeStatus(ProjectStatus status)
            { }

            public Dictionary<string, Delegate> GetPublicDelegate()
            {
                Dictionary<string, Delegate> list = new Dictionary<string, Delegate>();
                return list;
            }

            public void Initialize()
            { }

            public virtual ApplicationEnvironment Environment
            {
                get { return null; }
                set { }
            }

            public void DataAdd(List<EcellObject> data)
            { }

            public void DataChanged(string modelID, string key, string type, EcellObject data)
            { }

            public void DataDelete(string modelID, string key, string type)
            { }

            public void ResetSelect()
            { }

            public void RemoveSelect(string modelID, string key, string type)
            { }

            public void AddSelect(string modelID, string key, string type)
            { }

            public void SelectChanged(string modelID, string key, string type)
            { }

            /// <summary>
            /// The event sequence when the simulation parameter is added.
            /// </summary>
            /// <param name="projectID">The current project ID.</param>
            /// <param name="parameterID">The added parameter ID.</param>
            public void ParameterAdd(string projectID, string parameterID)
            {
                // nothing
            }

            /// <summary>
            /// The event sequence when the simulation parameter is deleted.
            /// </summary>
            /// <param name="projectID">The current project ID.</param>
            /// <param name="parameterID">The deleted parameter ID.</param>
            public void ParameterDelete(string projectID, string parameterID)
            {
                // nothing
            }

            /// <summary>
            /// The event sequence when the simulation parameter is set.
            /// </summary>
            /// <param name="projectID">The current project ID.</param>
            /// <param name="parameterID">The deleted parameter ID.</param>
            public void ParameterSet(string projectID, string parameterID)
            {
                // nothing
            }

            public void ParameterUpdate(string projectID, string parameterID)
            { }

            public void LoggerAdd(LoggerEntry entry)
            { }

            public void AdvancedTime(double time)
            { }

            public void Clear()
            { }

            public void SaveModel(string modelID, string directory)
            { }
        }

        private static PluginManager _unitUnderTest;
        private static ApplicationEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = _env.PluginManager;

            // Load plugins
            foreach (string pluginDir in Util.GetPluginDirs())
            {
                string[] files = Directory.GetFiles(
                    pluginDir,
                    Constants.delimiterWildcard + Constants.FileExtPlugin);
                foreach (string fileName in files)
                {
                    _env.PluginManager.LoadPlugin(fileName);
                }
            }
            
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
        /// TestConstructorPluginManager
        /// </summary>
        [Test()]
        public void TestConstructorPluginManager()
        {
            Assert.IsNotNull(_unitUnderTest, "Constructor of type, PluginManager failed to create instance.");

            _unitUnderTest.AppVersion = new Version();
            Assert.IsNotNull(_unitUnderTest.AppVersion, "AppVersion is unexpected value.");
            Assert.IsNotEmpty((List<IEcellPlugin>)_unitUnderTest.Plugins, "Plugins is unexpected value.");
            Assert.IsNotEmpty((List<IRasterizable>)_unitUnderTest.Rasterizables, "Rasterizables is unexpected value.");
            Assert.IsNotEmpty((List<IDataHandler>)_unitUnderTest.DataHandlers, "DataHandlers is unexpected value.");
            Assert.IsNotEmpty((List<ILayoutAlgorithm>)_unitUnderTest.LayoutAlgorithms, "LayoutAlgorithms is unexpected value.");
            Assert.IsNotEmpty(_unitUnderTest.NodeImageList.Images, "NodeImageList is unexpected value.");
            Assert.AreEqual(ProjectStatus.Uninitialized, _unitUnderTest.Status, "Status is unexpected value.");
            Assert.AreEqual(_unitUnderTest.GetPlugin("PathwayWindow"), _unitUnderTest.DiagramEditor, "DiagramEditor is unexpected value.");
            Assert.AreEqual(null, _unitUnderTest.RootMenuProvider, "RootMenuProvider is unexpected value.");
            Assert.IsNull(_unitUnderTest.DockPanel, "DockPanel is unexpected value.");

        }

        /// <summary>
        /// TestSelectChanged
        /// </summary>
        [Test()]
        public void TestSelectChanged()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            _unitUnderTest.SelectChanged(modelID, key, type);

            EcellObject sys = _env.DataManager.GetEcellObject(modelID, key, type);
            _unitUnderTest.SelectChanged(sys);

        }

        /// <summary>
        /// TestAddSelect
        /// </summary>
        [Test()]
        public void TestAddSelect()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            _unitUnderTest.AddSelect(modelID, key, type);

        }
        /// <summary>
        /// TestRemoveSelect
        /// </summary>
        [Test()]
        public void TestRemoveSelect()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            _unitUnderTest.AddSelect(modelID, key, type);
            _unitUnderTest.RemoveSelect(modelID, key, type);

        }
        /// <summary>
        /// TestResetSelect
        /// </summary>
        [Test()]
        public void TestResetSelect()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            _unitUnderTest.AddSelect(modelID, key, type);
            _unitUnderTest.ResetSelect();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChanged()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";

            EcellObject sys = _env.DataManager.GetEcellObject(modelID, key, type);
            _unitUnderTest.DataChanged(modelID, key, type, sys);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataAdd()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";

            EcellObject sys = _env.DataManager.CreateDefaultObject(modelID, key, type);
            _env.DataManager.DataAdd(sys);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDelete()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/CELL";
            string type = "System";

            _unitUnderTest.DataDelete(modelID, key, type);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParameterAdd()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";

            _unitUnderTest.ParameterAdd(modelID, modelID);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParameterDelete()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string paramID = "newParam";
            _unitUnderTest.ParameterAdd(modelID, paramID);
            _unitUnderTest.ParameterUpdate(modelID, paramID);
            _unitUnderTest.ParameterDelete(modelID, paramID);

        }

        /// <summary>
        /// TestClear
        /// </summary>
        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoggerAdd()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";
            string key = "/CELL:SIZE";
            string type = "Variable";
            string path = "Variable:/CELL:SIZE:Value";

            _unitUnderTest.LoggerAdd(new LoggerEntry(modelID, key, type, path));

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAdvancedTime()
        {
            double time = 0;
            _unitUnderTest.AdvancedTime(time);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveModel()
        {
            string modelID = null;
            string path = null;
            _unitUnderTest.SaveModel(modelID, path);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetImageIndex()
        {
            string type = null;
            int expectedInt32 = -1;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "Project";
            expectedInt32 = 0;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "Model";
            expectedInt32 = 1;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "System";
            expectedInt32 = 5;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "Process";
            expectedInt32 = 6;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "Variable";
            expectedInt32 = 7;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "dm";
            expectedInt32 = 2;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "Parameters";
            expectedInt32 = 3;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

            type = "Log";
            expectedInt32 = 4;
            resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUnloadPlugin()
        {
            IEcellPlugin p = _unitUnderTest.GetPlugin("Analysis");
            _unitUnderTest.UnloadPlugin(p);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestChangeStatus()
        {
            Ecell.ProjectStatus type = ProjectStatus.Uninitialized;
            _unitUnderTest.ChangeStatus(type);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddPlugin()
        {           
            IEcellPlugin plugin = _unitUnderTest.GetPlugin("PathwayWindow");
            try
            {
                _unitUnderTest.AddPlugin(plugin);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }
            try
            {
                object obj = new object();
                _unitUnderTest.RegisterPlugin(obj.GetType());
                Assert.Fail();
            }
            catch (EcellException)
            {
            }

            TestMainWindow min = new TestMainWindow();
            _unitUnderTest.AddPlugin(min);
            Assert.IsNotNull(_unitUnderTest.DockPanel, "DockPanel method returned unexpected result.");

            try
            {
                _unitUnderTest.AddPlugin(min);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            TestMainWindow2 min2 = new TestMainWindow2();
            try
            {
                _unitUnderTest.AddPlugin(min2);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetPlugin()
        {
            IEcellPlugin resultPluginBase = null;

            resultPluginBase = _unitUnderTest.GetPlugin("AlignLayout");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("AlignLayout", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("Analysis");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("Analysis", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("CircularLayout");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("CircularLayout", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("Console");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("Console", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("DistributeLayout");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("DistributeLayout", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("EntityList");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("EntityList", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("GridLayout");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("GridLayout", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("MessageListWindow");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("MessageListWindow", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("PathwayWindow");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("PathwayWindow", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("Plotter");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("Plotter", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("ProjectExplorer");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("ProjectExplorer", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("PropertyWindow");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("PropertyWindow", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("ScriptWindow");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("ScriptWindow", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("Simulation");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("Simulation", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("Spreadsheet");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("Spreadsheet", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("StaticDebugWindow");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("StaticDebugWindow", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("TracerWindow");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("TracerWindow", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

            resultPluginBase = _unitUnderTest.GetPlugin("Hoge");
            Assert.IsNull(resultPluginBase, "GetPlugin method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetPluginVersionList()
        {
            Dictionary<string, string> resultDictionary = _unitUnderTest.GetPluginVersionList();
            Assert.IsNotEmpty(resultDictionary, "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["AlignLayout"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["Analysis"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["CircularLayout"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["Console"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["DistributeLayout"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["EntityList"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["GridLayout"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["MessageListWindow"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["PathwayWindow"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["Plotter"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["ProjectExplorer"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["PropertyWindow"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["ScriptWindow"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["Simulation"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["Spreadsheet"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["StaticDebugWindow"], "GetPluginVersionList method returned unexpected result.");
            Assert.IsNotEmpty(resultDictionary["TracerWindow"], "GetPluginVersionList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetPosition()
        {
            EcellObject data = null;
            _unitUnderTest.SetPosition(data);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestObservedData()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string path = "System:/CELL:SIZE:Value";

            EcellObservedData data = new EcellObservedData(path, 0.1);
            _unitUnderTest.SetObservedData(data);
            _unitUnderTest.RemoveObservedData(data);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParameterData()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            string path = "System:/CELL:SIZE:Value";

            EcellParameterData data = new EcellParameterData(path, 0.1);
            _unitUnderTest.SetParameterData(data);
            _unitUnderTest.RemoveParameterData(data);
        }

        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestEventHandler()
        {
            _unitUnderTest.Refresh += new EventHandler(_unitUnderTest_Refresh);
            _unitUnderTest.Refresh -= _unitUnderTest_Refresh;

            _unitUnderTest.NodeImageListChange += new EventHandler(_unitUnderTest_NodeImageListChange);
            _unitUnderTest.NodeImageListChange -= _unitUnderTest_NodeImageListChange;
        }

        void _unitUnderTest_NodeImageListChange(object sender, EventArgs e)
        {
        }

        void _unitUnderTest_Refresh(object sender, EventArgs e)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetDelegate()
        {
            string name = "";
            Delegate method = _unitUnderTest.GetDelegate(name);
            Assert.IsNull(method);

            name = "SaveSimulationResult";
            method = _unitUnderTest.GetDelegate(name);
            Assert.IsNotNull(method);

            name = "ShowGraphWithLog";
            method = _unitUnderTest.GetDelegate(name);
            Assert.IsNotNull(method);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadPlugin()
        {
            string path = "hoge.dll";
            IEcellPlugin plugin;
            try
            {
                plugin = _unitUnderTest.LoadPlugin(path);
                Assert.Fail("");
            }
            catch (Exception)
            {
            }

            path = Path.Combine(Util.GetBinDir(), "ecell-ide.exe");
            plugin = _unitUnderTest.LoadPlugin(path);

            try
            {
                _unitUnderTest.AddPlugin(plugin);
                Assert.Fail("");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetPropertySettings()
        {
            List<IPropertyItem> list = _unitUnderTest.GetPropertySettings();
            Assert.IsNotEmpty(list, "GetPropertySettings method returned unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPluginBase()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            PluginBase plugin = (PluginBase)_unitUnderTest.GetPlugin("PathwayWindow");
            Assert.IsNotNull(plugin, "GetPlugin method returned unexpected value.");

            Assert.AreEqual(_env, plugin.Environment, "Environment is unexpected value.");
            Assert.AreEqual(_env.DataManager, plugin.DataManager, "DataManager is unexpected value.");
            Assert.AreEqual(_env.PluginManager, plugin.PluginManager, "PluginManager is unexpected value.");
            Assert.AreEqual(_env.LogManager, plugin.MessageManager, "MessageManager is unexpected value.");

            Assert.AreEqual("PathwayWindow", plugin.GetPluginName(), "GetPluginName method returned unexpected value.");
            Assert.IsNotNull(plugin.GetVersionString(), "GetVersionString method returned unexpected value.");

            Assert.IsNotNull(plugin.GetData(null), "GetData method returned unexpected value.");

            Assert.IsNotNull(plugin.GetEcellObject("Drosophila", "/", "System"), "GetEcellObject method returned unexpected value.");

            Assert.IsNotNull(plugin.GetMenuStripItems(), "GetMenuStripItems method returned unexpected value.");

            Assert.IsNotNull(plugin.GetToolBarMenuStrip(), "GetToolBarMenuStrip method returned unexpected value.");

            Assert.IsNull(plugin.GetPublicDelegate(), "GetPublicDelegate method returned unexpected value.");

        }
    }
}
