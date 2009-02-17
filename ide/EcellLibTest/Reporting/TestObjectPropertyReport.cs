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
    public class TestObjectPropertyReport
    {

        private ObjectPropertyReport _unitUnderTest;
        /// <summary>
        /// 
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            Ecell.MessageType type = MessageType.Warning;
            string message = null;
            string group = null;
            Ecell.Objects.EcellObject obj = null;
            string propertyName = null;
            _unitUnderTest = new ObjectPropertyReport(type, message, group, obj, propertyName);
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
        public void TestConstructorObjectPropertyReport()
        {
            Ecell.MessageType type = MessageType.Error;
            string message = "Error";
            string group = "Group";
            string propertyName = "Value";
            EcellObject obj = EcellObject.CreateObject("Model", "/:S", EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
            obj.SetEcellValue(propertyName, new EcellValue(0.1));

            ObjectPropertyReport testObjectPropertyReport = new ObjectPropertyReport(type, message, group, obj, propertyName);
            Assert.IsNotNull(testObjectPropertyReport, "Constructor of type, ObjectPropertyReport failed to create instance.");
            Assert.IsNotNull(testObjectPropertyReport, "Constructor of type, ObjectReport failed to create instance.");
            Assert.AreEqual(MessageType.Error, testObjectPropertyReport.Type, "Type is unexpected value.");
            Assert.AreEqual(message, testObjectPropertyReport.Message, "Message is unexpected value.");
            Assert.AreEqual(group, testObjectPropertyReport.Group, "Group is unexpected value.");
            Assert.AreEqual(obj, testObjectPropertyReport.Object, "Object is unexpected value.");
            Assert.AreEqual(obj.FullID + ":Value", testObjectPropertyReport.Location, "Location is unexpected value.");
            Assert.AreEqual(propertyName, testObjectPropertyReport.PropertyName, "Location is unexpected value.");
            Assert.AreNotEqual(0, testObjectPropertyReport.GetHashCode(), "GetHashCode method returns unexpected value.");
            Assert.IsNotNull(testObjectPropertyReport.ToString(), "ToString method returns unexpected value.");

        }
    }
}
