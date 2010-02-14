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
// written by Moriyoshi Koizumi <mozo@sfc.keio.ac.jp>.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Reporting
{
    /// <summary>
    /// Reporting session object.
    /// </summary>
    public class ReportingSession: IList<IReport>, IDisposable
    {
        /// <summary>
        /// get / set the index of report.
        /// </summary>
        /// <param name="idx">index</param>
        /// <returns>Report object.</returns>
        public IReport this[int idx]
        {
            get { return m_reports[idx]; }
            set { throw new InvalidOperationException(); }
        }
        /// <summary>
        /// get the count of report.
        /// </summary>
        public int Count
        {
            get { return m_reports.Count; }
        }
        /// <summary>
        /// get the group string.
        /// </summary>
        public string Group
        {
            get { return m_group; }
        }
        /// <summary>
        /// get the flag whether this report is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="group">the group name.</param>
        /// <param name="rm">ReportManager</param>
        public ReportingSession(string group, ReportManager rm)
        {
            m_group = group;
            m_man = rm;
            m_reports = new List<IReport>();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ReportingSession()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Add the report object.
        /// </summary>
        /// <param name="item">Report object.</param>
        public void Add(IReport item)
        {
            if (m_reports.Contains(item))
                return;
            m_man.OnReportAdded(item);
            m_reports.Add(item);
        }

        /// <summary>
        /// Clear the all reports.
        /// </summary>
        public void Clear()
        {
            m_reports.Clear();
            m_man.OnReportCleared(this.Group);
        }

        /// <summary>
        /// Check whether this report contain the report list.
        /// </summary>
        /// <param name="item">Report object.</param>
        /// <returns>Return true, when this report is exist.</returns>
        public bool Contains(IReport item)
        {
            return m_reports.Contains(item);
        }

        /// <summary>
        /// Get Enumerator of the report list.
        /// </summary>
        /// <returns>the list of report object.</returns>
        public IEnumerator<IReport> GetEnumerator()
        {
            return m_reports.GetEnumerator();
        }

        /// <summary>
        /// Get Enumerator of the report list.
        /// </summary>
        /// <returns>the list of report object.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_reports.GetEnumerator();
        }

        /// <summary>
        /// Get the index of this report.
        /// </summary>
        /// <param name="item">the report object.</param>
        /// <returns>Index.</returns>
        public int IndexOf(IReport item)
        {
            return m_reports.IndexOf(item);
        }

        /// <summary>
        /// Insert the report object.
        /// </summary>
        /// <param name="idx">index</param>
        /// <param name="item">the report object.</param>
        public void Insert(int idx, IReport item)
        {
            m_reports.Insert(idx, item);
            m_man.OnReportAdded(item);
        }

        /// <summary>
        /// Remove the report by using index,
        /// </summary>
        /// <param name="idx">index of removed report object.</param>
        public void RemoveAt(int idx)
        {
            IReport item = m_reports[idx];
            m_reports.RemoveAt(idx);
            m_man.OnReportRemoved(item);
        }

        /// <summary>
        /// Remove the report by using the report object.
        /// </summary>
        /// <param name="item">the removed report object.</param>
        /// <returns>Return true when the report object is removed.</returns>
        public bool Remove(IReport item)
        {
            bool retval = m_reports.Remove(item);
            m_man.OnReportRemoved(item);
            return retval;
        }

        /// <summary>
        /// Copy the report object.
        /// </summary>
        /// <param name="a">the list of report object</param>
        /// <param name="idx">the copy index.</param>
        public void CopyTo(IReport[] a, int idx)
        {
            m_reports.CopyTo(a, idx);
        }

        /// <summary>
        /// Close the this report session.
        /// </summary>
        public void Close()
        {
            m_man.OnSessionClosed(this.Group);
        }

        #region Fields
        /// <summary>
        /// the group name.
        /// </summary>
        string m_group;
        /// <summary>
        /// ReportManager.
        /// </summary>
        ReportManager m_man;
        /// <summary>
        /// The list of reports.
        /// </summary>
        List<IReport> m_reports;
        #endregion
    }
}
