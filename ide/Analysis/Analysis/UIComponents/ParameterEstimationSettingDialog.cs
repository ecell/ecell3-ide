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
using Ecell.UI.Components;
using Ecell.Objects;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Setting Dialog for parameter estimation.
    /// </summary>
    public partial class ParameterEstimationSettingDialog : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// Plugin object.
        /// </summary>
        private Analysis m_owner;
        /// <summary>
        /// Parameter object for parameter estimation.
        /// </summary>
        private ParameterEstimationParameter m_param;
        /// <summary>
        /// The form to set the estimation formulator.
        /// </summary>
        private FormulatorDialog m_fwin;
        /// <summary>
        /// The parameter of simplex crossover parameter.
        /// </summary>
        private SimplexCrossoverParameter m_simParam;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner">Plugin object</param>
        public ParameterEstimationSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }
        #endregion

        /// <summary>
        /// Set the parameter and display the set parameter.
        /// </summary>
        /// <param name="param">parameter object.</param>
        public void SetParameter(ParameterEstimationParameter param)
        {
            parameterEstimationSimulationTimeTextBox.Text = Convert.ToString(param.SimulationTime);
            parameterEstimationPopulationTextBox.Text = Convert.ToString(param.Population);
            parameterEstimationGenerationTextBox.Text = Convert.ToString(param.Generation);
            estimationFormulatorTextBox.Text = param.EstimationFormulator;

            m_param = param;
            m_simParam = param.Param;

            PEMTextBox.Text = Convert.ToString(m_simParam.M);
            PEUpsilonTextBox.Text = Convert.ToString(m_simParam.Upsilon);
            PEM0TextBox.Text = Convert.ToString(m_simParam.Initial);
            PEKTextBox.Text = Convert.ToString(m_simParam.K);
            PEMaxRateTextBox.Text = Convert.ToString(m_simParam.Max);
        }

        /// <summary>
        /// Get the parameter displayed this dialog.
        /// </summary>
        /// <returns>the input parameter object.</returns>
        public ParameterEstimationParameter GetParameter()
        {
            m_param.Param = m_simParam;
            return m_param;
        }

        /// <summary>
        /// Close project
        /// </summary>
        public void Clear()
        {
            parameterEstimationParameterDataGrid.Rows.Clear();
            estimationFormulatorTextBox.Text = "";
        }

        /// <summary>
        /// Remove the parameter data.
        /// </summary>
        /// <param name="data">the removed parameter data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            foreach (DataGridViewRow r in parameterEstimationParameterDataGrid.Rows)
            {
                string fullPN = r.Cells[paramFullPNColumn.Index].Value.ToString();
                if (fullPN.Equals(data.Key))
                {
                    parameterEstimationParameterDataGrid.Rows.Remove(r);
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
            foreach (DataGridViewRow r1 in parameterEstimationParameterDataGrid.Rows)
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

                    r1.Tag = data.Copy();
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

            r.Tag = data.Copy();

            parameterEstimationParameterDataGrid.Rows.Add(r);
        }

        /// <summary>
        /// Get the estimation type.
        /// </summary>
        /// <returns>Estimation type.</returns>
        private EstimationFormulatorType GetFormulatorType()
        {
            EstimationFormulatorType type = EstimationFormulatorType.Max;
            if (estimationTypeComboBox.SelectedIndex == 1)
            {
                type = EstimationFormulatorType.SumMax;
            }
            else if (estimationTypeComboBox.SelectedIndex == 2)
            {
                type = EstimationFormulatorType.Min;
            }
            else if (estimationTypeComboBox.SelectedIndex == 3)
            {
                type = EstimationFormulatorType.SumMin;
            }
            else if (estimationTypeComboBox.SelectedIndex == 4)
            {
                type = EstimationFormulatorType.EqualZero;
            }
            else if (estimationTypeComboBox.SelectedIndex == 5)
            {
                type = EstimationFormulatorType.SumEqualZero;
            }
            return type;
        }

        /// <summary>
        /// Get the list of parameter data.
        /// </summary>
        /// <returns>The list of EcellParameterData.</returns>
        public List<EcellParameterData> GetParameterDataList()
        {
            List<EcellParameterData> result = new List<EcellParameterData>();
            for (int i = 0; i < parameterEstimationParameterDataGrid.Rows.Count; i++)
            {
                EcellParameterData data = parameterEstimationParameterDataGrid.Rows[i].Tag as EcellParameterData;
                result.Add(data);
            }
            return result;
        }

        /// <summary>
        /// Change the status of project.
        /// </summary>
        /// <param name="status">the status of project.</param>
        public void ChangeStatus(ProjectStatus status)
        {            
            if (status == ProjectStatus.Loaded)
                executeButton.Enabled = true;
            else
                executeButton.Enabled = false;
        }

        #region Events
        /// <summary>
        /// The event sequence when the formulator window is shown.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void formulatorButtonClicked(object sender, EventArgs e)
        {
            DataManager manager = m_owner.DataManager;
            m_fwin = new FormulatorDialog();
            m_fwin.SetExpression(false);

            List<string> list = new List<string>();
            List<string> mlist = manager.GetModelList();
            List<EcellObject> objList = manager.GetData(mlist[0], null);
            if (objList != null)
            {
                foreach (EcellObject obj in objList)
                {
                    if (obj is EcellSystem)
                    {
                        if (obj.Children == null) continue;
                        foreach (EcellObject data in obj.Children)
                        {
                            if (data is EcellProcess)
                            {
                                list.Add("Process:" + data.Key + ":Activity");
                            }
                            else if (data is EcellVariable)
                            {
                                list.Add("Variable:" + data.Key + ":MolarConc");
                            }
                        }
                    }
                }
            }
            m_fwin.AddReserveString(list);
            m_fwin.ImportFormulate(estimationFormulatorTextBox.Text);

            using (m_fwin)
            {
                DialogResult res = m_fwin.ShowDialog();
                if (res == DialogResult.OK)
                {
                    string ext = m_fwin.Result;
                    estimationFormulatorTextBox.Text = ext;
                    m_param.EstimationFormulator = ext;
                }
            }
        }
        /// <summary>
        /// The event to load the form.
        /// </summary>
        /// <param name="sender">ParameterEstimationSettingDialog.</param>
        /// <param name="e">EventArgs</param>
        private void FormLoad(object sender, EventArgs e)
        {
            parameterEstimationToolTip.SetToolTip(parameterEstimationSimulationTimeTextBox,
                String.Format(MessageResources.CommonToolTipMoreThan, 0.0));
            parameterEstimationToolTip.SetToolTip(parameterEstimationGenerationTextBox,
                String.Format(MessageResources.CommonToolTipIntMoreThan, 0));
            parameterEstimationToolTip.SetToolTip(parameterEstimationPopulationTextBox,
                String.Format(MessageResources.CommonToolTipIntMoreThan, 0));
            estimationTypeComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Validating the value of simulation time.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void SimulationTime_Validating(object sender, CancelEventArgs e)
        {
            string text = parameterEstimationSimulationTimeTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameSimulationTime));
                parameterEstimationSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                parameterEstimationSimulationTimeTextBox.Text = Convert.ToString(m_param.SimulationTime);
                e.Cancel = true;
                return;
            }
            m_param.SimulationTime = dummy;
        }

        /// <summary>
        /// Validating the value of population.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void Population_Validating(object sender, CancelEventArgs e)
        {
            string text = parameterEstimationPopulationTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NamePopulation));
                parameterEstimationPopulationTextBox.Text = Convert.ToString(m_param.Population);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePopulation));
                parameterEstimationPopulationTextBox.Text = Convert.ToString(m_param.Population);
                e.Cancel = true;
                return;
            }
            m_param.Population = dummy;
        }

        /// <summary>
        /// Validating the value of generation.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void Generation_Validating(object sender, CancelEventArgs e)
        {
            string text = parameterEstimationGenerationTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameGenerationNum));
                parameterEstimationGenerationTextBox.Text = Convert.ToString(m_param.Generation);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameGenerationNum));
                parameterEstimationGenerationTextBox.Text = Convert.ToString(m_param.Generation);
                e.Cancel = true;
                return;
            }
            m_param.Generation = dummy;
        }

        /// <summary>
        /// Validating the value of M.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void M_Validating(object sender, CancelEventArgs e)
        {
            string text = PEMTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameM));
                PEMTextBox.Text = Convert.ToString(m_simParam.M);
                e.Cancel = true;
                return;
            }
            int dummy;
            if (!Int32.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM));
                PEMTextBox.Text = Convert.ToString(m_simParam.M);
                e.Cancel = true;
                return;
            }
            m_simParam.M = dummy;
        }

        /// <summary>
        /// Validating the value of Upsilon.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void Upsilon_Validating(object sender, CancelEventArgs e)
        {
            string text = PEUpsilonTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameUpsilon));
                PEUpsilonTextBox.Text = Convert.ToString(m_simParam.Upsilon);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameUpsilon));
                PEUpsilonTextBox.Text = Convert.ToString(m_simParam.Upsilon);
                e.Cancel = true;
                return;
            }
            m_simParam.Upsilon = dummy;
        }

        /// <summary>
        /// Validating the value of M0.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void M0_Validating(object sender, CancelEventArgs e)
        {
            string text = PEM0TextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameM0));
                PEM0TextBox.Text = Convert.ToString(m_simParam.Initial);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM0));
                PEM0TextBox.Text = Convert.ToString(m_simParam.Initial);
                e.Cancel = true;
                return;
            }
            m_simParam.Initial = dummy;
        }

        /// <summary>
        /// Validating the value of K.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void K_Validating(object sender, CancelEventArgs e)
        {
            string text = PEKTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameK));
                PEKTextBox.Text = Convert.ToString(m_simParam.K);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameK));
                PEKTextBox.Text = Convert.ToString(m_simParam.K);
                e.Cancel = true;
                return;
            }
            m_simParam.K = dummy;
        }

        /// <summary>
        /// Validating the value of max rate.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">CancelEventArgs.</param>
        private void MaxRate_Validating(object sender, CancelEventArgs e)
        {
            string text = PEMaxRateTextBox.Text;
            if (string.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameMaxRate));
                PEMaxRateTextBox.Text = Convert.ToString(m_simParam.Max);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxRate));
                PEMaxRateTextBox.Text = Convert.ToString(m_simParam.Max);
                e.Cancel = true;
                return;
            }
            m_simParam.Max = dummy;
        }

        /// <summary>
        /// Change the property of data on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void ParameterDataChanged(object sender, DataGridViewCellEventArgs e)
        {
            EcellParameterData data = parameterEstimationParameterDataGrid.Rows[e.RowIndex].Tag as EcellParameterData;
            if (data == null) return;
            double dummy = 0;
            bool isCorrect = true;
            if (parameterEstimationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value == null ||
                !double.TryParse(parameterEstimationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString(), out dummy))
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
                        parameterEstimationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Max;
                        break;
                    case 2:
                        parameterEstimationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Min;
                        break;
                    case 3:
                        parameterEstimationParameterDataGrid[e.ColumnIndex, e.RowIndex].Value = data.Step;
                        break;
                }
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
            System.Threading.Thread.Sleep(1000);
            if (m_param.Generation <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameGenerationNum));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.Population <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePopulation));
                executeButton.Enabled = true;
                return;
            }
            if (m_param.SimulationTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                executeButton.Enabled = true;
                return;
            }


            if (m_simParam.Max <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameMaxRate));
                executeButton.Enabled = true;
                return;
            }
            if (m_simParam.K <= 1.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameK));
                executeButton.Enabled = true;
                return;
            }
            if (m_simParam.Initial <= 1.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM0));
                executeButton.Enabled = true;
                return;
            }
            if (m_simParam.Upsilon <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameUpsilon));
                executeButton.Enabled = true;
                return;
            }
            if (m_simParam.M <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameM));
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
            m_owner.ExecuteParameterEstimation();
            executeButton.Enabled = true;
        }

        /// <summary>
        /// Opening the ContextMenuStrip of DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">ContextMenuStrip</param>
        /// <param name="e">CancelEventArgs</param>
        private void ParamContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (parameterEstimationParameterDataGrid.SelectedCells.Count <= 0)
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
        private void DeleteParameterDataClick(object sender, EventArgs e)
        {
            List<string> delList = new List<string>();
            foreach (DataGridViewCell c in parameterEstimationParameterDataGrid.SelectedCells)
            {
                string fullPN = parameterEstimationParameterDataGrid.Rows[c.RowIndex].Cells[paramFullPNColumn.Index].Value.ToString();
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
        /// Enter the drag object on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DragEventArgs</param>
        private void ParamDataDragEnter(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            EcellDragObject dobj = obj as EcellDragObject;
            foreach (EcellDragEntry ent in dobj.Entries)
            {
                EcellObject t = m_owner.DataManager.GetEcellObject(dobj.ModelID, ent.Key, ent.Type);
                foreach (EcellData d in t.Value)
                {
                    if (d.EntityPath.Equals(ent.Path) && d.Settable && d.Value.IsDouble)
                    {
                        e.Effect = DragDropEffects.Move;
                        return;
                    }
                }
            }

            e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Drop the parameter data on DataGridView of the parameter data.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DragEventArgs</param>
        private void ParamDataDragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            foreach (EcellDragEntry ent in dobj.Entries)
            {
                EcellObject t = m_owner.DataManager.GetEcellObject(dobj.ModelID, ent.Key, ent.Type);
                foreach (EcellData d in t.Value)
                {
                    if (d.EntityPath.Equals(ent.Path) && d.Settable && d.Value.IsDouble)
                    {
                        EcellParameterData data =
                            new EcellParameterData(d.EntityPath, Double.Parse(d.Value.ToString()));
                        m_owner.Environment.DataManager.SetParameterData(data);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}