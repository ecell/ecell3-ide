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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
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
    /// <summary>
    /// Entity list control object.
    /// </summary>
    public partial class EntityListControl : UserControl
    {
        #region Fields
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
        /// The selected row.
        /// </summary>
        private DataGridViewRow m_selectedRow = null;
        /// <summary>
        /// The last selected row.
        /// </summary>
        private DataGridViewRow m_lastSelected = null;
        /// <summary>
        /// The flag whether the reset action is executing.
        /// </summary>
        private bool m_intact = false;
        /// <summary>
        /// The flag whether the column of type is shown.
        /// </summary>
        private bool m_isShowType = true;
        /// <summary>
        /// The flag whether the column of name is shown.
        /// </summary>
        private bool m_isShowName = true;
        /// <summary>
        /// The flag whether the column of class name is shown.
        /// </summary>
        private bool m_isShowClassName = true;
        /// <summary>
        /// The flag whether ths column of Path ID is shown.
        /// </summary>
        private bool m_isShowPathID = true;
        #endregion

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
        /// <param name="owner">the owner object.</param>
        /// <param name="icons">the list of icon.</param>
        public EntityListControl(EntityList owner, ImageList icons)
        {
            m_owner = owner;
            m_iconList = icons;
            m_owner.PluginManager.NodeImageListChange += new EventHandler(PluginManager_NodeImageListChange);
            InitializeComponent();
            ResetSearchTextBox();
        }
        #endregion

        #region Inherited from PluginBase
        /// <summary>
        /// Event on SelectChange
        /// </summary>
        /// <param name="modelID">the model ID of selected object.</param>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
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
            m_shiftIndex = -1;
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
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
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            objectListDataGrid.ClearSelection();
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null)
                row.Selected = false;
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="obj">The value of the adding object.</param>
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

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
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
                r.Cells[IndexType].Value = GetIconImage(data);
                r.Cells[IndexClass].Value = data.Classname;
                EcellData d = data.GetEcellData("Name");
                r.Cells[IndexName].Value = d != null ? d.Value.ToString() : "";
            }
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
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
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            objectListDataGrid.Rows.Clear();
            m_selectedRow = null;
            m_lastSelected = null;
        }

        #endregion

        /// <summary>
        /// Converter for Type.
        /// </summary>
        /// <param name="type1">the type string of object.</param>
        /// <param name="type2">the type string of object.</param>
        /// <returns>stepper, variable, system, process.</returns>
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
        /// Search the insert position of object.
        /// </summary>
        /// <param name="key">the key of object.</param>
        /// <param name="type">the type of object.</param>
        /// <returns>the insert position.</returns>
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
        /// Search System object start with ID.
        /// </summary>
        /// <param name="id">the key of object.</param>
        /// <returns>the rows of System object.</returns>
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
        /// Search the object in DataGridView.
        /// </summary>
        /// <param name="type">the type of object.</param>
        /// <param name="id">the key of object.</param>
        /// <returns>the hit row.</returns>
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
        /// Get icon image from object.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <returns>Icon image.</returns>
        private Image GetIconImage(EcellObject obj)
        {
            Image image;
            if (m_iconList.Images.ContainsKey(obj.Layout.Figure))
                image = m_iconList.Images[obj.Layout.Figure];
            else
                image = m_iconList.Images[obj.Type];
            return image;
        }

        /// <summary>
        /// Reset TextBox for search.
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
        /// Enter the drag mode.
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
                if (objectListDataGrid.SelectedRows.Count > 1)
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
        /// Create the new row for Object.
        /// </summary>
        /// <param name="obj">the inserted object.</param>
        /// <returns>the inserted row.</returns>
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
        /// Delete the selected all row.
        /// </summary>
        private void DeletedSelectionRow()
        {
            List<DataGridViewRow> delrows = new List<DataGridViewRow>();
            foreach (DataGridViewRow r in objectListDataGrid.SelectedRows)
            {
                if (r.Tag == null) continue;
                delrows.Add(r);
            }

            try
            {
                for (int i = 0; i < delrows.Count; i++)
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
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        private int m_shiftIndex = -1;
        /// <summary>
        /// Press key on DataGridView.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="keyData">Key data.</param>
        /// <returns>the flag whether this event is handled.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Shortcuts to control searchTextBox
            if (this.searchTextBox.Focused)
            {
                if ((int)keyData == (int)Keys.Control + (int)Keys.C)
                {
                    searchTextBox.Copy();
                    return true;
                }
                else if ((int)keyData == (int)Keys.Control + (int)Keys.X)
                {
                    searchTextBox.Cut();
                    return true;
                }
                else if ((int)keyData == (int)Keys.Control + (int)Keys.V)
                {
                    searchTextBox.Paste();
                    return true;
                }
                else if ((int)keyData == (int)Keys.Control + (int)Keys.A)
                {
                    searchTextBox.SelectAll();
                    return true;
                }
            }

            // Shortcuts to select entities.
            if ((int)keyData == (int)Keys.Control + (int)Keys.D ||
                (int)keyData == (int)Keys.Delete)
            {
                DeletedSelectionRow();
                return true;
            }
            else if ((int)keyData == (int)Keys.Up + (int)Keys.Shift)
            {
                if (objectListDataGrid.CurrentRow != null && !m_isSelected)
                {
                    int rindex = objectListDataGrid.CurrentCell.RowIndex;
                    if (m_shiftIndex > 0)
                    {
                        if (m_shiftIndex <= rindex)
                        {
                            EcellObject obj = objectListDataGrid.Rows[m_shiftIndex - 1].Tag as EcellObject;
                            if (obj != null)
                            {
                                objectListDataGrid.Rows[m_shiftIndex - 1].Selected = true;
                                m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                        }
                        else if (m_shiftIndex > rindex)
                        {
                            EcellObject obj = objectListDataGrid.Rows[m_shiftIndex].Tag as EcellObject;
                            if (obj != null)
                            {
                                objectListDataGrid.Rows[m_shiftIndex].Selected = false;
                                m_owner.PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                        }
                        m_shiftIndex = m_shiftIndex -1;
                    }
                    else if (m_shiftIndex == -1)
                    {
                        if (rindex > 0)
                        {
                            m_shiftIndex = rindex - 1;
                            EcellObject obj = objectListDataGrid.Rows[m_shiftIndex].Tag as EcellObject;
                            if (obj != null)
                            {
                                objectListDataGrid.Rows[m_shiftIndex].Selected = true;
                                m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                        }
                    }
                }
                return true;
            }
            else if ((int)keyData == (int)Keys.Down + (int)Keys.Shift)
            {
                if (objectListDataGrid.CurrentRow != null && !m_isSelected)
                {
                    int rindex = objectListDataGrid.CurrentCell.RowIndex;
                    if (m_shiftIndex != -1 && m_shiftIndex + 1 < objectListDataGrid.Rows.Count)
                    {
                        if (m_shiftIndex >= rindex)
                        {
                            EcellObject obj = objectListDataGrid.Rows[m_shiftIndex + 1].Tag as EcellObject;
                            if (obj != null)
                            {
                                objectListDataGrid.Rows[m_shiftIndex + 1].Selected = true;
                                m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                        }
                        else if (m_shiftIndex < rindex)
                        {
                            EcellObject obj = objectListDataGrid.Rows[m_shiftIndex].Tag as EcellObject;
                            if (obj != null)
                            {
                                objectListDataGrid.Rows[m_shiftIndex].Selected = false;
                                m_owner.PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                        }
                        m_shiftIndex = m_shiftIndex + 1;
                    }
                    else if (m_shiftIndex == -1)
                    {
                        if (rindex < objectListDataGrid.Rows.Count - 1)
                        {
                            m_shiftIndex = rindex + 1;
                            EcellObject obj = objectListDataGrid.Rows[m_shiftIndex].Tag as EcellObject;
                            if (obj != null)
                            {
                                objectListDataGrid.Rows[m_shiftIndex].Selected = true;
                                m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                            }
                        }
                    }
                }
                return true;
            }
            else if ((int)keyData == (int)Keys.Up)
            {
                if (objectListDataGrid.CurrentRow != null && !m_isSelected)
                {
                    int rindex = objectListDataGrid.CurrentCell.RowIndex;
                    int cindex = objectListDataGrid.CurrentCell.ColumnIndex;
                    if (rindex > 0)
                    {
                        m_isSelected = true;
                        EcellObject obj = objectListDataGrid.Rows[rindex - 1].Tag as EcellObject;
                        if (obj != null)
                        {
                            m_owner.PluginManager.SelectChanged(obj);
                        }
                        m_isSelected = false;
                        objectListDataGrid.CurrentCell = objectListDataGrid[cindex, rindex - 1];
                        m_selectedRow = null;
                        m_shiftIndex = -1;
                    }
                }
                return true;
            }
            else if ((int)keyData == (int)Keys.Down)
            {
                if (objectListDataGrid.CurrentRow != null && !m_isSelected)
                {
                    int rindex = objectListDataGrid.CurrentCell.RowIndex;
                    int cindex = objectListDataGrid.CurrentCell.ColumnIndex;
                    if (rindex + 1 < objectListDataGrid.Rows.Count)
                    {
                        m_isSelected = true;
                        EcellObject obj = objectListDataGrid.Rows[rindex + 1].Tag as EcellObject;
                        if (obj != null)
                        {
                            m_owner.PluginManager.SelectChanged(obj);
                        }
                        m_isSelected = false;
                        objectListDataGrid.CurrentCell = objectListDataGrid[cindex, rindex + 1];
                        m_selectedRow = null;
                        m_shiftIndex = -1;
                    }
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        #region Events
        /// <summary>
        /// Event when the cell is clicked.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
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
                {
                    m_owner.PluginManager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
                    m_shiftIndex = -1;
                }
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
        /// Event when the list of node image is changed.
        /// </summary>
        /// <param name="sender">PluginManager</param>
        /// <param name="e">EventArgs</param>
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
        /// Click the clear button.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs</param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            ResetSearchTextBox();
        }

        /// <summary>
        /// Enter the TextBox for search.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).ForeColor = SystemColors.WindowText;
            if (m_intact)
                ((TextBox)sender).Clear();
        }

        /// <summary>
        /// Change the text of TextBox.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
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
                    || ((string)r.Cells[IndexName].Value).ToLower().Contains(searchCnd.ToLower())
                    || ((string)r.Cells[IndexClass].Value).ToLower().Contains(searchCnd.ToLower());
            }
            objectListDataGrid.ResumeLayout();
        }

        /// <summary>
        /// Leave the mouse from TextBox for search.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">EventArgs</param>
        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length == 0)
                ResetSearchTextBox();
        }

        /// <summary>
        /// Mouse move on DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">MouseEventArgs</param>
        private void DataGridViewMouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if (m_dragObject == null) return;
            EnterDragMode();
            m_dragObject = null;
        }

        /// <summary>
        /// Mouse leave on DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">EventArgs</param>
        private void DataGridViewMouseLeave(object sender, EventArgs e)
        {
            m_isSelected = false;
        }

        /// <summary>
        /// Mouse up on DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">MouseEventArgs</param>
        private void DataGridViewMouseUp(object sender, MouseEventArgs e)
        {
            m_selectedRow = null;
            m_isSelected = false;
        }
        /// <summary>
        /// Mouse down on DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">MouseEventArgs</param>
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
                m_shiftIndex = -1;
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
        /// Click the menu of show column.
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">EventArgs</param>
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

        /// <summary>
        /// Compare the row of DataGridView to use Type.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewSortCompareEventArgs</param>
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

        /// <summary>
        /// Change the selection of DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">EventArgs</param>
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
        #endregion
    }
}
