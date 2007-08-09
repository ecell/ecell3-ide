using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    public partial class SetupIDEWindow : Form
    {
        private String m_lang;
        /// <summary>
        /// ResourceManager for MainWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResMain));

        /// <summary>
        /// Constructor.
        /// </summary>
        public SetupIDEWindow()
        {
            InitializeComponent();
        }

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

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

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
                    MessageBox.Show(m_resources.GetString("ConfirmRestart"), "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (tmpLang == "EN_US")
                    MessageBox.Show("The change will take effect after you restart this application.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Ç±ÇÃê›íËÇÕéüâÒãNìÆéûÇ©ÇÁóLå¯Ç…Ç»ÇËÇ‹Ç∑ÅB", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }
    }
}