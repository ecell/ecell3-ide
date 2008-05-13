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

namespace EcellLib.Message
{
    /// <summary>
    /// Abstract class of the message entry.
    /// </summary>
    public abstract class MessageEntry : IMessageEntry
    {
        #region Fields
        /// <summary>
        /// Type of message.
        /// </summary>
        protected MessageType m_type;
        /// <summary>
        /// The location of message.
        /// </summary>
        protected string m_location;
        /// <summary>
        /// The message string.
        /// </summary>
        protected string m_message;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageEntry()
        {
            m_type = MessageType.Error;
            m_location = "";
            m_message = "";
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="type">the message type.</param>
        /// <param name="location">the location of message.</param>
        /// <param name="message">the message string.</param>
        public MessageEntry(MessageType type, string location, string message)
        {
            m_type = type;
            m_location = location;
            m_message = message;
        }
        #endregion

        /// <summary>
        /// Get the type of message.
        /// </summary>
        /// <returns>the type of message.</returns>
        public virtual MessageType GetMessageType()
        {
            return m_type;
        }

        /// <summary>
        /// Get the location of message.
        /// </summary>
        /// <returns>the location string of message.</returns>
        public virtual String GetLocation()
        {
            return m_location;
        }

        /// <summary>
        /// Get the message string.
        /// </summary>
        /// <returns>the message string.</returns>
        public virtual String GetMessage()
        {
            return m_message;
        }
    }
}
