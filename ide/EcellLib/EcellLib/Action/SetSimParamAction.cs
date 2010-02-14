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
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Action
{

    /// <summary>
    /// Action class to setup parameter of simulation.
    /// </summary>
    public class SetSimParamAction : UserAction
    {
        #region Fields
        /// <summary>
        /// New parameter ID.
        /// </summary>
        private string m_newParamID;
        /// <summary>
        /// Old parameter ID.
        /// </summary>
        private string m_oldParamID;
        #endregion

        /// <summary>
        /// The constructor for SetSimParamAction with initial parameters.
        /// </summary>
        /// <param name="newParamID">new parameter ID</param>
        /// <param name="oldParamID">old parameter ID</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public SetSimParamAction(string newParamID, string oldParamID, bool isAnchor)
        {
            m_newParamID = newParamID;
            m_oldParamID = oldParamID;
            m_isAnchor = isAnchor;
        }

        /// <summary>
        /// Convert this object to string.
        /// </summary>
        /// <returns>object string.</returns>
        public override string ToString()
        {
            return "SetSimParamAction:" + m_isAnchor.ToString() + ", " + m_oldParamID + ", " + m_newParamID;
        }
        /// <summary>
        /// Execute to set the simulation parameter using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.SetSimulationParameter(m_newParamID, false, false);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.SetSimulationParameter(m_oldParamID, false, false);
        }
    }
}
