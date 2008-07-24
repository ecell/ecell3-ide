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
using System.Diagnostics;

using Ecell;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Simulation
{
    /// <summary>
    /// Dialog to setup the properties of simulation.
    /// </summary>
    public partial class SimulationConfigurationDialog : Form
    {
        #region Fields
        /// <summary>
        /// The owner of this object.
        /// </summary>
        private Simulation m_owner = null;
        /// <summary>
        /// loaded stepper list.
        /// </summary>
        private List<EcellObject> m_steppList = null;
        /// <summary>
        /// List of value for selected stepper.
        /// </summary>
        private List<EcellData> m_selectValue;
        #endregion

        #region Accessors
        /// <summary>
        /// DataManager.
        /// </summary>
        public DataManager DataManager
        {
            get { return m_owner.DataManager; }
        }
        #endregion

        /// <summary>
        /// Constructor for SimulationSetup.
        /// </summary>
        public SimulationConfigurationDialog(Simulation owner)
        {
            m_owner = owner;
            InitializeComponent();

            paramCombo.SelectedIndexChanged += new EventHandler(SelectedIndexChangedParam);
            stepperListBox.SelectedIndexChanged += new EventHandler(StepperListBoxSelectedIndexChanged);
            stepCombo.SelectedIndexChanged += new EventHandler(StepComboSelectedIndexChanged);
            modelCombo.SelectedIndexChanged += new EventHandler(ModelComboSelectedIndexChanged);
            SSCreateButton.Click += new EventHandler(NewButtonClick);
            SSDeleteButton.Click += new EventHandler(DeleteButtonClick);
            SSCloseButton.Click += new EventHandler(CloseButtonClick);
            SSApplyButton.Click += new EventHandler(UpdateButtonClick);
            modelCombo.SelectedIndexChanged += new EventHandler(InitModelComboSelectedIndexChanged);

            SSSetButton.Click += new EventHandler(SetButtonClick);
            SSAddStepperButton.Click += new EventHandler(AddStepperClick);
            SSDeleteStepperButton.Click += new EventHandler(DeleteStepperClick);
        }

        /// <summary>
        /// Check whether this steppr is existed.
        /// </summary>
        /// <param name="data">stepper name.</param>
        /// <returns>if exist, return true.</returns>
        public bool IsExistStepper(string data)
        {
            return stepCombo.Items.Contains(data);
        }

        /// <summary>
        /// Get the current parameter name.
        /// </summary>
        /// <returns>the parameter name.</returns>
        public string GetCurrentParameter()
        {
            return paramCombo.Text;
        }

        /// <summary>
        /// Get the current model name.
        /// </summary>
        /// <returns>the model name.</returns>
        public string GetCurrentModel()
        {
            return modelCombo.Text;
        }

        /// <summary>
        /// Get the current stepper name.
        /// </summary>
        /// <returns>the stepper name.</returns>
        public string GetCurrentStepper()
        {
            return stepCombo.Text;
        }

        /// <summary>
        /// Add the stepper to ListBox.
        /// </summary>
        /// <param name="data">the stepper name.</param>
        public void AddStepper(string data)
        {
            stepperListBox.Items.Add(data);
        }

        /// <summary>
        /// Set the created parameter.
        /// </summary>
        /// <param name="data">the parameter name.</param>
        public void SetNewParameter(string data)
        {
            paramCombo.Items.Add(data);
            paramCombo.SelectedItem = data;
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
            m_steppList = m_owner.DataManager.GetStepper(param, modelID);
            foreach (EcellObject obj in m_steppList)
            {
                if (obj.Value == null) continue;
                stepperListBox.Items.Add(obj.Key);
                if (j != 0) continue;
                ChangeDataGrid(obj.Value);
                int ind = stepCombo.Items.IndexOf(obj.Classname);
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
            dgv.Rows.Clear();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = Color.LightGray;
            foreach (EcellData d in values)
            {
                if (d.Name == "ClassName") continue;
                string name = d.Name;
                string value = d.Value.ToString();
                string get = "+";
                string set = "+";
                if (!d.Gettable) get = "-";
                if (!d.Settable) set = "-";

                dgv.Rows.Add(new object[] { name, value, get, set });
                int ind = dgv.Rows.GetLastRow(DataGridViewElementStates.None);
                if (!d.Settable)
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
            List<string> modelList = m_owner.DataManager.GetModelList();
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
            SetInitialParameter();
            SetLoggingCondition();
        }

        /// <summary>
        /// The action of changing selected model in Initial condition tab.
        /// </summary>
        private void SetInitialParameter()
        {
            InitProDGV.Rows.Clear();
            InitVarDGV.Rows.Clear();

            SetInitialParameterGrid(Constants.xpathProcess, InitProDGV);
            SetInitialParameterGrid(Constants.xpathVariable, InitVarDGV);
        }

        /// <summary>
        /// Set the initial parameters of data to DataGridView.
        /// </summary>
        /// <param name="type">data type.</param>
        /// <param name="dgv">DataGridView for data type.</param>
        private void SetInitialParameterGrid(string type, DataGridView dgv)
        {
            string modelName = modelCombo.Text;
            string currentParam = paramCombo.Text;

            Dictionary<string, double> initList =
                m_owner.DataManager.GetInitialCondition(currentParam,
                                modelName, type);
            foreach (string key in initList.Keys)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = key;
                row.Cells.Add(c1);

                DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                c2.Value = initList[key];
                row.Cells.Add(c2);

                row.Tag = initList[key];
                dgv.Rows.Add(row);
                c1.ReadOnly = true;
            }
        }

        /// <summary>
        /// Update the property of stepper from the information of DataGridView.
        /// </summary>
        private void UpdateStepperCondition()
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
                        if (tmp.Name != name) continue;
                        if (tmp.Value.IsInt())
                            tmp.Value = new EcellValue(Convert.ToInt32(value));
                        else if (tmp.Value.IsDouble())
                        {
                            if (value == "1.79769313486232E+308")
                                tmp.Value = new EcellValue(Double.MaxValue);
                            else
                                tmp.Value = new EcellValue(System.Double.Parse(value));
                        }
                        else if (tmp.Value.IsList())
                            tmp.Value = EcellValue.ToList(value);
                        else
                            tmp.Value = new EcellValue(value);

                    }
                }
                catch (Exception)
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    return;
                }
            }
            EcellObject obj = EcellObject.CreateObject(modelID, stepperID,
                Constants.xpathStepper, classname, m_selectValue);
            list.Add(obj);

            m_owner.DataManager.UpdateStepperID(paramID, list);
            foreach (EcellObject tmp in m_steppList)
            {
                if (tmp.Value == null) continue;
                if (tmp.Key != stepperID) continue;

                m_steppList.Remove(tmp);
                break;
            }
            m_steppList.Add(obj);
        }

        /// <summary>
        /// Set the simulation condition when this form is shown.
        /// </summary>
        private void SetSimulationCondition()
        {
            int i = 0;
            List<string> stepList = m_owner.DataManager.GetStepperList();
            foreach (string step in stepList)
            {
                stepCombo.Items.Add(step);
            }

            string currentParam = m_owner.DataManager.GetCurrentSimulationParameterID();
            List<string> paramList = m_owner.DataManager.GetSimulationParameterIDs();
            foreach (string param in paramList)
            {
                paramCombo.Items.Add(param);
                if (param == currentParam || (currentParam == null && i == 0))
                {
                    paramCombo.SelectedIndex = i;
                    ChangePameterID(param);
                    ChangeModelID(modelCombo.Text);
                }
                i++;
            }
            SetInitialParameter();
        }

        /// <summary>
        /// Set the property of logging for current parameters.
        /// </summary>
        public void SetLoggingCondition()
        {
            LoggerPolicy log = m_owner.DataManager.GetLoggerPolicy(paramCombo.Text);
            if (log.m_reloadStepCount > 0)
            {
                freqByStepRadio.Checked = true;
                freqByStepTextBox.Text = log.m_reloadStepCount.ToString();
            }
            else if (log.m_reloadInterval > 0.0)
            {
                freqBySecRadio.Checked = true;
                freqBySecTextBox.Text = log.m_reloadInterval.ToString();
            }
            if (log.m_diskFullAction == 0)
            {
                exceptionRadio.Checked = true;
            }
            else
            {
                overrideRadio.Checked = true;
            }
            if (log.m_maxDiskSpace == 0)
            {
                noLimitRadio.Checked = true;
            }
            else
            {
                maxSizeRadio.Checked = true;
                maxKbTextBox.Text = log.m_maxDiskSpace.ToString();
            }
        }

        /// <summary>
        /// Update the property of logging from DataGridView.
        /// </summary>
        private void UpdateLoggingCondition()
        {
            int stepNum = 0;
            double secNum = 0.0;
            int fullAction = 0;
            int diskSpace = 0;
            string paramID = paramCombo.Text;

            try
            {
                // frequency
                if (freqByStepRadio.Checked)
                {
                    if (String.IsNullOrEmpty( freqByStepTextBox.Text))
                    {
                        Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput,
                            new object[] { "Step" }));

                        return;
                    }
                    stepNum = Convert.ToInt32(freqByStepTextBox.Text);
                }
                else if (freqBySecRadio.Checked)
                {
                    if (freqBySecTextBox.Text == "")
                    {
                        Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput,
                            new object[] { "Sec" }));

                        return;
                    }
                    secNum = Convert.ToDouble(freqBySecTextBox.Text);
                }

                // action when disk is full
                if (exceptionRadio.Checked) { }
                else if (overrideRadio.Checked)
                {
                    fullAction = 1;
                }

                // disk space
                if (noLimitRadio.Checked) { }
                else if (maxSizeRadio.Checked)
                {
                    if (maxKbTextBox.Text == "")
                    {
                        Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput,
                            new object[] { "Disk Size" }));

                        return;
                    }

                    diskSpace = Convert.ToInt32(maxKbTextBox.Text);
                }

                LoggerPolicy log = new LoggerPolicy(stepNum, secNum, fullAction, diskSpace);
                m_owner.DataManager.SetLoggerPolicy(paramID, ref log);
            }
            catch (Exception ex)
            {
                String errmes = MessageResources.ErrUpdateLog;
                Util.ShowErrorDialog(errmes + "\n\n" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// Update the property of initial parameter from DataGridView.
        /// </summary>
        private void UpdateInitialCondition()
        {
            string paramName = paramCombo.Text;
            string modelName = modelCombo.Text;
            Dictionary<string, double> updateList = new Dictionary<string, double>();

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
            m_owner.DataManager.UpdateInitialCondition(paramName, modelName, "Process", updateList);
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
            m_owner.DataManager.UpdateInitialCondition(paramName, modelName, "Variable", updateList);
            updateList.Clear();
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
            SetInitialParameter();
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
                propDict = m_owner.DataManager.GetStepperProperty(stepperID);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
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
        /// <param name="sender">ListBox.</param>
        /// <param name="e">EventArgs.</param>
        public void StepperListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (stepperListBox.Text == "") return;
            string selectID = stepperListBox.SelectedItem.ToString();
            foreach (EcellObject obj in m_steppList)
            {
                if (obj.Value == null) continue;
                if (obj.Key != selectID) continue;

                for (int i = 0; i < stepCombo.Items.Count; i++)
                {
                    if (obj.Classname == (string)stepCombo.Items[i])
                    {
                        stepCombo.SelectedIndex = i;
                        break;
                    }
                }

                ChangeDataGrid(obj.Value);
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
            if (paramCombo.SelectedItem == null) return;
            string param = paramCombo.SelectedItem.ToString();

            m_owner.DataManager.SetSimulationParameter(param, false, false);
        }

        /// <summary>
        /// The action of clicking new button in SimulationSetupWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void NewButtonClick(object sender, EventArgs e)
        {
            InputParameterNameDialog newwin = new InputParameterNameDialog(this);
            using (newwin) 
            {
                if (newwin.ShowDialog() != DialogResult.OK)
                    return;
                string newParamName = newwin.InputText;
                DataManager.CreateSimulationParameter(newParamName);
                SetNewParameter(newParamName);
            }
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
            if (param == "DefaultParameter" || paramCombo.Items.Count == 1)
            {
                Util.ShowWarningDialog(String.Format(MessageResources.ErrDelParam,
                    new object[] { param }));
                return;
            }
            paramCombo.Items.Remove(param);
            paramCombo.SelectedIndex = 0;
            m_owner.DataManager.DeleteSimulationParameter(param);
        }

        /// <summary>
        /// The action of clicking save button in SimulationSetupWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void SaveButtonClick(object sender, EventArgs e)
        {
            string param = paramCombo.SelectedItem.ToString();
            m_owner.DataManager.SaveSimulationParameter(param);
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
                UpdateStepperCondition();
            }
            else if (tabControl1.SelectedTab.Text == "Logging")
            {
                UpdateLoggingCondition();
            }
            else if (tabControl1.SelectedTab.Text == "Initial Condition")
            {
                UpdateInitialCondition();
            }
            this.Close();
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
                Util.ShowWarningDialog(MessageResources.ErrDelStep);

                return;
            }

            EcellObject obj = EcellObject.CreateObject(modelID, stepperID,
                Constants.xpathStepper, "", new List<EcellData>());
            m_owner.DataManager.DeleteStepperID(param, obj);
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
            InputParameterNameDialog newwin = new InputParameterNameDialog(this);
            using (newwin)
            {
                newwin.Text = MessageResources.NewStepperText;
                if (newwin.ShowDialog() != DialogResult.OK)
                    return;
                try
                {
                    string paramID = GetCurrentParameter();
                    string modelID = GetCurrentModel();
                    string stepperID = GetCurrentStepper();
                    Dictionary<string, EcellData> propDict =
                        m_owner.DataManager.GetStepperProperty(stepperID);
                    List<EcellData> list = new List<EcellData>();
                    foreach (string key in propDict.Keys)
                    {
                        if (propDict[key].Name == Constants.propProcessList ||
                            propDict[key].Name == Constants.propSystemList ||
                            propDict[key].Name == Constants.propReadVariableList ||
                            propDict[key].Name == Constants.propWriteVariableList)
                        {
                            EcellData d = propDict[key];
                            List<EcellValue> nulllist = new List<EcellValue>();
                            d.Value = new EcellValue(nulllist);
                            list.Add(d);
                        }
                        else
                        {
                            list.Add(propDict[key]);
                        }
                    }
                    EcellObject obj = EcellObject.CreateObject(modelID, newwin.InputText, Constants.xpathStepper, stepperID, list);
                    DataManager.AddStepperID(paramID, obj);
                    AddStepper(newwin.InputText);
                    SetStepperList(m_owner.DataManager.GetStepper(paramID, modelID));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    Util.ShowErrorDialog(ex.Message);
                }
            }
        }

        /// <summary>
        /// Event when the setup window of simulation is shown.
        /// </summary>
        /// <param name="sender">Form.</param>
        /// <param name="e">EventArgs.</param>
        private void ShowSimulationSetupWin(object sender, EventArgs e)
        {
            SetSimulationCondition();
            SetLoggingCondition();
            if (freqBySecTextBox.Text == "")
            {
                this.freqByStepRadio.Checked = true;
                this.freqByStepTextBox.ReadOnly = false;
                this.freqBySecTextBox.ReadOnly = true;
            }
            else
            {
                this.freqBySecRadio.Checked = true;
                this.freqBySecTextBox.ReadOnly = false;
                this.freqByStepTextBox.ReadOnly = true;
            }

            maxKbTextBox.ReadOnly = noLimitRadio.Checked;
            this.paramCombo.Focus();
        }

        /// <summary>
        /// Event when the check of frequency is changed.
        /// </summary>
        /// <param name="sender">RadioButton.</param>
        /// <param name="e">EventArgs.</param>
        private void FreqCheckChanged(object sender, EventArgs e)
        {
            if (freqBySecRadio.Checked)
            {
                freqBySecTextBox.ReadOnly = false;
                freqByStepTextBox.ReadOnly = true;
            }
            else
            {
                freqBySecTextBox.ReadOnly = true;
                freqByStepTextBox.ReadOnly = false;
            }
        }

        /// <summary>
        /// Event when the check of action is changed.
        /// </summary>
        /// <param name="sender">RadioButton.</param>
        /// <param name="e">EventArgs.</param>
        private void ActionCheckChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Event when the check of space is changed.
        /// </summary>
        /// <param name="sender">RadioButton.</param>
        /// <param name="e">EventArgs.</param>
        private void SpaceCheckChanged(object sender, EventArgs e)
        {
            maxKbTextBox.ReadOnly = noLimitRadio.Checked;
        }

        /// <summary>
        /// Event when key is pressed.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">KeyPressEventArgs</param>
        private void SetupKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SSApplyButton.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                SSCloseButton.PerformClick();
            }
        }
        #endregion


    }
}
