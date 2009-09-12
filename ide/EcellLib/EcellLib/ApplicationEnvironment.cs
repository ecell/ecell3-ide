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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Ecell.Logging;
using Ecell.Logger;
using Ecell.Job;
using Ecell.Reporting;
using Ecell.Action;
using Ecell.Plugin;

namespace Ecell
{
    /// <summary>
    /// Application environment manager class.
    /// </summary>
    public class ApplicationEnvironment
    {
        #region Fields
        /// <summary>
        /// DataManager
        /// </summary>
        private DataManager m_dManager;
        /// <summary>
        /// PluginManager
        /// </summary>
        private PluginManager m_pManager;
        /// <summary>
        /// LogManager
        /// </summary>
        private LogManager m_lManager;
        /// <summary>
        /// LoggerManager
        /// </summary>
        private LoggerManager m_gManager;
        /// <summary>
        /// ReportManager
        /// </summary>
        private ReportManager m_rManager;
        /// <summary>
        /// ActionManager
        /// </summary>
        private ActionManager m_aManager;
        /// <summary>
        /// CommandManager
        /// </summary>
        private CommandManager m_cManager;
        /// <summary>
        /// JobManager
        /// </summary>
        private IJobManager m_jManager;
        /// <summary>
        /// ConsoleManager
        /// </summary>
        private ConsoleManager m_console;
        /// <summary>
        /// DMDescriptorKeeper
        /// </summary>
        private DMDescriptorKeeper m_dmManager;
        #endregion

        #region Accessors
        /// <summary>
        /// get DataManager.
        /// </summary>
        public DataManager DataManager
        {
            get { return m_dManager; }
        }

        /// <summary>
        /// get PluginManager.
        /// </summary>
        public PluginManager PluginManager
        {
            get { return m_pManager; }
        }

        /// <summary>
        /// get LogManager.
        /// </summary>
        public LogManager LogManager
        {
            get { return m_lManager; }
        }

        /// <summary>
        /// get ReportManager.
        /// </summary>
        public ReportManager ReportManager
        {
            get { return m_rManager; }
        }

        /// <summary>
        /// get ActionManager.
        /// </summary>
        public ActionManager ActionManager
        {
            get { return m_aManager; }
        }

        /// <summary>
        /// get CommandManager.
        /// </summary>
        public CommandManager CommandManager
        {
            get { return m_cManager; }
        }

        /// <summary>
        /// get JobManager.
        /// </summary>
        public IJobManager JobManager
        {
            get { return m_jManager; }
            // set { m_jManager = value; }
        }

        /// <summary>
        /// get LoggerManager.
        /// </summary>
        public LoggerManager LoggerManager
        {
            get { return m_gManager; }
        }

        /// <summary>
        /// get Console.
        /// </summary>
        public ConsoleManager Console
        {
            get { return m_console; }
        }

        /// <summary>
        // get DMDescriptor.
        /// </summary>
        public DMDescriptorKeeper DMDescriptorKeeper
        {
            get { return m_dmManager; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        public ApplicationEnvironment()
        {
            m_dManager = new DataManager(this);
            m_lManager = new LogManager(this);
            m_gManager = new LoggerManager(this);
            m_pManager = new PluginManager(this);
            m_aManager = new ActionManager(this);
            m_jManager = new JobManager(this);
            m_rManager = new ReportManager(this);
            m_cManager = new CommandManager(this);
            m_console = new ConsoleManager(this);
            m_dmManager = new DMDescriptorKeeper(Util.GetDMDirs());
        }
        #endregion
    }
}
