using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            label1.Text = _.Message("Version") + " "
                          + Assembly.GetExecutingAssembly().GetName().Version
                          + "\r\n"
                          + global::MainWindow.Properties.Resources.CopyrightNotice;
        }
    }
}