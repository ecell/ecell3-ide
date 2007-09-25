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
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace EcellLib
{
    /// <summary>
    /// Class to manager actions.
    /// </summary>
    public class ActionManager
    {
        #region Fields
        /// <summary>
        /// List of user action on IDE.
        /// </summary>
        private List<UserAction> m_list;
        /// <summary>
        /// An index of m_list for undoing and redoing.
        /// </summary>
        private int m_listIndex;
        /// <summary>
        /// s_instance (singleton instance)
        /// </summary>
        private static ActionManager s_instance = null;
        /// <summary>
        /// Whether duaring load the actions or others.
        /// </summary>
        private bool isLoadAction = false;
        #endregion

        /// <summary>
        /// Constructor for ActionManager.
        /// </summary>
        public ActionManager()
        {
            m_list = new List<UserAction>();
        }

        /// <summary>
        /// Get the count of UserAction.
        /// </summary>
        public int Count
        {
            get { return m_list.Count; }
        }

        /// <summary>
        /// Add the UserAction to ActionManager.
        /// </summary>
        /// <param name="u">The adding UserAction.</param>
        public void AddAction(UserAction u)
        {
            if (isLoadAction == true) return;

            if (m_list.Count > m_listIndex)
                m_list.RemoveRange(m_listIndex, m_list.Count - m_listIndex);
            
            m_list.Add(u);
            m_listIndex++;
            NotifyStatus();
        }

        /// <summary>
        /// Get the UserAction from ActionManager by index.
        /// </summary>
        /// <param name="index">The index of UserAction.</param>
        /// <returns>The target UserAction.</returns>
        public UserAction GetAction(int index)
        {
            if (index > m_list.Count || index < 0) return null;
            return m_list[index];
        }

        /// <summary>
        /// Clear the UserAction of ActionManager.
        /// </summary>
        public void Clear()
        {
            m_list.Clear();
            m_listIndex = 0;
        }

        /// <summary>
        /// Load the information of UserAction and 
        /// execute the UserActions.
        /// </summary>
        /// <param name="fileName">File name of UserActions.</param>
        public void LoadActionFile(string fileName)
        {
            isLoadAction = true;
            XmlDocument l_doc = new XmlDocument();
            l_doc.Load(fileName);

            XmlNodeList topList = l_doc.SelectNodes("ActionList");
            foreach (XmlNode top in topList)
            {
                XmlNodeList commandList = top.SelectNodes("Action");

                foreach (XmlNode child in commandList)
                {
                    XmlNode cNode = child.Attributes.GetNamedItem("command");
                    if (cNode == null) continue;
                    string command = cNode.InnerText;
                    UserAction act = null;
                    if (command.Equals("DataAdd")) act = new DataAddAction();
                    else if (command.Equals("DataDelete")) act = new DataDeleteAction();
                    else if (command.Equals("DataChanged")) act = new DataChangeAction();
                    else if (command.Equals("ImportModel")) act = new ImportModelAction();
                    else if (command.Equals("NewProject")) act = new NewProjectAction();
                    else if (command.Equals("LoadProject")) act = new LoadProjectAction();
                    else if (command.Equals("AddStepper")) act = new AddStepperAction();
                    else if (command.Equals("DeleteStepper")) act = new DeleteStepperAction();
                    else if (command.Equals("UpdateStepper")) act = new UpdateStepperAction();
                    else if (command.Equals("NewSimParam")) act = new NewSimParamAction();
                    else if (command.Equals("DeleteSimParam")) act = new DeleteSimParamAction();
                    else if (command.Equals("SetSimParam")) act = new SetSimParamAction();
                    else if (command.Equals("SystemMerge")) act = new SystemMergeAction();

                    if (act != null)
                    {
                        act.LoadScript(child);
                        m_list.Add(act);
                    }
                }
            }
            l_doc = null;

            foreach (UserAction u in m_list)
                u.Execute();
            isLoadAction = false;

            m_listIndex = m_list.Count;
        }

        /// <summary>
        /// Save the information of UserAction to the file.
        /// </summary>
        /// <param name="fileName">Saved file name.</param>
        public void SaveActionFile(string fileName)
        {
            TextWriter streamWriter =
                                new StreamWriter(fileName);
            XmlTextWriter w = new XmlTextWriter(streamWriter);
            w.Formatting = Formatting.Indented;

            w.WriteStartElement("ActionList");
            foreach (UserAction u in m_list)
                u.SaveScript(w);
            w.WriteEndElement();

            w.Close();
            streamWriter.Close();
        }

        /// <summary>
        /// Get ActionManager with using singleton pattern.
        /// </summary>
        /// <returns>The ActionManager in application.</returns>
        public static ActionManager GetActionManager()
        {
            if (s_instance == null)
            {
                s_instance = new ActionManager();
            }
            return s_instance;
        }

        /// <summary>
        /// Undo action.
        /// </summary>
        public void UndoAction()
        {
            if (m_listIndex == 0 || !m_list[m_listIndex - 1].IsUndoable)
                return;
            do
            {
                m_list[m_listIndex - 1].UnExecute();
                //Console.WriteLine("index:" + (m_listIndex - 1) + " unexecute");
                m_listIndex--;

            } while (0 < m_listIndex && !m_list[m_listIndex - 1].IsAnchor);
            NotifyStatus();
        }

        /// <summary>
        /// Redo action.
        /// </summary>
        public void RedoAction()
        {
            if (m_list.Count <= m_listIndex)
                return;
            do
            {
                m_list[m_listIndex].Execute();
                //Console.WriteLine("index:" + m_listIndex + " execute");
                m_listIndex++;

            } while (m_listIndex < m_list.Count && !m_list[m_listIndex - 1].IsAnchor);
            NotifyStatus();
        }

        /// <summary>
        /// Notify undoable/redoable status to PluginManager
        /// </summary>
        private void NotifyStatus()
        {
            bool l_undoable = false;
            if (0 < m_listIndex && m_list[m_listIndex - 1].IsUndoable)
                l_undoable = true;

            bool l_redoable = false;
            if (m_listIndex < m_list.Count)
                l_redoable = true;

            PluginManager l_pManager = PluginManager.GetPluginManager();
            if (l_undoable && l_redoable)
                l_pManager.ChangeUndoStatus(UndoStatus.UNDO_REDO);
            else if (l_undoable)
                l_pManager.ChangeUndoStatus(UndoStatus.UNDO_ONLY);
            else if (l_redoable)
                l_pManager.ChangeUndoStatus(UndoStatus.REDO_ONLY);
            else
                l_pManager.ChangeUndoStatus(UndoStatus.NOTHING);
        }
    }

    /// <summary>
    /// Abstract class for action.
    /// </summary>
    public abstract class UserAction
    {
        /// <summary>
        /// Whether this UserAction is the last one in a sequence of UserAction.
        /// </summary>
        protected bool m_isAnchor = true;
        /// <summary>
        /// Whether this UserAction is undoable or not.
        /// </summary>
        protected bool m_isUndoable = true;
        /// <summary>
        /// Whether this UserAction is the last one in a sequence of UserAction.
        /// </summary>
        public bool IsAnchor
        {
            get { return m_isAnchor; }
            set { m_isAnchor = value; }
        }
        /// <summary>
        /// Whether this UserAction is undoable or not.
        /// </summary>
        public bool IsUndoable
        {
            get { return m_isUndoable; }
        }
        /// <summary>
        /// Abstract function of UserAction.
        /// </summary>
        /// <param name="write">The object for writing the xml file.</param>
        public abstract void SaveScript(XmlTextWriter write);
        /// <summary>
        /// Abstract function of UserAction.
        /// </summary>
        /// <param name="node">The node object to load.</param>
        public abstract void LoadScript(XmlNode node);
        /// <summary>
        /// Abstract function of UserAction.
        /// Execute action of this UserAction.
        /// </summary>
        public abstract void Execute();
        /// <summary>
        /// Abstract function of UserAction.
        /// Unexecute action of this UserAction.
        /// </summary>
        public abstract void UnExecute();
        /// <summary>
        /// Convert the xml node to the EcellObject.
        /// </summary>
        /// <param name="node">The xml node.</param>
        /// <returns>EcellObject.</returns>
        static public EcellObject LoadObject(XmlNode node)
        {
            XmlNode tmp = node.Attributes.GetNamedItem("model");
            string modelID = tmp.InnerText;
            tmp = node.Attributes.GetNamedItem("type");
            string type = tmp.InnerText;
            tmp = node.Attributes.GetNamedItem("key");
            string key = tmp.InnerText;
            tmp = node.Attributes.GetNamedItem("classname");
            string classname = tmp.InnerText;
            tmp = node.Attributes.GetNamedItem("x");
            float x = float.Parse(tmp.InnerText);
            tmp = node.Attributes.GetNamedItem("y");
            float y = float.Parse(tmp.InnerText);
            tmp = node.Attributes.GetNamedItem("offsetx");
            float offsetx = float.Parse(tmp.InnerText);
            tmp = node.Attributes.GetNamedItem("offsety");
            float offsety = float.Parse(tmp.InnerText);
            tmp = node.Attributes.GetNamedItem("width");
            float width = float.Parse(tmp.InnerText);
            tmp = node.Attributes.GetNamedItem("height");
            float height = float.Parse(tmp.InnerText);

            XmlNodeList dataList = node.SelectNodes("Data");
            List<EcellData> list = new List<EcellData>();
            foreach (XmlNode data in dataList)
            {
                EcellData d = new EcellData();
                tmp = data.Attributes.GetNamedItem("name");
                d.M_name = tmp.InnerText;
                tmp = data.Attributes.GetNamedItem("path");
                d.M_entityPath = tmp.InnerText;

                tmp = data.Attributes.GetNamedItem("isGetable");
                if (tmp.InnerText == "true") d.M_isGettable = true;
                else d.M_isGettable = false;

                tmp = data.Attributes.GetNamedItem("isLoadable");
                if (tmp.InnerText == "true") d.M_isLoadable = true;
                else d.M_isLoadable = false;

                tmp = data.Attributes.GetNamedItem("isLogable");
                if (tmp.InnerText == "true") d.M_isLogable = true;
                else d.M_isLogable = false;

                tmp = data.Attributes.GetNamedItem("isLogger");
                if (tmp.InnerText == "true") d.M_isLogger = true;
                else d.M_isLogger = false;

                tmp = data.Attributes.GetNamedItem("isSavable");
                if (tmp.InnerText == "true") d.M_isSavable = true;
                else d.M_isSavable = false;

                tmp = data.Attributes.GetNamedItem("isSetable");
                if (tmp.InnerText == "true") d.M_isSettable = true;
                else d.M_isSettable = false;

                XmlNodeList valueList = data.SelectNodes("Value");
                foreach (XmlNode value in valueList)
                {
                    tmp = value.Attributes.GetNamedItem("value_type");
                    string vtype = tmp.InnerText;
                    tmp = value.Attributes.GetNamedItem("value");
                    string valueData = tmp.InnerText;
                    EcellValue v;
                    if (vtype.Equals(typeof(string).ToString())) v = new EcellValue(valueData);
                    else if (vtype.Equals(typeof(double).ToString()))
                    {
                        if (valueData == "1.79769313486232E+308")
                            v = new EcellValue(Double.MaxValue);
                        else 
                            v = new EcellValue(Convert.ToDouble(valueData));
                    }
                    else if (vtype.Equals(typeof(int).ToString())) v = new EcellValue(Convert.ToInt32(valueData));
                    else v = EcellValue.ToVariableReferenceList(valueData);
                    d.M_value = v;
                }

                list.Add(d);
            }
            EcellObject obj = EcellObject.CreateObject(modelID, key, type, classname, list);
            obj.X = x;
            obj.Y = y;
            obj.OffsetX = offsetx;
            obj.OffsetY = offsety;
            obj.Width = width;
            obj.Height = height;

            obj.M_instances = new List<EcellObject>();

            return obj;
        }
        /// <summary>
        /// Write the information of EcellObject by xml format.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        /// <param name="m_obj">The wrote object.</param>
        static public void WriteObject(XmlTextWriter writer, EcellObject m_obj)
        {
            writer.WriteAttributeString("model", null, m_obj.modelID);
            writer.WriteAttributeString("type", null, m_obj.type);
            writer.WriteAttributeString("key", null, m_obj.key);
            writer.WriteAttributeString("classname", null, m_obj.classname);
            writer.WriteAttributeString("x", null, Convert.ToString(m_obj.X));
            writer.WriteAttributeString("y", null, Convert.ToString(m_obj.Y));
            writer.WriteAttributeString("offsetx", null, Convert.ToString(m_obj.OffsetX));
            writer.WriteAttributeString("offsety", null, Convert.ToString(m_obj.OffsetY));
            writer.WriteAttributeString("width", null, Convert.ToString(m_obj.Width));
            writer.WriteAttributeString("height", null, Convert.ToString(m_obj.Height));

            if (m_obj.M_value != null)
            {
                foreach (EcellData d in m_obj.M_value)
                {
                    writer.WriteStartElement("Data");
                    writer.WriteAttributeString("name", null, d.M_name);
                    writer.WriteAttributeString("path", null, d.M_entityPath);
                    if (d.M_isGettable == true) writer.WriteAttributeString("isGetable", null, "true");
                    else writer.WriteAttributeString("isGetable", null, "false");
                    if (d.M_isLoadable == true) writer.WriteAttributeString("isLoadable", null, "true");
                    else writer.WriteAttributeString("isLoadable", null, "false");
                    if (d.M_isLogable == true) writer.WriteAttributeString("isLogable", null, "true");
                    else writer.WriteAttributeString("isLogable", null, "false");
                    if (d.M_isLogger == true) writer.WriteAttributeString("isLogger", null, "true");
                    else writer.WriteAttributeString("isLogger", null, "false");
                    if (d.M_isSavable == true) writer.WriteAttributeString("isSavable", null, "true");
                    else writer.WriteAttributeString("isSavable", null, "false");
                    if (d.M_isSettable == true) writer.WriteAttributeString("isSetable", null, "true");
                    else writer.WriteAttributeString("isSetable", null, "false");
                    writer.WriteStartElement("Value");
                    if (d.M_value != null)
                    {
                        writer.WriteAttributeString("value_type", null, d.M_value.M_type.ToString());
                        writer.WriteAttributeString("value", null, d.M_value.M_value.ToString());
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
        }
    }

    /// <summary>
    /// Action class to import the model.
    /// </summary>
    public class ImportModelAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The file name to import the model information.
        /// </summary>
        private string m_fileName;
        #endregion

        /// <summary>
        /// The constructor for ImportModelAction.
        /// </summary>
        public ImportModelAction()
        {
            m_isUndoable = false;
        }
        /// <summary>
        /// The constructor for ImportModelAction with initial parameters.
        /// </summary>
        /// <param name="fileName">The file name to import.</param>
        public ImportModelAction(string fileName)
        {
            m_fileName = fileName;
            m_isUndoable = false;
        }
        /// <summary>
        /// Write the information to import the model to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "ImportModel");
            writer.WriteAttributeString("fileName", null, m_fileName);            
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to import the model.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("fileName");
            if (child == null) return;
            m_fileName = child.InnerText;
        }
        /// <summary>
        /// Execute to import the model using the information.
        /// </summary>
        public override void Execute()
        {
            string modelID = DataManager.GetDataManager().LoadModel(m_fileName, true);
            PluginManager.GetPluginManager().DataAdd(DataManager.GetDataManager().GetData(modelID, null));
            PluginManager.GetPluginManager().ChangeStatus(Util.LOADED);
        }
        /// <summary>
        /// Do nothing
        /// </summary>
        public override void UnExecute()
        {
            throw new Exception("Sorry. Not implemented.");
        }
    }

    /// <summary>
    /// Action class to create the project.
    /// </summary>
    public class NewProjectAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The name of created project.
        /// </summary>
        private string m_prjName;
        /// <summary>
        /// The comment of created project.
        /// </summary>
        private string m_comment;
        #endregion

        /// <summary>
        /// The constructor for NewProjectAction.
        /// </summary>
        public NewProjectAction()
        {
            m_isUndoable = false;
        }
        /// <summary>
        /// The constructor for NewProjectAction with initial parameters.
        /// </summary>
        /// <param name="prjName"></param>
        /// <param name="comment"></param>
        public NewProjectAction(string prjName, string comment)
        {
            m_prjName = prjName;
            m_comment = comment;
            m_isUndoable = false;
        }
        /// <summary>
        /// Write the information to create the project to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "NewProject");
            writer.WriteAttributeString("project", null, m_prjName);
            writer.WriteAttributeString("comment", null, m_comment);
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to create the project.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("project");
            if (child == null) return;
            m_prjName = child.InnerText;
            child = node.Attributes.GetNamedItem("comment");
            if (child == null) return;
            m_comment = child.InnerText;
        }
        /// <summary>
        /// Execute to create the project using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().NewProject(m_prjName, m_comment);
            PluginManager.GetPluginManager().ChangeStatus(Util.LOADED);
        }
        /// <summary>
        /// Do nothing.
        /// </summary>
        public override void UnExecute()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    /// <summary>
    /// Action class to create the object.
    /// </summary>
    public class DataAddAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The added EcellObject.
        /// </summary>
        private EcellObject m_obj;
        #endregion

        /// <summary>
        /// The constructor for DataAddAction.
        /// </summary>
        public DataAddAction()
        {
            m_obj = null;
        }
        /// <summary>
        /// The constructor for DataAddAction with initial parameters.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isUndoable">The flag the action is undoable.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public DataAddAction(EcellObject obj, bool isUndoable, bool isAnchor)
        {
            m_obj = obj;
            m_isUndoable = isUndoable;
            m_isAnchor = isAnchor;
        }
        /// <summary>
        /// Write the information to add the object to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "DataAdd");
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            UserAction.WriteObject(writer, m_obj);
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to add the object.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean( child.InnerText );
            m_obj = UserAction.LoadObject(node);
            if (m_obj != null && m_obj.key == "/") m_obj = null;
        }
        /// <summary>
        /// Execute to add the object using the information.
        /// </summary>
        public override void Execute()
        {
            if (m_obj == null) return;
            List<EcellObject> list = new List<EcellObject>();
            list.Add(m_obj);
            DataManager.GetDataManager().DataAdd(list, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// object will be deleted.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().DataDelete(m_obj.modelID, m_obj.key, m_obj.type, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to delete the object.
    /// </summary>
    public class DataDeleteAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The model ID of deleted object.
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// The key of deleted object.
        /// </summary>
        private string m_key;
        /// <summary>
        /// The type of deleted object.
        /// </summary>
        private string m_type;
        /// <summary>
        /// Deleted object.
        /// </summary>
        private EcellObject m_obj;
        #endregion

        /// <summary>
        /// The constructor for DataDeleteAction.
        /// </summary>
        public DataDeleteAction()
        {
        }
        /// <summary>
        /// The constructor for DataDeleteAction with initial parameters.
        /// </summary>
        /// <param name="modelID">The modelID of deleted object.</param>
        /// <param name="key">The key of deleted object.</param>
        /// <param name="type">The type of deleted object.</param>
        /// <param name="obj">deleted object.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public DataDeleteAction(string modelID, string key, string type, EcellObject obj, bool isAnchor)
        {
            m_modelID = modelID;
            m_key = key;
            m_type = type;
            m_obj = obj;
            m_isAnchor = isAnchor;
        }
        /// <summary>
        /// Write the information to delete the object to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "DataDelete");
            writer.WriteAttributeString("model", null, m_modelID);
            writer.WriteAttributeString("key", null, m_key);
            writer.WriteAttributeString("type", null, m_type);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            writer.WriteStartElement("Deleted");
            UserAction.WriteObject(writer, m_obj);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to delete the object.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("model");
            if (child == null) return;
            m_modelID = child.InnerText;
            child = node.Attributes.GetNamedItem("key");
            if (child == null) return;
            m_key = child.InnerText;
            child = node.Attributes.GetNamedItem("type");
            if (child == null) return;
            m_type = child.InnerText;
            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);
            XmlNodeList list = node.SelectNodes("Deleted");
            foreach (XmlNode n in list)
            {
                m_obj = UserAction.LoadObject(n);
            }
        }
        /// <summary>
        /// Execute to delete the object using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().DataDelete(m_modelID, m_key, m_type, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// An object will be deleted.
        /// </summary>
        public override void UnExecute()
        {
            if (m_obj == null) return;
            List<EcellObject> list = new List<EcellObject>();
            list.Add(m_obj);
            DataManager.GetDataManager().DataAdd(list, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to change the properties of object.
    /// </summary>
    public class DataChangeAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The modelID of changed object.
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// The type of changed object.
        /// </summary>
        private string m_type;
        /// <summary>
        /// An object before changing.
        /// </summary>
        private EcellObject m_oldObj;
        /// <summary>
        /// An object after changing.
        /// </summary>
        private EcellObject m_newObj;
        #endregion

        /// <summary>
        /// The constructor for DataChangeAction.
        /// </summary>
        public DataChangeAction()
        {
        }
        /// <summary>
        /// The constructor for DataChangeAction with initial parameters.
        /// </summary>
        /// <param name="modelID">The modelID of changed object.</param>
        /// <param name="type">The type of changed object.</param>
        /// <param name="oldObj">An object before changing.</param>
        /// <param name="newObj">An object after changing.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public DataChangeAction(string modelID, string type, EcellObject oldObj, EcellObject newObj, bool isAnchor)
        {
            m_modelID = modelID;
            m_type = type;
            m_oldObj = oldObj;
            m_newObj = newObj;
            m_isAnchor = isAnchor;
        }
        /// <summary>
        /// Write the information to change the object to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "DataChanged");
            writer.WriteAttributeString("model", null, m_modelID);
            writer.WriteAttributeString("type", null, m_type);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));

            writer.WriteStartElement("OldObj");
            UserAction.WriteObject(writer, m_oldObj);
            writer.WriteEndElement();

            writer.WriteStartElement("NewObj");
            UserAction.WriteObject(writer, m_newObj);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to change the object.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("model");
            if (child == null) return;
            m_modelID = child.InnerText;

            child = node.Attributes.GetNamedItem("type");
            if (child == null) return;
            m_type = child.InnerText;

            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);

            XmlNodeList children = node.SelectNodes("OldObj");
            if (children == null || children.Count == 0) return;
            m_oldObj = UserAction.LoadObject(children[0]);

            children = node.SelectNodes("NewObj");
            if (children == null || children.Count == 0) return;
            m_newObj = UserAction.LoadObject(children[0]);
        }
        /// <summary>
        /// Execute to change the object using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().DataChanged(m_modelID, m_oldObj.key, m_type, m_newObj, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// Changing will be aborted.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().DataChanged(m_modelID, m_newObj.key, m_type, m_oldObj, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to load the project.
    /// </summary>
    public class LoadProjectAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The load project ID.
        /// </summary>
        private string m_prjID;
        #endregion

        /// <summary>
        /// The constructor for LoadProjectAction.
        /// </summary>
        public LoadProjectAction()
        {
            m_isUndoable = false;
        }
        /// <summary>
        /// The constructor for LoadProjectAction with initial parameters.
        /// </summary>
        /// <param name="prjID">The load project ID.</param>
        public LoadProjectAction(string prjID)
        {
            m_prjID = prjID;
            m_isUndoable = false;
        }
        /// <summary>
        /// Write the information to load the project to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "LoadProject");
            writer.WriteAttributeString("prjID", null, m_prjID);
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to load the project.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("prjID");
            if (child == null) return;
            m_prjID = child.InnerText;
        }
        /// <summary>
        /// Execute to load the project using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().LoadProject(m_prjID);
            PluginManager.GetPluginManager().ChangeStatus(Util.LOADED);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    /// <summary>
    /// Action class to create stepper.
    /// </summary>
    public class AddStepperAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The parameter ID added the stepper.
        /// </summary>
        private string m_paramID;
        /// <summary>
        /// The stepper object.
        /// </summary>
        private EcellObject m_stepper;
        #endregion

        /// <summary>
        /// The constructor for AddStepperAction.
        /// </summary>
        public AddStepperAction()
        {
        }
        /// <summary>
        /// The constructor for AddStepperAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The parameter ID added the stepper.</param>
        /// <param name="stepper">The stepper object.</param>
        public AddStepperAction(string paramID, EcellObject stepper)
        {
            m_paramID = paramID;
            m_stepper = stepper;
        }
        /// <summary>
        /// Write the information to add the stepper to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "AddStepper");
            writer.WriteAttributeString("paramID", null, m_paramID);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            writer.WriteStartElement("Stepper");
            UserAction.WriteObject(writer, m_stepper);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to add the stepper.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("paramID");
            if (child == null) return;
            m_paramID = child.InnerText;

            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);

            XmlNodeList list = node.SelectNodes("Stepper");
            foreach (XmlNode n in list)
            {
                m_stepper = UserAction.LoadObject(n);
            }
        }
        /// <summary>
        /// Execute to add the stepper using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().AddStepperID(m_paramID, m_stepper, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().DeleteStepperID(m_paramID, m_stepper, false);
        }
    }

    /// <summary>
    /// Action class to delete stepper.
    /// </summary>
    public class DeleteStepperAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The parameter ID deleted the stepper.
        /// </summary>
        private string m_paramID;
        /// <summary>
        /// The deleted stepper.
        /// </summary>
        private EcellObject m_stepper;
        #endregion

        /// <summary>
        /// The constructor for DeleteStepperAction.
        /// </summary>
        public DeleteStepperAction()
        {
        }
        /// <summary>
        /// The constructor for DeleteStepperAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The parameter ID deleted the stepper.</param>
        /// <param name="stepper">The deleted stepper.</param>
        public DeleteStepperAction(string paramID, EcellObject stepper)
        {
            m_paramID = paramID;
            m_stepper = stepper;
        }
        /// <summary>
        /// Write the information to delete the stepper to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "DeleteStepper");
            writer.WriteAttributeString("paramID", null, m_paramID);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            writer.WriteStartElement("Stepper");
            UserAction.WriteObject(writer, m_stepper);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to delete the stepper.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("paramID");
            if (child == null) return;
            m_paramID = child.InnerText;

            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);

            XmlNodeList list = node.SelectNodes("Stepper");
            foreach (XmlNode n in list)
            {
                m_stepper = UserAction.LoadObject(n);
            }
        }
        /// <summary>
        /// Execute to delete the stepper using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().DeleteStepperID(m_paramID, m_stepper, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().AddStepperID(m_paramID, m_stepper, false);
        }
    }

    /// <summary>
    /// Action class to update the parameter of simulation.
    /// </summary>
    public class UpdateStepperAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The parameter id updated the stepper.
        /// </summary>
        private string m_paramID;
        /// <summary>
        /// The updated stepper.
        /// </summary>
        private List<EcellObject> m_newStepperList;
        /// <summary>
        /// The original stepper.
        /// </summary>
        private List<EcellObject> m_oldStepperList;
        #endregion

        /// <summary>
        /// The constructor for UpdateStepperAction.
        /// </summary>
        public UpdateStepperAction()
        {
            m_newStepperList = new List<EcellObject>();
            m_oldStepperList = new List<EcellObject>();
        }
        /// <summary>
        /// The constructor for UpdateStepperAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The parameter id updated the stepper.</param>
        /// <param name="newStepper">The updated stepper.</param>
        /// <param name="oldStepper">The old stepper.</param>
        public UpdateStepperAction(string paramID, List<EcellObject> newStepper, List<EcellObject> oldStepper)
        {
            m_paramID = paramID;
            m_newStepperList = newStepper;
            m_oldStepperList = oldStepper;
        }
        /// <summary>
        /// Write the information to update the stepper to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "UpdateStepper");
            writer.WriteAttributeString("paramID", null, m_paramID);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            foreach (EcellObject obj in m_newStepperList)
            {
                writer.WriteStartElement("NewStepper");
                UserAction.WriteObject(writer, obj);
                writer.WriteEndElement();
            }
            foreach (EcellObject obj in m_oldStepperList)
            {
                writer.WriteStartElement("OldStepper");
                UserAction.WriteObject(writer, obj);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to update the stepper.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("paramID");
            if (child == null) return;
            m_paramID = child.InnerText;
            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);
            XmlNodeList list = node.SelectNodes("NewStepper");
            foreach (XmlNode n in list)
            {
                EcellObject obj = UserAction.LoadObject(n);
                m_newStepperList.Add(obj);
            }
            list = node.SelectNodes("OldStepper");
            foreach (XmlNode n in list)
            {
                EcellObject obj = UserAction.LoadObject(n);
                m_oldStepperList.Add(obj);
            }
        }
        /// <summary>
        /// Execute to update the stepper using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().UpdateStepperID(m_paramID, m_newStepperList, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().UpdateStepperID(m_paramID, m_oldStepperList, false);
        }
    }

    /// <summary>
    /// Action class to create the parameter of simulation.
    /// </summary>
    public class NewSimParamAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The created parameter id.
        /// </summary>
        private string m_paramID;
        #endregion

        /// <summary>
        /// The constructor for NewSimParamAction.
        /// </summary>
        public NewSimParamAction()
        {
        }
        /// <summary>
        /// The constructor for NewSimParamAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The created paramter ID.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public NewSimParamAction(string paramID, bool isAnchor)
        {
            m_paramID = paramID;
            m_isAnchor = isAnchor;
        }
        /// <summary>
        /// Write the information to create the simulation parameter to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "NewSimParam");
            writer.WriteAttributeString("paramID", null, m_paramID);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to create the simulation parameter.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("paramID");
            if (child == null) return;
            m_paramID = child.InnerText;

            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);
        }
        /// <summary>
        /// Execute to create the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().NewSimulationParameter(m_paramID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().DeleteSimulationParameter(m_paramID, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to delete the parameter of simulation.
    /// </summary>
    public class DeleteSimParamAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The deleted parameter ID.
        /// </summary>
        private string m_paramID;
        #endregion

        /// <summary>
        /// The constructor for DeleteSimParamAction.
        /// </summary>
        public DeleteSimParamAction()
        {
        }
        /// <summary>
        /// The constructor for DeleteSimParamAction with initial parameters.
        /// </summary>
        /// <param name="paramID">The deleted parameter ID.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public DeleteSimParamAction(string paramID, bool isAnchor)
        {
            m_paramID = paramID;
            m_isAnchor = isAnchor;
        }
        /// <summary>
        /// Write the information to delete the simulation parameter to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "DeleteSimParam");
            writer.WriteAttributeString("paramID", null, m_paramID);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to delete the simulation parameter.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("paramID");
            if (child == null) return;
            m_paramID = child.InnerText;

            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);
        }
        /// <summary>
        /// Execute to delete the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().DeleteSimulationParameter(m_paramID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().NewSimulationParameter(m_paramID, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to setup parameter of simulation.
    /// </summary>
    public class SetSimParamAction : UserAction
    {
        #region Fields
        /// <summary>
        /// New parameter ID.
        /// </summary>
        private string m_newParamID;
        /// <summary>
        /// Old parameter ID.
        /// </summary>
        private string m_oldParamID;
        #endregion

        /// <summary>
        /// The constructor for SetSimParamAction.
        /// </summary>
        public SetSimParamAction()
        {
        }
        /// <summary>
        /// The constructor for SetSimParamAction with initial parameters.
        /// </summary>
        /// <param name="newParamID">new parameter ID</param>
        /// <param name="oldParamID">old parameter ID</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public SetSimParamAction(string newParamID, string oldParamID, bool isAnchor)
        {
            m_newParamID = newParamID;
            m_oldParamID = oldParamID;
            m_isAnchor = isAnchor;
        }
        /// <summary>
        /// Write the information to set the simulation parameter to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "SetSimParam");
            writer.WriteAttributeString("newParamID", null, m_newParamID);
            writer.WriteAttributeString("oldParamID", null, m_oldParamID);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to set the simulation parameter.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("newParamID");
            if (child == null) return;
            m_newParamID = child.InnerText;
            child = node.Attributes.GetNamedItem("oldParamID");
            if (child == null) return;
            m_oldParamID = child.InnerText;
            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);
        }
        /// <summary>
        /// Execute to set the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().SetSimulationParameter(m_newParamID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().SetSimulationParameter(m_oldParamID, false, m_isAnchor);
        }
    }

    /// <summary>
    /// Action class to merge a system with upper system.
    /// </summary>
    public class SystemMergeAction : UserAction
    {
        #region Fields
        /// <summary>
        /// Model ID.
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// Merged system.
        /// </summary>
        private EcellObject m_obj;
        /// <summary>
        /// List of merged system's children systems.
        /// </summary>
        private List<EcellObject> m_sysList = new List<EcellObject>();
        /// <summary>
        /// List of merged system's children objects.
        /// </summary>
        private List<EcellObject> m_objList = new List<EcellObject>();
        #endregion

        /// <summary>
        /// The constructor for SetSimParamAction.
        /// </summary>
        public SystemMergeAction()
        {
        }
        /// <summary>
        /// The constructor for SystemMergeAction with initial parameters.
        /// </summary>
        /// <param name="modelID">Model ID</param>
        /// <param name="merged">Merged system</param>
        /// <param name="sysList">Child systems of the merged system</param>
        /// <param name="objList">Child objects of the merged system</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public SystemMergeAction(
            string modelID,
            EcellObject merged,
            List<EcellObject> sysList,
            List<EcellObject> objList,
            bool isAnchor)
        {
            m_modelID = modelID;
            m_obj = merged;
            foreach (EcellObject sys in sysList)
                m_sysList.Add(sys.Copy());
            foreach (EcellObject obj in objList)
                m_objList.Add(obj.Copy());
            m_isAnchor = isAnchor;
        }
        /// <summary>
        /// Write the information to set the simulation parameter to the xml file.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        public override void SaveScript(XmlTextWriter writer)
        {
            writer.WriteStartElement("Action");
            writer.WriteAttributeString("command", null, "SystemMerge");
            writer.WriteAttributeString("modelID", null, m_modelID);
            writer.WriteAttributeString("isAnchor", null, Convert.ToString(base.m_isAnchor));

            writer.WriteStartElement("MergedSystem");
            UserAction.WriteObject(writer, m_obj);
            writer.WriteEndElement();

            writer.WriteStartElement("ChildSystems");
            foreach (EcellObject child in m_sysList)
            {
                writer.WriteStartElement("System");
                UserAction.WriteObject(writer, child);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("ChildObjects");
            foreach (EcellObject child in m_objList)
            {
                writer.WriteStartElement("Obj");
                UserAction.WriteObject(writer, child);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to set the simulation parameter.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("modelID");
            if (child == null) return;
            m_modelID = child.InnerText;

            child = node.Attributes.GetNamedItem("isAnchor");
            if (child == null) return;
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);

            XmlNodeList children = node.SelectNodes("MergedSystem");
            if (children == null || children.Count == 0) return;
            m_obj = UserAction.LoadObject(children[0]);

            children = node.SelectNodes("ChildSystems/System");
            if (children != null && children.Count != 0)
            {
                m_sysList = new List<EcellObject>();
                foreach(XmlNode sys in children)
                    m_sysList.Add(UserAction.LoadObject(sys));                
            }

            children = node.SelectNodes("ChildObjects/Obj");
            if (children != null && children.Count != 0)
            {
                m_objList = new List<EcellObject>();
                foreach (XmlNode obj in children)
                    m_objList.Add(UserAction.LoadObject(obj));                    
            }
        }
        /// <summary>
        /// Execute to set the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            DataManager.GetDataManager().SystemDeleteAndMove(m_modelID, m_obj.key);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            DataManager.GetDataManager().SystemAddAndMove(m_modelID, m_obj, m_sysList, m_objList);
        }
    }
}