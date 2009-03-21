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
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public GridJobStatusDialog(IJobManager manager)
        {
            m_manager = manager;
            InitializeComponent();               
            JobGridView.CellDoubleClick += new DataGridViewCellEventHandler(JobGridViewDoubleClick);
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
                DEWUpdateButton_Click(null, new EventArgs());
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
            DEWUpdateButton_Click(null, new EventArgs());
            m_timer.Enabled = true;
        }

        /// <summary>
        /// Event when the job entry is double clicked on GridView.
        /// Display the log of this job.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs.</param>
        void JobGridViewDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int jobid = Convert.ToInt32(JobGridView[0, e.RowIndex].Value);
            if (!m_manager.JobList.ContainsKey(jobid)) return;

            JobMessageDialog dialog = new JobMessageDialog(m_manager.JobList[jobid].StdErr);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Event when this form is shown.
        /// Display the all list of job that is got from SessionManager.
        /// </summary>
        /// <param name="sender">Form.</param>
        /// <param name="e">EventArgs.</param>
        private void WinShown(object sender, EventArgs e)
        {
            foreach (Ecell.Job.Job s in m_manager.JobList.Values)
            {
                JobGridView.Rows.Add(new object[] { s.JobID, s.Status, s.Machine, s.ScriptFile, s.Argument });
            }
        }

        /// <summary>
        /// Event when the clear button is clicked.
        /// Delete the all jobs from SessionManager.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DEWClearButtonClick(object sender, EventArgs e)
        {
            m_manager.ClearJob(0);
            JobGridView.Rows.Clear();
        }

        /// <summary>
        /// Event when the delete button is clicked.
        /// Delete the selected job from SessionManager.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DEWDeleteButtonClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in JobGridView.SelectedRows)
            {
                try
                {
                    int jobid = Convert.ToInt32(r.Cells[0].Value);
                    m_manager.ClearJob(jobid);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            JobGridView.Rows.Clear();
            foreach (Ecell.Job.Job s in m_manager.JobList.Values)
            {
                JobGridView.Rows.Add(new object[] { s.JobID, s.Status, s.Machine, s.ScriptFile, s.Argument });
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
        /// Event when the update button is clicked.
        /// Update the latest information of jobs that is got from SessionManager.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DEWUpdateButton_Click(object sender, EventArgs e)
        {
            JobGridView.Rows.Clear();
            foreach (Ecell.Job.Job s in m_manager.JobList.Values)
            {
                JobGridView.Rows.Add(new object[] { s.JobID, s.Status, s.Machine, s.ScriptFile, s.Argument });
            }
            if (JobGridView.SortedColumn != null)
                JobGridView.Sort(JobGridView.SortedColumn, 
                    (JobGridView.SortOrder == SortOrder.Ascending ?  
                    ListSortDirection.Ascending :
                    ListSortDirection.Descending));
        }

        /// <summary>
        /// Event when the stop button is clicked.
        /// Stop the selected jobs.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DEWStopButton_Click(object sender, EventArgs e)
        {
            if (JobGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow r in JobGridView.SelectedRows)
                {
                    int jobid = Convert.ToInt32(r.Cells[0].Value);
                    m_manager.Stop(jobid);
                }
                return;
            }

            m_manager.Stop(0);            
        }

        /// <summary>
        /// Event when the start button is clicked.
        /// Start the selected job.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void DEWStartButton_Click(object sender, EventArgs e)
        {
            if (JobGridView.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow r in JobGridView.SelectedRows)
                {
                    int jobid = Convert.ToInt32(r.Cells[0].Value);
                    m_manager.JobList[jobid].Status = JobStatus.QUEUED;
                }
                m_manager.Run();
                return;
            }
        }
    }
}