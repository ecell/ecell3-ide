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
    /// Delegate to start the report session.
    /// </summary>
    /// <param name="o">ReportManager</param>
    /// <param name="e">ReportingSessionEventArgs</param>
    public delegate void ReportingSessionStartedEventHandler(object o, ReportingSessionEventArgs e);
    
    /// <summary>
    /// Delegate to close the report session.
    /// </summary>
    /// <param name="o">ReportManager</param>
    /// <param name="e">ReportingSessionEventArgs</param>
    public delegate void ReportingSessionClosedEventHandler(object o, ReportingSessionEventArgs e);

    /// <summary>
    /// Delegate to add the report.
    /// </summary>
    /// <param name="o">ReportManager</param>
    /// <param name="e">ReportEventArgs</param>
    public delegate void ReportAddedEventHandler(object o, ReportEventArgs e);

    /// <summary>
    /// Delegate to remove the report.
    /// </summary>
    /// <param name="o">ReportManager</param>
    /// <param name="e">ReportEventArgs</param>
    public delegate void ReportRemovedEventHandler(object o, ReportEventArgs e);

    /// <summary>
    /// Delegate to clear the report session.
    /// </summary>
    /// <param name="o">ReportManager</param>
    /// <param name="e">EventArgs</param>
    public delegate void ReportClearEventHandler(object o, EventArgs e);

    /// <summary>
    /// Delegate to update the status.
    /// </summary>
    /// <param name="o">ReportManager</param>
    /// <param name="e">StatusUpdateEventArgs</param>
    public delegate void StatusUpdatedEventHandler(object o, StatusUpdateEventArgs e);

    /// <summary>
    /// Delegate to progress the report.
    /// </summary>
    /// <param name="o">ReportManager</param>
    /// <param name="e">ProgressReportEventArgs</param>
    public delegate void ProgressReportEventHandler(object o, ProgressReportEventArgs e);

    /// <summary>
    /// Report Manage class 
    /// </summary>
    public class ReportManager
    {
        #region Fields
        /// <summary>
        /// EventHandler to start the report session.
        /// </summary>
        public event ReportingSessionStartedEventHandler ReportingSessionStarted;
        /// <summary>
        /// EventHandler to close the report session.
        /// </summary>
        public event ReportingSessionClosedEventHandler ReportingSessionClosed;
        /// <summary>
        /// EventHandler to add the report.
        /// </summary>
        public event ReportAddedEventHandler ReportAdded;
        /// <summary>
        /// EventHandler to remove the report.
        /// </summary>
        public event ReportRemovedEventHandler ReportRemoved;
        /// <summary>
        /// EventHandler to clear the report session.
        /// </summary>
        public event ReportClearEventHandler Cleared;
        /// <summary>
        ///  EventHandler to update the status.
        /// </summary>
        public event StatusUpdatedEventHandler StatusUpdated;
        /// <summary>
        /// EventHandler to progress the report.
        /// </summary>
        public event ProgressReportEventHandler ProgressValueUpdated;
        /// <summary>
        /// The application environment
        /// </summary>
        private ApplicationEnvironment m_env;
        /// <summary>
        /// The list of ReportSession
        /// </summary>
        private Dictionary<string, ReportingSession> m_repList = new Dictionary<string,ReportingSession>();
        #endregion

        /// <summary>
        /// get the application environment.
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="env">ApplicationEnvironment</param>
        public ReportManager(ApplicationEnvironment env)
        {
            m_env = env;
        }

        /// <summary>
        /// Event when the report session is closed.
        /// </summary>
        /// <param name="groupname">the group name.</param>
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

        /// <summary>
        /// Event when the report is added.
        /// </summary>
        /// <param name="rep">the report object.</param>
        internal void OnReportAdded(IReport rep)
        {       
            Trace.WriteLine("Report added");
            if (ReportAdded != null && m_repList.ContainsKey(rep.Group))
                ReportAdded(m_repList[rep.Group], new ReportEventArgs(rep));
        }

        /// <summary>
        /// Event when the report is removed.
        /// </summary>
        /// <param name="rep">the report object.</param>
        internal void OnReportRemoved(IReport rep)
        {
            if (ReportRemoved != null && m_repList.ContainsKey(rep.Group))
                ReportRemoved(m_repList[rep.Group], new ReportEventArgs(rep));
        }

        /// <summary>
        /// Event when the report is cleared.
        /// </summary>
        /// <param name="groupname">the group name.</param>
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
        /// Get the report session by usin the group name.
        /// </summary>
        /// <param name="group">the group stirng.</param>
        /// <returns>the report settion.</returns>
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
        /// Set the status of report.
        /// </summary>
        /// <param name="type">message type.</param>
        /// <param name="text">message string.</param>
        public void SetStatus(StatusBarMessageKind type, string text)
        {
            if (StatusUpdated != null)
                StatusUpdated(this, new StatusUpdateEventArgs(type, text));
        }

        /// <summary>
        /// Set the progress of value.
        /// </summary>
        /// <param name="value">the percentage value.</param>
        public void SetProgress(int value)
        {
            if (ProgressValueUpdated != null)
                ProgressValueUpdated(this, new ProgressReportEventArgs(value));
        }
    }
}
