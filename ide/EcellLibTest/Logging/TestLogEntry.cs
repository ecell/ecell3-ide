//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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

    /// <summary>
    /// TestLogEntry
    /// </summary>
    [TestFixture()]
    public class TestLogEntry
    {

        private LogEntry _unitUnderTest;
        /// <summary>
        /// Constructor.
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.MessageType type = MessageType.Information;
            string message = "Information.";
            _unitUnderTest = new ApplicationLogEntry(type, message, new object());
        }
        /// <summary>
        /// Destructor.
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
        public void TestConstructorLogEntryTypeMessage()
        {
            string expectedMessage = "";
            MessageType expectedType = MessageType.Warning;
            DateTime expectedTimestamp = DateTime.Now;
            ApplicationLogEntry testLogEntry = new ApplicationLogEntry(MessageType.Warning, null, new object());
            Assert.IsNotNull(testLogEntry, "Constructor of type, LogEntry failed to create instance.");
            Assert.AreEqual(expectedType, testLogEntry.Type, "Type is unexpected value.");
            Assert.AreEqual(expectedMessage, testLogEntry.Message, "Message is unexpected value.");
            Assert.AreEqual(expectedTimestamp, testLogEntry.Timestamp, "Timestamp is unexpected value.");
            Assert.AreEqual("Object", testLogEntry.Location, "Location is unexpected value.");
            Assert.IsNotNull(testLogEntry.GetHashCode(), "GetHashCode method returned unexpected value.");
            Assert.IsNotNull(testLogEntry.ToString(), "GetHashCode method returned unexpected value.");

            expectedMessage = "Information.";
            expectedType = MessageType.Information;
            expectedTimestamp = DateTime.Now;
            testLogEntry = new ApplicationLogEntry(MessageType.Information, "Information.", new object());
            Assert.IsNotNull(testLogEntry, "Constructor of type, LogEntry failed to create instance.");
            Assert.AreEqual(expectedType, testLogEntry.Type, "Type is unexpected value.");
            Assert.AreEqual(expectedMessage, testLogEntry.Message, "Message is unexpected value.");
            Assert.AreEqual(expectedTimestamp, testLogEntry.Timestamp, "Timestamp is unexpected value.");
            Assert.AreEqual("Object", testLogEntry.Location, "Location is unexpected value.");
            Assert.IsNotNull(testLogEntry.GetHashCode(), "GetHashCode method returned unexpected value.");
            Assert.IsNotNull(testLogEntry.ToString(), "GetHashCode method returned unexpected value.");
        }

        /// <summary>
        /// TestToString
        /// </summary>
        [Test()]
        public void TestToString()
        {
            string expectedString = _unitUnderTest.ToString();
            string resultString = null;
            resultString = _unitUnderTest.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

        }

        /// <summary>
        /// TestEquals
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            ApplicationLogEntry testLogEntry = new ApplicationLogEntry(MessageType.Warning, "", new object());
            object obj = null;
            bool expectedBoolean = false;
            bool resultBoolean = false;
            resultBoolean = testLogEntry.Equals(obj);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;
            resultBoolean = testLogEntry.Equals(testLogEntry);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

        }

        /// <summary>
        /// TestGetHashCode
        /// </summary>
        [Test()]
        public void TestGetHashCode()
        {
            ApplicationLogEntry testLogEntry1 = new ApplicationLogEntry(MessageType.Warning, "", new object());
            ApplicationLogEntry testLogEntry2 = new ApplicationLogEntry(MessageType.Warning, "", new object());

            int expectedInt32 = testLogEntry1.GetHashCode();
            int resultInt32 = testLogEntry2.GetHashCode();
            Assert.AreEqual(expectedInt32, resultInt32, "GetHashCode method returned unexpected result.");

            resultInt32 = _unitUnderTest.GetHashCode();
            Assert.AreNotEqual(expectedInt32, resultInt32, "GetHashCode method returned unexpected result.");

        }
    }
}
