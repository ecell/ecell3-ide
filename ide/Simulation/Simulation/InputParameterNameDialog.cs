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
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Simulation
{
    public partial class InputParameterNameDialog : Form
    {
        #region Fields
        /// <summary>
        /// DataManager.
        /// </summary>
        SimulationConfigurationDialog m_owner;
        #endregion


        public string InputText
        {
            get { return paramTextBox.Text; }
        }

        /// <summary>
        /// Constructor for NewParameterWindow.
        /// </summary>
        internal InputParameterNameDialog(SimulationConfigurationDialog owner)
        {
            m_owner = owner;
            InitializeComponent();
        }

        #region Event
        /// <summary>
        /// Event when this form is shown.
        /// </summary>
        /// <param name="sender">Form.</param>
        /// <param name="e">EventArgs.</param>
        private void ShowCreateParameterWin(object sender, EventArgs e)
        {
            this.paramTextBox.Focus();
        }
        #endregion

        private void InputParameterNameDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
                return;

            if (String.IsNullOrEmpty(paramTextBox.Text))
            {
                Util.ShowWarningDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameName));
                e.Cancel = true;
                return;
            }
            if (Util.IsNGforID(paramTextBox.Text))
            {
                Util.ShowWarningDialog(MessageResources.ErrIDNG);
                e.Cancel = true;
                return;
            }
        }
    }
}
