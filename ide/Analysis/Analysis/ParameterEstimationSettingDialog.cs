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
        /// <summary>
        /// The user control to set the estimation formulator.
        /// </summary>
        private FormulatorControl m_fcnt;
        private SimplexCrossoverParameter m_parameter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner"></param>
        public ParameterEstimationSettingDialog(Analysis owner)
        {
            InitializeComponent();
            m_owner = owner;
            m_parameter = new SimplexCrossoverParameter();
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

            m_parameter = param.Param;
        }

        /// <summary>
        /// Get the parameter displayed this dialog.
        /// </summary>
        /// <returns></returns>
        public ParameterEstimationParameter GetParameter()
        {
            string estForm = estimationFormulatorTextBox.Text;
            double simTime = Convert.ToDouble(parameterEstimationSimulationTimeTextBox.Text);
            int popNum = Convert.ToInt32(parameterEstimationPopulationTextBox.Text);
            int genNum = Convert.ToInt32(parameterEstimationGenerationTextBox.Text);
            EstimationFormulatorType type = GetFormulatorType();

            return new ParameterEstimationParameter(estForm, simTime, popNum, genNum, type, m_parameter);
        }

        public void SetParameterDataList(Dictionary<string, EcellData> dic)
        {
            foreach (string key in dic.Keys)
            {
                if (!m_owner.DataManager.IsContainsParameterData(key)) continue;
                EcellParameterData d = m_owner.DataManager.GetParameterData(key);
                SetParameterData(d);
            }
        }

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

            r.Tag = data;

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
        /// The event sequence when the formulator window is shown.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void formulatorButtonClicked(object sender, EventArgs e)
        {
            DataManager manager = m_owner.DataManager;
            m_fwin = new FormulatorDialog();
            m_fcnt = new FormulatorControl();
            m_fwin.tableLayoutPanel.Controls.Add(m_fcnt, 0, 0);
            m_fcnt.Dock = DockStyle.Fill;
            m_fcnt.IsExpression = false;

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
            m_fcnt.AddReserveString(list);
            m_fcnt.ImportFormulate(estimationFormulatorTextBox.Text);

            using (m_fwin)
            {
                DialogResult res = m_fwin.ShowDialog();
                if (res == DialogResult.OK)
                {
                    string ext = m_fcnt.ExportFormulate();
                    estimationFormulatorTextBox.Text = ext;
                }
            }
        }
    }
}