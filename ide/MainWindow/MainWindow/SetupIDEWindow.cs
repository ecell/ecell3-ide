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

namespace EcellLib.MainWindow
{
    /// <summary>
    /// The form to setup the language setting.
    /// </summary>
    public partial class SetupIDEWindow : Form
    {
        /// <summary>
        /// The current language setting.
        /// </summary>
        private String m_lang;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetupIDEWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event when this form is shown.
        /// </summary>
        /// <param name="sender">This Form.</param>
        /// <param name="e">EventArgs.</param>
        private void SetupWindowIDEWindowShown(object sender, EventArgs e)
        {
            m_lang = Util.GetLang();
            if (m_lang == null || m_lang.ToUpper() == "AUTO")
            {
                autoRadioButton.Checked = true;
                m_lang = "AUTO";
            }
            else if (m_lang.ToUpper() == "EN_US")
            {
                enRadioButton.Checked = true;
                m_lang = "EN_US";
            }
            else if (m_lang.ToUpper() == "JA")
            {
                jpRadioButton.Checked = true;
                m_lang = "JA";
            }
            else
            {
                autoRadioButton.Checked = true;
                m_lang = "AUTO";
            }
        }

        /// <summary>
        /// Event when cancel button is clicked. This form is closed.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event when OK button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void OKButtonClick(object sender, EventArgs e)
        {
            String tmpLang = "";
            if (autoRadioButton.Checked) tmpLang = "AUTO";
            else if (enRadioButton.Checked) tmpLang = "EN_US";
            else tmpLang = "JA";

            if (tmpLang != m_lang)
            {
                Util.SetLanguage(tmpLang);
                if (tmpLang == "AUTO")
                    MessageBox.Show(MainWindow.s_resources.GetString("ConfirmRestart"), "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (tmpLang == "EN_US")
                    MessageBox.Show("The change will take effect after you restart this application.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Ç±ÇÃê›íËÇÕéüâÒãNìÆéûÇ©ÇÁóLå¯Ç…Ç»ÇËÇ‹Ç∑ÅB", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }
    }
}