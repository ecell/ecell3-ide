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
            m_dManager = DataManager.GetDataManager();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Test()
        {
            m_dManager.CreateProject("testProject", "comment", null, new List<string>());
            m_dManager.SaveProject("testProject");
        }
    }
}
