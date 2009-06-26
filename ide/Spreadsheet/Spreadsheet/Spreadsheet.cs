﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

using Ecell.Plugin;
using Ecell.Objects;
using Ecell.Logger;

namespace Ecell.IDE.Plugins.Spreadsheet
{
    public partial class Spreadsheet : EcellDockContent, IEcellPlugin, IDataHandler, IDockContentProvider
    {
        /// <summary>
        /// Color of header.
        /// </summary>
        private Color m_headerColor = Color.Gray;
        private Color m_headerForeColor = Color.White;
        private Color m_systemColor = Color.FromArgb(0xff, 0xff, 0xd0);
        private Color m_processColor = Color.FromArgb(0xcc, 0xff, 0xcc);
        private Color m_variableColor = Color.FromArgb(0xcc, 0xcc, 0xff);
        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        protected ApplicationEnvironment m_env = null;
        /// <summary>
        /// The flag whether the select change start in this plugin.
        /// </summary>
        private bool m_isSelected = false;
        private bool m_isSelectionChanged = false;
        private DataGridViewRow m_selectedRow = null;

        /// <summary>
        /// Popup menu.
        /// </summary>
        private ContextMenuStrip m_contextMenu;
        private EcellObject m_dragObject;
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
        private static int s_ID = 1;

        private DataGridViewRow m_lastSelected = null;

        #region Accessors
        /// <summary>
        /// The application environment associated to this plugin
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            set { m_env = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Construcotor.
        /// </summary>
        public Spreadsheet()
        {
            InitializeComponent();
            foreach (char c in s_columnChars)
            {
                DataGridViewColumn col = new DataGridViewTextBoxColumn();
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.HeaderText = Convert.ToString(c);
                col.Name = Convert.ToString(c);
                m_gridView.Columns.Add(col);
            }
            CreateSystemHeader();
            CreateVariableHeader();
            CreateProcessHeader();

            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(FireTimer);

            m_gridView.RowTemplate.DefaultHeaderCellType = typeof(DataGridViewNumberedRowHeaderCell);
        }
        #endregion


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
            m_lastSelected = null;
            m_prevCell = null;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"Spreadsheet"</returns>
        public string GetPluginName()
        {
            return "Spreadsheet";
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
        /// Initialize of this plugin.
        /// </summary>
        public void Initialize()
        {
            m_env.DataManager.DisplayFormatEvent += new DisplayFormatChangedEventHandler(DisplayFormatChangeEvent);
        }
        /// <summary>
        /// The event sequence when the user adds the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The added parameter ID/</param>
        public void ParameterAdd(string projectID, string parameterID)
        {
        }
        /// <summary>
        /// The event sequence when the user deletes the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The deleted parameter ID.</param>
        public void ParameterDelete(string projectID, string parameterID)
        {
        }

        /// <summary>
        /// The event sequence when the user updates the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The set parameter ID.</param>
        public void ParameterUpdate(string projectID, string parameterID)
        {
        }

        /// <summary>
        /// The event sequence when the user sets the simulation parameter.
        /// </summary>
        /// <param name="projectID">The current project ID.</param>
        /// <param name="parameterID">The set parameter ID.</param>
        public void ParameterSet(string projectID, string parameterID)
        {
        }

        /// <summary>
        /// The event sequence on advancing time.
        /// </summary>
        /// <param name="time">The current simulation time.</param>
        public void AdvancedTime(double time)
        {
        }
        /// <summary>
        /// The event sequence on adding the logger at other plugin.
        /// </summary>
        /// <param name="entry">Logger entry data.</param>
        public void LoggerAdd(LoggerEntry entry)
        {
        }

        /// <summary>
        /// Notify a plugin that it should save model-related information if necessary.
        /// </summary>
        /// <param name="modelID">ModelID of a model which is going to be saved</param>
        /// <param name="directory">A saved file must be under this directory </param>
        public void SaveModel(string modelID, string directory)
        {
        }

        /// <summary>
        /// Get the window form for Spreadsheet.
        /// </summary>
        /// <returns>UserControl</returns>        
        public IEnumerable<EcellDockContent> GetWindowsForms()
        {            
            m_gridView.Dock = DockStyle.Fill;
            this.TabText = this.Text;
            this.IsSavable = true;
            return new EcellDockContent[] { this };
        }

        /// <summary>
        /// Event when the selected object is added.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="key">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
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
        /// Event when the selected object is removed.
        /// </summary>
        /// <param name="modelID">ModelID of the removed object.</param>
        /// <param name="key">ID of the removed object.</param>
        /// <param name="type">Type of the removed object.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null)
                row.Selected = false;
        }

        /// <summary>
        /// ResetSelect
        /// </summary>
        public void ResetSelect()
        {
            m_gridView.ClearSelection();
        }

        /// <summary>
        /// Event when object is selected.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="key">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            if (m_isSelected)
                return;
            m_gridView.ClearSelection();
            m_selectedRow = null;
            AddSelect(modelID, key, type);
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null && m_gridView.FirstDisplayedScrollingRowIndex >= 0 &&
                    row.Visible)
            {
                m_gridView.FirstDisplayedScrollingRowIndex = row.Index;
            }
        }

