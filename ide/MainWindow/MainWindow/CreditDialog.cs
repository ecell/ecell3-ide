using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// Credit dialog
    /// </summary>
    public partial class CreditDialog : Form
    {
        /// <summary>
        /// constructors.
        /// </summary>
        public CreditDialog()
        {
            InitializeComponent();
        }

        #region Events
        /// <summary>
        /// Click ok button
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">EventArgs</param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Load credit dialog
        /// </summary>
        /// <param name="sender">CreditDialog</param>
        /// <param name="e">EventArgs</param>
        private void CreditDialogLoad(object sender, EventArgs e)
        {
            string dir = Util.GetBinDir();
            string cresitFile = Path.Combine(dir, "credit.info");
            if (File.Exists(cresitFile))
            {
                string text = File.ReadAllText(cresitFile);
                richTextBox1.Text = text;
            }
            button1.Focus();
        }
        #endregion
    }
}