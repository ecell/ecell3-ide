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
    public class ReportingSession: IList<IReport>, IDisposable
    {
        public IReport this[int idx]
        {
            get { return m_reports[idx]; }
            set { throw new InvalidOperationException(); }
        }

        public int Count
        {
            get { return m_reports.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ReportingSession(ReportManager rm)
        {
            m_man = rm;
            m_reports = new List<IReport>();
        }

        ~ReportingSession()
        {
            Dispose();
        }

        public void Dispose()
        {
            Close();
        }

        public void Add(IReport item)
        {
            m_man.OnReportAdded(item);
            m_reports.Add(item);
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(IReport item)
        {
            return m_reports.Contains(item);
        }

        public IEnumerator<IReport> GetEnumerator()
        {
            return m_reports.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_reports.GetEnumerator();
        }

        public int IndexOf(IReport item)
        {
            return m_reports.IndexOf(item);
        }

        public void Insert(int idx, IReport item)
        {
            m_reports.Insert(idx, item);
            m_man.OnReportAdded(item);
        }

        public void RemoveAt(int idx)
        {
            IReport item = m_reports[idx];
            m_reports.RemoveAt(idx);
            m_man.OnReportRemoved(item);
        }

        public bool Remove(IReport item)
        {
            bool retval = m_reports.Remove(item);
            m_man.OnReportRemoved(item);
            return retval;
        }

        public void CopyTo(IReport[] a, int idx)
        {
            m_reports.CopyTo(a, idx);
        }

        public void Close()
        {
            m_man.OnSessionClosed();
        }

        ReportManager m_man;
        List<IReport> m_reports;
    }
}
