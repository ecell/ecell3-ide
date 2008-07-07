using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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

using System.Windows.Forms;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Form class to set the line style.
    /// </summary>
    public partial class LineStyleDialog : Form
    {
        /// <summary>
        /// the set style of line.
        /// </summary>
        System.Drawing.Drawing2D.DashStyle m_style = System.Drawing.Drawing2D.DashStyle.Custom;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LineStyleDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close thiw window when user click cancel button.
        /// </summary>
        /// <param name="sender">Button(Cancel)</param>
        /// <param name="e">EventArgs</param>
        private void LineCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Check the set style of line.
        /// </summary>
        /// <returns>the set style of line.</returns>
        public System.Drawing.Drawing2D.DashStyle ShowLineStyleDialog()
        {
            LSApplyButton.Click += new EventHandler(ApplyButton_Click);
            this.ShowDialog();
            return m_style;
        }

        /// <summary>
        /// Check the set style and close this window when user click apply button.
        /// </summary>
        /// <param name="sender">Button(Apply).</param>
        /// <param name="e">EventArgs.</param>
        void ApplyButton_Click(object sender, EventArgs e)
        {
            if (solidRadioButton.Checked == true)
                m_style = System.Drawing.Drawing2D.DashStyle.Solid;
            else if (dashRadioButton.Checked == true)
                m_style = System.Drawing.Drawing2D.DashStyle.Dash;
            else if (dashDotRadioButton.Checked == true)
                m_style = System.Drawing.Drawing2D.DashStyle.DashDot;
            else if (dotRadioButton.Checked == true)
                m_style = System.Drawing.Drawing2D.DashStyle.Dot;
            else if (dashDotDotRadioButton.Checked == true)
                m_style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            this.Close();
        }

        /// <summary>
        /// The event when this window is shown.
        /// </summary>
        /// <param name="sender">this window.</param>
        /// <param name="e">EventArgs.</param>
        private void LineStyleShown(object sender, EventArgs e)
        {
            this.LSApplyButton.Focus();
        }
    }
}