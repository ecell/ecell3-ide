using System;
using System.Collections.Generic;
using System.Text;
using Ecell;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Simulation
{
    internal class KeyValuePairConverter<Tkey, Tvalue>
    {
        public static MutableKeyValuePair<Tkey, Tvalue> Convert(KeyValuePair<Tkey, Tvalue> orig)
        {
            return new MutableKeyValuePair<Tkey, Tvalue>(orig.Key, orig.Value);
        }
    }

    internal class MutableKeyValuePair<Tkey, Tvalue>
    {
        public Tkey Key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        public Tvalue Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public MutableKeyValuePair()
        {
        }

        public MutableKeyValuePair(Tkey key, Tvalue value)
        {
            m_key = key;
            m_value = value;
        }

        private Tkey m_key;
        private Tvalue m_value;
    }

    internal class PerTypeInitialConditions: ICloneable
    {
        List<MutableKeyValuePair<string, double>> m_variableInitialConds;
        List<MutableKeyValuePair<string, double>> m_processInitialConds;

        public List<MutableKeyValuePair<string, double>> VariableInitialConditions
        {
            get { return m_variableInitialConds; }
        }

        public List<MutableKeyValuePair<string, double>> ProcessInitialConditions
        {
            get { return m_processInitialConds; }
        }

        public object Clone()
        {
            return new PerTypeInitialConditions(this);
        }

        public PerTypeInitialConditions()
        {
            m_variableInitialConds = new List<MutableKeyValuePair<string, double>>();
            m_processInitialConds = new List<MutableKeyValuePair<string, double>>();
        }

        public PerTypeInitialConditions(PerTypeInitialConditions rhs)
        {
            m_variableInitialConds = new List<MutableKeyValuePair<string, double>>();
            m_variableInitialConds.AddRange(rhs.m_variableInitialConds);
            m_processInitialConds = new List<MutableKeyValuePair<string, double>>();
            m_processInitialConds.AddRange(rhs.m_processInitialConds);
        }
    }

    internal class StepperConfiguration: ICloneable
    {
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string ClassName
        {
            get { return m_className; }
            set { m_className = value; }
        }

        public List<MutableKeyValuePair<string, string>> Properties
        {
            get { return m_properties; }
        }

        public StepperConfiguration()
        {
            m_properties = new List<MutableKeyValuePair<string,string>>();
        }

        public StepperConfiguration(StepperConfiguration rhs)
        {
            m_name = rhs.Name;
            m_className = rhs.ClassName;
            m_properties = new List<MutableKeyValuePair<string, string>>(rhs.Properties);
        }

        public object Clone()
        {
            return new StepperConfiguration(this);
        }

        private string m_name;
        private string m_className;
        private List<MutableKeyValuePair<string, string>> m_properties;
    }

    internal class PerModelSimulationParameter: ICloneable
    {
        public string ModelID
        {
            get { return m_modelID; }
            set { m_modelID = value; }
        }

        public PerTypeInitialConditions PerTypeInitialConditions
        {
            get { return m_perTypeInitialConds; }
            set
            {
                m_perTypeInitialConds = value;
            }
        }

        public IList<StepperConfiguration> Steppers
        {
            get { return m_steppers; }
        }

        public object Clone()
        {
            return new PerModelSimulationParameter(this);
        }

        public PerModelSimulationParameter(string modelID)
        {
            m_modelID = modelID;
            m_perTypeInitialConds = new PerTypeInitialConditions();
            m_steppers = new List<StepperConfiguration>();
        }

        public PerModelSimulationParameter(PerModelSimulationParameter rhs)
        {
            m_modelID = rhs.ModelID;
            m_perTypeInitialConds = new PerTypeInitialConditions(rhs.PerTypeInitialConditions);
            m_steppers = new List<StepperConfiguration>(rhs.Steppers);
        }

        string m_modelID;
        PerTypeInitialConditions m_perTypeInitialConds;
        List<StepperConfiguration> m_steppers;
    }

    internal class SimulationParameterSet
    {
        private string m_name;
        private LoggerPolicy m_loggerPol;
        private List<PerModelSimulationParameter> m_perModelParams;

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public LoggerPolicy LoggerPolicy
        {
            get { return m_loggerPol; }
            set { m_loggerPol = value; }
        }

        public IList<PerModelSimulationParameter> PerModelSimulationParameters
        {
            get { return m_perModelParams; }
        }

        public SimulationParameterSet(string name)
        {
            m_loggerPol = new LoggerPolicy();
            m_name = name;
            m_perModelParams = new List<PerModelSimulationParameter>();
        }
    }
}
