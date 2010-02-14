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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Reporting
{
    /// <summary>
    /// Report for Compile
    /// </summary>
    public class CompileReport : Report
    {
        /// <summary>
        /// get the location of report.
        /// </summary>
        public override string Location
        {
            get
            {
                string[] ele = Message.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                return ele[0];
            }
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">message type.</param>
        /// <param name="message">message.</param>
        /// <param name="group">report group name.</param>
        public CompileReport(MessageType type, string message, string group)
            : base (type, message, group)
        {
        }

        /// <summary>
        /// Override Equal functions.
        /// </summary>
        /// <param name="obj">the compared object.</param>
        /// <returns>if equal, return true.</returns>
        public override bool Equals(object obj)
        {
            if (obj is CompileReport)
            {
                CompileReport dst = obj as CompileReport;
                if (this.Type == dst.Type &&
                    this.Message.Equals(dst.Message) &&
                    this.Group.Equals(dst.Group) &&
                    this.Location.Equals(dst.Location))
                    return true;
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Override get the hash code.
        /// </summary>
        /// <returns>the hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