        /// <summary>
        /// Add the object to DataGridView.
        /// </summary>
        /// <param name="obj"></param>
        public void DataAdd(EcellObject obj)
        {

            if (obj.Type != EcellObject.VARIABLE &&
                obj.Type != EcellObject.PROCESS &&
                obj.Type != EcellObject.SYSTEM)
                return;

            int ind = SearchInsertIndex(obj.Type, obj.Key);
            if (ind == -1)
                return;
            if (obj.Type.Equals(Constants.xpathSystem))
            {
                AddSystem(ind, obj);
                DataAdd(obj.Children);
            }
            else if (obj.Type.Equals(Constants.xpathProcess))
            {
                AddProcess(ind, obj);
            }
            else if (obj.Type.Equals(Constants.xpathVariable))
            {
                AddVariable(ind, obj);
            }
            m_currentModelID = obj.ModelID;
        }


        /// <summary>
        /// Add the list of object.
        /// </summary>
        /// <param name="list">the list of object.</param>
        public void DataAdd(List<EcellObject> list)
        {
            foreach (EcellObject obj in list)
                DataAdd(obj);
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            DataDelete(modelID, key, type, true);
        }

        /// <summary>
        /// Event when the property of object is changed.
        /// </summary>
        /// <param name="modelID">ModelID of the changed object.</param>
        /// <param name="key">ID of the changed object.</param>
        /// <param name="type">Type of the changed object.</param>
        /// <param name="obj">The changed object.</param>
        public void DataChanged(string modelID, string key, string type, EcellObject obj)
        {
            if (key != obj.Key)
            {
                DataDelete(modelID, key, type, !obj.Key.Equals(key));
                DataAdd(obj);
                AddSelect(obj.ModelID, obj.Key, obj.Type);
            }
            else
            {
                DataGridViewRow r = SearchIndex(type, key);
                if (r == null)
                {
                    DataAdd(obj);
                    return;
                }
                if (obj.Type.Equals(Constants.xpathSystem))
                {
                    UpdateSystem(r.Index, obj);
                }
                else if (obj.Type.Equals(Constants.xpathProcess))
                {
                    UpdateProcess(r.Index, obj);
                }
                else if (obj.Type.Equals(Constants.xpathVariable))
                {
                    UpdateVariable(r.Index, obj);
                }
            }
        }



        /// <summary>
        /// Get the list of property shown in Common Setting Dialog.
        /// </summary>
        /// <returns>the list of IPropertyItem.</returns>
        public List<IPropertyItem> GetPropertySettings()
        {
            return null;
        }

