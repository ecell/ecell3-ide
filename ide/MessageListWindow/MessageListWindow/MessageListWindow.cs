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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Ecell;
using Ecell.Reporting;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.MessageListWindow
{
    /// <summary>
    /// The plugin to display the list of error messages.
    /// </summary>
    public class MessageListWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// control object.
        /// </summary>
        private MessageListWindowControl m_control;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageListWindow()
        {
            m_control = new MessageListWindowControl(this);
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Close project and clear all object.
        /// </summary>
        public override void Clear()
        {
            m_control.Clear(null);
        }
        /// <summary>
        /// Get the window form for MessageListWindow plugin.
        /// </summary>
        /// <returns>Windows form</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return new EcellDockContent[] { m_control };
        }

        /// <summary>
        /// Initialize of Plugin.
        /// </summary>
        public override void Initialize()
        {
            m_env.ReportManager.ReportAdded +=new ReportAddedEventHandler(ReportManager_ReportAdded);
            m_env.ReportManager.Cleared += new ReportClearEventHandler(ReportManager_Cleared);
            m_env.ReportManager.ReportingSessionStarted += new ReportingSessionStartedEventHandler(ReportManager_ReportingSessionStarted);
        }

        /// <summary>
        /// The event sequence on clear group.
        /// </summary>
        /// <param name="o">ReportManager</param>
        /// <param name="e">EventArgs</param>
        private void ReportManager_Cleared(object o, EventArgs e)
        {
            Ecell.Reporting.ReportingSession s = o as Ecell.Reporting.ReportingSession;
            if (s == null)
                return;
            m_control.Clear(s.Group);
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        /// <param name="obj">ReportManager</param>
        /// <param name="e">ReportingSessionEventArgs</param>
        private void ReportManager_ReportingSessionStarted(object obj, ReportingSessionEventArgs e)
        {
            m_control.Clear(e.ReportingSession.Group);
        }

        /// <summary>
        /// The event sequence to display the message.
        /// </summary>
        /// <param name="obj">ReportManager</param>
        /// <param name="e">ReportEventArgs</param>
        private void ReportManager_ReportAdded(object obj, ReportEventArgs e)
        {
            m_control.AddMessageEntry(e.Report);
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"MessageWindow"</returns>
        public override string GetPluginName()
        {
            return "MessageListWindow";
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
    }
}
