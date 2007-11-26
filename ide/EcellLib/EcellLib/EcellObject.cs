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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using EcellCoreLib;
using System.Text.RegularExpressions;


namespace EcellLib
{
    /// <summary>
    /// The base class of E-CELL model editor.
    /// </summary>
    public class EcellObject
    {
        #region Constant
        /// <summary>
        /// Type string of "Process".
        /// </summary>
        public const string PROCESS = "Process";
        /// <summary>
        /// Type string of "System".
        /// </summary>
        public const string SYSTEM = "System";
        /// <summary>
        /// Type string of "Variable".
        /// </summary>
        public const string VARIABLE = "Variable";
        /// <summary>
        /// Value of fixed object.
        /// </summary>
        public const int Fixed = 1;
        /// <summary>
        /// Value of not fixed object.
        /// </summary>
        public const int NotFixed = 0;
        #endregion

        #region Fields
        /// <summary>
        /// The class
        /// </summary>
        private string m_class;
        /// <summary>
        /// The model ID
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// The key
        /// </summary>
        private string m_key;
        /// <summary>
        /// The type of object.
        /// </summary>
        private string m_type;
        /// <summary>
        /// The layer include this object.
        /// </summary>
        private string m_layer = "";
        /// <summary>
        /// X coordinate
        /// </summary>
        private float m_x;
        /// <summary>
        /// Y coordinate
        /// </summary>
        private float m_y;
        /// <summary>
        /// X offset
        /// </summary>
        private float m_offsetX;
        /// <summary>
        /// Y offset
        /// </summary>
        private float m_offsetY;
        /// <summary>
        /// Width
        /// </summary>
        private float m_width;
        /// <summary>
        /// Height
        /// </summary>
        private float m_height;
        /// <summary>
        /// The value
        /// </summary>
        protected List<EcellData> m_ecellDatas;
        /// <summary>
        /// The children of this
        /// </summary>
        private List<EcellObject> m_children;
        /// <summary>
        /// Fixed flag.
        /// </summary>
        private bool m_isFixed = false;
        #endregion

        #region Constractor
        /// <summary>
        /// Creates the new "EcellObject" instance with no argument.
        /// </summary>
        protected EcellObject()
        {
            m_children = new List<EcellObject>();
        }