        /// <summary>
        /// Get the information of Plugin.
        /// </summary>
        /// <returns>the information of Plugin.</returns>
        public System.Xml.XmlNode GetPluginStatus()
        {
            return null;
        }
        /// <summary>
        /// Set the information of Plugin.
        /// </summary>
        /// <param name="status">the information of Plugin.</param>
        public void SetPluginStatus(System.Xml.XmlNode nstatus)
        {
            ;
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
        /// Get the list of public delegation function.
        /// </summary>
        /// <returns>the dictionary of name and delegation function</returns>
        public Dictionary<string, Delegate> GetPublicDelegate()
        {
            return null;
        }

        #region General
        /// <summary>
        /// The property of object is reset when simulation is stopped.
        /// </summary>
        private void ResetPropForSimulation()
        {
            List<EcellObject> list = m_env.DataManager.GetData(m_currentModelID, null);
            Clear();
            DataAdd(list);
        }

        /// <summary>
        /// The property of object is updated when simulation is runniing.
        /// </summary>
        private void UpdatePropForSimulation()
        {
            double ctime = m_env.DataManager.GetCurrentSimulationTime();
            if (ctime == 0.0)
                return;

            try
            {
                foreach (string entPath in m_propDic.Keys)
                {
                    EcellValue v = m_env.DataManager.GetEntityProperty(entPath);
                    if (v == null) continue;
                    if (v.IsDouble)
                        m_propDic[entPath].Value = ((double)v).ToString(m_env.DataManager.DisplayStringFormat);
                }
            }
            catch (Exception)
            {
                // 他のプラグインでデータを編集したか
                // シミュレーションが異常終了したがデータを取得できなかったため。
                // 他のプラグインでエラーメッセージが表示されるので
                // ここでは出さないようにする。
            }
        }

        /// <summary>
        /// Search the index of the inserted object.
        /// </summary>
        /// <param name="type">The header position.</param>
        /// <param name="key">The ID of inserted object.</param>
        /// <returns>The position index of inserted object.</returns>
        private int SearchInsertIndex(string type, string key)
        {
            int startPos = 0;
            int typeNum = 0;
            for (int i = 0; i < m_gridView.Rows.Count; i++)
            {
                startPos = i + 1;
                if (m_gridView.Rows[i].Tag != null)
                    continue;

                if (type == Constants.xpathSystem && typeNum == 0)
                    break;
                else if (type == Constants.xpathVariable && typeNum == 1)
                    break;
                else if (type == Constants.xpathProcess && typeNum == 2)
                    break;
                typeNum++;
            }

            if (startPos == m_gridView.Rows.Count)
                return startPos;
            for (int i = startPos; i < m_gridView.Rows.Count; i++)
            {
                startPos = m_gridView.Rows[i].Index;
                if (m_gridView.Rows[i].Tag == null)
                {
                    return startPos;
                }
                if (m_gridView[s_ID, i].Value.ToString().CompareTo(key) == 0)
                    return -1;
                if (m_gridView[s_ID, i].Value.ToString().CompareTo(key) > 0)
                {
                    return startPos;
                }
            }

            return ++startPos;
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
                if (m_gridView.Rows[i].Tag == null) continue;
                EcellObject obj = m_gridView.Rows[i].Tag as EcellObject;

                if (!type.Equals(obj.Type))
                    continue;
                if (!key.Equals(obj.Key))
                    continue;
                return i;
            }
            return -1;
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
            if (type.Equals(Constants.xpathStepper))
                return;

            int ind = SearchObjectIndex(id, type);
            if (ind < 0)
                return;
            if (type.Equals(Constants.xpathSystem))
            {
                int len = m_gridView.Rows.Count;
                for (int i = len - 1; i >= 0; i--)
                {
                    if (m_gridView[s_ID, i].Value.Equals(id))
                    {
                        DeleteDictionary(i);
                        m_isSelected = true;
                        m_gridView.Rows.RemoveAt(i);
                        m_isSelected = false;
                    }

                    if (m_gridView[s_ID, i].Value.ToString().StartsWith(id))
                    {
                        m_isSelected = true;
                        DeleteDictionary(i);
                        m_gridView.Rows.RemoveAt(i);
                        m_isSelected = false;
                    }
                }
            }
            else
            {
                m_isSelected = true;
                DeleteDictionary(ind);
                m_gridView.Rows.RemoveAt(ind);
                m_isSelected = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        private int TypeConverter(string type1, string type2)
        {
            if (type1 == type2) return 0;
            if (type1 == EcellObject.SYSTEM) return 1;
            if (type2 == EcellObject.SYSTEM) return -1;
            if (type1 == EcellObject.VARIABLE) return -1;
            return 1;
        }

        /// <summary>
        /// Get the data string from entity name.
        /// </summary>
        /// <param name="name">The entity name.</param>
        /// <param name="obj">The searched object.</param>
        /// <returns>the data string.</returns>
        private string GetData(string name, EcellObject obj)
        {
            if (name.Equals(s_indexType))
            {
                return obj.Type;
            }
            else if (name.Equals(s_indexID))
            {
                return obj.Key;
            }
            else if (name.Equals(s_indexModel))
            {
                return obj.ModelID;
            }
            else if (name.Equals(s_indexClass))
            {
                return obj.Classname;
            }
            else if (name.Equals(s_indexName))
            {
                EcellData data = obj.GetEcellData("Name");
                return data != null ? (string)data.Value : "";
            }
            else if (name.Equals(s_indexStepper))
            {
                foreach (EcellData d in obj.Value)
                {
                    if (d.Name.Equals(Constants.xpathStepperID))
                        return (string)d.Value;
                }
            }
            else if (name.Equals(s_indexSize))
            {
                EcellSystem data = obj as EcellSystem;
                return data.SizeInVolume.ToString(m_env.DataManager.DisplayStringFormat);
            }
            else
            {
                foreach (EcellData d in obj.Value)
                {
                    if (name.Equals(d.Name))
                    {
                        if (d.Value.IsDouble)
                            return ((double)d.Value).ToString(m_env.DataManager.DisplayStringFormat);
                        return (string)d.Value;
                    }
                }
            }

            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsNumeric(string name, EcellObject obj)
        {
            if (name.Equals(s_indexType))
            {
                return false;
            }
            else if (name.Equals(s_indexID))
            {
                return false;
            }
            else if (name.Equals(s_indexModel))
            {
                return false;
            }
            else if (name.Equals(s_indexClass))
            {
                return false;
            }
            else if (name.Equals(s_indexName))
            {
                return false;
            }
            else if (name.Equals(s_indexStepper))
            {
                return false;
            }
            else
            {
                foreach (EcellData d in obj.Value)
                {
                    if (name.Equals(d.Name))
                    {
                        if (d.Value.IsDouble || d.Value.IsInt)
                            return true;
                        else
                            return false;
                    }
                }
            }

            return false;
        }


        private DataGridViewRow SearchIndex(string type, string key)
        {
            foreach (DataGridViewRow r in m_gridView.Rows)
            {
                EcellObject obj = r.Tag as EcellObject;
                if (obj == null)
                    continue;
                if (obj.Type == type && obj.Key == key)
                    return r;
            }
            return null;
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


        /// <summary>
        /// Insert the object at the set position index.
        /// </summary>
        /// <param name="index">tTe position index.</param>
        /// <param name="obj">The inserted object.</param>
        private void AddProcess(int index, EcellObject obj)
        {
            int len = m_processProp.Length;
            DataGridViewRow rs = m_gridView.RowTemplate.Clone() as DataGridViewRow;
            rs.DefaultCellStyle.BackColor = m_processColor;
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_processProp[i], obj);
                DataGridViewCell c = new DataGridViewTextBoxCell();
                bool isNum = IsNumeric(m_processProp[i], obj);
                if (isNum)
                    c.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                    if (!m_propDic.ContainsKey(entPath))
                        m_propDic.Add(entPath, c);
                    break;
                }

            }
            for (int i = len; i < s_columnChars.Length; i++)
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = "";
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }

            rs.Tag = obj;
            rs.ContextMenuStrip = spreadSheetContextMenuStrip;

            m_gridView.Rows.Insert(index, rs);
        }

