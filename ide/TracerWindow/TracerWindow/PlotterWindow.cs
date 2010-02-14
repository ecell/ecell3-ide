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
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ZedGraph;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Plotter window.
    /// </summary>
    public partial class PlotterWindow : EcellDockContent
    {
        #region Fields
        /// <summary>
        /// The object managed this window.
        /// </summary>
        private TracerWindow m_owner;
        /// <summary>
        /// Graph control for tracer.
        /// </summary>
        private ZedGraphControl m_zCnt;
        /// <summary>
        /// The current row.
        /// </summary>
        private DataGridViewRow m_row = null;
        /// <summary>
        /// The number of log entry.
        /// </summary>
        private int m_entryNum = 0;
        /// <summary>
        /// The dictionary of data and LineItem.
        /// </summary>
        private Dictionary<int, LineItem> m_lineDic = new Dictionary<int, LineItem>();
        #endregion

        #region Constructor
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="control">Owner object</param>
        public PlotterWindow(TracerWindow control)
        {
            m_owner = control;
            this.HideOnClose = false;
            InitializeComponent();

            m_zCnt = new ZedGraphControl();
            m_zCnt.Dock = DockStyle.Fill;
            m_zCnt.GraphPane.Title.Text = "";
            m_zCnt.GraphPane.XAxis.Title.IsVisible = false;
            m_zCnt.GraphPane.YAxis.Title.IsVisible = false;
            m_zCnt.GraphPane.Legend.IsVisible = false;
            m_zCnt.GraphPane.XAxis.Scale.Max = 100;
            m_zCnt.GraphPane.XAxis.Scale.MaxAuto = true;
            m_zCnt.GraphPane.YAxis.Scale.MaxAuto = true;
            m_zCnt.GraphPane.XAxis.Scale.Min = 0;
            m_zCnt.GraphPane.Margin.Top = 35.0f;
            m_zCnt.GraphPane.YAxis.MajorGrid.IsVisible = true;
            m_zCnt.GraphPane.XAxis.MinorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.XAxis.MajorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MinorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MajorTic.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.Chart.Border.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.YAxis.MajorGrid.Color = Color.FromArgb(200, 200, 200);
            m_zCnt.GraphPane.Fill = new Fill(Color.White, Color.LightGray, 90.0f);

            plotTableLayoutPanel.Controls.Add(m_zCnt, 0, 0);
            m_zCnt.AxisChange();
            m_zCnt.Refresh();

            displaySettingDataGrid.ContextMenuStrip = gridContextMenuStrip;
        }
        #endregion

        #region Events
        /// <summary>
        /// Click the add log menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ClickAddEntry(object sender, EventArgs e)
        {
            SelectPlotDataDialog dialog = new SelectPlotDataDialog(m_owner.Environment);
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string xName = dialog.X;
            string yName = dialog.Y;

            int ind = displaySettingDataGrid.Rows.Count;

            double interval =
                m_owner.DataManager
                    .GetLoggerPolicy(m_owner.DataManager.GetCurrentSimulationParameterID())
                    .ReloadInterval;
            double endTime =
                m_owner.DataManager.GetCurrentSimulationTime();
            LogData dataX = m_owner.DataManager.GetLogData(0.0, endTime, endTime/10000.0, xName);
            LogData dataY = m_owner.DataManager.GetLogData(0.0, endTime, endTime/10000.0, yName);
            if (dataX.logValueList.Count != dataY.logValueList.Count)
            {
                return;
            }

            Bitmap b = new Bitmap(20, 20);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(Util.GetColorBlush(ind), 3, 3, 14, 14);
            g.ReleaseHdc(g.GetHdc());

            DataGridViewRow r = new DataGridViewRow();
            DataGridViewImageCell c1 = new DataGridViewImageCell();
            c1.Value = b;
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = xName;
            r.Cells.Add(c2);

            DataGridViewTextBoxCell c3 = new DataGridViewTextBoxCell();
            c3.Value = yName;
            r.Cells.Add(c3);

            r.Tag = m_entryNum;
            displaySettingDataGrid.Rows.Add(r);
            c2.ReadOnly = true;
            c3.ReadOnly = true;
          
            LineItem i = m_zCnt.GraphPane.AddCurve(xName,
                    new PointPairList(), Util.GetColor(ind), SymbolType.None);
            i.Line.Width = 1;

            m_lineDic.Add(m_entryNum, i);
            m_entryNum++;

            for (int j = 0; j < dataX.logValueList.Count; j++)
            {
                i.AddPoint(dataX.logValueList[j].value, dataY.logValueList[j].value);
            }

            m_zCnt.AxisChange();
            Graphics g1 = m_zCnt.CreateGraphics();
            g1.ResetClip();
            g1.SetClip(m_zCnt.MasterPane.Rect);
            i.Draw(g1, m_zCnt.GraphPane, 0, 1.5F);
            g1.ResetClip();
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Click the delete log menu.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
        private void ClickDeleteEntry(object sender, EventArgs e)
        {
            if (m_row.Tag == null) return;
            int index = (int)m_row.Tag;

            m_zCnt.GraphPane.CurveList.Remove(m_lineDic[index]);
            displaySettingDataGrid.Rows.Remove(m_row);
            m_lineDic.Remove(index);
            m_zCnt.Refresh();
        }

        /// <summary>
        /// Opening the context menu on DataGridView.
        /// </summary>
        /// <param name="sender">ContextMenuStripItem</param>
        /// <param name="e">CancelEventArgs</param>
        private void OpeningContextMenu(object sender, CancelEventArgs e)
        {
            if (m_row == null)
            {
                deleteToolStripMenuItem.Enabled = false;
            }
            else
                deleteToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Double click on DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        private void CellDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewImageCell c = displaySettingDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] 
                as DataGridViewImageCell;
            if (c == null) return;

            int index = (int)displaySettingDataGrid.Rows[e.RowIndex].Tag;

            ShowColorSetDialog(index, e.RowIndex, e.ColumnIndex);
        }

        /// <summary>
        /// Mouse click on DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellMouseEventArgs</param>
        private void CellMouseClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_row = null;
                if (e.RowIndex >= 0)
                {
                    m_row = displaySettingDataGrid.Rows[e.RowIndex];
                }
            }
        }
        #endregion

        /// <summary>
        /// Show the color select dialog.
        /// </summary>
        /// <param name="index">the line index.</param>
        /// <param name="rowIndex">the row index of cell.</param>
        /// <param name="columnIndex">the column index of cell.</param>
        private void ShowColorSetDialog(int index, int rowIndex, int columnIndex)
        {
            DataGridViewImageCell cell = displaySettingDataGrid.Rows[rowIndex].Cells[columnIndex] 
                as DataGridViewImageCell;
            Debug.Assert(cell != null);

            DialogResult r = m_colorDialog.ShowDialog();
            if (r == DialogResult.OK)
            {
                Bitmap b = new Bitmap(20, 20);
                Graphics g = Graphics.FromImage(b);
                Pen p = new Pen(m_colorDialog.Color);
                g.FillRectangle(p.Brush, 3, 3, 14, 14);
                g.ReleaseHdc(g.GetHdc());

                m_lineDic[index].Color = m_colorDialog.Color;
                cell.Value = b;
                m_zCnt.Refresh();
            }
        }

    }
}
