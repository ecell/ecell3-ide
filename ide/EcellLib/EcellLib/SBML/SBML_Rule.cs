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
using libsbml;

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    public class SBML_Rule
    {
        /// <summary>
        /// 
        /// </summary>
        private SBML_Model Model;
        /// <summary>
        /// 
        /// </summary>
        public int RuleNumber;
        /// <summary>
        /// 
        /// </summary>
        public int VariableNumber;
        /// <summary>
        /// 
        /// </summary>
        public int ParameterNumber;
        /// <summary>
        /// 
        /// </summary>
        public List<VariableReferenceStruct> VariableReferenceList;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aModel"></param>
        public SBML_Rule(SBML_Model aModel)
        {
            this.Model = aModel;
            this.RuleNumber = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void initialize()
        {
            this.RuleNumber++;
            this.VariableNumber = 0;
            this.ParameterNumber = 0;
            this.VariableReferenceList = new List<VariableReferenceStruct>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getRuleID()
        {
            return "/SBMLRule:Rule" + this.RuleNumber.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <returns></returns>
        public int getVariableType( string aName )
        {
            foreach(SpeciesStruct aSpecies in this.Model.SpeciesList)
                if ( ( this.Model.Level == 1 && aSpecies.Name == aName ) ||
                     ( this.Model.Level == 2 && aSpecies.ID == aName ) )
                    return libsbml.libsbml.SBML_SPECIES;

            foreach(ParameterStruct aParameter in this.Model.ParameterList)
                if ( ( this.Model.Level == 1 && aParameter.Name == aName ) ||
                     ( this.Model.Level == 2 && aParameter.ID == aName ) )
                    return libsbml.libsbml.SBML_PARAMETER;

            foreach(CompartmentStruct aCompartment in this.Model.CompartmentList)
                if ( ( this.Model.Level == 1 && aCompartment.Name == aName ) ||
                     ( this.Model.Level == 2 && aCompartment.ID == aName ) )
                    return libsbml.libsbml.SBML_COMPARTMENT;

            throw new EcellException("Variable type must be Species, Parameter, or Compartment");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="aStoichiometry"></param>
        /// <returns></returns>
        public string setSpeciesToVariableReference(string aName, int aStoichiometry)
        {
            foreach(SpeciesStruct aSpecies in this.Model.SpeciesList)
            {
                if ( ( this.Model.Level == 1 && aSpecies.Name == aName ) ||
                     ( this.Model.Level == 2 && aSpecies.ID == aName ) )
                {
                    string compartmentName;
                    foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                    {
                        if (aVariableReference.Variable.Split(':')[2] == aName)
                        {
                            if (aStoichiometry != 0)
                                aVariableReference.Coefficient = aStoichiometry;

                            compartmentName = this.setCompartmentToVariableReference(aSpecies.Compartment, 0);
                            return aVariableReference.Name;
                        }
                    }
                    List<string> aVariableList = new List<string>();

                    string variableName = "V" + this.VariableNumber.ToString();
                    string aVariableID = this.Model.getSpeciesReferenceID( aName );

                    VariableReferenceStruct varRef = new VariableReferenceStruct(
                        variableName,
                        "Variable:" + aVariableID,
                        aStoichiometry );
                    
                    this.VariableReferenceList.Add( varRef );
                    this.VariableNumber++;
                    
                    compartmentName = this.setCompartmentToVariableReference(aSpecies.Compartment, 0);

                    return variableName;
                }
            }
            throw new EcellException("Error set species to VariableReference");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="aStoichiometry"></param>
        /// <returns></returns>
        public string setParameterToVariableReference(string aName, int aStoichiometry)
        {
            foreach(ParameterStruct aParameter in this.Model.ParameterList)
            {
                if ( ( this.Model.Level == 1 && aParameter.Name == aName ) ||
                     ( this.Model.Level == 2 && aParameter.ID == aName ) )
                {
                    foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                    {
                        if (aVariableReference.Variable.Split(':')[2] == aName)
                        {
                            if (aStoichiometry != 0)
                                aVariableReference.Coefficient = aStoichiometry;
                            return aVariableReference.Name;
                        }
                    }
                        
                    string variableName = "P" + this.ParameterNumber.ToString();
                    this.ParameterNumber++;
                    VariableReferenceStruct varRef = new VariableReferenceStruct(
                        variableName,
                        "Variable:/SBMLParameter:" + aName,
                        aStoichiometry );
                    this.VariableReferenceList.Add( varRef );

                    return variableName;
                }
            }
            throw new EcellException("Error set parameter to VariableReference");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="aStoichiometry"></param>
        /// <returns></returns>
        public string setCompartmentToVariableReference(string aName, int aStoichiometry)
        {
            foreach(CompartmentStruct aCompartment in this.Model.CompartmentList)
            {
                if ( ( this.Model.Level == 1 && aCompartment.Name == aName ) ||
                     ( this.Model.Level == 2 && aCompartment.ID == aName ) )
                {
                    foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                    {
                        if( ( aVariableReference.Variable.Split(':')[1] == this.Model.getPath( aName ) )
                         && ( aVariableReference.Variable.Split(':')[2] == "SIZE" ) )
                        {
                            if (aStoichiometry != 0)
                                aVariableReference.Coefficient = aStoichiometry;

                            return aVariableReference.Name;
                        }
                    }
                    VariableReferenceStruct varRef = new VariableReferenceStruct(
                        aName,
                        "Variable:" + this.Model.getPath( aName ) + ":SIZE",
                        aStoichiometry );

                    this.VariableReferenceList.Add( varRef );
                    
                    return aName;
                }
            }
            throw new EcellException("Error set compartment to VariableReference");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anASTNode"></param>
        /// <returns></returns>
        private ASTNode convertVariableName(ASTNode anASTNode)
        {
            long aNumChildren = anASTNode.getNumChildren();

            if ( aNumChildren == 2 )
            {
                this.convertVariableName( anASTNode.getLeftChild() );
                this.convertVariableName( anASTNode.getRightChild() );
            }
            else if ( aNumChildren == 1 )
            {
                this.convertVariableName( anASTNode.getLeftChild() );
            }
            else if ( aNumChildren == 0 )
            {
                if ( anASTNode.isNumber() )
                {
                }
                else
                {
                    string aName = anASTNode.getName();
                    int aType = this.getVariableType( aName );

                    //# Species
                    if ( aType == libsbml.libsbml.SBML_SPECIES )
                    {
                        string variableName = this.setSpeciesToVariableReference( aName , 0);
                        if( variableName != "" )
                        {
                            anASTNode.setName( variableName + ".Value" );
                            return anASTNode;
                        }
                    }
                    //# Parameter
                    else if ( aType == libsbml.libsbml.SBML_PARAMETER )
                    {
                        string variableName = this.setParameterToVariableReference( aName , 0);
                        if( variableName != "" )
                        {
                            anASTNode.setName( variableName + ".Value" );
                            return anASTNode;
                        }
                    }
                    //# Compartment
                    else if (aType == libsbml.libsbml.SBML_COMPARTMENT)
                    {
                        string variableName = this.setCompartmentToVariableReference(aName, 0);
                        if( variableName != "" )
                        {
                            anASTNode.setName( variableName + ".Value" );
                            return anASTNode;
                        }
                    }
                }
            }
            return anASTNode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFormula"></param>
        /// <returns></returns>
        public string convertRuleFormula(string aFormula)
        {
            ASTNode aASTRootNode = libsbml.libsbml.parseFormula( aFormula );
            ASTNode convertedAST = this.convertVariableName( aASTRootNode );
            string convertedFormula = libsbml.libsbml.formulaToString( convertedAST );
            return convertedFormula;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<object> GetVariableReferenceList()
        {
            List<object> list = new List<object>();
            foreach (VariableReferenceStruct varref in this.VariableReferenceList)
            {
                List<object> vr = new List<object>();
                vr.Add(varref.Name);
                vr.Add(varref.Variable);
                vr.Add(varref.Coefficient);
                list.Add(vr);
            }
            return list;
        }
    }
}
