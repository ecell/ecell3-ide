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
    using System.Collections.Generic;
    using Ecell.Exceptions;
    using Ecell.Objects;

    /// <summary>
    /// 
    /// </summary>
    [TestFixture()]
    public class TestEcellReference
    {

        private EcellReference _unitUnderTest;

        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellReference();
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
        public void TestConstructorEcellReference()
        {
            EcellReference er = null;
            er = new EcellReference();
            Assert.IsNotNull(er, "Constructor of type, EcellReference failed to create instance.");
            Assert.IsNull(er.Name, "Name is not null.");
            Assert.IsNull(er.FullID, "FullID is not null.");
            Assert.IsNull(er.Key, "Key is not null.");
            Assert.AreEqual(0, er.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(0, er.IsAccessor, "IsAccessor is not expected value.");

            er = null;
            er = new EcellReference(
                "S1",
                "Variable:/:S1",
                1,
                0);
            Assert.IsNotNull(er, "Constructor of type, EcellReference failed to create instance.");
            Assert.AreEqual("S1", er.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er.Key, "Key is not expected value.");
            Assert.AreEqual(1, er.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(0, er.IsAccessor, "IsAccessor is not expected value.");
        }

        /// <summary>
        /// Test of Constructor with parameters.
        /// </summary>
        [Test()]
        public void TestConstructorEcellReferenceNameFullIDCoefAccessor()
        {
            string name = "S1";
            string fullID = "Variable:/:S1";
            int coef = 1;
            int accessor = 0;
            EcellReference er = new EcellReference(name, fullID, coef, accessor);
            Assert.IsNotNull(er, "Constructor of type, EcellReference failed to create instance.");
            Assert.AreEqual("S1", er.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er.Key, "Key is not expected value.");
            Assert.AreEqual(1, er.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(0, er.IsAccessor, "IsAccessor is not expected value.");
        }

        /// <summary>
        /// Test of Constructor with string.
        /// </summary>
        [Test()]
        public void TestConstructorEcellReferenceStr()
        {
            string str1 = "(\"S1\", \"Variable:/:S1\", \"1\", \"0\")";
            EcellReference er1 = new EcellReference(str1);
            Assert.IsNotNull(er1, "Constructor of type, EcellReference failed to create instance.");
            Assert.AreEqual("S1", er1.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er1.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er1.Key, "Key is not expected value.");
            Assert.AreEqual(1, er1.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(0, er1.IsAccessor, "IsAccessor is not expected value.");

            string str2 = "(\"S1\", \"Variable:/:S1\", 1, 0)";
            EcellReference er2 = new EcellReference(str2);
            Assert.IsNotNull(er2, "Constructor of type, string failed to create instance.");
            Assert.AreEqual("S1", er2.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er2.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er2.Key, "Key is not expected value.");
            Assert.AreEqual(1, er2.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(0, er2.IsAccessor, "IsAccessor is not expected value.");

            string str3 = "(\"S1\", \"Variable:/:S1\", \"1\")";
            EcellReference er3 = new EcellReference(str3);
            Assert.IsNotNull(er3, "Constructor of type, EcellReference failed to create instance.");
            Assert.AreEqual("S1", er3.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er3.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er3.Key, "Key is not expected value.");
            Assert.AreEqual(1, er3.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(1, er3.IsAccessor, "IsAccessor is not expected value.");

            string str4 = "(\"S1\", \"Variable:/:S1\", 1)";
            EcellReference er4 = new EcellReference(str4);
            Assert.IsNotNull(er2, "Constructor of type, EcellReference failed to create instance.");
            Assert.AreEqual("S1", er4.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er4.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er4.Key, "Key is not expected value.");
            Assert.AreEqual(1, er4.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(1, er4.IsAccessor, "IsAccessor is not expected value.");

            string str5 = "(\"S1\", \"Variable:/:S1\")";
            EcellReference er5 = new EcellReference(str5);
            Assert.IsNotNull(er5, "Constructor of type, EcellReference failed to create instance.");
            Assert.AreEqual("S1", er5.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er5.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er5.Key, "Key is not expected value.");
            Assert.AreEqual(0, er5.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(1, er5.IsAccessor, "IsAccessor is not expected value.");

            try
            {
                string strNull = null;
                EcellReference er6 = new EcellReference(strNull);
                Assert.Fail("Failed to check null.");
            }
            catch (EcellException)
            {
            }

            try
            {
                EcellReference er7 = new EcellReference("");
                Assert.Fail("Failed to check empty.");
            }
            catch (EcellException)
            {
            }

            try
            {
                EcellReference er8 = new EcellReference("hoge");
                Assert.Fail("Failed to throw parsing error.");
            }
            catch (EcellException)
            {
            }

        }

        /// <summary>
        /// Test of Constructor with EcellValue.
        /// </summary>
        [Test()]
        public void TestConstructorEcellReferenceValue()
        {
            string str1 = "(\"S1\", \"Variable:/:S1\", \"1\", \"0\")";
            EcellReference er1 = new EcellReference(str1);
            EcellValue value = new EcellValue(er1);

            EcellReference er2 = new EcellReference(value);
            Assert.IsNotNull(er2, "Constructor of type, EcellReference failed to create instance.");
            Assert.AreEqual("S1", er2.Name, "Name is not expected value.");
            Assert.AreEqual("Variable:/:S1", er2.FullID, "FullID is not expected value.");
            Assert.AreEqual("/:S1", er2.Key, "Key is not expected value.");
            Assert.AreEqual(1, er2.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(0, er2.IsAccessor, "IsAccessor is not expected value.");

            try
            {
                EcellReference er3 = new EcellReference(new EcellValue(0));
                Assert.Fail("Failed to throw parsing error.");
            }
            catch (EcellException)
            {
            }

            try
            {
                List<object> list = null;
                EcellReference er4 = new EcellReference((IEnumerator<object>)list);
                Assert.Fail("Failed to throw parsing error.");
            }
            catch (EcellException)
            {
            }
        }

        /// <summary>
        /// Test ToString()
        /// </summary>
        [Test()]
        public void TestAccessors()
        {
            string str1 = "(\"S1\", \"Variable:/:S1\", -1)";
            EcellReference er1 = new EcellReference(str1);

            EcellReference er2 = new EcellReference();
            er2.Name = "S1";
            er2.FullID = "Variable:/:S1";
            er2.Coefficient = -1;
            er2.IsAccessor = 1;

            Assert.AreEqual(er1.Name, er2.Name, "Name is not expected value.");
            Assert.AreEqual(er1.FullID, er2.FullID, "FullID is not expected value.");
            Assert.AreEqual(er1.Key, er2.Key, "Key is not expected value.");
            Assert.AreEqual(er1.Coefficient, er2.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(er1.IsAccessor, er2.IsAccessor, "IsAccessor is not expected value.");

            EcellReference er3 = new EcellReference();
            er3.Name = "S1";
            er3.Key = "/:S1";
            er3.Coefficient = -1;
            er3.IsAccessor = 1;

            Assert.AreEqual(er1.Name, er3.Name, "Name is not expected value.");
            Assert.AreEqual(er1.FullID, er3.FullID, "FullID is not expected value.");
            Assert.AreEqual(er1.Key, er3.Key, "Key is not expected value.");
            Assert.AreEqual(er1.Coefficient, er3.Coefficient, "Coefficient is not expected value.");
            Assert.AreEqual(er1.IsAccessor, er3.IsAccessor, "IsAccessor is not expected value.");

            try
            {
                EcellReference er4 = new EcellReference();
                er4.Name = "S1";
                er4.FullID = null;
                er4.Coefficient = -1;
                er4.IsAccessor = 1;

                string key = er4.Key;

                er4.FullID = "/S1";
                key = er4.Key;

                Assert.Fail("Failed to throw parsing error.");
            }
            catch(EcellException)
            {
            }

        }
        /// <summary>
        /// Test ToString()
        /// </summary>
        [Test()]
        public void TestToString()
        {
            string expectedString = "(\"S1\", \"Variable:/:S1\", 1, 0)";
            string resultString = "";
            EcellReference er = new EcellReference(expectedString);
            resultString = er.ToString();
            Assert.AreEqual(expectedString, resultString, "ToString() method returned unexpected result.");

        }

        /// <summary>
        /// Test ToString()
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            string str1 = "(\"S1\", \"Variable:/:S1\", 1, 0)";
            EcellReference er1 = new EcellReference(str1);
            Assert.IsFalse(er1.Equals(null), "Equals method returns unexpected value.");

            string str2 = "(\"S2\", \"Variable:/:S1\", 1, 0)";
            EcellReference er2 = new EcellReference(str2);
            Assert.IsFalse(er1.Equals(er2), "Equals method returns unexpected value.");

            string str3 = "(\"S1\", \"Variable:/:S2\", 1, 0)";
            EcellReference er3 = new EcellReference(str3);
            Assert.IsFalse(er1.Equals(er3), "Equals method returns unexpected value.");

            string str4 = "(\"S1\", \"Variable:/:S1\", -1, 0)";
            EcellReference er4 = new EcellReference(str4);
            Assert.IsFalse(er1.Equals(er4), "Equals method returns unexpected value.");

            string str5 = "(\"S1\", \"Variable:/:S1\", 1, 1)";
            EcellReference er5 = new EcellReference(str5);
            Assert.IsFalse(er1.Equals(er5), "Equals method returns unexpected value.");

            string str6 = "(\"S1\", \"Variable:/:S1\", 1, 0)";
            EcellReference er6 = new EcellReference(str6);
            Assert.IsTrue(er1.Equals(er6), "Equals method returns unexpected value.");

        }

        /// <summary>
        /// Test ToString()
        /// </summary>
        [Test()]
        public void TestGetHashCode()
        {
            string str1 = "(\"S1\", \"Variable:/:S1\", 1, 0)";
            EcellReference er1 = new EcellReference(str1);

            string str2 = "(\"S2\", \"Variable:/:S1\", 1, 0)";
            EcellReference er2 = new EcellReference(str2);

            string str3 = "(\"S1\", \"Variable:/:S2\", 1, 0)";
            EcellReference er3 = new EcellReference(str3);

            string str4 = "(\"S1\", \"Variable:/:S1\", -1, 0)";
            EcellReference er4 = new EcellReference(str4);

            string str5 = "(\"S1\", \"Variable:/:S1\", 1, 1)";
            EcellReference er5 = new EcellReference(str5);

            string str6 = "(\"S1\", \"Variable:/:S1\", 1, 0)";
            EcellReference er6 = new EcellReference(str6);

            Assert.AreNotEqual(er1.GetHashCode(), er2.GetHashCode(), "GetHashCode() method returns unexpected value.");
            Assert.AreNotEqual(er1.GetHashCode(), er3.GetHashCode(), "GetHashCode() method returns unexpected value.");
            Assert.AreNotEqual(er1.GetHashCode(), er4.GetHashCode(), "GetHashCode() method returns unexpected value.");
            Assert.AreNotEqual(er1.GetHashCode(), er5.GetHashCode(), "GetHashCode() method returns unexpected value.");
            Assert.AreEqual(er1.GetHashCode(), er6.GetHashCode(), "GetHashCode() method returns unexpected value.");
        }

        /// <summary>
        /// Test of Clone()
        /// </summary>
        [Test()]
        public void TestClone()
        {
            string str = "(\"S1\", \"Variable:/:S1\", 1, 0)";

            EcellReference er1 = new EcellReference(str);
            EcellReference er2 = er1.Clone();
            Assert.AreEqual(er1.Name, er2.Name);
            Assert.AreEqual(er1.FullID, er2.FullID);
            Assert.AreEqual(er1.Coefficient, er2.Coefficient);
            Assert.AreEqual(er1.IsAccessor, er2.IsAccessor);
            Assert.AreEqual(er1, er2, "Copy method returned unexpected result.");

            List<EcellReference> list1 = new List<EcellReference>();
            EcellReference er3 = (EcellReference)((ICloneable)er1).Clone();
            Assert.AreEqual(er1.Name, er3.Name);
            Assert.AreEqual(er1.FullID, er3.FullID);
            Assert.AreEqual(er1.Coefficient, er3.Coefficient);
            Assert.AreEqual(er1.IsAccessor, er3.IsAccessor);
            Assert.AreEqual(er1, er3, "Copy method returned unexpected result.");
        }

        /// <summary>
        /// Test of ConvertFromString()
        /// </summary>
        [Test()]
        public void TestConvertFromString()
        {
            List<EcellReference> expectedList = new List<EcellReference>();
            EcellReference er1 = new EcellReference("S1", "Variable:/:S1", 1, 0);
            EcellReference er2 = new EcellReference("S2", "Variable:/:S2", 1, 1);
            expectedList.Add(er1);
            expectedList.Add(er2);

            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            List<EcellReference> resultList = EcellReference.ConvertFromString(str);

            Assert.AreEqual(expectedList, resultList, "ConvertFromString method returned unexpected result.");
            Assert.AreEqual(expectedList[0], resultList[0], "EcellReference is not expected value.");
            Assert.AreEqual(expectedList[1], resultList[1], "EcellReference is not expected value.");

            List<EcellReference> empty = EcellReference.ConvertFromString("");
            Assert.IsEmpty(empty, "Returned List shold be empty.");
        }

        /// <summary>
        /// Test of ConvertFromEcellValue()
        /// </summary>
        [Test()]
        public void TestConvertFromEcellValue()
        {
            List<EcellReference> expectedList = new List<EcellReference>();
            EcellReference er1 = new EcellReference("S1", "Variable:/:S1", 1, 0);
            EcellReference er2 = new EcellReference("S2", "Variable:/:S2", 1, 1);
            expectedList.Add(er1);
            expectedList.Add(er2);

            string str = "((\"S1\", \"Variable:/:S1\", 1, 0), (\"S2\", \"Variable:/:S2\", 1, 1))";
            EcellValue value = EcellValue.ConvertFromListString(str);
            List<EcellReference> resultList = EcellReference.ConvertFromEcellValue(value);
            Assert.AreEqual(expectedList, resultList, "ConvertFromEcellValue method returned unexpected result.");
            Assert.AreEqual(expectedList[0], resultList[0], "EcellReference is not expected value.");
            Assert.AreEqual(expectedList[1], resultList[1], "EcellReference is not expected value.");

            List<EcellReference> empty1 = EcellReference.ConvertFromEcellValue(new EcellValue(0));
            Assert.IsEmpty(empty1, "Returned List shold be empty.");

            List<EcellReference> empty2 = EcellReference.ConvertFromEcellValue(new EcellValue(new List<EcellValue>()));
            Assert.IsEmpty(empty2, "Returned List shold be empty.");

            List<EcellReference> empty3 = EcellReference.ConvertFromEcellValue(null);
            Assert.IsEmpty(empty3, "Returned List shold be empty.");
        }

        /// <summary>
        /// Test of ConvertToEcellValue()
        /// </summary>
        [Test()]
        public void TestConvertToEcellValue()
        {
            string str = "((\"S1\", \"Variable:/:S1\", -1, 0), (\"S2\", \"Variable:/:S2\", -1, 1))";
            EcellValue expectedEcellValue = EcellValue.ConvertFromListString(str);

            List<EcellReference> list = new List<EcellReference>();
            EcellReference er1 = new EcellReference("S1", "Variable:/:S1", -1, 0);
            EcellReference er2 = new EcellReference("S2", "Variable:/:S2", -1, 1);
            list.Add(er1);
            list.Add(er2);
            EcellValue resultEcellValue = EcellReference.ConvertToEcellValue(list);

            Assert.IsTrue(expectedEcellValue.Equals(resultEcellValue), "ConvertToEcellValue method returned unexpected result.");
            Assert.AreEqual(expectedEcellValue.ToString(), resultEcellValue.ToString(), "ConvertToEcellValue method returned unexpected result.");

            EcellValue empty1 = EcellReference.ConvertToEcellValue(new List<EcellReference>());
            Assert.IsEmpty((List<object>)empty1.Value, "Returned List shold be empty.");

            EcellValue empty2 = EcellReference.ConvertToEcellValue(null);
            Assert.IsEmpty((List<object>)empty2.Value, "Returned List shold be empty.");
        }
    }
}
