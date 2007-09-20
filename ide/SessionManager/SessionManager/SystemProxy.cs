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
        private SessionManager m_manager;

        /// <summary>
        /// get / set manager.
        /// </summary>
        public SessionManager Manager
        {
            get { return this.m_manager; }
            set { this.m_manager = value; }
        }

        /// <summary>
        /// Get environment name of this proxy.
        /// </summary>
        /// <returns>environment name.</returns>
        public String GetEnvironment()
        {
            return null;
        }

        /// <summary>
        /// Create the proxy for session.
        /// </summary>
        /// <returns>Class of proxy for session.</returns>
        public SessionProxy CreateSessionProxy()
        {
            return null;
        }

        /// <summary>
        /// Update the property of this proxy.
        /// </summary>
        public void Update()
        {
        }

        /// <summary>
        /// Get the property of this proxy.
        /// </summary>
        /// <returns>list of property name.</returns>
        public List<string> GetProperty()
        {
            return null;
        }

        /// <summary>
        /// Update the property of proxy.
        /// </summary>
        /// <param name="list">the list of property.</param>
        public void SetProperty(Dictionary<String, Object> list)
        {
            // not implement
        }
    }
}
