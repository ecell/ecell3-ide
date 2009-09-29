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

namespace Ecell.Reporting
{
    using System;
    using NUnit.Framework;
    using Ecell.Objects;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestObjectReport
    {

        private ObjectReport _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.MessageType type = MessageType.Debug;
            string message = null;
            string group = null;
            Ecell.Objects.EcellObject obj = null;
            _unitUnderTest = new ObjectReport(type, message, group, obj);
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
        public void TestConstructorObjectReport()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = "Error";
            string group = "Group";
            EcellObject obj = EcellObject.CreateObject("Model", "/:S",EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
            ObjectReport testObjectReport = new ObjectReport(type, message, group, obj);
            Assert.IsNotNull(testObjectReport, "Constructor of type, ObjectReport failed to create instance.");
            Assert.AreEqual(MessageType.Error, testObjectReport.Type, "Type is unexpected value.");
            Assert.AreEqual(message, testObjectReport.Message, "Message is unexpected value.");
            Assert.AreEqual(group, testObjectReport.Group, "Group is unexpected value.");
            Assert.AreEqual(obj, testObjectReport.Object, "Object is unexpected value.");
            Assert.AreEqual(obj.FullID, testObjectReport.Location, "Location is unexpected value.");
            Assert.AreNotEqual(0, testObjectReport.GetHashCode(), "GetHashCode method returns unexpected value.");
            Assert.IsNotNull(testObjectReport.ToString(), "ToString method returns unexpected value.");
        }
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = "Error";
            string group = "Group";
            EcellObject obj = EcellObject.CreateObject("Model", "/:S", EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
            ObjectReport testObjectReport1 = new ObjectReport(type, message, group, obj);
            Assert.IsFalse(testObjectReport1.Equals(new object()));

            ObjectReport testObjectReport2 = new ObjectReport(MessageType.Debug, message, group, obj);
            Assert.IsFalse(testObjectReport1.Equals(testObjectReport2));

            ObjectReport testObjectReport3 = new ObjectReport(type, message, "Hoge", obj);
            Assert.IsFalse(testObjectReport1.Equals(testObjectReport3));
        }
    }
}
