// Formulator C# Library
// COPYRIGHT (C) 2006  MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.UI.Components
{
    /// <summary>
    /// Node class corredponds with the component in formula.
    /// </summary>
    internal class FNode
    {
        #region Fields
        /// <summary>
        /// Data type of FNode. This information refer to FUtil.
        /// </summary>
        int m_type;
        /// <summary>
        /// Data of FNode. Only string is stored in this.
        /// </summary>
        string m_data;
        /// <summary>
        /// X position of FNode in the formulator.
        /// </summary>
        int m_x;
        /// <summary>
        /// Y position of FNode in the formulator.
        /// </summary>
        int m_y;
        /// <summary>
        /// Width of FNode in the formulator.
        /// </summary>
        float m_width;
        /// <summary>
        /// Spliter object. Parent object.
        /// If FNode is included in denominator, this is not null.
        /// </summary>
        public FNode m_parentD = null;
        /// <summary>
        /// Spliter object. Parent object.
        /// If FNode is included in numerator, this is not null.
        /// </summary>
        public FNode m_parentN = null;
        /// <summary>
        /// List of numerator.
        /// If FNode is FUtil.SPLIT, this count is not zero.
        /// </summary>
        public List<FNode> m_Numerator = new List<FNode>();
        /// <summary>
        /// List of demoninator.
        /// If FNode is FUtil.SPLIT, this count is not zero.
        /// </summary>
        public List<FNode> m_Denominator = new List<FNode>();
        /// <summary>
        /// This is next object of FNode.
        /// </summary>
        public FNode m_next = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with default parameters.
        /// Default type is FUtil.NONE;
        /// </summary>
        public FNode()
        {
            m_type = FUtil.NONE;
            m_data = "";
            SetDefaultWidth();
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="type">data type of FNode.</param>
        /// <param name="data">data of FNode.</param>
        public FNode(int type, string data)
        {
            m_type = type;
            m_data = data;
            SetDefaultWidth();
        }
        #endregion

        #region Getter/Setter
        /// <summary>
        /// Getter / Setter of m_next.
        /// If this FNode is in Numerator or Demoninator, 
        /// this function set m_parentN or m_parentD at a time.
        /// </summary>
        public FNode Next
        {
            get { return this.m_next; }
            set
            {
                this.m_next = value;
                if (m_parentD != null)
                {
                    int i = 0;
                    foreach (FNode n in m_parentD.m_Denominator)
                    {
                        if (n.X == this.X && n.Y == this.Y)
                        {
                            value.X = n.X + 1;
                            value.Y = n.Y;
                            break;
                        }
                        i++;
                    }
                    m_parentD.m_Denominator.Insert(i + 1, value);
                    value.m_parentD = m_parentD;
                }
                else if (m_parentN != null)
                {
                    int i = 0;
                    foreach (FNode n in m_parentN.m_Numerator)
                    {
                        if (n.X == this.X && n.Y == this.Y)
                        {
                            value.X = n.X + 1;
                            value.Y = n.Y;
                            break;
                        }
                        i++;
                    }
                    m_parentN.m_Numerator.Insert(i + 1, value);
                    value.m_parentN = m_parentN;
                }
            }
        }

        /// <summary>
        /// Getter / Setter of x position.
        /// </summary>
        public int X
        {
            get { return this.m_x; }
            set { this.m_x = value; }
        }

        /// <summary>
        /// Getter / Setter of y position.
        /// </summary>
        public int Y
        {
            get { return this.m_y; }
            set { this.m_y = value; }
        }

        /// <summary>
        /// Getter / Setter of data type.
        /// </summary>
        public int Type
        {
            get { return this.m_type; }
            set { this.m_type = value; SetDefaultWidth();  }
        }

        /// <summary>
        /// Getter / Setter of data.
        /// </summary>
        public string Data
        {
            get { return this.m_data; }
            set { this.m_data = value; }
        }

        /// <summary>
        /// Getter / Setter of width.
        /// </summary>
        public float Width
        {
            get { return this.m_width; }
            set { this.m_width = value; }
        }
        #endregion

        #region Function
        /// <summary>
        /// Check data type of FNode, and if data type of FNode is not FUtil.STRING,
        /// width is set default.
        /// </summary>
        private void SetDefaultWidth()
        {
            if (m_type == FUtil.NONE)
                m_width = 0;
            else if (m_type != FUtil.STRING)
                m_width = FUtil.SPLIT_WIDTH;
        }

        /// <summary>
        /// Get the depth of Numerator at FNode.
        /// If FNode is not FUtil.SPLIT, return 0.
        /// </summary>
        /// <returns>depth.</returns>
        public int GetNumeratorNum()
        {
            int result = 0;

            foreach (FNode n in m_Numerator)
            {
                int tmp = n.GetNum();
                if (tmp > result) result = tmp;
            }

            return result;
        }

        /// <summary>
        /// Get the depth of Demoninator at FNode.
        /// If FNode is not FUtil.SPLIT, return 0.
        /// </summary>
        /// <returns>depth.</returns>
        public int GetDenominatorNum()
        {
            int result = 0;

            foreach (FNode n in m_Denominator)
            {
                int tmp = n.GetNum();
                if (tmp > result) result = tmp;
            }

            return result;
        }

        /// <summary>
        /// Get the depth of Demoninator and Numerator at FNode.
        /// If FNode is not FUtil.SPLIT, return 1.
        /// </summary>
        /// <returns>depth.</returns>
        public int GetNum()
        {
            int numMax, denoMax;

            numMax = 0;
            foreach (FNode n in m_Numerator)
            {
                int tmp = n.GetNum();
                if (tmp > numMax) numMax = tmp;
            }

            denoMax = 0;
            foreach (FNode n in m_Denominator)
            {
                int tmp = n.GetNum();
                if (tmp > denoMax) denoMax = tmp;
            }

            return numMax + denoMax + 1;
        }

        /// <summary>
        /// Map the tree node data to the array data.
        /// </summary>
        /// <param name="x">x base position.</param>
        /// <param name="y">y base position.</param>
        /// <param name="array">the array data.</param>
        public void Mapping(int x, int y, FNode[,] array)
        {
            if (this.Type == FUtil.SPLIT)
            {
                int num = 0;
                foreach (FNode n in m_Numerator)
                {
                    int tmp = n.GetDenominatorNum();
                    if (tmp > num) num = tmp;
                }

                int baseX = x;
                int baseY = y - num - 1;
                foreach (FNode n in m_Numerator)
                {
                    n.Mapping(baseX, baseY, array);
                    int tmp = n.GetLength();
                    baseX = baseX + tmp;
                }


                num = 0;
                foreach (FNode n in m_Denominator)
                {
                    int tmp = n.GetNumeratorNum();
                    if (tmp > num) num = tmp;
                }
                baseX = x;
                baseY = y + num + 1;
                foreach (FNode n in m_Denominator)
                {
                    n.Mapping(baseX, baseY, array);
                    int tmp = n.GetLength();
                    baseX = baseX + tmp;
                }

                num = this.GetLength();
                for (int i = 0; i < num; i++)
                {
                    array[x + i, y].Type = FUtil.SPLIT;
                    array[x + i, y].Width = FUtil.SPLIT_WIDTH;
                }
                this.X = x;
                this.Y = y;
            }
            else
            {
                array[x, y].Type = this.Type;
                array[x, y].Data = this.Data;
                if (this.Type == FUtil.STRING || 
                    this.IsFunction1() ||
                    this.IsFunction2())
                    array[x, y].Width = this.Width;
                this.X = x;
                this.Y = y;
            }
        }

        /// <summary>
        /// Get the amount of count of FNode in numerator.
        /// </summary>
        /// <returns>count of FNode.</returns>
        public int GetNumeratorLength()
        {
            int result = 0;

            foreach (FNode n in m_Numerator)
            {
                result = result + n.GetLength();
            }

            return result;
        }

        /// <summary>
        /// Get the amount of count of FNode in demoninator.
        /// </summary>
        /// <returns>count of FNode.</returns>
        public int GetDemoninatorLength()
        {
            int result = 0;

            foreach (FNode n in m_Denominator)
            {
                result = result + n.GetLength();
            }

            return result;
        }

        /// <summary>
        /// Get the amount of count of FNode in same y position.
        /// </summary>
        /// <returns>count of FNode.</returns>
        public int GetCurrentLength()
        {
            int nextLength = 0;
            if (m_next != null)
            {
                nextLength = m_next.GetCurrentLength();
            }
            int length = this.GetLength();

            return length + nextLength;
        }

        /// <summary>
        /// Get the amount of count of FNode.
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            if (m_Numerator.Count == 0) return 1;

            int numNum = 0;
            foreach (FNode n in m_Numerator)
            {
                numNum = numNum + n.GetLength();
            }

            int demoNum = 0;
            foreach (FNode n in m_Denominator)
            {
                demoNum = demoNum + n.GetLength();
            }

            if (numNum > demoNum) return numNum;
            else return demoNum;
        }

        /// <summary>
        /// Get FNode with x and y.
        /// </summary>
        /// <param name="x">x position.</param>
        /// <param name="y">y position.</param>
        /// <returns>FNode.</returns>
        public FNode GetNode(int x, int y)
        {
            if (x == m_x && y == m_y) return this;
            int length = GetLength();

            if (length + X <= x)
            {
                if (m_next != null) return m_next.GetNode(x, y);
                else return null;
            }

            if (y == m_y)
            {
                return this;
            }
            if (y < m_y)
            {
                foreach (FNode n in m_Numerator)
                {
                    FNode tmp = n.GetNode(x, y);
                    if (tmp != null) return tmp;
                }
            }
            else
            {
                foreach (FNode n in m_Denominator)
                {
                    FNode tmp = n.GetNode(x, y);
                    if (tmp != null) return tmp;
                }
            }
            return null;
        }

        /// <summary>
        /// Check whether FNode is operator or not.
        /// FUtil.PLUS, FUtil.MINUS, FUtil.MULTI.
        /// </summary>
        /// <returns>If FNode is opeator, return true.</returns>
        public bool IsOperator()
        {
            if (m_type == FUtil.PLUS) return true;
            else if (m_type == FUtil.MINUS) return true;
            else if (m_type == FUtil.MULTI) return true;

            return false;
        }

        /// <summary>
        /// Check function that argument is one parameter.
        /// </summary>
        /// <returns>boolean</returns>
        public bool IsFunction1()
        {
            if (m_type == FUtil.LOG || m_type == FUtil.LOG10 ||
                m_type == FUtil.SQRT || m_type == FUtil.EXP ||
                m_type == FUtil.CEIL || m_type == FUtil.FLOOR ||
                m_type == FUtil.ABS || m_type == FUtil.SIN ||
                m_type == FUtil.COS || m_type == FUtil.TAN ||
                m_type == FUtil.ASIN || m_type == FUtil.ACOS ||
                m_type == FUtil.ATAN || m_type == FUtil.IFNOT)
                return true;
            return false;
        }

        /// <summary>
        /// Check function that argument is two parameter.
        /// </summary>
        /// <returns>boolean</returns>
        public bool IsFunction2()
        {
            if (m_type == FUtil.POW || m_type == FUtil.IFAND ||
                m_type == FUtil.IFOR || m_type == FUtil.IFXOR ||
                m_type == FUtil.IFEQ || m_type == FUtil.IFNEQ ||
                m_type == FUtil.IFGT || m_type == FUtil.IFLT ||
                m_type == FUtil.IFGEQ || m_type == FUtil.IFLEQ)
                return true;
            return false;
        }

        /// <summary>
        /// Get string of FNode while system export the formulator to file.
        /// </summary>
        /// <returns>string of FNode.</returns>
        public String Output()
        {
            if (m_type == FUtil.PLUS) return "+ ";
            else if (m_type == FUtil.MINUS) return "- ";
            else if (m_type == FUtil.LEFT) return "( ";
            else if (m_type == FUtil.MULTI) return "* ";
            else if (m_type == FUtil.RIGHT) return ") ";
            else if (this.IsFunction1()) return m_data + " ";
            else if (this.IsFunction2()) return m_data + " ";
            else if (m_type == FUtil.KAMMA) return ", ";
            else if (m_type == FUtil.STRING) return m_data + " ";
            else if (m_type == FUtil.SPLIT)
            {
                string result = "";
                bool isParent = false;
                for (int i = 0; i < m_Numerator.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m_Numerator[i].Type != FUtil.LEFT)
                        {
                            result = result + "( ";
                            isParent = true;
                        }
                    }

                    result = result + m_Numerator[i].Output();
                }

                if (isParent) result = result + ") / ";
                else result = result + "/ ";

                isParent = false;
                for (int i = 0; i < m_Denominator.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m_Denominator[i].Type != FUtil.LEFT)
                        {
                            result = result + "( ";
                            isParent = true;
                        }
                    }
                    result = result + m_Denominator[i].Output();
                }
                if (isParent) result = result + ") ";

                return result;
            }
            return "";
        }

        /// <summary>
        /// Remove the specific node from denominator.
        /// </summary>
        /// <param name="list">the removed node.</param>
        public void DRemoveList(List<FNode> list)
        {
            FNode prev = null;
            foreach (FNode n in m_Denominator)
            {
                bool isHit = false;
                foreach (FNode m in list)
                {
                    if (n.X == m.X && n.Y == m.Y)
                    {
                        if (prev != null) prev.m_next = n.Next;
                        isHit = true;
                        break;
                    }
                }
                if (isHit == false)
                    prev = n;
            }

            foreach (FNode n in list)
                m_Denominator.Remove(n);
        }

        /// <summary>
        /// Remove the specific node from numerator.
        /// </summary>
        /// <param name="list">the removed node.</param>
        public void NRemoveList(List<FNode> list)
        {
            FNode prev = null;
            foreach (FNode n in m_Numerator)
            {
                bool isHit = false;
                foreach (FNode m in list)
                {
                    if (n.X == m.X && n.Y == m.Y)
                    {
                        if (prev != null) prev.m_next = n.Next;
                        isHit = true;
                        break;
                    }
                }
                if (isHit == false)
                    prev = n;
            }

            foreach (FNode n in list)
                m_Numerator.Remove(n);
        }

        #endregion
    }
}
