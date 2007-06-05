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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using ZedGraph;

namespace EcellLib.TracerWindow
{
    public partial class TraceWindow : Form
    {
        #region Fields
        /// <summary>
        /// The parent form of this form.
        /// </summary>
        public Form m_parent;
        /// <summary>
        /// The flag whether simulation runs.
        /// </summary>
        bool isRun = false;
        /// <summary>
        /// The flag whether simulation suspends.
        /// </summary>
        bool isSuspend = false;
        /// <summary>
        /// TextBox of output directory.
        /// </summary>
        string m_text;
        /// <summary>
        /// Graph control for tracer.
        /// </summary>
        public ZedGraphControl m_zCnt;
        /// <summary>
        /// The last time on drawing tracer.
        /// </summary>
        public double m_current;
        /// <summary>
        /// The dictionary of entity path and LineItem.
        /// </summary>
        public Dictionary<string, LineItem> m_paneDic;
        public Dictionary<string, LineItem> m_tmpPaneDic;
        /// <summary>
        /// The List of entity path on tracer.
        /// </summary>
        public List<TagData> m_entry;
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
        public TraceWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// add logger entry to DataGridView when this window is shown.
        /// </summary>
        /// <param name="m_entry">logger entry</param>
        public void InitializeWindow(List<TagData> m_entry)
        {
            foreach (TagData t in m_entry) AddLoggerEntry(t);
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
        /// Display directory selection dialog.
        /// </summary>
        void ShowSelectDialog()
        {
            if (DialogResult.Cancel == m_folderDialog.ShowDialog())
            {
                m_text = "";
            }
            else
            {
                m_text = m_folderDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Add logger entry to DataGridView and ZedGraphControl.
        /// Added logger entry is registed to m_paneDic.
        /// </summary>
        /// <param name="tag">logger entry</param>
        public void AddLoggerEntry(TagData tag)
        {
            int ind = dgv.Rows.Count;
            Bitmap b = new Bitmap(20, 20);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(ColorCreator.GetColorBlush(ind), 3, 3, 14, 14);
            g.ReleaseHdc(g.GetHdc());

            DataGridViewRow r = new DataGridViewRow();
            DataGridViewCheckBoxCell c1 = new DataGridViewCheckBoxCell();
            c1.Value = true;
            r.Cells.Add(c1);
            DataGridViewImageCell c2 = new DataGridViewImageCell();
            c2.Value = b;
            r.Cells.Add(c2);
            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = tag.M_path;
            r.Cells.Add(c3);
            r.Tag = new TagData(tag.M_modelID, tag.M_key, tag.M_type, tag.M_path);
            dgv.Rows.Add(r);
            c3.ReadOnly = true;

            LineItem i = m_zCnt.GraphPane.AddCurve(tag.M_path,
                    new PointPairList(), ColorCreator.GetColor(ind), SymbolType.None);
            i.Line.Width = 2;
            LineItem i1 = m_zCnt.GraphPane.AddCurve(tag.M_path,
                    new PointPairList(), ColorCreator.GetColor(ind), SymbolType.None);
            i1.Line.Width = 2;
            m_paneDic.Add(tag.M_path, i);
            m_tmpPaneDic.Add(tag.M_path, i1);
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
                    t.M_type == tag.M_type && t.M_path == tag.M_path)
                {
                    dgv.Rows.Remove(r);
                    break;
                }
            }
            if (m_paneDic.ContainsKey(tag.M_path))
            {
                m_paneDic[tag.M_path].Clear();
                m_zCnt.GraphPane.CurveList.Remove(m_paneDic[tag.M_path]);
                m_paneDic.Remove(tag.M_path);

                m_tmpPaneDic[tag.M_path].Clear();
                m_zCnt.GraphPane.CurveList.Remove(m_tmpPaneDic[tag.M_path]);
                m_tmpPaneDic.Remove(tag.M_path);
            }
        }

        /// <summary>
        /// Call this function, when simulation start.
        /// </summary>
        public void StartSimulation()
        {
            if (!isSuspend)
            {
                foreach (string key in m_paneDic.Keys)
                {
                    m_paneDic[key].Clear();
                    m_tmpPaneDic[key].Clear();
                }
                m_current = 0.0;
            }
            m_zCnt.IsShowContextMenu = false;
            if (this.InvokeRequired)
            {
                ChangeStatusCallBack f = new ChangeStatusCallBack(ChangeStatus);
                this.Invoke(f, new object[] { false });
            }
            else
            {
                searchDirButton.Enabled = false;
                updateButton.Enabled = false;
            }
            isRun = true;
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
                if (isSuspend == false)
                    m_zCnt.GraphPane.XAxis.Max = 10.0;
                m_zCnt.AxisChange();
                m_zCnt.Refresh();
            }
            searchDirButton.Enabled = status;
            updateButton.Enabled = status;
        }

