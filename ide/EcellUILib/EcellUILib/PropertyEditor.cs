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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
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
using Ecell.UI.Components;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell.IDE
{
    public partial class PropertyEditor : Form
    {
        #region Fields
        /// <summary>
        /// current variable reference list.
        /// </summary>
        public List<EcellReference> m_refList = new List<EcellReference>();
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
        /// current selected object.
        /// </summary>
        private EcellObject m_currentObj;
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
        private String m_title = null;
        private Dictionary<string, TextBox> m_maxDic = new Dictionary<string, TextBox>();
        private Dictionary<string, TextBox> m_minDic = new Dictionary<string, TextBox>();
        private Dictionary<string, TextBox> m_stepDic = new Dictionary<string, TextBox>();
        private Dictionary<string, EcellParameterData> m_paramDic = new Dictionary<string, EcellParameterData>();
        static private string DefinedSize = "DefinedSize";
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
            if (obj == null) return;
            try
            {
                editor.SetCurrentObject(obj);
                if (editor.ShowDialog() == DialogResult.OK)
                {
                }
            }
            catch (IgnoreException ex)
            {
                ex.ToString();
                return;
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrSetProp,
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
        /// Set the object to current selected object.
        /// </summary>
        /// <param name="obj">EcellObject</param>
        private void SetCurrentObject(EcellObject obj)
        {
            m_currentObj = obj;
            m_type = obj.Type;
            if (m_currentObj.Type.Equals(EcellObject.PROCESS))
            {
                m_propName = m_currentObj.Classname;
                EcellProcess process = (EcellProcess)obj;
                m_refList = process.ReferenceList;
            }
            LayoutPropertyEditor();
        }

        /// <summary>
        /// Get the commit information of this data.
        /// </summary>
        /// <param name="d">data object.</param>
        private void GetCommitInfo(EcellData d)
        {
            bool isCheck = false;
            double max = 0.0, min = 0.0, step = 0.0;
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
                    max = Convert.ToDouble(t.Text);
                }
                else if (pos.Column == 3) // Min
                {
                    TextBox t = c as TextBox;
                    if (t == null) continue;
                    if (t.Text == null || t.Text == "") continue;
                    min = Convert.ToDouble(t.Text);
                }
                else if (pos.Column == 4) // Step
                {
                    TextBox t = c as TextBox;
                    if (t == null) continue;
                    if (t.Text == null || t.Text == "") continue;
                    step = Convert.ToDouble(t.Text);
                }
            }
            if (max < min)
            {
                throw new EcellException("Invalid parameter(MAX < Min).");
            }
            
            string fullPath = d.EntityPath;
            if (d.Name.Equals(Constants.xpathSize))
            {
                fullPath = Constants.xpathVariable + ":" + m_currentObj.Key + ":SIZE:Value";
            }
            if (!isCheck)            
                m_dManager.SetParameterData(new EcellParameterData(fullPath,
                    max, min, step));
            else
                m_dManager.RemoveParameterData(new EcellParameterData(fullPath, 0.0));
        }

        /// <summary>
        /// layout the column of property editor according to data type.
        /// </summary>
        private void LayoutPropertyEditor()
        {
            layoutPanel.SuspendLayout();
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
            layoutPanel.ResumeLayout();
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
            m_paramDic.Clear();

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

                EcellParameterData param = m_dManager.GetParameterData(m_propDict[key].EntityPath);
                if (param == null)
                {
                    param = new EcellParameterData(key, (double)m_propDict[key].Value);
                }
                CheckBox c = new CheckBox();
                commitLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                if (m_propDict[key].Settable &&
                    m_propDict[key].Value.Type == EcellValueType.Double)
                {
                    if (m_dManager.IsContainsParameterData(m_propDict[key].EntityPath))
                        c.Checked = false;
                    else 
                        c.Checked = true;
                    c.Enabled = true;
                    c.CheckedChanged += new EventHandler(c_CheckedChanged);
                }
                else
                {
                    c.Checked = true;
                    c.Enabled = false;
                }
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                c.Text = "";
                c.Tag = key;
                c.AutoSize = true;
                commitLayoutPanel.Controls.Add(c, 0, i);

                Label l = new Label();
                l.Text = key;
                l.Dock = DockStyle.Fill;
                commitLayoutPanel.Controls.Add(l, 1, i);

                TextBox t1 = new TextBox();
                t1.Dock = DockStyle.Fill;
                t1.Tag = key;
                if (!m_propDict[key].Settable ||
                    m_propDict[key].Value.Type != EcellValueType.Double)
                {
                    t1.ReadOnly = true;
                    t1.Text = param.Max.ToString();
                }
                else
                {
                    if (param.Max == 0.0)
                    {
                        double d = (double)m_propDict[key].Value;
                        if (d >= 0.0)
                            t1.Text = Convert.ToString(d * 1.5);
                        else
                            t1.Text = Convert.ToString(d * 0.5);
                        param.Max = Convert.ToDouble(t1.Text);
                    }
                    else
                    {
                        t1.Text = param.Max.ToString();
                    }
                }
                t1.Validating += new CancelEventHandler(MaxDataValidating);
                commitLayoutPanel.Controls.Add(t1, 2, i);

                TextBox t2 = new TextBox();
                t2.Dock = DockStyle.Fill;
                t2.Tag = key;
                if (!m_propDict[key].Settable ||
                    m_propDict[key].Value.Type != EcellValueType.Double)
                {
                    t2.ReadOnly = true;
                    t2.Text = param.Min.ToString();
                }
                else
                {
                    if (param.Min == 0.0)
                    {
                        double d = (double)m_propDict[key].Value;
                        if (d >= 0.0)
                            t2.Text = Convert.ToString(d * 0.5);
                        else
                            t2.Text = Convert.ToString(d * 1.5);
                        param.Min = Convert.ToDouble(t2.Text);
                    }
                    else
                    {
                        t2.Text = param.Min.ToString();
                    }
                }
                t2.Validating += new CancelEventHandler(MinDataValidating);
                commitLayoutPanel.Controls.Add(t2, 3, i);

                TextBox t3 = new TextBox();
                t3.Text = param.Step.ToString();
                t3.Dock = DockStyle.Fill;
                t3.Tag = key;
                if (!m_propDict[key].Settable ||
                    m_propDict[key].Value.Type != EcellValueType.Double)
                {
                    t3.ReadOnly = true;
                }
                t3.Validating += new CancelEventHandler(StepDataValidating);
                commitLayoutPanel.Controls.Add(t3, 4, i);
                i++;

                t1.ReadOnly = c.Checked;
                t2.ReadOnly = c.Checked;
                t3.ReadOnly = c.Checked;

                m_maxDic[key] = t1;
                m_minDic[key] = t2;
                m_stepDic[key] = t3;
                m_paramDic.Add(key, param);
            }

            if (m_type.Equals(EcellObject.SYSTEM))
            {
                EcellObject sizeObj = null;
                foreach (EcellObject obj in m_currentObj.Children)
                {
                    if (!obj.LocalID.Equals("SIZE")) continue;
                    sizeObj = obj;
                    break;                    
                }
                if (sizeObj == null)
                {
                    CheckBox c = new CheckBox();
                    c.Checked = true;
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    c.Text = "";
                    c.AutoSize = true;
                    c.Enabled = true;
                    c.Tag = "Size";
                    c.CheckedChanged += new EventHandler(c_CheckedChanged);
                    commitLayoutPanel.Controls.Add(c, 0, i);

                    commitLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                    Label l = new Label();
                    l.Text = "Size";
                    l.Dock = DockStyle.Fill;
                    commitLayoutPanel.Controls.Add(l, 1, i);

                    TextBox t1 = new TextBox();
                    t1.Text = "0.15";
                    t1.Dock = DockStyle.Fill;
                    t1.ReadOnly = true;
                    t1.Tag = "Size";
                    commitLayoutPanel.Controls.Add(t1, 2, i);

                    TextBox t2 = new TextBox();
                    t2.Text = "0.05";
                    t2.Dock = DockStyle.Fill;
                    t2.ReadOnly = true;
                    t2.Tag = "Size";
                    commitLayoutPanel.Controls.Add(t2, 3, i);

                    TextBox t3 = new TextBox();
                    t3.Text = "0";
                    t3.Dock = DockStyle.Fill;
                    t3.ReadOnly = true;
                    t3.Tag = "Size";
                    commitLayoutPanel.Controls.Add(t3, 4, i);
                    i++;

                    m_maxDic["Size"] = t1;
                    m_minDic["Size"] = t2;
                    m_stepDic["Size"] = t3;

                    t1.ReadOnly = c.Checked;
                    t2.ReadOnly = c.Checked;
                    t3.ReadOnly = c.Checked;
                }
                else 
                {
                    EcellParameterData param = null;
                    string fullPath = Constants.xpathVariable + ":" + m_currentObj.Key + ":SIZE:Value";
                    param = m_dManager.GetParameterData(fullPath);

                    commitLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

                    CheckBox c = new CheckBox();
                    c.Checked = (param == null);
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    c.Text = "";
                    c.Tag = "Size";
                    c.AutoSize = true;
                    c.Enabled = true;
                    c.CheckedChanged += new EventHandler(c_CheckedChanged);
                    commitLayoutPanel.Controls.Add(c, 0, i);

                    Label l = new Label();
                    l.Text = "Size";
                    l.Dock = DockStyle.Fill;
                    commitLayoutPanel.Controls.Add(l, 1, i);

                    TextBox t1 = new TextBox();
                    if (param == null)
                        t1.Text = "";
                    else
                        t1.Text = param.Max.ToString();
                    t1.Dock = DockStyle.Fill;
                    t1.Tag = "Size";
                    commitLayoutPanel.Controls.Add(t1, 2, i);

                    TextBox t2 = new TextBox();
                    if (param == null)
                        t2.Text = "";
                    else
                        t2.Text = param.Min.ToString();
                    t2.Dock = DockStyle.Fill;
                    t2.Tag = "Size";
                    commitLayoutPanel.Controls.Add(t2, 3, i);

                    TextBox t3 = new TextBox();
                    if (param == null)
                        t3.Text = "";
                    else
                        t3.Text = param.Step.ToString();
                    t3.Dock = DockStyle.Fill;
                    t3.Tag = "Size";
                    commitLayoutPanel.Controls.Add(t3, 4, i);

                    m_maxDic["Size"] = t1;
                    m_minDic["Size"] = t2;
                    m_stepDic["Size"] = t3;
                    t1.ReadOnly = c.Checked;
                    t2.ReadOnly = c.Checked;
                    t3.ReadOnly = c.Checked;

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
                                            pvalue = new EcellParameterData(d.EntityPath,(double)d.Value);
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
            }

            panel2.ClientSize = panel2.Size;
        }

        void c_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            if (c.Checked)
            {
                m_maxDic[(string)c.Tag].ReadOnly = true;
                m_minDic[(string)c.Tag].ReadOnly = true;
                m_stepDic[(string)c.Tag].ReadOnly = true;
            }
            else
            {
                m_maxDic[(string)c.Tag].ReadOnly = false;
                m_minDic[(string)c.Tag].ReadOnly = false;
                m_stepDic[(string)c.Tag].ReadOnly = false;
            }
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
            l.Text = "ModelID";
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

        private void IntInputTextValidating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
            string propName = (string)(textBox.Tag);
            
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSet, propName));
                textBox.Text = m_propDict[propName].Value.ToString();
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                textBox.Text = m_propDict[propName].Value.ToString();
                e.Cancel = true;
                return;
            }          
        }

        private void PathIDInputTextValidating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;

            if (m_currentObj.Type != EcellObject.SYSTEM &&
                m_currentObj.Type != EcellObject.PROCESS &&
                m_currentObj.Type != EcellObject.VARIABLE)
                return;

            string parentSystemId = Util.GetSuperSystemPath(text);
            EcellObject sysObj = m_dManager.CurrentProject.GetSystem(m_currentObj.ModelID, parentSystemId);
            if (!text.Equals("/") && sysObj == null)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSystem, parentSystemId));
                e.Cancel = true;
                textBox.Text = m_currentObj.Key;
                return;
            }
        }

        private void DoubleInputTextValidating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
            string propName = (string)(textBox.Tag);
            string preData = "";

            if (propName.Equals(PropertyEditor.DefinedSize))
                preData = ((EcellSystem)m_currentObj).SizeInVolume.ToString();
            else
                preData = m_propDict[propName].Value.ToString();
            
            if (string.IsNullOrEmpty(text))
            {
                if (propName.Equals(PropertyEditor.DefinedSize))
                    return;
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSet, propName));
                textBox.Text = preData;
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                textBox.Text = preData;
                e.Cancel = true;
                return;
            }
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
                layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 6));
                layoutPanel.RowCount = m_propDict.Keys.Count + 6;
            }
            else if (m_currentObj != null && m_currentObj.Type.Equals(EcellObject.SYSTEM))
            {
                layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 6));
                layoutPanel.RowCount = m_propDict.Keys.Count + 6;
            }
            else
            {
                layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 6));
                layoutPanel.RowCount = m_propDict.Keys.Count + 6;
            }

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l1 = new Label();
            l1.Text = "ModelID";
            l1.Anchor = AnchorStyles.Left;
            layoutPanel.Controls.Add(l1, 1, i);
            TextBox t1 = new TextBox();
            t1.Tag = "modelID";
            t1.Dock = DockStyle.Fill;
            t1.KeyPress += new KeyPressEventHandler(EnterKeyPress);
            t1.Text = m_currentObj.ModelID;
            t1.ReadOnly = true;
            layoutPanel.Controls.Add(t1, 2, i);
            i++;

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l2 = new Label();
            l2.Text = "Path:ID";
            l2.Anchor = AnchorStyles.Left;
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
            t2.Validating += new CancelEventHandler(PathIDInputTextValidating);
            m_idText = t2;
            t2.Dock = DockStyle.Fill;
            layoutPanel.Controls.Add(t2, 2, i);
            i++;

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label l3 = new Label();
            l3.Text = "Classname";
            l3.Anchor = AnchorStyles.Left;
            layoutPanel.Controls.Add(l3, 1, i);
            ComboBox combo = new ComboBox();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            int j = 0;
            if (m_type.Equals(EcellObject.PROCESS))
            {
                List<string> list = m_dManager.CurrentProject.ProcessDmList;
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
            l4.Text = "Type";
            l4.Anchor = AnchorStyles.Left;
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
                    c.Text = "";
                    c.AutoSize = true;
                    c.Enabled = true;
                    layoutPanel.Controls.Add(c, 0, i);
                }


                Label l = new Label();
                l.Text = key;
                l.Anchor = AnchorStyles.Left;
                l.AutoSize = true;
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
                    t.DropDownStyle = ComboBoxStyle.DropDownList;
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
                    else
                    {
                        if (m_propDict[key].Value.IsDouble)
                        {
                            t.Validating += new CancelEventHandler(DoubleInputTextValidating);
                        }
                        else if (m_propDict[key].Value.IsInt)
                        {
                            t.Validating += new CancelEventHandler(IntInputTextValidating);
                        }
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
                        b.Text = MessageResources.ButtonDeleteProperty;
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
                b.Text = MessageResources.ButtonAddProperty;
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
                l.Anchor = AnchorStyles.Left;
                layoutPanel.Controls.Add(l, 1, i);

                TextBox t = new TextBox();
                t.Text = "1";
                t.Tag = PropertyEditor.DefinedSize;
                t.Dock = DockStyle.Fill;
                t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                t.Validating += new CancelEventHandler(DoubleInputTextValidating);
                layoutPanel.Controls.Add(t, 2, i);
            }
            else if (m_currentObj != null && m_currentObj.Type.Equals(EcellObject.SYSTEM))
            {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l = new Label();
                l.Text = "Size";
                l.Anchor = AnchorStyles.Left;
                layoutPanel.Controls.Add(l, 1, i);

                TextBox t = new TextBox();
                t.Text = ((EcellSystem)m_currentObj).SizeInVolume.ToString();
                t.Tag = PropertyEditor.DefinedSize;
                t.Dock = DockStyle.Fill;
                t.KeyPress += new KeyPressEventHandler(EnterKeyPress);
                t.Validating += new CancelEventHandler(DoubleInputTextValidating);
                layoutPanel.Controls.Add(t, 2, i);
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

        /// <summary>
        /// Update property of the selected TreeNode.
        /// </summary>
        private void PropertyEditorFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK) return;
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
                    // Property
                    if ((string)c.Tag == "Add Property")
                    {
                        continue;
                    }
                    // ModelID
                    if ((string)c.Tag == "modelID")
                    {
                        modelID = c.Text;
                        isLogger = false;
                    }
                    // ID
                    else if ((string)c.Tag == "id")
                    {
                        key = c.Text;
                        if (c.Text == "")
                        {
                            Util.ShowWarningDialog(String.Format(MessageResources.ErrNoSet,
                                new object[] { "ID" }));
                            e.Cancel = true;
                            return;
                        }
                        else if (Util.IsReservedID(c.Text))
                        {
                            Util.ShowWarningDialog(String.Format(MessageResources.ErrReserved,
                                new object[] { c.Text }));
                            e.Cancel = true;
                            return;
                        }
                        else if (m_currentObj.Type.Equals(EcellObject.SYSTEM) &&
                            Util.IsNGforSystemFullID(c.Text))
                        {
                            Util.ShowWarningDialog(MessageResources.ErrInvalidID);
                            e.Cancel = true;
                            return;
                        }
                        else if (!m_currentObj.Type.Equals(EcellObject.SYSTEM) &&
                            Util.IsNGforComponentFullID(c.Text))
                        {
                            Util.ShowWarningDialog(MessageResources.ErrInvalidID);
                            e.Cancel = true;
                            return;
                        }
                        else if (m_currentObj.Type.Equals(EcellObject.PROCESS) ||
                                    m_currentObj.Type.Equals(EcellObject.VARIABLE))
                        {
                            int kpos = c.Text.IndexOf(':');
                            if (kpos < 0 || kpos == c.Text.Length - 1)
                            {
                                Util.ShowWarningDialog(MessageResources.ErrInvalidID);
                                e.Cancel = true;
                                return;
                            }
                        }
                        isLogger = false;
                    }
                    // Classname
                    else if ((string)c.Tag == "classname")
                    {
                        isLogger = false;
                        classname = c.Text;
                    }
                    // Type
                    else if ((string)c.Tag == "type")
                    {
                        isLogger = false;
                        type = c.Text;
                    }
                    // VariableReference
                    else if ((string)c.Tag == EcellProcess.VARIABLEREFERENCELIST)
                    {
                        EcellData data = new EcellData();
                        data.Name = (string)c.Tag;
                        data.Value = EcellReference.ConvertToEcellValue(m_refList);
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
                        isLogger = false;
                        list.Add(data);
                    }
                    // System size.
                    else if ((string)c.Tag == PropertyEditor.DefinedSize)
                    {
                        double sizeData = Convert.ToDouble(c.Text);
                        EcellSystem system = (EcellSystem)m_currentObj;
                        system.SizeInVolume = sizeData;

                        EcellData data = (EcellData)m_propDict["Size"].Clone();
                        data.Value = new EcellValue(sizeData);
                        GetCommitInfo(data);
                        list.Add(data);
                    }
                    // Property 
                    else
                    {
                        EcellData data = new EcellData();
                        data.Name = (string)c.Tag;
                        if (m_propDict[data.Name].Value.Type == EcellValueType.Integer)
                            data.Value = new EcellValue(Convert.ToInt32(c.Text));
                        else if (m_propDict[data.Name].Value.Type == EcellValueType.Double)
                        {
                            if (c.Text == "1.79769313486232E+308")
                                data.Value = new EcellValue(Double.MaxValue);
                            else
                                data.Value = new EcellValue(Convert.ToDouble(c.Text));
                        }
                        else if (m_propDict[data.Name].Value.Type == EcellValueType.List)
                            data.Value = new EcellValue(c.Text);
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
                        data.Logged = isLogger;
                        if (isLogger != m_propDict[data.Name].Logged && isLogger)
                            m_pManager.LoggerAdd(modelID, m_currentObj.Key, type, m_propDict[data.Name].EntityPath);
                        isLogger = false;

                        list.Add(data);
                    }
                }

                EcellObject uobj = EcellObject.CreateObject(modelID, key, type, classname, list);
                uobj.Children = m_currentObj.Children;
                uobj.SetPosition(m_currentObj);
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
                e.Cancel = true;
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
            LayoutNodeCommit();
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
            if (String.IsNullOrEmpty(name)) return;

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

            if (m_propDict.ContainsKey(name))
            {
                Util.ShowErrorDialog(MessageResources.ErrAlreadyExist);
                return;
            }
            m_propDict.Add(name, data);

            Control cnt = null;
            int width = layoutPanel.Width;
            layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 6));
            layoutPanel.RowCount = m_propDict.Keys.Count + 6;

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
                        b.Text = MessageResources.ButtonDeleteProperty;
                        b.Tag = name;
                        b.Dock = DockStyle.Fill;
                        b.Click += new EventHandler(DeletePropertyForProcess);
                        layoutPanel.Controls.Add(b, 3, pos.Row);

                        layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                        Button b1 = new Button();
                        b1.Text = MessageResources.ButtonAddProperty;
                        b1.Tag = "Add Property";
                        b1.Dock = DockStyle.Fill;
                        b1.Click += new EventHandler(AddPropertyForProcess);
                        layoutPanel.Controls.Add(b1, 2, pos.Row + 1);

                        break;
                    }
                }

            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrAddProperty);
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
            foreach (EcellReference r in m_refList)
            {
                list.Add(r.Name + ".MolarConc");
            }
            foreach (EcellReference r in m_refList)
            {
                list.Add(r.Name + ".Value");
            }
            m_fwin.AddReserveString(list);
            m_fwin.ImportFormulate(m_text.Text);


            using (m_fwin)
            {
                DialogResult res = m_fwin.ShowDialog();
                if (res == DialogResult.OK)
                {
                    m_text.Text = m_fwin.ExportFormulate();
                }
            }
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
            VariableReferenceEditDialog win = new VariableReferenceEditDialog(m_dManager, m_pManager, m_refList);
            using (win)
            {
                DialogResult res = win.ShowDialog();
                if (res == DialogResult.OK)
                {
                    m_refList = win.ReferenceList;

                }
            }
        }

        private void PropertyEditorShown(object sender, EventArgs e)
        {
            if (m_idText != null)
            {
                m_idText.Focus();
            }
        }

        private void MaxDataValidating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
            string propName = (string)(textBox.Tag);

            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSet, propName));
                textBox.Text = Convert.ToString(m_paramDic[propName].Max);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy) || dummy < m_paramDic[propName].Min)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                textBox.Text = Convert.ToString(m_paramDic[propName].Max);
                e.Cancel = true;
                return;
            }
            m_paramDic[propName].Max = dummy;
        }

        private void MinDataValidating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
            string propName = (string)(textBox.Tag);

            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSet, propName));
                textBox.Text = Convert.ToString(m_paramDic[propName].Min);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy) || dummy > m_paramDic[propName].Max)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                textBox.Text = Convert.ToString(m_paramDic[propName].Min);
                e.Cancel = true;
                return;
            }
            m_paramDic[propName].Min = dummy;
        }

        private void StepDataValidating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
            string propName = (string)(textBox.Tag);

            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoSet, propName));
                textBox.Text = Convert.ToString(m_paramDic[propName].Step);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy) || dummy < 0.0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                textBox.Text = Convert.ToString(m_paramDic[propName].Step);
                e.Cancel = true;
                return;
            }
            m_paramDic[propName].Step = dummy;
        }

        #endregion

        #endregion
    }
}
