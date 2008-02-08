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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;

namespace EcellLib.TracerWindow
{
    /// <summary>
    /// Plugin Class of TracerWindow.
    /// </summary>
    public class TracerWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// The delegate function for changins system status.
        /// </summary>
        /// <param name="r">system status.</param>
        delegate void ChangeStatusCallBack(int r);
        DataManager m_dManager = DataManager.GetDataManager();
        /// <summary>
        /// The menu item for [Show] -> [Show TraceWindow].
        /// </summary>
        ToolStripMenuItem m_showWin;
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
        private TracerWindowSetup m_setup = null;
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
        /// <summary>
        /// ResourceManager for TraceWindow.
        /// </summary>
        public static ComponentResourceManager s_resources = new ComponentResourceManager(typeof(MessageResTrace));

        #endregion

        /// <summary>
        /// Construcot for TracerWindow.
        /// </summary>
        public TracerWindow()
        {
            m_currentMax = 1.0;
            m_winCount = 1;

            m_entry = new Dictionary<TagData, List<TraceWindow>>();
            m_tagList = new Dictionary<string, TagData>();
            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(TimerFire);
        }

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
                    if (list != null)
                    {
                        foreach (LogData d in list)
                        {
                            List<LogValue> delList = new List<LogValue>();
                            foreach (LogValue v in d.logValueList)
                            {
                                if (v.time < m_current) delList.Add(v);
                            }
                            foreach (LogValue v in delList)
                            {
                                d.logValueList.Remove(v);
                            }
                        }
                    }
                }
            }
            else if (m_current + m_step < nextTime)
            {
                list = m_dManager.GetLogData(m_current, nextTime, m_step);
                if (list != null)
                {
                    foreach (LogData d in list)
                    {
                        List<LogValue> delList = new List<LogValue>();
                        foreach (LogValue v in d.logValueList)
                        {
                            if (v.time < m_current) delList.Add(v);
                        }
                        foreach (LogValue v in delList)
                        {
                            d.logValueList.Remove(v);
                        }
                    }
                }
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
                    t.AddPoints(m_currentMax, nextTime, list);
            }
            list.Clear();
            list = null;
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

            if (m_win == null) return;
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
                m_currentMax = 10.0;

                Dictionary<TagData, bool> tagDic = new Dictionary<TagData, bool>();
                foreach (TagData t in m_tagList.Values)
                {
                    bool isHit = false;
                    if (t.Type != EcellObject.PROCESS) continue;
                    foreach (TagData ct in tagDic.Keys)
                    {
                        if (t.M_modelID == ct.M_modelID &&
                            t.M_key == ct.M_key)
                        {
                            isHit = true;
                            break;
                        }
                    }
                    if (isHit) continue;
                    string FullPN = t.Type + Constants.delimiterColon + t.M_key +
                        Constants.delimiterColon + EcellProcess.ISCONTINUOUS;
                    EcellValue v = m_dManager.GetEntityProperty(FullPN);
                    if (v == null) continue;
                    bool isCont = false;
                    if (v.CastToInt() == 1) isCont = true;

                    tagDic.Add(t, isCont);
                    foreach (TraceWindow win in m_winList)
                    {
                        win.SetIsContinuous(t.M_path, isCont);
                    }
                }
            }
            if (m_timespan <= 0) m_timespan = 1000;
            m_time.Interval = m_timespan;

            foreach (TraceWindow w in m_winList) w.StartSimulation();

            isSuspend = false;
            m_time.Enabled = true;
            m_time.Start();
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


        #region Event
        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void TimerFire(object sender, EventArgs e)
        {
            m_time.Enabled = false;
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
            m_setup = new TracerWindowSetup();
            m_setup.numberTextBox.Text = Convert.ToString(TracerWindow.s_count);
            m_setup.intervalTextBox.Text = Convert.ToString(m_timespan / 1000.0);
            m_setup.stepCountTextBox.Text = Convert.ToString(m_dManager.StepCount);
            m_setup.TSApplyButton.Click += new EventHandler(this.SetupTraceWindowClick);
            m_setup.ShowDialog();
        }

        /// <summary>
        /// The action on clicking OK Button in TracerWindowSetup.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void SetupTraceWindowClick(object sender, EventArgs e)
        {
            try
            {
                TracerWindow.s_count = Convert.ToInt32(m_setup.numberTextBox.Text);
                m_timespan = Convert.ToInt32(Convert.ToDouble(m_setup.intervalTextBox.Text) * 1000.0);
                m_dManager.StepCount = Convert.ToInt32(m_setup.stepCountTextBox.Text);
            }
            catch (Exception ex)
            {
                ex.ToString();
                TracerWindow.s_resources.GetString("ErrInputData");
                m_setup.Dispose();
                return;
            }

            m_setup.Dispose();
        }

        /// <summary>
        /// Show the dialog to save the trace.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowSaveTracerWindow(Object sender, EventArgs e)
        {
            SaveTraceWindow win = new SaveTraceWindow();
            win.AddEntry(m_entry);
            win.ShowDialog();
        }

        /// <summary>
        /// Show tracer window with thread.
        /// Tracer window works busy, then this function is threading.
        /// </summary>
        /// <param name="sender">object(MenuItem)</param>
        /// <param name="e">EventArgs</param>
        void ShowTracerWindow(Object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            m_win = new TraceWindow();
            m_win.m_parent = Form.ActiveForm;
            m_win.Disposed += new EventHandler(FormDisposed);
            m_win.Shown += new EventHandler(m_win.ShownEvent);
            m_win.m_entry = new List<TagData>();
            m_win.Control = this;
            m_win.Text = m_win.Text + m_winCount;
            m_win.TabText = m_win.Text;
            m_winCount++;

            // Set Dock settings
            DockPanel panel = PluginManager.GetPluginManager().DockPanel;
            m_win.DockHandler.DockPanel = panel;
            m_win.DockHandler.FloatPane = panel.DockPaneFactory.CreateDockPane(m_win, DockState.Float, true);
            FloatWindow fw = panel.FloatWindowFactory.CreateFloatWindow(
                                panel,
                                m_win.FloatPane,
                                new Rectangle(m_win.Left, m_win.Top, m_win.Width, m_win.Height));
            m_win.Pane.DockTo(fw);

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

        #region PluginBase
        /// <summary>
        /// Get menustrips for TracerWindow.
        /// </summary>
        /// <returns>MenuStripItems</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_showWin = new ToolStripMenuItem();
            m_showWin.Text = TracerWindow.s_resources.GetString( "MenuItemShowTraceText");
            m_showWin.Name = "MenuItemShowTrace";
            m_showWin.Size = new Size(96, 22);
            m_showWin.Enabled = false;
            m_showWin.Click += new EventHandler(this.ShowTracerWindow);

            m_showSaveWin = new ToolStripMenuItem();
            m_showSaveWin.Text = TracerWindow.s_resources.GetString("MenuItemShowSaveTraceText");
            m_showSaveWin.Name = "MenuItemShowSaveTrace";
            m_showSaveWin.Size = new Size(96, 22);
            m_showSaveWin.Enabled = false;
            m_showSaveWin.Click += new EventHandler(this.ShowSaveTracerWindow);

            ToolStripMenuItem view = new ToolStripMenuItem();
            view.DropDownItems.AddRange(new ToolStripItem[] {
                m_showWin,
                m_showSaveWin
            });
            view.Name = "MenuItemView";
            view.Size = new Size(36, 20);
            view.Text = "View";
            tmp.Add(view);

            m_setupWin = new ToolStripMenuItem();
            m_setupWin.Name = "MenuItemShowTraceSetup";
            m_setupWin.Size = new Size(96, 22);
            m_setupWin.Text = TracerWindow.s_resources.GetString("MenuItemShowTraceSetupText");
//            m_setupWin.Text = "TracerWindow";
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

            return tmp;
        }

        /// <summary>
        /// Get toolbar buttons for TracerWindow
        /// </summary>
        /// <returns>null</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for TracerWindow plugin.
        /// </summary>
        /// <returns>null</returns>
        public List<EcellDockContent> GetWindowsForms()
        {
            return null;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // nothing
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            // not implement
        }


        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
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
                        if (d.Value.CastToInt() == 1) isContinue = true;
                        else isContinue = false;
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
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            if (data.Value == null) return;

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
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            if (isLogAdding) return;
            EcellObject obj = m_dManager.GetEcellObject(modelID, key, type);
            if (obj == null) return;
            bool isContinue = true;
            foreach (EcellData d in obj.Value)
            {
                if (d.Name.Equals(EcellProcess.ISCONTINUOUS))
                {
                    if (d.Value.CastToInt() == 1) isContinue = true;
                    else isContinue = false;
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
        public void DataDelete(string modelID, string key, string type)
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
        /// The event sequence when the simulation parameter is added.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID.</param>
        public void ParameterAdd(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is deleted.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterDelete(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence when the simulation parameter is set.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterSet(string projectID, string parameterID)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            foreach (TagData t in m_tagList.Values)
            {
                foreach (TraceWindow win in m_entry[t]) 
                {
                    if (!m_winList.Contains(win)) continue;
                    win.RemoveLoggerEntry(t);
                }
            }
            m_tagList.Clear();
            m_entry.Clear();
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
            // nothing
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Uninitialized)
            {
                isStep = false;
                m_showWin.Enabled = false;
                m_showSaveWin.Enabled = false;
                m_setupWin.Enabled = true;
            }
            else if (type == ProjectStatus.Loaded)
            {
                isStep = false;
                m_showWin.Enabled = true;
                m_showSaveWin.Enabled = true;
                m_setupWin.Enabled = true;
            }
            else if (type == ProjectStatus.Running)
            {
                m_currentMax = 1.0;
                isStep = false;
                m_showWin.Enabled = false;
                m_showSaveWin.Enabled = false;
                m_setupWin.Enabled = false;
            }
            else if (type == ProjectStatus.Stepping)
            {
                if (isStep == false && m_type != ProjectStatus.Suspended)
                {
                    m_current = 0.0;
                    m_currentMax = 1.0;
                    foreach (TraceWindow t in m_winList)
                    {
                        t.ClearTime();
                        t.m_current = 0.0;
                    }
                }
                isStep = true;
                UpdateGraphDelegate();
            }
            else
            {
                isStep = false;
                m_showWin.Enabled = false;
                m_showSaveWin.Enabled = false;
                m_setupWin.Enabled = false;
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
        /// Change availability of undo/redo function
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="directory">output directory.</param>
        public void SaveModel(string modelID, string directory)
        {
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
        public string GetPluginName()
        {
            return "TracerWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public List<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            foreach (TraceWindow t in m_winList)
            {
                names.Add(t.Text);
            }
            return names;
        }

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
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

    /// <summary>
    /// The class to create color object and color brush.
    /// </summary>
    public class ColorCreator
    {
        /// <summary>
        /// Get the object of color.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>color object.</returns>
        public static Color GetColor(int i)
        {
            int j = i % 3;
            if (j == 0) return Color.OrangeRed;
            else if (j == 1) return Color.LightSkyBlue;
            else if (j == 2) return Color.LightGreen;
            else if (j == 3) return Color.LightSalmon;
            else if (j == 4) return Color.Gold;
            else if (j == 5) return Color.LimeGreen;
            else if (j == 6) return Color.Coral;
            else if (j == 7) return Color.Navy;
            else if (j == 8) return Color.Lime;
            else if (j == 9) return Color.Purple;
            else if (j == 10) return Color.SkyBlue;
            else if (j == 11) return Color.Green;
            else if (j == 12) return Color.Plum;
            else if (j == 13) return Color.HotPink;
            else if (j == 14) return Color.Orchid;
            else if (j == 15) return Color.Tomato;
            else if (j == 16) return Color.Orange;
            else if (j == 17) return Color.Magenta;
            else if (j == 18) return Color.Blue;
            else if (j == 19) return Color.Red;
            else return Color.Black;
        }

        /// <summary>
        /// Get the brush of color.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>Brush.</returns>
        public static Brush GetColorBlush(int i)
        {
            int j = i % 3;
            if (j == 0) return Brushes.OrangeRed;
            else if (j == 1) return Brushes.LightSkyBlue;
            else if (j == 2) return Brushes.LightGreen;
            else if (j == 3) return Brushes.LightSalmon;
            else if (j == 4) return Brushes.Gold;
            else if (j == 5) return Brushes.LimeGreen;
            else if (j == 6) return Brushes.Coral;
            else if (j == 7) return Brushes.Navy;
            else if (j == 8) return Brushes.Lime;
            else if (j == 9) return Brushes.Purple;
            else if (j == 10) return Brushes.SkyBlue;
            else if (j == 11) return Brushes.Green;
            else if (j == 12) return Brushes.Plum;
            else if (j == 13) return Brushes.HotPink;
            else if (j == 14) return Brushes.Orchid;
            else if (j == 15) return Brushes.Tomato;
            else if (j == 16) return Brushes.Orange;
            else if (j == 17) return Brushes.Magenta;
            else if (j == 18) return Brushes.Blue;
            else if (j == 19) return Brushes.Red;
            else return Brushes.Black;
        }
    }
}
