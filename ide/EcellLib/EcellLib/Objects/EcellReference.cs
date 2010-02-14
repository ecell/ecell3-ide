//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.Text.RegularExpressions;
using System.Collections;
using Ecell.Exceptions;

namespace Ecell.Objects
{
    /// <summary>
    /// Object class for Reference.
    /// </summary>
    public class EcellReference : ICloneable
    {
        #region Constant
        private const string parse1 = "\"(?<name>.+)\",(.+)\"(?<id>.+)\",(\"|.*)\\-(?<coe>\\d+)(\"|.*),(\"|.*)(?<fix>\\d+)(\"|.*)";
        private const string parse2 = "\"(?<name>.+)\",(.+)\"(?<id>.+)\",(\"|.*)(?<coe>\\d+)(\"|.*),(\"|.*)(?<fix>\\d+)(\"|.*)";
        private const string parse3 = "\"(?<name>.+)\",(.*)\"(?<id>.+)\", (\"|.*)\\-(?<coe>\\d+)(\"|.*)";
        private const string parse4 = "\"(?<name>.+)\",(.*)\"(?<id>.+)\", (\"|.*)(?<coe>\\d+)(\"|.*)";
        private const string parse5 = "\"(?<name>.+)\",(.*)\"(?<id>.+)\"";
        private const string stringParse = "\\((?<refer>.+?)\\)";
        #endregion

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
            this.m_name = null;
            this.m_fullID = null;
            this.m_coeff = 0;
            this.m_accessor = 0;
        }
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="name">The name of EcellReference</param>
        /// <param name="fullID">FullID of connecting Variable</param>
        /// <param name="coef">Coefficient of this VariableReference. It takes 1, 0 or -1.</param>
        /// <param name="accessor">IsAccessor</param>
        public EcellReference(string name, string fullID, int coef, int accessor)
        {
            this.m_name = name;
            this.m_fullID = fullID;
            this.m_coeff = coef;
            this.m_accessor = accessor;
        }

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="str">string.</param>
        public EcellReference(string str)
        {
            // Check null.
            if (string.IsNullOrEmpty(str))
                throw new EcellException("EcellRefference Constructor does not arrow empty string");

            Regex parser = new Regex(parse1);
            Match m = parser.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value) * -1;
                this.m_accessor = Convert.ToInt32(m.Groups["fix"].Value);
                return;
            }
            parser = new Regex(parse2);
            m = parser.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value);
                this.m_accessor = Convert.ToInt32(m.Groups["fix"].Value);
                return;
            }

            // ロードしたモデルによってはAccessorが書かれていないものがあるため、
            // Accessorが書かれていないものにも対応できるようにした。
            parser = new Regex(parse3);
            m = parser.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value) * -1;
                this.m_accessor = 1;
                return;
            }

            parser = new Regex(parse4);
            m = parser.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value);
                this.m_accessor = 1;
                return;
            }

            parser = new Regex(parse5);
            m = parser.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = 0;
                this.m_accessor = 1;
                return;
            }

            throw new EcellException("EcellRefference parsing error:[" + str + "]");
        }

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="list">IEnumerator</param>
        public EcellReference(IEnumerator list)
        {
            if (list == null)
                throw new EcellException("EcellRefference Constructor does not arrow empty list.");
            list.MoveNext();
            this.m_name = Convert.ToString(list.Current);
            list.MoveNext();
            this.m_fullID = Convert.ToString(list.Current);
            this.m_coeff = 0;
            this.m_accessor = 1;
            if (list.MoveNext())
            {
                this.m_coeff = Convert.ToInt32(list.Current);
                if (list.MoveNext())
                    this.m_accessor = Convert.ToInt32(list.Current);
            }
        }
        #endregion

        #region Accessors
        /// <summary>
        ///The name of this EcellReference.
        /// get / set.
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// The full ID of connecting variable.
        /// get / set.
        /// </summary>
        public string FullID
        {
            get { return this.m_fullID; }
            set { this.m_fullID = value; }
        }

        /// <summary>
        /// The key of connecting variable.
        /// get / set.
        /// </summary>
        public string Key
        {
            get {
                if (string.IsNullOrEmpty(m_fullID))
                    return null;

                string[] ele = m_fullID.Split(':');
                if (ele.Length < 2)
                    throw new EcellException(MessageResources.ErrInvalidID);
                string result = ele[ele.Length - 2] + Constants.delimiterColon + ele[ele.Length - 1];
                    
                return result;
            }
            set { this.m_fullID = Constants.xpathVariable + Constants.delimiterColon + value; }

        }

        /// <summary>
        /// Coefficient of this VariableReference. It takes 1, 0 or -1.
        /// get / set.
        /// </summary>
        public int Coefficient
        {
            get { return this.m_coeff; }
            set { this.m_coeff = value; }
        }

        /// <summary>
        /// Whether this properties is accessor or not.
        /// get / set .
        /// </summary>
        public int IsAccessor
        {
            get { return this.m_accessor; }
            set { this.m_accessor = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the list of reference from string.
        /// </summary>
        /// <param name="str">string.</param>
        /// <returns>the list of reference.</returns>
        public static List<EcellReference> ConvertFromString(string str)
        {
            List<EcellReference> list = new List<EcellReference>();
            if (str == null || str == "")
                return list;
            string text = str.Substring(1);
            text = text.Substring(0, text.Length - 1);
            Regex parser = new Regex(stringParse);
            MatchCollection coll = parser.Matches(text);

            IEnumerator iter = coll.GetEnumerator();
            while (iter.MoveNext())
            {
                Match match = (Match)iter.Current;
                EcellReference er = new EcellReference(match.Groups["refer"].Value);
                list.Add(er);
            }
            return list;
        }

        /// <summary>
        /// Get the list of reference from VariableReferenceList.
        /// </summary>
        /// <param name="varRef">VariableReferenceList.</param>
        /// <returns>the list of EcellReference.</returns>
        public static List<EcellReference> ConvertFromEcellValue(EcellValue varRef)
        {
            List<EcellReference> list = new List<EcellReference>();
            if (varRef == null || !varRef.IsList)
                return list;
            foreach (object value in (IEnumerable)varRef.Value)
            {
                EcellReference er = new EcellReference(((IEnumerable)value).GetEnumerator());
                list.Add(er);
            }
            return list;
        }

        /// <summary>
        /// Get the list of reference from VariableReferenceList.
        /// </summary>
        /// <param name="refList">VariableReferenceList.</param>
        /// <returns>the list of EcellReference.</returns>
        public static EcellValue ConvertToEcellValue(IEnumerable<EcellReference> refList)
        {
            List<object> list = new List<object>();
            if (refList == null)
                return new EcellValue(list);

            foreach (EcellReference er in refList)
            {
                list.Add(new object[] { er.Name, er.FullID, er.Coefficient, er.IsAccessor });
            }
            return new EcellValue(list);
        }
        #endregion

        #region ICloneable メンバ
        /// <summary>
        /// Create a copy of this EcellReference.
        /// </summary>
        /// <returns>object</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Create a copy of this EcellReference.
        /// </summary>
        /// <returns>EcellValue</returns>
        public EcellReference Clone()
        {
            EcellReference er = new EcellReference(
                this.Name,
                this.FullID,
                this.Coefficient,
                this.IsAccessor);

            return er;
        }
        #endregion

        #region Inherited Method
        /// <summary>
        /// Compare to another EcellReference.
        /// </summary>
        /// <param name="obj">the compared object</param>
        /// <returns>Return true, object is equal.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is EcellReference))
                return false;

            EcellReference er = (EcellReference)obj;
            if (!er.Name.Equals(m_name))
                return false;
            if (!er.FullID.Equals(m_fullID))
                return false;
            if (!er.Coefficient.Equals(m_coeff))
                return false;
            if (!er.IsAccessor.Equals(m_accessor))
                return false;

            return true;
        }

        /// <summary>
        /// Get Hash code.
        /// </summary>
        /// <returns>hash code.</returns>
        public override int GetHashCode()
        {
            return m_name.GetHashCode() ^ m_fullID.GetHashCode() ^ m_coeff.GetHashCode() ^ m_accessor.GetHashCode();
        }

        /// <summary>
        /// Get string of object.
        /// </summary>
        /// <returns>the object string.</returns>
        public override string ToString()
        {
            string str = "(\"" + m_name + "\", \"" + m_fullID + "\", " + m_coeff + ", " + m_accessor + ")";
            return str;
        }
        #endregion
    }
}
