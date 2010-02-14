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

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace Ecell.Objects
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestEcellObservedData
    {
        private EcellObservedData _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellObservedData();
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
            EcellObservedData data = new EcellObservedData();
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("", data.Key, "Key is unexpected value.");
            Assert.AreEqual(100.0, data.Max, "Max is unexpected value.");
            Assert.AreEqual(0, data.Min, "Min is unexpected value.");
            Assert.AreEqual(100.0, data.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.5, data.Rate, "Rate is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellObservedData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");
            Assert.IsNotNull(data.GetHashCode(), "ToString method returned unexpected value.");

            data.Key = "Variable:/S0:Value:MolarConc";
            data.Max = 10;
            data.Min = 1;
            data.Differ = 9;
            data.Rate = 0.8;

            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(10, data.Max, "Max is unexpected value.");
            Assert.AreEqual(1.0, data.Min, "Min is unexpected value.");
            Assert.AreEqual(9.0, data.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.8, data.Rate, "Rate is unexpected value.");
            Assert.IsNotNull(data.GetHashCode(), "ToString method returned unexpected value.");

        }

        /// <summary>
        /// TestConstructorWithCurrent
        /// </summary>
        [Test()]
        public void TestConstructorWithCurrent()
        {
            EcellObservedData data = new EcellObservedData("Variable:/S0:Value:MolarConc", 0.5);
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(0.75d, data.Max, "Max is unexpected value.");
            Assert.AreEqual(0.25d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(0.5d, data.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.5d, data.Rate, "Rate is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellObservedData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");

            data.Key = "Variable:/S0:Value:MolarConc";
            data.Max = 10;
            data.Min = 1;
            data.Differ = 9;
            data.Rate = 0.8;

            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(10, data.Max, "Max is unexpected value.");
            Assert.AreEqual(1.0, data.Min, "Min is unexpected value.");
            Assert.AreEqual(9.0, data.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.8, data.Rate, "Rate is unexpected value.");

            data = new EcellObservedData("Variable:/S0:Value:MolarConc", -10);
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(-5, data.Max, "Max is unexpected value.");
            Assert.AreEqual(-15, data.Min, "Min is unexpected value.");
            Assert.AreEqual(10, data.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.5, data.Rate, "Rate is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellObservedData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");
        }

        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestConstructorWithParams()
        {
            EcellObservedData data = new EcellObservedData("Variable:/S0:Value:MolarConc", 0.75, 0.25, 0.5, 0.5);
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(0.75d, data.Max, "Max is unexpected value.");
            Assert.AreEqual(0.25d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(0.5d, data.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.5d, data.Rate, "Rate is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellObservedData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");

            data.Key = "Variable:/S0:Value:MolarConc";
            data.Max = 10;
            data.Min = 1;
            data.Differ = 9;
            data.Rate = 0.8;

            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(10, data.Max, "Max is unexpected value.");
            Assert.AreEqual(1.0, data.Min, "Min is unexpected value.");
            Assert.AreEqual(9.0, data.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.8, data.Rate, "Rate is unexpected value.");

        }
        
        /// <summary>
        /// TestCopy
        /// </summary>
        [Test()]
        public void TestCopy()
        {
            EcellObservedData data = new EcellObservedData();
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");

            EcellObservedData newData = data.Copy();
            Assert.IsNotNull(newData, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("", newData.Key, "Key is unexpected value.");
            Assert.AreEqual(100.0, newData.Max, "Max is unexpected value.");
            Assert.AreEqual(0, newData.Min, "Min is unexpected value.");
            Assert.AreEqual(100.0, newData.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.5, newData.Rate, "Rate is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellObservedData", newData.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(newData.ToString(), "ToString method returned unexpected value.");


            data = new EcellObservedData("Variable:/S0:Value:MolarConc", 0.75, 0.25, 0.5, 0.5);
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");

            newData = data.Copy();
            Assert.IsNotNull(newData, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", newData.Key, "Key is unexpected value.");
            Assert.AreEqual(0.75d, newData.Max, "Max is unexpected value.");
            Assert.AreEqual(0.25d, newData.Min, "Min is unexpected value.");
            Assert.AreEqual(0.5d, newData.Differ, "Differ is unexpected value.");
            Assert.AreEqual(0.5d, newData.Rate, "Rate is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellObservedData", newData.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(newData.ToString(), "ToString method returned unexpected value.");
        }
        
        /// <summary>
        /// TestEquals
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            EcellObservedData data1 = new EcellObservedData("Variable:/S0:Value:MolarConc", 0.75, 0.25, 0.5, 0.5);
            EcellObservedData data2 = new EcellObservedData("Variable:/S0:Value1:MolarConc", 0.75, 0.25, 0.5, 0.5);
            EcellObservedData data3 = new EcellObservedData("Variable:/S0:Value:MolarConc", 20, 0, 20, 0.8);
            object obj = new object();

            Assert.AreEqual(false, data1.Equals(data2), "Equals method returned unexpected value.");
            Assert.AreEqual(false, data1.Equals(obj), "Equals method returned unexpected value.");
            Assert.AreEqual(true, data1.Equals(data3), "Equals method returned unexpected value.");
        }

    }
}
