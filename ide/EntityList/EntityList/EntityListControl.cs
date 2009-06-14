using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.Objects;
using Ecell.IDE;

namespace Ecell.IDE.Plugins.EntityList
{
    public partial class EntityListControl : UserControl
    {
        /// <summary>
        /// I/F of EntityList plugin.
        /// </summary>
        private EntityList m_owner;
        /// <summary>
        /// ImageList of node icons.
        /// </summary>
        private ImageList m_iconList;
        /// <summary>
        /// Dragged object.
        /// </summary>
        private EcellObject m_dragObject;
        /// <summary>
        /// Set true when this plugin throw SelectChange event.
        /// </summary>
        private bool m_isSelected = false;
        /// <summary>
        /// Set true when SelectionChanged is executed.
        /// </summary>
        private bool m_isSelectionChanged = false;
        /// <summary>
        /// Selected row.
        /// </summary>
        private DataGridViewRow m_selectedRow = null;
        private DataGridViewRow m_lastSelected = null;
        /// <summary>
        /// 
        /// </summary>
        private bool m_intact = false;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isShowType = true;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isShowName = true;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isShowClassName = true;
        /// <summary>
        /// 
        /// </summary>
        private bool m_isShowPathID = true;

        #region Constants
        /// <summary>
        /// The reserved name for the type of object.
        /// </summary>
        protected const string IndexType = "Type";
        /// <summary>
        /// The reserved name for ID of object.
        /// </summary>
        protected const string IndexID = "ID";
        /// <summary>
        /// The reserved name for the class name of object.
        /// </summary>
        protected const string IndexClass = "ClassName";
        /// <summary>
        /// The reserved name for the name of object.
        /// </summary>
        protected const string IndexName = "ObjectName";

