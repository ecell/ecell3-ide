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

namespace Ecell.Objects
{
    using System;
    using NUnit.Framework;
    using Ecell.Objects;
    using System.Collections.Generic;

    /// <summary>
    /// Test code of EcellData
    /// </summary>
    [TestFixture()]
    public class TestEcellData
    {

        private EcellData _unitUnderTest;

        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellData();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        [TearDown()]
        public void TearDown()
        {
            _unitUnderTest = null;
        }

        /// <summary>
        /// Test of Constructor.
        /// </summary>
        [Test()]
        public void TestConstructorEcellData()
        {
            EcellData data = new EcellData();
            Assert.IsNotNull(data, "Constructor of type, EcellData failed to create instance.");
            Assert.IsNull(data.Name, "Name is not null.");
            Assert.IsNull(data.EntityPath, "EntityPath is not null.");
            Assert.IsNull(data.Value, "Value is not null.");
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsFalse(data.IsInitialized(), "IsInitialized is not expected value.");

        }

        /// <summary>
        /// Testof Constructor with parameters.
        /// </summary>
        [Test()]
        public void TestConstructorEcellDataNameValueEntityPath()
        {
            string name = null;
            EcellValue value = null;
            string entityPath = null;

            // null (uninitialized)
            EcellData data = new EcellData(name, value, entityPath);
            Assert.IsNotNull(data, "Constructor of type, EcellData failed to create instance.");
            Assert.IsNull(data.Name, "Name is not null.");
            Assert.IsNull(data.EntityPath, "EntityPath is not null.");
            Assert.IsNull(data.Value, "Value is not null.");
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsFalse(data.IsInitialized(), "IsInitialized is not expected value.");

            name = "TestData";
            entityPath = "Variable:/:V0:TestData";

            // int
            value = new EcellValue(1);
            data = new EcellData(name, value, entityPath);
            Assert.IsNotNull(data, "Constructor of type, EcellData failed to create instance.");
            Assert.AreEqual(name, data.Name, "Name is not expected value.");
            Assert.AreEqual(entityPath, data.EntityPath, "EntityPath is not expected value.");
            Assert.AreEqual(value, data.Value, "Value is not expected value.");
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsTrue(data.IsInitialized(), "IsInitialized is not expected value.");

            // double
            value = new EcellValue(0.001);
            data = new EcellData(name, value, entityPath);
            Assert.IsNotNull(data, "Constructor of type, EcellData failed to create instance.");
            Assert.AreEqual(name, data.Name, "Name is not expected value.");
            Assert.AreEqual(entityPath, data.EntityPath, "EntityPath is not expected value.");
            Assert.AreEqual(value, data.Value, "Value is not expected value.");
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsTrue(data.IsInitialized(), "IsInitialized is not expected value.");

            // string
            value = new EcellValue("test");
            data = new EcellData(name, value, entityPath);
            Assert.IsNotNull(data, "Constructor of type, EcellData failed to create instance.");
            Assert.AreEqual(name, data.Name, "Name is not expected value.");
            Assert.AreEqual(entityPath, data.EntityPath, "EntityPath is not expected value.");
            Assert.IsTrue(value.Equals(data.Value), "Value is not expected value.");
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsFalse(data.IsInitialized(), "IsInitialized is not expected value.");

            // List
            List<EcellValue> list = new List<EcellValue>();
            value = new EcellValue(list);
            data = new EcellData(name, value, entityPath);
            Assert.IsNotNull(data, "Constructor of type, EcellData failed to create instance.");
            Assert.AreEqual(name, data.Name, "Name is not expected value.");
            Assert.AreEqual(entityPath, data.EntityPath, "EntityPath is not expected value.");
            Assert.IsTrue(value.Equals(data.Value), "Value is not expected value.");
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsFalse(data.IsInitialized(), "IsInitialized is not expected value.");
        }

