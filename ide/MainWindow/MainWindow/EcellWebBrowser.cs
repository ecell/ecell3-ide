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
using Ecell.Action;
using Ecell.Plugin;

namespace Ecell.IDE.MainWindow
{
    /// <summary>
    /// StartUpWindow
    /// </summary>
    public class EcellWebBrowser : EcellDockContent
    {
        #region Fields
        private Uri m_startupPage;
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
        private ToolStripButton ButtonHome;
        private List<KeyValuePair<string, string>> m_recentFiles;
        #endregion

        /// <summary>
        /// get / set ApplicationEnvironment object.
        /// </summary>
        public virtual ApplicationEnvironment Environment
        {
            get { return m_env; }
        }

        public Uri Url
        {
            get { return webBrowser.Url; }
            set
            {
                Activate();
                webBrowser.Navigate(value);
            }
        }

        internal IEnumerable<KeyValuePair<string, string>> RecentFiles
        {
            get { return m_recentFiles; }
        }
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public EcellWebBrowser(ApplicationEnvironment env, List<KeyValuePair<string, string>> recentFiles)
        {
            m_env = env;
            InitializeComponent();
            Util.InitialLanguage();
            webBrowser.ObjectForScripting = new AutomationStub(this);
            m_recentFiles = recentFiles;
            m_startupPage = FindStartPage();
            SetStartPage();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EcellWebBrowser));
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.URLLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.ButtonBack = new System.Windows.Forms.ToolStripButton();
            this.ButtonForward = new System.Windows.Forms.ToolStripButton();
            this.ButtonStop = new System.Windows.Forms.ToolStripButton();
            this.ButtonHome = new System.Windows.Forms.ToolStripButton();
            this.ButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.ButtonNavigate = new System.Windows.Forms.ToolStripButton();
            this.URLComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.AccessibleDescription = null;
            this.webBrowser.AccessibleName = null;
            resources.ApplyResources(this.webBrowser, "webBrowser");
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.CanGoForwardChanged += new System.EventHandler(this.webBrowser_CanGoForwardChanged);
            this.webBrowser.CanGoBackChanged += new System.EventHandler(this.webBrowser_CanGoBackChanged);
            this.webBrowser.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.webBrowser_ProgressChanged);
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.StatusTextChanged += new System.EventHandler(this.webBrowser_StatusTextChanged);
            // 
            // URLLabel
            // 
            this.URLLabel.AccessibleDescription = null;
            this.URLLabel.AccessibleName = null;
            resources.ApplyResources(this.URLLabel, "URLLabel");
            this.URLLabel.BackColor = System.Drawing.SystemColors.Control;
            this.URLLabel.BackgroundImage = null;
            this.URLLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.URLLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.URLLabel.Name = "URLLabel";
            this.URLLabel.Spring = true;
            // 
            // toolStrip
            // 
            this.toolStrip.AccessibleDescription = null;
            this.toolStrip.AccessibleName = null;
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.BackgroundImage = null;
            this.toolStrip.Font = null;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonBack,
            this.ButtonForward,
            this.ButtonStop,
            this.ButtonHome,
            this.ButtonRefresh,
            this.ButtonNavigate,
            this.URLComboBox});
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.SizeChanged += new System.EventHandler(this.toolStrip_SizeChanged);
            // 
            // ButtonBack
            // 
            this.ButtonBack.AccessibleDescription = null;
            this.ButtonBack.AccessibleName = null;
            resources.ApplyResources(this.ButtonBack, "ButtonBack");
            this.ButtonBack.BackgroundImage = null;
            this.ButtonBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonBack.Name = "ButtonBack";
            this.ButtonBack.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonForward
            // 
            this.ButtonForward.AccessibleDescription = null;
            this.ButtonForward.AccessibleName = null;
            resources.ApplyResources(this.ButtonForward, "ButtonForward");
            this.ButtonForward.BackgroundImage = null;
            this.ButtonForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonForward.Name = "ButtonForward";
            this.ButtonForward.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.AccessibleDescription = null;
            this.ButtonStop.AccessibleName = null;
            resources.ApplyResources(this.ButtonStop, "ButtonStop");
            this.ButtonStop.BackgroundImage = null;
            this.ButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonHome
            // 
            this.ButtonHome.AccessibleDescription = null;
            this.ButtonHome.AccessibleName = null;
            resources.ApplyResources(this.ButtonHome, "ButtonHome");
            this.ButtonHome.BackgroundImage = null;
            this.ButtonHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonHome.Name = "ButtonHome";
            this.ButtonHome.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.AccessibleDescription = null;
            this.ButtonRefresh.AccessibleName = null;
            resources.ApplyResources(this.ButtonRefresh, "ButtonRefresh");
            this.ButtonRefresh.BackgroundImage = null;
            this.ButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Click += new System.EventHandler(this.Button_Click);
            // 
            // ButtonNavigate
            // 
            this.ButtonNavigate.AccessibleDescription = null;
            this.ButtonNavigate.AccessibleName = null;
            resources.ApplyResources(this.ButtonNavigate, "ButtonNavigate");
            this.ButtonNavigate.BackgroundImage = null;
            this.ButtonNavigate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonNavigate.Name = "ButtonNavigate";
            this.ButtonNavigate.Click += new System.EventHandler(this.Button_Click);
            // 
            // URLComboBox
            // 
            this.URLComboBox.AccessibleDescription = null;
            this.URLComboBox.AccessibleName = null;
            this.URLComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.URLComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            resources.ApplyResources(this.URLComboBox, "URLComboBox");
            this.URLComboBox.Name = "URLComboBox";
            this.URLComboBox.SelectedIndexChanged += new System.EventHandler(this.URLComboBox_SelectedIndexChanged);
            this.URLComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.URLComboBox_KeyDown);
            this.URLComboBox.LocationChanged += new System.EventHandler(this.URLComboBox_LocationChanged);
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.webBrowser);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // EcellWebBrowser
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BackgroundImage = null;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip);
            this.Name = "EcellWebBrowser";
            this.TabText = this.Text;
            this.ToolTipText = null;
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
                candidates.Add(Path.Combine(documentDir, Constants.fileStartupHTML + "."
                    + lang.TwoLetterISOLanguageName.ToLower() + Constants.FileExtHTML));
                candidates.Add(Path.Combine(documentDir,
                    Constants.fileStartupHTML + Constants.FileExtHTML));
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
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        private void SetStartPage()
        {
            Url = m_startupPage;
        }
        #endregion

        #region Event Handlers
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
            else if (sender == ButtonHome)
                SetStartPage();
        }

        /// <summary>
        /// Event on ComboBox changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void URLComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (URLComboBox.Text != webBrowser.Url.AbsoluteUri)
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
            SetURLComboBoxSize();
        }

        /// <summary>
        /// Event on size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip_SizeChanged(object sender, EventArgs e)
        {
            SetURLComboBoxSize();
        }

        private void SetURLComboBoxSize()
        {
            URLComboBox.Width = toolStrip.Width - 150;
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
            m_env.ReportManager.SetStatus(
                StatusBarMessageKind.Generic,
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
            m_env.ReportManager.SetStatus(
                StatusBarMessageKind.Generic,
                MessageResources.MessageWebBrowse
            );
            int progress = (int)(100 * ((double)e.CurrentProgress / (double)e.MaximumProgress));
            m_env.ReportManager.SetProgress(progress);
        }
        /// <summary>
        /// Event on status changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_StatusTextChanged(object sender, EventArgs e)
        {
//            this.URLLabel.Text = webBrowser.StatusText;
            m_env.ReportManager.SetStatus(
                        StatusBarMessageKind.Generic,
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

        [ComVisible(true)]
        public class MyIEnumerator
        {
            IEnumerator<KeyValuePair<string, string>> m_en;

            public MyIEnumerator(IEnumerator<KeyValuePair<string, string>> en)
            {
                m_en = en;
            }

            public string CurrentKey
            {
                get { return m_en.Current.Key; }
            }

            public string CurrentValue
            {
                get
                {
                    try
                    {
                        return new Uri(m_en.Current.Value).ToString();
                    }
                    catch { }
                    return null;
                }
            }

            public bool MoveNext()
            {
                return m_en.MoveNext();
            }

            public void Reset()
            {
                m_en.Reset();
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
            /// <param name="uri"></param>
            public bool LoadProject(string url)
            {
                try
                {
                    Uri uri = new Uri(url);
                    if (!uri.IsFile)
                        return false;
                    if (SimulationConfirm())
                        return false;
                    if (!SaveConfirm())
                        return false;
                    // Close project.
                    m_browser.Environment.DataManager.CloseProject();
                    // Load project.
                    m_browser.Environment.DataManager.LoadProject(uri.LocalPath);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    Util.ShowErrorDialog(e.Message);
                    return false;
                }
                return true;

            }

            /// <summary>
            /// Save confirm.
            /// </summary>
            /// <returns>
            /// It return true when the current project was closed successfully
            /// and returns false when SaveProject is canceled.
            /// </returns>
            private bool SaveConfirm()
            {
                ActionManager aManager = m_browser.Environment.ActionManager;
                if (aManager.Undoable || aManager.Redoable)
                {
                    try
                    {
                        // Save if answer is yes.
                        if (Util.ShowYesNoCancelDialog(MessageResources.SaveConfirm))
                            m_browser.Environment.DataManager.SaveProject();
                    }
                    catch (Exception)
                    {
                        // Return false when canceled
                        return false;
                    }
                }
                // Return true when the current project was closed successfully.
                return true;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            private bool SimulationConfirm()
            {
                // Check simulation status.
                bool simulation = false;
                try
                {
                    ProjectStatus status = m_browser.Environment.PluginManager.Status;
                    simulation = status == ProjectStatus.Running || status == ProjectStatus.Stepping || status == ProjectStatus.Suspended; 
                }
                catch (Exception)
                {
                    simulation = false;
                }

                // Show Alert dialog.
                if (simulation)
                {
                    Util.ShowNoticeDialog(MessageResources.ErrLoadWhileSimulation);
                }
                return simulation;
            }

            /// <summary>
            /// Create new project
            /// </summary>
            public void CreateNewProject()
            {
                m_browser.Environment.DataManager.CreateNewProject(
                    "NewProject",
                    "");
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public MyIEnumerator GetRecentFiles()
            {
                return new MyIEnumerator(m_browser.RecentFiles.GetEnumerator());
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetLabelForRecentProjects()
            {
                return MessageResources.MenuRecentProjects;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetLabelForTutorial()
            {
                return MessageResources.MenuTutorial;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetLabelForManual()
            {
                return MessageResources.MenuManual;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetLabelForDockManual()
            {
                return MessageResources.MenuDockManual;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetManual()
            {
                return MessageResources.FileManual;
            }
        }
    }
}
