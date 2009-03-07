//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2009 Keio University
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
using System.Threading;
using System.Text;
using System.Windows.Forms;

using Ecell.Objects;
using Ecell.Logger;

namespace Ecell.IDE.Plugins.TracerWindow
{
    /// <summary>
    /// Logger window class.
    /// </summary>
    public partial class LoggerWindow : EcellDockContent
    {
        #region Fields
        private TracerWindow m_owner;
        private bool m_isChanged = false;
        private Dictionary<string, LoggerEntry> m_logList = new Dictionary<string, LoggerEntry>();
        /// <summary>
        /// The delegate for event handler function.
        /// </summary>
        delegate void EventHandlerCallBack();
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public LoggerWindow(TracerWindow owner)
        {
            InitializeComponent();
            m_owner = owner;
            loggerDataGrid.ContextMenuStrip = gridContextMenuStrip;
        }
        #endregion
        
        #region Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loggerDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewImageCell c = loggerDataGrid[e.ColumnIndex, e.RowIndex] as DataGridViewImageCell;
            if (c == null) return;

            LoggerEntry entry = (LoggerEntry)loggerDataGrid.Rows[e.RowIndex].Tag;

            List<DataGridViewRow> rlist = new List<DataGridViewRow>();
            rlist.Add(loggerDataGrid.Rows[e.RowIndex]);

            if (e.ColumnIndex == ColorColumn.Index)            
                ShowColorSelectDialog(rlist);            
            else if (e.ColumnIndex == LineColumn.Index)
                ShowLineStyleDialog(rlist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loggerDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewCheckBoxCell c = loggerDataGrid[e.ColumnIndex, e.RowIndex] as DataGridViewCheckBoxCell;
            if (c == null) return;

            int sindex = IsShownColumn.Index;
            LoggerEntry entry = (LoggerEntry)loggerDataGrid.Rows[e.RowIndex].Tag;

            if (sindex == e.ColumnIndex)
            {
                bool isCheck = (bool)c.Value;
                entry.IsShown = isCheck;
                m_isChanged = true;
                m_owner.Environment.LoggerManager.LoggerChanged(entry.FullPN, entry);
                m_isChanged = false;
                loggerDataGrid.Rows[e.RowIndex].Tag = entry;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loggerDataGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loggerDataGrid_DragEnter(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loggerDataGrid_DragDrop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData("Ecell.Objects.EcellDragObject");
            if (obj == null) return;
            EcellDragObject dobj = obj as EcellDragObject;

            foreach (EcellDragEntry ent in dobj.Entries)
            {
                m_owner.Environment.LoggerManager.AddLoggerEntry(dobj.ModelID, ent.Key, ent.Type, ent.Path);
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
            //m_owner.CurrentWin = tWin;
        }

        private void gridContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (loggerDataGrid.CurrentCell == null ||
                loggerDataGrid.CurrentRow.Tag == null)
            {
                deleteToolStripMenuItem.Enabled = false;
                windowToolStripMenuItem.Enabled = false;
                lineStyleToolStripMenuItem.Enabled = false;
                colorToolStripMenuItem.Enabled = false;
                return;
            }

            int selectedCount = loggerDataGrid.SelectedRows.Count;
            if (selectedCount == 1)
            {
                deleteToolStripMenuItem.Enabled = true;
                windowToolStripMenuItem.Enabled = true;
                lineStyleToolStripMenuItem.Enabled = true;
                colorToolStripMenuItem.Enabled = true;

                LoggerEntry entry = loggerDataGrid.CurrentRow.Tag as LoggerEntry;
                if (entry == null) return;

                Dictionary<string, bool> displayDic = m_owner.GetDisplayWindows(entry);
                windowToolStripMenuItem.DropDownItems.Clear();
                foreach (string name in displayDic.Keys)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(name);
                    item.Tag = entry;
                    item.Checked = displayDic[name];
                    item.Click += new EventHandler(ClickWindowMenuItem);
                    windowToolStripMenuItem.DropDownItems.Add(item);
                }
            }
            else if (selectedCount > 1)
            {
                deleteToolStripMenuItem.Enabled = true;
                windowToolStripMenuItem.Enabled = false;
                lineStyleToolStripMenuItem.Enabled = true;
                colorToolStripMenuItem.Enabled = true;
            }
        }

        private void ClickWindowMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            string winname = item.Text;
            LoggerEntry entry = item.Tag as LoggerEntry;

            m_owner.ChangeDisplayStatus(entry, winname, !item.Checked);
        }


        private void importLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportLog(null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteEntry();
        }


        private void lineStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rList = new List<DataGridViewRow>();

            foreach (DataGridViewRow r in loggerDataGrid.SelectedRows)
            {
                rList.Add(r);
            }
            if (rList.Count <= 0) return;
            ShowLineStyleDialog(rList);
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rList = new List<DataGridViewRow>();

