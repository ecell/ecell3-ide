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

namespace EcellLib.TracerWindow
{
    /// <summary>
    /// Form class to save the trace.
    /// </summary>
    public partial class SaveTraceWindow : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SaveTraceWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close this window.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void STCloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Set the directory to save the trace.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void STSearchDirButtonClick(object sender, EventArgs e)
        {
            if (DialogResult.Cancel == m_folderDialog.ShowDialog())
            {
                dirTextBox.Text = "";
            }
            else
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
        /// Save the trace which is check on.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void STSaveButtonClick(object sender, EventArgs e)
        {
            double start, end;
            string dirName = "";
            string fileType = "";
            List<string> fullID = new List<string>();
            fileType = typeComboBox.Text;
            if (dirTextBox.Text == "") dirName = "";
            else dirName = dirTextBox.Text;

            for (int i = 0; i < SaveEntrySelectView.Rows.Count; i++)
            {
                if ((bool)SaveEntrySelectView[0, i].Value == true)
                {
                    fullID.Add(SaveEntrySelectView[1, i].Value.ToString());
                }
            }

            if (startTextBox.Text == "" || startTextBox.Text == null) start = 0.0;
            else start = Convert.ToDouble(startTextBox.Text);

            if (endTextBox.Text == "" || endTextBox.Text == null) end = 0.0;
            else end = Convert.ToDouble(endTextBox.Text);

            DataManager manager = DataManager.GetDataManager();
            manager.SaveSimulationResult(dirName, start, end, fileType, fullID);

            String mes = TracerWindow.s_resources.GetString(MessageConstants.FinishSave);
            Util.ShowNoticeDialog(mes);
        }
    }
}