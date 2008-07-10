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

namespace Ecell.Objects
{
    /// <summary>
    /// The base class of E-CELL model editor.
    /// </summary>
    public class EcellObject
    {
        #region Constant
        /// <summary>
        /// Type string of "Project".
        /// </summary>
        public const string PROJECT = "Project";
        /// <summary>
        /// Type string of "Model".
        /// </summary>
        public const string MODEL = "Model";
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
        /// Type string of "Variable".
        /// </summary>
        public const string TEXT = "Text";
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
        private string m_layerID = "";
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
        protected List<EcellObject> m_children;
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
        public virtual string ModelID
        {
            get { return m_modelID; }
            set { m_modelID = value; }
        }

        /// <summary>
        /// get / set name.
        /// </summary>
        public virtual string Name
        {
            get {
                string name;
                if (string.IsNullOrEmpty(m_key) || m_key.Equals("/"))
                    name = "/";
                else if (m_key.Contains(":"))
                    name = m_key.Substring(m_key.LastIndexOf(":") + 1);
                else
                    name = m_key.Substring(m_key.LastIndexOf("/") + 1);
                return name;
            }
            set
            {
                if (m_key == null || m_key.Equals("/"))
                    this.m_key = "/";
                else if (m_key.Contains(":"))
                    this.m_key = ParentSystemID + ":" + value;
                else
                    this.m_key = ParentSystemID + "/" + value;
            }
        }

        /// <summary>
        /// get/set m_keyID.
        /// </summary>
        public virtual string Key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        /// <summary>
        /// get parent system ID.
        /// </summary>
        public virtual string ParentSystemID
        {
            get { return GetParentSystemId(m_key); }
            set
            {
                if (this.m_type == EcellObject.PROCESS || this.m_type == EcellObject.VARIABLE)
                {
                    this.m_key = value + ":" + this.Name;
                    return;
                }
                else if (m_key == null || m_key.Equals("/"))
                    this.m_key = "/";
                else if (value.Equals("/"))
                    this.m_key = value + this.Name;
                else
                    this.m_key = value + "/" + this.Name;

                foreach (EcellObject eo in m_children)
                    eo.ParentSystemID = this.m_key;
            }

        }

        /// <summary>
        /// get/set m_class.
        /// </summary>
        public virtual string Classname
        {
            get { return m_class; }
            set { this.m_class = value; }
        }

        /// <summary>
        /// get/set the layer property.
        /// </summary>
        public virtual string LayerID
        {
            get { return this.m_layerID; }
            set { this.m_layerID = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public virtual PointF PointF
        {
            get { return new PointF(m_x, m_y); }
            set
            {
                m_x = value.X;
                m_y = value.Y;
            }
        }

        /// <summary>
        /// Accessor for CenterPointF.
        /// </summary>
        public PointF CenterPointF
        {
            get { return new PointF(m_x + m_offsetX + m_width / 2f, m_y + m_offsetY + m_height / 2f); }
            set
            {
                m_x = value.X - m_width / 2f;
                m_y = value.Y - m_height / 2f;
            }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public virtual RectangleF Rect
        {
            get { return new RectangleF(m_x, m_y, m_width, m_height); }
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public virtual float X
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
        public virtual float Y
        {
            get { return m_y; }
            set {
                m_y = value;
            }
        }

        /// <summary>
        /// X offset
        /// </summary>
        public virtual float OffsetX
        {
            get { return m_offsetX; }
            set { m_offsetX = value; }
        }

        /// <summary>
        /// Y offset
        /// </summary>
        public virtual float OffsetY
        {
            get { return m_offsetY; }
            set { m_offsetY = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public virtual float Width
        {
            get { return m_width; }
            set {
                m_width = value;
            }
        }

        /// <summary>
        /// Height
        /// </summary>
        public virtual float Height
        {
            get { return m_height; }
            set {
                m_height = value;
            }
        }

        /// <summary>
        /// get isLogger.
        /// </summary>
        public virtual bool Logged
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
        public virtual string Type
        {
            get { return m_type; }
            set { this.m_type = value; }
        }

        /// <summary>
        /// get/set Value.
        /// </summary>
        [Browsable(false)]
        public virtual List<EcellData> Value
        {
            get { return m_ecellDatas; }
            // set { this.Value = value; }
        }

        /// <summary>
        /// get/set Children.
        /// </summary>
        [Browsable(false)]
        public virtual List<EcellObject> Children
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
        public virtual bool IsPosSet
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
        public virtual bool isFixed
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
        public virtual EcellObject Copy()
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
                l_newEcellObject.LayerID = this.LayerID;
                l_newEcellObject.Children = this.CopyChildren();
                return l_newEcellObject;
            }
            catch (Exception l_ex)
            {
                throw new Exception(String.Format(MessageResources.ErrCopy,
                    new object[] { this.Key }), l_ex);
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
            this.LayerID = obj.LayerID;
        }

        /// <summary>
        /// Set Moving delta.
        /// </summary>
        /// <param name="delta"></param>
        public void MovePosition(PointF delta)
        {
            this.X = this.X + delta.X;
            this.Y = this.Y + delta.Y;
        }

        /// <summary>
        /// get parent system ID.
        /// </summary>
        /// <param name="key">The key</param>
        public static string GetParentSystemId(string key)
        {
            Regex postColonRegex = new Regex(":\\w*$");
            Regex postSlashRegex = new Regex("/\\w*$");
            if (string.IsNullOrEmpty(key) || key.Equals("/"))
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
            else if (TEXT.Equals(l_type))
                return new EcellText(l_modelID, l_key, l_type, l_class, l_data);
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
            string entytyPath = this.m_type + ":" + this.m_key + ":" + name;
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
            sb.Append(Type);
            sb.Append("(name=" + Name + ")" + "(Location="+ this.Rect.ToString()+ ")");
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
}
