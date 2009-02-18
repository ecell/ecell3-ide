namespace Ecell
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestProject
    {

        private Project _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.ProjectInfo info = null;
            _unitUnderTest = new Project(info);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorProject()
        {
            Ecell.ProjectInfo info = null;
            Project testProject = new Project(info);
            Assert.IsNotNull(testProject, "Constructor of type, Project failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestInitialize()
        {
            string modelID = null;
            _unitUnderTest.Initialize(modelID);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSetDMList()
        {
            _unitUnderTest.SetDMList();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestCreateSimulatorInstance()
        {
            EcellCoreLib.WrappedSimulator expectedWrappedSimulator = null;
            EcellCoreLib.WrappedSimulator resultWrappedSimulator = null;
            //resultWrappedSimulator = _unitUnderTest.CreateSimulatorInstance();
            Assert.AreEqual(expectedWrappedSimulator, resultWrappedSimulator, "CreateSimulatorInstance method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSortSystems()
        {
            _unitUnderTest.SortSystems();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClose()
        {
            _unitUnderTest.Close();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestCopyDMDirs()
        {
            System.Collections.Generic.List<System.String> dmDirs = null;
            _unitUnderTest.CopyDMDirs(dmDirs);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSave()
        {
            _unitUnderTest.Save();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetSavableModel()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            //resultList = _unitUnderTest.GetSavableModel();
            Assert.AreEqual(expectedList, resultList, "GetSavableModel method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetSavableSimulationParameter()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            //resultList = _unitUnderTest.GetSavableSimulationParameter();
            Assert.AreEqual(expectedList, resultList, "GetSavableSimulationParameter method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetSavableSimulationResult()
        {
            System.Collections.Generic.List<System.String> expectedList = null;
            System.Collections.Generic.List<System.String> resultList = null;
            //resultList = _unitUnderTest.GetSavableSimulationResult();
            Assert.AreEqual(expectedList, resultList, "GetSavableSimulationResult method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

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
            Assert.Fail("Create or modify test(s).");

        }

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
            Assert.Fail("Create or modify test(s).");

        }

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
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetSystem()
        {
            string model = null;
            string key = null;
            Ecell.Objects.EcellObject expectedEcellObject = null;
            Ecell.Objects.EcellObject resultEcellObject = null;
            resultEcellObject = _unitUnderTest.GetSystem(model, key);
            Assert.AreEqual(expectedEcellObject, resultEcellObject, "GetSystem method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

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
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAddSystem()
        {
            Ecell.Objects.EcellObject system = null;
            _unitUnderTest.AddSystem(system);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAddEntity()
        {
            Ecell.Objects.EcellObject entity = null;
            _unitUnderTest.AddEntity(entity);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestDeleteSystem()
        {
            Ecell.Objects.EcellObject system = null;
            _unitUnderTest.DeleteSystem(system);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestDeleteEntity()
        {
            Ecell.Objects.EcellObject entity = null;
            _unitUnderTest.DeleteEntity(entity);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAddSimulationParameter()
        {
            Ecell.Objects.EcellObject eo = null;
            _unitUnderTest.AddSimulationParameter(eo);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestDeleteSimulationParameter()
        {
            Ecell.Objects.EcellObject eo = null;
            _unitUnderTest.DeleteSimulationParameter(eo);
            Assert.Fail("Create or modify test(s).");

        }
    }
}
