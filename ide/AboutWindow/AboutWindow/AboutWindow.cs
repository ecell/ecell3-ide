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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using Ecell;
using System.ComponentModel;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.AboutWindow
{
    /// <summary>
    /// About window plguin.
    /// </summary>
    public class AboutWindow : PluginBase
    {
        #region Fields
        private ToolStripMenuItem MenuItemAboutPlatform;
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for AboutWindow plugin.
        /// [Help] -> [About Platform]
        /// </summary>
        /// <returns>MenuStipItems</returns>
        public override List<ToolStripMenuItem> GetMenuStripItems()
        {
            MenuItemAboutPlatform = new ToolStripMenuItem();
            MenuItemAboutPlatform.Name = "MenuItemAboutPlatform";
            MenuItemAboutPlatform.Size = new System.Drawing.Size(96, 22);
            MenuItemAboutPlatform.Tag = 1;
            MenuItemAboutPlatform.Text = MessageResAbout.MenuItemAboutPlatformText;
            MenuItemAboutPlatform.Click += new System.EventHandler(this.AboutMenuClick);

            ToolStripMenuItem helpMenu = new ToolStripMenuItem();
            helpMenu.DropDownItems.AddRange(new ToolStripItem[] { MenuItemAboutPlatform });
            helpMenu.Text = "Help";
            helpMenu.Name = "MenuItemHelp";
            helpMenu.Size = new System.Drawing.Size(36, 20);

            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
            list.Add(helpMenu);

            return list;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"AboutWindow"</returns>        
        public override string GetPluginName()
        {
            return "AboutModelEditor";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        /// <summary>
        /// The action when select [About Platform] menu.
        /// This action display AboutForm.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void AboutMenuClick(object sender, EventArgs e)
        {

            AboutForm aboutForm = new AboutForm();
            Version v = m_pManager.AppVersion;
            aboutForm.versionLabel.Text = "version: " + v.ToString();
            StringBuilder copyrightText = new StringBuilder();
            AssemblyCopyrightAttribute a = (AssemblyCopyrightAttribute)
                Assembly.GetExecutingAssembly().GetCustomAttributes(
                    typeof(AssemblyCopyrightAttribute), false)[0];
            aboutForm.copyLabel.Text = a.Copyright;

            aboutForm.Show();
        }

    }
}
