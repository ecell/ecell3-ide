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
using Ecell.Exceptions;

namespace Ecell.Objects
{
    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestEcellLayer
    {
        private EcellLayer _unitUnderTest;

        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EcellLayer();
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
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestConstructor()
        {
            EcellLayer el = new EcellLayer();
            Assert.IsNotNull(el, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual(null, el.Name, "Name is unexpected value.");
            Assert.AreEqual(false, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"\", 0)", el.ToString(), "ToString method returned unexpected value.");

            el.Name = "Name1";
            el.Visible = true;
            Assert.AreEqual("Name1", el.Name, "Name is unexpected value.");
            Assert.AreEqual(true, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Name1\", 1)", el.ToString(), "ToString method returned unexpected value.");

            el.Name = "Name2";
            el.Visible = false;
            Assert.AreEqual("Name2", el.Name, "Name is unexpected value.");
            Assert.AreEqual(false, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Name2\", 0)", el.ToString(), "ToString method returned unexpected value.");

        }

        /// <summary>
        /// TestConstructor
        /// </summary>
        [Test()]
        public void TestConstructorWithString()
        {
            string str = null;
            EcellLayer el;
            try
            {
                el = new EcellLayer(str);
                Assert.Fail("Failed to check null.");
            }
            catch (EcellException)
            {
            }

            try
            {
                el = new EcellLayer("aaa, 0");
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            try
            {
                el = new EcellLayer("(aaa, bbb)");
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }
            str = "(\"name\", 1)";
            el = new EcellLayer(str);
            Assert.IsNotNull(el, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual("name", el.Name, "Name is unexpected value.");
            Assert.AreEqual(true, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"name\", 1)", el.ToString(), "ToString method returned unexpected value.");
        }
        
        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestConstructorWithParams()
        {
            EcellLayer el;

            el = new EcellLayer("", false);
            Assert.IsNotNull(el, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual("", el.Name, "Name is unexpected value.");
            Assert.AreEqual(false, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"\", 0)", el.ToString(), "ToString method returned unexpected value.");

            el = new EcellLayer("Name", true);
            Assert.IsNotNull(el, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual("Name", el.Name, "Name is unexpected value.");
            Assert.AreEqual(true, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Name\", 1)", el.ToString(), "ToString method returned unexpected value.");

        }

        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestConstructorWithEcellValue()
        {
            EcellValue value = null;
            EcellLayer el;
            try
            {
                el = new EcellLayer(value);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            try
            {
                el = new EcellLayer(new EcellValue(1));
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            try
            {
                value = new EcellValue(new EcellReference("S1", "/:S1", 0, 0));
                el = new EcellLayer(value);
                Assert.Fail("Failed to throw EcellException.");
            }
            catch (EcellException)
            {
            }

            List<EcellValue> list = new List<EcellValue>();
            list.Add(new EcellValue("Name"));
            list.Add(new EcellValue(1));
            value = new EcellValue(list);

            el = new EcellLayer(value);
            Assert.IsNotNull(el, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual("Name", el.Name, "Name is unexpected value.");
            Assert.AreEqual(true, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Name\", 1)", el.ToString(), "ToString method returned unexpected value.");

        }
                
        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestConvertFromString()
        {
            string str = "";
            List<EcellLayer> layers = EcellLayer.ConvertFromString(str);
            Assert.IsEmpty(layers, "ConvertFromString method returned unexpected value.");

            str = "((\"layer0\", 1), (\"Layer1\", 0))";
            layers = EcellLayer.ConvertFromString(str);
            Assert.IsNotEmpty(layers, "ConvertFromString method returned unexpected value.");

            Assert.AreEqual("layer0", layers[0].Name, "Name is unexpected value.");
            Assert.AreEqual(true, layers[0].Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"layer0\", 1)", layers[0].ToString(), "ToString method returned unexpected value.");

            Assert.AreEqual("Layer1", layers[1].Name, "Name is unexpected value.");
            Assert.AreEqual(false, layers[1].Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Layer1\", 0)", layers[1].ToString(), "ToString method returned unexpected value.");

        }
        
        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestConvertToEcellValue()
        {
            List<EcellLayer> layers = null;
            EcellValue value = EcellLayer.ConvertToEcellValue(layers);
            Assert.IsNotNull(value, "TestConvertToEcellValue method returned unexpected value.");
            Assert.AreEqual(true, value.IsList, "IsList is unexpected value.");
            Assert.IsEmpty(value.CastToList(), "TestConvertToEcellValue method returned unexpected value.");

            layers = new List<EcellLayer>();
            value = EcellLayer.ConvertToEcellValue(layers);
            Assert.IsNotNull(value, "TestConvertToEcellValue method returned unexpected value.");
            Assert.AreEqual(true, value.IsList, "IsList is unexpected value.");
            Assert.IsEmpty(value.CastToList(), "TestConvertToEcellValue method returned unexpected value.");

            string str = "((\"layer0\", 1), (\"Layer1\", 0))";
            layers = EcellLayer.ConvertFromString(str);
            value = EcellLayer.ConvertToEcellValue(layers);
            Assert.IsNotNull(value, "TestConvertToEcellValue method returned unexpected value.");
            Assert.AreEqual(true, value.IsList, "IsList is unexpected value.");

            List<EcellValue> list = value.CastToList();
            Assert.IsNotEmpty(list, "CastToList method returned unexpected value.");

            Assert.AreEqual("layer0", list[0].CastToList()[0].CastToString(), "Name is unexpected value.");
            Assert.AreEqual(1, list[0].CastToList()[1].CastToInt(), "Name is unexpected value.");
            Assert.AreEqual("(\"layer0\", 1)", list[0].ToString(), "ToString method returned unexpected value.");

            Assert.AreEqual("Layer1", list[1].CastToList()[0].CastToString(), "Name is unexpected value.");
            Assert.AreEqual(0, list[1].CastToList()[1].CastToInt(), "Name is unexpected value.");
            Assert.AreEqual("(\"Layer1\", 0)", list[1].ToString(), "ToString method returned unexpected value.");

        }

        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestConvertFromEcellValue()
        {
            EcellValue value = null;
            List<EcellLayer> layers = EcellLayer.ConvertFromEcellValue(value);
            Assert.IsEmpty(layers, "ConvertFromEcellValue method returned unexpected value.");

            List<EcellValue> list = new List<EcellValue>();
            value = new EcellValue(list);
            layers = EcellLayer.ConvertFromEcellValue(value);
            Assert.IsEmpty(layers, "ConvertFromEcellValue method returned unexpected value.");

            string str = "((\"layer0\", 1), (\"Layer1\", 0))";
            layers = EcellLayer.ConvertFromString(str);
            value = EcellLayer.ConvertToEcellValue(layers);
            layers = EcellLayer.ConvertFromEcellValue(value);
            Assert.IsNotEmpty(layers, "ConvertFromEcellValue method returned unexpected value.");

            Assert.AreEqual("layer0", layers[0].Name, "Name is unexpected value.");
            Assert.AreEqual(true, layers[0].Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"layer0\", 1)", layers[0].ToString(), "ToString method returned unexpected value.");

            Assert.AreEqual("Layer1", layers[1].Name, "Name is unexpected value.");
            Assert.AreEqual(false, layers[1].Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Layer1\", 0)", layers[1].ToString(), "ToString method returned unexpected value.");
        }
        
        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestClone()
        {
            EcellLayer el;
            List<EcellValue> list = new List<EcellValue>();
            list.Add(new EcellValue("Name"));
            list.Add(new EcellValue(1));
            EcellValue value = new EcellValue(list);

            el = new EcellLayer(value);
            Assert.IsNotNull(el, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual("Name", el.Name, "Name is unexpected value.");
            Assert.AreEqual(true, el.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Name\", 1)", el.ToString(), "ToString method returned unexpected value.");

            EcellLayer el1 = el.Clone();
            Assert.IsNotNull(el1, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el1.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual("Name", el1.Name, "Name is unexpected value.");
            Assert.AreEqual(true, el1.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Name\", 1)", el1.ToString(), "ToString method returned unexpected value.");

            EcellLayer el2 = (EcellLayer)((ICloneable)el).Clone();
            Assert.IsNotNull(el2, "Constructor of type, object failed to create instance.");
            Assert.AreEqual("Ecell.Objects.EcellLayer", el2.GetType().ToString(), "GetType method returned unexpected value.");
            Assert.AreEqual("Name", el2.Name, "Name is unexpected value.");
            Assert.AreEqual(true, el2.Visible, "Name is unexpected value.");
            Assert.AreEqual("(\"Name\", 1)", el2.ToString(), "ToString method returned unexpected value.");

        }
        
        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestEquals()
        {
            EcellLayer layer1 = new EcellLayer("Layer0", true);
            EcellLayer layer2 = new EcellLayer("Layer1", true);
            EcellLayer layer3 = new EcellLayer("Layer0", false);
            EcellLayer layer4 = new EcellLayer("Layer0", true);

            Assert.AreEqual(false, layer1.Equals(new object()), "Equals method returned unexpected value.");
            Assert.AreEqual(false, layer1.Equals(layer2), "Equals method returned unexpected value.");
            Assert.AreEqual(false, layer1.Equals(layer3), "Equals method returned unexpected value.");
            Assert.AreEqual(true, layer1.Equals(layer4), "Equals method returned unexpected value.");
        }

        /// <summary>
        /// TestConstructorWithParams
        /// </summary>
        [Test()]
        public void TestGetHashCode()
        {
            EcellLayer layer1 = new EcellLayer("Layer0", true);
            EcellLayer layer2 = new EcellLayer("Layer1", true);
            EcellLayer layer3 = new EcellLayer("Layer0", false);
            EcellLayer layer4 = new EcellLayer("Layer0", true);

            Assert.AreNotEqual(layer1.GetHashCode(), layer2.GetHashCode(), "GetHashCode method returned unexpected value.");
            Assert.AreNotEqual(layer1.GetHashCode(), layer3.GetHashCode(), "GetHashCode method returned unexpected value.");
            Assert.AreEqual(layer1.GetHashCode(), layer4.GetHashCode(), "GetHashCode method returned unexpected value.");

            EcellLayer layer = new EcellLayer();
            Assert.AreEqual(0, layer.GetHashCode(), "GetHashCode method returned unexpected value.");
        }
    }
}
