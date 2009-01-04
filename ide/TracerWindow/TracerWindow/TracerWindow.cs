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
        ToolStripMenuItem m_plotWin;
        /// <summary>
        /// The menu item for [Show] -> [Show Save Trace].
        /// </summary>
        ToolStripMenuItem m_showSaveWin;
        /// <summary>
        /// The menu item for [Setup] -> [TraceWindow].
        /// </summary>
        ToolStripMenuItem m_setupWin;
        /// <summary>
        /// The current TracerWindow.
        /// </summary>
        private TraceWindow m_win = null;
        /// <summary>
        /// The setup window for TracerWindow.
        /// </summary>
        private TracerConfigurationDialog m_setup = null;
        private string m_dataformat = "e4";
        /// <summary>
        /// The list of TracerWindow.
        /// </summary>
        private List<TraceWindow> m_winList = new List<TraceWindow>();
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
        /// 
        /// </summary>
        static public double s_duple = 1.25;
        /// <summary>
        /// The time interval to redraw.
        /// </summary>
        int m_timespan = 100;
        bool isStep = false;
        bool isLogAdding = false;
        int m_winCount = 1;
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
        }
        #endregion

        #region Initializer
        /// <summary>
        /// Initialize the plugin.
        /// </summary>
        public override void Initialize()
        {
            m_currentMax = 1.0;
            m_winCount = 1;
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Get menustrips for TracerWindow.
        /// </summary>
        /// <returns>MenuStripItems</returns>
        public override IEnumerable<ToolStripMenuItem> GetMenuStripItems()
        {
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_showWin = new ToolStripMenuItem();
            m_showWin.Text = MessageResources.MenuItemShowTraceText;
            m_showWin.Name = "MenuItemShowTrace";
            m_showWin.Size = new Size(96, 22);
            m_showWin.Enabled = false;
            m_showWin.Click += new EventHandler(this.ShowTracerWindow);

            m_plotWin = new ToolStripMenuItem();
            m_plotWin.Text = MessageResources.MenuItemShowPlotText;
            m_plotWin.Name = "MenuItemShowPlot";
            m_plotWin.Size = new Size(96, 22);
            m_plotWin.Enabled = false;
            m_plotWin.Click += new EventHandler(this.ShowPlotWindow);

            ToolStripMenuItem view = new ToolStripMenuItem();
            view.DropDownItems.AddRange(new ToolStripItem[] {
                m_showWin,
                m_plotWin
            });
            view.Name = "MenuItemView";
            view.Size = new Size(36, 20);
            view.Text = "View";
            tmp.Add(view);

            m_setupWin = new ToolStripMenuItem();
            m_setupWin.Name = "MenuItemShowTraceSetup";
            m_setupWin.Size = new Size(96, 22);
            m_setupWin.Text = MessageResources.MenuItemShowTraceSetupText;
            m_setupWin.Enabled = true;
            m_setupWin.Click += new EventHandler(this.ShowSetupTracerWindow);

            ToolStripMenuItem setup = new ToolStripMenuItem();
            setup.DropDownItems.AddRange(new ToolStripItem[] {
                m_setupWin
            });
            setup.Name = "MenuItemSetup";
            setup.Size = new Size(36, 20);
            setup.Text = "Setup";
            tmp.Add(setup);

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
            tmp.Add(filem);

            return tmp;
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            if (data == null) return;
            foreach (EcellObject obj in data)
            {
                if (obj.Value == null) continue;
                bool isContinue = true;
                foreach (EcellData d in obj.Value)
                {
                    if (d.Name.Equals(EcellProcess.ISCONTINUOUS))
                    {
                        isContinue = (int)d.Value != 0;
                    }
                }
                foreach (EcellData d in obj.Value)
                {
                    if (d.Logged)
                    {
                        AddToEntry(new TagData(obj.ModelID, obj.Key, obj.Type, d.EntityPath, isContinue));
                    }
                }
            }
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            if (data.Value == null) return;

            if (!key.Equals(data.Key))
            {
                foreach (TagData t in m_entry.Keys)
                {
                    if (t.M_modelID != modelID ||
                        t.M_key != key ||
                        t.Type != type) continue;

                    foreach (EcellData d in data.Value)
                    {
                        if (!t.M_path.EndsWith(":" + d.Name)) continue;
                        foreach (TraceWindow w in m_entry[t])
                        {
                            w.ChangeLoggerEntry(t, data.Key, d.EntityPath);
                        }
                        m_tagList.Remove(t.ToString());                          
                        t.M_key = data.Key;
                        t.M_path = d.EntityPath;
                        m_tagList.Add(t.ToString(), t);
                        break;
                    }
                }

                return;
            }

            List<TagData> tagList = new List<TagData>();
            foreach (TagData t in m_entry.Keys)
            {
                if (t.M_modelID != modelID ||
                    t.M_key != key ||
                    t.Type != type) continue;

                bool isHit = false;
                foreach (EcellData d in data.Value)
                {
                    if (!d.Logable) continue;
                    if (t.M_path == d.EntityPath)
                    {
                        isHit = true;
                        if (!d.Logged)
                        {
                            tagList.Add(t);
                        }
                        break;
                    }
                }
                if (isHit == false)
                {
                    tagList.Add(t);
                }
            }

            foreach (TagData t in tagList)
            {
                RemoveFromEntry(t);
            }
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public override void LoggerAdd(string modelID, string key, string type, string path)
        {
            if (isLogAdding) return;
            EcellObject obj = m_dManager.GetEcellObject(modelID, key, type);
            if (obj == null) return;
            bool isContinue = true;
            foreach (EcellData d in obj.Value)
            {
                if (d.Name.Equals(EcellProcess.ISCONTINUOUS))
                {
                    isContinue = (int)d.Value != 0;
                    break;
                }
            }
            TagData tag = new TagData(modelID, key, type, path, isContinue);
            AddToEntry(tag);
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            List<TagData> removeList = new List<TagData>();
            foreach (TagData t in m_entry.Keys)
            {
                if (type == EcellObject.SYSTEM)
                {
                    if (modelID == t.M_modelID && t.M_key.StartsWith(key)) removeList.Add(t);
                }
                else
                {
                    if (modelID == t.M_modelID && t.M_key == key) removeList.Add(t);
                }
            }
            foreach (TagData t in removeList)
                RemoveFromEntry(t);
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            foreach (TagData t in m_tagList.Values)
            {
                foreach (TraceWindow win in m_entry[t])
                {
                    if (!m_winList.Contains(win)) continue;
                    win.RemoveLoggerEntry(t);
                }
            }
            foreach (TraceWindow win in m_winList)
            {
                win.Clear();
            }

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
                m_setupWin.Enabled = true;
                m_showSaveWin.Enabled = false;
            }
            else if (type == ProjectStatus.Loaded ||
                type == ProjectStatus.Loading)
            {
                isStep = false;
                m_showWin.Enabled = true;
                m_setupWin.Enabled = true;
            }
            else if (type == ProjectStatus.Running)
            {
                m_currentMax = 1.0;
                isStep = false;
                m_showWin.Enabled = true;
                m_plotWin.Enabled = true;
                m_setupWin.Enabled = false; 
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
                UpdateGraphDelegate();
                m_showWin.Enabled = true;
                m_plotWin.Enabled = true;
                m_setupWin.Enabled = false;
                m_showSaveWin.Enabled = true;
            }
            else
            {
                isStep = false;
                m_showWin.Enabled = true;
                m_setupWin.Enabled = true;
                m_showSaveWin.Enabled = true;
            }

            if (type == ProjectStatus.Running)
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
                    return t.GetBitmap();
                }
            }
            return null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"TracerWindow"</returns>
        public override string GetPluginName()
        {
            return "TracerWindow";
        }


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
                names.Add(t.Text);
            }
            return names;
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// get/set the current TraceWindow.
        /// </summary>
        public TraceWindow CurrentWin
        {
            get { return this.m_win; }
            set { this.m_win = value; }
        }

        /// <summary>
        /// Get the current simulation time, and calculate the step using that time.
        /// This function get the plot data from DataManager with start, end and step time.
        /// </summary>
        void UpdateGraphDelegate()
        {
            IEnumerable<LogData> list;
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
        }

        /// <summary>
        /// Show the setting dialog.
        /// </summary>
        public void ShowSetupWindow()
        {
            ShowSetupTracerWindow(new object(), new EventArgs());
        }

        /// <summary>
        /// Invoke method to add the data to DataGridView.
        /// </summary>
        /// <param name="tag">tag data</param>
        void AddToEntry(TagData tag)
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
            m_win.AddLoggerEntry(tag);
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
                        if (v.IsList && (int)v.Value == 1)
                            isCont = true;

                        foreach (TraceWindow win in m_winList)
                        {
                            win.SetIsContinuous(t.M_path, isCont);
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
        #endregion

        #region Event
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
                        win.SetIsContinuous(tag.M_path, isCont);
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
        /// Show the setup window for TracerWindow.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void ShowSetupTracerWindow(Object sender, EventArgs e)
        {
            m_setup = new TracerConfigurationDialog(TracerWindow.s_count, (double)(m_timespan / 1000.0), m_dManager.StepCount, m_dataformat);
            using (m_setup)
            {
                if (m_setup.ShowDialog() == DialogResult.OK)
                {
                    TracerWindow.s_count = m_setup.PlotNumber;
                    m_timespan = (int)(m_setup.IntervalSecond * 1000.0);
                    m_dManager.StepCount = m_setup.StepNumber;
                    m_dataformat = m_setup.DataFormat;
                    foreach (TraceWindow m in m_winList)
                        m.DataFormat = m_dataformat;
                }
            }
        }

        void ShowTraceWindow(string filename, bool isNewWin)
        {
            if (isNewWin == true || m_win == null)
                m_showWin.PerformClick();

            m_win.ImportLog(filename);
        }

        /// <summary>
        /// Show the dialog to save the trace.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        m_env.DataManager.SaveSimulationResult(win.DirectoryName,
                            win.Start, win.End, win.FileType, win.SaveList);
                        SaveSimulationResultDelegate dlg = m_env.PluginManager.GetDelegate("SaveSimulationResult") as SaveSimulationResultDelegate;
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

        void ShowPlotWindow(Object sender, EventArgs e)
        {
            PlotterWindow win = new PlotterWindow(this);
            win.Show();
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
            m_win.Shown += new EventHandler(m_win.ShownEvent);
            m_win.m_entry = new List<TagData>();
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
            SetDockContentDelegate dlg = m_env.PluginManager.GetDelegate("SetDockContent") as SetDockContentDelegate;
            if (dlg != null)
                dlg(m_win);

            if (m_winList.Count == 0)
            {
                foreach (TagData tag in m_entry.Keys)
                {
                    m_entry[tag].Add(m_win);
                    m_win.m_entry.Add(tag);
                }
            }
            m_winList.Add(m_win);
            m_win.Show();
        }


        /// <summary>
        /// Execute tracer widonw with threading.
        /// </summary>
        public void TraceWindowAppStart()
        {
            Util.InitialLanguage();
            Application.Run(m_win);
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
        private string m_modelID;
        private string m_key;
        private string m_type;
        private string m_path;
        private bool m_isLoaded = false;
        private bool m_isContinue = false;

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
        /// <returns></returns>
        public override string ToString()
        {
            string data = "0";
            if (m_isContinue) data = "1";
            return this.m_modelID + ":" + this.m_type + ":" + this.m_key + ":" + this.m_path + ":" + data;
        }

        /// <summary>
        /// Set hash code to sort.
        /// </summary>
        /// <returns></returns>
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
                t.Type == m_type && t.M_path == m_path)
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
                t.Type == m_type && t.M_path == m_path)
                return true;
            return false;
        }
    }

    /// <summary>
    /// Class to create line style.
    /// </summary>
    public class LineCreator
    {
        /// <summary>
        /// Get the line style from index.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>line style.</returns>
        public static DashStyle GetLine(int i)
        {
            int j = i / 3;
            if (j == 0) return DashStyle.Solid;
            else if (j == 1) return DashStyle.Dash;
            else if (j == 2) return DashStyle.DashDot;
            else if (j == 3) return DashStyle.Dot;
            else return DashStyle.DashDotDot;
        }
    }

    public enum ValueDataFormat
    {
        Normal = 0,
        Exponential1 = 1,
        Exponential2 = 2,
        Exponential3 = 3,
        Exponential4 = 4,
        Exponential5 = 5,
    }
}
