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
using Ecell.Exceptions;

namespace Ecell.Objects
{
    /// <summary>
    /// The base class of E-CELL model editor.
    /// </summary>
    public class EcellObject : ICloneable
    {
        #region Constant
        /// <summary>
        /// Type string of "Project".
        /// </summary>
        public const string PROJECT = Constants.xpathProject;
        /// <summary>
        /// Type string of "Model".
        /// </summary>
        public const string MODEL = Constants.xpathModel;
        /// <summary>
        /// Type string of "Process".
        /// </summary>
        public const string PROCESS = Constants.xpathProcess;
        /// <summary>
        /// Type string of "System".
        /// </summary>
        public const string SYSTEM = Constants.xpathSystem;
        /// <summary>
        /// Type string of "Variable".
        /// </summary>
        public const string VARIABLE = Constants.xpathVariable;
        /// <summary>
        /// Type string of "Variable".
        /// </summary>
        public const string TEXT = Constants.xpathText;
        /// <summary>
        /// Type string of "Variable".
        /// </summary>
        public const string STEPPER = Constants.xpathStepper;
        #endregion

        #region Fields
        /// <summary>
        /// The class
        /// </summary>
        protected string m_class;
        /// <summary>
        /// The model ID
        /// </summary>
        protected string m_modelID;
        /// <summary>
        /// The key
        /// </summary>
        protected string m_key;
        /// <summary>
        /// The type of object.
        /// </summary>
        protected string m_type;
        /// <summary>
        /// The layout struct of EcellObject.
        /// </summary>
        protected EcellLayout m_layout;
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
        protected bool m_isFixed = false;
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
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key</param>
        /// <param name="type">The type</param>
        /// <param name="classname">The class</param>
        /// <param name="data">The data</param>
        protected EcellObject(string modelID, string key,
            string type, string classname, List<EcellData> data): this()
        {
            this.m_modelID = modelID;
            this.m_key = key;
            this.m_type = type;
            this.m_class = classname;
            this.m_ecellDatas = data;
            this.m_layout = new EcellLayout();
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
        public virtual string LocalID
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
        /// FullID
        /// </summary>
        public virtual string FullID
        {
            get { return m_type + Constants.delimiterColon + m_key; }
        }

        /// <summary>
        /// get parent system ID.
        /// </summary>
        public virtual string ParentSystemID
        {
            get { return Util.GetSuperSystemPath(m_key); }
            set
            {
                if (this.m_type == EcellObject.PROCESS || this.m_type == EcellObject.VARIABLE)
                {
                    this.m_key = value + ":" + this.LocalID;
                    return;
                }
                else if (m_key == null || m_key.Equals("/"))
                    this.m_key = "/";
                else if (value.Equals("/"))
                    this.m_key = value + this.LocalID;
                else
                    this.m_key = value + "/" + this.LocalID;

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
        /// get/set the EcellLayout.
        /// </summary>
        public virtual EcellLayout Layout
        {
            get { return m_layout; }
            set { m_layout = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public virtual string Layer
        {
            get { return m_layout.Layer; }
            set { m_layout.Layer = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public virtual PointF PointF
        {
            get { return m_layout.Location; }
            set { m_layout.Location = value; }
        }

        /// <summary>
        /// Accessor for CenterPointF.
        /// </summary>
        public PointF CenterPointF
        {
            get { return m_layout.Center; }
            set { m_layout.Center = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public virtual RectangleF Rect
        {
            get { return m_layout.Rect; }
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public virtual float X
        {
            get { return m_layout.X; }
            set { m_layout.X = value; }
        }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public virtual float Y
        {
            get { return m_layout.Y; }
            set { m_layout.Y = value; }
        }

        /// <summary>
        /// X offset
        /// </summary>
        public virtual float OffsetX
        {
            get { return m_layout.OffsetX; }
            set { m_layout.OffsetX = value; }
        }

        /// <summary>
        /// Y offset
        /// </summary>
        public virtual float OffsetY
        {
            get { return m_layout.OffsetY; }
            set { m_layout.OffsetY = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public virtual float Width
        {
            get { return m_layout.Width; }
            set { m_layout.Width = value; }
        }

        /// <summary>
        /// Height
        /// </summary>
        public virtual float Height
        {
            get { return m_layout.Height; }
            set { m_layout.Height = value; }
        }

        /// <summary>
        /// get isLogger.
        /// </summary>
        public virtual bool Logged
        {
            get
            {
                //return true if any Logger exists.
                if (m_ecellDatas != null)
                {
                    foreach (EcellData d in m_ecellDatas)
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

        /// <summary>
        /// Tests whether the "EcellObject" is usable.
        /// </summary>
        public bool IsUsable
        {
            get
            {
                if (string.IsNullOrEmpty(m_modelID))
                    return false;
                if (string.IsNullOrEmpty(m_type))
                    return false;
                // 4 "Process", "Stepper", "System" and "Variable"
                if (!m_type.Equals(Constants.xpathProject) && !m_type.Equals(Constants.xpathModel))
                {
                    if (string.IsNullOrEmpty(m_key))
                        return false;
                    if (string.IsNullOrEmpty(m_class))
                        return false;
                }
                return true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Copy coordinates of passed object.
        /// </summary>
        /// <param name="obj">EcellObject</param>
        public void SetPosition(EcellObject obj)
        {
            m_layout = obj.Layout;
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
        /// Returns the new "EcellObject" instance with initialized arguments.
        /// </summary>
        /// <param name="modelID">The model ID</param>
        /// <param name="key">The key</param>
        /// <param name="type">The type</param>
        /// <param name="classname">The class</param>
        /// <param name="data">The data</param>
        /// <returns>The new "EcellObject" instance</returns>
        public static EcellObject CreateObject(string modelID, string key,
            string type, string classname, List<EcellData> data)
        {
            if (type.Equals(MODEL))
                return new EcellModel(modelID, key, type, classname, data);
            else if (type.Equals(PROCESS))
                return new EcellProcess(modelID, key, type, classname, data);
            else if (type.Equals(VARIABLE))
                return new EcellVariable(modelID, key, type, classname, data);
            else if (type.Equals(SYSTEM))
                return new EcellSystem(modelID, key, type, classname, data);
            else if (type.Equals(TEXT))
                return new EcellText(modelID, key, type, classname, data);
            else if (type.Equals(STEPPER))
                return new EcellStepper(modelID, key, type, classname, data);
            else
                return new EcellObject(modelID, key, type, classname, data);
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
        /// Remove EcellData.
        /// </summary>
        /// <param name="name"></param>
        public void RemoveValue(string name)
        {
            foreach (EcellData data in m_ecellDatas)
            {
                if (!data.Name.Equals(name))
                    continue;
                m_ecellDatas.Remove(data);
                break;
            }
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
        /// Set EcellValue
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetEcellValue(string name, EcellValue value)
        {
            foreach (EcellData d in Value)
            {
                if (d.Name == name)
                {
                    d.Value = value;
                    return;
                }
            }
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
            sb.Append("(localID=" + LocalID + ")" + "(Location="+ m_layout.Rect.ToString()+ ")");
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

        #region ICloneable ÉÅÉìÉo
        /// <summary>
        /// Create a copy of this EcellObject.
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Create a copy of this EcellObject.
        /// </summary>
        /// <returns>The copy "EcellObject"</returns>
        public virtual EcellObject Clone()
        {
            try
            {
                EcellObject newEcellObject =
                    CreateObject(this.m_modelID, this.m_key, this.m_type, this.m_class, this.CopyValueList());
                newEcellObject.Layout = this.m_layout;
                newEcellObject.Children = this.CopyChildren();
                return newEcellObject;
            }
            catch (Exception ex)
            {
                throw new EcellException(String.Format(MessageResources.ErrCopy,
                    new object[] { this.Key }), ex);
            }
        }

        private List<EcellData> CopyValueList()
        {
            List<EcellData> copyValueList = null;
            if (this.m_ecellDatas != null)
            {
                copyValueList = new List<EcellData>();
                if (this.m_ecellDatas.Count > 0)
                {
                    foreach (EcellData data in this.m_ecellDatas)
                    {
                        copyValueList.Add((EcellData)data.Clone());
                    }
                }
            }
            return copyValueList;
        }

        private List<EcellObject> CopyChildren()
        {
            List<EcellObject> list = new List<EcellObject>();
            foreach (EcellObject ecellObject in this.m_children)
            {
                list.Add(ecellObject.Clone());
            }
            return list;
        }
        #endregion
    }
}