        /// <summary>
        /// Call this function, when simulation stop.
        /// Stop the timer.
        /// </summary>
        public void StopSimulation()
        {
            if (this.InvokeRequired)
            {
                ChangeStatusCallBack f = new ChangeStatusCallBack(ChangeStatus);
                this.Invoke(f, new object[] { true });
            }
            else
            {
                searchDirButton.Enabled = true;
                updateButton.Enabled = true;
            }
            m_zCnt.IsShowContextMenu = true;
            isRun = false;
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
            else
            {
                searchDirButton.Enabled = true;
                updateButton.Enabled = true;
            }
            isRun = false;
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
                m_zCnt.Refresh();
            }
            else
            {
                Graphics g = m_zCnt.CreateGraphics();
                g.ResetClip();
                g.SetClip(m_zCnt.MasterPane.PaneRect);
                foreach (string key in m_tmpPaneDic.Keys)
                {
                    m_tmpPaneDic[key].Draw(g, m_zCnt.GraphPane, 0, 1.5F);
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
        public void AddPoints(double maxAxis, double nextTime, List<LogData> data)
        {
            bool isAxis = false;
            
            if (m_zCnt.GraphPane.IsZoomed)
            {
                if (m_current > m_zCnt.GraphPane.XAxis.Max ||
                    nextTime < m_zCnt.GraphPane.XAxis.Min)
                {
                    m_current = nextTime;
                    return;
                }
            }
            else
            {
                if (nextTime > m_zCnt.GraphPane.XAxis.Max)
                {
                    if (nextTime > m_zCnt.GraphPane.XAxis.Max * 1.3)
                    {
                        m_zCnt.GraphPane.XAxis.Max = maxAxis;
                        foreach (string key in m_paneDic.Keys)
                        {
                            m_paneDic[key].Clear();
                            m_tmpPaneDic[key].Clear();
                        }
                    }
                    else
                    {
                        m_zCnt.GraphPane.XAxis.Max = maxAxis;
                        foreach (string key in m_paneDic.Keys)
                        {
                            for (int j = 0; j < m_tmpPaneDic[key].Points.Count; j++)
                            {
                                m_paneDic[key].AddPoint(m_tmpPaneDic[key].Points[j]);
                            }
                            m_tmpPaneDic[key].Clear();

                            if (m_paneDic[key].Points.Count > 20000)
                            {
                                int i = 1;
                                while (i < m_paneDic[key].Points.Count)
                                {
                                    m_paneDic[key].RemovePoint(i);
                                    i = i + 2;
                                }
                            }
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
                if (!m_tmpPaneDic.ContainsKey(p)) continue;

                foreach (LogValue v in d.logValueList)
                {
                    if (isAxis == false)
                    {
                        if (m_zCnt.GraphPane.YAxis.Max < v.value) isAxis = true;
                        if (m_zCnt.GraphPane.YAxis.Min > v.value) isAxis = true;
                    }
                    m_tmpPaneDic[p].AddPoint(v.time, v.value);
                }
            }
            if (m_zCnt.GraphPane.IsZoomed) isAxis = false;
            
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
            DataManager manager = DataManager.GetDataManager();
            manager.SaveSimulationResult(dirName, start, end, fileType, fullID);
        }

        #region Event

        /// <summary>
        /// The action of displaying this window.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        public void ShownEvent(object sender, EventArgs e)
        {
            m_zCnt = new ZedGraphControl();
            m_zCnt.Dock = DockStyle.Fill;
            m_zCnt.GraphPane.Title = "";
            m_zCnt.GraphPane.XAxis.Title = "Time";
            m_zCnt.GraphPane.YAxis.Title = "Value";
            m_zCnt.GraphPane.Legend.IsVisible = false;
            m_zCnt.GraphPane.XAxis.Max = 100;
            m_zCnt.GraphPane.XAxis.Min = 0;
            m_zCnt.ContextMenu.Popup += new EventHandler(ContextMenuPopup);
            m_zCnt.ZoomEvent += new ZedGraphControl.ZoomEventHandler(ZcntZoomEvent);
            dgv.CellDoubleClick += new DataGridViewCellEventHandler(CellDoubleClicked);
            dgv.CurrentCellDirtyStateChanged += new EventHandler(CurrentCellDirtyStateChanged);
            dgv.CellValueChanged += new DataGridViewCellEventHandler(CellValueChanged);
            dgv.Columns[0].Width = 40;
            dgv.Columns[1].Width = 40;

            tableLayoutPanel1.Controls.Add(m_zCnt, 0, 0);
            m_paneDic = new Dictionary<string, LineItem>();
            m_tmpPaneDic = new Dictionary<string, LineItem>();
            m_zCnt.AxisChange();
            m_zCnt.Refresh();

            Thread.CurrentThread.IsBackground = true;

            InitializeWindow(m_entry);
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
            DialogResult r = m_colorDialog.ShowDialog();
            if (r == DialogResult.OK)
            {
                Bitmap b = new Bitmap(20, 20);
                Graphics g = Graphics.FromImage(b);
                Pen p = new Pen(m_colorDialog.Color);
                g.FillRectangle(p.Brush, 3, 3, 14, 14);
                g.ReleaseHdc(g.GetHdc());

                c.Value = b;

                m_paneDic[t.M_path].Color = m_colorDialog.Color;
                m_tmpPaneDic[t.M_path].Color = m_colorDialog.Color;
                m_zCnt.Refresh();
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
            if ((bool)cell.Value == true)
            {
                m_paneDic[t.M_path].IsVisible = true;
                m_tmpPaneDic[t.M_path].IsVisible = true;
            }
            else
            {
                m_paneDic[t.M_path].IsVisible = false;
                m_tmpPaneDic[t.M_path].IsVisible = false;
            }
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
        /// the event of popup the context menu.
        /// Because [Save Image As ...] menu don't work correctly on threading process,
        /// this menu item is invisible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContextMenuPopup(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                EventHandlerCallBack f = new EventHandlerCallBack(ContextPopup);
                this.Invoke(f);
            }
            else
            {
                foreach (MenuItem m in m_zCnt.ContextMenu.MenuItems)
                {
                    if (m.Text.Contains("Save Image As"))
                    {
                        m_zCnt.ContextMenu.MenuItems.Remove(m);
                        break;
                    }
                }
                foreach (MenuItem m in m_zCnt.ContextMenu.MenuItems)
                {
                    if (m.Text.Contains("Copy"))
                    {
                        m_zCnt.ContextMenu.MenuItems.Remove(m);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// remove menu item on on threading process.
        /// </summary>
        public void ContextPopup()
        {
            foreach (MenuItem m in m_zCnt.ContextMenu.MenuItems)
            {
                if (m.Text.Contains("Save Image As"))
                {
                    m_zCnt.ContextMenu.MenuItems.Remove(m);
                    break;
                }
            }
        }

        /// <summary>
        /// The action of clicking search button.
        /// This action display DirectorySelectionDialog.
        /// If the process fired this action is threading, you delegate this process.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void SearchDirButtonClick(object sender, EventArgs e)
        {
            if (isRun) return;
            this.Hide();
            if (m_parent.InvokeRequired)
            {
                ShowDialogCallBack f = new ShowDialogCallBack(ShowSelectDialog);
                m_parent.Invoke(f);
            }
            else
            {
                m_folderDialog.ShowDialog();
            }
            dirTextBox.Text = m_text;
            this.Show();
        }

        /// <summary>
        /// The event of clicking close button.
        /// This action close this window.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// The event of zoom on ZedGraphControl.
        /// </summary>
        /// <param name="control">ZedGraphControl.</param>
        /// <param name="oldState">Old zoom state.</param>
        /// <param name="newState">New zoom state.</param>
        void ZcntZoomEvent(ZedGraphControl control, ZoomState oldState, ZoomState newState)
        {
            bool isAxis = false;
            if (m_current == 0.0) return;
            foreach (string key in m_paneDic.Keys)
            {
                m_paneDic[key].Clear();
            }
            double sx = m_zCnt.GraphPane.XAxis.Min;
            double ex = m_zCnt.GraphPane.XAxis.Max;
            double m_step = (ex - sx) / 10000;
            List<LogData> list;
            if (!m_zCnt.GraphPane.IsZoomed)
            {
                double nextTime = DataManager.GetDataManager().GetCurrentSimulationTime();
                if (nextTime > ex)
                {
                    m_zCnt.GraphPane.XAxis.Max = nextTime * 1.5;
                    ex = m_zCnt.GraphPane.XAxis.Max ;
                    m_step = (ex - sx) / 10000;
                    isAxis = true;
                }
            }
            list = DataManager.GetDataManager().GetLogData(sx, ex, m_step);
            foreach (LogData l in list)
            {
                string p = l.type + ":" + l.key + ":" + l.propName;
                if (!m_paneDic.ContainsKey(p)) continue;

                foreach (LogValue v in l.logValueList)
                {
                    m_paneDic[p].AddPoint(v.time, v.value);
                }
            }

            UpdateGraphCallBack f = new UpdateGraphCallBack(UpdateGraph);
            this.Invoke(f, new object[] { isAxis });
            list.Clear();
            list = null;
            
        }

        /// <summary>
        /// The action of clicking save button.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        private void UpdateButtonClick(object sender, EventArgs e)
        {
            double start, end;
            if (isRun) return;
            string saveDir;
            if (dirTextBox.Text == null) saveDir = "";
            else saveDir = dirTextBox.Text;
            DataManager manager = DataManager.GetDataManager();

            List<string> fullList = new List<string>();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                bool check = (bool)row.Cells[0].Value;
                string full = (string)row.Cells[2].Value;

                if (check && full != null)
                {
                    fullList.Add(full);
                }
            }
            if (startText.Text == "" || startText.Text == null) start = 0.0;
            else start = Convert.ToDouble(startText.Text);

            if (endText.Text == "" || endText.Text == null) end = 0.0;
            else end = Convert.ToDouble(endText.Text);

            if (m_parent.InvokeRequired)
            {
                SaveSimulationCallback f = new SaveSimulationCallback(SaveSimulationInvoke);
                m_parent.Invoke(f, new object[] { saveDir, start, end, saveTypeCombo.Text, fullList });
            }
            else
            {
                manager.SaveSimulationResult(dirTextBox.Text, start, end, saveTypeCombo.Text, fullList);
            }
            MessageBox.Show("Simulation log saved successfully.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}