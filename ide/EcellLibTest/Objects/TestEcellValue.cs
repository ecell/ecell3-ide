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

namespace Ecell.Objects
{
    using System;
    using NUnit.Framework;
    using Ecell.Objects;
    using System.Collections.Generic;
    using EcellCoreLib;
    using Ecell.Exceptions;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestEcellValue
    {

        private EcellValue _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellValue(0);
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
        /// Constructor with value.
        /// </summary>
        [Test()]
        public void TestConstructorEcellValueValue()
        {
            // int
            EcellValue value = null;
            value = new EcellValue(0);
            Assert.IsNotNull(value, "Constructor of type, EcellValue failed to create instance.");
            Assert.IsTrue(value.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(value.IsDouble, "IsDouble is not expected value.");
            Assert.IsFalse(value.IsList, "IsList is not expected value.");
            Assert.IsFalse(value.IsString, "IsString is not expected value.");
            Assert.AreEqual(0, (int)value.Value, "Value is not expected value.");
            Assert.AreEqual(EcellValueType.Integer, value.Type, "Type is not expected value.");

            // double
            value = null;
            value = new EcellValue(0.01);
            Assert.IsNotNull(value, "Constructor of type, EcellValue failed to create instance.");
            Assert.IsFalse(value.IsInt, "IsInt is not expected value.");
            Assert.IsTrue(value.IsDouble, "IsDouble is not expected value.");
            Assert.IsFalse(value.IsList, "IsList is not expected value.");
            Assert.IsFalse(value.IsString, "IsString is not expected value.");
            Assert.AreEqual(0.01, (double)value.Value, "Value is not expected value.");
            Assert.AreEqual(EcellValueType.Double, value.Type, "Type is not expected value.");

            // string.
            value = null;
            value = new EcellValue("test");
            Assert.IsNotNull(value, "Constructor of type, EcellValue failed to create instance.");
            Assert.IsFalse(value.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(value.IsDouble, "IsDouble is not expected value.");
            Assert.IsFalse(value.IsList, "IsList is not expected value.");
            Assert.IsTrue(value.IsString, "IsString is not expected value.");
            Assert.AreEqual("test", (string)value.Value, "Value is not expected value.");
            Assert.AreEqual(EcellValueType.String, value.Type, "Type is not expected value.");

        }

        /// <summary>
        /// Constructor with EcellReference.
        /// </summary>
        [Test()]
        public void TestConstructorEcellValueEr()
        {
            // EcellReference
            EcellReference er = new EcellReference("S1","Variable:/:S1",1,0);
            EcellValue testEcellValue = new EcellValue(er);
            Assert.IsNotNull(testEcellValue, "Constructor of type, EcellValue failed to create instance.");
            Assert.IsFalse(testEcellValue.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(testEcellValue.IsDouble, "IsDouble is not expected value.");
            Assert.IsTrue(testEcellValue.IsList, "IsList is not expected value.");
            Assert.IsFalse(testEcellValue.IsString, "IsString is not expected value.");
            Assert.AreEqual(EcellValueType.List, testEcellValue.Type, "Type is not expected value.");
            Assert.AreEqual(er.ToString(), testEcellValue.ToString(), "ToString() is not expected value.");

            // List
            EcellValue temp = new EcellValue(er);
            List<EcellValue> list = new List<EcellValue>();
            list.Add(temp);
            EcellValue value = new EcellValue(list);
            Assert.IsNotNull(value, "Constructor of type, EcellValue failed to create instance.");
            Assert.IsFalse(value.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(value.IsDouble, "IsDouble is not expected value.");
            Assert.IsTrue(value.IsList, "IsList is not expected value.");
            Assert.IsFalse(value.IsString, "IsString is not expected value.");
            Assert.AreEqual(EcellValueType.List, value.Type, "Type is not expected value.");

        }

        /// <summary>
        /// Constructor with EcellReference.
        /// </summary>
        [Test()]
        public void TestAccessor()
        {
            string str = "string"; ;
            EcellValue value = new EcellValue(str);
            Assert.AreEqual("string", value.Value);

            Type type = value.GetType();
            FieldInfo info = type.GetField("m_value", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(value, new object());
            try
            {
                ValueType vtype = value.Type;
                Assert.Fail();
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Test of ConvertFromListString()
        /// </summary>
        [Test()]
        public void TestConvertFromListString()
        {
            List<EcellReference> list = new List<EcellReference>();
            EcellReference er1 = new EcellReference("S1", "Variable:/:S1", 1, 0);
            EcellReference er2 = new EcellReference("S2", "Variable:/:S2", 1, 1);
            list.Add(er1);
            list.Add(er2);
            EcellValue value = EcellReference.ConvertToEcellValue(list);
            List<object> expectedList = (List<object>)value.Value;

            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            value = EcellValue.ConvertFromListString(str);
            List<object> resultList = (List<object>)value.Value;
            Assert.AreEqual(expectedList, resultList, "CastToList method returned unexpected result.");

            Assert.IsFalse(value.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(value.IsDouble, "IsDouble is not expected value.");
            Assert.IsTrue(value.IsList, "IsList is not expected value.");
            Assert.IsFalse(value.IsString, "IsString is not expected value.");

            str = "";
            value = EcellValue.ConvertFromListString(str);
            expectedList = new List<object>();
            resultList = (List<object>)value.Value;
            Assert.AreEqual(expectedList, resultList, "CastToList method returned unexpected result.");
        }

        /// <summary>
        /// Test of CastToDouble()
        /// </summary>
        [Test()]
        public void TestCastToDouble()
        {
            double expectedDouble = 0.0001;
            double resultDouble = 0.0001;
            EcellValue value = new EcellValue(expectedDouble);
            resultDouble = (double)value;
            Assert.IsTrue(value.IsDouble, "IsDouble is not expected value.");
            Assert.AreEqual(expectedDouble, resultDouble, "CastToDouble method returned unexpected result.");

            value = new EcellValue(1);
            resultDouble = (double)value;
            Assert.IsFalse(value.IsDouble, "IsDouble is not expected value.");
            Assert.AreEqual(1.0, resultDouble, "CastToDouble method returned unexpected result.");

            value = new EcellValue("0.01");
            resultDouble = (double)value;
            Assert.IsFalse(value.IsDouble, "IsDouble is not expected value.");
            Assert.AreEqual(0.01, resultDouble, "CastToDouble method returned unexpected result.");

            value = new EcellValue("Test");
            try
            {
                resultDouble = (double)value;
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            value = new EcellValue(new List<object>());
            try
            {
                resultDouble = (double)value;
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Test of CastToInt()
        /// </summary>
        [Test()]
        public void TestCastToInt()
        {
            int expectedInt32 = 10;
            int resultInt32 = 10;
            EcellValue value = new EcellValue(expectedInt32);
            resultInt32 = (int)value;
            Assert.IsTrue(value.IsInt, "IsInt is not expected value.");
            Assert.AreEqual(expectedInt32, resultInt32, "CastToInt method returned unexpected result.");

            value = new EcellValue(0.01);
            resultInt32 = (int)value;
            Assert.IsFalse(value.IsInt, "IsInt is not expected value.");
            Assert.AreEqual(0, resultInt32, "CastToInt method returned unexpected result.");

            value = new EcellValue("1");
            resultInt32 = (int)value;
            Assert.IsFalse(value.IsInt, "IsInt is not expected value.");
            Assert.AreEqual(1, resultInt32, "CastToInt method returned unexpected result.");

            value = new EcellValue("Test");
            try
            {
                resultInt32 = (int)value;
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            value = new EcellValue(new List<object>());
            try
            {
                resultInt32 = (int)value;
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Test of CastToList()
        /// </summary>
        [Test()]
        public void TestCastToList()
        {
            List<EcellReference> list = new List<EcellReference>();
            EcellReference er1 = new EcellReference("S1", "Variable:/:S1", 1, 0);
            EcellReference er2 = new EcellReference("S2", "Variable:/:S2", 1, 1);
            list.Add(er1);
            list.Add(er2);
            EcellValue resultValue = EcellReference.ConvertToEcellValue(list);
            List<object> expectedList = (List<object>)resultValue.Value;

            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            EcellValue value = EcellValue.ConvertFromListString(str);
            List<object> resultList = (List<object>)value.Value;
            Assert.AreEqual(expectedList, resultList, "CastToList method returned unexpected result.");

            value = EcellValue.ConvertFromListString("");
            resultList = (List<object>)value.Value;
            Assert.IsEmpty(resultList, "resultList shold be empty.");

        }

        /// <summary>
        /// Test of CastToString()
        /// </summary>
        [Test()]
        public void TestCastToString()
        {
            string expectedString;
            string resultString;
            EcellValue value;

            EcellReference er1 = new EcellReference("S1", "Variable:/:S1", 1, 0);
            value = new EcellValue(er1);
            expectedString = er1.ToString();
            resultString = (string)value;
            Assert.AreEqual(expectedString, resultString, "CastToString method returned unexpected result.");

            value = new EcellValue(1);
            expectedString = "1";
            resultString = (string)value;
            Assert.AreEqual(expectedString, resultString, "CastToString method returned unexpected result.");

            value = new EcellValue(0.0002);
            expectedString = "0.0002";
            resultString = (string)value;
            Assert.AreEqual(expectedString, resultString, "CastToString method returned unexpected result.");

            value = new EcellValue("string");
            expectedString = "string";
            resultString = (string)value;
            Assert.AreEqual(expectedString, resultString, "CastToString method returned unexpected result.");
        }

        /// <summary>
        /// Test of ToString()
        /// </summary>
        [Test()]
        public void TestToString()
        {
            string expectedString;
            string resultString;
            EcellValue value;

            EcellReference er1 = new EcellReference("S1", "Variable:/:S1", 1, 0);
            value = new EcellValue(er1);
            expectedString = er1.ToString();
            resultString = value.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            value = new EcellValue(new List<object>());
            expectedString = "()";
            resultString = value.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            List<object> list = new List<object>();
            list.Add(0.001);
            list.Add("Test");
            value = new EcellValue(list);
            expectedString = "(0.001, \"Test\")";
            resultString = value.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            value = new EcellValue(1);
            expectedString = "1";
            resultString = value.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            value = new EcellValue(0.0002);
            expectedString = "0.0002";
            resultString = value.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            value = new EcellValue("string");
            expectedString = "string";
            resultString = value.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString method returned unexpected result.");

            expectedString = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            value = EcellValue.ConvertFromListString(expectedString);
            resultString = value.ToString();
            Assert.AreEqual(expectedString, resultString, "CastToList method returned unexpected result.");

        }

        /// <summary>
        /// Test of Clone()
        /// </summary>
        [Test()]
        public void TestClone()
        {
            EcellValue value = new EcellValue(1);
            EcellValue clonedValue = value.Clone();
            Assert.IsNotNull(clonedValue, "Constructor of type, EcellValue failed to create instance.");
            Assert.AreEqual(value, clonedValue, "Clone method returned unexpected result.");
            Assert.IsTrue(clonedValue.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(clonedValue.IsDouble, "IsDouble is not expected value.");
            Assert.IsFalse(clonedValue.IsList, "IsList is not expected value.");
            Assert.IsFalse(clonedValue.IsString, "IsString is not expected value.");
            Assert.AreEqual(1, (int)clonedValue.Value, "Value is not expected value.");
            Assert.AreEqual(EcellValueType.Integer, clonedValue.Type, "Type is not expected value.");

            // double
            value = new EcellValue(0.01);
            clonedValue = value.Clone();
            Assert.IsNotNull(clonedValue, "Constructor of type, EcellValue failed to create instance.");
            Assert.AreEqual(value, clonedValue, "Clone method returned unexpected result.");
            Assert.IsFalse(clonedValue.IsInt, "IsInt is not expected value.");
            Assert.IsTrue(clonedValue.IsDouble, "IsDouble is not expected value.");
            Assert.IsFalse(clonedValue.IsList, "IsList is not expected value.");
            Assert.IsFalse(clonedValue.IsString, "IsString is not expected value.");
            Assert.AreEqual(0.01, (double)clonedValue.Value, "Value is not expected value.");
            Assert.AreEqual(EcellValueType.Double, clonedValue.Type, "Type is not expected value.");

            // string.
            value = new EcellValue("test");
            clonedValue = value.Clone();
            Assert.IsNotNull(clonedValue, "Constructor of type, EcellValue failed to create instance.");
            Assert.AreEqual((string)value, (string)clonedValue, "Clone method returned unexpected result.");
            Assert.IsFalse(clonedValue.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(clonedValue.IsDouble, "IsDouble is not expected value.");
            Assert.IsFalse(clonedValue.IsList, "IsList is not expected value.");
            Assert.IsTrue(clonedValue.IsString, "IsString is not expected value.");
            Assert.AreEqual("test", (string)clonedValue.Value, "Value is not expected value.");
            Assert.AreEqual(EcellValueType.String, clonedValue.Type, "Type is not expected value.");

            // list
            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            value = EcellValue.ConvertFromListString(str);
            clonedValue = value.Clone();
            Assert.IsNotNull(clonedValue, "Constructor of type, EcellValue failed to create instance.");
            Assert.AreEqual((string)value, (string)clonedValue, "Clone method returned unexpected result.");
            Assert.IsFalse(clonedValue.IsInt, "IsInt is not expected value.");
            Assert.IsFalse(clonedValue.IsDouble, "IsDouble is not expected value.");
            Assert.IsTrue(clonedValue.IsList, "IsList is not expected value.");
            Assert.IsFalse(clonedValue.IsString, "IsString is not expected value.");
            Assert.AreEqual(str, clonedValue.ToString(), "ToString method returned unexpected value.");
            Assert.AreEqual(EcellValueType.List, clonedValue.Type, "Type is not expected value.");

            object obj = ((ICloneable)value).Clone();
            Assert.IsNotNull(obj, "Constructor of type, EcellValue failed to create instance.");

            try
            {
                obj = null;
                value = new EcellValue(obj);
                EcellValue newValue = value.Clone();
                Assert.Fail("Failed to throw TypeError Exception.");
            }
            catch (EcellException)
            {
            }
        }

        /// <summary>
        /// Test of GetHashCode()
        /// </summary>
        [Test()]
        public void TestGetHashCode()
        {
            EcellValue value1 = new EcellValue(1);
            EcellValue value2 = value1.Clone();
            Assert.AreEqual(value1.GetHashCode(), value2.GetHashCode(), "Clone method returned unexpected result.");
        }

        /// <summary>
        /// Test of Clone()
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            // Compare int.
            EcellValue value1 = new EcellValue(1);
            EcellValue value2 = value1.Clone();
            Assert.IsTrue(value1.Equals(value2), "Equals method returned unexpected result.");
            Assert.IsTrue(value1.Equals(1), "Equals method returned unexpected result.");
            Assert.IsFalse(value1.Equals(1.0), "Equals method returned unexpected result.");

            value2 = null;
            Assert.IsFalse(value1.Equals(value2), "Equals method returned unexpected result.");
            Assert.IsFalse(value1.Equals(new object()), "Equals method returned unexpected result.");

            // Compare double.
            value1 = new EcellValue(0.01);
            value2 = value1.Clone();
            Assert.IsTrue(value1.Equals(value2), "Equals method returned unexpected result.");
            Assert.IsTrue(value1.Equals(0.01), "Equals method returned unexpected result.");

            // Compare string.
            value1 = new EcellValue("test");
            value2 = value1.Clone();
            Assert.IsTrue(value1.Equals(value2), "Equals method returned unexpected result.");
            Assert.IsTrue(value1.Equals("test"), "Equals method returned unexpected result.");

            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            value1 = EcellValue.ConvertFromListString(str);
            value2 = value1.Clone();
            Assert.AreEqual((List<object>)value1.Value, (List<object>)value2.Value, "Equals method returned unexpected result.");
            Assert.IsTrue(value1.Equals((List<object>)value2.Value), "Equals method returned unexpected result.");

            str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S3\", \"Variable:/:S3\", 1, 1))";
            value2 = EcellValue.ConvertFromListString(str);
            Assert.AreNotEqual((List<object>)value1.Value, (List<object>)value2.Value, "Equals method returned unexpected result.");
            Assert.IsFalse(value1.Equals((List<object>)value2.Value), "Equals method returned unexpected result.");

            str = "((\"S1\", \"Variable:/:S1\", 1, 0))";
            value2 = EcellValue.ConvertFromListString(str);
            Assert.AreNotEqual((List<object>)value1.Value, (List<object>)value2.Value, "Equals method returned unexpected result.");
            Assert.IsFalse(value1.Equals((List<object>)value2.Value), "Equals method returned unexpected result.");
        }
    }
}
