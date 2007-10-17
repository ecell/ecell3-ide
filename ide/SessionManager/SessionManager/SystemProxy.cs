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
using System.Text;

namespace SessionManager
{
    /// <summary>
    /// SystemProxy class. This class is abstract class managed session.
    /// </summary>
    public class SystemProxy
    {
        private int m_defaultConcurrency;
        private SessionManager m_manager;

        /// <summary>
        /// get / set the default concurrency.
        /// </summary>
        public virtual int DefaultConcurrency
        {
            get { return this.m_defaultConcurrency; }
            set { this.m_defaultConcurrency = value; }
        }

        /// <summary>
        /// get / set manager.
        /// </summary>
        public virtual SessionManager Manager
        {
            get { return this.m_manager; }
            set { this.m_manager = value; }
        }

        /// <summary>
        /// Get environment name of this proxy.
        /// </summary>
        /// <returns>environment name.</returns>
        public virtual String GetEnvironment()
        {
            return null;
        }

        /// <summary>
        /// Create the proxy for session.
        /// </summary>
        /// <returns>Class of proxy for session.</returns>
        public virtual SessionProxy CreateSessionProxy()
        {
            return null;
        }

        /// <summary>
        /// Create the proxy for session with initial parameters.
        /// </summary>
        /// <param name="script">script file name.</param>
        /// <param name="arg">argument of script file.</param>
        /// <param name="extFile">extra file of script file.</param>
        /// <param name="tmpDir">tmp directory of script file</param>
        /// <returns>Class of proxy for session.</returns>
        public virtual SessionProxy CreateSessionProxy(String script, 
            String arg, List<String> extFile, String tmpDir)
        {
            return null;
        }

        /// <summary>
        /// Get the property of this proxy.
        /// </summary>
        /// <returns>list of property name.</returns>
        public virtual Dictionary<String, Object> GetProperty()
        {
            return null;
        }

        /// <summary>
        /// Get the list of option set all session.
        /// </summary>
        /// <returns>dictionary of option.</returns>
        public virtual Dictionary<string, object> GetDefaultProperty()
        {
            return null;
        }

        /// <summary>
        /// Update the property of proxy.
        /// </summary>
        /// <param name="list">the list of property.</param>
        public virtual void SetProperty(Dictionary<String, Object> list)
        {
        }

        /// <summary>
        /// Execute the queue session corresponding with Environment.
        /// </summary>
        public virtual void Update()
        {
            // nothing
        }

        /// <summary>
        /// Get the flag whether this script use IDE functions.
        /// </summary>
        /// <returns>if use IDE function, return true.</returns>
        public virtual bool IsIDE()
        {
            return false;
        }

        /// <summary>
        /// Get the default script name.
        /// </summary>
        /// <returns>the script file name.</returns>
        public virtual string GetDefaultScript()
        {
            return "";
        }
    }
}
