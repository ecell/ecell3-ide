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

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestReportManager
    {

        private ReportManager _unitUnderTest;
        private ApplicationEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = new ReportManager(_env);
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
        public void TestConstructorReportManager()
        {
            ReportManager testReportManager = new ReportManager(_env);
            Assert.IsNotNull(testReportManager, "Constructor of type, ReportManager failed to create instance.");
            Assert.AreEqual(_env, testReportManager.Environment, "Environment is unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClear()
        {
            _unitUnderTest.Cleared += new ReportClearEventHandler(_unitUnderTest_Cleared);
            _unitUnderTest.ReportingSessionClosed += new ReportingSessionClosedEventHandler(_unitUnderTest_ReportingSessionClosed);
            _unitUnderTest.Clear();

            try
            {
                _unitUnderTest.GetReportingSession("Group");
                _unitUnderTest.Clear();
                Assert.Fail("GetReportingSession must throw exception.");
            }
            catch (Exception)
            {
            }

        }

        void _unitUnderTest_Cleared(object o, EventArgs e)
        {
        }

        void _unitUnderTest_ReportingSessionClosed(object o, ReportingSessionEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetReportingSession()
        {
            string group = "Group";
            ReportingSession session = _unitUnderTest.GetReportingSession(group);
            Assert.IsNotNull(session, "GetReportingSession method returned unexpected result.");
            session.Dispose();

            _unitUnderTest.ReportingSessionStarted += new ReportingSessionStartedEventHandler(_unitUnderTest_ReportingSessionStarted);
            session = _unitUnderTest.GetReportingSession(group);

            try
            {
                session = _unitUnderTest.GetReportingSession(group);
                Assert.Fail("GetReportingSession must throw exception.");
            }
            catch (Exception)
            {
            }
        }

        void _unitUnderTest_ReportingSessionStarted(object o, ReportingSessionEventArgs e)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddAndRemoveReport()
        {
            _unitUnderTest.ReportAdded += new ReportAddedEventHandler(_unitUnderTest_ReportAdded);
            _unitUnderTest.ReportRemoved += new ReportRemovedEventHandler(_unitUnderTest_ReportRemoved);
            string group = "Group";
            ReportingSession session = _unitUnderTest.GetReportingSession(group);

            Ecell.MessageType type = MessageType.Error;
            string message = "Compile Error: Hoge";
            CompileReport item = new CompileReport(type, message, group);

            session.Add(item);
            session.Remove(item);
        }

        void _unitUnderTest_ReportAdded(object o, ReportEventArgs e)
        {
        }

        void _unitUnderTest_ReportRemoved(object o, ReportEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetStatus()
        {
            Ecell.StatusBarMessageKind type = StatusBarMessageKind.QuickInspector;
            string text = null;
            _unitUnderTest.StatusUpdated += new StatusUpdatedEventHandler(_unitUnderTest_StatusUpdated);
            _unitUnderTest.SetStatus(type, text);

        }

        void _unitUnderTest_StatusUpdated(object o, StatusUpdateEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetProgress()
        {
            int value = 10;
            _unitUnderTest.ProgressValueUpdated += new ProgressReportEventHandler(_unitUnderTest_ProgressValueUpdated);
            _unitUnderTest.SetProgress(value);

        }

        void _unitUnderTest_ProgressValueUpdated(object o, ProgressReportEventArgs e)
        {
        }
    }
}
