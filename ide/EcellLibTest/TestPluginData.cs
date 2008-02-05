namespace EcellLib
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestPluginData
    {

        private PluginData _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new PluginData();
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorPluginData()
        {
            PluginData testPluginData = new PluginData();
            Assert.IsNotNull(testPluginData, "Constructor of type, PluginData failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestConstructorPluginDataIdKey()
        {
            string id = null;
            string key = null;
            PluginData testPluginData = new PluginData(id, key);
            Assert.IsNotNull(testPluginData, "Constructor of type, PluginData failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestEquals()
        {
            EcellLib.PluginData obj = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.Equals(obj);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
