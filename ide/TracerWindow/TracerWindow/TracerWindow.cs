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
        /// The delegate function for adding rows into DataGridView.
        /// </summary>
        /// <param name="t">TagData.</param>
        delegate void AddRowCallBack(TagData t);
        /// <summary>
        /// The delegate function for removing rows from DataGridView.
        /// </summary>
        /// <param name="t">TagData.</param>
        delegate void RemoveRowCallBack(TagData t);
        /// <summary>
        /// The delegate function for changins system status.
        /// </summary>
        /// <param name="r">system status.</param>
        delegate void ChangeStatusCallBack(int r);
        /// <summary>
        /// The delegate function for clearing rows on DataGirdView.
        /// </summary>
        delegate void ClearCallBack();
        /// <summary>
        /// The delegate function for print screen of plugin.
        /// </summary>
        /// <returns></returns>
        delegate Bitmap PrintCallBack();
        /// <summary>
        /// The menu item for [Show] -> [Show TraceWindow].
        /// </summary>
        ToolStripMenuItem m_showWin;
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
        int m_type;
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
        double m_count = 5000.0;
        /// <summary>
        /// The time interval to redraw.
        /// </summary>
        int m_timespan = 100;
        bool isStep = false;
        #endregion

        /// <summary>
        /// Construcot for TracerWindow.
        /// </summary>
        public TracerWindow()
        {
            m_currentMax = 1.0;

            m_entry = new Dictionary<TagData, List<TraceWindow>>();
            m_tagList = new Dictionary<string, TagData>();
            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(TimerFire);
        }

        /// <summary>
        /// Get the current simulation time, and calculate the step using that time.
        /// This function get the plot data from DataManager with start, end and step time.
        /// </summary>
        void UpdateGraphDelegate()
        {
            List<LogData> list;
            DataManager manager = DataManager.GetDataManager();
            double nextTime = manager.GetCurrentSimulationTime();

            if (m_winList.Count == 0) return;
            if (nextTime > m_currentMax)
            {
                if (nextTime > m_currentMax * 1.3)
                {
                    m_currentMax = nextTime * 1.3;
                    m_step = m_currentMax / m_count;
                    list = manager.GetLogData(0.0, nextTime, m_step);
                }
                else
                {
                    m_currentMax = m_currentMax * 1.3;
                    m_step = m_currentMax / m_count;
                    list = manager.GetLogData(m_current, nextTime, m_step);
                }
            }
            else
            {
                list = manager.GetLogData(m_current, nextTime, m_step);
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
        /// Get the bitmap from TraceWindow in active.
        /// </summary>
        /// <returns>Bitmap</returns>
        Bitmap PrintInvoke()
        {
            return m_win.GetBitmap();
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

            if (m_win.InvokeRequired)
            {
                AddRowCallBack f = new AddRowCallBack(AddRowInvoke);
                m_win.Invoke(f, new object[] { tag });
            }
            else
            {
                m_win.AddLoggerEntry(tag);
            }
        }

        /// <summary>
        /// add the data to DataDridView.
        /// </summary>
        /// <param name="tag">tag data</param>
        void AddRowInvoke(TagData tag)
        {
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
                if (t.InvokeRequired)
                {
                    TraceWindow tmp = m_win;
                    m_win = t;
                    RemoveRowCallBack f = new RemoveRowCallBack(RemoveRowInvoke);
                    t.Invoke(f, new object[] { tag });
                    m_win = tmp;
                }
                else
                {
                    t.RemoveLoggerEntry(tag);
                }
            }
        }

        /// <summary>
        /// remove the row from DataGridView.
        /// </summary>
        /// <param name="tag">row</param>
        void RemoveRowInvoke(TagData tag)
        {
            m_win.RemoveLoggerEntry(tag);
        }

        /// <summary>
        /// Invoke method to clear the data of DataGridView.
        /// </summary>
        public void ClearInvoke()
        {
            foreach (string key in m_win.m_paneDic.Keys)
            {
                m_win.m_paneDic[key].Clear();
                m_win.m_tmpPaneDic[key].Clear();
            }
            m_win.m_paneDic.Clear();
            m_win.m_tmpPaneDic.Clear();
            m_win.dgv.Rows.Clear();
            m_win.UpdateGraph(true);
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
            m_setup.numberTextBox.Text = Convert.ToString(m_count);
            m_setup.intervalTextBox.Text = Convert.ToString(m_timespan / 1000.0);
            m_setup.stepCountTextBox.Text = Convert.ToString(DataManager.GetDataManager().StepCount);
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
            m_count = Convert.ToInt32(m_setup.numberTextBox.Text);
            m_timespan = Convert.ToInt32(Convert.ToDouble(m_setup.intervalTextBox.Text) * 1000.0);
            DataManager.GetDataManager().StepCount = Convert.ToInt32(m_setup.stepCountTextBox.Text);

            m_setup.Dispose();
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
//            m_win.FormClosed += new FormClosedEventHandler(FormDisposed);
            m_win.Shown += new EventHandler(m_win.ShownEvent);
            //            if (m_winList.Count == 0) m_win.m_entry = m_entry;
            //            else m_win.m_entry = new List<TagData>();
            m_win.m_entry = new List<TagData>();

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
            //Thread t = new Thread(new ThreadStart(TraceWindowAppStart));
            //t.Start();
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
            /*
            List<TraceWindow> removeList = new List<TraceWindow>();
            foreach (TraceWindow t in m_winList)
            {
                if (t.Disposing || t.IsDisposed) removeList.Add(t);
                
            }
            foreach (TraceWindow t in removeList)
            {
                m_winList.Remove(t);
                if (t == m_win) m_win = null;
            }
            if (m_win == null && m_winList.Count > 0)
            {
                m_win = m_winList[m_winList.Count - 1];
            }*/
        }
        #endregion

        #region PluginBase
        /// <summary>
        /// Get menustrips for TracerWindow.
        /// </summary>
        /// <returns>MenuStripItems</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResTrace));

            List<ToolStripMenuItem> tmp = new List<ToolStripMenuItem>();

            m_showWin = new ToolStripMenuItem();
            m_showWin.Text = resources.GetString( "MenuItemShowTraceText");
            m_showWin.Name = "MenuItemShowTrace";
            m_showWin.Size = new Size(96, 22);
//            m_showWin.Text = "Show TracerWindow";
            m_showWin.Enabled = false;
            m_showWin.Click += new EventHandler(this.ShowTracerWindow);

            ToolStripMenuItem view = new ToolStripMenuItem();
            view.DropDownItems.AddRange(new ToolStripItem[] {
                m_showWin
            });
            view.Name = "MenuItemView";
            view.Size = new Size(36, 20);
            view.Text = "View";
            tmp.Add(view);

            m_setupWin = new ToolStripMenuItem();
            m_setupWin.Name = "MenuItemShowTraceSetup";
            m_setupWin.Size = new Size(96, 22);
            m_setupWin.Text = resources.GetString("MenuItemShowTraceSetupText");
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
        public List<DockContent> GetWindowsForms()
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
            foreach (EcellObject obj in data)
            {
                if (obj.M_value == null) continue;
                foreach (EcellData d in obj.M_value)
                {
                    if (d.M_isLogger)
                    {
                        AddToEntry(new TagData(obj.modelID, obj.key, obj.type, d.M_entityPath));
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
            if (data.M_value == null) return;

            List<TagData> tagList = new List<TagData>();
            foreach (TagData t in m_entry.Keys)
            {
                if (t.M_modelID != modelID ||
                    t.M_key != key ||
                    t.M_type != type) continue;

                bool isHit = false;
                foreach (EcellData d in data.M_value)
                {
                    if (!d.M_isLogable) continue;
                    if (t.M_path == d.M_entityPath)
                    {
                        isHit = true;
                        if (!d.M_isLogger)
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

            //foreach (EcellData d in data.M_value)
            //{
            //    if (!d.M_isLogable) continue;
            //    //                bool isHit = false;

            //    // If the data changed from logging to no logging.
            //    TagData tag = null;
            //    foreach (TagData t in m_entry.Keys)
            //    {
            //        if (t.M_modelID == modelID && t.M_key == key &&
            //            t.M_type == type && t.M_path == d.M_entityPath)
            //        {
            //            if (!d.M_isLogger)
            //            {
            //                tag = t; ;
            //                RemoveFromEntry(tag);
            //            }
            //            break;
            //        }
            //    }
            //}
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
            TagData tag = new TagData(modelID, key, type, path);
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
                if (type == "System")
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
            if (m_win != null)
            {
                foreach (TraceWindow t in m_winList)
                {
                    if (t.InvokeRequired)
                    {
                        TraceWindow tmp = m_win;
                        m_win = t;
                        ClearCallBack f = new ClearCallBack(ClearInvoke);
                        t.Invoke(f);
                        m_win = tmp;
                    }
                    else
                    {
                        t.dgv.Rows.Clear();
                    }
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
        public void ChangeStatus(int type)
        {
            if (type == Util.NOTLOAD)
            {
                isStep = false;
                m_showWin.Enabled = false;
                m_setupWin.Enabled = true;
            }
            else if (type == Util.LOADED)
            {
                isStep = false;
                m_showWin.Enabled = true;
                m_setupWin.Enabled = true;
            }
            else if (type == Util.RUNNING)
            {
                isStep = false;
                m_showWin.Enabled = false;
                m_setupWin.Enabled = false;
            }
            else if (type == Util.STEP)
            {
                if (isStep == false && m_type != Util.SUSPEND)
                {
                    m_current = 0.0;
                    foreach (TraceWindow t in m_winList)
                    {
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
                m_setupWin.Enabled = false;
            }

            if (type == Util.RUNNING)
            {
                this.StartSimulation();
            }
            else if ((m_type == Util.RUNNING || m_type == Util.SUSPEND || m_type == Util.STEP) &&
                type == Util.LOADED)
            {
                this.StopSimulation();
            }
            else if (type == Util.SUSPEND)
            {
                this.SuspendSimulation();
            }
            else if (type == Util.STEP)
            {
                if (m_type == Util.STEP)
                {
                    this.SuspendSimulation();
                    type = Util.SUSPEND;
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
        public Bitmap Print()
        {
            if (m_win != null)
            {
                PrintCallBack f = new PrintCallBack(PrintInvoke);
                return m_win.Invoke(f) as Bitmap;

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
        public bool IsEnablePrint()
        {
            if (m_win != null) return true;
            else return false;
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
        public string M_type
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
        public TagData(string l_modelID, string l_key, string l_type, string l_path)
        {
            this.m_modelID = l_modelID;
            this.m_key = l_key;
            this.m_type = l_type;
            this.m_path = l_path;
        }

        /// <summary>
        /// Get string from tag data.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.m_modelID + ":" + this.m_type + ":" + this.m_key + ":" + this.m_path;
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
                t.M_type == m_type && t.M_path == m_path)
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
                t.M_type == m_type && t.M_path == m_path)
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
