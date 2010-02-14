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
    public class TestEcellParameterData
    {
        private EcellParameterData _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellParameterData();
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
            EcellParameterData data = new EcellParameterData();
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("", data.Key, "Key is unexpected value.");
            Assert.AreEqual(100.0, data.Max, "Max is unexpected value.");
            Assert.AreEqual(0.0d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(0.0d, data.Step, "Step is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellParameterData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");
            Assert.IsNotNull(data.GetHashCode(), "ToString method returned unexpected value.");

            data.Key = "Variable:/S0:Value:MolarConc";
            data.Max = 10;
            data.Min = 1;
            data.Step = 1;

            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(10, data.Max, "Max is unexpected value.");
            Assert.AreEqual(1.0d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(1.0d, data.Step, "Step is unexpected value.");

        }
        
        /// <summary>
        /// TestConstructorWithCurrent
        /// </summary>
        [Test()]
        public void TestConstructorWithCurrent()
        {
            EcellParameterData data = new EcellParameterData("Variable:/S0:Value:MolarConc", 10);
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(15.0, data.Max, "Max is unexpected value.");
            Assert.AreEqual(5.0d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(0.0d, data.Step, "Step is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellParameterData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");
            Assert.IsNotNull(data.GetHashCode(), "ToString method returned unexpected value.");

            data.Key = "Variable:/S0:Value1:MolarConc";
            data.Max = 10;
            data.Min = 1;
            data.Step = 1;

            Assert.AreEqual("Variable:/S0:Value1:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(10, data.Max, "Max is unexpected value.");
            Assert.AreEqual(1.0d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(1.0d, data.Step, "Step is unexpected value.");

            data = new EcellParameterData("Variable:/S0:Value:MolarConc", -10);
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(-5.0d, data.Max, "Max is unexpected value.");
            Assert.AreEqual(-15.0d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(0.0d, data.Step, "Step is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellParameterData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");
            Assert.IsNotNull(data.GetHashCode(), "ToString method returned unexpected value.");
        }
                
        /// <summary>
        /// TestConstructorWithParameters
        /// </summary>
        [Test()]
        public void TestConstructorWithParameters()
        {
            EcellParameterData data = new EcellParameterData("Variable:/S0:Value:MolarConc", 15.0, 5.0, 0.0);
            Assert.IsNotNull(data, "Constructor of type, EcellObservedData failed to create instance.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(15.0, data.Max, "Max is unexpected value.");
            Assert.AreEqual(5.0d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(0.0d, data.Step, "Step is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellParameterData", data.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.IsNotNull(data.ToString(), "ToString method returned unexpected value.");
            Assert.IsNotNull(data.GetHashCode(), "ToString method returned unexpected value.");

            data.Key = "Variable:/S0:Value1:MolarConc";
            data.Max = 10;
            data.Min = 1;
            data.Step = 1;

            Assert.AreEqual("Variable:/S0:Value1:MolarConc", data.Key, "Key is unexpected value.");
            Assert.AreEqual(10, data.Max, "Max is unexpected value.");
            Assert.AreEqual(1.0d, data.Min, "Min is unexpected value.");
            Assert.AreEqual(1.0d, data.Step, "Step is unexpected value.");

        }

        /// <summary>
        /// TestCopy
        /// </summary>
        [Test()]
        public void TestCopy()
        {
            EcellParameterData data = new EcellParameterData();
            EcellParameterData newData = data.Copy();
            Assert.AreEqual(data, newData, "Key is unexpected value.");
            Assert.AreEqual("", newData.Key, "Key is unexpected value.");
            Assert.AreEqual(100.0, newData.Max, "Max is unexpected value.");
            Assert.AreEqual(0.0d, newData.Min, "Min is unexpected value.");
            Assert.AreEqual(0.0d, newData.Step, "Step is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellParameterData", newData.GetType().ToString(), "GetType method returned unexpected value.");

            data = new EcellParameterData("Variable:/S0:Value:MolarConc", 10);
            newData = data.Copy();
            Assert.AreEqual(data, newData, "Key is unexpected value.");
            Assert.AreEqual("Variable:/S0:Value:MolarConc", newData.Key, "Key is unexpected value.");
            Assert.AreEqual(15.0, newData.Max, "Max is unexpected value.");
            Assert.AreEqual(5.0d, newData.Min, "Min is unexpected value.");
            Assert.AreEqual(0.0d, newData.Step, "Step is unexpected value.");
            Assert.AreEqual("Ecell.Objects.EcellParameterData", newData.GetType().ToString(), "GetType method returned unexpected value.");

        }

        /// <summary>
        /// TestEquals
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            EcellParameterData data1 = new EcellParameterData("Variable:/S0:Value:MolarConc", 0.75, 0.25, 0.05);
            EcellParameterData data2 = new EcellParameterData("Variable:/S0:Value1:MolarConc", 0.75, 0.25, 0.05);
            EcellParameterData data3 = new EcellParameterData("Variable:/S0:Value:MolarConc", 20, 0, 0);
            object obj = new object();

            Assert.AreEqual(false, data1.Equals(data2), "Equals method returned unexpected value.");
            Assert.AreEqual(false, data1.Equals(obj), "Equals method returned unexpected value.");
            Assert.AreEqual(true, data1.Equals(data3), "Equals method returned unexpected value.");

        }

    }
}
