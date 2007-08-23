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
        #endregion


        #region PluginBase
        /// <summary>
        /// Get menustrips for SearchWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResSearch));

            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_searchMenu = new ToolStripMenuItem();
            m_searchMenu.Name = "MenuItemSearch";
            m_searchMenu.Size = new Size(96, 22);
//            m_searchMenu.Text = "Search";
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
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
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
            button1.ToolTipText = "Search";
            button1.Click += new System.EventHandler(this.Search);
            list.Add(button1);


            return list;
        }

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

        private void Search(object sender, EventArgs e)
        {
            if (!m_searchMenu.Enabled) return;
            if (m_text.Text == null || m_text.Text.Equals("")) return;
            if (m_cnd == null || m_cnd.IsDisposed)
                m_cnd = new SearchCnd();
            m_cnd.Show();
            m_cnd.Search(m_text.Text);
        }

        /// <summary>
        /// Get the window form for SearchWindow.
        /// </summary>
        /// <returns>UserControl.</returns>
        public List<UserControl> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            // not implement
        }


        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
            // nothing
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(int type)
        {
            if (type == Util.NOTLOAD) m_searchMenu.Enabled = false;
            else m_searchMenu.Enabled = true;
        }

        /// <summary>
        /// Change availability of undo/redo function
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        public void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// Set the panel that show this plugin in MainWindow.
        /// </summary>
        /// <param name="panel">The set panel.</param>
        public void SetPanel(Panel panel)
        {
            
            // nothing
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>        
        public Bitmap Print()
        {
             if (m_cnd != null)
                 return m_cnd.Print();
             return null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"EntityListWindow"</returns>
        public string GetPluginName()
        {
            return "SearchWindow";
        }

        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// cCeck whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false(this plugin is EntityListWindow)</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public bool IsEnablePrint()
        {
            if (m_cnd != null && m_cnd.Visible)
                return true;
            return false;
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
        }
        #endregion
    }
}
