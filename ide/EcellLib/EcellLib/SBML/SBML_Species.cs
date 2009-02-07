using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    internal class SBML_Species
    {
        private SBML_Model Model;

        public SBML_Species(SBML_Model aModel)
        {
            this.Model = aModel;
        }

        public string getSpeciesID(SpeciesStruct aSpecies)
        {
            string aCompartmentID = aSpecies.Compartment;

            if ( aCompartmentID == "" )
                throw new EcellException("compartment property of Species must be defined");

            string aSystemID;
            if ( this.Model.Level == 1 )
                aSystemID = this.Model.getPath( aCompartmentID ) + ':' + aSpecies.Name;
            else if ( this.Model.Level == 2 )
                aSystemID = this.Model.getPath( aCompartmentID ) + ':' + aSpecies.ID;
            else
                throw new EcellException("Version"+ this.Model.Level + " ????");
                    
            return "Variable:" + aSystemID;
        }

        public double getSpeciesValue(SpeciesStruct aSpecies)
        {
            if ( aSpecies.InitialAmount != double.NaN)
            {
                return (float) aSpecies.InitialAmount;
            }
            else if (this.Model.Level == 2 && aSpecies.InitialConcentration != double.NaN)
            {
                float aSize = this.Model.CompartmentSize[ aSpecies.Compartment ];
                return aSpecies.InitialConcentration * aSize;
            }
            else
                throw new EcellException(string.Format( "InitialAmount or InitialConcentration of Species {0} must be defined.", aSpecies.ID ));

        }

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
