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
            Ecell.ProjectInfo info = null;
            Project testProject = new Project(info, _env);
            Assert.IsNotNull(testProject, "Constructor of type, Project failed to create instance.");

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
        public void TestCreateSimulatorInstance()
        {
            EcellCoreLib.WrappedSimulator expectedWrappedSimulator = null;
            EcellCoreLib.WrappedSimulator resultWrappedSimulator = null;
            //resultWrappedSimulator = _unitUnderTest.CreateSimulatorInstance();
            Assert.AreEqual(expectedWrappedSimulator, resultWrappedSimulator, "CreateSimulatorInstance method returned unexpected result.");

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

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCopyDMDirs()
        {
            System.Collections.Generic.List<System.String> dmDirs = null;
            _unitUnderTest.CopyDMDirs(dmDirs);

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
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            //resultList = _unitUnderTest.GetSavableModel();
            Assert.AreEqual(expectedList, resultList, "GetSavableModel method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSavableSimulationParameter()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            //resultList = _unitUnderTest.GetSavableSimulationParameter();
            Assert.AreEqual(expectedList, resultList, "GetSavableSimulationParameter method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSavableSimulationResult()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            //resultList = _unitUnderTest.GetSavableSimulationResult();
            Assert.AreEqual(expectedList, resultList, "GetSavableSimulationResult method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetTemporaryIDModelIDTypeSystemID()
        {
            string modelID = null;
            string type = null;
            string systemID = null;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetTemporaryID(modelID, type, systemID);
            Assert.AreEqual(expectedString, resultString, "GetTemporaryID method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCopiedID()
        {
            string modelID = null;
            string type = null;
            string key = null;
            string expectedString = null;
            string resultString = null;
            resultString = _unitUnderTest.GetCopiedID(modelID, type, key);
            Assert.AreEqual(expectedString, resultString, "GetCopiedID method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEcellObject()
        {
            string model = null;
            string type = null;
            string key = null;
            Ecell.Objects.EcellObject expectedEcellObject = null;
            Ecell.Objects.EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetEcellObject(model, type, key);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "GetEcellObject method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetSystem()
        {
            string model = null;
            string key = null;
            Ecell.Objects.EcellObject expectedEcellObject = null;
            Ecell.Objects.EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetSystem(model, key);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "GetSystem method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEntity()
        {
            string model = null;
            string key = null;
            string type = null;
            Ecell.Objects.EcellObject expectedEcellObject = null;
            Ecell.Objects.EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetEntity(model, key, type);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "GetEntity method returned unexpected result.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSystem()
        {
            Ecell.Objects.EcellObject system = null;
            _unitUnderTest.AddSystem(system);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddEntity()
        {
            Ecell.Objects.EcellObject entity = null;
            _unitUnderTest.AddEntity(entity);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSystem()
        {
            Ecell.Objects.EcellObject system = null;
            _unitUnderTest.DeleteSystem(system);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteEntity()
        {
            Ecell.Objects.EcellObject entity = null;
            _unitUnderTest.DeleteEntity(entity);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddSimulationParameter()
        {
            Ecell.Objects.EcellObject eo = null;
            _unitUnderTest.AddSimulationParameter(eo);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDeleteSimulationParameter()
        {
            Ecell.Objects.EcellObject eo = null;
            _unitUnderTest.DeleteSimulationParameter(eo);

        }
    }
}
