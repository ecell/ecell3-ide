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
        /// The message string.
        /// </summary>
        protected string m_message;
        /// <summary>
        /// The time when the message is composed
        /// </summary>
        protected DateTime m_time;
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public MessageType MessageType
        {
            get { return m_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return m_message; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp
        {
            get { return m_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public abstract string Location
        {
            get;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="type">the message type.</param>
        /// <param name="message">the message string.</param>
        public MessageEntry(MessageType type, string message)
            : this(type, message, DateTime.Now)
        {
        }

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="type">the message type.</param>
        /// <param name="message">the message string.</param>
        /// <param name="time">the date time of message.</param>
        public MessageEntry(MessageType type, string message, DateTime time)
        {
            m_type = type;
            m_message = message;
            m_time = time;
        }
        #endregion

        /// <summary>
        /// get the string of this object.
        /// </summary>
        /// <returns>the object string.</returns>
        public override string ToString()
        {
            return "[" + Timestamp + "] " + MessageType + ": " + Message + "(location: " + Location + ")";
        }
    }
}
