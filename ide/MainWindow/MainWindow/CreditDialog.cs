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
    public partial class CreditDialog : Form
    {
        public CreditDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreditDialogLoad(object sender, EventArgs e)
        {
            string dir = Util.GetBinDir();
            string cresitFile = Path.Combine(dir, "credit.info");
            if (File.Exists(cresitFile))
            {
                string text = File.ReadAllText(cresitFile);
                richTextBox1.Text = text;
            }
        }
    }
}