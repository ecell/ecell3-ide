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
using Ecell.Plugin;
using Ecell.Objects;
using Ecell.Logging;
using System.IO;
using System.Diagnostics;

namespace Ecell
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestPluginManager
    {
        private static PluginManager _unitUnderTest;
        private static ApplicationEnvironment m_env;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            m_env = new ApplicationEnvironment();
            _unitUnderTest = m_env.PluginManager;

            // Load plugins
            foreach (string pluginDir in Util.GetPluginDirs())
            {
                string[] files = Directory.GetFiles(
                    pluginDir,
                    Constants.delimiterWildcard + Constants.FileExtPlugin);
                foreach (string fileName in files)
                {
                    m_env.PluginManager.LoadPlugin(fileName);
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
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestFocusDataChanged()
        {
            string modelID = null;
            string key = null;
            IEcellPlugin pbase = null;
            _unitUnderTest.FocusDataChanged(modelID, key, pbase);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestFocusClear()
        {
            _unitUnderTest.FocusClear();

        }

        /// <summary>
        /// TestSelectChanged
        /// </summary>
        [Test()]
        public void TestSelectChanged()
        {
            m_env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            _unitUnderTest.SelectChanged(modelID, key, type);
        }

        /// <summary>
        /// TestAddSelect
        /// </summary>
        [Test()]
        public void TestAddSelect()
        {
            m_env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
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
            m_env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
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
            m_env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
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
            string modelID = null;
            string key = null;
            string type = null;
            EcellObject data = null;
            _unitUnderTest.DataChanged(modelID, key, type, data);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataAdd()
        {
            List<EcellObject> data = null;
            _unitUnderTest.DataAdd(data);

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

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParameterAdd()
        {
            string projectID = null;
            string paramID = null;
            _unitUnderTest.ParameterAdd(projectID, paramID);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestParameterDelete()
        {
            string projectID = null;
            string paramID = null;
            _unitUnderTest.ParameterDelete(projectID, paramID);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadData()
        {
            string modelID = null;
            _unitUnderTest.LoadData(modelID);

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
            string modelID = null;
            string type = null;
            string key = null;
            string path = null;
            _unitUnderTest.LoggerAdd(modelID, key, type, path);

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
            int expectedInt32 = 0;
            int resultInt32 = 0;
            resultInt32 = _unitUnderTest.GetImageIndex(type);
            Assert.AreEqual(expectedInt32, resultInt32, "GetImageIndex method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUnloadPlugin()
        {
            IEcellPlugin p = null;
            _unitUnderTest.UnloadPlugin(p);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCurrentToolBarMenu()
        {
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.CurrentToolBarMenu();
            Assert.AreEqual(expectedString, resultString, "CurrentToolBarMenu method returned unexpected result.");

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
        public void TestGetPlugin()
        {
            IEcellPlugin resultPluginBase = null;

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
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
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

            resultPluginBase = _unitUnderTest.GetPlugin("Tracer");
            Assert.IsNotNull(resultPluginBase, "GetPlugin method returned unexpected result.");
            Assert.AreNotEqual(0, resultPluginBase.GetHashCode(), "GetHashCode method returned unexpected result.");
            Assert.AreEqual("Tracer", resultPluginBase.GetPluginName(), "GetPluginName method returned unexpected result.");
            Assert.AreNotEqual(null, resultPluginBase.Environment, "Environment is unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.GetVersionString(), "GetVersionString method returned unexpected result.");
            Assert.AreNotEqual("", resultPluginBase.ToString(), "ToString method returned unexpected result.");
            Assert.AreEqual(null, resultPluginBase.GetPublicDelegate(), "GetPublicDelegate method returned unexpected result.");
            resultPluginBase.Initialize();
            resultPluginBase.ChangeStatus(ProjectStatus.Uninitialized);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetPluginVersionList()
        {
            Dictionary<System.String, System.String> expectedDictionary = null;
            Dictionary<System.String, System.String> resultDictionary = _unitUnderTest.GetPluginVersionList();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetPluginVersionList method returned unexpected result.");

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
    }
}
