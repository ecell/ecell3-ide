namespace Ecell
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestActionManager
    {

        private ActionManager _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            Ecell.ApplicationEnvironment env = null;
            _unitUnderTest = new ActionManager(env);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorActionManager()
        {
            Ecell.ApplicationEnvironment env = null;
            ActionManager testActionManager = new ActionManager(env);
            Assert.IsNotNull(testActionManager, "Constructor of type, ActionManager failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAddAction()
        {
            Ecell.UserAction u = null;
            _unitUnderTest.AddAction(u);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetAction()
        {
            int index = 0;
            Ecell.UserAction expectedUserAction = null;
            Ecell.UserAction resultUserAction = null;
            resultUserAction = _unitUnderTest.GetAction(index);
            Assert.AreEqual(expectedUserAction, resultUserAction, "GetAction method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestLoadActionFile()
        {
            string fileName = null;
            _unitUnderTest.LoadActionFile(fileName);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestSaveActionFile()
        {
            string fileName = null;
            _unitUnderTest.SaveActionFile(fileName);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestUndoAction()
        {
            _unitUnderTest.UndoAction();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRedoAction()
        {
            _unitUnderTest.RedoAction();
            Assert.Fail("Create or modify test(s).");

        }
    }
}
