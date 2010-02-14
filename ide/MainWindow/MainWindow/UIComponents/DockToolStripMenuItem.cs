//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Ecell.Plugin;
using WeifenLuo.WinFormsUI.Docking;

namespace Ecell.IDE.MainWindow.UIComponents
{
    // ToolStripMenuItem to manage EcellDockContent
    class DockToolStripMenuItem : ToolStripMenuItem
    {
        private EcellDockContent m_content;
        public DockToolStripMenuItem(EcellDockContent content)
        {
            this.Name = content.Name;
            this.Image = (Image)TypeDescriptor.GetConverter(content.Icon)
                    .ConvertTo(content.Icon, typeof(Image));
            this.Text = content.Text;
            this.Checked = !content.IsHidden;
            this.Click += new EventHandler(DockContentMenu_Click);
            m_content = content;
            m_content.DockStateChanged += new EventHandler(DockContent_DockStateChanged);
        }

        /// <summary>
        /// Event on DockContent VisibleChanged.
        /// </summary>
        /// <param name="sender">DockContent</param>
        /// <param name="e">EventArgs</param>
        void DockContent_DockStateChanged(object sender, EventArgs e)
        {
            this.Checked = !(m_content.DockState == DockState.Hidden || m_content.DockState == DockState.Unknown);
        }

        /// <summary>
        /// Event when docking window menu is clicked.
        /// </summary>
        /// <param name="sender">MenuItem</param>
        /// <param name="e">EventArgs</param>
        private void DockContentMenu_Click(object sender, EventArgs e)
        {
            if (this.Checked)
            {
                //Hide EntityList
                m_content.Hide();
                this.Checked = false;
            }
            else
            {
                //Show EntityList
                m_content.Show();
                this.Checked = true;
            }
        }

    }
}
