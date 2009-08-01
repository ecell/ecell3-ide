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
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.AccessControl;
using Ecell.Exceptions;

namespace Ecell.IDE.MainWindow.UIComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class RootFolderSettingPage : PropertyDialogPage
    {
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button button;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;


        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public RootFolderSettingPage()
        {
            InitializeComponent();
            textBox.Text = Util.GetBaseDir();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RootFolderSettingPage));
            System.Windows.Forms.Label detailWSLabel;
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox = new System.Windows.Forms.TextBox();
            this.button = new System.Windows.Forms.Button();
            label = new System.Windows.Forms.Label();
            detailWSLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label
            // 
            resources.ApplyResources(label, "label");
            label.Name = "label";
            // 
            // textBox
            // 
            resources.ApplyResources(this.textBox, "textBox");
            this.textBox.Name = "textBox";
            // 
            // button
            // 
            resources.ApplyResources(this.button, "button");
            this.button.Name = "button";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // detailWSLabel
            // 
            resources.ApplyResources(detailWSLabel, "detailWSLabel");
            detailWSLabel.Name = "detailWSLabel";
            // 
            // RootFolderSettingPage
            // 
            this.Controls.Add(detailWSLabel);
            this.Controls.Add(this.button);
            this.Controls.Add(this.textBox);
            this.Controls.Add(label);
            this.Name = "RootFolderSettingPage";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = textBox.Text;
            using (folderBrowserDialog)
            {
                folderBrowserDialog.Description = MessageResources.ExpModelMes;
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;
                textBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ApplyChange()
        {
            if (string.IsNullOrEmpty(textBox.Text) || !Directory.Exists(textBox.Text))
                return;
            Util.SetBaseDir(textBox.Text);
        }

        public override void PropertyDialogClosing()
        {
            base.PropertyDialogClosing();
            string path = textBox.Text;
            if (string.IsNullOrEmpty(path))
                throw new EcellException(string.Format(MessageResources.ErrNoSet, "WorkSpace" ));

            
        }
    }
}
