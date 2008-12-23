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
using System.Text.RegularExpressions;
using System.Collections;
using Ecell.Exceptions;

namespace Ecell.Objects
{

    /// <summary>
    /// Object class for Reference.
    /// </summary>
    public class EcellReference
    {
        #region Constant
        private static Regex parser1 = new Regex("\"(?<name>.+)\",(.+)\"(?<id>.+)\",(\"|.*)\\-(?<coe>\\d+)(\"|.*),(\"|.*)(?<fix>\\d+)(\"|.*)");
        private static Regex parser2 = new Regex("\"(?<name>.+)\",(.+)\"(?<id>.+)\",(\"|.*)(?<coe>\\d+)(\"|.*),(\"|.*)(?<fix>\\d+)(\"|.*)");
        private static Regex parser3 = new Regex("\"(?<name>.+)\",(.*)\"(?<id>.+)\", (\"|.*)\\-(?<coe>\\d+)(\"|.*)");
        private static Regex parser4 = new Regex("\"(?<name>.+)\",(.*)\"(?<id>.+)\", (\"|.*)(?<coe>\\d+)(\"|.*)");
        private static Regex parser5 = new Regex("\"(?<name>.+)\",(.*)\"(?<id>.+)\"");
        private static Regex stringParser = new Regex("\\((?<refer>.+?)\\)");
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
        }
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="name">The name of EcellReference</param>
        /// <param name="fullID">FullID of connecting variable</param>
        /// <param name="coef"></param>
        /// <param name="accessor"></param>
        public EcellReference(string name, string fullID, int coef, int accessor)
        {
            this.m_name = name;
            this.m_fullID = fullID;
            this.Coefficient = coef;
            this.m_accessor = accessor;
        }

        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="str">string.</param>
        public EcellReference(string str)
        {
            Match m = parser1.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value) * -1;
                this.m_accessor = Convert.ToInt32(m.Groups["fix"].Value);
                return;
            }
            m = parser2.Match(str);
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
            m = parser3.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value) * -1;
                this.m_accessor = 1;
                return;
            }

            m = parser4.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value);
                this.m_accessor = 1;
                return;
            }

            m = parser5.Match(str);
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
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// The full ID of connecting variable.
        /// </summary>
        public string FullID
        {
            get { return this.m_fullID; }
            set { this.m_fullID = value; }
        }

        /// <summary>
        /// The key of connecting variable.
        /// </summary>
        public string Key
        {
            get {
                string path = this.m_fullID;
                string[] ele = path.Split(new char[] { ':' });
                string result = ele[ele.Length - 2] + Constants.delimiterColon + ele[ele.Length - 1];
                    
                return result;
            }
            set { this.m_fullID = Constants.xpathVariable + Constants.delimiterColon + value; }

        }

        /// <summary>
        /// get / set coefficient.
        /// </summary>
        public int Coefficient
        {
            get { return this.m_coeff; }
            set { this.m_coeff = value; }
        }

        /// <summary>
        /// get / set whether this properties is accessor.
        /// </summary>
        public int IsAccessor
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
            string str = "(\"" + m_name + "\", \"" + m_fullID + "\", " + m_coeff + ", " + m_accessor + ")";
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
        public static List<EcellReference> ConvertFromString(string str)
        {
            List<EcellReference> list = new List<EcellReference>();
            if (str == null || str == "")
                return list;
            string text = str.Substring(1);
            text = text.Substring(0, text.Length - 1);
            MatchCollection coll = stringParser.Matches(text);

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
    }
}
