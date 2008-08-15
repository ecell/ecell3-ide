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
        private MessageListWindowControl m_control;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageListWindow()
        {

        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get the window form for MessageListWindow plugin.
        /// </summary>
        /// <returns>Windows form</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            m_control = new MessageListWindowControl(this);
            return new EcellDockContent[] { m_control };
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Report_ReportingSessionStarted(object obj, ReportingSessionEventArgs e)
        {
            m_control.Clear();
        }

        /// <summary>
        /// The event sequence to display the message.
        /// </summary>
        /// <param name="message">the message entry object.</param>
        public void Report_ReportEntryAdded(object obj, ReportEventArgs e)
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
