using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using EcellCoreLib;
using Ecell.Objects;

namespace Ecell
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
        /// <param name="initialCondition">the list of initial condition.</param>
        private void WriteInitialConditionElement(
                Dictionary<string, Dictionary<string, double>> initialCondition)
        {
            m_tx.WriteStartElement(Constants.xpathInitialCondition.ToLower());
            //
            // Creates the "System" part.
            //
            m_tx.WriteStartElement(Constants.xpathSystem.ToLower());
            foreach (string key in initialCondition[Constants.xpathSystem].Keys)
            {
                m_tx.WriteStartElement(Constants.xpathID.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, key);
                this.WriteValueElements(
                    new EcellValue(initialCondition[Constants.xpathSystem][key]), false);
                m_tx.WriteEndElement();
            }
            m_tx.WriteEndElement();
            //
            // Creates the "Process" part.
            //
            m_tx.WriteStartElement(Constants.xpathProcess.ToLower());
            foreach (string key in initialCondition[Constants.xpathProcess].Keys)
            {
                m_tx.WriteStartElement(Constants.xpathID.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, key);
                this.WriteValueElements(
                    new EcellValue(initialCondition[Constants.xpathProcess][key]), false);
                m_tx.WriteEndElement();
            }
            m_tx.WriteEndElement();
            //
            // Creates the "Variable part.
            //
            m_tx.WriteStartElement(Constants.xpathVariable.ToLower());
            foreach (string key in initialCondition[Constants.xpathVariable].Keys)
            {
                m_tx.WriteStartElement(Constants.xpathID.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, key);
                this.WriteValueElements(
                        new EcellValue(initialCondition[Constants.xpathVariable][key]), false);
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
        /// <param name="ecellObject">The "EcellObject"</param>
        private void WriteStepperElements(EcellObject ecellObject)
        {
            m_tx.WriteStartElement(Constants.xpathStepper.ToLower());
            m_tx.WriteAttributeString(Constants.xpathClass, null, ecellObject.Classname);
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, ecellObject.Key);

            if (ecellObject.Value != null && ecellObject.Value.Count > 0)
            {
                foreach (EcellData ecellData in ecellObject.Value)
                {
                    if (ecellData == null
                        || !ecellData.Settable
                        || !ecellData.Loadable
                        || ecellData.Value == null
                        || (ecellData.Value.IsString &&
                            ecellData.Value.CastToString().Length <= 0))
                        continue;
                    m_tx.WriteStartElement(Constants.xpathProperty.ToLower());
                    m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, ecellData.Name);
                    WriteValueElements(ecellData.Value, false);
                    m_tx.WriteEndElement();
                }
            }
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Creates the "LoggerPolicy" elements.
        /// </summary>
        /// <param name="loggerPolicy">The "LoggerPolicy"</param>
        private void WriteLoggerPolicyElement(LoggerPolicy loggerPolicy)
        {
            m_tx.WriteStartElement(Constants.xpathLoggerPolicy.ToLower());
            m_tx.WriteElementString(
                Constants.xpathStep.ToLower(),
                null,
                System.Environment.NewLine + loggerPolicy.ReloadStepCount + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Constants.xpathInterval.ToLower(),
                null,
                System.Environment.NewLine + loggerPolicy.ReloadInterval + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Constants.xpathAction.ToLower(),
                null,
                System.Environment.NewLine + (loggerPolicy.DiskFullAction == DiskFullAction.Terminate? 0 : 1) + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Constants.xpathSpace.ToLower(),
                null,
                System.Environment.NewLine + loggerPolicy.MaxDiskSpace + System.Environment.NewLine
                );
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Write the property of stepper.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        /// <param name="stepperList">the list of stepper.</param>
        /// <param name="initialCondition">the dictionary of initial parameters.</param>
        public void WriteSteppers(string modelID,
                List<EcellObject> stepperList,
                Dictionary<string, Dictionary<string, double>> initialCondition)
        {
            m_tx.WriteStartElement(Constants.xpathModel.ToLower());
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, modelID);
            foreach (EcellObject stepper in stepperList)
            {
                WriteStepperElements(stepper);
            }
            WriteInitialConditionElement(initialCondition);
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
        /// <param name="fileName">The parameter file name</param>
        /// <param name="sp">The simulation parameter</param>
        public static void Create(string fileName, SimulationParameter sp)
        {
            //
            // Checks the old model file.
            //
            if (File.Exists(fileName))
            {
                string date
                    = File.GetLastAccessTime(fileName).ToString().Replace(
                        Constants.delimiterColon, Constants.delimiterUnderbar);
                date = date.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                date = date.Replace(Constants.delimiterSpace, Constants.delimiterUnderbar);
                string destFileName
                    = Path.GetDirectoryName(fileName) + Constants.delimiterPath
                    + Constants.delimiterUnderbar + date + Constants.delimiterUnderbar + Path.GetFileName(fileName);
                File.Move(fileName, destFileName);
            }
            //
            // Saves the model
            //
            XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            try
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 0;
                SimulationParameterWriter pw = new SimulationParameterWriter(writer);
                pw.WriteStartDocument();
                //
                // Divide stepper list into per-model lists;
                //
                Dictionary<string, List<EcellObject>> dic = new Dictionary<string, List<EcellObject>>();
                foreach (EcellObject stepper in sp.Steppers)
                {
                    if (stepper.Type.Equals(Constants.xpathStepper))
                    {
                        if (!dic.ContainsKey(stepper.ModelID))
                        {
                            dic[stepper.ModelID] = new List<EcellObject>();
                        }
                        dic[stepper.ModelID].Add(stepper);
                    }
                }

                foreach (string modelID in dic.Keys)
                {
                    pw.WriteSteppers(modelID, dic[modelID], sp.InitialConditions[modelID]);
                }
                pw.WriteLoggerPolicyElement(sp.LoggerPolicy);
                pw.WriteEndDocument();
            }
            finally
            {
                writer.Close();
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
        /// <param name="modelID">model ID.</param>
        /// <param name="node">XML node.</param>
        private Dictionary<string, Dictionary<string, double>> ParseInitialCondition(
                string modelID, XmlNode node)
        {
            Dictionary<string, Dictionary<string, double>> initialCondition = new Dictionary<string, Dictionary<string, double>>();
            foreach (XmlNode nodeType in node.ChildNodes)
            {
                if (!this.IsValidNode(nodeType))
                {
                    continue;
                }
                string type = null;
                if (nodeType.Name.Equals(Constants.xpathSystem.ToLower()))
                {
                    type = Constants.xpathSystem;
                }
                else if (nodeType.Name.Equals(Constants.xpathProcess.ToLower()))
                {
                    type = Constants.xpathProcess;
                }
                else if (nodeType.Name.Equals(Constants.xpathVariable.ToLower()))
                {
                    type = Constants.xpathVariable;
                }
                else
                {
                    continue;
                }
                initialCondition[type] = new Dictionary<string, double>();
                foreach (XmlNode nodeID in nodeType.ChildNodes)
                {
                    if (!this.IsValidNode(nodeID))
                    {
                        continue;
                    }
                    XmlNode nodeName = nodeID.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                    if (!this.IsValidNode(nodeName))
                    {
                        continue;
                    }
                    try
                    {
                        initialCondition[type][nodeName.InnerText]
                            = XmlConvert.ToDouble(nodeID.InnerText);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex);
                        // do nothing
                    }
                }
            }
            if (!initialCondition.ContainsKey(Constants.xpathSystem))
            {
                initialCondition[Constants.xpathSystem] = new Dictionary<string, double>();
            }
            if (!initialCondition.ContainsKey(Constants.xpathProcess))
            {
                initialCondition[Constants.xpathProcess] = new Dictionary<string, double>();
            }
            if (!initialCondition.ContainsKey(Constants.xpathVariable))
            {
                initialCondition[Constants.xpathVariable] = new Dictionary<string, double>();
            }
            return initialCondition;
        }

        /// <summary>
        /// Parses the "LoggerPolicy" node.
        /// </summary>
        /// <param name="node">The "LoggerPolicy" node</param>
        private LoggerPolicy ParseLoggerPolicy(XmlNode node)
        {
            int step = -1;
            double interval = -1.0;
            int action = -1;
            int diskSpace = -1;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name.Equals(Constants.xpathStep.ToLower()))
                {
                    step = XmlConvert.ToInt32(childNode.InnerText);
                }
                else if (childNode.Name.Equals(Constants.xpathInterval.ToLower()))
                {
                    interval = XmlConvert.ToDouble(childNode.InnerText);
                }
                else if (childNode.Name.Equals(Constants.xpathAction.ToLower()))
                {
                    action = XmlConvert.ToInt32(childNode.InnerText);
                }
                else if (childNode.Name.Equals(Constants.xpathSpace.ToLower()))
                {
                    diskSpace = XmlConvert.ToInt32(childNode.InnerText);
                }
            }

            // validate values
            if (step < 0)
            {
                throw new InvalidDataException("step interval is not in the valid ranga");
            }

            if (interval < 0.0)
            {
                throw new InvalidDataException("time interval is not in the valid ranga");
            }

            if (action < 0 || action > 1)
            {
                throw new InvalidDataException("action value should be either 0 or 1");
            }

            if (diskSpace < 0)
            {
                throw new InvalidDataException("maximum disk usage should be greater than 0");
            }

            return new LoggerPolicy(step, interval,
                action == 0 ?
                    DiskFullAction.Terminate:
                    DiskFullAction.Overwrite,
                diskSpace);
        }

        /// <summary>
        /// Parses the "Stepper" node.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="stepper">The "Stepper" node</param>
        private EcellObject ParseStepper(string modelID, XmlNode stepper)
        {
            XmlNode stepperClass = stepper.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode stepperID = stepper.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(stepperClass) || !this.IsValidNode(stepperID))
            {
                throw new SimulationParameterParseException("Invalid stepper node found");
            }

            try
            {
                m_simulator.CreateStepper(stepperClass.InnerText, stepperID.InnerText);
            }
            catch (Exception e)
            {
                throw new Exception(
                    String.Format(
                        "Could not create {0}",
                        new object[] { stepperClass.InnerText }),
                    e
                );
            }

            List<EcellData> ecellDataList = new List<EcellData>();
            XmlNodeList stepperPropertyList = stepper.ChildNodes;
            foreach (XmlNode stepperProperty in stepperPropertyList)
            {
                if (!stepperProperty.Name.Equals(Constants.xpathProperty))
                {
                    continue;
                }
                XmlNode stepperPropertyName = stepperProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(stepperPropertyName))
                {
                    continue;
                }
                EcellValue ecellValue = this.GetValueList(stepperProperty);
                if (ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
                    m_simulator.LoadStepperProperty(
                        stepperID.InnerText,
                        stepperPropertyName.InnerText,
                        EcellValue.CastToWrappedPolymorph4EcellValue(ecellValue));
                    EcellData ecellData = new EcellData(
                            stepperPropertyName.InnerText, ecellValue, stepperPropertyName.InnerText);
                    ecellData.Gettable = true;
                    ecellData.Loadable = false;
                    ecellData.Saveable = false;
                    ecellData.Settable = true;
                    ecellDataList.Add(ecellData);
                }
            }

            //
            // 4 "EcellLib"
            //
            return EcellObject.CreateObject(
                modelID,
                stepperID.InnerText,
                Constants.xpathStepper,
                stepperClass.InnerText,
                ecellDataList);
        }

        /// <summary>
        /// Parse the initial parameters that this object is managed.
        /// </summary>
        /// <returns></returns>
        public SimulationParameter Parse()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> initialConditions = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            List<EcellObject> stepperList = new List<EcellObject>();

            //
            // Parses the "Stepper".
            //
            XmlNodeList modelList = m_doc.SelectNodes("/" + Constants.xpathPrm.ToLower() + "/" + Constants.xpathModel.ToLower());
            foreach (XmlNode model in modelList)
            {
                XmlNode modelID = model.Attributes.GetNamedItem(Constants.xpathID.ToLower());
                if (!this.IsValidNode(modelID))
                    throw new SimulationParameterParseException("Invalid model node found");

                foreach (XmlNode child in model.ChildNodes)
                {
                    if (child.Name.Equals(Constants.xpathStepper.ToLower()))
                    {
                        stepperList.Add(ParseStepper(modelID.InnerText, child));
                    }
                    else if (child.Name.Equals(Constants.xpathInitialCondition.ToLower()))
                    {
                        initialConditions[modelID.InnerText] = ParseInitialCondition(
                                modelID.InnerText, child);
                    }
                }
            }

            //
            // Parses the "LoggerPolicy"
            //
            LoggerPolicy loggerPolicy = ParseLoggerPolicy(
                    m_doc.SelectSingleNode("/" + Constants.xpathPrm.ToLower() + "/" + Constants.xpathLoggerPolicy.ToLower()));

            return new SimulationParameter(
                stepperList,
                initialConditions,
                loggerPolicy,
                m_parameterID);
        }

        /// <summary>
        /// Parses the simulation parameter file.
        /// </summary>
        /// <param name="fileName">The simulation parameter file name</param>
        /// <param name="sim">The simulator.</param>
        public static SimulationParameter Parse(string fileName, WrappedSimulator sim)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return new SimulationParameterReader(doc, sim, Path.GetFileNameWithoutExtension(fileName)).Parse();
        }
    }
}
