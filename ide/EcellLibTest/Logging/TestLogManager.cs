//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
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

namespace Ecell.Logging
{
    using System;
    using NUnit.Framework;
    using System.Diagnostics;

    /// <summary>
    /// TestLogManager
    /// </summary>
    [TestFixture()]
    public class TestLogManager
    {

        private LogManager _unitUnderTest;

        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            ApplicationEnvironment env = new ApplicationEnvironment();
            _unitUnderTest = new LogManager(env);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestConstructorLogManager()
        {
            ApplicationEnvironment env = new ApplicationEnvironment();
            LogManager testLogManager = new LogManager(env);
            Assert.IsNotNull(testLogManager, "Constructor of type, LogManager failed to create instance.");
            Assert.AreEqual(0, testLogManager.Count, "Count is unexpected value.");
        }

        /// <summary>
        /// TestAppend
        /// </summary>
        [Test()]
        public void TestAppend()
        {
            ILogEntry entry = new ApplicationLogEntry(MessageType.Information, "Information", new object());
            _unitUnderTest.Append(entry);

            _unitUnderTest.LogEntryAppended += new LogEntryAppendedEventHandler(_unitUnderTest_LogEntryAppended);
            _unitUnderTest.Append(entry);
            Assert.AreEqual(entry, _unitUnderTest[0], "Count is unexpected value.");
        }

        void _unitUnderTest_LogEntryAppended(object o, LogEntryEventArgs e)
        {
            Trace.WriteLine("LogManager.LogEntryAppendedEvent");
        }
    }
}
