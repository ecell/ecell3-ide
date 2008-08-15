//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using Ecell.Logging;

namespace Ecell.IDE
{
    /// <summary>
    /// SplashSheet window class.
    /// </summary>
    public partial class Splash : Form
    {
        private ApplicationEnvironment m_env;
        private LogEntryAppendedEventHandler m_ehandler;
        /// <summary>
        /// Constructor.
        /// </summary>
        public Splash(ApplicationEnvironment env)
        {
            m_env = env;
            InitializeComponent();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            VersionNumber.Text = ((AssemblyProductAttribute)executingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;
            CopyrightNotice.Text = ((AssemblyCopyrightAttribute)executingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            m_ehandler = new Ecell.Logging.LogEntryAppendedEventHandler(LogManager_LogEntryAppended);
            env.LogManager.LogEntryAppended += m_ehandler;
        }

        private void LogManager_LogEntryAppended(object o, LogEntryEventArgs e)
        {
            if (e.LogEntry.Type == MessageType.Information)
            {
                progressInfo.Text = e.LogEntry.Message;
                progressInfo.Update();
            }
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_env.LogManager.LogEntryAppended -= m_ehandler;
        }
    }
}