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



namespace Ecell.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestLoggerManager
    {
        private LoggerManager _unitUnderTest;
        private ApplicationEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _env = new ApplicationEnvironment();
            _unitUnderTest = new LoggerManager(_env);
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
        public void TestConstructorLoggerManagerr()
        {
            LoggerManager testLoggerManager = new LoggerManager(_env);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAddLogger()
        {
            _unitUnderTest.LoggerAddEvent += new LoggerAddEventHandler(_unitUnderTest_LoggerAddEvent);
            _unitUnderTest.AddLoggerEntry("ModelID", "Key", "Process", "FullPN");
            _unitUnderTest.AddLoggerEntry("ModelID", "Key", "Process", "FullPN");
            List<string> res = _unitUnderTest.GetLoggerList();
            Assert.AreEqual(1, res.Count, "Add entry is unexpected work.");
            _unitUnderTest.AddLoggerEntry(null);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoggerChanged()
        {
            _unitUnderTest.LoggerChangedEvent += new LoggerChangedEventHandler(_unitUnderTest_LoggerChangedEvent);
            LoggerEntry ent = new LoggerEntry("ModelID", "Key", "Process", "FullPN");
            _unitUnderTest.AddLoggerEntry(ent);
            _unitUnderTest.LoggerChanged("FullPN1", ent);
            ent = new LoggerEntry("ModelID", "Key1", "Process", "FullPN1");
            _unitUnderTest.LoggerChanged("FullPN", ent);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestLoggerRemoved()
        {
            _unitUnderTest.LoggerDeleteEvent += new LoggerDeleteEventHandler(_unitUnderTest_LoggerDeleteEvent);
            LoggerEntry ent = new LoggerEntry("ModelID", "Key", "Process", "FullPN");
            _unitUnderTest.AddLoggerEntry(ent);
            List<string> res = _unitUnderTest.GetLoggerList();
            Assert.AreEqual(1, res.Count, "Add entry is unexpected work.");
            LoggerEntry ent1 = new LoggerEntry("ModelID", "Key", "Process", "FullPN1");
            _unitUnderTest.LoggerRemoved(ent1);
            res = _unitUnderTest.GetLoggerList();
            Assert.AreEqual(1, res.Count, "Remove entry is unexpected work.");
            _unitUnderTest.LoggerRemoved(ent);
            res = _unitUnderTest.GetLoggerList();
            Assert.AreEqual(0, res.Count, "Remove entry is unexpected work.");
            _unitUnderTest.LoggerRemoved(null);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLoggerEntryForObject()
        {
            LoggerEntry ent = new LoggerEntry("ModelID", "Key", "Process", "FullPN");
            _unitUnderTest.AddLoggerEntry(ent);
            List<LoggerEntry> res = _unitUnderTest.GetLoggerEntryForObject("Key", "Process");
            Assert.AreEqual(1, res.Count, "GetLoggerEntryForObject is unexpected work.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetLoggerEntryForFullPN()
        {
            LoggerEntry ent = new LoggerEntry("ModelID", "Key", "Process", "FullPN");
            _unitUnderTest.AddLoggerEntry(ent);
            LoggerEntry res = _unitUnderTest.GetLoggerEntryForFullPN("FullPN1");
            Assert.AreEqual(null, res, "GetLoggerEntryForFullPN  is unexpected work.");
            res = _unitUnderTest.GetLoggerEntryForFullPN("FullPN");
            Assert.AreNotEqual(null, res, "GetLoggerEntryForObject is unexpected work.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestClear()
        {
            _unitUnderTest.AddLoggerEntry("ModelID", "Key", "Process", "FullPN");
            List<string> res = _unitUnderTest.GetLoggerList();
            Assert.AreEqual(1, res.Count, "Clear is unexpected work.");
            _unitUnderTest.Clear();
            res = _unitUnderTest.GetLoggerList();
            Assert.AreEqual(0, res.Count, "Clear is unexpected work.");
        }


        void _unitUnderTest_LoggerAddEvent(object o, LoggerEventArgs e)
        {
        }

        void _unitUnderTest_LoggerChangedEvent(object o, LoggerEventArgs e)
        {
        }

        void _unitUnderTest_LoggerDeleteEvent(object o, LoggerEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSystemRemoved()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            _unitUnderTest.AddLoggerEntry("Drosophila", "/CELL/CYTOPLASM:P1", "Variable", "Variable:/CELL/CYTOPLASM:P1:Value");
            List<string> res = _unitUnderTest.GetLoggerList();
            _unitUnderTest.SystemRemoved(_env.DataManager.GetEcellObject("Drosophila", "/CELL/CYTOPLASM", "System"));

        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestNodeRemoved()
        {
            _env.DataManager.LoadProject(TestConstant.Project_Drosophila);
            _unitUnderTest.AddLoggerEntry("Drosophila", "/CELL/CYTOPLASM:P1", "Variable", "Variable:/CELL/CYTOPLASM:P1:Value");
            List<string> res = _unitUnderTest.GetLoggerList();
            _unitUnderTest.NodeRemoved(_env.DataManager.GetEcellObject("Drosophila", "/CELL/CYTOPLASM:P1", "Variable"));

        }
    }
}
