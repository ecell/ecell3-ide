//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Threading;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// StartUpWindow
    /// </summary>
    public class EcellWebBrowser : EcellDockContent
    {
        #region Fields
        private const string URL = "http://chaperone.e-cell.org/trac/ecell-ide";
        private PictureBox pictureBox;
        private GroupBox groupBox;
        private WebBrowser webBrowser;
        private ToolStripStatusLabel URLLabel;
        private ToolStrip toolStrip;
        private ToolStripButton ButtonBack;
        private ToolStripButton ButtonForward;
        private ToolStripComboBox URLComboBox;
        private ToolStripButton ButtonStop;
        private ToolStripButton ButtonNavigate;
        private Panel panel1;
        private ToolStripButton ButtonRefresh;
        private MainWindow m_window = null;
        private static Uri STARTUP;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public EcellWebBrowser(MainWindow window)
        {
            m_window = window;
            InitializeComponent();
            this.Text = MessageResources.StartUpWindow;
            this.TabText = this.Text;
            Uri startPage = FindStartPage();
            STARTUP = startPage;
            if (startPage != null)
            {
                webBrowser.Navigate(startPage);
            }
            SetRecentFiles();
        }

        private Uri FindStartPage()
        {
            List<string> candidates = new List<string>();

            string documentDir = Util.GetWindowSettingDir();
            string lang = Util.GetLang();
            if (documentDir != null)
            {
                candidates.Add(Path.Combine(documentDir,
                    Constants.fileStartupHTML));
                candidates.Add(Path.Combine(documentDir,
                    lang + "_" + Constants.fileStartupHTML));
            }
            foreach (string candidate in candidates)
            {
                if (File.Exists(candidate))
                {
                    UriBuilder ub = new UriBuilder("file", null);
                    ub.Path = candidate;
                    return ub.Uri;
                }
            }
            return null;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EcellWebBrowser));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.URLLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ButtonBack = new System.Windows.Forms.ToolStripButton();
            this.ButtonForward = new System.Windows.Forms.ToolStripButton();
            this.ButtonStop = new System.Windows.Forms.ToolStripButton();
            this.ButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.ButtonNavigate = new System.Windows.Forms.ToolStripButton();
            this.URLComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(259, 89);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox.Location = new System.Drawing.Point(12, 95);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(245, 345);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Recent Projects";
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(265, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(453, 443);
            this.webBrowser.TabIndex = 3;
            this.webBrowser.CanGoForwardChanged += new System.EventHandler(this.webBrowser_CanGoForwardChanged);
            this.webBrowser.CanGoBackChanged += new System.EventHandler(this.webBrowser_CanGoBackChanged);
            this.webBrowser.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.webBrowser_ProgressChanged);
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.StatusTextChanged += new System.EventHandler(this.webBrowser_StatusTextChanged);
            // 
            // URLLabel
            // 
            this.URLLabel.BackColor = System.Drawing.SystemColors.Control;
            this.URLLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.URLLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.URLLabel.Name = "URLLabel";
            this.URLLabel.Size = new System.Drawing.Size(672, 17);
            this.URLLabel.Spring = true;
            this.URLLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonBack,
            this.ButtonForward,
            this.ButtonStop,
            this.ButtonRefresh,
            this.ButtonNavigate,
            this.URLComboBox});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(718, 25);
            this.toolStrip.TabIndex = 5;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.SizeChanged += new System.EventHandler(this.toolStrip_SizeChanged);
            // 
            // ButtonBack
            // 
            this.ButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonBack.Image = ((System.Drawing.Image)(resources.GetObject("ButtonBack.Image")));
            this.ButtonBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonBack.Name = "ButtonBack";
            this.ButtonBack.Size = new System.Drawing.Size(23, 22);
            this.ButtonBack.Text = "Back";
            this.ButtonBack.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonForward
            // 
            this.ButtonForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonForward.Image = ((System.Drawing.Image)(resources.GetObject("ButtonForward.Image")));
            this.ButtonForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonForward.Name = "ButtonForward";
            this.ButtonForward.Size = new System.Drawing.Size(23, 22);
            this.ButtonForward.Text = "Go";
            this.ButtonForward.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("ButtonStop.Image")));
            this.ButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(23, 22);
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRefresh.Image")));
            this.ButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(23, 22);
            this.ButtonRefresh.Text = "Refresh";
            this.ButtonRefresh.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonNavigate
            // 
            this.ButtonNavigate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonNavigate.Image = ((System.Drawing.Image)(resources.GetObject("ButtonNavigate.Image")));
            this.ButtonNavigate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonNavigate.Name = "ButtonNavigate";
            this.ButtonNavigate.Size = new System.Drawing.Size(23, 22);
            this.ButtonNavigate.Text = "Navigate";
            this.ButtonNavigate.Click += new System.EventHandler(this.Button_Click);
            // 
            // URLComboBox
            // 
            this.URLComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.URLComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.URLComboBox.AutoSize = false;
            this.URLComboBox.MaxDropDownItems = 10;
            this.URLComboBox.MaxLength = 1000;
            this.URLComboBox.Name = "URLComboBox";
            this.URLComboBox.Size = new System.Drawing.Size(121, 20);
            this.URLComboBox.ToolTipText = "URL ComboBox";
            this.URLComboBox.SelectedIndexChanged += new System.EventHandler(this.URLComboBox_SelectedIndexChanged);
            this.URLComboBox.LocationChanged += new System.EventHandler(this.URLComboBox_LocationChanged);
            this.URLComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.URLComboBox_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.webBrowser);
            this.panel1.Controls.Add(this.groupBox);
            this.panel1.Controls.Add(this.pictureBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(718, 443);
            this.panel1.TabIndex = 6;
            // 
            // StartUpWindow
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(718, 490);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartUpWindow";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void SetRecentFiles()
        {
            int i = 0;
            foreach (KeyValuePair<string, string> project in m_window.RecentProjects)
            {
                i++;
                ProjectLabel label = new ProjectLabel(project.Key, project.Value);
                label.Text = i.ToString() + ". " + project.Key;
                label.Width = 220;
                label.Left = 20;
                label.Top = i * 25;
                label.MouseClick += new MouseEventHandler(label_MouseClick);
                groupBox.Controls.Add(label);
            }
        }
        #endregion

        #region private methods
        private void go()
        {                   
            this.webBrowser.Navigate(this.URLComboBox.Text);    
        }

        private void stop()
        {
            this.webBrowser.Stop();        
        }

        private void forward()
        {
            this.webBrowser.GoForward();
        
        }

        private void back()
        {
            this.webBrowser.GoBack();    
        }

        private void refresh()
        {
            try
            {
                this.webBrowser.Refresh();    
            }
            catch 
            {
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event on menu mouse clisk.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_MouseClick(object sender, MouseEventArgs e)
        {
            ProjectLabel label = (ProjectLabel)sender;
            m_window.LoadProject(label.Project, label.FilePath);
        }

        /// <summary>
        /// Event on ToolButton click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender == ButtonBack)
                back();
            else if (sender == ButtonForward)
                forward();
            else if (sender == ButtonStop)
                stop();
            else if (sender == ButtonNavigate)
                go();
            else if (sender == ButtonRefresh)
                refresh();
        }

        /// <summary>
        /// Event on ComboBox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void URLComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            go();
        }

        /// <summary>
        /// Event on ComboBox input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void URLComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                go();
        }

        /// <summary>
        /// Event on location changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void URLComboBox_LocationChanged(object sender, EventArgs e)
        {
            URLComboBox.Width = toolStrip.Width - 130;
        }

        /// <summary>
        /// Event on size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip_SizeChanged(object sender, EventArgs e)
        {
            URLComboBox.Width = toolStrip.Width - 130;
        }

        /// <summary>
        /// Event on complete refresh page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.URLComboBox.Text = webBrowser.Url.AbsoluteUri;
            if (URLComboBox.Items.Count == 0 || (string)URLComboBox.Items[0] != URLComboBox.Text)
                URLComboBox.Items.Insert(0, URLComboBox.Text);
            if (URLComboBox.Items.Count > URLComboBox.MaxDropDownItems)
                URLComboBox.Items.RemoveAt(URLComboBox.Items.Count - 1);

            this.Text = webBrowser.Url.ToString();
            if (webBrowser.Url == STARTUP)
            {
                webBrowser.Left = 265;
                webBrowser.Width = panel1.Width - 265;
            }
            else
            {
                webBrowser.Left = 0;
                webBrowser.Width = panel1.Width;
            }
            this.m_window.Environment.PluginManager.SetStatusBarMessage(
                Ecell.Plugin.StatusBarMessageKind.Generic,
                ""
                );
        }
        /// <summary>
        /// Event on progress changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (e.MaximumProgress == 0.0) return;
            this.m_window.Environment.PluginManager.SetStatusBarMessage(
                Ecell.Plugin.StatusBarMessageKind.Generic,
                MessageResources.MessageWebBrowse
            );
            int progress = (int)(100 * ((double)e.CurrentProgress / (double)e.MaximumProgress));
            this.m_window.Environment.PluginManager.SetProgressBarValue(progress);
        }
        /// <summary>
        /// Event on status changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_StatusTextChanged(object sender, EventArgs e)
        {
//            this.URLLabel.Text = webBrowser.StatusText;
            this.m_window.Environment.PluginManager.SetStatusBarMessage(
                        Ecell.Plugin.StatusBarMessageKind.Generic,
                        webBrowser.StatusText);
        }
        /// <summary>
        /// Event on GoForward changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            this.ButtonForward.Enabled = webBrowser.CanGoForward;
        }
        /// <summary>
        /// Event on GoBack changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            this.ButtonBack.Enabled = webBrowser.CanGoBack;
        }

        #endregion

        #region Internal class
        /// <summary>
        /// Custom Label.
        /// </summary>
        private class ProjectLabel : Label
        {
            private string m_project;
            private string m_filePath;

            public string Project
            {
                get { return m_project; }
                set { m_project = value; }
            }

            public string FilePath
            {
                get { return m_filePath; }
                set { m_filePath = value; }
            }

            public ProjectLabel(string project, string file)
            {
                m_project = project;
                m_filePath = file;

                this.Text = project;
                this.Font = new Font(Font.SystemFontName, 10);
                this.MouseHover += new EventHandler(label_MouseHover);
                this.MouseLeave += new EventHandler(label_MouseLeave);
            }

            void label_MouseLeave(object sender, EventArgs e)
            {
                Label label = (Label)sender;
                label.Font = new Font(label.Font, FontStyle.Regular);
            }

            void label_MouseHover(object sender, EventArgs e)
            {
                Label label = (Label)sender;
                label.Font = new Font(label.Font, FontStyle.Underline);
            }

        }
        #endregion
    }
}
