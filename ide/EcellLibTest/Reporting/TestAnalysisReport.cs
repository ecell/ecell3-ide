﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Drawing;

namespace Ecell.Reporting
{

    /// <summary>
    /// TestTemplate1
    /// </summary>
    [TestFixture()]
    public class TestAnalysisReport
    {
        private AnalysisReport _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new AnalysisReport(MessageType.Information, "Test", "group", "job");
        }
        /// <summary>
        /// Disposer
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
        public void TestConstructor()
        {
            AnalysisReport obj = new AnalysisReport(MessageType.Information, "Test", "group", "job");
            Assert.IsNotNull(obj, "Constructor of type, object failed to create instance.");
            Assert.AreEqual(MessageType.Information, obj.Type, "GetType method returned unexpected value.");
            Assert.AreEqual("Test", obj.Message, "Type is unexpected value.");
            Assert.AreEqual("group", obj.Group, "Type is unexpected value.");
            Assert.AreEqual("job", obj.Location, "Type is unexpected value.");
            Assert.AreNotEqual(0, obj.GetHashCode(), "GetHashCode method returned unexpected value.");
        }
        
        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestEqual()
        {
            Report report = new AnalysisReport(MessageType.Information, "Test", "group", "job");
            bool equal = report.Equals(report);
            Assert.AreEqual(true, equal, "Equals method returned unexpected value.");

            equal = report.Equals(null);
            Assert.AreEqual(false, equal, "Equals method returned unexpected value.");

            //
            equal = report.Equals(new CompileReport(MessageType.Information, "Test", "group"));
            Assert.AreEqual(true, equal, "Equals method returned unexpected value.");
        }
    }
}
