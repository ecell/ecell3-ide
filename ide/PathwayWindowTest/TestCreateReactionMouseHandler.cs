namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestCreateReactionMouseHandler
    {

        private CreateReactionMouseHandler _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            _unitUnderTest = new CreateReactionMouseHandler(control);
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
        public void TestConstructorCreateReactionMouseHandler()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            CreateReactionMouseHandler testCreateReactionMouseHandler = new CreateReactionMouseHandler(control);
            Assert.IsNotNull(testCreateReactionMouseHandler, "Constructor of type, CreateReactionMouseHandler failed to create instance.");
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
        public void TestSetStartNode()
        {
            Ecell.IDE.Plugins.PathwayWindow.Nodes.PPathwayNode obj = null;
            _unitUnderTest.SetStartNode(obj);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetStartNode()
        {
            _unitUnderTest.ResetStartNode();
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
