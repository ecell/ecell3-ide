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
using libsbml;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    /// <summary>
    /// static functions which parse SBML to structs for converter.
    /// </summary>
    public class SbmlFunctions
    {
        /// <summary>
        /// [ CompartmentStruct ]
        /// [[ Id , Name , SpatialDimension , Size , Volume , Unit , Ouside , Constant ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<CompartmentStruct> getCompartment(Model aSBMLmodel)
        {
            List<CompartmentStruct> list = new List<CompartmentStruct>();
            ListOfCompartments compartments = aSBMLmodel.getListOfCompartments();
            for (int i = 0; i < compartments.size(); i++ )
            {
                Compartment item = aSBMLmodel.getCompartment(i);
                string anId = item.getId();
                string aName = item.getName();
                long aSpatialDimension = item.getSpatialDimensions();
                double aSize = GetCompartmentSize(item);
                double aVolume = GetCompartmentVolume(item);

                string anUnit = item.getUnits();
                string anOutside = item.getOutside();
                bool aConstant = item.getConstant();

                CompartmentStruct compartment = new CompartmentStruct(
                    anId,
                    aName,
                    aSpatialDimension,
                    aSize,
                    aVolume,
                    anUnit,
                    anOutside,
                    aConstant);

                list.Add(compartment);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static double GetCompartmentVolume(Compartment item)
        {
            double aVolume;
            if (item.isSetVolume())
                aVolume = item.getVolume();
            else
                aVolume = double.NaN;
            return aVolume;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static double GetCompartmentSize(Compartment item)
        {
            double aSize;
            if (item.isSetSize())
                aSize = item.getSize();
            else
                aSize = double.NaN;
            return aSize;
        }

        /// <summary>
        /// [ EventStruct ]
        ///  [[ Id , Name , StringTrigger , StringDelay , TimeUnit , [[ VariableAssignment , StringAssignment ]] ]] 
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<EventStruct> getEvent(Model aSBMLmodel)
        {
            List<EventStruct> list = new List<EventStruct>();
            ListOfEvents events = aSBMLmodel.getListOfEvents();
            for (int i = 0; i < events.size(); i++ )
            {
                Event anEvent = aSBMLmodel.getEvent(i);
                string anId_Ev = anEvent.getId();
                string aName_Ev = anEvent.getName();
                Trigger anASTNode_Ev_Tr = anEvent.getTrigger();
                string aString_Ev_Tr = libsbml.libsbml.formulaToString(anASTNode_Ev_Tr.getMath());
                Delay anASTNode_Ev_De = anEvent.getDelay();
                string aString_Ev_De = libsbml.libsbml.formulaToString(anASTNode_Ev_Tr.getMath());

                string aTimeUnit_Ev = anEvent.getTimeUnits();

                List<EventAssignmentStruct> listOfEventAssignments = new List<EventAssignmentStruct>();

                ListOfEventAssignments assignments = anEvent.getListOfEventAssignments();
                for (int j = 0; j < assignments.size(); j++ )
                {
                    EventAssignment anEventAssignment = anEvent.getEventAssignment(j);

                    string aVariable_Ev_As = anEventAssignment.getVariable();
                    ASTNode anASTNode_Ev_As = anEventAssignment.getMath();
                    string aString_Ev_As = libsbml.libsbml.formulaToString(anASTNode_Ev_As);

                    EventAssignmentStruct listOfEventAssignment = new EventAssignmentStruct(
                        aVariable_Ev_As,
                        aString_Ev_As);

                    listOfEventAssignments.Add(listOfEventAssignment);
                }
                EventStruct eventStruct = new EventStruct(
                    anId_Ev,
                    aName_Ev,
                    aString_Ev_Tr,
                    aString_Ev_De,
                    aTimeUnit_Ev,
                    listOfEventAssignments);

                list.Add(eventStruct);
            }
            return list;
        }

        /// <summary>
        /// [ FunctionDefinitionStruct ]
        /// [[ Id , Name , String ]]
        /// <param name="aSBMLmodel"></param>
        /// </summary>
        public static List<FunctionDefinitionStruct> getFunctionDefinition(Model aSBMLmodel)
        {
            List<FunctionDefinitionStruct> list = new List<FunctionDefinitionStruct>();

            ListOfFunctionDefinitions functionDefinitions = aSBMLmodel.getListOfFunctionDefinitions();
            for (int i = 0; i < functionDefinitions.size(); i++ )
            {
                FunctionDefinition aFunctionDefinition = aSBMLmodel.getFunctionDefinition(i);

                string anId_FD = aFunctionDefinition.getId();
                string aName_FD = aFunctionDefinition.getName();

                ASTNode anASTNode_FD = aFunctionDefinition.getMath();
                string aString_FD = libsbml.libsbml.formulaToString(anASTNode_FD );

                FunctionDefinitionStruct fd = new FunctionDefinitionStruct(
                    anId_FD,
                    aName_FD,
                    aString_FD);

                list.Add(fd);
            }
            return list;
        }

        /// <summary>
        /// [ ParameterStruct ]
        /// [[ Id , Name , Value , Unit , Constant ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<ParameterStruct> getParameter(Model aSBMLmodel)
        {
            List<ParameterStruct> list = new List<ParameterStruct>();

            ListOfParameters parameters = aSBMLmodel.getListOfParameters();
            for (int i = 0; i < parameters.size(); i++ )
            {
                Parameter aParameter = aSBMLmodel.getParameter(i);

                string anId_Pa = aParameter.getId();
                string aName_Pa = aParameter.getName();
                double aValue_Pa = GetParameterValue(aParameter);
                    
                string anUnit_Pa = aParameter.getUnits();
                bool aConstant_Pa = aParameter.getConstant();

                ParameterStruct parameter = new ParameterStruct(
                    anId_Pa,
                    aName_Pa,
                    aValue_Pa,
                    anUnit_Pa,
                    aConstant_Pa);

                list.Add(parameter);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aParameter"></param>
        /// <returns></returns>
        public static double GetParameterValue(Parameter aParameter)
        {
            double aValue_Pa;
            if (aParameter.isSetValue())
                aValue_Pa = aParameter.getValue();
            else
                aValue_Pa = double.NaN;
            return aValue_Pa;
        }

        /// <summary>
        /// [ ReactionStruct ]
        /// [[ Id , Name , [ KineticLawStruct ] , Reversible , Fast , [ ReactantStruct ] , [ ProductStruct ] , [ ModifierSpecies ] ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<ReactionStruct> getReaction(Model aSBMLmodel)
        {
            List<ReactionStruct> list = new List<ReactionStruct>();

            ListOfReactions reactions = aSBMLmodel.getListOfReactions();
            for (int i = 0; i < reactions.size(); i++ )
            {
                Reaction aReaction = aSBMLmodel.getReaction(i);

                string anId = aReaction.getId();
                string aName =aReaction.getName();

                //----------KineticLaw----------------------------------
                List<KineticLawStruct> ListOfKineticLaw = new List<KineticLawStruct>();
                if( aReaction.isSetKineticLaw())
                {
                    KineticLaw aKineticLaw = aReaction.getKineticLaw();
                    if( aKineticLaw != null)
                    {
                        string aFormula_KL;
                        if( aKineticLaw.isSetFormula())
                            aFormula_KL = aKineticLaw.getFormula();
                        else
                            aFormula_KL = "";
                      
                        List<string> aString_KL = new List<string>();
                        if (aSBMLmodel.getLevel() == 1)
                        {
                            aString_KL.Add( "" );
                        }
                        else
                        {
                            if (aKineticLaw.isSetMath())
                            {
                                ASTNode anASTNode_KL = aKineticLaw.getMath();
                                aString_KL.Add( libsbml.libsbml.formulaToString( anASTNode_KL ) );
                            }
                            else
                                aString_KL.Add( "" );
                        }
                        
                        string aTimeUnit_KL = aKineticLaw.getTimeUnits();
                        string aSubstanceUnit_KL = aKineticLaw.getSubstanceUnits();
                
                        List<ParameterStruct> listOfParameters = new List<ParameterStruct>();

                        ListOfParameters parameters = aKineticLaw.getListOfParameters();
                        for (int j = 0; j < parameters.size(); j++ )
                        {
                            Parameter aParameter = aKineticLaw.getParameter(j);
                            if (aParameter == null)
                                continue;
                            string anId_KL_P = aParameter.getId();
                            string aName_KL_P = aParameter.getName();
                            double aValue_KL_P = aParameter.getValue();
                            string aUnit_KL_P = aParameter.getUnits();
                            bool aConstant_KL_P = aParameter.getConstant();

                            ParameterStruct parameter = new ParameterStruct(
                                anId_KL_P,
                                aName_KL_P,
                                aValue_KL_P,
                                aUnit_KL_P,
                                aConstant_KL_P);

                            listOfParameters.Add( parameter );
                        }

                        XMLNode anExpressionAnnotation = aKineticLaw.getAnnotation();

                        KineticLawStruct kineticLaw = new KineticLawStruct(
                            aFormula_KL,
                            aString_KL,
                            aTimeUnit_KL,
                            aSubstanceUnit_KL,
                            listOfParameters,
                            anExpressionAnnotation );

                        ListOfKineticLaw.Add(kineticLaw);
                    }
                }

                bool aReversible = aReaction.getReversible();
                bool aFast = aReaction.getFast();

                //----------Reactants----------------------------------
                List<ReactantStruct> ListOfReactants = new List<ReactantStruct>();

                ListOfSpeciesReferences reactants = aReaction.getListOfReactants();
                for (int k = 0; k < reactants.size(); k++ )
                {
                    SpeciesReference aSpeciesReference = aReaction.getReactant(k);

                    string aSpecies_R = aSpeciesReference.getSpecies();
                    int aStoichiometry_R = (int)aSpeciesReference.getStoichiometry();

                    string aString_R = GetStoichiometryMath(aSpeciesReference);

                    int aDenominator_R = aSpeciesReference.getDenominator();

                    ReactantStruct reactant = new ReactantStruct(
                        aSpecies_R,
                        aStoichiometry_R,
                        aString_R,
                        aDenominator_R);

                    ListOfReactants.Add( reactant );
                }

                //----------Products----------------------------------
                List<ProductStruct> ListOfProducts = new List<ProductStruct>();

                ListOfSpeciesReferences products = aReaction.getListOfProducts();
                long max = products.size();
                for (int l = 0; l < max; l++)
                {
                    SpeciesReference aSpeciesReference = aReaction.getProduct(l);

                    string aSpecies_P = aSpeciesReference.getSpecies();
                    double aStoichiometry_P = aSpeciesReference.getStoichiometry();

                    string aString_P = GetStoichiometryMath(aSpeciesReference);

                    int aDenominator_P = aSpeciesReference.getDenominator();

                    ProductStruct product = new ProductStruct(
                        aSpecies_P,
                        aStoichiometry_P,
                        aString_P,
                        aDenominator_P);

                    ListOfProducts.Add( product );
                }

                //----------Modifiers----------------------------------
                List<string> ListOfModifiers = new List<string>();
                ListOfSpeciesReferences modifiers = aReaction.getListOfModifiers();
                for (long l = 0; l < modifiers.size(); l++ )
                {
                    ModifierSpeciesReference aSpeciesReference = aReaction.getModifier(l);

                    string aSpecies_M = aSpeciesReference.getSpecies();
                    ListOfModifiers.Add( aSpecies_M );
                }
                ReactionStruct reaction = new ReactionStruct(
                    anId,
                    aName,
                    ListOfKineticLaw,
                    aReversible,
                    aFast,
                    ListOfReactants,
                    ListOfProducts,
                    ListOfModifiers );

                list.Add(reaction);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpeciesReference"></param>
        /// <returns></returns>
        public static string GetStoichiometryMath(SpeciesReference aSpeciesReference)
        {
            string aString_R = null;
            if (aSpeciesReference.isSetStoichiometryMath())
            {
                ASTNode anASTNode_R = aSpeciesReference.getStoichiometryMath().getMath();
                aString_R = libsbml.libsbml.formulaToString(anASTNode_R);
            }
            return aString_R;
        }

        /// <summary>
        /// [ RuleStruct ]
        /// [[ RuleType, Formula, Variable ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<RuleStruct> getRule(Model aSBMLmodel)
        {
            List<RuleStruct> list = new List<RuleStruct>();

            ListOfRules rules = aSBMLmodel.getListOfRules();
            for (int i = 0; i < rules.size(); i++ )
            {
                Rule aRule = aSBMLmodel.getRule(i);

                int aRuleType = aRule.getTypeCode();
                string aFormula = aRule.getFormula();
                string aVariable = null;

                if ( aRuleType == libsbml.libsbml.SBML_ALGEBRAIC_RULE )
                    aVariable = "";
                else if (aRuleType == libsbml.libsbml.SBML_ASSIGNMENT_RULE ||
                       aRuleType == libsbml.libsbml.SBML_RATE_RULE)
                    aVariable = aRule.getVariable();
                //else if (aRuleType == libsbml.libsbml.SBML_SPECIES_CONCENTRATION_RULE)
                //    aVariable = aRule.getSpecies();
                //else if (aRuleType == libsbml.libsbml.SBML_COMPARTMENT_VOLUME_RULE)
                //    aVariable = aRule.getCompartment();
                // ToDo: LibSBML3.2C#で呼び出せない。どうするか検討する。
                else if (aRuleType == libsbml.libsbml.SBML_PARAMETER_RULE)
                    aVariable = aRule.getName();
                else
                    throw new EcellException(" The type of Rule must be Algebraic, Assignment or Rate Rule");

                RuleStruct rule = new RuleStruct(
                    aRuleType,
                    aFormula,
                    aVariable );

                list.Add(rule);
            }

            return list;
        }

        /// <summary>
        /// [ SpeciesStruct ]
        /// [[ Id , Name , Compartment , InitialAmount , InitialConcentration , SubstanceUnit , SpatialSizeUnit , Unit , HasOnlySubstanceUnit , BoundaryCondition , Charge , Constant ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<SpeciesStruct> getSpecies(Model aSBMLmodel)
        {
            List<SpeciesStruct> list = new List<SpeciesStruct>();

            ListOfSpecies listOfSpecies = aSBMLmodel.getListOfSpecies();
            for (int i = 0; i < listOfSpecies.size(); i++ )
            {
                Species aSpecies = aSBMLmodel.getSpecies(i);

                string anId_Sp = aSpecies.getId();
                string aName_Sp = aSpecies.getName();
                string aCompartment_Sp = aSpecies.getCompartment();

                double anInitialAmount_Sp = GetInitialAmount(aSpecies);
                double anInitialConcentration_Sp = GetInitialConcentration(aSpecies);
                    
                string aSubstanceUnit_Sp = aSpecies.getSubstanceUnits();
                string aSpatialSizeUnit_Sp = aSpecies.getSpatialSizeUnits();
                string anUnit_Sp = aSpecies.getUnits();
                bool aHasOnlySubstanceUnit_Sp = aSpecies.getHasOnlySubstanceUnits();
                bool aBoundaryCondition_Sp = aSpecies.getBoundaryCondition();
                int aCharge_Sp = aSpecies.getCharge();
                bool aConstant_Sp = aSpecies.getConstant();

                SpeciesStruct species = new SpeciesStruct(
                    anId_Sp,
                    aName_Sp,
                    aCompartment_Sp,
                    anInitialAmount_Sp,
                    anInitialConcentration_Sp,
                    aSubstanceUnit_Sp,
                    aSpatialSizeUnit_Sp,
                    anUnit_Sp,
                    aHasOnlySubstanceUnit_Sp,
                    aBoundaryCondition_Sp,
                    aCharge_Sp,
                    aConstant_Sp);
           
                list.Add( species );
            }

            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpecies"></param>
        /// <returns></returns>
        public static double GetInitialConcentration(Species aSpecies)
        {
            double anInitialConcentration_Sp;
            if (aSpecies.isSetInitialConcentration())
                anInitialConcentration_Sp = aSpecies.getInitialConcentration();
            else
                anInitialConcentration_Sp = double.NaN;
            return anInitialConcentration_Sp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpecies"></param>
        /// <returns></returns>
        public static double GetInitialAmount(Species aSpecies)
        {
            double anInitialAmount_Sp;
            if (aSpecies.isSetInitialAmount())
                anInitialAmount_Sp = aSpecies.getInitialAmount();
            else
                anInitialAmount_Sp = double.NaN;
            return anInitialAmount_Sp;
        }

        /// <summary>
        /// [ UnitDefinitionStruct ]
        /// [[ Id , Name , [ UnitStruct ] ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<UnitDefinitionStruct> getUnitDefinition(Model aSBMLmodel)
        {
            List<UnitDefinitionStruct> list = new List<UnitDefinitionStruct>();

            ListOfUnitDefinitions listOfUnitDefinitions = aSBMLmodel.getListOfUnitDefinitions();
            for (int i = 0; i < listOfUnitDefinitions.size(); i++ )
            {
                UnitDefinition anUnitDefinition = aSBMLmodel.getUnitDefinition(i);

                string anId = anUnitDefinition.getId();
                string aName = anUnitDefinition.getName();


                List<UnitStruct> unitList = new List<UnitStruct>();

                ListOfUnits listOfUnits = anUnitDefinition.getListOfUnits();
                for (int j = 0; j < listOfUnits.size(); j++)
                {
                    Unit anUnit = anUnitDefinition.getUnit(j);

                    int anUnitKind = anUnit.getKind();
                    string aKind = libsbml.libsbml.UnitKind_toString( anUnitKind );
                    int anExponent = anUnit.getExponent();
                    int aScale = anUnit.getScale();
                    double aMultiplier = anUnit.getMultiplier();
                    double anOffset = anUnit.getOffset();

                    UnitStruct unit = new UnitStruct(
                        aKind,
                        anExponent,
                        aScale,
                        aMultiplier,
                        anOffset );

                    unitList.Add( unit );
                }

                UnitDefinitionStruct unitDefinition = new UnitDefinitionStruct(
                    anId,
                    aName,
                    unitList );

                list.Add(unitDefinition);
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        public static List<InitialAssignmentStruct> getInitialAssignments(Model aSBMLmodel)
        {
            List<InitialAssignmentStruct> list = new List<InitialAssignmentStruct>();
            ListOfInitialAssignments initList = aSBMLmodel.getListOfInitialAssignments();
            for (int i = 0; i < initList.size(); i++)
            {
                InitialAssignment init = initList.get(i);
                string symbol = init.getSymbol();
                ASTNode math = init.getMath();
            }
            return list;

        }
    }

    /// <summary>
    /// [ Id , Name , SpatialDimension , Size , Volume , Unit , Ouside , Constant ]
    /// </summary>
    public struct CompartmentStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public long SpatialDimension;
        /// <summary>
        /// 
        /// </summary>
        public double Size;
        /// <summary>
        /// 
        /// </summary>
        public double Volume;
        /// <summary>
        /// 
        /// </summary>
        public string Unit;
        /// <summary>
        /// 
        /// </summary>
        public string Outside;
        /// <summary>
        /// 
        /// </summary>
        public bool Constant;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="dimension"></param>
        /// <param name="size"></param>
        /// <param name="volume"></param>
        /// <param name="unit"></param>
        /// <param name="outside"></param>
        /// <param name="constant"></param>
        public CompartmentStruct(
            string id,
            string name,
            long dimension,
            double size,
            double volume,
            string unit,
            string outside,
            bool constant)
        {
            this.ID = id;
            this.Name = name;
            this.SpatialDimension = dimension;
            this.Size = size;
            this.Volume = volume;
            this.Unit = unit;
            this.Outside = outside;
            this.Constant = constant;
        }
    }

    /// <summary>
    ///  [ Id , Name , StringTrigger , StringDelay , TimeUnit , [EventAssignmentStruct] ]
    /// </summary>
    public struct EventStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public string Trigger;
        /// <summary>
        /// 
        /// </summary>
        public string Delay;
        /// <summary>
        /// 
        /// </summary>
        public string TimeUnits;
        /// <summary>
        /// 
        /// </summary>
        public List<EventAssignmentStruct> EventAssignments;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="trigger"></param>
        /// <param name="delay"></param>
        /// <param name="timeUnits"></param>
        /// <param name="eventAssignments"></param>
        public EventStruct(
            string id,
            string name,
            string trigger,
            string delay,
            string timeUnits,
            List<EventAssignmentStruct> eventAssignments)
        {
            this.ID = id;
            this.Name = name;
            this.Trigger = trigger;
            this.Delay = delay;
            this.TimeUnits = timeUnits;
            this.EventAssignments = eventAssignments;
        }
    }

    /// <summary>
    ///  [ Variable , StringFormula ] 
    /// </summary>
    public struct EventAssignmentStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Variable;
        /// <summary>
        /// 
        /// </summary>
        public string Formula;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="formula"></param>
        public EventAssignmentStruct(string variable, string formula)
        {
            this.Variable = variable;
            this.Formula = formula;
        }
    }

    /// <summary>
    /// [ Id , Name , StringFormula ]
    /// </summary>
    public struct FunctionDefinitionStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public string Formula;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        public FunctionDefinitionStruct(string id, string name, string formula)
        {
            this.ID = id;
            this.Name = name;
            this.Formula = formula;
        }
    }

    /// <summary>
    /// [ Id , Name , Value , Unit , Constant ]
    /// </summary>
    public struct ParameterStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public double Value;
        /// <summary>
        /// 
        /// </summary>
        public string Unit;
        /// <summary>
        /// 
        /// </summary>
        public bool Constant;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="constant"></param>
        public ParameterStruct(
            string id,
            string name,
            double value,
            string unit,
            bool constant)
        {
            this.ID = id;
            this.Name = name;
            this.Value = value;
            this.Unit = unit;
            this.Constant = constant;
        }
    }

    /// <summary>
    /// [ Id , Name , [ KineticLawStruct ] , Reversible , Fast , [ ReactantStruct ] , [ ProductStruct ] , [ ModifierSpecies ] ]
    /// </summary>
    public struct ReactionStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public List<KineticLawStruct> KineticLaws;
        /// <summary>
        /// 
        /// </summary>
        public bool Reversible;
        /// <summary>
        /// 
        /// </summary>
        public bool Fast;
        /// <summary>
        /// 
        /// </summary>
        public List<ReactantStruct> Reactants;
        /// <summary>
        /// 
        /// </summary>
        public List<ProductStruct> Products;
        /// <summary>
        /// 
        /// </summary>
        public List<string> Modifiers;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="kineticLaws"></param>
        /// <param name="reversible"></param>
        /// <param name="fast"></param>
        /// <param name="reactants"></param>
        /// <param name="products"></param>
        /// <param name="modifiers"></param>
        public ReactionStruct(
             string id,
             string name,
             List<KineticLawStruct> kineticLaws,
             bool reversible,
             bool fast,
             List<ReactantStruct> reactants,
             List<ProductStruct> products,
             List<string> modifiers)
        {
            this.ID = id;
            this.Name = name;
            this.KineticLaws = kineticLaws;
            this.Reversible = reversible;
            this.Fast = fast;
            this.Reactants = reactants;
            this.Products = products;
            this.Modifiers = modifiers;
        }
    }

    /// <summary>
    /// [ Formula , [ Math ] , TimeUnit , SubstanceUnit , [ ParameterStruct ] , ExpressionAnnotation ]
    /// </summary>
    public struct KineticLawStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Formula;
        /// <summary>
        /// 
        /// </summary>
        public List<string> MathList;
        /// <summary>
        /// 
        /// </summary>
        public string TimeUnit;
        /// <summary>
        /// 
        /// </summary>
        public string SubstanceUnit;
        /// <summary>
        /// 
        /// </summary>
        public List<ParameterStruct> Parameters;
        /// <summary>
        /// 
        /// </summary>
        public XMLNode ExpressionAnnotation;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="math"></param>
        /// <param name="timeUnit"></param>
        /// <param name="substance"></param>
        /// <param name="parameters"></param>
        /// <param name="annotation"></param>
        public KineticLawStruct(
            string formula,
            List<string> math,
            string timeUnit,
            string substance,
            List<ParameterStruct> parameters,
            XMLNode annotation)
        {
            this.Formula = formula;
            this.MathList = math;
            this.TimeUnit = timeUnit;
            this.SubstanceUnit = substance;
            this.Parameters = parameters;
            this.ExpressionAnnotation = annotation;
        }
    }

    /// <summary>
    /// [ Species , ( Stoichiometry , StoichiometryMath ) , Denominator  ]
    /// </summary>
    public struct ReactantStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Species;
        /// <summary>
        /// 
        /// </summary>
        public double Stoichiometry;
        /// <summary>
        /// 
        /// </summary>
        public string Formula;
        /// <summary>
        /// 
        /// </summary>
        public int Denominator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="species"></param>
        /// <param name="stoichiometry"></param>
        /// <param name="formula"></param>
        /// <param name="denominator"></param>
        public ReactantStruct(
                    string species,
                    int stoichiometry,
                    string formula,
                    int denominator)
        {
            this.Species = species;
            this.Stoichiometry = stoichiometry;
            this.Formula = formula;
            this.Denominator = denominator;
        }
    }

    /// <summary>
    /// [  Species , ( Stoichiometry , StoichiometryMath ) , Denominator ]
    /// </summary>
    public struct ProductStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Species;
        /// <summary>
        /// 
        /// </summary>
        public double Stoichiometry;
        /// <summary>
        /// 
        /// </summary>
        public string Formula;
        /// <summary>
        /// 
        /// </summary>
        public int Denominator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="species"></param>
        /// <param name="stoichiometry"></param>
        /// <param name="formula"></param>
        /// <param name="denominator"></param>
        public ProductStruct(
                    string species,
                    double stoichiometry,
                    string formula,
                    int denominator)
        {
            this.Species = species;
            this.Stoichiometry = stoichiometry;
            this.Formula = formula;
            this.Denominator = denominator;
        }
    }

    /// <summary>
    /// [ RuleType, Formula, Variable ]
    /// </summary>
    public struct RuleStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public int RuleType;
        /// <summary>
        /// 
        /// </summary>
        public string Formula;
        /// <summary>
        /// 
        /// </summary>
        public string Variable;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="formula"></param>
        /// <param name="variable"></param>
        public RuleStruct(
            int ruleType,
            string formula,
            string variable)
        {
            this.RuleType = ruleType;
            this.Formula = formula;
            this.Variable = variable;
        }
    }

    /// <summary>
    /// [ Id , Name , Compartment , InitialAmount , InitialConcentration , SubstanceUnit , SpatialSizeUnit , Unit , HasOnlySubstanceUnit , BoundaryCondition , Charge , Constant ]
    /// </summary>
    public struct SpeciesStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public string Compartment;
        /// <summary>
        /// 
        /// </summary>
        public double InitialAmount;
        /// <summary>
        /// 
        /// </summary>
        public double InitialConcentration;
        /// <summary>
        /// 
        /// </summary>
        public string SubstanceUnit;
        /// <summary>
        /// 
        /// </summary>
        public string SpatialSizeUnit;
        /// <summary>
        /// 
        /// </summary>
        public string Unit;
        /// <summary>
        /// 
        /// </summary>
        public bool HasOnlySubstanceUnit;
        /// <summary>
        /// 
        /// </summary>
        public bool BoundaryCondition;
        /// <summary>
        /// 
        /// </summary>
        public int Charge;
        /// <summary>
        /// 
        /// </summary>
        public bool Constant;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="compartment"></param>
        /// <param name="initialAmount"></param>
        /// <param name="initialConcentration"></param>
        /// <param name="substanceUnit"></param>
        /// <param name="spatialSizeUnit"></param>
        /// <param name="unit"></param>
        /// <param name="hasOnlySubstanceUnit"></param>
        /// <param name="boundaryCondition"></param>
        /// <param name="charge"></param>
        /// <param name="constant"></param>
        public SpeciesStruct(
            string id,
            string name,
            string compartment,
            double initialAmount,
            double initialConcentration,
            string substanceUnit,
            string spatialSizeUnit,
            string unit,
            bool hasOnlySubstanceUnit,
            bool boundaryCondition,
            int charge,
            bool constant)
        {
            this.ID = id;
            this.Name = name;
            this.Compartment = compartment;
            this.InitialAmount = initialAmount;
            this.InitialConcentration = initialConcentration;
            this.SubstanceUnit = substanceUnit;
            this.SpatialSizeUnit = spatialSizeUnit;
            this.Unit = unit;
            this.HasOnlySubstanceUnit = hasOnlySubstanceUnit;
            this.BoundaryCondition = boundaryCondition;
            this.Charge = charge;
            this.Constant = constant;
        }
    }

    /// <summary>
    /// [ Id , Name , [ UnitStruct ] ]
    /// </summary>
    public struct UnitDefinitionStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public List<UnitStruct> Units;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="units"></param>
        public UnitDefinitionStruct(
            string id,
            string name,
            List<UnitStruct> units)
        {
            this.ID = id;
            this.Name = name;
            this.Units = units;
        }
    }

    /// <summary>
    /// [ Kind , Exponent , Scale , Multiplier , Offset ]
    /// </summary>
    public struct UnitStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Kind;
        /// <summary>
        /// 
        /// </summary>
        public int Exponent;
        /// <summary>
        /// 
        /// </summary>
        public int Scale;
        /// <summary>
        /// 
        /// </summary>
        public double Multiplier;
        /// <summary>
        /// 
        /// </summary>
        public double Offset;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="exponent"></param>
        /// <param name="scale"></param>
        /// <param name="multiplier"></param>
        /// <param name="offset"></param>
        public UnitStruct(
            string kind,
            int exponent,
            int scale,
            double multiplier,
            double offset)
        {
            this.Kind = kind;
            this.Exponent = exponent;
            this.Scale = scale;
            this.Multiplier = multiplier;
            this.Offset = offset;
        }
    }

    /// <summary>
    /// [ Name , Variable , Coefficient ]
    /// </summary>
    public class VariableReferenceStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public string Variable;
        /// <summary>
        /// 
        /// </summary>
        public int Coefficient;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variable"></param>
        /// <param name="coefficient"></param>
        public VariableReferenceStruct(
            string name,
            string variable,
            int coefficient)
        {
            this.Name = name;
            this.Variable = variable;
            this.Coefficient = coefficient;
        }
    }

    /// <summary>
    /// InitialAssignment
    /// </summary>
    public struct InitialAssignmentStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public double Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public InitialAssignmentStruct(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
