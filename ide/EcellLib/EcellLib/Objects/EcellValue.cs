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
using System.Collections.Generic;
using System.Text;
using EcellCoreLib;
using System.Collections;
using System.Text.RegularExpressions;

namespace EcellLib.Objects
{

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
            EcellValue value1 = new EcellValue(l_ref.Name);
            EcellValue value2 = new EcellValue(l_ref.FullID);
            EcellValue value3 = new EcellValue(l_ref.Coefficient);
            EcellValue value4 = new EcellValue(l_ref.IsAccessor);
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
            set
            {
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
        static public ArrayList CastToArrayList4EcellValue(EcellValue l_ecellValue)
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
        static public WrappedPolymorph CastToWrappedPolymorph4EcellValue(EcellValue l_ecellValue)
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
}
