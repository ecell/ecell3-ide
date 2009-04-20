namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestCreateNodeMouseHandler
    {

        private CreateNodeMouseHandler _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            Ecell.IDE.Plugins.PathwayWindow.Components.ComponentSetting cs = null;
            _unitUnderTest = new CreateNodeMouseHandler(control, cs);
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
        public void TestConstructorCreateNodeMouseHandler()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            Ecell.IDE.Plugins.PathwayWindow.Components.ComponentSetting cs = null;
            CreateNodeMouseHandler testCreateNodeMouseHandler = new CreateNodeMouseHandler(control, cs);
            Assert.IsNotNull(testCreateNodeMouseHandler, "Constructor of type, CreateNodeMouseHandler failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDoesAcceptEvent()
        {
            UMD.HCIL.Piccolo.Event.PInputEventArgs e = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.DoesAcceptEvent(e);
            Assert.AreEqual(expectedBoolean, resultBoolean, "DoesAcceptEvent method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestOnMouseMove()
        {
            object sender = null;
            UMD.HCIL.Piccolo.Event.PInputEventArgs e = null;
            _unitUnderTest.OnMouseMove(sender, e);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestOnMouseDown()
        {
            object sender = null;
            UMD.HCIL.Piccolo.Event.PInputEventArgs e = null;
            _unitUnderTest.OnMouseDown(sender, e);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestInitialize()
        {
            _unitUnderTest.Initialize();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestReset()
        {
            _unitUnderTest.Reset();
            Assert.Fail("Create or modify test(s).");

        }
    }
}