        /// <summary>
        /// Test of Accessors
        /// </summary>
        [Test()]
        public void TestAccessors()
        {
            string name = "TestData";
            EcellValue value = new EcellValue(0.001);
            string entityPath = "Variable:/:V0:TestData";

            EcellData data = new EcellData();
            data.Name = name;
            data.Value = value;
            data.EntityPath = entityPath;
            Assert.IsNotNull(data, "Constructor of type, EcellData failed to create instance.");
            Assert.AreEqual(name, data.Name, "Name is not expected value.");
            Assert.AreEqual(entityPath, data.EntityPath, "EntityPath is not expected value.");
            Assert.AreEqual(value, data.Value, "Value is not expected value.");
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsTrue(data.IsInitialized(), "IsInitialized is not expected value.");

            data.Settable = false;
            Assert.IsFalse(data.Settable, "Settable is not expected value.");
            Assert.IsFalse(data.IsInitialized(), "IsInitialized is not expected value.");

            data.Gettable = false;
            data.Loadable = false;
            data.Logable = false;
            data.Logged = false;
            data.Saveable = false;
            Assert.IsFalse(data.Gettable, "Gettable is not expected value.");
            Assert.IsFalse(data.Loadable, "Loadable is not expected value.");
            Assert.IsFalse(data.Logable, "Logable is not expected value.");
            Assert.IsFalse(data.Logged, "Logged is not expected value.");
            Assert.IsFalse(data.Saveable, "Saveable is not expected value.");

            data.Gettable = true;
            data.Loadable = true;
            data.Logable = true;
            data.Logged = true;
            data.Saveable = true;
            Assert.IsTrue(data.Gettable, "Gettable is not expected value.");
            Assert.IsTrue(data.Loadable, "Loadable is not expected value.");
            Assert.IsTrue(data.Logable, "Logable is not expected value.");
            Assert.IsTrue(data.Logged, "Logged is not expected value.");
            Assert.IsTrue(data.Saveable, "Saveable is not expected value.");
            Assert.IsFalse(data.Settable, "Settable is not expected value.");
            Assert.IsFalse(data.IsInitialized(), "IsInitialized is not expected value.");

            data.Settable = true;
            Assert.IsTrue(data.Settable, "Settable is not expected value.");
            Assert.IsTrue(data.IsInitialized(), "IsInitialized is not expected value.");

        }

        /// <summary>
        /// Test of ToString
        /// </summary>
        [Test()]
        public void TestToString()
        {
            string expectedString = null;
            string resultString = null;

            EcellData data = new EcellData();
            expectedString = "";
            resultString = data.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            data = new EcellData("Value", null, "Variable:/:Test:Value");
            expectedString = "Variable:/:Test:Value";
            resultString = data.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            data = new EcellData("Value", new EcellValue(0.1), null);
            expectedString = ", 0.1";
            resultString = data.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");


            data = new EcellData("Value", new EcellValue(0.1), "Variable:/:Test:Value");
            expectedString = "Variable:/:Test:Value, 0.1";
            resultString = data.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");
        }

