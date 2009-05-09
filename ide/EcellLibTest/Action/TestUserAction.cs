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
using System.Reflection;
using Ecell.Objects;
using Ecell.Action;

namespace Ecell
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestUserAction
    {
        private ApplicationEnvironment _env;
        private ActionManager _manager;

        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _manager = _env.ActionManager;
        }
        /// <summary>
        /// 
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _env = null;
            _manager = null;
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAnchorAction()
        {
            UserAction action = new AnchorAction();
            Assert.IsNotNull(action, "Constructor of type, AnchorAction failed to create instance.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            Assert.AreEqual(_env, action.Environment, "Environment is unexpected value.");
            Assert.AreEqual(true, action.IsAnchor, "IsAnchor is unexpected value.");
            Assert.AreEqual(true, action.IsUndoable, "IsUndoable is unexpected value.");
            Assert.AreEqual("AnchorAction: True", action.ToString(), "ToString is unexpected value.");

            action.UnExecute();
            action.Execute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNewProjectAction()
        {
            UserAction action = new NewProjectAction("project", "comment");
            Assert.IsNotNull(action, "Constructor of type, NewProjectAction failed to create instance.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            Assert.AreEqual(_env, action.Environment, "Environment is unexpected value.");
            Assert.AreEqual(true, action.IsAnchor, "IsAnchor is unexpected value.");
            Assert.AreEqual(false, action.IsUndoable, "IsUndoable is unexpected value.");
            Assert.AreEqual("NewProjectAction:project", action.ToString(), "ToString is unexpected value.");

            action.Execute();
            action.UnExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadProjectAction()
        {
            UserAction action = new LoadProjectAction("Drosophila", TestConstant.Project_Drosophila);
            Assert.IsNotNull(action, "Constructor of type, LoadProjectAction failed to create instance.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            Assert.AreEqual(_env, action.Environment, "Environment is unexpected value.");
            Assert.AreEqual(true, action.IsAnchor, "IsAnchor is unexpected value.");
            Assert.AreEqual(false, action.IsUndoable, "IsUndoable is unexpected value.");
            Assert.AreEqual("LoadProjectAction:" + TestConstant.Project_Drosophila, action.ToString(), "ToString is unexpected value.");

            action.Execute();
            action.UnExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataAddAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            EcellObject sys = _env.DataManager.CreateDefaultObject(modelID, key, type);
            _env.DataManager.DataAdd(sys.Clone());
            Assert.AreEqual(UndoStatus.UNDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            _env.ActionManager.UndoAction();
            Assert.AreEqual(UndoStatus.REDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");
            _env.ActionManager.RedoAction();
            Assert.AreEqual(UndoStatus.UNDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            UserAction action = _env.ActionManager.GetAction(1);
            Assert.IsTrue(action.ToString().Contains("DataAddAction"), "ToString is unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangeAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            EcellObject sys = _env.DataManager.GetEcellObject(modelID, key, type);
            _env.DataManager.DataChanged(modelID, key, type, sys.Clone());
            Assert.AreEqual(UndoStatus.UNDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            _env.ActionManager.UndoAction();
            Assert.AreEqual(UndoStatus.REDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");
            _env.ActionManager.RedoAction();
            Assert.AreEqual(UndoStatus.UNDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            UserAction action = _env.ActionManager.GetAction(1);
            Assert.IsTrue(action.ToString().Contains("DataChangeAction"), "ToString is unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDeleteAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            string modelID = "Drosophila";
            string key = "/CELL";
            string type = "System";
            EcellObject sys = _env.DataManager.GetEcellObject(modelID, key, type);
            _env.DataManager.DataDelete(modelID, key, type);
            Assert.AreEqual(UndoStatus.UNDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            _env.ActionManager.UndoAction();
            Assert.AreEqual(UndoStatus.REDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");
            _env.ActionManager.RedoAction();
            Assert.AreEqual(UndoStatus.UNDO_ONLY, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            UserAction action = _env.ActionManager.GetAction(1);
            Assert.IsTrue(action.ToString().Contains("DataDeleteAction"), "ToString is unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSimParamAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            UserAction action = new AddSimParamAction("newParam", true);
            Assert.IsNotNull(action, "Constructor of type, AddSimParamAction failed to create instance.");
            Assert.IsTrue(action.ToString().Contains("AddSimParamAction"), "ToString is unexpected value.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            action.Execute();
            action.UnExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSimParamAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");
            _env.DataManager.CreateSimulationParameter("newParam");

            UserAction action = new DeleteSimParamAction("newParam", true);
            Assert.IsNotNull(action, "Constructor of type, DeleteSimParamAction failed to create instance.");
            Assert.IsTrue(action.ToString().Contains("DeleteSimParamAction"), "ToString is unexpected value.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            action.Execute();
            action.UnExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetSimParamAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            UserAction action = new SetSimParamAction("DefaultParameter", "DefaultParameter", true);
            Assert.IsNotNull(action, "Constructor of type, SetSimParamAction failed to create instance.");
            Assert.IsTrue(action.ToString().Contains("SetSimParamAction"), "ToString is unexpected value.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            // ToDo:
            action.Execute();
            action.UnExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddStepperAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            UserAction action = new AddStepperAction(new EcellStepper("Drosophila", "ODE", "Stepper", "ODEStepper", new List<EcellData>()));
            Assert.IsNotNull(action, "Constructor of type, AddStepperAction failed to create instance.");
            Assert.IsTrue(action.ToString().Contains("AddStepperAction"), "ToString is unexpected value.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            action.Execute();
            action.UnExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteStepperAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");

            _env.DataManager.AddStepperID(new EcellStepper("Drosophila", "ODE", "Stepper", "ODEStepper", new List<EcellData>()));

            UserAction action = new DeleteStepperAction(new EcellStepper("Drosophila", "ODE", "Stepper", "ODEStepper", new List<EcellData>()));
            Assert.IsNotNull(action, "Constructor of type, DeleteStepperAction failed to create instance.");
            Assert.IsTrue(action.ToString().Contains("DeleteStepperAction"), "ToString is unexpected value.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            action.Execute();
            action.UnExecute();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestChangeStepperAction()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            Assert.AreEqual(UndoStatus.NOTHING, _env.ActionManager.UndoStatus, "UndoStatus is unexpected value.");
            EcellObject stepper = new EcellStepper("Drosophila", "ODE", "Stepper", "ODEStepper", new List<EcellData>());
            _env.DataManager.AddStepperID(stepper);

            EcellObject oldStepper = stepper;
            EcellObject newStepper = stepper.Clone();

            UserAction action = new ChangeStepperAction(newStepper.Key, oldStepper.Key , newStepper, oldStepper);
            Assert.IsNotNull(action, "Constructor of type, ChangeStepperAction failed to create instance.");
            Assert.IsTrue(action.ToString().Contains("ChangeStepperAction"), "ToString is unexpected value.");
            Type type = action.GetType();
            FieldInfo info = type.GetField("m_env", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(action, _env);

            action.Execute();
            action.UnExecute();
        }

    }
}
