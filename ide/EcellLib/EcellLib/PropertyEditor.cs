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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Formulator;

namespace EcellLib
{
    public partial class PropertyEditor : Form
    {
        #region Fields
        /// <summary>
        /// current variable reference list string.
        /// </summary>
        public string m_refStr;
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
        private Dictionary<string, EcellData> m_propDict = new Dictionary<string,EcellData>();
        /// <summary>
        /// m_win (editable variable reference list window)
        /// </summary>
        VariableRefWindow m_win;
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
        /// text box of expression.
        /// </summary>
        public TextBox m_text;
        /// <summary>
        /// window to edit formulator of expression at process.
        /// </summary>
        private FormulatorWindow m_fwin = null;
        /// <summary>
        /// user control of formulator.
        /// </summary>
        private FormulatorControl m_cnt = null;
        #endregion

        /// <summary>
        /// Constructor for PropertyEditor.
        /// </summary>
        public PropertyEditor()
        {
            InitializeComponent();
            m_dManager = DataManager.GetDataManager();
        }

        /// <summary>
        /// Set the object to parent object.
        /// </summary>
        /// <param name="obj">the parent object to add.</param>
        public void SetParentObject(EcellObject obj)
        {
            m_parentObj = obj;
            m_currentObj = null;
        }

        /// <summary>
        /// Set the object to current selected object.
        /// </summary>
        /// <param name="obj">EcellObject</param>
        public void SetCurrentObject(EcellObject obj)
        {
            m_currentObj = obj;
            m_parentObj = null;
            if (m_currentObj.type.Equals("Process"))
                m_propName = m_currentObj.classname;
        }

        /// <summary>
        /// Set data type displayed in PropertyEditor.
        /// </summary>
        /// <param name="type">data type of object.</param>
        public void SetDataType(string type)
        {
            /*
            if (type.Equals("Model"))
            {
                if (m_currentObj == null) button1.Click += new EventHandler(this.AddModel);
                else button1.Click += new EventHandler(this.UpdateProperty);
            }
            else if (type.Equals("Process"))
            {
                if (m_currentObj == null) button1.Click += new EventHandler(this.AddNodeElement);
                else button1.Click += new EventHandler(this.UpdateProperty);
            }
            else if (type.Equals("System"))
            {
                if (m_currentObj== null) button1.Click += new EventHandler(this.AddSystem);
                else button1.Click += new EventHandler(this.UpdateProperty);
            }
            else if (type.Equals("Variable"))
            {
                if (m_currentObj == null) button1.Click += new EventHandler(this.AddNodeElement);
                else button1.Click += new EventHandler(this.UpdateProperty);
            }
             */
            button2.Click += new EventHandler(this.AddCancel);

            m_type = type;
        }

        /// <summary>
        /// layout the column of property editor according to data type.
        /// </summary>
        public void LayoutPropertyEditor()
        {
            if (m_type.Equals("Model")) LayoutModelPropertyEditor();
            else if (m_type.Equals("System")) LayoutNodePropertyEditor();
            else if (m_type.Equals("Variable")) LayoutNodePropertyEditor();
            else if (m_type.Equals("Process")) LayoutNodePropertyEditor();
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
            layoutPanel.Controls.Add(l, 0, 0);
            TextBox t = new TextBox();
            if (m_currentObj == null) t.Text = "";
            else
            {
                t.Text = m_currentObj.modelID;
                t.ReadOnly = true;
            }
            t.Dock = DockStyle.Fill;
            t.Tag = "modelID";
            layoutPanel.Controls.Add(t, 1, 0);
            layoutPanel.ResumeLayout(false);
        }


