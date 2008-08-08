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
using System.Globalization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// StartUpWindow
    /// </summary>
    public class EcellWebBrowser : EcellDockContent
    {
        #region Fields
        private const string URL = "http://chaperone.e-cell.org/trac/ecell-ide";
        private static Uri STARTUP;

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

        private ApplicationEnvironment m_env;
        private List<KeyValuePair<string, string>> m_recentFiles;
        #endregion

        /// <summary>
        /// get / set ApplicationEnvironment object.
        /// </summary>
        public virtual ApplicationEnvironment Environment
        {
            get { return m_env; }
        }

        public List<KeyValuePair<string, string>> RecentFiles
        {
            get { return m_recentFiles; }
        }

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EcellWebBrowser(List<KeyValuePair<string, string>> recentFiles)
        {
            m_env = ApplicationEnvironment.GetInstance();
            m_recentFiles = recentFiles;
            InitializeComponent();
            this.Text = MessageResources.StartUpWindow;
            this.TabText = this.Text;
            this.webBrowser.ObjectForScripting = new AutomationStub(this);
            Uri startPage = FindStartPage();
            STARTUP = startPage;
            if (startPage != null)
            {
                webBrowser.Navigate(startPage);
            }
            SetRecentFiles(recentFiles);
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
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            // 
            // groupBox
            // 
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // webBrowser
            // 
            resources.ApplyResources(this.webBrowser, "webBrowser");
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.StatusTextChanged += new System.EventHandler(this.webBrowser_StatusTextChanged);
            this.webBrowser.CanGoForwardChanged += new System.EventHandler(this.webBrowser_CanGoForwardChanged);
            this.webBrowser.CanGoBackChanged += new System.EventHandler(this.webBrowser_CanGoBackChanged);
            this.webBrowser.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.webBrowser_ProgressChanged);
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // URLLabel
            // 
            this.URLLabel.BackColor = System.Drawing.SystemColors.Control;
            this.URLLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.URLLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.URLLabel.Name = "URLLabel";
            resources.ApplyResources(this.URLLabel, "URLLabel");
            this.URLLabel.Spring = true;
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
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.SizeChanged += new System.EventHandler(this.toolStrip_SizeChanged);
            // 
            // ButtonBack
            // 
            this.ButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ButtonBack, "ButtonBack");
            this.ButtonBack.Name = "ButtonBack";
            this.ButtonBack.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonForward
            // 
            this.ButtonForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ButtonForward, "ButtonForward");
            this.ButtonForward.Name = "ButtonForward";
            this.ButtonForward.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ButtonStop, "ButtonStop");
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ButtonRefresh, "ButtonRefresh");
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonNavigate
            // 
            this.ButtonNavigate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ButtonNavigate, "ButtonNavigate");
            this.ButtonNavigate.Name = "ButtonNavigate";
            this.ButtonNavigate.Click += new System.EventHandler(this.Button_Click);
            // 
            // URLComboBox
            // 
            this.URLComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.URLComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            resources.ApplyResources(this.URLComboBox, "URLComboBox");
            this.URLComboBox.Name = "URLComboBox";
            this.URLComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.URLComboBox_KeyDown);
            this.URLComboBox.LocationChanged += new System.EventHandler(this.URLComboBox_LocationChanged);
            this.URLComboBox.SelectedIndexChanged += new System.EventHandler(this.URLComboBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.webBrowser);
            this.panel1.Controls.Add(this.groupBox);
            this.panel1.Controls.Add(this.pictureBox);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // EcellWebBrowser
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip);
            this.Name = "EcellWebBrowser";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Uri FindStartPage()
        {
            List<string> candidates = new List<string>();

            string documentDir = Util.GetWindowSettingDir();
            CultureInfo lang = Util.GetLanguage();
            if (documentDir != null)
            {
                candidates.Add(Path.Combine(documentDir,
                    Constants.fileStartupHTML));
                candidates.Add(Path.Combine(documentDir,
                    lang.TwoLetterISOLanguageName + "_" + Constants.fileStartupHTML));
            }
            foreach (string candidate in candidates)
            {
                Trace.WriteLine("Checking if " + candidate + " exists");
                if (File.Exists(candidate))
                {
                    UriBuilder ub = new UriBuilder("file", null);
                    ub.Path = candidate;
                    return ub.Uri;
                }
            }
            return null;
        }

        private void SetRecentFiles(List<KeyValuePair<string, string>> recentDics)
        {
                int i = 0;
                foreach (KeyValuePair<string, string> project in recentDics)
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

        //private void SetRecentFiles()
        //{
        //    string recentFiles = "<div id =\"recentProject\">\n<h2>最近使ったファイル</h2>\n";
        //    string temp;
        //    int i = 0;
        //    foreach (KeyValuePair<string, string> project in m_window.RecentProjects)
        //    {
        //        temp = "<li><a onclick=\"window.external.LoadProject('" + project.Key + "','" + project.Value.Replace("\\","/") + "');\">" + project.Key + "</a></li>\n";
        //        recentFiles += temp;
        //        i++;
        //        ProjectLabel label = new ProjectLabel(project.Key, project.Value);
        //        label.Text = i.ToString() + ". " + project.Key;
        //        label.Width = 220;
        //        label.Left = 20;
        //        label.Top = i * 25;
        //        label.MouseClick += new MouseEventHandler(label_MouseClick);
        //        groupBox.Controls.Add(label);
        //    }
        //    recentFiles += "</div>";
        //    //webBrowser.DocumentText = webBrowser.DocumentText.Replace("<div id =\"recentProject\"></div>",recentFiles);
        //}
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
            try
            {
                m_env.DataManager.LoadProject(label.FilePath);
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
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
            m_env.PluginManager.SetStatusBarMessage(
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
            m_env.PluginManager.SetStatusBarMessage(
                Ecell.Plugin.StatusBarMessageKind.Generic,
                MessageResources.MessageWebBrowse
            );
            int progress = (int)(100 * ((double)e.CurrentProgress / (double)e.MaximumProgress));
            m_env.PluginManager.SetProgressBarValue(progress);
        }
        /// <summary>
        /// Event on status changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_StatusTextChanged(object sender, EventArgs e)
        {
//            this.URLLabel.Text = webBrowser.StatusText;
            m_env.PluginManager.SetStatusBarMessage(
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

        /// <summary>
        /// AutomationClass
        /// </summary>
        [ComVisible(true)]
        public class AutomationStub
        {
            private EcellWebBrowser m_browser;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="browser"></param>
            public AutomationStub(EcellWebBrowser browser)
            {
                this.m_browser = browser;
            }
            /// <summary>
            /// LoadProject
            /// </summary>
            /// <param name="filename"></param>
            public void LoadProject(string filename)
            {
                try
                {
                    m_browser.Environment.DataManager.LoadProject(filename);
                }
                catch (Exception e)
                {
                    Util.ShowErrorDialog(e.Message);
                }

            }

            /// <summary>
            /// Create new project
            /// </summary>
            public void CreateNewProject()
            {
                m_browser.Environment.DataManager.CreateNewProject(
                    "NewProject",
                    "NewProject",
                    "",
                    new List<string>());
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetRecentFiles()
            {
                string recentFiles = "";
                string temp;
                int i = 0;
                foreach (KeyValuePair<string, string> project in m_browser.RecentFiles)
                {
                    temp = "<li><a href=\"#\" onclick=\"window.external.LoadProject('" + project.Value.Replace("\\", "/") + "');\">" + project.Key + "</a></li>\n";
                    recentFiles += temp;
                }
                return recentFiles;
            }
        }
        #endregion
    }
}
