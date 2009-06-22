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
// Last Update:2009/06/24
//

using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    public class SBML_Species
    {
        /// <summary>
        /// 
        /// </summary>
        private SBML_Model Model;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aModel"></param>
        public SBML_Species(SBML_Model aModel)
        {
            this.Model = aModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpecies"></param>
        /// <returns></returns>
        public string getSpeciesID(SpeciesStruct aSpecies)
        {
            string aCompartmentID = aSpecies.Compartment;

            if ( aCompartmentID == "" )
                throw new EcellException("compartment property of Species must be defined");

            string aSpeciesID;
            if ( this.Model.Level == 1 )
                aSpeciesID = this.Model.getPath( aCompartmentID ) + ':' + aSpecies.Name;
            else if ( this.Model.Level == 2 )
                aSpeciesID = this.Model.getPath( aCompartmentID ) + ':' + aSpecies.ID;
            else
                throw new EcellException("Version"+ this.Model.Level + " ????");
                    
            return aSpeciesID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpecies"></param>
        /// <returns></returns>
        public double getSpeciesValue(SpeciesStruct aSpecies)
        {
            if ( !aSpecies.InitialAmount.Equals(double.NaN))
            {
                return aSpecies.InitialAmount;
            }
            else if (this.Model.Level == 2 && !aSpecies.InitialConcentration.Equals(double.NaN))
            {
                double aSize = this.Model.CompartmentSize[ aSpecies.Compartment ];
                return aSpecies.InitialConcentration * aSize;
            }
            else
                throw new EcellException(string.Format( "InitialAmount or InitialConcentration of Species {0} must be defined.", aSpecies.ID ));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpecies"></param>
        /// <returns></returns>
        public int getConstant(SpeciesStruct aSpecies)
        {
            if (this.Model.Level == 1)
            {
                if (aSpecies.BoundaryCondition)
                    return 1;
                else
                    return 0;
            }
            else if (this.Model.Level == 2)
            {
                if (aSpecies.Constant)
                    return 1;
                else
                    return 0;
            }
            else
                throw new EcellException("Version" + this.Model.Level + " ????");
        }
    }
}
