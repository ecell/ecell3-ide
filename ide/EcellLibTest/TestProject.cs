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
    using Ecell.Exceptions;
    using Ecell.Objects;
    using System.Reflection;
    using System.IO;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestProject
    {
        private string Oscillation = "c:/temp/Oscillation.eml";
        private string Drosophila = "c:/temp/Drosophila/project.xml";
        private string RBC = "c:/temp/rbc.eml";

        private Project _unitUnderTest;
        private ApplicationEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            ProjectInfo info = ProjectInfoLoader.Load(Drosophila);
            _unitUnderTest = new Project(info, _env);
            _unitUnderTest.LoadModel();
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
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorProject()
        {
            _unitUnderTest.Close();

            Project testProject = null;
            Ecell.ProjectInfo info = null;
            try
            {
                testProject = new Project(info, _env);
                Assert.Fail("Error param");
            }
            catch (EcellException)
            {
            }
            try
            {
                info = ProjectInfoLoader.Load(Oscillation);
                testProject = new Project(info, null);
                Assert.Fail("Error param");
            }
            catch (EcellException)
            {
            }

            info = ProjectInfoLoader.Load(Oscillation);
            testProject = new Project(info, _env);
            testProject.LoadModel();
            Assert.IsNotNull(testProject, "Constructor of type, Project failed to create instance.");

            Assert.IsNotEmpty(testProject.DmDic, "DmDic is unexpected value.");
            Assert.IsNotEmpty(testProject.StepperDmList, "StepperDmList is unexpected value.");
            Assert.IsNotEmpty(testProject.SystemDmList, "SystemDmList is unexpected value.");
            Assert.IsNotEmpty(testProject.ProcessDmList, "ProcessDmList is unexpected value.");
            Assert.IsNotEmpty(testProject.VariableDmList, "VariableDmList is unexpected value.");

            Assert.IsNotEmpty(testProject.InitialCondition, "InitialCondition is unexpected value.");
            Assert.IsNull(testProject.LogableEntityPathDic, "LogableEntityPathDic is unexpected value.");
            testProject.LogableEntityPathDic = new Dictionary<string, string>();
            Assert.IsNotNull(testProject.LogableEntityPathDic, "LogableEntityPathDic is unexpected value.");
            Assert.IsNotEmpty(testProject.LoggerPolicyDic, "LoggerPolicyDic is unexpected value.");
            Assert.IsNotNull(testProject.LoggerPolicy, "LoggerPolicy is unexpected value.");

            Assert.IsNotEmpty(testProject.ModelList, "ModelList is unexpected value.");
            Assert.IsNotNull(testProject.Model, "Model is unexpected value.");
            Assert.IsNotEmpty(testProject.StepperDic, "StepperDic is unexpected value.");
            Assert.IsNotEmpty(testProject.SystemDic, "SystemDic is unexpected value.");
            Assert.IsNotEmpty(testProject.SystemList, "SystemList is unexpected value.");
            Assert.IsNotEmpty(testProject.ProcessList, "ProcessList is unexpected value.");
            Assert.IsNotEmpty(testProject.VariableList, "VariableList is unexpected value.");
            Assert.IsNotEmpty(testProject.TextList, "TextList is unexpected value.");

            Assert.AreEqual(SimulationStatus.Wait, testProject.SimulationStatus, "SimulationStatus is unexpected value.");
            testProject.SimulationStatus = SimulationStatus.Suspended;
            Assert.AreEqual(SimulationStatus.Suspended, testProject.SimulationStatus, "SimulationStatus is unexpected value.");

            Assert.AreEqual(info, testProject.Info, "Info is unexpected value.");
            Assert.IsNotNull(testProject.Simulator, "Simulator is unexpected value.");
            testProject.Simulator = null;
            Assert.IsNull(testProject.Simulator, "Simulator is unexpected value.");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetSimParams()
        {
            string param = null;
            try
            {
                _unitUnderTest.SetSimParams(param);
                Assert.Fail("");
            }
            catch (Exception)
            {
            }
            try
            {
                param = "newSimParam";
                _unitUnderTest.Info.SimulationParam = null;
                _unitUnderTest.SetSimParams(param);
                Assert.Fail("");
            }
            catch (Exception)
            {
            }
            param = "newModelID";
            _unitUnderTest.Info.SimulationParam = Constants.defaultSimParam;
            _unitUnderTest.SetSimParams(param);
            Assert.IsNotNull(_unitUnderTest.InitialCondition[Constants.defaultSimParam][param], "SimulationParam is unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetDMList()
        {
            _unitUnderTest.SetDMList();

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSortSystems()
        {
            _unitUnderTest.SortSystems();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClose()
        {
            _unitUnderTest.Close();

            _unitUnderTest.Simulator = new EcellCoreLib.WrappedSimulator(Util.GetDMDirs(null));
            string path = Util.GetNewDir(_unitUnderTest.Info.ProjectPath);
            _unitUnderTest.Info.ProjectPath = path;
            _unitUnderTest.Close();

            Directory.CreateDirectory(path);
            _unitUnderTest.Simulator = new EcellCoreLib.WrappedSimulator(Util.GetDMDirs(null));
            _unitUnderTest.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCopyDMDirs()
        {
            string basedir = Util.GetBaseDir();
            Util.SetBaseDir("c:/temp");
            string path = Util.GetNewDir(_unitUnderTest.Info.ProjectPath);

            List<string> dmDirs = new List<string>();
            dmDirs.Add(Path.Combine(_unitUnderTest.Info.ProjectPath, Constants.DMDirName));
            _unitUnderTest.Info.ProjectPath = path;
            _unitUnderTest.Info.Name = Path.GetFileName(path);
            _unitUnderTest.CopyDMDirs(dmDirs);

            Directory.Delete(path, true);
            Util.SetBaseDir(basedir);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSave()
        {
            _unitUnderTest.Save();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSavableModel()
        {
            Type type = _unitUnderTest.GetType();
            MethodInfo methodInfo = type.GetMethod("GetSavableModel", BindingFlags.NonPublic | BindingFlags.Instance);

            List<string> expectedList = new List<string>();
            expectedList.Add("Drosophila");
            List<string> resultList = null;

            resultList = (List<string>)methodInfo.Invoke(_unitUnderTest, new object[] { });
            //resultList = _unitUnderTest.GetSavableModel();
            Assert.AreEqual(expectedList, resultList, "GetSavableModel method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSavableSimulationParameter()
        {
            Type type = _unitUnderTest.GetType();
            MethodInfo methodInfo = type.GetMethod("GetSavableSimulationParameter", BindingFlags.NonPublic | BindingFlags.Instance);

            List<string> expectedList = new List<string>();
            expectedList.Add("DefaultParameter");
            List<string> resultList = null;

            resultList = (List<string>)methodInfo.Invoke(_unitUnderTest, new object[] { });
            //resultList = _unitUnderTest.GetSavableSimulationParameter();
            Assert.AreEqual(expectedList, resultList, "GetSavableSimulationParameter method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSavableSimulationResult()
        {
            Type type = _unitUnderTest.GetType();
            MethodInfo methodInfo = type.GetMethod("GetSavableSimulationResult", BindingFlags.NonPublic | BindingFlags.Instance);

            List<string> expectedList = new List<string>();
            expectedList.Add(Constants.xpathParameters + Constants.xpathResult);
            List<string> resultList = null;

            resultList = (List<string>)methodInfo.Invoke(_unitUnderTest, new object[] { });
            //resultList = _unitUnderTest.GetSavableSimulationResult();
            Assert.AreEqual(expectedList, resultList, "GetSavableSimulationResult method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetTemporaryIDModelIDTypeSystemID()
        {
            string modelID = "Drosophila";
            string type = "Variable";
            string systemID = "/CELL/CYTOPLASM";
            string expectedString = "/CELL/CYTOPLASM:V0";
            string resultString = null;
            resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetTemporaryID method returned unexpected result.");

            type = "Process";
            expectedString = "/CELL/CYTOPLASM:P0";
            resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetTemporaryID method returned unexpected result.");

            systemID = "/";
            type = "System";
            expectedString = "/S0";
            resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetTemporaryID method returned unexpected result.");

            type = "Text";
            expectedString = "/:Text0";
            resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetTemporaryID method returned unexpected result.");

            try
            {
                type = "Hoge";
                resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCopiedID()
        {
            string modelID = "Drosophila";
            string key = "/CELL/CYTOPLASM:P0";
            string type = "Variable";
            string expectedString = "/CELL/CYTOPLASM:P0_copy0";
            string resultString = null;
            resultString = _unitUnderTest.GetCopiedID(modelID, type, key);
            Assert.AreEqual(expectedString, resultString, "GetCopiedID method returned unexpected result.");

            EcellObject entity = EcellObject.CreateObject(modelID, resultString, type, type, new List<EcellData>());
            _unitUnderTest.AddEntity(entity);

            expectedString = "/CELL/CYTOPLASM:P0_copy1";
            resultString = _unitUnderTest.GetCopiedID(modelID, type, resultString);
            Assert.AreEqual(expectedString, resultString, "GetCopiedID method returned unexpected result.");

            key = "/CELL";
            type = "System";
            expectedString = "/CELL_copy0";
            resultString = _unitUnderTest.GetCopiedID(modelID, type, key);
            Assert.AreEqual(expectedString, resultString, "GetCopiedID method returned unexpected result.");

            key = "/CELL/CYTOPLASM:R_toy1";
            type = "Process";
            expectedString = "/CELL/CYTOPLASM:R_toy1_copy0";
            resultString = _unitUnderTest.GetCopiedID(modelID, type, key);
            Assert.AreEqual(expectedString, resultString, "GetCopiedID method returned unexpected result.");

            key = "/:Text";
            type = "Text";
            expectedString = "/:Text_copy0";
            resultString = _unitUnderTest.GetCopiedID(modelID, type, key);
            Assert.AreEqual(expectedString, resultString, "GetCopiedID method returned unexpected result.");

            try
            {
                type = "Hoge";
                resultString = _unitUnderTest.GetCopiedID(modelID, type, key);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEcellObject()
        {
            string model = "Drosophila";
            string key = "";
            string type = "Model";
            EcellObject resultEcellObject = null;

            // Model
            resultEcellObject = _unitUnderTest.GetEcellObject(model, type, key);
            Assert.IsNotNull(resultEcellObject, "GetEcellObject method returned unexpected result.");
            Assert.AreEqual(model, resultEcellObject.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual(key, resultEcellObject.Key, "Key is unexpected value.");
            Assert.AreEqual(type, resultEcellObject.Type, "Type is unexpected value.");

            // Stepper
            model = "Drosophila";
            key = "";
            type = "Stepper";

            resultEcellObject = _unitUnderTest.GetEcellObject(model, type, key);
            Assert.IsNull(resultEcellObject, "GetEcellObject method returned unexpected result.");

            // System
            model = "Drosophila";
            key = "/CELL";
            type = "System";

            resultEcellObject = _unitUnderTest.GetEcellObject(model, type, key);
            Assert.IsNotNull(resultEcellObject, "GetEcellObject method returned unexpected result.");
            Assert.AreEqual(model, resultEcellObject.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual(key, resultEcellObject.Key, "Key is unexpected value.");
            Assert.AreEqual(type, resultEcellObject.Type, "Type is unexpected value.");

            // Variable
            model = "Drosophila";
            key = "/CELL/CYTOPLASM:P1";
            type = "Variable";

            resultEcellObject = _unitUnderTest.GetEcellObject(model, type, key);
            Assert.IsNotNull(resultEcellObject, "GetEcellObject method returned unexpected result.");
            Assert.AreEqual(model, resultEcellObject.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual(key, resultEcellObject.Key, "Key is unexpected value.");
            Assert.AreEqual(type, resultEcellObject.Type, "Type is unexpected value.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSystem()
        {
            string model = "Drosophila";
            string key = "/";
            EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetSystem(model, key);
            Assert.IsNotNull(resultEcellObject, "GetSystem method returned unexpected result.");
            Assert.AreEqual(model, resultEcellObject.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual(key, resultEcellObject.Key, "Key is unexpected value.");

            model = "hoge";
            resultEcellObject = _unitUnderTest.GetSystem(model, key);
            Assert.IsNull(resultEcellObject, "GetSystem method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEntity()
        {
            string model = "Drosophila";
            string key = "/CELL/CYTOPLASM:P0";
            string type = "Variable";
            EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetEntity(model, key, type);
            Assert.IsNotNull(resultEcellObject, "GetSystem method returned unexpected result.");
            Assert.AreEqual(model, resultEcellObject.ModelID, "ModelID is unexpected value.");
            Assert.AreEqual(key, resultEcellObject.Key, "Key is unexpected value.");
            Assert.AreEqual(type, resultEcellObject.Type, "Type is unexpected value.");

            key = "/CELL/CYTOPLASM1:P0";
            resultEcellObject = _unitUnderTest.GetEntity(model, key, type);
            Assert.IsNull(resultEcellObject, "GetSystem method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSystem()
        {
            string modelID = "Drosophila";
            string syskey = "/";
            string type = "System";
            string newKey = _unitUnderTest.GetTemporaryID(modelID, type, syskey);

            EcellObject system = EcellObject.CreateObject(modelID, newKey, type, type, new List<EcellData>());
            _unitUnderTest.AddSystem(system);

            newKey = _unitUnderTest.GetTemporaryID(modelID, type, syskey);
            system = EcellObject.CreateObject(modelID, newKey, type, type, new List<EcellData>());

            newKey = _unitUnderTest.GetTemporaryID(modelID, "Variable", system.Key);
            EcellObject var = EcellObject.CreateObject(modelID, newKey, "Variable", "Variable", new List<EcellData>());
            system.Children.Add(var);
            _unitUnderTest.AddSystem(system);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddEntity()
        {
            string modelID = "Drosophila";
            string key = "/CELL/CYTOPLASM:P0";
            string type = "Variable";
            string newKey = _unitUnderTest.GetCopiedID(modelID, type, key);

            EcellObject entity = EcellObject.CreateObject(modelID, newKey, type, type, new List<EcellData>());
            _unitUnderTest.AddEntity(entity);

            newKey = _unitUnderTest.GetCopiedID(modelID, type, key);
            entity = EcellObject.CreateObject(modelID, newKey, type, type, new List<EcellData>());
            _unitUnderTest.AddEntity(entity);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSystem()
        {
            EcellObject system = _unitUnderTest.GetSystem("Drosophila", "/CELL/CYTOPLASM");
            _unitUnderTest.DeleteSystem(system);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteEntity()
        {
            EcellObject entity = _unitUnderTest.GetEntity("Drosophila", "/CELL/CYTOPLASM:P0", "Variable");
            _unitUnderTest.DeleteEntity(entity);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSimulationParameter()
        {
            EcellObject system = _unitUnderTest.GetSystem("Drosophila", "/CELL/CYTOPLASM").Clone();
            system.Key = _unitUnderTest.GetCopiedID(system.ModelID, system.Type, system.Key);
            _unitUnderTest.AddSimulationParameter(system);

            EcellObject entity = _unitUnderTest.GetEntity("Drosophila", "/CELL/CYTOPLASM:P0", "Variable").Clone();
            entity.Key = _unitUnderTest.GetCopiedID(entity.ModelID, entity.Type, entity.Key);
            _unitUnderTest.AddSimulationParameter(entity);

            entity = _unitUnderTest.GetEntity("Drosophila", "/CELL/CYTOPLASM:R_toy1", "Process").Clone();
            entity.Key = _unitUnderTest.GetCopiedID(entity.ModelID, entity.Type, entity.Key);
            _unitUnderTest.AddSimulationParameter(entity);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSimulationParameter()
        {
            EcellObject system = _unitUnderTest.GetSystem("Drosophila", "/CELL/CYTOPLASM");
            _unitUnderTest.DeleteSimulationParameter(system);

        }
    }
}