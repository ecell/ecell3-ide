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
    /// Abstract class of the message entry.
    /// </summary>
    public abstract class Report : IReport
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
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public MessageType Type
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
        public abstract string Location
        {
            get;
        }
        #endregion

        #region Constructor
        public Report(MessageType type, string message)
        {
            m_type = type;
            m_message = message;
        }
        #endregion

        /// <summary>
        /// get the string of this object.
        /// </summary>
        /// <returns>the object string.</returns>
        public override string ToString()
        {
            return  Type + ": " + Message + "(location: " + Location + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Report))
                return false;
            Report ent = obj as Report;
            return ent.m_type == this.m_type &&
                ent.m_message == this.m_message;
        }

        public override int GetHashCode()
        {
            return m_type.GetHashCode() ^ m_message.GetHashCode();
        }
    }
}
