using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// Compartment Class
    /// </summary>
    internal class SBML_Compartment
    {
        public SBML_Model Model;

        public SBML_Compartment( SBML_Model aModel)
        {
            this.Model = aModel;
        }

        public void initialize(CompartmentStruct aCompartment)
        {
            setSizeToDictionary(aCompartment);
            setUnitToDictionary(aCompartment);
        }

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
            if( this.Model.Level == 1 )
            {
                if (!aCompartment.Volume.Equals( double.NaN))
                    this.Model.CompartmentSize[ aCompartment.Name ] = (float)aCompartment.Volume;
                else
                    this.Model.CompartmentSize[ aCompartment.Name ] = this.getOutsideSize( aCompartment.Outside );
            }       
            else if( this.Model.Level == 2 )
            {
                if (!aCompartment.Size.Equals(double.NaN))
                    this.Model.CompartmentSize[ aCompartment.ID ] = (float)aCompartment.Size;
                else
                    this.Model.CompartmentSize[ aCompartment.ID ] = this.getOutsideSize( aCompartment.Outside );
            }
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

        private float getOutsideSize(string anOutsideCompartment)
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

        public float getCompartmentSize(CompartmentStruct aCompartment)
        {
            if (this.Model.Level == 1)
                return this.Model.CompartmentSize[aCompartment.Name];
            else if (this.Model.Level == 2)
                return this.Model.CompartmentSize[aCompartment.ID];
            else
                throw new EcellException();
        }

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
