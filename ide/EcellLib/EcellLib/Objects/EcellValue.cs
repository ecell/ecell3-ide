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

namespace Ecell.Objects
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
        /// <param name="value">The "int" value</param>
        public EcellValue(int value)
        {
            Value = value;
            m_type = typeof(int);
        }


        /// <summary>
        /// Creates a new "EcellValue" instance with a "double" argument.
        /// </summary>
        /// <param name="value">The "double" value</param>
        public EcellValue(double value)
        {
            Value = value;
            m_type = typeof(double);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "string" argument.
        /// </summary>
        /// <param name="value">The "string" value</param>
        public EcellValue(string value)
        {
            Value = value;
            m_type = typeof(string);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "List&lt;EcellValue&gt;" argument.
        /// </summary>
        /// <param name="value">The "List&lt;EcellValue&gt;" value</param>
        public EcellValue(List<EcellValue> value)
        {
            List<EcellValue> list = new List<EcellValue>();
            list.AddRange(value);
            Value = list;
            m_type = typeof(List<EcellValue>);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "List&lt;EcellValue&gt;" argument.
        /// </summary>
        /// <param name="er">The "List&lt;EcellValue&gt;" value</param>
        public EcellValue(EcellReference er)
        {
            List<EcellValue> list = new List<EcellValue>();
            EcellValue value1 = new EcellValue(er.Name);
            EcellValue value2 = new EcellValue(er.FullID);
            EcellValue value3 = new EcellValue(er.Coefficient);
            EcellValue value4 = new EcellValue(er.IsAccessor);
            list.Add(value1);
            list.Add(value2);
            list.Add(value3);
            list.Add(value4);

            Value = list;
            m_type = typeof(List<EcellValue>);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with a "WrappedPolymorph" argument.
        /// </summary>
        /// <param name="value">The "WrappedPolymorph" value</param>
        internal EcellValue(WrappedPolymorph value)
        {
            if (value == null)
            {
                Value = "";
                m_type = typeof(string);
            }
            else
            {
                if (value.IsDouble())
                {
                    Value = value.CastToDouble();
                    m_type = typeof(double);
                }
                else if (value.IsInt())
                {
                    Value = value.CastToInt();
                    m_type = typeof(int);
                }
                else if (value.IsList())
                {
                    Value = this.CastToEcellValue4WrappedPolymorph(value.CastToList());
                    m_type = typeof(List<EcellValue>);
                }
                else
                {
                    Value = value.CastToString();
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

        /// <summary>
        /// Tests whether the type is a "int" type.
        /// </summary>
        /// <returns>true if the type is "int"; false otherwise</returns>
        public bool IsInt
        {
            get
            {
                Type type = typeof(int);
                return CheckValueType(type);
            }
        }

        /// <summary>
        /// Tests whether the type is a "double" type.
        /// </summary>
        /// <returns>true if the type is "double"; false otherwise</returns>
        public bool IsDouble
        {
            get
            {
                Type type = typeof(double);
                return CheckValueType(type);
            }
        }
        /// <summary>
        /// Tests whether the type is a "string" type.
        /// </summary>
        /// <returns>true if the type is "string"; false otherwise</returns>
        public bool IsString
        {
            get
            {
                Type type = typeof(string);
                return CheckValueType(type);
            }
        }
        /// <summary>
        /// Tests whether the type is the list of EcellValue type.
        /// </summary>
        /// <returns>true if the type is the list of EcellValue; false otherwise</returns>
        public bool IsList
        {
            get
            {
                Type type = typeof(List<EcellValue>);
                return CheckValueType(type);
            }
        }

        private bool CheckValueType(Type type)
        {
            if (this.m_type == type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Convert to EcellValue from string.
        /// </summary>
        /// <param name="str">string.</param>
        /// <returns>EcellValue.</returns>
        public static EcellValue ToList(string str)
        {
            List<EcellValue> list = new List<EcellValue>();
            if (str == null || str == "") return new EcellValue(list);

            string text = str.Substring(1);
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
        /// <param name="ecellValue">The "EcellValue" value</param>
        /// <returns>The "ArrayList" value</returns>
        static public ArrayList CastToArrayList4EcellValue(EcellValue ecellValue)
        {
            if (ecellValue == null)
            {
                return null;
            }
            if (ecellValue.IsDouble)
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ecellValue.CastToDouble().ToString());
                return arrayList;
            }
            else if (ecellValue.IsInt)
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ecellValue.CastToInt().ToString());
                return arrayList;
            }
            else if (ecellValue.IsList)
            {
                ArrayList arrayList = new ArrayList();
                foreach (EcellValue childEcellValue in ecellValue.CastToList())
                {
                    ArrayList childList = CastToArrayList4EcellValue(childEcellValue);
                    arrayList.AddRange(childList);
                }
                return arrayList;
            }
            else
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ecellValue.CastToString());
                return arrayList;
            }
        }

        /// <summary>
        /// Returns the "double" casting value.
        /// </summary>
        /// <returns>The "double" value</returns>
        public double CastToDouble()
        {
            if (this.IsDouble)
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
        /// <param name="polymorphList">The list of a "WrappedPolymorph" value</param>
        /// <returns>The list of a "EcellValue"</returns>
        private List<EcellValue> CastToEcellValue4WrappedPolymorph(List<WrappedPolymorph> polymorphList)
        {
            List<EcellValue> ecellValueList = new List<EcellValue>();
            foreach (WrappedPolymorph polymorph in polymorphList)
            {
                if (polymorph.IsDouble())
                {
                    ecellValueList.Add(new EcellValue(polymorph.CastToDouble()));
                }
                else if (polymorph.IsInt())
                {
                    ecellValueList.Add(new EcellValue(polymorph.CastToInt()));
                }
                else if (polymorph.IsList())
                {
                    ecellValueList.Add(new EcellValue(this.CastToEcellValue4WrappedPolymorph(polymorph.CastToList())));
                }
                else
                {
                    ecellValueList.Add(new EcellValue(polymorph.CastToString()));
                }
            }
            return ecellValueList;
        }

        /// <summary>
        /// Returns the "int" casting value.
        /// </summary>
        /// <returns>The "int" value</returns>
        public int CastToInt()
        {
            if (this.IsInt)
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
            if (this.IsList)
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
            if (this.IsString)
            {
                return this.Value.ToString();
            }
            else
            {
                return default(string);
            }
        }

        /// <summary>
        /// Returns the "WrappedPolymorph" casting value.
        /// </summary>
        /// <param name="ecellValue">The "EcellValue" value</param>
        /// <returns>The "WrappedPolymorph" value</returns>
        internal static WrappedPolymorph CastToWrappedPolymorph4EcellValue(EcellValue ecellValue)
        {
            if (ecellValue.IsDouble)
            {
                return new WrappedPolymorph(ecellValue.CastToDouble());
            }
            else if (ecellValue.IsInt)
            {
                return new WrappedPolymorph(ecellValue.CastToInt());
            }
            else if (ecellValue.IsList)
            {
                List<WrappedPolymorph> wrappedPolymorphList = new List<WrappedPolymorph>();
                foreach (EcellValue childEcellValue in ecellValue.CastToList())
                {
                    wrappedPolymorphList.Add(CastToWrappedPolymorph4EcellValue(childEcellValue));
                }
                return new WrappedPolymorph(wrappedPolymorphList);
            }
            else
            {
                return new WrappedPolymorph(ecellValue.CastToString());
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
            if (this.IsList)
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
        /// <param name="str">string.</param>
        /// <returns>EcellValue.</returns>
        public static EcellValue ToVariableReferenceList(string str)
        {
            List<EcellValue> list = new List<EcellValue>();
            if (str == null || str == "") return new EcellValue(list);

            string text = str.Substring(1);
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
        /// <param name="ecellValueList">The list of "EcellValue"</param>
        /// <returns>The "string" value</returns>
        private string ToString4List(List<EcellValue> ecellValueList)
        {
            String value = "";
            foreach (EcellValue ecellValue in ecellValueList)
            {
                if (ecellValue.IsList)
                {
                    value += ", " + this.ToString4List(ecellValue.CastToList());
                }
                else if (ecellValue.IsInt)
                {
                    value += ", " + ecellValue.CastToInt();
                }
                else if (ecellValue.IsDouble)
                {
                    value += ", " + ecellValue.CastToDouble();
                }
                else
                {
                    value += ", " + "\"" + ecellValue.ToString() + "\"";
                }
            }
            if (value.Length >= 2)
            {
                value = "(" + value.Substring(2) + ")";
            }
            else
            {
                value = "(" + value + ")";
            }
            return value;
        }

        /// <summary>
        /// Copy EcellValue.
        /// </summary>
        /// <returns>EcellValue</returns>
        public EcellValue Copy()
        {
            if (IsList)
            {
                List<EcellValue> list = new List<EcellValue>();
                foreach (EcellValue value in this.CastToList())
                    list.Add(value.Copy());
                return new EcellValue(list);
            }
            else if (IsInt)
            {
                return new EcellValue(this.CastToInt());
            }
            else if (IsDouble)
            {
                return new EcellValue(this.CastToDouble());
            }
            else if (IsString)
            {
                return new EcellValue(this.CastToString());
            }
            else return null;
        }
        #endregion
    }
}
