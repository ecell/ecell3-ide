using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;
using libsbml;

namespace Ecell.SBML
{
    internal class SBML_Rule
    {
        private SBML_Model Model;
        public int RuleNumber;
        public int VariableNumber;
        public int ParameterNumber;
        public List<VariableReferenceStruct> VariableReferenceList;

        public SBML_Rule(SBML_Model aModel)
        {
            this.Model = aModel;
            this.RuleNumber = 0;
            this.RuleNumber++;
            this.VariableNumber = 0;
            this.ParameterNumber = 0;
            this.VariableReferenceList = new List<VariableReferenceStruct>();
        }

        public string getRuleID()
        {
            return "Process:/SBMLRule:Rule" + this.RuleNumber.ToString();
        }
        
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

        public string[] setSpeciesToVariableReference(string aName, int aStoichiometry)
        {
            foreach(SpeciesStruct aSpecies in this.Model.SpeciesList)
            {
                if ( ( this.Model.Level == 1 && aSpecies.Name == aName ) ||
                     ( this.Model.Level == 2 && aSpecies.ID == aName ) )
                {
                    string compartmentName;
                    foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                    {
                        if ( aVariableReference.Variable.Split(':')[2] == aName)
                            if (aStoichiometry != 0)
                                aVariableReference.Coefficient = aStoichiometry;

                            compartmentName = this.setCompartmentToVariableReference( aSpecies.Compartment, 0);
                            return new string[] { aVariableReference.Name, compartmentName };
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

                    return new string[] { variableName, compartmentName };
                }
            }
            throw new EcellException("Error set species to VariableReference");
        }

        public string setParameterToVariableReference(string aName, int aStoichiometry)
        {
            foreach(ParameterStruct aParameter in this.Model.ParameterList)
            {
                if ( ( this.Model.Level == 1 && aParameter.Name == aName ) ||
                     ( this.Model.Level == 2 && aParameter.ID == aName ) )
                {
                    foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                    {
                        if ( aVariableReference.Variable.Split(':')[2] == aName)
                            if (aStoichiometry != 0)
                                aVariableReference.Coefficient = aStoichiometry;
                            return aVariableReference.Name;
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
                        string[] temp = this.setSpeciesToVariableReference( aName , 0);
                        string variableName = temp[0];
                        string compartmentName = temp[1];
                        if( variableName != "" )
                        {
                            anASTNode.setType(libsbml.libsbml.AST_DIVIDE);
                            anASTNode.addChild(new ASTNode(libsbml.libsbml.AST_NAME));
                            anASTNode.addChild(new ASTNode(libsbml.libsbml.AST_NAME));
                            anASTNode.getLeftChild().setName( variableName + ".Value" );
                            anASTNode.getRightChild().setName( compartmentName + ".Value" );
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

        public string convertRuleFormula(string aFormula)
        {
            ASTNode aASTRootNode = libsbml.libsbml.parseFormula( aFormula );
            ASTNode convertedAST = this.convertVariableName( aASTRootNode );
            string convertedFormula = libsbml.libsbml.formulaToString( convertedAST );
            return convertedFormula;

        }
    }
}
