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

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EcellCoreLib;
using System.Text.RegularExpressions;


namespace EcellLib
{
    /// <summary>
    /// The base class of E-CELL model editor.
    /// </summary>
    public class EcellObject
    {
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
        /// The type
        /// </summary>
        private string m_type;
        /// <summary>
        /// The value
        /// </summary>
        protected List<EcellData> m_value;
        /// <summary>
        /// The children of this
        /// </summary>
        private List<EcellObject> m_instances;
        #endregion

        /// <summary>
        /// Creates the new "EcellObject" instance with no argument.
        /// </summary>
        protected EcellObject()
        {
            //throw new Exception("Don't use the method or operation.");
            // m_modelID = "Galakta";
            // m_key = "/CELL/AHO";
            // m_type = "bbbb";
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
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.m_modelID = l_modelID;
            this.m_key = l_key;
            this.m_type = l_type;
            this.m_class = l_class;
            this.m_value = l_data;
        }

        /// <summary>
        /// get/set m_modelID.
        /// </summary>
        public string modelID
        {
            get { return m_modelID; }
            set { m_modelID = value; }
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
        /// get/set m_class.
        /// </summary>
        public string classname
        {
            get { return m_class; }
            set { this.m_class = value; }
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
        /// get/set m_value.
        /// </summary>
        [Browsable(false)]
        public List<EcellData> M_value
        {
            get { return m_value; }
            // set { this.m_value = value; }
        }

        /// <summary>
        /// get/set m_instances.
        /// </summary>
        [Browsable(false)]
        public List<EcellObject> M_instances
        {
            get { return m_instances; }
            set { this.m_instances = value; }
        }

        /// <summary>
        /// Create the copy "EcellObject".
        /// </summary>
        /// <returns>The copy "EcellObject"</returns>
        public EcellObject Copy()
        {
            try
            {
                List<EcellData> l_copyValueList = null;
                if (this.m_value != null)
                {
                    l_copyValueList = new List<EcellData>();
                    if (this.m_value.Count > 0)
                    {
                        foreach (EcellData l_value in this.m_value)
                        {
                            l_copyValueList.Add(l_value.Copy());
                        }
                    }
                }
                List<EcellObject> l_copyInstancesList = null;
                if (this.m_instances != null)
                {
                    l_copyInstancesList = new List<EcellObject>();
                    if (this.m_instances.Count > 0)
                    {
                        foreach (EcellObject l_ecellObject in this.m_instances)
                        {
                            l_copyInstancesList.Add(l_ecellObject.Copy());
                        }
                    }
                }
                EcellObject l_newEcellObject =
                    CreateObject(this.m_modelID, this.m_key, this.m_type, this.m_class, l_copyValueList);
                l_newEcellObject.M_instances = l_copyInstancesList;
                return l_newEcellObject;
            }
            catch (Exception l_ex)
            {
                throw new Exception("Can't copy the \"EcellObject\". {" + l_ex.ToString() + "}");
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
            /*
            EcellObject obj = null;
            string ecellstr = "EcellLib." + l_class;
            Type ecellType = Type.GetType(ecellstr);
            obj = (EcellObject)ecellType.InvokeMember(
                null, System.Reflection.BindingFlags.CreateInstance, 
                null, null, new object[] { l_modelID, l_key, l_type, l_class, l_data });
            return obj;
             */
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
        /// add the data to m_value.
        /// </summary>
        /// <param name="d">EcellData.</param>
        public void AddValue(EcellData d)
        {
            this.m_value.Add(d);
            this.DistributeValue(d);
        }

        protected void DistributeValue(EcellData d)
        {
        }

        public void SetValue(List<EcellData> list)
        {
            this.m_value = list;

            if (list == null) return;
            foreach (EcellData d in list)
            {
                DistributeValue(d);
            }
        }
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
        #endregion

        /// <summary>
        /// Creates the new "EcellData" instance with no argument.
        /// </summary>
        public EcellData()
        {
            this.m_name = null;
            this.m_value = null;
            this.m_entityPath = null;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogger = false;
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
            this.m_value = l_data;
            this.m_entityPath = l_entityPath;
            this.m_isGettable = true;
            this.m_isSettable = true;
            this.m_isLoadable = true;
            this.m_isSavable = true;
            this.m_isLogable = false;
            this.m_isLogger = false;
        }

        /// <summary>
        /// get/set m_name.
        /// </summary>
        public string M_name
        {
            get { return m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// get/set m_value
        /// </summary>
        public EcellValue M_value
        {
            get { return m_value; }
            set { this.m_value = value; }
        }

        /// <summary>
        /// get/set m_entityPath
        /// </summary>
        public string M_entityPath
        {
            get { return m_entityPath; }
            set { this.m_entityPath = value; }
        }

        /// <summary>
        /// get/set m_isGettable
        /// </summary>
        public bool M_isGettable
        {
            get { return m_isGettable; }
            set { this.m_isGettable = value; }
        }

        /// <summary>
        /// get/set m_isLoadable
        /// </summary>
        public bool M_isLoadable
        {
            get { return m_isLoadable; }
            set { this.m_isLoadable = value; }
        }

        /// <summary>
        /// get/set m_isLogable
        /// </summary>
        public bool M_isLogable
        {
            get { return this.m_isLogable; }
            set { this.m_isLogable = value; }
        }

        /// <summary>
        /// get/set m_isLogger
        /// </summary>
        public bool M_isLogger
        {
            get { return this.m_isLogger; }
            set { this.m_isLogger = value; }
        }

        /// <summary>
        /// get/set m_isSavable
        /// </summary>
        public bool M_isSavable
        {
            get { return m_isSavable; }
            set { this.m_isSavable = value; }
        }

        /// <summary>
        /// get/set m_isSettable
        /// </summary>
        public bool M_isSettable
        {
            get { return m_isSettable; }
            set { this.m_isSettable = value; }
        }

        /// <summary>
        /// Create the copy "EcellData".
        /// </summary>
        /// <returns>The copy "EcellData"</returns>
        public EcellData Copy()
        {
            try
            {
                EcellData l_newData = new EcellData(this.m_name, this.m_value, this.m_entityPath);
                l_newData.M_isGettable = this.m_isGettable;
                l_newData.M_isLoadable = this.m_isLoadable;
                l_newData.M_isLogable = this.m_isLogable;
                l_newData.M_isLogger = this.m_isLogger;
                l_newData.M_isSavable = this.m_isSavable;
                l_newData.M_isSettable = this.m_isSettable;
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
            if (this.m_name == l_obj.m_name && this.m_value == l_obj.m_value)
            {
                return true;
            }
            return false;
        }
    }


    /// <summary>
    /// The polymorphism 4 the "EcellData".
    /// </summary>
    public class EcellValue
    {
        /// <summary>
        /// The stored value.
        /// </summary>
        private Object m_value = null;
        /// <summary>
        /// The type of the stored value.
        /// </summary>
        private Type m_type = null;


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
            m_value = l_value;
            m_type = typeof(int);
        }


        /// <summary>
        /// Creates a new "EcellValue" instance with a "double" argument.
        /// </summary>
        /// <param name="l_value">The "double" value</param>
        public EcellValue(double l_value)
        {
            m_value = l_value;
            m_type = typeof(double);
        }


        /// <summary>
        /// Creates a new "EcellValue" instance with a "string" argument.
        /// </summary>
        /// <param name="l_value">The "string" value</param>
        public EcellValue(string l_value)
        {
            m_value = l_value;
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
            m_value = l_list;
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
                m_value = "";
                m_type = typeof(string);
            }
            else
            {
                if (l_value.IsDouble())
                {
                    m_value = l_value.CastToDouble();
                    m_type = typeof(double);
                }
                else if (l_value.IsInt())
                {
                    m_value = l_value.CastToInt();
                    m_type = typeof(int);
                }
                else if (l_value.IsList())
                {
                    m_value = this.CastToEcellValue4WrappedPolymorph(l_value.CastToList());
                    m_type = typeof(List<EcellValue>);
                }
                else
                {
                    m_value = l_value.CastToString();
                    m_type = typeof(string);
                }
            }
        }


        /// <summary>
        /// The property of the type of the stored value.
        /// </summary>
        public Type M_type
        {
            get { return this.m_type; }
            // set { this.m_type = value; }
        }


        /// <summary>
        /// The property of the stored value.
        /// </summary>
        public Object M_value
        {
            get { return this.m_value; }
            // set { this.m_value = value; }
        }

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
                return Convert.ToDouble(this.m_value);
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
                return (int)this.m_value;
            }
            else
            {
                return default(int);
            }
        }


        /// <summary>
        /// Returns the "List<EcellValue>" casting value.
        /// </summary>
        /// <returns>The "List<EcellValue>" value</returns>
        public List<EcellValue> CastToList()
        {
            if (this.IsList())
            {
                return this.m_value as List<EcellValue>;
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
                return this.m_value as string;
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
            if (this.m_value == null)
            {
                return null;
            }
            if (this.IsList())
            {
                return this.ToString4List((List<EcellValue>)this.m_value);
            }
            else
            {
                return this.m_value.ToString();
            }
        }


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
        /// Tests whether the type is a "List<EcellValue>" type.
        /// </summary>
        /// <returns>true if the type is "List<EcellValue>"; false otherwise</returns>
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
    }

    public class EcellReference
    {
        private string m_name;
        private string m_fullID;
        private int m_coeff;
        private int m_accessor;

        public EcellReference()
        {
        }

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

        public string name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        public string fullID
        {
            get { return this.m_fullID; }
            set { this.m_fullID = value; }
        }

        public int coefficient
        {
            get { return this.m_coeff; }
            set { this.m_coeff = value; }
        }

        public int isAccessor
        {
            get { return this.m_accessor; }
            set { this.m_accessor = value; }
        }

        public override string ToString()
        {
            string str = "";
            str = "(\"" + m_name + "\", \"" + m_fullID + "\", " + m_coeff + ", " + m_accessor + ")";
            return str;
        }

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
    }

    public class EcellSystem : EcellObject
    {
        private string m_name;
        private double m_size;
        private string m_stepperID;

        public string name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        public double size
        {
            get { return this.size; }
            set { this.m_size = value; }
        }

        public string stepperID
        {
            get { return this.m_stepperID; }
            set { this.m_stepperID = value; }
        }

        public EcellSystem(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.SetValue(l_data);
        }

        public new void DistributeValue(EcellData d)
        {
            if (d.M_name == "Name") m_name = d.M_value.CastToString();
            else if (d.M_name == "Size") m_size = d.M_value.CastToDouble();
            else if (d.M_name == "StepperID") m_stepperID = d.M_value.CastToString();
        }
    }

    public class EcellVariable : EcellObject
    {
        private int m_isFixed;
        private double m_molarConc;
        private string m_name;
        private double m_numberConc;
        private double m_totalVelocity;
        private double m_valueData;
        private double m_velocity;

        public int isFixed
        {
            get { return this.m_isFixed; }
            set { this.m_isFixed = value; }
        }

        public double molarConc
        {
            get { return this.m_molarConc; }
            set { this.m_molarConc = value; }
        }

        public string name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        public double numberConc
        {
            get { return this.m_numberConc; }
            set { this.m_numberConc = value; }
        }

        public double totalVelocity
        {
            get { return this.m_totalVelocity; }
            set { this.m_totalVelocity = value; }
        }

        public double value
        {
            get { return this.m_valueData; }
            set { this.m_valueData = value; }
        }

        public double velocity
        {
            get { return this.m_velocity; }
            set { this.m_velocity = value; }
        }

        public EcellVariable(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.SetValue(l_data);
        }

        public new void DistributeValue(EcellData d)
        {
            if (d.M_name == "Fixed") m_isFixed = d.M_value.CastToInt();
            else if (d.M_name == "MolarConc") m_molarConc = d.M_value.CastToDouble();
            else if (d.M_name == "Name") m_name = d.M_value.CastToString();
            else if (d.M_name == "NumberConc") m_numberConc = d.M_value.CastToDouble();
            else if (d.M_name == "TotalVelocity") m_totalVelocity = d.M_value.CastToDouble();
            else if (d.M_name == "Value") m_valueData = d.M_value.CastToDouble();
            else if (d.M_name == "Velocity") m_velocity = d.M_value.CastToDouble();
        }
    }

    public class EcellModel : EcellObject
    {
        public EcellModel(string l_modelID, string l_key,
             string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.m_value = l_data;
        }
    }

    public class EcellProcess : EcellObject
    {
        private double m_activity;
        private string m_expression;
        private int m_iscontinuous;
        private string m_name;
        private int m_priority;
        private string m_stepperID;
        private List<EcellReference> m_refList;

        public double activity
        {
            get { return this.m_activity; }
            set { this.m_activity = value; }
        }

        public string expression
        {
            get { return this.m_expression; }
            set { this.m_expression = value; }
        }

        public int isContinuous
        {
            get { return this.m_iscontinuous; }
            set { this.m_iscontinuous = value; }
        }

        public string name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        public int priority
        {
            get { return this.m_priority; }
            set { this.m_priority = value; }
        }

        public string stepperID
        {
            get { return this.m_stepperID; }
            set { this.m_stepperID = value; }
        }

        public List<EcellReference> VariableReferenceList
        {
            get { return this.m_refList; }
            set { this.m_refList = value; }
        }

        public EcellProcess(string l_modelID, string l_key,
            string l_type, string l_class, List<EcellData> l_data)
        {
            this.modelID = l_modelID;
            this.key = l_key;
            this.type = l_type;
            this.classname = l_class;
            this.SetValue(l_data);
        }

        public new void DistributeValue(EcellData d)
        {
            if (d.M_name == "Activity") m_activity = d.M_value.CastToDouble();
            else if (d.M_name == "Expression") m_expression = d.M_value.CastToString();
            else if (d.M_name == "IsContinuous") m_iscontinuous = d.M_value.CastToInt();
            else if (d.M_name == "Name") m_name = d.M_value.CastToString();
            else if (d.M_name == "Priority") m_priority = d.M_value.CastToInt();
            else if (d.M_name == "StepperID") m_stepperID = d.M_value.CastToString();
            else if (d.M_name == "VariableReferenceList")
                m_refList = EcellReference.ConvertString(d.M_value.CastToString());
        }
    }
}
