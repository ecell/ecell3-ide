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

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Form class to save the trace.
    /// </summary>
    public partial class SaveTraceDialog : Form
    {
        private TracerWindow m_owner;
        private bool m_isCancel = false;
        private string m_directoryName;
        private double m_start;
        private double m_end;
        private string m_fileType;
        private List<string> m_saveList;

        public string DirectoryName
        {
            get { return this.m_directoryName; }
        }

        public double Start
        {
            get { return this.m_start; }
        }

        public double End
        {
            get { return this.m_end; }
        }

        public string FileType
        {
            get { return this.m_fileType; }
        }

        public List<string> SaveList
        {
            get { return m_saveList; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SaveTraceDialog(TracerWindow owner)
        {
            m_owner = owner;
            InitializeComponent();
            dirTextBox.Text = owner.DataManager.GetSimulationResultSaveDirectory();
            endTextBox.Text = Convert.ToString(owner.DataManager.GetCurrentSimulationTime());
        }

        /// <summary>
        /// Close this window.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void STCloseButtonClick(object sender, EventArgs e)
        {
            m_isCancel = true;
            this.Close();
        }

        /// <summary>
        /// Set the directory to save the trace.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void STSearchDirButtonClick(object sender, EventArgs e)
        {
            m_folderDialog.SelectedPath = dirTextBox.Text;
            if (DialogResult.OK == m_folderDialog.ShowDialog())
            {
                dirTextBox.Text = m_folderDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Add the all logger entry in save target. 
        /// </summary>
        /// <param name="entry">the all logger entry.</param>
        public void AddEntry(Dictionary<TagData, List<TraceWindow>> entry)
        {
            foreach (TagData tag in entry.Keys)
            {
                SaveEntrySelectView.Rows.Add(new object[] {true, tag.M_path});
            }
        }

        private void SaveTraceDialogClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (m_isCancel == false)
                {
                    m_fileType = typeComboBox.Text;
                    if (dirTextBox.Text == "") m_directoryName = "";
                    else m_directoryName = dirTextBox.Text;

                    if (startTextBox.Text == "" || startTextBox.Text == null) m_start = 0.0;
                    else m_start = Convert.ToDouble(startTextBox.Text);

                    if (endTextBox.Text == "" || endTextBox.Text == null) m_end = 0.0;
                    else m_end = Convert.ToDouble(endTextBox.Text);

                    m_saveList = new List<string>();
                    for (int i = 0; i < SaveEntrySelectView.Rows.Count; i++)
                    {
                        if ((bool)SaveEntrySelectView[0, i].Value == true)
                        {
                            m_saveList.Add(SaveEntrySelectView[1, i].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrInputData);
                e.Cancel = true;
            }
        }
    }
}
