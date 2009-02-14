using System;
using System.Collections.Generic;
using System.Text;
using libsbml;
using Ecell.Exceptions;

namespace Ecell.SBML
{
    internal class SBML_Model
    {
        public long Level;
        public long Version;

        public Dictionary<string, float> CompartmentSize;
        public Dictionary<string, string> CompartmentUnit;
        public Dictionary<string, string> FunctionDefinition;

        public List<CompartmentStruct> CompartmentList;
        public List<EventStruct> EventList;
        public List<FunctionDefinitionStruct> FunctionDefinitionList;
        public List<ParameterStruct> ParameterList;
        public List<ReactionStruct> ReactionList;
        public List<RuleStruct> RuleList;
        public List<SpeciesStruct> SpeciesList;
        public List<UnitDefinitionStruct> UnitDefinitionList;

        public SBML_Model(SBMLDocument aSBMLDocument, Model aSBMLmodel)
        {
            this.CompartmentSize = new Dictionary<string,float>();
            this.CompartmentUnit = new Dictionary<string,string>();
            this.FunctionDefinition = new Dictionary<string, string>();

            this.Level = aSBMLDocument.getLevel();
            this.Version = aSBMLDocument.getVersion() ;

            this.CompartmentList = SbmlFunctions.getCompartment(aSBMLmodel);
            this.EventList = SbmlFunctions.getEvent(aSBMLmodel);
            this.FunctionDefinitionList = SbmlFunctions.getFunctionDefinition(aSBMLmodel);
            this.ParameterList = SbmlFunctions.getParameter(aSBMLmodel);
            this.ReactionList = SbmlFunctions.getReaction(aSBMLmodel, aSBMLDocument);
            this.RuleList = SbmlFunctions.getRule(aSBMLmodel);
            this.SpeciesList = SbmlFunctions.getSpecies(aSBMLmodel);
            this.UnitDefinitionList = SbmlFunctions.getUnitDefinition(aSBMLmodel);

            this.setFunctionDefinitionToDictionary();
        }

        private void setFunctionDefinitionToDictionary()
        {
            if ( this.FunctionDefinitionList == null || this.FunctionDefinitionList.Count <= 0 )
                return;

            foreach (FunctionDefinitionStruct aFunctionDefinition in this.FunctionDefinitionList)
            {
                this.FunctionDefinition[aFunctionDefinition.ID ] = aFunctionDefinition.Formula;
            }
        }

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
