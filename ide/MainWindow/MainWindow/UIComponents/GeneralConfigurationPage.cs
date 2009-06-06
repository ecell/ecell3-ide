//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.IDE;
using Ecell.Exceptions;

namespace Ecell.IDE.MainWindow.UIComponents
{
    /// <summary>
    /// GeneralConfigurationPage
    /// </summary>
    public partial class GeneralConfigurationPage : PropertyDialogPage
    {
        private Label label1;
        private ComboBox valueFormatComboBox;
        private DataManager m_manager;
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public GeneralConfigurationPage(DataManager manager)
        {
            InitializeComponent();
            m_manager = manager;

            switch (m_manager.DisplayFormat)
            {
                case ValueDataFormat.Normal:
                    valueFormatComboBox.SelectedIndex = 0;
                    break;
                case ValueDataFormat.Exponential1:
                    valueFormatComboBox.SelectedIndex = 1;
                    break;
                case ValueDataFormat.Exponential2:
                    valueFormatComboBox.SelectedIndex = 2;
                    break;
                case ValueDataFormat.Exponential3:
                    valueFormatComboBox.SelectedIndex = 3;
                    break;
                case ValueDataFormat.Exponential4:
                    valueFormatComboBox.SelectedIndex = 4;
                    break;
                case ValueDataFormat.Exponential5:
                    valueFormatComboBox.SelectedIndex = 5;
                    break;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralConfigurationPage));
            this.label1 = new System.Windows.Forms.Label();
            this.valueFormatComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // valueFormatComboBox
            // 
            this.valueFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueFormatComboBox.FormattingEnabled = true;
            this.valueFormatComboBox.Items.AddRange(new object[] {
            resources.GetString("valueFormatComboBox.Items"),
            resources.GetString("valueFormatComboBox.Items1"),
            resources.GetString("valueFormatComboBox.Items2"),
            resources.GetString("valueFormatComboBox.Items3"),
            resources.GetString("valueFormatComboBox.Items4"),
            resources.GetString("valueFormatComboBox.Items5")});
            resources.ApplyResources(this.valueFormatComboBox, "valueFormatComboBox");
            this.valueFormatComboBox.Name = "valueFormatComboBox";
            // 
            // GeneralConfigurationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valueFormatComboBox);
            this.Controls.Add(this.label1);
            this.Name = "GeneralConfigurationPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public override void ApplyChange()
        {
            ValueDataFormat value = ValueDataFormat.Normal;

            switch (valueFormatComboBox.SelectedIndex)
            {
                case 0:
                    value = ValueDataFormat.Normal;
                    break;
                case 1:
                    value = ValueDataFormat.Exponential1;
                    break;
                case 2:
                    value = ValueDataFormat.Exponential2;
                    break;
                case 3:
                    value = ValueDataFormat.Exponential3;
                    break;
                case 4:
                    value = ValueDataFormat.Exponential4;
                    break;
                case 5:
                    value = ValueDataFormat.Exponential5;
                    break;
            }

            m_manager.DisplayFormat = value;
        }
    }
}
