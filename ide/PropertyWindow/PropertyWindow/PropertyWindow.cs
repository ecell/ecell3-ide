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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

using Ecell;
using Ecell.Objects;
using Ecell.Plugin;
using Ecell.Logger;
using Ecell.IDE;
using Ecell.Exceptions;

namespace Ecell.IDE.Plugins.PropertyWindow
{
    public partial class PropertyWindow : EcellDockContent, IEcellPlugin, IDataHandler, IDockContentProvider
    {
        #region Fields
        /// <summary>
        /// The displayed object.
        /// </summary>
        private EcellObject m_current = null;
        private EcellData m_data = null;
        /// <summary>
        /// Variable Reference List.
        /// </summary>
        private List<EcellReference> m_refList = new List<EcellReference>();
        /// <summary>
        /// Timer for executing redraw event at each 0.5 minutes.
        /// </summary>
        Timer m_time;
        /// <summary>
        /// Current status of project.
        /// </summary>
        private ProjectStatus m_type = ProjectStatus.Uninitialized;
        /// <summary>
        /// Dictionary of property of displayed object.
        /// </summary>
        private Dictionary<String, EcellData> m_propDic;
        /// <summary>
        /// Flag whether this properties is changing.
        /// </summary>
        private bool m_isChanging = false;
        /// <summary>
        /// 
        /// </summary>
        private DataGridViewComboBoxEditingControl m_combo = null;
        /// <summary>
        /// 
        /// </summary>
        private List<string> m_nonDataProps;
        /// <summary>
        /// 
        /// </summary>
        private List<string> m_procList = null;
        /// <summary>
        /// 
        /// </summary>
        private DataGridViewComboBoxCell m_stepperIDComboBox;

        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        protected ApplicationEnvironment m_env = null;
        #endregion

        private const string s_newPropPrefix = "userDefined";


