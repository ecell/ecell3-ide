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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Logger
{
    /// <summary>
    /// LoggerEventArgs
    /// </summary>
    public class LoggerEventArgs : EventArgs
    {
        #region Fields
        private string m_orgFullPN;
        private LoggerEntry m_entry;
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the logger entry.
        /// </summary>
        public LoggerEntry Entry
        {
            get { return m_entry; }
        }
        /// <summary>
        /// get / set the original full PN.
        /// </summary>
        public string OriginalFullPN
        {
            get { return m_orgFullPN; }
        }
        #endregion

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="orgFullPN">the original FullPN</param>
        /// <param name="entry">the logger entry.</param>
        public LoggerEventArgs(string orgFullPN, LoggerEntry entry)
        {
            m_orgFullPN = orgFullPN;
            m_entry = entry;
        }
    }
}
