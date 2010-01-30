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
    /// 
    /// </summary>
    public class SBML_Model
    {
        /// <summary>
        /// 
        /// </summary>
        public long Level;
        /// <summary>
        /// 
        /// </summary>
        public long Version;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, double> CompartmentSize;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> CompartmentUnit;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> FunctionDefinition;
        /// <summary>
        /// 
        /// </summary>
        public List<CompartmentStruct> CompartmentList;
        /// <summary>
        /// 
        /// </summary>
        public List<EventStruct> EventList;
        /// <summary>
        /// 
        /// </summary>
        public List<FunctionDefinitionStruct> FunctionDefinitionList;
        /// <summary>
        /// 
        /// </summary>
        public List<ParameterStruct> ParameterList;
        /// <summary>
        /// 
        /// </summary>
        public List<ReactionStruct> ReactionList;
        /// <summary>
        /// 
        /// </summary>
        public List<RuleStruct> RuleList;
        /// <summary>
        /// 
        /// </summary>
        public List<SpeciesStruct> SpeciesList;
        /// <summary>
        /// 
        /// </summary>
        public List<UnitDefinitionStruct> UnitDefinitionList;
        /// <summary>
        /// 
        /// </summary>
        public List<InitialAssignmentStruct> InitialAssignmentList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSBMLmodel"></param>
        public SBML_Model(Model aSBMLmodel)
        {
            this.CompartmentSize = new Dictionary<string,double>();
            this.CompartmentUnit = new Dictionary<string,string>();
            this.FunctionDefinition = new Dictionary<string, string>();

            this.Level = aSBMLmodel.getLevel();
            this.Version = aSBMLmodel.getVersion();

            this.CompartmentList = SbmlFunctions.getCompartment(aSBMLmodel);
            this.EventList = SbmlFunctions.getEvent(aSBMLmodel);
            this.FunctionDefinitionList = SbmlFunctions.getFunctionDefinition(aSBMLmodel);
            this.ParameterList = SbmlFunctions.getParameter(aSBMLmodel);
            this.ReactionList = SbmlFunctions.getReaction(aSBMLmodel);
            this.RuleList = SbmlFunctions.getRule(aSBMLmodel);
            this.SpeciesList = SbmlFunctions.getSpecies(aSBMLmodel);
            this.UnitDefinitionList = SbmlFunctions.getUnitDefinition(aSBMLmodel);
            this.InitialAssignmentList = SbmlFunctions.getInitialAssignments(aSBMLmodel);
            this.setFunctionDefinitionToDictionary();
        }
        /// <summary>
        /// 
        /// </summary>
        private void setFunctionDefinitionToDictionary()
        {
            if ( this.FunctionDefinitionList == null || this.FunctionDefinitionList.Count <= 0 )
                return;

            foreach (FunctionDefinitionStruct aFunctionDefinition in this.FunctionDefinitionList)
            {
                this.FunctionDefinition[aFunctionDefinition.ID ] = aFunctionDefinition.Formula;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aCompartmentID"></param>
        /// <returns></returns>
        public string getPath(string aCompartmentID)
        {
            if( aCompartmentID == "default" )
                return "/";
            string aPath;
            if ( this.Level == 1 )
            {
                foreach (CompartmentStruct aCompartment in this.CompartmentList)
                {
                    if ( aCompartment.Name != aCompartmentID )
                        continue;

                    if (aCompartment.Outside == "" || aCompartment.Outside == "default")
                        aPath = "/" + aCompartmentID;
                    else
                        aPath = this.getPath(aCompartment.Outside) + '/' + aCompartmentID;
                    return aPath;
                }
            }
            else if( this.Level == 2 )
            {
                foreach (CompartmentStruct aCompartment in this.CompartmentList)
                {
                    if( aCompartment.ID != aCompartmentID )
                        continue;
                    if (aCompartment.Outside == "" || aCompartment.Outside == "default")
                        aPath = "/" + aCompartmentID;
                    else
                        aPath = this.getPath(aCompartment.Outside) + '/' + aCompartmentID;
                    return aPath;
                }
            }
            else
            {
                throw new EcellException("Version"+ this.Level.ToString() +" ????");
            }
            throw new EcellException("Cannot get path");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSpeciesID"></param>
        /// <returns></returns>
        public string getSpeciesReferenceID(string aSpeciesID)
        {
            if ( this.Level == 1 )
            {
                foreach (SpeciesStruct aSpecies in this.SpeciesList)
                    if ( aSpecies.Name == aSpeciesID )
                        return this.getPath( aSpecies.Compartment ) + ":" + aSpeciesID;
            }
            else if (this.Level == 2)
            {
                foreach(SpeciesStruct aSpecies in this.SpeciesList)
                    if ( aSpecies.ID == aSpeciesID )
                        return this.getPath( aSpecies.Compartment ) + ":" + aSpeciesID;
            }
            else
            {
                throw new EcellException("Version"+ this.Level.ToString() +" ????");
            }
            throw new EcellException("Cannot find " + aSpeciesID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aValueUnit"></param>
        /// <param name="aValue"></param>
        /// <returns></returns>
        public double convertUnit(string aValueUnit, double aValue )
        {
            List<double> newValue = new List<double>();
            if (this.Level == 1)
            {
                foreach (UnitDefinitionStruct unitList in this.UnitDefinitionList)
                {
                    if ( unitList.Name != aValueUnit )
                        continue;

                    foreach(UnitStruct anUnit in unitList.Units)
                        aValue = aValue * this.getNewUnitValue( anUnit );

                    newValue.Add( aValue );
                }
            }
            else if (this.Level == 2)
            {
                foreach (UnitDefinitionStruct unitList in this.UnitDefinitionList)
                {
                    if ( unitList.ID != aValueUnit )
                        continue;
                    foreach (UnitStruct anUnit in unitList.Units)
                        aValue = aValue * this.getNewUnitValue( anUnit );
                    newValue.Add( aValue );
                }
            }
            else
            {
                throw new EcellException("Version"+ this.Level.ToString() +" ????");
            }

            if (newValue.Count <= 0)
                return aValue;
            else
                return newValue[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anUnit"></param>
        /// <returns></returns>
        private double getNewUnitValue(UnitStruct anUnit)
        {
            double aValue = 1;

            // Scale
            if ( anUnit.Scale != 0 )
                aValue = aValue * Math.Pow( 10, anUnit.Scale );

            // Multiplier
            aValue = aValue * anUnit.Multiplier;

            // Exponent
            aValue = Math.Pow( aValue, anUnit.Exponent );

            // Offset
            aValue = aValue + anUnit.Offset;

            return aValue;
        }
    }
}
