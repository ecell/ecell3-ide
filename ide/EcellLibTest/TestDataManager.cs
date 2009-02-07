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
    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestDataManager
    {
        private static readonly string ActionFile = Path.Combine(Util.GetUserDir(), "");
        private static readonly string ActionFileUnCorrect = Path.Combine(Util.GetUserDir(), "");

        private DataManager _unitUnderTest;
        /// <summary>
        /// TestFixtureSetUp
        /// </summary>
        [SetUp()]
        public void TestFixtureSetUp()
        {
            _unitUnderTest = new ApplicationEnvironment().DataManager;
        }
        /// <summary>
        /// TestFixtureTearDown
        /// </summary>
        [TearDown()]
        public void TestFixtureTearDown()
        {
            _unitUnderTest = null;
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
        }

        /// <summary>
        /// TestSaveUserAction
        /// </summary>
        [Test()]
        public void TestSaveUserAction()
        {
            string fileName = "";
            _unitUnderTest.SaveUserAction(fileName);
        }
        /// <summary>
        /// TestLoadUserActionFile
        /// </summary>
        [Test()]
        public void TestLoadUserActionFile()
        {
            // Test null
            try
            {
                _unitUnderTest.LoadUserActionFile(null);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Null error.");
                Trace.WriteLine(ex);
            }
            // Test empty.
            try
            {
                _unitUnderTest.LoadUserActionFile("");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Empty error.");
                Trace.WriteLine(ex);
            }
            
            // Test uncorrect file
            try
            {
                _unitUnderTest.LoadUserActionFile(ActionFileUnCorrect);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Trace.WriteLine("");
            }

            // Test correct file.
            _unitUnderTest.LoadUserActionFile(ActionFile);
        }
        /// <summary>
        /// TestAddStepperIDL_parameterIDL_stepper
        /// </summary>
        [Test()]
        public void TestAddStepperIDL_parameterIDL_stepper()
        {
            string l_parameterID = "";
            Ecell.Objects.EcellObject l_stepper = null;
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// TestAddStepperIDL_parameterIDL_stepperL_isRecorded
        /// </summary>
        [Test()]
        public void TestAddStepperIDL_parameterIDL_stepperL_isRecorded()
        {
            string l_parameterID = null;
            Ecell.Objects.EcellObject l_stepper = null;
            bool l_isRecorded = false;
            _unitUnderTest.AddStepperID(l_parameterID, l_stepper, l_isRecorded);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCloseProject()
        {
            _unitUnderTest.CloseProject();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataAddL_ecellObjectListL_isRecordedL_isAnchor()
        {
            System.Collections.Generic.List<Ecell.Objects.EcellObject> l_ecellObjectList = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataAdd(l_ecellObjectList, l_isRecorded, l_isAnchor);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_ecellObjectList()
        {
            System.Collections.Generic.List<Ecell.Objects.EcellObject> l_ecellObjectList = null;
            _unitUnderTest.DataChanged(l_ecellObjectList);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_ecellObjectListL_isRecordedL_isAnchor()
        {
            System.Collections.Generic.List<Ecell.Objects.EcellObject> l_ecellObjectList = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataChanged(l_ecellObjectList, l_isRecorded, l_isAnchor);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_modelIDL_keyL_typeL_ecellObject()
        {
            string l_modelID = null;
            string l_key = null;
            string l_type = null;
            Ecell.Objects.EcellObject l_ecellObject = null;
            _unitUnderTest.DataChanged(l_modelID, l_key, l_type, l_ecellObject);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataChangedL_modelIDL_keyL_typeL_ecellObjectL_isRecordedL_isAnchor()
        {
            string l_modelID = null;
            string l_key = null;
            string l_type = null;
            Ecell.Objects.EcellObject l_ecellObject = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataChanged(l_modelID, l_key, l_type, l_ecellObject, l_isRecorded, l_isAnchor);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDeleteL_modelIDL_keyL_type()
        {
            string l_modelID = null;
            string l_key = null;
            string l_type = null;
            _unitUnderTest.DataDelete(l_modelID, l_key, l_type);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataDeleteL_modelIDL_keyL_typeL_isRecordedL_isAnchor()
        {
            string l_modelID = null;
            string l_key = null;
            string l_type = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DataDelete(l_modelID, l_key, l_type, l_isRecorded, l_isAnchor);
            Assert.Fail("Create or modify test(s).");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDataMerge()
        {
            string modelID = null;
            string key = null;
            _unitUnderTest.DataMerge(modelID, key);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSimulationParameterL_parameterID()
        {
            string l_parameterID = null;
            _unitUnderTest.DeleteSimulationParameter(l_parameterID);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSimulationParameterL_parameterIDL_isRecordedL_isAnchor()
        {
            string l_parameterID = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.DeleteSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteStepperIDL_parameterIDL_stepper()
        {
            string l_parameterID = null;
            Ecell.Objects.EcellObject l_stepper = null;
            _unitUnderTest.DeleteStepperID(l_parameterID, l_stepper);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteStepperIDL_parameterIDL_stepperL_isRecorded()
        {
            string l_parameterID = null;
            Ecell.Objects.EcellObject l_stepper = null;
            bool l_isRecorded = false;
            _unitUnderTest.DeleteStepperID(l_parameterID, l_stepper, l_isRecorded);
            Assert.Fail("Create or modify test(s).");

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
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetCurrentSimulationParameterID();
            Assert.AreEqual(expectedString, resultString, "GetCurrentSimulationParameterID method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCurrentSimulationTime()
        {
            double expectedDouble = 0;
            double resultDouble = 0;
            resultDouble = _unitUnderTest.GetCurrentSimulationTime();
            Assert.AreEqual(expectedDouble, resultDouble, "GetCurrentSimulationTime method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetData()
        {
            string l_modelID = null;
            string l_key = null;
            System.Collections.Generic.List<Ecell.Objects.EcellObject> expectedList = null;
            System.Collections.Generic.List<Ecell.Objects.EcellObject> resultList = null;
            resultList = _unitUnderTest.GetData(l_modelID, l_key);
            Assert.AreEqual(expectedList, resultList, "GetData method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEcellObject()
        {
            string modelId = null;
            string key = null;
            string type = null;
            Ecell.Objects.EcellObject expectedEcellObject = null;
            Ecell.Objects.EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetEcellObject(modelId, key, type);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "GetEcellObject method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

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
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            resultList = _unitUnderTest.GetLoggerList();
            Assert.AreEqual(expectedList, resultList, "GetLoggerList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLoggerPolicy()
        {
            string l_parameterID = null;
            Ecell.LoggerPolicy expectedLoggerPolicy = new LoggerPolicy();
            Ecell.LoggerPolicy resultLoggerPolicy = new LoggerPolicy();
            resultLoggerPolicy = _unitUnderTest.GetLoggerPolicy(l_parameterID);
            Assert.AreEqual(expectedLoggerPolicy, resultLoggerPolicy, "GetLoggerPolicy method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

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
            string l_modelID = null;
            string l_entityName = null;
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            resultList = _unitUnderTest.GetEntityList(l_modelID, l_entityName);
            Assert.AreEqual(expectedList, resultList, "GetEntityList method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

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
        public void TestIsEnableAddProperty()
        {
            string l_dmName = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.IsEnableAddProperty(l_dmName);
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsEnableAddProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetProcessProperty()
        {
            string l_dmName = null;
            System.Collections.Generic.Dictionary<System.String, Ecell.Objects.EcellData> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, Ecell.Objects.EcellData> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetProcessProperty(l_dmName);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetProcessProperty method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetStepper()
        {
            string l_parameterID = null;
            string l_modelID = null;
            System.Collections.Generic.List<Ecell.Objects.EcellObject> expectedList = null;
            System.Collections.Generic.List<Ecell.Objects.EcellObject> resultList = null;
            resultList = _unitUnderTest.GetStepper(l_parameterID, l_modelID);
            Assert.AreEqual(expectedList, resultList, "GetStepper method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

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
            System.Collections.Generic.Dictionary<System.String, Ecell.Objects.EcellData> expectedDictionary = null;
            System.Collections.Generic.Dictionary<System.String, Ecell.Objects.EcellData> resultDictionary = null;
            resultDictionary = _unitUnderTest.GetStepperProperty(l_dmName);
            Assert.AreEqual(expectedDictionary, resultDictionary, "GetStepperProperty method returned unexpected result.");
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
            string modelID = null;
            string key = null;
            string type = null;
            Ecell.Objects.EcellObject expectedEcellObject = null;
            Ecell.Objects.EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.CreateDefaultObject(modelID, key, type);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "CreateDefaultObject method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateNewProject()
        {
            string l_prjID = null;
            string l_comment = null;
            string l_projectPath = null;
            System.Collections.Generic.List<System.String> l_setDirList = null;
            _unitUnderTest.CreateNewProject(l_prjID, l_comment, l_projectPath, l_setDirList);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateSimulationParameterL_parameterID()
        {
            string l_parameterID = null;
            _unitUnderTest.CreateSimulationParameter(l_parameterID);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCreateSimulationParameterL_parameterIDL_isRecordedL_isAnchor()
        {
            string l_parameterID = null;
            bool l_isRecorded = false;
            bool l_isAnchor = false;
            _unitUnderTest.CreateSimulationParameter(l_parameterID, l_isRecorded, l_isAnchor);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveScript()
        {
            string l_fileName = null;
            _unitUnderTest.SaveScript(l_fileName);
            Assert.Fail("Create or modify test(s).");

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
            double l_time = 1.0;
            _unitUnderTest.StartSimulation(l_time);
            Assert.Fail("Can not start the simulation.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStartStepSimulation_Step()
        {
            int l_step1 = 1;
            _unitUnderTest.StartStepSimulation(l_step1);
            Assert.Fail("Error step simulation by step[1].");

            int l_step10 = 10;
            _unitUnderTest.StartStepSimulation(l_step10);
            Assert.Fail("Error step simulation by step[10].");

            int l_step100 = 100;
            _unitUnderTest.StartStepSimulation(l_step100);
            Assert.Fail("Error step simulation by step[100].");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStartStepSimulation_Sec()
        {
            double l_sec1 = 0.5;
            _unitUnderTest.StartStepSimulation(l_sec1);
            Assert.Fail("Error step simulation by sec[0.5].");

            double l_sec2 = 1.0;
            _unitUnderTest.StartStepSimulation(l_sec2);
            Assert.Fail("Error step simulation by sec[1.0].");

            double l_sec3 = 5.0;
            _unitUnderTest.StartStepSimulation(l_sec3);
            Assert.Fail("Error step simulation by sec[5.0].");
        }


        ///// <summary>
        ///// 
        ///// </summary>
        //[Test()]
        //public void TestSimulationStartL_timeLimitL_statusNum()
        //{
        //    double l_timeLimit = 0;
        //    int l_statusNum = 0;
            
        //    _unitUnderTest.SimulationStart(l_timeLimit, l_statusNum);
        //    Assert.Fail("Create or modify test(s).");

        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //[Test()]
        //public void TestSimulationStartKeepSettingL_stepLimit()
        //{
        //    int l_stepLimit = 0;
        //    _unitUnderTest.SimulationStartKeepSetting(l_stepLimit);
        //    Assert.Fail("Create or modify test(s).");

        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //[Test()]
        //public void TestSimulationStartKeepSettingL_timeLimit()
        //{
        //    double l_timeLimit = 0;
        //    _unitUnderTest.SimulationStartKeepSetting(l_timeLimit);
        //    Assert.Fail("Create or modify test(s).");

        //}
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSimulationStop()
        {
            _unitUnderTest.SimulationStop();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSimulationSuspend()
        {
            _unitUnderTest.SimulationSuspend();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateInitialCondition()
        {
            string l_parameterID = null;
            string l_modelID = null;
            System.Collections.Generic.Dictionary<System.String, System.Double> l_initialList = null;
            _unitUnderTest.UpdateInitialCondition(l_parameterID, l_modelID, l_initialList);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateStepperIDL_parameterIDL_stepperList()
        {
            string l_parameterID = null;
            System.Collections.Generic.List<Ecell.Objects.EcellObject> l_stepperList = null;
            _unitUnderTest.UpdateStepperID(l_parameterID, l_stepperList);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdateStepperIDL_parameterIDL_stepperListL_isRecorded()
        {
            string l_parameterID = null;
            System.Collections.Generic.List<Ecell.Objects.EcellObject> l_stepperList = null;
            bool l_isRecorded = false;
            _unitUnderTest.UpdateStepperID(l_parameterID, l_stepperList, l_isRecorded);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
