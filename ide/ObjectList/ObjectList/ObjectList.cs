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
// written by Motokazu Ishikawa<m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.ComponentModel;

using EcellLib;

namespace EcellLib.ObjectList
{
    /// <summary>
    /// Plugin class to display object by list.
    /// </summary>
    public class ObjectList : PluginBase
    {
        #region Fields
        /// <summary>
        /// modelID of a model, which is currently displayed on the ObjectList.
        /// </summary>
        private string m_currentModelID;
        /// <summary>
        ///  tab control of ObjectList.
        /// </summary>
        private TabControl m_tabControl;
        /// <summary>
        /// key is EcellObject.M_type, value is ObjectListOfType which has type-related resources.
        /// </summary>
        private Dictionary<string, ObjectListOfType> m_dict;
        /// <summary>
        /// key is EcellObject.M_type, value is column header list.
        /// </summary>
        private Dictionary<string, List<string>> m_columnDict;
        /// <summary>
        /// key is data type name(string), value is tab page(TabPage).
        /// </summary>
        private Dictionary<string, TabPage> m_pageDict;
        /// <summary>
        /// key is data type name(string), value is data grid(DataGridView).
        /// </summary>
        private Dictionary<string, DataGridView> m_gridDict;
        /// <summary>
        /// data search window in ObjectListWindow.
        /// </summary>
        private SearchInstance m_searchWin;
        /// <summary>
        /// popup menu on ObjectListWindow.
        /// </summary>
        private ContextMenuStrip m_contextStrip;
        private DataManager m_dManager;
        private bool isDouble = false;
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResObjList));
        #endregion

        /// <summary>
        /// Constructor for ObjectList.
        /// </summary>
        public ObjectList()
        {
            m_dManager = DataManager.GetDataManager();
            m_dict = new Dictionary<string, ObjectListOfType>();
            m_pageDict = new Dictionary<string, TabPage>();
            m_gridDict = new Dictionary<string, DataGridView>();

            m_contextStrip = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
//            m_resources.ApplyResources(it, "SearchMenu");
            it.Text = m_resources.GetString("SearchMenuText");
            it.ShortcutKeys = Keys.Control | Keys.F;
            it.Click += new EventHandler(SearchMenuClick);

            ToolStripMenuItem cr = new ToolStripMenuItem();
//            m_resources.ApplyResources(cr, "CreLoggerMenu");
            cr.Text = m_resources.GetString("CreLoggerMenuText");
            cr.ShortcutKeys = Keys.Control | Keys.A;
            cr.Click += new EventHandler(CreateLoggerMenuClick);

            m_contextStrip.Items.AddRange(new ToolStripItem[] { it, cr });

            
            XmlDocument xmlD = null;
            System.Reflection.Assembly asm = null;

            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;

            m_columnDict = new Dictionary<string, List<string>>();
            asm = System.Reflection.Assembly.GetExecutingAssembly();
            xmlD = new XmlDocument();
            using (Stream stm =
                asm.GetManifestResourceStream("EcellLib.ObjectList.column.xml"))
            {
                xmlD.Load(stm);
            }
            foreach (XmlNode typeNode in xmlD.ChildNodes[0].ChildNodes)
            {
                string type = typeNode.Attributes["type"].Value;
                List<string> columns = new List<string>();
                columns.Add("ID");

                foreach (XmlNode columnNode in typeNode.ChildNodes)
                {
                    columns.Add(columnNode.InnerText);
                }
                m_columnDict.Add(type, columns);

                TabPage page = new TabPage();
                page.Text = type;
                m_tabControl.Controls.Add(page);
                m_pageDict.Add(type, page);

                DataSet dataSet = null;
                DataTable dataTable = null;
                DataGridView dgr = null;

                dgr = new DataGridView();
                dgr.Dock = DockStyle.Fill;
                dgr.AllowUserToAddRows = false;
                dgr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgr.MultiSelect = true;
                dgr.RowHeadersVisible = false;
                dgr.ReadOnly = true;
                dgr.AllowUserToDeleteRows = false;
                dgr.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgr.CellClick += new DataGridViewCellEventHandler(DataGridCellClick);
                dgr.SelectionChanged += new EventHandler(DataGridSelectChanged);
                dgr.ContextMenuStrip = null;
                page.Controls.Add(dgr);

                dataSet = new DataSet();
                dgr.DataSource = dataSet;
                dgr.DataMember = type;
                dataTable = new DataTable(type);
                foreach (string column in m_columnDict[type])
                {
                    DataColumn dataColumn = new DataColumn();
                    dataColumn.ColumnName = column;
                    dataColumn.DataType = typeof(string);

                    dataTable.Columns.Add(dataColumn);
                    if (column == "ID")
                    {
                        dataTable.PrimaryKey = new DataColumn[] { dataColumn };
                    }
                }

                dataSet.Tables.Add(dataTable);
                ObjectListOfType objectListOfType
                 = new ObjectListOfType(type, m_columnDict[type], dgr, dataSet);
                m_dict.Add(type, objectListOfType);
                m_gridDict.Add(type, dgr);

            }
        }

        void DataGridSelectChanged(object sender, EventArgs e)
        {
            int i = 0;
            for (i = 0; i < 10; i++)
            {
                if (m_gridDict["Process"].SelectedRows.Count > 1)
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Deconstructor for ObjectList.
        /// </summary>
        ~ObjectList()
        {
            foreach (string type in m_pageDict.Keys)
            {
                TabPage page = m_pageDict[type];
                page.Dispose();
            }

            if (m_tabControl != null)
            {
                foreach (TabPage page in m_tabControl.Controls)
                    page.Dispose();
                m_tabControl.Dispose();
            }
        }

        /// <summary>
        /// Search the object from DataGridView.
        /// </summary>
        /// <param name="text">Search text.</param>
        public void SearchObjectFromDgv(string text)
        {
            string type = m_tabControl.SelectedTab.Text;
            DataGridView view = m_gridDict[type];
//            int ind = view.CurrentRow.Index;
            int ind = view.SelectedRows[0].Index;
            int i = ind + 1;
            while (true)
            {
                if (i == view.RowCount)
                {
                    i = 0;
                }
                if (i == ind)
                {
                    String errmes = m_resources.GetString("ErrNotFindPage");
                    MessageBox.Show(errmes, "MESSAGE",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string value = (string)view.Rows[i].Cells["ID"].Value;
                if (value.Contains(text))
                {
                    DataGridViewCell cell = view[0, i];
                    PluginManager.GetPluginManager().SelectChanged(m_currentModelID, (string)cell.Value, type);
                    return;
                }
                i++;
            }
        }

        /// <summary>
        /// Convert from key string to key string of system.
        /// </summary>
        /// <param name="keyName">key string of process or variable.</param>
        /// <returns>key string of system.</returns>
        public string GetSystemName(string keyName)
        {
            int ind = keyName.IndexOf(':');
            if (ind >= 0)
            {
                return keyName.Substring(0, ind);
            }
            else
            {
                return keyName;
            }
        }

        /// <summary>
        /// Create a new ObjectListOfType instance, An ObjectListOfType contains a TabPage
        /// and data.
        /// </summary>
        /// <param name="type">EcellObject.M_type</param>
        /// <param name="columnList">these columns will be displayed in DataGridView of this tabPage</param>
        /// <returns>ObjectListOfType</returns>
        internal ObjectListOfType CreateNewPage(string type, List<string> columnList)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = type;

            DataGridView dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersVisible = false;
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = true;

            tabPage.Controls.Add(dataGridView);

            DataSet dataSet = new DataSet();
            dataGridView.DataSource = dataSet;
            dataGridView.DataMember = "objectList";

            DataTable dataTable = new DataTable("objectList");

            foreach (string column in columnList)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = column;
                dataColumn.DataType = typeof(string);

                dataTable.Columns.Add(dataColumn);
            }

            dataSet.Tables.Add(dataTable);

            // Set each object to objectListOfType
            ObjectListOfType objectListOfType
             = new ObjectListOfType(type, columnList, dataGridView, dataSet);
            m_tabControl.Controls.Add(tabPage);

            return objectListOfType;
        }

        #region Event
        /// <summary>
        /// The action of clicking search button in popup menu on ObjectList.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void SearchMenuClick(object sender, EventArgs e)
        {
            string type = m_tabControl.SelectedTab.Text;
            DataGridView view = m_gridDict[type];

            if (view.RowCount <= 0) return;

            m_searchWin = new SearchInstance();
            m_searchWin.SetPlugin(this);
            m_searchWin.SISearchButton.Click += new EventHandler(m_searchWin.SearchButtonClick);
            m_searchWin.SICloseButton.Click += new EventHandler(m_searchWin.SearchCloseButtonClick);
            m_searchWin.searchText.KeyPress += new KeyPressEventHandler(m_searchWin.SearchTextKeyPress);
            m_searchWin.Tag = view.CurrentRow.Index;

            m_searchWin.ShowDialog();
        }

        /// <summary>
        /// The action of clicking create logger button in popup menu on ObjectList.
        /// </summary>
        /// <param name="sender">object(ToolStripMenuItem)</param>
        /// <param name="e">EventArgs</param>
        public void CreateLoggerMenuClick(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> sysDic = 
                new Dictionary<string, List<string>>();

            string type = m_tabControl.SelectedTab.Text;
            DataGridView view = m_gridDict[type];

            if (view.RowCount <= 0) return;

            string model = m_currentModelID;

            if (type == "System")
            {
                foreach (DataGridViewRow row in view.SelectedRows)
                {
                    string sysName = (string)row.Cells[0].Value;
                    sysDic.Add(sysName, new List<string>());
                }
                foreach (string key in sysDic.Keys)
                {
                    List<EcellObject> list = m_dManager.GetData(model, key);
                    foreach (EcellObject obj in list)
                    {
                        foreach (EcellData d in obj.M_value)
                        {
                            if ((type == "System" && d.M_name == "Size"))
                            {
                                PluginManager.GetPluginManager().LoggerAdd(
                                    obj.modelID,
                                    obj.key,
                                    obj.type,
                                    d.M_entityPath);
                                if (d.M_isLogger == true) break;
                                d.M_isLogger = true;
                                break;
                            }
                        }
                        m_dManager.DataChanged(model, obj.key, type, obj);
                    }
                }
                return;
            }

            foreach (DataGridViewRow row in view.SelectedRows)
            {
                string sysName = GetSystemName((string)row.Cells[0].Value);
                if (!sysDic.ContainsKey(sysName))
                {
                    sysDic.Add(sysName, new List<string>());
                }
                sysDic[sysName].Add((string)row.Cells[0].Value);
            }

            foreach (string key in sysDic.Keys)
            {
                List<EcellObject> list = m_dManager.GetData(model, key);
                foreach (EcellObject top in list)
                {
                    if (top.M_instances == null) continue;
                    foreach (EcellObject obj in top.M_instances)
                    {
                        if (obj.type != type || !sysDic[key].Contains(obj.key))
                            continue;
                        foreach (EcellData d in obj.M_value)
                        {
                            if ((type == "Process" && d.M_name == "Activity") ||
                                (type == "Variable" && d.M_name == "Value"))
                            {
                                PluginManager.GetPluginManager().LoggerAdd(
                                    obj.modelID,
                                    obj.key,
                                    obj.type,
                                    d.M_entityPath);
                                if (d.M_isLogger == true) break;
                                d.M_isLogger = true;
                                break;
                            }
                        }
                        m_dManager.DataChanged(model, obj.key, type, obj);
                    }
                }
            }
        }

        /// <summary>
        /// The action of clicking cell in DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">click cell</param>
        public void DataGridCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridView view = (DataGridView)sender;
            DataGridViewRow row = view.CurrentRow;
            string type = m_tabControl.SelectedTab.Text;

            if (row != null)
            {
                isDouble = true;
                DataGridViewCell cell = view[0, row.Index];
                if (view.SelectedRows.Count > 1)
                {
                    PluginManager.GetPluginManager().AddSelect(m_currentModelID,
                        (string)cell.Value, type);
                }
                else
                {
                    if (view.SelectedRows.Contains(row))
                    {
                        PluginManager.GetPluginManager().SelectChanged(m_currentModelID,
                            (string)cell.Value, type);
                    }
                    else
                    {
                        PluginManager.GetPluginManager().RemoveSelect(m_currentModelID,
                            (string)cell.Value, type);
                    }
                }
                isDouble = false;
            }
        }
        #endregion

        #region PuginBase
        /// <summary>
        /// Get menustrips for ObjectList.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get toolbar buttons for ObjectList.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for ObjectList.
        /// </summary>
        /// <returns>UserControl</returns>        
        public List<UserControl> GetWindowsForms()
        {
            UserControl control = new UserControl();
            control.Dock = DockStyle.Fill;
            control.Controls.Add(m_tabControl);

            List<UserControl> list = new List<UserControl>();
            list.Add(control);

            return list;
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            try
            {
                if (m_currentModelID == null || !m_currentModelID.Equals(modelID))
                {
                    this.Clear();
                    DataManager dm = DataManager.GetDataManager();
                    List<EcellObject> list = dm.GetData(modelID, null);
                    if (list == null) return;

                    foreach (EcellObject eo in list)
                    {
                        if (eo.type != "System") continue;
                        if (eo.M_instances != null)
                        {
                            foreach (EcellObject child in eo.M_instances)
                            {
                                if (child.key.EndsWith(":SIZE")) continue;
                                // When eo has a new type
                                if (m_dict.ContainsKey(child.type))
                                {
                                    m_dict[child.type].AddObject(child);
                                }
                            }
                        }
                        m_dict[eo.type].AddObject(eo);
                    }
                    this.m_currentModelID = modelID;
                }

                if (key == null) return;
                if (type != "System" && type != "Process" && type != "Variable") return;
                DataGridView view = m_gridDict[type];
                m_tabControl.SelectedTab = m_pageDict[type];
                if (isDouble == false) view.ClearSelection();

                for (int i = 0; i < view.RowCount; i++)
                {
                    if ((string)view.Rows[i].Cells["ID"].Value == key)
                    {
                        view.Rows[i].Cells["ID"].Selected = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrSelectObj");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            try
            {
                if (m_currentModelID == null || !m_currentModelID.Equals(modelID))
                {
                    this.Clear();
                    DataManager dm = DataManager.GetDataManager();
                    List<EcellObject> list = dm.GetData(modelID, null);
                    if (list == null) return;

                    foreach (EcellObject eo in list)
                    {
                        if (eo.type != "System") continue;
                        if (eo.M_instances != null)
                        {
                            foreach (EcellObject child in eo.M_instances)
                            {
                                if (child.key.EndsWith(":SIZE")) continue;
                                // When eo has a new type
                                if (m_dict.ContainsKey(child.type))
                                {
                                    m_dict[child.type].AddObject(child);
                                }
                            }
                        }
                        m_dict[eo.type].AddObject(eo);
                    }
                    this.m_currentModelID = modelID;
                }

                if (key == null) return;
                if (type != "System" && type != "Process" && type != "Variable") return;
                DataGridView view = m_gridDict[type];
                m_tabControl.SelectedTab = m_pageDict[type];

                for (int i = 0; i < view.RowCount; i++)
                {
                    if ((string)view.Rows[i].Cells["ID"].Value == key)
                    {
                        view.Rows[i].Cells["ID"].Selected = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrSelectObj");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            if (modelID == null)
                return;

            try
            {
                if (m_currentModelID == null || !m_currentModelID.Equals(modelID))
                {
                    this.Clear();
                    DataManager dm = DataManager.GetDataManager();
                    List<EcellObject> list = dm.GetData(modelID, null);
                    if (list == null) return;

                    foreach (EcellObject eo in list)
                    {
                        if (eo.type != "System") continue;
                        if (eo.M_instances != null)
                        {
                            foreach (EcellObject child in eo.M_instances)
                            {
                                if (child.key.EndsWith(":SIZE")) continue;
                                // When eo has a new type
                                if (m_dict.ContainsKey(child.type))
                                {
                                    m_dict[child.type].AddObject(child);
                                }
                            }
                        }
                        m_dict[eo.type].AddObject(eo);
                    }
                    this.m_currentModelID = modelID;
                }

                if (key == null) return;
                if (type != "System" && type != "Process" && type != "Variable") return;
                DataGridView view = m_gridDict[type];
                m_tabControl.SelectedTab = m_pageDict[type];

                for (int i = 0; i < view.RowCount; i++)
                {
                    if ((string)view.Rows[i].Cells["ID"].Value == key)
                    {
                        view.Rows[i].Cells["ID"].Selected = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrSelectObj");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            if (m_tabControl == null)
                return;

            foreach (string key in m_dict.Keys)
            {
                m_gridDict[key].ClearSelection();
            }
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            if (data == null) return;

            try
            {
                foreach (EcellObject eo in data)
                {
                    if (eo.modelID == null || this.m_currentModelID == null ||
                        !this.m_currentModelID.Equals(eo.modelID))
                        continue;

                    if (m_dict.ContainsKey(eo.type))
                    {
                        m_dict[eo.type].AddObject(eo);
                        if (eo.M_instances == null) continue;
                        foreach (EcellObject obj in eo.M_instances)
                        {
                            m_dict[obj.type].AddObject(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrAddObj");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
            if (modelID == null || key == null || m_currentModelID == null ||
               !m_currentModelID.Equals(modelID))
                return;

            ObjectListOfType olot = m_dict[type];
            olot.DeleteObjectWithFull(key);

            m_dict[data.type].AddObject(data);
        }

        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="key">The ID.</param>
        /// <param name="type">The data type.</param>
        /// <param name="path">The path of entity.</param>
        public void LoggerAdd(string modelID, string key, string type, string path)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            if (this.m_currentModelID == null ||
                this.m_currentModelID != modelID) return;

            if (key == null)
            {
                Clear();
                return;
            }

            if (type == "System")
            {
                foreach (ObjectListOfType objListOfType in m_dict.Values)
                {
                    objListOfType.DeleteObject(key, type);
                }
            }
            else
            {
                ObjectListOfType objListOfType = m_dict[type];
                objListOfType.DeleteObject(key, type);
            }
        }

        /// <summary>
        /// The event sequence on changing value with the simulation.
        /// </summary>
        /// <param name="modelID">The model ID of object changed value.</param>
        /// <param name="key">The ID of object changed value.</param>
        /// <param name="type">The object type of object changed value.</param>
        /// <param name="propName">The property name of object changed value.</param>
        /// <param name="data">Changed value of object.</param>
        public void LogData(string modelID, string key, string type, string propName, List<LogData> data)
        {
            // not implement
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            if (m_tabControl == null)
                return;

            foreach (string key in m_dict.Keys)
            {
                m_dict[key].Ds.Clear();
            }
        }

        /// <summary>
        /// The event sequence on generating warning data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID generating warning data.</param>
        /// <param name="key">The ID generating warning data.</param>
        /// <param name="type">The data type generating warning data.</param>
        /// <param name="warntype">The type of waring data.</param>
        public void WarnData(string modelID, string key, string type, string warntype)
        {
            // nothing
        }

        /// <summary>
        /// The execution log of simulation, debug and analysis.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="message">Message.</param>
        public void Message(string type, string message)
        {
            // nothing
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
            if (time == 0.0) return;

            foreach (DataGridViewRow r in m_gridDict["System"].Rows)
            {
                string path = Util.ConvertSystemEntityPath(r.Cells[0].Value.ToString(), "Size");
                EcellValue e = m_dManager.GetEntityProperty(path);
                if (e == null) continue;
                r.Cells[1].Value = e.ToString();
            }
            foreach (DataGridViewRow r in m_gridDict["Variable"].Rows)
            {
                string path = "Variable:" + r.Cells[0].Value + ":Value";
                EcellValue e = m_dManager.GetEntityProperty(path);
                if (e == null) continue;
                r.Cells[1].Value = e.ToString();
            }
            foreach (DataGridViewRow r in m_gridDict["Process"].Rows)
            {
                string path = "Process:" + r.Cells[0].Value + ":Activity";
                EcellValue e = m_dManager.GetEntityProperty(path);
                if (e == null) continue;
                r.Cells[1].Value = e.ToString();
            }
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(int type)
        {
            if (type == Util.RUNNING)
            {
                foreach (string name in m_gridDict.Keys)
                {
                    m_gridDict[name].ContextMenuStrip = null;
                }
            }
            else
            {
                foreach (string name in m_gridDict.Keys)
                {
                    m_gridDict[name].ContextMenuStrip = m_contextStrip;
                }
            }
        }

        /// <summary>
        /// Save the selected model to directory.
        /// </summary>
        /// <param name="modelID">selected model.</param>
        /// <param name="directory">output directory.</param>
        public void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// Set the panel that show this plugin in MainWindow.
        /// </summary>
        /// <param name="panel">The set panel.</param>
        public void SetPanel(Panel panel)
        {
            // nothing
        }

        /// <summary>
        /// get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>bitmap data</returns>
        public Bitmap Print()
        {
            try
            {
                Bitmap bitmap = new Bitmap(m_tabControl.Width, m_tabControl.Height);
                m_tabControl.DrawToBitmap(bitmap, m_tabControl.ClientRectangle);

                return bitmap;
            }
            catch (Exception ex)
            {
                String errmes = m_resources.GetString("ErrCreBitmap");
                MessageBox.Show(errmes + "\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"ObjectList"</returns>
        public string GetPluginName()
        {
            return "ObjectList";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin is MessageWindow.
        /// </summary>
        /// <returns>false</returns>
        public bool IsMessageWindow()
        {
            return false;
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public bool IsEnablePrint()
        {
            return true;
        }
        #endregion
    }
}