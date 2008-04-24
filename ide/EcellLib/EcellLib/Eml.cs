//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.IO;

using EcellCoreLib;
using EcellLib.Objects;

namespace EcellLib
{
    /// <summary>
    /// Exception on treating the "eml" formatted file.
    /// </summary>
    internal class EmlParseException : EcellXmlReaderException
    {
        public EmlParseException(string msg)
            : base(msg)
        {
        }

        public EmlParseException(string msg, Exception innerExc)
            : base(msg, innerExc)
        {
        }
    }

    /// <summary>
    /// Treats the "eml" formatted file.
    /// </summary>
    internal class EmlWriter: EcellXmlWriter
    {
        /// <summary>
        /// Creates the "Process" or "Variable" elements.
        /// </summary>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        /// <param name="l_entityName">The name of "Process" or "Variable"</param>
        private void WriteEntityElements(EcellObject l_ecellObject, string l_entityName)
        {
            m_tx.WriteStartElement(l_entityName.ToLower());
            m_tx.WriteAttributeString(Constants.xpathClass, null, l_ecellObject.Classname);
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, l_ecellObject.Name);
            if (l_ecellObject.Value != null)
                WriteDataElement(l_ecellObject);
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
            if (l_ecellObject.Value != null)
                WriteDataElement(l_ecellObject);
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Creates the "System" element.
        /// </summary>
        /// <param name="l_ecellObject">The "EcellObject"</param>
        private void WriteSystemElement(EcellObject l_ecellObject)
        {
            m_tx.WriteStartElement(Constants.xpathSystem.ToLower());
            m_tx.WriteAttributeString(Constants.xpathClass, null, l_ecellObject.Classname);
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, l_ecellObject.Key);
            if (l_ecellObject.Value != null)
                WriteDataElement(l_ecellObject);

            // 4 children
            if (l_ecellObject.Children != null && l_ecellObject.Children.Count > 0)
            {
                List<EcellObject> l_processList = new List<EcellObject>();
                List<EcellObject> l_variableList = new List<EcellObject>();
                List<EcellObject> l_textList = new List<EcellObject>();
                foreach (EcellObject l_childEcellObject in l_ecellObject.Children)
                {
                    if (l_childEcellObject.Type.Equals(Constants.xpathProcess))
                    {
                        l_processList.Add(l_childEcellObject);
                    }
                    else if (l_childEcellObject.Type.Equals(Constants.xpathVariable))
                    {
                        l_variableList.Add(l_childEcellObject);
                    }
                    else if (l_childEcellObject.Type.Equals(Constants.xpathText))
                    {
                        l_textList.Add(l_childEcellObject);
                    }
                }
                foreach (EcellObject l_textEcellObject in l_textList)
                {
                    WriteEntityElements(l_textEcellObject, Constants.xpathText);
                }
                foreach (EcellObject l_variableEcellObject in l_variableList)
                {
                    WriteEntityElements(l_variableEcellObject, Constants.xpathVariable);
                }
                foreach (EcellObject l_processEcellObject in l_processList)
                {
                    WriteEntityElements(l_processEcellObject, Constants.xpathProcess);
                }
            }
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Creates the "EcellData" elements.
        /// </summary>
        /// <param name="l_ecellObject"></param>
        private void WriteDataElement(EcellObject l_ecellObject)
        {
            foreach (EcellData l_ecellData in l_ecellObject.Value)
            {
                if (l_ecellData == null || !l_ecellData.Saveable)
                    continue;
                if (l_ecellData.Value == null
                    || (l_ecellData.Value.IsString() && l_ecellData.Value.CastToString().Length <= 0))
                    continue;

                m_tx.WriteStartElement(Constants.xpathProperty.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, l_ecellData.Name);
                WriteValueElements(l_ecellData.Value, false);
                m_tx.WriteEndElement();
            }
        }

        /// <summary>
        /// WriteStartDocument
        /// </summary>
        public void WriteStartDocument()
        {
            m_tx.WriteStartDocument(true);
            m_tx.WriteStartElement(Constants.xpathEml);
        }

        /// <summary>
        /// WriteEndDocument
        /// </summary>
        public void WriteEndDocument()
        {
            m_tx.WriteEndElement();
            m_tx.WriteEndDocument();
        }

