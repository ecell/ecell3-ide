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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE
{
    /// <summary>
    /// Dialog class to add property for process.
    /// </summary>
    public partial class AddPropertyDialog : Form
    {
        private String m_resultName = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddPropertyDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event process when user clicked cancel button,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event process when user clicked add button.
        /// </summary>
        /// <param name="sender">Button(Apply)</param>
        /// <param name="e">EventArgs.</param>
        private void AddPropertyApplyButton_Click(object sender, EventArgs e)
        {
            m_resultName = this.propertyTextBox.Text;
            this.Close();
        }

        /// <summary>
        /// Disply this dialog.
        /// </summary>
        /// <returns></returns>
        public String ShowPropertyDialog()
        {
            this.ShowDialog();
            return m_resultName;
        }

        /// <summary>
        /// Event process when this window is shown.
        /// </summary>
        /// <param name="sender">this window.</param>
        /// <param name="e">EventArgs/</param>
        private void AddPropertyDialogShown(object sender, EventArgs e)
        {
            this.propertyTextBox.Focus();
        }

        private void PropertyKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                okButton.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                cancelButton.PerformClick();
            }
        }
    }
}