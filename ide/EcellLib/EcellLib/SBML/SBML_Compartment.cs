//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// Compartment Class
    /// </summary>
    public class SBML_Compartment
    {
        /// <summary>
        /// SBML Model object.
        /// </summary>
        public SBML_Model Model;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aModel">the model object.</param>
        public SBML_Compartment( SBML_Model aModel)
        {
            this.Model = aModel;
            foreach (CompartmentStruct aCompartment in aModel.CompartmentList)
            {
                setSizeToDictionary(aCompartment);
                setUnitToDictionary(aCompartment);
            }

        }

        /// <summary>
        /// Get the compartment ID.
        /// </summary>
        /// <param name="aCompartment">the compartment object.</param>
        /// <returns>the comaprtment ID.</returns>
        public string getCompartmentID(CompartmentStruct aCompartment)
        {
            string aSystemID;
            if ( aCompartment.Outside == "" )
            {
                if ( this.Model.Level == 1 )
                    aSystemID = "/:" + aCompartment.Name;
                else if ( this.Model.Level == 2 )
                    aSystemID = "/:" + aCompartment.ID;
                else
                    throw new EcellException("Compartment Class needs a ['ID']");
            }
            else
            {
                if( this.Model.Level == 1 )
                    aSystemID = this.Model.getPath(aCompartment.Outside) + ":" + aCompartment.Name;
                else if (this.Model.Level == 2)
                    aSystemID = this.Model.getPath(aCompartment.Outside) + ":" + aCompartment.ID;
                else
                    throw new EcellException("Compartment Class needs a ['ID']");
            }
            return "System:" + aSystemID;
        }

        private void setSizeToDictionary(CompartmentStruct aCompartment)
        {
            string aCompartmentID;
            if (this.Model.Level == 1)
                aCompartmentID = aCompartment.Name;
            else if (this.Model.Level == 2)
                aCompartmentID = aCompartment.ID;
            else
                throw new EcellException();

            if (!aCompartment.Volume.Equals(double.NaN))
                this.Model.CompartmentSize[aCompartmentID] = aCompartment.Volume;
            else
                this.Model.CompartmentSize[aCompartmentID] = this.getOutsideSize(aCompartment.Outside);

        }

        private void setUnitToDictionary(CompartmentStruct aCompartment)
        {
            string aCompartmentID;
            if( this.Model.Level == 1 )
                aCompartmentID = aCompartment.Name;
            else if( this.Model.Level == 2 )
                aCompartmentID = aCompartment.ID;
            else
                throw new EcellException();

            if( aCompartment.Unit != "" )
                this.Model.CompartmentUnit[ aCompartmentID ] = aCompartment.Unit;
            else
                this.Model.CompartmentUnit[ aCompartmentID ] = this.getOutsideUnit( aCompartment.Outside );

        }

        private double getOutsideSize(string anOutsideCompartment)
        {
            if ( anOutsideCompartment == "" )
                return 1f;
            else
                return this.Model.CompartmentSize[ anOutsideCompartment ];
        }

        private string getOutsideUnit(string anOutsideCompartment)
        {
            if ( anOutsideCompartment == "" )
                return "";
            else
                return this.Model.CompartmentUnit[ anOutsideCompartment ];
        }

        /// <summary>
        /// Get the compartment size.
        /// </summary>
        /// <param name="aCompartment">the compartment object.</param>
        /// <returns>the compartment size.</returns>
        public double getCompartmentSize(CompartmentStruct aCompartment)
        {
            if (this.Model.Level == 1)
                return this.Model.CompartmentSize[aCompartment.Name];
            else if (this.Model.Level == 2)
                return this.Model.CompartmentSize[aCompartment.ID];
            else
                throw new EcellException();
        }

        /// <summary>
        /// Get the compartment unit.
        /// </summary>
        /// <param name="aCompartment">the compartment object.</param>
        /// <returns>the compartment unit.</returns>
        public string getCompartmentUnit(CompartmentStruct aCompartment)
        {
            if ( this.Model.Level == 1 )
                return this.Model.CompartmentUnit[ aCompartment.Name ];
            else if (this.Model.Level == 2)
                return this.Model.CompartmentUnit[ aCompartment.ID ];
            else
                throw new EcellException();
        }
    }
}
