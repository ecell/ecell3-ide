namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;


    [TestFixture()]
    public class TestReportingSession
    {

        private ReportingSession _unitUnderTest;

        [SetUp()]
        public void SetUp()
        {
            string group = null;
            Ecell.Reporting.ReportManager rm = null;
            _unitUnderTest = new ReportingSession(group, rm);
        }

        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        [Test()]
        public void TestConstructorReportingSession()
        {
            string group = null;
            Ecell.Reporting.ReportManager rm = null;
            ReportingSession testReportingSession = new ReportingSession(group, rm);
            Assert.IsNotNull(testReportingSession, "Constructor of type, ReportingSession failed to create instance.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestDispose()
        {
            _unitUnderTest.Dispose();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestAdd()
        {
            Ecell.Reporting.IReport item = null;
            _unitUnderTest.Add(item);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Clear();
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestContains()
        {
            Ecell.Reporting.IReport item = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.Contains(item);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Contains method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestGetEnumerator()
        {
            System.Collections.Generic.IEnumerator<Ecell.Reporting.IReport> expectedIEnumerator = null;
            System.Collections.Generic.IEnumerator<Ecell.Reporting.IReport> resultIEnumerator = null;
            resultIEnumerator = _unitUnderTest.GetEnumerator();
            Assert.AreEqual(expectedIEnumerator, resultIEnumerator, "GetEnumerator method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestIndexOf()
        {
            Ecell.Reporting.IReport item = null;
            int expectedInt32 = 1;
            int resultInt32 = 1;
            resultInt32 = _unitUnderTest.IndexOf(item);
            Assert.AreEqual(expectedInt32, resultInt32, "IndexOf method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestInsert()
        {
            int idx = 1;
            Ecell.Reporting.IReport item = null;
            _unitUnderTest.Insert(idx, item);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRemoveAt()
        {
            int idx = 1;
            _unitUnderTest.RemoveAt(idx);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestRemove()
        {
            Ecell.Reporting.IReport item = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.Remove(item);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Remove method returned unexpected result.");
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestCopyTo()
        {
            Ecell.Reporting.IReport[] a = null;
            int idx = 1;
            _unitUnderTest.CopyTo(a, idx);
            Assert.Fail("Create or modify test(s).");

        }

        [Test()]
        public void TestClose()
        {
            _unitUnderTest.Close();
            Assert.Fail("Create or modify test(s).");

        }
    }
}