        /// <summary>
        /// Write EML
        /// </summary>
        /// <param name="storedList"></param>
        public void Write(List<EcellObject> storedList)
        {
            foreach (EcellObject ecellObject in storedList)
            {
                if (ecellObject.Type.Equals(Constants.xpathStepper))
                {
                    WriteStepperElements(ecellObject);
                }
                else if (ecellObject.Type.Equals(Constants.xpathSystem))
                {
                    WriteSystemElement(ecellObject);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tx"></param>
        public EmlWriter(XmlTextWriter tx): base(tx)
        {
        }

        /// <summary>
        /// Creates the eml formatted file.
        /// </summary>
        /// <param name="l_fileName">The eml formatted file name</param>
        /// <param name="l_storedList">The list of the stored "EcellObject"</param>
        /// <param name="isProjectSave"></param>
        public static void Create(string l_fileName, List<EcellObject> l_storedList, bool isProjectSave)
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
                    + Constants.delimiterUnderbar + l_date + Constants.delimiterUnderbar + Path.GetFileName(l_fileName) + Constants.FileExtBackUp;
                File.Move(l_fileName, l_destFileName);
            }

            // For single model
            if (isProjectSave)
            {
                string dirName = Path.GetDirectoryName(l_fileName);
                string[] l_models = Directory.GetFileSystemEntries(
                                        dirName,
                                        Constants.delimiterWildcard + Constants.delimiterPeriod + Constants.xpathEml);
                if (l_models != null && l_models.Length > 0)
                {
                    foreach (string l_model in l_models)
                    {
                        string fileName = Path.GetFileName(l_model);
                        if (fileName.IndexOf(Constants.delimiterUnderbar) != 0)
                        {
                            string l_date
                                = File.GetLastAccessTime(l_model).ToString().Replace(
                                    Constants.delimiterColon, Constants.delimiterUnderbar);
                            l_date = l_date.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                            l_date = l_date.Replace(Constants.delimiterSpace, Constants.delimiterUnderbar);
                            string l_destFileName
                                = Path.GetDirectoryName(l_model) + Constants.delimiterPath
                                + Constants.delimiterUnderbar + l_date + Constants.delimiterUnderbar + Path.GetFileName(fileName) + Constants.FileExtBackUp;
                            File.Move(l_model, l_destFileName);
                        }
                    }
                }
            }

            //
            // Saves the model
            //
            XmlTextWriter m_tx = new XmlTextWriter(l_fileName, System.Text.Encoding.UTF8);
            try
            {
                m_tx.Formatting = Formatting.Indented;
                m_tx.Indentation = 0;
                EmlWriter ew = new EmlWriter(m_tx);
                ew.WriteStartDocument();
                ew.Write(l_storedList);                   
                ew.WriteEndDocument();
            }
            finally
            {
                m_tx.Close();
            }
        }
    }

    /// <summary>
    ///  Treats the "eml" formatted file.
    /// </summary>
    internal class EmlReader: EcellXmlReader
    {
        private XmlDocument m_doc;

        private WrappedSimulator m_simulator;

        private string m_modelID;

        private Dictionary<string, WrappedPolymorph> m_processPropertyDic;

        private bool m_isWarn = false;


        /// <summary>
        /// Creates a new "Eml" instance with no argument.
        /// </summary>
        public EmlReader(XmlDocument doc, WrappedSimulator sim, string modelID)
        {
            m_doc = doc;
            m_simulator = sim;
            m_modelID = modelID;
            m_processPropertyDic = new Dictionary<string, WrappedPolymorph>();
        }

