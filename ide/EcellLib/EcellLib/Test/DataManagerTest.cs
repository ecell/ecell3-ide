using System;
using System.Collections.Generic;
using System.Text;
using EcellCoreLib;
using NUnit.Framework;
using EcellLib;

namespace test
{
    [TestFixture]
    public class DataManagerTest
    {
        private DataManager m_dManager;

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Test()
        {
            m_dManager = DataManager.GetDataManager();
            string project = "testProject";
            m_dManager.CreateProject(project, "comment", null, new List<string>());
            m_dManager.SaveProject(project);
            Assert.AreEqual(project, m_dManager.CurrentProjectID);
            m_dManager.CloseProject(project);
            Assert.IsNull(m_dManager.CurrentProjectID);
        }
    }
}
