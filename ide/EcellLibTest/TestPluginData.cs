namespace EcellLib
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestPluginData
    {

        private PluginData _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new PluginData();
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
        public void TestConstructorPluginData()
        {
            PluginData testPluginData = new PluginData();
            Assert.IsNotNull(testPluginData, "Constructor of type, PluginData failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorPluginDataIdKey()
        {
            string id = null;
            string key = null;
            PluginData testPluginData = new PluginData(id, key);
            Assert.IsNotNull(testPluginData, "Constructor of type, PluginData failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
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
