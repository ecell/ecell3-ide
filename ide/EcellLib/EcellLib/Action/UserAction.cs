//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell.Action
{
    /// <summary>
    /// Abstract class for action.
    /// </summary>
    public abstract class UserAction
    {
        #region Fields
        /// <summary>
        /// Whether this UserAction is the last one in a sequence of UserAction.
        /// </summary>
        protected bool m_isAnchor = false;
        /// <summary>
        /// Whether this UserAction is undoable or not.
        /// </summary>
        protected bool m_isUndoable = true;

        /// <summary>
        /// ApplicationEnvironment
        /// </summary>
        protected ApplicationEnvironment m_env;
        #endregion

        #region Accessors
        /// <summary>
        /// Whether this UserAction is the last one in a sequence of UserAction.
        /// </summary>
        public bool IsAnchor
        {
            get { return m_isAnchor; }
        }
        /// <summary>
        /// Whether this UserAction is undoable or not.
        /// </summary>
        public bool IsUndoable
        {
            get { return m_isUndoable; }
        }

        /// <summary>
        /// get / set the Environment.
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            internal set { m_env = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Abstract function of UserAction.
        /// Execute action of this UserAction.
        /// </summary>
        public abstract void Execute();
        /// <summary>
        /// Abstract function of UserAction.
        /// Unexecute action of this UserAction.
        /// </summary>
        public abstract void UnExecute();

        #endregion
    }
}
