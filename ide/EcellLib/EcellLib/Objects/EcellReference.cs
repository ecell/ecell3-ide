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

namespace Ecell.Objects
{

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
                  new Regex("\"(?<name>.+)\",(.+)\"(?<id>.+)\",(\"|.*)\\-(?<coe>\\d+)(\"|.*),(\"|.*)(?<fix>\\d+)(\"|.*)");
            Match m = reg.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value) * -1;
                this.m_accessor = Convert.ToInt32(m.Groups["fix"].Value);
                return;
            }
            reg =
                new Regex("\"(?<name>.+)\",(.+)\"(?<id>.+)\",(\"|.*)(?<coe>\\d+)(\"|.*),(\"|.*)(?<fix>\\d+)(\"|.*)");
            m = reg.Match(str);
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
            reg = new Regex("\"(?<name>.+)\",(.*)\"(?<id>.+)\", (\"|.*)\\-(?<coe>\\d+)(\"|.*)");
            m = reg.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value) * -1;
                this.m_accessor = 1;
                return;
            }
            reg = new Regex("\"(?<name>.+)\",(.*)\"(?<id>.+)\", (\"|.*)(?<coe>\\d+)(\"|.*)");
            m = reg.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = Convert.ToInt32(m.Groups["coe"].Value);
                this.m_accessor = 1;
                return;
            }

            reg = new Regex("\"(?<name>.+)\",(.*)\"(?<id>.+)\"");
            m = reg.Match(str);
            if (m.Success)
            {
                this.m_name = m.Groups["name"].Value;
                this.m_fullID = m.Groups["id"].Value;
                this.m_coeff = 0;
                this.m_accessor = 1;
            }
        }
        /// <summary>
        /// Constructor with initial parameter.
        /// </summary>
        /// <param name="value">EcellValue</param>
        public EcellReference(IEnumerable value)
        {
            IEnumerator i = value.GetEnumerator();
            i.MoveNext();
            this.m_name = (string)i.Current;
            i.MoveNext();
            this.m_fullID = (string)i.Current;
            this.m_coeff = 0;
            this.m_accessor = 1;
            if (i.MoveNext())
            {
                this.m_coeff = (int)i.Current;
                if (i.MoveNext())
                    this.m_accessor = (int)i.Current;
            }
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set name.
        /// </summary>
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        /// <summary>
        /// get / set full ID.
        /// </summary>
        public string FullID
        {
            get { return this.m_fullID; }
            set { this.m_fullID = value; }
        }

        /// <summary>
        /// get / set full ID.
        /// </summary>
        public string Key
        {
            get {
                string path = this.m_fullID;
                string[] ele = path.Split(new char[] { ':' });
                string result = ele[ele.Length - 2] + Constants.delimiterColon + ele[ele.Length - 1];
                    
                return result;
            }
            set { this.m_fullID = Constants.delimiterColon + value; }

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
            List<EcellReference> list = new List<EcellReference>();
            foreach (IEnumerable value in (IEnumerable)varRef.Value)
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
        public static EcellValue ConvertToVarRefList(IEnumerable<EcellReference> refList)
        {
            List<EcellValue> list = new List<EcellValue>();
            if (refList == null)
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
}
