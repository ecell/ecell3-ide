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
    public partial class NewParameterWindow : Form
    {
        #region Fields
        /// <summary>
        /// DataManager.
        /// </summary>
        DataManager m_dManager;
        /// <summary>
        /// the parent window with SimulationSetupWindow.
        /// </summary>
        SimulationSetup m_win;
        /// <summary>
        /// ResourceManager for NewParameterWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResSimulation));
        #endregion

        /// <summary>
        /// Constructor for NewParameterWindow.
        /// </summary>
        public NewParameterWindow()
        {
            InitializeComponent();
            m_dManager = DataManager.GetDataManager();
        }

        /// <summary>
        /// Set SimulationSetup to the parent window.
        /// </summary>
        /// <param name="s"></param>
        public void SetParentWindow(SimulationSetup s)
        {
            m_win = s;
        }

        #region Event
        /// <summary>
        /// The action of clicking Add button in SimulationSetupWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void AddStepperClick(object sender, EventArgs e)
        {
            string data = paramTextBox.Text;
            if (data == "" || data == null)
            {
                String errmes = m_resources.GetString("ErrNoInput");
                MessageBox.Show(errmes, "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (m_win.stepperListBox.Items.Contains(data))
            {
                String errmes = m_resources.GetString("ErrAlready");
                MessageBox.Show(errmes, "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Util.IsNGforID(data))
            {
                String errmes = m_resources.GetString("ErrIDNG");
                MessageBox.Show(errmes, "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string paramID = m_win.paramCombo.Text;
            string modelID = m_win.modelCombo.Text;
            string stepperID = m_win.stepCombo.Text;
            Dictionary<string, EcellData> propDict =
                m_dManager.GetStepperProperty(stepperID);
            List<EcellData> list = new List<EcellData>();
            foreach (string key in propDict.Keys)
            {
                if (propDict[key].M_name == "ProcessList" || propDict[key].M_name == "SystemList" ||
                    propDict[key].M_name == "ReadVariableList" ||
                    propDict[key].M_name == "WriteVariableList")
                {
                    EcellData d = propDict[key];
                    List<EcellValue> nulllist = new List<EcellValue>();
                    d.M_value = new EcellValue(nulllist);
                    list.Add(d);
                }
                else
                {
                    list.Add(propDict[key]);
                }
            }
            EcellObject obj = EcellObject.CreateObject(modelID, data, "Stepper", stepperID, list);
            m_dManager.AddStepperID(paramID, obj);
            m_win.stepperListBox.Items.Add(data);
            m_win.SetStepperList(m_dManager.GetStepper(paramID, modelID));
            Close();
            Dispose();
        }

        /// <summary>
        /// The action of clicking cancel button in new stepper window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelStepperClick(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        /// <summary>
        /// The action of clicking cancel button in NewParameterWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void CancelParameterClick(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// The action of clicking ok button in NewParameterWindow.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void NewParameterClick(object sender, EventArgs e)
        {
            if (paramTextBox.Text == "")
            {
                String errmes = m_resources.GetString("ErrNoInput");
                MessageBox.Show(errmes, "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Util.IsNGforID(paramTextBox.Text))
            {
                String errmes = m_resources.GetString("ErrIDNG");
                MessageBox.Show(errmes, "WARNING",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string newParamName = paramTextBox.Text;

            m_dManager.NewSimulationParameter(newParamName);
            m_win.paramCombo.Items.Add(newParamName);
            m_win.paramCombo.SelectedItem = newParamName;
            Dispose();
        }
        #endregion

        private void CreateParameterShown(object sender, EventArgs e)
        {
            this.paramTextBox.Focus();
        }
    }
}