//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//       This file is part of the E-Cell System
//
//       Copyright (C) 1996-2009 Keio University
//       Copyright (C) 2005-2008 The Molecular Sciences Institute
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell System is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
// 
// E-Cell System is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public
// License along with E-Cell System -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
// 
// END_HEADER
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
// Created    :2009/01/14
// Last Update:2009/02/02
//

using System;
using System.Collections.Generic;
using System.Text;
using libsbml;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    public class SBML2EML
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public static void Convert(string filename)
        {
            SBMLDocument document = libsbml.libsbml.readSBMLFromString(filename);
            Model model = document.getModel();

            SBML_Model theModel = new SBML_Model(document, model);
            SBML_Compartment theCompartment = new SBML_Compartment(theModel);
            SBML_Parameter theParameter = new SBML_Parameter( theModel );
            SBML_Species theSpecies = new SBML_Species( theModel );
            SBML_Rule theRule = new SBML_Rule( theModel );
            SBML_Reaction theReaction = new SBML_Reaction(theModel);
            // SBML_Event theEvent = new SBML_Event(theModel);

            document.Dispose();

            //Eml eml = new Eml(null);

        }

        private static Model SBML_Model(SBMLDocument document, Model model)
        {
            throw new EcellException("Not implemented");
        }
    }
}
