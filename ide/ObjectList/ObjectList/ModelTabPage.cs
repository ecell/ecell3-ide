//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2007 Keio University
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
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace EcellLib.ObjectList
{
    /// <summary>
    /// TabPage to display the property of model.
    /// </summary>
    class ModelTabPage : IObjectListTabPage
    {
        /// <summary>
        /// TabPage to dock DataGridView.
        /// </summary>
        private TabPage m_tabPage;
        /// <summary>
        /// DataGridView to display the property of model.
        /// </summary>
        private DataGridView m_gridView;
        /// <summary>
        /// Popup menu.
        /// </summary>
        private ContextMenuStrip m_contextMenu;
        /// <summary>
        /// Color of header.
        /// </summary>
        private Color m_headerColor = Color.Lavender;
        /// <summary>
        /// Timer for executing redraw event at each 0.5 minutes.
        /// </summary>
        private System.Windows.Forms.Timer m_time;
        /// <summary>
        /// The ID of the selected Model now.
        /// </summary>
        private string m_currentModelID = null;
        /// <summary>
        /// The status of selected Model now.
        /// </summary>
        private ProjectStatus m_type = ProjectStatus.Uninitialized;
        /// <summary>
        /// Dictionary of entity path and DataGridViewCell displayed the property of entity path.
        /// </summary>
        private Dictionary<string, DataGridViewCell> m_propDic = new Dictionary<string, DataGridViewCell>();

        /// <summary>
        /// The property array of System.
        /// </summary>
        private String[] m_systemProp = new string[] {
            ModelTabPage.s_indexType, 
            ModelTabPage.s_indexID, 
            ModelTabPage.s_indexModel, 
            ModelTabPage.s_indexClass,
            ModelTabPage.s_indexName, 
            ModelTabPage.s_indexStepper, 
            ModelTabPage.s_indexSize
        };
        /// <summary>
        /// The property array of Variable.
        /// </summary>
        private String[] m_variableProp = new string[] {
            ModelTabPage.s_indexType, 
            ModelTabPage.s_indexID, 
            ModelTabPage.s_indexModel, 
            ModelTabPage.s_indexClass, 
            ModelTabPage.s_indexName, 
            ModelTabPage.s_indexValue,
            ModelTabPage.s_indexMolarConc
        };
        /// <summary>
        /// The property array of Process.
        /// </summary>
        private String[] m_processProp = new string[] {
            ModelTabPage.s_indexType,
            ModelTabPage.s_indexID, 
            ModelTabPage.s_indexModel, 
            ModelTabPage.s_indexClass,
            ModelTabPage.s_indexName, 
            ModelTabPage.s_indexStepper, 
            ModelTabPage.s_indexActivity, 
            ModelTabPage.s_indexVariableRefList
        };
        /// <summary>
        /// The property array of object that is not enable to edit.
        /// </summary>
        private String[] m_notEditProp = new string[] {
            ModelTabPage.s_indexType,
            ModelTabPage.s_indexModel
        };
        /// <summary>
        /// The property array of object that is update the value when simulation is running.
        /// </summary>
        private String[] m_simulationProp = new string[] {
            ModelTabPage.s_indexSize,
            ModelTabPage.s_indexActivity,
            ModelTabPage.s_indexValue,
            ModelTabPage.s_indexMolarConc
        };

        #region STATICS
        /// <summary>
        /// Max number of column.
        /// </summary>
        private static int s_columnNum = 8;
        /// <summary>
        /// The reserved name for the type of object.
        /// </summary>
        private static string s_indexType = "Type";
        /// <summary>
        /// The reserved name for ID of object.
        /// </summary>
        private static string s_indexID = "ID";
        /// <summary>
        /// The reserved name for the Model ID of object.
        /// </summary>
        private static string s_indexModel = "Model";
        /// <summary>
        /// The reserved name for the class name of object.
        /// </summary>
        private static string s_indexClass = "ClassName";
        /// <summary>
        /// The reserved name for the name of object.
        /// </summary>
        private static string s_indexName = "Name";
        /// <summary>
        /// The reserved name for the stepperID of object.
        /// </summary>
        private static string s_indexStepper = "StepperID";
        /// <summary>
        /// The reserved name for the Size of object.
        /// </summary>
        private static string s_indexSize = "Size";
        /// <summary>
        /// The reserved name for the MolarConc of object.
        /// </summary>
        private static string s_indexMolarConc = "MolarConc";
        /// <summary>
        /// The reserved name for the activity of object.
        /// </summary>
        private static string s_indexActivity = "Activity";
        /// <summary>
        /// The reserved name for the value of object.
        /// </summary>
        private static string s_indexValue = "Value";
        /// <summary>
        /// The reserved name for the VariableReferenceList of object.
        /// </summary>
        private static string s_indexVariableRefList = "VariableReferenceList";
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModelTabPage()
        {
            m_tabPage = new TabPage();
            m_gridView = new DataGridView();
            m_gridView.Dock = DockStyle.Fill;
            m_gridView.MultiSelect = true;
            m_gridView.AllowUserToAddRows = false;
            m_gridView.RowHeadersVisible = false;
            m_gridView.ColumnHeadersVisible = false;
            m_gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_gridView.CellClick += new DataGridViewCellEventHandler(ClickObjectCell);
            m_tabPage.Controls.Add(m_gridView);
            m_tabPage.Text = GetTabPageName();

            for (int i = 0; i < ModelTabPage.s_columnNum; i++)
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.HeaderText = Convert.ToString(i);
                c.Name = Convert.ToString(i);
                m_gridView.Columns.Add(c);
            }
            CreateSystemHeader();
            CreateVariableHeader();
            CreateProcessHeader();

            m_contextMenu = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = ObjectList.s_resources.GetString("SearchMenuText");
            it.ShortcutKeys = Keys.Control | Keys.F;
            it.Click += new EventHandler(ClickSearchMenu);

            m_contextMenu.Items.AddRange(new ToolStripItem[] { it });

            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(FireTimer);
        }



        /// <summary>
        /// The property of object is updated when simulation is runniing.
        /// </summary>
        private void UpdatePropForSimulation()
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
        /// The property of object is reset when simulation is stopped.
        /// </summary>
        private void ResetPropForSimulation()
        {
            DataManager manager = DataManager.GetDataManager();
            List<EcellObject> list = manager.GetData(m_currentModelID, null);
            Clear();
            DataAdd(list);
        }

        /// <summary>
        /// Search the position of header by type.
        /// </summary>
        /// <param name="type">Type to searched header.</param>
        /// <returns>The position index of header.</returns>
        private int SearchHeaderPos(string type)
        {
            int typeNum = 0;
            for (int i = 0; i < m_gridView.Rows.Count; i++)
            {
                if (m_gridView.Rows[i].Tag != null)
                    continue;

                if (type == Constants.xpathSystem && typeNum == 0)
                    return i;
                else if (type == Constants.xpathVariable && typeNum == 1)
                    return i;
                else if (type == Constants.xpathProcess && typeNum == 2)
                    return i;
                typeNum++;
            }
            return -1;
        }

        /// <summary>
        /// Search the index of the inserted object.
        /// </summary>
        /// <param name="startpos">The header position.</param>
        /// <param name="id">The ID of inserted object.</param>
        /// <returns>The position index of inserted object.</returns>
        private int SearchInsertIndex(int startpos, string id)
        {
            int preIndex = startpos + 1;
            for (int i = startpos + 1; i < m_gridView.Rows.Count; i++)
            {
                if (m_gridView.Rows[i].Tag == null) 
                    return m_gridView.Rows[i].Index;
                if (m_gridView[1, i].Value.ToString().CompareTo(id) > 0) 
                    return m_gridView.Rows[i].Index;
                preIndex = m_gridView.Rows[i].Index;
            }
            return preIndex;
        }

        /// <summary>
        /// Search the position index of object.
        /// </summary>
        /// <param name="key">ID of object.</param>
        /// <param name="type">Type of object</param>
        /// <returns>The position index of object.</returns>
        private int SearchObjectIndex(string key, string type)
        {
            int len = m_gridView.Rows.Count;
            for (int i = 0; i < len; i++)
            {
                if (!type.Equals(m_gridView[0, i].Value)) 
                    continue;
                if (!key.Equals(m_gridView[1, i].Value)) 
                    continue;
                return i;
            }
            return -1;
        }

        /// <summary>
        /// Create the header of system.
        /// </summary>
        private void CreateSystemHeader()
        {
            int len = m_systemProp.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < ModelTabPage.s_columnNum; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();
                cs1.Style.BackColor = m_headerColor;
                if (i < len)
                    cs1.Value = m_systemProp[i];
                else
                    cs1.Value = "";
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            m_gridView.Rows.Add(rs);
        }

        /// <summary>
        /// Create the header of variable.
        /// </summary>
        private void CreateVariableHeader()
        {
            int len = m_variableProp.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < ModelTabPage.s_columnNum; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();
                cs1.Style.BackColor = m_headerColor;
                if (i < len)
                    cs1.Value = m_variableProp[i];
                else
                    cs1.Value = "";
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            m_gridView.Rows.Add(rs);
        }

        /// <summary>
        /// Create the header of process.
        /// </summary>
        private void CreateProcessHeader()
        {
            int len = m_processProp.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < ModelTabPage.s_columnNum; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();
                cs1.Style.BackColor = m_headerColor;
                if (i < len)
                    cs1.Value = m_processProp[i];
                else
                    cs1.Value = "";
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            m_gridView.Rows.Add(rs);
        }

        /// <summary>
        /// Insert the object at the set position index.
        /// </summary>
        /// <param name="index">tTe position index.</param>
        /// <param name="obj">The inserted object.</param>
        private void AddSystem(int index, EcellObject obj)
        {
            int len = m_systemProp.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_systemProp[i], obj);
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = data;
                rs.Cells.Add(c);
                c.ReadOnly = true;
                foreach (string name in m_notEditProp)
                {
                    if (!name.Equals(m_systemProp[i]))
                        continue;
                    c.ReadOnly = true;
                    break;
                }
                foreach (string name in m_simulationProp)
                {
                    if (!name.Equals(m_systemProp[i]))
                        continue;
                    string entPath = Util.ConvertSystemEntityPath(obj.Key, name);
                    m_propDic.Add(entPath, c);
                    break;
                }
            }
            for (int i = len; i < ModelTabPage.s_columnNum; i++)
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = "";
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }

            rs.Tag = obj;
            m_gridView.Rows.Insert(index, rs);
        }

        /// <summary>
        /// Insert the object at the set position index.
        /// </summary>
        /// <param name="index">tTe position index.</param>
        /// <param name="obj">The inserted object.</param>
        private void AddVariable(int index, EcellObject obj)
        {
            int len = m_variableProp.Length;
            if (obj.Key.EndsWith(":SIZE")) return;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_variableProp[i], obj);
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = data;
                rs.Cells.Add(c);
                c.ReadOnly = true;
                foreach (string name in m_notEditProp)
                {
                    if (!name.Equals(m_variableProp[i]))
                        continue;
                    c.ReadOnly = true;
                    break;
                }
                foreach (string name in m_simulationProp)
                {
                    if (!name.Equals(m_variableProp[i]))
                        continue;
                    string entPath = Constants.xpathVariable +
                        Constants.delimiterColon + obj.Key +
                        Constants.delimiterColon + name;
                    m_propDic.Add(entPath, c);
                    break;
                }
            }
            for (int i = len; i < ModelTabPage.s_columnNum; i++)
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = "";
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }
            rs.Tag = obj;
            m_gridView.Rows.Insert(index, rs);
        }

        /// <summary>
        /// Insert the object at the set position index.
        /// </summary>
        /// <param name="index">tTe position index.</param>
        /// <param name="obj">The inserted object.</param>
        private void AddProcess(int index, EcellObject obj)
        {
            int len = m_processProp.Length;
            DataGridViewRow rs = new DataGridViewRow();
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_processProp[i], obj);
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = data;
                rs.Cells.Add(c);
                c.ReadOnly = true;
                foreach (string name in m_notEditProp)
                {
                    if (!name.Equals(m_processProp[i]))
                        continue;
                    c.ReadOnly = true;
                    break;
                }
                foreach (string name in m_simulationProp)
                {
                    if (!name.Equals(m_processProp[i]))
                        continue;
                    string entPath = Constants.xpathProcess +
                        Constants.delimiterColon + obj.Key +
                        Constants.delimiterColon + name;
                    m_propDic.Add(entPath, c);
                    break;
                }

            }
            for (int i = len; i < ModelTabPage.s_columnNum; i++)
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = "";
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }

            rs.Tag = obj;
            m_gridView.Rows.Insert(index, rs);
        }

        /// <summary>
        /// Add the object to DataGridView.
        /// </summary>
        /// <param name="obj">The added object.</param>
        private void DataAdd(EcellObject obj)
        {
            int pos = SearchHeaderPos(obj.Type);
            if (pos == -1) return;
            int index = SearchInsertIndex(pos, obj.Key);
            if (obj.Type.Equals(Constants.xpathSystem))
                AddSystem(index, obj);
            else if (obj.Type.Equals(Constants.xpathProcess))
                AddProcess(index, obj);
            else if (obj.Type.Equals(Constants.xpathVariable))
                AddVariable(index, obj);
            m_currentModelID = obj.ModelID;
        }

        /// <summary>
        /// Get the data string from entity name.
        /// </summary>
        /// <param name="name">The entity name.</param>
        /// <param name="obj">The searched object.</param>
        /// <returns>the data string.</returns>
        private string GetData(string name, EcellObject obj)
        {
            if (name.Equals(ModelTabPage.s_indexType))
            {
                return obj.Type;
            }
            else if (name.Equals(ModelTabPage.s_indexID))
            {
                return obj.Key;
            }
            else if (name.Equals(ModelTabPage.s_indexModel))
            {
                return obj.ModelID;
            }
            else if (name.Equals(ModelTabPage.s_indexClass))
            {
                return obj.Classname;
            }
            else if (name.Equals(ModelTabPage.s_indexName))
            {
                return obj.Name;
            }
            else if (name.Equals(ModelTabPage.s_indexStepper))
            {
                foreach (EcellData d in obj.Value)
                {
                    if (d.Name.Equals(Constants.xpathStepperID))
                        return d.Value.ToString();
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
        /// Delete the entry from dictionaty.
        /// </summary>
        /// <param name="index">index of removed entry.</param>
        private void DeleteDictionary(int index)
        {
            while (true)
            {
                bool isHit = false;
                foreach (string entPath in m_propDic.Keys)
                {
                    DataGridViewCell c = m_propDic[entPath];
                    if (c.RowIndex != index)
                        continue;

                    m_propDic.Remove(entPath);
                    isHit = true;
                    break;
                }
                if (isHit == false) break;
            }
        }

        #region IObjectListTabPage
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
                for (int j = 0; j < s_columnNum; j++)
                {
                    if (!m_gridView[j, i].Value.ToString().Contains(text))
                        continue;

                    ClearSelection();
                    if (i < 0 || i > m_gridView.RowCount - 1)
                        continue;
                    if (!m_gridView.Rows[i].Visible || m_gridView.Rows[i].Frozen)
                        continue;
                    m_gridView.Rows[i].Selected = true;
                    m_gridView.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }
            String mes = ObjectList.s_resources.GetString("ErrNotFindPage");
            MessageBox.Show(mes, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Event when the project is closed.
        /// </summary>
        public void Clear()
        {
            m_gridView.Rows.Clear();
            m_propDic.Clear();
            CreateSystemHeader();
            CreateVariableHeader();
            CreateProcessHeader();
            m_currentModelID = null;
        }

        /// <summary>
        /// Event when the selected object is added.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="key">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public void AddSelection(string modelID, string key, string type)
        {
            int ind = SearchObjectIndex(key, type);
            if (ind < 0 || ind > m_gridView.RowCount - 1)
                return;
            if (!m_gridView.Rows[ind].Visible || m_gridView.Rows[ind].Frozen)
                return;
            m_gridView.Rows[ind].Selected = true;
            m_gridView.FirstDisplayedScrollingRowIndex = ind;
        }

        /// <summary>
        /// Event when the selected object is removed.
        /// </summary>
        /// <param name="modelID">ModelID of the removed object.</param>
        /// <param name="key">ID of the removed object.</param>
        /// <param name="type">Type of the removed object.</param>
        public void RemoveSelection(string modelID, string key, string type)
        {
            int ind = SearchObjectIndex(key, type);
            if (ind < 0 || ind > m_gridView.RowCount - 1)
                return;
            if (!m_gridView.Rows[ind].Visible || m_gridView.Rows[ind].Frozen) 
                return;
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
        /// Event when object is selected.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="id">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public void SelectChanged(string modelID, string id, string type)
        {
            ClearSelection();
            int ind = SearchObjectIndex(id, type);
            if (ind < 0 || ind > m_gridView.RowCount - 1)
                return;
            if (!m_gridView.Rows[ind].Visible || m_gridView.Rows[ind].Frozen)
                return;
            m_gridView[0, ind].Selected = true;
            m_gridView.FirstDisplayedScrollingRowIndex = ind;
        }

        /// <summary>
        /// Event when object is added.
        /// </summary>
        /// <param name="objList">the list of added object.</param>
        public void DataAdd(List<EcellObject> objList)
        {
            foreach (EcellObject obj in objList)
            {
                if (obj.Type.Equals(Constants.xpathModel))
                    continue;
                if (m_currentModelID != null && !m_currentModelID.Equals(obj.ModelID))
                    continue;
                DataAdd(obj);
                if (obj.Children == null)
                    continue;
                foreach (EcellObject cobj in obj.Children)
                {
                    DataAdd(cobj);
                }
            }
        }

        /// <summary>
        /// Event when the property of object is changed.
        /// </summary>
        /// <param name="modelID">ModelID of the changed object.</param>
        /// <param name="id">ID of the changed object.</param>
        /// <param name="type">Type of the changed object.</param>
        /// <param name="obj">The changed object.</param>
        public void DataChanged(string modelID, string id, string type, EcellObject obj)
        {
            bool isIDChanged = !(id == obj.Key);
            DataDelete(modelID, id, type, isIDChanged);
            DataAdd(obj);
            int index = SearchObjectIndex(obj.Key, obj.Type);
            if (index < 0 || index > m_gridView.RowCount - 1)
                return;
            if (!m_gridView.Rows[ind].Visible || m_gridView.Rows[ind].Frozen)
                return;
            m_gridView.Rows[index].Selected = true;
            m_gridView.FirstDisplayedScrollingRowIndex = index;
        }

        /// <summary>
        /// Event when object is deleted.
        /// </summary>
        /// <param name="modelID">ModelID of the deleted object.</param>
        /// <param name="id">ID of the deleted object.</param>
        /// <param name="type">Type of the deleted object.</param>
        /// <param name="isChanged">whether id is changed.</param>
        public void DataDelete(string modelID, string id, string type, bool isChanged)
        {
            int ind = SearchObjectIndex(id, type);
            if (ind < 0) return;
            if (type.Equals(Constants.xpathSystem))
            {
                int len = m_gridView.Rows.Count;
                for (int i = len - 1; i >= 0; i--)
                {
                    if (m_gridView[1, i].Value.ToString().Equals(id))
                    {
                        DeleteDictionary(i);
                        m_gridView.Rows.RemoveAt(i);
                    }
                    if (isChanged)
                    {
                        if (m_gridView[1, i].Value.ToString().StartsWith(id))
                        {
                            DeleteDictionary(i);
                            m_gridView.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                DeleteDictionary(ind);
                m_gridView.Rows.RemoveAt(ind);
            }
        }

        /// <summary>
        /// Event when the status of system is changed.
        /// </summary>
        /// <param name="status">the changed status.</param>
        public void ChangeStatus(ProjectStatus status)
        {
            if (status == ProjectStatus.Running ||
                status == ProjectStatus.Suspended ||
                status == ProjectStatus.Uninitialized)
            {
                m_gridView.ContextMenu = null;
            }
            else
            {
                m_gridView.ContextMenuStrip = m_contextMenu;
            }

            if (status == ProjectStatus.Running)
            {
                m_time.Enabled = true;
                m_time.Start();
            }
            else if (status == ProjectStatus.Suspended)
            {
                m_time.Enabled = false;
                m_time.Stop();
                UpdatePropForSimulation();
            }
            else if ((m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping) &&
                    status == ProjectStatus.Loaded)
            {
                m_time.Enabled = false;
                m_time.Stop();
                ResetPropForSimulation();
            }
            else if (status == ProjectStatus.Stepping)
            {
                UpdatePropForSimulation();
            }
            m_type = status;
        }

        /// <summary>
        /// Get tab name.
        /// </summary>
        /// <returns>"Model"</returns>
        public string GetTabPageName()
        {
            return "Model";
        }

        /// <summary>
        /// Get TabPage.
        /// </summary>
        /// <returns>TabPage.</returns>
        public TabPage GetTabPage()
        {
            return m_tabPage;
        }
        #endregion

        #region Events
        /// <summary>
        /// Event when search button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void ClickSearchMenu(object sender, EventArgs e)
        {
            SearchInstance win = new SearchInstance();
            win.SetPlugin(this);
            win.ShowDialog();
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

        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void FireTimer(object sender, EventArgs e)
        {
            m_time.Enabled = false;
            UpdatePropForSimulation();
            m_time.Enabled = true;
        }
        #endregion
    }
}
