//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
    using System.Windows.Forms;
    using NUnit.Framework;
    using System.IO;
    using System.Diagnostics;
    using Ecell.Objects;
    using System.Collections.Generic;
    using Ecell.Exceptions;
    using EcellCoreLib;
    using System.Reflection;
    using System.Collections;
    using Ecell.Plugin;
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestDataManager
    {
        private static readonly string ActionFile = Path.Combine(Util.GetUserDir(), "");
        private static readonly string ActionFileUnCorrect = Path.Combine(Util.GetUserDir(), "");

        private ApplicationEnvironment _env;
        private DataManager _unitUnderTest;
        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [SetUp()]
        public void TestFixtureSetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = _env.DataManager;
        }
        /// <summary>
        /// TestFixtureTearDown
        /// </summary>
        [TearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
            _env = null;
        }

        /// <summary>
        /// TestAccessor
        /// </summary>
        [Test()]
        public void TestAccessor()
        {
            Assert.IsNull(_unitUnderTest.CurrentProject, "CurrentProject is unexpected value.");
            Assert.IsNull(_unitUnderTest.CurrentProjectID, "CurrentProjectID is unexpected value.");

            Assert.AreEqual(Util.GetBaseDir(),_unitUnderTest.DefaultDir, "DefaultDir is unexpected value.");
            Assert.AreEqual(_env, _unitUnderTest.Environment, "Environment is unexpected value.");
            Assert.IsFalse(_unitUnderTest.IsActive, "IsActive is unexpected value.");
            Assert.AreEqual(-1.0, _unitUnderTest.SimulationTimeLimit, "SimulationTimeLimit is unexpected value.");
            Assert.AreEqual(10, _unitUnderTest.StepCount, "StepCount is unexpected value.");

            _unitUnderTest.SimulationTimeLimit = 100;
            _unitUnderTest.StepCount = 20;
            Assert.AreEqual(100.0, _unitUnderTest.SimulationTimeLimit, "SimulationTimeLimit is unexpected value.");
            Assert.AreEqual(20, _unitUnderTest.StepCount, "StepCount is unexpected value.");

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            _unitUnderTest.DisplayFormat = ValueDataFormat.Exponential2;
            Assert.AreEqual(_unitUnderTest.DisplayFormat, ValueDataFormat.Exponential2, "DisplayFormat is unexpected value.");
            _unitUnderTest.DisplayFormat = ValueDataFormat.Exponential2;

            _unitUnderTest.DisplayFormat = ValueDataFormat.Exponential1;
            Assert.AreEqual(_unitUnderTest.DisplayStringFormat, "e1", "DisplayStringFormat is unexpected value.");
            _unitUnderTest.DisplayFormat = ValueDataFormat.Exponential2;
            Assert.AreEqual(_unitUnderTest.DisplayStringFormat, "e2", "DisplayStringFormat is unexpected value.");
            _unitUnderTest.DisplayFormat = ValueDataFormat.Exponential3;
            Assert.AreEqual(_unitUnderTest.DisplayStringFormat, "e3", "DisplayStringFormat is unexpected value.");
            _unitUnderTest.DisplayFormat = ValueDataFormat.Exponential4;
            Assert.AreEqual(_unitUnderTest.DisplayStringFormat, "e4", "DisplayStringFormat is unexpected value.");
            _unitUnderTest.DisplayFormat = ValueDataFormat.Exponential5;
            Assert.AreEqual(_unitUnderTest.DisplayStringFormat, "e5", "DisplayStringFormat is unexpected value.");
            _unitUnderTest.DisplayFormat = ValueDataFormat.Normal;
            Assert.AreEqual(_unitUnderTest.DisplayStringFormat, "G", "DisplayStringFormat is unexpected value.");
        }

        /// <summary>
        /// TestGetDMDLLFileName
        /// </summary>
        [Test()]
        public void TestGetDMDLLFileName()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_CoupledOscillator);

            string path = _unitUnderTest.GetDMDLLFileName("");
            Assert.AreEqual(null, path, "DLLFileName is unexpected value.");

            path = _unitUnderTest.GetDMDLLFileName("FOProcess");
            string name = Path.GetFileName(path);
            Assert.AreEqual("FOProcess.dll", name, "DLLFileName is unexpected value.");
        }

        /// <summary>
        /// TestLoadProject
        /// </summary>
        [Test()]
        public void TestLoadSBML()
        {
            string testDir = TestConstant.TestDirectory + "Test";
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);

            Util.CopyFile(TestConstant.SBML_Oscillation, testDir);
            string filename = Path.Combine(testDir, Path.GetFileName(TestConstant.SBML_Oscillation));
            // Error1
            try
            {
                _unitUnderTest.LoadSBML(TestConstant.TestDirectory + "hoge.sbml");
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            // Error2
            try
            {
                _unitUnderTest.LoadSBML(TestConstant.Project_Drosophila);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            // Correct case.
            try
            {
                _unitUnderTest.LoadSBML(filename);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            finally
            {
                if (Directory.Exists(testDir))
                    Directory.Delete(testDir, true);
            }
        }
        /// <summary>
        /// TestLoadProject
        /// </summary>
        [Test()]
        public void TestLoadProject()
        {
            // Load null
            string filename = null;
            try
            {
                _unitUnderTest.LoadProject(filename);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            // Load null
            try
            {
                filename = "";
                _unitUnderTest.LoadProject(filename);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            // Load null
            try
            {
                ProjectInfo info = null;
                _unitUnderTest.LoadProject(info);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            // Load incorrect file
            try
            {
                filename = TestConstant.TestDirectory + "hoge.eml";
                _unitUnderTest.LoadProject(filename);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            // Load RBC
            filename = TestConstant.Model_RBC;
            _unitUnderTest.LoadProject(filename);

            // Load Drosophila
            filename = TestConstant.Project_Drosophila;
            _unitUnderTest.LoadProject(filename);
        }

        /// <summary>
        /// TestAddStepperIDL_parameterIDL_stepper
        /// </summary>
        [Test()]
        public void TestGetDir()
        {
            // Load Drosophila
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string dir = _unitUnderTest.GetDMDir();
            List<string> list = _unitUnderTest.GetDMNameList();
            string dmName = _unitUnderTest.GetDMSourceFileName("");

            string simDir = _unitUnderTest.GetSimulationResultSaveDirectory();

            string dmDir = _unitUnderTest.GetDMDir();
            Assert.IsNotNull(dmDir, "GetDMDir is unexpected value.");

            _unitUnderTest.CurrentProject.Info.ProjectPath = null;
            dmDir = _unitUnderTest.GetDMDir();
            Assert.IsNull(dmDir, "GetDMDir is unexpected value.");
        }

        /// <summary>
        /// TestAddStepperIDL_parameterIDL_stepper
        /// </summary>
        [Test()]
        public void TestAddStepperIDL_parameterIDL_stepper()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            EcellObject l_stepper = null;

            try
            {
                _unitUnderTest.AddStepperID(l_stepper);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }
            
            l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID(l_stepper);

            try
            {
                _unitUnderTest.AddStepperID(l_stepper);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }

            try
            {
                l_stepper = EcellObject.CreateObject("Drosophila1", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
                _unitUnderTest.AddStepperID(l_stepper);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }

        }

        /// <summary>
        /// TestAddStepperIDL_parameterIDL_stepperL_isRecorded
        /// </summary>
        [Test()]
        public void TestBulidDefaultSimulator()
        {
            Type type = _unitUnderTest.GetType();
            MethodInfo methodInfo = type.GetMethod("BuildDefaultSimulator", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(methodInfo, "GetMethod method returned unexpected value.");

            WrappedSimulator simulator = new WrappedSimulator(Util.GetDMDirs());
            string defProcess = null;
            string defStepper = null;
            methodInfo.Invoke(_unitUnderTest, new object[] { simulator, defProcess, defStepper });

            try
            {
                defProcess = "hoge";
                methodInfo.Invoke(_unitUnderTest, new object[] { simulator, defProcess, defStepper });
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
        public void TestCloseProject()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            _unitUnderTest.CloseProject();

            try
            {
                _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
                _unitUnderTest.CurrentProject.Simulator = null;
                _unitUnderTest.CloseProject();
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
        public void TestConfirmReset()
        {
            Type type = _unitUnderTest.GetType();
            MethodInfo info = type.GetMethod("ConfirmReset", BindingFlags.Public | BindingFlags.Instance);
            info.Invoke(_unitUnderTest, new object[] { "", "" });

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            info = type.GetMethod("ConfirmReset", BindingFlags.Public | BindingFlags.Instance);
            info.Invoke(_unitUnderTest, new object[] { "", "" });

            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            info.Invoke(_unitUnderTest, new object[] { "Add", "Text" });

            MessageBox.Show("Click \"OK\" to next dialog.");
            info.Invoke(_unitUnderTest, new object[] { "Add", "Variable" });

            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            try
            {
                MessageBox.Show("Click \"Cancel\" to next dialog.");
                info.Invoke(_unitUnderTest, new object[] { "Add", "Variable" });
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
        public void TestDataAddL_ecellObjectListL_isRecordedL_isAnchor()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            EcellObject sys = _unitUnderTest.CreateDefaultObject(modelID, key, type);

            List<EcellObject> l_ecellObjectList = new List<EcellObject>();
            l_ecellObjectList.Add(sys);
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataAdd(l_ecellObjectList, l_isRecorded, l_isAnchor);

            try
            {
                _unitUnderTest.DataAdd(sys);
                Assert.Fail();
            }
            catch (Exception)
            {
            }


            key = "/";
            type = "Variable";
            EcellObject var = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            _unitUnderTest.DataAdd(var);

            try
            {
                _unitUnderTest.DataAdd(var);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            EcellObject stepper = _unitUnderTest.GetEcellObject("Drosophila", "DE", "Stepper");
            stepper.Key = "DE2";
            _unitUnderTest.DataAdd(stepper);

            EcellObject text = _unitUnderTest.CreateDefaultObject("Drosophila", "/", "Text");
            _unitUnderTest.DataAdd(text);

            EcellObject variable = _unitUnderTest.CreateDefaultObject("Drosophila", "/CELL", "Variable");
            variable.GetEcellData("Value").Logged = true;
            _unitUnderTest.DataAdd(variable);

            EcellObject variable2 = _unitUnderTest.CreateDefaultObject("Drosophila", "/CELL1", "Variable");
            variable2.ModelID = null;
            _unitUnderTest.DataAdd(variable2);

            MessageBox.Show("Click \"Cancel\" button on next dialog.");
            EcellObject variable3 = _unitUnderTest.CreateDefaultObject("Drosophila", "/CELL3", "Variable");
            _env.PluginManager.ChangeStatus(ProjectStatus.Suspended);
            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            _unitUnderTest.DataAdd(variable3);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_ecellObjectList()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string key = "/CELL";
            string type = "System";
            EcellObject sys = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            _unitUnderTest.DataAdd(sys);

            sys = _unitUnderTest.GetEcellObject(modelID, sys.Key, type);
            sys.X = 40;
            List<EcellObject> l_ecellObjectList = new List<EcellObject>();
            l_ecellObjectList.Add(sys);
            _unitUnderTest.DataChanged(l_ecellObjectList);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_modelIDL_keyL_typeL_ecellObject()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            EcellObject sys = _unitUnderTest.GetEcellObject(modelID, key, type);
            _unitUnderTest.DataChanged(modelID, key, type, sys);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_modelIDL_keyL_typeL_ecellObjectL_isRecordedL_isAnchor()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            string l_modelID = "Drosophila";
            string l_parameterID = "NewParam";
            EcellObject sys = _unitUnderTest.GetEcellObject(modelID, key, type);
            bool l_isRecorded = false;
            bool l_isAnchor = false;

            // Test Change system. 
            _unitUnderTest.DataChanged(modelID, key, type, sys, l_isRecorded, l_isAnchor);
            try
            {
                sys.Key = "/hoge";
                _unitUnderTest.DataChanged(modelID, "/", type, sys);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            sys = _unitUnderTest.GetEcellObject(modelID, "/CELL", type);
            _unitUnderTest.DataChanged(modelID, "/CELL", type, sys);

            sys.Key = "/CELL2";
            _unitUnderTest.DataChanged(modelID, "/CELL", type, sys);

            // Test Change model.
            EcellObject model = _unitUnderTest.GetEcellObject(modelID, "", "Model");
            _unitUnderTest.DataChanged(modelID, "", "Model", model);
            try
            {
                model.ModelID = "Drosophila2";
                _unitUnderTest.DataChanged(modelID, "", "Model", model);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            // Test Change Variable. 
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            EcellObject variable = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:M", "Variable");
            _env.LoggerManager.AddLoggerEntry(variable.ModelID, variable.Key, variable.Type, variable.Key + ":Value");
            variable.Key = "/CELL:M";

            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:M", "Variable", variable);

            _env.PluginManager.ChangeStatus(ProjectStatus.Suspended);
            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            _unitUnderTest.DataChanged(modelID, "/CELL:M", "Variable", variable);

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            variable = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:M", "Variable");
            variable.SetEcellValue("Value", new EcellValue(0.1));
            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:M", "Variable", variable);

            // Test Change Variable. 
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            variable = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:M", "Variable");
            variable.Key = "/CELL/CYTOPLASM:P1";
            try
            {
                _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:M", "Variable", variable);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            // Test Change Variable. 
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            variable = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:M", "Variable");
            EcellData d = variable.GetEcellData(Constants.xpathValue);
            d.Logged = true;
            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:M", "Variable", variable);

            variable = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:M", "Variable");
            d = variable.GetEcellData(Constants.xpathValue);
            d.Logged = false;
            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:M", "Variable", variable);

            variable = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:M", "Variable");
            d = variable.GetEcellData(Constants.xpathValue);
            d.Logged = true;
            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:M", "Variable", variable);

            // Test Change Stepper
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            EcellObject stepper = _unitUnderTest.GetEcellObject(modelID, "DE", Constants.xpathStepper);
            stepper.Key = "DE1";
            _unitUnderTest.DataChanged(modelID, "DE", Constants.xpathStepper, stepper);


            // Test CheckDifference
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            EcellObject p = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess);
            EcellData d4 = p.GetEcellData(Constants.xpathActivity);
            d4.Logged = true;
            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess, p);

            // Class name changed at DefaultSets .
            p = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess);
            EcellData d1 = p.GetEcellData(Constants.xpathExpression);
            EcellData d2 = p.GetEcellData("K1");
            EcellData d3 = p.GetEcellData("V1");
            d4 = p.GetEcellData(Constants.xpathActivity);
            d4.Logged = true;

            d3.Name = "k";
            d3.EntityPath = d3.EntityPath.Replace("V1", "k");
            p.Value.Remove(d1);
            p.Value.Remove(d2);
            p.Classname = "ConstantFluxProcess";
            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess, p);

            // ID changed.
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            p = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess);
            p.Key = "/CELL/CYTOPLASM:R_toy41";
            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess, p);

            // Class name changed at No DefaultSets .
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            p = _unitUnderTest.GetEcellObject(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess);
            d1 = p.GetEcellData(Constants.xpathExpression);
            d2 = p.GetEcellData("K1");
            d3 = p.GetEcellData("V1");
            d4 = p.GetEcellData(Constants.xpathActivity);
            d4.Logged = true;

            d3.Name = "k";
            d3.EntityPath = d3.EntityPath.Replace("V1", "k");
            p.Value.Remove(d1);
            p.Value.Remove(d2);
            p.Classname = "ConstantFluxProcess";

            Dictionary<string, double> l_initialList = new Dictionary<string, double>();
            l_initialList.Add("Process:/CELL/CYTOPLASM:R_toy4:Activity", 0.1);

            _unitUnderTest.CreateSimulationParameter(l_parameterID);
            _unitUnderTest.UpdateInitialCondition(l_parameterID, l_modelID, l_initialList);

            _unitUnderTest.DataChanged(modelID, "/CELL/CYTOPLASM:R_toy4", Constants.xpathProcess, p);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDelete()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string key = "/CELL";
            string type = "System";
            EcellObject sys = _unitUnderTest.GetEcellObject(modelID, key, type);
            _unitUnderTest.DataDelete(sys);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDeleteL_modelIDL_keyL_type()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "";
            string key = "";
            string type = "Model";
            // Check ModelID
            _unitUnderTest.DataDelete(modelID, key, type);

            // Check root
            modelID = "Drosophila";
            key = "/";
            type = "System";
            _unitUnderTest.DataDelete(modelID, key, type);

            // Check exist object.
            modelID = "Drosophila";
            key = "/CELL2";
            type = "System";
            _unitUnderTest.DataDelete(modelID, key, type);

            // Check ParameterData/ObservedData.
            _unitUnderTest.SetParameterData(new EcellParameterData("Variable:/CELL/CYTOPLASM:M:Value", 0));
            _unitUnderTest.SetObservedData(new EcellObservedData("Variable:/CELL/CYTOPLASM:M:Value", 0));
            _unitUnderTest.DataDelete(modelID, "/CELL/CYTOPLASM:M", "Variable");

            // Delete model
            _unitUnderTest.DataDelete(modelID, "", "Model");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDeleteL_modelIDL_keyL_typeL_isRecordedL_isAnchor()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string key = "/CELL/CYTOPLASM:P0";
            string type = "Variable";
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataDelete(modelID, key, type, l_isRecorded, l_isAnchor);

            MessageBox.Show("Click \"Cancel\" button on next dialog.");
            key = "/CELL/CYTOPLASM:P1";
            _env.PluginManager.ChangeStatus(ProjectStatus.Suspended);
            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            _unitUnderTest.DataDelete(modelID, key, type, l_isRecorded, l_isAnchor);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataMerge()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            string modelID = "Drosophila";

            // Error root.
            try
            {
                _unitUnderTest.DataMerge(modelID, "/");
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            // Error model.
            try
            {
                _unitUnderTest.DataMerge("Hoge", "/CELL");
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            string key = "/CELL";
            _unitUnderTest.DataMerge(modelID, key);
            Assert.IsNotNull(_unitUnderTest.GetEcellObject(modelID, "/CYTOPLASM", "System"), "DataMerge method returns unexpected result.");

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            EcellObject process = _unitUnderTest.CreateDefaultObject(modelID, "/", "Process");
            _unitUnderTest.DataAdd(process);
            key = "/CELL/CYTOPLASM";
            _unitUnderTest.DataMerge(modelID, key);
            Assert.IsNotNull(_unitUnderTest.GetEcellObject(modelID, "/CELL:P0", "Variable"), "DataMerge method returns unexpected result.");

            // Error already exist.
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            EcellObject newVar = _unitUnderTest.CreateDefaultObject(modelID, "/CELL", "Variable");
            newVar.Key = "/CELL:P0";
            _unitUnderTest.DataAdd(newVar);
            try
            {
                _unitUnderTest.DataMerge(modelID, "/CELL/CYTOPLASM");
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            // Check Simulation Confirm.
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            _env.PluginManager.ChangeStatus(ProjectStatus.Running);
            try
            {
                _unitUnderTest.DataMerge(modelID, "/CELL/CYTOPLASM");
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
        public void TestDeleteSimulationParameterL_parameterID()
        {
            string l_prjID = "NewProject";
            string l_comment = "";
            List<string> l_setDirList = new List<string>();
            _unitUnderTest.CreateNewProject(l_prjID, l_comment, l_setDirList);

            string l_parameterID = "Newparam";
            _unitUnderTest.CreateSimulationParameter(l_parameterID);

            _unitUnderTest.DeleteSimulationParameter(l_parameterID);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSimulationParameterL_parameterIDL_isRecordedL_isAnchor()
        {
            string l_prjID = "Test";
            string l_comment = "";
            List<string> l_setDirList = new List<string>();
            _unitUnderTest.CreateNewProject(l_prjID, l_comment, l_setDirList);
            
            string l_parameterID = "Newparam";
            try
            {
                _unitUnderTest.DeleteSimulationParameter(l_parameterID);

            }
            catch (Exception)
            {
            }

            _unitUnderTest.CreateSimulationParameter(l_parameterID);

            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.SetSimulationParameter(l_parameterID);
            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            MessageBox.Show("Click \"No\" on next dialog.");
            _unitUnderTest.DeleteSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);

            MessageBox.Show("Click \"Yes\" on next dialog.");
            _unitUnderTest.DeleteSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);
            string baseDir = Util.GetBaseDir();
            Util.SetBaseDir(TestConstant.TestDirectory);

            string dir = TestConstant.TestDirectory + "Test";
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            string paramDir = Path.Combine(dir,"Parameters");
            Directory.CreateDirectory(paramDir);
            FileStream stream = File.Create(Path.Combine(paramDir,"_2009_03_06_16_17_18_DefaultParameter.xml"));
            stream.Close();
            try
            {
                _unitUnderTest.DeleteSimulationParameter(Constants.defaultSimParam, l_isRecorded, l_isAnchor);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            _unitUnderTest.CreateSimulationParameter(l_parameterID);
            try
            {
                _unitUnderTest.DeleteSimulationParameter(Constants.defaultSimParam, l_isRecorded, l_isAnchor);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (Directory.Exists(dir))
                    Directory.Delete(dir, true);
                Util.SetBaseDir(baseDir);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteStepperIDL_parameterIDL_stepper()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            EcellObject deStepper = _unitUnderTest.GetEcellObject("Drosophila", "DE", EcellObject.STEPPER);

            try
            {
                _unitUnderTest.DeleteStepperID(deStepper);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID(l_stepper);

            try
            {
                _unitUnderTest.DeleteStepperID(deStepper);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            _unitUnderTest.DeleteStepperID(l_stepper);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestExecuteScript()
        {
            string filename = TestConstant.TestDirectory + "0.ess";
            _unitUnderTest.ExecuteScript(filename);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestExportModel()
        {
            List<string> l_modelIDList = new List<string>();
            string l_fileName = null;
            _unitUnderTest.ExportModel(l_modelIDList, l_fileName);

            l_modelIDList.Add("hoge");
            _unitUnderTest.ExportModel(l_modelIDList, l_fileName);

            if (Directory.Exists(TestConstant.TestDirectory + "hoge"))
                Directory.Delete(TestConstant.TestDirectory + "hoge", true);
            l_fileName = TestConstant.TestDirectory + "hoge/test.eml";
            try
            {
                _unitUnderTest.ExportModel(l_modelIDList, l_fileName);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            l_modelIDList.Clear();
            l_modelIDList.Add("Drosophila");

            l_fileName = TestConstant.TestDirectory + "test.eml";
            _unitUnderTest.ExportModel(l_modelIDList, l_fileName);

            if (File.Exists(l_fileName))
                File.Delete(l_fileName);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCurrentSimulationParameterID()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string expectedString = Constants.defaultSimParam;
            string resultString = "";
            resultString = _unitUnderTest.GetCurrentSimulationParameterID();
            Assert.AreEqual(expectedString, resultString, "GetCurrentSimulationParameterID method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCurrentSimulationTime()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            double expectedDouble = 0;
            double resultDouble = 0;
            resultDouble = _unitUnderTest.GetCurrentSimulationTime();
            Assert.AreEqual(expectedDouble, resultDouble, "GetCurrentSimulationTime method returned unexpected result.");

            _unitUnderTest.CurrentProject.Simulator = null;
            resultDouble = _unitUnderTest.GetCurrentSimulationTime();
            Assert.AreEqual(double.NaN, resultDouble, "GetCurrentSimulationTime method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetData()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_modelID = null;
            string l_key = null;
            List<EcellObject> resultList = null;
            resultList = _unitUnderTest.GetData(l_modelID, l_key);
            Assert.IsNotEmpty(resultList, "GetData method returned unexpected result.");

            l_modelID = "Drosophila";
            l_key = null;
            resultList = _unitUnderTest.GetData(l_modelID, l_key);
            Assert.IsNotEmpty(resultList, "GetData method returned unexpected result.");

            l_modelID = "Drosophila";
            l_key = "/";
            resultList = _unitUnderTest.GetData(l_modelID, l_key);
            Assert.IsNotEmpty(resultList, "GetData method returned unexpected result.");

            l_modelID = "hoge";
            l_key = "/";
            resultList = _unitUnderTest.GetData(l_modelID, l_key);
            Assert.IsNull(resultList, "GetData method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEcellObject()
        {
            string modelId = "Drosophila";
            string key = null;
            string type = null;
            EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetEcellObject(modelId, key, type);
            Assert.IsNull(resultEcellObject, "GetEcellObject method returned unexpected result.");

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            resultEcellObject = _unitUnderTest.GetEcellObject(modelId, key, type);
            Assert.IsNull(resultEcellObject, "GetEcellObject method returned unexpected result.");

            key = "/CELL";
            type = "System";
            resultEcellObject = _unitUnderTest.GetEcellObject(modelId, key, type);
            Assert.AreEqual(EcellObject.SYSTEM, resultEcellObject.Type, "GetEcellObject method returned unexpected result.");
            Assert.AreEqual(key, resultEcellObject.Key, "GetEcellObject method returned unexpected result.");

            key = "/CELL/CYTOPLASM:P0";
            type = "Variable";
            resultEcellObject = _unitUnderTest.GetEcellObject(modelId, key, type);
            Assert.AreEqual(EcellObject.VARIABLE, resultEcellObject.Type, "GetEcellObject method returned unexpected result.");
            Assert.AreEqual(key, resultEcellObject.Key, "GetEcellObject method returned unexpected result.");            
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetPosition()
        {
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

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelId = "Drosophila";
            string key = "/CELL/CYTOPLASM:P0";
            string type = "Variable";
            EcellObject variable = _unitUnderTest.GetEcellObject(modelId, key, type);
            _unitUnderTest.SetPosition(variable);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLogDataL_startTimeL_endTimeL_intervalL_fullID()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            double l_startTime = 0;
            double l_endTime = 10;
            double l_interval = 0;
            string l_fullID = "Variable:/CELL/CYTOPLASM:P0:Value";
            LogData resultLogData = null;
            resultLogData = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval, l_fullID);
            Assert.IsNull(resultLogData, "GetLogData method returned unexpected result.");


            _unitUnderTest.CurrentProject.LogableEntityPathDic.Add("Variable:/CELL/CYTOPLASM:P0:Value", "Drosophila");
            _env.LoggerManager.AddLoggerEntry("Drosophila", "/CELL/CYTOPLASM:P0", "Variable", "Variable:/CELL/CYTOPLASM:P0:Value");
            try
            {
                resultLogData = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval, l_fullID);
            }
            catch (Exception)
            {
            }
            try
            {
                _unitUnderTest.CurrentProject.Simulator = null;
                resultLogData = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval, l_fullID);
            }
            catch (Exception)
            {
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetInitialConditionL_paremterIDL_modelID()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_paremterID = Constants.defaultSimParam;
            string l_modelID = "Drosophila";
            Dictionary<string, double> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetInitialCondition(l_paremterID, l_modelID);
            Assert.IsNotNull(resultDictionary, "GetInitialCondition method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLogDataL_startTimeL_endTimeL_interval()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            double l_startTime = 0;
            double l_endTime = 10;
            double l_interval = 0;
            List<Ecell.LogData> resultList = null;
            resultList = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval);
            Assert.IsNull(resultList, "GetLogData method returned unexpected result.");

            _unitUnderTest.CurrentProject.LogableEntityPathDic.Add("Variable:/CELL/CYTOPLASM:P0:Value", "Drosophila");
            resultList = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval);

            _env.LoggerManager.AddLoggerEntry("Drosophila", "/CELL/CYTOPLASM:P0", "Variable", "Variable:/CELL/CYTOPLASM:P0:Value");
            try
            {
                resultList = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval);
            }
            catch (Exception)
            {
            }
            try
            {
                _unitUnderTest.CurrentProject.Simulator = null;
                resultList = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval);
            }
            catch (Exception)
            {
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLogDataList()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLoggerList()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            List<string> resultList = null;
            try
            {
                resultList = _unitUnderTest.GetLoggerList();
                Assert.IsEmpty(resultList, "GetLoggerList method returned unexpected result.");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLoggerPolicy()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            Type type = _unitUnderTest.GetType();
            MethodInfo info = type.GetMethod("GetCurrentLoggerPolicy", BindingFlags.NonPublic | BindingFlags.Instance);
            LoggerPolicy expectedLoggerPolicy = (LoggerPolicy)info.Invoke(_unitUnderTest, new object[] { });

            string l_parameterID = Constants.defaultSimParam;
            LoggerPolicy resultLoggerPolicy = _unitUnderTest.GetLoggerPolicy(l_parameterID);
            Assert.AreEqual(expectedLoggerPolicy, resultLoggerPolicy, "GetLoggerPolicy method returned unexpected result.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetNextEvent()
        {
            ArrayList resultArrayList = null;
            resultArrayList = _unitUnderTest.GetNextEvent();
            Assert.IsNull(resultArrayList, "GetNextEvent method returned unexpected result.");

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            resultArrayList = _unitUnderTest.GetNextEvent();
            Assert.IsNotNull(resultArrayList, "GetNextEvent method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSystemList()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            List<string> resultList = null;
            string l_modelID = "Drosophila";

            resultList = _unitUnderTest.GetSystemList(l_modelID);
            Assert.IsNotEmpty(resultList, "GetEntityList method returned unexpected result.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEntityList()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_modelID = "Drosophila";
            string l_type = "System";

            List<string> resultList = null;
            resultList = _unitUnderTest.GetEntityList(l_modelID, l_type);
            Assert.IsNotEmpty(resultList, "GetEntityList method returned unexpected result.");

            l_type = "Process";
            resultList = _unitUnderTest.GetEntityList(l_modelID, l_type);
            Assert.IsNotEmpty(resultList, "GetEntityList method returned unexpected result.");

            try
            {
                l_modelID = "hoge";
                resultList = _unitUnderTest.GetEntityList(l_modelID, l_type);
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
        public void TestGetEntityProperty()
        {
            EcellValue resultEcellValue = null;
            string l_fullPN = null;

            try
            {
                resultEcellValue = _unitUnderTest.GetEntityProperty(l_fullPN);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            l_fullPN = "Variable:/CELL/CYTOPLASM:P0:Value";
            resultEcellValue = _unitUnderTest.GetEntityProperty(l_fullPN);
            Assert.IsNotNull(resultEcellValue, "GetEntityProperty method returned unexpected result.");

            _unitUnderTest.CurrentProject.Simulator = null;
            resultEcellValue = _unitUnderTest.GetEntityProperty(l_fullPN);
            Assert.IsNull(resultEcellValue, "GetEntityProperty method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetPropertyValue()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string fullPN = "Variable:/CELL/CYTOPLASM:P0:Value";
            double value = _unitUnderTest.GetPropertyValue(fullPN);

            try
            {
                fullPN = "Variable:/CELL/CYTOPLASM:P0:Hoge";
                value = _unitUnderTest.GetPropertyValue(fullPN);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            try
            {
                fullPN = "Variable:/CELL/CYTOPLASM:P0:Value";
                _unitUnderTest.CurrentProject.Simulator = null;
                value = _unitUnderTest.GetPropertyValue(fullPN);
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
        public void TestGetPropertyValue4Stepper()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            string key = "DE";
            string name = "CurrentTime";
            _unitUnderTest.GetPropertyValue4Stepper(key, name);

            name = "AAA";
            try
            {
                _unitUnderTest.GetPropertyValue4Stepper(key, name);
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
        public void TestGetModelList()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            List<System.String> resultList = null;
            resultList = _unitUnderTest.GetModelList();
            Assert.AreEqual("Drosophila", resultList[0], "GetModelList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsDataExists()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            bool isExist = _unitUnderTest.IsDataExists("Drosophila", "/CELL", "System");
            Assert.AreEqual(true, isExist, "IsDataExists method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsEnableAddProperty()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_dmName = "hoge";
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsEnableAddProperty(l_dmName);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsEnableAddProperty method returned unexpected result.");

            l_dmName = "ExpressionFluxProcess";
            expectedBoolean = true;
            resultBoolean = _unitUnderTest.IsEnableAddProperty(l_dmName);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsEnableAddProperty method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetProcessProperty()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_dmName = "hoge";
            Dictionary<string, EcellData> resultDictionary = null;
            try
            {
                resultDictionary = _unitUnderTest.GetProcessProperty(l_dmName);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            l_dmName = "ExpressionFluxProcess";
            resultDictionary = _unitUnderTest.GetProcessProperty(l_dmName);
            Assert.IsNotEmpty(resultDictionary, "GetProcessProperty method returned unexpected result.");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStepper()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_modelID = "Drosophila";
            List<EcellObject> resultList = null;
            resultList = _unitUnderTest.GetStepper(l_modelID);
            Assert.IsNotEmpty(resultList, "GetStepper method returned unexpected result.");

            try
            {
                _unitUnderTest.CurrentProject.Info.SimulationParam = null;
                resultList = _unitUnderTest.GetStepper(l_modelID + "1");
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
        public void TestGetSimulationParameterIDs()
        {

            List<string> resultList = null;
            resultList = _unitUnderTest.GetSimulationParameterIDs();
            Assert.IsEmpty(resultList, "GetSimulationParameterIDs method returned unexpected result.");

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            resultList = _unitUnderTest.GetSimulationParameterIDs();
            Assert.IsNotEmpty(resultList, "GetSimulationParameterIDs method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStepperProperty()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_dmName = "DAEStepper";
            List<EcellData> resultList = null;
            resultList = _unitUnderTest.GetStepperProperty(l_dmName);
            Assert.IsNotEmpty(resultList, "GetStepperProperty method returned unexpected result.");

            try
            {
                l_dmName = "ExpressionFluxProcess";
                resultList = _unitUnderTest.GetStepperProperty(l_dmName);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            try
            {
                l_dmName = "HogeStepper";
                resultList = _unitUnderTest.GetStepperProperty(l_dmName);
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
        public void TestGetSystemProperty()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            Dictionary<string, EcellData> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetSystemProperty();
            Assert.IsNotEmpty(resultDictionary, "GetSystemProperty method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetTemporaryID()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string type = "System";
            string systemID = "/";
            string expectedString = "/S0";
            string resultString = "";
            resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetTemporaryID method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCopiedID()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string type = "System";
            string systemID = "/CELL";
            string expectedString = "/CELL_copy0";
            string resultString = "";
            resultString = _unitUnderTest.GetCopiedID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetCopiedID method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestInitialize()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            _env.LoggerManager.AddLoggerEntry("Drosophila", "/CELL/CYTOPLASM:P1", "Variable", "Variable:/CELL/CYTOPLASM:P1:Value");

            bool l_flag = false;
            _unitUnderTest.Initialize(l_flag);

            l_flag = true;
            _unitUnderTest.Initialize(l_flag);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadSimulationResult()
        {
            string filename = null;
            LogData data = null;

            try
            {
                data = _unitUnderTest.LoadSimulationResult(filename);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            try
            {
                filename = TestConstant.TestDirectory + "hoge.log";
                data = _unitUnderTest.LoadSimulationResult(filename);
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
        public void TestMessage()
        {
            _unitUnderTest.MessageCreateEntity("", "");
            _unitUnderTest.MessageDeleteEntity("", "");
            _unitUnderTest.MessageUpdateData("", "", "", "");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateDefaultObject()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string modelID = "Drosophila";
            string key = "/HOGE";
            string type = "System";
            EcellObject sys = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(null, sys, "CreateDefaultObject method returned unexpected result.");

            key = "/";
            type = "System";
            EcellObject resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(EcellObject.SYSTEM, resultEcellObject.Type, "CreateDefaultObject method returned unexpected result.");

            key = "/";
            type = "Variable";
            resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(EcellObject.VARIABLE, resultEcellObject.Type, "CreateDefaultObject method returned unexpected result.");

            key = "/";
            type = "Process";
            resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(EcellObject.PROCESS, resultEcellObject.Type, "CreateDefaultObject method returned unexpected result.");

            key = "/Hoge";
            type = "Process";
            resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(null, resultEcellObject, "CreateDefaultObject method returned unexpected result.");

            key = "/";
            type = "Text";
            resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(EcellObject.TEXT, resultEcellObject.Type, "CreateDefaultObject method returned unexpected result.");

            key = "ODEStepepr";
            type = Constants.xpathStepper;
            resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(EcellObject.STEPPER, resultEcellObject.Type, "CreateDefaultObject method returned unexpected result.");

            try
            {
                resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, "hoge", type);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            try
            {
                key = "/Hoge";
                type = "Process";
                resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
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
        public void TestCreateNewRevision()
        {
            _unitUnderTest.CreateNewRevision();
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            _unitUnderTest.CreateNewRevision();
            RemoveRevisions(TestConstant.Project_Drosophila);
        }

        private void RemoveRevisions(string path)
        {
            string project = Path.GetDirectoryName(path);
            string[] revisions = Directory.GetDirectories(project, "Revision*");
            foreach (string revision in revisions)
            {
                Directory.Delete(revision, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateNewProject()
        {
            string l_prjID = "NewProject";
            string l_comment = "";
            List<string> l_setDirList = new List<string>();
            _unitUnderTest.CreateNewProject(l_prjID, l_comment);
            _unitUnderTest.CreateNewProject(l_prjID, l_comment);

            try
            {
                _unitUnderTest.CreateNewProject("", l_comment);
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
        public void TestCreateSimulationParameterL_parameterID()
        {
            string l_prjID = "NewProject";
            string l_comment = "";
            List<string> l_setDirList = new List<string>();
            _unitUnderTest.CreateNewProject(l_prjID, l_comment, l_setDirList);

            string l_parameterID = "Newparam";
            _unitUnderTest.CreateSimulationParameter(l_parameterID);

            try
            {
                _unitUnderTest.CreateSimulationParameter(l_parameterID);
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
        public void TestCreateSimulationParameterL_parameterIDL_isRecordedL_isAnchor()
        {
            string l_parameterID = "Newparam";
            bool l_isRecorded = false;
            bool l_isAnchor = false;

            string l_prjID = "NewProject";
            string l_comment = "";
            List<string> l_setDirList = new List<string>();
            _unitUnderTest.CreateNewProject(l_prjID, l_comment, l_setDirList);
            _unitUnderTest.CreateSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveScript()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_fileName = TestConstant.TestDirectory + "script.ess";
            _unitUnderTest.SaveScript(l_fileName);

            try
            {
                l_fileName = TestConstant.TestDirectory + "hogehoge/script.ess";
                _unitUnderTest.SaveScript(l_fileName);
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
        public void TestSaveProject()
        {
            string basedir = Util.GetBaseDir();
            Util.SetBaseDir(TestConstant.TestDirectory);

            try
            {
                _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
                _unitUnderTest.CurrentProject.Info.Name = "Drosophila2";
                _unitUnderTest.SaveProject();
            }
            catch (Exception)
            {
                Util.SetBaseDir(basedir);
                Assert.Fail();
            }

            try
            {
                _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
                _unitUnderTest.StartStepSimulation(1.0, true);
                _unitUnderTest.CurrentProject.Info.Name = "Drosophila2";
                _unitUnderTest.SaveProject();             
                _unitUnderTest.SimulationStop();
            }
            catch (Exception)
            {
                Util.SetBaseDir(basedir);
                Assert.Fail();
            }

            Util.SetBaseDir(basedir);


        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestExportSBML()
        {
            string basedir = Util.GetBaseDir();
            Util.SetBaseDir(TestConstant.TestDirectory);

            try
            {
                string filename = TestConstant.TestDirectory + TestConstant.SBMLOUT_FILE;
                _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
                _unitUnderTest.ExportSBML(filename);
            }
            catch (Exception)
            {
                Util.SetBaseDir(basedir);
                Assert.Fail();
            }
            Util.SetBaseDir(basedir);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNewRevision()
        {
            string basedir = Util.GetBaseDir();
            Util.SetBaseDir(TestConstant.TestDirectory);

            try
            {
                _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
                _unitUnderTest.CreateNewRevision();
            }
            catch (Exception)
            {
                Util.SetBaseDir(basedir);
                Assert.Fail();
            }
            Util.SetBaseDir(basedir);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadRevision()
        {
            string basedir = Util.GetBaseDir();
            Util.SetBaseDir(TestConstant.TestDirectory);
            _unitUnderTest.LoadRevision("Revision1");

            try
            {
                _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
                _unitUnderTest.CreateNewRevision();
                _unitUnderTest.LoadRevision("Revision1");
                _unitUnderTest.LoadRevision("Revision1");
                RemoveRevisions(TestConstant.Project_Drosophila);
            }
            catch (Exception)
            {
                Util.SetBaseDir(basedir);
                Assert.Fail();
            }
            Util.SetBaseDir(basedir);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveSimulationResult()
        {
            try
            {
                _unitUnderTest.SaveSimulationResult();
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            EcellObject variable = _unitUnderTest.GetEcellObject("Drosophila", "/CELL/CYTOPLASM:M", "Variable");
            EcellData d = variable.GetEcellData(Constants.xpathValue);
            d.Logged = true;
            _unitUnderTest.StartStepSimulation(10.0, true);
            _unitUnderTest.SaveSimulationResult();

            string l_savedDirName = "";
            double l_startTime = 0;
            double l_endTime = 0;
            string l_savedType = "ecd";
            List<string> l_fullIDList = new List<string>();
            _unitUnderTest.SaveSimulationResult(l_savedDirName, l_startTime, l_endTime, l_savedType, l_fullIDList);

            l_fullIDList.Add("Variable:/CELL/CYTOPLASM:M:Value");
            _unitUnderTest.SaveSimulationResult(l_savedDirName, l_startTime, l_endTime, l_savedType, l_fullIDList);
            l_savedType = "csv";
            _unitUnderTest.SaveSimulationResult(l_savedDirName, l_startTime, l_endTime, l_savedType, l_fullIDList);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetEntityProperty()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_fullPN = "";
            string l_value = "";
            _unitUnderTest.SetEntityProperty(l_fullPN, l_value);

            _unitUnderTest.StartStepSimulation(100, true);

            try
            {
                _unitUnderTest.SetEntityProperty(l_fullPN, l_value);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            l_fullPN = "Variable:/CELL/CYTOPLASM:P1:Value";
            l_value = "0.1";
            _unitUnderTest.SetEntityProperty(l_fullPN, l_value);

            l_fullPN = "Variable:/CELL/CYTOPLASM:P1:Name";
            l_value = "hoge";
            _unitUnderTest.SetEntityProperty(l_fullPN, l_value);

            l_fullPN = "Process:/CELL/CYTOPLASM:R_toy1:VariableReferenceList";
            l_value = "hoge";
            _unitUnderTest.SetEntityProperty(l_fullPN, l_value);

            l_fullPN = "Process:/CELL/CYTOPLASM:R_toy1:Priority";
            l_value = "1";
            _unitUnderTest.SetEntityProperty(l_fullPN, l_value);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetLoggerPolicy()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_parameterID = null;
            LoggerPolicy l_loggerPolicy = new LoggerPolicy();
            LoggerPolicy expectedl_loggerPolicy = new LoggerPolicy();
            try
            {
                _unitUnderTest.SetLoggerPolicy(l_parameterID, l_loggerPolicy);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            l_parameterID = "DefaultParameter";
            _unitUnderTest.SetLoggerPolicy(l_parameterID, l_loggerPolicy);
            Assert.IsNotNull(l_loggerPolicy, "l_loggerPolicy ref parameter has unexpected result.");
            Assert.AreNotEqual(expectedl_loggerPolicy, l_loggerPolicy, "l_loggerPolicy ref parameter has unexpected result.");
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCopySimulationParameter()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            _unitUnderTest.CopySimulationParameter("NewParam", Constants.defaultSimParam);
            _unitUnderTest.GetSimulationParameterIDs().Contains("NewParam");
            _unitUnderTest.SetSimulationParameter("NewParam");
            EcellObject obj = _unitUnderTest.GetEcellObject("Drosophila", "/CELL/CYTOPLASM:M", Constants.xpathVariable);
            EcellData d = obj.GetEcellData(Constants.xpathValue);
            d.Value = new EcellValue(12.0);
            _unitUnderTest.DataChanged("Drosophila", "/CELL/CYTOPLASM:M", Constants.xpathVariable, obj);
            _unitUnderTest.CopySimulationParameter("NewParam1", "NewParam");

            try
            {
                _unitUnderTest.CopySimulationParameter("NewParam", Constants.defaultSimParam);
                Assert.Fail();
            }
            catch(Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetSimulationParameterL_parameterID()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_parameterID = null;
            try
            {
                _unitUnderTest.SetSimulationParameter(l_parameterID);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            try
            {
                l_parameterID = "NewParam";
                _unitUnderTest.SetSimulationParameter(l_parameterID);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            l_parameterID = "NewParam";
            string l_modelID = "Drosophila";

            Dictionary<string, double> l_initialList = new Dictionary<string, double>();
            l_initialList.Add("Process:/CELL/CYTOPLASM:R_toy1:Activity", 0.1);
            l_initialList.Add("Process:/CELL/CYTOPLASM:R_toy2:Activity", 0.2);
            l_initialList.Add("Process:/CELL/CYTOPLASM:R_toy3:Activity", 0.3);

            _unitUnderTest.CreateSimulationParameter(l_parameterID);
            _unitUnderTest.UpdateInitialCondition(l_parameterID, l_modelID, l_initialList);

            _unitUnderTest.SetSimulationParameter(l_parameterID);
            _unitUnderTest.SetSimulationParameter(Constants.defaultSimParam);

            MessageBox.Show("Click \"Cancel\" button on next dialog.");
            _env.PluginManager.ChangeStatus(ProjectStatus.Suspended);
            _unitUnderTest.CurrentProject.SimulationStatus = SimulationStatus.Suspended;
            _unitUnderTest.SetSimulationParameter(l_parameterID);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetSimulationParameterL_parameterIDL_isRecordedL_isAnchor()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_parameterID = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            try
            {
                _unitUnderTest.SetSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
            l_parameterID = "NewParam";
            l_isRecorded = true;
            l_isAnchor = true;
            try
            {
                _unitUnderTest.SetSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);
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
        public void TestStartSimulation()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            double time = 1.0;

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(StopTimer);
            // Start simulation.
            timer.Start();
            _unitUnderTest.StartSimulation(time);
            timer.Stop();
            _unitUnderTest.SimulationSuspend();
            timer.Start();
            _unitUnderTest.StartSimulation(time);
            timer.Stop();
            _unitUnderTest.SimulationSuspend();


            Type type = _unitUnderTest.GetType();
            FieldInfo info;
            info = type.GetField("m_remainTime", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(_unitUnderTest, 1);
            timer.Start();
            _unitUnderTest.WaitTime = 1;
            _unitUnderTest.StartSimulation(time);
            timer.Stop();
            _unitUnderTest.SimulationSuspend();

            info = type.GetField("m_remainStep", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(_unitUnderTest, 1);
            timer.Start();
            _unitUnderTest.StartSimulation(time);
            timer.Stop();
            _unitUnderTest.SimulationSuspend();

            // Exception.
            try
            {
                timer.Start();
                _unitUnderTest.StartSimulation(time);
                timer.Stop();
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            // WrappedException.
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            try
            {
                info = type.GetField("m_isStepStepping", BindingFlags.NonPublic | BindingFlags.Instance);
                info.SetValue(_unitUnderTest, true);
                info = type.GetField("m_remainStep", BindingFlags.NonPublic | BindingFlags.Instance);
                info.SetValue(_unitUnderTest, 1);
                timer.Start();
                _unitUnderTest.StartSimulation(time);
                timer.Stop();
                _unitUnderTest.SimulationSuspend();

                time = 10000;
                _unitUnderTest.CurrentProject.Simulator.SetEntityProperty("Process:/CELL/CYTOPLASM:R_toy1:Expression", "hoge");
                timer.Start();
                _unitUnderTest.StartSimulation(time);
                timer.Stop();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            _unitUnderTest.SimulationStop();

            // Test Catch Exception.
            try
            {
                _unitUnderTest.CurrentProject.Simulator = null;
                timer.Start();
                _unitUnderTest.StartSimulation(time);
                timer.Stop();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

        }

        void StopTimer(object sender, EventArgs e)
        {
            _unitUnderTest.SimulationStop();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStartStepSimulation_Step()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            int l_step1 = 1;
            _unitUnderTest.StartStepSimulation(l_step1, true);

            int l_step10 = 10;
            _unitUnderTest.StartStepSimulation(l_step10, true);

            int l_step100 = 100;
            _unitUnderTest.StartStepSimulation(l_step100, true);

            Type type = _unitUnderTest.GetType();
            FieldInfo info;
            info = type.GetField("m_remainTime", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(_unitUnderTest, 10);
            info = type.GetField("m_defaultTime", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(_unitUnderTest, 1);
            _unitUnderTest.StartStepSimulation(l_step10, true);
            // Test Catch WrappedException.
            try
            {
                _unitUnderTest.CurrentProject.Simulator.SetEntityProperty("Process:/CELL/CYTOPLASM:R_toy1:Expression", "hoge");
                _unitUnderTest.StartStepSimulation(l_step10, true);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            // Test Catch Exception.
            try
            {
                _unitUnderTest.CurrentProject.Simulator = null;
                _unitUnderTest.StartStepSimulation(l_step10, true);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            _unitUnderTest.SimulationStop();

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStartStepSimulation_Sec()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            double l_sec1 = 0.5;
            _unitUnderTest.StartStepSimulation(l_sec1, true);

            double l_sec2 = 1.0;
            _unitUnderTest.StartStepSimulation(l_sec2, true);

            double l_sec3 = 5.0;
            _unitUnderTest.StartStepSimulation(l_sec3, true);

            Type type = _unitUnderTest.GetType();
            FieldInfo info;
            info = type.GetField("m_remainTime", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(_unitUnderTest, 10);
            info = type.GetField("m_defaultTime", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(_unitUnderTest, 1);
            _unitUnderTest.StartStepSimulation(l_sec2, true);
            // Test Catch WrappedException.
            try
            {
                _unitUnderTest.CurrentProject.Simulator.SetEntityProperty("Process:/CELL/CYTOPLASM:R_toy1:Expression", "hoge");
                _unitUnderTest.StartStepSimulation(l_sec2, true);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            // Test Catch Exception.
            try
            {
                _unitUnderTest.CurrentProject.Simulator = null;
                _unitUnderTest.StartStepSimulation(l_sec2, true);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }

            _unitUnderTest.SimulationStop();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSimulationStop()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            _unitUnderTest.StartStepSimulation(2000, true);

            _unitUnderTest.SimulationStop();

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSimulationSuspend()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(SuspendTimer);
            timer.Start();

            double l_time = 1.0;
            _unitUnderTest.StartSimulation(l_time);
            timer.Stop();
        }

        void SuspendTimer(object sender, EventArgs e)
        {
            _unitUnderTest.SimulationSuspend();
            _unitUnderTest.SimulationStop();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateInitialCondition()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string l_parameterID = "NewParam";
            string l_modelID = "Drosophila";
            
            Dictionary<string, double> l_initialList = new Dictionary<string,double>();
            l_initialList.Add("Process:/CELL/CYTOPLASM:R_toy1:Activity", 0.1);
            l_initialList.Add("Process:/CELL/CYTOPLASM:R_toy2:Activity", 0.2);
            l_initialList.Add("Process:/CELL/CYTOPLASM:R_toy3:Activity", 0.3);

            _unitUnderTest.CreateSimulationParameter(l_parameterID);
            _unitUnderTest.UpdateInitialCondition(l_parameterID, l_modelID, l_initialList);

            l_initialList.Remove("Process:/CELL/CYTOPLASM:R_toy1:Activity");
            _unitUnderTest.UpdateInitialCondition(l_parameterID, l_modelID, l_initialList);

            _unitUnderTest.UpdateInitialCondition(null, l_modelID, l_initialList);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdatePropertyForDataChanged()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            EcellObject variable = _unitUnderTest.GetEcellObject("Drosophila", "/CELL/CYTOPLASM:P0", "Variable");
            variable.Key = variable.ParentSystemID + ":hoge";
            _unitUnderTest.UpdatePropertyForDataChanged(variable, null);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateStepperIDL_parameterIDL_stepperList()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            string l_stepperName = "ODEStepper";

            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", l_stepperName, EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID( l_stepper);

            _unitUnderTest.UpdateStepperID(l_stepperName, l_stepper, true);

            string l_newParam = "newParam";
            _unitUnderTest.CreateSimulationParameter(l_newParam);
            _unitUnderTest.SetSimulationParameter(l_newParam);

            _unitUnderTest.UpdateStepperID(l_stepperName, l_stepper, true);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetParameterData()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string fullPN = "Variable:/CELL/CYTOPLASM:M:Value";
            Assert.IsFalse(_unitUnderTest.IsContainsParameterData(fullPN));

            EcellParameterData parameterData = new EcellParameterData(fullPN, 0.2);
            _unitUnderTest.SetParameterData(parameterData);
            _unitUnderTest.SetParameterData(parameterData);
            Assert.IsTrue(_unitUnderTest.IsContainsParameterData(fullPN));

            Assert.AreEqual(_unitUnderTest.GetParameterData()[0], _unitUnderTest.GetParameterData(fullPN));

            _unitUnderTest.RemoveParameterData(parameterData);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetObservedData()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string fullPN = "Variable:/CELL/CYTOPLASM:M:Value";
            Assert.IsFalse(_unitUnderTest.IsContainsObservedData(fullPN));

            EcellObservedData observedData = new EcellObservedData(fullPN, 0.2);
            _unitUnderTest.SetObservedData(observedData);
            _unitUnderTest.SetObservedData(observedData);
            Assert.IsTrue(_unitUnderTest.IsContainsObservedData(fullPN));

            Assert.AreEqual(_unitUnderTest.GetObservedData()[0], _unitUnderTest.GetObservedData(fullPN));

            _unitUnderTest.RemoveObservedData(observedData);

        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCheckParameterData()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            string fullPN = "Variable:/CELL/CYTOPLASM:M:Value";
            Assert.IsFalse(_unitUnderTest.IsContainsObservedData(fullPN));

            EcellObservedData observedData = new EcellObservedData(fullPN, 0.2);
            _unitUnderTest.SetObservedData(observedData);
            EcellParameterData parameterData = new EcellParameterData(fullPN, 0.2);
            _unitUnderTest.SetParameterData(parameterData);

            EcellObject variable = _unitUnderTest.GetEcellObject("Drosophila", "/CELL/CYTOPLASM:M", "Variable");
            variable.Key = "/CELL:M";

            _unitUnderTest.DataChanged("Drosophila", "/CELL/CYTOPLASM:M", "Variable", variable);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStep()
        {
            Assert.AreEqual(false, _unitUnderTest.IsSaveStep, "IsSaveStep returned unexpected value.");

            _unitUnderTest.IsSaveStep = true;
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);

            _unitUnderTest.StartStepSimulation(1, true);
            _unitUnderTest.StartStepSimulation(1, true);
            _unitUnderTest.StartStepSimulation(1, true);
            _unitUnderTest.LoadSteppingModel(1);
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUnload()
        {
            _unitUnderTest.LoadProject(TestConstant.Project_Drosophila);
            _unitUnderTest.ReloadSimulator();
            _unitUnderTest.UnloadSimulator();
        }
    }
}