        /// <summary>
        /// Loads the "process" or "variable" element.
        /// </summary>
        /// <param name="l_node">The "process" or "variable" element</param>
        /// <param name="l_systemID">The system ID of the parent "System" element</param>
        /// <param name="l_flag">"Process" if this element is "Process" element; "Variable" otherwise</param>
        private EcellObject ParseEntity(
            XmlNode l_node,
            string l_systemID,
            string l_flag)
        {
            bool isCreated = true;
            XmlNode l_nodeClass = l_node.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode l_nodeID = l_node.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(l_nodeClass) || !this.IsValidNode(l_nodeID))
            {
                throw new EmlParseException("Invalid entity node found");
            }
            // 4 "EcellCoreLib"
            try
            {
                m_simulator.CreateEntity(
                    l_nodeClass.InnerText,
                    Util.BuildFullID(l_flag, l_systemID, l_nodeID.InnerText));
            }
            catch (Exception l_ex)
            {
                Trace.WriteLine(l_ex.ToString());
                isCreated = false;
                m_isWarn = true;
            }
            // 4 children
            List<EcellData> l_ecellDataList = new List<EcellData>();
            XmlNodeList l_nodePropertyList = l_node.ChildNodes;
            foreach (XmlNode l_nodeProperty in l_nodePropertyList)
            {
                if (l_nodeProperty.NodeType == XmlNodeType.Whitespace)
                    continue;

                if (l_nodeProperty.NodeType != XmlNodeType.Element)
                    throw new EmlParseException("Unexpected node");
                if (!l_nodeProperty.Name.Equals(Constants.xpathProperty))
                    throw new EmlParseException(
                        String.Format(
                            "Element {0} found where {1} is expected",
                            l_nodeProperty.Name, Constants.xpathProperty));

                XmlNode l_nodePropertyName = l_nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(l_nodePropertyName))
                    continue;

                EcellValue l_ecellValue = this.GetValueList(l_nodeProperty);
                if (l_ecellValue == null)
                    continue;