        /// <summary>
        /// layout the column of property editor for System, Process and Variable.
        /// </summary>
        private void LayoutNodePropertyEditor()
        {
            if (m_currentObj == null)
            {
                if (m_type.Equals("Process"))
                    m_propDict = DataManager.GetProcessProperty(m_propName);
                else if (m_type.Equals("System"))
                    m_propDict = DataManager.GetSystemProperty();
                else if (m_type.Equals("Variable"))
                    m_propDict = DataManager.GetVariableProperty();
            }
            else
            {
                if (m_propDict.Count == 0)
                {
                    m_propDict.Clear();
                    foreach (EcellData d in m_currentObj.M_value)
                        m_propDict.Add(d.M_name, d);
                }
            }

            int i = 0;
            int width = layoutPanel.Width;
            layoutPanel.Controls.Clear();
            layoutPanel.RowStyles.Clear();
            layoutPanel.Size = new Size(width, 30 * (m_propDict.Keys.Count + 4));
//            layoutPanel.SuspendLayout();
            layoutPanel.RowCount = m_propDict.Keys.Count + 4;

            try
            {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l1 = new Label();
                l1.Text = "modelID";
                l1.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(l1, 0, i);
                TextBox t1 = new TextBox();
                t1.Tag = "modelID";
                t1.Dock = DockStyle.Fill;
                if (m_currentObj != null) t1.Text = m_currentObj.modelID;
                else t1.Text = m_parentObj.modelID;
                t1.ReadOnly = true;
                layoutPanel.Controls.Add(t1, 1, i);
                i++;

                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l2 = new Label();
                l2.Text = "id";
                l2.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(l2, 0, i);
                TextBox t2 = new TextBox();
                t2.Tag = "id";
                if (m_currentObj == null) t2.Text = "";
                else 
                {
                    t2.ReadOnly = true;
                    t2.Text = m_currentObj.key;
                }
                t2.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(t2, 1, i);
                i++;

                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l3 = new Label();
                l3.Text = "classname";
                l3.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(l3, 0, i);
                ComboBox combo = new ComboBox();
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                int j = 0;
                if (m_type.Equals("Process"))
                {
                    List<string> list = m_dManager.GetProcessList();
                    foreach (string str in list)
                    {
                        combo.Items.AddRange(new object[] { str });
                        if (str == m_propName) combo.SelectedIndex = j;
                        j++;
                    }
                    combo.SelectedIndexChanged += new EventHandler(ComboSelectedIndexChanged);
                }
                else if (m_type.Equals("System"))
                {
                    combo.Items.AddRange(new object[] { "System" });
                    combo.SelectedIndex = j;
                }
                else if (m_type.Equals("Variable"))
                {
                    combo.Items.AddRange(new object[] { "Variable" });
                    combo.SelectedIndex = j;
                }
                combo.Tag = "classname";
                combo.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(combo, 1, i);
                i++;

                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                Label l4 = new Label();
                l4.Text = "type";
                l4.Dock = DockStyle.Fill;
                layoutPanel.Controls.Add(l4, 0, i);
                TextBox t4 = new TextBox();
                t4.Text = "";
                t4.Tag = "type";
                t4.Dock = DockStyle.Fill;
                t4.Text = m_type;
                t4.ReadOnly = true;
                layoutPanel.Controls.Add(t4, 1, i);
                i++;

                foreach (string key in m_propDict.Keys)
                {
                    layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                    Label l = new Label();
                    l.Text = key;
                    l.Dock = DockStyle.Fill;
                    layoutPanel.Controls.Add(l, 0, i);

                    if (key == "VariableReferenceList")
                    {
                        Button b = new Button();
                        b.Text = "ReferenceList";
                        b.Tag = "VariableReferenceList";
                        b.Dock = DockStyle.Fill;
                        b.Click += new EventHandler(ShowVarRefWindow);
                        m_refStr = m_propDict[key].M_value.ToString();
                        layoutPanel.Controls.Add(b, 1, i);
                    }
                    else if (key == "StepperID")
                    {
                        TextBox t = new TextBox();
                        if (m_currentObj == null)
                        {
                            List<EcellObject> slist;
                            slist = m_dManager.GetStepper(null, m_parentObj.modelID);
                            foreach (EcellObject obj in slist)
                            {
                                t.Text = obj.key;
                                break;
                            }
                        }
                        else
                        {
                            t.Text = m_propDict[key].M_value.ToString();
                        }
                        t.Tag = key;
                        t.Dock = DockStyle.Fill;
                        layoutPanel.Controls.Add(t, 1, i);
                    }
                    else
                    {
                        TextBox t = new TextBox();
                        t.Text = "";
                        t.Tag = key;
                        t.Dock = DockStyle.Fill;
                        t.Text = m_propDict[key].M_value.ToString();
                        if (!m_propDict[key].M_isSettable)
                        {
                            t.ReadOnly = true;
                        }
                        layoutPanel.Controls.Add(t, 1, i);

                        if ((key == "Expression"))
                        {
                            Button b = new Button();
                            b.Text = "Formulator";
                            b.Tag = "Formulator";
                            b.Dock = DockStyle.Fill;
                            b.Click += new EventHandler(ShowFormulatorWindow);
                            m_text = t;
                            layoutPanel.Controls.Add(b, 2, i);
                        }
                    }
                    i++;
                }
//                layoutPanel.ResumeLayout(false);
                panel1.ClientSize = panel1.Size;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to layout process list window.\n" +
                    "Please close this window.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Event
        /// <summary>
        /// event of clicking the formulator button.
        /// show the window to edit the formulator.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void ShowFormulatorWindow(object sender, EventArgs e)
        {
            m_fwin = new FormulatorWindow();
            m_cnt = new FormulatorControl();
            m_fwin.tableLayoutPanel.Controls.Add(m_cnt, 0, 0);
            m_cnt.Dock = DockStyle.Fill;

            List<string> list = new List<string>();
            list.Add("self.getSuperSystem().SizeN_A");
            foreach (string str in m_propDict.Keys)
            {
                if (str != "modelID" && str != "key" && str != "type" &&
                    str != "classname" && str != "Activity" &&
                    str != "Expression" && str != "Name" &&
                    str != "Priority" && str != "StepperID" &&
                    str != "VariableReferenceList" && str!="IsContinuous")
                    list.Add(str);
            }
            List<EcellReference> tmpList = EcellReference.ConvertString(m_refStr);
            foreach (EcellReference r in tmpList)
            {
                list.Add(r.name + ".MolarConc");
            }
            m_cnt.AddReserveString(list);


            m_cnt.ImportFormulate(m_text.Text);

            m_fwin.OKButton.Click += new EventHandler(UpdateFormulator);
            m_fwin.CButton.Click += new EventHandler(m_fwin.CancelButtonClick);

            m_fwin.ShowDialog();
        }

        /// <summary>
        /// event of clicking the OK button in formulator window.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void UpdateFormulator(object sender, EventArgs e)
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
        public void ComboSelectedIndexChanged(object sender, EventArgs e)
        {
            string propName = ((ComboBox)sender).Text;
            m_propName = propName;
            if (m_type.Equals("Process"))
            {
                m_propDict = DataManager.GetProcessProperty(m_propName);
            }

            LayoutPropertyEditor();
        }

        /// <summary>
        /// The action of clicking cancel button in AddForm.
        /// </summary>
        /// <param name="sender">Cancel Button</param>
        /// <param name="e">EventArgs</param>
        public void AddCancel(object sender, EventArgs e)
        {
            Dispose();
        }

        public EcellObject Collect()
        {
            string id = "";
            string modelID = "";
            string key = "";
            string classname = "";
            string type = "";
            List<EcellData> list = new List<EcellData>();

            try
            {
                IEnumerator iter = layoutPanel.Controls.GetEnumerator();
                while (iter.MoveNext())
                {
                    Control c = (Control)iter.Current;
                    if (c == null) continue;
                    TableLayoutPanelCellPosition pos =
                        layoutPanel.GetPositionFromControl(c);
                    if (pos.Column != 1) continue;

                    if ((string)c.Tag == "modelID") modelID = c.Text;
                    else if ((string)c.Tag == "id")
                    {
                        id = c.Text;
                        if (c.Text == "")
                        {
                            MessageBox.Show("Id is null. please input id.\n",
                                            "WARNING",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                            return null;
                        }
                        else if (Util.IsNG(c.Text))
//                        else if (c.Text.Contains("/") || c.Text.Contains(":"))
                        {
                            MessageBox.Show("Id contains invalid character.\n",
                                            "WARNING", 
                                            MessageBoxButtons.OK, 
                                            MessageBoxIcon.Warning);
                            return null;
                        }
                        else if (c.Text.ToUpper() == "SIZE")
                        {
                            MessageBox.Show("SIZE is the reserved name.\n",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return null;
                        }

                        if (!m_type.Equals("System"))
                        {
                            if (m_parentObj.key == "") key = c.Text;
                            else if (m_parentObj.key == "/") key = "/:" + c.Text;
                            else key = m_parentObj.key + ":" + c.Text;
                        }
                        else
                        {
                            if (m_parentObj.key == "") key = c.Text;
                            else if (m_parentObj.key == "/") key = "/" + c.Text;
                            else key = m_parentObj.key + "/" + c.Text;

                        }
                    }
                    else if ((string)c.Tag == "classname") classname = c.Text;
                    else if ((string)c.Tag == "type") type = c.Text;
                    else if ((string)c.Tag == "VariableReferenceList")
                    {
                        EcellData data = new EcellData();
                        data.M_name = (string)c.Tag;
                        data.M_value = EcellValue.ToVariableReferenceList(m_refStr);
                        data.M_entityPath = type + ":" + m_parentObj.key +
                            ":" + id + ":" + (string)c.Tag;
                        data.M_isSettable = m_propDict[data.M_name].M_isSettable;
                        data.M_isSavable = m_propDict[data.M_name].M_isSavable;
                        data.M_isLoadable = m_propDict[data.M_name].M_isLoadable;
                        data.M_isGettable = m_propDict[data.M_name].M_isGettable;
                        data.M_isLogable = m_propDict[data.M_name].M_isLogable;
                        data.M_isLogger = m_propDict[data.M_name].M_isLogger;

                        list.Add(data);
                    }
                    else
                    {
                        EcellData data = new EcellData();
                        try
                        {
                            data.M_name = (string)c.Tag;
                            if (m_propDict[data.M_name].M_value.M_type == typeof(int))
                                data.M_value = new EcellValue(Convert.ToInt32(c.Text));
                            else if (m_propDict[data.M_name].M_value.M_type == typeof(double))
                            {
                                if (c.Text == "1.79769313486232E+308")
                                    data.M_value = new EcellValue(Double.MaxValue);
                                else
                                    data.M_value = new EcellValue(Convert.ToDouble(c.Text));
                            }
                            else if (m_propDict[data.M_name].M_value.M_type == typeof(List<EcellValue>))
                                data.M_value = EcellValue.ToList(c.Text);
                            else
                                data.M_value = new EcellValue(c.Text);
                            data.M_entityPath = type + ":" + m_parentObj.key +
                                ":" + id + ":" + (string)c.Tag;
                            data.M_isSettable = m_propDict[data.M_name].M_isSettable;
                            data.M_isSavable = m_propDict[data.M_name].M_isSavable;
                            data.M_isLoadable = m_propDict[data.M_name].M_isLoadable;
                            data.M_isGettable = m_propDict[data.M_name].M_isGettable;
                            data.M_isLogable = m_propDict[data.M_name].M_isLogable;
                            data.M_isLogger = m_propDict[data.M_name].M_isLogger;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                        list.Add(data);
                    }
                }

                EcellObject obj = EcellObject.CreateObject(modelID, key, type, classname, list);
                obj.M_instances = new List<EcellObject>();

                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The action of clicking ok button in AddForm with System. 
        /// </summary>
        /// <param name="sender">OK Button</param>
        /// <param name="e">EventArgs</param>
        public void AddSystem(object sender, EventArgs e)
        {
            string id = "";
            string modelID = "";
            string key = "";
            string classname = "";
            string type = "";
            List<EcellData> list = new List<EcellData>();

            try
            {
                IEnumerator iter = layoutPanel.Controls.GetEnumerator();
                while (iter.MoveNext())
                {
                    Control c = (Control)iter.Current;
                    if (c == null) continue;
                    TableLayoutPanelCellPosition pos =
                        layoutPanel.GetPositionFromControl(c);
                    if (pos.Column == 0) continue;

                    if ((string)c.Tag == "modelID") modelID = c.Text;
                    else if ((string)c.Tag == "id")
                    {
                        id = c.Text;
                        if (c.Text == "")
                        {
                            MessageBox.Show("Can't read id. Please input id of instance.",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (Util.IsNG(c.Text))
//                        else if (c.Text.Contains(":") || (c.Text.Contains("/") && (m_currentObj.type != "Model" || c.Text != "/")))
                        {
                            MessageBox.Show("Id contains invalid character.\n",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (m_parentObj.key == "") key = c.Text;
                        else if (m_parentObj.key == "/") key = "/" + c.Text;
                        else key = m_parentObj.key + "/" + c.Text;
                    }
                    else if ((string)c.Tag == "classname") classname = c.Text;
                    else if ((string)c.Tag == "type") type = c.Text;
                    else
                    {
                        EcellData data = new EcellData();
                        try
                        {
                            data.M_name = (string)c.Tag;
                            if (m_propDict[data.M_name].M_value.M_type == typeof(int))
                                data.M_value = new EcellValue(Convert.ToInt32(c.Text));
                            else if (m_propDict[data.M_name].M_value.M_type == typeof(double))
                            {
                                if (c.Text == "1.79769313486232E+308")
                                    data.M_value = new EcellValue(Double.MaxValue);
                                else
                                    data.M_value = new EcellValue(Convert.ToDouble(c.Text));
                            }
                            else if (m_propDict[data.M_name].M_value.M_type == typeof(List<EcellValue>))
                                data.M_value = EcellValue.ToList(c.Text);
                            else
                                data.M_value = new EcellValue(c.Text);

                            data.M_isSettable = m_propDict[data.M_name].M_isSettable;
                            data.M_isSavable = m_propDict[data.M_name].M_isSavable;
                            data.M_isLoadable = m_propDict[data.M_name].M_isLoadable;
                            data.M_isGettable = m_propDict[data.M_name].M_isGettable;
                            data.M_isLogable = m_propDict[data.M_name].M_isLogable;
                            data.M_isLogger = m_propDict[data.M_name].M_isLogger;

                            data.M_entityPath = type + ":" + m_parentObj.key +
                                ":" + id + ":" + (string)c.Tag;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Format Error :\n\n" + ex,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        list.Add(data);

                    }
                }
                EcellObject obj = EcellObject.CreateObject(modelID, key, type, classname, list);
                obj.M_instances = new List<EcellObject>();
                List<EcellObject> objList = new List<EcellObject>();
                objList.Add(obj);
                m_dManager.DataAdd(objList);

                Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get exception while adding data.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The action of clicking ok button in AddForm with Model. 
        /// </summary>
        /// <param name="sender">OK Button</param>
        /// <param name="e">EventArgs</param>
        public void AddModel(object sender, EventArgs e)
        {
            string modelID = "";
            string key = "";
            string classname = "";
            string type = "Model";
            List<EcellData> list = new List<EcellData>();
            try
            {
                IEnumerator iter = layoutPanel.Controls.GetEnumerator();
                while (iter.MoveNext())
                {
                    Control c = (Control)iter.Current;
                    if (c == null) continue;
                    TableLayoutPanelCellPosition pos =
                        layoutPanel.GetPositionFromControl(c);
                    if (pos.Column == 0) continue;

                    if ((string)c.Tag == "modelID") modelID = c.Text;
                }

                if (modelID == "")
                {
                    MessageBox.Show("No set modelID.\n", "WARNING",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (Util.IsNG(modelID))
//                else if (modelID.Contains(":") || modelID.Contains("/"))
                {
                    MessageBox.Show("Id contains invalid character.\n",
                        "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                EcellObject obj = EcellObject.CreateObject(modelID, key, type, classname, list);
                obj.M_instances = new List<EcellObject>();
                List<EcellObject> objList = new List<EcellObject>();
                objList.Add(obj);
                m_dManager.DataAdd(objList);

                Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get exception while adding model.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The action of clicking ok button in AddForm with Variable or Process.
        /// </summary>
        /// <param name="sender">OK button</param>
        /// <param name="e">EventArgs</param>
        public void AddNodeElement(object sender, EventArgs e)
        {
            string id = "";
            string modelID = "";
            string key = "";
            string classname = "";
            string type = "";
            List<EcellData> list = new List<EcellData>();

            try
            {
                IEnumerator iter = layoutPanel.Controls.GetEnumerator();
                while (iter.MoveNext())
                {
                    Control c = (Control)iter.Current;
                    if (c == null) continue;
                    TableLayoutPanelCellPosition pos =
                        layoutPanel.GetPositionFromControl(c);
                    if (pos.Column != 1) continue;

                    if ((string)c.Tag == "modelID") modelID = c.Text;
                    else if ((string)c.Tag == "id")
                    {
                        id = c.Text;
                        if (c.Text == "")
                        {
                            MessageBox.Show("Can't read id text.Please input id of instance",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
//                        else if (c.Text.Contains("/") || c.Text.Contains(":"))
                        else if (Util.IsNG(c.Text))
                        {
                            MessageBox.Show("Id contains invalid character.\n",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (c.Text.ToUpper() == "SIZE")
                        {
                            MessageBox.Show("SIZE is the reserved name.\n",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (m_parentObj.key == "") key = c.Text;
                        else if (m_parentObj.key == "/") key = "/:" + c.Text;
                        else key = m_parentObj.key + ":" + c.Text;
                    }
                    else if ((string)c.Tag == "classname") classname = c.Text;
                    else if ((string)c.Tag == "type") type = c.Text;
                    else if ((string)c.Tag == "VariableReferenceList")
                    {
                        EcellData data = new EcellData();
                        data.M_name = (string)c.Tag;
                        data.M_value = EcellValue.ToVariableReferenceList(m_refStr);
                        data.M_entityPath = type + ":" + m_parentObj.key +
                            ":" + id + ":" + (string)c.Tag;
                        data.M_isSettable = m_propDict[data.M_name].M_isSettable;
                        data.M_isSavable = m_propDict[data.M_name].M_isSavable;
                        data.M_isLoadable = m_propDict[data.M_name].M_isLoadable;
                        data.M_isGettable = m_propDict[data.M_name].M_isGettable;
                        data.M_isLogable = m_propDict[data.M_name].M_isLogable;
                        data.M_isLogger = m_propDict[data.M_name].M_isLogger;

                        list.Add(data);
                    }
                    else
                    {
                        EcellData data = new EcellData();
                        try
                        {
                            data.M_name = (string)c.Tag;
                            if (m_propDict[data.M_name].M_value.M_type == typeof(int))
                                data.M_value = new EcellValue(Convert.ToInt32(c.Text));
                            else if (m_propDict[data.M_name].M_value.M_type == typeof(double))
                            {
                                if (c.Text == "1.79769313486232E+308")
                                    data.M_value = new EcellValue(Double.MaxValue);
                                else
                                    data.M_value = new EcellValue(Convert.ToDouble(c.Text));
                            }
                            else if (m_propDict[data.M_name].M_value.M_type == typeof(List<EcellValue>))
                                data.M_value = EcellValue.ToList(c.Text);
                            else
                                data.M_value = new EcellValue(c.Text);
                            data.M_entityPath = type + ":" + m_parentObj.key +
                                ":" + id + ":" + (string)c.Tag;
                            data.M_isSettable = m_propDict[data.M_name].M_isSettable;
                            data.M_isSavable = m_propDict[data.M_name].M_isSavable;
                            data.M_isLoadable = m_propDict[data.M_name].M_isLoadable;
                            data.M_isGettable = m_propDict[data.M_name].M_isGettable;
                            data.M_isLogable = m_propDict[data.M_name].M_isLogable;
                            data.M_isLogger = m_propDict[data.M_name].M_isLogger;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Format error:\n\n" + ex,
                                "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        list.Add(data);
                    }
                }

                EcellObject obj = EcellObject.CreateObject(modelID, key, type, classname, list);
                obj.M_instances = new List<EcellObject>();

                List<EcellObject> objList = new List<EcellObject>();
                objList.Add(obj);

                m_dManager.DataAdd(objList);
                Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get exception while adding data.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Update property of the selected TreeNode.
        /// </summary>
        /// <param name="sender">Button (Update)</param>
        /// <param name="e">EventArgs</param>
        public void UpdateProperty(object sender, EventArgs e)
        {
            string modelID = "";
            string key = "";
            string classname = "";
            string type = "";
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
                    if (pos.Column != 1) continue;

                    if ((string)c.Tag == "modelID") modelID = c.Text;
                    else if ((string)c.Tag == "id")
                    {
                        key = c.Text;
                        if (Util.IsNG(key))
//                        if (key.Contains("/") || key.Contains(":"))
                        {
                            MessageBox.Show("Id contains invalid character.\n",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (c.Text == "")
                        {
                            MessageBox.Show("Can't read id text.Please input id of instance",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (c.Text.ToUpper() == "SIZE")
                        {
                            MessageBox.Show("SIZE is the reserved name.\n",
                                "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else if ((string)c.Tag == "classname") classname = c.Text;
                    else if ((string)c.Tag == "type") type = c.Text;
                    else if ((string)c.Tag == "VariableReferenceList")
                    {
                        EcellData data = new EcellData();
                        data.M_name = (string)c.Tag;
                        data.M_value = EcellValue.ToVariableReferenceList(m_refStr);
                        if (key.Contains(":"))
                        {
                            int ind = key.LastIndexOf(":");
                            data.M_entityPath = type + ":" + key.Substring(0, ind) +
                                ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                        }
                        else
                        {
                            if (key == "/")
                            {
                                data.M_entityPath = type + ":" + "" +
                                    ":" + "/" + ":" + (string)c.Tag;
                            }
                            else
                            {
                                int ind = key.LastIndexOf("/");
                                data.M_entityPath = type + ":" + key.Substring(0, ind) +
                                    ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                            }
                        }

                        list.Add(data);
                    }
                    else
                    {
                        EcellData data = new EcellData();
                        data.M_name = (string)c.Tag;
                        if (m_propDict[data.M_name].M_value.M_type == typeof(int))
                            data.M_value = new EcellValue(Convert.ToInt32(c.Text));
                        else if (m_propDict[data.M_name].M_value.M_type == typeof(double))
                        {
                            if (c.Text == "1.79769313486232E+308")
                                data.M_value = new EcellValue(Double.MaxValue);
                            else
                                data.M_value = new EcellValue(Convert.ToDouble(c.Text));
                        }
                        else if (m_propDict[data.M_name].M_value.M_type == typeof(List<EcellValue>))
                            data.M_value = EcellValue.ToList(c.Text);
                        else
                            data.M_value = new EcellValue(c.Text);

                        if (key.Contains(":"))
                        {
                            int ind = key.LastIndexOf(":");
                            data.M_entityPath = type + ":" + key.Substring(0, ind) +
                                ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                        }
                        else
                        {
                            if (key == "/")
                            {
                                data.M_entityPath = type + ":" + "" +
                                    ":" + "/" + ":" + (string)c.Tag;
                            }
                            else
                            {
                                int ind = key.LastIndexOf("/");
                                data.M_entityPath = type + ":" + key.Substring(0, ind) +
                                    ":" + key.Substring(ind + 1) + ":" + (string)c.Tag;
                            }
                        }

                        data.M_isSettable = m_propDict[data.M_name].M_isSettable;
                        data.M_isSavable = m_propDict[data.M_name].M_isSavable;
                        data.M_isLoadable = m_propDict[data.M_name].M_isLoadable;
                        data.M_isGettable = m_propDict[data.M_name].M_isGettable;
                        data.M_isLogable = m_propDict[data.M_name].M_isLogable;
                        data.M_isLogger = m_propDict[data.M_name].M_isLogger;

                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Format error of input data.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                EcellObject obj = EcellObject.CreateObject(modelID, key, type, classname, list);
                obj.M_instances = m_currentObj.M_instances;
                m_dManager.DataChanged(m_currentObj.modelID, m_currentObj.key, m_currentObj.type, obj);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't change property of object.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Dispose();
        }

        /// <summary>
        /// Cancel to edit property of the selected TreeNode.
        /// </summary>
        /// <param name="sender">Button (Cancel)</param>
        /// <param name="e">EventArgs</param>
        public void CancelProperty(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Show variable reference list window to edit variable reference.
        /// </summary>
        /// <param name="sender">Button(VariableReferenceList)</param>
        /// <param name="e">EventArgs</param>
        public void ShowVarRefWindow(object sender, EventArgs e)
        {
            m_win = new VariableRefWindow();
            m_win.button1.Click += new EventHandler(m_win.AddVarReference);
            m_win.button2.Click += new EventHandler(m_win.DeleteVarReference);
            m_win.button3.Click += new EventHandler(m_win.ApplyVarReference);
            m_win.button4.Click += new EventHandler(m_win.CloseVarReference);
            m_win.button5.Click += new EventHandler(m_win.OKVarReference);

            List<EcellReference> list = EcellReference.ConvertString(m_refStr);
            foreach (EcellReference v in list)
            {
                DataGridViewRow row = new DataGridViewRow();

                bool isFixed = false;
                if (v.isFixed == 1) isFixed = true;
                m_win.dgv.Rows.Add(new object[] { v.name, v.fullID, v.coefficient, isFixed });
            }

            m_win.m_editor = this;
            m_win.ShowDialog();
        }
        #endregion
    }
}