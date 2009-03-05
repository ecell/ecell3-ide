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

namespace Ecell.IDE.Plugins.Analysis
{
    /// <summary>
    /// Dialog to set the parameter of parameter estimation.
    /// </summary>
    public partial class ParameterEstimationSettingDialog : Form
    {
        private Analysis m_owner;
        /// <summary>
        /// The form to set the estimation formulator.
        /// </summary>
        private FormulatorDialog m_fwin;
        private ParameterEstimationParameter m_param;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner"></param>
        public ParameterEstimationSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
        }

        /// <summary>
        /// Set the parameter and display the set parameter.
        /// </summary>
        /// <param name="param"></param>
        public void SetParameter(ParameterEstimationParameter param)
        {
            parameterEstimationSimulationTimeTextBox.Text = Convert.ToString(param.SimulationTime);
            parameterEstimationPopulationTextBox.Text = Convert.ToString(param.Population);
            parameterEstimationGenerationTextBox.Text = Convert.ToString(param.Generation);

            m_param = param;
        }

        /// <summary>
        /// Get the parameter displayed this dialog.
        /// </summary>
        /// <returns></returns>
        public ParameterEstimationParameter GetParameter()
        {
            return m_param;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dic"></param>
        public void SetParameterDataList(Dictionary<string, EcellData> dic)
        {
            foreach (string key in dic.Keys)
            {
                if (!m_owner.DataManager.IsContainsParameterData(key)) continue;
                EcellParameterData d = m_owner.DataManager.GetParameterData(key);
                SetParameterData(d);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetParameterData(EcellParameterData data)
        {
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

            if (parameterEstimationParameterDataGrid.ColumnCount >= 4)
            {
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = data.Step;
                r.Cells.Add(c4);
            }

            r.Tag = data.Copy();

            parameterEstimationParameterDataGrid.Rows.Add(r);
        }

                /// <summary>
        /// The event sequence when the user remove the data from the list of parameter data.
        /// </summary>
        /// <param name="data">The removed parameter data.</param>
        public void RemoveParameterData(EcellParameterData data)
        {
            foreach (DataGridViewRow r in parameterEstimationParameterDataGrid.Rows)
            {
                if (r.Tag == null) continue;
                EcellParameterData obj = r.Tag as EcellParameterData;
                if (obj == null) continue;

                if (!obj.Equals(data)) continue;
                parameterEstimationParameterDataGrid.Rows.Remove(r);
                return;
            }
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
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLoad(object sender, EventArgs e)
        {
            parameterEstimationToolTip.SetToolTip(parameterEstimationSimulationTimeTextBox, MessageResources.ToolTipSimulationTime);
            parameterEstimationToolTip.SetToolTip(parameterEstimationGenerationTextBox, MessageResources.ToolTipGeneration);
            parameterEstimationToolTip.SetToolTip(parameterEstimationPopulationTextBox, MessageResources.ToolTipPopulation);
            parameterEstimationToolTip.SetToolTip(estimationFormulatorTextBox, MessageResources.ToopTipEstimation);
            parameterEstimationToolTip.SetToolTip(groupBox3, MessageResources.ToolTipUnknownParameterGrid);
            estimationTypeComboBox.SelectedIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvancedButtonClicked(object sender, EventArgs e)
        {
            ParameterEstimationAdvancedSettingDialog dlg = new ParameterEstimationAdvancedSettingDialog();
            SimplexCrossoverParameter p = new SimplexCrossoverParameter(m_param.Param.M,
                m_param.Param.Initial, m_param.Param.Max, m_param.Param.K, m_param.Param.Upsilon);
            dlg.SetParameter(p);
            using (dlg)
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_param.Param = dlg.GetParam();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            }
            else
            {
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

        private void ParameterEstimationSettingDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            if (m_param.Generation <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameGenerationNum));
                e.Cancel = true;
                return;
            }
            if (m_param.Population <= 0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NamePopulation));
                e.Cancel = true;
                return;
            }
            if (m_param.SimulationTime <= 0.0)
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameSimulationTime));
                e.Cancel = true;
                return;
            }

            List<EcellParameterData> plist = GetParameterDataList();
            foreach (EcellParameterData p in plist)
            {
                if (p.Max < p.Min || p.Step < 0.0)
                {
                    Util.ShowErrorDialog(String.Format(MessageResources.ErrInvalidValue, MessageResources.NameParameterData));
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}