using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.Objects;

namespace Ecell.IDE.Plugins.EntityList
{
    public partial class EntityListControl : UserControl
    {
        private EntityList m_owner;
        private bool m_isSelected = false;

        protected ImageList m_icons;

        /// <summary>
        /// The reserved name for the type of object.
        /// </summary>
        protected const string s_indexType = "Type";
        /// <summary>
        /// The reserved name for ID of object.
        /// </summary>
        protected const string s_indexID = "ID";
        /// <summary>
        /// The reserved name for the class name of object.
        /// </summary>
        protected const string s_indexClass = "ClassName";
        /// <summary>
        /// The reserved name for the name of object.
        /// </summary>
        protected const string s_indexName = "ObjectName";

                /// <summary>
        /// The property array of System.
        /// </summary>
        private static String[] m_propArray = new string[] {
            EntityListControl.s_indexType,
            EntityListControl.s_indexClass,
            EntityListControl.s_indexID,
            EntityListControl.s_indexName
        };

        public EntityListControl(EntityList owner, ImageList icons)
        {
            m_owner = owner;
            m_icons = icons;
            InitializeComponent();
        }

        public void SelectChanged(string modelID, string key, string type)
        {
            if (m_isSelected) return;
            objectListDataGrid.ClearSelection();
            AddSelection(modelID, key, type);
        }

        public void AddSelection(string modelID, string key, string type)
        {
            for (int i = 0; i < objectListDataGrid.Rows.Count; i++)
            {
                EcellObject obj = objectListDataGrid.Rows[i].Tag as EcellObject;
                if (obj.Key.Equals(key) && obj.Type.Equals(type))
                {
                    objectListDataGrid.Rows[i].Selected = true;
                    if (objectListDataGrid.Rows[i].Visible)
                        objectListDataGrid.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }
        }

        public void RemoveSelection(string modelID, string key, string type)
        {
            for (int i = 0; i < objectListDataGrid.Rows.Count; i++)
            {
                EcellObject obj = objectListDataGrid.Rows[i].Tag as EcellObject;
                if (obj.Key.Equals(key) && obj.Type.Equals(type))
                {
                    objectListDataGrid.Rows[i].Selected = false;
                    return;
                }
            }
        }

        public void DataAdd(EcellObject obj)
        {
            if (obj.Type != Constants.xpathSystem &&
                obj.Type != Constants.xpathVariable &&
                obj.Type != Constants.xpathProcess)
                return;

            if (obj.Key.EndsWith(":SIZE")) return;
            int ind = SearchInsertPosition(obj.Key, obj.Type);
            int len = m_propArray.Length;

            DataGridViewRow rs = new DataGridViewRow();
            {
                DataGridViewImageCell c = new DataGridViewImageCell();
                c.Value = m_icons.Images[obj.Type];
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
                c.Value = d.Value.ToString();
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }
            rs.Tag = obj;
            objectListDataGrid.Rows.Insert(ind, rs);
        }

        public void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            if (key != data.Key)
            {
                DataDelete(modelID, key, type);
                DataAdd(data);
                AddSelection(data.ModelID, data.Key, data.Type);
                return;
            }
            DataGridViewRow r = SearchIndex(key, type);
            if (r != null)
            {
                r.Cells[s_indexClass].Value = data.Classname;
                r.Cells[s_indexName].Value = data.Name;
            }
        }

        public void DataDelete(string modelID, string key, string type)
        {
            DataGridViewRow r = SearchIndex(type, key);
            if (r != null)
                objectListDataGrid.Rows.Remove(r);
        }

        public void ClearSelection()
        {
            objectListDataGrid.ClearSelection();
        }

        public void Clear()
        {
            objectListDataGrid.Rows.Clear();
        }

        private int TypeConverter(string type1, string type2)
        {
            if (type1 == type2) return 0;
            if (type1 == EcellObject.SYSTEM) return 1;
            if (type2 == EcellObject.SYSTEM) return -1;
            if (type1 == EcellObject.VARIABLE) return -1;
            return 1;
        }

        private int SearchInsertPosition(string key, string type)
        {
            int i = 0;
            for (; i < objectListDataGrid.Rows.Count ; i++)
            {
                EcellObject obj = objectListDataGrid.Rows[i].Tag as EcellObject;
                if (TypeConverter(type, obj.Type) == 0)
                {
                    if (String.Compare(key, obj.Type) < 0)
                        return i;
                }
                else if (TypeConverter(type, obj.Type) == 1)
                {
                    return i;
                }
            }
            return i;
        }

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
            if (objectListDataGrid.Rows[ind].Selected)
            {
                if (objectListDataGrid.SelectedRows.Count <= 1)
                    m_owner.PluginManager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
                else
                {
                    if (obj.Type.Equals(Constants.xpathSystem))
                    {
                        objectListDataGrid.ClearSelection();
                        objectListDataGrid.Rows[ind].Selected = true;
                        m_owner.PluginManager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
                    }
                    else
                    {
                        m_owner.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                    }
                }
            }
            else
            {
                m_owner.PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);
            }
            m_isSelected = false;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchCnd = searchTextBox.Text;
            foreach (DataGridViewRow r in objectListDataGrid.Rows)
            {
                r.Visible =
                    ((EcellObject)r.Tag).Key.Contains(searchCnd)
                    || ((EcellObject)r.Tag).Name.Contains(searchCnd);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
        }
    }
}
