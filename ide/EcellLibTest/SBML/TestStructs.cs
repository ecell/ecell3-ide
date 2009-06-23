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

namespace Ecell.SBML
{

    /// <summary>
    /// TestTemplate
    /// </summary>
    [TestFixture()]
    public class TestStructs
    {
        /// <summary>
        /// TestCompartmentStruct
        /// </summary>
        [Test()]
        public void TestConstructorOfStructs()
        {
            //
            CompartmentStruct c = new CompartmentStruct(
                "ID",
                "Name",
                0,
                0.0,
                0.0,
                "Unit",
                "Parent",
                false);
            //
            EventStruct e = new EventStruct(
                "ID",
                "Name",
                "Trigger",
                "delay",
                "TimeUnit",
                new List<EventAssignmentStruct>());
            //
            EventAssignmentStruct ea = new EventAssignmentStruct(
                "Variable",
                "Formula");
            //
            FunctionDefinitionStruct fd = new FunctionDefinitionStruct(
                "ID",
                "Name",
                "Formula");
            //
            ParameterStruct p = new ParameterStruct(
                "ID",
                "Name",
                0.0,
                "Unit",
                false);
            //
            ReactionStruct r = new ReactionStruct(
                "ID",
                "Name",
                new List<KineticLawStruct>(),
                false,
                false,
                new List<ReactantStruct>(),
                new List<ProductStruct>(),
                new List<string>());
            //
            KineticLawStruct k = new KineticLawStruct(
                "Formula",
                new List<string>(),
                "TimeUnit",
                "Substance",
                new List<ParameterStruct>(),
                null);
            //
            ReactantStruct rs = new ReactantStruct(
                "Species",
                0,
                "Formula",
                0);
            //
            ProductStruct ps = new ProductStruct(
                "Species",
                0.0,
                "Formula",
                0);
            //
            RuleStruct rule = new RuleStruct(
                0,
                "Formula",
                "Variable");
            //
            SpeciesStruct s = new SpeciesStruct(
                "ID",
                "Name",
                "Parent",
                0.0,
                0.0,
                "Substance",
                "Spatial",
                "Unit",
                false,
                false,
                0,
                false);
            //
            UnitDefinitionStruct ud = new UnitDefinitionStruct(
                "ID",
                "Name",
                new List<UnitStruct>());
            //
            UnitStruct u = new UnitStruct(
                "Kind",
                0,
                0,
                0.0,
                0.0);
            //
            VariableReferenceStruct v = new VariableReferenceStruct(
                "Name",
                "Variable",
                0);
            //
            InitialAssignmentStruct i = new InitialAssignmentStruct(
                "Name",
                0.0);


        }

        /// <summary>
        /// TestSBML_Model
        /// </summary>
        [Test()]
        public void TestSBML_Model()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_Oscillation);

            SBML_Model model = new SBML_Model(document.getModel());

        }
        /// <summary>
        /// TestSBML_Event
        /// </summary>
        [Test()]
        public void TestSBML_Event()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_Oscillation);

            SBML_Model model = new SBML_Model(document.getModel());
            SBML_Event e = new SBML_Event(model);

            EventStruct es = new EventStruct();
            es.ID = "Test1";
            string id = e.getEventID(es);
            Assert.AreEqual("Process:/:Test1", id, "getEventID returns unexpected value.");

            model.Level = 1;
            es.Name = "Test2";
            id = e.getEventID(es);
            Assert.AreEqual("Process:/:Test2", id, "getEventID returns unexpected value.");

            model.Level = 0;
            id = e.getEventID(es);
            Assert.AreEqual("Process:/:Event0", id, "getEventID returns unexpected value.");
        }
        /// <summary>
        /// TestSBML_Compartment
        /// </summary>
        [Test()]
        public void TestSBML_Compartment()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_Oscillation);

            SBML_Model model = new SBML_Model(document.getModel());
            SBML_Compartment c = new SBML_Compartment(model);

        }
        /// <summary>
        /// TestSBML_Parameter
        /// </summary>
        [Test()]
        public void TestSBML_Parameter()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_Oscillation);

            SBML_Model model = new SBML_Model(document.getModel());
            SBML_Parameter p = new SBML_Parameter(model);
        }
        /// <summary>
        /// TestSBML_Reaction
        /// </summary>
        [Test()]
        public void TestSBML_Reaction()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_Oscillation);

            SBML_Model model = new SBML_Model(document.getModel());
            SBML_Reaction reaction = new SBML_Reaction(model);
        }
        /// <summary>
        /// TestSBML_Rule
        /// </summary>
        [Test()]
        public void TestSBML_Rule()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_Oscillation);

            SBML_Model model = new SBML_Model(document.getModel());
            SBML_Rule rule = new SBML_Rule(model);
        }
        /// <summary>
        /// TestSBML_Rule
        /// </summary>
        [Test()]
        public void TestSBML_Species()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_Oscillation);

            SBML_Model model = new SBML_Model(document.getModel());
            SBML_Compartment sc = new SBML_Compartment(model);
            SBML_Species species = new SBML_Species(model);

            // getConstant
            SpeciesStruct ss = new SpeciesStruct();
            int i = species.getConstant(ss);
            Assert.AreEqual(0, i, "getConstant returns unexpected value.");

            ss.Constant = true;
            i = species.getConstant(ss);
            Assert.AreEqual(1, i, "getConstant returns unexpected value.");

            model.Level = 1;
            i = species.getConstant(ss);
            Assert.AreEqual(0, i, "getConstant returns unexpected value.");

            ss.BoundaryCondition = true;
            i = species.getConstant(ss);
            Assert.AreEqual(1, i, "getConstant returns unexpected value.");

            try
            {
                model.Level = 0;
                i = species.getConstant(ss);
            }
            catch (Exception)
            {
            }

            // getSpeciesValue
            double d = species.getSpeciesValue(ss);
            Assert.AreEqual(0.0, d, "getSpeciesValue returns unexpected value.");

            model.Level = 2;
            ss.InitialAmount = double.NaN;
            ss.Compartment = "cell";
            d = species.getSpeciesValue(ss);
            Assert.AreEqual(0.0, d, "getSpeciesValue returns unexpected value.");

            try
            {
                ss.InitialConcentration = double.NaN;
                d = species.getSpeciesValue(ss);
            }
            catch (Exception)
            {
            }

            // getSpeciesID
            ss.ID = "Test1";
            string id = species.getSpeciesID(ss);
            Assert.AreEqual("/cell:Test1", id, "getSpeciesID returns unexpected value.");

            try
            {
                model.Level = 1;
                ss.Name = "Test2";
                id = species.getSpeciesID(ss);
            }
            catch (Exception)
            {
            }

            try
            {
                model.Level = 0;
                id = species.getSpeciesID(ss);
            }
            catch (Exception)
            {
            }

            try
            {
                model.Level = 2;
                ss.Compartment = "";
                id = species.getSpeciesID(ss);
            }
            catch (Exception)
            {
            }

        }
    }
}
