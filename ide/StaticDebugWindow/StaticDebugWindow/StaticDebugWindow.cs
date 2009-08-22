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
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;	// ñºëOãÛä‘ÇÃêÈåæ
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

using Ecell;
using Ecell.Plugin;
using Ecell.Objects;
using Ecell.Reporting;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.StaticDebugWindow
{
    /// <summary>
    /// Controls the static debug.
    /// </summary>
    public class StaticDebugWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// Timer
        /// </summary>
        private Timer m_timer;
        /// <summary>
        /// message list.
        /// </summary>
        List<IReport> m_messages;
        /// <summary>
        /// The dictionary of StaticDebugPlugin.
        /// Word is the name of static debug. Data is the plugin of static debug.
        /// </summary>
        List<IStaticDebugPlugin> m_plugins = new List<IStaticDebugPlugin>();
        private bool m_isExecute;
        /// <summary>
        /// The number of edit after saving or loading.
        /// </summary>
        private int m_editCount = 0;
        /// <summary>
        /// MenuItem to change whether static debug execute automatically.
        /// </summary>
        private ToolStripMenuItem m_isAutoMenuItem;
        /// <summary>
        /// MenuItem to execute static debug manually.
        /// </summary>
        private ToolStripMenuItem m_StaticDebugMenuItem;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public StaticDebugWindow()
        {
            m_isExecute = true;
            m_messages = new List<IReport>();
            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Tick += new EventHandler(FireTimer);
        }
        #endregion


        #region PluginBase
        /// <summary>
        /// Get menustrips for each plugin.
        /// </summary>
        /// <returns>AutoDebug, ManualDebug and separator.</returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            ToolStripMenuItem analysisMenu = new ToolStripMenuItem();
            analysisMenu.DropDownItems.AddRange(new ToolStripItem[] { 
                m_isAutoMenuItem, m_StaticDebugMenuItem
            });
            analysisMenu.Text = "Tools";
            analysisMenu.Name = MenuConstants.MenuItemTools;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResources));
            List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
            list.Add(analysisMenu);

            return list;
        }

        /// <summary>
        /// Returns the name of this plugin.
        /// </summary>
        /// <returns>"StaticDebugWindow"(Fixed)</returns>        
        public override string GetPluginName()
        {
            return "StaticDebugWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Add the object.
        /// </summary>
        /// <param name="data">the added object.</param>
        public override void DataAdd(EcellObject data)
        {
            m_editCount++;
        }

        /// <summary>
        /// Add the list of object.
        /// </summary>
        /// <param name="data">the list of added objects.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            m_editCount++;
        }

        /// <summary>
        /// Change the property of object.
        /// </summary>
        /// <param name="modelID">the model ID of changed object.</param>
        /// <param name="key">the key of changed object.</param>
        /// <param name="type">the type of changed object.</param>
        /// <param name="data">the changed object.</param>
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            m_editCount++;
        }

        /// <summary>
        /// Delete the object.
        /// </summary>
        /// <param name="modelID">the model ID of deleted object.</param>
        /// <param name="key">the key of deleted object.</param>
        /// <param name="type">the type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            m_editCount++;
        }

        /// <summary>
        /// Clear the edit count.
        /// </summary>
        public override void Clear()
        {
            m_editCount = 0;
        }

        /// <summary>
        /// Initializes validated patterns.
        /// </summary>
        public override void Initialize()
        {
            m_timer.Interval = 5000;

            m_plugins.Add(new StaticDebugForModel(this));
            m_plugins.Add(new StaticDebugForNetwork(this));

            m_isAutoMenuItem = new ToolStripMenuItem();
            m_isAutoMenuItem.Text = MessageResources.MenuItemAutoDebug;
            m_isAutoMenuItem.Tag = 0;
            m_isAutoMenuItem.Enabled = true;
            m_isAutoMenuItem.Checked = m_isExecute;
            m_isAutoMenuItem.Click += new EventHandler(ChangeAutoUpdate);

            m_StaticDebugMenuItem = new ToolStripMenuItem();
            m_StaticDebugMenuItem.Text = MessageResources.MenuItemStaticDebugText;
            m_StaticDebugMenuItem.Tag = 5;
            m_StaticDebugMenuItem.Enabled = !m_isExecute;
            m_StaticDebugMenuItem.Click += new EventHandler(StaticDebuggerExecute);

        }

        /// <summary>
        /// When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">project status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Loaded)
            {
                m_timer.Enabled = true;
                m_isAutoMenuItem.Enabled = true;
                m_StaticDebugMenuItem.Enabled = !m_isExecute;
            }
            else
            {
                m_timer.Enabled = false;
                m_isAutoMenuItem.Enabled = false;
                m_StaticDebugMenuItem.Enabled = false;
            }
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Get key from entity path.
        /// </summary>
        /// <param name="entityPath">input entity path.</param>
        /// <returns>key.</returns>
        public String GetKeyFromPath(String entityPath)
        {
            String[] list = entityPath.Split(new char[] { ':' });
            if (list.Length == 2) return entityPath;
            bool isSystem = false;
            if (list[0] == Constants.xpathSystem) isSystem = true;
            String result = list[1];
            for (int i = 2; i < list.Length - 1; i++)
            {
                if (isSystem)
                {
                    if (result == "") result = list[i];
                    else result = result + "/" + list[i];
                }
                else
                    result = result + ":" + list[i];
            }
            return result;
        }

        /// <summary>
        /// execute the static debug in existing the list.
        /// </summary>
        public void Debug()
        {
            try
            {
                ReportingSession rs = m_env.ReportManager.GetReportingSession(Constants.groupDebug);
                using (rs)
                {
                    List<string> mList = m_dManager.GetModelList();
                    foreach (string modelID in mList)
                    {
                        List<EcellObject> olist = m_dManager.GetData(modelID, null);
                        foreach (IStaticDebugPlugin plugin in m_plugins)
                        {
                            foreach (IReport rep in plugin.Debug(olist))
                            {
                                rs.Add(rep);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }       

        /// <summary>
        /// Validates the mass conservation.
        /// </summary>
        /// <param name="modelID">the debug model ID.</param>
        public void ValidateMassConservation(string modelID)
        {
            try
            {
                // nothing
            }
            catch (Exception ex)
            {
                throw new EcellException("The static debug of the mass conservation failed. [" + ex.ToString() + "]");
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void FireTimer(object sender, EventArgs e)
        {
            if (m_isExecute && m_editCount > 0)
            {
                m_timer.Enabled = false;
                Debug();
                m_timer.Enabled = true;
                m_editCount = 0;
            }
        }

        /// <summary>
        /// Click the auto debug menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ChangeAutoUpdate(object sender, EventArgs e)
        {
            m_isAutoMenuItem.Checked = !m_isExecute;
            m_isExecute = !m_isExecute;
            m_StaticDebugMenuItem.Enabled = !m_isExecute;
        }

        /// <summary>
        /// Click the manual debug menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void StaticDebuggerExecute(object sender, EventArgs e)
        {
            Debug();
        }
        #endregion
    }
}
