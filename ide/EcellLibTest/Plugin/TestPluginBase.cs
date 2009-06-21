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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using Ecell.Objects;
using System.IO;

namespace Ecell.Plugin
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestPluginBase
    {
        private ApplicationEnvironment _env;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();

        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _env = null;
        }

        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestConstructor()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);

            TestPlugin plugin = new TestPlugin();
            Assert.IsNotNull(plugin, "Constructor of type, object failed to create instance.");
            // env
            plugin.Environment = _env;
            Assert.AreEqual(_env, plugin.Environment, "Environment is unexpected value.");
            Assert.AreEqual(_env.DataManager, plugin.DataManager, "DataManager is unexpected value.");
            Assert.AreEqual(_env.PluginManager, plugin.PluginManager, "PluginManager is unexpected value.");
            Assert.AreEqual(_env.LogManager, plugin.MessageManager, "MessageManager is unexpected value.");
            // Getter
            Assert.AreEqual("TestPlugin", plugin.GetPluginName(), "GetPluginName method returned unexpected value.");
            Assert.IsNotNull(plugin.GetVersionString(), "GetVersionString method returned unexpected value.");

            Assert.IsNotNull(plugin.GetData(null), "GetData method returned unexpected value.");
            Assert.IsNotNull(plugin.GetEcellObject("Drosophila", "/", "System"), "GetEcellObject method returned unexpected value.");
            Assert.IsNotNull(plugin.GetEcellObject(plugin.GetEcellObject("Drosophila", "/", "System")), "GetEcellObject method returned unexpected value.");
            Assert.IsNotNull(plugin.GetTemporaryID("Drosophila", "System", "/"), "GetEcellObject method returned unexpected value.");
            Assert.IsNull(plugin.GetPluginStatus(), "GetPluginStatus method returned unexpected value.");
            Assert.IsNull(plugin.GetPropertySettings(), "GetPropertySettings method returned unexpected value.");
            Assert.IsNull(plugin.GetMenuStripItems(), "GetMenuStripItems method returned unexpected value.");
            Assert.IsNull(plugin.GetToolBarMenuStrip(), "GetToolBarMenuStrip method returned unexpected value.");
            Assert.IsNull(plugin.GetWindowsForms(), "GetWindowsForms method returned unexpected value.");
            Assert.IsNull(plugin.GetPublicDelegate(), "GetPublicDelegate method returned unexpected value.");
        }

        /// <summary>
        /// TestMethodTemplate
        /// </summary>
        [Test()]
        public void TestMethodTemplate()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);

            TestPlugin plugin = new TestPlugin();
            plugin.DataAdd(new List<EcellObject>());
            EcellObject obj = null;
            plugin.DataAdd(obj);
            plugin.DataChanged(null, null, null, null);
            plugin.DataDelete(null, null, null);
            //
            plugin.AddSelect(null, null, null);
            plugin.RemoveSelect(null, null, null);
            plugin.SelectChanged(null, null, null);
            plugin.ResetSelect();

            plugin.AdvancedTime(0);
            plugin.Initialize();
            plugin.ChangeStatus(ProjectStatus.Loaded);
            plugin.Clear();

            plugin.LoggerAdd(null);
            plugin.ParameterAdd(null, null);
            plugin.ParameterDelete(null, null);
            plugin.ParameterSet(null, null);
            plugin.ParameterUpdate(null, null);
            plugin.RemoveMessage(null);

            plugin.SaveModel(null, null);
            plugin.SetPluginStatus(null);
            plugin.SetProgressBarValue(0);
            plugin.SetStatusBarMessage(StatusBarMessageKind.Generic, "");

        }

        
        /// <summary>
        /// TestNotifier
        /// </summary>
        [Test()]
        public void TestNotifier()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            TestPlugin plugin = new TestPlugin();
            plugin.Environment = _env;

            EcellObject obj1 = _env.DataManager.CreateDefaultObject("Drosophila", "/", "System");
            plugin.NotifyDataAdd(obj1, true);

            EcellObject obj2 = _env.DataManager.CreateDefaultObject("Drosophila", "/", "System");
            List<EcellObject> list = new List<EcellObject>();
            list.Add(obj2);
            plugin.NotifyDataAdd(list, true);
            plugin.NotifyDataChanged(obj1.Key, obj1, true, true);
            plugin.NotifySetPosition(obj1);
            plugin.NotifyLoggerAdd(obj1.ModelID, obj1.Key, obj1.Type, obj1.FullID + ":Size");

            plugin.NotifyAddSelect(obj1.ModelID, obj1.Key, obj1.Type);
            plugin.NotifySelectChanged(obj1.ModelID, obj1.Key, obj1.Type);
            plugin.NotifyRemoveSelect(obj1.ModelID, obj1.Key, obj1.Type);
            plugin.NotifyResetSelect();
            plugin.NotifyDataMerge(obj1.ModelID, obj1.Key);
            plugin.NotifyDataMerge(obj1.ModelID, obj1.Key);

            plugin.NotifyDataDelete(obj2.ModelID, obj2.Key, obj2.Type, true);



        }

        /// <summary>
        /// 
        /// </summary>
        internal class TestPlugin : PluginBase
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string GetPluginName()
            {
                return "TestPlugin";
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string GetVersionString()
            {
                return "1.0";
            }
        }

    }
}
