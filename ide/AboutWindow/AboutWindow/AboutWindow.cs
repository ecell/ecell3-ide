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
using EcellLib;

namespace EcellLib.AboutWindow
{
    /// <summary>
    /// About window plguin.
    /// </summary>
    public class AboutWindow : PluginBase
    {
        ToolStripMenuItem MenuItemAboutPlatform;

        #region Fields
        /// <summary>
        /// Get menustrips for AboutWindow plugin.
        /// [Help] -> [About Platform]
        /// </summary>
        /// <returns>MenuStipItems</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResAbout));

            MenuItemAboutPlatform = new ToolStripMenuItem();
            MenuItemAboutPlatform.Name = "MenuItemAboutPlatform";
            MenuItemAboutPlatform.Size = new System.Drawing.Size(96, 22);
            MenuItemAboutPlatform.Tag = 1;
            MenuItemAboutPlatform.Text = resources.GetString("MenuItemAboutPlatformText");
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
        /// Get toolbar buttons for AboutWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for AboutWindow plugin.
        /// </summary>
        /// <returns>null.</returns>
        public List<UserControl> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// This plugin don't display data, and don't work the event sequence.
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
        /// <param name="time">current simulation time</param>
        public void AdvancedTime(double time)
        {
            // nothing
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        public void ChangeStatus(int type)
        {
        }

        /// <summary>
        /// Change availability of undo/redo status
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="directory">output directory.</param>
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
        /// <returns>bitmap data.</returns>
        public Bitmap Print()
        {
            return null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"AboutWindow"</returns>        
        public string GetPluginName()
        {
            return "AboutModelEditor";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false.</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true.</returns>
        public bool IsEnablePrint()
        {
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

        /// <summary>
        /// The action when select [About Platform] menu.
        /// This action display AboutForm.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        private void AboutMenuClick(object sender, EventArgs e)
        {

            PluginManager pManager = PluginManager.GetPluginManager();
            AboutForm aboutForm = new AboutForm();
            Version v = pManager.AppVersion;
            aboutForm.versionLabel.Text = "version: " + v.ToString();
            aboutForm.copyLabel.Text = pManager.CopyRight;
            aboutForm.Show();
        }

    }
}
