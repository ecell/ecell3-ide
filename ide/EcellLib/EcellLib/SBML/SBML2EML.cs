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

namespace Ecell.SBML
{
    /// <summary>
    /// 
    /// </summary>
    public class SBML2EML
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public static EcellObject Convert(string filename)
        {
            if (!File.Exists(filename))
                throw new EcellException(string.Format(MessageResources.ErrFindFile, filename));

            StreamReader reader = new StreamReader(filename);
            string aSbmlString = reader.ReadToEnd();
            reader.Close();
            SBMLDocument document = libsbml.libsbml.readSBMLFromString(aSbmlString);
            Model model = document.getModel();

            SBML_Model theModel = new SBML_Model(document, model);
            SBML_Compartment theCompartment = new SBML_Compartment(theModel);
            SBML_Parameter theParameter = new SBML_Parameter( theModel );
            SBML_Species theSpecies = new SBML_Species( theModel );
            SBML_Rule theRule = new SBML_Rule( theModel );
            SBML_Reaction theReaction = new SBML_Reaction(theModel);
            // SBML_Event theEvent = new SBML_Event(theModel);

            document.Dispose();

            //Eml eml = new Eml(null);
            string modelId = Path.GetFileNameWithoutExtension(filename);
            EcellModel modelObject = (EcellModel)EcellObject.CreateObject(modelId, "", Constants.xpathModel, "", new List<EcellData>());

            //
            // Set Stepper.
            //
            EcellObject stepper = EcellObject.CreateObject(modelId, "DE", EcellObject.STEPPER, "ODEStepper", null);
            modelObject.Children.Add(stepper);

            //
            // Set Compartment ( System ).
            //
            EcellSystem system = (EcellSystem)EcellObject.CreateObject(modelId, "/", EcellObject.SYSTEM, EcellObject.SYSTEM, new List<EcellData>());
            system.SetEcellValue("StepperID", new EcellValue("DE"));
            system.SetEcellValue("Name", new EcellValue("Default"));
            modelObject.Children.Add(system);

            foreach (CompartmentStruct aCompartment in theModel.CompartmentList)
            {
                // initialize
                theCompartment.initialize( aCompartment );

                // getPath
                string aPath = "";
                if ( theModel.Level == 1 )
                    aPath = theModel.getPath( aCompartment.Name );
                else if ( theModel.Level == 2 )
                    aPath = theModel.getPath( aCompartment.ID );       
                
                // setFullID
                if( aPath != "/" )
                {
                    system = (EcellSystem)EcellObject.CreateObject(modelId, aPath, EcellObject.SYSTEM, EcellObject.SYSTEM, new List<EcellData>());
                    modelObject.Children.Add(system);
                }

                // setStepper 
                system.SetEcellValue("StepperID", new EcellValue("DE"));

                // setName( default = [] )
                if (theModel.Level == 2)
                    if (aCompartment.Name != "")
                        system.SetEcellValue("Name", new EcellValue(aCompartment.Name));

                // setDimensions( default = 3 )
                EcellObject dimension = EcellObject.CreateObject(modelId, aPath + ":Dimensions", EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
                dimension.SetEcellValue("Value", new EcellValue((int)aCompartment.SpatialDimension));
                system.Children.Add(dimension);

                // setSIZE
                EcellObject size = EcellObject.CreateObject(modelId, aPath + ":SIZE", EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
                size.SetEcellValue("Value", new EcellValue((double)theCompartment.getCompartmentSize(aCompartment)));
                system.Children.Add(size);
            }

            // Set GlobalParameter ( Variable )
            if ( theModel.ParameterList.Count > 0)
            {
                // setGlobalParameterSystem
                EcellObject globalParameter = EcellObject.CreateObject(modelId, "/SBMLParameter", EcellObject.SYSTEM, EcellObject.SYSTEM, new List<EcellData>());
                globalParameter.SetEcellValue("StepperID", new EcellValue("DE"));
                globalParameter.SetEcellValue("Name", new EcellValue("Global Parameter"));
                modelObject.Children.Add(globalParameter);

                foreach(ParameterStruct aParameter in theModel.ParameterList)
                {
                    // setFullID
                    string parameterKey = theParameter.getParameterID( aParameter );
                    EcellObject parameter = EcellObject.CreateObject(modelId, parameterKey, EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
                        
                    // setName
                    if ( aParameter.Name != "" )
                        parameter.SetEcellValue("Name", new EcellValue(aParameter.Name));

                    // setValue
                    parameter.SetEcellValue("Value", new EcellValue(aParameter.Value));

                    // setFixed ( default = 1 )
                    if ( aParameter.Constant)
                        parameter.SetEcellValue("Fixed", new EcellValue(1));

                    // set to system.
                    globalParameter.Children.Add(parameter);
                }
            }

            // Set Species ( Variable )
            foreach(SpeciesStruct aSpecies in theModel.SpeciesList)
            {
                // setSpeciesID
                string aSpeciesID = theSpecies.getSpeciesID( aSpecies );
                EcellObject variable = EcellObject.CreateObject(modelId, aSpeciesID, EcellObject.VARIABLE, EcellObject.VARIABLE, new List<EcellData>());
                modelObject.AddEntity(variable);
                // setName
                if( theModel.Level == 2 )
                    if ( aSpecies.Name != "" )
                        variable.SetEcellValue("Name", new EcellValue(aSpecies.Name));

                // setValue
                variable.SetEcellValue("Value", new EcellValue(theSpecies.getSpeciesValue( aSpecies )));

                // setFixed
                variable.SetEcellValue("Fixed", new EcellValue(theSpecies.getConstant( aSpecies )));
            }

            // Set Rule ( Process )
            if ( theModel.RuleList.Count > 0)
            {
                // make Rule System
                string aSystemKey = "/SBMLRule";
                system = (EcellSystem)EcellObject.CreateObject(modelId, aSystemKey, EcellObject.SYSTEM, EcellObject.SYSTEM, new List<EcellData>());
                system.SetEcellValue("Name", new EcellValue("System for SBML Rule"));
                system.SetEcellValue("StepperID", new EcellValue("DE"));
                
                foreach(RuleStruct aRule in theModel.RuleList)
                {
                    theRule.initialize();

                    // setFullID        
                    string aRuleID = theRule.getRuleID();
                    EcellObject process = EcellObject.CreateObject(modelId, aRuleID, EcellObject.PROCESS, EcellObject.PROCESS, new List<EcellData>());
                    modelObject.AddEntity(process);

                    // Algebraic Rule
                    if ( aRule.RuleType == libsbml.libsbml.SBML_ALGEBRAIC_RULE )
                    {
                        process.Classname = "ExpressionAlgebraicProcess";
                    }
                    // Assignment Rule
                    else if (aRule.RuleType == libsbml.libsbml.SBML_ASSIGNMENT_RULE ||
                           aRule.RuleType == libsbml.libsbml.SBML_SPECIES_CONCENTRATION_RULE ||
                           aRule.RuleType == libsbml.libsbml.SBML_COMPARTMENT_VOLUME_RULE ||
                           aRule.RuleType == libsbml.libsbml.SBML_PARAMETER_RULE)
                    {
                        process.Classname = "ExpressionAssignmentProcess";

                        int aVariableType = theRule.getVariableType( aRule.Variable );

                        if (aVariableType == libsbml.libsbml.SBML_SPECIES)
                            theRule.setSpeciesToVariableReference( aRule.Variable, 1 );
                        else if (aVariableType == libsbml.libsbml.SBML_PARAMETER)
                            theRule.setParameterToVariableReference( aRule.Variable, 1 );
                        else if (aVariableType == libsbml.libsbml.SBML_COMPARTMENT)
                            theRule.setCompartmentToVariableReference( aRule.Variable, 1 );
                        else
                            throw new EcellException("Variable type must be Species, Parameter, or Compartment");
                    }
                    // Rate Rule
                    else if (aRule.RuleType == libsbml.libsbml.SBML_RATE_RULE)
                    {
                        process.Classname = "ExpressionFluxProcess";

                        int aVariableType = theRule.getVariableType( aRule.Variable );

                        if (aVariableType == libsbml.libsbml.SBML_SPECIES)
                            theRule.setSpeciesToVariableReference( aRule.Variable, 1 );
                        else if (aVariableType == libsbml.libsbml.SBML_PARAMETER)
                            theRule.setParameterToVariableReference( aRule.Variable, 1 );
                        else if (aVariableType == libsbml.libsbml.SBML_COMPARTMENT)
                            theRule.setCompartmentToVariableReference( aRule.Variable, 1 );
                        else
                            throw new EcellException("Variable type must be Species, Parameter, or Compartment");
                    }
                    else
                        throw new EcellException(" The type of Rule must be Algebraic, Assignment or Rate Rule");

                    // convert SBML formula to E-Cell formula
                    string convertedFormula = theRule.convertRuleFormula( aRule.Formula );

                    // set Expression Property
                    process.SetEcellValue("Expression", new EcellValue(convertedFormula));
                    
                    // setVariableReferenceList
                    process.SetEcellValue("VariableReferenceList", new EcellValue(theRule.GetVariableReferenceList()));
                }
            }

            // Set Reaction ( Process )
            foreach(ReactionStruct aReaction in theModel.ReactionList)
            {
                theReaction.initialize();

                // setFullID
                string aReactionID = theReaction.getReactionID( aReaction );
                EcellObject process = EcellObject.CreateObject(modelId, aReactionID, EcellObject.PROCESS, "ExpressionFluxProcess", new List<EcellData>());
                modelObject.AddEntity(process);

                // setName
                if ( theModel.Level == 2 )
                    if( aReaction.Name != "" )
                        process.SetEcellValue("Name",new EcellValue(aReaction.Name));

                // setSubstrate
                foreach(ReactantStruct aSubstrate in aReaction.Reactants)
                {
                    string name = "S" + theReaction.SubstrateNumber.ToString();
                    string aSubstrateID = theModel.getSpeciesReferenceID( aSubstrate.Species );
                    if ( aSubstrateID == null )
                        throw new EcellException("Species "+aSubstrate.Species+" not found");

                    if ( aSubstrate.Denominator != 1 )
                        throw new EcellException("Stoichiometry Error : E-Cell System can't set a floating Stoichiometry");
                    int coefficient = -1 * theReaction.getStoichiometry(aSubstrate.Species, aSubstrate.Stoichiometry );

                    VariableReferenceStruct aSubstrateList = new VariableReferenceStruct(name, "Variable:" + aSubstrateID, coefficient);
                    theReaction.VariableReferenceList.Add( aSubstrateList );
                    theReaction.SubstrateNumber = theReaction.SubstrateNumber + 1;
                }

                // setProduct
                foreach(ProductStruct aProduct in aReaction.Products)
                {
                    string name = 'P' + theReaction.ProductNumber.ToString();
                    string aProductID = theModel.getSpeciesReferenceID( aProduct.Species );
                    if ( aProductID == "" )
                        throw new EcellException("Species "+aProduct.Species+" not found");

                    if ( aProduct.Denominator != 1 )
                        throw new EcellException("Stoichiometry Error : E-Cell System can't set a floating Stoichiometry");

                    int coefficient = 1 * theReaction.getStoichiometry(aProduct.Species,  aProduct.Stoichiometry );

                    VariableReferenceStruct aProductList = new VariableReferenceStruct(name, "Variable:" + aProductID, coefficient);
                    theReaction.VariableReferenceList.Add( aProductList );
                    theReaction.ProductNumber = theReaction.ProductNumber + 1;
                }

                // setCatalyst
                foreach(string aModifier in aReaction.Modifiers)
                {
                    string name = "C" + theReaction.ModifierNumber.ToString();
                    string aModifierID = theModel.getSpeciesReferenceID( aModifier );
                    if ( aModifierID == "" )
                        throw new EcellException("Species "+aModifier+" not found");

                    VariableReferenceStruct aModifierList = new VariableReferenceStruct(name, "Variable:" + aModifierID, 0);
                    theReaction.VariableReferenceList.Add( aModifierList );
                    theReaction.ModifierNumber = theReaction.ModifierNumber + 1;

                }

                // setProperty
                foreach(KineticLawStruct kineticLaw in aReaction.KineticLaws)
                {
                    foreach(ParameterStruct aParameter in kineticLaw.Parameters)
                    {
                        if (theModel.Level == 1)
                            process.SetEcellValue(aParameter.Name, new EcellValue(aParameter.Value));
                        else if (theModel.Level == 2)
                            process.SetEcellValue(aParameter.ID, new EcellValue(aParameter.Value));
                    }
                                  
                    // set "Expression" Property
                    // convert SBML format formula to E-Cell format formula
                    if( kineticLaw.Formula != "" )
                    {
                        string anExpression = theReaction.convertKineticLawFormula( kineticLaw.Formula );

                        // set Expression Property for ExpressionFluxProcess
                        process.SetEcellValue("Expression", new EcellValue(anExpression));

                        // setVariableReferenceList
                        process.SetEcellValue("VariableReferenceList", new EcellValue(theReaction.GetVariableReferenceList()));
                    }
                }
            }

            return modelObject;
        }
    }
}
