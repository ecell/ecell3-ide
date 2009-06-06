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

namespace Ecell.IDE.MainWindow
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
        /// 
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
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LanguageSettingPage));
            this.SILangGroupBox = new System.Windows.Forms.GroupBox();
            this.SIEnglishRadioButton = new System.Windows.Forms.RadioButton();
            this.SIJapaneseRadioButton = new System.Windows.Forms.RadioButton();
            this.SIAutoRadioButton = new System.Windows.Forms.RadioButton();
            this.SILangGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SILangGroupBox
            // 
            this.SILangGroupBox.AccessibleDescription = null;
            this.SILangGroupBox.AccessibleName = null;
            resources.ApplyResources(this.SILangGroupBox, "SILangGroupBox");
            this.SILangGroupBox.BackgroundImage = null;
            this.SILangGroupBox.Controls.Add(this.SIEnglishRadioButton);
            this.SILangGroupBox.Controls.Add(this.SIJapaneseRadioButton);
            this.SILangGroupBox.Controls.Add(this.SIAutoRadioButton);
            this.SILangGroupBox.Name = "SILangGroupBox";
            this.SILangGroupBox.TabStop = false;
            // 
            // SIEnglishRadioButton
            // 
            this.SIEnglishRadioButton.AccessibleDescription = null;
            this.SIEnglishRadioButton.AccessibleName = null;
            resources.ApplyResources(this.SIEnglishRadioButton, "SIEnglishRadioButton");
            this.SIEnglishRadioButton.BackgroundImage = null;
            this.SIEnglishRadioButton.Name = "SIEnglishRadioButton";
            this.SIEnglishRadioButton.TabStop = true;
            this.SIEnglishRadioButton.UseVisualStyleBackColor = true;
            this.SIEnglishRadioButton.CheckedChanged += new System.EventHandler(this.SIEnglishRadioButton_CheckedChanged);
            // 
            // SIJapaneseRadioButton
            // 
            this.SIJapaneseRadioButton.AccessibleDescription = null;
            this.SIJapaneseRadioButton.AccessibleName = null;
            resources.ApplyResources(this.SIJapaneseRadioButton, "SIJapaneseRadioButton");
            this.SIJapaneseRadioButton.BackgroundImage = null;
            this.SIJapaneseRadioButton.Name = "SIJapaneseRadioButton";
            this.SIJapaneseRadioButton.TabStop = true;
            this.SIJapaneseRadioButton.UseVisualStyleBackColor = true;
            this.SIJapaneseRadioButton.CheckedChanged += new System.EventHandler(this.SIJapaneseRadioButton_CheckedChanged);
            // 
            // SIAutoRadioButton
            // 
            this.SIAutoRadioButton.AccessibleDescription = null;
            this.SIAutoRadioButton.AccessibleName = null;
            resources.ApplyResources(this.SIAutoRadioButton, "SIAutoRadioButton");
            this.SIAutoRadioButton.BackgroundImage = null;
            this.SIAutoRadioButton.Name = "SIAutoRadioButton";
            this.SIAutoRadioButton.TabStop = true;
            this.SIAutoRadioButton.UseVisualStyleBackColor = true;
            this.SIAutoRadioButton.CheckedChanged += new System.EventHandler(this.SIAutoRadioButton_CheckedChanged);
            // 
            // LanguageSettingPage
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.SILangGroupBox);
            this.Font = null;
            this.Name = "LanguageSettingPage";
            this.SILangGroupBox.ResumeLayout(false);
            this.SILangGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SIAutoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SIJapaneseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.GetCultureInfo("ja");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SIEnglishRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            m_lang = CultureInfo.GetCultureInfo("en-us");
        }
        #endregion

        /// <summary>
        /// 
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
