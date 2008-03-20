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
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Reflection;

namespace EcellLib.SearchWindow
{
    class SearchWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// the menu strip for [Search]
        /// </summary>
        private ToolStripMenuItem m_searchMenu;
        /// <summary>
        /// the window for search objects.
        /// </summary>
        private SearchCnd m_cnd;
        /// <summary>
        /// ToolBox to search the objects.
        /// </summary>
        ToolStripTextBox m_text;
        #endregion

        #region Events
        /// <summary>
        /// the action of clicking [Search] menu.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void ShowSearchWindow(object sender, EventArgs e)
        {
            m_cnd = new SearchCnd();
            m_cnd.ShowDialog();
        }

        /// <summary>
        /// Event when TextBox in ToolBar is input the search condition.
        /// Display the search result window after system search the object.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyPressEventArgs</param>
        private void m_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!m_searchMenu.Enabled) return;
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (m_text.Text == null || m_text.Text.Equals("")) return;
                if (m_cnd == null || m_cnd.IsDisposed)
                    m_cnd = new SearchCnd();
                m_cnd.Show();
                m_cnd.Search(m_text.Text);
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                m_cnd.Close();
            }
        }

        /// <summary>
        /// Event when the search button is clicked.
        /// Display the search result window after system search the object.        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void Search(object sender, EventArgs e)
        {
            if (!m_searchMenu.Enabled) return;
            if (m_text.Text == null || m_text.Text.Equals("")) return;
            if (m_cnd == null || m_cnd.IsDisposed)
                m_cnd = new SearchCnd();
            m_cnd.Show();
            m_cnd.Search(m_text.Text);
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for SearchWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        public override List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResSearch));

            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_searchMenu = new ToolStripMenuItem();
            m_searchMenu.Name = "MenuItemSearch";
            m_searchMenu.Size = new Size(96, 22);
            m_searchMenu.Image = (Image)Resource1.find;
            m_searchMenu.Text = resources.GetString("MenuItemSearchText");
            m_searchMenu.Enabled = false;
            m_searchMenu.Click += new EventHandler(this.ShowSearchWindow);

            ToolStripMenuItem edit = new ToolStripMenuItem();
            edit.DropDownItems.AddRange(new ToolStripItem[] {
                m_searchMenu            
            });
            edit.Name = "MenuItemEdit";
            edit.Size = new Size(36, 20);
            edit.Text = "Edit";
            tmp.Add(edit);
                
            return tmp;
        }

        /// <summary>
        /// Get toolbar buttons for SearchWindow plugin.
        /// </summary>
        /// <returns>null</returns>
        public override List<ToolStripItem> GetToolBarMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResSearch));

            List<ToolStripItem> list = new List<ToolStripItem>();
            m_text = new ToolStripTextBox();
            m_text.Name = "SearchText";
            m_text.Size = new System.Drawing.Size(60, 25);
            m_text.Text = "";
            m_text.ReadOnly = false;
            m_text.Tag = 5;
            m_text.TextBoxTextAlign = HorizontalAlignment.Left;
            m_text.KeyPress += new KeyPressEventHandler(m_text_KeyPress);
            list.Add(m_text);

            ToolStripButton button1 = new ToolStripButton();
            button1.Image = (Image)Resource1.find;
            button1.ImageTransparentColor = System.Drawing.Color.Magenta;
            button1.Tag = 0;
            button1.Name = "Search";

            button1.Size = new System.Drawing.Size(23, 22);
            button1.Text = "";
            button1.ToolTipText = resources.GetString("ToolTipSearch");
            button1.Click += new System.EventHandler(this.Search);
            list.Add(button1);


            return list;
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Uninitialized) m_searchMenu.Enabled = false;
            else m_searchMenu.Enabled = true;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>        
        public override Bitmap Print(string name)
        {
             if (m_cnd != null)
                 return m_cnd.Print();
             return null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"EntityListWindow"</returns>
        public override string GetPluginName()
        {
            return "SearchWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>the plugin version.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public override List<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            if (m_cnd != null && m_cnd.Visible)
                names.Add("Search result.");
            return names;
        }
        #endregion
    }
}
