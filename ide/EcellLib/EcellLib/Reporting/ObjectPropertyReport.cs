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
using Ecell.Objects;

namespace Ecell.Reporting
{
    public class ObjectPropertyReport: ObjectReport
    {
        private string m_propName;

        #region Accessors
        /// <summary>
        /// get / set the object of message.
        /// </summary>
        public string PropertyName
        {
            get { return this.m_propName; }
        }

        /// <summary>
        /// get the location of message.
        /// </summary>
        public override string Location
        {
            get
            {
                return Object.Type
                    + Constants.delimiterColon
                    + Object.Key
                    + Constants.delimiterColon
                    + m_propName;
            }
        }
        #endregion

        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="type">the type of message.</param>
        /// <param name="message">the message string.</param>
        /// <param name="obj">the object of message.</param>
        public ObjectPropertyReport(MessageType type, String message, EcellObject obj, string propertyName)
            : base(type, message, obj)
        {
            m_propName = propertyName;
        }
    }
}
