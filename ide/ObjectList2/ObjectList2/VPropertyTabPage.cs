using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace EcellLib.ObjectList2
{
    /// <summary>
    /// 
    /// </summary>
    public class VPropertyTabPage
    {
        private Type m_Type;
        /// <summary>
        /// 
        /// </summary>
        protected DataGridView m_gridView;
        /// <summary>
        /// 
        /// </summary>
        protected TabPage m_tabPage;
        /// <summary>
        /// Color of header.
        /// </summary>
        protected Color m_headerColor = Color.Lavender;
        /// <summary>
        /// Dictionary of entity path and DataGridViewCell displayed the property of entity path.
        /// </summary>
        protected Dictionary<string, DataGridViewCell> m_propDic = 
            new Dictionary<string, DataGridViewCell>();


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
        /// The reserved name for the stepperID of object.
        /// </summary>
        protected static string s_indexStepper = "StepperID";
        /// <summary>
        /// The reserved name for the Size of object.
        /// </summary>
        protected static string s_indexSize = "Size";
        /// <summary>
        /// The reserved name for the MolarConc of object.
        /// </summary>
        protected static string s_indexMolarConc = "MolarConc";
        /// <summary>
        /// The reserved name for the activity of object.
        /// </summary>
        protected static string s_indexActivity = "Activity";
        /// <summary>
        /// The reserved name for the value of object.
        /// </summary>
        protected static string s_indexValue = "Value";
        /// <summary>
        /// The reserved name for the VariableReferenceList of object.
        /// </summary>
        protected static string s_indexVariableRefList = "VariableReferenceList";

        /// <summary>
        /// The property array of object that is not enable to edit.
        /// </summary>
        protected String[] m_notEditProp = new string[] {
            VPropertyTabPage.s_indexType,
            VPropertyTabPage.s_indexModel
        };


        /// <summary>
        /// The property array of object that is update the value when simulation is running.
        /// </summary>
        protected String[] m_simulationProp = new string[] {
            VPropertyTabPage.s_indexSize,
            VPropertyTabPage.s_indexActivity,
            VPropertyTabPage.s_indexValue,
            VPropertyTabPage.s_indexMolarConc
        };


        /// <summary>
        /// 
        /// </summary>
        public VPropertyTabPage()
        {
            m_gridView = new DataGridView();
            m_gridView.Dock = DockStyle.Fill;
            m_gridView.MultiSelect = true;
            m_gridView.AllowUserToAddRows = false;
            m_gridView.RowHeadersVisible = false;
            m_gridView.ColumnHeadersVisible = false;
            m_gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_gridView.CellClick += new DataGridViewCellEventHandler(ClickObjectCell);

            m_tabPage = new TabPage();
            m_tabPage.Text = GetTabPageName();
            m_tabPage.Controls.Add(m_gridView);

            for (int i = 0; i < ColumnNum; i++)
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.HeaderText = Convert.ToString(i);
                c.Name = Convert.ToString(i);
                m_gridView.Columns.Add(c);
            }
            CreateHeader();
        }

        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get { return this.m_Type; }
            set { this.m_Type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public virtual void DataAdd(EcellObject data)
        {
            // nothing.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        public void DataChanged(string modelID, string id, EcellObject obj)
        {
            bool isIDChanged = !(id == obj.Key);
            DataDelete(modelID, id, isIDChanged, m_Type);
            DataAdd(obj);
            int index = SearchObjectIndex(obj.Key);
            if (index < 0) return;
            m_gridView.Rows[index].Selected = true;
            m_gridView.FirstDisplayedScrollingRowIndex = index;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="id"></param>
        /// <param name="isChanged"></param>
        /// <param name="dType"></param>
        public virtual void DataDelete(string modelID, string id, bool isChanged, Type dType)
        {
            int ind = SearchObjectIndex(id);
            if (ind >= 0)
            {
                DeleteDictionary(ind);
                m_gridView.Rows.RemoveAt(ind);
            }
            if (dType == typeof(EcellSystem))
            {
                ind = SearchIncludeObjectIndex(id);
                while (ind != -1)
                {
                    DeleteDictionary(ind);
                    m_gridView.Rows.RemoveAt(ind);
                    ind = SearchIncludeObjectIndex(id);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void SelectChanged(string modelID, string id, string type)
        {
            int ind = SearchObjectIndex(id);
            if (ind < 0) return;
            m_gridView[0, ind].Selected = true;
            m_gridView.FirstDisplayedScrollingRowIndex = ind;
        }

        /// <summary>
        /// Event when the project is closed.
        /// </summary>
        public void Clear()
        {
            m_gridView.Rows.Clear();
            m_propDic.Clear();
            CreateHeader();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void CreateHeader()
        {
            // nothing.
        }

        /// <summary>
        /// Event when the selected object is added.
        /// </summary>
        /// <param name="modelId">ModelID of the selected object.</param>
        /// <param name="id">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public void AddSelection(string modelId, string id, string type)
        {
            int ind = SearchObjectIndex(id);
            if (ind < 0) return;
            m_gridView.Rows[ind].Selected = true;
            m_gridView.FirstDisplayedScrollingRowIndex = ind;
        }

        /// <summary>
        /// Event when the selected object is removed.
        /// </summary>
        /// <param name="modelId">ModelID of the removed object.</param>
        /// <param name="key">ID of the removed object.</param>
        /// <param name="type">Type of the removed object.</param>
        public void RemoveSelection(string modelId, string key, string type)
        {
            int ind = SearchObjectIndex(key);
            if (ind < 0) return;
            m_gridView.Rows[ind].Selected = false;
            m_gridView.FirstDisplayedScrollingRowIndex = ind;
        }

        /// <summary>
        /// Event when the selected object is changed to no select.
        /// </summary>
        public void ClearSelection()
        {
            m_gridView.ClearSelection();
        }

        /// <summary>
        /// Search the position index of object.
        /// </summary>
        /// <param name="key">ID of object.</param>
        /// <returns>The position index of object.</returns>
        protected int SearchObjectIndex(string key)
        {
            int len = m_gridView.Rows.Count;
            for (int i = 0; i < len; i++)
            {
                if (!key.Equals(m_gridView[1, i].Value)) continue;
                return i;
            }
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected int SearchIncludeObjectIndex(string key)
        {
            int len = m_gridView.Rows.Count;
            for (int i = 0; i < len; i++)
            {
                if (!m_gridView[1, i].Value.ToString().StartsWith(key)) continue;
                return i;
            }
            return -1;
        }

        /// <summary>
        /// Search the index of the inserted object.
        /// </summary>
        /// <param name="startpos">The header position.</param>
        /// <param name="id">The ID of inserted object.</param>
        /// <returns>The position index of inserted object.</returns>
        protected int SearchInsertIndex(int startpos, string id)
        {
            int preIndex = startpos + 1;
            for (int i = startpos + 1; i < m_gridView.Rows.Count; i++)
            {
                if (m_gridView.Rows[i].Tag == null) return m_gridView.Rows[i].Index;
                if (m_gridView[1, i].Value.ToString().CompareTo(id) > 0) return m_gridView.Rows[i].Index;
                preIndex = m_gridView.Rows[i].Index + 1;
            }
            return preIndex;
        }

        /// <summary>
        /// Search the position of header by type.
        /// </summary>
        /// <returns>The position index of header.</returns>
        protected int SearchHeaderPos()
        {
            for (int i = 0; i < m_gridView.Rows.Count; i++)
            {
                if (m_gridView.Rows[i].Tag == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get tab name.
        /// </summary>
        /// <returns>name of tab.</returns>
        public virtual string GetTabPageName()
        {
            return "";
        }

        /// <summary>
        /// Get TabPage.
        /// </summary>
        /// <returns>TabPage.</returns>
        public TabPage GetTabPage()
        {
            return m_tabPage;
        }

        /// <summary>
        /// The property of object is updated when simulation is runniing.
        /// </summary>
        public void UpdatePropForSimulation()
        {
            DataManager manager = DataManager.GetDataManager();
            double ctime = manager.GetCurrentSimulationTime();
            if (ctime == 0.0) return;

            foreach (string entPath in m_propDic.Keys)
            {
                EcellValue v = manager.GetEntityProperty(entPath);
                if (v == null) continue;
                m_propDic[entPath].Value = v.Value.ToString();
            }
        }

        /// <summary>
        /// Delete the entry from dictionaty.
        /// </summary>
        /// <param name="index">index of removed entry.</param>
        protected void DeleteDictionary(int index)
        {
            while (true)
            {
                bool isHit = false;
                foreach (string entPath in m_propDic.Keys)
                {
                    DataGridViewCell c = m_propDic[entPath];
                    if (c.RowIndex == index)
                    {
                        m_propDic.Remove(entPath);
                        isHit = true;
                        break;
                    }
                }
                if (isHit == false) break;
            }
        }

        /// <summary>
        /// Get the data string from entity name.
        /// </summary>
        /// <param name="name">The entity name.</param>
        /// <param name="obj">The searched object.</param>
        /// <returns>the data string.</returns>
        protected string GetData(string name, EcellObject obj)
        {
            if (name.Equals(VPropertyTabPage.s_indexType))
            {
                return obj.Type;
            }
            else if (name.Equals(VPropertyTabPage.s_indexID))
            {
                return obj.Key;
            }
            else if (name.Equals(VPropertyTabPage.s_indexModel))
            {
                return obj.ModelID;
            }
            else if (name.Equals(VPropertyTabPage.s_indexClass))
            {
                return obj.Classname;
            }
            else if (name.Equals(VPropertyTabPage.s_indexName))
            {
                return obj.Name;
            }
            else if (name.Equals(VPropertyTabPage.s_indexStepper))
            {
                foreach (EcellData d in obj.Value)
                {
                    if (d.Name.Equals(Constants.xpathStepperID)) return d.Value.ToString();
                }
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


        /// <summary>
        /// 
        /// </summary>
        public virtual int ColumnNum
        {
            get { return 0; }
        }

        /// <summary>
        /// Event when system search the object by text.
        /// </summary>
        /// <param name="text">search condition.</param>
        public void SearchInstance(string text)
        {
            int ind = -1;
            if (m_gridView.SelectedRows.Count > 0)
            {
                ind = m_gridView.SelectedRows[0].Index;
            }
            int len = m_gridView.Rows.Count;
            for (int i = ind + 1; i < len; i++)
            {
                for (int j = 0; j < ColumnNum; j++)
                {
                    if (m_gridView[j, i].Value.ToString().Contains(text))
                    {
                        ClearSelection();
                        m_gridView.Rows[i].Selected = true;
                        m_gridView.FirstDisplayedScrollingRowIndex = i;
                        return;
                    }
                }
            }
            String mes = ObjectList2.s_resources.GetString("ErrNotFindPage");
            MessageBox.Show(mes, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Event when the cell is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClickObjectCell(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;
            EcellObject obj = m_gridView.Rows[index].Tag as EcellObject;
            if (obj == null) return;
            PluginManager manager = PluginManager.GetPluginManager();
            manager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
        }
    }
}