        /// <summary>
        /// The application environment associated to this plugin
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            set { m_env = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyWindow()
        {
            m_nonDataProps = new List<string>();

            InitializeComponent();

            defineANewPropertyToolStripMenuItem.Enabled = false;
            deleteThisPropertyToolStripMenuItem.Enabled = false;

            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(FireTimer);
        }

        /// <summary>
        /// Change the property of data. 
        /// </summary>
        /// <param name="modelID">the modelID of object changed property.</param>
        /// <param name="key">the key of object changed property.</param>
        /// <param name="obj">the object changed property.</param>
        private bool NotifyDataChanged(string modelID, string key, EcellObject obj)
        {
            bool changeSuccess = true;
            m_isChanging = true;
            try
            {
                m_env.DataManager.DataChanged(modelID, key, obj.Type, obj);
                if (obj != m_current)
                {
                    UpdateProperties();
                    changeSuccess = false;
                }
            }
            finally
            {
                m_isChanging = false;
            }
            return changeSuccess;
        }

        /// <summary>
        /// Display the property of current object.
        /// </summary>
        private void ResetProperty()
        {
            if (m_current == null) return;
            foreach (EcellData d in m_current.Value)
            {
                if (!d.Value.IsDouble &&
                    !d.Value.IsInt)
                    continue;
                for (int i = 0; i < m_dgv.Rows.Count; i++)
                {
                    if (m_dgv.Rows[i].IsNewRow) continue;
                    if (m_dgv[0, i].Value == null) continue;
                    if (d.Name == (string)m_dgv[0, i].Value)
                    {
                        if (d.Name.Equals(Constants.xpathVRL))
                        {
                            m_dgv[1, i].Value = MessageResources.LabelEdit;
                        }
                        else
                        {
                            m_dgv[1, i].Value = (string)d.Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event of clicking the formulator button.
        /// Show the window to edit the formulator.
        /// </summary>
        private string ShowFormulatorDialog(string formula)
        {
            FormulatorDialog fwin = new FormulatorDialog();
            using (fwin)
            {
                List<string> list = new List<string>();
                list.Add("self.getSuperSystem().SizeN_A");
                foreach (EcellData d in m_current.Value)
                {
                    String str = d.Name;
                    if (str != EcellProcess.ACTIVITY &&
                        str != EcellProcess.EXPRESSION && str != EcellProcess.NAME &&
                        str != EcellProcess.PRIORITY && str != EcellProcess.STEPPERID &&
                        str != EcellProcess.VARIABLEREFERENCELIST && str != EcellProcess.ISCONTINUOUS)
                        list.Add(str);
                }
                foreach (EcellReference r in m_refList)
                {
                    list.Add(r.Name + ".MolarConc");
                }
                foreach (EcellReference r in m_refList)
                {
                    list.Add(r.Name + ".Value");
                }
                fwin.AddReserveString(list);

                fwin.ImportFormulate(formula);
                if (fwin.ShowDialog() != DialogResult.OK)
                    return null;

                return fwin.Result;
            }
        }

        /// <summary>
        /// Update the property value with simulation.
        /// </summary>
        void UpdatePropForSimulation()
        {
            // Checker for update.
            if (m_current is EcellText)
                return;
            if (m_current == null || m_current.Value == null)
                return;
            double l_time = m_env.DataManager.GetCurrentSimulationTime();
            if (l_time == 0.0)
                return;

            // Set current parameter.
            foreach (EcellData d in m_current.Value)
            {
                if (!d.Value.IsDouble || !d.Gettable)
                    continue;
                double value = m_env.DataManager.GetPropertyValue(d.EntityPath);
                foreach (DataGridViewRow row in m_dgv.Rows)
                {
                    if ((string)row.Cells[0].Value != d.Name)
                        continue;

                    row.Cells[1].Value = value.ToString();
                    if (d.Name == "FullID")
                        row.Cells[1].ReadOnly = true;
                    break;
                }
            }
        }


        /// <summary>
        /// Get the object from DataManager.
        /// </summary>
        /// <param name="modelID">model ID of object.</param>
        /// <param name="key">key of object.</param>
        /// <param name="type">type of object.</param>
        /// <returns>the result object.</returns>
        EcellObject GetData(string modelID, string key, string type)
        {
            return m_env.DataManager.GetEcellObject(modelID, key, type);
        }

        private void UpdateExpression(string express)
        {
            EcellObject obj = m_current.Clone();
            foreach (EcellData data in obj.Value)
            {
                if (data.Name.Equals(Constants.xpathExpression))
                {
                    data.Value = new EcellValue(express);
                    break;
                }
            }
            NotifyDataChanged(m_current.ModelID, m_current.Key, obj);
        }

        /// <summary>
        /// Add the property to PropertyWindow.
        /// </summary>
        /// <param name="d">EcellData of property.</param>
        /// <param name="type">Type of property.</param>
        /// <returns>Row in DataGridView.</returns>
        DataGridViewRow AddProperty(EcellData d, string type)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell propNameCell = new DataGridViewTextBoxCell();
            DataGridViewCell propValueCell;
            propNameCell.Value = d.Name;
            row.Cells.Add(propNameCell);
            if (m_propDic == null || m_current.Type != Constants.xpathProcess
                || m_propDic.ContainsKey(d.Name))
            {
                propNameCell.Style.BackColor = Color.LightGray;
                propNameCell.Style.SelectionBackColor = Color.LightGray;
                propNameCell.ReadOnly = true;
            }
            else
            {
                propNameCell.ReadOnly = false;
                propNameCell.Style.BackColor = SystemColors.Window;
                propNameCell.Style.ForeColor = SystemColors.WindowText;
                propNameCell.Style.SelectionBackColor = SystemColors.Highlight;
                propNameCell.Style.SelectionForeColor = SystemColors.HighlightText; 
            }

            if (d.Value == null) return null;

            if (d.Name == Constants.xpathExpression)
            {
                propValueCell = new DataGridViewOutOfPlaceEditableCell();
                propValueCell.Value = (string)d.Value;
                ((DataGridViewOutOfPlaceEditableCell)propValueCell).OnOutOfPlaceEditRequested =
                    delegate(DataGridViewOutOfPlaceEditableCell c)
                    {
                        string retval = ShowFormulatorDialog(c.Value == null ? "" : c.Value.ToString());
                        if (retval != null)
                        {                            
                            propValueCell.Value = retval;
                            UpdateExpression(retval);
                            return true;
                        }
                        return false;
                    };
            }
            else if (d.Name == Constants.xpathVRL)
            {
                propValueCell = new DataGridViewLinkCell();
                propValueCell.Value = MessageResources.LabelEdit;
            }
            else if (d.Name == Constants.xpathStepperID)
            {
                propValueCell = new DataGridViewComboBoxCell();
                bool isexist = false;
                string stepperName = d.Value.ToString();
                foreach (EcellObject obj in m_env.DataManager.GetStepper(null, m_current.ModelID))
                {
                    if (!string.IsNullOrEmpty(stepperName) &&
                        stepperName.Equals(obj.Key))
                    {
                        isexist = true;
                    }
                    ((DataGridViewComboBoxCell)propValueCell).Items.Add(obj.Key);
                }
                if (isexist)
                    propValueCell.Value = d.Value.ToString();
                m_stepperIDComboBox = (DataGridViewComboBoxCell)propValueCell;
            }
            else
            {
                propValueCell = new DataGridViewTextBoxCell();
                propValueCell.Value = (string)d.Value;
            }
            row.Cells.Add(propValueCell);
            m_dgv.Rows.Add(row);

            if (d.Settable)
            {
                propValueCell.ReadOnly = false;
            }
            else
            {
                propValueCell.ReadOnly = true;
                propValueCell.Style.ForeColor = SystemColors.GrayText;
            }
            propValueCell.Tag = d;

            return row;
        }

        DataGridViewRow AddNonDataProperty(string propName, string propValue, bool readOnly)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell propNameCell = new DataGridViewTextBoxCell();
            DataGridViewCell propValueCell = null;
            row.Cells.Add(propNameCell);
            propNameCell.Style.BackColor = Color.LightGray;
            propNameCell.Style.SelectionBackColor = Color.LightGray;
            propNameCell.ReadOnly = true;
            propNameCell.Value = propName;

            if (propName == Constants.xpathClassName)
            {
                if (m_current.Type == Constants.xpathSystem ||
                    m_current.Type == Constants.xpathVariable)
                {
                    propValueCell = new DataGridViewComboBoxCell();
                    ((DataGridViewComboBoxCell)propValueCell).Items.Add(propValue);
                    propValueCell.Value = propValue;
                }
                else if (m_current.Type == Constants.xpathText)
                {
                    propValueCell = new DataGridViewTextBoxCell();
                    propValueCell.Value = "Text";
                    readOnly = true; // forcefully marked as readonly
                }
                else
                {
                    propValueCell = new DataGridViewComboBoxCell();
                    bool isHit = false;
                    foreach (string pName in m_procList)
                    {
                        ((DataGridViewComboBoxCell)propValueCell).Items.Add(pName);
                        if (pName == propValue)
                            isHit = true;
                    }

                    if (!isHit)
                    {
                        ((DataGridViewComboBoxCell)propValueCell).Items.Add(propValue);
                    }
                    propValueCell.Value = propValue;
                }
            }
            else
            {
                propValueCell = new DataGridViewTextBoxCell();
                propValueCell.Value = propValue;
            }
            row.Cells.Add(propValueCell);
            propValueCell.Tag = propName;
            propValueCell.ReadOnly = readOnly;
            if (readOnly)
                propValueCell.Style.ForeColor = SystemColors.GrayText;

            m_dgv.Rows.Add(row);
            m_nonDataProps.Add(propName);
            return row;
        }

        private void ReloadProperties()
        {
            m_propDic = null;
            m_dgv.Rows.Clear();
            if (m_current == null)
                return;

            string localID = m_current.LocalID;
            string parentSystemPath = m_current.ParentSystemID;

            if (m_current.Type == Constants.xpathProcess &&
                    m_env.DataManager.IsEnableAddProperty(m_current.Classname))
            {
                defineANewPropertyToolStripMenuItem.Enabled = true;
                m_propDic = m_env.DataManager.GetProcessProperty(m_current.Classname);
            }
            else
            {
                defineANewPropertyToolStripMenuItem.Enabled = false;
                deleteThisPropertyToolStripMenuItem.Enabled = false;
                m_propDic = null;
            }

            {
                AddNonDataProperty("ModelID", m_current.ModelID, true);
                AddNonDataProperty("ID", localID, false);
                AddNonDataProperty("ClassName", m_current.Classname, false);
            }

            EcellData sizeData = null;
            foreach (EcellData d in m_current.Value)
            {
                if (d.Name == Constants.xpathSize)
                {
                    sizeData = d;
                    continue;
                }

                AddProperty(d, m_current.Type);
                if (d.Name == EcellProcess.VARIABLEREFERENCELIST)
                    m_refList = EcellReference.ConvertFromEcellValue(d.Value);
            }

            if (m_current.Type == Constants.xpathSystem)
            {
                EcellSystem system = (EcellSystem)m_current;
                EcellData dSize = new EcellData();
                dSize.EntityPath = sizeData.EntityPath;
                dSize.Name = Constants.xpathSize;
                dSize.Settable = true;
                dSize.Logable = sizeData.Logable;
                dSize.Logged = sizeData.Logged;
                dSize.Value = new EcellValue(system.SizeInVolume);
                AddProperty(dSize, m_current.Type);
            }

            if (m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
            {
                UpdatePropForSimulation();
            }

            label1.Text = m_current.FullID;
        }

        private void UpdateProperties()
        {
            if (m_current == null)
                return;

            foreach (DataGridViewRow r in m_dgv.Rows)
            {
                EcellData prop = r.Cells[1].Tag as EcellData;
                if (prop == null)
                {
                    string data = r.Cells[1].Tag as string;
                    if (data.Equals(Constants.xpathID))
                        r.Cells[1].Value = m_current.LocalID;
                    
                    continue;
                }
                EcellData d = m_current.GetEcellData(prop.Name);
                if ((m_env.PluginManager.Status == ProjectStatus.Running ||
                    m_env.PluginManager.Status == ProjectStatus.Stepping ||
                    m_env.PluginManager.Status == ProjectStatus.Suspended) &&
                    (d.Value.IsInt || d.Value.IsDouble) &&
                    d.Gettable)
                {
                    EcellValue v = m_env.DataManager.GetEntityProperty(d.EntityPath);
                    d = new EcellData(d.Name, v, d.EntityPath);
                }
                if (d == null) continue;
                if (r.Cells[1] is DataGridViewCell)
                {
                    if (!r.Cells[1].Value.ToString().Equals(d.Value.ToString()))
                    {
                        if (d.Name.Equals(Constants.xpathVRL))
                        {
                            r.Cells[1].Value = MessageResources.LabelEdit;
                        }
                        else
                        {
                            r.Cells[1].Value = d.Value.ToString();
                        }
                    }                    
                }
            }
        }

        #region PluginBase
        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string modelID, string key, string type)
        {
            // When called with illegal arguments, this method will do nothing;
            if (String.IsNullOrEmpty(modelID) || String.IsNullOrEmpty(key))
            {
                return;
            }

            EcellObject obj = GetData(modelID, key, type);
            if (obj == null) return;
            m_current = obj;

            ReloadProperties();
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            SelectChanged(modelID, key, type);
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            if (data == null)
                return;
            foreach (EcellObject obj in data)
            {
                if (obj.Type == Constants.xpathStepper)
                {
                    Trace.WriteLine(obj.Type);
                    if (m_stepperIDComboBox != null)
                        m_stepperIDComboBox.Items.Add(obj.Key);
                }
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
            if (m_current == null)
                return;
            if (m_isChanging)
            {
                if (m_current.Key == key)
                {
                    m_current = data;
                    UpdateProperties();
                }
                return;
            }
            if (m_current.ModelID == modelID && m_current.Key == key && m_current.Type == type)
            {
                m_current = data;
                ReloadProperties();
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
            if (m_current == null) return;
            if (type == Constants.xpathStepper)
            {
                if (m_stepperIDComboBox != null && m_stepperIDComboBox.Items.Contains(key))
                {
                    m_stepperIDComboBox.Items.Remove(key);
                }
            }
            else if (type == m_current.Type)
            {
                if (modelID == m_current.ModelID && key == m_current.Key)
                    Clear();
            }
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public void Clear()
        {
            m_current = null;
            ReloadProperties();
            label1.Text = "";
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Running)
            {
                m_dgv.ReadOnly = true;
//                m_dgv.Enabled = false;
                m_time.Enabled = true;
                m_time.Start();
            }
            else if (type == ProjectStatus.Suspended)
            {
                m_dgv.ReadOnly = false;
//                m_dgv.Enabled = true;
                m_time.Enabled = false;
                m_time.Stop();
                try
                {
                    UpdatePropForSimulation();
                }
                catch (Exception)
                {
                    // 他のプラグインでデータを編集したか
                    // シミュレーションが異常終了したがデータを取得できなかったため。
                    // 他のプラグインでエラーメッセージが表示されるので
                    // ここでは出さないようにする。
                }
            }
            else if (type == ProjectStatus.Loading)
            {
                m_procList = m_env.DataManager.CurrentProject.ProcessDmList;
            }
            else if (type == ProjectStatus.Loaded)
            {
//                m_dgv.Enabled = true;
                m_dgv.ReadOnly = false;
                m_procList = m_env.DataManager.CurrentProject.ProcessDmList;
                if (m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
                {
                    m_time.Enabled = false;
                    m_time.Stop();
                    ResetProperty();
                }
            }
            else if (type == ProjectStatus.Uninitialized)
            {
                m_dgv.ReadOnly = true;
//                m_dgv.Enabled = false;
                m_procList = null;
            }
            else if (type == ProjectStatus.Stepping)
            {
                m_dgv.ReadOnly = false;
//                m_dgv.Enabled = true;
                try
                {
                    UpdatePropForSimulation();
                }
                catch (Exception)
                {
                    // 他のプラグインでデータを編集したか
                    // シミュレーションが異常終了したがデータを取得できなかったため。
                    // 他のプラグインでエラーメッセージが表示されるので
                    // ここでは出さないようにする。
                }
            }
            m_type = type;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PropertyWindow"</returns>
        public string GetPluginName()
        {
            return "PropertyWindow";
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
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IPropertyItem> GetPropertySettings()
        {
            return null;
        }

        #endregion


        #region Events
        /// <summary>
        /// Event when the mouse is down in row.
        /// This data of row is used for Drag and Drop.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">MouseEventArgs.</param>
        private void MouseDownOnDataGrid(object sender, MouseEventArgs e)
        {
            DataGridView v = sender as DataGridView;

            DataGridView.HitTestInfo hti = v.HitTest(e.X, e.Y);
            if (hti.RowIndex <= 0)
                return;

//            m_dgv[1, hti.RowIndex].Selected = true;
        }

        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void FireTimer(object sender, EventArgs e)
        {
            m_time.Enabled = false;
            try
            {
                UpdatePropForSimulation();
            }
            catch (Exception)
            {
                // 他のプラグインでデータを編集したか
                // シミュレーションが異常終了したがデータを取得できなかったため。
                // 他のプラグインでエラーメッセージが表示されるので
                // ここでは出さないようにする。
            }

            m_time.Enabled = true;
        }

        /// <summary>
        /// Event when mouse is leave on DataDridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">EventArgs</param>
        void LeaveMouse(object sender, EventArgs e)
        {
            m_dgv.EndEdit();
        }

        /// <summary>
        /// Event when user click the cell in DataGridView.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs.</param>
        void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Return immediately in case one of row headers or column headers is clicked.
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            DataGridViewCell c = m_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCell;
            EcellData d = c.Tag as EcellData;
            if (d != null && d.Name == Constants.xpathVRL)
                VarRefListCellClicked(c, e);
        }

        void VarRefListCellClicked(object o, EventArgs e)
        {
            DataGridViewCell c = o as DataGridViewCell;
            List<EcellReference> list = EcellReference.ConvertFromEcellValue(((EcellData)c.Tag).Value);
            VariableReferenceEditDialog win = new VariableReferenceEditDialog(m_env.DataManager, m_env.PluginManager, list);
            using (win)
            {
                if (win.ShowDialog() != DialogResult.OK)
                    return;
                EcellObject eo = m_current.Clone();
                EcellData nd = eo.GetEcellData(((EcellData)c.Tag).Name);
                nd.Value = EcellReference.ConvertToEcellValue(win.ReferenceList);
                try
                {
                    if (NotifyDataChanged(eo.ModelID, eo.Key, eo))
                    {
                        c.Tag = nd;
                    }
//                    m_current = eo;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    Util.ShowErrorDialog(ex.Message);
                }
            }
        }

        /// <summary>
        /// Event when the value of cell is changed.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs.</param>
        private void ChangeProperty(object sender, DataGridViewCellParsingEventArgs e)
        {
            e.ParsingApplied = true;
            if (m_current == null)
                return;
            if (m_env.PluginManager.Status == ProjectStatus.Running)
                return;
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            if (m_isChanging)
                return;

            DataGridViewCell editCell = m_dgv[e.ColumnIndex, e.RowIndex];
            // Update the name of user defined Property.
            if (e.ColumnIndex == 0)
            {
                string propName = e.Value.ToString();
                string oldName = editCell.Value.ToString();
                try
                {
                    if (propName.Equals(oldName))
                        return;
                    if (m_current.IsEcellValueExists(propName)
                            || m_nonDataProps.Contains(propName))
                        throw new EcellException(MessageResources.ErrSameProp);

                    // Set value.
                    DataGridViewCell valueCell = m_dgv[1, e.RowIndex];
                    double value;
                    if (valueCell.Tag == null || valueCell.Value.Equals(""))
                    {
                        value = 0.0d;
                    }
                    else
                    {
                        value = Convert.ToDouble(valueCell.Value);
                    }
                    m_current.RemoveEcellValue(oldName);
                    m_current.SetEcellValue(propName, new EcellValue(value));
                    valueCell.Tag = m_current.GetEcellData(propName);
                }
                catch (Exception ex)
                {
                    e.Value = oldName;
                    Trace.WriteLine(ex);
                    m_dgv.MouseLeave -= new EventHandler(this.LeaveMouse);
                    Util.ShowErrorDialog(ex.Message);
                    m_dgv.MouseLeave += new EventHandler(this.LeaveMouse);
                }
                return;
            }
            // Ignore invalid index.
            if (e.ColumnIndex != 1)
                return;
            // Update property value.
            try
            {
                // Update property.
                if (editCell.Tag is EcellData)
                {
                    EcellData tag = editCell.Tag as EcellData;
                    string data = e.Value.ToString();
                    if ((m_env.PluginManager.Status == ProjectStatus.Running ||
                        m_env.PluginManager.Status == ProjectStatus.Stepping ||
                        m_env.PluginManager.Status == ProjectStatus.Suspended) &&
                        !tag.Name.Equals(Constants.xpathSize) &&
                        !tag.Name.Equals(Constants.xpathExpression) &&
                        !tag.Name.Equals(Constants.xpathActivity))
                    {
                        m_env.DataManager.SetEntityProperty(tag.EntityPath, data);
                        UpdatePropForSimulation();
                    }
                    else
                    {
                        EcellObject eo = m_current.Clone();
                        EcellData d = eo.GetEcellData(tag.Name);
                        EcellValue value;
                        try
                        {
                            if (tag.Name == Constants.xpathSize)
                            {
                                value = new EcellValue(Convert.ToDouble(data));
                                EcellSystem system = (EcellSystem)eo;
                                system.SizeInVolume = (double)value;
                            }
                            else
                            {
                                if (d.Value.IsDouble)
                                    value = new EcellValue(Convert.ToDouble(data));
                                else if (d.Value.IsInt)
                                    value = new EcellValue(Convert.ToInt32(data));
                                else
                                    value = new EcellValue(data);
                                d.Value = value;
                            }
                        }
                        catch (Exception ex)
                        {
                            e.Value = editCell.Value;
                            Trace.WriteLine(ex);
                            m_dgv.MouseLeave -= new EventHandler(this.LeaveMouse);
                            Util.ShowErrorDialog(MessageResources.ErrFormat);
                            m_dgv.MouseLeave += new EventHandler(this.LeaveMouse);
                            return;
                        }
                        if (!NotifyDataChanged(m_current.ModelID, m_current.Key, eo))
                        {
                            e.Value = editCell.Value;
                        }
                    }
                }
                // Update ID.
                else if (editCell.Tag is string)
                {
                    string propName = editCell.Tag as string;
                    if (!propName.Equals("ID"))
                        return;
                    string tmpID = e.Value.ToString();
                    // Error check.
                    if (tmpID.Equals(m_current.Key))
                        return;
                    if (string.IsNullOrEmpty(tmpID) || Util.IsReservedID(tmpID) || Util.IsNGforID(tmpID))
                        throw new EcellException(MessageResources.ErrID);

                    EcellObject p = m_current.Clone();
                    if (p.Type == Constants.xpathSystem)
                    {
                        // ルートの場合、ルートの/とデミリタの/が2つ重なる
                        if (p.ParentSystemID.Equals("/"))
                            p.Key = p.ParentSystemID + tmpID;
                        else
                            p.Key = p.ParentSystemID + Constants.delimiterPath + tmpID;
                    }
                    else
                        p.Key = p.ParentSystemID + Constants.delimiterColon + tmpID;
                    if (!NotifyDataChanged(m_current.ModelID, m_current.Key, p))
                    {
                        e.Value = editCell.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Value = editCell.Value;
                Trace.WriteLine(ex);
                m_dgv.MouseLeave -= new EventHandler(this.LeaveMouse);
                Util.ShowErrorDialog(ex.Message);
                m_dgv.MouseLeave += new EventHandler(this.LeaveMouse);
            }
        }
        #endregion

        #region IDataHandler メンバ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        public void AdvancedTime(double time)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public void LoggerAdd(LoggerEntry entry)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="parameterID"></param>
        public void ParameterAdd(string projectID, string parameterID)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="parameterID"></param>
        public void ParameterDelete(string projectID, string parameterID)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="parameterID"></param>
        public void ParameterUpdate(string projectID, string parameterID)
        {
            if (parameterID == m_env.DataManager.GetCurrentSimulationParameterID() &&
                m_current != null && m_stepperIDComboBox != null)
            {
                m_stepperIDComboBox.Items.Clear();
                foreach (EcellObject obj in m_env.DataManager.GetStepper(parameterID, m_current.ModelID))
                {
                    m_stepperIDComboBox.Items.Add(obj.Key);
                }
                if (!m_stepperIDComboBox.Items.Contains(m_stepperIDComboBox.Value))
                {
                    m_stepperIDComboBox.Value = m_stepperIDComboBox.Items[0];
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="parameterID"></param>
        public void ParameterSet(string projectID, string parameterID)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public void RemoveSelect(string modelID, string key, string type)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetSelect()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="directory"></param>
        public void SaveModel(string modelID, string directory)
        {
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Delegate> GetPublicDelegate()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return new EcellDockContent[] { this };
        }

        
        private void defineANewPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> existingPropsIndices = new List<int>();
            foreach (DataGridViewRow r in m_dgv.Rows)
            {
                if (r.Cells[0].Value != null && ((string)r.Cells[0].Value).StartsWith(s_newPropPrefix))
                {
                    try
                    {
                        existingPropsIndices.Add(
                            int.Parse(((string)r.Cells[0].Value).Substring(s_newPropPrefix.Length)));
                    }
                    catch (Exception) { }
                }
            }

            int newPropIndex = 1;
            while (existingPropsIndices.Contains(newPropIndex))
                newPropIndex++;

            string propName = s_newPropPrefix + newPropIndex;
            m_current.SetEcellValue(propName, new EcellValue(0.0));

            m_env.DataManager.DataChanged(m_current.ModelID,
                m_current.Key, m_current.Type, m_current);
        }

        private void deleteThisPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_propDic == null)
                return;

            DataGridViewRow row = m_dgv.CurrentRow;
            EcellData data = row.Cells[1].Tag as EcellData;

            if (!m_propDic.ContainsKey(data.Name))
            {
                EcellObject p = m_current.Clone();
                foreach (EcellData d in p.Value)
                {
                    if (d.Name == data.Name)
                    {
                        p.Value.Remove(d);
                        break;
                    }
                }
                try
                {
                    NotifyDataChanged(m_current.ModelID, m_current.Key, p);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    Util.ShowErrorDialog(ex.Message);
                }
                ReloadProperties();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (m_dgv.CurrentRow == null ||
                m_dgv.CurrentRow.Cells[1].Tag == null)
            {
                e.Cancel = true;
                return;
            }
            object tag = m_dgv.CurrentRow.Cells[1].Tag;
            deleteThisPropertyToolStripMenuItem.Enabled =
                m_propDic != null && tag is EcellData &&
                !m_propDic.ContainsKey(((EcellData)tag).Name);

            loggingToolStripMenuItem.Enabled =
                tag is EcellData &&
                ((EcellData)tag).Logable;
            loggingToolStripMenuItem.Checked =
                tag is EcellData &&
                ((EcellData)tag).Logged;

            observedToolStripMenuItem.Enabled =
                tag is EcellData &&
                ((EcellData)tag).Logable;

            observedToolStripMenuItem.Checked =
                tag is EcellData &&
                m_env.DataManager.IsContainsObservedData(((EcellData)tag).EntityPath);
        

            parameterToolStripMenuItem.Enabled =
                tag is EcellData &&
                ((EcellData)tag).Settable && ((EcellData)tag).Value.IsDouble;
            if (tag is EcellData && ((EcellData)tag).Name.Equals(Constants.xpathSize))
            {
                string entityPath = Constants.xpathVariable + ":" + m_current.Key + ":SIZE:Value";
                parameterToolStripMenuItem.Checked =
                    m_env.DataManager.IsContainsParameterData(entityPath);
            }
            else
            {
                parameterToolStripMenuItem.Checked =
                    tag is EcellData &&
                    m_env.DataManager.IsContainsParameterData(((EcellData)tag).EntityPath);
            }

            if (m_env.DataManager.CurrentProject.SimulationStatus == SimulationStatus.Wait)
            {
                propertyToolStripMenuItem.Enabled = true;
            }
            else
            {
                propertyToolStripMenuItem.Enabled = false;
            }

            m_data = tag != null ? tag as EcellData : null;
        }

        private void combo_selectedIndexChanged(object sender, EventArgs e)
        {
            string newClassName = (string)((DataGridViewComboBoxEditingControl)sender).SelectedItem;
            if (newClassName.Equals(m_current.Classname))
            {
                return;
            }
            List<EcellData> props = new List<EcellData>();
            // Get Process properties.
            Dictionary<string, EcellData> properties = m_env.DataManager.GetProcessProperty(newClassName);
            foreach (KeyValuePair<string, EcellData> pair in properties)
            {
                EcellData val = pair.Value;
                if (pair.Value.Name == Constants.xpathStepperID)
                {
                    List<EcellObject> steppers = m_env.DataManager.GetStepper(null, m_current.ModelID);
                    if (steppers.Count > 0)
                    {
                        val = new EcellData(pair.Value.Name,
                            new EcellValue(steppers[0].Key),
                            val.EntityPath);
                    }
                }
                else if (pair.Value.Name == Constants.xpathVRL)
                {
                    EcellData d  = m_current.GetEcellData(Constants.xpathVRL);
                    if (d != null)
                        val = d;
                }
                props.Add(val);
            }
            EcellObject obj = EcellObject.CreateObject(
                m_current.ModelID, m_current.Key, m_current.Type,
                newClassName, props);
            obj.SetPosition(m_current);
            NotifyDataChanged(obj.ModelID, obj.Key, obj);
            ReloadProperties();
        }

        void m_dgv_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (m_combo != null)
            {
                m_combo.SelectedIndexChanged -= new System.EventHandler(combo_selectedIndexChanged);
                m_combo = null;
            }
        }

        private void m_dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewCell cell = m_dgv[1, m_dgv.CurrentCell.RowIndex];
            if (cell.Tag is string)
            {
                if ((string)cell.Tag == "ClassName" && cell is DataGridViewComboBoxCell)
                {
                    m_combo = (DataGridViewComboBoxEditingControl)e.Control;
                    m_combo.SelectedIndexChanged += new EventHandler(combo_selectedIndexChanged);
                }
            }
        }


        private void MouseMoveOnDataGridView(object sender, MouseEventArgs e)
        {
            DataGridView v = sender as DataGridView;

            DataGridView.HitTestInfo hti = v.HitTest(e.X, e.Y);
            if (hti.RowIndex <= 0)
                return;

            if (e.Button != MouseButtons.Left)
                return;

            string name = v[0, hti.RowIndex].Value as string;
            if (string.IsNullOrEmpty(name))
                return;

            // Get EcellData.
            EcellData data = m_current.GetEcellData(name);
            if (data == null)
                return;
            if (!data.Logable && !data.Settable)
                return;
            if (!data.Value.IsDouble)
                return;

            // Create new EcellDragObject.
            EcellDragObject dobj = new EcellDragObject(m_current.ModelID);

            // Add Entry.
            dobj.Entries.Add(new EcellDragEntry(
                m_current.Key,
                m_current.Type,
                data.EntityPath,
                data.Settable,
                data.Logable));

            // Drag & Drop Event.
            v.DoDragDrop(dobj, DragDropEffects.Move | DragDropEffects.Copy);

        }

        private void ClickLoggingMenu(object sender, EventArgs e)
        {
            if (m_data == null) return;
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            string prop = m_data.Name;

            if (m.Checked)
            {
                m_current.GetEcellData(prop).Logged = false;
            }
            else
            {
                m_env.LoggerManager.AddLoggerEntry(m_current.ModelID,
                    m_current.Key, m_current.Type, m_data.EntityPath);
                m_current.GetEcellData(prop).Logged = true;
            }
            m_env.DataManager.DataChanged(m_current.ModelID,
                m_current.Key, m_current.Type, m_current);
        }

        private void ClickObservedDataMenu(object sender, EventArgs e)
        {
            if (m_data == null) return;
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            string prop = m_data.Name;

            if (m.Checked)
            {
                m_env.DataManager.RemoveObservedData(
                    new EcellObservedData(m_current.GetEcellData(prop).EntityPath, 0.0));
            }
            else
            {
                EcellData d = m_current.GetEcellData(prop);
                m_env.DataManager.SetObservedData(
                    new EcellObservedData(
                        d.EntityPath,
                        Convert.ToDouble(d.Value.ToString())));
            }
        }

        private void ClickUnknownParameterMenu(object sender, EventArgs e)
        {
            if (m_data == null) return;
            ToolStripMenuItem m = (ToolStripMenuItem)sender;
            string prop = m_data.Name;
            string entityPath = "";

            if (prop.Equals(Constants.xpathSize))
            {
                entityPath = Constants.xpathVariable + ":" + m_current.Key + ":SIZE:Value";
            }
            else
            {
                EcellData d = m_current.GetEcellData(prop);
                entityPath = d.EntityPath;
            }

            if (m.Checked)
            {
                m_env.DataManager.RemoveParameterData(
                    new EcellParameterData(entityPath, 0.0));
            }
            else
            {
                EcellData d = m_current.GetEcellData(prop);
                m_env.DataManager.SetParameterData(
                    new EcellParameterData(
                        entityPath,
                        Convert.ToDouble(d.Value.ToString())));
            }
        }

        private void ClickShowPropertyMenu(object sender, EventArgs e)
        {
            PropertyEditor.Show(m_env, m_current);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (m_dgv.CurrentCell != null && m_dgv.CurrentCell is DataGridViewComboBoxCell)
                return base.ProcessCmdKey(ref msg, keyData);
            if ((int)keyData == (int)Keys.Control + (int)Keys.C)
            {
                if (m_dgv.CurrentCell != null && m_dgv.CurrentCell.Value != null)
                {
                    string copytext = m_dgv.CurrentCell.Value.ToString();
                    Clipboard.SetText(copytext);
                }
                return true;
            }
            if ((int)keyData == (int)Keys.Control + (int)Keys.V)
            {
                string pastetext = Clipboard.GetText();
                if (!String.IsNullOrEmpty(pastetext) && m_dgv.CurrentCell != null &&
                    m_dgv.CurrentCell.ReadOnly == false)
                {
                    m_dgv.CurrentCell.Value = pastetext;
                }
                return true;
            }
            if ((int)keyData == (int)Keys.Control + (int)Keys.X)
            {
                if (m_dgv.CurrentCell != null && m_dgv.CurrentCell.Value != null &&
                    m_dgv.CurrentCell.ReadOnly == false)
                {
                    string cuttext = m_dgv.CurrentCell.Value.ToString();
                    Clipboard.SetText(cuttext);
                    m_dgv.CurrentCell.Value = "";
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
