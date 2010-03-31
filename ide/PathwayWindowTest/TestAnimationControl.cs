namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestAnimationControl
    {
        /// <summary>
        /// 
        /// </summary>
        private AnimationControl _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            _unitUnderTest = new AnimationControl(control);
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
        public void TestConstructorAnimationControl()
        {
            Ecell.IDE.Plugins.PathwayWindow.PathwayControl control = null;
            AnimationControl testAnimationControl = new AnimationControl(control);
            Assert.IsNotNull(testAnimationControl, "Constructor of type, AnimationControl failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDispose()
        {
            _unitUnderTest.Dispose();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestTimerFire()
        {
            object sender = null;
            System.EventArgs e = null;
            _unitUnderTest.TimerFire(sender, e);
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStartSimulation()
        {
            _unitUnderTest.StartSimulation();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestPauseSimulation()
        {
            _unitUnderTest.PauseSimulation();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStopSimulation()
        {
            _unitUnderTest.StopSimulation();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestTimerStart()
        {
            _unitUnderTest.TimerStart();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestTimerStop()
        {
            _unitUnderTest.TimerStop();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetPropForSimulation()
        {
            _unitUnderTest.SetAnimation();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestUpdatePropForSimulation()
        {
            _unitUnderTest.UpdateAnimation();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestResetPropForSimulation()
        {
            _unitUnderTest.ResetAnimation();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSaveSettings()
        {
            _unitUnderTest.SaveSettings();
            Assert.Fail("Create or modify test(s).");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoadSettings()
        {
            _unitUnderTest.LoadDefaultSettings();
            Assert.Fail("Create or modify test(s).");

        }
    }
}
