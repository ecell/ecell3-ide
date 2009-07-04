//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Select the log entry to show the plot window.
    /// </summary>
    public partial class SelectPlotDataDialog : Form
    {
        #region Accessors
        /// <summary>
        /// get the log entry to set x label.
        /// </summary>
        public string X
        {
            get { return this.XplotComboBox.Text; }
        }
        /// <summary>
        /// get the log entry to set y label.
        /// </summary>
        public String Y
        {
            get { return this.YPlotComboBox.Text; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="env">ApplicationEnvironment</param>
        public SelectPlotDataDialog(ApplicationEnvironment env)
        {
            InitializeComponent();

            List<string> resList = env.DataManager.GetLoggerList();
            foreach (String data in resList)
            {
                XplotComboBox.Items.Add(data);
                YPlotComboBox.Items.Add(data);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Closing the SelectPlotDataDialog.
        /// </summary>
        /// <param name="sender">SelectPlotDataDialog</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void ClosingSelectPlotDataDialog(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;

            if (String.IsNullOrEmpty(XplotComboBox.Text) ||
                String.IsNullOrEmpty(YPlotComboBox.Text))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNoInput, MessageResources.NamePlotData));
                e.Cancel = true;
            }
            if (XplotComboBox.Text == YPlotComboBox.Text)
            {
                Util.ShowErrorDialog(MessageResources.ErrSameAxis);
                e.Cancel = true;
            }
        }
        #endregion
    }
}