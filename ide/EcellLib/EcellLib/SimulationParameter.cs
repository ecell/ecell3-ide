using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using EcellCoreLib;

namespace EcellLib
{
    /// <summary>
    /// Managed class for the simulation parameters.
    /// </summary>
    public class SimulationParameter
    {
        List<EcellObject> m_steppers;
        Dictionary<string, Dictionary<string, Dictionary<string, double>>> m_initialConditions;
        LoggerPolicy m_loggerPolicy;
        string m_id;

        /// <summary>
        /// get the list of steppers.
        /// </summary>
        public List<EcellObject> Steppers
        {
            get
            {
                return m_steppers;
            }
        }

        /// <summary>
        /// get the dictionary of initial parameters.
        /// </summary>
        public Dictionary<string, Dictionary<string, Dictionary<string, double>>> InitialConditions
        {
            get
            {
                return m_initialConditions;

            }
        }

        /// <summary>
        /// get the logging policy.
        /// </summary>
        public LoggerPolicy LoggerPolicy
        {
            get
            {
                return m_loggerPolicy;
            }
        }

        /// <summary>
        /// get the simulation parameter ID.
        /// </summary>
        public string ID
        {
            get
            {
                return m_id;
            }
        }

        /// <summary>
        /// Constructor for the parameters of simulation.
        /// </summary>
        /// <param name="steppers">the list of stepper.</param>
        /// <param name="initialConditions">the dictionary of the initial parameters.</param>
        /// <param name="loggerPolicy">the logging policy.</param>
        /// <param name="id">the simulation ID.</param>
        public SimulationParameter(List<EcellObject> steppers,
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> initialConditions,
                LoggerPolicy loggerPolicy,
                string id)
        {
            m_steppers = steppers;
            m_initialConditions = initialConditions;
            m_loggerPolicy = loggerPolicy;
            m_id = id;
        }
    }

    /// <summary>
    /// Internal exception class for SimulationParameter.
    /// </summary>
    internal class SimulationParameterParseException : EcellXmlReaderException
    {
        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="msg">the error message.</param>
        public SimulationParameterParseException(string msg)
            : base(msg)
        {
        }
    }

