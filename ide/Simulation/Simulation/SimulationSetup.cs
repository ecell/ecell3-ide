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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    namespace EcellLib.Simulation
    {
        public partial class SimulationSetup : Form
        {
            #region Fields
            /// <summary>
            /// DataManager.
            /// </summary>
            private DataManager m_dManager = null;
            /// <summary>
            /// loaded stepper list.
            /// </summary>
            private List<EcellObject> m_steppList = null;
            /// <summary>
            /// property and type dictionary.
            /// </summary>
            private Dictionary<string, string> m_propDict;
            /// <summary>
            /// List of value for selected stepper.
            /// </summary>
            private List<EcellData> m_selectValue;
            #endregion

            /// <summary>
            /// Constructor for SimulationSetup.
            /// </summary>
            public SimulationSetup()
            {
                InitializeComponent();
                m_propDict = new Dictionary<string, string>();
                m_dManager = DataManager.GetDataManager();
            }

            /// <summary>
            /// Set stepper list.
            /// </summary>
            /// <param name="list">stepper list</param>
            public void SetStepperList(List<EcellObject> list)
            {
                m_steppList = list;
            }

            /// <summary>
            /// Redraw simulation setup window on changing model ID.
            /// </summary>
            /// <param name="modelID">model ID</param>
            public void ChangeModelID(string modelID)
            {
                int j = 0;

                string param = paramCombo.Text;
                stepperListBox.Items.Clear();
                dgv.Rows.Clear();
                m_steppList = m_dManager.GetStepper(param, modelID);
                foreach (EcellObject obj in m_steppList)
                {
                    if (obj.M_value == null) continue;
                    stepperListBox.Items.Add(obj.key);
                    if (j != 0) continue;
                    ChangeDataGrid(obj.M_value);
                    int ind = stepCombo.Items.IndexOf(obj.classname);
                    stepCombo.SelectedIndex = ind;

                    j++;
                }
                if (j != 0)
                {
                    stepperListBox.SelectedIndex = 0;
                }
            }

            /// <summary>
            /// The action of changing data grid value.
            /// </summary>
            /// <param name="values">list of value.</param>
            public void ChangeDataGrid(List<EcellData> values)
            {
                m_propDict.Clear();
                dgv.Rows.Clear();
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                style.BackColor = Color.LightGray;
                foreach (EcellData d in values)
                {
                    if (d.M_name == "ClassName") continue;
                    string name = d.M_name;
                    string value = d.M_value.ToString();
                    string get = "+";
                    string set = "+";
                    if (!d.M_isGettable) get = "-";
                    if (!d.M_isSettable) set = "-";

                    if (d.M_value.IsDouble()) m_propDict.Add(d.M_name, "double");
                    else if (d.M_value.IsString()) m_propDict.Add(d.M_name, "string");
                    else if (d.M_value.IsInt()) m_propDict.Add(d.M_name, "int");
                    else if (d.M_value.IsList()) m_propDict.Add(d.M_name, "list");

                    dgv.Rows.Add(new object[] { name, value, get, set });
                    int ind = dgv.Rows.GetLastRow(DataGridViewElementStates.None);
                    if (!d.M_isSettable)
                    {
                        dgv.Rows[ind].ReadOnly = true;
                        dgv.Rows[ind].DefaultCellStyle = style;
                    }
                }
                m_selectValue = values;
            }

            /// <summary>
            /// Redraw simulation setup window on changing parameter ID.
            /// </summary>
            /// <param name="param">parameter ID</param>
            public void ChangePameterID(string param)
            {
                int j = 0;
                string selectModelName = "";

                modelCombo.Items.Clear();
                List<string> modelList = m_dManager.GetModelList();
                foreach (String modelName in modelList)
                {
                    modelCombo.Items.Add(modelName);
                    if (j == 0)
                    {
                        modelCombo.SelectedIndex = 0;
                        selectModelName = modelName;
                    }
                    j++;
                }
            }

            #region Event
            /// <summary>
            /// The action of changing selected model in stepper tab.
            /// </summary>
            /// <param name="sender">object(ComboBox)</param>
            /// <param name="e">EventArgs</param>
            public void ModelComboSelectedIndexChanged(object sender, EventArgs e)
            {
                string modelName = modelCombo.Text;
                ChangeModelID(modelName);
            }

            /// <summary>
            /// The action of changing selected model in Initial condition tab.
            /// </summary>
            /// <param name="sender">object(ComboBox)</param>
            /// <param name="e">EventArgs</param>
            public void InitModelComboSelectedIndexChanged(object sender, EventArgs e)
            {
                string modelName = modelCombo.Text;
                string currentParam = paramCombo.Text;

                InitProDGV.Rows.Clear();
                InitVarDGV.Rows.Clear();

                Dictionary<string, double> initList;

/*
                initList = m_dManager.GetInitialCondition(currentParam,
                    modelName, "System");
                foreach (string key in initList.Keys)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    c1.Value = key;
                    c1.ReadOnly = true;
                    row.Cells.Add(c1);

                    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                    c2.Value = initList[key];
                    row.Cells.Add(c2);

                    row.Tag = initList[key];
                    InitSysDGV.Rows.Add(row);
                }
*/

                initList = m_dManager.GetInitialCondition(currentParam,
                    modelName, "Process");
                foreach (string key in initList.Keys)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    c1.Value = key;
                    c1.ReadOnly = true;
                    row.Cells.Add(c1);

                    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                    c2.Value = initList[key];
                    row.Cells.Add(c2);

                    row.Tag = initList[key];
                    InitProDGV.Rows.Add(row);
                }

                initList = m_dManager.GetInitialCondition(currentParam,
                    modelName, "Variable");
                foreach (string key in initList.Keys)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    c1.Value = key;
                    c1.ReadOnly = true;
                    row.Cells.Add(c1);

                    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                    c2.Value = initList[key];
                    row.Cells.Add(c2);

                    row.Tag = initList[key];
                    InitVarDGV.Rows.Add(row);
                }
            }

            /// <summary>
            /// The action of changing the selected classname.
            /// </summary>
            /// <param name="sender">object(ComboBox)</param>
            /// <param name="e">EventArgs</param>
            public void StepComboSelectedIndexChanged(object sender, EventArgs e)
            {
                string stepperID = stepCombo.Text;
                Dictionary<string, EcellData> propDict;
                try
                {
                     propDict = DataManager.GetStepperProperty(stepperID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<EcellData> data = new List<EcellData>();
                foreach (string keys in propDict.Keys)
                {
                    data.Add(propDict[keys]);
                }

                ChangeDataGrid(data);
            }

            /// <summary>
            /// The action of changing the selected item in StepperListBox.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void StepperListBoxSelectedIndexChanged(object sender, EventArgs e)
            {
                if (stepperListBox.Text == "") return;
                string selectID = stepperListBox.SelectedItem.ToString();
                foreach (EcellObject obj in m_steppList)
                {
                    if (obj.M_value == null) continue;
                    if (obj.key != selectID) continue;

                    for (int i = 0; i < stepCombo.Items.Count; i++)
                    {
                        if (obj.classname == (string)stepCombo.Items[i])
                        {
                            stepCombo.SelectedIndex = i;
                            break;
                        }
                    }

                    ChangeDataGrid(obj.M_value);
                    break;
                }
            }

            /// <summary>
            /// The action of changing the selected item in ParaComboBox.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void SelectedIndexChangedParam(object sender, EventArgs e)
            {
                stepperListBox.Items.Clear();
                dgv.Rows.Clear();
                string param = paramCombo.SelectedItem.ToString();
                ChangePameterID(param);
            }

            /// <summary>
            /// The action of clicking set button in SimulationSetupWindow.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void SetButtonClick(object sender, EventArgs e)
            {
                string param = paramCombo.SelectedItem.ToString();

                m_dManager.SetSimulationParameter(param);
            }

            /// <summary>
            /// The action of clicking new button in SimulationSetupWindow.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void NewButtonClick(object sender, EventArgs e)
            {
                NewParameterWindow m_newwin = new NewParameterWindow();

                m_newwin.OKButton.Click += new EventHandler(m_newwin.NewParameterClick);
                m_newwin.cancelButton.Click += new EventHandler(m_newwin.CancelParameterClick);

                m_newwin.SetParentWindow(this);

                m_newwin.ShowDialog();
            }

            /// <summary>
            /// The action of clicking delete button in SimulationSetupWindow.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void DeleteButtonClick(object sender, EventArgs e)
            {
                if (paramCombo.SelectedItem == null) return;

                string param = paramCombo.SelectedItem.ToString();
                if (param == "DefaultParameter")
                {
                    MessageBox.Show("Can't delete default parameter.", "WARNING",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (paramCombo.Items.Count == 1)
                {
                    MessageBox.Show("Can't delete default parameter.\nAt least one parameter is necessary for the project.", "WARNING",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    paramCombo.Items.Remove(param);
                    paramCombo.SelectedIndex = 0;
                }
                m_dManager.DeleteSimulationParameter(param);
            }

            /// <summary>
            /// The action of clicking save button in SimulationSetupWindow.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void SaveButtonClick(object sender, EventArgs e)
            {
                string param = paramCombo.SelectedItem.ToString();
                m_dManager.SaveSimulationParameter(param);
            }

            /// <summary>
            /// The action of clicking close button in SimulationSetupWindow
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void CloseButtonClick(object sender, EventArgs e)
            {
                this.Dispose();
            }

            /// <summary>
            /// The action of clicking the Update Button on SimulationSetup.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void UpdateButtonClick(object sender, EventArgs e)
            {
                if (tabControl1.SelectedTab.Text == "Stepper")
                {
                    List<EcellObject> list = new List<EcellObject>();
                    string paramID = paramCombo.Text;
                    string stepperID = stepperListBox.Text;
                    string modelID = modelCombo.Text;
                    string classname = stepCombo.Text;

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string name = (string)row.Cells[0].Value;
                        string value = (string)row.Cells[1].Value;
                        try
                        {
                            foreach (EcellData tmp in m_selectValue)
                            {
                                if (tmp.M_name == name)
                                {
                                    if (tmp.M_value.IsInt())
                                        tmp.M_value = new EcellValue(Convert.ToInt32(value));
                                    else if (tmp.M_value.IsDouble())
                                    {
                                        if (value == "1.79769313486232E+308")
                                            tmp.M_value = new EcellValue(Double.MaxValue);
                                        else
                                            tmp.M_value = new EcellValue(System.Double.Parse(value));
                                    }
                                    else if (tmp.M_value.IsList())
                                        tmp.M_value = EcellValue.ToList(value);
                                    else
                                        tmp.M_value = new EcellValue(value);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Format Error:\n\n" + ex, "ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    EcellObject obj = EcellObject.CreateObject(modelID, stepperID, 
                        "Stepper", classname, m_selectValue);
                    list.Add(obj);

                    m_dManager.UpdateStepperID(paramID, list);
                    foreach (EcellObject tmp in m_steppList)
                    {
                        if (tmp.M_value == null) continue;
                        if (tmp.key != stepperID) continue;

                        m_steppList.Remove(tmp);
                        break;
                    }
                    m_steppList.Add(obj);
                }
                else if (tabControl1.SelectedTab.Text == "Logging")
                {
                    int stepNum = 0;
                    double secNum = 0.0;
                    int fullAction = 0;
                    int diskSpace = 0;
                    string paramID = paramCombo.Text;

                    try 
                    {
                        // frequency
                        if (radioButton1.Checked)
                        {
                            if (textBox1.Text == "")
                            {
                                MessageBox.Show("Please input frequency for steps.", "ERROR",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            stepNum = Convert.ToInt32(textBox1.Text);
                        }
                        else if (radioButton2.Checked)
                        {
                            if (textBox2.Text == "")
                            {
                                MessageBox.Show("Please input frequency for seconds.", "ERROR",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            secNum = Convert.ToDouble(textBox2.Text);
                        }
                        else
                        {
                            MessageBox.Show("No select the policy of logging frequency.", "ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // action when disk is full
                        if (radioButton3.Checked) { }
                        else if (radioButton4.Checked)
                        {
                            fullAction = 1;
                        }
                        else
                        {
                            MessageBox.Show("No select the policy of action when disk is full.", "ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // disk space
                        if (radioButton5.Checked) { }
                        else if (radioButton6.Checked)
                        {
                            if (textBox6.Text == "")
                            {
                                MessageBox.Show("Please input disk size.", "ERROR",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            diskSpace = Convert.ToInt32(textBox6.Text);
                        }
                        else
                        {
                            MessageBox.Show("No select the size of disk space.", "ERROR",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        LoggerPolicy log = new LoggerPolicy(stepNum, secNum, fullAction, diskSpace);
                        m_dManager.SetLoggerPolicy(paramID, ref log);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Get exception while update logging policy.\n\n" + ex, "ERROR",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (tabControl1.SelectedTab.Text == "Initial Condition")
                {
                    string paramName = paramCombo.Text;
                    string modelName = iModelCombo.Text;
                    Dictionary<string, double> updateList = new Dictionary<string, double>();

                    /*
                    // ======================================== 
                    // initial parameter of system update.
                    // ========================================
                    foreach (DataGridViewRow row in InitSysDGV.Rows)
                    {
                        double value = Convert.ToDouble(row.Cells[1].Value);
                        double pre = Convert.ToDouble(row.Tag);

                        if (value == pre) continue;
                        string id = (string)row.Cells[0].Value;

                        updateList.Add(id, value);
                    }
                    m_dManager.UpdateInitialCondition(paramName, modelName, "System", updateList);
                    updateList.Clear();
                    */


                    // ======================================== 
                    // initial parameter of process update.
                    // ========================================
                    foreach (DataGridViewRow row in InitProDGV.Rows)
                    {
                        double value = Convert.ToDouble(row.Cells[1].Value);
                        double pre = Convert.ToDouble(row.Tag);

                        if (value == pre) continue;
                        string id = (string)row.Cells[0].Value;

                        updateList.Add(id, value);
                    }
                    m_dManager.UpdateInitialCondition(paramName, modelName, "Process", updateList);
                    updateList.Clear();

                    // ======================================== 
                    // initial parameter of variable update.
                    // ========================================
                    foreach (DataGridViewRow row in InitVarDGV.Rows)
                    {
                        double value = Convert.ToDouble(row.Cells[1].Value);
                        double pre = Convert.ToDouble(row.Tag);

                        if (value == pre) continue;
                        string id = (string)row.Cells[0].Value;

                        updateList.Add(id, value);
                    }
                    m_dManager.UpdateInitialCondition(paramName, modelName, "Variable", updateList);
                    updateList.Clear();
                }
            }

            /// <summary>
            /// The action of clicking Delete Button on SimulationSetup.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void DeleteStepperClick(object sender, EventArgs e)
            {
                string param = paramCombo.SelectedItem.ToString();
                string modelID = modelCombo.SelectedItem.ToString();
                string stepperID = stepperListBox.SelectedItem.ToString();

                if (stepperListBox.Items.Count <= 1)
                {
                    MessageBox.Show("Can't delete the selected stepper.\nAt lease one stepper is necessary for the model.", "WARNING",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                EcellObject obj = EcellObject.CreateObject(modelID, stepperID, "Stepper",
                    "", new List<EcellData>());
                m_dManager.DeleteStepperID(param, obj);
                stepperListBox.Items.Remove(stepperID);
                dgv.Rows.Clear();
                stepperListBox.SelectedIndex = 0;
            }

            /// <summary>
            /// The action of clicking the add Button[Stepper] in SimulationSetupWindow.
            /// </summary>
            /// <param name="sender">object(Button)</param>
            /// <param name="e">EventArgs</param>
            public void AddStepperClick(object sender, EventArgs e)
            {
                NewParameterWindow m_newwin = new NewParameterWindow();
                m_newwin.Text = "New Stepper";
                m_newwin.OKButton.Click += new EventHandler(m_newwin.AddStepperClick);
                m_newwin.cancelButton.Click += new EventHandler(m_newwin.CancelStepperClick);

                m_newwin.SetParentWindow(this);

                m_newwin.ShowDialog();
            }
            #endregion

            private void SimulationSetupShown(object sender, EventArgs e)
            {
                this.paramCombo.Focus();
            }
        }
    }