        /// <summary>
        /// Test of Equals()
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            // double
            bool expectedBoolean = false;
            bool resultBoolean = false;
            EcellData data1 = new EcellData("Value", new EcellValue(1), "Variable:/:Test:Value");
            EcellData data2 = new EcellData("Value1", new EcellValue(1), "Variable:/:Test:Value");
            EcellData data3 = new EcellData("Value", new EcellValue(2), "Variable:/:Test:Value");
            EcellData data4 = new EcellData("Value", new EcellValue(1), "Variable:/:Test:Test");
            EcellData data5 = new EcellData("Value", new EcellValue(1), "Variable:/:Test:Value");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data2);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data3);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data4);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;
            resultBoolean = data1.Equals(data5);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            // double
            data1 = new EcellData("Value", new EcellValue(0.1), "Variable:/:Test:Value");
            data2 = new EcellData("Value1", new EcellValue(0.1), "Variable:/:Test:Value");
            data3 = new EcellData("Value", new EcellValue(0.2), "Variable:/:Test:Value");
            data4 = new EcellData("Value", new EcellValue(0.1), "Variable:/:Test:Test");
            data5 = new EcellData("Value", new EcellValue(0.1), "Variable:/:Test:Value");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data2);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data3);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data4);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;
            resultBoolean = data1.Equals(data5);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            // string
            data1 = new EcellData("Value", new EcellValue("Test"), "Variable:/:Test:Value");
            data2 = new EcellData("Value1", new EcellValue("Test"), "Variable:/:Test:Value");
            data3 = new EcellData("Value", new EcellValue("Test2"), "Variable:/:Test:Value");
            data4 = new EcellData("Value", new EcellValue("Test"), "Variable:/:Test:Test");
            data5 = new EcellData("Value", new EcellValue("Test"), "Variable:/:Test:Value");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data2);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data3);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data4);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;
            resultBoolean = data1.Equals(data5);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            // List
            List<object> list1 = new List<object>();
            list1.Add("Test1");
            EcellValue value1 = new EcellValue(list1);
            List<object> list2 = new List<object>();
            list2.Add("Test2");
            EcellValue value2 = new EcellValue(list2);

            data1 = new EcellData("Value", value1.Clone(), "Variable:/:Test:Value");
            data2 = new EcellData("Value1", value1.Clone(), "Variable:/:Test:Value");
            data3 = new EcellData("Value", value2.Clone(), "Variable:/:Test:Value");
            data4 = new EcellData("Value", value1.Clone(), "Variable:/:Test:Test");
            data5 = new EcellData("Value", value1.Clone(), "Variable:/:Test:Value");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data2);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data3);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(data4);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = true;
            resultBoolean = data1.Equals(data5);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            expectedBoolean = false;
            resultBoolean = data1.Equals(new object());
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            EcellData data6 = data1.Clone();
            //object obj = null;
            //data6.Value = new EcellValue(obj);
            //expectedBoolean = false;
            //resultBoolean = data1.Equals(data6);
            //Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");
            //resultBoolean = data6.Equals(data1);
            //Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            data6.Value = null;
            resultBoolean = data1.Equals(data6);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");
            resultBoolean = data6.Equals(data1);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            data6 = data1.Clone();
            data6.Gettable = false;
            expectedBoolean = false;
            resultBoolean = data1.Equals(data6);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            data6 = data1.Clone();
            data6.Loadable = false;
            expectedBoolean = false;
            resultBoolean = data1.Equals(data6);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            data6 = data1.Clone();
            data6.Logable = true;
            expectedBoolean = false;
            resultBoolean = data1.Equals(data6);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            data6 = data1.Clone();
            data6.Logged = true;
            expectedBoolean = false;
            resultBoolean = data1.Equals(data6);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            data6 = data1.Clone();
            data6.Saveable = false;
            expectedBoolean = false;
            resultBoolean = data1.Equals(data6);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");

            data6 = data1.Clone();
            data6.Settable = false;
            expectedBoolean = false;
            resultBoolean = data1.Equals(data6);
            Assert.AreEqual(expectedBoolean, resultBoolean, "Equals method returned unexpected result.");
        }

        /// <summary>
        /// Test of IsInitialized
        /// </summary>
        [Test()]
        public void TestIsInitialized()
        {
            bool expectedBoolean = false;
            bool resultBoolean = false;

            EcellData data = new EcellData();
            expectedBoolean = false;
            resultBoolean = data.IsInitialized();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsInitialized method returned unexpected result.");

            data = new EcellData("Value", new EcellValue(1), "Variable:/:Test:Value");
            expectedBoolean = true;
            resultBoolean = data.IsInitialized();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsInitialized method returned unexpected result.");

            data = new EcellData("Value", new EcellValue(0.1), "Variable:/:Test:Value");
            expectedBoolean = true;
            resultBoolean = data.IsInitialized();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsInitialized method returned unexpected result.");

            data = new EcellData("Value", new EcellValue("data"), "Variable:/:Test:Value");
            expectedBoolean = false;
            resultBoolean = data.IsInitialized();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsInitialized method returned unexpected result.");

            data = new EcellData("Value", new EcellValue(new List<EcellValue>()), "Variable:/:Test:Value");
            expectedBoolean = false;
            resultBoolean = data.IsInitialized();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsInitialized method returned unexpected result.");

            data.Settable = false;
            expectedBoolean = false;
            resultBoolean = data.IsInitialized();
            Assert.AreEqual(expectedBoolean, resultBoolean, "IsInitialized method returned unexpected result.");

        }

        /// <summary>
        /// Test of Clone
        /// </summary>
        [Test()]
        public void TestClone()
        {
            EcellData data1 =new EcellData();
            EcellData data1Copy = data1.Clone();
            Assert.AreEqual(data1, data1Copy, "Clone method returned unexpected result.");

            EcellData data2 = new EcellData("Value", new EcellValue(0.1), "Variable:/:Test:Value");
            EcellData data2Copy = data2.Clone();
            Assert.AreEqual(data2, data2Copy, "Clone method returned unexpected result.");

            object obj = ((ICloneable)data1).Clone();
            Assert.AreEqual(data1, obj, "Clone method returned unexpected result.");

            obj = ((ICloneable)data2).Clone();
            Assert.AreEqual(data2, obj, "Clone method returned unexpected result.");
        }

        /// <summary>
        /// Test of GetHashCode()
        /// </summary>
        [Test()]
        public void TestGetHashCode()
        {
            EcellData data = new EcellData();
            EcellData dataCopy = data.Clone();
            Assert.AreEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            data = new EcellData("Value", new EcellValue(0.1), "Variable:/:Test:Value");
            dataCopy = data.Clone();
            Assert.AreEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            // NotEqual
            dataCopy = data.Clone();
            dataCopy.Gettable = false;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Settable = false;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Loadable = false;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Saveable = false;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Logable = true;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Logged = true;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Name = "Test";
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Name = null;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.EntityPath = "Variable:/:Test:Test";
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.EntityPath = null;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Value = new EcellValue(0);
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");

            dataCopy = data.Clone();
            dataCopy.Value = null;
            Assert.AreNotEqual(data.GetHashCode(), dataCopy.GetHashCode(), "TestGetHashCode method returned unexpected result.");
        }
    }
}
