using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell
{
    /// <summary>
    /// Abstract class for action.
    /// </summary>
    public abstract class UserAction
    {
        #region Fields
        /// <summary>
        /// Whether this UserAction is the last one in a sequence of UserAction.
        /// </summary>
        protected bool m_isAnchor = true;
        /// <summary>
        /// Whether this UserAction is undoable or not.
        /// </summary>
        protected bool m_isUndoable = true;

        /// <summary>
        /// ApplicationEnvironment
        /// </summary>
        protected ApplicationEnvironment m_env;
        #endregion

        #region Accessors
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
        /// 
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            internal set { m_env = value; }
        }
        #endregion

        #region Methods
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
                d.Name = tmp.InnerText;
                tmp = data.Attributes.GetNamedItem("path");
                d.EntityPath = tmp.InnerText;

                tmp = data.Attributes.GetNamedItem("isGetable");
                if (tmp.InnerText == "true") d.Gettable = true;
                else d.Gettable = false;

                tmp = data.Attributes.GetNamedItem("isLoadable");
                if (tmp.InnerText == "true") d.Loadable = true;
                else d.Loadable = false;

                tmp = data.Attributes.GetNamedItem("isLogable");
                if (tmp.InnerText == "true") d.Logable = true;
                else d.Logable = false;

                tmp = data.Attributes.GetNamedItem("isLogger");
                if (tmp.InnerText == "true") d.Logged = true;
                else d.Logged = false;

                tmp = data.Attributes.GetNamedItem("isSavable");
                if (tmp.InnerText == "true") d.Saveable = true;
                else d.Saveable = false;

                tmp = data.Attributes.GetNamedItem("isSetable");
                if (tmp.InnerText == "true") d.Settable = true;
                else d.Settable = false;

                XmlNodeList valueList = data.SelectNodes("Value");
                foreach (XmlNode value in valueList)
                {
                    tmp = value.Attributes.GetNamedItem("value_type");
                    string vtype = tmp.InnerText;
                    tmp = value.Attributes.GetNamedItem("value");
                    string valueData = tmp.InnerText;

                    EcellValue v;
                    if (vtype.Equals(typeof(string).ToString()))
                        v = new EcellValue(valueData);
                    else if (vtype.Equals(typeof(double).ToString()))
                    {
                        if (valueData == "1.79769313486232E+308")
                            v = new EcellValue(Double.MaxValue);
                        else
                            v = new EcellValue(Convert.ToDouble(valueData));
                    }
                    else if (vtype.Equals(typeof(int).ToString()))
                        v = new EcellValue(Convert.ToInt32(valueData));
                    else
                        v = EcellValue.FromListString(valueData);
                    d.Value = v;
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

            obj.Children = new List<EcellObject>();

            return obj;
        }
        /// <summary>
        /// Write the information of EcellObject by xml format.
        /// </summary>
        /// <param name="writer">The object for writing the xml file.</param>
        /// <param name="m_obj">The wrote object.</param>
        static public void WriteObject(XmlTextWriter writer, EcellObject m_obj)
        {
            writer.WriteAttributeString("model", null, m_obj.ModelID);
            writer.WriteAttributeString("type", null, m_obj.Type);
            writer.WriteAttributeString("key", null, m_obj.Key);
            writer.WriteAttributeString("classname", null, m_obj.Classname);
            writer.WriteAttributeString("x", null, Convert.ToString(m_obj.X));
            writer.WriteAttributeString("y", null, Convert.ToString(m_obj.Y));
            writer.WriteAttributeString("offsetx", null, Convert.ToString(m_obj.OffsetX));
            writer.WriteAttributeString("offsety", null, Convert.ToString(m_obj.OffsetY));
            writer.WriteAttributeString("width", null, Convert.ToString(m_obj.Width));
            writer.WriteAttributeString("height", null, Convert.ToString(m_obj.Height));

            if (m_obj.Value != null)
            {
                foreach (EcellData d in m_obj.Value)
                {
                    writer.WriteStartElement("Data");
                    writer.WriteAttributeString("name", null, d.Name);
                    writer.WriteAttributeString("path", null, d.EntityPath);
                    if (d.Gettable == true) writer.WriteAttributeString("isGetable", null, "true");
                    else writer.WriteAttributeString("isGetable", null, "false");
                    if (d.Loadable == true) writer.WriteAttributeString("isLoadable", null, "true");
                    else writer.WriteAttributeString("isLoadable", null, "false");
                    if (d.Logable == true) writer.WriteAttributeString("isLogable", null, "true");
                    else writer.WriteAttributeString("isLogable", null, "false");
                    if (d.Logged == true) writer.WriteAttributeString("isLogger", null, "true");
                    else writer.WriteAttributeString("isLogger", null, "false");
                    if (d.Saveable == true) writer.WriteAttributeString("isSavable", null, "true");
                    else writer.WriteAttributeString("isSavable", null, "false");
                    if (d.Settable == true) writer.WriteAttributeString("isSetable", null, "true");
                    else writer.WriteAttributeString("isSetable", null, "false");
                    writer.WriteStartElement("Value");
                    if (d.Value != null)
                    {
                        writer.WriteAttributeString("value_type", null, d.Value.Type.ToString());
                        writer.WriteAttributeString("value", null, d.Value.ToString());
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
        }
        #endregion
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
        private string m_prjPath;
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
        /// <param name="prjName">the projectID.</param>
        /// <param name="comment">the project comment.</param>
        /// <param name="path">the path of this project.</param>
        public NewProjectAction(string prjName, string comment, string path)
        {
            m_prjName = prjName;
            m_comment = comment;
            m_prjPath = path;
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
            if (m_prjPath != null)
            {
                writer.WriteAttributeString("path", null, m_prjPath);
            }
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

            child = node.Attributes.GetNamedItem("path");
            if (child == null) return;
            m_comment = child.InnerText;
        }
        /// <summary>
        /// Execute to create the project using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.CreateProject(m_prjName, m_comment, m_prjPath, new List<string>());
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }
        /// <summary>
        /// Do nothing.
        /// </summary>
        public override void UnExecute()
        {
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
            base.m_isAnchor = Convert.ToBoolean(child.InnerText);
            m_obj = UserAction.LoadObject(node);
            if (m_obj != null && m_obj.Key == "/") m_obj = null;
        }
        /// <summary>
        /// Execute to add the object using the information.
        /// </summary>
        public override void Execute()
        {
            if (m_obj == null) 
                return;
            m_env.DataManager.DataAdd(m_obj, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// object will be deleted.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DataDelete(m_obj, false, m_isAnchor);
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
            m_env.DataManager.DataDelete(m_modelID, m_key, m_type, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// An object will be deleted.
        /// </summary>
        public override void UnExecute()
        {
            if (m_obj == null)
                return;
            m_env.DataManager.DataAdd(m_obj, false, m_isAnchor);
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
            m_env.DataManager.DataChanged(m_modelID, m_oldObj.Key, m_type, m_newObj, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// Changing will be aborted.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DataChanged(m_modelID, m_newObj.Key, m_type, m_oldObj, false, m_isAnchor);
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
        /// <summary>
        /// The load project file.
        /// </summary>
        private string m_prjFile;
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
        /// <param name="prjFile">The load project file.</param>
        public LoadProjectAction(string prjID, string prjFile)
        {
            m_prjID = prjID;
            m_prjFile = prjFile;
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
            writer.WriteAttributeString("prjFile", null, m_prjFile);
            writer.WriteEndElement();
        }
        /// <summary>
        /// Load the information to load the project.
        /// </summary>
        /// <param name="node">The xml node wrote the information.</param>
        public override void LoadScript(XmlNode node)
        {
            XmlNode child = node.Attributes.GetNamedItem("prjID");
            if (child == null)
                return;
            m_prjID = child.InnerText;

            child = node.Attributes.GetNamedItem("prjFile");
            if (child == null)
                return;
            m_prjFile = child.InnerText;
        }
        /// <summary>
        /// Execute to load the project using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.LoadProject(m_prjFile);
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            throw new EcellException("The method or operation is not implemented.");
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
            m_env.DataManager.AddStepperID(m_paramID, m_stepper, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DeleteStepperID(m_paramID, m_stepper, false);
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
            m_env.DataManager.DeleteStepperID(m_paramID, m_stepper, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.AddStepperID(m_paramID, m_stepper, false);
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
            m_env.DataManager.UpdateStepperID(m_paramID, m_newStepperList, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.UpdateStepperID(m_paramID, m_oldStepperList, false);
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
            m_env.DataManager.CreateSimulationParameter(m_paramID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DeleteSimulationParameter(m_paramID, false, m_isAnchor);
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
            m_env.DataManager.DeleteSimulationParameter(m_paramID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.CreateSimulationParameter(m_paramID, false, m_isAnchor);
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
            m_env.DataManager.SetSimulationParameter(m_newParamID, false, m_isAnchor);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.SetSimulationParameter(m_oldParamID, false, m_isAnchor);
        }
    }
}
