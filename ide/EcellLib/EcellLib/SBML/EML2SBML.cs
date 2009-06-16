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
// Last Update:2009/02/02
//

using System;
using System.Collections.Generic;
using System.Text;
using libsbml;
using Ecell.Exceptions;
using Ecell.Objects;
using System.IO;
using System.Diagnostics;
using EcellCoreLib;

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    public class EML2SBML
    {
        private static List<string> ID_Namespace = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public static void Convert(string filename)
        {
            string sbml = filename.Replace(Constants.FileExtEML, Constants.FileExtSBML);
            Convert(filename, sbml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emlfile"></param>
        /// <param name="output"></param>
        public static void Convert(string emlfile, string output)
        {
            if (!File.Exists(emlfile))
                throw new EcellException(string.Format(MessageResources.ErrFindFile, emlfile));
            WrappedSimulator sim = new WrappedSimulator(Util.GetDMDirs());
            EcellModel model = EmlReader.Parse(emlfile, sim);
            SaveSBML(model, output);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anEml"></param>
        /// <param name="filename"></param>
        public static void SaveSBML(EcellModel anEml, string filename)
        {
            SBMLDocument aSBMLDocument = convertToSBMLModel(anEml, anEml.ModelID, 2, 3);
            libsbml.libsbml.writeSBML(aSBMLDocument, filename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anEml"></param>
        /// <param name="aBaseName"></param>
        /// <param name="aLevel"></param>
        /// <param name="aVersion"></param>
        public static SBMLDocument convertToSBMLModel(EcellModel anEml, string aBaseName, int aLevel, int aVersion)
        {
            // Check and convert Processes.
            CheckAndConvertProcesses(anEml);

            SBMLDocument aSBMLDocument = new SBMLDocument();
            aSBMLDocument.setLevelAndVersion((long)aLevel, (long)aVersion);

            Model aSBMLModel = aSBMLDocument.createModel(aBaseName);

            createModel( anEml, aSBMLModel );

            // set abogadro number and EmptySet to SBML model
            setEssentialEntity(aSBMLModel);

            //return libsbml.libsbml.writeSBMLToString( aSBMLDocument );

            return aSBMLDocument;
        }

        private static void CheckAndConvertProcesses(EcellModel anEml)
        {

        }

        private static void createModel(EcellObject anEml, Model aSBMLModel)
        {
            // set Species
            foreach(EcellObject variable in anEml.Children)
            {
                if(!(variable is EcellVariable) || variable.LocalID.Equals(EcellSystem.SIZE))
                    continue;
                createSpecies(variable, aSBMLModel);
            }
            // set Reaction
            foreach(EcellObject process in anEml.Children)
            {
                if(!(process is EcellProcess))
                    continue;
                createReaction(process, aSBMLModel);
            }
            // create SubSystem by iterating calling createModel
            foreach (EcellObject system in anEml.Children)
            {
                if(!(system is EcellSystem))
                    continue;
                // set System
                createSystem(system, aSBMLModel);
                createModel(system, aSBMLModel);
            }
        }

        private static void createSystem(EcellObject anEml, Model aSBMLModel)
        {
            if ( anEml.LocalID == "SBMLParameter" || anEml.LocalID == "SBMLRule" )
                return;

            // create Compartment object
            Compartment aCompartment = aSBMLModel.createCompartment();

            // set ID ROOT System and Other System
            string aCompartmentID = "";
            if (anEml.LocalID == "/")
                aCompartmentID = "default"; // Root system
            else
                aCompartmentID = anEml.LocalID;
            //  aCompartmentID = "default" + anEml.Key.Replace( "/", "__" );

            ID_Namespace.Add( aCompartmentID );

            if (aSBMLModel.getLevel() == 1)
                aCompartment.setName( aCompartmentID );
            else if (aSBMLModel.getLevel() == 2)
                aCompartment.setId( aCompartmentID );
                    
            foreach(EcellObject child in anEml.Children)
            {
                if (!(child is EcellVariable))
                    continue;
                // set Size and constant of Compartment
                if( child.LocalID == "SIZE" )
                {
                    double size = ((EcellSystem)anEml).SizeInVolume;
                    aCompartment.setSize(size);

                    EcellValue value = anEml.GetEcellValue("Fixed");
                    if(value != null)
                        aCompartment.setConstant(System.Convert.ToBoolean((int)value));
                }
                // set Dimensions of Compartment
                else if( child.LocalID == "Dimensions" )
                {
                    int dimension = (int)child.GetEcellValue("Value");
                    aCompartment.setSpatialDimensions(dimension);
                }
            }
            // set Outside element of Compartment
            if( anEml.ParentSystemID == "/" && anEml.LocalID != "")
            {
                aCompartment.setOutside( "default" );
            }       
            else if (!string.IsNullOrEmpty(anEml.ParentSystemID))
            {
                aCompartment.setOutside(
                    getCurrentCompartment( anEml.ParentSystemID ));
            }
        }

        private static string getCurrentCompartment(string key)
        {
            if (key == "/")
                return "default";

            string[] systems = key.Split('/');
            return systems[ systems.Length -1 ];
        }

        private static void createReaction(EcellObject anEml, Model aSBMLModel)
        {
            EcellProcess aProcess = (EcellProcess)anEml;
            List<EcellReference> aVariableReferenceList = aProcess.ReferenceList;
            string anExpression = aProcess.Expression;
            // ------------------
            //  make Rule object
            // ------------------
            if ( anEml.ParentSystemID == "/SBMLRule" )
            {
                // get Process Class
                bool aDelayFlag = false;

                anExpression = convertExpression(
                    anExpression,
                    aVariableReferenceList,
                    aProcess.ParentSystemID,
                    ID_Namespace );


                if( aProcess.Classname == "ExpressionAlgebraicProcess" )
                {
                    // create AlgebraicRule object
                    AlgebraicRule anAlgebraicRule = aSBMLModel.createAlgebraicRule();

                    // set AlgebraicRule Formula
                    anAlgebraicRule.setFormula( anExpression );
                }
                else if( aProcess.Classname == "ExpressionAssignmentProcess" )
                {
                    foreach(EcellReference aVariableReference in aVariableReferenceList)
                    {
                        if ( aVariableReference.Coefficient != 0 )
                        {
                            // create AssignmentRule object
                            AssignmentRule anAssignmentRule =aSBMLModel.createAssignmentRule();
                            // set AssignmentRule Formula
                            anAssignmentRule.setFormula( aVariableReference.Coefficient.ToString() + "* ( " + anExpression + ")" );
                            string aVariableID = getVariableReferenceId( aVariableReference.FullID, aProcess.ParentSystemID );
                            anAssignmentRule.setVariable( aVariableID );
                        }
                    }
                }
                else if( aProcess.Classname == "ExpressionFluxProcess" )
                {
                    foreach(EcellReference aVariableReference in aVariableReferenceList)
                    {
                        if ( aVariableReference.Coefficient != 0 )
                        {
                            // create AssignmentRule object
                            RateRule aRateRule = aSBMLModel.createRateRule();

                            // set AssignmentRule Formula
                            aRateRule.setFormula( aVariableReference.Coefficient + "* ( " + anExpression + ")" );

                            string aVariableID = getVariableReferenceId( aVariableReference.FullID, aProcess.ParentSystemID );
                            aRateRule.setVariable( aVariableID );
                        }
                    }
                }
                else
                {
                    throw new EcellException("The type of Process must be Algebraic, Assignment, Flux Processes: " + aProcess.Key);
                }

            }
            // ----------------------
            //  make Reaction object
            // ----------------------
            else
            {
                // create Parameter object
                Reaction aReaction = aSBMLModel.createReaction();

                // create KineticLaw Object
                KineticLaw aKineticLaw = aSBMLModel.createKineticLaw();

                // set Reaction ID
                if (aSBMLModel.getLevel() == 1)
                    aReaction.setName( anEml.LocalID );
                if (aSBMLModel.getLevel() == 2)
                    aReaction.setId( anEml.LocalID );


                foreach(EcellData aProperty in anEml.Value)
                {
                    if (!aProperty.Loadable)
                        continue;

                    string aFullPN = aProcess.FullID + ":" + aProperty.Name;

                    // set Name property ( Name )
                    if ( aProperty.Name == "Name" )
                    {
                        // set Reaction Name
                        if (aSBMLModel.getLevel() == 1)
                        {
                        }
                        else if (aSBMLModel.getLevel() == 2)
                            aReaction.setName( (string)aProperty.Value );

                    }
                    // set Expression property ( KineticLaw Formula )
                    else if ( aProperty.Name == "Expression")
                    {
                        // convert Expression of the ECELL format to
                        // SBML kineticLaw formula
                        // setExpressionAnnotation( aKineticLaw, anExpression );
                        bool aDelayFlag = false;
                        anExpression = convertExpression(anExpression,
                                             aVariableReferenceList,
                                             aProcess.ParentSystemID,
                                             ID_Namespace);

                        // get Current System Id
                        string CompartmentOfReaction = "";
                        foreach (EcellReference aVariableReference in aVariableReferenceList)
                        {
                            if (aVariableReference.Coefficient == 0)
                                continue;

                            int aFirstColon = aVariableReference.FullID.IndexOf(":");
                            int aLastColon = aVariableReference.FullID.LastIndexOf(":");
                            string sysKey = aVariableReference.FullID.Substring(aFirstColon + 1, aLastColon - aFirstColon - 1);
                            string localID = "";
                            if (sysKey.Equals("."))
                            {
                                Util.ParseSystemKey(aProcess.ParentSystemID, out sysKey, out localID);
                                CompartmentOfReaction = localID;
                            }
                            else
                            {
                                string dummy;
                                Util.ParseSystemKey(sysKey, out dummy, out localID);
                                CompartmentOfReaction = localID;
                            }
                        }

                        //if( CompartmentOfReaction == "" )
                        //{
                        //    anExpression = "(" + anExpression + ")/default/N_A";
                        //}
                        //else
                        //{   
                        //    anExpression = "(" + anExpression + ")/" + CompartmentOfReaction + "/N_A";
                        //}

                        // set KineticLaw Formula
                        if ( aDelayFlag == false )
                            aKineticLaw.setFormula( anExpression );
                        else
                        {
                            ASTNode anASTNode = libsbml.libsbml.parseFormula( anExpression );
                            anASTNode = setDelayType( anASTNode );

                            aKineticLaw.setMath( anASTNode );
                        }
                    }
                    // set VariableReference property ( SpeciesReference )
                    else if ( aProperty.Name == "VariableReferenceList" )
                    {
                        // make a flag. Because SBML model is defined
                        // both Product and Reactant. This flag is required
                        // in order to judge whether the Product and the
                        // Reactant are defined.

                        bool aReactantFlag = false;
                        bool aProductFlag = false;

                        foreach(EcellReference aVariableReference in aVariableReferenceList)
                        {
                            // --------------------------------
                            // add Reactants to Reaction object
                            // --------------------------------
                            if ( aVariableReference.Coefficient < 0 )
                            {
                                // change the Reactant Flag
                                aReactantFlag = true;
                            
                                // create Reactant object
                                SpeciesReference aReactant = aSBMLModel.createReactant();

                                // set Species Id to Reactant object
                                string aSpeciesReferenceId = getVariableReferenceId
                                                      ( aVariableReference.FullID,
                                                        aProcess.ParentSystemID );

                                aReactant.setSpecies( aSpeciesReferenceId );


                                // set Stoichiometry 
                                aReactant.setStoichiometry( -aVariableReference.Coefficient );
                            }
                            // -------------------------------
                            // add Products to Reaction object
                            // -------------------------------
                            else if ( aVariableReference.Coefficient > 0 )
                            {
                                // change the Product Flag
                                aProductFlag = true;
                                
                                // create Product object
                                SpeciesReference aProduct = aSBMLModel.createProduct();
                                
                                // set Species Id
                                string aSpeciesReferenceId = getVariableReferenceId
                                                      ( aVariableReference.FullID,
                                                        aProcess.ParentSystemID );

                                aProduct.setSpecies( aSpeciesReferenceId );
                                
                                // set Stoichiometry
                                aProduct.setStoichiometry( aVariableReference.Coefficient );
                            }

                            // --------------------------------
                            // add Modifiers to Reaction object
                            // --------------------------------
                            
                            else
                            {
                                // create Modifier object
                                ModifierSpeciesReference aModifier = aSBMLModel.createModifier();
                                
                                // set Species Id to Modifier object
                                string aVariableReferenceId = getVariableReferenceId( aVariableReference.FullID, aProcess.ParentSystemID );

                                aModifier.setSpecies( aVariableReferenceId );
                            }

                        }


                        if ( !aReactantFlag || !aProductFlag )
                        {
                            // set EmptySet Species, because if it didn"t define,
                            // Reactant or Product can not be defined.
                        
                            if ( aReactantFlag == false )
                            {
                                // create Reactant object
                                SpeciesReference aReactant = aSBMLModel.createReactant();
                                
                                // set Species Id to Reactant object
                                aReactant.setSpecies( "EmptySet" );
                        
                                // set Stoichiometry 
                                aReactant.setStoichiometry( 0 );
                            }

                            else if( aProductFlag == false )
                            {
                                // create Product object
                                SpeciesReference aProduct = aSBMLModel.createProduct();
                                
                                // set Species Id
                                aProduct.setSpecies( "EmptySet" );
                                
                                // set Stoichiometry
                                aProduct.setStoichiometry( 0 );
                            }
                        }
                    }
                    // These properties are not defined in SBML Lv2
                    else if ( aProperty.Name == "Priority" ||
                           aProperty.Name == "Activity" ||
                           aProperty.Name == "IsContinuous" ||
                           aProperty.Name == "StepperID" ||
                           aProperty.Name == "FireMethod" ||
                           aProperty.Name == "InitializeMethod" )
                    {
                        continue;
                    }
                    else
                    {
                        // create Parameter Object (Local)
                        Parameter aParameter = aSBMLModel.createKineticLawParameter();
                    
                        // set Parameter ID
                        aParameter.setId( aProperty.Name );

                        // set Parameter Value
                        aParameter.setValue((double)aProperty.Value);
                    }
                }
                // add KineticLaw Object to Reaction Object
                aReaction.setKineticLaw( aKineticLaw );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anExpression"></param>
        /// <param name="aVariableReferenceList"></param>
        /// <param name="aSystemKey"></param>
        /// <param name="ID_Namespace"></param>
        /// <returns></returns>
        private static string convertExpression(string anExpression, List<EcellReference> aVariableReferenceList, string aSystemKey, List<string> ID_Namespace)
        {
            string sbmlExpression = anExpression;
            string type;
            string sysKey;
            string localID;
            // Replace entity key.
            foreach (EcellReference er in aVariableReferenceList)
            {
                Util.ParseFullID(er.FullID, out type, out sysKey, out localID);
                sbmlExpression = sbmlExpression.Replace(er.Name, localID);
            }

            Util.ParseSystemKey(aSystemKey, out sysKey, out localID);
            sbmlExpression = sbmlExpression.Replace("self.getSuperSystem().SizeN_A", localID + " * N_A");
            sbmlExpression = sbmlExpression.Replace(".Value", "");
            sbmlExpression = sbmlExpression.Replace(".MolarConc", " / (" + localID + " * N_A)");
            return sbmlExpression;
        }

        private static ASTNode setDelayType(ASTNode anASTNode)
        {
            long aNumChildren = anASTNode.getNumChildren();

            if ( aNumChildren == 2 )
            { 
                if ( anASTNode.isFunction() == true && anASTNode.getName() == "delay" )
                    anASTNode.setType( libsbml.libsbml.AST_FUNCTION_DELAY );

                setDelayType(anASTNode.getLeftChild());
                setDelayType(anASTNode.getRightChild());
            }
            else if ( aNumChildren == 1 )
                setDelayType( anASTNode.getLeftChild() );

            return anASTNode;

        }

        private static string getVariableReferenceId(string aVariableReference, string aCurrentSystem)
        {

            int aFirstColon = aVariableReference.IndexOf(":");
            int aLastColon = aVariableReference.LastIndexOf(":");
            string type;
            string aSystemKey;
            string aVariableID;
            Util.ParseFullID(aVariableReference, out type, out aSystemKey, out aVariableID);

            // set Species Id to Reactant object
            string aSpeciesReferencePath ="";
            if ( aSystemKey == "." )
                aSpeciesReferencePath = aCurrentSystem.Substring(1).Replace( "/", "__" );
            else
                aSpeciesReferencePath = aSystemKey.Substring(1).Replace("/", "__");

            string aSystem = "";
            if( aSpeciesReferencePath.IndexOf( "__" ) < 0 )
            {
                aSystem = aSpeciesReferencePath;
            }
            else
            {
                int aLastUnderBar = aSpeciesReferencePath.LastIndexOf( "__" );
                aSystem = aSpeciesReferencePath.Substring(aLastUnderBar+2);
            }

            if ( aSpeciesReferencePath != "" )   // Root system
            {
                if( ID_Namespace.Contains( "default__" + aVariableID ) == false )
                {
                    if( aVariableID != "SIZE" )
                        return aVariableID;
                    else
                        return aSystem;
                }
                else
                {
                    if( aVariableID != "SIZE" )
                        return "default__" + aVariableID;
                    else
                        return "default";
                }
            }
            else if( aSpeciesReferencePath == "SBMLParameter" )  // Parameter
            {
                if( ID_Namespace.Contains( "SBMLParameter__" + aVariableID ) == false )
                {
                    return aVariableID;
                }
                else
                {
                    return "SBMLParameter__" + aVariableID;
                }
            }
            else    // other system
            {
                if( ID_Namespace.Contains( aSpeciesReferencePath + "__" + aVariableID ) == false )
                {
                    if( aVariableID != "SIZE" )
                        return aVariableID;
                    else
                        return aSystem;
                }
                else
                {
                    if( aVariableID != "SIZE" )
                        return aSpeciesReferencePath + "__" + aVariableID;
                    else
                        return aSpeciesReferencePath;
                }
            }
        }

        private static void createSpecies(EcellObject anEml, Model aSBMLModel)
        {
            string aCurrentCompartment = getCurrentCompartment( anEml.ParentSystemID );
            Compartment aCurrentCompartmentObj = aSBMLModel.getCompartment( aCurrentCompartment );

            if( aCurrentCompartment == "SBMLParameter" )
            {

                // ------------------------
                // create Parameter object
                // ------------------------
                Parameter aParameter = aSBMLModel.createParameter();

                string aParameterID;
                if ( ID_Namespace.Contains(anEml.LocalID) == false )
                    aParameterID = anEml.LocalID;
                else
                    aParameterID = "SBMLParamter__" + anEml.LocalID;


                // set Paramter ID to Id namespace
                ID_Namespace.Add( aParameterID );
                        

                // set Parameter ID
                if (aSBMLModel.getLevel() == 1)
                    aParameter.setName( aParameterID );

                else if (aSBMLModel.getLevel() == 2)
                    aParameter.setId( aParameterID );


                // set Parameter Name, Value and Constant
                foreach(EcellData aProperty in anEml.Value)
                {
                    string aFullPN = anEml.FullID + ":" + aProperty.Name;

                    // set Parameter Name
                    if ( aProperty.Name == "Name" )
                    {
                        if (aSBMLModel.getLevel() == 1)
                        {
                        }
                        if (aSBMLModel.getLevel() == 2)
                        {
                            aParameter.setName( (string)aProperty.Value );
                        }
                    }

                    // set Parameter Value
                    else if ( aProperty.Name == "Value" )
                    {
                        aParameter.setValue( (double)aProperty.Value );
                    }
                    // set Constant 
                    else if ( aProperty.Name == "Fixed" )
                    {
                        aParameter.setConstant(System.Convert.ToBoolean((int)aProperty.Value));
                    }
                    else
                    {
                        Trace.WriteLine("Unrepresentable property; " + aFullPN);
                        //throw new EcellException("Unrepresentable property; " + aFullPN);
                    }
                }
            }
            else
            {
                if( anEml.LocalID != "SIZE" && anEml.LocalID != "Dimensions" )
                {
                    // create Species object
                    Species aSpecies = aSBMLModel.createSpecies();

                    // set Species ID
                    string aSpeciesID;
                    if ( ID_Namespace.Contains( anEml.LocalID) == false )
                        aSpeciesID = anEml.LocalID;
                    else
                    {
                        if ( anEml.ParentSystemID.Substring(1) != "" )
                            aSpeciesID = anEml.ParentSystemID.Substring(1).Replace( "/", "__" )
                                         + "__" + anEml.LocalID;
                        else
                            aSpeciesID = "default__" + anEml.LocalID;
                    }

                    ID_Namespace.Add( aSpeciesID );

                    if (aSBMLModel.getLevel() == 1)
                        aSpecies.setName( aSpeciesID );
                    if (aSBMLModel.getLevel() == 2)
                        aSpecies.setId( aSpeciesID );


                    // set Compartment of Species
                    if( aCurrentCompartment == "" )
                        aSpecies.setCompartment( "default" );
                    else
                        aSpecies.setCompartment( aCurrentCompartment );
                    
                    // set Species Name, Value and Constant
                    foreach(EcellData aProperty in anEml.Value)
                    {
                        string aFullPN = anEml.FullID + ":" + aProperty.Name;

                        // set Species Name
                        if ( aProperty.Name == "Name" )
                        {
                            if (aSBMLModel.getLevel() == 1)
                            {
                            }
                            else if (aSBMLModel.getLevel() == 2)
                            {
                                aSpecies.setName( (string)aProperty.Value );
                            }
                        }
                        // set Species Value
                        else if ( aProperty.Name == "Value" )
                        {
    //                        aMolarValue = convertToMoleUnit(
    //                            anEml.getEntityProperty( aFullPN )[0] )

                            aSpecies.setInitialAmount( (double)aProperty.Value / 6.0221367e+23 );
                        }
                        // set Species Constant
                        else if ( aProperty.Name == "Fixed" )
                        {
                            aSpecies.setConstant(System.Convert.ToBoolean((int)aProperty.Value));
                        }
                        // set Concentration by rule
                        else if ( aProperty.Name == "MolarConc" )
                        {
                            // XXX: units are just eventually correct here, because
                            // SBML falls back to mole and liter for substance and
                            // volume of the species if these are unspecified.
                            if (aSBMLModel.getLevel() == 1)
                            {
                                double compVol;
                                if (aCurrentCompartmentObj != null)
                                    compVol = aCurrentCompartmentObj.getVolume();
                                else
                                    compVol = 1.0;
                                double propValue = (double)aProperty.Value;
                                aSpecies.setInitialAmount( compVol * propValue );
                            }
                            else // SBML lv.2
                            {
                                aSpecies.setInitialConcentration( (double)aProperty.Value );
                            }
                        }
                        else
                        {
                            Trace.WriteLine("Unrepresentable property; " + aFullPN);
                            //throw new EcellException("Unrepresentable property: " + aFullPN);
                        }
                    }
                }
            }
        }

        private static void setEssentialEntity(Model aSBMLModel)
        {
            //
            //  set N_A Parameter
            //
            bool isAbogadroNumber = false;
            foreach (ParameterStruct aParameter in SbmlFunctions.getParameter(aSBMLModel))
            {
                if (aSBMLModel.getLevel() == 1 && aParameter.Name == "N_A")
                    isAbogadroNumber = true;
                else if (aSBMLModel.getLevel() == 2 && aParameter.ID == "N_A")
                    isAbogadroNumber = true;
            }
            if ( !isAbogadroNumber )
            {
                // create Parameter object
                Parameter aParameter = aSBMLModel.createParameter();
                // set Parameter Name
                if (aSBMLModel.getLevel() == 1)
                    aParameter.setName("N_A");
                else if (aSBMLModel.getLevel() == 2)
                    aParameter.setId("N_A");
                // set Parameter Value
                aParameter.setValue(6.0221367e+23);
                // set Parameter Constant
                aParameter.setConstant(true);
            }

            // ------------
            // set EmptySet
            // ------------
            bool isEmptySet = false;
            foreach (SpeciesStruct aSpecies in SbmlFunctions.getSpecies(aSBMLModel))
            {
                if (aSBMLModel.getLevel() == 1 && aSpecies.Name == "EmptySet")
                    isEmptySet = true;
                else if (aSBMLModel.getLevel() == 2 && aSpecies.ID == "EmptySet")
                    isEmptySet = true;
            }
            if (!isEmptySet)
            {
                // create Species object
                Species aSpecies = aSBMLModel.createSpecies();
                // set Species Name
                if (aSBMLModel.getLevel() == 1)
                    aSpecies.setName("EmptySet");
                else if (aSBMLModel.getLevel() == 2)
                    aSpecies.setId("EmptySet");
                // set Species Compartment
                aSpecies.setCompartment("default");
                // set Species Amount
                aSpecies.setInitialAmount(0);
                // set Species Constant
                aSpecies.setConstant(true);
            }
        }

        private static double convertToMoleUnit(float aValue )
        {
            return aValue / 6.0221367e+23;  // N_A 
        }
    }
}
