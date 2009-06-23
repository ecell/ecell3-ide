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


            // getSpeciesReferenceID
            SpeciesStruct ss = new SpeciesStruct();
            ss.Compartment = "cell";
            ss.ID = "Test1";
            ss.Name = "Test1";

            string id;
            try
            {
                id = model.getSpeciesReferenceID(ss.ID);
            }
            catch (Exception)
            {
            }
            model.SpeciesList.Add(ss);
            id = model.getSpeciesReferenceID(ss.ID);
            Assert.AreEqual("/cell:Test1", id, "getEventID returns unexpected value.");

            try
            {
                model.Level = 1;
                id = model.getSpeciesReferenceID(ss.ID);
            }
            catch (Exception)
            {
            }

            try
            {
                model.Level = 0;
                id = model.getSpeciesReferenceID(ss.ID);
            }
            catch (Exception)
            {
            }

            // setFunctionDefinitionToDictionary
            FunctionDefinitionStruct ud = new FunctionDefinitionStruct();
            ud.ID = "Function";
            ud.Name = "Function";
            ud.Formula = "1 * 2";
            model.FunctionDefinitionList.Add(ud);

            Type type = model.GetType();
            MethodInfo info1 = type.GetMethod("setFunctionDefinitionToDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
            info1.Invoke(model, new object[] { });

            // getNewUnitValue
            MethodInfo info2 = type.GetMethod("getNewUnitValue", BindingFlags.NonPublic | BindingFlags.Instance);
            UnitStruct unit = new UnitStruct("Kind", 2, 1, 0.5, 3);
            double value = (double)info2.Invoke(model, new object[] { unit });
            Assert.AreEqual(28.0d, value, "getNewUnitValue returns unexpected value.");

            // convertUnit
            model.Level = 2;
            value = model.convertUnit("test", 0.1d);
            Assert.AreEqual(0.1d, value, "convertUnit returns unexpected value.");

            value = model.convertUnit("substance", 1.0d);
            Assert.AreEqual(1.0d, value, "convertUnit returns unexpected value.");

            model.Level = 1;
            value = model.convertUnit("test", 0.1d);
            Assert.AreEqual(0.1d, value, "convertUnit returns unexpected value.");

            value = model.convertUnit("minute", 1.0d);
            Assert.AreEqual(60.0d, value, "convertUnit returns unexpected value.");

            try
            {
                model.Level = 0;
                model.convertUnit("test", 0.1d);
            }
            catch (Exception)
            {
            }

            // getPath
            string path;
            try
            {
                path = model.getPath("");
            }
            catch (Exception)
            {
            }
            path = model.getPath("default");
            Assert.AreEqual("/", path, "getPath returns unexpected value.");

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

            CompartmentStruct cs = new CompartmentStruct();
            try
            {
                c.getCompartmentID(cs);
            }
            catch (Exception)
            {
            }
            string id;

            cs.Outside = "";
            cs.ID = "Test1";
            id = c.getCompartmentID(cs);
            Assert.AreEqual("System:/:Test1", id, "getCompartmentID returns unexpected value.");

            try
            {
                model.Level = 1;
                id = c.getCompartmentID(cs);
            }
            catch (Exception)
            {
            }

            try
            {
                model.Level = 0;
                id = c.getCompartmentID(cs);
            }
            catch (Exception)
            {
            }

            model.Level = 2;
            cs.Outside = "cell";
            id = c.getCompartmentID(cs);
            Assert.AreEqual("System:/cell:Test1", id, "getCompartmentID returns unexpected value.");

            try
            {
                model.Level = 1;
                id = c.getCompartmentID(cs);
            }
            catch (Exception)
            {
            }

            try
            {
                model.Level = 0;
                id = c.getCompartmentID(cs);
            }
            catch (Exception)
            {
            }

            // GetModuleType
            Type type = c.GetType();
            MethodInfo info1 = type.GetMethod("setSizeToDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo info2 = type.GetMethod("setUnitToDictionary", BindingFlags.NonPublic | BindingFlags.Instance);
            try
            {
                info1.Invoke(c, new object[] { cs });
            }
            catch (Exception)
            {
            }
            try
            {
                info2.Invoke(c, new object[] { cs });
            }
            catch (Exception)
            {
            }
            model.Level = 1;
            cs.Volume = double.NaN;
            cs.Name = "Test1";
            info1.Invoke(c, new object[] { cs });
            info2.Invoke(c, new object[] { cs });

            // getOutsideSize
            model.Level = 2;
            MethodInfo info3 = type.GetMethod("getOutsideSize", BindingFlags.NonPublic | BindingFlags.Instance);
            double size;
            size = (double)info3.Invoke(c, new object[] { "" });
            Assert.AreEqual(1.0d, size, "getOutsideSize returns unexpected value.");

            size = (double)info3.Invoke(c, new object[] { "cell" });
            Assert.AreEqual(1.0d, size, "getOutsideSize returns unexpected value.");

            // getOutsideUnit
            MethodInfo info4 = type.GetMethod("getOutsideUnit", BindingFlags.NonPublic | BindingFlags.Instance);
            string unit;
            unit = (string)info4.Invoke(c, new object[] { "" });
            Assert.AreEqual("", unit, "getCompartmentSize returns unexpected value.");

            unit = (string)info4.Invoke(c, new object[] { "cell" });
            Assert.AreEqual("", unit, "getCompartmentSize returns unexpected value.");

            // getCompartmentSize
            size = c.getCompartmentSize(cs);
            Assert.AreEqual(1.0d, size, "getCompartmentSize returns unexpected value.");

            model.Level = 1;
            size = c.getCompartmentSize(cs);
            Assert.AreEqual(1.0d, size, "getCompartmentSize returns unexpected value.");

            try
            {
                model.Level = 0;
                size = c.getCompartmentSize(cs);
            }
            catch (Exception)
            {
            }

            // getCompartmentUnit
            model.Level = 2;
            id = c.getCompartmentUnit(cs);
            Assert.AreEqual(null, id, "getCompartmentUnit returns unexpected value.");

            model.Level = 1;
            id = c.getCompartmentUnit(cs);
            Assert.AreEqual(null, id, "getCompartmentUnit returns unexpected value.");

            try
            {
                model.Level = 0;
                id = c.getCompartmentUnit(cs);
            }
            catch (Exception)
            {
            }
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

            ParameterStruct ps = new ParameterStruct();

            // getParameterID
            ps.ID = "Test1";
            string id = p.getParameterID(ps);
            Assert.AreEqual("/SBMLParameter:Test1", id, "getParameterID returns unexpected value.");
            try
            {
                ps.ID = "";
                id = p.getParameterID(ps);
            }
            catch (Exception)
            {
            }
            model.Level = 1;
            ps.Name = "Test2";
            id = p.getParameterID(ps);
            Assert.AreEqual("/SBMLParameter:Test2", id, "getParameterID returns unexpected value.");
            try
            {
                ps.Name = "";
                id = p.getParameterID(ps);
            }
            catch (Exception)
            {
            }
            try
            {
                model.Level = 0;
                id = p.getParameterID(ps);
            }
            catch (Exception)
            {
            }

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
            SBMLDocument document = reader.readSBML(TestConstant.SBML_BIOMD0000000003);
            Model sbmlModel = document.getModel();
            SBML_Model model = new SBML_Model(sbmlModel);
            SBML_Compartment sc = new SBML_Compartment(model);
            SBML_Species species = new SBML_Species(model);
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
