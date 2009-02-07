using System;
using System.Collections.Generic;
using System.Text;
using Ecell.Exceptions;
using libsbml;

namespace Ecell.SBML
{
    internal class SBML_Reaction
    {
        private SBML_Model Model;
        private int SubstrateNumber;
        private int ProductNumber;
        private int ModifierNumber;
        private int ParameterNumber;

        private List<VariableReferenceStruct> VariableReferenceList;

        public SBML_Reaction(SBML_Model aModel)
        {
            this.Model = aModel;
            this.SubstrateNumber = 0;
            this.ProductNumber = 0;
            this.ModifierNumber = 0;
            this.ParameterNumber = 0;

            this.VariableReferenceList = new List<VariableReferenceStruct>();
        }

        public string getReactionID(ReactionStruct aReaction)
        {
            if ( this.Model.Level == 1 )
            {
                if ( aReaction.Name != "" )
                    return "Process:/:" + aReaction.Name;
                else
                    throw new EcellException("Reaction must set the Reaction name");
            }       
            else if ( this.Model.Level == 2 )
            {
                if ( aReaction.ID != "" )
                    return "Process:/:" + aReaction.ID;
                else
                    throw new EcellException("Reaction must set the Reaction ID");
            }
            throw new EcellException("Reaction must set the Reaction name");
        }

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

                    foreach(SpeciesStruct aSpecies in this.Model.SpeciesList)
                    {
                        if ( aSpecies.ID != aName && aSpecies.Name != aName)
                            continue;

                        foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                            if (aVariableReference.Variable.Split(':')[2] == aName)
                                variableName =  aVariableReference.Name;

                        if( this.Model.Level == 2 && variableName == "" )
                            throw new EcellException( "in libSBML :" + aName + " isn't defined in VariableReferenceList");
                        else if (this.Model.Level == 1 && variableName == "")
                        {
                            string aModifierID = this.Model.getSpeciesReferenceID( aName );

                            VariableReferenceStruct varRef = new VariableReferenceStruct(
                                "C" + this.ModifierNumber.ToString(),
                                "Variable:" + aModifierID,
                                0);
                            this.VariableReferenceList.Add( varRef );

                            variableName = varRef.Name;
                            this.ModifierNumber++;
                        }
                        string compartmentName = this.setCompartmentToVariableReference( aSpecies.Compartment );

                        anASTNode.setType( libsbml.libsbml.AST_DIVIDE );
                        anASTNode.addChild( new ASTNode( libsbml.libsbml.AST_NAME ) );
                        anASTNode.addChild( new ASTNode( libsbml.libsbml.AST_NAME ) );
                        anASTNode.getLeftChild().setName( variableName + ".Value" );  
                        anASTNode.getRightChild().setName( compartmentName + ".Value" ) ;    
                        
                        return anASTNode;
                    }
    //                if variableName == '':
                    foreach(ParameterStruct aParameter in this.Model.ParameterList)
                    {
                        if ( aParameter.ID == aName || aParameter.Name == aName )
                        {
                            foreach(VariableReferenceStruct aVariableReference in this.VariableReferenceList)
                                if (aVariableReference.Variable.Split(':')[2] == aName)
                                    variableName = aVariableReference.Name;

                            if( variableName == "" )
                            {
                                VariableReferenceStruct varRef = new VariableReferenceStruct(
                                    "Param" + this.ParameterNumber.ToString(),
                                    "Variable:/SBMLParameter:" + aName,
                                    0 );
                                this.VariableReferenceList.Add( varRef );

                                this.ParameterNumber++;
                                variableName = varRef.Name;
                            }

                            anASTNode.setName( variableName + ".Value" );
                            
                            return anASTNode;
                        }
                    }
    //                if variableName == '':
                    variableName = this.setCompartmentToVariableReference( aName );
                    if (variableName != "")
                        anASTNode.setName( variableName + ".Value" );
                }
            }
            return anASTNode;
        }

        public string convertKineticLawFormula(string aFormula)
        {
            ASTNode aASTRootNode = libsbml.libsbml.parseFormula( aFormula );
            ASTNode convertedAST = this.convertVariableName( aASTRootNode );
            return libsbml.libsbml.formulaToString( convertedAST );
        }

        public int getStoichiometry(string aSpeciesID, int aStoichiometry )
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
                            return aStoichiometry;
                    }
                }
            }
            else if ( this.Model.Level == 2 )
            {
                foreach (SpeciesStruct aSpecies in this.Model.SpeciesList)
                {
                    if (aSpecies.ID == aSpeciesID)
                    {
                        if (aSpecies.Constant)
                            return 0;
                        else
                            return aStoichiometry;
                    }
                }
            }
            else
                throw new EcellException("Version"+ this.Model.Level.ToString() +" ????");
            throw new EcellException("Cannot get stoichiometry.");
        }
    

    }
}
