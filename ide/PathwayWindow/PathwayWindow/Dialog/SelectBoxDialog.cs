﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
// written by by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.PathwayWindow.Dialog
{
    public partial class SelectBoxDialog : PathwayDialog
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="options"></param>
        public SelectBoxDialog(string message, string title, List<string> options)
        {
            InitializeComponent();
            if (message != null)
                this.message.Text = message;
            if (title != null)
                this.Text = title;
            if (options != null)
                this.comboBox.Items.AddRange(options.ToArray());
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Show InputBoxDialog.
        /// It returns the input string. And returns null when canceled.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string Show(string message, string title, List<string> options)
        {
            SelectBoxDialog dialog = new SelectBoxDialog(message, title, options);
            string ans = null;
            if (dialog.ShowDialog() == DialogResult.OK)
                ans = dialog.comboBox.Text;
            dialog.Dispose();
            return ans;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Event on enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                buttonOK.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                buttonCancel.PerformClick();
            }
        }
        #endregion

    }
}