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

using Ecell;
using Ecell.Plugin;
using Ecell.Objects;
using Ecell.Reporting;

namespace Ecell.IDE.Plugins.StaticDebugWindow
{
    /// <summary>
    /// Controls the static debug.
    /// </summary>
    public class StaticDebugWindow : PluginBase
    {
        #region Fields
        private Timer m_timer;
        List<IReport> m_messages;
        /// <summary>
        /// The dictionary of StaticDebugPlugin.
        /// Word is the name of static debug. Data is the plugin of static debug.
        /// </summary>
        List<IStaticDebugPlugin> m_plugins = new List<IStaticDebugPlugin>();
        /// <summary>
        /// ResourceManager for StaticDebugWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResources));
        #endregion

        public StaticDebugWindow()
        {
            m_messages = new List<IReport>();
            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Tick += new EventHandler(FireTimer);
        }

        #region PluginBase
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
        #endregion

        #region Internal Methods
        /// <summary>
        /// Initializes validated patterns.
        /// </summary>
        public override void Initialize()
        {
            m_timer.Interval = 5000;

            m_plugins.Add(new StaticDebugForModel(this));
            m_plugins.Add(new StaticDebugForNetwork(this));
        }

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
        /// <param name="list">the list of static debug.</param>
        public void Debug()
        {
            ReportingSession rs = m_env.ReportManager.GetReportingSession();

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
        
        /// <summary>
        /// Validates the list of the "EcellObject" 4 the mass conservation.
        /// </summary>
        /// <param name="ecellObjectList"></param>
        private void ValidateMassConservation(List<EcellObject> ecellObjectList)
        {
            // MEN WORKING
        }

        /// <summary>
        /// Validates the mass conservation.
        /// </summary>
        /// <param name="modelID"></param>
        public void ValidateMassConservation(string modelID)
        {
            try
            {
                this.ValidateMassConservation(this.m_dManager.GetData(modelID, null));
            }
            catch (Exception ex)
            {
                throw new Exception("The static debug of the mass conservation failed. [" + ex.ToString() + "]");
            }
        }

        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void FireTimer(object sender, EventArgs e)
        {
            m_timer.Enabled = false;
            Debug();
            m_timer.Enabled = true;
        }

        #endregion
    }
}
