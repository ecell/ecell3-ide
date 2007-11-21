using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.IO;

using EcellCoreLib;

namespace EcellLib
{
    /// <summary>
    ///  Treats the "eml" formatted file.
    /// </summary>
    internal class Eml
    {
        /// <summary>
        /// ResourceManager for PropertyEditor.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResLib));

        /// <summary>
        /// Creates a new "Eml" instance with no argument.
        /// </summary>
        public Eml()
        {
            ; // do nothing
        }

        /// <summary>
        /// Appneds new "EcellObject" to the existing "EcellObject" list.
        /// </summary>
        /// <param name="l_srcObjectList">The existing "EcellObject" list</param>
        /// <param name="l_dstObject">New "EcellObject"</param>
        private void AppendEcellObject(List<EcellObject> l_srcObjectList, EcellObject l_dstObject)
        {
            bool l_appendFlag = false;
            foreach (EcellObject l_srcObject in l_srcObjectList)
            {
                if (l_srcObject.modelID.Equals(l_dstObject.modelID)
                    && l_srcObject.key.Equals(l_dstObject.key)
                    && l_srcObject.type.Equals(l_dstObject.type)
                    && l_srcObject.classname.Equals(l_dstObject.classname))
                {
                    l_appendFlag = true;
                    //
                    // 4 Value
                    //
                    foreach (EcellData l_dstData in l_dstObject.M_value)
                    {
                        List<EcellData> l_deleteSrcDataList = new List<EcellData>();
                        foreach (EcellData l_srcData in l_srcObject.M_value)
                        {
                            if (l_dstData.M_name.Equals(l_srcData.M_name)
                                && l_dstData.M_entityPath.Equals(l_srcData.M_entityPath))
                            {
                                l_deleteSrcDataList.Add(l_srcData);
                            }
                        }
                        foreach (EcellData l_deleteEcellData in l_deleteSrcDataList)
                        {
                            l_srcObject.M_value.Remove(l_deleteEcellData);
                        }
                        l_deleteSrcDataList.Clear();
                        l_deleteSrcDataList = null;
                        l_srcObject.M_value.Add(l_dstData);
                    }
                    //
                    // 4 Child
                    //
                    foreach (EcellObject l_dstChildObject in l_dstObject.M_instances)
                    {
                        this.AppendEcellObject(l_srcObject.M_instances, l_dstChildObject);
                    }
                }
            }
            if (!l_appendFlag)
            {
                l_srcObjectList.Add(l_dstObject);
            }
        }