            foreach (DataGridViewRow r in loggerDataGrid.SelectedRows)
            {
                rList.Add(r);
            }
            if (rList.Count <= 0) return;
            ShowColorSelectDialog(rList);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rList"></param>
        private void ShowColorSelectDialog(List<DataGridViewRow> rList)
        {
            if (m_colorDialog.ShowDialog() != DialogResult.OK)
                return;

            //SetColor(rList, m_colorDialog.Color);
            DataGridViewImageCell lcell;
            DataGridViewImageCell ccell;
            int lindex = LineColumn.Index;
            int cindex = ColorColumn.Index;
            Color setColor = m_colorDialog.Color;

            foreach (DataGridViewRow r in rList)
            {
                LoggerEntry entry = r.Tag as LoggerEntry;
                lcell = loggerDataGrid[lindex, r.Index] as DataGridViewImageCell;
                ccell = loggerDataGrid[cindex, r.Index] as DataGridViewImageCell;

                Bitmap b = new Bitmap(20, 20);
                Graphics g = Graphics.FromImage(b);
                Pen p = new Pen(setColor);
                g.FillRectangle(p.Brush, 3, 3, 14, 14);
                g.ReleaseHdc(g.GetHdc());

                Bitmap b1 = new Bitmap(20, 20);
                Graphics g1 = Graphics.FromImage(b1);
                Pen p1 = new Pen(setColor);
                p1.DashStyle = entry.LineStyle;
                p1.Width = entry.LineWidth;
                g1.DrawLine(p1, 0, 10, 20, 10);
                g1.ReleaseHdc(g1.GetHdc());

                lcell.Value = b1;
                ccell.Value = b;

                entry.Color = setColor;
                m_isChanged = true;
                m_owner.Environment.LoggerManager.LoggerChanged(entry.FullPN, entry);
                m_isChanged = false;
                loggerDataGrid.Rows[r.Index].Tag = entry;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rList"></param>
        private void ShowLineStyleDialog(List<DataGridViewRow> rList)
        {
            int lineWidth = 2;
            System.Drawing.Drawing2D.DashStyle style = System.Drawing.Drawing2D.DashStyle.Solid;

            if (rList.Count == 1)
            {
                LoggerEntry entry = rList[0].Tag as LoggerEntry;
                if (entry != null)
                {
                    lineWidth = entry.LineWidth;
                    style = entry.LineStyle;
                }
            }
            LineStyleDialog dialog = new LineStyleDialog(lineWidth, style);
            using (dialog)
            {
                 DataGridViewImageCell c;
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                if (style == System.Drawing.Drawing2D.DashStyle.Custom) return;
                lineWidth = dialog.LineWidth;
                style = dialog.LineStyle;

                foreach (DataGridViewRow r in rList)
                {
                    LoggerEntry  entry = r.Tag as LoggerEntry;
                    if (entry == null ) continue;
                    c = loggerDataGrid[LineColumn.Index, r.Index] as DataGridViewImageCell;

                    Bitmap b1 = new Bitmap(20, 20);
                    Graphics g1 = Graphics.FromImage(b1);
                    Pen p1 = new Pen(entry.Color);
                    p1.DashStyle = style;
                    p1.Width = lineWidth;
                    g1.DrawLine(p1, 0, 10, 20, 10);
                    g1.ReleaseHdc(g1.GetHdc());
                    c.Value = b1;

                    entry.LineStyle = style;
                    entry.LineWidth = lineWidth;
                    m_isChanged = true;
                    m_owner.Environment.LoggerManager.LoggerChanged(entry.FullPN, entry);
                    m_isChanged = false;
                    loggerDataGrid.Rows[r.Index].Tag = entry;
                }
            }
        }

        /// <summary>
        /// Commit the change of cell.
        /// </summary>
        private void StateChanged()
        {
            if (loggerDataGrid.IsCurrentCellDirty)
            {
                loggerDataGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            loggerDataGrid.Rows.Clear();
            m_logList.Clear();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public void LoggerAdd(LoggerEntry entry)
        {
            Bitmap b = new Bitmap(20, 20);
            Graphics g = Graphics.FromImage(b);
            Pen pen = new Pen(entry.Color);
            g.FillRectangle(pen.Brush, 3, 3, 14, 14);
            g.ReleaseHdc(g.GetHdc());

            Bitmap b1 = new Bitmap(20, 20);
            Graphics g1 = Graphics.FromImage(b1);
            pen.DashStyle = entry.LineStyle;
            pen.Width = entry.LineWidth;
            g1.DrawLine(pen, 0, 10, 20, 10);
            g1.ReleaseHdc(g1.GetHdc());


            DataGridViewRow r = new DataGridViewRow();
            DataGridViewCheckBoxCell c1 = new DataGridViewCheckBoxCell();
            c1.Value = entry.IsShown;
            r.Cells.Add(c1);

            DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
            c2.Value = entry.FullPN;
            r.Cells.Add(c2);

            DataGridViewImageCell c3 = new DataGridViewImageCell();
            c3.Value = b;
            r.Cells.Add(c3);

            DataGridViewImageCell c4 = new DataGridViewImageCell();
            c4.Value = b1;
            r.Cells.Add(c4);

            r.Tag = entry;
            loggerDataGrid.Rows.Add(r);
            c2.ReadOnly = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgFullPN"></param>
        /// <param name="entry"></param>
        public void LoggerChanged(string orgFullPN, LoggerEntry entry)
        {
            if (m_isChanged) return;

            int findex = FullPNColumn.Index;
            int rindex = -1;
            for (int i = 0; i < loggerDataGrid.Rows.Count; i++)
            {
                LoggerEntry ent = loggerDataGrid.Rows[i].Tag as LoggerEntry;
                if (ent.Equals(entry))
                {
                    rindex = i;
                    break;
                }
            }

            if (rindex == -1) return;
            Bitmap b = new Bitmap(20, 20);
            Graphics g = Graphics.FromImage(b);
            Pen pen = new Pen(entry.Color);
            g.FillRectangle(pen.Brush, 3, 3, 14, 14);
            g.ReleaseHdc(g.GetHdc());

            Bitmap b1 = new Bitmap(20, 20);
            Graphics g1 = Graphics.FromImage(b1);
            pen.DashStyle = entry.LineStyle;
            g1.DrawLine(pen, 0, 10, 20, 10);
            g1.ReleaseHdc(g1.GetHdc());

            loggerDataGrid[IsShownColumn.Index, rindex].Value = entry.IsShown;
            loggerDataGrid[FullPNColumn.Index, rindex].Value = entry.FullPN;
            loggerDataGrid[ColorColumn.Index, rindex].Value = b;
            loggerDataGrid[LineColumn.Index, rindex].Value = b1;
            loggerDataGrid.Rows[rindex].Tag = entry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public void LoggerDeleted(LoggerEntry entry)
        {
            if (m_isChanged) return;

            int findex = FullPNColumn.Index;
            for (int i = 0; i < loggerDataGrid.Rows.Count; i++)
            {
                LoggerEntry ent = loggerDataGrid.Rows[i].Tag as LoggerEntry;
                if (ent.Equals(entry))
                {
                    loggerDataGrid.Rows.RemoveAt(i);
                    break;
                }
            }

            if (entry.IsLoaded)
            {
                m_logList.Remove(entry.FileName);
            }
        }


        private void DeleteEntry()
        {
            if (loggerDataGrid.SelectedRows.Count <= 0)
                return;

            List<LoggerEntry> entryList = new List<LoggerEntry>();
            foreach (DataGridViewRow r in loggerDataGrid.SelectedRows)
            {
                LoggerEntry entry = r.Tag as LoggerEntry;
                if (entry != null)
                    entryList.Add(entry);
            }

            foreach (LoggerEntry entry in entryList)
            {
                if (entry.IsLoaded)
                {
                    m_owner.LoggerManager_LoggerDeleteEvent(null, 
                        new LoggerEventArgs(entry.FullPN, entry));
                    return;
                }

                EcellObject obj = m_owner.DataManager.GetEcellObject(
                    entry.ModelID, entry.ID, entry.Type);

                foreach (EcellData d in obj.Value)
                {
                    if (d.EntityPath.Equals(entry.FullPN))
                    {
                        d.Logged = false;
                        break;
                    }
                }
                m_owner.DataManager.DataChanged(obj.ModelID, obj.Key, obj.Type, obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void ImportLog(string fileName)
        {
            if (fileName == null)
            {
                if (m_openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                fileName = m_openFileDialog.FileName;
            }

            if (m_logList.ContainsKey(fileName)) return;

            LogData log = m_owner.DataManager.LoadSimulationResult(fileName);
            if (log.logValueList.Count <= 0)
            {
                Util.ShowWarningDialog(string.Format(MessageResources.WarnNoLog, fileName));
                return;
            }
            string[] ele = log.propName.Split(new char[] { ':' });
            string propName = "Log:" + log.key + ":" + ele[ele.Length - 1];

            LoggerEntry entry = new LoggerEntry(log.model, log.key, log.type, propName);
            entry.IsLoaded = true;
            entry.FileName = fileName;
            m_logList.Add(fileName, entry);

            m_owner.LoggerManager_LoggerAddEvent(null, new LoggerEventArgs(entry.FullPN, entry));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((int)keyData == (int)Keys.Control + (int)Keys.D ||
                (int)keyData == (int)Keys.Delete)
            {
                DeleteEntry();
            }
            if ((int)keyData == (int)Keys.Control + (int)Keys.I)
            {
                ImportLog(null);
            }
            if ((int)keyData == (int)Keys.Control + (int)Keys.C)
            {
                colorToolStripMenuItem.PerformClick();
            }
            if ((int)keyData == (int)Keys.Control + (int)Keys.L)
            {
                lineStyleToolStripMenuItem.PerformClick();
            }



            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}