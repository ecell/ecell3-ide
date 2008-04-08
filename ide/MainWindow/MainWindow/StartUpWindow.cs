using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.MainWindow
{
    class StartUpWindow : EcellDockContent
    {
        private WebBrowser EcellBrowser;
        private static string URL = "http://chaperone.e-cell.org/trac/ecell-ide";

        public StartUpWindow()
        {
            InitializeComponent();
            this.EcellBrowser.Navigate(URL);
        }

        private void InitializeComponent()
        {
            this.EcellBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.EcellBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EcellBrowser.Location = new System.Drawing.Point(0, 0);
            this.EcellBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.EcellBrowser.Name = "webBrowser1";
            this.EcellBrowser.Size = new System.Drawing.Size(688, 501);
            this.EcellBrowser.TabIndex = 0;
            this.EcellBrowser.Url = new System.Uri(URL, System.UriKind.Absolute);
            // 
            // StartUpWindow
            // 
            this.ClientSize = new System.Drawing.Size(688, 501);
            this.Controls.Add(this.EcellBrowser);
            this.Name = "StartUpWindow";
            this.ResumeLayout(false);

        }
    }
}
