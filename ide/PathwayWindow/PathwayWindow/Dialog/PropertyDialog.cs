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

using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.PathwayWindow.Dialog
{
    /// <summary>
    /// Tabbed PropertyDialog for PathwayWindow.
    /// </summary>
    public partial class PropertyDialog : PathwayDialog
    {
        private PathwayControl m_con;
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyDialog(PathwayControl control)
        {
            InitializeComponent();
            m_con = control;
            PropertyDialogTabPage pathwayPage = m_con.Animation.PathwayDialogTabPage;
            PropertyDialogTabPage animationPage = m_con.Animation.AnimationDialogTabPage;
            PropertyDialogTabPage componentPage = m_con.ComponentManager.ComponentTabPage;
            tabControl.Controls.Add(pathwayPage);
            tabControl.Controls.Add(animationPage);
            tabControl.Controls.Add(componentPage);

        }
        /// <summary>
        /// Get tabControl.
        /// </summary>
        public TabControl TabControl
        {
            get { return this.tabControl; }
        }
        /// <summary>
        /// Apply changes for Pathway settings.
        /// </summary>
        public void ApplyChanges()
        {
            foreach (PropertyDialogTabPage tabPage in tabControl.TabPages)
                tabPage.ApplyChange();
        }

        private void PropertyDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel) return;
            try
            {
                foreach (PropertyDialogTabPage tabPage in TabControl.TabPages)
                    tabPage.TabPageClosing();
            }
            catch (EcellException ex)
            {
                Util.ShowErrorDialog(ex.Message);
                e.Cancel = true;
                return;
            }
        }
    }
}