        /// <summary>
        /// Insert the object at the set position index.
        /// </summary>
        /// <param name="index">tTe position index.</param>
        /// <param name="obj">The inserted object.</param>
        private void AddSystem(int index, EcellObject obj)
        {
            int len = m_systemProp.Length;
            DataGridViewRow rs = m_gridView.RowTemplate.Clone() as DataGridViewRow;
            rs.DefaultCellStyle.BackColor = m_systemColor;
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_systemProp[i], obj);
                DataGridViewCell c = new DataGridViewTextBoxCell();
                bool isNum = IsNumeric(m_systemProp[i], obj);
                if (isNum)
                    c.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                    if (!m_propDic.ContainsKey(entPath))
                        m_propDic.Add(entPath, c);
                    break;
                }
            }
            for (int i = len; i < s_columnChars.Length; i++)
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = "";
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }

            rs.Tag = obj;
            rs.ContextMenuStrip = spreadSheetContextMenuStrip;
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
            DataGridViewRow rs = m_gridView.RowTemplate.Clone() as DataGridViewRow;
            rs.DefaultCellStyle.BackColor = m_variableColor;
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_variableProp[i], obj);
                DataGridViewCell c = new DataGridViewTextBoxCell();
                bool isNum = IsNumeric(m_variableProp[i], obj);
                if (isNum)
                    c.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                    if (!m_propDic.ContainsKey(entPath))
                        m_propDic.Add(entPath, c);
                    break;
                }
            }
            for (int i = len; i < s_columnChars.Length; i++)
            {
                DataGridViewTextBoxCell c = new DataGridViewTextBoxCell();
                c.Value = "";
                rs.Cells.Add(c);
                c.ReadOnly = true;
            }
            rs.Tag = obj;
            rs.ContextMenuStrip = spreadSheetContextMenuStrip;

            m_gridView.Rows.Insert(index, rs);
        }


        private void UpdateVariable(int index, EcellObject obj)
        {
            int len = m_variableProp.Length;
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_variableProp[i], obj);
                string value = m_gridView[i, index].Value.ToString();
                if (data.Equals(value)) continue;
                m_gridView[i, index].Value = data;
            }

            m_gridView.Rows[index].Tag = obj;
        }

        private void UpdateProcess(int index, EcellObject obj)
        {
            int len = m_processProp.Length;
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_processProp[i], obj);
                string value = m_gridView[i, index].Value.ToString();
                if (data.Equals(value)) continue;
                m_gridView[i, index].Value = data;
            }

            m_gridView.Rows[index].Tag = obj;
        }

        private void UpdateSystem(int index, EcellObject obj)
        {
            int len = m_systemProp.Length;
            for (int i = 0; i < len; i++)
            {
                string data = GetData(m_systemProp[i], obj);
                string value = m_gridView[i, index].Value.ToString();
                if (data.Equals(value)) continue;
                m_gridView[i, index].Value = data;
            }

            m_gridView.Rows[index].Tag = obj;
        }


        /// <summary>
        /// Create the header of system.
        /// </summary>
        private void CreateSystemHeader()
        {
            int len = m_systemProp.Length;
            DataGridViewRow rs = m_gridView.RowTemplate.Clone() as DataGridViewRow;
            rs.DefaultCellStyle.BackColor = m_headerColor;
            rs.DefaultCellStyle.SelectionBackColor = m_headerColor;
            rs.DefaultCellStyle.ForeColor = m_headerForeColor;
            rs.DefaultCellStyle.SelectionForeColor = m_headerForeColor;
            rs.DefaultCellStyle.Font = new Font(m_gridView.DefaultCellStyle.Font, FontStyle.Bold);
            for (int i = 0; i < s_columnChars.Length; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();
                if (i < len)
                    cs1.Value = m_systemProp[i];
                else
                    cs1.Value = "";
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            rs.ContextMenuStrip = spreadSheetContextMenuStrip;
            m_gridView.Rows.Add(rs);
        }

        /// <summary>
        /// Create the header of variable.
        /// </summary>
        private void CreateVariableHeader()
        {
            int len = m_variableProp.Length;
            DataGridViewRow rs = m_gridView.RowTemplate.Clone() as DataGridViewRow;
            rs.DefaultCellStyle.BackColor = m_headerColor;
            rs.DefaultCellStyle.SelectionBackColor = m_headerColor;
            rs.DefaultCellStyle.ForeColor = m_headerForeColor;
            rs.DefaultCellStyle.SelectionForeColor = m_headerForeColor;
            rs.DefaultCellStyle.Font = new Font(m_gridView.DefaultCellStyle.Font, FontStyle.Bold);
            for (int i = 0; i < s_columnChars.Length; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();
                if (i < len)
                    cs1.Value = m_variableProp[i];
                else
                    cs1.Value = "";
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            rs.ContextMenuStrip = spreadSheetContextMenuStrip;
            m_gridView.Rows.Add(rs);
        }

        /// <summary>
        /// Create the header of process.
        /// </summary>
        private void CreateProcessHeader()
        {
            int len = m_processProp.Length;
            DataGridViewRow rs = m_gridView.RowTemplate.Clone() as DataGridViewRow;
            Trace.WriteLine(rs.HeaderCell.GetType());
            rs.DefaultCellStyle.BackColor = m_headerColor;
            rs.DefaultCellStyle.SelectionBackColor = m_headerColor;
            rs.DefaultCellStyle.ForeColor = m_headerForeColor;
            rs.DefaultCellStyle.SelectionForeColor = m_headerForeColor;
            rs.DefaultCellStyle.Font = new Font(m_gridView.DefaultCellStyle.Font, FontStyle.Bold);
            for (int i = 0; i < s_columnChars.Length; i++)
            {
                DataGridViewCell cs1 = new DataGridViewTextBoxCell();

                if (i < len)
                    cs1.Value = m_processProp[i];
                else
                    cs1.Value = "";
                rs.Cells.Add(cs1);
                cs1.ReadOnly = true;
            }
            rs.ContextMenuStrip = spreadSheetContextMenuStrip;
            m_gridView.Rows.Add(rs);
        }

        private void SetClipBoardText()
        {
            string text = "";
            foreach (DataGridViewRow r in m_gridView.Rows)
            {
                if (!r.Selected) continue;
                for (int i = 0; i < r.Cells.Count; i++)
                {
                    text = text + r.Cells[i].Value.ToString() + "\t";
                }
                text = text + "\r\n";
            }
            Clipboard.SetText(text);
        }
        #endregion



        #region Statics
        /// <summary>
        /// Max number of column.
        /// </summary>
        private static char[] s_columnChars = new char[] {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// The property array of System.
        /// </summary>
        private static String[] m_systemProp = new string[] {
            s_indexType, 
            s_indexID, 
            s_indexModel, 
            s_indexClass,
            s_indexName, 
            s_indexStepper, 
            s_indexSize
        };
        /// <summary>
        /// The property array of Variable.
        /// </summary>
        private static String[] m_variableProp = new string[] {
            s_indexType, 
            s_indexID, 
            s_indexModel, 
            s_indexClass, 
            s_indexName, 
            s_indexValue,
            s_indexMolarConc
        };
        /// <summary>
        /// The property array of Process.
        /// </summary>
        private static String[] m_processProp = new string[] {
            s_indexType,
            s_indexID, 
            s_indexModel, 
            s_indexClass,
            s_indexName, 
            s_indexStepper, 
            s_indexActivity, 
            s_indexVariableRefList
        };
        /// <summary>
        /// The property array of object that is not enable to edit.
        /// </summary>
        private static String[] m_notEditProp = new string[] {
            s_indexType,
            s_indexModel
        };
        /// <summary>
        /// The property array of object that is update the value when simulation is running.
        /// </summary>
        private static String[] m_simulationProp = new string[] {
            s_indexSize,
            s_indexActivity,
            s_indexValue,
            s_indexMolarConc
        };
        #endregion

        #region Constants
        /// <summary>
        /// The reserved name for the type of object.
        /// </summary>
        private const string s_indexType = "Type";
        /// <summary>
        /// The reserved name for ID of object.
        /// </summary>
        private const string s_indexID = "Path:ID";
        /// <summary>
        /// The reserved name for the Model ID of object.
        /// </summary>
        private const string s_indexModel = "Model";
        /// <summary>
        /// The reserved name for the class name of object.
        /// </summary>
        private const string s_indexClass = "Classname";
        /// <summary>
        /// The reserved name for the name of object.
        /// </summary>
        private const string s_indexName = "Name";
        /// <summary>
        /// The reserved name for the stepperID of object.
        /// </summary>
        private const string s_indexStepper = "StepperID";
        /// <summary>
        /// The reserved name for the Size of object.
        /// </summary>
        private const string s_indexSize = "Size";
        /// <summary>
        /// The reserved name for the MolarConc of object.
        /// </summary>
        private const string s_indexMolarConc = "MolarConc";
        /// <summary>
        /// The reserved name for the activity of object.
        /// </summary>
        private const string s_indexActivity = "Activity";
        /// <summary>
        /// The reserved name for the value of object.
        /// </summary>
        private const string s_indexValue = "Value";
        /// <summary>
        /// The reserved name for the VariableReferenceList of object.
        /// </summary>
        private const string s_indexVariableRefList = "VariableReferenceList";
        #endregion

        #region Events
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

        void GridViewMouseLeave(object sender, EventArgs e)
        {
            m_isSelected = false;
        }

        void GridViewMouseUp(object sender, MouseEventArgs e)
        {
            m_isSelected = false;
        }

        private void DisplayFormatChangeEvent(object o, Ecell.Events.DisplayFormatEventArgs e)
        {
            if (m_type == ProjectStatus.Uninitialized || m_type == ProjectStatus.Loading)
                return;
            ResetPropForSimulation();
        }

        void GridViewMouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hti = m_gridView.HitTest(e.X, e.Y);
            if (e.Button != MouseButtons.Left)
                return;

            if (hti.RowIndex < 0)
                return;
            DataGridViewRow r = m_gridView.Rows[hti.RowIndex];
            if (Control.ModifierKeys != Keys.Shift)
            {
                m_selectedRow = r;
                m_lastSelected = r;
            }
            else if (m_lastSelected != null)
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
                m_isSelected = true;
                foreach (DataGridViewRow r1 in m_gridView.Rows)
                {
                    EcellObject obj = r1.Tag as EcellObject;
                    if (obj == null)
                        continue;
                    if (r1.Index >= startindex && r1.Index <= endindex)
                    {
                        if (!r1.Selected)
                            m_env.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                    }
                    else
                    {
                        if (r1.Selected)
                            m_env.PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);
                    }
                }
                m_isSelected = false;
            }
            m_dragObject = r.Tag as EcellObject;
        }

        void GridViewMouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;

            if (m_dragObject == null) return;
            EnterDragMode();
            m_dragObject = null;
        }

        void GridViewSelectionChanged(object sender, EventArgs e)
        {
            if (m_isSelected && !m_isSelectionChanged && m_selectedRow != null)
            {
                m_isSelectionChanged = true;
                m_gridView.ClearSelection();
                m_selectedRow.Selected = true;
                m_isSelectionChanged = false;
            }
            else if (!m_isSelected && m_gridView.SelectedRows.Count >= 2)
            {
                m_isSelected = true;
                foreach (DataGridViewRow r in m_gridView.SelectedRows)
                {
                    EcellObject obj = r.Tag as EcellObject;
                    if (obj != null)
                        m_env.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                }
                m_isSelected = false;
            }
        }


        /// <summary>
        /// Event when the cell is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GridViewClickObjectCell(object sender, DataGridViewCellEventArgs e)
        {
            int ind = e.RowIndex;
            if (ind < 0) return;
            EcellObject obj = m_gridView.Rows[ind].Tag as EcellObject;
            if (obj == null) return;
            m_isSelected = true;
            m_selectedRow = m_gridView.Rows[ind];
            m_dragObject = null;
            if (m_gridView.Rows[ind].Selected)
            {
                if (m_gridView.SelectedRows.Count <= 1)
                {
                    m_env.PluginManager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
                }
                else
                {
                    m_env.PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                }
            }
            else
            {
                m_env.PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);
            }
            m_isSelected = false;
            m_selectedRow = null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void EnterDragMode()
        {
            EcellDragObject dobj = null;
            if (m_gridView.SelectedRows.Count <= 0)
                return;

            foreach (DataGridViewRow r in m_gridView.SelectedRows)
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
                if (m_gridView.SelectedRows.Count == 1)
                {
                    m_isSelected = true;
                    m_env.PluginManager.SelectChanged(obj);
                    m_isSelected = false;
                }
            }
            // Drag & Drop Event.
            if (dobj != null)
                m_gridView.DoDragDrop(dobj, DragDropEffects.Move | DragDropEffects.Copy);
        }


        private void ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            if (m_gridView.SelectedRows.Count <= 0)
                e.Cancel = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((int)keyData == (int)Keys.Control + (int)Keys.C)
            {
                SetClipBoardText();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ClickCopyMenu(object sender, EventArgs args)
        {
            SetClipBoardText();
        }
        #endregion

        class DataGridViewNumberedRowHeaderCell : DataGridViewRowHeaderCell
        {
            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                if ((paintParts & DataGridViewPaintParts.SelectionBackground) != 0)
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, DataGridViewPaintParts.Background);

                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts & ~(DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.SelectionBackground));

                if ((paintParts & DataGridViewPaintParts.ContentForeground) != 0)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.None;
                    sf.FormatFlags |= StringFormatFlags.NoWrap;
                    Brush b = new SolidBrush(SystemColors.ControlText);
                    using (b) graphics.DrawString(Convert.ToString(rowIndex + 1), cellStyle.Font, b, cellBounds, sf);
                }
            }
        }

        private DataGridViewCell m_prevCell = null;
        private void GridViewCurrentCellChanged(object sender, EventArgs e)
        {
            if (m_gridView.CurrentCell == null)
                return;
            if (m_prevCell != null)
            {
                m_prevCell.Style.SelectionBackColor =
                       m_gridView.DefaultCellStyle.SelectionBackColor;
                m_prevCell.Style.SelectionForeColor =
                    m_gridView.DefaultCellStyle.SelectionForeColor;
            }
            if (m_gridView.Rows[m_gridView.CurrentCell.RowIndex].DefaultCellStyle.ForeColor ==
                m_headerForeColor)
            {
                m_prevCell = null;
                return;
            }
            m_gridView.CurrentCell.Style.SelectionBackColor =
                m_gridView.DefaultCellStyle.BackColor;
            m_gridView.CurrentCell.Style.SelectionForeColor =
                Color.Red;
            
            m_prevCell = m_gridView.CurrentCell;
        }

    }
}
