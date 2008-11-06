//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;
using Ecell.Plugin;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.Plotter
{
    /// <summary>
    /// Plugin Class of Plotter.
    /// </summary>
    public class Plotter : PluginBase
    {
        #region Fields
        ToolStripMenuItem m_plotWin;
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for TracerWindow.
        /// </summary>
        /// <returns>MenuStripItems</returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_plotWin = new ToolStripMenuItem();
            m_plotWin.Text = MessageResources.MenuItemShowPlotText;
            m_plotWin.Name = "MenuItemShowPlot";
            m_plotWin.Size = new Size(96, 22);
            m_plotWin.Enabled = false;
            m_plotWin.Click += new EventHandler(this.ShowPlotWindow);

            ToolStripMenuItem view = new ToolStripMenuItem();
            view.DropDownItems.AddRange(new ToolStripItem[] {
                m_plotWin
            });
            view.Name = "MenuItemView";
            view.Size = new Size(36, 20);
            view.Text = "View";
            tmp.Add(view);

            return tmp;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"TracerWindow"</returns>
        public override string GetPluginName()
        {
            return "Plotter";
        }

        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Uninitialized)
            {
                m_plotWin.Enabled = false;
            }
            else if (type == ProjectStatus.Loaded)
            {
            }
            else if (type == ProjectStatus.Running)
            {
                m_plotWin.Enabled = true;
            }
            else if (type == ProjectStatus.Stepping)
            {
                m_plotWin.Enabled = true;
            }
        }
        #endregion

        #region Events
        void ShowPlotWindow(Object sender, EventArgs e)
        {
            PlotterWindow win = new PlotterWindow(this);
            win.Show();
        }
        #endregion
    }

}