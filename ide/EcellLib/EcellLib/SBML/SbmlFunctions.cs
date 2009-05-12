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

namespace Ecell.SBML
{
    /// <summary>
    /// static functions which parse SBML to structs for converter.
    /// </summary>
    internal class SbmlFunctions
    {
        /// <summary>
        /// [ CompartmentStruct ]
        /// [[ Id , Name , SpatialDimension , Size , Volume , Unit , Ouside , Constant ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        internal static List<CompartmentStruct> getCompartment(Model aSBMLmodel)
        {
            List<CompartmentStruct> list = new List<CompartmentStruct>();
            ListOfCompartments compartments = aSBMLmodel.getListOfCompartments();
            for (int i = 0; i < compartments.size(); i++ )
            {
                Compartment item = aSBMLmodel.getCompartment(i);
                string anId = item.getId();
                string aName = item.getName();
                long aSpatialDimension = item.getSpatialDimensions();
                double aSize;
                if (item.isSetSize())
                    aSize = item.getSize();
                else
                    aSize = double.NaN;
                double aVolume;
                if (item.isSetVolume())
                    aVolume = item.getVolume();
                else
                    aVolume = double.NaN;

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
        /// [ EventStruct ]
        ///  [[ Id , Name , StringTrigger , StringDelay , TimeUnit , [[ VariableAssignment , StringAssignment ]] ]] 
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        internal static List<EventStruct> getEvent(Model aSBMLmodel)
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
        internal static List<FunctionDefinitionStruct> getFunctionDefinition(Model aSBMLmodel)
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
        internal static List<ParameterStruct> getParameter(Model aSBMLmodel)
        {
            List<ParameterStruct> list = new List<ParameterStruct>();

            ListOfParameters parameters = aSBMLmodel.getListOfParameters();
            for (int i = 0; i < parameters.size(); i++ )
            {
                Parameter aParameter = aSBMLmodel.getParameter(i);

                string anId_Pa = aParameter.getId();
                string aName_Pa = aParameter.getName();
                double aValue_Pa;
                if( aParameter.isSetValue())
                    aValue_Pa = aParameter.getValue();
                else
                    aValue_Pa = double.NaN;
                    
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
        /// [ ReactionStruct ]
        /// [[ Id , Name , [ KineticLawStruct ] , Reversible , Fast , [ ReactantStruct ] , [ ProductStruct ] , [ ModifierSpecies ] ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <param name="aSBMLDocument"></param>
        /// <returns></returns>
        internal static List<ReactionStruct> getReaction(Model aSBMLmodel, SBMLDocument aSBMLDocument)
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
                        if( aSBMLDocument.getLevel() == 1 )
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

                    string aString_R = null;
                    if (aSpeciesReference.isSetStoichiometryMath())
                    {
                        ASTNode anASTNode_R = aSpeciesReference.getStoichiometryMath().getMath();
                        aString_R = libsbml.libsbml.formulaToString(anASTNode_R );
                    }

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

                    string aString_P = null;
                    if (aSpeciesReference.isSetStoichiometryMath())
                    {
                        ASTNode anASTNode_P = aSpeciesReference.getStoichiometryMath().getMath();
                        aString_P = libsbml.libsbml.formulaToString( anASTNode_P );
                    }

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
        /// [ RuleStruct ]
        /// [[ RuleType, Formula, Variable ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        internal static List<RuleStruct> getRule(Model aSBMLmodel)
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
        internal static List<SpeciesStruct> getSpecies(Model aSBMLmodel)
        {
            List<SpeciesStruct> list = new List<SpeciesStruct>();

            ListOfSpecies listOfSpecies = aSBMLmodel.getListOfSpecies();
            for (int i = 0; i < listOfSpecies.size(); i++ )
            {
                Species aSpecies = aSBMLmodel.getSpecies(i);

                string anId_Sp = aSpecies.getId();
                string aName_Sp = aSpecies.getName();
                string aCompartment_Sp = aSpecies.getCompartment();

                double anInitialAmount_Sp;
                if (aSpecies.isSetInitialAmount())
                    anInitialAmount_Sp = aSpecies.getInitialAmount();
                else
                    anInitialAmount_Sp = double.NaN;

                double anInitialConcentration_Sp;
                if (aSpecies.isSetInitialConcentration())
                    anInitialConcentration_Sp = aSpecies.getInitialConcentration();
                else
                    anInitialConcentration_Sp = double.NaN;
                    
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
        /// [ UnitDefinitionStruct ]
        /// [[ Id , Name , [ UnitStruct ] ]]
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        /// <returns></returns>
        internal static List<UnitDefinitionStruct> getUnitDefinition(Model aSBMLmodel)
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
    }

    /// <summary>
    /// [ Id , Name , SpatialDimension , Size , Volume , Unit , Ouside , Constant ]
    /// </summary>
    internal struct CompartmentStruct
    {
        public string ID;
        public string Name;
        public long SpatialDimension;
        public double Size;
        public double Volume;
        public string Unit;
        public string Outside;
        public bool Constant;
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
    internal struct EventStruct
    {
        public string ID;
        public string Name;
        public string Trigger;
        public string Delay;
        public string TimeUnits;
        public List<EventAssignmentStruct> EventAssignments;

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
    internal struct EventAssignmentStruct
    {
        public string Variable;
        public string Formula;
        public EventAssignmentStruct(string variable, string formula)
        {
            this.Variable = variable;
            this.Formula = formula;
        }
    }

    /// <summary>
    /// [ Id , Name , StringFormula ]
    /// </summary>
    internal struct FunctionDefinitionStruct
    {
        public string ID;
        public string Name;
        public string Formula;

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
    internal struct ParameterStruct
    {
        public string ID;
        public string Name;
        public double Value;
        public string Unit;
        public bool Constant;
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
    internal struct ReactionStruct
    {
        public string ID;
        public string Name;
        public List<KineticLawStruct> KineticLaws;
        public bool Reversible;
        public bool Fast;
        public List<ReactantStruct> Reactants;
        public List<ProductStruct> Products;
        public List<string> Modifiers;

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
    internal struct KineticLawStruct
    {
        public string Formula;
        public List<string> MathList;
        public string TimeUnit;
        public string SubstanceUnit;
        public List<ParameterStruct> Parameters;
        public XMLNode ExpressionAnnotation;

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
    internal struct ReactantStruct
    {
        public string Species;
        public double Stoichiometry;
        public string Formula;
        public int Denominator;
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
    internal struct ProductStruct
    {
        public string Species;
        public double Stoichiometry;
        public string Formula;
        public int Denominator;
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
    internal struct RuleStruct
    {
        public int RuleType;
        public string Formula;
        public string Variable;

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
    internal struct SpeciesStruct
    {
        public string ID;
        public string Name;
        public string Compartment;
        public double InitialAmount;
        public double InitialConcentration;
        public string SubstanceUnit;
        public string SpatialSizeUnit;
        public string Unit;
        public bool HasOnlySubstanceUnit;
        public bool BoundaryCondition;
        public int Charge;
        public bool Constant;

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
    internal struct UnitDefinitionStruct
    {
        public string ID;
        public string Name;
        public List<UnitStruct> Units;

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
    internal struct UnitStruct
    {
        public string Kind;
        public int Exponent;
        public int Scale;
        public double Multiplier;
        public double Offset;

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
    internal class VariableReferenceStruct
    {
        public string Name;
        public string Variable;
        public int Coefficient;

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
}