        /// <summary>
        /// Creates the new "EcellObject" instance with initialized arguments.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key</param>
        /// <param name="l_type">The type</param>
        /// <param name="l_class">The class</param>
        /// <param name="l_data">The data</param>
        protected EcellObject(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data): this()
        {
            this.m_modelID = l_modelID;
            this.m_key = l_key;
            this.m_type = l_type;
            this.m_class = l_class;
            this.m_ecellDatas = l_data;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get/set m_modelID.
        /// </summary>
        public string modelID
        {
            get { return m_modelID; }
            set { m_modelID = value; }
        }

        /// <summary>
        /// get / set name.
        /// </summary>
        public string name
        {
            get {
                string name;
                if (key == null || key.Equals("/"))
                    name = "/";
                else if (key.Contains(":"))
                    name = key.Substring(key.LastIndexOf(":") + 1);
                else
                    name = key.Substring(key.LastIndexOf("/") + 1);
                return name;
            }
            set
            {
                if (key == null || key.Equals("/"))
                    this.m_key = "/";
                else if (key.Contains(":"))
                    this.m_key = parentSystemID + ":" + value;
                else
                    this.m_key = parentSystemID + "/" + value;
            }
        }

        /// <summary>
        /// get/set m_keyID.
        /// </summary>
        public string key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        /// <summary>
        /// get parent system ID.
        /// </summary>
        public string parentSystemID
        {
            get { return GetParentSystemId(key); }
            set
            {
                if (key == null || key.Equals("/"))
                    this.m_key = "/";
                else if (key.Contains(":"))
                    this.m_key = value + ":" + name;
                else
                    this.m_key = value + "/" + name;
            }

        }

        /// <summary>
        /// get text.
        /// </summary>
        public string Text
        {
            get
            {
                string text = this.name;
                if (Logged)
                    text += " *";

                return text;
            }
        }

        /// <summary>
        /// get/set m_class.
        /// </summary>
        public string classname
        {
            get { return m_class; }
            set { this.m_class = value; }
        }

        /// <summary>
        /// get/set the layer property.
        /// </summary>
        public string Layer
        {
            get { return this.m_layer; }
            set { this.m_layer = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public PointF PointF
        {
            get { return new PointF(m_x, m_y); }
            set
            {
                m_x = value.X;
                m_y = value.Y;
            }
        }
        /// <summary>
        /// PointF
        /// </summary>
        public RectangleF Rect
        {
            get { return new RectangleF(m_x, m_y, m_width, m_height); }
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public float X
        {
            get { return m_x; }
            set
            {
                m_x = value;
            }
        }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public float Y
        {
            get { return m_y; }
            set {
                m_y = value;
            }
        }

        /// <summary>
        /// X offset
        /// </summary>
        public float OffsetX
        {
            get { return m_offsetX; }
            set { m_offsetX = value; }
        }

        /// <summary>
        /// Y offset
        /// </summary>
        public float OffsetY
        {
            get { return m_offsetY; }
            set { m_offsetY = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public float Width
        {
            get { return m_width; }
            set {
                m_width = value;
            }
        }

        /// <summary>
        /// Height
        /// </summary>
        public float Height
        {
            get { return m_height; }
            set {
                m_height = value;
            }
        }

        /// <summary>
        /// get isLogger.
        /// </summary>
        public bool Logged
        {
            get
            {
                //return true if any Logger exists.
                if (Value != null)
                {
                    foreach (EcellData d in Value)
                        if ( d.Logable && d.Logged)
                            return true;
                }
                return false;
            }
        }

        /// <summary>
        /// get/set m_type.
        /// </summary>
        public string type
        {
            get { return m_type; }
            set { this.m_type = value; }
        }

        /// <summary>
        /// get/set Value.
        /// </summary>
        [Browsable(false)]
        public List<EcellData> Value
        {
            get { return m_ecellDatas; }
            // set { this.Value = value; }
        }

        /// <summary>
        /// get/set Children.
        /// </summary>
        [Browsable(false)]
        public List<EcellObject> Children
        {
            get
            {
                return m_children;
            }
            set
            {
                Debug.Assert(value != null);
                this.m_children = value;
            }
        }

        /// <summary>
        /// Whether position for this object has been set or not.
        /// </summary>
        public bool IsPosSet
        {
            get
            {
                if (this.X != 0 || this.Y != 0)
                    return true;
                if (this.Width != 0 || this.Y != Height)
                    return true;
                if (this.OffsetX != 0 || this.OffsetY != 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// get / set whether this parameter is fix.
        /// </summary>
        public bool isFixed
        {
            get { return m_isFixed; }
            set { this.m_isFixed = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create the copy "EcellObject".
        /// </summary>
        /// <returns>The copy "EcellObject"</returns>
        public EcellObject Copy()
        {
            try
            {
                EcellObject l_newEcellObject =
                    CreateObject(this.m_modelID, this.m_key, this.m_type, this.m_class, this.CopyValueList());
                l_newEcellObject.X = this.m_x;
                l_newEcellObject.Y = this.m_y;
                l_newEcellObject.OffsetX = this.m_offsetX;
                l_newEcellObject.OffsetY = this.m_offsetY;
                l_newEcellObject.Width = this.m_width;
                l_newEcellObject.Height = this.m_height;
                l_newEcellObject.Layer = this.Layer;
                l_newEcellObject.Children = this.CopyChildren();
                return l_newEcellObject;
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't make a copy of an EcellObject", l_ex);
            }
        }

        private List<EcellData> CopyValueList()
        {
            List<EcellData> l_copyValueList = null;
            if (this.m_ecellDatas != null)
            {
                l_copyValueList = new List<EcellData>();
                if (this.m_ecellDatas.Count > 0)
                {
                    foreach (EcellData l_value in this.m_ecellDatas)
                    {
                        l_copyValueList.Add(l_value.Copy());
                    }
                }
            }
            return l_copyValueList;
        }
        private List<EcellObject> CopyChildren()
        {
            List<EcellObject> l_list = new List<EcellObject>();
            foreach (EcellObject l_ecellObject in this.m_children)
            {
                l_list.Add(l_ecellObject.Copy());
            }
            return l_list;
        }
        /// <summary>
        /// Set object coordinates.
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        public void SetPosition(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        /// <summary>
        /// Copy coordinates of passed object.
        /// </summary>
        /// <param name="obj">EcellObject</param>
        public void SetPosition(EcellObject obj)
        {
            this.X = obj.X;
            this.Y = obj.Y;
            this.OffsetX = obj.OffsetX;
            this.OffsetY = obj.OffsetY;
            this.Width = obj.Width;
            this.Height = obj.Height;
        }

        /// <summary>
        /// get parent system ID.
        /// </summary>
        /// <param name="key">The key</param>
        private string GetParentSystemId(string key)
        {
            Regex postColonRegex = new Regex(":\\w*$");
            Regex postSlashRegex = new Regex("/\\w*$");
            if (key == null || key.Equals("") || key.Equals("/"))
                return "";
            else if (key.Contains(":"))
            {
                return postColonRegex.Replace(key, "");
            }
            else
            {
                string returnStr = postSlashRegex.Replace(key, "");
                if (returnStr.Equals(""))
                    return "/";
                else
                    return returnStr;
            }
        }

        /// <summary>
        /// Returns the new "EcellObject" instance with initialized arguments.
        /// </summary>
        /// <param name="l_modelID">The model ID</param>
        /// <param name="l_key">The key</param>
        /// <param name="l_type">The type</param>
        /// <param name="l_class">The class</param>
        /// <param name="l_data">The data</param>
        /// <returns>The new "EcellObject" instance</returns>
        public static EcellObject CreateObject(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            if (PROCESS.Equals(l_type) )
                return new EcellProcess(l_modelID, l_key, l_type, l_class, l_data);
            else if (VARIABLE.Equals(l_type))
                return new EcellVariable(l_modelID, l_key, l_type, l_class, l_data);
            else if (SYSTEM.Equals(l_type))
                return new EcellSystem(l_modelID, l_key, l_type, l_class, l_data);
            else
                return new EcellObject(l_modelID, l_key, l_type, l_class, l_data);
        }

        /// <summary>
        /// override the equal method on EcellObject.
        /// </summary>
        /// <param name="obj">the comparing data</param>
        /// <returns>if equal, return true.</returns>
        public bool Equals(EcellObject obj)
        {
            if (this.m_modelID == obj.m_modelID && this.m_key == obj.m_key)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// add the data to Value.
        /// </summary>
        /// <param name="d">EcellData.</param>
        public void AddValue(EcellData d)
        {
            this.m_ecellDatas.Add(d);
        }

        /// <summary>
        /// Set value from the list of EcellData.
        /// </summary>
        /// <param name="list">the list of EcellData.</param>
        public void SetEcellDatas(List<EcellData> list)
        {
            this.m_ecellDatas = list;
        }

        /// <summary>
        /// get EcellData from the list of EcellData.
        /// </summary>
        /// <param name="name">the key of EcellData.</param>
        public EcellData GetEcellData(string name)
        {
            // Check List.
            if (Value == null)
                return null;
            //return EcellData if EcellValue exists.
            foreach (EcellData data in Value)
            {
                if (data.Name == name)
                    return data;
            }
            return null;
        }
        /// <summary>
        /// get EcellValue from the list of EcellData.
        /// </summary>
        /// <param name="name">the key of EcellValue.</param>
        public EcellValue GetEcellValue(string name)
        {
            // Check List.
            if (Value == null)
                return null;
            //return EcellValue if EcellValue exists.
            foreach (EcellData data in Value)
            {
                if (data.Name == name)
                    return data.Value;
            }
            return null;
        }

        /// <summary>
        /// get isEcellValueExists.
        /// </summary>
        public bool IsEcellValueExists(string name)
        {
            // Check List.
            if (Value == null)
                return false;
            //return true if EcellValue exists.
            foreach (EcellData d in Value)
                if (d.Name == name)
                    return true;
            return false;
        }

        /// <summary>
        /// Add EcellValue.
        /// </summary>
        protected void AddEcellValue(string name, EcellValue value)
        {
            string entytyPath = this.type + ":" + this.key + ":" + name;
            EcellData data = new EcellData(name, value, entytyPath);
            AddValue(data);
        }

        /// <summary>
        /// Convert from this object to the object string.
        /// (Override function). 
        /// </summary>
        /// <returns>the object string.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(type);
            sb.Append("(name=" + name + ")");
            if (Children.Count > 0)
            {
                sb.Append("{");
                for (int i = 0; i < Children.Count; ++i)
                {
                    if (i > 0)
                        sb.Append(", ");
                    EcellObject child = Children[i];
                    if (child != null)
                        sb.Append(child.ToString());
                    else
                        sb.Append("*null*");
                }
                sb.Append("}");
            }
            return sb.ToString();
        }

        #endregion
    }


    /// <summary>
    /// Stores the property data.
    /// </summary>
    public class EcellData
    {
        #region Fields
        /// <summary>
        /// The property name 
        /// </summary>
        private string m_name;
        /// <summary>
        /// The property value
        /// </summary>
        private EcellValue m_value;
        /// <summary>
        /// The entity path 4 "EcellCoreLib"
        /// </summary>
        private string m_entityPath;
        /// <summary>
        /// The flag of gettable
        /// </summary>
        private bool m_isGettable;
        /// <summary>
        /// The flag of loadable
        /// </summary>
        private bool m_isLoadable;
        /// <summary>
        /// The flag of logable
        /// </summary>
        private bool m_isLogable;
        /// <summary>
        /// The flag of logger
        /// </summary>
        private bool m_isLogger;
        /// <summary>
        /// The flag of savable
        /// </summary>
        private bool m_isSavable;
        /// <summary>
        /// The flag of settable
        /// </summary>
        private bool m_isSettable;
        /// <summary>
        /// The flag of commitment.
        /// </summary>
        private bool m_isCommit;
        /// <summary>
        /// Max value of this data.
        /// </summary>
        private double m_max;
        /// <summary>
        /// Min value of this data.
        /// </summary>
        private double m_min;
        /// <summary>
        /// Step value of this data.
        /// If this value is 0, this data is random parameter.
        /// </summary>
        private double m_step;
        #endregion

        #region Constractors
        /// <summary>
        /// Creates the new "EcellData" instance with no argument.
        /// </summary>
        public EcellData()
        {
            this.m_name = null;
            this.Value = null;
            this.m_entityPath = null;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogger = false;
            this.m_isCommit = true;
            this.m_max = 0.0;
            this.m_min = 0.0;
            this.m_step = 0.0;
        }

        /// <summary>
        /// Creates the new "EcellData" instance with some arguments.
        /// </summary>
        /// <param name="l_name">The property name</param>
        /// <param name="l_data">The property value</param>
        /// <param name="l_entityPath">The entity path</param>
        public EcellData(string l_name, EcellValue l_data, string l_entityPath)
        {
            this.m_name = l_name;
            this.Value = l_data;
            this.m_entityPath = l_entityPath;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogger = false;
            this.m_isCommit = true;
            this.m_max = 0.0;
            this.m_min = 0.0;
            this.m_step = 0.0;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get/set name.
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// get/set value
        /// </summary>
        public EcellValue Value
        {
            get { return m_value; }
            set { this.m_value = value; }
        }

        /// <summary>
        /// get/set m_entityPath
        /// </summary>
        public string EntityPath
        {
            get { return m_entityPath; }
            set { this.m_entityPath = value; }
        }

        /// <summary>
        /// get/set m_isGettable
        /// </summary>
        public bool Gettable
        {
            get { return m_isGettable; }
            set { this.m_isGettable = value; }
        }

        /// <summary>
        /// get/set m_isLoadable
        /// </summary>
        public bool Loadable
        {
            get { return m_isLoadable; }
            set { this.m_isLoadable = value; }
        }

        /// <summary>
        /// get/set m_isLogable
        /// </summary>
        public bool Logable
        {
            get { return this.m_isLogable; }
            set { this.m_isLogable = value; }
        }

        /// <summary>
        /// get/set m_isLogger
        /// </summary>
        public bool Logged
        {
            get { return this.m_isLogger; }
            set { this.m_isLogger = value; }
        }

        /// <summary>
        /// get/set m_isSavable
        /// </summary>
        public bool Saveable
        {
            get { return m_isSavable; }
            set { this.m_isSavable = value; }
        }

        /// <summary>
        /// get/set m_isSettable
        /// </summary>
        public bool Settable
        {
            get { return m_isSettable; }
            set { this.m_isSettable = value; }
        }

        /// <summary>
        /// get/set m_isCommit
        /// </summary>
        public bool Committed
        {
            get { return m_isCommit; }
            set { this.m_isCommit = value; }
        }

        /// <summary>
        /// get/set the max value of this data.
        /// </summary>
        public double Max
        {
            get { return this.m_max; }
            set { this.m_max = value; }
        }

        /// <summary>
        /// get/set the min value of this data.
        /// </summary>
        public double Min
        {
            get { return this.m_min; }
            set { this.m_min = value; }
        }

        /// <summary>
        /// get/set the step value of this data.
        /// </summary>
        public double Step
        {
            get { return this.m_step; }
            set { this.m_step = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create the copy "EcellData".
        /// </summary>
        /// <returns>The copy "EcellData"</returns>
        public EcellData Copy()
        {
            try
            {
                EcellData l_newData = new EcellData(this.m_name, this.Value, this.m_entityPath);
                l_newData.Gettable = this.m_isGettable;
                l_newData.Loadable = this.m_isLoadable;
                l_newData.Logable = this.m_isLogable;
                l_newData.Logged = this.m_isLogger;
                l_newData.Saveable = this.m_isSavable;
                l_newData.Settable = this.m_isSettable;
                l_newData.Committed = this.m_isCommit;
                l_newData.Max = this.m_max;
                l_newData.Min = this.m_min;
                l_newData.Step = this.m_step;
                return l_newData;
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't copy the \"EcellData\". {" + l_ex.ToString() + "}");
            }
        }

        /// <summary>
        /// override equal method on EcellData.
        /// </summary>
        /// <param name="l_obj">the comparing object</param>
        /// <returns>if equal, return true</returns>
        public bool Equals(EcellData l_obj)
        {
            if (this.m_name == l_obj.m_name && this.Value == l_obj.Value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tests whether it is possible to initialize this or not.
        /// </summary>
        /// <returns>true if it is possible to initialize; false otherwise</returns>
        public bool IsInitialized()
        {
            if (!this.m_isSettable)
            {
                return false;
            }
            if (this.Value == null || (!this.Value.IsInt() && !this.Value.IsDouble()))
            {
                return false;
            }
            return true;
        }
        #endregion
    }


    /// <summary>
    /// The polymorphism 4 the "EcellData".
    /// </summary>
    public class EcellValue
    {
        #region Fields
        /// <summary>
        /// The stored value.
        /// </summary>
        private Object m_value = null;
        /// <summary>
        /// The type of the stored value.
        /// </summary>
        private Type m_type = null;
        #endregion

        #region Constractors
        /// <summary>
        /// Creates a new "EcellValue" instance with no argument.
        /// </summary>
        private EcellValue()
        {
            ; // do nothing
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "int" argument.
        /// </summary>
        /// <param name="l_value">The "int" value</param>
        public EcellValue(int l_value)
        {
            Value = l_value;
            m_type = typeof(int);
        }


        /// <summary>
        /// Creates a new "EcellValue" instance with a "double" argument.
        /// </summary>
        /// <param name="l_value">The "double" value</param>
        public EcellValue(double l_value)
        {
            Value = l_value;
            m_type = typeof(double);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "string" argument.
        /// </summary>
        /// <param name="l_value">The "string" value</param>
        public EcellValue(string l_value)
        {
            Value = l_value;
            m_type = typeof(string);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "List&lt;EcellValue&gt;" argument.
        /// </summary>
        /// <param name="l_value">The "List&lt;EcellValue&gt;" value</param>
        public EcellValue(List<EcellValue> l_value)
        {
            List<EcellValue> l_list = new List<EcellValue>();
            l_list.AddRange(l_value);
            Value = l_list;
            m_type = typeof(List<EcellValue>);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "List&lt;EcellValue&gt;" argument.
        /// </summary>
        /// <param name="l_ref">The "List&lt;EcellValue&gt;" value</param>
        public EcellValue(EcellReference l_ref)
        {
            List<EcellValue> l_list = new List<EcellValue>();
            EcellValue value1 = new EcellValue(l_ref.name);
            EcellValue value2 = new EcellValue(l_ref.fullID);
            EcellValue value3 = new EcellValue(l_ref.coefficient);
            EcellValue value4 = new EcellValue(l_ref.isAccessor);
            l_list.Add(value1);
            l_list.Add(value2);
            l_list.Add(value3);
            l_list.Add(value4);

            Value = l_list;
            m_type = typeof(List<EcellValue>);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "WrappedPolymorph" argument.
        /// </summary>
        /// <param name="l_value">The "WrappedPolymorph" value</param>
        public EcellValue(WrappedPolymorph l_value)
        {
            if (l_value == null)
            {
                Value = "";
                m_type = typeof(string);
            }
            else
            {
                if (l_value.IsDouble())
                {
                    Value = l_value.CastToDouble();
                    m_type = typeof(double);
                }
                else if (l_value.IsInt())
                {
                    Value = l_value.CastToInt();
                    m_type = typeof(int);
                }
                else if (l_value.IsList())
                {
                    Value = this.CastToEcellValue4WrappedPolymorph(l_value.CastToList());
                    m_type = typeof(List<EcellValue>);
                }
                else
                {
                    Value = l_value.CastToString();
                    m_type = typeof(string);
                }
            }
        }
        #endregion

        #region Accessors
        /// <summary>
        /// The property of the type of the stored value.
        /// </summary>
        public Type Type
        {
            get { return this.m_type; }
        }

        /// <summary>
        /// The property of the stored value.
        /// </summary>
        public Object Value
        {
            get { return this.m_value; }
            set {
                this.m_value = value;
                m_type = this.Value.GetType();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Convert to EcellValue from string.
        /// </summary>
        /// <param name="l_str">string.</param>
        /// <returns>EcellValue.</returns>
        public static EcellValue ToList(string l_str)
        {
            List<EcellValue> list = new List<EcellValue>();
            if (l_str == null || l_str == "") return new EcellValue(list);

            string text = l_str.Substring(1);
            text = text.Substring(0, text.Length - 1);
            Regex reg = new Regex("\"(?<refer>.+?)\"");

            MatchCollection coll = reg.Matches(text);
            IEnumerator iter = coll.GetEnumerator();
            while (iter.MoveNext())
            {
                Match m1 = (Match)iter.Current;
                string refStr = m1.Groups["refer"].Value;
                list.Add(new EcellValue(refStr));
            }
            return new EcellValue(list);
        }

        /// <summary>
        /// Returns the "ArrayList" casting value.
        /// </summary>
        /// <param name="l_ecellValue">The "EcellValue" value</param>
        /// <returns>The "ArrayList" value</returns>
        static public ArrayList CastToArrayList4EcellValue(EcellLib.EcellValue l_ecellValue)
        {
            if (l_ecellValue == null)
            {
                return null;
            }
            if (l_ecellValue.IsDouble())
            {
                ArrayList l_arrayList = new ArrayList();
                l_arrayList.Add(l_ecellValue.CastToDouble().ToString());
                return l_arrayList;
            }
            else if (l_ecellValue.IsInt())
            {
                ArrayList l_arrayList = new ArrayList();
                l_arrayList.Add(l_ecellValue.CastToInt().ToString());
                return l_arrayList;
            }
            else if (l_ecellValue.IsList())
            {
                ArrayList l_arrayList = new ArrayList();
                foreach (EcellValue l_childEcellValue in l_ecellValue.CastToList())
                {
                    ArrayList l_childList = CastToArrayList4EcellValue(l_childEcellValue);
                    l_arrayList.AddRange(l_childList);
                }
                return l_arrayList;
            }
            else
            {
                ArrayList l_arrayList = new ArrayList();
                l_arrayList.Add(l_ecellValue.CastToString());
                return l_arrayList;
            }
        }

        /// <summary>
        /// Returns the "double" casting value.
        /// </summary>
        /// <returns>The "double" value</returns>
        public double CastToDouble()
        {
            if (this.IsDouble())
            {
                return Convert.ToDouble(this.Value);
            }
            else
            {
                return default(double);
            }
        }

        /// <summary>
        /// Returns the "EcellValue" casting value 4 "WrappedPolymorph".
        /// </summary>
        /// <param name="l_polymorphList">The list of a "WrappedPolymorph" value</param>
        /// <returns>The list of a "EcellValue"</returns>
        private List<EcellValue> CastToEcellValue4WrappedPolymorph(List<WrappedPolymorph> l_polymorphList)
        {
            List<EcellValue> l_ecellValueList = new List<EcellValue>();
            foreach (WrappedPolymorph l_polymorph in l_polymorphList)
            {
                if (l_polymorph.IsDouble())
                {
                    l_ecellValueList.Add(new EcellValue(l_polymorph.CastToDouble()));
                }
                else if (l_polymorph.IsInt())
                {
                    l_ecellValueList.Add(new EcellValue(l_polymorph.CastToInt()));
                }
                else if (l_polymorph.IsList())
                {
                    l_ecellValueList.Add(new EcellValue(this.CastToEcellValue4WrappedPolymorph(l_polymorph.CastToList())));
                }
                else
                {
                    l_ecellValueList.Add(new EcellValue(l_polymorph.CastToString()));
                }
            }
            return l_ecellValueList;
        }

        /// <summary>
        /// Returns the "int" casting value.
        /// </summary>
        /// <returns>The "int" value</returns>
        public int CastToInt()
        {
            if (this.IsInt())
            {
                return (int)this.Value;
            }
            else
            {
                return default(int);
            }
        }

        /// <summary>
        /// Returns the list of EcellValue casting value.
        /// </summary>
        /// <returns>The list of EcellValue</returns>
        public List<EcellValue> CastToList()
        {
            if (this.IsList())
            {
                return this.Value as List<EcellValue>;
            }
            else
            {
                return default(List<EcellValue>);
            }
        }

        /// <summary>
        /// Returns the "string" casting value.
        /// </summary>
        /// <returns>The "string" value</returns>
        public string CastToString()
        {
            if (this.IsString())
            {
                return this.Value as string;
            }
            else
            {
                return default(string);
            }
        }

        /// <summary>
        /// Returns the "WrappedPolymorph" casting value.
        /// </summary>
        /// <param name="l_ecellValue">The "EcellValue" value</param>
        /// <returns>The "WrappedPolymorph" value</returns>
        static public WrappedPolymorph CastToWrappedPolymorph4EcellValue(EcellLib.EcellValue l_ecellValue)
        {
            if (l_ecellValue.IsDouble())
            {
                return new WrappedPolymorph(l_ecellValue.CastToDouble());
            }
            else if (l_ecellValue.IsInt())
            {
                return new WrappedPolymorph(l_ecellValue.CastToInt());
            }
            else if (l_ecellValue.IsList())
            {
                List<WrappedPolymorph> l_wrappedPolymorphList = new List<WrappedPolymorph>();
                foreach (EcellValue l_childEcellValue in l_ecellValue.CastToList())
                {
                    l_wrappedPolymorphList.Add(CastToWrappedPolymorph4EcellValue(l_childEcellValue));
                }
                return new WrappedPolymorph(l_wrappedPolymorphList);
            }
            else
            {
                return new WrappedPolymorph(l_ecellValue.CastToString());
            }
        }

        /// <summary>
        /// Casts the value to "string".
        /// </summary>
        /// <returns>The "string" value</returns>
        public override string ToString()
        {
            if (this.Value == null)
            {
                return null;
            }
            if (this.IsList())
            {
                return this.ToString4List((List<EcellValue>)this.Value);
            }
            else
            {
                return this.Value.ToString();
            }
        }

        /// <summary>
        /// Convert to EcellValue from string.
        /// </summary>
        /// <param name="l_str">string.</param>
        /// <returns>EcellValue.</returns>
        public static EcellValue ToVariableReferenceList(string l_str)
        {
            List<EcellValue> list = new List<EcellValue>();
            if (l_str == null || l_str == "") return new EcellValue(list);

            string text = l_str.Substring(1);
            text = text.Substring(0, text.Length - 1);
            Regex reg = new Regex("\\((?<refer>.+?)\\)");

            MatchCollection coll = reg.Matches(text);
            IEnumerator iter = coll.GetEnumerator();
            while (iter.MoveNext())
            {
                Match m1 = (Match)iter.Current;
                string refStr = m1.Groups["refer"].Value;
                /*
                Regex regObj =
                    new Regex("\"(?<name>.+)\",(.+)\"(?<id>.+)\",(.+)(?<coe>.+),(.+)(?<fix>.+)");
                 */
                Regex regObj =
                    new Regex("(.*)\"(?<name>.+)\"(.*),(.*)\"(?<id>.+)\"(.*),(?<coe>.+),(?<fix>.+)");
                Match m = regObj.Match(refStr);
                if (m.Success)
                {
                    List<EcellValue> tmpList = new List<EcellValue>();
                    tmpList.Add(new EcellValue(m.Groups["name"].Value.Trim()));
                    tmpList.Add(new EcellValue(m.Groups["id"].Value.Trim()));
                    tmpList.Add(new EcellValue(Convert.ToInt32(m.Groups["coe"].Value.Trim())));
                    tmpList.Add(new EcellValue(Convert.ToInt32(m.Groups["fix"].Value.Trim())));
                    list.Add(new EcellValue(tmpList));
                }
            }
            return new EcellValue(list);
        }

        /// <summary>
        /// Casts the list of "EcellObject" to "string"
        /// </summary>
        /// <param name="l_ecellValueList">The list of "EcellValue"</param>
        /// <returns>The "string" value</returns>
        private string ToString4List(List<EcellValue> l_ecellValueList)
        {
            String l_value = "";
            foreach (EcellValue l_ecellValue in l_ecellValueList)
            {
                if (l_ecellValue.IsList())
                {
                    l_value += ", " + this.ToString4List(l_ecellValue.CastToList());
                }
                else if (l_ecellValue.IsInt())
                {
                    l_value += ", " + l_ecellValue.CastToInt();
                }
                else if (l_ecellValue.IsDouble())
                {
                    l_value += ", " + l_ecellValue.CastToDouble();
                }
                else
                {
                    l_value += ", " + "\"" + l_ecellValue.ToString() + "\"";
                }
            }
            if (l_value.Length >= 2)
            {
                l_value = "(" + l_value.Substring(2) + ")";
            }
            else
            {
                l_value = "(" + l_value + ")";
            }
            return l_value;
        }

        /// <summary>
        /// Tests whether the type is a "double" type.
        /// </summary>
        /// <returns>true if the type is "double"; false otherwise</returns>
        public bool IsDouble()
        {
            if (this.m_type == typeof(double))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tests whether the type is a "int" type.
        /// </summary>
        /// <returns>true if the type is "int"; false otherwise</returns>
        public bool IsInt()
        {
            if (this.m_type == typeof(int))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tests whether the type is the list of EcellValue type.
        /// </summary>
        /// <returns>true if the type is the list of EcellValue; false otherwise</returns>
        public bool IsList()
        {
            if (this.m_type == typeof(List<EcellValue>))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tests whether the type is a "string" type.
        /// </summary>
        /// <returns>true if the type is "string"; false otherwise</returns>
        public bool IsString()
        {
            if (this.m_type == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copy EcellValue.
        /// </summary>
        /// <returns>EcellValue</returns>
        public EcellValue Copy()
        {
            if (IsList())
            {
                List<EcellValue> list = new List<EcellValue>();
                foreach (EcellValue value in this.CastToList())
                    list.Add(value.Copy());
                return new EcellValue(list);
            }
            else if (IsInt())
            {
                return new EcellValue(this.CastToInt());
            }
            else if (IsDouble())
            {
                return new EcellValue(this.CastToDouble());
            }
            else if (IsString())
            {
                return new EcellValue(this.CastToString());
            }
            else return null;
        }
        #endregion
    }


    /// <summary>
    /// Object class for Reference.
    /// </summary>
    public class EcellReference
    {
        #region Fields
        private string m_name;
        private string m_fullID;
        private int m_coeff;
        private int m_accessor;
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellReference()
        {
        }

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="str">string.</param>
        public EcellReference(string str)
        {
            Regex reg =
                  new Regex("\"(?<name>.+)\",(.+)\"(?<id>.+)\", (?<coe>.+), (?<fix>.+)");
            Match m = reg.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value);
                this.m_accessor = Convert.ToInt32(m.Groups["fix"].Value);
            }
        }
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="value">EcellValue</param>
        public EcellReference(EcellValue value)
        {
            List<EcellValue> list = value.CastToList();
            if (list.Count != 4)
                return;
            this.m_name = list[0].CastToString();
            this.m_fullID = list[1].CastToString();
            this.m_coeff = list[2].CastToInt();
            this.m_accessor = list[3].CastToInt();
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set name.
        /// </summary>
        public string name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// get / set full ID.
        /// </summary>
        public string fullID
        {
            get { return this.m_fullID; }
            set { this.m_fullID = value; }
        }

        /// <summary>
        /// get / set full ID.
        /// </summary>
        public string Key
        {
            get { return this.m_fullID.Substring(1); }
            set { this.m_fullID = ":" + value; }
        }

        /// <summary>
        /// get / set coefficient.
        /// </summary>
        public int coefficient
        {
            get { return this.m_coeff; }
            set { this.m_coeff = value; }
        }

        /// <summary>
        /// get / set whether this properties is accessor.
        /// </summary>
        public int isAccessor
        {
            get { return this.m_accessor; }
            set { this.m_accessor = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get string of object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";
            str = "(\"" + m_name + "\", \"" + m_fullID + "\", " + m_coeff + ", " + m_accessor + ")";
            return str;
        }

        /// <summary>
        /// Copy EcellReference.
        /// </summary>
        /// <returns></returns>
        public EcellReference Copy()
        {
            EcellReference er = new EcellReference();
            er.m_name = this.m_name;
            er.m_fullID = this.m_fullID;
            er.m_coeff = this.m_coeff;
            er.m_accessor = this.m_accessor;

            return er;
        }

        /// <summary>
        /// Get the list of reference from string.
        /// </summary>
        /// <param name="str">string.</param>
        /// <returns>the list of reference.</returns>
        public static List<EcellReference> ConvertString(string str)
        {
            List<EcellReference> list = new List<EcellReference>();
            if (str == null || str == "") return list;
            string text = str.Substring(1);
            text = text.Substring(0, text.Length - 1);
            Regex reg = new Regex("\\((?<refer>.+?)\\)");

            MatchCollection coll = reg.Matches(text);
            IEnumerator iter = coll.GetEnumerator();
            while (iter.MoveNext())
            {
                Match m1 = (Match)iter.Current;
                EcellReference v = new EcellReference(m1.Groups["refer"].Value);
                list.Add(v);
            }
            return list;
        }

        /// <summary>
        /// Get the list of reference from VariableReferenceList.
        /// </summary>
        /// <param name="varRef">VariableReferenceList.</param>
        /// <returns>the list of EcellReference.</returns>
        public static List<EcellReference> ConvertFromVarRefList(EcellValue varRef)
        {
            List<EcellValue> varRefList = varRef.CastToList();
            List<EcellReference> list = new List<EcellReference>();
            if (varRefList == null || varRefList.Count == 0)
                return list;
            foreach (EcellValue value in varRefList)
            {
                EcellReference er = new EcellReference(value);
                list.Add(er);
            }
            return list;
        }

        /// <summary>
        /// Get the list of reference from VariableReferenceList.
        /// </summary>
        /// <param name="refList">VariableReferenceList.</param>
        /// <returns>the list of EcellReference.</returns>
        public static EcellValue ConvertToVarRefList(List<EcellReference> refList)
        {
            List<EcellValue> list = new List<EcellValue>();
            if (refList == null || refList.Count == 0)
                return new EcellValue(list);

            foreach (EcellReference er in refList)
            {
                EcellValue value = new EcellValue(er);
                list.Add(value);
            }
            return new EcellValue(list);
        }
        #endregion
    }


    /// <summary>
    /// Object class for System.
    /// </summary>
    public class EcellSystem : EcellObject
    {

        #region Constant
        /// <summary>
        /// Size name. The reserved name.
        /// </summary>
        public const string SIZE = "Size";
        #endregion

        #region Fields
        /// <summary>
        /// List of child systems;
        /// </summary>
        List<EcellSystem> m_childSystems = new List<EcellSystem>();
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellSystem()
        {
        }

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="System").</param>
        /// <param name="l_class">class name.</param>
        /// <param name="l_data">properties.</param>
        public EcellSystem(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.SetEcellDatas(l_data);
            this.Children = new List<EcellObject>();
        }
        #endregion

        #region Accessors

        /// <summary>
        /// get / set size;
        /// </summary>
        public double size
        {
            get {
                if (IsEcellValueExists(SIZE))
                    return GetEcellValue(SIZE).CastToDouble();
                else
                    return 0.1d;
                }
            set {
                if (IsEcellValueExists(SIZE))
                    GetEcellValue(SIZE).Value = value;
                else
                    AddEcellValue(SIZE, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set Stepper ID.
        /// </summary>
        public string stepperID
        {
            get {
                if (IsEcellValueExists("StepperID"))
                    return GetEcellValue("StepperID").ToString();
                else
                    return null;
                }
            set {
                if (IsEcellValueExists("StepperID"))
                    GetEcellValue("StepperID").Value = value;
                else
                    AddEcellValue("StepperID", new EcellValue(value) );
            }
        }

        /// <summary>
        /// get/set system name.
        /// </summary>
        public new string Text
        {
            get
            {
                string text = base.Text;
                if (size != 0.1d)
                    text += " (SIZE:" + GetEcellValue(SIZE).ToString() +")";
                return text;
            }
        }
        #endregion
    }


    /// <summary>
    /// Object class for Variable.
    /// </summary>
    public class EcellVariable : EcellObject
    {
        #region Fields
        #endregion

        #region Constractors
        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellVariable()
        {
        }
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="Variable").</param>
        /// <param name="l_class">class name.</param>
        /// <param name="l_data">properties.</param>
        public EcellVariable(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.SetEcellDatas(l_data);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the molar concentrate.
        /// </summary>
        public double molarConc
        {
            get {
                if (IsEcellValueExists("MolarConc"))
                    return GetEcellValue("MolarConc").CastToDouble();
                else
                    return 0;
                }
            set {
                if (IsEcellValueExists("MolarConc"))
                    GetEcellValue("MolarConc").Value = value;
                else
                    AddEcellValue("MolarConc", new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set the number of concentrate.
        /// </summary>
        public double numberConc
        {
            get
            {
                if (IsEcellValueExists("NumberConc"))
                    return GetEcellValue("NumberConc").CastToDouble();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists("NumberConc"))
                    GetEcellValue("NumberConc").Value = value;
                else
                    AddEcellValue("NumberConc", new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set total velocity.
        /// </summary>
        public double totalVelocity
        {
            get
            {
                if (IsEcellValueExists("TotalVelocity"))
                    return GetEcellValue("TotalVelocity").CastToDouble();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists("TotalVelocity"))
                    GetEcellValue("TotalVelocity").Value = value;
                else
                    AddEcellValue("TotalVelocity", new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set value.
        /// </summary>
        public double value
        {
            get
            {
                if (IsEcellValueExists("Value"))
                    return GetEcellValue("Value").CastToDouble();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists("Value"))
                    GetEcellValue("Value").Value = value;
                else
                    AddEcellValue("Value", new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set velocity.
        /// </summary>
        public double velocity
        {
            get
            {
                if (IsEcellValueExists("Velocity"))
                    return GetEcellValue("Velocity").CastToDouble();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists("Velocity"))
                    GetEcellValue("Velocity").Value = value;
                else
                    AddEcellValue("Velocity", new EcellValue(value));
            }
        }
        #endregion

        #region Methods
        #endregion
    }


    /// <summary>
    /// Object class for Model.
    /// </summary>
    public class EcellModel : EcellObject
    {
        #region Constractors
        /// <summary>
        /// constructor with initial parameter.
        /// </summary>
        /// <param name="l_modelID">modelID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="Model")</param>
        /// <param name="l_class">class name</param>
        /// <param name="l_data">properties of object.</param>
        public EcellModel(string l_modelID, string l_key,
             string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.m_ecellDatas = l_data;
        }
        #endregion
    }


    /// <summary>
    /// Object class for Process.
    /// </summary>
    public class EcellProcess : EcellObject
    {
        #region Constants
        /// <summary>
        /// VariableReferenceList. The reserved name.
        /// </summary>
        public const string VARIABLEREFERENCELIST = "VariableReferenceList";
        /// <summary>
        /// Activity. The reserved name.
        /// </summary>
        public const string ACTIVITY = "Activity";
        /// <summary>
        /// Expression. The reserved name.
        /// </summary>
        public const string EXPRESSION = "Expression";
        /// <summary>
        /// IsContinuous. The reserved name.
        /// </summary>
        public const string ISCONTINUOUS = "IsContinuous";
        /// <summary>
        /// Name. The reserved name.
        /// </summary>
        public const string NAME = "Name";
        /// <summary>
        /// Priority. The reserved name.
        /// </summary>
        public const string PRIORITY = "Priority";
        /// <summary>
        /// StepperID. The reserved name.
        /// </summary>
        public const string STEPPERID = "StepperID";
        #endregion

        #region Fields
        #endregion
        
        #region Constractors
        /// <summary>
        /// Constructor.
        /// </summary>
        public EcellProcess()
        {
        }
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="l_modelID">model ID.</param>
        /// <param name="l_key">key.</param>
        /// <param name="l_type">type(="Variable").</param>
        /// <param name="l_class">class name.</param>
        /// <param name="l_data">properties.</param>
        public EcellProcess(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.SetEcellDatas(l_data);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get /set the activity.
        /// </summary>
        public double activity
        {
            get
            {
                if (IsEcellValueExists(ACTIVITY))
                    return GetEcellValue(ACTIVITY).CastToDouble();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists(ACTIVITY))
                    GetEcellValue(ACTIVITY).Value = value;
                else
                    AddEcellValue(ACTIVITY, new EcellValue(value));
            }
        }

        /// <summary>
        /// get /set the expression of process.
        /// </summary>
        public string expression
        {
            get {
                if ( IsEcellValueExists(EXPRESSION) )
                    return GetEcellValue(EXPRESSION).ToString();
                else
                    return null;
                }
            set {
                if (IsEcellValueExists(EXPRESSION))
                    GetEcellValue(EXPRESSION).Value = value;
                else
                    AddEcellValue(EXPRESSION, new EcellValue(value) );
            }
        }

        /// <summary>
        /// get / set whether this property is continious.
        /// </summary>
        public int isContinuous
        {
            get
            {
                if (IsEcellValueExists(ISCONTINUOUS))
                    return GetEcellValue(ISCONTINUOUS).CastToInt();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists(ISCONTINUOUS))
                    GetEcellValue(ISCONTINUOUS).Value = value;
                else
                    AddEcellValue(ISCONTINUOUS, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set priority.
        /// </summary>
        public int priority
        {
            get
            {
                if (IsEcellValueExists(PRIORITY))
                    return GetEcellValue(PRIORITY).CastToInt();
                else
                    return 0;
            }
            set
            {
                if (IsEcellValueExists(PRIORITY))
                    GetEcellValue(PRIORITY).Value = value;
                else
                    AddEcellValue(PRIORITY, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set stepperID.
        /// </summary>
        public string stepperID
        {
            get
            {
                if (IsEcellValueExists(STEPPERID))
                    return GetEcellValue(STEPPERID).ToString();
                else
                    return null;
            }
            set
            {
                if (IsEcellValueExists(STEPPERID))
                    GetEcellValue(STEPPERID).Value = value;
                else
                    AddEcellValue(STEPPERID, new EcellValue(value));
            }
        }

        /// <summary>
        /// get / set the property of VariableReferenceList.
        /// </summary>
        public List<EcellReference> ReferenceList
        {
            get { return EcellReference.ConvertFromVarRefList(this.GetEcellValue(VARIABLEREFERENCELIST)); }
            set {
                EcellValue varRef = EcellReference.ConvertToVarRefList(value);
                this.GetEcellData(VARIABLEREFERENCELIST).Value = varRef;
            }
        }
        #endregion

        #region Methods
        #endregion
    }

    /// <summary>
    /// EcellObject to drag and drop.
    /// </summary>
    public class EcellDragObject
    {
        string m_modelID;
        string m_key;
        string m_type;
        string m_path;

        /// <summary>
        /// Constructor without initial parameters.
        /// </summary>
        public EcellDragObject()
        {
            m_modelID = "";
            m_key = "";
            m_type = "";
            m_path = "";
        }

        /// <summary>
        /// Constructor with initial parameters.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="path"></param>
        public EcellDragObject(string modelID, string key, string type, string path)
        {
            m_modelID = modelID;
            m_key = key;
            m_type = type;
            m_path = path;
        }

        /// <summary>
        /// get/set model ID.
        /// </summary>
        public String ModelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }
        }

        /// <summary>
        /// get/set key of object.
        /// </summary>
        public String Key
        {
            get { return this.m_key; }
            set { this.m_key = value; }
        }

        /// <summary>
        /// get/set type of object.
        /// </summary>
        public String Type
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }

        /// <summary>
        /// get/set entity path of logger.
        /// </summary>
        public String Path
        {
            get { return this.m_path; }
            set { this.m_path = value; }
        }
    }
}
