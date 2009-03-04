//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Ecell.Logger;

namespace Ecell.Logger
{
    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestLoggerEntry
    {
        private LoggerEntry _unitUnderTest;

        /// <summary>
        /// Constructor.
        /// </summary>
        [SetUp()]
        public void Setup()
        {
            _unitUnderTest = new LoggerEntry("Model", "/:ID", "Process", "Process:/:ID:Activity");
        }

        /// <summary>
        /// Deconstructor.
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        /// <summary>
        /// TestLoggerEntry.
        /// </summary>
        [Test()]
        public void TestConstructor()
        {
            LoggerEntry value = null;
            value = new LoggerEntry("Model", "/:ID", "Process", "Process:/:ID:Activity");
            Assert.IsNotNull(value, "Constructor of of type, LoggerEntry failed to create instance.");
            Assert.IsFalse(value.IsLoaded, "IsLogging is not expected value.");
            Assert.AreEqual("Model", value.ModelID, "ModelID is not expected value.");
            Assert.AreEqual("/:ID", value.ID, "ID is not expected value.");
            Assert.AreEqual("Process", value.Type, "ModelID is not expected value.");
            Assert.AreEqual("Process:/:ID:Activity", value.FullPN, "ModelID is not expected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestSetter()
        {
            LoggerEntry value = null;
            value = new LoggerEntry("Model", "/:ID", "Process", "Process:/:ID:Activity");

            value.ModelID = "Model1";
            Assert.AreEqual("Model1", value.ModelID, "ModelID is not expected value.");

            value.ID = "/:ID1";
            Assert.AreEqual("/:ID1", value.ID, "ID is not expected value.");

            value.Type = "Variable";
            Assert.AreEqual("Variable", value.Type, "Type is not expected value.");

            value.FullPN = "Process:/:ID:Vm";
            Assert.AreEqual("Process:/:ID:Vm", value.FullPN, "FullPN is not expected value.");

            value.IsLoaded = false;
            Assert.AreEqual(false, value.IsLoaded, "IsLogging is not expected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;
            LoggerEntry value = null;

            value = new LoggerEntry("Model", "/:ID", "Process", "Process:/:ID:Activity");
            expectedBoolean = false;
            resultBoolean = false;

            resultBoolean = value.Equals(new object());
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = value.Equals(new LoggerEntry("Model1", "/:ID", "Process", "Process:/:ID:Activity"));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = value.Equals(new LoggerEntry("Model", "/:ID1", "Process", "Process:/:ID:Activity"));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = value.Equals(new LoggerEntry("Model", "/:ID", "Variable", "Process:/:ID:Activity"));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            resultBoolean = value.Equals(new LoggerEntry("Model1", "/:ID", "Process", "Process:/:ID:Km"));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;
            resultBoolean = value.Equals(new LoggerEntry("Model", "/:ID", "Process", "Process:/:ID:Activity"));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");
            
            resultBoolean = value.Equals(new LoggerEntry("Model", "/:ID", "Process", "Process:/:ID:Activity"));
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");
        }
    }
}
