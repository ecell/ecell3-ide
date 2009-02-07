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
    public enum EcellValueType
    {
        Integer = 1,
        Double = 2,
        String = 3,
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
        /// Creates a new "EcellValue" instance with no argument.
        /// </summary>
        private EcellValue()
        {
            ; // do nothing
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with EcellValue.
        /// </summary>
        /// <param name="that"></param>
        public EcellValue(EcellValue that)
            : this(that.m_value)
        {
        }

        /// <summary>
        /// Creates a new "EcellValue" instance with an object.
        /// </summary>
        /// <param name="o"></param>
        public EcellValue(object o)
        {
            m_value = Normalize(o);
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
                else if (m_value is IEnumerable<EcellValue>)
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
                return new EcellValue(new List<EcellValue>());

            List<EcellReference> refList = EcellReference.ConvertFromString(str);
            return EcellReference.ConvertToEcellValue(refList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
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
            throw new ArgumentException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string ToSerializedForm()
        {
            string value = "";
            if (m_value is string)
            {
                return "\"" + ((string)m_value).Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
            }
            else if (m_value is List<object>)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('(');
                foreach (object obj in (IEnumerable<object>)m_value)
                {
                    sb.Append(obj.ToString());
                    sb.Append(", ");
                }
                if (sb.Length > 1)
                {
                    sb.Length -= 2;
                }
                sb.Append(')');
                value = sb.ToString();
            }
            else
            {
                value = m_value.ToString();
            }
            return value;
        }
        /// <summary>
        /// To int
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
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
        /// <param name="val"></param>
        /// <returns></returns>
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
        /// <param name="val"></param>
        /// <returns></returns>
        public static implicit operator string(EcellValue val)
        {
            if (val.Value is string)
            {
                return (string)val.Value;
            }
            else
            {
                return val.Value.ToString();
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
            if (m_value == null)
                return base.GetHashCode();
            else
                return m_value.ToString().GetHashCode();
        }

        /// <summary>
        /// Compare to another EcellValue.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is EcellValue)
                return Equals(((EcellValue)obj).m_value);
            else if (obj is int)
                return (int)this == (int)obj;
            else if (obj is double)
                return (double)this == (double)obj;
            else if (obj is string)
                return (string)this == (string)obj;
            else if (obj is IEnumerable)
            {
                return (IEnumerable)this.m_value == (IEnumerable<object>)obj;
            }
            throw new InvalidOperationException();
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
                return ToSerializedForm();
        }
        #endregion

    }
}
