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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Ecell.Reporting
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ReportingSessionStartedEventHandler(object o, ReportingSessionEventArgs e);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ReportingSessionClosedEventHandler(object o, ReportingSessionEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ReportAddedEventHandler(object o, ReportEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ReportRemovedEventHandler(object o, ReportEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ReportClearEventHandler(object o, EventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void StatusUpdatedEventHandler(object o, StatusUpdateEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    public delegate void ProgressReportEventHandler(object o, ProgressReportEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class ReportManager
    {
        /// <summary>
        /// 
        /// </summary>
        public event ReportingSessionStartedEventHandler ReportingSessionStarted;
        /// <summary>
        /// 
        /// </summary>
        public event ReportingSessionClosedEventHandler ReportingSessionClosed;
        /// <summary>
        /// 
        /// </summary>
        public event ReportAddedEventHandler ReportAdded;
        /// <summary>
        /// 
        /// </summary>
        public event ReportRemovedEventHandler ReportRemoved;
        /// <summary>
        /// 
        /// </summary>
        public event ReportClearEventHandler Cleared;
        /// <summary>
        /// 
        /// </summary>
        public event StatusUpdatedEventHandler StatusUpdated;
        /// <summary>
        /// 
        /// </summary>
        public event ProgressReportEventHandler ProgressValueUpdated;

        private ApplicationEnvironment m_env;
        private Dictionary<string, ReportingSession> m_repList = new Dictionary<string,ReportingSession>();
        /// <summary>
        /// 
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public ReportManager(ApplicationEnvironment env)
        {
            m_env = env;
        }

        internal void OnSessionClosed(string groupname)
        {
            Trace.WriteLine("ReportingSession closed");
            if (ReportingSessionClosed != null && m_repList.ContainsKey(groupname))
                ReportingSessionClosed(this, new ReportingSessionEventArgs(m_repList[groupname]));
            lock (this)
            {
                if (m_repList.ContainsKey(groupname))
                    m_repList.Remove(groupname);
            }
        }

        internal void OnReportAdded(IReport rep)
        {       
            Trace.WriteLine("Report added");
            if (ReportAdded != null && m_repList.ContainsKey(rep.Group))
                ReportAdded(m_repList[rep.Group], new ReportEventArgs(rep));
        }

        internal void OnReportRemoved(IReport rep)
        {
            if (ReportRemoved != null && m_repList.ContainsKey(rep.Group))
                ReportRemoved(m_repList[rep.Group], new ReportEventArgs(rep));
        }

        internal void OnReportCleared(string groupname)
        {
            Trace.WriteLine("ReportSesion cleared");
            if (Cleared != null && m_repList.ContainsKey(groupname))
                Cleared(m_repList[groupname], new EventArgs());
        }

        /// <summary>
        /// Clear()
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                if (m_repList.Count > 0)
                {
                    throw new InvalidOperationException();
                }
            }
            if (Cleared != null)
                Cleared(this, new EventArgs());
            m_repList.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public ReportingSession GetReportingSession(string group)
        {
            lock (this)
            {
                if (m_repList.ContainsKey(group))
                {
                    return this.m_repList[group];
//                    throw new InvalidOperationException();
                }
                m_repList[group] = new ReportingSession(group, this);
            }
            Trace.WriteLine("ReportingSession acquired");
            if (ReportingSessionStarted != null)
                ReportingSessionStarted(this, new ReportingSessionEventArgs(m_repList[group]));
            return m_repList[group];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public void SetStatus(StatusBarMessageKind type, string text)
        {
            if (StatusUpdated != null)
                StatusUpdated(this, new StatusUpdateEventArgs(type, text));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetProgress(int value)
        {
            if (ProgressValueUpdated != null)
                ProgressValueUpdated(this, new ProgressReportEventArgs(value));
        }
    }
}
