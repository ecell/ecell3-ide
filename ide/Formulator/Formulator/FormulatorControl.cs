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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ecell.UI.Components
{
    public partial class FormulatorControl : UserControl
    {
        #region Fields
        /// <summary>
        /// FNode array includes the postion information in formulator.
        /// </summary>
        private FNode[,] m_array;
        /// <summary>
        /// List of x position at image.
        /// </summary>
        private int[] m_posList;
        /// <summary>
        /// Top FNode of displayed formulator.
        /// </summary>
        private FNode m_top;
        /// <summary>
        /// Selected FNode.
        /// </summary>
        private FNode m_current;

        /// <summary>
        /// Width of image.
        /// </summary>
        private int m_sizeX = 3000;
        /// <summary>
        /// Height of image.
        /// </summary>
        private int m_sizeY = 500;

        /// <summary>
        /// Selected FNode is whether update mode or not.
        /// </summary>
        private bool m_isExist = false;
        /// <summary>
        /// Selected FNode is whether text or not.
        /// </summary>
        private bool m_isText = true;
        /// <summary>
        /// The flag whether this data is expression in process.
        /// </summary>
        private bool m_isExpression = true;

        /// <summary>
        /// Pen for writing input FNode.
        /// </summary>
        private Pen m_iPen;
        /// <summary>
        /// Pen for wiritng selected FNode.
        /// </summary>
        private Pen m_sPen;
        /// <summary>
        /// Image of displaying the formulator.
        /// </summary>
        private Bitmap m_image;
        /// <summary>
        /// Font for writing string in image.
        /// </summary>
        private Font m_font = new Font("Arial", 9F, FontStyle.Regular);
        /// <summary>
        /// Rectangle of selected FNode.
        /// </summary>
        private Rectangle m_rect = new Rectangle(0, 0, 0, 0);
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for FormulatorControl.
        /// </summary>
        public FormulatorControl()
        {
            InitializeComponent();

            m_array = new FNode[FUtil.XMAX, FUtil.YMAX];
            m_posList = new int[FUtil.XMAX];
            m_image = new Bitmap(m_sizeX, m_sizeY);
            pictureBox1.Image = m_image;
            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            Graphics g = Graphics.FromImage(m_image);
            g.Clear(Color.WhiteSmoke);
            g.Dispose();

            m_iPen = new Pen(Brushes.Black);
            m_iPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            m_sPen = new Pen(Brushes.Red);
            m_sPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;


            stringBox.ReadOnly = true;

            for (int i = 0; i < FUtil.XMAX; i++)
            {
                for (int j = 0; j < FUtil.YMAX; j++)
                {
                    m_array[i, j] = new FNode();
                }
            }
            m_top = new FNode(FUtil.INPUT, "");

            FunctionBox.Items.Add(FUtil.STRLOG);
            FunctionBox.Items.Add(FUtil.STRLOG10);
            FunctionBox.Items.Add(FUtil.STRSQRT);
            FunctionBox.Items.Add(FUtil.STREXP);
            FunctionBox.Items.Add(FUtil.STRCEIL);
            FunctionBox.Items.Add(FUtil.STRFLOOR);
            FunctionBox.Items.Add(FUtil.STRABS);
            FunctionBox.Items.Add(FUtil.STRSIN);
            FunctionBox.Items.Add(FUtil.STRCOS);
            FunctionBox.Items.Add(FUtil.STRTAN);
            FunctionBox.Items.Add(FUtil.STRASIN);
            FunctionBox.Items.Add(FUtil.STRACOS);
            FunctionBox.Items.Add(FUtil.STRATAN);

            FunctionBox.Items.Add(FUtil.STRPOW);
            FunctionBox.Items.Add(FUtil.STRNOT);
            FunctionBox.Items.Add(FUtil.STRAND);
            FunctionBox.Items.Add(FUtil.STROR);
            FunctionBox.Items.Add(FUtil.STRXOR);
            FunctionBox.Items.Add(FUtil.STREQ);
            FunctionBox.Items.Add(FUtil.STRNEQ);
            FunctionBox.Items.Add(FUtil.STRGT);
            FunctionBox.Items.Add(FUtil.STRLT);
            FunctionBox.Items.Add(FUtil.STRGEQ);
            FunctionBox.Items.Add(FUtil.STRLEQ);


            UpdateFormulator();
        }
        #endregion

        #region Getter/Setter
        /// <summary>
        /// Get image of the formulator.
        /// </summary>
        public Bitmap Image
        {
            get { return this.m_image; }
        }

        /// <summary>
        /// Get/Set the flag whether this data is expression in process.
        /// </summary>
        public bool IsExpression
        {
            get { return this.m_isExpression; }
            set { 
                this.m_isExpression = value;
                this.FunctionBox.Enabled = value;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event of mouse down in Editer.
        /// </summary>
        /// <param name="sender">PictureBox.</param>
        /// <param name="e">Events data.</param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int pX = e.X;
            int pY = e.Y;

            if (pX < FUtil.MARGIN) return;
            if (pY < FUtil.MARGIN) return;

            int x = 0;
            int y = 0;
            for (int i = 0; i < FUtil.XMAX; i++)
            {
                if (pX < m_posList[i])
                {
                    x = i;
                    break;
                }
            }

            for (int i = 0; i < FUtil.YMAX; i++)
            {
                if (pY < (i + 1) * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN)
                {
                    y = i;
                    break;
                }
            }

            Select(x, y);
        }

        /// <summary>
        /// Event of clicking the Button of Plus and add "+" to the formulator.
        /// </summary>
        /// <param name="sender">plus button.</param>
        /// <param name="e">event parameters.</param>
        private void PlusButton_Click(object sender, EventArgs e)
        {
            if (m_isText == true) return;
            if (m_current == null) return;
            if (!m_current.IsOperator() && m_current.Type != FUtil.OINPUT) return;

            m_current.Type = FUtil.PLUS;
            m_current.Width = FUtil.PLUS_WIDTH;

            if (m_current.Next == null)
                m_current.Next = new FNode(FUtil.INPUT, "");
            else if (m_isExist == false)
            {
                FNode tmp = new FNode(FUtil.INPUT, "");
                tmp.Next = m_current.Next;
                m_current.Next = tmp;
            }


            UpdateFormulator();
        }

        /// <summary>
        /// Event of clicking the Button of Minus and add "-" to the formulator.
        /// </summary>
        /// <param name="sender">minus button.</param>
        /// <param name="e">event parameters.</param>
        private void MinusButton_Click(object sender, EventArgs e)
        {
            if (m_isText == true) return;
            if (m_current == null) return;
            if (!m_current.IsOperator() && m_current.Type != FUtil.OINPUT) return;

            m_current.Type = FUtil.MINUS;
            m_current.Width = FUtil.MINUS_WIDTH;

            if (m_current.Next == null)
                m_current.Next = new FNode(FUtil.INPUT, "");
            else if (m_isExist == false)
            {
                FNode tmp = new FNode(FUtil.INPUT, "");
                tmp.Next = m_current.Next;
                m_current.Next = tmp;
            }

            UpdateFormulator();
        }

        /// <summary>
        /// Event of clicking the Button of Multiply and add "*" to the formulator.
        /// </summary>
        /// <param name="sender">multiply button.</param>
        /// <param name="e">event parameters.</param>
        private void MultiplyButton_Click(object sender, EventArgs e)
        {
            if (m_isText == true) return;
            if (m_current == null) return;
            if (!m_current.IsOperator() && m_current.Type != FUtil.OINPUT) return;

            m_current.Type = FUtil.MULTI;
            m_current.Width = FUtil.MULTI_WIDTH;

            if (m_current.Next == null)
                m_current.Next = new FNode(FUtil.INPUT, "");
            else if (m_isExist == false)
            {
                FNode tmp = new FNode(FUtil.INPUT, "");
                tmp.Next = m_current.Next;
                m_current.Next = tmp;
            }

            UpdateFormulator();
        }

        /// <summary>
        /// Event of clicking the Button of String and add "XX" to the formulator.
        /// </summary>
        /// <param name="sender">string button.</param>
        /// <param name="e">event parameters.</param>
        private void StringButton_Click(object sender, EventArgs e)
        {
            if (m_isText == false) return;
            if (m_current == null) return;
            if (stringBox.Text.Equals("")) return;
            if (!FUtil.CheckReserveStrin(stringBox.Text)) return;

            Graphics g = Graphics.FromImage(m_image);
            m_current.Type = FUtil.STRING;
            m_current.Data = stringBox.Text;
            m_current.Width = (float)g.MeasureString(m_current.Data, m_font).Width;
            g.Dispose();

            if (m_current.Next == null)
                m_current.Next = new FNode(FUtil.OINPUT, "");
            else if (m_isExist == false)
            {
                FNode tmp = new FNode(FUtil.OINPUT, "");
                tmp.Next = m_current.Next;
                m_current.Next = tmp;
            }

            UpdateFormulator();
        }

        /// <summary>
        /// Event of clicking the Button of Parent and add "( )" to the formulator.
        /// </summary>
        /// <param name="sender">parent button.</param>
        /// <param name="e">event parameters.</param>
        private void ParentButton_Click(object sender, EventArgs e)
        {
            if (m_isText == false) return;
            if (m_current == null) return;

            FNode tmpNode = m_current.Next;

            m_current.Type = FUtil.LEFT;
            m_current.Width = FUtil.LEFT_WIDTH;

            FNode inputNode = new FNode(FUtil.INPUT, "");
            m_current.Next = inputNode;

            FNode rightNode = new FNode(FUtil.RIGHT, "");
            inputNode.Next = rightNode;

            if (tmpNode == null || tmpNode.Type == FUtil.RIGHT)
            {
                FNode opeatorNode = new FNode(FUtil.OINPUT, "");
                rightNode.Next = opeatorNode;
                opeatorNode.m_next = tmpNode;
            }
            else
            {
                rightNode.m_next = tmpNode;
            }

            UpdateFormulator();
        }

        /// <summary>
        /// Event of clicking the Button of Split and add "/" to the formulator.
        /// </summary>
        /// <param name="sender">split button.</param>
        /// <param name="e">event parameters.</param>
        private void SplitButton_Click(object sender, EventArgs e)
        {
            if (m_isText == false) return;
            if (m_current == null) return;

            m_current.Type = FUtil.SPLIT;
            m_current.Width = FUtil.SPLIT_WIDTH;

            FNode n1 = new FNode(FUtil.LEFT, "");
            FNode n2 = new FNode(FUtil.INPUT, "");
            FNode n3 = new FNode(FUtil.RIGHT, "");
            n1.Next = n2;
            n2.Next = n3;
            m_current.m_Numerator.Add(n1);
            m_current.m_Numerator.Add(n2);
            m_current.m_Numerator.Add(n3);
            n1.m_parentN = m_current;
            n2.m_parentN = m_current;
            n3.m_parentN = m_current;

            FNode d1 = new FNode(FUtil.LEFT, "");
            FNode d2 = new FNode(FUtil.INPUT, "");
            FNode d3 = new FNode(FUtil.RIGHT, "");
            d1.Next = d2;
            d2.Next = d3;
            m_current.m_Denominator.Add(d1);
            m_current.m_Denominator.Add(d2);
            m_current.m_Denominator.Add(d3);

            d1.m_parentD = m_current;
            d2.m_parentD = m_current;
            d3.m_parentD = m_current;

            if (m_current.Next == null)
            {
                FNode c1 = new FNode(FUtil.OINPUT, "");
                m_current.Next = c1;
            }
            else
            {
                FNode c1 = new FNode(FUtil.OINPUT, "");
                c1.Next = m_current.Next;
                m_current.Next = c1;
            }

            UpdateFormulator();
        }

        /// <summary>
        /// Event of downing key at TextBox. 
        /// If key is Enter, system update string in textbox to the formulator.
        /// </summary>
        /// <param name="sender">text box.</param>
        /// <param name="e">event parameters.</param>
        private void stringBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StringButton_Click(StringButton, new EventArgs());
            }
        }

        /// <summary>
        /// Event of clicking the Button of Reserved String and add "XX" to the formulator.
        /// </summary>
        /// <param name="sender">reserved string button.</param>
        /// <param name="e">event parameters.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (m_isText == false) return;
            if (m_current == null) return;
            f (String.IsNullOrEmpty(reserveBox.Text)) return;

            Graphics g = Graphics.FromImage(m_image);
            m_current.Type = FUtil.STRING;
            m_current.Data = reserveBox.Text;
            m_current.Width = (float)g.MeasureString(m_current.Data, m_font).Width;
            g.Dispose();

            if (m_current.Next == null)
                m_current.Next = new FNode(FUtil.OINPUT, "");
            else if (m_isExist == false)
            {
                FNode tmp = new FNode(FUtil.OINPUT, "");
                tmp.Next = m_current.Next;
                m_current.Next = tmp;
            }

            UpdateFormulator();
        }

        /// <summary>
        /// Event of clicking the Button of delete the selected component.
        /// </summary>
        /// <param name="sender">delete button.</param>
        /// <param name="e">EventArgs.</param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (m_current == null) return;

            int x = m_current.X;
            int y = m_current.Y;

            if (m_current.Type == FUtil.STRING || m_current.Type == FUtil.SPLIT)
            {
                if (m_current.Type == FUtil.SPLIT)
                {
                    m_current.m_Numerator.Clear();
                    m_current.m_Denominator.Clear();
                }

                if (m_current.m_parentD != null || m_current.m_parentN != null)
                {
                    if (m_current.Next.IsOperator() || m_current.Next.Type == FUtil.OINPUT)
                    {
                        List<FNode> list = new List<FNode>();
                        if (m_current.Next.Type != FUtil.OINPUT) list.Add(m_current);
                        else m_current.Type = FUtil.INPUT;
                        list.Add(m_current.Next);

                        if (m_current.m_parentN != null) m_current.m_parentN.NRemoveList(list);
                        else if (m_current.m_parentD != null) m_current.m_parentD.DRemoveList(list);
                    }
                    else m_current.Type = FUtil.INPUT;
                }
                else
                {
                    FNode prev = null;
                    if (x > 0) prev = m_top.GetNode(x - 1, y);

                    if (m_current.Next != null)
                    {
                        if (m_current.Next.Type == FUtil.OINPUT)
                        {
                            if (m_current.Next.Next == null ||
                                m_current.Next.Next.Type == FUtil.NONE)
                            {
                                m_current.Type = FUtil.INPUT;
                                m_current.m_next = null;
                            }
                            else
                            {
                                m_current.Type = FUtil.INPUT;
                                m_current.m_next = m_current.Next.Next;
                            }
                        }
                        else
                        {
                            if (prev == null) m_top = m_current.Next.Next;
                            else prev.m_next = m_current.Next.Next;
                        }
                    }
                    else m_current.Type = FUtil.INPUT;
                }
            }
            else if (m_current.IsOperator())
            {
                if (m_current.m_parentD != null || m_current.m_parentN != null)
                {
                    if (m_current.Next.Type == FUtil.STRING || m_current.Next.Type == FUtil.INPUT)
                    {
                        List<FNode> list = new List<FNode>();
                        if (m_current.Next.Type != FUtil.INPUT) list.Add(m_current);
                        else m_current.Type = FUtil.OINPUT;
                        list.Add(m_current.Next);

                        if (m_current.m_parentN != null) m_current.m_parentN.NRemoveList(list);
                        else if (m_current.m_parentD != null) m_current.m_parentD.DRemoveList(list);
                    }
                    else m_current.Type = FUtil.OINPUT;
                }
                else
                {
                    FNode prev = null;
                    if (x > 0) prev = m_top.GetNode(x - 1, y);

                    if (m_current.Next != null)
                    {
                        if (m_current.Next.Type == FUtil.INPUT)
                        {
                            m_current.Type = FUtil.OINPUT;
                            m_current.m_next = m_current.Next.Next;
                        }
                        else
                        {
                            if (prev == null) m_top = m_current.Next.Next;
                            else prev.m_next = m_current.Next.Next;
                        }
                    }
                    else m_current.Type = FUtil.OINPUT;
                }
            }
            else if (m_current.IsFunction1() || m_current.IsFunction2())
            {
                if (m_current.m_parentD != null || m_current.m_parentN != null)
                {
                    List<FNode> list = new List<FNode>();
                    list.Add(m_current);
                    int depth = 0;
                    FNode n = m_current.Next;
                    while (n != null)
                    {
                        if (n.Type == FUtil.LEFT) depth++;
                        else if (n.Type == FUtil.RIGHT) depth--;

                        list.Add(n);
                        if (depth == 0) break;
                        n = n.Next;
                    }
                    if (n.Next.Type == FUtil.OINPUT)
                        n.Next.Type = FUtil.INPUT;
                    else
                        list.Add(n.Next);

                    if (m_current.m_parentN != null) m_current.m_parentN.NRemoveList(list);
                    else if (m_current.m_parentD != null) m_current.m_parentD.DRemoveList(list);
                }
                else
                {
                    FNode prev = null;
                    if (x > 0) prev = m_top.GetNode(x - 1, y);
                    int depth = 0;
                    FNode n = m_current.Next;
                    while (n != null)
                    {
                        if (n.Type == FUtil.LEFT) depth++;
                        else if (n.Type == FUtil.RIGHT) depth--;

                        if (depth == 0) break;
                        n = n.Next;
                    }

                    if (n.Next == null)
                    {
                        if (prev != null) prev.m_next = new FNode(FUtil.INPUT, "");
                        else m_top = new FNode(FUtil.INPUT, "");
                    }
                    else
                    {
                        if (n.Next.IsOperator())
                        {
                            if (prev != null) prev.m_next = n.Next.Next;
                            else m_top = n.Next.Next;
                        }
                        else if (n.Next.Type == FUtil.OINPUT)
                        {
                            n.Next.Type = FUtil.INPUT;
                            if (prev != null)
                                prev.m_next = n.Next;
                            else m_top = n.Next;
                        }
                    }
                }
            }

            UpdateFormulator();
        }

        /// <summary>
        /// Event of clicking the add button to insert the selected function.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void AddFunctionButton_Click(object sender, EventArgs e)
        {
            if (m_isText == false) return;
            if (m_current == null) return;
            if (FunctionBox.Text == null || FunctionBox.Text.Equals("")) return;

            FNode n = m_current.Next;

            FNode f = CreateStrFNode(FunctionBox.Text);
            FNode l = new FNode(FUtil.LEFT, "");
            FNode r = new FNode(FUtil.RIGHT, "");

            if (f.IsFunction1())
            {
                FNode i = new FNode(FUtil.INPUT, "");

                m_current.Type = f.Type;
                m_current.Width = f.Width;
                m_current.Data = f.Data;

                m_current.Next = l;
                l.Next = i;
                i.Next = r;
            }
            else
            {
                FNode i1 = new FNode(FUtil.INPUT, "");
                FNode i2 = new FNode(FUtil.INPUT, "");
                FNode k = new FNode(FUtil.KAMMA, "");

                m_current.Type = f.Type;
                m_current.Width = f.Width;
                m_current.Data = f.Data;

                m_current.Next = l;
                l.Next = i1;
                i1.Next = k;
                k.Next = i2;
                i2.Next = r;
            }

            FNode tmp = new FNode(FUtil.OINPUT, "");
            if (n == null)
                r.Next = tmp;
            else if (m_isExist == false)
            {
                tmp.Next = n;
                r.Next = tmp;
            }

            UpdateFormulator();
        }

        #endregion

        #region Function
        /// <summary>
        /// Create FNode of string data.
        /// Calculate the width of FNode using Font.
        /// </summary>
        /// <param name="str">string data.</param>
        /// <returns>FNode of string data.</returns>
        private FNode CreateStrFNode(string str)
        {
            Graphics g = Graphics.FromImage(m_image);
            FNode n = null;
            if (str.Equals(FUtil.STRLOG)) n = new FNode(FUtil.LOG, str);
            else if (str.Equals(FUtil.STRLOG10)) n = new FNode(FUtil.LOG10, str);
            else if (str.Equals(FUtil.STRSQRT)) n = new FNode(FUtil.SQRT, str);
            else if (str.Equals(FUtil.STREXP)) n = new FNode(FUtil.EXP, str);
            else if (str.Equals(FUtil.STRCEIL)) n = new FNode(FUtil.CEIL, str);
            else if (str.Equals(FUtil.STRFLOOR)) n = new FNode(FUtil.FLOOR, str);
            else if (str.Equals(FUtil.STRABS)) n = new FNode(FUtil.ABS, str);
            else if (str.Equals(FUtil.STRSIN)) n = new FNode(FUtil.SIN, str);
            else if (str.Equals(FUtil.STRCOS)) n = new FNode(FUtil.COS, str);
            else if (str.Equals(FUtil.STRTAN)) n = new FNode(FUtil.TAN, str);
            else if (str.Equals(FUtil.STRASIN)) n = new FNode(FUtil.ASIN, str);
            else if (str.Equals(FUtil.STRACOS)) n = new FNode(FUtil.ACOS, str);
            else if (str.Equals(FUtil.STRATAN)) n = new FNode(FUtil.ATAN, str);
            else if (str.Equals(FUtil.STRPOW)) n = new FNode(FUtil.POW, str);
            else if (str.Equals(FUtil.STRAND)) n = new FNode(FUtil.IFAND, str);
            else if (str.Equals(FUtil.STROR)) n = new FNode(FUtil.IFOR, str);
            else if (str.Equals(FUtil.STRNOT)) n = new FNode(FUtil.IFNOT, str);
            else if (str.Equals(FUtil.STRXOR)) n = new FNode(FUtil.IFXOR, str);
            else if (str.Equals(FUtil.STREQ)) n = new FNode(FUtil.IFEQ, str);
            else if (str.Equals(FUtil.STRNEQ)) n = new FNode(FUtil.IFNEQ, str);
            else if (str.Equals(FUtil.STRGT)) n = new FNode(FUtil.IFGT, str);
            else if (str.Equals(FUtil.STRLT)) n = new FNode(FUtil.IFLT, str);
            else if (str.Equals(FUtil.STRGEQ)) n = new FNode(FUtil.IFGEQ, str);
            else if (str.Equals(FUtil.STRLEQ)) n = new FNode(FUtil.IFLEQ, str);

            else n = new FNode(FUtil.STRING, str);

            float w = 10.0F;
            if (str != null && str != "") w = g.MeasureString(str, m_font).Width;
            n.Width = w;
            g.Dispose();

            return n;
        }

        /// <summary>
        /// Get string of the formulator using tree node data.
        /// </summary>
        /// <returns>string of the formulator.</returns>
        public string ExportFormulate()
        {
            string result = "";

            if (m_top != null)
            {
                FNode m = m_top;

                while (m != null)
                {
                    string tmp = m.Output();
                    result = result + tmp;
                    m = m.Next;
                }
            }

            return result;
        }

        /// <summary>
        /// Create tree node of FNode from string.
        /// </summary>
        /// <param name="text">the formulator string.</param>
        public void ImportFormulate(string text)
        {
            FNode n = new FNode();
            ConvertToFNode(text, 0, n);
            if (n != null)
                m_top = n.Next;
            UpdateFormulator();
        }

        /// <summary>
        /// Create tree node of FNode from string.
        /// </summary>
        /// <param name="text">formulator string.</param>
        /// <param name="s">start position of string.</param>
        /// <param name="result">top FNode.</param>
        /// <returns>position of string.</returns>
        private int ConvertToFNode(string text, int s, FNode result)
        {
            int i;
            string cStr = "";
            FNode p1 = null;
            FNode p2 = null;
            FNode c = null;
            bool isParent = false;
            bool isDemoni = false;
            bool isFunction = false;
            bool inOpe = false;

            for (i = s; i < text.Length; i++)
            {
                c = null;
                if (text[i] == ' ')
                {
                    if (cStr.Length >= 1)
                    {
                        c = CreateStrFNode(cStr);
                        if (c.IsFunction1() || c.IsFunction2())
                            inOpe = false;
                        else 
                            inOpe = true;
                        cStr = "";
                    }
                }
                else if (text[i] == ',')
                {
                    if (cStr.Length >= 1)
                    {
                        FNode ic = CreateStrFNode(cStr);
                        p1.Next = ic;
                        p2 = p1;
                        p1 = ic;

                        inOpe = false;
                        cStr = "";
                    }
                    if (p1.Type == FUtil.INPUT || p1.Type == FUtil.OINPUT)
                    {
                    }
                    else if (p1.IsOperator() || p1.Type == FUtil.LEFT)
                    {
                        FNode ip = new FNode(FUtil.INPUT, "");
                        p1.Next = ip;
                        p2 = p1;
                        p1 = ip;
                    }
                    else
                    {
                        FNode ip = new FNode(FUtil.OINPUT, "");
                        p1.Next = ip;
                        p2 = p1;
                        p1 = ip;
                    }
                    c = new FNode(FUtil.KAMMA, ",");
                }
                else if (text[i] == '(')
                {
                    if (cStr.Length >= 1)
                    {
                        FNode p = CreateStrFNode(cStr);
                        if (p.IsFunction1() || p.IsFunction2())
                        {
                            if (p1 == null) result.Next = p;
                            else p1.Next = p;
                            p1 = p;
                            cStr = "";
                            c = new FNode(FUtil.LEFT, "");
                            i = ConvertToFNode(text, i + 1, c);
                            if (isDemoni == false) isParent = true;

                            inOpe = true;
                        }
                        else
                        {
                            cStr = cStr + "(";
                            p = null;
                            isFunction = true;
                        }
                    }
                    else
                    {
                        c = new FNode(FUtil.LEFT, "");
                        i = ConvertToFNode(text, i + 1, c);
                        if (isDemoni == false) isParent = true;
                        inOpe = true;
                    }
                }
                else if (text[i] == ')')
                {
                    if (isFunction == true)
                    {
                        cStr = cStr + ")";
                        isFunction = false;
                    }
                    else
                    {
                        FNode rightNode = new FNode(FUtil.RIGHT, "");
                        if (cStr.Length >= 1)
                        {
                            FNode strNode = CreateStrFNode(cStr);
                            FNode inputNode = new FNode(FUtil.OINPUT, "");

                            strNode.Next = inputNode;
                            inputNode.Next = rightNode;

                            if (p1 == null) result.Next = strNode;
                            else p1.Next = strNode;
                        }
                        else
                        {
                            FNode inputNode;
                            if (p1 == null || p1.IsOperator()) inputNode = new FNode(FUtil.INPUT, "");
                            else inputNode = new FNode(FUtil.OINPUT, "");

                            inputNode.Next = rightNode;
                            if (p1 == null) result.Next = inputNode;
                            else
                            {
                                while (p1.Next != null)
                                {
                                    p1 = p1.m_next;
                                }
                                p1.Next = inputNode;
                            }
                        }
                        return i;
                    }
                }
                else if (inOpe == true &&
                    (text[i] == '+' || text[i] == '-' || text[i] == '*'))
                {
                    if (isParent == true)
                    {
                        FNode m = p1;
                        while (m != null)
                        {
                            p2 = p1;
                            p1 = m;
                            m = m.m_next;
                        }

                        isParent = false;
                    }
                    if (text[i] == '+') c = new FNode(FUtil.PLUS, "");
                    else if (text[i] == '-') c = new FNode(FUtil.MINUS, "");
                    else if (text[i] == '*') c = new FNode(FUtil.MULTI, "");

                    if (cStr.Length >= 1)
                    {
                        FNode tmp = CreateStrFNode(cStr);
                        cStr = "";

                        if (isDemoni == true)
                        {
                            FNode leftNode = new FNode(FUtil.LEFT, "");
                            FNode inputNode = new FNode(FUtil.OINPUT, "");
                            FNode rightNode = new FNode(FUtil.RIGHT, "");

                            p1.m_Denominator.Add(leftNode);
                            p1.m_Denominator.Add(tmp);
                            p1.m_Denominator.Add(inputNode);
                            p1.m_Denominator.Add(rightNode);
                            tmp.m_parentD = p1;
                            leftNode.m_parentD = p1;
                            inputNode.m_parentD = p1;
                            rightNode.m_parentD = p1;
                            isDemoni = false;
                        }
                        else
                        {
                            if (p1 == null) result.Next = tmp;
                            else p1.Next = tmp;
                            p2 = p1;
                            p1 = tmp;
                        }
                    }
                    inOpe = false;
                }
                else if (text[i] == '/' && IsExpression)
                {
                    FNode tmp = new FNode(FUtil.SPLIT, "");

                    if (p2 != null) p2.Next = tmp;
                    else result.Next = tmp;

                    FNode m = p1;
                    while (m != null)
                    {
                        tmp.m_Numerator.Add(m);
                        m.m_parentN = tmp;
                        m = m.Next;
                    }
                    p1 = tmp;
                    isDemoni = true;
                    isParent = false;
                    continue;
                }
                else
                {
                    if (text[i] != '\n' && text[i] != '\r')
                    {
                        string a = text.Substring(i, 1);
                        cStr = cStr + a;
                        inOpe = true;
                    }
                }

                if (c != null)
                {
                    if (isDemoni == true)
                    {
                        if (!c.IsOperator())
                        {
                            FNode m = c;
                            while (m != null)
                            {
                                p1.m_Denominator.Add(m);
                                m.m_parentD = p1;
                                m = m.Next;
                            }
                        }
                        isDemoni = false;
                    }
                    else
                    {
                        if (p1 == null) result.Next = c;
                        else p1.Next = c;
                        p2 = p1;
                        p1 = c;
                    }
                }
            }

            if (cStr.Length >= 1)
            {
                FNode tmp = CreateStrFNode(cStr);
                cStr = "";

                if (isDemoni == true)
                {
                    if (!tmp.IsOperator())
                    {
                        FNode m = tmp;
                        while (m != null)
                        {
                            p1.m_Denominator.Add(m);
                            m.m_parentD = p1;
                            m = m.Next;
                        }
                    }
                    isDemoni = false;
                }
                else
                {
                    if (p1 == null) result.Next = tmp;
                    else p1.Next = tmp;
                    p2 = p1;
                    p1 = tmp;
                }
            }
            FNode endNode;
            if (p1 == null || p1.IsOperator()) endNode = new FNode(FUtil.INPUT, "");
            else endNode = new FNode(FUtil.OINPUT, "");

            if (p1 == null) result.Next = endNode;
            else
            {
                while (p1.m_next != null)
                {
                    p1 = p1.m_next;
                }
                p1.Next = endNode;
            }

            return i;
        }

        /// <summary>
        /// Change the selected FNode and edit mode.
        /// </summary>
        /// <param name="x">x position of selected FNode.</param>
        /// <param name="y">y position of selected FNode.</param>
        public void Select(int x, int y)
        {
            UpdateSelectPos(x, y);
            if (m_array[x, y].Type == FUtil.NONE)
            {
                m_current = null;
                return;
            }

            if (m_array[x, y].Type == FUtil.INPUT)
            {
                stringBox.Text = "";
                EditMode(true);
            }
            else if (m_array[x, y].Type == FUtil.STRING)
            {
                stringBox.Text = m_array[x, y].Data;
                EditMode(true);
            }
            else if (m_array[x, y].IsFunction1() ||
                m_array[x,y].IsFunction2())
            {
                FunctionBox.Text = m_array[x, y].Data;
                FunctionMode();
            }
            else if (m_array[x, y].Type == FUtil.SPLIT)
            {
                AllDisableMode();
                DeleteButton.Enabled = true;
            }
            else if (m_array[x, y].Type == FUtil.LEFT ||
                m_array[x, y].Type == FUtil.RIGHT)
            {
                AllDisableMode();
            }
            else
            {
                EditMode(false);
            }

            m_current = m_top.GetNode(x, y);
            m_isExist = false;
            if (m_current != null && m_current.Next != null)
            {
                if (m_current.Next.Type == FUtil.RIGHT ||
                    m_current.Next.Type == FUtil.KAMMA) m_isExist = false;
                else m_isExist = true;
            }
        }

        private void FunctionMode()
        {
            stringBox.ReadOnly = false;
            stringBox.Focus();

            PlusButton.Enabled = false;
            MinusButton.Enabled = false;
            MultiplyButton.Enabled = false;
            SplitButton.Enabled = false;
            ParentButton.Enabled = false;
            StringButton.Enabled = false;
            if (m_isExpression)
                AddFunctionButton.Enabled = true;
            else
                AddFunctionButton.Enabled = false;
            DeleteButton.Enabled = true;

            reserveBox.Enabled = false;
            AddButton.Enabled = false;            
        }

        /// <summary>
        /// Clear the selected FNode.
        /// </summary>
        public void Unselect()
        {
            m_current = null;

            stringBox.ReadOnly = true;
            PlusButton.Enabled = false;
            MinusButton.Enabled = false;
            MultiplyButton.Enabled = false;
            SplitButton.Enabled = false;
            ParentButton.Enabled = false;
            StringButton.Enabled = false;
            AddFunctionButton.Enabled = false;
        }

        /// <summary>
        /// Write the rectangle of selected FNode.
        /// </summary>
        /// <param name="x">x position of the selected FNode.</param>
        /// <param name="y">y position of the selected FNode.</param>
        private void UpdateSelectPos(int x, int y)
        {
            Graphics g = Graphics.FromImage(m_image);
            if (m_rect.X != 0 && m_rect.Y != 0)
                g.DrawRectangle(Pens.WhiteSmoke, m_rect);

            if (m_array[x, y].Type == FUtil.INPUT ||
                m_array[x, y].Type == FUtil.OINPUT ||
                m_array[x, y].Type == FUtil.STRING ||
                m_array[x, y].IsOperator())
            {
                int sx = (int)FUtil.MARGIN;
                int sy = (int)(y * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                int ex = (int)(FUtil.MARGIN + m_array[x, y].Width);
                int ey = (int)((y + 1) * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN - FUtil.HEIGHT_GAP);
                if (x != 0)
                {
                    sx = m_posList[x - 1];
                    ex = (int)(m_posList[x - 1] + m_array[x, y].Width);
                }
                m_rect.X = sx + 1;
                m_rect.Y = sy + 1;
                m_rect.Width = ex - sx - 2;
                m_rect.Height = ey - sy - 2;
                g.DrawRectangle(m_sPen, m_rect);
            }
            else if (m_array[x, y].Type == FUtil.SPLIT)
            {
                FNode n = m_top.GetNode(x, y);
                x = n.X;

                int sx = (int)FUtil.MARGIN;
                if (x != 0)
                    sx = m_posList[x - 1];
                int len = n.GetLength();
                int ex = m_posList[x + len - 1];

                int nnum = n.GetNumeratorNum();
                int dnum = n.GetDenominatorNum();

                int sy = (int)((y - nnum) * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                int ey = (int)((y + dnum + 1) * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);

                m_rect.X = sx + 1;
                m_rect.Y = sy - 1;
                m_rect.Width = ex - sx - 2;
                m_rect.Height = ey - sy - 2;
                g.DrawRectangle(m_sPen, m_rect);
            }
            else if (m_array[x, y].IsFunction1() ||
                m_array[x,y].IsFunction2())
            {
                FNode n = m_top.GetNode(x, y);
                x = n.X;

                int sx = (int)FUtil.MARGIN;
                if (x != 0)
                    sx = m_posList[x - 1];
                int depth = 0;
                int nnum = n.GetNumeratorNum();
                int dnum = n.GetDenominatorNum();
                FNode m = n.Next;
                while (m != null)
                {
                    if (m.Type == FUtil.LEFT) depth++;
                    else if (m.Type == FUtil.RIGHT) depth--;

                    if (depth == 0) break;
                    if (m.GetNumeratorNum() > nnum) nnum = m.GetNumeratorNum();
                    if (m.GetDenominatorNum() > dnum) dnum = m.GetDenominatorNum();
                    m = m.Next;
                }
                int ex = m_posList[m.X];

                int sy = (int)((y - nnum) * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                int ey = (int)((y + dnum + 1) * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);

                m_rect.X = sx + 1;
                m_rect.Y = sy - 1;
                m_rect.Width = ex - sx - 2;
                m_rect.Height = ey - sy - 2;
                g.DrawRectangle(m_sPen, m_rect);
            }
            else
            {
                m_current = null;
                AllDisableMode();
            }

            g.Dispose();
            this.Refresh();        
        }

        /// <summary>
        /// Change mode of Button and TextBox to be diable.
        /// </summary>
        private void AllDisableMode()
        {

            stringBox.ReadOnly = true;
            stringBox.Focus();

            PlusButton.Enabled = false;
            MinusButton.Enabled = false;
            MultiplyButton.Enabled = false;
            SplitButton.Enabled = false;
            ParentButton.Enabled = false;
            StringButton.Enabled = false;
            AddFunctionButton.Enabled = false;
            DeleteButton.Enabled = false;

            reserveBox.Enabled = false;
            AddButton.Enabled = false;
        }


        /// <summary>
        /// Change enable or unable of Button and TextBox.
        /// </summary>
        /// <param name="mode">edit mode.</param>
        private void EditMode(bool mode)
        {
            m_isText = mode;

            if (mode == true)
            {
                stringBox.ReadOnly = false;
                stringBox.Focus();

                PlusButton.Enabled = false;
                MinusButton.Enabled = false;
                MultiplyButton.Enabled = false;
                StringButton.Enabled = true;
                if (m_isExpression)
                {
                    AddFunctionButton.Enabled = true;
                    SplitButton.Enabled = true;
                    ParentButton.Enabled = true;
                }
                else
                {
                    AddFunctionButton.Enabled = false;
                    SplitButton.Enabled = false;
                    ParentButton.Enabled =false;
                }
                DeleteButton.Enabled = true;

                reserveBox.Enabled = true;
                AddButton.Enabled = true;
            }
            else
            {
                stringBox.ReadOnly = true;

                PlusButton.Enabled = true;
                MinusButton.Enabled = true;
                MultiplyButton.Enabled = true;
                SplitButton.Enabled = false;
                ParentButton.Enabled = false;
                StringButton.Enabled = false;
                AddFunctionButton.Enabled = false;
                DeleteButton.Enabled = true;

                reserveBox.Enabled = false;
                AddButton.Enabled = false;
            }
        }

        /// <summary>
        /// Redraw the formulator.
        /// </summary>
        private void UpdateFormulator()
        {
            Maping();

            int xpos = (int)FUtil.MARGIN;
            for (int i = 0; i < FUtil.XMAX; i++)
            {
                float maxWindth = 0;
                for (int j = 0; j < FUtil.YMAX; j++)
                {
                    if (m_array[i, j].Width > maxWindth)
                        maxWindth = m_array[i, j].Width;
                }
                if (maxWindth > 0)
                    xpos = xpos + (int)(maxWindth + FUtil.WIDTH_GAP);
            }
            if (xpos > m_sizeX)
            {
                m_sizeX = m_sizeX + 500;
                m_image = new Bitmap(m_sizeX, m_sizeY);
                pictureBox1.Image = m_image;
            }

            xpos = (int)FUtil.MARGIN;
            Graphics g = Graphics.FromImage(m_image);
            g.Clear(Color.WhiteSmoke);

            for (int i = 0; i < FUtil.XMAX; i++)
            {
                float maxWindth = 0;
                for (int j = 0; j < FUtil.YMAX; j++)
                {
                    if (m_array[i, j].Width > maxWindth)
                        maxWindth = m_array[i, j].Width;
                }

                for (int j = 0; j < FUtil.YMAX; j++)
                {
                    if (m_array[i, j].Type == FUtil.INPUT ||
                        m_array[i, j].Type == FUtil.OINPUT)
                        g.DrawRectangle(m_iPen,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN,
                            m_array[i, j].Width, FUtil.LINE_HEIGHT);
                    else if (m_array[i, j].Type == FUtil.STRING)
                        g.DrawString(m_array[i, j].Data, m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                    else if (m_array[i, j].Type == FUtil.PLUS)
                        g.DrawString("+", m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                    else if (m_array[i, j].Type == FUtil.MINUS)
                        g.DrawString("-", m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                    else if (m_array[i, j].Type == FUtil.MULTI)
                        g.DrawString("*", m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                    else if (m_array[i, j].Type == FUtil.LEFT)
                        g.DrawString("(", m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                    else if (m_array[i, j].Type == FUtil.RIGHT)
                        g.DrawString(")", m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                    else if (m_array[i, j].Type == FUtil.SPLIT)
                    {
                        if (m_array[i + 1, j].Type == FUtil.SPLIT)
                            g.DrawLine(Pens.Black, xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN + FUtil.LINE_HEIGHT / 2,
                                xpos + maxWindth + 5, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN + FUtil.LINE_HEIGHT / 2);
                        else
                            g.DrawLine(Pens.Black, xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN + FUtil.LINE_HEIGHT / 2,
                                xpos + maxWindth, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN + FUtil.LINE_HEIGHT / 2);
                    }
                    else if (m_array[i, j].IsFunction1() ||
                        m_array[i, j].IsFunction2())
                    {
                        g.DrawString(m_array[i, j].Data, m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                    }
                    else if (m_array[i,j].Type == FUtil.KAMMA)
                        g.DrawString(",", m_font, Brushes.Black,
                            xpos, j * (FUtil.LINE_HEIGHT + FUtil.HEIGHT_GAP) + FUtil.MARGIN);
                }
                if (maxWindth > 0)
                    xpos = xpos + (int)(maxWindth + FUtil.WIDTH_GAP);
                m_posList[i] = xpos;
            }
            g.Dispose();
            this.Refresh();
            m_rect.X = 0;
            m_rect.Y = 0;

            if (m_current != null)
            {
                if (m_current.Next != null && (m_current.Next.Type == FUtil.INPUT ||
                    m_current.Next.Type == FUtil.OINPUT))
                    Select(m_current.Next.X, m_current.Next.Y);
                else if (m_current.Type == FUtil.INPUT || m_current.Type == FUtil.OINPUT)
                    Select(m_current.X, m_current.Y);
                else if (m_current.IsFunction1())
                    Select(m_current.X + 2, m_current.Y);
                else if (m_current.IsFunction2())
                    Select(m_current.X + 2, m_current.Y);
                else
                    Unselect();
            }
        }

        /// <summary>
        /// Map the tree node data to the array data. 
        /// </summary>
        private void Maping()
        {
            for (int i = 0; i < FUtil.XMAX; i++)
            {
                for (int j = 0; j < FUtil.YMAX; j++)
                {
                    m_array[i, j].Type = FUtil.NONE;
                }
            }

            int baseY = 0;
            FNode current = m_top;
            while (current != null)
            {
                int tmp = current.GetNumeratorNum();
                if (tmp > baseY) baseY = tmp;
                current = current.Next;
            }

            int baseX = 0;
            current = m_top;
            while (current != null)
            {
                int tmp = current.GetLength();
                current.Mapping(baseX, baseY, m_array);

                baseX = baseX + tmp;
                current = current.Next;
            }
        }

        /// <summary>
        /// Add the reserved string to ComboBox.
        /// </summary>
        /// <param name="list">list of string added ComboBox.</param>
        public void AddReserveString(List<string> list)
        {
            foreach (string str in list)
            {
                reserveBox.Items.Add(str);
            }
        }

        /// <summary>
        /// Create FNode for function with using the type of function.
        /// This function does not set the width of this data.
        /// Please set the width to the return object.
        /// </summary>
        /// <param name="type">The function type.</param>
        /// <returns>FNode for function.</returns>
        internal FNode CreateFunctionNode(int type)
        {
            FNode n = null;
            if (type == FUtil.LOG) n = new FNode(FUtil.LOG, FUtil.STRLOG);
            else if (type == FUtil.LOG10) n = new FNode(FUtil.LOG10, FUtil.STRLOG10);
            else if (type == FUtil.SQRT) n = new FNode(FUtil.SQRT, FUtil.STRSQRT);
            else if (type == FUtil.EXP) n = new FNode(FUtil.EXP, FUtil.STREXP);
            else if (type == FUtil.CEIL) n = new FNode(FUtil.CEIL, FUtil.STRCEIL);
            else if (type == FUtil.FLOOR) n = new FNode(FUtil.FLOOR, FUtil.STRFLOOR);
            else if (type == FUtil.ABS) n = new FNode(FUtil.ABS, FUtil.STRABS);
            else if (type == FUtil.SIN) n = new FNode(FUtil.SIN, FUtil.STRSIN);
            else if (type == FUtil.COS) n = new FNode(FUtil.COS, FUtil.STRCOS);
            else if (type == FUtil.TAN) n = new FNode(FUtil.TAN, FUtil.STRTAN);
            else if (type == FUtil.ASIN) n = new FNode(FUtil.ASIN, FUtil.STRASIN);
            else if (type == FUtil.ACOS) n = new FNode(FUtil.ACOS, FUtil.STRACOS);
            else if (type == FUtil.ATAN) n = new FNode(FUtil.ATAN, FUtil.STRATAN);

            return n;
        }

        /// <summary>
        /// Create FNode for function with using the function name.
        /// (log, log10, sqrt, exp, ceil, floor, abs, sin, cos, tan, asin, acos, atan)
        /// This function does not set the width of this data.
        /// Please set the width to the return object.
        /// </summary>
        /// <param name="type">The function name.</param>
        /// <returns>FNode for function.</returns>
        internal FNode CreateFunctionNode(string type)
        {
            FNode n = null;
            if (type.Equals(FUtil.STRLOG)) n = new FNode(FUtil.LOG, FUtil.STRLOG);
            else if (type.Equals(FUtil.STRLOG10)) n = new FNode(FUtil.LOG10, FUtil.STRLOG10);
            else if (type.Equals(FUtil.STRSQRT)) n = new FNode(FUtil.SQRT, FUtil.STRSQRT);
            else if (type.Equals(FUtil.STREXP)) n = new FNode(FUtil.EXP, FUtil.STREXP);
            else if (type.Equals(FUtil.CEIL)) n = new FNode(FUtil.CEIL, FUtil.STRCEIL);
            else if (type.Equals(FUtil.FLOOR)) n = new FNode(FUtil.FLOOR, FUtil.STRFLOOR);
            else if (type.Equals(FUtil.ABS)) n = new FNode(FUtil.ABS, FUtil.STRABS);
            else if (type.Equals(FUtil.SIN)) n = new FNode(FUtil.SIN, FUtil.STRSIN);
            else if (type.Equals(FUtil.COS)) n = new FNode(FUtil.COS, FUtil.STRCOS);
            else if (type.Equals(FUtil.TAN)) n = new FNode(FUtil.TAN, FUtil.STRTAN);
            else if (type.Equals(FUtil.ASIN)) n = new FNode(FUtil.ASIN, FUtil.STRASIN);
            else if (type.Equals(FUtil.ACOS)) n = new FNode(FUtil.ACOS, FUtil.STRACOS);
            else if (type.Equals(FUtil.ATAN)) n = new FNode(FUtil.ATAN, FUtil.STRATAN);

            return n;
        }
        #endregion
    }
}