        /// <summary>
        /// Creates the parameter file.
        /// </summary>
        /// <param name="l_fileName">The parameter file name</param>
        /// <param name="l_stepperList">The list of the "Stepper"</param>
        /// <param name="l_loggerPolicy">The "LoggerPolicy"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        public void Create(
                string l_fileName,
                List<EcellObject> l_stepperList,
                LoggerPolicy l_loggerPolicy,
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_initialCondition)
        {
            try
            {
                //
                // Initializes
                //
                if (l_fileName == null || l_fileName.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrSaveNull"));
                }
                if (l_stepperList == null || l_stepperList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrSaveNull"));
                }
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
                XmlTextWriter l_writer = null;
                try
                {
                    l_writer = new XmlTextWriter(l_fileName, System.Text.Encoding.UTF8);
                    l_writer.Formatting = Formatting.Indented;
                    l_writer.Indentation = 0;
                    l_writer.WriteStartDocument(true);
                    l_writer.WriteStartElement(Util.s_xpathPrm.ToLower());
                    //
                    // Divides.
                    //
                    Dictionary<string, List<EcellObject>> l_dic = new Dictionary<string, List<EcellObject>>();
                    foreach (EcellObject l_stepper in l_stepperList)
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
                        l_writer.WriteStartElement(Util.s_xpathModel.ToLower());
                        l_writer.WriteAttributeString(Util.s_xpathID.ToLower(), null, l_modelID);
                        foreach (EcellObject l_stepper in l_dic[l_modelID])
                        {
                            this.CreateStepperElements(l_writer, l_stepper, false);
                        }
                        this.CreateInitialConditionElement(l_writer, l_initialCondition[l_modelID]);
                        l_writer.WriteEndElement();
                    }
                    this.CreateLoggerPolicyElement(l_writer, l_loggerPolicy);
                    l_writer.WriteEndElement();
                    l_writer.WriteEndDocument();
                }
                finally
                {
                    if (l_writer != null)
                    {
                        l_writer.Close();
                    }
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrCreXml") + "[" + l_fileName + "] {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Creates the eml formatted file.
        /// </summary>
        /// <param name="l_fileName">The eml formatted file name</param>
        /// <param name="l_storedList">The list of the stored "EcellObject"</param>
        public void Create(string l_fileName, List<EcellObject> l_storedList)
        {
            try
            {
                //
                // Initializes
                //
                if (l_fileName == null || l_fileName.Length <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrSaveNull"));
                }
                if (l_storedList == null || l_storedList.Count <= 0)
                {
                    throw new Exception(m_resources.GetString("ErrSaveNull"));
                }
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
                XmlTextWriter l_writer = null;
                try
                {
                    l_writer = new XmlTextWriter(l_fileName, System.Text.Encoding.UTF8);
                    l_writer.Formatting = Formatting.Indented;
                    l_writer.Indentation = 0;
                    l_writer.WriteStartDocument(true);
                    l_writer.WriteStartElement(Util.s_xpathEml);
                    foreach (EcellObject l_ecellObject in l_storedList)
                    {
                        if (l_ecellObject.type.Equals(Util.s_xpathStepper))
                        {
                            this.CreateStepperElements(l_writer, l_ecellObject, true);
                        }
                        else if (l_ecellObject.type.Equals(Util.s_xpathSystem))
                        {
                            this.CreateSystemElements(l_writer, l_ecellObject);
                        }
                    }
                    l_writer.WriteEndElement();
                    l_writer.WriteEndDocument();
                }
                finally
                {
                    if (l_writer != null)
                    {
                        l_writer.Close();
                    }
                }
            }
            catch (Exception l_ex)
            {
                throw new Exception(m_resources.GetString("ErrCreEml") + "[" + l_fileName + "] {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// Creates the "Process" or "Variable" elements.
        /// </summary>
        /// <param name="l_writer">The xml writer</param>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        /// <param name="l_entityName">The name of "Process" or "Variable"</param>
        private void CreateEntityElements(XmlTextWriter l_writer, EcellObject l_ecellObject, string l_entityName)
        {
            l_writer.WriteStartElement(l_entityName.ToLower());
            l_writer.WriteAttributeString(Util.s_xpathClass, null, l_ecellObject.classname);
            l_writer.WriteAttributeString(
                Util.s_xpathID.ToLower(),
                null,
                l_ecellObject.key.Substring(l_ecellObject.key.IndexOf(Util.s_delimiterColon) + 1));
            if (l_ecellObject.M_value != null && l_ecellObject.M_value.Count > 0)
            {
                foreach (EcellData l_ecellData in l_ecellObject.M_value)
                {
                    if (l_ecellData == null || !l_ecellData.M_isSavable)
                    {
                        continue;
                    }
                    if (l_ecellData.M_value == null
                        || (l_ecellData.M_value.IsString() && l_ecellData.M_value.CastToString().Length <= 0))
                    {
                        continue;
                    }
                    l_writer.WriteStartElement(Util.s_xpathProperty.ToLower());
                    l_writer.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_ecellData.M_name);
                    this.CreateValueElements(l_writer, l_ecellData.M_value, false);
                    l_writer.WriteEndElement();
                }
            }
            l_writer.WriteEndElement();
            l_writer.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_writer"></param>
        /// <param name="l_initialCondition"></param>
        private void CreateInitialConditionElement(
                XmlTextWriter l_writer, Dictionary<string, Dictionary<string, double>> l_initialCondition)
        {
            l_writer.WriteStartElement(Util.s_xpathInitialCondition.ToLower());
            //
            // Creates the "System" part.
            //
            l_writer.WriteStartElement(Util.s_xpathSystem.ToLower());
            foreach (string l_key in l_initialCondition[Util.s_xpathSystem].Keys)
            {
                l_writer.WriteStartElement(Util.s_xpathID.ToLower());
                l_writer.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_key);
                this.CreateValueElements(
                        l_writer, new EcellValue(l_initialCondition[Util.s_xpathSystem][l_key]), false);
                l_writer.WriteEndElement();
            }
            l_writer.WriteEndElement();
            //
            // Creates the "Process" part.
            //
            l_writer.WriteStartElement(Util.s_xpathProcess.ToLower());
            foreach (string l_key in l_initialCondition[Util.s_xpathProcess].Keys)
            {
                l_writer.WriteStartElement(Util.s_xpathID.ToLower());
                l_writer.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_key);
                this.CreateValueElements(
                        l_writer, new EcellValue(l_initialCondition[Util.s_xpathProcess][l_key]), false);
                l_writer.WriteEndElement();
            }
            l_writer.WriteEndElement();
            //
            // Creates the "Variable part.
            //
            l_writer.WriteStartElement(Util.s_xpathVariable.ToLower());
            foreach (string l_key in l_initialCondition[Util.s_xpathVariable].Keys)
            {
                l_writer.WriteStartElement(Util.s_xpathID.ToLower());
                l_writer.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_key);
                this.CreateValueElements(
                        l_writer, new EcellValue(l_initialCondition[Util.s_xpathVariable][l_key]), false);
                l_writer.WriteEndElement();
            }
            l_writer.WriteEndElement();
            //
            // Closes
            //
            l_writer.WriteEndElement();
            l_writer.Flush();
        }

        /// <summary>
        /// Creates the "LoggerPolicy" elements.
        /// </summary>
        /// <param name="l_writer">The xml writer</param>
        /// <param name="l_loggerPolicy">The "LoggerPolicy"</param>
        private void CreateLoggerPolicyElement(XmlTextWriter l_writer, LoggerPolicy l_loggerPolicy)
        {
            l_writer.WriteStartElement(Util.s_xpathLoggerPolicy.ToLower());
            l_writer.WriteElementString(
                Util.s_xpathStep.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_reloadStepCount + System.Environment.NewLine
                );
            l_writer.WriteElementString(
                Util.s_xpathInterval.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_reloadInterval + System.Environment.NewLine
                );
            l_writer.WriteElementString(
                Util.s_xpathAction.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_diskFullAction + System.Environment.NewLine
                );
            l_writer.WriteElementString(
                Util.s_xpathSpace.ToLower(),
                null,
                System.Environment.NewLine + l_loggerPolicy.m_maxDiskSpace + System.Environment.NewLine
                );
            l_writer.WriteEndElement();
            l_writer.Flush();
        }

        /// <summary>
        /// Creates the "Stepper" elements.
        /// </summary>
        /// <param name="l_writer">The xml writer</param>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        /// <param name="l_emlFlag">The flag of "eml"</param>
        private void CreateStepperElements(XmlTextWriter l_writer, EcellObject l_ecellObject, bool l_emlFlag)
        {
            l_writer.WriteStartElement(Util.s_xpathStepper.ToLower());
            l_writer.WriteAttributeString(Util.s_xpathClass, null, l_ecellObject.classname);
            l_writer.WriteAttributeString(Util.s_xpathID.ToLower(), null, l_ecellObject.key);
            if (l_ecellObject.M_value != null && l_ecellObject.M_value.Count > 0)
            {
                foreach (EcellData l_ecellData in l_ecellObject.M_value)
                {
                    if (l_ecellData == null)
                    {
                        continue;
                    }
                    if (l_emlFlag)
                    {
                        if (!l_ecellData.M_isSavable)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!l_ecellData.M_isSettable)
                        {
                            continue;
                        }
                    }
                    if (l_ecellData.M_value == null
                        || (l_ecellData.M_value.IsString() && l_ecellData.M_value.CastToString().Length <= 0))
                    {
                        continue;
                    }
                    /*
                    if (l_ecellData.M_value.IsDouble())
                    {
                        if (l_emlFlag)
                        {
                            if (l_ecellData.M_value.CastToDouble() == Double.MaxValue)
                            {
                                continue;
                            }
                            else if (Double.IsInfinity(l_ecellData.M_value.CastToDouble()))
                            {
                                continue;
                            }
                            /*
                            else if (l_ecellData.M_value.CastToDouble() <= Double.Epsilon)
                            {
                                continue;
                            }
                        }
                    }
                     */
                    l_writer.WriteStartElement(Util.s_xpathProperty.ToLower());
                    l_writer.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_ecellData.M_name);
                    this.CreateValueElements(l_writer, l_ecellData.M_value, false);
                    l_writer.WriteEndElement();
                }
            }
            l_writer.WriteEndElement();
            l_writer.Flush();
        }

        /// <summary>
        /// Creates the "System" elements.
        /// </summary>
        /// <param name="l_writer">The xml writer</param>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        private void CreateSystemElements(XmlTextWriter l_writer, EcellObject l_ecellObject)
        {
            l_writer.WriteStartElement(Util.s_xpathSystem.ToLower());
            l_writer.WriteAttributeString(Util.s_xpathClass, null, l_ecellObject.classname);
            l_writer.WriteAttributeString(Util.s_xpathID.ToLower(), null, l_ecellObject.key);
            if (l_ecellObject.M_value != null && l_ecellObject.M_value.Count > 0)
            {
                foreach (EcellData l_ecellData in l_ecellObject.M_value)
                {
                    if (l_ecellData == null || !l_ecellData.M_isSavable)
                    {
                        continue;
                    }
                    if (l_ecellData.M_value == null
                        || (l_ecellData.M_value.IsString() && l_ecellData.M_value.CastToString().Length <= 0))
                    {
                        continue;
                    }
                    l_writer.WriteStartElement(Util.s_xpathProperty.ToLower());
                    l_writer.WriteAttributeString(Util.s_xpathName.ToLower(), null, l_ecellData.M_name);
                    this.CreateValueElements(l_writer, l_ecellData.M_value, false);
                    l_writer.WriteEndElement();
                }
                //
                // 4 children
                //
                if (l_ecellObject.M_instances != null && l_ecellObject.M_instances.Count > 0)
                {
                    List<EcellObject> l_processList = new List<EcellObject>();
                    List<EcellObject> l_variableList = new List<EcellObject>();
                    foreach (EcellObject l_childEcellObject in l_ecellObject.M_instances)
                    {
                        if (l_childEcellObject.type.Equals(Util.s_xpathProcess))
                        {
                            l_processList.Add(l_childEcellObject);
                        }
                        else if (l_childEcellObject.type.Equals(Util.s_xpathVariable))
                        {
                            l_variableList.Add(l_childEcellObject);
                        }
                    }
                    foreach (EcellObject l_variableEcellObject in l_variableList)
                    {
                        this.CreateEntityElements(l_writer, l_variableEcellObject, Util.s_xpathVariable);
                    }
                    foreach (EcellObject l_processEcellObject in l_processList)
                    {
                        this.CreateEntityElements(l_writer, l_processEcellObject, Util.s_xpathProcess);
                    }
                }
            }
            l_writer.WriteEndElement();
            l_writer.Flush();
        }

        /// <summary>
        /// Creates the "value" elements.
        /// </summary>
        /// <param name="l_writer">The xml writer</param>
        /// <param name="l_ecellValue">The "EcellValue"</param>
        /// <param name="l_isElement">The flag whether the "Value" element add</param>
        private void CreateValueElements(XmlTextWriter l_writer, EcellValue l_ecellValue, bool l_isElement)
        {
            if (l_ecellValue == null)
            {
                return;
            }
            else if (l_ecellValue.IsDouble())
            {
                if (Double.IsInfinity(l_ecellValue.CastToDouble()))
                {
                    l_writer.WriteElementString(
                        Util.s_xpathValue.ToLower(),
                        null,
                        System.Environment.NewLine + XmlConvert.ToString(Double.PositiveInfinity)
                            + System.Environment.NewLine);
                }
                else if (l_ecellValue.CastToDouble() == Double.MaxValue)
                {
                    l_writer.WriteElementString(
                        Util.s_xpathValue.ToLower(),
                        null,
                        System.Environment.NewLine + XmlConvert.ToString(Double.MaxValue)
                            + System.Environment.NewLine);
                }
                else
                {
                    l_writer.WriteElementString(
                        Util.s_xpathValue.ToLower(),
                        null,
                        System.Environment.NewLine + l_ecellValue.CastToDouble().ToString()
                            + System.Environment.NewLine);
                }
            }
            else if (l_ecellValue.IsInt())
            {
                l_writer.WriteElementString(
                    Util.s_xpathValue.ToLower(),
                    null,
                    System.Environment.NewLine + l_ecellValue.CastToInt().ToString() + System.Environment.NewLine);
            }
            else if (l_ecellValue.IsList())
            {
                if (l_ecellValue.CastToList() == null || l_ecellValue.CastToList().Count <= 0)
                {
                    return;
                }
                if (l_isElement)
                {
                    l_writer.WriteStartElement(Util.s_xpathValue.ToLower());
                    foreach (EcellValue l_childEcellValue in l_ecellValue.CastToList())
                    {
                        this.CreateValueElements(l_writer, l_childEcellValue, true);
                    }
                    l_writer.WriteEndElement();
                }
                else
                {
                    foreach (EcellValue l_childEcellValue in l_ecellValue.CastToList())
                    {
                        this.CreateValueElements(l_writer, l_childEcellValue, true);
                    }
                }
            }
            else
            {
                l_writer.WriteElementString(
                    Util.s_xpathValue.ToLower(),
                    null,
                    System.Environment.NewLine + l_ecellValue.CastToString() + System.Environment.NewLine);
            }
        }

        /// <summary>
        /// Loads nested "value" elements.
        /// </summary>
        /// <param name="l_node">The "property" element that is parent element of "value" elements</param>
        /// <param name="l_depth">The depth of sub-directories</param>
        /// <param name="l_count">The count of data</param>
        /// <returns>The "EcellValue"</returns>
        private EcellValue GetValueList(XmlNode l_node, ref int l_depth, ref int l_count)
        {
            bool l_depthFlag = true;
            List<EcellValue> l_ecellValueList = new List<EcellValue>();
            XmlNodeList l_childrenNode = l_node.ChildNodes;
            foreach (XmlNode l_childNode in l_childrenNode)
            {
                if (!this.IsValidNode(l_childNode))
                {
                    continue;
                }
                if (l_childNode.Name.Equals(Util.s_xpathValue.ToLower()))
                {
                    XmlNodeList l_grandChildrenNode = l_childNode.ChildNodes;
                    foreach (XmlNode l_grandChildNode in l_grandChildrenNode)
                    {
                        if (l_grandChildNode.GetType() == typeof(XmlText))
                        {
                            string l_value = l_grandChildNode.Value.Replace(System.Environment.NewLine, "");
                            l_value = l_value.Replace("\r", "");
                            l_value = l_value.Replace("\n", "");
                            if (l_value.Equals(XmlConvert.ToString(Double.PositiveInfinity)))
                            {
                                l_ecellValueList.Add(new EcellValue(Double.PositiveInfinity));
                            }
                            else
                            {
                                l_ecellValueList.Add(new EcellValue(l_value));
                            }
                            if (l_depthFlag)
                            {
                                l_depth++;
                                l_depthFlag = false;
                            }
                            l_count++;
                        }
                    }
                    EcellValue l_childEcellValue = this.GetValueList(l_childNode, ref l_depth, ref l_count);
                    if (l_childEcellValue != null)
                    {
                        l_ecellValueList.Add(l_childEcellValue);
                    }
                }
            }
            if (l_ecellValueList.Count <= 0)
            {
                return null;
            }
            else
            {
                return new EcellValue(l_ecellValueList);
            }
        }

        /// <summary>
        /// Tests whether the element is null or empty.
        /// </summary>
        /// <param name="l_node">The checked element</param>
        /// <returns>false if the element is null or empty; true otherise</returns>
        private bool IsValidNode(XmlNode l_node)
        {
            if (l_node == null || l_node.InnerText == null || l_node.InnerText.Length < 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Parses the simulation parameter file.
        /// </summary>
        /// <param name="l_fileName">The simulation parameter file name</param>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_stepperList">The list of the "Stepper"</param>
        /// <param name="l_initialCondition">The initial condition.</param>
        /// <param name="l_loggerPolicy">The "LoggerPolicy"</param>
        /// <param name="l_parameterID">The parameter ID</param>
        public void Parse(
            string l_fileName,
            WrappedSimulator l_simulator,
            List<EcellObject> l_stepperList,
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_initialCondition,
            ref LoggerPolicy l_loggerPolicy,
            ref string l_parameterID
            )
        {
            XmlDocument l_doc = new XmlDocument();
            try
            {
                l_doc.Load(l_fileName);
                //
                // Parses the "Stepper".
                //
                l_parameterID = Path.GetFileNameWithoutExtension(l_fileName);
                XmlNodeList l_modelList = l_doc.SelectNodes(
                        Util.s_delimiterPath + Util.s_delimiterPath + Util.s_xpathModel.ToLower());
                foreach (XmlNode l_model in l_modelList)
                {
                    XmlNode l_modelID = l_model.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
                    if (!this.IsValidNode(l_modelID))
                    {
                        continue;
                    }
                    foreach (XmlNode l_child in l_model.ChildNodes)
                    {
                        if (l_child.Name.Equals(Util.s_xpathStepper.ToLower()))
                        {
                            this.ParseStepper(l_modelID.InnerText, l_child, l_simulator, l_stepperList);
                        }
                        else if (l_child.Name.Equals(Util.s_xpathInitialCondition.ToLower()))
                        {
                            this.ParseInitialCondition(l_modelID.InnerText, l_child, l_initialCondition);
                        }
                    }
                }
                //
                // Parses the "LoggerPolicy"
                //
                this.ParseLoggerPolicy(
                        l_doc.SelectSingleNode(
                                Util.s_delimiterPath + Util.s_delimiterPath + Util.s_xpathLoggerPolicy.ToLower()),
                        ref l_loggerPolicy);
            }
            catch (Exception l_ex)
            {
                l_simulator = null;
                l_parameterID = null;
                l_stepperList = null;
                throw new Exception(m_resources.GetString("ErrParseEml") + "[" + l_fileName + "] {" + l_ex.ToString() + "}");
            }
            finally
            {
                l_doc = null;
            }
        }

        /// <summary>
        /// Parses the "eml" formatted file.
        /// </summary>
        /// <param name="l_fileName">The "eml" formatted file</param>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_ecellObjectList">The list of "EcellObject"</param>
        /// <param name="l_modelID">The model ID</param>
        public void Parse(
            string l_fileName,
            WrappedSimulator l_simulator,
            List<EcellObject> l_ecellObjectList,
            ref string l_modelID
            )
        {
            XmlDocument l_doc = new XmlDocument();
            try
            {
                l_doc.Load(l_fileName);
                //
                // 4 EcellObject( "Model" )
                //
                l_modelID = Path.GetFileNameWithoutExtension(l_fileName);
                EcellObject l_modelObject = EcellObject.CreateObject(l_modelID, "", Util.s_xpathModel, "", null);
                l_ecellObjectList.Add(l_modelObject);
                //
                // Parse
                //
                this.ParseStepper(l_modelID, l_doc, l_simulator, l_ecellObjectList);
                Dictionary<string, WrappedPolymorph> l_processPropertyDic = new Dictionary<string, WrappedPolymorph>();
                this.ParseSystem(l_modelID, l_doc, l_simulator, l_ecellObjectList, l_processPropertyDic);
                if (l_processPropertyDic.Count > 0)
                {
                    // The "VariableReferenceList" is previously loaded. 
                    string[] l_keys = null;
                    l_processPropertyDic.Keys.CopyTo(l_keys = new string[l_processPropertyDic.Keys.Count], 0);
                    foreach (string l_entityPath in l_keys)
                    {
                        if (l_entityPath.EndsWith(Util.s_xpathVRL))
                        {
                            l_simulator.LoadEntityProperty(l_entityPath, l_processPropertyDic[l_entityPath]);
                            l_processPropertyDic.Remove(l_entityPath);
                        }
                    }
                    foreach (string l_entityPath in l_processPropertyDic.Keys)
                    {
                        l_simulator.LoadEntityProperty(l_entityPath, l_processPropertyDic[l_entityPath]);
                    }
                }
            }
            catch (Exception l_ex)
            {
                l_simulator = null;
                l_modelID = null;
                l_ecellObjectList = null;
                throw new Exception(m_resources.GetString("ErrParseEml") + "[" + l_fileName + "] {" + l_ex.ToString() + "}");
            }
            finally
            {
                l_doc = null;
            }
        }

        /// <summary>
        /// Parses the "eml" formatted file.
        /// </summary>
        /// <param name="l_fileName">The "eml" formatted file</param>
        /// <param name="l_simulator">The simulator</param>
        public void Parse(string l_fileName, WrappedSimulator l_simulator)
        {
            XmlDocument l_doc = new XmlDocument();
            try
            {
                l_doc.Load(l_fileName);
                this.ParseStepper(l_doc, l_simulator);
                this.ParseSystem(l_doc, l_simulator);
            }
            catch (Exception l_ex)
            {
                l_simulator = null;
                throw new Exception(m_resources.GetString("ErrParseEml") + "[" + l_fileName + "] {" + l_ex.ToString() + "}");
            }
            finally
            {
                l_doc = null;
            }
        }

        /// <summary>
        /// Loads the "process" or "variable" element.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_node">The "process" or "variable" element</param>
        /// <param name="l_systemID">The system ID of the parent "System" element</param>
        /// <param name="l_flag">"Process" if this element is "Process" element; "Variable" otherwise</param>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_childEcellObjectList">The list of a "EcellObject"</param>
        /// <param name="l_processPropertyDic">The dictionary of a process property</param>
        private void ParseEntity(
            string l_modelID,
            XmlNode l_node,
            string l_systemID,
            string l_flag,
            WrappedSimulator l_simulator,
            List<EcellObject> l_childEcellObjectList,
            Dictionary<string, WrappedPolymorph> l_processPropertyDic)
        {
            XmlNode l_nodeClass = l_node.Attributes.GetNamedItem(Util.s_xpathClass);
            XmlNode l_nodeID = l_node.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
            if (!this.IsValidNode(l_nodeClass) || !this.IsValidNode(l_nodeID))
            {
                return;
            }
            //
            // 4 "EcellCoreLib"
            //
            l_simulator.CreateEntity(
                l_nodeClass.InnerText,
                l_flag + Util.s_delimiterColon + l_systemID + Util.s_delimiterColon + l_nodeID.InnerText);
            //
            // 4 children
            //
            List<EcellData> l_ecellDataList = new List<EcellData>();
            XmlNodeList l_nodePropertyList = l_node.ChildNodes;
            foreach (XmlNode l_nodeProperty in l_nodePropertyList)
            {
                if (!l_nodeProperty.Name.Equals(Util.s_xpathProperty))
                {
                    continue;
                }
                XmlNode l_nodePropertyName = l_nodeProperty.Attributes.GetNamedItem(Util.s_xpathName.ToLower());
                if (!this.IsValidNode(l_nodePropertyName))
                {
                    continue;
                }
                int l_depth = 0;
                int l_count = 0;
                EcellValue l_ecellValue = this.GetValueList(l_nodeProperty, ref l_depth, ref l_count);
                if (l_ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
                    string l_entityPath =
                        l_flag + Util.s_delimiterColon +
                        l_systemID + Util.s_delimiterColon +
                        l_nodeID.InnerText + Util.s_delimiterColon +
                        l_nodePropertyName.InnerText;
                    WrappedPolymorph l_polymorph = EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue);
                    if (l_flag.Equals(Util.s_xpathVariable))
                    {
                        l_simulator.LoadEntityProperty(l_entityPath, l_polymorph);
                    }
                    else
                    {
                        l_processPropertyDic[l_entityPath] = l_polymorph;
                    }
                    EcellData l_ecellData = new EcellData(l_nodePropertyName.InnerText, l_ecellValue, l_entityPath);
                    l_ecellDataList.Add(l_ecellData);
                }
            }
            //
            // 4 "EcellLib"
            //
            l_childEcellObjectList.Add(
                EcellObject.CreateObject(
                    l_modelID,
                    l_systemID + Util.s_delimiterColon + l_nodeID.InnerText,
                    l_flag,
                    l_nodeClass.InnerText,
                    l_ecellDataList));
        }

        /// <summary>
        /// Parses the "process" or "variable" element.
        /// </summary>
        /// <param name="l_node">The "process" or "variable" element</param>
        /// <param name="l_systemID">The system ID of the parent "System" element</param>
        /// <param name="l_flag">"Process" if this element is "Process" element; "Variable" otherwise</param>
        /// <param name="l_simulator">The simulator</param>
        private void ParseEntity(
            XmlNode l_node,
            string l_systemID,
            string l_flag,
            WrappedSimulator l_simulator)
        {
            XmlNode l_nodeClass = l_node.Attributes.GetNamedItem(Util.s_xpathClass);
            XmlNode l_nodeID = l_node.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
            if (!this.IsValidNode(l_nodeClass) || !this.IsValidNode(l_nodeID))
            {
                return;
            }
            //
            // 4 "EcellCoreLib"
            //
            l_simulator.CreateEntity(
                l_nodeClass.InnerText,
                l_flag + Util.s_delimiterColon + l_systemID + Util.s_delimiterColon + l_nodeID.InnerText
                );
            //
            // 4 children
            //
            XmlNodeList l_nodePropertyList = l_node.ChildNodes;
            foreach (XmlNode l_nodeProperty in l_nodePropertyList)
            {
                if (!l_nodeProperty.Name.Equals(Util.s_xpathProperty))
                {
                    continue;
                }
                XmlNode l_nodePropertyName = l_nodeProperty.Attributes.GetNamedItem(Util.s_xpathName.ToLower());
                if (!this.IsValidNode(l_nodePropertyName))
                {
                    continue;
                }
                int l_depth = 0;
                int l_count = 0;
                EcellValue l_ecellValue = this.GetValueList(l_nodeProperty, ref l_depth, ref l_count);
                if (l_ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
                    string l_entityPath =
                        l_flag + Util.s_delimiterColon +
                        l_systemID + Util.s_delimiterColon +
                        l_nodeID.InnerText + Util.s_delimiterColon +
                        l_nodePropertyName.InnerText;
                    l_simulator.LoadEntityProperty(
                        l_entityPath,
                        EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue)
                        );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l_modelID"></param>
        /// <param name="l_node"></param>
        /// <param name="l_initialCondition"></param>
        private void ParseInitialCondition(
                string l_modelID,
                XmlNode l_node,
                Dictionary<string, Dictionary<string, Dictionary<string, double>>> l_initialCondition)
        {
            l_initialCondition[l_modelID] = new Dictionary<string, Dictionary<string, double>>();
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
                l_initialCondition[l_modelID][l_type] = new Dictionary<string, double>();
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
                        l_initialCondition[l_modelID][l_type][l_nodeName.InnerText]
                            = XmlConvert.ToDouble(l_nodeID.InnerText);
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
            }
            if (!l_initialCondition[l_modelID].ContainsKey(Util.s_xpathSystem))
            {
                l_initialCondition[l_modelID][Util.s_xpathSystem] = new Dictionary<string, double>();
            }
            if (!l_initialCondition[l_modelID].ContainsKey(Util.s_xpathProcess))
            {
                l_initialCondition[l_modelID][Util.s_xpathProcess] = new Dictionary<string, double>();
            }
            if (!l_initialCondition[l_modelID].ContainsKey(Util.s_xpathVariable))
            {
                l_initialCondition[l_modelID][Util.s_xpathVariable] = new Dictionary<string, double>();
            }
        }

        /// <summary>
        /// Parses the "LoggerPolicy" node.
        /// </summary>
        /// <param name="l_node">The "LoggerPolicy" node</param>
        /// <param name="l_loggerPolicy">The stored "LoggerPolicy"</param>
        private void ParseLoggerPolicy(
            XmlNode l_node,
            ref LoggerPolicy l_loggerPolicy
            )
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
            if (l_step >= 0 && l_interval >= 0.0
                    && (l_action == 0 || l_action == 1)
                    && l_diskSpace >= 0)
            {
                l_loggerPolicy = new LoggerPolicy(l_step, l_interval, l_action, l_diskSpace);
            }
        }

        /// <summary>
        /// Loads the "Stepper" elements.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_doc">The "eml" "XmlObject"</param>
        /// <param name="l_ecellObjectList">The list of "EcellObject"</param>
        /// <param name="l_simulator">The simulator</param>
        private void ParseStepper(
            string l_modelID,
            XmlDocument l_doc,
            WrappedSimulator l_simulator,
            List<EcellObject> l_ecellObjectList
            )
        {
            XmlNodeList l_stepperList = l_doc.SelectNodes(
                Util.s_delimiterPath + Util.s_xpathEml + Util.s_delimiterPath + Util.s_xpathStepper.ToLower());
            foreach (XmlNode l_stepper in l_stepperList)
            {
                this.ParseStepper(l_modelID, l_stepper, l_simulator, l_ecellObjectList);
            }
        }

        /// <summary>
        /// Parses the "Stepper" node.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_stepper">The "Stepper" node</param>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_ecellObjectList">The stored list of the "EcellObject"</param>
        private void ParseStepper(
            string l_modelID,
            XmlNode l_stepper,
            WrappedSimulator l_simulator,
            List<EcellObject> l_ecellObjectList
            )
        {
            XmlNode l_stepperClass = l_stepper.Attributes.GetNamedItem(Util.s_xpathClass);
            XmlNode l_stepperID = l_stepper.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
            if (!this.IsValidNode(l_stepperClass) || !this.IsValidNode(l_stepperID))
            {
                return;
            }
            //
            // 4 "EcellCoreLib"
            //
            if (l_simulator != null)
            {
                try
                {
                    l_simulator.CreateStepper(l_stepperClass.InnerText, l_stepperID.InnerText);
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
            }
            //
            // 4 children
            //
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
                int l_depth = 0;
                int l_count = 0;
                EcellValue l_ecellValue = this.GetValueList(l_stepperProperty, ref l_depth, ref l_count);
                if (l_ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
                    if (l_simulator != null)
                    {
                        l_simulator.LoadStepperProperty(
                            l_stepperID.InnerText,
                            l_stepperPropertyName.InnerText,
                            EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue));
                    }
                    EcellData l_ecellData = new EcellData(
                            l_stepperPropertyName.InnerText, l_ecellValue, l_stepperPropertyName.InnerText);
                    l_ecellData.M_isGettable = true;
                    l_ecellData.M_isLoadable = false;
                    l_ecellData.M_isSavable = false;
                    l_ecellData.M_isSettable = true;
                    l_ecellDataList.Add(l_ecellData);
                }
            }
            //
            // 4 "EcellLib"
            //
            EcellObject l_ecellObject = EcellObject.CreateObject(
                l_modelID,
                l_stepperID.InnerText,
                Util.s_xpathStepper,
                l_stepperClass.InnerText,
                l_ecellDataList
                );
            l_ecellObjectList.Add(l_ecellObject);
        }

        /// <summary>
        /// Parses the "Stepper" elements.
        /// </summary>
        /// <param name="l_doc">The "eml" "XmlObject"</param>
        /// <param name="l_simulator">The simulator</param>
        private void ParseStepper(XmlDocument l_doc, WrappedSimulator l_simulator)
        {
            XmlNodeList l_stepperList = l_doc.SelectNodes(
                    Util.s_delimiterPath + Util.s_xpathEml + Util.s_delimiterPath + Util.s_xpathStepper.ToLower());
            foreach (XmlNode l_stepper in l_stepperList)
            {
                XmlNode l_stepperClass = l_stepper.Attributes.GetNamedItem(Util.s_xpathClass);
                XmlNode l_stepperID = l_stepper.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
                if (!this.IsValidNode(l_stepperClass) || !this.IsValidNode(l_stepperID))
                {
                    return;
                }
                l_simulator.CreateStepper(l_stepperClass.InnerText, l_stepperID.InnerText);
                XmlNodeList l_stepperPropertyList = l_stepper.ChildNodes;
                foreach (XmlNode l_stepperProperty in l_stepperPropertyList)
                {
                    if (!l_stepperProperty.Name.Equals(Util.s_xpathProperty))
                    {
                        continue;
                    }
                    XmlNode l_stepperPropertyName
                            = l_stepperProperty.Attributes.GetNamedItem(Util.s_xpathName.ToLower());
                    if (!this.IsValidNode(l_stepperPropertyName))
                    {
                        continue;
                    }
                    int l_depth = 0;
                    int l_count = 0;
                    EcellValue l_ecellValue = this.GetValueList(l_stepperProperty, ref l_depth, ref l_count);
                    if (l_ecellValue != null)
                    {
                        l_simulator.LoadStepperProperty(
                            l_stepperID.InnerText,
                            l_stepperPropertyName.InnerText,
                            EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue)
                            );
                    }
                }
            }
        }

