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
using Ecell.Objects;
using libsbml;
using System.Reflection;

namespace Ecell.SBML
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestEML2SBML
    {
        private EML2SBML _unitUnderTest;
        /// <summary>
        /// Constructor
        /// </summary>
        [SetUp()]
        public void SetUp()
        {
            _unitUnderTest = new EML2SBML();
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
        /// TestConvert
        /// </summary>
        [Test()]
        public void TestConvert()
        {
            EML2SBML.Convert(TestConstant.Model_Oscillation);
            EML2SBML.Convert(TestConstant.Model_Drosophila, TestConstant.TestDirectory + "Drosophila.sbml");

            try
            {
                EML2SBML.Convert("c:/Hoge.eml");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// TestParse
        /// </summary>
        [Test()]
        public void TestParse()
        {
            EcellModel model = EmlReader.Parse(TestConstant.Model_Drosophila, new EcellCoreLib.WrappedSimulator(Util.GetDMDirs()));
            libsbml.SBMLDocument doc = EML2SBML.convertToSBMLModel(model, "Drosophila", 2, 3);
            Assert.IsNotNull(doc, "Convert method returned unexpected value.");

            doc = null;
            model = SBML2EML.Convert(TestConstant.TestDirectory + "BIOMD0000000003.xml");
            doc = EML2SBML.convertToSBMLModel(model, "Drosophila", 2, 3);
            Assert.IsNotNull(doc, "Convert method returned unexpected value.");
        }
        
        /// <summary>
        /// TestCheckDelayType
        /// </summary>
        [Test()]
        public void TestCheckDelayType()
        {
            ASTNode node = libsbml.libsbml.parseFormula("2 * K");
            // GetModuleType
            Type type = _unitUnderTest.GetType();
            MethodInfo info = type.GetMethod("setDelayType", BindingFlags.NonPublic | BindingFlags.Static);
            ASTNode value = (ASTNode)info.Invoke(_unitUnderTest, new object[] { node });
            Assert.AreNotEqual(libsbml.libsbml.AST_FUNCTION_DELAY, value.getType(), "setDelayType method returns unexpected value.");

            node = libsbml.libsbml.parseFormula("log( 2 )");
            value = (ASTNode)info.Invoke(_unitUnderTest, new object[] { node });
            Assert.AreNotEqual(libsbml.libsbml.AST_FUNCTION_DELAY, value.getType(), "setDelayType method returns unexpected value.");

            node = libsbml.libsbml.parseFormula("delay(x, 2)");
            value = (ASTNode)info.Invoke(_unitUnderTest, new object[] { node });
            Assert.AreEqual(libsbml.libsbml.AST_FUNCTION_DELAY, value.getType(), "setDelayType method returns unexpected value.");

        }

                
        /// <summary>
        /// TestGetCurrentCompartment
        /// </summary>
        [Test()]
        public void TestGetCurrentCompartment()
        {
            // GetModuleType
            Type type = _unitUnderTest.GetType();
            MethodInfo info = type.GetMethod("getCurrentCompartment", BindingFlags.NonPublic | BindingFlags.Static);

            string compartment = (string)info.Invoke(_unitUnderTest, new object[] { "/" });
            Assert.AreEqual("default", compartment, "getCurrentCompartment method returns unexpected value.");

            compartment = (string)info.Invoke(_unitUnderTest, new object[] { "/CELL" });
            Assert.AreEqual("CELL", compartment, "getCurrentCompartment method returns unexpected value.");
        }

        
        /// <summary>
        /// TestGetCurrentCompartment
        /// </summary>
        [Test()]
        public void TestGetCompartmentID()
        {
            // GetModuleType
            Type type = _unitUnderTest.GetType();
            MethodInfo info = type.GetMethod("getCompartmentID", BindingFlags.NonPublic | BindingFlags.Static);

            string compartment = (string)info.Invoke(_unitUnderTest, new object[] { "Variable:/:V0", "/"});
            Assert.AreEqual("default", compartment, "getCurrentCompartment method returns unexpected value.");

            compartment = (string)info.Invoke(_unitUnderTest, new object[] { "Variable:/CELL/Drosophila:V0", "/CELL"});
            Assert.AreEqual("Drosophila", compartment, "getCurrentCompartment method returns unexpected value.");

            compartment = (string)info.Invoke(_unitUnderTest, new object[] { "Variable:.:V0", "/CELL"});
            Assert.AreEqual("CELL", compartment, "getCurrentCompartment method returns unexpected value.");

            compartment = (string)info.Invoke(_unitUnderTest, new object[] { "Variable:.:V0", "/"});
            Assert.AreEqual("default", compartment, "getCurrentCompartment method returns unexpected value.");

        }
        
        /// <summary>
        /// TestGetCurrentCompartment
        /// </summary>
        [Test()]
        public void TestGetVariableReferenceId()
        {
            // GetModuleType
            Type type = _unitUnderTest.GetType();
            MethodInfo info = type.GetMethod("getVariableReferenceId", BindingFlags.NonPublic | BindingFlags.Static);

            string varRefID = (string)info.Invoke(_unitUnderTest, new object[] { "Variable:/:V0", "/"});
            Assert.AreEqual("V0", varRefID, "getCurrentCompartment method returns unexpected value.");

        }
    }
}
