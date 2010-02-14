//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2010 Keio University
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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using ZedGraph;

using Ecell.Objects;
using Ecell.Logger;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Form to show trace of target object property.
    /// </summary>
    public partial class TraceWindow : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// The flag whether this windos contains only the log data.
        /// </summary>
        private bool m_isLog = false;
        /// <summary>
        /// The object managed this window.
        /// </summary>
        TracerWindow m_owner;
        /// <summary>
        /// The flag whether simulation suspends.
        /// </summary>
        bool isSuspend = false;
        /// <summary>
        /// Graph control for tracer.
        /// </summary>
        public ZedGraphControl m_zCnt;
        /// <summary>
        /// The last time on drawing tracer.
        /// </summary>
        public double m_current;
        /// <summary>
        /// The flag whether this window set the default axis settings.
        /// </summary>
        private bool m_isDefault = true;
        /// <summary>
        /// The setting objects.
        /// </summary>
        private YAxisSettings m_AxisSetting;
        /// <summary>
        /// The number of entry in this window.
        /// </summary>
        private int m_entryCount = 0;
        /// <summary>
        /// The number of log data in this window.
        /// </summary>
        private int m_logCount = 0;
        /// <summary>
        /// The list of logging data in this window.
        /// </summary>
        private List<TagData> m_logList = new List<TagData>();
        /// <summary>
        /// The dictionary of fullPN and the flag whether this entry is continuous.
        /// </summary>
        private Dictionary<string, bool> m_tagDic = new Dictionary<string, bool>();
        /// <summary>
        /// The dictionary of ID and entry object.
        /// </summary>
        private Dictionary<string, TraceEntry> m_entryDic = new Dictionary<string, TraceEntry>();
        /// <summary>
        /// The delegate for updating the graph window.
        /// </summary>
        /// <param name="isAxis">the flag whether the range of axis is changed.</param>
        delegate void UpdateGraphCallBack(bool isAxis);
        /// <summary>
        /// The delegate for changing this aplication status.
        /// </summary>
        /// <param name="status">system status.</param>
        delegate void ChangeStatusCallBack(bool status);
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the setting objects.
        /// </summary>
        public YAxisSettings AxisSettings
        {
            get { return this.m_AxisSetting; }
            set
            {
                if (m_isDefault)
                    this.m_AxisSetting = value;
            }
        }

        /// <summary>
        /// set the default format of data.
        /// </summary>
        public string DataFormat
        {
            set { 
                m_zCnt.PointValueFormat = value;
                m_zCnt.GraphPane.YAxis.Scale.Format = value;
                m_zCnt.GraphPane.Y2Axis.Scale.Format = value;
                m_zCnt.Refresh();
            }
        }
        #endregion

        /// <summary>
        /// Constructor for TraceWindow.
        /// </summary>
        public TraceWindow(TracerWindow control)
        {
            m_owner = control;
            this.HideOnClose = false;
            InitializeComponent();
            
            m_zCnt = new ZedGraphControl();
            m_zCnt.Dock = DockStyle.Fill;
            m_zCnt.GraphPane.Title.Text = "";
            m_zCnt.GraphPane.XAxis.Title.Text = "Time(sec)";
            m_zCnt.GraphPane.YAxis.Scale.Format = m_owner.DataManager.DisplayStringFormat;
            m_zCnt.GraphPane.Y2Axis.Scale.Format = m_owner.DataManager.DisplayStringFormat;
            m_zCnt.GraphPane.YAxis.Title.Text = "";
            m_zCnt.GraphPane.Legend.IsVisible = false;
            m_zCnt.GraphPane.XAxis.Scale.Max = 10;
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = false;
            m_zCnt.GraphPane.XAxis.Scale.Min = 0;
            m_zCnt.IsEnableWheelZoom = false;
            m_zCnt.IsEnableHPan = false;
            m_zCnt.IsEnableVPan = false;
            m_zCnt.ZoomEvent += new ZedGraphControl.ZoomEventHandler(ZcntZoomEvent);                       
            m_zCnt.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(ZedControlContextMenuBuilder);
            m_zCnt.GraphPane.Margin.Top = 35.0f;
            m_zCnt.GraphPane.YAxis.MajorGrid.IsVisible = true;
            m_zCnt.GraphPane.XAxis.MinorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.XAxis.MajorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MinorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MajorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.Chart.Border.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MajorGrid.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.Fill = new Fill(Color.White, Color.LightGray, 90.0f);

            this.m_AxisSetting = new YAxisSettings();

            tableLayoutPanel1.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();

            this.ContentType = DockContentType.GRAPH;
            this.HideOnClose = false;
        }

        /// <summary>
        /// Print this window.
        /// </summary>
        public void Print()
        {
            m_zCnt.DoPrint();
        }

        /// <summary>
        /// Changed the logger object.
        /// </summary>
        /// <param name="orgFullPN">the orijinal FullPN.</param>
        /// <param name="entry">the logger object.</param>
        public void LoggerChanged(string orgFullPN, LoggerEntry entry)
        {            
            TagData otag = new TagData(entry.ModelID, entry.ID, entry.Type, orgFullPN, true);
            TagData ntag = new TagData(entry.ModelID, entry.ID, entry.Type, entry.FullPN, true);
            otag.isLoaded = entry.IsLoaded;
            otag.FileName = entry.FileName;
            ntag.isLoaded = entry.IsLoaded;
            ntag.FileName = entry.FileName;
            if (!m_entryDic.ContainsKey(otag.ToShortString()))
                return;

            if (orgFullPN != entry.FullPN)
            {
                m_entryDic[ntag.ToShortString()] = m_entryDic[otag.ToShortString()];
                m_entryDic[ntag.ToShortString()].Path = entry.FullPN;
                m_entryDic.Remove(otag.ToShortString());
            }
            
            m_entryDic[ntag.ToShortString()].SetStyle(entry.LineStyle);
            m_entryDic[ntag.ToShortString()].SetVisible(entry.IsShown);
            m_entryDic[ntag.ToShortString()].SetColor(entry.Color);
            m_entryDic[ntag.ToShortString()].SetLineWidth(entry.LineWidth);
            m_entryDic[ntag.ToShortString()].SetY2Axis(entry.IsY2Axis);
            if (entry.IsY2Axis)
            {
                m_zCnt.GraphPane.Y2Axis.IsVisible = true;
            }
            else
            {
                bool isHit = false;
                foreach (string entStr in m_entryDic.Keys)
                {
                    if (m_entryDic[entStr].IsY2)
                    {
                        m_zCnt.GraphPane.Y2Axis.IsVisible = true;
                        isHit = true;
                        break;

                    }
                }
                if (isHit == false)
                    m_zCnt.GraphPane.Y2Axis.IsVisible = false;
            }

            if (entry.IsShown)
            {
                if (!m_zCnt.GraphPane.IsZoomed)
                {
                    m_zCnt.AxisChange();
                }
            }
            m_zCnt.Refresh();
        }


        /// <summary>
        /// Set the continuous of data.
        /// </summary>
        /// <param name="tag">the entry name.</param>
        /// <param name="isCont">the flag whether this data is continuous.</param>
        public void SetIsContinuous(string tag, bool isCont)
        {
            if (m_tagDic.ContainsKey(tag))
            {
                m_tagDic[tag] = isCont;
            }
        }

        /// <summary>
        /// Get bitmap from ZedGraphControl.
        /// </summary>
        /// <returns>bitmap</returns>
        public Bitmap GetBitmap()
        {
            Bitmap b = new Bitmap(m_zCnt.Width, m_zCnt.Height);
            m_zCnt.DrawToBitmap(b, m_zCnt.ClientRectangle);
            return b;
        }

        /// <summary>
        /// Clear the information of project.
        /// </summary>
        public void Clear()
        {
            foreach (TagData t in m_logList)
            {
                RemoveLoggerEntry(t);
            }
            m_tagDic.Clear();
            m_entryDic.Clear();
            m_logList.Clear();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Add logger entry to DataGridView and ZedGraphControl.
        /// Added logger entry is registed to m_paneDic.
        /// </summary>
        /// <param name="entry">the log entry</param>
        /// <param name="tag">the tag of logger entry</param>
        public void AddLoggerEntry(LoggerEntry entry, TagData tag)
        {
            LineItem i = m_zCnt.GraphPane.AddCurve(entry.FullPN,
                    new PointPairList(), entry.Color, SymbolType.None);
            i.Line.Width = entry.LineWidth;
            i.Line.Style = entry.LineStyle;
            LineItem i1 = m_zCnt.GraphPane.AddCurve(entry.FullPN,
                    new PointPairList(), entry.Color, SymbolType.None);
            i1.Line.Width = entry.LineWidth;
            i1.Line.Style = entry.LineStyle;
            m_entryDic.Add(tag.ToShortString(), new TraceEntry(tag.M_path, i, i1, tag.IsContinue, tag.isLoaded));
            m_tagDic.Add(tag.ToShortString(), tag.IsContinue);
            if (!tag.isLoaded)
            {
                if (m_logCount == 0 && 
                    (m_owner.PluginManager.Status == ProjectStatus.Running ||
                    m_owner.PluginManager.Status == ProjectStatus.Suspended ||
                    m_owner.PluginManager.Status == ProjectStatus.Stepping))
                    this.StartSimulation();
                m_logCount++;
            }
            else
            {
                LogData log = m_owner.DataManager.LoadSimulationResult(entry.FileName);
                
                string[] ele = log.propName.Split(new char[] { ':' });
                LogData newLog = new LogData(log.model, log.key, Constants.xpathLog, ele[ele.Length - 1], log.logValueList);
                newLog.IsLoaded = true;
                newLog.FileName = entry.FileName;
                List<LogData> logList = new List<LogData>();
                logList.Add(newLog);
                m_entryDic[tag.ToShortString()].SetVisible(entry.IsShown);
                AddPoints(log.logValueList[log.logValueList.Count - 1].time, log.logValueList[log.logValueList.Count - 1].time, logList, true);
            }
            m_logList.Add(tag);
            m_entryCount++;
        }

        /// <summary>
        /// Remove logger entry from DataGridView,
        /// </summary>
        /// <param name="tag">logger entry.</param>
        public void RemoveLoggerEntry(TagData tag)
        {
            if (m_entryDic.ContainsKey(tag.ToShortString()))
            {
                if (!m_entryDic[tag.ToShortString()].IsLoaded) m_logCount--;
                string path = tag.M_path;
                m_entryDic[tag.ToShortString()].ClearPoint();
                m_zCnt.GraphPane.CurveList.Remove(m_entryDic[tag.ToShortString()].CurrentLineItem);
                m_zCnt.GraphPane.CurveList.Remove(m_entryDic[tag.ToShortString()].TmpLineItem);
                m_entryDic.Remove(tag.ToShortString());

                UpdateGraphCallBack dlg = new UpdateGraphCallBack(UpdateGraph);
                this.Invoke(dlg, new object[] { true });
            }
            if (m_tagDic.ContainsKey(tag.ToShortString()))
                m_tagDic.Remove(tag.ToShortString());
        }

        /// <summary>
        /// Change the display status of data.
        /// </summary>
        /// <param name="entry">the log entry object.</param>
        /// <param name="isDisplay">the flag whether this entry is displayed.</param>
        public void ChangedDisplayStatus(LoggerEntry entry, bool isDisplay)
        {
            TagData tag = new TagData(entry.ModelID, entry.ID, entry.Type, entry.FullPN, true);
            tag.isLoaded = entry.IsLoaded;
            tag.FileName = entry.FileName;
            if (isDisplay)
            {
                AddLoggerEntry(entry, tag);
            }
            else
            {
                RemoveLoggerEntry(tag);
            }
        }

        /// <summary>
        /// Get the flag whether this entry is displayed.
        /// </summary>
        /// <param name="entry">the logger entry.</param>
        /// <returns>Return true if </returns>
        public bool IsDisplay(LoggerEntry entry)
        {
            TagData tag = new TagData(entry.ModelID, entry.ID, entry.Type, entry.FullPN, true);
            tag.isLoaded = entry.IsLoaded;
            tag.FileName = entry.FileName;
            if (m_entryDic.ContainsKey(tag.ToShortString())) return true;
            return false;
        }

        /// <summary>
        /// Initial time.
        /// </summary>
        public void ClearTime()
        {
            foreach (string key in m_entryDic.Keys)
            {
                if (m_entryDic[key].IsLoaded) continue;
                m_entryDic[key].ClearPoint();
            }
            m_current = 0.0;
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Reset x axis.
        /// </summary>
        public void Reset()
        {
            if (m_zCnt.GraphPane.IsZoomed)
            {
                m_zCnt.ZoomOutAll(m_zCnt.GraphPane);
            }
            else
            {
                m_zCnt.GraphPane.XAxis.Scale.Max = 10;
            }
        }

        /// <summary>
        /// Call this function, when simulation start.
        /// </summary>
        public void StartSimulation()
        {
            if (m_logCount <= 0) return;

            if (!isSuspend || m_zCnt.GraphPane.IsZoomed)
            {
                foreach (string key in m_entryDic.Keys)
                {
                    if (m_entryDic[key].IsLoaded) continue;
                    m_entryDic[key].ClearPoint();
                }
                m_current = 0.0;
            }
            double max = 0.0;
            double plotCount = TracerWindow.s_count;
            List<EcellObject> stepperList = m_owner.DataManager.GetStepper(
                m_owner.DataManager.GetModelList()[0]);
            foreach (EcellObject obj in stepperList)
            {
                if (obj.Value == null) continue;
                foreach (EcellData data in obj.Value)
                {
                    if (data.Name.Equals("StepInterval"))
                    {
                        double tmp = Convert.ToDouble(data.Value.ToString()) * plotCount;
                        if (tmp > max)
                            max = tmp;
                    }
                }
            }            

            if (this.InvokeRequired)
            {
                ChangeStatusCallBack f = new ChangeStatusCallBack(ChangeStatus);
                this.Invoke(f, new object[] { false });
            }
            else
            {
                if (!isSuspend)
                {
                    m_zCnt.AxisChange();
                    m_zCnt.Refresh();
                }
            }
            isSuspend = false;
        }

        /// <summary>
        /// Set the default setting of Y axis.
        /// </summary>
        /// <param name="setting">the setting object.</param>
        public void SetDefaultSetting(YAxisSettings setting)
        {
            if (m_isDefault)
            {
                m_AxisSetting = setting;
                ResetWindow();
            }
        }


        /// <summary>
        /// Call this function, when status of system is changed.
        /// </summary>
        /// <param name="status">system status.</param>
        public void ChangeStatus(bool status)
        {
            if (status == false)
            {
                m_zCnt.AxisChange();
                m_zCnt.Refresh();
            }
        }

        /// <summary>
        /// Call this function, when simulation stop.
        /// Stop the timer.
        /// </summary>
        public void StopSimulation()
        {
            ChangeStatus(true);
            isSuspend = false;
        }

        /// <summary>
        /// Call this function, when simulation suspend.
        /// Stop the timer.
        /// </summary>
        public void SuspendSimulation()
        {
            if (this.InvokeRequired)
            {
                ChangeStatusCallBack f = new ChangeStatusCallBack(ChangeStatus);
                this.Invoke(f, new object[] { true });
            }
            isSuspend = true;
        }

        /// <summary>
        /// Update graph view using ZedGraphControl function.
        /// </summary>
        /// <param name="isAxis">the flag whether process execute AxisChange function.</param>
        public void UpdateGraph(bool isAxis)
        {
            if (isAxis && !m_zCnt.GraphPane.IsZoomed)
            {
                m_zCnt.AxisChange();
                m_zCnt.Refresh();
            }
            else
            {
                Graphics g = m_zCnt.CreateGraphics();
                g.ResetClip();
                g.SetClip(m_zCnt.MasterPane.Rect);
                foreach (string key in m_entryDic.Keys)
                {
                    m_entryDic[key].TmpLineItem.Draw(g, m_zCnt.GraphPane, 0, 1.5F);
                }
                g.ResetClip();
            }
        }


        /// <summary>
        /// Add the simulation data and redraw the points.
        /// </summary>
        /// <param name="maxAxis">max axis of x.</param>
        /// <param name="nextTime">current time of simulation.</param>
        /// <param name="data">the simulation data.</param>
        /// <param name="isLoaded">the flag whether this log is loaded.</param>
        public void AddPoints(double maxAxis, double nextTime, List<LogData> data, bool isLoaded)
        {
            bool isAxis = false;

            if (!isLoaded && m_logCount <= 0) return;

            if (m_zCnt.GraphPane.IsZoomed)
            {
                if (m_current > m_zCnt.GraphPane.XAxis.Scale.Max ||
                    nextTime < m_zCnt.GraphPane.XAxis.Scale.Min)
                {
                    m_current = nextTime;
                    return;
                }
            }
            else
            {
                if (nextTime > m_zCnt.GraphPane.XAxis.Scale.Max)
                {
                    if (nextTime > m_zCnt.GraphPane.XAxis.Scale.Max * TracerWindow.s_duple)
                    {
                        m_zCnt.GraphPane.XAxis.Scale.Max = maxAxis;
                        foreach (string key in m_entryDic.Keys)
                        {
                            if (m_entryDic[key].IsLoaded) continue;
                            m_entryDic[key].ClearPoint();
                        }
                    }
                    else
                    {
                        m_zCnt.GraphPane.XAxis.Scale.Max = maxAxis;
                        foreach (string key in m_entryDic.Keys)
                        {
                            if (m_entryDic[key].IsLoaded) continue;
                            m_entryDic[key].ThinPoints();
                        }
                    }
                    isAxis = true;
                }
            }
            m_current = nextTime;
 
            if (data == null) return;
            foreach (LogData d in data)
            {
                string file = "";
                if (d.FileName != null) file = d.FileName;
                string p;
                if (!d.type.Equals(EcellObject.SYSTEM))
                {
                    p = d.type + ":" + d.key + ":" + d.propName + ":" + file;
                }
                else
                {
                    string pre, post;
                    int ind = d.key.LastIndexOf('/');
                    if (d.key.Equals("/"))
                    {
                        pre = "";
                        post = "/";
                    }
                    else
                    {
                        if (ind == 0)
                        {
                            pre = "/";
                        }
                        else
                        {
                            pre = d.key.Substring(0, ind);
                        }
                        post = d.key.Substring(ind + 1);
                    }
                    p = d.type + ":" + pre + ":" + post + ":" + d.propName + ":" + file;
                }
                if (!m_entryDic.ContainsKey(p)) continue;
                if (m_entryDic[p].IsLoaded != d.IsLoaded) continue;

                bool isRet = m_entryDic[p].AddPoint(d.logValueList, 
                    m_zCnt.GraphPane.XAxis.Scale.Max,
                    m_zCnt.GraphPane.XAxis.Scale.Min,
                    m_zCnt.GraphPane.YAxis.Scale.Max, 
                    m_zCnt.GraphPane.YAxis.Scale.Min,
                    m_zCnt.GraphPane.IsZoomed);
                if (isAxis == false)
                {
                    isAxis = isRet;
                }
            }
            // Zoom中に軸の変更をしないようする
            if (m_zCnt.GraphPane.IsZoomed) isAxis = false;

            if (isAxis == true)
            {
                // 変動が少ないトレースでは点線が実線になってしまうため、
                // 変動の状態を確認しLine.IsSmoothプロパティをtrueに変更する。
                // 全データを変更しないのは、Drosophilaのように振動している場合に
                // Smoothを利用すると髭が発生してしまうために使用できなかった。
                foreach (string key in m_entryDic.Keys)
                {
                    if (m_entryDic[key].IsLoaded) continue;
                    if (m_isLog) continue;
                    if (m_entryDic[key].CurrentLineItem.Line.IsSmooth) continue;
                    if (m_zCnt.GraphPane.IsZoomed) continue;
                    if (!m_entryDic[key].IsSmoothing(m_zCnt.GraphPane.XAxis.Scale.Max,
                        m_zCnt.GraphPane.XAxis.Scale.Min,
                        m_zCnt.GraphPane.YAxis.Scale.Max,
                        m_zCnt.GraphPane.YAxis.Scale.Min,
                        m_zCnt.Width, m_zCnt.Height))
                    {
                        m_entryDic[key].CurrentLineItem.Line.IsSmooth = true;
                        m_entryDic[key].TmpLineItem.Line.IsSmooth = true;                        
                    }
                    else
                    {
                        m_entryDic[key].CurrentLineItem.Line.IsSmooth = false;
                        m_entryDic[key].TmpLineItem.Line.IsSmooth = false;
                    }

                }
            }
            UpdateGraphCallBack dlg = new UpdateGraphCallBack(UpdateGraph);
            this.Invoke(dlg, new object[] { isAxis });
        }

        /// <summary>
        /// Reset the window to change the setting property.
        /// </summary>
        private void ResetWindow()
        {
            if (!m_zCnt.GraphPane.IsZoomed)
            {
                m_zCnt.GraphPane.YAxis.Scale.MaxAuto = m_AxisSetting.IsAutoMaxY;
                m_zCnt.GraphPane.YAxis.Scale.MinAuto = m_AxisSetting.IsAutoMinY;
                m_zCnt.GraphPane.Y2Axis.Scale.MaxAuto = m_AxisSetting.IsAutoMaxY2;
                m_zCnt.GraphPane.Y2Axis.Scale.MinAuto = m_AxisSetting.IsAutoMinY2;

                if (!m_AxisSetting.IsAutoMaxY)
                    m_zCnt.GraphPane.YAxis.Scale.Max = m_AxisSetting.YMax;
                if (!m_AxisSetting.IsAutoMinY)
                    m_zCnt.GraphPane.YAxis.Scale.Min = m_AxisSetting.YMin;
                if (!m_AxisSetting.IsAutoMaxY2)
                    m_zCnt.GraphPane.Y2Axis.Scale.Max = m_AxisSetting.Y2Max;
                if (!m_AxisSetting.IsAutoMinY2)
                    m_zCnt.GraphPane.Y2Axis.Scale.Min = m_AxisSetting.Y2Min;

                if (m_AxisSetting.IsAutoMaxY || m_AxisSetting.IsAutoMinY ||
                    m_AxisSetting.IsAutoMaxY2 || m_AxisSetting.IsAutoMinY2)
                    m_zCnt.AxisChange();
                m_zCnt.Refresh();
            }
        }

        #region Event
        /// <summary>
        /// Process when user delete the logger.
        /// </summary>
        /// <param name="tag">the tag of logger entry.</param>
        public void DeleteTraceEntry(TagData tag)
        {
            EcellObject m_currentObj = m_owner.DataManager.GetEcellObject(tag.M_modelID, tag.M_key, tag.Type);
            // for load data.
            if (m_currentObj == null)
            {
                RemoveLoggerEntry(tag);
                m_tagDic.Remove(tag.ToShortString());
                if (m_logList.Contains(tag))
                    m_logList.Remove(tag);
                return;
            }

            foreach (EcellData d in m_currentObj.Value)
            {
                if (d.EntityPath == tag.M_path)
                {
                    d.Logged = false;
                }
            }

            m_owner.DataManager.DataChanged(m_currentObj.ModelID,
                m_currentObj.Key,
                m_currentObj.Type,
                m_currentObj);
        }


        /// <summary>
        /// Build the context menu strip.
        /// </summary>
        /// <param name="sender">Control object.</param>
        /// <param name="menuStrip">the context menu strip</param>
        /// <param name="mousePt">point of mouse.</param>
        /// <param name="objState">object status</param>
        private void ZedControlContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            // ZedGraphでContextMenuを表示するたびに作り直しているので、
            // このイベントでも毎回メニューの削除、追加をする必要がある

            foreach (ToolStripMenuItem m in menuStrip.Items)
            {
                if (m.Name.Contains("copy"))
                {
                    menuStrip.Items.Remove(m);
                    break;
                }
            }
            foreach (ToolStripMenuItem m in menuStrip.Items)
            {
                if (m.Name.Contains("save_as"))
                {
                    menuStrip.Items.Remove(m);
                    break;
                }
            }
            foreach (ToolStripMenuItem m in menuStrip.Items)
            {
                if (m.Name.Contains("set_default"))
                {
                    menuStrip.Items.Remove(m);
                    break;
                }
            }

            ToolStripMenuItem p = new ToolStripMenuItem();
            if (m_isLog)
                p.Text = MessageResources.MenuItemLinear;
            else
                p.Text = MessageResources.MenuItemLog;
            p.Name = "set_log";
            p.Click += new EventHandler(SetLogAxis);
            menuStrip.Items.Add(p);


            ToolStripMenuItem p2 = new ToolStripMenuItem();
            p2.Text = MessageResources.MenuItemYAxis;
            p2.Name = "set_yaxis";
            p2.Click += new EventHandler(SetAxisSize);
            menuStrip.Items.Add(p2);
        }

        /// <summary>
        /// Click the menu to set the log axis or linear axis.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void SetLogAxis(object sender, EventArgs e)
        {
            m_isLog = !m_isLog;
            if (m_isLog)
            {
                m_zCnt.GraphPane.YAxis.Type = AxisType.Log;
                foreach (string key in m_entryDic.Keys)
                {
                    m_entryDic[key].CurrentLineItem.Line.IsSmooth = false;
                    m_entryDic[key].TmpLineItem.Line.IsSmooth = false;
                }
            }
            else
                m_zCnt.GraphPane.YAxis.Type = AxisType.Linear;
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Click the menu to set Y axis.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void SetAxisSize(object sender, EventArgs e)
        {
            YAxisSettings settings = new YAxisSettings(
                m_zCnt.GraphPane.YAxis.Scale.MaxAuto,
                m_zCnt.GraphPane.YAxis.Scale.MinAuto,
                m_zCnt.GraphPane.Y2Axis.Scale.MaxAuto,
                m_zCnt.GraphPane.Y2Axis.Scale.MinAuto,
                m_zCnt.GraphPane.YAxis.Scale.Max,
                m_zCnt.GraphPane.YAxis.Scale.Min,
                m_zCnt.GraphPane.Y2Axis.Scale.Max,
                m_zCnt.GraphPane.Y2Axis.Scale.Min);
            SetGraphSizeDialog dlg = new SetGraphSizeDialog(
                m_isDefault, settings);
            using (dlg)
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_isDefault = dlg.IsDefault;
                    if (m_isDefault)
                    {
                        m_AxisSetting = m_owner.Settings;
                    }
                    else
                    {
                        m_AxisSetting = dlg.Settings;
                    }
                    ResetWindow();
                }
            }
            
        }

        /// <summary>
        /// Process when user click close button on Window.
        /// </summary>
        /// <param name="m">Message</param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && m.WParam.ToInt32() == SC_CLOSE)
            {
                if (Util.ShowOKCancelDialog(MessageResources.ConfirmClose))
                {
                    this.Dispose();
                }
                return;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// The event of zoom on ZedGraphControl.
        /// </summary>
        /// <param name="control">ZedGraphControl.</param>
        /// <param name="oldState">Old zoom state.</param>
        /// <param name="newState">New zoom state.</param>
        void ZcntZoomEvent(ZedGraphControl control, ZoomState oldState, ZoomState newState)
        {
            if (m_current == 0.0) return;
            double sx = m_zCnt.GraphPane.XAxis.Scale.Min;
            double ex = m_zCnt.GraphPane.XAxis.Scale.Max;
            double m_step = (ex - sx) / TracerWindow.s_count;
            List<LogData> list = m_owner.DataManager.GetLogData(sx, ex, m_step);

            if (list == null)
            {
                m_zCnt.ZoomOut(m_zCnt.GraphPane);
                return;
            }
            foreach (LogData l in list)
            {
                if (l.logValueList.Count <= 3)
                {
                    m_zCnt.ZoomOut(m_zCnt.GraphPane);
                    return;
                }
            }
                         
            foreach (string key in m_entryDic.Keys)
            {
                if (m_entryDic[key].IsLoaded) continue;
                m_entryDic[key].ClearPoint();
            }

            if (!m_zCnt.GraphPane.IsZoomed)
            {
                double nextTime = m_owner.DataManager.GetCurrentSimulationTime();
                if (nextTime > ex)
                {
                    m_zCnt.GraphPane.XAxis.Scale.Max = nextTime * 1.5;
                    ex = m_zCnt.GraphPane.XAxis.Scale.Max;
  
                    m_step = (ex - sx) / TracerWindow.s_count;
                    list = m_owner.DataManager.GetLogData(sx, ex, m_step);
                }
            }
            if (m_zCnt.GraphPane.IsZoomed)
            {
                m_zCnt.GraphPane.YAxis.Scale.MaxAuto = false;
                foreach (string key in m_entryDic.Keys)
                {
                    m_entryDic[key].CurrentLineItem.Line.IsSmooth = false;
                    m_entryDic[key].TmpLineItem.Line.IsSmooth = false;
                }
            }
            else
            {
                m_zCnt.GraphPane.YAxis.Scale.MaxAuto = true;
                m_zCnt.GraphPane.YAxis.Scale.MinAuto = true;
            }

            foreach (LogData l in list)
            {
                string p = l.type + ":" + l.key + ":" + l.propName + ":";
                if (!m_entryDic.ContainsKey(p)) continue;

                m_entryDic[p].AddPoint(l.logValueList, 
                    m_zCnt.GraphPane.XAxis.Scale.Max,
                    m_zCnt.GraphPane.XAxis.Scale.Min,
                    0.0, 0.0,
                    m_zCnt.GraphPane.IsZoomed);
            }

            UpdateGraph(true);
            //UpdateGraphCallBack f = new UpdateGraphCallBack(UpdateGraph);
            //this.Invoke(f, new object[] { isAxis });
            list.Clear();
            list = null;            
        }
        #endregion
    }

    /// <summary>
    /// The Y axis setting object.
    /// </summary>
    public class YAxisSettings
    {
        #region Fields
        /// <summary>
        /// The flag whether max of Y is auto.
        /// </summary>
        private bool m_isAutoMaxY = true;
        /// <summary>
        /// The flag whether min of Y is auto.
        /// </summary>
        private bool m_isAutoMinY = true;
        /// <summary>
        /// The flag whether max of Y2 is auto.
        /// </summary>
        private bool m_isAutoMaxY2 = true;
        /// <summary>
        /// The flag whether min of Y2 is auto.
        /// </summary>
        private bool m_isAutoMinY2 = true;
        /// <summary>
        /// Max of Y axis.
        /// </summary>
        private double m_yMax = 10.0;
        /// <summary>
        /// Min of Y axis.
        /// </summary>
        private double m_yMin = 0.0;
        /// <summary>
        /// Max of Y2 axis.
        /// </summary>
        private double m_y2Max = 10.0;
        /// <summary>
        /// Min of Y2 axis.
        /// </summary>
        private double m_y2Min = 0.0;
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the flag whether max of Y is auto.
        /// </summary>
        public bool IsAutoMaxY
        {
            get { return m_isAutoMaxY; }
            set { m_isAutoMaxY = value; }
        }

        /// <summary>
        /// get / set the flag whether min of Y is auto.
        /// </summary>
        public bool IsAutoMinY
        {
            get { return m_isAutoMinY; }
            set { m_isAutoMinY = value; }
        }

        /// <summary>
        /// get / set the flag whether max of Y2 is auto.
        /// </summary>
        public bool IsAutoMaxY2
        {
            get { return m_isAutoMaxY2; }
            set { m_isAutoMaxY2 = value; }
        }

        /// <summary>
        /// get / set the flag whether min of Y2 is auto.
        /// </summary>
        public bool IsAutoMinY2
        {
            get { return m_isAutoMinY2; }
            set { m_isAutoMinY2 = value; }
        }

        /// <summary>
        /// get / set Max of Y axis.
        /// </summary>
        public double YMax
        {
            get { return m_yMax; }
            set { m_yMax = value; }
        }

        /// <summary>
        /// get / set Min of Y axis.
        /// </summary>
        public double YMin
        {
            get { return m_yMin; }
            set { m_yMin = value; }
        }

        /// <summary>
        /// get / set Max of Y2 axis.
        /// </summary>
        public double Y2Max
        {
            get { return m_y2Max; }
            set { m_y2Max = value; }
        }

        /// <summary>
        /// get / set Min of Y2 axis.
        /// </summary>
        public double Y2Min
        {
            get { return m_y2Min; }
            set { m_y2Min = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        public YAxisSettings()
        {
        }

        /// <summary>
        /// Constructors with the initial parametesr.
        /// </summary>
        /// <param name="isAutoMaxY">The flag whether max of Y is auto</param>
        /// <param name="isAutoMinY">The flag whether min of Y is auto</param>
        /// <param name="isAutoMaxY2">The flag whether max of Y2 is auto</param>
        /// <param name="isAutoMinY2">The flag whether min of Y2 is auto</param>
        /// <param name="yMax">Max of Y axis.</param>
        /// <param name="yMin">Min of Y axis.</param>
        /// <param name="y2Max">Max of Y2 axis.</param>
        /// <param name="y2Min">Min of Y2 axis.</param>
        public YAxisSettings(bool isAutoMaxY, bool isAutoMinY, bool isAutoMaxY2, bool isAutoMinY2,
            double yMax, double yMin, double y2Max, double y2Min)
        {
            m_isAutoMaxY = isAutoMaxY;
            m_isAutoMinY = isAutoMinY;
            m_isAutoMaxY2 = isAutoMaxY2;
            m_isAutoMinY2 = isAutoMinY2;
            m_yMax = yMax;
            m_yMin = yMin;
            m_y2Max = y2Max;
            m_y2Min = y2Min;
        }
        #endregion

        /// <summary>
        /// Copy the setting objects of Y axis.
        /// </summary>
        /// <returns></returns>
        public YAxisSettings Copy()
        {
            return new YAxisSettings(this.IsAutoMaxY, this.IsAutoMinY, this.IsAutoMaxY2, this.IsAutoMinY2,
                this.YMax, this.YMin, this.Y2Max, this.Y2Min);
        }

    }
}