        /// <summary>
        /// The property array of System.
        /// </summary>
        private static string[] m_propArray = new string[] {
            IndexType,
            IndexClass,
            IndexID,
            IndexName
        };
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="icons"></param>
        public EntityListControl(EntityList owner, ImageList icons)
        {
            m_owner = owner;
            m_iconList = icons;
            m_owner.PluginManager.NodeImageListChange += new EventHandler(PluginManager_NodeImageListChange);
            InitializeComponent();
            ResetSearchTextBox();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PluginManager_NodeImageListChange(object sender, EventArgs e)
        {
            for (int i = 0; i < objectListDataGrid.Rows.Count; i++)
            {
                EcellObject obj = (EcellObject)objectListDataGrid.Rows[i].Tag;
                Image image = GetIconImage(obj);
                objectListDataGrid.Rows[i].Cells[0].Value = image;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Image GetIconImage(EcellObject obj)
        {
            Image image;
            if (m_iconList.Images.ContainsKey(obj.Layout.Figure))
                image = m_iconList.Images[obj.Layout.Figure];
            else
                image = m_iconList.Images[obj.Type];
            return image;
        }

        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Event on SelectChange
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void SelectChanged(string modelID, string key, string type)
        {
            if (m_isSelected)
                return;
            objectListDataGrid.ClearSelection();
            m_selectedRow = null;
            AddSelect(modelID, key, type);
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null && objectListDataGrid.FirstDisplayedScrollingRowIndex >= 0 &&
                    row.Visible)
            {
                objectListDataGrid.FirstDisplayedScrollingRowIndex = row.Index;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void AddSelect(string modelID, string key, string type)
        {
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null)
            {
                m_isSelected = true;
                row.Selected = true;
                m_isSelected = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null)
                row.Selected = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void DataAdd(EcellObject obj)
        {
            if (obj.Type != Constants.xpathSystem &&
                obj.Type != Constants.xpathVariable &&
                obj.Type != Constants.xpathProcess &&
                obj.Type != Constants.xpathStepper)
                return;

            if (obj.Key.EndsWith(":SIZE")) return;
            int ind = SearchInsertPosition(obj.Key, obj.Type);

            DataGridViewRow rs = CreateRow(obj);
            objectListDataGrid.Rows.Insert(ind, rs);
            if (!m_intact)
            {
                string searchCnd = searchTextBox.Text;
                rs.Visible =
                    ((string)rs.Cells[IndexID].Value).ToLower().Contains(searchCnd.ToLower())
                    || ((string)rs.Cells[IndexName].Value).ToLower().Contains(searchCnd.ToLower());
            }
        }

        private DataGridViewRow CreateRow(EcellObject obj)
        {
            DataGridViewRow rs = new DataGridViewRow();
            {
                DataGridViewImageCell c = new DataGridViewImageCell();
                c.Value = GetIconImage(obj);
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = obj.Classname;
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = obj.Key;
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                EcellData d = obj.GetEcellData("Name");
                // for loading the project include the process not in DM directory.
                if (d == null)
                    c.Value = "";
                else
                    c.Value = d.Value.ToString();
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }
            rs.Tag = obj;
            return rs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            if (key != data.Key)
            {
                DataDelete(modelID, key, type);
                DataAdd(data);
                AddSelect(data.ModelID, data.Key, data.Type);
                return;
            }
            DataGridViewRow r = SearchIndex(type, key);
            if (r != null)
            {
                r.Tag = data;
                r.Cells[IndexClass].Value = data.Classname;
                EcellData d = data.GetEcellData("Name");
                r.Cells[IndexName].Value = d != null ? d.Value.ToString() : "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void DataDelete(string modelID, string key, string type)
        {
            if (type == Constants.xpathSystem)
            {
                List<DataGridViewRow> res = SearchIndexInSystem(key);
                foreach (DataGridViewRow r in res)
                {
                    m_isSelected = true;
                    objectListDataGrid.Rows.Remove(r);
                    m_isSelected = false;
                }
            }
            else
            {
                DataGridViewRow r = SearchIndex(type, key);
                if (r != null)
                {
                    m_isSelected = true;
                    objectListDataGrid.Rows.Remove(r);
                    m_isSelected = false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetSelect()
        {
            objectListDataGrid.ClearSelection();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            objectListDataGrid.Rows.Clear();
            m_selectedRow = null;
            m_lastSelected = null;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        private int TypeConverter(string type1, string type2)
        {
            int ind1, ind2;

            ind1 = ind2 = 4;
            switch (type1)
            {
                case EcellObject.SYSTEM:
                    ind1 = 2;
                    break;
                case EcellObject.PROCESS:
                    ind1 = 3;
                    break;
                case EcellObject.VARIABLE:
                    ind1 = 1;
                    break;
                case EcellObject.STEPPER:
                    ind1 = 0;
                    break;
            }

            switch (type2)
            {
                case EcellObject.SYSTEM:
                    ind2 = 2;
                    break;
                case EcellObject.PROCESS:
                    ind2 = 3;
                    break;
                case EcellObject.VARIABLE:
                    ind2 = 1;
                    break;
                case EcellObject.STEPPER:
                    ind2 = 0;
                    break;
            }

            return ind1 - ind2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private int SearchInsertPosition(string key, string type)
        {
            int i = 0;
            for (; i < objectListDataGrid.Rows.Count ; i++)
            {
                EcellObject obj = objectListDataGrid.Rows[i].Tag as EcellObject;
                if (TypeConverter(type, obj.Type) == 0)
                {
                    if (string.Compare(key, obj.Type) < 0)
                        return i;
                }
                else if (TypeConverter(type, obj.Type) == 1)
                {
                    return i;
                }
            }
            return i;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<DataGridViewRow> SearchIndexInSystem(string id)
        {
            List<DataGridViewRow> result = new List<DataGridViewRow>();
            foreach (DataGridViewRow r in objectListDataGrid.Rows)
            {
                EcellObject obj = r.Tag as EcellObject;
                if (obj.Key.StartsWith(id))
                    result.Add(r);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private DataGridViewRow SearchIndex(string type, string id)
        {
            foreach (DataGridViewRow r in objectListDataGrid.Rows)
            {
                EcellObject obj = r.Tag as EcellObject;
                if (obj.Type == type && obj.Key == id)
                    return r;
            }
            return null;
        }

        /// <summary>
        /// Event when the cell is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClickObjectCell(object sender, DataGridViewCellEventArgs e)
        {
            int ind = e.RowIndex;
            if (ind < 0) return;
            EcellObject obj = objectListDataGrid.Rows[ind].Tag as EcellObject;
            if (obj == null) return;
            m_isSelected = true;
            m_selectedRow = objectListDataGrid.Rows[ind];
            m_dragObject = null;

            if (objectListDataGrid.Rows[ind].Selected)
            {
                if (objectListDataGrid.SelectedRows.Count <= 1)
                    m_owner.PluginManager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
                else
                {
                    m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                }
            }
            else
            {
                m_owner.PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);
            }
            m_isSelected = false;
            m_selectedRow = null;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResetSearchTextBox()
        {
            searchTextBox.ForeColor = SystemColors.GrayText;
            searchTextBox.Text = MessageResources.InitialText;
            foreach (DataGridViewRow r in objectListDataGrid.Rows)
            {
                r.Visible = true;
            }
            m_intact = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            ResetSearchTextBox();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).ForeColor = SystemColors.WindowText;
            if (m_intact)
                ((TextBox)sender).Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!((TextBox)sender).Focused)
                return;
            m_intact = false;

            string searchCnd = searchTextBox.Text;
            objectListDataGrid.SuspendLayout();
            foreach (DataGridViewRow r in objectListDataGrid.Rows)
            {
                r.Visible =
                    ((string)r.Cells[IndexID].Value).ToLower().Contains(searchCnd.ToLower())
                    || ((string)r.Cells[IndexName].Value).ToLower().Contains(searchCnd.ToLower());
            }
            objectListDataGrid.ResumeLayout();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length == 0)
                ResetSearchTextBox();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewMouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if (m_dragObject == null) return;
            EnterDragMode();
            m_dragObject = null;
        }
        private void DataGridViewMouseLeave(object sender, EventArgs e)
        {
            m_isSelected = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewMouseUp(object sender, MouseEventArgs e)
        {
            m_selectedRow = null;
            m_isSelected = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewMouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hti = objectListDataGrid.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {                
                if (hti.RowIndex < 0)
                    return;
                DataGridViewRow r = objectListDataGrid.Rows[hti.RowIndex];
                if (Control.ModifierKeys != Keys.Shift)
                {
                    m_selectedRow = r;
                    m_lastSelected = r;
                }
                else
                {
                    if (m_lastSelected != null)
                    {
                        int startindex, endindex;
                        if (hti.RowIndex > m_lastSelected.Index)
                        {
                            endindex = hti.RowIndex;
                            startindex = m_lastSelected.Index;
                        }
                        else
                        {
                            startindex = hti.RowIndex;
                            endindex = m_lastSelected.Index;
                        }
                        foreach (DataGridViewRow r1 in objectListDataGrid.Rows)
                        {
                            EcellObject obj = r1.Tag as EcellObject;
                            if (obj == null) continue;
                            m_isSelected = true;
                            if (r1.Index >= startindex && r1.Index <= endindex)
                            {
                                if (!r1.Selected)
                                    m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                            else
                            {
                                if (r1.Selected)
                                    m_owner.PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                            m_isSelected = false;
                        }
                    }
                }
                m_dragObject = r.Tag as EcellObject;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (hti.Type == DataGridViewHitTestType.ColumnHeader)
                {
                    typeToolStripMenuItem.Checked = m_isShowType;
                    classToolStripMenuItem.Checked = m_isShowClassName;
                    pathIDToolStripMenuItem.Checked = m_isShowPathID;
                    nameToolStripMenuItem.Checked = m_isShowName;
                    objectListDataGrid.ContextMenuStrip = titleContextMenuStrip;                    
                    return;
                }
                if (hti.Type != DataGridViewHitTestType.Cell)
                {
                    objectListDataGrid.ContextMenuStrip = null;
                    return;
                }
                int rIndex = hti.RowIndex;
                EcellObject obj = objectListDataGrid.Rows[rIndex].Tag as EcellObject;
                CommonContextMenu m = new CommonContextMenu(obj, m_owner.Environment);
                objectListDataGrid.ContextMenuStrip = m.Menu;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void EnterDragMode()
        {
            EcellDragObject dobj = null;
            if (objectListDataGrid.SelectedRows.Count <= 0)
                return;

            foreach (DataGridViewRow r in objectListDataGrid.SelectedRows)
            {
                EcellObject obj = r.Tag as EcellObject;
                if (obj == null)
                    continue;

                // Create new EcellDragObject.
                if (dobj == null)
                    dobj = new EcellDragObject(obj.ModelID);

                foreach (EcellData v in obj.Value)
                {
                    if (!v.Name.Equals(Constants.xpathActivity) &&
                        !v.Name.Equals(Constants.xpathMolarConc) &&
                        !v.Name.Equals(Constants.xpathSize))
                        continue;

                    // Add new EcellDragEntry.
                    dobj.Entries.Add(new EcellDragEntry(
                                                obj.Key,
                                                obj.Type,
                                                v.EntityPath,
                                                v.Settable,
                                                v.Logable));
                    break;

                }
                if (objectListDataGrid.SelectedRows.Count == 1)
                {
                    m_isSelected = true;
                    m_owner.PluginManager.SelectChanged(obj);
                    m_isSelected = false;
                }
            }
            // Drag & Drop Event.
            if (dobj != null)
                this.DoDragDrop(dobj, DragDropEffects.Move | DragDropEffects.Copy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickShowColumnMenu(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) return;

            String name = item.Tag as string;
            if (name.Equals(IndexType))
            {
                m_isShowType = !m_isShowType;
                typeToolStripMenuItem.Checked = m_isShowType;
                Type.Visible = m_isShowType;
            }
            else if (name.Equals(IndexName))
            {
                m_isShowName = !m_isShowName;
                nameToolStripMenuItem.Checked = m_isShowName;
                ObjectName.Visible = m_isShowName;
            }
            else if (name.Equals(IndexClass))
            {
                m_isShowClassName = !m_isShowClassName;
                classToolStripMenuItem.Checked = m_isShowClassName;
                ClassName.Visible = m_isShowClassName;
            }
            else if (name.Equals(IndexID))
            {
                m_isShowPathID = !m_isShowPathID;
                pathIDToolStripMenuItem.Checked = m_isShowPathID;
                ID.Visible = m_isShowPathID;
            }
        }

        private void TypeSortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column is DataGridViewImageColumn)
            {
                EcellObject obj1 = (EcellObject)objectListDataGrid.Rows[e.RowIndex1].Tag;
                EcellObject obj2 = (EcellObject)objectListDataGrid.Rows[e.RowIndex2].Tag;

                e.SortResult = TypeConverter(obj1.Type, obj2.Type);

                e.Handled = true;
            }
        }

        private void EntSelectionChanged(object sender, EventArgs e)
        {
            if (m_isSelected && m_selectedRow != null && m_selectedRow.Index != -1 && !m_isSelectionChanged)
            {
                m_isSelectionChanged = true;
                objectListDataGrid.ClearSelection();
                m_selectedRow.Selected = true;
                m_isSelectionChanged = false;
            }
            else if (!m_isSelected && objectListDataGrid.SelectedRows.Count >= 2)
            {
                m_isSelected = true;
                foreach (DataGridViewRow r in objectListDataGrid.SelectedRows)
                {
                    EcellObject obj = r.Tag as EcellObject;
                    m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                }
                m_isSelected = false;
            }
        }

        private void DeletedSelectionRow()
        {
            List<DataGridViewRow> delrows = new List<DataGridViewRow>();
            foreach (DataGridViewRow r in objectListDataGrid.SelectedRows)
            {
                if (r.Tag == null) continue;
                delrows.Add(r);
            }

            for (int i = 0 ; i < delrows.Count ; i++ )
            {
                EcellObject obj = delrows[i].Tag as EcellObject;
                if (i == delrows.Count - 1)
                {
                    m_owner.DataManager.DataDelete(obj, true, true);
                }
                else
                {
                    m_owner.DataManager.DataDelete(obj, true, false);
                }
            }
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
                DeletedSelectionRow();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
