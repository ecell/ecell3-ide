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
using System.Drawing;
using System.Drawing.Drawing2D;
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
            Assert.AreNotEqual(0, value.GetHashCode(), "GetHashCode returns unexpected value.");
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

            value.Color = Color.Red;
            Assert.AreEqual(value.Color, Color.Red, "Color is not expected value.");

            value.LineStyle = DashStyle.DashDot;
            Assert.AreEqual(value.LineStyle, DashStyle.DashDot, "Line style is not expected value.");

            value.LineWidth = 10;
            Assert.AreEqual(value.LineWidth, 10, "Line width is not expected value.");

            value.IsLoaded = false;
            Assert.AreEqual(false, value.IsLoaded, "IsLogging is not expected value.");

            value.IsShown = false;
            Assert.AreEqual(false, value.IsShown, "IsShown is not expected value.");

            value.IsY2Axis = true;
            Assert.AreEqual(true, value.IsY2Axis, "IsY2 axis is not expected value.");

            value.IsY2AxisInt = 0;
            Assert.AreEqual(false, value.IsY2Axis, "IsY2AxisInt is not expected value.");
            Assert.AreEqual(0, value.IsY2AxisInt, "IsY2AxisInt is not expected value.");

            value.IsY2AxisInt = 1;
            Assert.AreEqual(true, value.IsY2Axis, "IsY2AxisInt is not expected value.");
            Assert.AreEqual(1, value.IsY2AxisInt, "IsY2AxisInt is not expected value.");

            value.IsShowInt = 0;
            Assert.AreEqual(false, value.IsShown, "IsShowInt is not expected value.");
            Assert.AreEqual(0, value.IsShowInt, "IsShowInt is not expected value.");

            value.IsShowInt = 1;
            Assert.AreEqual(true, value.IsShown, "IsShowInt is not expected value.");
            Assert.AreEqual(1, value.IsShowInt, "IsShowInts is not expected value.");

            value.LineStyleInt = 0;
            Assert.AreEqual(value.LineStyle, DashStyle.Solid, "LineStyleInt is not expected value.");
            Assert.AreEqual(value.LineStyleInt, 0, "LineStyleInt is not expected value.");

            value.LineStyleInt = 1;
            Assert.AreEqual(value.LineStyle, DashStyle.Dot, "LineStyleInt is not expected value.");
            Assert.AreEqual(value.LineStyleInt, 1, "LineStyleInt is not expected value.");

            value.LineStyleInt = 2;
            Assert.AreEqual(value.LineStyle, DashStyle.Dash, "LineStyleInt is not expected value.");
            Assert.AreEqual(value.LineStyleInt, 2, "LineStyleInt is not expected value.");

            value.LineStyleInt = 3;
            Assert.AreEqual(value.LineStyle, DashStyle.DashDot, "LineStyleInt is not expected value.");
            Assert.AreEqual(value.LineStyleInt, 3, "LineStyleInt is not expected value.");

            value.LineStyleInt = 4;
            Assert.AreEqual(value.LineStyle, DashStyle.DashDotDot, "LineStyleInt is not expected value.");
            Assert.AreEqual(value.LineStyleInt, 4, "LineStyleInt is not expected value.");
            

            value.FileName = "aaa";
            Assert.AreEqual("aaa", value.FileName, "Filename is not expected value.");
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
