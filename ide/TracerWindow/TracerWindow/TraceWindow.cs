//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Form to show trace of target object property.
    /// </summary>
    public partial class TraceWindow : EcellDockContent
    {
        #region Fields
        private bool m_isLog = false;
        private double m_MaxXAxis = 10.0;
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
        private int m_entryCount = 0;
        /// <summary>
        /// The List of entity path on tracer.
        /// </summary>
        public List<TagData> m_entry;
        private List<TagData> m_logList = new List<TagData>();
        private Dictionary<string, bool> m_tagDic = new Dictionary<string, bool>();
        private Dictionary<string, TraceEntry> m_entryDic = new Dictionary<string, TraceEntry>();
        /// <summary>
        /// The delegate for event handler function.
        /// </summary>
        delegate void EventHandlerCallBack();
        /// <summary>
        /// The delegate for showing dialog.
        /// </summary>
        delegate void ShowDialogCallBack();
        /// <summary>
        /// The delegate for updating the graph window.
        /// </summary>
        /// <param name="isAxis"></param>
        delegate void UpdateGraphCallBack(bool isAxis);
        /// <summary>
        /// The delegate for changing this aplication status.
        /// </summary>
        /// <param name="status">system status.</param>
        delegate void ChangeStatusCallBack(bool status);
        /// <summary>
        /// The delegate for saving the simulation results.
        /// </summary>
        /// <param name="dirName">set the directory to save the data.</param>
        /// <param name="start">start time.</param>
        /// <param name="end">end time.</param>
        /// <param name="fileType">file format.</param>
        /// <param name="fullID">list of ids for saving.</param>
        delegate void SaveSimulationCallback(string dirName, double start, double end,
            string fileType, List<string> fullID);
        #endregion

        /// <summary>
        /// Constructor for TraceWindow.
        /// </summary>
        public TraceWindow(TracerWindow control)
        {
            m_owner = control;
            m_isSavable = false;
            InitializeComponent();
            dgv.DragEnter += new DragEventHandler(dgv_DragEnter);
            dgv.DragDrop += new DragEventHandler(dgv_DragDrop);

            ContextMenuStrip cStrip = new ContextMenuStrip();
            ToolStripMenuItem it1 = new ToolStripMenuItem();
            it1.Text = MessageResources.MenuItemImportData;
            it1.Click += new EventHandler(ImportDataItem);

            ToolStripMenuItem it2 = new ToolStripMenuItem();
            it2.Text = MessageResources.MenuItemShowTraceSetupText;
            it2.Click += new EventHandler(ShowSetupWindow);

            cStrip.Items.AddRange(new ToolStripItem[] { it1, it2 });
            dgv.ContextMenuStrip = cStrip;

            m_zCnt = new ZedGraphControl();
            m_zCnt.Dock = DockStyle.Fill;
            m_zCnt.GraphPane.Title.Text = "";
            m_zCnt.GraphPane.XAxis.Title.Text = "Time(sec)";
            m_zCnt.GraphPane.YAxis.Title.IsVisible = false;
            m_zCnt.GraphPane.Legend.IsVisible = false;
            m_zCnt.GraphPane.YAxis.Scale.Format = "G";
            m_zCnt.GraphPane.XAxis.Scale.Max = 100;
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = false;
            m_zCnt.GraphPane.XAxis.Scale.Min = 0;
            m_zCnt.ZoomEvent += new ZedGraphControl.ZoomEventHandler(ZcntZoomEvent);
            m_zCnt.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(ZedControlContextMenuBuilder);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(CellDoubleClicked);
            dgv.CurrentCellDirtyStateChanged += new EventHandler(CurrentCellDirtyStateChanged);
            dgv.CellValueChanged += new DataGridViewCellEventHandler(CellValueChanged);
            dgv.Columns[0].Width = 40;
            dgv.Columns[1].Width = 40;
            m_zCnt.GraphPane.Margin.Top = 35.0f;
            m_zCnt.GraphPane.YAxis.MajorGrid.IsVisible = true;
            m_zCnt.GraphPane.XAxis.MinorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.XAxis.MajorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MinorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MajorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.Chart.Border.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MajorGrid.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.Fill = new Fill(Color.White, Color.LightGray, 90.0f);

            tableLayoutPanel1.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        public string DataFormat
        {
            set { this.m_zCnt.PointValueFormat = value; }
        }

        void dgv_DragEnter(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void dgv_DragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            TraceWindow tWin = m_owner.CurrentWin;
            m_owner.CurrentWin = this;
            foreach (EcellDragEntry ent in dobj.Entries)
            {
                m_owner.PluginManager.LoggerAdd(dobj.ModelID, ent.Key, ent.Type, ent.Path);
                EcellObject t = m_owner.DataManager.GetEcellObject(dobj.ModelID, ent.Key, ent.Type);
                foreach (EcellData d in t.Value)
                {
                    if (d.EntityPath.Equals(ent.Path))
                    {
                        d.Logged = true;
                        break;
                    }
                }
                m_owner.DataManager.DataChanged(t.ModelID, t.Key, t.Type, t);

            }

            foreach (string fileName in dobj.LogList)
            {
                ImportLog(fileName);
            }
            m_owner.CurrentWin = tWin;
        }

        /// <summary>
        /// add logger entry to DataGridView when this window is shown.
        /// </summary>
        /// <param name="m_entry">logger entry</param>
        public void InitializeWindow(List<TagData> m_entry)
        {
            foreach (TagData t in m_entry) AddLoggerEntry(t);
            m_entry.Clear();
        }

        /// <summary>
        /// Commit the change of cell.
        /// </summary>
        public void StateChanged()
        {
            if (dgv.IsCurrentCellDirty)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="isCont"></param>
        public void SetIsContinuous(string tag, bool isCont)
        {
            if (m_tagDic.ContainsKey(tag))
            {
                m_tagDic[tag] = isCont;
            }
        }

        public void ImportLog(string fileName)
        {
            LogData log = m_owner.DataManager.LoadSimulationResult(fileName);
            string[] ele = log.propName.Split(new char[] { ':' });
            string propName = "Log:" + log.key + ":" + ele[ele.Length - 1];
            TagData tag = new TagData(log.model, log.key, Constants.xpathLog, propName, true);
            List<LogData> logList = new List<LogData>();
            tag.isLoaded = true;
            if (m_logList.Contains(tag))
                return;
            AddLoggerEntry(tag);
            m_logList.Add(tag);

            LogData newLog = new LogData(log.model, log.key, Constants.xpathLog, ele[ele.Length - 1], log.logValueList);
            newLog.IsLoaded = true;
            logList.Add(newLog);
            AddPoints(log.logValueList[log.logValueList.Count - 1].time, log.logValueList[log.logValueList.Count - 1].time, logList);
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
            m_entry.Clear();
            m_entryDic.Clear();
            m_logList.Clear();
        }

        /// <summary>
        /// Add logger entry to DataGridView and ZedGraphControl.
        /// Added logger entry is registed to m_paneDic.
        /// </summary>
        /// <param name="tag">logger entry</param>
        public void AddLoggerEntry(TagData tag)
        {
            int ind = m_entryCount;
            Bitmap b = new Bitmap(20, 20);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(Util.GetColorBlush(ind), 3, 3, 14, 14);
            g.ReleaseHdc(g.GetHdc());

            Bitmap b1 = new Bitmap(20, 20);
            Graphics g1 = Graphics.FromImage(b1);
            Pen pen = new Pen(Util.GetColorBlush(ind), 2);
            pen.DashStyle = LineCreator.GetLine(ind);
            g1.DrawLine(pen, 0, 10, 20, 10);
            g1.ReleaseHdc(g1.GetHdc());            

            DataGridViewRow r = new DataGridViewRow();
            DataGridViewCheckBoxCell c1 = new DataGridViewCheckBoxCell();
            c1.Value = true;
            r.Cells.Add(c1);
            DataGridViewImageCell c2 = new DataGridViewImageCell();
            c2.Value = b;
            r.Cells.Add(c2);
            DataGridViewImageCell c3 = new DataGridViewImageCell();
            c3.Value = b1;
            r.Cells.Add(c3);
            DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
            c4.Value = tag.M_path;
            r.Cells.Add(c4);
            r.Tag = new TagData(tag.M_modelID, tag.M_key, tag.Type, tag.M_path, tag.IsContinue);

            ContextMenuStrip contextStrip = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = MessageResources.MenuItemDeleteText;
            it.ShortcutKeys = Keys.Control | Keys.D;
            it.Click += new EventHandler(DeleteTraceItem);
            it.Tag = r;
            contextStrip.Items.AddRange(new ToolStripItem[] { it });
            r.ContextMenuStrip = contextStrip;
            dgv.Rows.Add(r);
            c4.ReadOnly = true;

            LineItem i = m_zCnt.GraphPane.AddCurve(tag.M_path,
                    new PointPairList(), Util.GetColor(ind), SymbolType.None);
            i.Line.Width = 1;
            i.Line.Style = LineCreator.GetLine(ind);
            LineItem i1 = m_zCnt.GraphPane.AddCurve(tag.M_path,
                    new PointPairList(), Util.GetColor(ind), SymbolType.None);
            i1.Line.Width = 1;
            i1.Line.Style = LineCreator.GetLine(ind);
            m_entryDic.Add(tag.M_path, new TraceEntry(tag.M_path, i, i1, tag.IsContinue, tag.isLoaded));
            m_tagDic.Add(tag.M_path, tag.IsContinue);
            m_entryCount++;
        }

        /// <summary>
        /// Change the entry id of logger.
        /// </summary>
        /// <param name="org">the original entry data.</param>
        /// <param name="key">the new entry id.</param>
        /// <param name="path">the new entry path.</param>
        public void ChangeLoggerEntry(TagData org, string key, string path)
        {
            foreach (DataGridViewRow r in dgv.Rows)
            {
                if (!r.Cells[3].Value.ToString().Equals(org.M_path)) continue;
                r.Cells[3].Value = path;

                r.Tag = new TagData(org.M_modelID, key, org.Type, path, org.IsContinue);
                break;
            }

            foreach (string entPath in m_entryDic.Keys)
            {
                if (!entPath.Equals(org.M_path)) continue;
                TraceEntry p = m_entryDic[entPath];
                m_entryDic.Add(path, new TraceEntry(path, p.CurrentLineItem, p.TmpLineItem, p.IsContinuous, p.IsLoaded));
                m_entryDic.Remove(entPath);
                break;
            }

            foreach (string entPath in m_tagDic.Keys)
            {
                if (!entPath.Equals(org.M_path)) continue;
                m_tagDic.Add(path, org.IsContinue);
                m_tagDic.Remove(entPath);
                break;
            }
        }

        /// <summary>
        /// Remove logger entry from DataGridView,
        /// </summary>
        /// <param name="tag">logger entry.</param>
        public void RemoveLoggerEntry(TagData tag)
        {
            foreach (DataGridViewRow r in dgv.Rows)
            {
                TagData t = (TagData)r.Tag;
                if (t.M_modelID == tag.M_modelID && t.M_key == tag.M_key &&
                    t.Type == tag.Type && t.M_path == tag.M_path)
                {
                    dgv.Rows.Remove(r);
                    break;
                }
            }
            if (m_entryDic.ContainsKey(tag.M_path))
            {
                string path = tag.M_path;
                m_entryDic[path].ClearPoint();
                m_zCnt.GraphPane.CurveList.Remove(m_entryDic[path].CurrentLineItem);
                m_zCnt.GraphPane.CurveList.Remove(m_entryDic[path].TmpLineItem);
                m_entryDic.Remove(path);

                UpdateGraphCallBack dlg = new UpdateGraphCallBack(UpdateGraph);
                this.Invoke(dlg, new object[] { true });
            }
            if (m_tagDic.ContainsKey(tag.M_path))
                m_tagDic.Remove(tag.M_path);
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
            if (!m_zCnt.GraphPane.IsZoomed)
                m_zCnt.GraphPane.XAxis.Scale.Max = m_MaxXAxis;
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Call this function, when simulation start.
        /// </summary>
        public void StartSimulation()
        {
            Console.WriteLine(isSuspend);
            if (!isSuspend || m_zCnt.GraphPane.IsZoomed)
            {
                foreach (string key in m_entryDic.Keys)
                {
                    if (m_entryDic[key].IsLoaded) continue;
                    m_entryDic[key].ClearPoint();
                }
                m_current = 0.0;
            }
            m_zCnt.IsShowContextMenu = false;
            double max = 0.0;
            double plotCount = TracerWindow.s_count;
            List<EcellObject> stepperList = m_owner.DataManager.GetStepper(m_owner.DataManager.GetCurrentSimulationParameterID(),
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
//            m_MaxXAxis = max;

            if (this.InvokeRequired)
            {
                ChangeStatusCallBack f = new ChangeStatusCallBack(ChangeStatus);
                this.Invoke(f, new object[] { false });
            }
            else
            {
                if (!isSuspend)
                {
                    m_zCnt.GraphPane.XAxis.Scale.Max = m_MaxXAxis;
                    m_zCnt.AxisChange();
                    m_zCnt.Refresh();
                }
            }
            isSuspend = false;
        }


        /// <summary>
        /// Call this function, when status of system is changed.
        /// </summary>
        /// <param name="status">system status.</param>
        public void ChangeStatus(bool status)
        {
            if (status == false)
            {
                if (isSuspend == false && !m_zCnt.GraphPane.IsZoomed)
                    m_zCnt.GraphPane.XAxis.Scale.Max = m_MaxXAxis;
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
            //if (this.InvokeRequired)
            //{
            //    ChangeStatusCallBack f = new ChangeStatusCallBack(ChangeStatus);
            //    this.Invoke(f, new object[] { true });
            //}
            ChangeStatus(true);
            m_zCnt.IsShowContextMenu = true;
            isSuspend = false;
        }

        /// <summary>
        /// Call this function, when simulation suspend.
        /// Stop the timer.
        /// </summary>
        public void SuspendSimulation()
        {
            m_zCnt.IsShowContextMenu = true;
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
            if (isAxis)
            {
                m_zCnt.AxisChange();
                if (m_zCnt.GraphPane.YAxis.Scale.Min > 0)
                    m_zCnt.GraphPane.YAxis.Scale.Min = 0;
                if (m_zCnt.GraphPane.YAxis.Scale.Max > 1000)
                    m_zCnt.GraphPane.YAxis.Scale.Format = "e1";
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
        public void AddPoints(double maxAxis, double nextTime, IEnumerable<LogData> data)
        {
            bool isAxis = false;

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
                string p = d.type + ":" + d.key + ":" + d.propName;
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
                    if (m_entryDic[key].CurrentLineItem.Line.IsSmooth) continue;
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
        /// The invoke of saving the simulation results to files.
        /// </summary>
        /// <param name="dirName">set the directory to save the data.</param>
        /// <param name="start">start time.</param>
        /// <param name="end">emd time.</param>
        /// <param name="fileType">file format.</param>
        /// <param name="fullID">list of ids.</param>
        public void SaveSimulationInvoke(string dirName, double start, double end,
            string fileType, List<string> fullID)
        {
            m_owner.DataManager.SaveSimulationResult(dirName, start, end, fileType, fullID);
        }

        #region Event

        /// <summary>
        /// The action of displaying this window.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        public void ShownEvent(object sender, EventArgs e)
        {
            Thread.CurrentThread.IsBackground = true;

            InitializeWindow(m_entry);
        }

        /// <summary>
        /// Display the dialog to set the style of line.
        /// </summary>
        /// <param name="t">TagData of selected row.</param>
        /// <param name="rowIndex">row index of selected cell.</param>
        /// <param name="columnIndex">column index of selected cell.</param>
        void ShowLineStyleDialog(TagData t, int rowIndex, int columnIndex)
        {
            LineStyleDialog dialog = 
                new LineStyleDialog(m_entryDic[t.M_path].CurrentLineItem.Line.Style);

            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Drawing2D.DashStyle style = dialog.GetLineStyle();
                    if (style == System.Drawing.Drawing2D.DashStyle.Custom) return;
                    DataGridViewImageCell cell = dgv.Rows[rowIndex].Cells[columnIndex] as DataGridViewImageCell;

                    Bitmap b1 = new Bitmap(20, 20);
                    Graphics g1 = Graphics.FromImage(b1);
                    Pen p1 = new Pen(m_entryDic[t.M_path].CurrentLineItem.Color);
                    p1.DashStyle = style;
                    p1.Width = 2;
                    g1.DrawLine(p1, 0, 10, 20, 10);
                    g1.ReleaseHdc(g1.GetHdc());
                    cell.Value = b1;

                    m_entryDic[t.M_path].SetStyle(style);
                    m_zCnt.Refresh();
                }
            }
        }

        /// <summary>
        /// Display the dialog to set color of line.
        /// </summary>
        /// <param name="t">TagData of selected row.</param>
        /// <param name="rowIndex">Row index of selected cell.</param>
        /// <param name="columnIndex">Column index of selected cell.</param>
        void ShowColorSetDialog(TagData t, int rowIndex, int columnIndex)
        {
            DataGridViewImageCell cell = dgv.Rows[rowIndex].Cells[columnIndex] as DataGridViewImageCell;
            DataGridViewImageCell cell1 = dgv.Rows[rowIndex].Cells[columnIndex + 1] as DataGridViewImageCell;
            Debug.Assert(cell != null);
            Debug.Assert(cell1 != null);

            DialogResult r = m_colorDialog.ShowDialog();
            if (r == DialogResult.OK)
            {
                Bitmap b = new Bitmap(20, 20);
                Graphics g = Graphics.FromImage(b);
                Pen p = new Pen(m_colorDialog.Color);
                g.FillRectangle(p.Brush, 3, 3, 14, 14);
                g.ReleaseHdc(g.GetHdc());

                m_entryDic[t.M_path].SetColor(m_colorDialog.Color);

                Bitmap b1 = new Bitmap(20, 20);
                Graphics g1 = Graphics.FromImage(b1);
                Pen p1 = new Pen(m_colorDialog.Color);
                p1.DashStyle = m_entryDic[t.M_path].CurrentLineItem.Line.Style;
                p1.Width = 2;
                g1.DrawLine(p1, 0, 10, 20, 10);
                g1.ReleaseHdc(g1.GetHdc());

                cell.Value = b;
                cell1.Value = b1;

                m_entryDic[t.M_path].SetColor(m_colorDialog.Color);

                m_zCnt.Refresh();
            }
        }

        /// <summary>
        /// The action of double clicking the cell in DataGridView.
        /// Show the color select dialog, change color of line in
        /// ZedGraphControl.t This action is avaiable at DataGridViewImageCell only.
        /// </summary>
        /// <param name="sender">object(DataGridView)</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        void CellDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewImageCell c = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewImageCell;
            if (c == null) return;

            TagData t = (TagData)dgv.Rows[e.RowIndex].Tag;
            if (e.ColumnIndex != 1)
            {
                ShowLineStyleDialog(t, e.RowIndex, e.ColumnIndex);
            }
            else
            {
                ShowColorSetDialog(t, e.RowIndex, e.ColumnIndex);
            }
        }

        /// <summary>
        /// The action of changing the value of cell.
        /// Changing cell is DataGridViewCheckBoxCell only.
        /// </summary>
        /// <param name="sender">object(DataGridView)</param>
        /// <param name="e">DataGridViewCheckBoxCell</param>
        void CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell c = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            DataGridViewCheckBoxCell cell = c as DataGridViewCheckBoxCell;
            if (cell == null) return;

            DataGridViewRow r = dgv.Rows[e.RowIndex];
            TagData t = (TagData)r.Tag;
            m_entryDic[t.M_path].SetVisible((bool)cell.Value);
            m_zCnt.Refresh();
        }

        /// <summary>
        /// The action of changing the value on cell.
        /// Commit the change of cell, fire CellValueChanged Event.
        /// </summary>
        /// <param name="sender">object(DataGirdViewCell)</param>
        /// <param name="e">EventArgs</param>
        void CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandlerCallBack f = new EventHandlerCallBack(StateChanged);
                this.Invoke(f);
            }
            else
            {
                StateChanged();
            }
        }

        /// <summary>
        /// Process when user delete the logger.
        /// </summary>
        /// <param name="tag"></param>
        public void DeleteTraceEntry(TagData tag)
        {
            EcellObject m_currentObj = m_owner.DataManager.GetEcellObject(tag.M_modelID, tag.M_key, tag.Type);
            // for load data.
            if (m_currentObj == null)
            {
                RemoveLoggerEntry(tag);
                m_tagDic.Remove(tag.M_key);
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
        /// The event sequence when the delete menu is clicked. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteTraceItem(object sender, EventArgs e)
        {
            DataGridViewRow r = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
            if (r == null) return;
            TagData tag = r.Tag as TagData;
            if (tag == null) return;

            DeleteTraceEntry(tag);
        }

        /// <summary>
        /// The event sequence when the setup windos is shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSetupWindow(object sender, EventArgs e)
        {
            m_owner.ShowSetupWindow();
        }

        // ZedGraphでContextMenuを表示するたびに作り直しているので、
        // このイベントでも毎回メニューの削除、追加をする必要がある
        private void ZedControlContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
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
        }

        private void SetLogAxis(object sender, EventArgs e)
        {
            m_isLog = !m_isLog;
            if (m_isLog)
                m_zCnt.GraphPane.YAxis.Type = AxisType.Log;
            else
                m_zCnt.GraphPane.YAxis.Type = AxisType.Linear;
            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// The event sequence when the log data is imported.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportDataItem(object sender, EventArgs e)
        {
            DialogResult r = m_openDialog.ShowDialog();
            if (r != DialogResult.OK) return;
            ImportLog(m_openDialog.FileName);
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
            bool isAxis = false;
            if (m_current == 0.0) return;
            foreach (string key in m_entryDic.Keys)
            {
                if (m_entryDic[key].IsLoaded) continue;
                m_entryDic[key].ClearPoint();
            }
            double sx = m_zCnt.GraphPane.XAxis.Scale.Min;
            double ex = m_zCnt.GraphPane.XAxis.Scale.Max;
            double m_step = (ex - sx) / TracerWindow.s_count;
            IEnumerable<LogData> list;
            if (!m_zCnt.GraphPane.IsZoomed)
            {
                double nextTime = m_owner.DataManager.GetCurrentSimulationTime();
                if (nextTime > ex)
                {
                    m_zCnt.GraphPane.XAxis.Scale.Max = nextTime * 1.5;
                    ex = m_zCnt.GraphPane.XAxis.Scale.Max;
  
                    m_step = (ex - sx) / TracerWindow.s_count;
                    isAxis = true;
                }
            }
            list = m_owner.DataManager.GetLogData(sx, ex, m_step);
            if (list == null) return;
            foreach (LogData l in list)
            {
                string p = l.type + ":" + l.key + ":" + l.propName;
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
            list = null;
        }

        #endregion
    }
}
