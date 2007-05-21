using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;
            MainWindow frm = new MainWindow();
            frm.Shown += new EventHandler(frm_Shown);
            Program.ThisContext.MainForm = frm;
            frm.Show();
        }

        void frm_Shown(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}