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
using Ecell.Objects;

namespace Ecell
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
        /// <param name="ecellObject">The "EcellObject"</param>
        /// <param name="entityName">The name of "Process" or "Variable"</param>
        private void WriteEntityElements(EcellObject ecellObject, string entityName)
        {
            m_tx.WriteStartElement(entityName.ToLower());
            m_tx.WriteAttributeString(Constants.xpathClass, null, ecellObject.Classname);
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, ecellObject.LocalID);
            if (ecellObject.Value != null)
                WriteDataElement(ecellObject);
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
            if (ecellObject.Value != null)
                WriteDataElement(ecellObject);
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Creates the "System" element.
        /// </summary>
        /// <param name="ecellObject">The "EcellObject"</param>
        private void WriteSystemElement(EcellObject ecellObject)
        {
            m_tx.WriteStartElement(Constants.xpathSystem.ToLower());
            m_tx.WriteAttributeString(Constants.xpathClass, null, ecellObject.Classname);
            m_tx.WriteAttributeString(Constants.xpathID.ToLower(), null, ecellObject.Key);
            if (ecellObject.Value != null)
                WriteDataElement(ecellObject);

            // 4 children
            if (ecellObject.Children != null && ecellObject.Children.Count > 0)
            {
                List<EcellObject> processList = new List<EcellObject>();
                List<EcellObject> variableList = new List<EcellObject>();
                List<EcellObject> textList = new List<EcellObject>();
                foreach (EcellObject childEcellObject in ecellObject.Children)
                {
                    if (childEcellObject.Type.Equals(Constants.xpathProcess))
                    {
                        processList.Add(childEcellObject);
                    }
                    else if (childEcellObject.Type.Equals(Constants.xpathVariable))
                    {
                        variableList.Add(childEcellObject);
                    }
                    else if (childEcellObject.Type.Equals(Constants.xpathText))
                    {
                        textList.Add(childEcellObject);
                    }
                }
                foreach (EcellObject variableEcellObject in variableList)
                {
                    WriteEntityElements(variableEcellObject, Constants.xpathVariable);
                }
                foreach (EcellObject textEcellObject in textList)
                {
                    WriteEntityElements(textEcellObject, Constants.xpathText);
                }
                foreach (EcellObject processEcellObject in processList)
                {
                    WriteEntityElements(processEcellObject, Constants.xpathProcess);
                }
            }
            m_tx.WriteEndElement();
        }

        /// <summary>
        /// Creates the "EcellData" elements.
        /// </summary>
        /// <param name="ecellObject"></param>
        private void WriteDataElement(EcellObject ecellObject)
        {
            foreach (EcellData ecellData in ecellObject.Value)
            {
                if (ecellData == null || !ecellData.Saveable)
                    continue;
                if (ecellData.Value == null
                    || (ecellData.Value.IsString && ((string)ecellData.Value).Length <= 0))
                    continue;

                m_tx.WriteStartElement(Constants.xpathProperty.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, ecellData.Name);
                WriteValueElements(ecellData.Value, false);
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
        /// <param name="fileName">The eml formatted file name</param>
        /// <param name="storedList">The list of the stored "EcellObject"</param>
        /// <param name="isProjectSave"></param>
        public static void Create(string fileName, List<EcellObject> storedList, bool isProjectSave)
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
                    + Constants.delimiterUnderbar + date + Constants.delimiterUnderbar + Path.GetFileName(fileName) + Constants.FileExtBackUp;
                File.Move(fileName, destFileName);
            }

            // For single model
            if (isProjectSave)
            {
                string dirName = Path.GetDirectoryName(fileName);
                string[] models = Directory.GetFileSystemEntries(
                                        dirName,
                                        Constants.delimiterWildcard + Constants.delimiterPeriod + Constants.xpathEml);
                if (models != null && models.Length > 0)
                {
                    foreach (string model in models)
                    {
                        string modelName = Path.GetFileName(model);
                        if (modelName.IndexOf(Constants.delimiterUnderbar) != 0)
                        {
                            string date
                                = File.GetLastAccessTime(model).ToString().Replace(
                                    Constants.delimiterColon, Constants.delimiterUnderbar);
                            date = date.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
                            date = date.Replace(Constants.delimiterSpace, Constants.delimiterUnderbar);
                            string destFileName
                                = Path.GetDirectoryName(model) + Constants.delimiterPath
                                + Constants.delimiterUnderbar + date + Constants.delimiterUnderbar + Path.GetFileName(modelName) + Constants.FileExtBackUp;
                            File.Move(model, destFileName);
                        }
                    }
                }
            }

            //
            // Saves the model
            //
            XmlTextWriter m_tx = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            try
            {
                m_tx.Formatting = Formatting.Indented;
                m_tx.Indentation = 0;
                EmlWriter ew = new EmlWriter(m_tx);
                ew.WriteStartDocument();
                ew.Write(storedList);                   
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
        /// <param name="node">The "process" or "variable" element</param>
        /// <param name="systemID">The system ID of the parent "System" element</param>
        /// <param name="flag">"Process" if this element is "Process" element; "Variable" otherwise</param>
        private EcellObject ParseEntity(
            XmlNode node,
            string systemID,
            string flag)
        {
            bool isCreated = true;
            XmlNode nodeClass = node.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode nodeID = node.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(nodeClass) || !this.IsValidNode(nodeID))
            {
                throw new EmlParseException("Invalid entity node found");
            }
            // 4 "EcellCoreLib"
            try
            {
                m_simulator.CreateEntity(
                    nodeClass.InnerText,
                    Util.BuildFullID(flag, systemID, nodeID.InnerText));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                isCreated = false;
                m_isWarn = true;
            }
            // 4 children
            List<EcellData> ecellDataList = new List<EcellData>();
            XmlNodeList nodePropertyList = node.ChildNodes;
            foreach (XmlNode nodeProperty in nodePropertyList)
            {
                if (nodeProperty.NodeType == XmlNodeType.Whitespace)
                    continue;

                if (nodeProperty.NodeType != XmlNodeType.Element)
                    throw new EmlParseException("Unexpected node");
                if (!nodeProperty.Name.Equals(Constants.xpathProperty))
                    throw new EmlParseException(
                        String.Format(
                            "Element {0} found where {1} is expected",
                            nodeProperty.Name, Constants.xpathProperty));

                XmlNode nodePropertyName = nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(nodePropertyName))
                    continue;

                EcellValue ecellValue = this.GetValueList(nodeProperty);
                if (ecellValue == null)
                    continue;

                // 4 "EcellCoreLib"
                string entityPath =
                    flag + Constants.delimiterColon +
                    systemID + Constants.delimiterColon +
                    nodeID.InnerText + Constants.delimiterColon +
                    nodePropertyName.InnerText;
                WrappedPolymorph polymorph = ecellValue.ToWrappedPolymorph();
                if (flag.Equals(Constants.xpathVariable))
                {
                    if (isCreated == true)
                        m_simulator.LoadEntityProperty(entityPath, polymorph);
                }
                else
                {
                    m_processPropertyDic[entityPath] = polymorph;
                }
                EcellData ecellData = new EcellData(nodePropertyName.InnerText, ecellValue, entityPath);
                ecellDataList.Add(ecellData);
            }
            //
            // 4 "EcellLib"
            //
            return EcellObject.CreateObject(
                    m_modelID,
                    systemID + Constants.delimiterColon + nodeID.InnerText,
                    flag,
                    nodeClass.InnerText,
                    ecellDataList);
        }

        /// <summary>
        /// Loads the "process" or "variable" element.
        /// </summary>
        /// <param name="node">The "process" or "variable" element</param>
        /// <param name="systemID">The system ID of the parent "System" element</param>
        /// <param name="flag">"Process" if this element is "Process" element; "Variable" otherwise</param>
        private EcellObject ParseText(
            XmlNode node,
            string systemID,
            string flag)
        {
            XmlNode nodeClass = node.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode nodeID = node.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(nodeClass) || !this.IsValidNode(nodeID))
            {
                throw new EmlParseException("Invalid text node found");
            }
            // 4 children
            List<EcellData> ecellDataList = new List<EcellData>();
            XmlNodeList nodePropertyList = node.ChildNodes;
            foreach (XmlNode nodeProperty in nodePropertyList)
            {
                if (nodeProperty.NodeType == XmlNodeType.Whitespace)
                    continue;
                if (nodeProperty.NodeType != XmlNodeType.Element)
                    throw new EmlParseException("Unexpected node");
                if (!nodeProperty.Name.Equals(Constants.xpathProperty))
                    throw new EmlParseException(
                        String.Format(
                            "Element {0} found where {1} is expected",
                            nodeProperty.Name, Constants.xpathProperty));
                XmlNode nodePropertyName = nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(nodePropertyName))
                    continue;

                EcellValue ecellValue = new EcellValue(nodeProperty.InnerText);
                if (ecellValue == null)
                    continue;

                // 4 "EcellCoreLib"
                string entityPath =
                    flag + Constants.delimiterColon +
                    systemID + Constants.delimiterColon +
                    nodeID.InnerText + Constants.delimiterColon +
                    nodePropertyName.InnerText;

                EcellData ecellData = new EcellData(nodePropertyName.InnerText, ecellValue, entityPath);
                ecellDataList.Add(ecellData);
            }
            // 4 "EcellLib"
            return EcellObject.CreateObject(
                    m_modelID,
                    systemID + Constants.delimiterColon + nodeID.InnerText,
                    flag,
                    nodeClass.InnerText,
                    ecellDataList);
        }

        /// <summary>
        /// Parses the "Stepper" node.
        /// </summary>
        /// <param name="stepper">The "Stepper" node</param>
        private EcellObject ParseStepper(XmlNode stepper)
        {
            XmlNode stepperClass = stepper.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode stepperID = stepper.Attributes.GetNamedItem(Constants.xpathID.ToLower());

            if (!IsValidNode(stepperClass) || !IsValidNode(stepperID))
                throw new SimulationParameterParseException("Invalid stepper node found");

            try
            {
                m_simulator.CreateStepper(stepperClass.InnerText, stepperID.InnerText);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                m_isWarn = true;
            }

            //
            // 4 children
            //
            List<EcellData> ecellDataList = new List<EcellData>();
            foreach (XmlNode nodeProperty in stepper.ChildNodes)
            {
                if (nodeProperty.NodeType == XmlNodeType.Whitespace)
                    continue;

                if (nodeProperty.NodeType != XmlNodeType.Element)
                {
                    throw new EmlParseException("Unexpected node");
                }

                if (!nodeProperty.Name.Equals(Constants.xpathProperty))
                {
                    throw new EmlParseException(
                        String.Format(
                            "Element {0} found where {1} is expected",
                            nodeProperty.Name, Constants.xpathProperty));
                }

                XmlNode propertyName = nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(propertyName))
                {
                    throw new EmlParseException("Invalid property node");
                }
                EcellValue ecellValue = this.GetValueList(nodeProperty);
                if (ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
                    m_simulator.LoadStepperProperty(
                        stepperID.InnerText,
                        propertyName.InnerText,
                        ecellValue.ToWrappedPolymorph());
                    EcellData ecellData = new EcellData(
                            propertyName.InnerText, ecellValue,
                            propertyName.InnerText);
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
                m_modelID,
                stepperID.InnerText,
                Constants.xpathStepper,
                stepperClass.InnerText,
                ecellDataList
                );
        }

        /// <summary>
        /// Loads the "System" elements.
        /// </summary>
        private EcellObject ParseSystem(XmlNode systemNode)
        {
            XmlNode systemClass = systemNode.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode systemID = systemNode.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(systemClass) || !this.IsValidNode(systemID))
            {
                throw new EmlParseException("Invalid system node found");
            }
            if (Util.IsNGforSystemFullID(systemID.InnerText))
            {
                throw new EmlParseException("Invalid system ID");
            }
            //
            // 4 "EcellCoreLib"
            //
            string parentPath = systemID.InnerText.Substring(
                    0, systemID.InnerText.LastIndexOf(Constants.delimiterPath));
            string childPath = systemID.InnerText.Substring(
                    systemID.InnerText.LastIndexOf(Constants.delimiterPath) + 1);
            if (systemID.InnerText.Equals(Constants.delimiterPath))
            {
                if (childPath.Length == 0)
                {
                    childPath = Constants.delimiterPath;
                }
            }
            else
            {
                if (parentPath.Length == 0)
                {
                    parentPath = Constants.delimiterPath;
                }
                try
                {
                    m_simulator.CreateEntity(
                            systemClass.InnerText,
                            systemClass.InnerText + Constants.delimiterColon + parentPath
                                + Constants.delimiterColon + childPath);
                }
                catch (Exception e)
                {
                    throw new EmlParseException("Failed to create a System entity", e);
                }
            }

            List<EcellData> ecellDataList = new List<EcellData>();
            List<EcellObject> childEcellObjectList = new List<EcellObject>();
            XmlNodeList systemPropertyList = systemNode.ChildNodes;
            foreach (XmlNode systemProperty in systemPropertyList)
            {
                if (systemProperty.Name.Equals(Constants.xpathVariable.ToLower()))
                {
                    childEcellObjectList.Add(
                        this.ParseEntity(
                            systemProperty,
                            systemID.InnerText,
                            Constants.xpathVariable));
                }
                else if (systemProperty.Name.Equals(Constants.xpathProcess.ToLower()))
                {
                    childEcellObjectList.Add(
                        this.ParseEntity(
                            systemProperty,
                            systemID.InnerText,
                            Constants.xpathProcess));
                }
                else if (systemProperty.Name.Equals(Constants.xpathText.ToLower()))
                {
                    childEcellObjectList.Add(
                        this.ParseText(
                            systemProperty,
                            systemID.InnerText,
                            Constants.xpathText));
                }
                else if (systemProperty.Name.Equals(Constants.xpathProperty))
                {
                    XmlNode systemPropertyName = systemProperty.Attributes.GetNamedItem(
                            Constants.xpathName.ToLower());
                    if (!this.IsValidNode(systemPropertyName))
                    {
                        throw new EmlParseException("Invalid property node found");
                    }

                    EcellValue ecellValue = this.GetValueList(systemProperty);
                    if (ecellValue != null)
                    {
                        string entityPath =
                            Constants.xpathSystem + Constants.delimiterColon +
                            parentPath + Constants.delimiterColon +
                            childPath + Constants.delimiterColon +
                            systemPropertyName.InnerText;
                        m_simulator.LoadEntityProperty(
                                entityPath,
                                ecellValue.ToWrappedPolymorph());
                        EcellData ecellData = new EcellData(
                                systemPropertyName.InnerText,
                                ecellValue,
                                entityPath);
                        ecellDataList.Add(ecellData);
                    }
                }
            }
            //
            // 4 EcellLib
            //
            EcellObject ecellObject = EcellObject.CreateObject(
                m_modelID, systemID.InnerText, Constants.xpathSystem, systemClass.InnerText,
                ecellDataList);
            ecellObject.Children = childEcellObjectList;
            return ecellObject;
        }

        public EcellObject Parse()
        {
            m_isWarn = false;
            EcellObject modelObject = EcellObject.CreateObject(
                    m_modelID, "", Constants.xpathModel, "", null);

            // Parse Steppers
            XmlNodeList stepperListNode = m_doc.SelectNodes(
                    "/" + Constants.xpathEml + "/" + Constants.xpathStepper.ToLower());
            foreach (XmlNode stepperNode in stepperListNode)
            {
                modelObject.Children.Add(ParseStepper(stepperNode));
            }

            // Parse Systems
            XmlNodeList systemNodeList = m_doc.SelectNodes(
                    "/" + Constants.xpathEml + "/" + Constants.xpathSystem.ToLower());
            foreach (XmlNode systemNode in systemNodeList)
            {
                modelObject.Children.Add(ParseSystem(systemNode));
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
                String errmes = MessageResources.WarnLoadDM;
                Util.ShowWarningDialog(errmes + "\n");
            }

            return modelObject;
        }

        /// <summary>
        /// Parses the "eml" formatted file.
        /// </summary>
        /// <param name="fileName">The "eml" formatted file</param>
        /// <param name="sim">Simulator instance</param>
        public static EcellObject Parse(string fileName, WrappedSimulator sim)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return new EmlReader(doc, sim, Path.GetFileNameWithoutExtension(fileName)).Parse();
        }
    }
}
