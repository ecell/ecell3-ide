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

namespace Ecell.Reporting
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportingSession: IList<IReport>, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public IReport this[int idx]
        {
            get { return m_reports[idx]; }
            set { throw new InvalidOperationException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return m_reports.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Group
        {
            get { return m_group; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="rm"></param>
        public ReportingSession(string group, ReportManager rm)
        {
            m_group = group;
            m_man = rm;
            m_reports = new List<IReport>();
        }
        /// <summary>
        /// 
        /// </summary>
        ~ReportingSession()
        {
            Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(IReport item)
        {
            m_man.OnReportAdded(item);
            m_reports.Add(item);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(IReport item)
        {
            return m_reports.Contains(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IReport> GetEnumerator()
        {
            return m_reports.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_reports.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(IReport item)
        {
            return m_reports.IndexOf(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="item"></param>
        public void Insert(int idx, IReport item)
        {
            m_reports.Insert(idx, item);
            m_man.OnReportAdded(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveAt(int idx)
        {
            IReport item = m_reports[idx];
            m_reports.RemoveAt(idx);
            m_man.OnReportRemoved(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IReport item)
        {
            bool retval = m_reports.Remove(item);
            m_man.OnReportRemoved(item);
            return retval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="idx"></param>
        public void CopyTo(IReport[] a, int idx)
        {
            m_reports.CopyTo(a, idx);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            m_man.OnSessionClosed();
        }

        string m_group;
        ReportManager m_man;
        List<IReport> m_reports;
    }
}
