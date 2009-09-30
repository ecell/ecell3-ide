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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace Ecell.Objects
{

    /// <summary>
    /// TestEcellDragObject
    /// </summary>
    [TestFixture()]
    public class TestEcellDragObject
    {
        private EcellDragObject _dragObject;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _dragObject = new EcellDragObject();
        }
        /// <summary>
        /// Disposer
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _dragObject = null;
        }

        /// <summary>
        /// TestSample
        /// </summary>
        [Test()]
        public void TestEcellDragEntry()
        {
            EcellDragEntry entry = new EcellDragEntry();
            Assert.IsNotNull(entry, "Constructor of type, EcellDragEntry failed to create instance.");
            Assert.AreEqual("", entry.Key, "Key is unexpected value.");
            Assert.AreEqual("", entry.Type, "Type is unexpected value.");
            Assert.AreEqual("", entry.Path, "Path is unexpected value.");
            Assert.AreEqual(false, entry.IsLogable, "IsLogable is unexpected value.");
            Assert.AreEqual(false, entry.IsSettable, "IsSettable is unexpected value.");

            entry = new EcellDragEntry("/:V0", "Variable", "Variable:/:V0:MolerConc", true, true);
            Assert.IsNotNull(entry, "Constructor of type, EcellDragEntry failed to create instance.");
            Assert.AreEqual("/:V0", entry.Key, "Key is unexpected value.");
            Assert.AreEqual("Variable", entry.Type, "Type is unexpected value.");
            Assert.AreEqual("Variable:/:V0:MolerConc", entry.Path, "Path is unexpected value.");
            Assert.AreEqual(true, entry.IsLogable, "IsLogable is unexpected value.");
            Assert.AreEqual(true, entry.IsSettable, "IsSettable is unexpected value.");
        }

        /// <summary>
        /// TestSample
        /// </summary>
        [Test()]
        public void TestEcellDragObjectConstructor()
        {
            EcellDragObject dragObj = new EcellDragObject();
            Assert.IsNotNull(dragObj, "Constructor of type, EcellDragObject failed to create instance.");
            Assert.AreEqual("", dragObj.ModelID, "Key is unexpected value.");
            Assert.AreEqual(false, dragObj.IsEnableChange, "IsEnableChange is unexpected value.");
            dragObj.IsEnableChange = true;
            Assert.AreEqual(true, dragObj.IsEnableChange, "IsEnableChange is unexpected value.");
            Assert.IsEmpty(dragObj.LogList, "LogList is unexpected value.");
            Assert.IsEmpty(dragObj.Entries, "Entries is unexpected value.");

            dragObj = new EcellDragObject("Model");
            Assert.IsNotNull(dragObj, "Constructor of type, EcellDragObject failed to create instance.");
            Assert.AreEqual("Model", dragObj.ModelID, "Key is unexpected value.");
            Assert.IsEmpty(dragObj.LogList, "LogList is unexpected value.");
            Assert.IsEmpty(dragObj.Entries, "Entries is unexpected value.");

            dragObj.Entries.Add(new EcellDragEntry("/:V0", "Variable", "Variable:/:V0:MolerConc", true, true));
            dragObj.Entries.Add(new EcellDragEntry("/:P0", "Process", "Process:/:V0:MolerActivity", true, true));
            Assert.IsNotEmpty(dragObj.Entries, "Entries is unexpected value.");

            List<string> logs = new List<string>();
            logs.Add("test");
            logs.Add("sample");
            dragObj.LogList = logs;
            Assert.IsNotEmpty(dragObj.LogList, "LogList is unexpected value.");

        }
    }
}
