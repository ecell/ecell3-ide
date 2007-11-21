using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

using EcellCoreLib;

namespace EcellLib
{
    public class SimulationParameter
    {
        List<EcellObject> m_steppers;
        Dictionary<string, Dictionary<string, Dictionary<string, double>>> m_initialConditions;
        LoggerPolicy m_loggerPolicy;
        string m_id;

        public List<EcellObject> Steppers
        {
            get
            {
                return m_steppers;
            }
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, double>>> InitialConditions
        {
            get
            {
                return m_initialConditions;

            }
        }

        public LoggerPolicy LoggerPolicy
        {
            get
            {
                return m_loggerPolicy;
            }
        }

        public string ID
        {
            get
            {
                return m_id;
            }
        }

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


    internal class SimulationParameterParseException : EcellXmlReaderException
    {
        public SimulationParameterParseException(string msg)
            : base(msg)
        {
        }
    }

    internal class SimulationParameterWriter : EcellXmlWriter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_initialCondition"></param>
        private void WriteInitialConditionElement(
                Dictionary<string, Dictionary<string, double>> l_initialCondition)
        {
            m_tx.WriteStartElement(Util.s_xpathInitialCondition.ToLower());
            //
            // Creates the "System" part.
            //
            m_tx.WriteStartElement(Util.s_xpathSystem.ToLower());
            foreach (string l_key in l_initialCondition[Util.s_xpathSystem].Keys)
            {
                m_tx.WriteStartElement(Util.s_xpathID.ToLower());
                m_tx.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_key);
                this.WriteValueElements(
                    new EcellValue(l_initialCondition[Util.s_xpathSystem][l_key]), false);
                m_tx.WriteEndElement();
            }
            m_tx.WriteEndElement();
            //
            // Creates the "Process" part.
            //
            m_tx.WriteStartElement(Util.s_xpathProcess.ToLower());
            foreach (string l_key in l_initialCondition[Util.s_xpathProcess].Keys)
            {
                m_tx.WriteStartElement(Util.s_xpathID.ToLower());
                m_tx.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_key);
                this.WriteValueElements(
                    new EcellValue(l_initialCondition[Util.s_xpathProcess][l_key]), false);
                m_tx.WriteEndElement();
            }
            m_tx.WriteEndElement();
            //
            // Creates the "Variable part.
            //
            m_tx.WriteStartElement(Util.s_xpathVariable.ToLower());
            foreach (string l_key in l_initialCondition[Util.s_xpathVariable].Keys)
            {
                m_tx.WriteStartElement(Util.s_xpathID.ToLower());
                m_tx.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_key);
                this.WriteValueElements(
                        new EcellValue(l_initialCondition[Util.s_xpathVariable][l_key]), false);
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
        /// <param name="l_emlFlag">The flag of "eml"</param>
        private void WriteStepperElements(EcellObject l_ecellObject)
        {
            m_tx.WriteStartElement(Util.s_xpathStepper.ToLower());
            m_tx.WriteAttributeString(Util.s_xpathClass, null, l_ecellObject.classname);
            m_tx.WriteAttributeString(Util.s_xpathID.ToLower(), null, l_ecellObject.key);

            if (l_ecellObject.Value != null && l_ecellObject.Value.Count > 0)
            {
                foreach (EcellData l_ecellData in l_ecellObject.Value)
                {
                    if (l_ecellData == null
                        || !l_ecellData.Settable
                        || l_ecellData.Value == null
                        || (l_ecellData.Value.IsString() &&
                            l_ecellData.Value.CastToString().Length <= 0))
                        continue;
                    m_tx.WriteStartElement(Util.s_xpathProperty.ToLower());
                    m_tx.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_ecellData.Name);
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
            m_tx.WriteStartElement(Util.s_xpathLoggerPolicy.ToLower());
            m_tx.WriteElementString(
                Util.s_xpathStep.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_reloadStepCount + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Util.s_xpathInterval.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_reloadInterval + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Util.s_xpathAction.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_diskFullAction + System.Environment.NewLine
                );
            m_tx.WriteElementString(
                Util.s_xpathSpace.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_maxDiskSpace + System.Environment.NewLine
                );
            m_tx.WriteEndElement();
        }

        public void WriteSteppers(string l_modelID,
                List<EcellObject> l_stepperList,
                Dictionary<string, Dictionary<string, double>> l_initialCondition)
        {
            m_tx.WriteStartElement(Util.s_xpathModel.ToLower());
            m_tx.WriteAttributeString(Util.s_xpathID.ToLower(), null, l_modelID);
            foreach (EcellObject l_stepper in l_stepperList)
            {
                WriteStepperElements(l_stepper);
            }
            WriteInitialConditionElement(l_initialCondition);
            m_tx.WriteEndElement();
        }

        public void WriteStartDocument()
        {
            m_tx.WriteStartDocument(true);
            m_tx.WriteStartElement(Util.s_xpathPrm.ToLower());
        }

        public void WriteEndDocument()
        {
            m_tx.WriteEndElement();
            m_tx.WriteEndDocument();
        }

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
                        Util.s_delimiterColon, Util.s_delimiterUnderbar);
                l_date = l_date.Replace(Util.s_delimiterPath, Util.s_delimiterUnderbar);
                l_date = l_date.Replace(Util.s_delimiterSpace, Util.s_delimiterUnderbar);
                string l_destFileName
                    = Path.GetDirectoryName(l_fileName) + Util.s_delimiterPath
                    + Util.s_delimiterUnderbar + l_date + Util.s_delimiterUnderbar + Path.GetFileName(l_fileName);
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
                    if (l_stepper.type.Equals(Util.s_xpathStepper))
                    {
                        if (!l_dic.ContainsKey(l_stepper.modelID))
                        {
                            l_dic[l_stepper.modelID] = new List<EcellObject>();
                        }
                        l_dic[l_stepper.modelID].Add(l_stepper);
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

    internal class SimulationParameterReader : EcellXmlReader
    {
        private XmlDocument m_doc;

        private WrappedSimulator m_simulator;

        private string m_parameterID;

        public SimulationParameterReader(XmlDocument doc, WrappedSimulator sim, string parameterID)
        {
            m_doc = doc;
            m_simulator = sim;
            m_parameterID = parameterID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_modelID"></param>
        /// <param name="l_node"></param>
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
                if (l_nodeType.Name.Equals(Util.s_xpathSystem.ToLower()))
                {
                    l_type = Util.s_xpathSystem;
                }
                else if (l_nodeType.Name.Equals(Util.s_xpathProcess.ToLower()))
                {
                    l_type = Util.s_xpathProcess;
                }
                else if (l_nodeType.Name.Equals(Util.s_xpathVariable.ToLower()))
                {
                    l_type = Util.s_xpathVariable;
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
                    XmlNode l_nodeName = l_nodeID.Attributes.GetNamedItem(Util.s_xpathName.ToLower());
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
            if (!l_initialCondition.ContainsKey(Util.s_xpathSystem))
            {
                l_initialCondition[Util.s_xpathSystem] = new Dictionary<string, double>();
            }
            if (!l_initialCondition.ContainsKey(Util.s_xpathProcess))
            {
                l_initialCondition[Util.s_xpathProcess] = new Dictionary<string, double>();
            }
            if (!l_initialCondition.ContainsKey(Util.s_xpathVariable))
            {
                l_initialCondition[Util.s_xpathVariable] = new Dictionary<string, double>();
            }
            return l_initialCondition;
        }

        /// <summary>
        /// Parses the "LoggerPolicy" node.
        /// </summary>
        /// <param name="l_node">The "LoggerPolicy" node</param>
        /// <param name="l_loggerPolicy">The stored "LoggerPolicy"</param>
        private LoggerPolicy ParseLoggerPolicy(XmlNode l_node)
        {
            int l_step = -1;
            double l_interval = -1.0;
            int l_action = -1;
            int l_diskSpace = -1;
            foreach (XmlNode l_childNode in l_node.ChildNodes)
            {
                if (l_childNode.Name.Equals(Util.s_xpathStep.ToLower()))
                {
                    l_step = XmlConvert.ToInt32(l_childNode.InnerText);
                }
                else if (l_childNode.Name.Equals(Util.s_xpathInterval.ToLower()))
                {
                    l_interval = XmlConvert.ToDouble(l_childNode.InnerText);
                }
                else if (l_childNode.Name.Equals(Util.s_xpathAction.ToLower()))
                {
                    l_action = XmlConvert.ToInt32(l_childNode.InnerText);
                }
                else if (l_childNode.Name.Equals(Util.s_xpathSpace.ToLower()))
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

            if (l_diskSpace <= 0)
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
        /// <param name="l_ecellObjectList">The stored list of the "EcellObject"</param>
        private EcellObject ParseStepper(string l_modelID, XmlNode l_stepper)
        {
            XmlNode l_stepperClass = l_stepper.Attributes.GetNamedItem(Util.s_xpathClass);
            XmlNode l_stepperID = l_stepper.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
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
                if (!l_stepperProperty.Name.Equals(Util.s_xpathProperty))
                {
                    continue;
                }
                XmlNode l_stepperPropertyName = l_stepperProperty.Attributes.GetNamedItem(Util.s_xpathName.ToLower());
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
                Util.s_xpathStepper,
                l_stepperClass.InnerText,
                l_ecellDataList);
        }

        public SimulationParameter Parse()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_initialConditions = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            List<EcellObject> l_stepperList = new List<EcellObject>();

            //
            // Parses the "Stepper".
            //
            XmlNodeList l_modelList = m_doc.SelectNodes("/" + Util.s_xpathPrm + "/" + Util.s_xpathModel.ToLower());
            foreach (XmlNode l_model in l_modelList)
            {
                XmlNode l_modelID = l_model.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
                if (!this.IsValidNode(l_modelID))
                    throw new SimulationParameterParseException("Invalid model node found");

                foreach (XmlNode l_child in l_model.ChildNodes)
                {
                    if (l_child.Name.Equals(Util.s_xpathStepper.ToLower()))
                    {
                        l_stepperList.Add(ParseStepper(l_modelID.InnerText, l_child));
                    }
                    else if (l_child.Name.Equals(Util.s_xpathInitialCondition.ToLower()))
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
                    m_doc.SelectSingleNode("/" + Util.s_xpathPrm + "/" + Util.s_xpathLoggerPolicy.ToLower()));

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
        public static SimulationParameter Parse(string l_fileName, WrappedSimulator sim)
        {
            XmlDocument l_doc = new XmlDocument();
            l_doc.Load(l_fileName);
            return new SimulationParameterReader(l_doc, sim, Path.GetFileNameWithoutExtension(l_fileName)).Parse();
        }
    }
}
