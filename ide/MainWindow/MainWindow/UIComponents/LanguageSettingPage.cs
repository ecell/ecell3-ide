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
using System.Globalization;

namespace Ecell.IDE.MainWindow.UIComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageSettingPage : PropertyDialogPage
    {
        #region Fields
        private System.Windows.Forms.GroupBox SILangGroupBox;
        private System.Windows.Forms.RadioButton SIEnglishRadioButton;
        private System.Windows.Forms.RadioButton SIJapaneseRadioButton;
        private System.Windows.Forms.RadioButton SIAutoRadioButton;
        /// <summary>
        /// The language code
        /// </summary>
        private CultureInfo m_lang = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public LanguageSettingPage()
        {
            InitializeComponent();
            string lang = Util.GetLanguage().TwoLetterISOLanguageName;
            switch (lang)
            {
                case "en":
                case "en-US":
                    SIEnglishRadioButton.Checked = true;
                    break;
                case "ja":
                    SIJapaneseRadioButton.Checked = true;
                    break;
                default:
                    SIAutoRadioButton.Checked = true;
                    break;
            }

        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageSettingPage));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label detailLALabel;
            this.SILangGroupBox = new System.Windows.Forms.GroupBox();
            this.SIEnglishRadioButton = new System.Windows.Forms.RadioButton();
            this.SIJapaneseRadioButton = new System.Windows.Forms.RadioButton();
            this.SIAutoRadioButton = new System.Windows.Forms.RadioButton();
            label1 = new System.Windows.Forms.Label();
            detailLALabel = new System.Windows.Forms.Label();
            this.SILangGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SILangGroupBox
            // 
            resources.ApplyResources(this.SILangGroupBox, "SILangGroupBox");
            this.SILangGroupBox.Controls.Add(this.SIEnglishRadioButton);
            this.SILangGroupBox.Controls.Add(this.SIJapaneseRadioButton);
            this.SILangGroupBox.Controls.Add(this.SIAutoRadioButton);
            this.SILangGroupBox.ForeColor = System.Drawing.Color.Black;
            this.SILangGroupBox.Name = "SILangGroupBox";
            this.SILangGroupBox.TabStop = false;
            // 
            // SIEnglishRadioButton
            // 
            resources.ApplyResources(this.SIEnglishRadioButton, "SIEnglishRadioButton");
            this.SIEnglishRadioButton.Name = "SIEnglishRadioButton";
            this.SIEnglishRadioButton.TabStop = true;
            this.SIEnglishRadioButton.UseVisualStyleBackColor = true;
            this.SIEnglishRadioButton.CheckedChanged += new System.EventHandler(this.SIEnglishRadioButton_CheckedChanged);
            // 
            // SIJapaneseRadioButton
            // 
            resources.ApplyResources(this.SIJapaneseRadioButton, "SIJapaneseRadioButton");
            this.SIJapaneseRadioButton.Name = "SIJapaneseRadioButton";
            this.SIJapaneseRadioButton.TabStop = true;
            this.SIJapaneseRadioButton.UseVisualStyleBackColor = true;
            this.SIJapaneseRadioButton.CheckedChanged += new System.EventHandler(this.SIJapaneseRadioButton_CheckedChanged);
            // 
            // SIAutoRadioButton
            // 
            resources.ApplyResources(this.SIAutoRadioButton, "SIAutoRadioButton");
            this.SIAutoRadioButton.Name = "SIAutoRadioButton";
            this.SIAutoRadioButton.TabStop = true;
            this.SIAutoRadioButton.UseVisualStyleBackColor = true;
            this.SIAutoRadioButton.CheckedChanged += new System.EventHandler(this.SIAutoRadioButton_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // detailLALabel
            // 
            resources.ApplyResources(detailLALabel, "detailLALabel");
            detailLALabel.Name = "detailLALabel";
            // 
            // LanguageSettingPage
            // 
            this.Controls.Add(detailLALabel);
            this.Controls.Add(label1);
            this.Controls.Add(this.SILangGroupBox);
            this.Name = "LanguageSettingPage";
            resources.ApplyResources(this, "$this");
            this.SILangGroupBox.ResumeLayout(false);
            this.SILangGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// The check of Auto RadioButton is changed.
        /// </summary>
        /// <param name="sender">RadioButton</param>
        /// <param name="e">EventArgs</param>
        private void SIAutoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// The check of japanese RadioButton is changed.
        /// </summary>
        /// <param name="sender">RadioButton</param>
        /// <param name="e">EventArgs</param>
        private void SIJapaneseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.GetCultureInfo("ja");
        }

        /// <summary>
        /// The check of english RadioButton is changed.
        /// </summary>
        /// <param name="sender">RadioButton</param>
        /// <param name="e">EventArgs</param>
        private void SIEnglishRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.GetCultureInfo("en-us");
        }
        #endregion

        /// <summary>
        /// Apply this property.
        /// </summary>
        public override void ApplyChange()
        {
            if (m_lang.Equals(Util.GetLanguage()))
                return;

            Util.SetLanguage(m_lang);
            Util.ShowNoticeDialog(MessageResources.ConfirmRestart);
        }
    }
}
