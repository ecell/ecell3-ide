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
    public class EmlParseException : EcellXmlReaderException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msg">the error message.</param>
        public EmlParseException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="msg">the error message.</param>
        /// <param name="innerExc">the inner exception.</param>
        public EmlParseException(string msg, Exception innerExc)
            : base(msg, innerExc)
        {
        }
    }

    /// <summary>
    /// Treats the "eml" formatted file.
    /// </summary>
    public class EmlWriter : EcellXmlWriter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tx">Text writer object.</param>
        public EmlWriter(XmlTextWriter tx)
            : base(tx)
        {
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
        /// <param name="storedList">the list of object to write EML.</param>
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
        /// Creates the eml formatted file.
        /// </summary>
        /// <param name="fileName">The eml formatted file name</param>
        /// <param name="storedList">The list of the stored "EcellObject"</param>
        /// <param name="isProjectSave">whether project is saved.</param>
        public static void Create(string fileName, List<EcellObject> storedList, bool isProjectSave)
        {
            //
            // Checks the old model file.
            //
            if (File.Exists(fileName))
            {
                BackUpModel(fileName);
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
                        if (Path.GetFileName(model).IndexOf(Constants.delimiterUnderbar) != 0)
                        {
                            BackUpModel(model);
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

        /// <summary>
        /// BackUpModel
        /// </summary>
        /// <param name="fileName">the model file name.</param>
        private static void BackUpModel(string fileName)
        {
            string date
                = File.GetLastAccessTime(fileName).ToString().Replace(
                    Constants.delimiterColon, Constants.delimiterUnderbar);
            date = date.Replace(Constants.delimiterPath, Constants.delimiterUnderbar);
            date = date.Replace(Constants.delimiterSpace, Constants.delimiterUnderbar);
            string destFileName
                = Path.GetDirectoryName(fileName) + Constants.delimiterPath
                + Constants.delimiterUnderbar + date + Constants.delimiterUnderbar + Path.GetFileName(fileName) + Constants.FileExtBackUp;
            if (File.Exists(destFileName))
                destFileName = Util.GetNewFileName(destFileName);
            File.Move(fileName, destFileName);
        }

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
        /// <param name="ecellObject">the wrote object.</param>
        private void WriteDataElement(EcellObject ecellObject)
        {
            foreach (EcellData ecellData in ecellObject.Value)
            {
                if (!ecellData.Saveable)
                    continue;
                if (ecellData.Value == null ||
                    (ecellData.Value.IsString && ecellData.Value.ToString().Length <= 0))
                    continue;

                m_tx.WriteStartElement(Constants.xpathProperty.ToLower());
                m_tx.WriteAttributeString(Constants.xpathName.ToLower(), null, ecellData.Name);
                WriteValueElements(ecellData.Value);
                m_tx.WriteEndElement();
            }
        }

    }

    /// <summary>
    ///  Treats the "eml" formatted file.
    /// </summary>
    public class EmlReader : EcellXmlReader
    {
        /// <summary>
        /// Documant object for EML.
        /// </summary>
        private XmlDocument m_doc;
        /// <summary>
        /// Model ID.
        /// </summary>
        private string m_modelID;

        /// <summary>
        /// Creates a new "Eml" instance with no argument.
        /// </summary>
        /// <param name="filename">the EML file name.</param>
        /// <param name="sim">The current simulator.</param>
        public EmlReader(string filename, WrappedSimulator sim)
        {
            m_doc = new XmlDocument();
            m_doc.Load(filename);
            m_modelID = Path.GetFileNameWithoutExtension(filename);
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
            XmlNode nodeClass = node.Attributes.GetNamedItem(Constants.xpathClass);
            XmlNode nodeID = node.Attributes.GetNamedItem(Constants.xpathID.ToLower());
            if (!this.IsValidNode(nodeClass) || !this.IsValidNode(nodeID))
            {
                throw new EmlParseException("Invalid entity node found");
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
                        string.Format(
                            "Element {0} found where {1} is expected",
                            nodeProperty.Name, Constants.xpathProperty));

                XmlNode nodePropertyName = nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(nodePropertyName))
                    continue;

                EcellValue ecellValue = this.ParseEcellValue(nodeProperty);
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
                        string.Format(
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
                        string.Format(
                            "Element {0} found where {1} is expected",
                            nodeProperty.Name, Constants.xpathProperty));
                }

                XmlNode propertyName = nodeProperty.Attributes.GetNamedItem(Constants.xpathName.ToLower());
                if (!this.IsValidNode(propertyName))
                {
                    throw new EmlParseException("Invalid property node");
                }
                EcellValue ecellValue = this.ParseEcellValue(nodeProperty);
                if (ecellValue != null)
                {
                    //
                    // 4 "EcellCoreLib"
                    //
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
            if (Util.IsNGforSystemKey(systemID.InnerText))
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
            }

            List<EcellData> ecellDataList = new List<EcellData>();
            List<EcellObject> childEcellObjectList = new List<EcellObject>();
            XmlNodeList systemPropertyList = systemNode.ChildNodes;
            foreach (XmlNode systemProperty in systemPropertyList)
            {
                if (systemProperty.Name.Equals(Constants.xpathVariable.ToLower()))
                {
                    //
                    // Parse Variable
                    //
                    EcellObject variable = ParseEntity(
                            systemProperty,
                            systemID.InnerText,
                            Constants.xpathVariable);
                    childEcellObjectList.Add(variable);
                }
                else if (systemProperty.Name.Equals(Constants.xpathProcess.ToLower()))
                {
                    //
                    // Parse Process
                    //
                    EcellObject process = ParseEntity(
                            systemProperty,
                            systemID.InnerText,
                            Constants.xpathProcess);
                    NormalizeVariableReferences(process);
                    childEcellObjectList.Add(process);
                }
                else if (systemProperty.Name.Equals(Constants.xpathText.ToLower()))
                {
                    //
                    // Parse Text
                    //
                    EcellObject text = ParseText(
                            systemProperty,
                            systemID.InnerText,
                            Constants.xpathText);
                    childEcellObjectList.Add(text);
                }
                else if (systemProperty.Name.Equals(Constants.xpathProperty))
                {
                    XmlNode systemPropertyName = systemProperty.Attributes.GetNamedItem(
                            Constants.xpathName.ToLower());
                    if (!this.IsValidNode(systemPropertyName))
                    {
                        throw new EmlParseException("Invalid property node found");
                    }

                    EcellValue ecellValue = this.ParseEcellValue(systemProperty);
                    if (ecellValue != null)
                    {
                        string entityPath =
                            Constants.xpathSystem + Constants.delimiterColon +
                            parentPath + Constants.delimiterColon +
                            childPath + Constants.delimiterColon +
                            systemPropertyName.InnerText;

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

        /// <summary>
        /// Normalize VariableReferences
        /// </summary>
        /// <param name="eo">The normalized object.</param>
        private static void NormalizeVariableReferences(EcellObject eo)
        {
            EcellProcess process = (EcellProcess)eo;
            List<EcellReference> list = process.ReferenceList;
            string superSystemPath = process.ParentSystemID;
            foreach (EcellReference vr in list)
                Util.NormalizeVariableReference(vr, superSystemPath);
            process.ReferenceList = list;
        }

        /// <summary>
        /// Parse the file to the model.
        /// </summary>
        /// <returns>the model object.</returns>
        public EcellModel Parse()
        {
            EcellModel modelObject = (EcellModel)EcellObject.CreateObject(
                    m_modelID, "", Constants.xpathModel, "", new List<EcellData>());

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

            return modelObject;
        }

        /// <summary>
        /// Parses the "eml" formatted file.
        /// </summary>
        /// <param name="fileName">The "eml" formatted file</param>
        /// <param name="sim">Simulator instance</param>
        public static EcellModel Parse(string fileName, WrappedSimulator sim)
        {
            EmlReader reader = new EmlReader(fileName, sim);
            EcellModel model = reader.Parse();
            InitializeModel(model, sim);
            return model;
        }

        /// <summary>
        /// Initialize the model
        /// </summary>
        /// <param name="modelObject">the model object.</param>
        /// <param name="simulator">the loaded simulator.</param>
        public static void InitializeModel(EcellModel modelObject, WrappedSimulator simulator)
        {
            bool isWarn = false;
            string errMsg = MessageResources.WarnLoadDM + "\n";
            Dictionary<string, object> processPropertyDic = new Dictionary<string, object>();

            // Initialize object
            foreach (EcellObject obj in modelObject.Children)
            {
                // Initialize Stepper
                if (obj is EcellStepper)
                {
                    try
                    {
                        simulator.CreateStepper(obj.Classname, obj.Key);
                    }
                    catch (Exception e)
                    {
                        errMsg += obj.FullID + ":" + e.Message + "\n";
                        Trace.WriteLine(e.ToString());
                        isWarn = true;
                    }
                    foreach (EcellData data in obj.Value)
                    {
                        simulator.LoadStepperProperty(
                            obj.Key,
                            data.Name,
                            data.Value.Value);
                    }
                }
                else if (obj is EcellSystem)
                {
                    // Initialize System
                    if (!obj.Key.Equals(Constants.delimiterPath))
                    {
                        try
                        {
                            simulator.CreateEntity(
                                    obj.Classname,
                                    obj.FullID);
                        }
                        catch (Exception e)
                        {
                            throw new EmlParseException("Failed to create a System entity", e);
                        }
                    }
                    foreach (EcellData data in obj.Value)
                    {
                        simulator.LoadEntityProperty(
                            data.EntityPath,
                            data.Value.Value);
                    }

                    // Initialize Entity
                    foreach (EcellObject entity in obj.Children)
                    {
                        if (entity.Type.Equals(EcellObject.TEXT))
                            continue;
                        bool isCreated = true;
                        // 4 "EcellCoreLib"
                        try
                        {
                            simulator.CreateEntity(
                                entity.Classname,
                                entity.FullID);
                        }
                        catch (Exception e)
                        {
                            errMsg += entity.FullID + ":" + e.Message + "\n";
                            Trace.WriteLine(e.ToString());
                            isCreated = false;
                            isWarn = true;
                        }

                        foreach (EcellData data in entity.Value)
                        {
                            string entityPath = data.EntityPath;
                            if (obj.Type.Equals(Constants.xpathVariable))
                            {
                                if (isCreated == true)
                                    simulator.LoadEntityProperty(entityPath, data.Value.Value);
                            }
                            else
                            {
                                processPropertyDic[entityPath] = data.Value.Value;
                            }
                        }
                    }
                }
            }
            // 
            List<string> removeList = new List<string>();
            foreach (KeyValuePair<string, object> pair in processPropertyDic)
            {
                try
                {
                    simulator.LoadEntityProperty(pair.Key, pair.Value);
                }
                catch (WrappedException e)
                {
                    isWarn = true;
                    errMsg += pair.Key + ":" + e.Message + "\n";
                }
                if (pair.Key.EndsWith(Constants.xpathVRL))
                    removeList.Add(pair.Key);
            }
            foreach (string entityPath in removeList)
            {
                processPropertyDic.Remove(entityPath);
            }
            if (isWarn)
            {
                modelObject.ErrMsg = errMsg;
            }

        }
    }
}
