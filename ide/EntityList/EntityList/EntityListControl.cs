using System;
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
        /// <summary>
        /// Color of header.
        /// </summary>
        private Color m_headerColor = Color.Lavender;

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

        public EntityListControl(EntityList owner)
        {
            m_owner = owner;
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
                if (objectListDataGrid[s_indexID, i].Value.ToString().Equals(key) &&
                    objectListDataGrid[s_indexType, i].Value.ToString().Equals(type))
                {
                    objectListDataGrid.Rows[i].Selected = true;
                    objectListDataGrid.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }
        }

        public void RemoveSelection(string modelID, string key, string type)
        {
            for (int i = 0; i < objectListDataGrid.Rows.Count; i++)
            {
                if (objectListDataGrid[s_indexID, i].Value.ToString().Equals(key) &&
                    objectListDataGrid[s_indexType, i].Value.ToString().Equals(type))
                {
                    objectListDataGrid.Rows[i].Selected = false;
                    return;
                }
            }
        }

        public void DataAdd(EcellObject obj)
        {
            if (obj.Key.EndsWith(":SIZE")) return;
            int ind = SearchInsertPosition(obj.Key, obj.Type);
            int len = m_propArray.Length;

            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_propArray[i], obj);
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = data;
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
            int ind = SearchIndex(key, type);
            objectListDataGrid[s_indexClass, ind].Value = data.Classname;
            objectListDataGrid[s_indexName, ind].Value = data.Name;
        }

        public void DataDelete(string modelID, string key, string type)
        {
            int ind;
            if (type == EcellObject.SYSTEM)
            {
                ind = SearchIndexInclude(key);
                while (ind != -1)
                {
                    DeleteRow(ind);
                    SearchIndexInclude(key);
                }
                return;
            }

            ind = SearchIndex(key, type);
            while (ind == -1)
            {
                DeleteRow(ind);
                SearchIndex(key, type);
            }            
        }

        public void ClearSelection()
        {
            objectListDataGrid.ClearSelection();
        }

        public void Clear()
        {
            objectListDataGrid.Rows.Clear();
        }

        private void DeleteRow(int ind)
        {
            objectListDataGrid.Rows.RemoveAt(ind);
        }

        private string GetData(string name, EcellObject obj)
        {
            if (name.Equals(EntityListControl.s_indexType))
            {
                return obj.Type;
            }
            else if (name.Equals(EntityListControl.s_indexID))
            {
                return obj.Key;
            }
            else if (name.Equals(EntityListControl.s_indexClass))
            {
                return obj.Classname;
            }
            else if (name.Equals(EntityListControl.s_indexName))
            {
                return obj.Name;
            }
            else
            {
                foreach (EcellData d in obj.Value)
                {
                    if (name.Equals(d.Name))
                        return d.Value.ToString();
                }
            }

            return "";
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
            for ( i = 0 ; i < objectListDataGrid.Rows.Count ; i++ )
            {
                if (TypeConverter(type, objectListDataGrid[s_indexType, i].Value.ToString()) == 0)
                {
                    if (String.Compare(key, objectListDataGrid[s_indexClass, i].Value.ToString()) < 0)
                        return i;
                }
                else if (TypeConverter(type, objectListDataGrid[s_indexType, i].Value.ToString()) == 1)
                {
                    return i;
                }
            }
            return i;
        }

        private int SearchIndex(string key, string type)
        {
            for (int i = 0; i < objectListDataGrid.Rows.Count; i++)
            {
                if (objectListDataGrid[s_indexID, i].Value.ToString().Equals(key) &&
                    objectListDataGrid[s_indexType, i].Value.ToString().Equals(type))
                {
                    return i;
                }
            }
            return -1;
        }

        private int SearchIndexInclude(string key)
        {
            for (int i = 1; i < objectListDataGrid.Rows.Count; i++)
            {
                if (objectListDataGrid[s_indexID, i].Value.ToString().StartsWith(key))
                {
                    return i;
                }
            }
            return -1;
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
                r.Visible = r.Cells[s_indexID].Value.ToString().Contains(searchCnd)
                    || r.Cells[s_indexName].Value.ToString().Contains(searchCnd);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Clear();
        }
    }
}
