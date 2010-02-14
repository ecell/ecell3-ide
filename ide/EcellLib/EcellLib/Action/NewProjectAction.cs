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
    /// Action class to create the project.
    /// </summary>
    public class NewProjectAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The name of created project.
        /// </summary>
        private string m_prjName;
        /// <summary>
        /// The comment of created project.
        /// </summary>
        private string m_comment;
        #endregion

        /// <summary>
        /// The constructor for NewProjectAction with initial parameters.
        /// </summary>
        /// <param name="prjName">the projectID.</param>
        /// <param name="comment">the project comment.</param>
        public NewProjectAction(string prjName, string comment)
        {
            m_prjName = prjName;
            m_comment = comment;
            m_isUndoable = false;
            m_isAnchor = true;
        }

        /// <summary>
        /// Convert this object to string.
        /// </summary>
        /// <returns>object string.</returns>
        public override string ToString()
        {
            return "NewProjectAction:" + m_prjName;
        }

        /// <summary>
        /// Execute to create the project using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.CreateNewProject(m_prjName, m_comment, new List<string>());
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }
        /// <summary>
        /// Do nothing.
        /// </summary>
        public override void UnExecute()
        {
        }
    }
}
