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
using Ecell.Objects;

namespace Ecell.Action
{

    /// <summary>
    /// Action class to change the properties of object.
    /// </summary>
    public class DataChangeAction : UserAction
    {
        #region Fields
        /// <summary>
        /// An object before changing.
        /// </summary>
        private EcellObject m_oldObj;
        /// <summary>
        /// An object after changing.
        /// </summary>
        private EcellObject m_newObj;
        #endregion

        /// <summary>
        /// The constructor for DataChangeAction with initial parameters.
        /// </summary>
        /// <param name="oldObj">An object before changing.</param>
        /// <param name="newObj">An object after changing.</param>
        public DataChangeAction(EcellObject oldObj, EcellObject newObj)
        {
            m_oldObj = oldObj.Clone();
            m_newObj = newObj.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DataChangeAction:" + m_isAnchor.ToString() + ", " + m_oldObj.ToString() + ", " + m_newObj.ToString();
        }

        /// <summary>
        /// Execute to change the object using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.DataChanged(m_oldObj.ModelID, m_oldObj.Key, m_oldObj.Type, m_newObj.Clone(), false, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// Changing will be aborted.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.DataChanged(m_newObj.ModelID, m_newObj.Key, m_newObj.Type, m_oldObj.Clone(), false, false);
        }
    }
}
