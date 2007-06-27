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
// written by Motokazu Ishikawa <m.ishikawa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EcellLib.PropertyWindow
{
    /// <summary>
    /// The Plugin Class to show property of object.
    /// </summary>
    public class PropertyWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// dgv (DataGridView) is property window grid.
        /// </summary>
        private DataGridView m_dgv = null;
        /// <summary>
        /// modelID of displaying property window.
        /// </summary>
        private string m_currentModelID = null;
        /// <summary>
        /// key ID of displaying property window.
        /// </summary>
        private string m_currentKey = null;
        /// <summary>
        /// type of dispaying property window.
        /// </summary>
        private string m_currentType = null;
        /// <summary>
        /// object data of displaying property window.
        /// </summary>
        private EcellObject m_currentObj = null;
        /// <summary>
        /// editable property list window.
        /// </summary>
        private EcellLib.PropertyEditor m_editor;
        /// <summary>
        /// key is property name, value is property data type.
        /// </summary>
        private Dictionary<string, EcellData> m_propDict;
        /// <summary>
        /// data manager.
        /// </summary>
        private DataManager m_dManager;
        /// <summary>
        /// System status.
        /// </summary>
        private int m_type;
        #endregion

        /// <summary>
        /// Constructor for PropertyWindow
        /// </summary>
        public PropertyWindow()
        {
            m_dManager = DataManager.GetDataManager();
            m_propDict = new Dictionary<string, EcellData>();
            m_dgv = new DataGridView();
            m_dgv.Dock = DockStyle.Fill;
            m_dgv.AllowUserToDeleteRows = false;
            m_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            m_dgv.AllowUserToAddRows = false;
            m_dgv.RowHeadersVisible = false;
            m_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            /*
            DataGridViewCheckBoxColumn checkRead = new DataGridViewCheckBoxColumn();
            checkRead.HeaderText = "Read";
            checkRead.Name = "Read";
            checkRead.ReadOnly = true;
            checkRead.Width = 40;
            checkRead.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewCheckBoxColumn checkWrite = new DataGridViewCheckBoxColumn();
            checkWrite.HeaderText = "Write";
            checkWrite.Name = "Write";
            checkWrite.ReadOnly = true;
            checkWrite.Width = 40;
            checkWrite.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            */

            DataGridViewTextBoxColumn textName = new DataGridViewTextBoxColumn();
            textName.HeaderText = "Name";
            textName.Name = "Name";
            textName.ReadOnly = true;

            DataGridViewTextBoxColumn textValue = new DataGridViewTextBoxColumn();
            textValue.HeaderText = "Value";
            textValue.Name = "Value";
            textValue.ReadOnly = true;

            m_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                textName, textValue});

            m_dgv.ReadOnly = true;
            m_dgv.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(m_dgv_DataBindingComplete);
            m_dgv.CellDoubleClick += new DataGridViewCellEventHandler(CellDoubleClick);
        }

        void m_dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        }

        /// <summary>
        /// Fill the contents of the dgv with EcellObject
        /// </summary>
        /// <param name="eo">DataGridView will be filled with data from this EcellObject.</param>
        internal void SetDataIntoGrid(EcellObject eo)
        {
            m_propDict.Clear();
            m_dgv.Rows.Clear();

            m_dgv.Rows.Add(new Object[] { "modelID", eo.modelID } );
            int ind = m_dgv.Rows.GetLastRow(DataGridViewElementStates.Visible);
            for (int i = 0; i < 2; i++)
            {
                m_dgv.Rows[ind].Cells[i].Style.BackColor = Color.Silver;
            }

            if (eo.type != "Model")
            {
                m_dgv.Rows.Add(new Object[] { "key", eo.key });
            }

            m_dgv.Rows.Add(new Object[] { "type", eo.type });
            ind = m_dgv.Rows.GetLastRow(DataGridViewElementStates.Visible);
            for (int i = 0; i < 2; i++)
            {
                m_dgv.Rows[ind].Cells[i].Style.BackColor = Color.Silver;
            }

            if (eo.type != "Model")
            {
                m_dgv.Rows.Add(new Object[] {  "classname", eo.classname });
            }
            if (eo.M_value == null) return;

            foreach (EcellData data in eo.M_value)
            {
                bool isWrite = data.M_isSettable;
                string name = data.M_name;
                string value = "";

                if (data.M_value == null) value = "";
                else value = data.M_value.ToString();

                /*
                if (data.M_name == "StepperID")
                {
                    List<EcellObject> list = m_dManager.GetStepper(null, eo.modelID);
                    int isHit = 0;
                    if (list != null)
                    {
                        foreach (EcellObject obj in list)
                        {
                            if (obj.key == data.M_value.ToString())
                            {
                                isHit = 1;
                                break;
                            }
                        }
                    }
                    if (isHit == 0)
                    {
                        MessageBox.Show("Selected object have no exist StepperID.", "WARNING",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }*/

                /*
                if (data.M_name == "VariableReferenceList")
                {
                    List<EcellReference> list = EcellReference.ConvertString(data.M_value.ToString());
                    foreach (EcellReference r in list)
                    {
                        string[] ele = r.fullID.Split(new char[] { ':' });

                        List<EcellObject> listObj = m_dManager.GetData(eo.modelID, ele[1]);
                        if (listObj == null || listObj.Count <= 0)
                        {

                            MessageBox.Show("Selected object have no exist variable in VariableReferenceList.", "WARNING",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                        int isHit = 0;
                        foreach (EcellObject sysObj in listObj)
                        {
                            foreach (EcellObject obj in sysObj.M_instances)
                            {
                                string key = ele[1] + ":" + ele[2];
                                if (obj.key == key)
                                {
                                    isHit = 1;
                                    break;
                                }
                            }
                            if (isHit == 1) break;
                        }
                        if (isHit == 0)
                        {
                            MessageBox.Show("Selected object have no exist variable in VariableReferenceList.", "WARNING",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                    }
                }
                */

                m_propDict.Add(data.M_name, data);
                m_dgv.Rows.Add(new Object[] { name, value });

                if (!isWrite)
                {
                    ind = m_dgv.Rows.GetLastRow(DataGridViewElementStates.Visible);
                    for (int i = 0; i < 2; i++)
                    {
                        m_dgv.Rows[ind].Cells[i].Style.BackColor = Color.Silver;
                    }
                }
            }
        }

        #region Event
        /// <summary>
        /// The action of double clicking in cell.
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">DataGridViewCellEventArgs</param>
        void CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (m_type != Util.LOADED && m_type != Util.NOTLOAD) return;
            try
            {
                m_editor = new PropertyEditor();
                m_editor.layoutPanel.SuspendLayout();
                m_editor.SetCurrentObject(m_currentObj);
                m_editor.SetDataType(m_currentObj.type);
                m_editor.PEApplyButton.Click += new EventHandler(m_editor.UpdateProperty);
                m_editor.LayoutPropertyEditor();
                m_editor.layoutPanel.ResumeLayout(false);
                m_editor.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get exception while layout property window.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion

        # region PluginBase
        /// <summary>
        /// Get menustrips for PropertyWindow.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get toolbar buttons for PropertyWindow.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the window form for PropertyWindow.
        /// </summary>
        /// <returns>UserControl</returns>
        public List<UserControl> GetWindowsForms()
        {
            UserControl control = new UserControl();
            control.Dock = DockStyle.Fill;
            control.Controls.Add(m_dgv);

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

            // When called with illegal arguments, this method will do nothing;
            if (modelID == null || key == null) // || modelID.Length <= 0 || key.Length <= 0)
            {
                return;
            }

            // dgv is filled with Ecell Object
            // When   1. No Ecell Object hasn't be shown on this dgv yet. (first two conditions)
            //    or  2. An Ecell Object which is different from the one presently displayed on this dgv 
            //           is selected. (last condition)
            if (m_currentModelID == null || m_currentKey == null || m_currentType == null ||
               !(m_currentModelID.Equals(modelID) && m_currentKey.Equals(key) &&
                   m_currentType.Equals(type)))
            {
                this.m_currentModelID = modelID;
                this.m_currentKey = key;
                this.m_currentType = type;

                DataManager dm = DataManager.GetDataManager();
                List<EcellObject> list;
                if (key.Contains(":"))
                { // not system
                    string[] keys = key.Split(new char[] { ':' });
                    list = dm.GetData(modelID, keys[0]);
                    if (list == null || list.Count == 0) return;
                    for (int i = 0; i < list.Count; i++)
                    {
                        List<EcellObject> insList = list[i].M_instances;
                        if (insList == null || insList.Count == 0) continue;
                        for (int j = 0; j < insList.Count; j++)
                        {
                            if (insList[j].key == key && insList[j].type == type)
                            {
                                this.m_currentObj = insList[j];
                                this.SetDataIntoGrid(insList[j]);
                                return;
                            }
                        }
                    }
                }
                else
                { // system
                    list = dm.GetData(modelID, key);
                    if (list == null || list.Count == 0) return;
                    this.m_currentObj = list[0];
                    this.SetDataIntoGrid(list[0]);
                    return;
                }
            }

            // When arguments matches to Ecell Objects presently shown on this view, there is nothing to do.
            return;
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            // nothing
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
            if (modelID == null) return;
            if (this.m_currentModelID == null) return;
            if (this.m_currentModelID == modelID && this.m_currentKey == key &&
                this.m_currentType == type)
            {
                this.SetDataIntoGrid(data);
                m_currentObj = data;
            }
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
            if (m_currentModelID == modelID && (key == null || key.Length <= 0))
            {
                m_dgv.Rows.Clear();
            }
            else if (m_currentModelID == modelID && m_currentKey.StartsWith(key))
            {
                if (type == "System" || m_currentType == type)
                    m_dgv.Rows.Clear();
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
            m_currentModelID = null;
            m_currentKey = null;
            m_dgv.Rows.Clear();
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
            // nothing
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(int type)
        {
            m_type = type;
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
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public Bitmap Print()
        {
            try
            {
                Bitmap bitmap = new Bitmap(m_dgv.Width, m_dgv.Height);
                m_dgv.DrawToBitmap(bitmap, m_dgv.ClientRectangle);
                return bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to create bitmap data.\n\n" + ex,
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
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
