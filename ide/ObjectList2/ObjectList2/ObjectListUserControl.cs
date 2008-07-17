using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ecell.Objects;

namespace Ecell.IDE.Plugins.ObjectList2
{
    public partial class ObjectListUserControl : UserControl
    {
        private ObjectList2 m_owner;
        private bool m_isSelected = false;
        /// <summary>
        /// Color of header.
        /// </summary>
        private Color m_headerColor = Color.Lavender;

        /// <summary>
        /// The reserved name for the type of object.
        /// </summary>
        protected static string s_indexType = "Type";
        /// <summary>
        /// The reserved name for ID of object.
        /// </summary>
        protected static string s_indexID = "ID";
        /// <summary>
        /// The reserved name for the Model ID of object.
        /// </summary>
        protected static string s_indexModel = "Model";
        /// <summary>
        /// The reserved name for the class name of object.
        /// </summary>
        protected static string s_indexClass = "ClassName";
        /// <summary>
        /// The reserved name for the name of object.
        /// </summary>
        protected static string s_indexName = "Name";

                /// <summary>
        /// The property array of System.
        /// </summary>
        private String[] m_propArray = new string[] {
            ObjectListUserControl.s_indexType,
            ObjectListUserControl.s_indexID,
            ObjectListUserControl.s_indexModel,
            ObjectListUserControl.s_indexClass,
            ObjectListUserControl.s_indexName
        };

        public ObjectListUserControl(ObjectList2 owner)
        {
            m_owner = owner;
            InitializeComponent();

            for (int i = 0; i < m_propArray.Length; i++)
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.HeaderText = Convert.ToString(i);
                c.Name = Convert.ToString(i);
                objectListDataGrid.Columns.Add(c);
            }

            CreateHeader();
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
                if (objectListDataGrid[2, i].Value.ToString().Equals(modelID) &&
                    objectListDataGrid[1, i].Value.ToString().Equals(key) &&
                    objectListDataGrid[0, i].Value.ToString().Equals(type))
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
                if (objectListDataGrid[2, i].Value.ToString().Equals(modelID) &&
                    objectListDataGrid[1, i].Value.ToString().Equals(key) &&
                    objectListDataGrid[0, i].Value.ToString().Equals(type))
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
            objectListDataGrid[3, ind].Value = data.Classname;
            objectListDataGrid[4, ind].Value = data.Name;
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
            CreateHeader();
        }

        private void DeleteRow(int ind)
        {
            objectListDataGrid.Rows.RemoveAt(ind);
        }

        private string GetData(string name, EcellObject obj)
        {
            if (name.Equals(ObjectListUserControl.s_indexType))
            {
                return obj.Type;
            }
            else if (name.Equals(ObjectListUserControl.s_indexID))
            {
                return obj.Key;
            }
            else if (name.Equals(ObjectListUserControl.s_indexModel))
            {
                return obj.ModelID;
            }
            else if (name.Equals(ObjectListUserControl.s_indexClass))
            {
                return obj.Classname;
            }
            else if (name.Equals(ObjectListUserControl.s_indexName))
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
            int i = 1;
            for ( i = 1 ; i < objectListDataGrid.Rows.Count ; i++ )
            {
                if (TypeConverter(type, objectListDataGrid[0, i].Value.ToString()) == 0)
                {
                    if (String.Compare(key, objectListDataGrid[1, i].Value.ToString()) < 0)
                        return i;
                }
                else if (TypeConverter(type, objectListDataGrid[0, i].Value.ToString()) == 1)
                {
                    return i;
                }
            }
            return i;
        }

        private int SearchIndex(string key, string type)
        {
            for (int i = 1; i < objectListDataGrid.Rows.Count; i++)
            {
                if (objectListDataGrid[1, i].Value.ToString().Equals(key) &&
                    objectListDataGrid[0, i].Value.ToString().Equals(type))
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
                if (objectListDataGrid[1, i].Value.ToString().StartsWith(key))
                {
                    return i;
                }
            }
            return -1;
        }

        private void CreateHeader()
        {
            int len = m_propArray.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < len; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();
                cs1.Style.BackColor = m_headerColor;
                cs1.Value = m_propArray[i];
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            objectListDataGrid.Rows.Add(rs);
        }

        private void ClickSearchButton(object sender, EventArgs e)
        {
            string searchCnd = searchTextBox.Text;
            if (String.IsNullOrEmpty(searchCnd))
                return;
            int ind = 1;
            if (objectListDataGrid.SelectedRows.Count > 0)
                ind = objectListDataGrid.SelectedRows[0].Index;

            for (int i = ind + 1 ; i < objectListDataGrid.Rows.Count ; i++)
            {
                if (objectListDataGrid[1, i].Value.ToString().Contains(searchCnd))
                {
                    m_owner.PluginManager.SelectChanged(
                        objectListDataGrid[2, i].Value.ToString(),
                        objectListDataGrid[1, i].Value.ToString(),
                        objectListDataGrid[0, i].Value.ToString());
                    return;
                }
            }

            for ( int i = 0 ; i < ind ; i++ )
            {
                if (objectListDataGrid[1, i].Value.ToString().Contains(searchCnd))
                {
                    m_owner.PluginManager.SelectChanged(
                        objectListDataGrid[2, i].Value.ToString(),
                        objectListDataGrid[1, i].Value.ToString(),
                        objectListDataGrid[0, i].Value.ToString());
                    return;
                }
            }        
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

        private void SearchTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                searchButton.PerformClick();
            }
        }
    }
}
