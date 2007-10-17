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
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using ZedGraph;

namespace EcellLib.Analysis
{
    /// <summary>
    /// Analysis window for robust analysis.
    /// </summary>
    public partial class RobustAnalysis : DockContent
    {
        private Dictionary<string, EcellData> m_observList = new Dictionary<string, EcellData>();
        private Dictionary<string, EcellData> m_paramList = new Dictionary<string, EcellData>();
        private ZedGraphControl m_zCnt = null;
        private Analysis m_parent = null;
        System.ComponentModel.ComponentResourceManager m_resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageResAnalysis));

        /// <summary>
        /// Constructor.
        /// </summary>
        public RobustAnalysis()
        {
            InitializeComponent();

            m_zCnt = new ZedGraphControl();
            m_zCnt.Dock = DockStyle.Fill;
            m_zCnt.GraphPane.Title.Text = "";
            m_zCnt.GraphPane.XAxis.Title.Text = "X";
            m_zCnt.GraphPane.YAxis.Title.Text = "Y";
            m_zCnt.GraphPane.Legend.IsVisible = false;
            m_zCnt.GraphPane.XAxis.Scale.Max = 100;
            m_zCnt.GraphPane.XAxis.Scale.Min = 0;
            m_zCnt.GraphPane.YAxis.Scale.Max = 100;
            m_zCnt.GraphPane.YAxis.Scale.Min = 0;
            RAAnalysisTableLayout.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();

            InitializeData();

            this.FormClosed += new FormClosedEventHandler(RobustAnalysis_FormClosed);
        }

        #region accessor
        /// <summary>
        /// get/set the controller of this window.
        /// </summary>
        public Analysis Control
        {
            get { return this.m_parent; }
            set { this.m_parent = value; }
        }
        #endregion

        /// <summary>
        /// Refresh the value of parameters.
        /// </summary>
        public void InitializeData()
        {
            ClearEntry();
            DataManager manager = DataManager.GetDataManager();

            List<string> mList = manager.GetModelList();
            foreach (string modelName in mList)
            {
                List<EcellObject> oList = manager.GetData(modelName, null);
                foreach (EcellObject sObj in oList)
                {
                    SearchAndAddEntry(sObj);
                    if (sObj.M_instances != null)
                    {
                        foreach (EcellObject obj in sObj.M_instances)
                        {
                            SearchAndAddEntry(obj);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clear the entries.
        /// </summary>
        public void Clear()
        {
            ClearEntry();
            ClearResult();
        }

        /// <summary>
        /// Clear the entries in observed data and parameter data.
        /// </summary>
        private void ClearEntry()
        {
            RAObservGridView.Rows.Clear();
            RAParamGridView.Rows.Clear();

            m_paramList.Clear();
            m_observList.Clear();
        }

        /// <summary>
        /// Clear the entries in result data.
        /// </summary>
        private void ClearResult()
        {
            RAResultGridView.Rows.Clear();
            CurveList l = m_zCnt.GraphPane.CurveList;
            l.Clear();
        }

        private void SearchAndAddEntry(EcellObject obj)
        {
            if (obj.M_value == null) return;
            foreach (EcellData d in obj.M_value)
            {
                if (d.M_isCommit) continue;
                if (m_paramList.ContainsKey(d.M_entityPath)) continue;
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = d.M_entityPath;
                r.Cells.Add(c1);
                DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                c2.Value = d.Max;
                r.Cells.Add(c2);
                DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
                c3.Value = d.Min;
                r.Cells.Add(c3);
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = d.Step;
                r.Cells.Add(c4);
                r.Tag = obj;
                AssignParamPopupMenu(r);
                RAParamGridView.Rows.Add(r);
                m_paramList.Add(d.M_entityPath, d);
            }
        }

        private void AssignParamPopupMenu(DataGridViewRow r)
        {
            ContextMenuStrip contextStrip = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = "Delete ";
            it.ShortcutKeys = Keys.Control | Keys.D;
            it.Click += new EventHandler(DeleteParamItem);
            it.Tag = r;
            contextStrip.Items.AddRange(new ToolStripItem[] { it });
            r.ContextMenuStrip = contextStrip;
        }

        private void AssignObservPopupMenu(DataGridViewRow r)
        {
            ContextMenuStrip contextStrip = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = "Delete ";
            it.ShortcutKeys = Keys.Control | Keys.D;
            it.Click += new EventHandler(DeleteObservItem);
            it.Tag = r;
            contextStrip.Items.AddRange(new ToolStripItem[] { it });
            r.ContextMenuStrip = contextStrip;
        }

        /// <summary>
        /// Remove the entry of parameter value.
        /// </summary>
        /// <param name="key">the key of parameter value.</param>
        public void RemoveParamEntry(string key)
        {
            if (m_paramList.ContainsKey(key)) m_paramList.Remove(key);
            else return;

            for (int i = 0; i < RAParamGridView.Rows.Count; i++)
            {
                String pData = RAParamGridView[0, i].Value.ToString();
                if (pData.Equals(key))
                {
                    EcellObject obj = RAParamGridView.Rows[i].Tag as EcellObject;
                    if (obj == null) continue;
                    RAParamGridView.Rows.RemoveAt(i);
                    m_paramList.Remove(key);

                    foreach (EcellData d in obj.M_value)
                    {
                        if (key.Equals(d.M_entityPath))
                        {
                            d.M_isCommit = true;
                        }
                        break;
                    }
                    DataManager dManager = DataManager.GetDataManager();
                    dManager.DataChanged(obj.modelID, obj.key, obj.type, obj);
                    break;
                }
            }
        }

        /// <summary>
        /// Remove the entry of observed value.
        /// </summary>
        /// <param name="key">the key of observed value.</param>
        public void RemoveObservEntry(string key)
        {
            if (m_observList.ContainsKey(key)) m_observList.Remove(key);
            else return;

            for (int i = 0; i < RAObservGridView.Rows.Count; i++)
            {
                String ind = RAObservGridView[0, i].Value.ToString();
                if (ind.Equals(key))
                {
                    RAObservGridView.Rows.RemoveAt(i);
                    continue;
                }
            }

        }

        /// <summary>
        ///  The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="obj">Changed value of object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject obj)
        {
            if (obj.M_value == null) return;
            foreach (EcellData d in obj.M_value)
            {
                if (m_paramList.ContainsKey(d.M_entityPath))
                {
                    if (!d.M_isCommit) continue;
                    m_paramList.Remove(d.M_entityPath);
                    for (int i = 0; i < RAParamGridView.Rows.Count; i++)
                    {
                        String pData = RAParamGridView[0, i].Value.ToString();
                        if (pData.Equals(d.M_entityPath))
                        {
                            RAParamGridView.Rows.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (d.M_isCommit) continue;
                DataGridViewRow r = new DataGridViewRow();
                DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                c1.Value = d.M_entityPath;
                r.Cells.Add(c1);
                DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                c2.Value = d.Max;
                r.Cells.Add(c2);
                DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
                c3.Value = d.Min;
                r.Cells.Add(c3);
                DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                c4.Value = d.Step;
                r.Cells.Add(c4);
                r.Tag = obj;
                AssignParamPopupMenu(r);
                RAParamGridView.Rows.Add(r); m_paramList.Add(d.M_entityPath, d);
            }
        }

        /// <summary>
        /// Redraw the result table and graph when axis data is changed.
        /// </summary>
        public void AxisIndexChanged()
        {
            if (RAXComboBox.SelectedText == null || !RAXComboBox.SelectedText.Equals(RAYComboBox.SelectedText))
                return;

            ClearResult();


            // not implement
        }

        /// <summary>
        /// Add the judgement data into GridView.
        /// </summary>
        /// <param name="x">the value of parameter.</param>
        /// <param name="y">the value of parameter.</param>
        /// <param name="isOK"></param>
        public void AddJudgeData(double x, double y, bool isOK)
        {
            LineItem line = null;
            if (isOK)
            {
                RAResultGridView.Rows.Add(new object[] { x, y });

                line = m_zCnt.GraphPane.AddCurve(
                    "Result", 
                    new PointPairList(),
                    Color.Blue,
                    SymbolType.TriangleDown);
            }
            else
            {
                line = m_zCnt.GraphPane.AddCurve(
                    "Result",
                    new PointPairList(),
                    Color.Red,
                    SymbolType.TriangleDown);
            }
            line.Line.Width = 3;
            line.AddPoint(new PointPair(x, y));

            m_zCnt.AxisChange();
            m_zCnt.Refresh();
        }


        #region Events
        void RobustAnalysis_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_parent != null)
            {
                m_parent.CloseRobustWindow();
            }
            m_parent = null;
        }

        private void YIndexChanged(object sender, EventArgs e)
        {
            AxisIndexChanged();
        }

        private void XIndexChanged(object sender, EventArgs e)
        {
            AxisIndexChanged();
        }

        private void ParamDragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            DataManager dManager = DataManager.GetDataManager();
            EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
            foreach (EcellData d in t.M_value)
            {
                if (d.M_entityPath.Equals(dobj.Path))
                {
                    d.M_isCommit = false;
                    break;
                }
            }
            dManager.DataChanged(t.modelID, t.key, t.type, t);
        }

        private void ParamDragEnter(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ObservDragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            DataManager dManager = DataManager.GetDataManager();
            EcellObject t = dManager.GetEcellObject(dobj.ModelID, dobj.Key, dobj.Type);
            foreach (EcellData d in t.M_value)
            {
                if (d.M_entityPath.Equals(dobj.Path))
                {
                    DataGridViewRow r = new DataGridViewRow();
                    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                    c1.Value = d.M_entityPath;
                    r.Cells.Add(c1);
                    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                    c2.Value = d.Max;
                    r.Cells.Add(c2);
                    DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
                    c3.Value = d.Min;
                    r.Cells.Add(c3);
                    DataGridViewTextBoxCell c4 = new DataGridViewTextBoxCell();
                    c4.Value = d.Max - d.Min;
                    r.Cells.Add(c4);
                    DataGridViewTextBoxCell c5 = new DataGridViewTextBoxCell();
                    c5.Value = 0.0;
                    r.Cells.Add(c5);
                    r.Tag = t;
                    AssignObservPopupMenu(r);
                    RAObservGridView.Rows.Add(r);
                    m_observList.Add(d.M_entityPath, d);
                }
            }
        }

        private void ObservDragEnter(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("EcellLib.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void DeleteObservItem(object sender, EventArgs e)
        {
            DataGridViewRow r = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
            if (r == null) return;

            string key = r.Cells[0].Value.ToString();
            RemoveObservEntry(key);
        }

        private void DeleteParamItem(object sender, EventArgs e)
        {
            DataGridViewRow r = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
            if (r == null) return;

            string key = r.Cells[0].Value.ToString();
            RemoveParamEntry(key);
        }
        #endregion

        /// <summary>
        /// Process when user click close button on Window.
        /// </summary>
        /// <param name="m">Message</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && m.WParam.ToInt32() == SC_CLOSE)
            {
                String mes = m_resources.GetString("ConfirmClose");

                DialogResult res = MessageBox.Show(mes,
                    "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.OK) this.Dispose();
                return;
            }

            base.WndProc(ref m);
        }
   }
}