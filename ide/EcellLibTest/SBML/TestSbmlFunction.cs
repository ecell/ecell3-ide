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
    public class TestSbmlFunction
    {
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestAll()
        {
            SBMLReader reader = new SBMLReader();
            SBMLDocument document = reader.readSBML(TestConstant.SBML_BIOMD0000000003);
            Model sbmlModel = document.getModel();
            // Set FunctionDefinition
            ASTNode math = libsbml.libsbml.parseFormula("V0 * 5");
            FunctionDefinition fd = sbmlModel.createFunctionDefinition();
            fd.setId("FD");
            fd.setName("FD");
            fd.setMath(math);
            // Set Event
            Event ev = sbmlModel.createEvent();
            ev.setId("Event");
            ev.setName("Event");
            ev.setTrigger(new Trigger(2,3));
            EventAssignment ea = ev.createEventAssignment();
            ea.setId("Assignment");
            ea.setName("Assignment");
            // Set Initial Amount
            InitialAssignment ia = sbmlModel.createInitialAssignment();
            ia.setSymbol("M1");
            ia.setMath(math);
            // Set UnitDefinition
            UnitDefinition ud = sbmlModel.createUnitDefinition();
            ud.setId("UD");
            ud.setName("UD");
            Unit unit = ud.createUnit();
            unit.setId("Unit");
            unit.setName("Unit");
            //
            SBML_Model model = new SBML_Model(sbmlModel);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCompartmentSize()
        {
            Compartment c = new Compartment(2, 3);
            double value;
            value = SbmlFunctions.GetCompartmentSize(c);
            Assert.AreEqual(double.NaN, value, "GetCompartmentSize returns unexpected value.");

            c.setSize(2.0);
            value = SbmlFunctions.GetCompartmentSize(c);
            Assert.AreEqual(2.0, value, "GetCompartmentSize returns unexpected value.");

        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetCompartmentVolume()
        {
            Compartment c = new Compartment(2, 3);
            double value;
            value = SbmlFunctions.GetCompartmentVolume(c);
            Assert.AreEqual(double.NaN, value, "GetCompartmentVolume returns unexpected value.");

            c.setVolume(2.0);
            value = SbmlFunctions.GetCompartmentVolume(c);
            Assert.AreEqual(2.0, value, "GetCompartmentVolume returns unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetInitialConcentration()
        {
            Species s = new Species(2, 3);
            double value;
            value = SbmlFunctions.GetInitialConcentration(s);
            Assert.AreEqual(double.NaN, value, "GetInitialConcentration returns unexpected value.");

            s.setInitialConcentration(2.0);
            value = SbmlFunctions.GetInitialConcentration(s);
            Assert.AreEqual(2.0, value, "GetInitialConcentration returns unexpected value.");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestGetInitialAmount()
        {
            Species s = new Species(2, 3);
            double value;
            value = SbmlFunctions.GetInitialAmount(s);
            Assert.AreEqual(double.NaN, value, "GetInitialAmount returns unexpected value.");

            s.setInitialAmount(2.0);
            value = SbmlFunctions.GetInitialAmount(s);
            Assert.AreEqual(2.0, value, "GetInitialAmount returns unexpected value.");
        }
        
        /// <summary>
        /// 
        /// </summary>
        [Test()]
        public void TestStoichiometryMath()
        {
            SpeciesReference sr = new SpeciesReference(2,3);
            sr.setId("S0");
            sr.setStoichiometry(0);
            sr.setStoichiometryMath(new StoichiometryMath(2, 3));
            string math = SbmlFunctions.GetStoichiometryMath(sr);
            Assert.AreEqual(null, math, "GetStoichiometryMath returns unexpected value.");
        }
    }
}
