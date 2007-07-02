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
    /// <summary>
    /// SplashSheet window class.
    /// </summary>
    public partial class Splash : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Splash()
        {
            InitializeComponent();
            VersionNumber.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            CopyrightNotice.Text = global::MainWindow.Properties.Resources.CopyrightNotice;
        }
    }
}