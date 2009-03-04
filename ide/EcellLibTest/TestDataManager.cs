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
    using System.Windows.Forms;
    using NUnit.Framework;
    using System.IO;
    using System.Diagnostics;
    using Ecell.Objects;
    using System.Collections.Generic;
    using Ecell.Exceptions;
    using EcellCoreLib;
    using System.Reflection;
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
                filename = "c:\\hoge\\hoge.eml";
                _unitUnderTest.LoadProject(filename);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            // Load RBC
            filename = "c:\\temp\\rbc.eml";
            _unitUnderTest.LoadProject(filename);
            // Load Drosophila
            filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);
        }

        /// <summary>
        /// TestAddStepperIDL_parameterIDL_stepper
        /// </summary>
        [Test()]
        public void TestGetDir()
        {
            // Load Drosophila
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string dir = _unitUnderTest.GetDMDir();
            List<string> list = _unitUnderTest.GetDMNameList();
            string dmName = _unitUnderTest.GetDMFileName("");
        }

        /// <summary>
        /// TestAddStepperIDL_parameterIDL_stepper
        /// </summary>
        [Test()]
        public void TestAddStepperIDL_parameterIDL_stepper()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_parameterID = "DefaultParameter";
            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper);

            try
            {
                _unitUnderTest.AddStepperID(l_parameterID, l_stepper);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }

            try
            {
                _unitUnderTest.AddStepperID(null, l_stepper);
                Assert.Fail();
            }
            catch (EcellException)
            {
            }

            try
            {
                _unitUnderTest.AddStepperID("newParam", l_stepper);
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
        public void TestAddStepperIDL_parameterIDL_stepperL_isRecorded()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_parameterID = "DefaultParameter";
            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            bool l_isRecorded = false;
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper, l_isRecorded);

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

            WrappedSimulator simulator = new WrappedSimulator(Util.GetDMDirs(null));
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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);
            _unitUnderTest.CloseProject();

            try
            {
                _unitUnderTest.LoadProject(filename);
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
        public void TestDataAddL_ecellObjectListL_isRecordedL_isAnchor()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            EcellObject sys = _unitUnderTest.CreateDefaultObject(modelID, key, type);

            List<EcellObject> l_ecellObjectList = new List<EcellObject>();
            l_ecellObjectList.Add(sys);
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataAdd(l_ecellObjectList, l_isRecorded, l_isAnchor);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_ecellObjectList()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string modelID = "Drosophila";
            string key = "/";
            string type = "System";
            EcellObject sys = _unitUnderTest.GetEcellObject(modelID, key, type);
            bool l_isRecorded = false;
            bool l_isAnchor = false;

            _unitUnderTest.DataChanged(modelID, key, type, sys, l_isRecorded, l_isAnchor);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDeleteL_modelIDL_keyL_type()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string modelID = "Drosophila";
            string key = "/CELL";
            string type = "System";
            _unitUnderTest.DataDelete(modelID, key, type);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDeleteL_modelIDL_keyL_typeL_isRecordedL_isAnchor()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string modelID = "Drosophila";
            string key = "/CELL";
            string type = "System";
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataDelete(modelID, key, type, l_isRecorded, l_isAnchor);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataMerge()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string modelID = "Drosophila";
            string key = "/CELL";
            _unitUnderTest.DataMerge(modelID, key);

            _unitUnderTest.LoadProject(filename);
            key = "/CELL/CYTOPLASM";
            _unitUnderTest.DataMerge(modelID, key);
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
            string l_prjID = "NewProject";
            string l_comment = "";
            List<string> l_setDirList = new List<string>();
            _unitUnderTest.CreateNewProject(l_prjID, l_comment, l_setDirList);

            string l_parameterID = "Newparam";
            _unitUnderTest.CreateSimulationParameter(l_parameterID);

            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DeleteSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);
            
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteStepperIDL_parameterIDL_stepper()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_parameterID = "DefaultParameter";
            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper);

            _unitUnderTest.DeleteStepperID(l_parameterID, l_stepper);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteStepperIDL_parameterIDL_stepperL_isRecorded()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_parameterID = "DefaultParameter";
            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper);

            bool l_isRecorded = false;
            _unitUnderTest.DeleteStepperID(l_parameterID, l_stepper, l_isRecorded);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestExportModel()
        {
            System.Collections.Generic.List<System.String> l_modelIDList = null;
            string l_fileName = null;
            _unitUnderTest.ExportModel(l_modelIDList, l_fileName);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCurrentSimulationParameterID()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string expectedString = "DefaultParameter";
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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            double expectedDouble = 0;
            double resultDouble = 0;
            resultDouble = _unitUnderTest.GetCurrentSimulationTime();
            Assert.AreEqual(expectedDouble, resultDouble, "GetCurrentSimulationTime method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetData()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEcellObject()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string modelId = "Drosophila";
            string key = null;
            string type = null;
            EcellObject resultEcellObject = null;
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
        public void TestGetLogDataL_startTimeL_endTimeL_intervalL_fullID()
        {
            double l_startTime = 0;
            double l_endTime = 0;
            double l_interval = 0;
            string l_fullID = null;
            Ecell.LogData expectedLogData = null;
            Ecell.LogData resultLogData = null;
            resultLogData = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval, l_fullID);
            Assert.AreEqual(expectedLogData, resultLogData, "GetLogData method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetInitialConditionL_paremterIDL_modelID()
        {
            string l_paremterID = null;
            string l_modelID = null;
            System.Collections.Generic.Dictionary<System.String, System.Double> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Double> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetInitialCondition(l_paremterID, l_modelID);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetInitialCondition method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetInitialConditionL_paremterIDL_modelIDL_type()
        {
            string l_paremterID = null;
            string l_modelID = null;
            System.Collections.Generic.Dictionary<System.String, System.Double> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, System.Double> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetInitialCondition(l_paremterID, l_modelID);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetInitialCondition method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLogDataL_startTimeL_endTimeL_interval()
        {
            double l_startTime = 0;
            double l_endTime = 0;
            double l_interval = 0;
            System.Collections.Generic.List<Ecell.LogData> expectedList = null;
            System.Collections.Generic.List<Ecell.LogData> resultList = null;
            resultList = _unitUnderTest.GetLogData(l_startTime, l_endTime, l_interval);
            Assert.AreEqual(expectedList, resultList, "GetLogData method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLoggerList()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            //List<string> resultList = null;
            //try
            //{
            //    resultList = _unitUnderTest.GetLoggerList();
            //    Assert.IsEmpty(resultList, "GetLoggerList method returned unexpected result.");
            //}
            //catch (Exception e)
            //{
            //    Trace.WriteLine(e.StackTrace);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLoggerPolicy()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            Type type = _unitUnderTest.GetType();
            MethodInfo info = type.GetMethod("GetCurrentLoggerPolicy", BindingFlags.NonPublic | BindingFlags.Instance);
            LoggerPolicy expectedLoggerPolicy = (LoggerPolicy)info.Invoke(_unitUnderTest, new object[] { });

            string l_parameterID = "DefaultParameter";
            LoggerPolicy resultLoggerPolicy = _unitUnderTest.GetLoggerPolicy(l_parameterID);
            Assert.AreEqual(expectedLoggerPolicy, resultLoggerPolicy, "GetLoggerPolicy method returned unexpected result.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetNextEvent()
        {
            System.Collections.ArrayList expectedArrayList = null;
            System.Collections.ArrayList resultArrayList = null;
            resultArrayList = _unitUnderTest.GetNextEvent();
            Assert.AreEqual(expectedArrayList, resultArrayList, "GetNextEvent method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEntityList()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_modelID = "Drosophila";
            string l_type = "System";

            List<string> resultList = null;
            resultList = _unitUnderTest.GetEntityList(l_modelID, l_type);
            Assert.IsNotEmpty(resultList, "GetEntityList method returned unexpected result.");

            l_type = "Process";
            resultList = _unitUnderTest.GetEntityList(l_modelID, l_type);
            Assert.IsNotEmpty(resultList, "GetEntityList method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEntityProperty()
        {
            string l_fullPN = null;
            Ecell.Objects.EcellValue expectedEcellValue = null;
            Ecell.Objects.EcellValue resultEcellValue = null;
            resultEcellValue = _unitUnderTest.GetEntityProperty(l_fullPN);
            Assert.AreEqual(expectedEcellValue, resultEcellValue, "GetEntityProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetModelList()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            resultList = _unitUnderTest.GetModelList();
            Assert.AreEqual(expectedList, resultList, "GetModelList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsDataExists()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            bool isExist = _unitUnderTest.IsDataExists("Drosophila", "/CELL", "System");
            Assert.AreEqual(true, isExist, "IsDataExists method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIsEnableAddProperty()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);


            string l_parameterID = "DefaultParameter";
            string l_modelID = "Drosophila";
            List<EcellObject> resultList = null;
            resultList = _unitUnderTest.GetStepper(l_parameterID, l_modelID);
            Assert.IsNotEmpty(resultList, "GetStepper method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSimulationParameterIDs()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            resultList = _unitUnderTest.GetSimulationParameterIDs();
            Assert.AreEqual(expectedList, resultList, "GetSimulationParameterIDs method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStepperProperty()
        {
            string l_dmName = null;
            System.Collections.Generic.List<Ecell.Objects.EcellData> expectedList = null;
            System.Collections.Generic.List<Ecell.Objects.EcellData> resultList = null;
            resultList = _unitUnderTest.GetStepperProperty(l_dmName);
            Assert.AreEqual(expectedList, resultList, "GetStepperProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSystemProperty()
        {
            System.Collections.Generic.Dictionary<System.String, Ecell.Objects.EcellData> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, Ecell.Objects.EcellData> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetSystemProperty();
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetSystemProperty method returned unexpected result.");
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
        public void TestInitialize()
        {
            bool l_flag = false;
            _unitUnderTest.Initialize(l_flag);
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadSimulationParameter()
        {
            string l_fileName = null;
            _unitUnderTest.LoadSimulationParameter(l_fileName);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateDefaultObject()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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

            try
            {
                resultEcellObject = _unitUnderTest.CreateDefaultObject("Hoge", key, type);
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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_fileName = "c:/temp/script.ess";
            _unitUnderTest.SaveScript(l_fileName);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveSimulationResult()
        {
            string l_savedDirName = null;
            double l_startTime = 0;
            double l_endTime = 0;
            string l_savedType = null;
            System.Collections.Generic.List<System.String> l_fullIDList = null;
            _unitUnderTest.SaveSimulationResult(l_savedDirName, l_startTime, l_endTime, l_savedType, l_fullIDList);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetEntityProperty()
        {
            string l_fullPN = null;
            string l_value = null;
            _unitUnderTest.SetEntityProperty(l_fullPN, l_value);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetLoggerPolicy()
        {
            string l_parameterID = null;
            Ecell.LoggerPolicy l_loggerPolicy = new LoggerPolicy();
            Ecell.LoggerPolicy expectedl_loggerPolicy = new LoggerPolicy();
            _unitUnderTest.SetLoggerPolicy(l_parameterID, l_loggerPolicy);
            Assert.AreEqual(expectedl_loggerPolicy, l_loggerPolicy, "l_loggerPolicy ref parameter has unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetSimulationParameterL_parameterID()
        {
            string l_parameterID = null;
            _unitUnderTest.SetSimulationParameter(l_parameterID);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetSimulationParameterL_parameterIDL_isRecordedL_isAnchor()
        {
            string l_parameterID = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.SetSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStartSimulation()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(StopTimer);
            timer.Start();
            double l_time = 1.0;
            _unitUnderTest.StartSimulation(l_time);

            timer.Stop();
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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            int l_step1 = 1;
            _unitUnderTest.StartStepSimulation(l_step1);

            int l_step10 = 10;
            _unitUnderTest.StartStepSimulation(l_step10);

            int l_step100 = 100;
            _unitUnderTest.StartStepSimulation(l_step100);

            _unitUnderTest.SimulationStop();

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStartStepSimulation_Sec()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            double l_sec1 = 0.5;
            _unitUnderTest.StartStepSimulation(l_sec1);

            double l_sec2 = 1.0;
            _unitUnderTest.StartStepSimulation(l_sec2);

            double l_sec3 = 5.0;
            _unitUnderTest.StartStepSimulation(l_sec3);

            _unitUnderTest.SimulationStop();

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSimulationStop()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            _unitUnderTest.StartStepSimulation(2000);

            _unitUnderTest.SimulationStop();

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSimulationSuspend()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_parameterID = "DefaultParameter";
            string l_modelID = "Drosophila";
            Dictionary<string, double> l_initialList = new Dictionary<string,double>();
            foreach(KeyValuePair<string, double> value in _unitUnderTest.CurrentProject.InitialCondition[l_parameterID][l_modelID])
                l_initialList.Add(value.Key, value.Value);
            _unitUnderTest.UpdateInitialCondition(null, l_modelID, l_initialList);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateStepperIDL_parameterIDL_stepperList()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_parameterID = "DefaultParameter";
            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper);

            List<Ecell.Objects.EcellObject> l_stepperList = new List<EcellObject>();
            l_stepperList.Add(l_stepper);
            _unitUnderTest.UpdateStepperID(l_parameterID, l_stepperList);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateStepperIDL_parameterIDL_stepperListL_isRecorded()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string l_parameterID = "DefaultParameter";
            EcellObject l_stepper = EcellObject.CreateObject("Drosophila", "ODEStepper", EcellObject.STEPPER, "ODEStepper", new List<EcellData>());
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper);

            List<Ecell.Objects.EcellObject> l_stepperList = new List<EcellObject>();
            l_stepperList.Add(l_stepper);

            bool l_isRecorded = false;
            _unitUnderTest.UpdateStepperID(l_parameterID, l_stepperList, l_isRecorded);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetParameterData()
        {
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

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
            string filename = "c:/temp/Drosophila/project.xml";
            _unitUnderTest.LoadProject(filename);

            string fullPN = "Variable:/CELL/CYTOPLASM:M:Value";
            Assert.IsFalse(_unitUnderTest.IsContainsObservedData(fullPN));

            EcellObservedData observedData = new EcellObservedData(fullPN, 0.2);
            _unitUnderTest.SetObservedData(observedData);
            _unitUnderTest.SetObservedData(observedData);
            Assert.IsTrue(_unitUnderTest.IsContainsObservedData(fullPN));

            Assert.AreEqual(_unitUnderTest.GetObservedData()[0], _unitUnderTest.GetObservedData(fullPN));

            _unitUnderTest.RemoveObservedData(observedData);

        }
    }
}
