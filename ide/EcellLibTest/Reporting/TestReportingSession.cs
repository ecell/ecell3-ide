//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Collections;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestReportingSession
    {
        private ReportingSession _unitUnderTest;
        private ApplicationEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();

            string group = "Group";
            ReportManager rm = _env.ReportManager;
            _unitUnderTest = rm.GetReportingSession(group);
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
        public void TestConstructorReportingSession()
        {
            string group = "Group";
            ReportManager rm = new ReportManager(_env);
            ReportingSession testReportingSession = new ReportingSession(group, rm);
            Assert.IsNotNull(testReportingSession, "Constructor of type, ReportingSession failed to create instance.");
            Assert.AreEqual(false, testReportingSession.IsReadOnly, "IsReadOnly is unexpected value.");
            Assert.AreEqual(group, testReportingSession.Group, "Group is unexpected value.");
            Assert.AreEqual(0, testReportingSession.Count, "Count is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestDispose()
        {
            _unitUnderTest.Dispose();

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAdd()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            CompileReport item = new CompileReport(type, message, group);
            _unitUnderTest.Add(item);
            _unitUnderTest.Add(item);

            IEnumerator<IReport> list = _unitUnderTest.GetEnumerator();
            Assert.IsTrue(_unitUnderTest.Count > 0, "Count is unexpected value.");
            Assert.AreEqual(item, _unitUnderTest[0], "Group is unexpected value.");

            try
            {
                _unitUnderTest[0] = item;
                Assert.Fail("Setter of RepportingSession must return an exception.");
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClear()
        {
            try
            {
                _env.ReportManager.Cleared += new ReportClearEventHandler(ReportManager_Cleared);
                _unitUnderTest.Clear();
                Assert.Fail("Clear method must return an exception.");
            }
            catch (Exception)
            {
            }

        }

        void ReportManager_Cleared(object o, EventArgs e)
        {
            ;
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestContains()
        {
            Ecell.Reporting.IReport item = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.Contains(item);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Contains method returned unexpected result.");

            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            item = new CompileReport(type, message, group);
            _unitUnderTest.Add(item);

            expectedBoolean = true;
            resultBoolean = _unitUnderTest.Contains(item);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Contains method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetEnumerator()
        {
            IEnumerator<IReport> resultIEnumerator = _unitUnderTest.GetEnumerator();
            Assert.AreEqual(null, resultIEnumerator.Current, "GetEnumerator method returned unexpected result.");

            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            IReport item = new CompileReport(type, message, group);
            _unitUnderTest.Add(item);

            resultIEnumerator = _unitUnderTest.GetEnumerator();
            Assert.AreEqual(null, resultIEnumerator.Current, "GetEnumerator method returned unexpected result.");
            resultIEnumerator.MoveNext();
            Assert.AreEqual(item, resultIEnumerator.Current, "GetEnumerator method returned unexpected result.");

            resultIEnumerator = (IEnumerator<IReport>)((IEnumerable)_unitUnderTest).GetEnumerator();
            Assert.AreEqual(null, resultIEnumerator.Current, "GetEnumerator method returned unexpected result.");
            resultIEnumerator.MoveNext();
            Assert.AreEqual(item, resultIEnumerator.Current, "GetEnumerator method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestIndexOf()
        {
            Ecell.Reporting.IReport item = null;
            int expectedInt32 = -1;
            int resultInt32 = 1;
            resultInt32 = _unitUnderTest.IndexOf(item);
            Assert.AreEqual(expectedInt32, resultInt32, "IndexOf method returned unexpected result.");

            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            item = new CompileReport(type, message, group);
            _unitUnderTest.Add(item);
            expectedInt32 = 0;
            resultInt32 = _unitUnderTest.IndexOf(item);
            Assert.AreEqual(expectedInt32, resultInt32, "IndexOf method returned unexpected result.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestInsert()
        {
            int idx = 0;

            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            CompileReport item = new CompileReport(type, message, group);
            _unitUnderTest.Insert(idx, item);

            IEnumerator<IReport> list = _unitUnderTest.GetEnumerator();
            Assert.IsTrue(_unitUnderTest.Count > 0, "Count is unexpected value.");
            Assert.AreEqual(item, _unitUnderTest[0], "Group is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRemoveAt()
        {
            int idx = 0;

            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            CompileReport item = new CompileReport(type, message, group);
            _unitUnderTest.Add(item);

            _unitUnderTest.RemoveAt(idx);

            IEnumerator<IReport> list = _unitUnderTest.GetEnumerator();
            Assert.IsTrue(_unitUnderTest.Count == 0, "Count is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestRemove()
        {
            Ecell.Reporting.IReport item = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = _unitUnderTest.Remove(item);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Remove method returned unexpected result.");

            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            item = new CompileReport(type, message, group);
            _unitUnderTest.Add(item);

            expectedBoolean = true;
            resultBoolean = _unitUnderTest.Remove(item);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Remove method returned unexpected result.");

            IEnumerator<IReport> list = _unitUnderTest.GetEnumerator();
            Assert.IsTrue(_unitUnderTest.Count == 0, "Count is unexpected value.");


        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestCopyTo()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            string group = "Group";
            IReport item = new CompileReport(type, message, group);
            _unitUnderTest.Add(item);

            IReport[] a = new IReport[1];
            int idx = 0;
            _unitUnderTest.CopyTo(a, idx);

            IEnumerator<IReport> list = _unitUnderTest.GetEnumerator();
            Assert.IsTrue(a.Length > 0, "Count is unexpected value.");
            Assert.AreEqual(item, a[0], "Group is unexpected value.");

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClose()
        {
            _unitUnderTest.Close();
        }
    }
}
