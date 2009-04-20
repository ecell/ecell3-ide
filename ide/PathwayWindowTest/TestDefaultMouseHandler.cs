namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestDefaultMouseHandler
    {

        private DefaultMouseHandler _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            _unitUnderTest = new DefaultMouseHandler(control);
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
        public void TestConstructorDefaultMouseHandler()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            DefaultMouseHandler testDefaultMouseHandler = new DefaultMouseHandler(control);
            Assert.IsNotNull(testDefaultMouseHandler, "Constructor of type, DefaultMouseHandler failed to create instance.");
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
        public void TestOnMouseUp()
        {
            object sender = null;
            UMD.HCIL.Piccolo.Event.PInputEventArgs e = null;
            _unitUnderTest.OnMouseUp(sender, e);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestOnMouseDrag()
        {
            object sender = null;
            UMD.HCIL.Piccolo.Event.PInputEventArgs e = null;
            _unitUnderTest.OnMouseDrag(sender, e);
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
    }
}
