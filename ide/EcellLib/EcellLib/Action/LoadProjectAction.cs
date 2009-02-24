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

namespace Ecell.Action
{

    /// <summary>
    /// Action class to load the project.
    /// </summary>
    public class LoadProjectAction : UserAction
    {
        #region Fields
        /// <summary>
        /// The load project ID.
        /// </summary>
        private string m_prjID;
        /// <summary>
        /// The load project file.
        /// </summary>
        private string m_prjFile;
        #endregion

        /// <summary>
        /// The constructor for LoadProjectAction with initial parameters.
        /// </summary>
        /// <param name="prjID">The load project ID.</param>
        /// <param name="prjFile">The load project file.</param>
        public LoadProjectAction(string prjID, string prjFile)
        {
            m_prjID = prjID;
            m_prjFile = prjFile;
            m_isUndoable = false;
            m_isAnchor = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "LoadProjectAction:" + m_prjFile;
        }
        /// <summary>
        /// Execute to load the project using the information.
        /// </summary>
        public override void Execute()
        {
            m_env.DataManager.LoadProject(m_prjFile);
            m_env.PluginManager.ChangeStatus(ProjectStatus.Loaded);
        }
        /// <summary>
        /// Unexecute this action.
        /// </summary>
        public override void UnExecute()
        {
            m_env.DataManager.CloseProject();
        }
    }
}
