//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//       This file is part of the E-Cell System
//
//       Copyright (C) 1996-2010 Keio University
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
// Last Update:2010/02/02
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
    public class SBML_Reaction
    {
        /// <summary>
        /// 
        /// </summary>
        private SBML_Model Model;
        /// <summary>
        /// 
        /// </summary>
        public int SubstrateNumber;
        /// <summary>
        /// 
        /// </summary>
        public int ProductNumber;
        /// <summary>
        /// 
        /// </summary>
        public int ModifierNumber;
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
        public SBML_Reaction(SBML_Model aModel)
        {
            this.Model = aModel;
        }
        /// <summary>
        /// 
        /// </summary>
        public void initialize()
        {
            this.SubstrateNumber = 0;
            this.ProductNumber = 0;
            this.ModifierNumber = 0;
            this.ParameterNumber = 0;

            this.VariableReferenceList = new List<VariableReferenceStruct>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aReaction"></param>
        /// <returns></returns>
        public string getReactionID(ReactionStruct aReaction)
        {
            if ( this.Model.Level == 1 )
            {
                if ( aReaction.Name != "" )
                    return "/:" + aReaction.Name;
                else
                    throw new EcellException("Reaction must set the Reaction name");
            }       
            else if ( this.Model.Level == 2 )
            {
                if ( aReaction.ID != "" )
                    return "/:" + aReaction.ID;
                else
                    throw new EcellException("Reaction must set the Reaction ID");
            }
            throw new EcellException("Reaction must set the Reaction name");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <returns></returns>
        public string setCompartmentToVariableReference(string aName)
        {
            foreach(CompartmentStruct aCompartment in this.Model.CompartmentList)
            {
                if ( aCompartment.ID == aName || aCompartment.Name == aName )
                {
                    foreach (VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                        if( aVariableReference.Variable.Split(':')[2] == "SIZE" )
                        {
                            string aCurrentPath = aVariableReference.Variable.Split(':')[1];
                            int aLastSlash = aCurrentPath.LastIndexOf( "/" );
                            return aVariableReference.Name;
                        } 
                    VariableReferenceStruct varRef = new VariableReferenceStruct(
                        aName,
                        "Variable:" + this.Model.getPath( aName ) + ":SIZE",
                        0);
                    this.VariableReferenceList.Add( varRef );

                    return varRef.Name;
                }
            }
            return "";
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

                return anASTNode;
            }
            else if ( aNumChildren == 1 )
            {
                this.convertVariableName( anASTNode.getLeftChild() );
                return anASTNode;
            }
            else if ( aNumChildren == 0 )
            {
                if ( anASTNode.isNumber() )
                {
                }
                else
                {
                    string aName = anASTNode.getName();
                    string variableName = "";

                    // Check Species
                    foreach(SpeciesStruct aSpecies in this.Model.SpeciesList)
                    {
                        if ( aSpecies.ID != aName && aSpecies.Name != aName)
                            continue;

                        // Check VariableReference
                        foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                            if (aVariableReference.Variable.Split(':')[2] == aName)
                                variableName =  aVariableReference.Name;
                        if (variableName == "")
                        {
                            string aModifierID = this.Model.getSpeciesReferenceID(aName);

                            VariableReferenceStruct varRef = new VariableReferenceStruct(
                                "C" + this.ModifierNumber.ToString(),
                                "Variable:" + aModifierID,
                                0);
                            this.VariableReferenceList.Add(varRef);

                            variableName = varRef.Name;
                            this.ModifierNumber++;
                        }
                        //string compartmentName = this.setCompartmentToVariableReference( aSpecies.Compartment );
                        //anASTNode.setType( libsbml.libsbml.AST_DIVIDE );
                        //anASTNode.addChild( new ASTNode( libsbml.libsbml.AST_NAME ) );
                        //anASTNode.addChild( new ASTNode( libsbml.libsbml.AST_NAME ) );
                        //anASTNode.getLeftChild().setName( variableName + ".Value" );  
                        //anASTNode.getRightChild().setName( compartmentName + ".Value" ) ;
                        anASTNode.setName(variableName + ".Value");
                        return anASTNode;
                    }

                    // Check Parameters.
                    foreach(ParameterStruct aParameter in this.Model.ParameterList)
                    {
                        if (aParameter.ID != aName && aParameter.Name != aName)
                            continue;
                        foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                            if (aVariableReference.Variable.Split(':')[2] == aName)
                                variableName = aVariableReference.Name;

                        if( variableName == "" )
                        {
                            VariableReferenceStruct varRef = new VariableReferenceStruct(
                                aName,
                                "Variable:/:" + aName,
                                0 );
                            this.VariableReferenceList.Add( varRef );

                            this.ParameterNumber++;
                            variableName = varRef.Name;
                        }

                        anASTNode.setName( variableName + ".Value" );
                        
                        return anASTNode;
                    }
    //                if variableName == '':
                    variableName = this.setCompartmentToVariableReference( aName );
                    if (variableName != "")
                        anASTNode.setName( variableName + ".Value" );
                }
            }
            return anASTNode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aFormula"></param>
        /// <returns></returns>
        public string convertKineticLawFormula(string aFormula)
        {
            ASTNode aASTRootNode = libsbml.libsbml.parseFormula( aFormula );
            ASTNode convertedAST = this.convertVariableName( aASTRootNode );
            return libsbml.libsbml.formulaToString( convertedAST );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpeciesID"></param>
        /// <param name="aStoichiometry"></param>
        /// <returns></returns>
        public int getStoichiometry(string aSpeciesID, double aStoichiometry )
        {
            if ( this.Model.Level == 1 )
            {
                foreach (SpeciesStruct aSpecies in this.Model.SpeciesList)
                {
                    if (aSpecies.Name == aSpeciesID)
                    {
                        if (aSpecies.BoundaryCondition)
                            return 0;
                        else
                            return (int)aStoichiometry;
                    }
                }
            }
            else if ( this.Model.Level == 2 )
            {
                foreach (SpeciesStruct aSpecies in this.Model.SpeciesList)
                {
                    if (aSpecies.ID == aSpeciesID)
                    {
                        if (aSpecies.Constant || aSpecies.BoundaryCondition)
                            return 0;
                        else
                            return (int)aStoichiometry;
                    }
                }
            }
            else
                throw new EcellException("Version"+ this.Model.Level.ToString() +" ????");
            throw new EcellException("Cannot get stoichiometry.");
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
