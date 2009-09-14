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
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.Simulation
{
    /// <summary>
    /// Dialog to setup the properties of simulation.
    /// </summary>
    internal partial class SimulationConfigurationDialog : Form
    {
        #region Fields
        /// <summary>
        /// The owner of this object.
        /// </summary>
        private Simulation m_owner = null;
        /// <summary>
        /// The flag whether the logger setting is by step.
        /// </summary>
        private bool freqByStepTextBox_filledWithDefaultValue = false;
        /// <summary>
        /// The flag whether the logger setting is by sec.
        /// </summary>
        private bool freqBySecTextBox_filledWithDefaultValue = false;
        /// <summary>
        /// Default step count.
        /// </summary>
        private const int m_defaultStepCount = 1;
        /// <summary>
        /// Default redraw interval.
        /// </summary>
        private const double m_defaultInterval = 0.01;
        /// <summary>
        /// Default log size.
        /// </summary>
        private const int m_defaultMaxLogSize = 500;
        #endregion

        #region Accessors
        /// <summary>
        /// get the list of result.
        /// </summary>
        public IEnumerable<SimulationParameterSet> Result
        {
            get
            {
                List<SimulationParameterSet> retval = new List<SimulationParameterSet>();
                foreach (SimulationParameterSet sps in m_simParamSets)
                    retval.Add(sps);
                return retval;
            }
        }

        /// <summary>
        /// get the current simulation parameter.
        /// </summary>
        public String CurrentParameterID
        {
            get
            {
                SimulationParameterSet sps = (SimulationParameterSet)m_simParamSets.Current;
                return sps.Name;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for SimulationSetup.
        /// </summary>
        public SimulationConfigurationDialog(Simulation owner, IEnumerable<SimulationParameterSet> simParamSets)
        {
            m_owner = owner;
            InitializeComponent();
            perModelSimulationParameterBindingSource.CurrentChanged += new EventHandler(perModelSimulationParameterBindingSource_CurrentChanged);
            perModelSimulationParameterBindingSource.MoveFirst();
            m_simParamSets.SuspendBinding();

            string currentParam = m_owner.DataManager.CurrentProject.Info.SimulationParam;
            SimulationParameterSet current = null;
            foreach (SimulationParameterSet i in simParamSets)
            {
                m_simParamSets.Add(i);
                if (i.Name.Equals(currentParam))
                    current = i;
            }
            m_simParamSets.ResumeBinding();
            if (current != null)
                ChangeParameterID(current);
        }
        #endregion

        /// <summary>
        /// Redraw simulation setup window on changing model ID.
        /// </summary>
        /// <param name="modelID">model ID</param>
        public void ChangeModelID(string modelID)
        {
            string param = paramCombo.Text;
        }

        /// <summary>
        /// Redraw simulation setup window on changing parameter ID.
        /// </summary>
        /// <param name="paramSet">parameter ID</param>
        public void ChangeParameterID(SimulationParameterSet paramSet)
        {
            m_simParamSets.CurrencyManager.Position = m_simParamSets.IndexOf(paramSet);
        }

        /// <summary>
        /// Check this simulation paramter is already exist.
        /// </summary>
        /// <param name="name">the simulation parameter</param>
        /// <returns>if exist, return true.</returns>
        public bool IsExistParameterSet(string name)
        {
            for (int i = 0; i < m_simParamSets.Count; i++)
            {
                if (((SimulationParameterSet)m_simParamSets[i]).Name.ToUpper().Equals(name.ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }

        #region Event
        /// <summary>
        /// The selected simulation parameter sets is changed.
        /// </summary>
        /// <param name="sender">BindingSource</param>
        /// <param name="e">EventArgs</param>
        void perModelSimulationParameterBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            PerModelSimulationParameter p = (PerModelSimulationParameter)((BindingSource)sender).Current;
            initialConditionsBindingSource.DataSource = p.InitialConditions;
        }

        /// <summary>
        /// The action of clicking new button in SimulationSetupWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void NewButtonClick(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (SimulationParameterSet s in m_simParamSets)
            {
                list.Add(s.Name);
            }
            InputParameterNameDialog newwin = new InputParameterNameDialog(this);
            newwin.AlreadyList = list;

            using (newwin) 
            {
                if (newwin.ShowDialog() != DialogResult.OK)
                    return;
                SimulationParameterSet sps = new SimulationParameterSet(newwin.InputText);
                SimulationParameterSet sp = (SimulationParameterSet)m_simParamSets.Current;
                for (int i = 0; i < sp.PerModelSimulationParameters.Count; i++)
                {
                    sps.PerModelSimulationParameters.Add(new PerModelSimulationParameter(sp.PerModelSimulationParameters[i]));
                }
                m_simParamSets.Add(sps);
            }
        }

        /// <summary>
        /// The action of clicking delete button in SimulationSetupWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void DeleteButtonClick(object sender, EventArgs e)
        {
            string simParam = paramCombo.Text;
            if (simParam.Equals(Constants.defaultSimParam))
            {
                Util.ShowErrorDialog(MessageResources.ErrDelDefault);
                return;
            }
            if (m_simParamSets.Count <= 1)
            {
                Util.ShowErrorDialog(MessageResources.ErrDelParam);
                return;
            }
            m_simParamSets.RemoveCurrent();
        }


        /// <summary>
        /// Event when key is pressed.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">KeyPressEventArgs</param>
        private void SetupKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
        }

        /// <summary>
        /// Selected simulation parameter is changed.
        /// </summary>
        /// <param name="sender">BindingSource</param>
        /// <param name="e">EventArgs</param>
        private void m_simParamSets_CurrentChanged(object sender, EventArgs e)
        {
            LoggerPolicy pol = ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy;
            switch (pol.DiskFullAction)
            {
                case DiskFullAction.Overwrite:
                    overrideRadio.Checked = true;
                    break;
                case DiskFullAction.Terminate:
                    exceptionRadio.Checked = true;
                    break;
            }

            if (pol.ReloadStepCount > 0)
            {
                freqByStepRadio.Checked = true;
                freqByStepTextBox.Enabled = true;
                freqByStepTextBox.Text = Convert.ToString(pol.ReloadStepCount);
                freqBySecTextBox.Enabled = false;
                freqBySecTextBox.Text = "";
                freqByStepTextBox_filledWithDefaultValue = false;
                freqBySecTextBox_filledWithDefaultValue = true;
            }
            else if (pol.ReloadInterval > 0)
            {
                freqBySecRadio.Checked = true;
                freqBySecTextBox.Enabled = true;
                freqBySecTextBox.Text = Convert.ToString(pol.ReloadInterval);
                freqByStepTextBox.Enabled = false;
                freqByStepTextBox.Text = "";
                freqByStepTextBox_filledWithDefaultValue = true;
                freqBySecTextBox_filledWithDefaultValue = false;
            }
            else 
            {
                freqByStepRadio.Checked = true;
                freqByStepTextBox.Enabled = true;
                freqByStepTextBox.Text = Convert.ToString(1);
                freqBySecTextBox.Enabled = false;
                freqBySecTextBox.Text = "";
                freqByStepTextBox_filledWithDefaultValue = true;
                freqBySecTextBox_filledWithDefaultValue = true;
            }

            if (pol.MaxDiskSpace == 0)
            {
                noLimitRadio.Checked = true;
                maxKbTextBox.Text = "";
                maxKbTextBox.Enabled = false;
            }
            else
            {
                maxSizeRadio.Checked = true;
                maxKbTextBox.Enabled = true;
                maxKbTextBox.Text = Convert.ToString(pol.MaxDiskSpace);
            }
        }

        /// <summary>
        /// The check of RadioButton for override is changed.
        /// </summary>
        /// <param name="sender">RadioButton</param>
        /// <param name="e">EventArgs</param>
        private void overrideRadio_CheckedChanged(object sender, EventArgs e)
        {
            ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.DiskFullAction =
                ((RadioButton)sender).Checked ?
                DiskFullAction.Overwrite :
                DiskFullAction.Terminate;
        }

        /// <summary>
        /// The check of RadioButton for by step is changed.
        /// </summary>
        /// <param name="sender">RadioButton</param>
        /// <param name="e">EventArgs</param>
        private void freqByStepRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                freqBySecTextBox.Enabled = false;
                freqByStepTextBox.Enabled = true;
                if (freqBySecTextBox_filledWithDefaultValue)
                {
                    freqBySecTextBox.Text = "";
                }
                if (string.IsNullOrEmpty(freqByStepTextBox.Text))
                {
                    freqByStepTextBox.Text = Convert.ToString(m_defaultStepCount);
                    freqByStepTextBox_filledWithDefaultValue = true;
                }
                freqByStepTextBox.Focus();
            }
            else
            {
                freqBySecTextBox.Enabled = true;
                freqByStepTextBox.Enabled = false;
                if (freqByStepTextBox_filledWithDefaultValue)
                {
                    freqByStepTextBox.Text = "";
                }
                if (string.IsNullOrEmpty(freqBySecTextBox.Text))
                {
                    freqBySecTextBox.Text = Convert.ToString(m_defaultInterval);
                    freqBySecTextBox_filledWithDefaultValue = true;
                }
                freqBySecTextBox.Focus();
            }
        }

        /// <summary>
        /// Validating the TextBox of by step.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void freqByStepTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            string text = freqByStepTextBox.Text;
            int stepcount = ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.ReloadStepCount;
            if (stepcount <= 0) stepcount = m_defaultStepCount;
            int dummy;
            string errMsg = "";
            if (string.IsNullOrEmpty(text))
            {
                errMsg = String.Format(MessageResources.ErrNoInput, MessageResources.NameStep);
            }
            else if (!Int32.TryParse(text, out dummy))
            {
                errMsg = MessageResources.ErrInvalidValue;
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                Util.ShowErrorDialog(errMsg);
                freqByStepTextBox.Text = Convert.ToString(stepcount);
                e.Cancel = true;
                loggingPage.Focus();
            }
        }

        /// <summary>
        /// Validated the TextBox of by step.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void freqByStepTextBox_Validated(object sender, EventArgs e)
        {
            ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.ReloadStepCount = Int32.Parse(freqByStepTextBox.Text);
            ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.ReloadInterval = 0;
            freqBySecTextBox.Text = "";
            freqBySecTextBox_filledWithDefaultValue = true;
        }

        /// <summary>
        /// Validating the TextBox of by sec.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void freqBySecTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            string text = freqBySecTextBox.Text;
            double interval = ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.ReloadInterval;
            if (interval <= 0.0) interval = m_defaultInterval;
            double dummy;
            string errMsg = "";
            if (string.IsNullOrEmpty(text))
            {
                errMsg = String.Format(MessageResources.ErrNoInput, MessageResources.NameSec);
            }
            else if (!Double.TryParse(text, out dummy))
            {
                errMsg = MessageResources.ErrInvalidValue;
            }
            if (!String.IsNullOrEmpty(errMsg))
            {
                Util.ShowErrorDialog(errMsg);
                freqBySecTextBox.Text = Convert.ToString(interval);
                loggingPage.Focus();
                e.Cancel = true;

            }
        }

        /// <summary>
        /// Validated the TextBox of by sec.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void freqBySecTextBox_Validated(object sender, EventArgs e)
        {
            ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.ReloadInterval = Double.Parse(freqBySecTextBox.Text);
            ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.ReloadStepCount = 0;
            freqByStepTextBox.Text = "";
            freqByStepTextBox_filledWithDefaultValue = true;
        }

        /// <summary>
        /// Check of RadioButton for no limit is changed.
        /// </summary>
        /// <param name="sender">RadioButton</param>
        /// <param name="e">EventArgs</param>
        private void noLimitRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.MaxDiskSpace = 0;
                maxKbTextBox.Enabled = false;
            }
            else
            {
                maxKbTextBox.Enabled = true;
                if (((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.MaxDiskSpace == 0)
                {
                    ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.MaxDiskSpace =
                        SimulationConfigurationDialog.m_defaultMaxLogSize;
                }
                maxKbTextBox.Text = SimulationConfigurationDialog.m_defaultMaxLogSize.ToString();
                maxKbTextBox.Focus();
            }
        }

        /// <summary>
        /// TextBox of by step is changed.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void freqByStepTextBox_TextChanged(object sender, EventArgs e)
        {
            freqByStepTextBox_filledWithDefaultValue = false;
        }

        /// <summary>
        /// TextBox of by sec is changed.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void freqBySecTextBox_TextChanged(object sender, EventArgs e)
        {
            freqBySecTextBox_filledWithDefaultValue = false;
        }

        /// <summary>
        /// Occur Error event on DataGridView of initial parameters.
        /// </summary>
        /// <param name="sender">BindingSource</param>
        /// <param name="e">BindingManagerDataErrorEventArgs</param>
        private void initialConditionsBindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
        {
            Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
            tabControl1.Focus();
        }

        /// <summary>
        /// Occur Error event on property of logger.
        /// </summary>
        /// <param name="sender">BindingSource</param>
        /// <param name="e">BindingManagerDataErrorEventArgs</param>
        private void propertiesBindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
        {
            Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
            tabControl1.Focus();
        }

        /// <summary>
        /// Validated the TextBox of max KB,
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void maxKbTextBox_Validated(object sender, EventArgs e)
        {
            if (maxSizeRadio.Checked)
            {
                ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.MaxDiskSpace = Convert.ToInt32(maxKbTextBox.Text);
            }
            else
            {
                ((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.MaxDiskSpace = 0;
            }
        }

        /// <summary>
        /// Validating the TextBox of max KB.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs</param>
        private void maxKbTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (maxSizeRadio.Checked)
            {
                int dummy;
                string errMsg = "";
                string text = maxKbTextBox.Text;
                if (String.IsNullOrEmpty(text))
                {
                    errMsg = String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxSize);
                }
                else if (!Int32.TryParse(text, out dummy))
                {
                    errMsg = MessageResources.ErrInvalidValue;
                }
                if (!String.IsNullOrEmpty(errMsg))
                {
                    Util.ShowErrorDialog(errMsg);
                    maxKbTextBox.Text =
                        Convert.ToString(((SimulationParameterSet)m_simParamSets.Current).LoggerPolicy.MaxDiskSpace);
                    e.Cancel = true;
                    loggingPage.Focus();
                    return;
                }
            }
        }

        /// <summary>
        /// Occur error event on DataGridView of the initial parameters.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewDataErrorEventArgs</param>
        private void InitialParameterDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
            initialConditionsBindingSource.ResetBindings(false);
        }

        /// <summary>
        /// Closing this window. Check the input parameter.
        /// </summary>
        /// <param name="sender">SimulationConfigurationDialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void SimulationConfigurationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (freqByStepRadio.Checked)
            {
                string text = freqByStepTextBox.Text;
                int dummy;
                if (!Int32.TryParse(text, out dummy))
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    e.Cancel = true;
                    return;
                }
                else if (dummy <= 0 || Double.IsInfinity(dummy))
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    e.Cancel = true;
                    return;
                }
            }

            if (freqBySecRadio.Checked)
            {
                string text = freqBySecTextBox.Text;
                double dummy;
                if (!Double.TryParse(text, out dummy))
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    e.Cancel = true;
                    return;
                }
                else if (dummy <= 0.0 || Double.IsInfinity(dummy))
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    e.Cancel = true;
                    return;
                }
            }

            if (maxSizeRadio.Checked)
            {
                int dummy;
                string text = maxKbTextBox.Text;
                if (!Int32.TryParse(text, out dummy))
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    e.Cancel = true;
                    return;
                }
                else if (dummy <= 0)
                {
                    Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                    e.Cancel = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Loading the this window. Set tool tip to Button.
        /// </summary>
        /// <param name="sender">SimulationConfigurationDialog</param>
        /// <param name="e">EventArgs</param>
        private void SimulationConfigurationDialog_Load(object sender, EventArgs e)
        {
            simSettingToolTip.SetToolTip(SSCreateButton, MessageResources.DialogToolTipCreSim);
            simSettingToolTip.SetToolTip(SSDeleteButton, MessageResources.DialogToolTipDeleteSim);
        }

        /// <summary>
        /// Opening the context menu on DataGridView of the initial parameters.
        /// </summary>
        /// <param name="sender">ContextMenuToolStrip</param>
        /// <param name="e">CancelEventArgs</param>
        private void initialContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (initialParameters.SelectedCells.Count <= 0)
                e.Cancel = true;
        }

        /// <summary>
        /// Delete the data of initial parameters.
        /// </summary>
        /// <param name="sender">MenuToolStripItem</param>
        /// <param name="e">EventArgs</param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string simParam = paramCombo.Text;
            string modelID = modelCombo.Text;
            foreach (DataGridViewCell r in initialParameters.SelectedCells)
            {
                int index = r.RowIndex;
                string entityPath = initialParameters[keyDataGridViewTextBoxColumn1.Index, index].Value.ToString();
                double data = Convert.ToDouble(initialParameters[valueDataGridViewTextBoxColumn1.Index, index].Value.ToString());
//                m_owner.DataManager.CurrentProject.DeleteInitialCondition(simParam, entityPath);

                PerModelSimulationParameter p = ((SimulationParameterSet)m_simParamSets.Current).PerModelSimulationParameters[0];
                foreach (MutableKeyValuePair<string, double> delData in p.InitialConditions)
                {
                    if (delData.Key.Equals(entityPath))
                    {
                        initialConditionsBindingSource.Remove(delData);
                        break;
                    }
                }
//                initialConditionsBindingSource.DataSource = p.InitialConditions;
                initialParameters.Refresh();
            }
        }

        /// <summary>
        /// Import the data of simulation parameter from the file.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void ImportSimulationParameterClicked(object sender, EventArgs e)
        {
            if (m_owner.DataManager.CurrentProject == null)
                return;

            SSOpenFileDialog.Filter = Constants.FilterCSVFile;
            SSOpenFileDialog.FileName = "";
            if (SSOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string parameterID = System.IO.Path.GetFileNameWithoutExtension(SSOpenFileDialog.FileName);
                try
                {                    
                    SimulationParameterSet sps = null;
                    bool isHit = false;
                    foreach (SimulationParameterSet s in m_simParamSets)
                    {
                        if (s.Name.Equals(parameterID))
                            isHit = true;
                    }
                    if (isHit == false)
                    {
                        sps = new SimulationParameterSet(parameterID);
                        SimulationParameterSet sp = (SimulationParameterSet)m_simParamSets.Current;
                        for (int i = 0; i < sp.PerModelSimulationParameters.Count; i++)
                        {
                            sps.PerModelSimulationParameters.Add(new PerModelSimulationParameter(sp.PerModelSimulationParameters[i]));
                        }
                        m_simParamSets.Add(sps);                        
                    }

                    int index = m_simParamSets.IndexOf(sps);
                    m_simParamSets.Position = index;

                    foreach (KeyValuePair<string, double> pair in
                            SimulationParameter.ConvertSimulationParameter(SSOpenFileDialog.FileName))
                    {
                        PerModelSimulationParameter p = ((SimulationParameterSet)m_simParamSets.Current).PerModelSimulationParameters[0];
                        bool isHitParam = false;
                        foreach (MutableKeyValuePair<string, double> delData in p.InitialConditions)
                        {
                            if (delData.Key.Equals(pair.Key))
                            {
                                if (delData.Value == pair.Value)
                                    isHitParam = true;
                                else
                                    p.InitialConditions.Remove(delData);
                                break;
                            }
                        }
                        if (!isHitParam)
                        {
                            initialConditionsBindingSource.Add(
                                KeyValuePairConverter<string, double>.Convert(pair));
                        }
                    }
                    initialParameters.Refresh();
                }
                catch (Exception)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrImportSim, parameterID));
                    return;
                }
                Util.ShowNoticeDialog(String.Format(MessageResources.InfoImportSim, parameterID));
            }
        }

        /// <summary>
        /// Export the data of simulation sets to the file.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void ExportSimulationParameterClicked(object sender, EventArgs e)
        {
            if (m_owner.DataManager.CurrentProject == null)
                return;
            SSSaveFileDialog.Filter = Constants.FilterCSVFile;
            SSSaveFileDialog.FileName = "";

            if (SSSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string modelID = m_owner.DataManager.CurrentProject.Model.ModelID;
                string parameterID = paramCombo.Text; 
                string fileName = SSSaveFileDialog.FileName;
                try
                {
                    m_owner.DataManager.ExportSimulationParameter(modelID, parameterID, fileName);
                }
                catch (Exception)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrExportSim, parameterID));
                    return;
                }
                Util.ShowNoticeDialog(String.Format(MessageResources.InfoExportSim, parameterID));
            }
        }
        #endregion
    }
}
