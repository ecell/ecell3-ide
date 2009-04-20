namespace Ecell.IDE.Plugins.PathwayWindow
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestHandle
    {

        private Handle _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.IDE.Plugins.PathwayWindow.Mode mode = Mode.Select;
            UMD.HCIL.Piccolo.Event.PBasicInputEventHandler handler = null;
            _unitUnderTest = new Handle(mode, handler);
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
        public void TestConstructorHandleModeHandler()
        {
            Ecell.IDE.Plugins.PathwayWindow.Mode mode = Mode.Select;
            UMD.HCIL.Piccolo.Event.PBasicInputEventHandler handler = null;
            Handle testHandle = new Handle(mode, handler);
            Assert.IsNotNull(testHandle, "Constructor of type, Handle failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestConstructorHandleModeZoomingRate()
        {
            Ecell.IDE.Plugins.PathwayWindow.Mode mode = Mode.Select;
            float zoomingRate = 0;
            Handle testHandle = new Handle(mode, zoomingRate);
            Assert.IsNotNull(testHandle, "Constructor of type, Handle failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
    }
}