                // 4 "EcellCoreLib"
                string l_entityPath =
                    l_flag + Constants.delimiterColon +
                    l_systemID + Constants.delimiterColon +
                    l_nodeID.InnerText + Constants.delimiterColon +
                    l_nodePropertyName.InnerText;
                WrappedPolymorph l_polymorph = EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue);
                if (l_flag.Equals(Constants.xpathVariable))
                {
                    if (isCreated == true)
                        m_simulator.LoadEntityProperty(l_entityPath, l_polymorph);
                }
                else
                {
                    m_processPropertyDic[l_entityPath] = l_polymorph;
                }
                EcellData l_ecellData = new EcellData(l_nodePropertyName.InnerText, l_ecellValue, l_entityPath);
                l_ecellDataList.Add(l_ecellData);
            }
            //
            // 4 "EcellLib"
            //
            return EcellObject.CreateObject(
                    m_modelID,
                    l_systemID + Constants.delimiterColon + l_nodeID.InnerText,
                    l_flag,
                    l_nodeClass.InnerText,
                    l_ecellDataList);
        }

        /// <summary>
        /// Loads the "process" or "variable" element.
        /// </summary>
        /// <param name="l_node">The "process" or "variable" element</param>
        /// <param name="l_systemID">The system ID of the parent "System" element</param>
        /// <param name="l_flag">"Process" if this element is "Process" element; "Variable" otherwise</param>
        private EcellObject ParseText(
            XmlNode l_node,
            string l_systemID,
            string l_flag)
        {
            XmlNode l_nodeClass = l_node.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode l_nodeID = l_node.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(l_nodeClass) || !this.IsValidNode(l_nodeID))
            {
                throw new EmlParseException("Invalid text node found");
            }
            // 4 children
            List<EcellData> l_ecellDataList = new List<EcellData>();
            XmlNodeList l_nodePropertyList = l_node.ChildNodes;
            foreach (XmlNode l_nodeProperty in l_nodePropertyList)
            {
                if (l_nodeProperty.NodeType == XmlNodeType.Whitespace)
                    continue;
                if (l_nodeProperty.NodeType != XmlNodeType.Element)
                    throw new EmlParseException("Unexpected node");
                if (!l_nodeProperty.Name.Equals(Constants.xpathProperty))
                    throw new EmlParseException(
                        String.Format(
                            "Element {0} found where {1} is expected",
                            l_nodeProperty.Name, Constants.xpathProperty));
                XmlNode l_nodePropertyName = l_nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(l_nodePropertyName))
                    continue;

                EcellValue l_ecellValue = new EcellValue(l_nodeProperty.InnerText);
                if (l_ecellValue == null)
                    continue;

                // 4 "EcellCoreLib"
                string l_entityPath =
                    l_flag + Constants.delimiterColon +
                    l_systemID + Constants.delimiterColon +
                    l_nodeID.InnerText + Constants.delimiterColon +
                    l_nodePropertyName.InnerText;

                EcellData l_ecellData = new EcellData(l_nodePropertyName.InnerText, l_ecellValue, l_entityPath);
                l_ecellDataList.Add(l_ecellData);
            }
            // 4 "EcellLib"
            return EcellObject.CreateObject(
                    m_modelID,
                    l_systemID + Constants.delimiterColon + l_nodeID.InnerText,
                    l_flag,
                    l_nodeClass.InnerText,
                    l_ecellDataList);
        }

        /// <summary>
        /// Parses the "Stepper" node.
        /// </summary>
        /// <param name="l_stepper">The "Stepper" node</param>
        private EcellObject ParseStepper(XmlNode l_stepper)
        {
            XmlNode l_stepperClass = l_stepper.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode l_stepperID = l_stepper.Attributes.GetNamedItem(Constants.xpathID.ToLower());

            if (!IsValidNode(l_stepperClass) || !IsValidNode(l_stepperID))
                throw new SimulationParameterParseException("Invalid stepper node found");

            try
            {
                m_simulator.CreateStepper(l_stepperClass.InnerText, l_stepperID.InnerText);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                m_isWarn = true;
            }

            //
            // 4 children
            //
            List<EcellData> l_ecellDataList = new List<EcellData>();
            foreach (XmlNode l_nodeProperty in l_stepper.ChildNodes)
            {
                if (l_nodeProperty.NodeType == XmlNodeType.Whitespace)
                    continue;

                if (l_nodeProperty.NodeType != XmlNodeType.Element)
                {
                    throw new EmlParseException("Unexpected node");
                }

                if (!l_nodeProperty.Name.Equals(Constants.xpathProperty))
                {
                    throw new EmlParseException(
                        String.Format(
                            "Element {0} found where {1} is expected",
                            l_nodeProperty.Name, Constants.xpathProperty));
                }

                XmlNode l_propertyName = l_nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(l_propertyName))
                {
                    throw new EmlParseException("Invalid property node");
                }
                EcellValue l_ecellValue = this.GetValueList(l_nodeProperty);
                if (l_ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
                    m_simulator.LoadStepperProperty(
                        l_stepperID.InnerText,
                        l_propertyName.InnerText,
                        EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue));
                    EcellData l_ecellData = new EcellData(
                            l_propertyName.InnerText, l_ecellValue,
                            l_propertyName.InnerText);
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
                m_modelID,
                l_stepperID.InnerText,
                Constants.xpathStepper,
                l_stepperClass.InnerText,
                l_ecellDataList
                );
        }

        /// <summary>
        /// Loads the "System" elements.
        /// </summary>
        private EcellObject ParseSystem(XmlNode l_systemNode)
        {
            XmlNode l_systemClass = l_systemNode.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode l_systemID = l_systemNode.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(l_systemClass) || !this.IsValidNode(l_systemID))
            {
                throw new EmlParseException("Invalid system node found");
            }
            if (Util.IsNGforSystemFullID(l_systemID.InnerText))
            {
                throw new EmlParseException("Invalid system ID");
            }
            //
            // 4 "EcellCoreLib"
            //
            string l_parentPath = l_systemID.InnerText.Substring(
                    0, l_systemID.InnerText.LastIndexOf(Constants.delimiterPath));
            string l_childPath = l_systemID.InnerText.Substring(
                    l_systemID.InnerText.LastIndexOf(Constants.delimiterPath) + 1);
            if (l_systemID.InnerText.Equals(Constants.delimiterPath))
            {
                if (l_childPath.Length == 0)
                {
                    l_childPath = Constants.delimiterPath;
                }
            }
            else
            {
                if (l_parentPath.Length == 0)
                {
                    l_parentPath = Constants.delimiterPath;
                }
                try
                {
                    m_simulator.CreateEntity(
                            l_systemClass.InnerText,
                            l_systemClass.InnerText + Constants.delimiterColon + l_parentPath
                                + Constants.delimiterColon + l_childPath);
                }
                catch (Exception e)
                {
                    throw new EmlParseException("Failed to create a System entity", e);
                }
            }

            List<EcellData> l_ecellDataList = new List<EcellData>();
            List<EcellObject> l_childEcellObjectList = new List<EcellObject>();
            XmlNodeList l_systemPropertyList = l_systemNode.ChildNodes;
            foreach (XmlNode l_systemProperty in l_systemPropertyList)
            {
                if (l_systemProperty.Name.Equals(Constants.xpathVariable.ToLower()))
                {
                    l_childEcellObjectList.Add(
                        this.ParseEntity(
                            l_systemProperty,
                            l_systemID.InnerText,
                            Constants.xpathVariable));
                }
                else if (l_systemProperty.Name.Equals(Constants.xpathProcess.ToLower()))
                {
                    l_childEcellObjectList.Add(
                        this.ParseEntity(
                            l_systemProperty,
                            l_systemID.InnerText,
                            Constants.xpathProcess));
                }
                else if (l_systemProperty.Name.Equals(Constants.xpathText.ToLower()))
                {
                    l_childEcellObjectList.Add(
                        this.ParseText(
                            l_systemProperty,
                            l_systemID.InnerText,
                            Constants.xpathText));
                }
                else if (l_systemProperty.Name.Equals(Constants.xpathProperty))
                {
                    XmlNode l_systemPropertyName = l_systemProperty.Attributes.GetNamedItem(
                            Constants.xpathName.ToLower());
                    if (!this.IsValidNode(l_systemPropertyName))
                    {
                        throw new EmlParseException("Invalid property node found");
                    }

                    EcellValue l_ecellValue = this.GetValueList(l_systemProperty);
                    if (l_ecellValue != null)
                    {
                        string l_entityPath =
                            Constants.xpathSystem + Constants.delimiterColon +
                            l_parentPath + Constants.delimiterColon +
                            l_childPath + Constants.delimiterColon +
                            l_systemPropertyName.InnerText;
                        m_simulator.LoadEntityProperty(
                                l_entityPath,
                                EcellValue.CastToWrappedPolymorph4EcellValue(l_ecellValue));
                        EcellData l_ecellData = new EcellData(
                                l_systemPropertyName.InnerText,
                                l_ecellValue,
                                l_entityPath);
                        l_ecellDataList.Add(l_ecellData);
                    }
                }
            }
            //
            // 4 EcellLib
            //
            EcellObject l_ecellObject = EcellObject.CreateObject(
                m_modelID, l_systemID.InnerText, Constants.xpathSystem, l_systemClass.InnerText,
                l_ecellDataList);
            l_ecellObject.Children = l_childEcellObjectList;
            return l_ecellObject;
        }

        public EcellObject Parse()
        {
            m_isWarn = false;
            EcellObject l_modelObject = EcellObject.CreateObject(
                    m_modelID, "", Constants.xpathModel, "", null);

            // Parse Steppers
            XmlNodeList l_stepperListNode = m_doc.SelectNodes(
                    "/" + Constants.xpathEml + "/" + Constants.xpathStepper.ToLower());
            foreach (XmlNode l_stepperNode in l_stepperListNode)
            {
                l_modelObject.Children.Add(ParseStepper(l_stepperNode));
            }

            // Parse Systems
            XmlNodeList l_systemNodeList = m_doc.SelectNodes(
                    "/" + Constants.xpathEml + "/" + Constants.xpathSystem.ToLower());
            foreach (XmlNode l_systemNode in l_systemNodeList)
            {
                l_modelObject.Children.Add(ParseSystem(l_systemNode));
            }

            List<string> removeList = new List<string>();
            foreach (KeyValuePair<string, WrappedPolymorph> pair in m_processPropertyDic)
            {
                try
                {
                    m_simulator.LoadEntityProperty(pair.Key, pair.Value);
                }
                catch (WrappedException e)
                {
                    e.ToString();
                    m_isWarn = true;
                }
                if (pair.Key.EndsWith(Constants.xpathVRL))
                    removeList.Add(pair.Key);
            }
            foreach (string entityPath in removeList)
            {
                m_processPropertyDic.Remove(entityPath);
            }
            if (m_isWarn == true)
            {
                String errmes = DataManager.s_resources.GetString("WarnLoadDM");
                MessageBox.Show(errmes + "\n",
                    "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return l_modelObject;
        }

        /// <summary>
        /// Parses the "eml" formatted file.
        /// </summary>
        /// <param name="l_fileName">The "eml" formatted file</param>
        /// <param name="sim">Simulator instance</param>
        public static EcellObject Parse(string l_fileName, WrappedSimulator sim)
        {
            XmlDocument l_doc = new XmlDocument();
            l_doc.Load(l_fileName);
            return new EmlReader(l_doc, sim, Path.GetFileNameWithoutExtension(l_fileName)).Parse();
        }
    }
}
