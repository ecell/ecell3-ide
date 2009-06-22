//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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

using Ecell.IDE;
using Ecell.Objects;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Setting Dialog for Bifurcation analysis.
    /// </summary>
    public partial class BifurcationSettingDialog : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// Plugin object.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// Parameter object for Bifurcation analysis.
        /// </summary>
        private BifurcationAnalysisParameter m_param;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="owner">Plugin object.</param>
        public BifurcationSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }
        #endregion

        /// <summary>
        /// Set the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <param name="p">the parameter set of bifurcation analysis.</param>
        public void SetParameter(BifurcationAnalysisParameter p)
        {            
            bifurcationSimulationTimeTextBox.Text = Convert.ToString(p.SimulationTime);
            bifurcationWindowSizeTextBox.Text = Convert.ToString(p.WindowSize);
            bifurcationMaxInputTextBox.Text = Convert.ToString(p.MaxInput);
            bifurcationMaxFrequencyTextBox.Text = Convert.ToString(p.MaxFreq);
            bifurcationMinFrequencyTextBox.Text = Convert.ToString(p.MinFreq);
            m_param = p;
        }

        /// <summary>
        /// Get the parameter set of bifurcation analysis in this form.
        /// </summary>
        /// <returns>the parameter set of bifurcation analysis.</returns>
        public BifurcationAnalysisParameter GetParameter()
        {
            return m_param;
        }

        /// <summary>
        /// Remove the parameter data.
        /// </summary>
        /// <param name="data">the removed parameter data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            foreach (DataGridViewRow r in bifurcationParameterDataGrid.Rows)
            {
                string fullPN = r.Cells[paramFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    bifurcationParameterDataGrid.Rows.Remove(r);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove the observed data.
        /// </summary>
        /// <param name="data">the removed observed data.</param>
        public void RemoveObservedData(EcellObservedData data)
        {
            foreach (DataGridViewRow r in bifurcationObservedDataGrid.Rows)
            {
                string fullPN = r.Cells[observedFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    bifurcationObservedDataGrid.Rows.Remove(r);
                    return;
                }
            }
        }        

        /// <summary>
        /// Set the parameter data.
        /// </summary>
        /// <param name="data">the set parameter data.</param>
        public void SetParameterData(EcellParameterData data)
        {
            foreach (DataGridViewRow r1 in bifurcationParameterDataGrid.Rows)
            {
                string fullPN = r1.Cells[paramFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    // max
                    int index = dataGridViewTextBoxColumn2.Index;
                    r1.Cells[index].Value = data.Max;

                    // min
                    index = dataGridViewTextBoxColumn3.Index;
                    r1.Cells[index].Value = data.Min;

                    // step
                    index = dataGridViewTextBoxColumn4.Index;
                    r1.Cells[index].Value = data.Step;

                    return;
                }
            }
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = data.Key;
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = data.Max;
            r.Cells.Add(c2);

            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = data.Min;
            r.Cells.Add(c3);

            if (bifurcationParameterDataGrid.ColumnCount >= 4)
            {
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = data.Step;
                r.Cells.Add(c4);
            }

            r.Tag = data.Copy();

            bifurcationParameterDataGrid.Rows.Add(r);
        }

        /// <summary>
        /// Set the observed data.
        /// </summary>
        /// <param name="data">the set observed data.</param>
        public void SetObservedData(EcellObservedData data)
        {
            foreach (DataGridViewRow r1 in bifurcationObservedDataGrid.Rows)
            {
                string fullPN = r1.Cells[observedFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    // max
                    int index = dataGridViewTextBoxColumn6.Index;
                    r1.Cells[index].Value = data.Max;

                    // min
                    index = dataGridViewTextBoxColumn7.Index;
                    r1.Cells[index].Value = data.Min;

                    // differ
                    index = dataGridViewTextBoxColumn8.Index;
                    r1.Cells[index].Value = data.Differ;

                    // rate
                    index = dataGridViewTextBoxColumn9.Index;
                    r1.Cells[index].Value = data.Rate;

                    return;
                }
            }
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            c1.Value = data.Key;
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = data.Max;
            r.Cells.Add(c2);

            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = data.Min;
            r.Cells.Add(c3);

            DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
            c4.Value = data.Differ;
            r.Cells.Add(c4);

            DataGridViewTextBoxCell c5 = new DataGridViewTextBoxCell();
            c5.Value = data.Rate;
            r.Cells.Add(c5);

            r.Tag = data.Copy();
            bifurcationObservedDataGrid.Rows.Add(r);
        }

        /// <summary>
        /// Get the list of observed data.
        /// </summary>
        /// <returns>The list of EcellObservedData.</returns>
        public List<EcellObservedData> GetObservedDataList()
        {
            List<EcellObservedData> result = new List<EcellObservedData>();
            for (int i = 0; i < bifurcationObservedDataGrid.Rows.Count; i++)
            {
                EcellObservedData data = bifurcationObservedDataGrid.Rows[i].Tag as EcellObservedData;
                result.Add(data);
            }
            return result;
        }

        /// <summary>
        /// Get the list of parameter data.
        /// </summary>
        /// <returns>The list of EcellParameterData.</returns>
        public List<EcellParameterData> GetParameterDataList()
        {
            List<EcellParameterData> result = new List<EcellParameterData>();
            for (int i = 0; i < bifurcationParameterDataGrid.Rows.Count; i++)
            {
                EcellParameterData data = bifurcationParameterDataGrid.Rows[i].Tag as EcellParameterData;
                result.Add(data);
            }
            return result;
        }

        #region Events
        /// <summary>
        /// The event to load the form.
        /// </summary>
        /// <param name="sender">BifurcationSettingDialog.</param>
        /// <param name="e">EventArgs</param>
        private void FormLoad(object sender, EventArgs e)
        {
            bifurcationToolTip.SetToolTip(bifurcationSimulationTimeTextBox,
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            bifurcationToolTip.SetToolTip(bifurcationWindowSizeTextBox, 
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            bifurcationToolTip.SetToolTip(bifurcationMaxFrequencyTextBox,
                String.Format(MessageResources.CommonToolTipMoreThanUpper, 0.0, MessageResources.NameMinFrequency));
            bifurcationToolTip.SetToolTip(bifurcationMinFrequencyTextBox,
                String.Format(MessageResources.CommonToolTipMoreThanLower, 0.0, MessageResources.NameMaxFrequency));
            bifurcationToolTip.SetToolTip(bifurcationMaxInputTextBox,
                String.Format(MessageResources.CommonToolTipRange, 0, 2097152));     
        }

        /// <summary>
        /// Validating the value of window size.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void WindowSize_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationWindowSizeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameWindowSize));
                bifurcationWindowSizeTextBox.Text = Convert.ToString(m_param.WindowSize);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWindowSize));
                bifurcationWindowSizeTextBox.Text = Convert.ToString(m_param.WindowSize);
                e.Cancel = true;
                return;
            }
            m_param.WindowSize = dummy;
        }

        /// <summary>
        /// Validating the value of simulation time.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void SimulationTime_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationSimulationTimeTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameSimulationTime));
                bifurcationSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                bifurcationSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            m_param.SimulationTime = dummy;
        }

        /// <summary>
        /// Validating the value of max input for FFT.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void MaxInput_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationMaxInputTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxSample));
                bifurcationMaxInputTextBox.Text = Convert.ToString(m_param.MaxInput);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxSample));
                bifurcationMaxInputTextBox.Text = Convert.ToString(m_param.MaxInput);
                e.Cancel = true;
                return;
            }
            m_param.MaxInput = dummy;
        }

        /// <summary>
        /// Validating the value of max frequency for FFT.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void MaxFrequency_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationMaxFrequencyTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxFrequency));
                bifurcationMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxFrequency));
                bifurcationMaxFrequencyTextBox.Text = Convert.ToString(m_param.MaxFreq);
                e.Cancel = true;
                return;
            }
            m_param.MaxFreq = dummy;
        }

        /// <summary>
        /// Validating the value of min frequency of FFT.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void MinFrequency_Validating(object sender, CancelEventArgs e)
        {
            string text = bifurcationMinFrequencyTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMinFrequency));
                bifurcationMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMinFrequency));
                bifurcationMinFrequencyTextBox.Text = Convert.ToString(m_param.MinFreq);
                e.Cancel = true;
                return;
            }
            m_param.MinFreq = dummy;
        }

        /// <summary>
        /// Change the property of data on DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void ObservedDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellObservedData data = bifurcationObservedDataGrid.Rows[e.RowIndex].Tag as EcellObservedData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value == null ||
                !double.TryParse(bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
            {
                isCorrect = false;
            }

            if (isCorrect)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        data.Max = dummy;
                        break;
                    case 2:
                        data.Min = dummy;
                        break;
                    case 3:
                        data.Differ = dummy;
                        break;
                    case 4:
                        data.Rate = dummy;
                        break;
                }
                try
                {
                    m_owner.NotifyObservedDataChanged(data);
                }
                catch (Exception)
                {
                    isCorrect = false;
                }
            }
            
            if (!isCorrect)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, 
                    MessageResources.NameObservedData));
                switch (e.ColumnIndex)
                {
                    case 1:
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Differ;
                        break;
                    case 4:
                        bifurcationObservedDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Rate;
                        break;
                }
            }
        }

        /// <summary>
        /// Change the property of data on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void ParameterDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellParameterData data = bifurcationParameterDataGrid.Rows[e.RowIndex].Tag as EcellParameterData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value == null ||
                !double.TryParse(bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
            {
                isCorrect = false;
            }

            if (isCorrect)
            {
                switch (e.ColumnIndex)
                {
                    case 1:
                        data.Max = dummy;
                        break;
                    case 2:
                        data.Min = dummy;
                        break;
                    case 3:
                        data.Step = dummy;
                        break;
                }
                try
                {
                    m_owner.NotifyParameterDataChanged(data);
                }
                catch (Exception)
                {
                    isCorrect = false;
                }
            }
            if (!isCorrect)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue,
                    MessageResources.NameParameterData));
                switch (e.ColumnIndex)
                {
                    case 1:
                        bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        bifurcationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Step;
                        break;
                }
            }

        }

        /// <summary>
        /// Opening the ContextMenuStrip of DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">ContextMenuStrip.</param>
        /// <param name="e">CancelEventArgs</param>
        private void observedConextMenuOpening(object sender, CancelEventArgs e)
        {
            if (bifurcationObservedDataGrid.SelectedCells.Count <= 0)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Opening the ContextMenuStrip of DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">ContextMenuStrip</param>
        /// <param name="e">CancelEventArgs</param>
        private void paramContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            if (bifurcationParameterDataGrid.SelectedCells.Count <= 0)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Click the delete menu on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">MenuToolStripItem</param>
        /// <param name="e">EventArgs</param>
        private void DeleteParamClick(object sender, EventArgs e)
        {
            List<string> delList = new List<string>();
            foreach (DataGridViewCell c in bifurcationParameterDataGrid.SelectedCells)
            {
                string fullPN = bifurcationParameterDataGrid.Rows[c.RowIndex].Cells[paramFullPNColumn.Index].Value.ToString();
                if (!delList.Contains(fullPN))
                    delList.Add(fullPN);
            }

            foreach (string r in delList)
            {
                m_owner.Environment.DataManager.RemoveParameterData(
                    new EcellParameterData(r, 0.0));
            }
        }

        /// <summary>
        /// Click the delete menu on DataGridView of the observed data.
        /// </summary>
        /// <param name="sender">MenuToolStripItem</param>
        /// <param name="e">EventArgs</param>
        private void ObservedDeleteClick(object sender, EventArgs e)
        {            
            List<string> delList = new List<string>();
            foreach (DataGridViewCell c in bifurcationObservedDataGrid.SelectedCells)
            {
                string fullPN = bifurcationObservedDataGrid.Rows[c.RowIndex].Cells[observedFullPNColumn.Index].Value.ToString();
                if (!delList.Contains(fullPN))
                    delList.Add(fullPN);
            }

            foreach (string r in delList)
            {
                m_owner.Environment.DataManager.RemoveObservedData(
                    new EcellObservedData(r, 0.0));
            }
        }

        /// <summary>
        /// Click the button to execute the analysis.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void ExecuteButtonClick(object sender, EventArgs e)
        {
            executeButton.Enabled = false;
            if (m_param.SimulationTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.WindowSize <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameWindowSize));
                executeButton.Enabled = true;
                return;
            }

            if (m_param.MaxInput <= 0 || m_param.MaxInput > 2097152)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxSample));
                executeButton.Enabled = true;
                return;
            }

            if (m_param.MinFreq <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMinFrequency));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.MaxFreq <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxFrequency));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.MaxFreq < m_param.MinFreq)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrLarger, MessageResources.NameMaxFrequency, MessageResources.NameMinFrequency));
                executeButton.Enabled = true;
                return;
            }

            List<EcellParameterData> plist = GetParameterDataList();
            foreach (EcellParameterData p in plist)
            {
                if (p.Max < p.Min || p.Step < 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameParameterData));
                    executeButton.Enabled = true;
                    return;
                }
            }

            List<EcellObservedData> olist = GetObservedDataList();
            foreach (EcellObservedData o in olist)
            {
                if (o.Max < o.Min || o.Rate < 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameParameterData));
                    executeButton.Enabled = true;
                    return;
                }
            }
            m_owner.ExecuteBifurcationAnalysis();
            executeButton.Enabled = true;
        }
        #endregion
    }
}