//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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

namespace Ecell.Events
{
    /// <summary>
    /// DisplayFormatEventArgs
    /// </summary>
    public class DisplayFormatEventArgs : EventArgs
    {
        #region Fields
        /// <summary>
        /// Display format string
        /// </summary>
        private string m_dataFormat;
        #endregion

        /// <summary>
        /// get the display format string
        /// </summary>
        public string DisplayFormat
        {
            get { return this.m_dataFormat; }
        }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="format">the display format string</param>
        public DisplayFormatEventArgs(string format)
        {
            m_dataFormat = format;
        }
    }
}
