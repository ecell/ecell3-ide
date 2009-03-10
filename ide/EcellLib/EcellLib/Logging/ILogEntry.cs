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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Logging
{
    /// <summary>
    /// Interface of the message entry.
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// Get the type of message.
        /// </summary>
        /// <returns>the type of message.</returns>
        MessageType Type { get; }
        /// <summary>
        /// Get the location of message.
        /// </summary>
        /// <returns>the location of message.</returns>
        string Location { get; }
        /// <summary>
        /// Get the timestamp of message.
        /// </summary>
        /// <returns>the time when the message is generated.</returns>
        DateTime Timestamp { get; }
        /// <summary>
        /// Get the message string.
        /// </summary>
        /// <returns>the message string</returns>
        string Message { get; }        
    }
}
