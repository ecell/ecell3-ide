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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;
using Ecell.Plugin;
using Ecell.Objects;
using Ecell.Logger;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Plugin Class of TracerWindow.
    /// </summary>
    public class TracerWindow : PluginBase, IRasterizable
    {
        #region Fields
        /// <summary>
        /// The delegate function for changins system status.
        /// </summary>
        /// <param name="r">system status.</param>
        delegate void ChangeStatusCallBack(int r);
        /// <summary>
        /// The menu item for [Show] -> [Show TraceWindow].
        /// </summary>
        ToolStripMenuItem m_showWin;
        /// <summary>
        /// The menu item for [Show] -> [Show PlotWindow].
        /// </summary>
        ToolStripMenuItem m_plotWin;
        /// <summary>
        /// The menu item for [Show] -> [Show Save Trace].
        /// </summary>
        ToolStripMenuItem m_showSaveWin;
        /// <summary>
        /// The current TracerWindow.
        /// </summary>
        private TraceWindow m_win = null;
        /// <summary>
        /// Data format
        /// </summary>
        private string m_dataformat = "e4";
        /// <summary>
        /// The list of TracerWindow.
        /// </summary>
        private List<TraceWindow> m_winList = new List<TraceWindow>();
        /// <summary>
        /// The list of TracerWindow.
        /// </summary>
        private List<ToolStripMenuItem> m_menuList;
        /// <summary>
        /// The current system status.
        /// </summary>
        ProjectStatus m_type;
        /// <summary>
        /// The list of logger with TracerWindow.
        /// </summary>
        private Dictionary<TagData, List<TraceWindow>> m_entry;
        /// <summary>
        /// The list of object with logger.
        /// </summary>
        private Dictionary<String, TagData> m_tagList;
        /// <summary>
        /// The flag whether simulation runs.
        /// </summary>
        bool isSuspend = false;
        /// <summary>
        /// Timer for executing redraw event at each 1 minutes.
        /// </summary>
        System.Windows.Forms.Timer m_time;
        /// <summary>
        /// The last time on drawing tracer.
        /// </summary>
        double m_current;
        /// <summary>
        /// The max time displayed in TracerWindow.
        /// </summary>
        double m_currentMax;
        /// <summary>
        /// The step time on drawing tracer.
        /// </summary>
        double m_step;
        /// <summary>
        /// The drawing points on drawing area.
        /// </summary>
        static public double s_count = 10000.0;
        /// <summary>
        /// The drawing area for duplicate.
        /// </summary>
        static public double s_duple = 1.25;
        /// <summary>
        /// The time interval to redraw.
        /// </summary>
        int m_timespan = 100;
        /// <summary>
        /// The flag whether simulation is stepping.
        /// </summary>
        bool isStep = false;
        /// <summary>
        /// The number of TraceWindow.
        /// </summary>
        int m_winCount = 1;
        /// <summary>
        /// Logger window.
        /// </summary>
        private LoggerWindow m_loggerWin;
        /// <summary>
        /// Setting object for Y2 axis.
        /// </summary>
        private YAxisSettings m_setting = new YAxisSettings();
        /// <summary>
        /// The list of continuous log entry.
        /// </summary>
        private List<TagData> m_isContiuousList = new List<TagData>();
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public TracerWindow()
        {
            m_entry = new Dictionary<TagData, List<TraceWindow>>();
            m_tagList = new Dictionary<string, TagData>();
            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(TimerFire);

            m_loggerWin = new LoggerWindow(this);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the number of plots.
        /// </summary>
        public int PlotNumber
        {
            get { return (int)TracerWindow.s_count; }
            set { TracerWindow.s_count = (double)value; }
        }

        /// <summary>
        /// get / set redraw interval.
        /// </summary>
        public double RedrawInterval
        {
            get { return m_timespan / 1000.0; }
            set { this.m_timespan = (int)(value * 1000.0); }
        }

        /// <summary>
        /// get / set the setting of y axis.
        /// </summary>
        public YAxisSettings Settings
        {
            get { return this.m_setting; }
            set { 
                this.m_setting = value;
                foreach (TraceWindow t in m_winList)
                    t.SetDefaultSetting(this.m_setting);
            }
        }

        /// <summary>
        /// get/set the current TraceWindow.
        /// </summary>
        public TraceWindow CurrentWin
        {
            get { return this.m_win; }
            set { this.m_win = value; }
        }
        #endregion

        #region Initializer
        /// <summary>
        /// Initialize the components.
        /// </summary>
        public override void  Initialize()
        {
            m_currentMax = 1.0;
            m_winCount = 1;

            m_menuList = new List<ToolStripMenuItem>();

            m_showWin = new GraphToolStripMenuItem();
            m_showWin.Text = MessageResources.MenuItemShowTraceText;
            m_showWin.Name = "MenuItemShowTrace";
            m_showWin.Size = new Size(96, 22);
            m_showWin.Enabled = false;
            m_showWin.Click += new EventHandler(this.ShowTracerWindow);

            m_plotWin = new ToolStripMenuItem();
            //m_plotWin.Text = MessageResources.MenuItemShowPlotText;
            //m_plotWin.Name = "MenuItemShowPlot";
            //m_plotWin.Size = new Size(96, 22);
            //m_plotWin.Enabled = false;
            //m_plotWin.Click += new EventHandler(this.ShowPlotWindow);

            ToolStripMenuItem view = new ToolStripMenuItem();
            view.DropDownItems.AddRange(new ToolStripItem[] {
                m_showWin
            });
            view.Name = "MenuItemView";
            view.Size = new Size(36, 20);
            view.Text = "View";

            //m_setupWin = new ToolStripMenuItem();
            //m_setupWin.Name = "MenuItemShowTraceSetup";
            //m_setupWin.Size = new Size(96, 22);
            //m_setupWin.Text = MessageResources.MenuItemShowTraceSetupText;
            //m_setupWin.Enabled = true;
            //m_setupWin.Click += new EventHandler(this.ShowSetupTracerWindow);

            //ToolStripMenuItem setup = new ToolStripMenuItem();
            //setup.DropDownItems.AddRange(new ToolStripItem[] {
            //    m_setupWin
            //});
            //setup.Name = "MenuItemSetup";
            //setup.Size = new Size(36, 20);
            //setup.Text = "Setup";

            m_showSaveWin = new ToolStripMenuItem();
            m_showSaveWin.Text = MessageResources.MenuItemShowSaveTraceText;
            m_showSaveWin.Name = "MenuItemShowSaveTrace";
            m_showSaveWin.Size = new Size(96, 22);
            m_showSaveWin.Enabled = false;
            m_showSaveWin.Tag = 36;
            m_showSaveWin.Click += new EventHandler(this.ShowSaveTracerWindow);
            ToolStripMenuItem filem = new ToolStripMenuItem();
            filem.DropDownItems.AddRange(new ToolStripItem[] {
                m_showSaveWin
            });
            filem.Name = "MenuItemFile";
            filem.Size = new Size(36, 20);
            filem.Text = "File";

            m_menuList.Add(view);
//            m_menuList.Add(setup);
            m_menuList.Add(filem);

            m_env.LoggerManager.LoggerAddEvent += new LoggerAddEventHandler(LoggerManager_LoggerAddEvent);
            m_env.LoggerManager.LoggerChangedEvent += new LoggerChangedEventHandler(LoggerManager_LoggerChangedEvent);
            m_env.LoggerManager.LoggerDeleteEvent += new LoggerDeleteEventHandler(LoggerManager_LoggerDeleteEvent);

            m_env.DataManager.DisplayFormatEvent += new DisplayFormatChangedEventHandler(DataManager_DisplayFormatEvent);
        }
        #endregion


        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for TracerWindow.
        /// </summary>
        /// <returns>MenuStripItems</returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            return m_menuList;
        }

        /// <summary>
        /// Get windows for TracerWindow.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return new EcellDockContent[] { m_loggerWin };
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            foreach (TraceWindow win in m_winList)
            {
                win.Clear();
            }
            m_loggerWin.Clear();
            m_tagList.Clear();
            m_entry.Clear();
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Uninitialized)
            {
                isStep = false;
                m_showWin.Enabled = false;
                m_plotWin.Enabled = false;
//                m_setupWin.Enabled = true;
                m_showSaveWin.Enabled = false;
            }
            else if (type == ProjectStatus.Loaded ||
                type == ProjectStatus.Loading)
            {
                isStep = false;
                m_showWin.Enabled = true;
//                m_setupWin.Enabled = true;
            }
            else if (type == ProjectStatus.Running)
            {
                m_currentMax = 1.0;
                isStep = false;
                m_showWin.Enabled = true;
                m_plotWin.Enabled = true;
//                m_setupWin.Enabled = false; 
                m_showSaveWin.Enabled = true;
            }
            else if (type == ProjectStatus.Stepping)
            {
                if (isStep == false && m_type != ProjectStatus.Suspended)
                {
                    m_current = 0.0;
                    m_currentMax = 1.0;
                    m_step = m_currentMax / (double)TracerWindow.s_count;
                    foreach (TraceWindow t in m_winList)
                    {
                        t.ClearTime();
                        t.m_current = 0.0;
                    }
                }
                isStep = true;
                m_showWin.Enabled = true;
                m_plotWin.Enabled = true;
//                m_setupWin.Enabled = false;
                m_showSaveWin.Enabled = true;
            }
            else
            {
                isStep = false;
                m_showWin.Enabled = true;
//                m_setupWin.Enabled = true;
                m_showSaveWin.Enabled = true;
            }

            if (type == ProjectStatus.Running || type == ProjectStatus.Stepping)
            {
                this.StartSimulation();
            }
            else if ((m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping) &&
                type == ProjectStatus.Loaded)
            {
                UpdateGraphDelegate(); // vomit the remainder log.
                this.StopSimulation();
            }
            else if (type == ProjectStatus.Suspended)
            {
                UpdateGraphDelegate(); // vomit the remainder log.
                this.SuspendSimulation();
            }
            else if (type == ProjectStatus.Stepping)
            {
                if (m_type == ProjectStatus.Stepping)
                {
                    UpdateGraphDelegate(); // vomit the remainder log.
                    this.SuspendSimulation();
                    type = ProjectStatus.Suspended;
                }
                else
                {
                    //                    this.StartSimulation();
                }
            }

            m_type = type;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print(string name)
        {
            foreach (TraceWindow t in m_winList)
            {
                if (name.Equals(t.Text))
                {
                    t.Print();
                    return t.GetBitmap();
                }
            }
            return null;
        }

        /// <summary>
        /// Get the property setting of common setting dialog.
        /// </summary>
        /// <returns>the list of property setting.</returns>
        public override List<IPropertyItem> GetPropertySettings()
        {
            PropertyNode node = new PropertyNode(MessageResources.NameGraphSetting);
            node.Nodes.Add(new PropertyNode(new TracerConfigurationPage(this, this.PlotNumber, this.RedrawInterval, m_setting.Copy())));

            List<IPropertyItem> nodeList = new List<IPropertyItem>();
            nodeList.Add(node);

            return nodeList;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"TracerWindow"</returns>
        public override string GetPluginName()
        {
            return "TracerWindow";
        }

        /// <summary>
        /// Get the public deledate function.
        /// </summary>
        /// <returns>the dictionary of name and delefate function.</returns>
        public override Dictionary<string, Delegate> GetPublicDelegate()
        {
            Dictionary<string, Delegate> list = new Dictionary<string, Delegate>();
            list.Add("ShowGraphWithLog", new ShowGraphDelegate(this.ShowTraceWindow));
            return list;
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public IEnumerable<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            foreach (TraceWindow t in m_winList)
            {
                if (!t.IsHidden)
                    names.Add(t.Text);
            }
            return names;
        }

        /// <summary>
        /// Get whether this plugin is enable to print directly.
        /// </summary>
        /// <returns></returns>
        public bool IsDirect()
        {
            return true;
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Get the current simulation time, and calculate the step using that time.
        /// This function get the plot data from DataManager with start, end and step time.
        /// </summary>
        void UpdateGraphDelegate()
        {
            List<LogData> list;
            double nextTime = m_dManager.GetCurrentSimulationTime();

            if (m_winList.Count == 0) return;
            if (nextTime > m_currentMax)
            {
                if (nextTime > m_currentMax * TracerWindow.s_duple)
                {
                    foreach (TraceWindow t in m_winList)
                    {
                        t.ClearTime();
                        t.m_current = 0.0;
                    }
                    m_currentMax = nextTime * TracerWindow.s_duple;
                    m_step = m_currentMax / TracerWindow.s_count;
                    list = m_dManager.GetLogData(0.0, nextTime, m_step);
                }
                else
                {
                    m_currentMax = m_currentMax * TracerWindow.s_duple;
                    m_step = m_currentMax / TracerWindow.s_count;
                    list = m_dManager.GetLogData(m_current, nextTime, m_step);
                    // StartとEndの間以外のデータがエンジンから返ってきており、
                    // 無視するとグラフが歯抜けになってしまうので、
                    // グラフに追加するように変更した。

                    //if (list != null)
                    //{
                    //    foreach (LogData d in list)
                    //    {
                    //        List<LogValue> delList = new List<LogValue>();
                    //        foreach (LogValue v in d.logValueList)
                    //        {
                    //            if (v.time < m_current) delList.Add(v);
                    //        }
                    //        foreach (LogValue v in delList)
                    //        {
                    //            d.logValueList.Remove(v);
                    //        }
                    //    }
                    //}
                }
            }
            else if (m_current + m_step < nextTime)
            {
                list = m_dManager.GetLogData(m_current, nextTime, m_step);
                // StartとEndの間以外のデータがエンジンから返ってきており、
                // 無視するとグラフが歯抜けになってしまうので、
                // グラフに追加するように変更した。

                //if (list != null)
                //{
                //    foreach (LogData d in list)
                //    {
                //        List<LogValue> delList = new List<LogValue>();
                //        foreach (LogValue v in d.logValueList)
                //        {
                //            if (v.time < m_current) delList.Add(v);
                //        }
                //        foreach (LogValue v in delList)
                //        {
                //            d.logValueList.Remove(v);
                //        }
                //    }
                //}
            }
            else
            {
                return;
            }

            m_current = nextTime + m_step / 10.0;
            if (list == null) return;
            foreach (TraceWindow t in m_winList)
            {
                if (t.IsDisposed) continue;
                    t.AddPoints(m_currentMax, nextTime, list, false);
            }
            list.Clear();
            list = null;
        }

        /// <summary>
        /// Invoke method to add the data to DataGridView.
        /// </summary>
        /// <param name="entry">the log entry</param>
        /// <param name="tag">tag data</param>
        void AddToEntry(LoggerEntry entry, TagData tag)
        {
            TagData tmp = tag;
            if (!m_tagList.ContainsKey(tag.ToString()))
            {
                m_entry.Add(tag, new List<TraceWindow>());
                m_tagList.Add(tag.ToString(), tag);
            }
            else
            {
                tmp = m_tagList[tag.ToString()];
            }

            if (m_win == null)
            {
                ShowTracerWindow(this, new EventArgs());
            }
            if (m_entry[tmp].Contains(m_win)) return;
            m_entry[tmp].Add(m_win);
            m_win.AddLoggerEntry(entry, tag);
        }


        /// <summary>
        /// Invoke method to remove the row from DataGridView.
        /// </summary>
        /// <param name="tag">delete row</param>
        void RemoveFromEntry(TagData tag)
        {
            if (m_tagList.ContainsKey(tag.ToString()))
            {
                TagData t = m_tagList[tag.ToString()];
                m_entry.Remove(t);
                m_tagList.Remove(tag.ToString());
            }
            if (m_win == null) return;
            foreach (TraceWindow t in m_winList)
            {
                t.RemoveLoggerEntry(tag);
            }
        }


        /// <summary>
        /// Call this function, when simulation start.
        /// Run the timer for firing redraw event.
        /// </summary>
        public void StartSimulation()
        {
            if (!isSuspend)
            {
                m_current = 0.0;
                m_isContiuousList.Clear();

                Dictionary<TagData, bool> tagDic = new Dictionary<TagData, bool>();
                foreach (TagData t in m_tagList.Values)
                {
                    try
                    {
                        EcellValue v = IsContinuous(t);
                        if (v == null)
                            continue;
                        bool isCont = false;
                        if ((int)v.Value == 1)
                            isCont = true;

                        foreach (TraceWindow win in m_winList)
                        {
                            win.SetIsContinuous(t.ToShortString(), isCont);
                        }
                    }
                    catch (Exception)
                    {
                        // まだ準備ができていなデータをチェックしていた。
                        m_isContiuousList.Add(t);
                        continue;
                    }
                }
            }
            if (m_timespan <= 0) m_timespan = 1000;
            m_time.Interval = m_timespan;

            foreach (TraceWindow w in m_winList) w.StartSimulation();
            if (m_winList.Count > 0)
                m_currentMax = m_winList[0].m_zCnt.GraphPane.XAxis.Scale.Max;

            isSuspend = false;
            m_time.Enabled = true;
            m_time.Start();
        }

        /// <summary>
        /// Get whether this entry is continuous.
        /// </summary>
        /// <param name="t">the log entry.</param>
        /// <returns>the flag whether this entry is continuous</returns>
        private EcellValue IsContinuous(TagData t)
        {
            string FullPN = t.Type + Constants.delimiterColon + t.M_key +
                Constants.delimiterColon + EcellProcess.ISCONTINUOUS;
            EcellValue v = m_dManager.GetEntityProperty(FullPN);
            return v;
        }

        /// <summary>
        /// Call this function, when simulation stop.
        /// </summary>
        public void StopSimulation()
        {
            m_time.Enabled = false;
            m_time.Stop();
            m_current = 0.0;

            foreach (TraceWindow w in m_winList) w.StopSimulation();

            isSuspend = false;
        }

        /// <summary>
        /// Call this function, when simulation suspend.
        /// </summary>
        public void SuspendSimulation()
        {
            m_time.Enabled = false;
            m_time.Stop();

            foreach (TraceWindow w in m_winList) w.SuspendSimulation();

            isSuspend = true;
        }

        /// <summary>
        /// Get the window list to display the logger entry.
        /// </summary>
        /// <param name="entry">the logger entry.</param>
        /// <returns>the window list.</returns>
        public Dictionary<string, bool> GetDisplayWindows(LoggerEntry entry)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (TraceWindow w in m_winList)
            {
                string name = w.TabText;
                bool isDisplay = w.IsDisplay(entry);
                result.Add(name, isDisplay);
            }
            return result;
        }

        /// <summary>
        /// Show TraceWindow to show the loaded log.
        /// </summary>
        /// <param name="filename">the log file.</param>
        /// <param name="isNewWin">the flag whether new window is shown.</param>
        private void ShowTraceWindow(string filename, bool isNewWin)
        {
            if (isNewWin == true || m_win == null)
                m_showWin.PerformClick();

            m_loggerWin.ImportLog(filename, m_win);
        }

        /// <summary>
        /// Change the status of display.
        /// </summary>
        /// <param name="entry">the logger entry.</param>
        /// <param name="name">the window name.</param>
        /// <param name="isDisplay">the flag whether the log entry is shown.</param>
        public void ChangeDisplayStatus(LoggerEntry entry, string name, bool isDisplay)
        {
            foreach (TraceWindow w in m_winList)
            {
                if (w.TabText.Equals(name))
                {
                    w.ChangedDisplayStatus(entry, isDisplay);
                }
            }
        }

        /// <summary>
        /// Execute tracer widonw with threading.
        /// </summary>
        public void TraceWindowAppStart()
        {
            Util.InitialLanguage();
            Application.Run(m_win);
        }
        #endregion

        #region Event
        /// <summary>
        /// Event when Display format is changed.
        /// </summary>
        /// <param name="o">DataManager.</param>
        /// <param name="e">DisplayFormatEventArgs</param>
        private void DataManager_DisplayFormatEvent(object o, Ecell.Events.DisplayFormatEventArgs e)
        {
            m_dataformat = e.DisplayFormat;
            foreach (TraceWindow m in m_winList)
                m.DataFormat = m_dataformat;
        }

        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void TimerFire(object sender, EventArgs e)
        {
            m_time.Enabled = false;
            List<TagData> removeList = new List<TagData>();
            foreach (TagData tag in m_isContiuousList)
            {
                try
                {
                    EcellValue v = IsContinuous(tag);
                    if (v == null)
                        continue;
                    bool isCont = false;
                    if (v.IsInt && (int)v.Value == 1)
                        isCont = true;

                    foreach (TraceWindow win in m_winList)
                    {
                        win.SetIsContinuous(tag.ToShortString(), isCont);
                    }
                    removeList.Add(tag);
                }
                catch (Exception)
                {
                }
            }
            foreach (TagData tag in removeList)
            {
                m_isContiuousList.Remove(tag);
            }
            UpdateGraphDelegate();
            m_time.Enabled = true;
        }


        /// <summary>
        /// Show the dialog to save the trace.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        void ShowSaveTracerWindow(Object sender, EventArgs e)
        {
            try
            {
                SaveTraceDialog win = new SaveTraceDialog(this);                
                win.AddEntry(m_entry);
                using (win)
                {
                    if (win.ShowDialog() == DialogResult.OK)
                    {
                        if (win.SaveList.Count <= 0) return;
                        if (String.IsNullOrEmpty(DataManager.CurrentProject.Info.ProjectPath))
                        {
                            Util.ShowWarningDialog(MessageResources.ErrProjectUnsaved);
                            return;
                        }
                        m_env.DataManager.SaveSimulationResult(win.DirectoryName,
                            win.Start, win.End, win.FileType, win.SaveList);
                        SaveSimulationResultDelegate dlg = 
                            m_env.PluginManager.GetDelegate(Constants.delegateSaveSimulationResult) as SaveSimulationResultDelegate;
                        if (dlg != null)
                            dlg(win.SaveList);
                        Util.ShowNoticeDialog(MessageResources.FinishSave);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// Show the plot window.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        void ShowPlotWindow(Object sender, EventArgs e)
        {
            PlotterWindow win = new PlotterWindow(this);
            win.Show();
        }

        /// <summary>
        /// Event when the logger entry is deleted.
        /// </summary>
        /// <param name="o">LoggerManager.</param>
        /// <param name="e">LoggerEventArgs</param>
        public void LoggerManager_LoggerDeleteEvent(object o, LoggerEventArgs e)
        {
            if (m_loggerWin != null)
                m_loggerWin.LoggerDeleted(e.Entry);

            LoggerEntry entry = e.Entry;
            TagData tag = new TagData(entry.ModelID, entry.ID, entry.Type, entry.FullPN, true);
            tag.isLoaded = e.Entry.IsLoaded;
            tag.FileName = e.Entry.FileName;
            RemoveFromEntry(tag);
        }

        /// <summary>
        /// Event when the logger entry is changed.
        /// </summary>
        /// <param name="o">LoggerManager.</param>
        /// <param name="e">LoggerEventArgs</param>
        private void LoggerManager_LoggerChangedEvent(object o, LoggerEventArgs e)
        {
            if (m_loggerWin != null)
                m_loggerWin.LoggerChanged(e.OriginalFullPN, e.Entry);

            foreach (TraceWindow w in m_winList)
                w.LoggerChanged(e.OriginalFullPN, e.Entry);
        }

        /// <summary>
        /// Event when the logger entry is added.
        /// </summary>
        /// <param name="o">LoggerManager.</param>
        /// <param name="e">LoggerEventArgs</param>
        public void LoggerManager_LoggerAddEvent(object o, LoggerEventArgs e)
        {
            LoggerEntry entry = e.Entry;
            if (m_loggerWin != null)
                m_loggerWin.LoggerAdd(entry);

            EcellObject obj = m_dManager.GetEcellObject(entry.ModelID, entry.ID, entry.Type);
            bool isContinue = true;
            if (obj != null)
            {
                foreach (EcellData d in obj.Value)
                {
                    if (d.Name.Equals(EcellProcess.ISCONTINUOUS))
                    {
                        isContinue = (int)d.Value != 0;
                        break;
                    }
                }
            }
            TagData tag = new TagData(entry.ModelID, entry.ID, entry.Type, entry.FullPN, isContinue);
            tag.isLoaded = e.Entry.IsLoaded;
            tag.FileName = e.Entry.FileName;
            AddToEntry(entry, tag);
        }


        /// <summary>
        /// Show tracer window with thread.
        /// Tracer window works busy, then this function is threading.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void ShowTracerWindow(Object sender, EventArgs e)
        {
            m_win = new TraceWindow(this);
            m_win.Disposed += new EventHandler(FormDisposed);
            m_win.Text = MessageResources.TracerWindow + m_winCount;
            m_win.Name = MessageResources.TracerWindow + m_winCount;
            m_win.TabText = m_win.Text;
            m_win.DataFormat = m_dataformat;            
            m_winCount++;

            // Set Dock settings
            DockPanel panel = m_pManager.DockPanel;
            m_win.DockHandler.DockPanel = panel;
            m_win.DockHandler.FloatPane = panel.DockPaneFactory.CreateDockPane(m_win, DockState.Float, true);
            FloatWindow fw = panel.FloatWindowFactory.CreateFloatWindow(
                                panel,
                                m_win.FloatPane,
                                new Rectangle(m_win.Left, m_win.Top, m_win.Width, m_win.Height));
            m_win.Pane.DockTo(fw);
            SetDockContentDelegate dlg = 
                m_env.PluginManager.GetDelegate(Constants.delegateSetDockContents) as SetDockContentDelegate;
            if (dlg != null)
                dlg(m_win);

            m_winList.Add(m_win);
            m_win.Show();
            m_win.SetDefaultSetting(m_setting);
        }

        /// <summary>
        /// The action of disposing tracer window.
        /// </summary>
        /// <param name="sender">Tracer Window.</param>
        /// <param name="e">EventArgs.</param>
        void FormDisposed(object sender, EventArgs e)
        {
            TraceWindow tmp = sender as TraceWindow;

            if (tmp == null) return;
            m_winList.Remove(tmp);
            if (m_win == tmp) m_win = null;
            if (m_win == null && m_winList.Count > 0)
            {
                m_win = m_winList[m_winList.Count - 1];
            }
        }
        #endregion
    }

    /// <summary>
    /// Class of tag data to trace entry.
    /// </summary>
    public class TagData
    {
        #region Fields
        /// <summary>
        /// The model ID of tag data.
        /// </summary>
        private string m_modelID;
        /// <summary>
        /// The key of tag data. 
        /// </summary>
        private string m_key;
        /// <summary>
        /// The type of tag data.
        /// </summary>
        private string m_type;
        /// <summary>
        /// Path of tag data.
        /// </summary>
        private string m_path;
        /// <summary>
        /// The loaded file name.
        /// </summary>
        private string m_fileName = null;
        /// <summary>
        /// The flag whether this log is loaded.
        /// </summary>
        private bool m_isLoaded = false;
        /// <summary>
        /// The flag whether this log is continuous.
        /// </summary>
        private bool m_isContinue = false;
        #endregion

        /// <summary>
        /// get / set model ID of tag data.
        /// </summary>
        public string M_modelID
        {
            get { return this.m_modelID; }
            set { this.m_modelID = value; }

        }

        /// <summary>
        /// get / set key of tag data.
        /// </summary>
        public string M_key
        {
            get { return this.m_key; }
            set { this.m_key = value; }
        }

        /// <summary>
        /// get / set type of tag data.
        /// </summary>
        public string Type
        {
            get { return this.m_type; }
            set { this.m_type = value; }
        }

        /// <summary>
        /// get / set path of tag data.
        /// </summary>
        public string M_path
        {
            get { return this.m_path; }
            set { this.m_path = value; }
        }

        /// <summary>
        /// get / set whether this log is continue.
        /// </summary>
        public bool IsContinue
        {
            get { return this.m_isContinue; }
            set { this.m_isContinue = value; }
        }

        /// <summary>
        /// get / set whether this log is loaded.
        /// </summary>
        public bool isLoaded
        {
            get { return this.m_isLoaded; }
            set { this.m_isLoaded = value; }
        }

        /// <summary>
        /// get / set the loaded file name.
        /// </summary>
        public string FileName
        {
            get { return this.m_fileName; }
            set { this.m_fileName = value; }
        }

        /// <summary>
        /// Constructor of tag data.
        /// </summary>
        public TagData()
        {
            this.m_modelID = "";
            this.m_key = "";
            this.m_type = "";
            this.m_path = "";
        }

        /// <summary>
        /// Constructor of tag data with initial parameter.
        /// </summary>
        /// <param name="l_modelID">initial model ID of object.</param>
        /// <param name="l_key">initial id of object.</param>
        /// <param name="l_type">initial type of object.</param>
        /// <param name="l_path">initial path of object.</param>
        /// <param name="l_isContinue">initial flag whether this log is continue.</param>
        public TagData(string l_modelID, string l_key, string l_type, string l_path, bool l_isContinue)
        {
            this.m_modelID = l_modelID;
            this.m_key = l_key;
            this.m_type = l_type;
            this.m_path = l_path;
            this.m_isContinue = l_isContinue;
        }

        /// <summary>
        /// Get string from tag data.
        /// </summary>
        /// <returns>the format string.</returns>
        public override string ToString()
        {
            string data = "0";
            if (m_isContinue) data = "1";
            string file = "";
            if (m_fileName != null) file = m_fileName;
            return this.m_modelID + ":" + this.m_type + ":" + this.m_key + ":" + this.m_path + ":" + data + ":"  + file;
        }

        /// <summary>
        /// Get string from tag data.
        /// </summary>
        /// <returns>the short format string.</returns>
        public String ToShortString()
        {
            string file = "";
            if (m_fileName != null) file = m_fileName;
            return this.m_path + ":" + file;
        }

        /// <summary>
        /// Set hash code to sort.
        /// </summary>
        /// <returns>the hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Check whethere each objects is equal.
        /// </summary>
        /// <param name="obj">check object.</param>
        /// <returns>check result. if each objects is equal, return true.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            TagData t = obj as TagData;
            if (t == null) return false;

            if (t.M_modelID == m_modelID && t.M_key == m_key &&
                t.Type == m_type && t.M_path == m_path &&
                t.isLoaded == m_isLoaded && 
                (t.isLoaded == false || t.FileName == m_fileName))
                return true;

            return base.Equals(obj);
        }

        /// <summary>
        /// Check whethere each objects is equal.
        /// </summary>
        /// <param name="t">check object.</param>
        /// <returns>check result. if each objects is equal, return true.</returns>
        public bool Equals(TagData t)
        {
            if (t == null) return false;

            if (t.M_modelID == m_modelID && t.M_key == m_key &&
                t.Type == m_type && t.M_path == m_path &&
                t.isLoaded == m_isLoaded &&
                (t.isLoaded == false || t.FileName == m_fileName))
                return true;
            return false;
        }
    }
}
