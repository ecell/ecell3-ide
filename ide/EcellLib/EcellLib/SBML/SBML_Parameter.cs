using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    internal class SBML_Parameter
    {
        private SBML_Model Model;

        public SBML_Parameter(SBML_Model aModel)
        {
            this.Model = aModel;
        }

        public string getParameterID(ParameterStruct aParameter )
        {
            if ( this.Model.Level == 1 )
            {
               if ( aParameter.Name != "" )
                   return "/SBMLParameter:" + aParameter.Name;
               else
                   throw new EcellException( "Parameter must set the Parameter Name");
            }
            else if ( this.Model.Level == 2 )
            {
               if ( aParameter.ID != "" )
                   return "/SBMLParameter:" + aParameter.ID;
               else
                   throw new EcellException( "Parameter must set the Parameter ID");
            }
            else
            {
               throw new EcellException("Version" + this.Model.Level + " ????");
            }
        }

        public double getParameterValue(ParameterStruct aParameter )
        {
            return aParameter.Value;
        }
    }
}
