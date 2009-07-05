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

using Ecell.UI.Components;

namespace Ecell.IDE
{
    /// <summary>
    /// Formulator setting dialog.
    /// </summary>
    public partial class FormulatorDialog : Form
    {
        #region Fields
        /// <summary>
        /// Formulator control.
        /// </summary>
        private FormulatorControl m_cnt;
        /// <summary>
        /// formulator string.
        /// </summary>
        private String m_result;
        #endregion

        /// <summary>
        /// get the formulator string.
        /// </summary>
        public String Result
        {
            get { return this.m_result; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FormulatorDialog()
        {
            InitializeComponent();

            m_cnt = new FormulatorControl();
            tableLayoutPanel.Controls.Add(m_cnt, 0, 0);
            m_cnt.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Add the reserved string.
        /// </summary>
        /// <param name="list">the list of reserved string.</param>
        public void AddReserveString(List<string> list)
        {
            m_cnt.AddReserveString(list);
        }

        /// <summary>
        /// Import the formulator string.
        /// </summary>
        /// <param name="formu">the formulator string.</param>
        public void ImportFormulate(string formu)
        {
            m_cnt.ImportFormulate(formu);
        }

        /// <summary>
        /// Set whether this is enable to add the expression.
        /// </summary>
        /// <param name="isExpression">the flag whether this is enable to add the expression</param>
        public void SetExpression(bool isExpression)
        {
            m_cnt.IsExpression = isExpression;
        }

        /// <summary>
        /// Closing FormulatorDialog.
        /// </summary>
        /// <param name="sender">FormulatorDialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void FormulatorDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;

            m_result = m_cnt.ExportFormulate();
        }

    }
}