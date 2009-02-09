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
        private string m_directoryName;
        private double m_start;
        private double m_end;
        private double m_simTime;
        private string m_fileType;
        private List<string> m_saveList;
        /// <summary>
        /// 
        /// </summary>
        public string DirectoryName
        {
            get { return this.m_directoryName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double Start
        {
            get { return this.m_start; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double End
        {
            get { return this.m_end; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileType
        {
            get { return this.m_fileType; }
        }
        /// <summary>
        /// 
        /// </summary>
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
            m_start = 0.0;
            m_end = owner.DataManager.GetCurrentSimulationTime();
            endTextBox.Text = m_end.ToString();
            double dummy = 0.0;
            if (double.TryParse(endTextBox.Text, out dummy))
                m_end = dummy;
            m_simTime = m_end;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveTraceDialogClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK) return;
            try
            {
                m_fileType = typeComboBox.Text;
                if (dirTextBox.Text == "") m_directoryName = "";
                else m_directoryName = dirTextBox.Text;

                m_saveList = new List<string>();
                for (int i = 0; i < SaveEntrySelectView.Rows.Count; i++)
                {
                    if ((bool)SaveEntrySelectView[0, i].Value == true)
                    {
                        m_saveList.Add(SaveEntrySelectView[1, i].Value.ToString());
                    }
                }
                if (m_saveList.Count <= 0)
                {
                    if (Util.ShowYesNoDialog(MessageResources.ConfirmNoSaveLog) == false)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrInputData);
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTime_Validating(object sender, CancelEventArgs e)
        {
            string text = startTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                text = "0.0";
            }
            double dummy;
            if (!Double.TryParse(text, out dummy) || dummy < 0.0 || m_end <= dummy || m_simTime < dummy)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                startTextBox.Text = Convert.ToString(m_start);
                e.Cancel = true;
                return;
            }
            m_start = dummy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndTime_Validating(object sender, CancelEventArgs e)
        {
            string text = endTextBox.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameEndTime));
                endTextBox.Text = Convert.ToString(m_end);
                e.Cancel = true;
                return;
            }
            double dummy;
            if (!Double.TryParse(text, out dummy) || dummy < 0.0 || m_start >= dummy || m_simTime < dummy)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                endTextBox.Text = m_end.ToString();
                e.Cancel = true;
                return;
            }
            m_end = dummy;
        }
    }
}
