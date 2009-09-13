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
using Ecell.Exceptions;

namespace Ecell.Objects
{
    /// <summary>
    /// Value type.
    /// </summary>
    public enum EcellValueType
    {
        /// <summary>
        /// Integer type.
        /// </summary>
        Integer = 1,
        /// <summary>
        /// Double type.
        /// </summary>
        Double = 2,
        /// <summary>
        /// String type.
        /// </summary>
        String = 3,
        /// <summary>
        /// List type.
        /// </summary>
        List = 4
    }

    /// <summary>
    /// The polymorphism 4 the "EcellData".
    /// </summary>
    public class EcellValue: ICloneable
    {
        #region Fields
        /// <summary>
        /// The stored value.
        /// </summary>
        private Object m_value = null;
        #endregion

        #region Constractors
        /// <summary>
        /// Creates a new "EcellValue" instance with EcellValue.
        /// </summary>
        /// <param name="that">the original value object.</param>
        public EcellValue(EcellValue that)
            : this(that.m_value)
        {
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with an object.
        /// </summary>
        /// <param name="o">the original value object.</param>
        public EcellValue(object o)
        {
            m_value = Normalize(o);
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with an EcellReference.
        /// </summary>
        /// <param name="er">The reference object.</param>
        public EcellValue(EcellReference er)
        {
            List<object> list = new List<object>();
            list.Add(er.Name);
            list.Add(er.FullID);
            list.Add(er.Coefficient);
            list.Add(er.IsAccessor);
            this.m_value = Normalize(list);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// The property of the stored value.
        /// </summary>
        public object Value
        {
            get { return this.m_value; }
        }
        /// <summary>
        /// The type of the stored value.
        /// </summary>
        public EcellValueType Type
        {
            get
            {
                if (m_value is int)
                    return EcellValueType.Integer;
                else if (m_value is double)
                    return EcellValueType.Double;
                else if (m_value is string)
                    return EcellValueType.String;
                else if (m_value is IEnumerable<object>)
                    return EcellValueType.List;
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Tests whether the type is a "int" type.
        /// </summary>
        /// <returns>true if the type is "int"; false otherwise</returns>
        public bool IsInt
        {
            get { return m_value is int; }
        }

        /// <summary>
        /// Tests whether the type is a "double" type.
        /// </summary>
        /// <returns>true if the type is "double"; false otherwise</returns>
        public bool IsDouble
        {
            get { return m_value is double; }
        }
        /// <summary>
        /// Tests whether the type is a "string" type.
        /// </summary>
        /// <returns>true if the type is "string"; false otherwise</returns>
        public bool IsString
        {
            get { return m_value is string; }
        }
        /// <summary>
        /// Tests whether the type is the list of EcellValue type.
        /// </summary>
        /// <returns>true if the type is the list of EcellValue; false otherwise</returns>
        public bool IsList
        {
            get { return m_value is List<object>; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Convert to EcellValue from string.
        /// </summary>
        /// <param name="str">string.</param>
        /// <returns>EcellValue.</returns>
        public static EcellValue ConvertFromListString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new EcellValue(new List<object>());

            List<EcellReference> refList = EcellReference.ConvertFromString(str);
            return EcellReference.ConvertToEcellValue(refList);
        }

        /// <summary>
        /// Normalize the target object.
        /// </summary>
        /// <param name="o">the original object.</param>
        /// <returns>the normalized object.</returns>
        private static object Normalize(object o)
        {
            if ((o is int) || (o is double) || (o is string))
            {
                return o;
            }
            else if (o is EcellValue)
            {
                return ((EcellValue)o).Value;
            }
            else if (o is IEnumerable)
            {
                List<object> val = new List<object>();
                foreach (object i in (IEnumerable)o)
                {
                    val.Add(Normalize(i));
                }
                return val;
            }
            throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "object"));
        }

        /// <summary>
        /// To int
        /// </summary>
        /// <param name="val">the value object.</param>
        /// <returns>int data of value object.</returns>
        public static implicit operator int(EcellValue val)
        {
            if (val.m_value is double)
                return (int)(double)val.m_value;
            else if (val.m_value is int)
                return (int)val.m_value;
            else if (val.m_value is string)
            {
                try
                {
                    return Convert.ToInt32((string)val.m_value);
                }
                catch (Exception e)
                {
                    throw new InvalidCastException("Specified value does not represent an integer value", e);
                }
            }
            throw new InvalidCastException("Specified value is not a numeric type");
        }

        /// <summary>
        /// To double.
        /// </summary>
        /// <param name="val">the value object.</param>
        /// <returns>double data of value object.</returns>
        public static implicit operator double(EcellValue val)
        {
            if (val.m_value is double)
                return (double)val.m_value;
            else if (val.m_value is int)
                return (double)(int)val.m_value;
            else if (val.m_value is string)
            {
                try
                {
                    return Convert.ToDouble((string)val.m_value);
                }
                catch (Exception e)
                {
                    throw new InvalidCastException("Specified value does not represent a double value", e);
                }
            }
            throw new InvalidCastException("Specified value is not a numeric type");
        }
        /// <summary>
        /// To string.
        /// </summary>
        /// <param name="val">the value object.</param>
        /// <returns>string data of value object.</returns>
        public static implicit operator string(EcellValue val)
        {
            if (val.Value is string)
            {
                return (string)val.Value;
            }
            else
            {
                return val.ToString();
            }
        }
        #endregion

        #region ICloneable メンバ
        /// <summary>
        /// Create a copy of this EcellValue.
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Copy EcellValue.
        /// </summary>
        /// <returns>EcellValue</returns>
        public EcellValue Clone()
        {
            return new EcellValue(this);
        }

        #endregion

        #region Inherited Method
        /// <summary>
        /// Get Hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_value.ToString().GetHashCode();
        }

        /// <summary>
        /// Compare to another EcellValue.
        /// </summary>
        /// <param name="obj">the compared object.</param>
        /// <returns>Return true when object is equal.</returns>
        public override bool Equals(object obj)
        {
            bool isEquals = false;
            if (obj == null)
                isEquals = false;
            if (obj is EcellValue)
                isEquals = Equals(((EcellValue)obj).m_value);
            else if (obj is int && this.IsInt)
                isEquals = (int)this == (int)obj;
            else if (obj is double && this.IsDouble)
                isEquals = (double)this == (double)obj;
            else if (obj is string && this.IsString)
                isEquals = (string)this == (string)obj;
            else if (obj is IEnumerable && this.IsList)
            {
                isEquals = CompareList((List<object>)this.m_value, (List<object>)obj);
            }
            return isEquals;
        }

        /// <summary>
        /// Compare the list.
        /// </summary>
        /// <param name="list1">the compared list.</param>
        /// <param name="list2">the compared list.</param>
        /// <returns>Return true, when the list is equal.</returns>
        private static bool CompareList(List<object> list1, List<object> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            for(int i = 0; i < list1.Count; i++)
            {
                object obj1 = list1[i];
                object obj2 = list2[i];
                if (obj1 is List<object> && obj2 is List<object>)
                {
                    if (!CompareList((List<object>)obj1, (List<object>)obj2))
                        return false;
                }
                else if (!object.Equals(obj1, obj2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Casts the value to "string".
        /// </summary>
        /// <returns>The "string" value</returns>
        public override string ToString()
        {
            if (m_value is string)
                return (string)m_value;
            else
                return ToSerializedForm(m_value);
        }

        /// <summary>
        /// Convert to the serialized form.
        /// </summary>
        /// <returns></returns>
        private static string ToSerializedForm(object value)
        {
            string str = "";
            if (value is IEnumerable<object>)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('(');
                foreach (object obj in (IEnumerable<object>)value)
                {
                    if (obj is string)
                        sb.Append("\"" + obj.ToString() + "\"");
                    else if (obj is IEnumerable<object>)
                        sb.Append(ToSerializedForm(obj));
                    else
                        sb.Append(obj.ToString());
                    sb.Append(", ");
                }
                if (sb.Length > 1)
                {
                    sb.Length -= 2;
                }
                sb.Append(')');
                str = sb.ToString();
            }
            else
            {
                str = value.ToString();
            }
            return str;
        }

        #endregion

    }
}
