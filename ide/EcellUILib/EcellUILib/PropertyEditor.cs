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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Formulator;
using EcellLib.Objects;

namespace EcellLib
{
    public partial class PropertyEditor : Form
    {
        #region Fields
        /// <summary>
        /// current variable reference list string.
        /// </summary>
        public string m_refStr = "";
        /// <summary>
        /// Data type displayed in PropertyEditor.
        /// </summary>
        public string m_type;
        /// <summary>
        /// Class name displayed in PropertyEditor.
        /// </summary>
        public string m_propName;
        /// <summary>
        /// key is property name, value is property data type.
        /// </summary>
        private Dictionary<string, EcellData> m_propDict = new Dictionary<string, EcellData>();
        /// <summary>
        /// m_win (editable variable reference list window)
        /// </summary>
        VariableReferenceEditDialog m_win;
        /// <summary>
        /// current selected object.
        /// </summary>
        private EcellObject m_currentObj;
        /// <summary>
        /// parent object to add object.
        /// </summary>
        private EcellObject m_parentObj;
        /// <summary>
        /// DataManager.
        /// </summary>
        DataManager m_dManager;
        /// <summary>
        /// PluginManager.
        /// </summary>
        PluginManager m_pManager;
        /// <summary>
        /// text box of expression.
        /// </summary>
        public TextBox m_text;
        /// <summary>
        /// TextBox for ID.
        /// </summary>
        public TextBox m_idText;
        /// <summary>
        /// window to edit formulator of expression at process.
        /// </summary>
        private FormulatorDialog m_fwin = null;
        /// <summary>
        /// user control of formulator.
        /// </summary>
        private FormulatorControl m_cnt = null;
        private String m_title = null;
        /// <summary>
        /// ResourceManager for PropertyEditor.
        /// </summary>
        private static ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResUILib));
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for PropertyEditor. 
        /// </summary>
        private PropertyEditor(DataManager dManager, PluginManager pManager)
        {
            InitializeComponent();
            m_title = this.Text;
            m_dManager = dManager;
            m_pManager = pManager;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Show PropertyEditor Dialog.
        /// </summary>
        /// <param name="dManager">DataManager.</param>
        /// <param name="pManager">PluginManager.</param>
        /// <param name="obj"></param>
        public static void Show(DataManager dManager, PluginManager pManager, EcellObject obj)
        {
            PropertyEditor editor = new PropertyEditor(dManager, pManager);
            try
            {
                editor.layoutPanel.SuspendLayout();
                editor.SetCurrentObject(obj);
                editor.SetDataType(obj.Type);
                editor.LayoutPropertyEditor();
                editor.layoutPanel.ResumeLayout(false);
                if (editor.ShowDialog() == DialogResult.OK)
                    editor.UpdateProperty();
            }
            catch (IgnoreException ex)
            {
                ex.ToString();
                return;
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResUILib.ErrSetProp,
                    new object[] { obj.Key }));
            }
            finally
            {
                editor.Dispose();
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Set the object to parent object.
        /// </summary>
        /// <param name="obj">the parent object to add.</param>
        private void SetParentObject(EcellObject obj)
        {
            m_parentObj = obj;
            m_currentObj = null;
        }

        /// <summary>
        /// Set the object to current selected object.
        /// </summary>
        /// <param name="obj">EcellObject</param>
        private void SetCurrentObject(EcellObject obj)
        {
            m_currentObj = obj;
            m_parentObj = null;
            if (m_currentObj.Type.Equals(EcellObject.PROCESS))
            {
                m_propName = m_currentObj.Classname;
                m_refStr = m_currentObj.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST).ToString();
            }
        }

        /// <summary>
        /// Set data type displayed in PropertyEditor.
        /// </summary>
        /// <param name="type">data type of object.</param>
        private void SetDataType(string type)
        {
            m_type = type;
        }

        /// <summary>
        /// Get the commit information of this data.
        /// </summary>
        /// <param name="d">data object.</param>
        private void GetCommitInfo(EcellData d)
        {
            bool isCheck = false;
            IEnumerator iter = commitLayoutPanel.Controls.GetEnumerator();
            while (iter.MoveNext())
            {
                Control c = (Control)iter.Current;
                if (c == null) continue;
                if (c.Tag == null) continue;
                if (!c.Tag.ToString().Equals(d.Name)) continue;
                TableLayoutPanelCellPosition pos =
                    layoutPanel.GetPositionFromControl(c);
                if (pos.Column == 0) // IsCommit
                {
                    CheckBox c1 = c as CheckBox;
                    if (c1 == null) continue;
                    if (c1.Checked)
                        isCheck = true;
                }
                else if (pos.Column == 2) // Max
                {
                    TextBox t = c as TextBox;
                    if (t == null) continue;
                    if (t.Text == null || t.Text == "") continue;
                    d.Max = Convert.ToDouble(t.Text);
                }
                else if (pos.Column == 3) // Min
                {
                    TextBox t = c as TextBox;
                    if (t == null) continue;
                    if (t.Text == null || t.Text == "") continue;
                    d.Min = Convert.ToDouble(t.Text);
                }
                else if (pos.Column == 4) // Step
                {
                    TextBox t = c as TextBox;
                    if (t == null) continue;
                    if (t.Text == null || t.Text == "") continue;
                    d.Step = Convert.ToDouble(t.Text);
                }
            }
            if (d.Max < d.Min)
            {
                throw new Exception("Invalid parameter(MAX < Min).");
            }
            if (!isCheck)
                m_dManager.SetParameterData(new EcellParameterData(d.EntityPath,
                    d.Max, d.Min, d.Step));
            else
                m_dManager.RemoveParameterData(new EcellParameterData(d.EntityPath, 0.0));
        }

        /// <summary>
        /// layout the column of property editor according to data type.
        /// </summary>
        private void LayoutPropertyEditor()
        {
            if (m_type.Equals("Model"))
            {
                LayoutModelPropertyEditor();
                LayoutModelCommit();
            }
            else if (m_type.Equals(EcellObject.SYSTEM) ||
                m_type.Equals(EcellObject.VARIABLE) ||
                m_type.Equals(EcellObject.PROCESS) ||
                m_type.Equals(EcellObject.TEXT))
            {
                LayoutNodePropertyEditor();
                LayoutNodeCommit();
            }
        }

        /// <summary>
        /// Layout the commit information of model data.
        /// </summary>
        private void LayoutModelCommit()
        {
            m_propDict.Clear();
        }

        /// <summary>
        /// Layout the commit infomation of node data.
        /// </summary>
        private void LayoutNodeCommit()
        {
            int i = 0;
            int width = commitLayoutPanel.Width;
            commitLayoutPanel.Controls.Clear();
            commitLayoutPanel.RowStyles.Clear();
            commitLayoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 1));
            commitLayoutPanel.RowCount = m_propDict.Keys.Count;

            commitLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l1 = new Label();
            l1.Text = "Name";
            l1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            l1.TextAlign = ContentAlignment.MiddleCenter;
            commitLayoutPanel.Controls.Add(l1, 1, i);

            Label l2 = new Label();
            l2.Text = "Max";
            l2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            l2.TextAlign = ContentAlignment.MiddleCenter;
            commitLayoutPanel.Controls.Add(l2, 2, i);

            Label l3 = new Label();
            l3.Text = "Min";
            l3.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            l3.TextAlign = ContentAlignment.MiddleCenter;
            commitLayoutPanel.Controls.Add(l3, 3, i);

            Label l4 = new Label();
            l4.Text = "Step";
            l4.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            l4.TextAlign = ContentAlignment.MiddleCenter;
            commitLayoutPanel.Controls.Add(l4, 4, i);
            i++;

            foreach (string key in m_propDict.Keys)
            {
                if (key == "Size")
                {
                    continue;
                }

                EcellParameterData param = m_dManager.GetParameterData(key);
                if (param == null)
                {
                    param = new EcellParameterData(key, m_propDict[key].Value.CastToDouble());
                }
                commitLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                if (m_propDict[key].Settable &&
                    m_propDict[key].Value.Type == typeof(double))
                {
                    CheckBox c = new CheckBox();
                    if (m_dManager.IsContainsParameterData(m_propDict[key].EntityPath))
                        c.Checked = false;
                    else c.Checked = true;
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    c.Text = "";
                    c.Tag = key;
                    c.AutoSize = true;
                    c.Enabled = true;
                    commitLayoutPanel.Controls.Add(c, 0, i);
                }
                else
                {
                    CheckBox c = new CheckBox();
                    c.Checked = true;
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    c.Text = "";
                    c.Tag = key;
                    c.AutoSize = true;
                    c.Enabled = false;
                    commitLayoutPanel.Controls.Add(c, 0, i);
                }

                Label l = new Label();
                l.Text = key;
                l.Dock = DockStyle.Fill;
                commitLayoutPanel.Controls.Add(l, 1, i);

                TextBox t1 = new TextBox();

                t1.Dock = DockStyle.Fill;
                t1.Tag = key;
                if (!m_propDict[key].Settable ||
                    m_propDict[key].Value.Type != typeof(double))
                {
                    t1.ReadOnly = true;
                    t1.Text = param.Max.ToString();
                }
                else
                {
                    if (m_propDict[key].Max == 0.0)
                    {
                        t1.Text = Convert.ToString(m_propDict[key].Value.CastToDouble() * 1.5);
                    }
                    else
                    {
                        t1.Text = param.Max.ToString();
                    }
                }
                //                    t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                commitLayoutPanel.Controls.Add(t1, 2, i);

                TextBox t2 = new TextBox();
                t2.Dock = DockStyle.Fill;
                t2.Tag = key;
                if (!m_propDict[key].Settable ||
                    m_propDict[key].Value.Type != typeof(double))
                {
                    t2.ReadOnly = true;
                    t2.Text = param.Min.ToString();
                }
                else
                {
                    if (m_propDict[key].Min == 0.0)
                    {
                        t2.Text = Convert.ToString(m_propDict[key].Value.CastToDouble() * 0.5);
                    }
                    else
                    {
                        t2.Text = param.Min.ToString();
                    }
                }


                //                    t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                commitLayoutPanel.Controls.Add(t2, 3, i);

                TextBox t3 = new TextBox();
                t3.Text = param.Step.ToString();
                t3.Dock = DockStyle.Fill;
                t3.Tag = key;
                if (!m_propDict[key].Settable ||
                    m_propDict[key].Value.Type != typeof(double))
                {
                    t3.ReadOnly = true;
                }
                //                    t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                commitLayoutPanel.Controls.Add(t3, 4, i);
                i++;
            }

            if (m_currentObj == null && m_type.Equals(EcellObject.SYSTEM))
            {
                CheckBox c = new CheckBox();
                c.Checked = true;
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                c.Text = "";
                c.AutoSize = true;
                c.Enabled = true;
                c.Tag = "Size";
                commitLayoutPanel.Controls.Add(c, 0, i);

                commitLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l = new Label();
                l.Text = "Size";
                l.Dock = DockStyle.Fill;
                commitLayoutPanel.Controls.Add(l, 1, i);


                TextBox t1 = new TextBox();
                t1.Text = "";
                t1.Dock = DockStyle.Fill;
                t1.ReadOnly = true;
                t1.Tag = "Size";
                commitLayoutPanel.Controls.Add(t1, 2, i);

                TextBox t2 = new TextBox();
                t2.Text = "";
                t2.Dock = DockStyle.Fill;
                t2.ReadOnly = true;
                t2.Tag = "Size";
                commitLayoutPanel.Controls.Add(t2, 3, i);

                TextBox t3 = new TextBox();
                t3.Text = "";
                t3.Dock = DockStyle.Fill;
                t3.ReadOnly = true;
                t3.Tag = "Size";
                commitLayoutPanel.Controls.Add(t3, 4, i);
                i++;
            }
            else if (m_currentObj != null && m_currentObj.Type.Equals(EcellObject.SYSTEM))
            {
                commitLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

                CheckBox c = new CheckBox();
                c.Checked = true;
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                c.Text = "";
                c.Tag = "Size";
                c.AutoSize = true;
                c.Enabled = true;
                commitLayoutPanel.Controls.Add(c, 0, i);

                Label l = new Label();
                l.Text = "Size";
                l.Dock = DockStyle.Fill;
                commitLayoutPanel.Controls.Add(l, 1, i);

                TextBox t1 = new TextBox();
                t1.Text = "";
                t1.Dock = DockStyle.Fill;
                t1.Tag = "Size";
                commitLayoutPanel.Controls.Add(t1, 2, i);

                TextBox t2 = new TextBox();
                t2.Text = "";
                t2.Dock = DockStyle.Fill;
                t2.Tag = "Size";
                commitLayoutPanel.Controls.Add(t2, 3, i);

                TextBox t3 = new TextBox();
                t3.Text = "";
                t3.Dock = DockStyle.Fill;
                t3.Tag = "Size";
                commitLayoutPanel.Controls.Add(t3, 4, i);

                if (m_currentObj.Children != null)
                {
                    foreach (EcellObject o in m_currentObj.Children)
                    {
                        if (o.Key.EndsWith(":SIZE"))
                        {
                            foreach (EcellData d in o.Value)
                            {
                                if (d.EntityPath.EndsWith(":Value"))
                                {
                                    EcellParameterData pvalue = m_dManager.GetParameterData(d.EntityPath);
                                    if (pvalue == null)
                                    {
                                        pvalue = new EcellParameterData(d.EntityPath,
                                            d.Value.CastToDouble());
                                    }
                                    t1.Text = pvalue.Max.ToString();
                                    t2.Text = pvalue.Min.ToString();
                                    t3.Text = pvalue.Step.ToString();
                                }
                            }
                        }
                    }
                }
            }

            panel2.ClientSize = panel2.Size;
        }

        /// <summary>
        /// layout the column of property editor for Model.
        /// </summary>
        private void LayoutModelPropertyEditor()
        {
            m_propDict.Clear();

            layoutPanel.Size = new Size(layoutPanel.Width, 30);
            layoutPanel.SuspendLayout();
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l = new Label();
            l.Text = "modelID";
            l.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(l, 1, 0);
            TextBox t = new TextBox();
            if (m_currentObj == null)
            {
                t.Text = "";
                t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
            }
            else
            {
                t.Text = m_currentObj.ModelID;
                t.ReadOnly = true;
            }
            m_idText = t;
            t.Dock = DockStyle.Fill;
            t.Tag = "modelID";
            layoutPanel.Controls.Add(t, 2, 0);
            layoutPanel.ResumeLayout(false);
        }

        /// <summary>
        /// layout the column of property editor for System, Process and Variable.
        /// </summary>
        private void LayoutNodePropertyEditor()
        {
            Dictionary<string, EcellData> tmpProcDict = null;
            if (m_currentObj == null)
            {
                if (m_type.Equals(EcellObject.PROCESS))
                    m_propDict = m_dManager.GetProcessProperty(m_propName);
                else if (m_type.Equals(EcellObject.SYSTEM))
                    m_propDict = m_dManager.GetSystemProperty();
                else if (m_type.Equals(EcellObject.VARIABLE))
                    m_propDict = m_dManager.GetVariableProperty();
            }
            else
            {
                if (m_propDict.Count == 0)
                {
                    m_propDict.Clear();
                    foreach (EcellData d in m_currentObj.Value)
                        m_propDict.Add(d.Name, d);
                }
                if (m_propName != null && m_propName.StartsWith("Expression"))
                {
                    tmpProcDict = m_dManager.GetProcessProperty(m_currentObj.Classname);
                }
                this.Text = m_title + "  - " + m_currentObj.Key;
            }

            String preId = "";
            IEnumerator iter = layoutPanel.Controls.GetEnumerator();
            while (iter.MoveNext())
            {
                Control c = (Control)iter.Current;
                if (c == null) continue;
                TableLayoutPanelCellPosition pos =
                    layoutPanel.GetPositionFromControl(c);
                if (pos.Column != 1) continue;

                if ((string)c.Tag == "id")
                {
                    preId = c.Text;
                    break;
                }
            }

            int i = 0;
            int width = layoutPanel.Width;
            layoutPanel.Controls.Clear();
            layoutPanel.RowStyles.Clear();

            if (m_currentObj == null && m_type.Equals(EcellObject.SYSTEM))
            {
                layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 5));
                layoutPanel.RowCount = m_propDict.Keys.Count + 5;
            }
            else if (m_currentObj != null && m_currentObj.Type.Equals(EcellObject.SYSTEM))
            {
                layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 5));
                layoutPanel.RowCount = m_propDict.Keys.Count + 5;
            }
            else
            {
                layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 5));
                layoutPanel.RowCount = m_propDict.Keys.Count + 5;
            }

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l1 = new Label();
            l1.Text = "modelID";
            l1.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(l1, 1, i);
            TextBox t1 = new TextBox();
            t1.Tag = "modelID";
            t1.Dock = DockStyle.Fill;
            t1.KeyPress += new KeyPressEventHandler(EnterKeyPress);
            if (m_currentObj != null) t1.Text = m_currentObj.ModelID;
            else t1.Text = m_parentObj.ModelID;
            t1.ReadOnly = true;
            layoutPanel.Controls.Add(t1, 2, i);
            i++;

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l2 = new Label();
            l2.Text = "id";
            l2.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(l2, 1, i);
            TextBox t2 = new TextBox();
            t2.Tag = "id";
            if (m_currentObj == null)
            {
                t2.Text = preId;
                t2.KeyPress += new KeyPressEventHandler(EnterKeyPress);
            }
            else
            {
                //                    t2.ReadOnly = true;
                t2.Text = m_currentObj.Key;
                if (m_currentObj.Key.Equals("/"))
                {
                    t2.ReadOnly = true;
                }
                else
                {
                    t2.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                }
            }
            m_idText = t2;
            t2.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(t2, 2, i);
            i++;

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l3 = new Label();
            l3.Text = "classname";
            l3.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(l3, 1, i);
            ComboBox combo = new ComboBox();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            int j = 0;
            if (m_type.Equals(EcellObject.PROCESS))
            {
                List<string> list = m_dManager.GetProcessList();
                int selectedIndex = -1;
                foreach (string str in list)
                {
                    combo.Items.AddRange(new object[] { str });
                    if (str == m_propName) selectedIndex = j;
                    j++;
                }
                if (selectedIndex == -1)
                {
                    combo.Items.AddRange(new object[] { m_propName });
                    selectedIndex = j;
                }
                combo.SelectedIndex = selectedIndex;
                combo.SelectedIndexChanged += new EventHandler(ComboSelectedIndexChanged);
            }
            else if (m_type.Equals(EcellObject.SYSTEM))
            {
                combo.Items.AddRange(new object[] { EcellObject.SYSTEM });
                combo.SelectedIndex = j;
            }
            else if (m_type.Equals(EcellObject.VARIABLE))
            {
                combo.Items.AddRange(new object[] { EcellObject.VARIABLE });
                combo.SelectedIndex = j;
            }
            else if (m_type.Equals(EcellObject.TEXT))
            {
                combo.Items.AddRange(new object[] { EcellObject.TEXT });
                combo.SelectedIndex = j;
            }
            combo.Tag = "classname";
            combo.Dock = DockStyle.Fill;
            combo.KeyPress += new KeyPressEventHandler(EnterKeyPress);
            layoutPanel.Controls.Add(combo, 2, i);
            i++;

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l4 = new Label();
            l4.Text = "type";
            l4.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(l4, 1, i);
            TextBox t4 = new TextBox();
            t4.Text = "";
            t4.Tag = "type";
            t4.Dock = DockStyle.Fill;
            t4.Text = m_type;
            t4.ReadOnly = true;
            t4.KeyPress += new KeyPressEventHandler(EnterKeyPress);
            layoutPanel.Controls.Add(t4, 2, i);
            i++;

            foreach (string key in m_propDict.Keys)
            {
                if (key == "Size")
                {
                    continue;
                }

                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                if (m_propDict[key].Logable)
                {
                    CheckBox c = new CheckBox();
                    if (m_propDict[key].Logged)
                    {
                        c.Checked = true;
                    }
                    else
                    {
                        c.Checked = false;
                    }
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    c.Text = "";
                    c.AutoSize = true;
                    c.Enabled = true;
                    layoutPanel.Controls.Add(c, 0, i);
                }
                else
                {
                    CheckBox c = new CheckBox();
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    c.Text = "";
                    c.AutoSize = true;
                    c.Checked = false;
                    c.Enabled = false;
                    layoutPanel.Controls.Add(c, 0, i);
                }

                Label l = new Label();
                l.Text = key;
                l.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(l, 1, i);

                if (key == EcellProcess.VARIABLEREFERENCELIST)
                {
                    Button b = new Button();
                    b.Text = "Edit Variable References ...";
                    b.Tag = key;
                    b.Dock = DockStyle.Fill;
                    b.Click += new EventHandler(ShowVarRefWindow);
                    b.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                    layoutPanel.Controls.Add(b, 2, i);
                }
                else if (key == EcellProcess.STEPPERID)
                {
                    ComboBox t = new ComboBox();
                    List<EcellObject> slist;
                    slist = m_dManager.GetStepper(null, m_currentObj.ModelID);
                    foreach (EcellObject obj in slist)
                    {
                        t.Items.AddRange(new object[] { obj.Key });
                    }

                    t.Text = m_propDict[key].Value.ToString();
                    t.Tag = key;
                    t.Dock = DockStyle.Fill;
                    t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                    layoutPanel.Controls.Add(t, 2, i);
                }
                else
                {
                    TextBox t = new TextBox();
                    t.Text = "";
                    t.Tag = key;
                    t.Dock = DockStyle.Fill;
                    t.Text = m_propDict[key].Value.ToString();
                    if (!m_propDict[key].Settable)
                    {
                        t.ReadOnly = true;
                    }
                    t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                    layoutPanel.Controls.Add(t, 2, i);

                    if ((key == "Expression"))
                    {
                        Button b = new Button();
                        b.Text = "...";
                        b.Tag = "Formulator";
                        b.Dock = DockStyle.Fill;
                        b.Click += new EventHandler(ShowFormulatorWindow);
                        m_text = t;
                        layoutPanel.Controls.Add(b, 3, i);
                    }
                    else if (tmpProcDict != null && !tmpProcDict.ContainsKey(key))
                    {
                        Button b = new Button();
                        b.Text = "Delete";
                        b.Tag = key;
                        b.Dock = DockStyle.Fill;
                        b.Click += new EventHandler(DeletePropertyForProcess);
                        layoutPanel.Controls.Add(b, 3, i);
                    }
                }
                i++;
            }
            if (m_type.Equals(EcellObject.PROCESS) &&
                this.m_dManager.IsEnableAddProperty(m_propName))
            {
                Button b = new Button();
                b.Text = "Add Property";
                b.Tag = "Add Property";
                b.Dock = DockStyle.Fill;
                b.Click += new EventHandler(AddPropertyForProcess);
                layoutPanel.Controls.Add(b, 2, i);
                i++;
            }

            if (m_currentObj == null && m_type.Equals(EcellObject.SYSTEM))
            {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l = new Label();
                l.Text = "Size";
                l.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(l, 1, i);

                TextBox t = new TextBox();
                t.Text = "";
                t.Tag = "DefinedSize";
                t.Dock = DockStyle.Fill;
                t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                layoutPanel.Controls.Add(t, 2, i);
            }
            else if (m_currentObj != null && m_currentObj.Type.Equals(EcellObject.SYSTEM))
            {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l = new Label();
                l.Text = "Size";
                l.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(l, 1, i);

                TextBox t = new TextBox();
                t.Text = "";
                t.Tag = "DefinedSize";
                t.Dock = DockStyle.Fill;
                t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                layoutPanel.Controls.Add(t, 2, i);

                if (m_currentObj.Children != null)
                {
                    foreach (EcellObject o in m_currentObj.Children)
                    {
                        if (o.Key.EndsWith(":SIZE"))
                        {
                            foreach (EcellData d in o.Value)
                            {
                                if (d.EntityPath.EndsWith(":Value"))
                                {
                                    t.Text = d.Value.ToString();
                                }
                            }
                        }
                    }
                }
            }

            panel1.ClientSize = panel1.Size;
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="modelID">the model of object.</param>
        /// <param name="oldKey">the key of object before edit.</param>
        /// <param name="eo">The EcellObject changed the property.</param>
        private void NotifyDataChanged(
            string modelID,
            string oldKey,
            EcellObject eo)
        {
            if (modelID == null || oldKey == null || eo.Key == null)
                return;
            m_dManager.DataChanged(eo.ModelID, oldKey, eo.Type, eo, true, true);
        }

        ///// <summary>
        ///// Get the object from the property in PropertyEditor.
        ///// </summary>
        ///// <returns></returns>
        //public EcellObject Collect()
        //{
        //    string id = "";
        //    string modelID = "";
        //    string key = "";
        //    string classname = "";
        //    string type = "";
        //    bool isLogger = false;
        //    EcellObject sizeObj = null;
        //    List<EcellData> list = new List<EcellData>();

        //    try
        //    {
        //        IEnumerator iter = layoutPanel.Controls.GetEnumerator();
        //        while (iter.MoveNext())
        //        {
        //            Control c = (Control)iter.Current;
        //            if (c == null) continue;
        //            TableLayoutPanelCellPosition pos =
        //                layoutPanel.GetPositionFromControl(c);
        //            if (pos.Column == 0)
        //            {
        //                CheckBox chk = c as CheckBox;
        //                if (chk == null)
        //                {
        //                    isLogger = false;
        //                    continue;
        //                }
        //                isLogger = chk.Checked;
        //                continue;

        //            }
        //            if (pos.Column != 2) continue;
        //            if ((string)c.Tag == "Add Property") continue;

        //            if ((string)c.Tag == "modelID") modelID = c.Text;
        //            else if ((string)c.Tag == "id")
        //            {
        //                id = c.Text;
        //                if (c.Text == "")
        //                {
        //                    String errmes = MessageResUILib.ErrNoInput;
        //                    Util.ShowWarningDialog(errmes + "(ID)");
        //                    return null;
        //                }
        //                else if (Util.IsNGforID(c.Text))
        //                //                        else if (c.Text.Contains("/") || c.Text.Contains(":"))
        //                {
        //                    Util.ShowWarningDialog(MessageResUILib.ErrInvalidID);

        //                    return null;
        //                }
        //                else if (c.Text.ToUpper() == "SIZE")
        //                {
        //                    Util.ShowWarningDialog(MessageResUILib.ErrReserveSize);

        //                    return null;
        //                }
        //                else if (m_currentObj != null)
        //                {
        //                    if (m_currentObj.Type == EcellObject.SYSTEM && Util.IsNGforSystemFullID(c.Text))
        //                    {
        //                        Util.ShowWarningDialog(MessageResUILib.ErrInvalidID);

        //                        return null;
        //                    }
        //                    if (m_currentObj.Type != "System" && Util.IsNGforComponentFullID(c.Text))
        //                    {
        //                        Util.ShowWarningDialog(MessageResUILib.ErrInvalidID);

        //                        return null;
        //                    }
        //                }

        //                if (!m_type.Equals(EcellObject.SYSTEM))
        //                {
        //                    if (m_parentObj.Key == "") key = c.Text;
        //                    else if (m_parentObj.Key == "/") key = "/:" + c.Text;
        //                    else key = m_parentObj.Key + ":" + c.Text;
        //                }
        //                else
        //                {
        //                    if (m_parentObj.Key == "") key = c.Text;
        //                    else if (m_parentObj.Key == "/") key = "/" + c.Text;
        //                    else key = m_parentObj.Key + "/" + c.Text;

        //                }
        //            }
        //            else if ((string)c.Tag == "classname") classname = c.Text;
        //            else if ((string)c.Tag == "type") type = c.Text;
        //            else if ((string)c.Tag == EcellProcess.VARIABLEREFERENCELIST)
        //            {
        //                EcellData data = new EcellData();
        //                data.Name = (string)c.Tag;
        //                data.Value = EcellValue.ToVariableReferenceList(m_refStr);
        //                data.EntityPath = type + ":" + m_parentObj.Key +
        //                    ":" + id + ":" + (string)c.Tag;
        //                data.Settable = m_propDict[data.Name].Settable;
        //                data.Saveable = m_propDict[data.Name].Saveable;
        //                data.Loadable = m_propDict[data.Name].Loadable;
        //                data.Gettable = m_propDict[data.Name].Gettable;
        //                data.Logable = m_propDict[data.Name].Logable;
        //                data.Logged = m_propDict[data.Name].Logged;

        //                list.Add(data);
        //            }
        //            else if ((string)c.Tag == "DefinedSize")
        //            {
        //                if (c.Text == "") continue;
        //                List<EcellData> dList = new List<EcellData>();
        //                Dictionary<string, EcellData> sList = m_dManager.GetVariableProperty();
        //                foreach (string p in sList.Keys)
        //                {
        //                    EcellData d = sList[p];
        //                    if (p == "Value")
        //                    {
        //                        d.Value = new EcellValue(Convert.ToDouble(c.Text));
        //                    }
        //                    dList.Add(d);
        //                }
        //                sizeObj = EcellObject.CreateObject(modelID, key + ":SIZE", EcellObject.VARIABLE, EcellObject.VARIABLE, dList);
        //            }
        //            else
        //            {
        //                EcellData data = new EcellData();
        //                try
        //                {
        //                    data.Name = (string)c.Tag;
        //                    if (m_propDict[data.Name].Value.Type == typeof(int))
        //                        data.Value = new EcellValue(Convert.ToInt32(c.Text));
        //                    else if (m_propDict[data.Name].Value.Type == typeof(double))
        //                    {
        //                        if (c.Text == "1.79769313486232E+308")
        //                            data.Value = new EcellValue(Double.MaxValue);
        //                        else
        //                            data.Value = new EcellValue(Convert.ToDouble(c.Text));
        //                    }
        //                    else if (m_propDict[data.Name].Value.Type == typeof(List<EcellValue>))
        //                        data.Value = EcellValue.ToList(c.Text);
        //                    else
        //                        data.Value = new EcellValue(c.Text);
        //                    data.EntityPath = type + ":" + m_parentObj.Key +
        //                        ":" + id + ":" + (string)c.Tag;
        //                    data.Settable = m_propDict[data.Name].Settable;
        //                    data.Saveable = m_propDict[data.Name].Saveable;
        //                    data.Loadable = m_propDict[data.Name].Loadable;
        //                    data.Gettable = m_propDict[data.Name].Gettable;
        //                    data.Logable = m_propDict[data.Name].Logable;
        //                    //                            data.Logged = m_propDict[data.Name].Logged;
        //                    data.Logged = isLogger;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Trace.WriteLine(ex);
        //                    return null;
        //                }
        //                list.Add(data);
        //            }
        //        }

        //        EcellObject obj = EcellObject.CreateObject(modelID, key, type, classname, list);
        //        obj.Children = new List<EcellObject>();
        //        if (sizeObj != null) obj.Children.Add(sizeObj);

        //        return obj;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Update property of the selected TreeNode.
        /// </summary>
        private void UpdateProperty()
        {
            string modelID = "";
            string key = "";
            string classname = "";
            string type = "";
            bool isLogger = false;
            List<EcellData> list = new List<EcellData>();
            IEnumerator iter = layoutPanel.Controls.GetEnumerator();
            try
            {
                while (iter.MoveNext())
                {
                    Control c = (Control)iter.Current;
                    if (c == null) continue;
                    TableLayoutPanelCellPosition pos =
                        layoutPanel.GetPositionFromControl(c);
                    if (pos.Column == 0)
                    {
                        CheckBox chk = c as CheckBox;
                        if (c == null)
                        {
                            isLogger = false;
                            continue;
                        }
                        isLogger = chk.Checked;
                    }
                    if (pos.Column != 2) continue;

                    if ((string)c.Tag == "Add Property") continue;
                    if ((string)c.Tag == "modelID") modelID = c.Text;
                    else if ((string)c.Tag == "id")
                    {
                        key = c.Text;
                        if (c.Text == "")
                        {
                            Util.ShowWarningDialog(String.Format(MessageResUILib.ErrNoSet,
                                new object[] { "ID" }));
                            return;
                        }
                        else if (c.Text.ToUpper() == "SIZE")
                        {
                            Util.ShowWarningDialog(String.Format(MessageResUILib.ErrReserved,
                                new object[] { "SIZE" }));
                            return;
                        }
                        else if (m_currentObj.Type.Equals(EcellObject.SYSTEM) &&
                            Util.IsNGforSystemFullID(c.Text))
                        {
                            Util.ShowWarningDialog(MessageResUILib.ErrInvalidID);
                            return;
                        }
                        else if (!m_currentObj.Type.Equals(EcellObject.SYSTEM) &&
                            Util.IsNGforComponentFullID(c.Text))
                        {
                            Util.ShowWarningDialog(MessageResUILib.ErrInvalidID);
                            return;
                        }
                        else if (m_currentObj.Type.Equals(EcellObject.PROCESS) ||
                                    m_currentObj.Type.Equals(EcellObject.VARIABLE))
                        {
                            int kpos = c.Text.IndexOf(':');
                            if (kpos < 0 || kpos == c.Text.Length - 1)
                            {
                                Util.ShowWarningDialog(MessageResUILib.ErrInvalidID);
                                return;
                            }
                        }
                    }
                    else if ((string)c.Tag == "classname") classname = c.Text;
                    else if ((string)c.Tag == "type") type = c.Text;
                    else if ((string)c.Tag == EcellProcess.VARIABLEREFERENCELIST)
                    {
                        EcellData data = new EcellData();
                        data.Name = (string)c.Tag;
                        data.Value = EcellValue.ToVariableReferenceList(m_refStr);
                        if (key.Contains(":"))
                        {
                            int ind = key.LastIndexOf(":");
                            data.EntityPath = type + ":" + key.Substring(0, ind) +
                                ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                        }
                        else
                        {
                            if (key == "/")
                            {
                                data.EntityPath = type + ":" + "" +
                                    ":" + "/" + ":" + (string)c.Tag;
                            }
                            else
                            {
                                int ind = key.LastIndexOf("/");
                                data.EntityPath = type + ":" + key.Substring(0, ind) +
                                    ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                            }
                        }

                        list.Add(data);
                    }
                    else if ((string)c.Tag == "DefinedSize")
                    {
                        EcellObject target = null;
                        Double sizeData = 0.1;
                        if (m_currentObj.Children != null)
                        {
                            foreach (EcellObject o in m_currentObj.Children)
                            {
                                if (o.Key.EndsWith(":SIZE"))
                                {
                                    target = o;
                                    break;
                                }
                            }
                        }

                        if (target == null)
                        {
                            if (c.Text != "")
                            {

                                Dictionary<string, EcellData> plist = m_dManager.GetVariableProperty();
                                List<EcellData> dlist = new List<EcellData>();
                                foreach (string pname in plist.Keys)
                                {
                                    if (pname.Equals("Value"))
                                    {
                                        EcellData d = plist[pname];
                                        d.Value = new EcellValue(Convert.ToDouble(c.Text));
                                        dlist.Add(d);
                                    }
                                    else
                                    {
                                        dlist.Add(plist[pname]);
                                    }
                                }
                                EcellObject obj = EcellObject.CreateObject(m_currentObj.ModelID,
                                    m_currentObj.Key + ":SIZE", EcellObject.VARIABLE, EcellObject.VARIABLE, dlist);
                                List<EcellObject> rList = new List<EcellObject>();
                                rList.Add(obj);
                                m_dManager.DataAdd(rList);
                                if (m_currentObj.Children == null)
                                    m_currentObj.Children = new List<EcellObject>();
                                m_currentObj.Children.Add(obj);
                                sizeData = Convert.ToDouble(c.Text);
                            }
                        }
                        else
                        {
                            if (c.Text == "")
                            {
                                m_dManager.DataDelete(target.ModelID, target.Key, target.Type);
                                m_currentObj.Children.Remove(target);
                            }
                            else
                            {
                                bool isChange = false;
                                foreach (EcellData d in target.Value)
                                {
                                    if (d.EntityPath.EndsWith(":Value"))
                                    {
                                        if (d.Value.CastToDouble() == Convert.ToDouble(c.Text))
                                        {
                                        }
                                        else
                                        {
                                            isChange = true;
                                            target.Value.Remove(d);
                                            d.Value = new EcellValue(Convert.ToDouble(c.Text));
                                            target.Value.Add(d);
                                        }
                                        break;
                                    }
                                }
                                if (isChange)
                                {
                                    NotifyDataChanged(target.ModelID, target.Key, target);
                                }
                                m_currentObj.Children.Remove(target);
                                m_currentObj.Children.Add(target);
                                sizeData = Convert.ToDouble(c.Text);
                            }
                        }
                        EcellData data = new EcellData();
                        data.Name = "Size";
                        data.Value = new EcellValue(sizeData);
                        data.EntityPath = m_propDict[data.Name].EntityPath;
                        data.Settable = m_propDict[data.Name].Settable;
                        data.Saveable = m_propDict[data.Name].Saveable;
                        data.Loadable = m_propDict[data.Name].Loadable;
                        data.Gettable = m_propDict[data.Name].Gettable;
                        data.Logable = m_propDict[data.Name].Logable;
                        data.Logged = m_propDict[data.Name].Logged;
                        GetCommitInfo(data);

                        list.Add(data);
                    }
                    else
                    {
                        EcellData data = new EcellData();
                        data.Name = (string)c.Tag;
                        if (m_propDict[data.Name].Value.Type == typeof(int))
                            data.Value = new EcellValue(Convert.ToInt32(c.Text));
                        else if (m_propDict[data.Name].Value.Type == typeof(double))
                        {
                            if (c.Text == "1.79769313486232E+308")
                                data.Value = new EcellValue(Double.MaxValue);
                            else
                                data.Value = new EcellValue(Convert.ToDouble(c.Text));
                        }
                        else if (m_propDict[data.Name].Value.Type == typeof(List<EcellValue>))
                            data.Value = EcellValue.ToList(c.Text);
                        else
                            data.Value = new EcellValue(c.Text);

                        if (key.Contains(":"))
                        {
                            int ind = key.LastIndexOf(":");
                            data.EntityPath = type + ":" + key.Substring(0, ind) +
                                ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                        }
                        else
                        {
                            if (key == "/")
                            {
                                data.EntityPath = type + ":" + "" +
                                    ":" + "/" + ":" + (string)c.Tag;
                            }
                            else
                            {
                                int ind = key.LastIndexOf("/");
                                data.EntityPath = type + ":" + key.Substring(0, ind) +
                                    ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                            }
                        }

                        data.Settable = m_propDict[data.Name].Settable;
                        data.Saveable = m_propDict[data.Name].Saveable;
                        data.Loadable = m_propDict[data.Name].Loadable;
                        data.Gettable = m_propDict[data.Name].Gettable;
                        data.Logable = m_propDict[data.Name].Logable;
                        GetCommitInfo(data);

                        if (m_propDict[data.Name].Logged != isLogger)
                        {
                            if (m_propDict[data.Name].Logged)
                            {
                                // nothing
                            }
                            else
                            {
                                m_pManager.LoggerAdd(modelID, key, type,
                                    m_propDict[data.Name].EntityPath);
                            }
                        }
                        data.Logged = isLogger;

                        list.Add(data);
                    }
                }

                EcellObject uobj = EcellObject.CreateObject(modelID, key, type, classname, list);
                uobj.Children = m_currentObj.Children;
                uobj.X = m_currentObj.X;
                uobj.Y = m_currentObj.Y;
                uobj.OffsetX = m_currentObj.OffsetX;
                uobj.OffsetY = m_currentObj.OffsetY;
                uobj.Width = m_currentObj.Width;
                uobj.Height = m_currentObj.Height;
                uobj.LayerID = m_currentObj.LayerID;
                NotifyDataChanged(m_currentObj.ModelID, m_currentObj.Key, uobj);
            }
            catch (IgnoreException ex)
            {
                Trace.WriteLine(ex);
                return;
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                return;
            }

            this.Dispose();
        }

        #region Event
        /// <summary>
        /// The event sequence when user remove the property of object.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="e">EventArgs.</param>
        private void DeletePropertyForProcess(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b == null) return;

            string delKey = b.Tag as string;
            if (delKey == null) return;

            if (m_propDict.ContainsKey(delKey))
            {
                m_propDict.Remove(delKey);
            }
            LayoutNodePropertyEditor();
        }
        /// <summary>
        /// The event sequence when user add the property of object.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="e">EventArgs</param>
        private void AddPropertyForProcess(object sender, EventArgs e)
        {
            AddPropertyDialog dialog = new AddPropertyDialog();

            String name = dialog.ShowPropertyDialog();
            if (name == null) return;

            EcellData data;
            if (m_currentObj != null)
                data = new EcellData(name, new EcellValue(0.0),
                    "Process:" + m_currentObj.Key + ":" + name);
            else
                data = new EcellData(name, new EcellValue(0.0),
                    "Process:/dummy:" + name);

            data.Gettable = true;
            data.Loadable = true;
            data.Logable = false;
            data.Logged = false;
            data.Saveable = true;
            data.Settable = true;

            m_propDict.Add(name, data);

            Control cnt = null;
            int width = layoutPanel.Width;
            layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 5));
            layoutPanel.RowCount = m_propDict.Keys.Count + 5;

            try
            {
                IEnumerator iter = layoutPanel.Controls.GetEnumerator();
                while (iter.MoveNext())
                {
                    Control c = (Control)iter.Current;
                    if (c == null) continue;
                    TableLayoutPanelCellPosition pos =
                        layoutPanel.GetPositionFromControl(c);
                    if (pos.Column != 2) continue;
                    if (c.Tag.Equals("Add Property"))
                    {
                        layoutPanel.Controls.Remove(c);
                        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

                        CheckBox chk = new CheckBox();
                        chk.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                        chk.Text = "";
                        chk.AutoSize = true;
                        chk.Checked = false;
                        chk.Enabled = false;
                        layoutPanel.Controls.Add(chk, 0, pos.Row);

                        Label l = new Label();
                        l.Text = name;
                        l.Dock = DockStyle.Fill;
                        layoutPanel.Controls.Add(l, 1, pos.Row);

                        TextBox t = new TextBox();
                        t.Text = "";
                        t.Tag = name;
                        t.Dock = DockStyle.Fill;
                        t.Text = "0.0";
                        layoutPanel.Controls.Add(t, 2, pos.Row);

                        Button b = new Button();
                        b.Text = "Delete";
                        b.Tag = name;
                        b.Dock = DockStyle.Fill;
                        b.Click += new EventHandler(DeletePropertyForProcess);
                        layoutPanel.Controls.Add(b, 3, pos.Row);

                        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                        Button b1 = new Button();
                        b1.Text = "Add Property";
                        b1.Tag = "Add Property";
                        b1.Dock = DockStyle.Fill;
                        b1.Click += new EventHandler(AddPropertyForProcess);
                        layoutPanel.Controls.Add(b1, 2, pos.Row + 1);

                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            panel1.ClientSize = panel1.Size;
            this.ActiveControl = cnt;
        }
        /// <summary>
        /// The event sequence when user press return in text box.
        /// </summary>
        /// <param name="sender">object(TextBox)</param>
        /// <param name="e">KeyPresssEventArgs.</param>
        private void EnterKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PEApplyButton.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                PECloseButton.PerformClick();
            }
        }

        /// <summary>
        /// event of clicking the formulator button.
        /// show the window to edit the formulator.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void ShowFormulatorWindow(object sender, EventArgs e)
        {
            m_fwin = new FormulatorDialog();
            m_cnt = new FormulatorControl();
            m_fwin.tableLayoutPanel.Controls.Add(m_cnt, 0, 0);
            m_cnt.Dock = DockStyle.Fill;

            List<string> list = new List<string>();
            list.Add("self.getSuperSystem().SizeN_A");
            foreach (string str in m_propDict.Keys)
            {
                if (str != "modelID" && str != "key" && str != "type" &&
                    str != "classname" && str != EcellProcess.ACTIVITY &&
                    str != EcellProcess.EXPRESSION && str != EcellProcess.NAME &&
                    str != EcellProcess.PRIORITY && str != EcellProcess.STEPPERID &&
                    str != EcellProcess.VARIABLEREFERENCELIST && str != EcellProcess.ISCONTINUOUS)
                    list.Add(str);
            }
            List<EcellReference> tmpList = EcellReference.ConvertString(m_refStr);
            foreach (EcellReference r in tmpList)
            {
                list.Add(r.Name + ".MolarConc");
            }
            foreach (EcellReference r in tmpList)
            {
                list.Add(r.Name + ".Value");
            }
            m_cnt.AddReserveString(list);


            m_cnt.ImportFormulate(m_text.Text);

            m_fwin.FApplyButton.Click += new EventHandler(UpdateFormulator);
            m_fwin.FCloseButton.Click += new EventHandler(m_fwin.CancelButtonClick);

            m_fwin.ShowDialog();
        }

        /// <summary>
        /// event of clicking the OK button in formulator window.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void UpdateFormulator(object sender, EventArgs e)
        {
            m_text.Text = m_cnt.ExportFormulate();

            m_fwin.Close();
            m_fwin.Dispose();
        }

        /// <summary>
        /// The action of changing process type.
        /// </summary>
        /// <param name="sender">object(ComboBox)</param>
        /// <param name="e">EventArgs</param>
        private void ComboSelectedIndexChanged(object sender, EventArgs e)
        {
            string propName = ((ComboBox)sender).Text;
            m_propName = propName;
            if (m_type.Equals(EcellObject.PROCESS))
            {
                Dictionary<string, EcellData> tmpDict = new Dictionary<string, EcellData>();
                foreach (string id in m_propDict.Keys)
                {
                    tmpDict.Add(id, m_propDict[id]);
                }
                m_propDict = m_dManager.GetProcessProperty(m_propName);
                foreach (string id in tmpDict.Keys)
                {
                    if (!m_propDict.ContainsKey(id)) continue;
                    if (!m_propDict[id].Settable) continue;
                    m_propDict[id].Value = tmpDict[id].Value;
                }
            }

            LayoutPropertyEditor();
        }

        /// <summary>
        /// Show variable reference list window to edit variable reference.
        /// </summary>
        /// <param name="sender">Button(VariableReferenceList)</param>
        /// <param name="e">EventArgs</param>
        private void ShowVarRefWindow(object sender, EventArgs e)
        {
            m_win = new VariableReferenceEditDialog(m_dManager, m_pManager);
            m_win.AddVarButton.Click += new EventHandler(m_win.AddVarReference);
            m_win.DeleteVarButton.Click += new EventHandler(m_win.DeleteVarReference);
            m_win.VRCloseButton.Click += new EventHandler(m_win.CloseVarReference);
            m_win.VRApplyButton.Click += new EventHandler(m_win.OKVarReference);

            List<EcellReference> list = EcellReference.ConvertString(m_refStr);
            foreach (EcellReference v in list)
            {
                DataGridViewRow row = new DataGridViewRow();

                bool isAccessor = false;
                if (v.IsAccessor == 1) isAccessor = true;
                m_win.dgv.Rows.Add(new object[] { v.Name, v.FullID, v.Coefficient, isAccessor });
            }

            m_win.m_editor = this;
            m_win.ShowDialog();
        }

        private void PropertyEditorShown(object sender, EventArgs e)
        {
            if (m_idText != null)
            {
                m_idText.Focus();
            }
        }
        #endregion
        #endregion
    }
}
