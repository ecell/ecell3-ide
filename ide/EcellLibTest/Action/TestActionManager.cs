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

namespace Ecell
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using System.Diagnostics;
    using Ecell.Objects;
    using System.IO;
    using Ecell.Action;
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestActionManager
    {
        private ApplicationEnvironment _env;
        private ActionManager _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = _env.ActionManager;

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
            _env = null;
            _unitUnderTest = null;
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorActionManager()
        {
            ActionManager testActionManager = new ActionManager(_env);
            Assert.IsNotNull(testActionManager, "Constructor of type, ActionManager failed to create instance.");
            Assert.AreEqual(0, testActionManager.Count, "Count is unexpected value.");
            Assert.AreEqual(false, testActionManager.IsLoadAction, "IsLoadAction is unexpected value.");
            Assert.AreEqual(false, testActionManager.Redoable, "Redoable is unexpected value.");
            Assert.AreEqual(false, testActionManager.Undoable, "Undoable is unexpected value.");
            Assert.AreEqual(UndoStatus.NOTHING, testActionManager.UndoStatus, "UndoStatus is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddAction()
        {
            _env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
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
        public void TestGetAction()
        {
            _env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";

            EcellObject sys = _env.DataManager.CreateDefaultObject(modelID, key, type);
            _env.DataManager.DataAdd(sys);

            int index = 0;
            UserAction resultUserAction = _unitUnderTest.GetAction(index);
            Assert.IsNotNull(resultUserAction, "GetAction method returned unexpected result.");

            index = -1;
            resultUserAction = _unitUnderTest.GetAction(index);
            Assert.IsNull(resultUserAction, "GetAction method returned unexpected result.");

        }
        /// <summary>
        /// 
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
        public void TestUndoAction()
        {
            _env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";

            Assert.AreEqual(UndoStatus.NOTHING, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");

            EcellObject sys = _env.DataManager.CreateDefaultObject(modelID, key, type);
            _env.DataManager.DataAdd(sys);

            Assert.AreEqual(UndoStatus.UNDO_ONLY, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");
            _unitUnderTest.UndoAction();
            Assert.AreEqual(UndoStatus.REDO_ONLY, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRedoAction()
        {
            _unitUnderTest.RedoableChange += new EventHandler(_unitUnderTest_RedoableChange);
            _unitUnderTest.UndoableChange += new EventHandler(_unitUnderTest_UndoableChange);

            _env.DataManager.LoadProject("c:/temp/Drosophila/project.xml");
            string modelID = "Drosophila";
            string key = "/";
            string type = "System";

            Assert.AreEqual(UndoStatus.NOTHING, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");
            _unitUnderTest.UndoAction();
            Assert.AreEqual(UndoStatus.NOTHING, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");
            _unitUnderTest.RedoAction();
            Assert.AreEqual(UndoStatus.NOTHING, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");

            EcellObject sys = _env.DataManager.CreateDefaultObject(modelID, key, type);
            _env.DataManager.DataAdd(sys.Clone());
            sys = _env.DataManager.CreateDefaultObject(modelID, key, type);
            _env.DataManager.DataAdd(sys.Clone());

            Assert.AreEqual(UndoStatus.UNDO_ONLY, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");
            _unitUnderTest.UndoAction();
            Assert.AreEqual(UndoStatus.UNDO_REDO, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");
            _unitUnderTest.UndoAction();
            Assert.AreEqual(UndoStatus.REDO_ONLY, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");
            _unitUnderTest.RedoAction();
            Assert.AreEqual(UndoStatus.UNDO_REDO, _unitUnderTest.UndoStatus, "UndoStatus is unexpected value.");

            _env.DataManager.DataAdd(sys.Clone());

            _unitUnderTest.RedoableChange -= _unitUnderTest_RedoableChange;
            _unitUnderTest.UndoableChange -= _unitUnderTest_UndoableChange;

        }

        void _unitUnderTest_UndoableChange(object sender, EventArgs e)
        {
        }

        void _unitUnderTest_RedoableChange(object sender, EventArgs e)
        {
        }
    }
}
