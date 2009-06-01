//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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

using Ecell.Job;
using Ecell.Plugin;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Form to display the status of jobs.
    /// </summary>
    public partial class GridJobStatusDialog : EcellDockContent
    {
        #region
        /// <summary>
        /// SessionManager
        /// </summary>
        IJobManager m_manager;
        Timer m_timer;
        private TreeNode m_topNode = null;
        private Dictionary<string, TreeNode> m_pointDic = new Dictionary<string, TreeNode>();
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public GridJobStatusDialog(IJobManager manager)
            : base()
        {
            InitializeComponent();

            m_manager = manager;

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Enabled = false;
            m_timer.Interval = 3000;
            m_timer.Tick += new EventHandler(FireTimer);
        }

        public void ChangeStatus(ProjectStatus status)
        {
            if (status == ProjectStatus.Analysis)
            {
                m_timer.Enabled = true;
                m_timer.Start();
            }
            else
            {
                m_timer.Enabled = false;
                m_timer.Stop();
            }
        }

        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void FireTimer(object sender, EventArgs e)
        {
            m_timer.Enabled = false;
            m_timer.Enabled = true;
        }

        /// <summary>
        /// Event when this form is shown.
        /// Display the all list of job that is got from SessionManager.
        /// </summary>
        /// <param name="sender">Form.</param>
        /// <param name="e">EventArgs.</param>
        private void WinShown(object sender, EventArgs e)
        {
            jobTreeView.Nodes.Clear();
            m_pointDic.Clear();

            m_topNode = new TreeNode(m_manager.Environment.DataManager.CurrentProject.Info.Name);

            foreach (string name in m_manager.GroupDic.Keys)
            {
                AddJobGroup(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddJobGroup(string name)
        {
            JobGroup group = m_manager.GroupDic[name];
            string analysisName = group.AnalysisName;
            if (!m_pointDic.ContainsKey(analysisName))
            {
                TreeNode node = new TreeNode(analysisName);
                m_topNode.Nodes.Add(node);
                m_pointDic[analysisName] = node;
            }
            JobGroupTreeNode groupNode = new JobGroupTreeNode(name);
            m_pointDic[analysisName].Nodes.Add(groupNode);
            foreach (Job.Job job in group.Jobs)
            {
                JobTreeNode jobNode = new JobTreeNode(name, job.JobID.ToString());
                groupNode.Nodes.Add(jobNode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void DeleteJobGroup(string name)
        {
            JobGroup group = m_manager.GroupDic[name];
            string analysisName = group.AnalysisName;
            if (!m_pointDic.ContainsKey(analysisName))
                return;
            foreach (TreeNode node in m_pointDic[analysisName].Nodes)
            {
                if (node.Text.Equals(node))
                {
                    m_pointDic[analysisName].Nodes.Remove(node);
                    return;
                }
            }
        }

        /// <summary>
        /// Event when the close button is clicked.
        /// Close this form.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridJobStatusDialog_DockStateChanged(object sender, EventArgs e)
        {
            ResetToolTip();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetToolTip()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridJobStatusDialog));
            this.DialogToolTip = new System.Windows.Forms.ToolTip(this.components);
//            this.DialogToolTip.SetToolTip(this.DEWStartButton, resources.GetString("DEWStartButton.ToolTip"));
//            this.DialogToolTip.SetToolTip(this.DEWStopButton, resources.GetString("DEWStopButton.ToolTip"));
//            this.DialogToolTip.SetToolTip(this.DEWDeleteButton, resources.GetString("DEWDeleteButton.ToolTip"));
//            this.DialogToolTip.SetToolTip(this.DEWUpdateButton, resources.GetString("DEWUpdateButton.ToolTip"));
            this.DialogToolTip.ShowAlways = true;

        }

        private void TreeNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (!(node is JobTreeNode) && !(node is JobGroupTreeNode))
            {
                ClearInformation();
                return;
            }
            if (node is JobTreeNode)
            {
                JobTreeNode jobNode = node as JobTreeNode;
                Job.Job job = m_manager.GroupDic[jobNode.GroupName].GetJob(Int32.Parse(node.Text));
                foreach (string fullPN in job.ExecParam.ParamDic.Keys)
                {
                    double data = job.ExecParam.ParamDic[fullPN];
                    parameterDataGridView.Rows.Add(new object[] { fullPN, data });
                }
            }
            if (node is JobGroupTreeNode)
            {
                JobGroup group = m_manager.GroupDic[node.Text];
                foreach (string name in group.AnalysisParameter.Keys)
                {
                    string data = group.AnalysisParameter[name];
                    parameterDataGridView.Rows.Add(new object[] { name, data });
                }
            }
        }

        private void ClearInformation()
        {
            jobIDTextBox.Text = "";
            statusTextBox.Text = "";
            parameterDataGridView.Rows.Clear();
        }
    }

    public class JobGroupTreeNode : TreeNode
    {
        public JobGroupTreeNode(string text)
            : base(text)
        {
        }
    }

    public class JobTreeNode : TreeNode
    {
        private string m_groupName;
        public JobTreeNode(string groupName, string text)
            : base(text)
        {
            m_groupName = groupName;
        }
        public string GroupName
        {
            get { return this.m_groupName; }
            set { this.m_groupName = value; }
        }
    }
}