        /// <summary>
        /// Loads the "System" elements.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_doc">The "eml" "XmlObject"</param>
        /// <param name="l_ecellObjectList">The list of "EcellObject"</param>
        /// <param name="l_simulator">The simulator</param>
        /// <param name="l_processPropertyDic">The dictionary of a process property</param>
        private void ParseSystem(
            string l_modelID,
            XmlDocument l_doc,
            WrappedSimulator l_simulator,
            List<EcellObject> l_ecellObjectList,
            Dictionary<string, WrappedPolymorph> l_processPropertyDic)
        {
            XmlNodeList l_systemList = l_doc.SelectNodes(
                Util.s_delimiterPath + Util.s_xpathEml + Util.s_delimiterPath + Util.s_xpathSystem.ToLower());
            foreach (XmlNode l_system in l_systemList)
            {
                XmlNode l_systemClass = l_system.Attributes.GetNamedItem(Util.s_xpathClass);
                XmlNode l_systemID = l_system.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
                if (!this.IsValidNode(l_systemClass) || !this.IsValidNode(l_systemID))
                {
                    continue;
                }
                if (l_systemID.InnerText.IndexOf(Util.s_delimiterPath) != 0)
                {
                    continue;
                }
                //
                // 4 "EcellCoreLib"
                //
                string l_parentPath
                        = l_systemID.InnerText.Substring(0, l_systemID.InnerText.LastIndexOf(Util.s_delimiterPath));
                string l_childPath
                        = l_systemID.InnerText.Substring(l_systemID.InnerText.LastIndexOf(Util.s_delimiterPath) + 1);
                if (l_systemID.InnerText.Equals(Util.s_delimiterPath))
                {
                    if (l_childPath.Length == 0)
                    {
                        l_childPath = Util.s_delimiterPath;
                    }
                }
                else
                {
                    if (l_parentPath.Length == 0)
                    {
                        l_parentPath = Util.s_delimiterPath;
                    }
                    l_simulator.CreateEntity(
                            l_systemClass.InnerText,
                            l_systemClass.InnerText + Util.s_delimiterColon + l_parentPath
                                    + Util.s_delimiterColon + l_childPath);
                }
                //
                // 4 children
                //
                List<EcellData> l_ecellDataList = new List<EcellData>();
                List<EcellObject> l_childEcellObjectList = new List<EcellObject>();
                XmlNodeList l_systemPropertyList = l_system.ChildNodes;
                foreach (XmlNode l_systemProperty in l_systemPropertyList)
                {
                    if (l_systemProperty.Name.Equals(Util.s_xpathVariable.ToLower()))
                    {
                        this.ParseEntity(
                            l_modelID,
                            l_systemProperty,
                            l_systemID.InnerText,
                            Util.s_xpathVariable,
                            l_simulator,
                            l_childEcellObjectList,
                            l_processPropertyDic);
                        continue;
                    }
                    else if (l_systemProperty.Name.Equals(Util.s_xpathProcess.ToLower()))
                    {
                        this.ParseEntity(
                            l_modelID,
                            l_systemProperty,
                            l_systemID.InnerText,
                            Util.s_xpathProcess,
                            l_simulator,
                            l_childEcellObjectList,
                            l_processPropertyDic);
                        continue;
                    }
                    else if (!l_systemProperty.Name.Equals(Util.s_xpathProperty))
                    {
                        continue;
                    }
                    XmlNode l_systemPropertyName = l_systemProperty.Attributes.GetNamedItem(Util.s_xpathName.ToLower());
                    if (!this.IsValidNode(l_systemPropertyName))
                    {
                        continue;
                    }
                    int l_depth = 0;
                    int l_count = 0;
                    EcellValue l_ecellValue = this.GetValueList(l_systemProperty, ref l_depth, ref l_count);
                    //
                    // 4 "EcellCoreLib"
                    //
                    if (l_ecellValue != null)
                    {
                        string l_entityPath =
                            Util.s_xpathSystem + Util.s_delimiterColon +
                            l_parentPath + Util.s_delimiterColon +
                            l_childPath + Util.s_delimiterColon +
                            l_systemPropertyName.InnerText;
                        l_simulator.LoadEntityProperty(
                            l_entityPath,
                            EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue));
                        EcellData l_ecellData
                                = new EcellData(l_systemPropertyName.InnerText, l_ecellValue, l_entityPath);
                        l_ecellDataList.Add(l_ecellData);
                    }
                }
                //
                // 4 EcellLib
                //
                EcellObject l_ecellObject = EcellObject.CreateObject(
                    l_modelID, l_systemID.InnerText, Util.s_xpathSystem, l_systemClass.InnerText,
                    l_ecellDataList);
                l_ecellObject.M_instances = l_childEcellObjectList;
                this.AppendEcellObject(l_ecellObjectList, l_ecellObject);
            }
        }

        /// <summary>
        /// Parses the "System" elements.
        /// </summary>
        /// <param name="l_doc">The "eml" "XmlObject"</param>
        /// <param name="l_simulator">The simulator</param>
        private void ParseSystem(XmlDocument l_doc, WrappedSimulator l_simulator)
        {
            XmlNodeList l_systemList = l_doc.SelectNodes(
                Util.s_delimiterPath + Util.s_xpathEml + Util.s_delimiterPath + Util.s_xpathSystem.ToLower());
            foreach (XmlNode l_system in l_systemList)
            {
                XmlNode l_systemClass = l_system.Attributes.GetNamedItem(Util.s_xpathClass);
                XmlNode l_systemID = l_system.Attributes.GetNamedItem(Util.s_xpathID.ToLower());
                if (!this.IsValidNode(l_systemClass) || !this.IsValidNode(l_systemID))
                {
                    continue;
                }
                if (l_systemID.InnerText.IndexOf(Util.s_delimiterPath) != 0)
                {
                    continue;
                }
                //
                // 4 "EcellCoreLib"
                //
                string l_parentPath = l_systemID.InnerText.Substring(
                        0, l_systemID.InnerText.LastIndexOf(Util.s_delimiterPath));
                string l_childPath = l_systemID.InnerText.Substring(
                        l_systemID.InnerText.LastIndexOf(Util.s_delimiterPath) + 1);
                if (l_systemID.InnerText.Equals(Util.s_delimiterPath))
                {
                    if (l_childPath.Length == 0)
                    {
                        l_childPath = Util.s_delimiterPath;
                    }
                }
                else
                {
                    if (l_parentPath.Length == 0)
                    {
                        l_parentPath = Util.s_delimiterPath;
                    }
                    l_simulator.CreateEntity(
                         l_systemClass.InnerText,
                         l_systemClass.InnerText + Util.s_delimiterColon
                                + l_parentPath + Util.s_delimiterColon + l_childPath);
                }
                //
                // 4 children
                //
                XmlNodeList l_systemPropertyList = l_system.ChildNodes;
                foreach (XmlNode l_systemProperty in l_systemPropertyList)
                {
                    if (l_systemProperty.Name.Equals(Util.s_xpathVariable.ToLower()))
                    {
                        this.ParseEntity(
                                l_systemProperty,
                                l_systemID.InnerText,
                                Util.s_xpathVariable,
                                l_simulator);
                        continue;
                    }
                    else if (l_systemProperty.Name.Equals(Util.s_xpathProcess.ToLower()))
                    {
                        this.ParseEntity(
                                l_systemProperty,
                                l_systemID.InnerText,
                                Util.s_xpathProcess,
                                l_simulator);
                        continue;
                    }
                    else if (!l_systemProperty.Name.Equals(Util.s_xpathProperty))
                    {
                        continue;
                    }
                    XmlNode l_systemPropertyName
                            = l_systemProperty.Attributes.GetNamedItem(Util.s_xpathName.ToLower());
                    if (!this.IsValidNode(l_systemPropertyName))
                    {
                        continue;
                    }
                    int l_depth = 0;
                    int l_count = 0;
                    EcellValue l_ecellValue = this.GetValueList(l_systemProperty, ref l_depth, ref l_count);
                    //
                    // 4 "EcellCoreLib"
                    //
                    if (l_ecellValue != null)
                    {
                        string l_entityPath =
                            Util.s_xpathSystem + Util.s_delimiterColon +
                            l_parentPath + Util.s_delimiterColon +
                            l_childPath + Util.s_delimiterColon +
                            l_systemPropertyName.InnerText;
                        l_simulator.LoadEntityProperty(
                            l_entityPath,
                            EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue)
                            );
                    }
                }
            }
        }
    }
}
