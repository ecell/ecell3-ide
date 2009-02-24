using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    internal class SBML_Parameter
    {
        private SBML_Model Model;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aModel"></param>
        public SBML_Parameter(SBML_Model aModel)
        {
            this.Model = aModel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aParameter"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aParameter"></param>
        /// <returns></returns>
        public double getParameterValue(ParameterStruct aParameter )
        {
            return aParameter.Value;
        }
    }
}
