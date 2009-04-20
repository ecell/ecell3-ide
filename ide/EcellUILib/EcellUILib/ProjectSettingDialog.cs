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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE
{
    /// <summary>
    /// Dialog to create the project.
    /// </summary>
    public partial class ProjectSettingDialog : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private ProjectInfo m_info;

        /// <summary>
        /// ProjectInfo
        /// </summary>
        public ProjectInfo ProjectInfo
        {
            get { return m_info; }
            set
            {
                m_info = value;
                textName.Text = m_info.Name;
                textCreator.Text = m_info.Creator;
                textCreated.Text = m_info.CreationTime;
                textLastUpdate.Text = m_info.UpdateTime;
                textEditCount.Text = m_info.EditCount.ToString();
                textDefaultParameter.Text = m_info.SimulationParam;
                textComment.Text = m_info.Comment;

            }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="info"></param>
        public ProjectSettingDialog(ProjectInfo info)
        {
            InitializeComponent();
            this.ProjectInfo = info;
            FormClosing += new FormClosingEventHandler(ProjectSettingDialog_FormClosing);
        }

        private void ProjectSettingDialog_FormClosing(object obj, FormClosingEventArgs args)
        {
            if (DialogResult == DialogResult.OK && !ValidateForm())
            {
                args.Cancel = true;
                return;
            }
        }

        private bool ValidateForm()
        {
            return ValidateProjectName();
        }

        private bool ValidateProjectName()
        {
            string projectName = textName.Text;
            if (string.IsNullOrEmpty(projectName))
            {
                Util.ShowWarningDialog(string.Format(MessageResources.ErrNoSet, "Project ID"));
                return false;
            }
            //if (Util.IsExistProject(projectName)
            //    && !Util.ShowOKCancelDialog(
            //    string.Format(MessageResources.ErrExistProject, projectName)
            //    + "\n" + MessageResources.ConfirmOverwrite)
            //    )
            //{
            //    return false;
            //}
            if (Util.IsNGforIDonWindows(projectName) || projectName.Length > 64)
            {
                Util.ShowWarningDialog(string.Format(MessageResources.ErrInvalidID, "Project ID"));
                return false;
            }
            return true;
        }

    }
}