    /// <summary>
    /// Internal I/O class to read/write the simulation parameter file.
    /// </summary>
    internal class SimulationParameterWriter : EcellXmlWriter
    {
        /// <summary>
        /// Write the initial parameters.
        /// </summary>
        /// <param name="l_initialCondition">the list of initial condition.</param>
        private void WriteInitialConditionElement(
                Dictionary<string, Dictionary<string, double>> l_initialCondition)
        {
            m_tx.WriteStartElement(Constants.xpathInitialCondition.ToLower());
            //
            // Creates the "System" part.
            //
            m_tx.WriteStartElement(Constants.xpathSystem.ToLower());
            foreach (string l_key in l_initialCondition[Constants.xpathSystem].Keys)
            {
                m_tx.WriteStartElement(Constants.xpathID.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, l_key);
                this.WriteValueElements(
                    new EcellValue(l_initialCondition[Constants.xpathSystem][l_key]), false);
                m_tx.WriteEndElement();
            }
            m_tx.WriteEndElement();
            //
            // Creates the "Process" part.
            //
            m_tx.WriteStartElement(Constants.xpathProcess.ToLower());
            foreach (string l_key in l_initialCondition[Constants.xpathProcess].Keys)
            {
                m_tx.WriteStartElement(Constants.xpathID.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, l_key);
                this.WriteValueElements(
                    new EcellValue(l_initialCondition[Constants.xpathProcess][l_key]), false);
                m_tx.WriteEndElement();
            }
            m_tx.WriteEndElement();
            //
            // Creates the "Variable part.
            //
            m_tx.WriteStartElement(Constants.xpathVariable.ToLower());
            foreach (string l_key in l_initialCondition[Constants.xpathVariable].Keys)
            {
                m_tx.WriteStartElement(Constants.xpathID.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, l_key);
                this.WriteValueElements(
                        new EcellValue(l_initialCondition[Constants.xpathVariable][l_key]), false);
                m_tx.WriteEndElement();
            }
            m_tx.WriteEndElement();
            //
            // Closes
            //
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Creates the "Stepper" elements.
        /// </summary>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        private void WriteStepperElements(EcellObject l_ecellObject)
        {
            m_tx.WriteStartElement(Constants.xpathStepper.ToLower());
            m_tx.WriteAttributeString(Constants.xpathClass, null, l_ecellObject.Classname);
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, l_ecellObject.Key);

            if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
            {
                foreach (EcellData l_ecellData in l_ecellObject.Value)
                {
                    if (l_ecellData == null
                        || !l_ecellData.Settable
                        || !l_ecellData.Loadable
                        || l_ecellData.Value == null
                        || (l_ecellData.Value.IsString() &&
                            l_ecellData.Value.CastToString().Length <= 0))
                        continue;
                    m_tx.WriteStartElement(Constants.xpathProperty.ToLower());
                    m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, l_ecellData.Name);
                    WriteValueElements(l_ecellData.Value, false);
                    m_tx.WriteEndElement();
                }
            }
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Creates the "LoggerPolicy" elements.
        /// </summary>
        /// <param name="l_loggerPolicy">The "LoggerPolicy"</param>
        private void WriteLoggerPolicyElement(LoggerPolicy l_loggerPolicy)
        {
            m_tx.WriteStartElement(Constants.xpathLoggerPolicy.ToLower());
            m_tx.WriteElementString(
                Constants.xpathStep.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_reloadStepCount + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Constants.xpathInterval.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_reloadInterval + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Constants.xpathAction.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_diskFullAction + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Constants.xpathSpace.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_maxDiskSpace + System.Environment.NewLine
                );
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Write the property of stepper.
        /// </summary>
        /// <param name="l_modelID">the model ID.</param>
        /// <param name="l_stepperList">the list of stepper.</param>
        /// <param name="l_initialCondition">the dictionary of initial parameters.</param>
        public void WriteSteppers(string l_modelID,
                List<EcellObject> l_stepperList,
                Dictionary<string, Dictionary<string, double>> l_initialCondition)
        {
            m_tx.WriteStartElement(Constants.xpathModel.ToLower());
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, l_modelID);
            foreach (EcellObject l_stepper in l_stepperList)
            {
                WriteStepperElements(l_stepper);
            }
            WriteInitialConditionElement(l_initialCondition);
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Write the key to start to write the simulation parameters.
        /// </summary>
        public void WriteStartDocument()
        {
            m_tx.WriteStartDocument(true);
            m_tx.WriteStartElement(Constants.xpathPrm.ToLower());
        }

        /// <summary>
        /// Write the key to end to write the simulation parameters.
        /// </summary>
        public void WriteEndDocument()
        {
            m_tx.WriteEndElement();
            m_tx.WriteEndDocument();
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="tx"></param>
        public SimulationParameterWriter(XmlTextWriter tx)
            : base(tx)
        {
        }

        /// <summary>
        /// Creates the parameter file.
        /// </summary>
        /// <param name="l_fileName">The parameter file name</param>
        /// <param name="sp">The simulation parameter</param>
        public static void Create(string l_fileName, SimulationParameter sp)
        {
            //
            // Checks the old model file.
            //
            if (File.Exists(l_fileName))
            {
                string l_date
                    = File.GetLastAccessTime(l_fileName).ToString().Replace(
                        Constants.delimiterColon, Constants.delimiterUnderbar);
                l_date = l_date.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                l_date = l_date.Replace(Constants.delimiterSpace, Constants.delimiterUnderbar);
                string l_destFileName
                    = Path.GetDirectoryName(l_fileName) + Constants.delimiterPath
                    + Constants.delimiterUnderbar + l_date + Constants.delimiterUnderbar + Path.GetFileName(l_fileName);
                File.Move(l_fileName, l_destFileName);
            }
            //
            // Saves the model
            //
            XmlTextWriter l_writer = new XmlTextWriter(l_fileName, System.Text.Encoding.UTF8);
            try
            {
                l_writer.Formatting = Formatting.Indented;
                l_writer.Indentation = 0;
                SimulationParameterWriter pw = new SimulationParameterWriter(l_writer);
                pw.WriteStartDocument();
                //
                // Divide stepper list into per-model lists;
                //
                Dictionary<string, List<EcellObject>> l_dic = new Dictionary<string, List<EcellObject>>();
                foreach (EcellObject l_stepper in sp.Steppers)
                {
                    if (l_stepper.Type.Equals(Constants.xpathStepper))
                    {
                        if (!l_dic.ContainsKey(l_stepper.ModelID))
                        {
                            l_dic[l_stepper.ModelID] = new List<EcellObject>();
                        }
                        l_dic[l_stepper.ModelID].Add(l_stepper);
                    }
                }

                foreach (string l_modelID in l_dic.Keys)
                {
                    pw.WriteSteppers(l_modelID, l_dic[l_modelID], sp.InitialConditions[l_modelID]);
                }
                pw.WriteLoggerPolicyElement(sp.LoggerPolicy);
                pw.WriteEndDocument();
            }
            finally
            {
                l_writer.Close();
            }
        }
    }

    /// <summary>
    /// Internal class to read the simulation parameter.
    /// </summary>
    internal class SimulationParameterReader : EcellXmlReader
    {
        private XmlDocument m_doc;

        private WrappedSimulator m_simulator;

        private string m_parameterID;

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="doc">document object.</param>
        /// <param name="sim">simulation engine.</param>
        /// <param name="parameterID">simulation parameters,</param>
        public SimulationParameterReader(XmlDocument doc, WrappedSimulator sim, string parameterID)
        {
            m_doc = doc;
            m_simulator = sim;
            m_parameterID = parameterID;
        }

        /// <summary>
        /// Parse the simulation parameter file to extract the initial parameters.
        /// </summary>
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_node">XML node.</param>
        private Dictionary<string, Dictionary<string, double>> ParseInitialCondition(
                string l_modelID, XmlNode l_node)
        {
            Dictionary<string, Dictionary<string, double>> l_initialCondition = new Dictionary<string, Dictionary<string, double>>();
            foreach (XmlNode l_nodeType in l_node.ChildNodes)
            {
                if (!this.IsValidNode(l_nodeType))
                {
                    continue;
                }
                string l_type = null;
                if (l_nodeType.Name.Equals(Constants.xpathSystem.ToLower()))
                {
                    l_type = Constants.xpathSystem;
                }
                else if (l_nodeType.Name.Equals(Constants.xpathProcess.ToLower()))
                {
                    l_type = Constants.xpathProcess;
                }
                else if (l_nodeType.Name.Equals(Constants.xpathVariable.ToLower()))
                {
                    l_type = Constants.xpathVariable;
                }
                else
                {
                    continue;
                }
                l_initialCondition[l_type] = new Dictionary<string, double>();
                foreach (XmlNode l_nodeID in l_nodeType.ChildNodes)
                {
                    if (!this.IsValidNode(l_nodeID))
                    {
                        continue;
                    }
                    XmlNode l_nodeName = l_nodeID.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                    if (!this.IsValidNode(l_nodeName))
                    {
                        continue;
                    }
                    try
                    {
                        l_initialCondition[l_type][l_nodeName.InnerText]
                            = XmlConvert.ToDouble(l_nodeID.InnerText);
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
            }
            if (!l_initialCondition.ContainsKey(Constants.xpathSystem))
            {
                l_initialCondition[Constants.xpathSystem] = new Dictionary<string, double>();
            }
            if (!l_initialCondition.ContainsKey(Constants.xpathProcess))
            {
                l_initialCondition[Constants.xpathProcess] = new Dictionary<string, double>();
            }
            if (!l_initialCondition.ContainsKey(Constants.xpathVariable))
            {
                l_initialCondition[Constants.xpathVariable] = new Dictionary<string, double>();
            }
            return l_initialCondition;
        }

        /// <summary>
        /// Parses the "LoggerPolicy" node.
        /// </summary>
        /// <param name="l_node">The "LoggerPolicy" node</param>
        private LoggerPolicy ParseLoggerPolicy(XmlNode l_node)
        {
            int l_step = -1;
            double l_interval = -1.0;
            int l_action = -1;
            int l_diskSpace = -1;
            foreach (XmlNode l_childNode in l_node.ChildNodes)
            {
                if (l_childNode.Name.Equals(Constants.xpathStep.ToLower()))
                {
                    l_step = XmlConvert.ToInt32(l_childNode.InnerText);
                }
                else if (l_childNode.Name.Equals(Constants.xpathInterval.ToLower()))
                {
                    l_interval = XmlConvert.ToDouble(l_childNode.InnerText);
                }
                else if (l_childNode.Name.Equals(Constants.xpathAction.ToLower()))
                {
                    l_action = XmlConvert.ToInt32(l_childNode.InnerText);
                }
                else if (l_childNode.Name.Equals(Constants.xpathSpace.ToLower()))
                {
                    l_diskSpace = XmlConvert.ToInt32(l_childNode.InnerText);
                }
            }

            // validate values
            if (l_step < 0)
            {
                throw new InvalidDataException("step interval is not in the valid ranga");
            }

            if (l_interval < 0.0)
            {
                throw new InvalidDataException("time interval is not in the valid ranga");
            }

            if (l_action < 0 || l_action > 1)
            {
                throw new InvalidDataException("action value should be either 0 or 1");
            }

            if (l_diskSpace < 0)
            {
                throw new InvalidDataException("maximum disk usage should be greater than 0");
            }

            return new LoggerPolicy(l_step, l_interval, l_action, l_diskSpace);
        }

        /// <summary>
        /// Parses the "Stepper" node.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_stepper">The "Stepper" node</param>
        private EcellObject ParseStepper(string l_modelID, XmlNode l_stepper)
        {
            XmlNode l_stepperClass = l_stepper.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode l_stepperID = l_stepper.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(l_stepperClass) || !this.IsValidNode(l_stepperID))
            {
                throw new SimulationParameterParseException("Invalid stepper node found");
            }

            try
            {
                m_simulator.CreateStepper(l_stepperClass.InnerText, l_stepperID.InnerText);
            }
            catch (Exception e)
            {
                throw new Exception(
                    String.Format(
                        "Could not create {0}",
                        new object[] { l_stepperClass.InnerText }),
                    e
                );
            }

            List<EcellData> l_ecellDataList = new List<EcellData>();
            XmlNodeList l_stepperPropertyList = l_stepper.ChildNodes;
            foreach (XmlNode l_stepperProperty in l_stepperPropertyList)
            {
                if (!l_stepperProperty.Name.Equals(Constants.xpathProperty))
                {
                    continue;
                }
                XmlNode l_stepperPropertyName = l_stepperProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(l_stepperPropertyName))
                {
                    continue;
                }
                EcellValue l_ecellValue = this.GetValueList(l_stepperProperty);
                if (l_ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
                    m_simulator.LoadStepperProperty(
                        l_stepperID.InnerText,
                        l_stepperPropertyName.InnerText,
                        EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue));
                    EcellData l_ecellData = new EcellData(
                            l_stepperPropertyName.InnerText, l_ecellValue, l_stepperPropertyName.InnerText);
                    l_ecellData.Gettable = true;
                    l_ecellData.Loadable = false;
                    l_ecellData.Saveable = false;
                    l_ecellData.Settable = true;
                    l_ecellDataList.Add(l_ecellData);
                }
            }

            //
            // 4 "EcellLib"
            //
            return EcellObject.CreateObject(
                l_modelID,
                l_stepperID.InnerText,
                Constants.xpathStepper,
                l_stepperClass.InnerText,
                l_ecellDataList);
        }

        /// <summary>
        /// Parse the initial parameters that this object is managed.
        /// </summary>
        /// <returns></returns>
        public SimulationParameter Parse()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_initialConditions = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            List<EcellObject> l_stepperList = new List<EcellObject>();

            //
            // Parses the "Stepper".
            //
            XmlNodeList l_modelList = m_doc.SelectNodes("/" + Constants.xpathPrm.ToLower() + "/" + Constants.xpathModel.ToLower());
            foreach (XmlNode l_model in l_modelList)
            {
                XmlNode l_modelID = l_model.Attributes.GetNamedItem(Constants.xpathID.ToLower());
                if (!this.IsValidNode(l_modelID))
                    throw new SimulationParameterParseException("Invalid model node found");

                foreach (XmlNode l_child in l_model.ChildNodes)
                {
                    if (l_child.Name.Equals(Constants.xpathStepper.ToLower()))
                    {
                        l_stepperList.Add(ParseStepper(l_modelID.InnerText, l_child));
                    }
                    else if (l_child.Name.Equals(Constants.xpathInitialCondition.ToLower()))
                    {
                        l_initialConditions[l_modelID.InnerText] = ParseInitialCondition(
                                l_modelID.InnerText, l_child);
                    }
                }
            }

            //
            // Parses the "LoggerPolicy"
            //
            LoggerPolicy l_loggerPolicy = ParseLoggerPolicy(
                    m_doc.SelectSingleNode("/" + Constants.xpathPrm.ToLower() + "/" + Constants.xpathLoggerPolicy.ToLower()));

            return new SimulationParameter(
                l_stepperList,
                l_initialConditions,
                l_loggerPolicy,
                m_parameterID);
        }

        /// <summary>
        /// Parses the simulation parameter file.
        /// </summary>
        /// <param name="l_fileName">The simulation parameter file name</param>
        /// <param name="sim">The simulator.</param>
        public static SimulationParameter Parse(string l_fileName, WrappedSimulator sim)
        {
            XmlDocument l_doc = new XmlDocument();
            l_doc.Load(l_fileName);
            return new SimulationParameterReader(l_doc, sim, Path.GetFileNameWithoutExtension(l_fileName)).Parse();
        }
    }
}
