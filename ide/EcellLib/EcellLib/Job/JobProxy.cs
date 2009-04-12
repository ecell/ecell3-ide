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

namespace Ecell.Job
{
    /// <summary>
    /// SystemProxy class. This class is abstract class managed session.
    /// </summary>
    public abstract class JobProxy
    {
        private int m_Concurrency;
        private JobManager m_manager;

        /// <summary>
        /// get / set the default concurrency.
        /// </summary>
        public virtual int Concurrency
        {
            get { return this.m_Concurrency; }
            set { this.m_Concurrency = value; }
        }

        /// <summary>
        /// get / set manager.
        /// </summary>
        public virtual JobManager Manager
        {
            get { return this.m_manager; }
            set { this.m_manager = value; }
        }

        /// <summary>
        /// Get environment name of this proxy.
        /// </summary>
        /// <returns>environment name.</returns>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Create the proxy for session.
        /// </summary>
        /// <returns>Class of proxy for session.</returns>
        public abstract Job CreateJob();

        /// <summary>
        /// Create the proxy for session with initial parameters.
        /// </summary>
        /// <param name="script">script file name.</param>
        /// <param name="arg">argument of script file.</param>
        /// <param name="extFile">extra file of script file.</param>
        /// <param name="tmpDir">tmp directory of script file</param>
        /// <returns>Class of proxy for session.</returns>
        public abstract Job CreateJob(string script,
            string arg, List<string> extFile, string tmpDir);

        /// <summary>
        /// Get the property of this proxy.
        /// </summary>
        /// <returns>list of property name.</returns>
        public abstract Dictionary<string, Object> GetProperty();

        /// <summary>
        /// Get the list of option set all session.
        /// </summary>
        /// <returns>dictionary of option.</returns>
        public abstract Dictionary<string, object> GetDefaultProperty();

        /// <summary>
        /// Update the property of proxy.
        /// </summary>
        /// <param name="list">the list of property.</param>
        public abstract void SetProperty(Dictionary<string, Object> list);

        /// <summary>
        /// Execute the queue session corresponding with Environment.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Get the flag whether this script use IDE functions.
        /// </summary>
        /// <returns>if use IDE function, return true.</returns>
        public abstract bool IsIDE();

        /// <summary>
        /// Get the default script name.
        /// </summary>
        /// <returns>the script file name.</returns>
        public abstract string GetDefaultScript();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract string GetData(string name);
    }
}
