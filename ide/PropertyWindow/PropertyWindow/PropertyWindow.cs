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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Formulator;

namespace EcellLib.PropertyWindow
{
    /// <summary>
    /// The Plugin Class to show property of object.
    /// </summary>
    public class PropertyWindow : PluginBase
    {
        #region Fields
        private EcellObject m_current = null;
        /// <summary>
        /// dgv (DataGridView) is property window grid.
        /// </summary>
        private DataGridView m_dgv = null;
        /// <summary>
        /// Window to set the list of variable.
        /// </summary>
        private VariableRefWindow m_win;
        /// <summary>
        /// Window to set the expression of process.
        /// </summary>
        private FormulatorWindow m_fwin;
        /// <summary>
        /// Control to display the expression.
        /// </summary>
        private FormulatorControl m_cnt;
        /// <summary>
        /// Controller to edit ComboBox in DataGridView.
        /// </summary>
        private DataGridViewComboBoxEditingControl m_ComboControl = null;
        /// <summary>
        /// Variable Reference List.
        /// </summary>
        public String m_refStr = null;
        /// <summary>
        /// Timer for executing redraw event at each 0.5 minutes.
        /// </summary>
        System.Windows.Forms.Timer m_time;
        System.Windows.Forms.Timer m_deletetime;
        private int m_type = Util.NOTLOAD;
        /// <summary>
        /// Expression.
        /// </summary>
        private String m_expression = null;
        /// <summary>
        /// data manager.
        /// </summary>
        private DataManager m_dManager;
        private Dictionary<String, EcellData> m_propDic;
        /// <summary>
        /// ResourceManager for PropertyWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResProperty));
        private bool m_isChanging = false;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyWindow()
        {
            m_dManager = DataManager.GetDataManager();
            m_dgv = new DataGridView();
            m_dgv.Dock = DockStyle.Fill;
            m_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            m_dgv.AllowUserToAddRows = false;
            m_dgv.AllowUserToDeleteRows = false;
            m_dgv.RowHeadersVisible = true;

            DataGridViewTextBoxColumn textName = new DataGridViewTextBoxColumn();
            textName.HeaderText = "Name";
            textName.Name = "Name";
            textName.ReadOnly = false;

            DataGridViewTextBoxColumn textValue = new DataGridViewTextBoxColumn();
            textValue.HeaderText = "Value";
            textValue.Name = "Value";
            textValue.ReadOnly = true;

            m_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    textName, textValue});

            m_dgv.UserDeletingRow += new DataGridViewRowCancelEventHandler(DgvUserDeletingRow);
            m_dgv.CellClick += new DataGridViewCellEventHandler(CellClick);
            m_dgv.CellEndEdit += new DataGridViewCellEventHandler(PropertyChanged);
            m_dgv.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(DgvEditingControlShowing);
            
            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(TimerFire);

            m_deletetime = new System.Windows.Forms.Timer();
            m_deletetime.Enabled = false;
            m_deletetime.Interval = 100;
            m_deletetime.Tick += new EventHandler(DeleteTimerFire);
        }

        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void TimerFire(object sender, EventArgs e)
        {
            m_time.Enabled = false;
            UpdatePropForSimulation();
            m_time.Enabled = true;
        }

        /// <summary>
        /// Execute redraw process on simulation running at every 1sec.
        /// </summary>
        /// <param name="sender">object(Timer)</param>
        /// <param name="e">EventArgs</param>
        void DeleteTimerFire(object sender, EventArgs e)
        {
            m_deletetime.Enabled = false;
            if (m_editRow >= 0)
            {
                m_dgv.Rows.RemoveAt(m_editRow);
                m_editRow = -1;
                m_deletetime.Stop();
            }
            else
            {
                m_deletetime.Stop();
            }
        }

        private void ResetProperty()
        {
            if (m_current == null) return;
            foreach (EcellData d in m_current.M_value)
            {
                if (!d.M_value.IsDouble() &&
                    !d.M_value.IsInt())
                    continue;
                for (int i = 0; i < m_dgv.Rows.Count; i++)
                {
                    if (m_dgv.Rows[i].IsNewRow) continue;
                    if (m_dgv[0, i].Value == null) continue;
                    if (d.M_name.Equals(m_dgv[0, i].Value.ToString()))
                    {
                        m_dgv[1, i].Value = d.M_value.ToString();
                    }
                }
            }
        }

        void UpdatePropForSimulation()
        {
            double l_time = m_dManager.GetCurrentSimulationTime();
            if (l_time == 0.0) return;
            if (m_current == null || m_current.M_value == null) return;
            foreach (EcellData d in m_current.M_value)
            {
                if (d.M_isGettable && (d.M_value.IsDouble()))
                {
                    EcellValue e = m_dManager.GetEntityProperty(d.M_entityPath);
                    foreach (DataGridViewRow r in m_dgv.Rows)
                    {
                        if (r.Cells[0].Value.Equals(d.M_name))
                        {
                            r.Cells[1].Value = e.ToString();
                            break;
                        }
                    }
                }
            }
        }

        void DgvUserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (m_propDic == null) return;

            string name = m_dgv.Rows[e.Row.Index].Cells[0].Value.ToString();

            if (m_propDic.ContainsKey(name))
            {
                e.Cancel = true;
            }
            else {
                EcellObject p = m_current.Copy();
                foreach (EcellData d in p.M_value)
                {
                    if (d.M_name.Equals(name))
                    {
                        p.M_value.Remove(d);
                        break;
                    }
                }
                m_isChanging = true;
                m_dManager.DataChanged(
                    m_current.modelID,
                    m_current.key,
                    m_current.type,
                    p);
                m_isChanging = false;
                return;
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
            List<EcellObject> list;
            if (type.Equals("System"))
            {
                list = m_dManager.GetData(modelID, key);
                if (list == null || list.Count != 1) return null;
                return list[0];
            }

            string[] keys = key.Split(new char[] { ':' });
            list = m_dManager.GetData(modelID, keys[0]);
            if (list == null || list.Count == 0) return null;
            for (int i = 0; i < list.Count; i++)
            {
                List<EcellObject> insList = list[i].M_instances;
                if (insList == null || insList.Count == 0) continue;
                for (int j = 0; j < insList.Count; j++)
                {
                    if (insList[j].key == key && insList[j].type == type)
                    {
                        return insList[j];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Add the property to PropertyWindow.
        /// </summary>
        /// <param name="d">EcellData of property.</param>
        /// <param name="type">Type of property.</param>
        /// <returns>Row in DataGridView.</returns>
        DataGridViewRow PropertyAdd(EcellData d, string type)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            DataGridViewCell c2;
            c1.Value = d.M_name;
            r.Cells.Add(c1);

            if (d.M_value == null) return null;
            if (d.M_name.Equals("ClassName"))
            {
                c2 = new DataGridViewComboBoxCell();
                if (type.Equals("System"))
                {
                    ((DataGridViewComboBoxCell)c2).Items.Add("System");
                    c2.Value = "System";
                    m_dgv.AllowUserToAddRows = false;
                    m_dgv.AllowUserToDeleteRows = false;
                }
                else if (type.Equals("Variable"))
                {
                    ((DataGridViewComboBoxCell)c2).Items.Add("Variable");
                    c2.Value = "Variable";
                    m_dgv.AllowUserToAddRows = false;
                    m_dgv.AllowUserToDeleteRows = false;
                }
                else
                {
                    List<string> procList = m_dManager.GetProcessList();
                    foreach (string pName in procList)
                    {
                        ((DataGridViewComboBoxCell)c2).Items.Add(pName);
                    }
                    c2.Value = d.M_value.ToString();
                    if (DataManager.IsEnableAddProperty(d.M_value.ToString()))
                    {
                        m_dgv.AllowUserToAddRows = true;
                        m_dgv.AllowUserToDeleteRows = true;
                        m_propDic = DataManager.GetProcessProperty(d.M_value.ToString());
                    }
                    else
                    {
                        m_dgv.AllowUserToAddRows = false;
                        m_dgv.AllowUserToDeleteRows = false;
                    }

                }
            }
            else if (d.M_name.Equals("Expression"))
            {
                c2 = new DataGridViewButtonCell();
                c2.Value = "...";
            }
            else if (d.M_name.Equals("VariableReferenceList"))
            {
                c2 = new DataGridViewButtonCell();
                c2.Value = "Edit Variable Reference ...";
            }
            else
            {
                c2 = new DataGridViewTextBoxCell();
                c2.Value = d.M_value.ToString();
            }
            r.Cells.Add(c2);
            m_dgv.Rows.Add(r);

            c1.ReadOnly = true;
            if (d.M_isSettable)
            {
                c2.ReadOnly = false;
            }
            else
            {
                c2.ReadOnly = true;
                c1.Style.BackColor = Color.Silver;
                c2.Style.BackColor = Color.Silver;
            }
            c2.Tag = d;

            return r;
        }

        #region PluginBase
        /// <summary>
        /// Get the list of menu item for PropertyWindow.
        /// </summary>
        /// <returns>null.</returns>
        public List<ToolStripMenuItem> GetMenuStripItems()
        {
            return null;
        }

        /// <summary>
        /// Get the list of tool bar item for PropertyWindow.
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
            if (modelID == null || key == null) 
            {
                return;
            }
            Clear();
            EcellObject obj = GetData(modelID, key, type);
            if (obj == null) return;
            EcellData dModelID = new EcellData();
            dModelID.M_name = "ModelID";
            dModelID.M_value = new EcellValue(modelID);
            dModelID.M_isSettable = false;
            PropertyAdd(dModelID, type);

            EcellData dKey = new EcellData();
            dKey.M_name = "ID";
            dKey.M_value = new EcellValue(key);
            dKey.M_isSettable = true;
            PropertyAdd(dKey, type);

            EcellData dClass = new EcellData();
            dClass.M_name = "ClassName";
            dClass.M_value = new EcellValue(obj.classname);
            dClass.M_isSettable = true;
            PropertyAdd(dClass, type);
            
            foreach (EcellData d in obj.M_value)
            {
                if (d.M_name.Equals("Size")) continue;
                PropertyAdd(d, type);
                if (d.M_name.Equals("VariableReferenceList"))
                    m_refStr = d.M_value.ToString();
                if (d.M_name.Equals("Expression"))
                    m_expression = d.M_value.ToString();
            }
            if (type.Equals("System"))
            {
                EcellData dSize = new EcellData();
                dSize.M_name = "Size";
                dSize.M_isSettable = true;
                dSize.M_value = new EcellValue("");
                if (obj.M_instances != null)
                {
                    foreach (EcellObject o in obj.M_instances)
                    {
                        if (o.key.EndsWith(":SIZE"))
                        {
                            foreach (EcellData d in o.M_value)
                            {
                                if (d.M_entityPath.EndsWith(":Value"))
                                {
                                    dSize.M_value = new EcellValue(d.M_value.CastToDouble());
                                }
                            }
                        }
                    }
                }
                PropertyAdd(dSize, type);
            }
            m_current = obj;
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// The event process when user remove object from the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object removed from seleted objects.</param>
        /// <param name="key">ID of object removed from selected objects.</param>
        /// <param name="type">Type of object removed from selected objects.</param>
        public void RemoveSelect(string modelID, string key, string type)
        {
            // not implement
        }

        /// <summary>
        /// Reset all selected objects.
        /// </summary>
        public void ResetSelect()
        {
            // not implement
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
            if (m_current == null) return;
            if (m_isChanging == true) return;
            if (!modelID.Equals(m_current.modelID) ||
                !key.Equals(m_current.key) ||
                !type.Equals(m_current.type)) return;

            SelectChanged(data.modelID, data.key, data.type);
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
            if (m_current == null) return;
            if (modelID.Equals(m_current.modelID) &&
                key.Equals(m_current.key) &&
                type.Equals(m_current.type))
            {
                Clear();
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
            m_current = null;
            m_dgv.Rows.Clear();
            m_dgv.AllowUserToAddRows = false;
            m_dgv.AllowUserToDeleteRows = false;
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
            //if (time == 0.0) return;
            //if (m_current == null || m_current.M_value == null) return;
            //foreach (EcellData d in m_current.M_value)
            //{
            //    if (d.M_isGettable && (d.M_value.IsDouble()))
            //    {
            //        EcellValue e = m_dManager.GetEntityProperty(d.M_entityPath);
            //        foreach (DataGridViewRow r in m_dgv.Rows)
            //        {
            //            if (r.Cells[0].Value.Equals(d.M_name))
            //            {
            //                r.Cells[1].Value = e.ToString();
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(int type)
        {
            if (type == Util.RUNNING)
            {
                m_time.Enabled = true;
                m_time.Start();
            }
            else if (type == Util.SUSPEND)
            {
                m_time.Enabled = false;
                m_time.Stop();
                UpdatePropForSimulation();
            }
            else if ((m_type == Util.RUNNING || m_type == Util.SUSPEND || m_type == Util.STEP) &&
                type == Util.LOADED)
            {
                m_time.Enabled = false;
                m_time.Stop();
                ResetProperty();
            }
            else if (type == Util.STEP)
            {
                UpdatePropForSimulation();
            }
            m_type = type;
        }

        /// <summary>
        /// Change availability of undo/redo status.
        /// </summary>
        /// <param name="status"></param>
        public void ChangeUndoStatus(UndoStatus status)
        {
            // Nothing should be done.
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
                String errmes = m_resources.GetString("ErrCreBitmap");
                MessageBox.Show(errmes + "\n\n" + ex.Message,
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

        /// <summary>
        /// Set the position of EcellObject.
        /// Actually, nothing will be done by this plugin.
        /// </summary>
        /// <param name="data">EcellObject, whose position will be set</param>
        public void SetPosition(EcellObject data)
        {
        }
        #endregion


        #region Events
        /// <summary>
        /// Set the controller when the edited cell is DataGridViewComboBoxCell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DgvEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                DataGridView dgv = (DataGridView)sender;
                if (dgv.CurrentCell is DataGridViewComboBoxCell)
                {
                    this.m_ComboControl =
                        (DataGridViewComboBoxEditingControl)e.Control;
                    this.m_ComboControl.SelectedIndexChanged +=
                        new EventHandler(DgvSelectedIndexChanged);
                }
            }
        }

        /// <summary>
        /// Event when the button in VariableReferenceList is clicked.
        /// Set the list of Variable Reference.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ApplyVarRefButton(object sender, EventArgs e)
        {
            String p = m_win.GetVarReference();
            if (p == null) return;
            if (m_refStr.Equals(p)) return;

            EcellObject t = m_current.Copy();
            foreach (EcellData d in t.M_value)
            {
                if (d.M_name.Equals("VariableReferenceList"))
                {
                    d.M_value = new EcellValue(p);
                    break;
                }
            }
            m_win.Close();
            try
            {
                m_isChanging = true;
                m_dManager.DataChanged(m_current.modelID,
                    m_current.key,
                    m_current.type,
                    t
                    );
                m_isChanging = false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                String errmes = m_resources.GetString("ErrChanged");
                MessageBox.Show(errmes + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_isChanging = false;
                return;
            }
        }

        /// <summary>
        /// Event of clicking the formulator button.
        /// Show the window to edit the formulator.
        /// </summary>
        public void ShowFormulatorWindow()
        {
            m_fwin = new FormulatorWindow();
            m_cnt = new FormulatorControl();
            m_fwin.tableLayoutPanel.Controls.Add(m_cnt, 0, 0);
            m_cnt.Dock = DockStyle.Fill;

            List<string> list = new List<string>();
            list.Add("self.getSuperSystem().SizeN_A");
            foreach (EcellData d in m_current.M_value)
            {
                String str = d.M_name;
                if (str != "modelID" && str != "key" && str != "type" &&
                    str != "classname" && str != "Activity" &&
                    str != "Expression" && str != "Name" &&
                    str != "Priority" && str != "StepperID" &&
                    str != "VariableReferenceList" && str != "IsContinuous")
                    list.Add(str);
            }
            List<EcellReference> tmpList = EcellReference.ConvertString(m_refStr);
            foreach (EcellReference r in tmpList)
            {
                list.Add(r.name + ".MolarConc");
            }
            foreach (EcellReference r in tmpList)
            {
                list.Add(r.name + ".Value");
            }
            m_cnt.AddReserveString(list);


            m_cnt.ImportFormulate(m_expression);
            m_fwin.FApplyButton.Click += new EventHandler(UpdateFormulator);
            m_fwin.FCloseButton.Click += new EventHandler(m_fwin.CancelButtonClick);

            m_fwin.ShowDialog();
        }

        /// <summary>
        /// Event of clicking the OK button in formulator window.
        /// </summary>
        /// <param name="sender">object(Button)</param>
        /// <param name="e">EventArgs</param>
        public void UpdateFormulator(object sender, EventArgs e)
        {
            string tmp = m_cnt.ExportFormulate();
            EcellObject p = m_current.Copy();
            foreach (EcellData d in p.M_value)
            {
                if (d.M_name.Equals("Expression"))
                {
                    d.M_value = new EcellValue(tmp);
                }
            }
            try
            {
                m_isChanging = true;
                m_dManager.DataChanged(
                        m_current.modelID,
                        m_current.key,
                        m_current.type,
                        p);
                m_expression = tmp;
                m_isChanging = false;
            }
            catch (Exception ex)
            {
                ex.ToString();
                String errmes = m_resources.GetString("ErrChanged");
                MessageBox.Show(errmes + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_isChanging = false;
            }

            m_fwin.Close();
            m_fwin.Dispose();
        }

        /// <summary>
        /// Event when user click the cell in DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rIndex = e.RowIndex;
            int cIndex = e.ColumnIndex;
            if (cIndex < 0) return;
            if (rIndex < 0) return;

            DataGridViewCell c = m_dgv.Rows[rIndex].Cells[cIndex] as DataGridViewCell;
            if (c == null) return;
            if (c is DataGridViewTextBoxCell) return;

            if (c.Value.Equals("..."))
            {
                ShowFormulatorWindow();
            }
            else if (c.Value.Equals("Edit Variable Reference ..."))
            {
                m_win = new VariableRefWindow();
                m_win.AddVarButton.Click += new EventHandler(m_win.AddVarReference);
                m_win.DeleteVarButton.Click += new EventHandler(m_win.DeleteVarReference);
                m_win.VRCloseButton.Click += new EventHandler(m_win.CloseVarReference);
                m_win.VRApplyButton.Click += new EventHandler(ApplyVarRefButton);

                List<EcellReference> list = EcellReference.ConvertString(m_refStr);
                foreach (EcellReference v in list)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    bool isAccessor = false;
                    if (v.isAccessor == 1) isAccessor = true;
                    m_win.dgv.Rows.Add(new object[] { v.name, v.fullID, v.coefficient, isAccessor });
                }
                m_win.ShowDialog();
            }
            else
            {
                // nothing
            }
        }

        private int m_editRow = -1;
        /// <summary>
        /// Event when the value of cell is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PropertyChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell editCell = m_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (editCell == null) return;
            EcellData tag = editCell.Tag as EcellData;
            if (tag == null)
            {
                if (e.ColumnIndex == 0)
                {
                    if (editCell.Value == null) return;
                    string name = editCell.Value.ToString();
                    for (int i = 0; i < m_dgv.Rows.Count; i++)
                    {
                        if (e.RowIndex == i) continue;
                        if (m_dgv[0, i].Value == null) continue;
                        if (name.Equals(m_dgv[0, i].Value.ToString()))
                        {
                            String errmes = m_resources.GetString("SameProp");
                            MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            try
                            {
                                m_dgv.Rows.RemoveAt(e.RowIndex);
                            }
                            catch (Exception ex)
                            {
                                m_editRow = e.RowIndex;
                                ex.ToString();
                                m_deletetime.Enabled = true;
                                m_deletetime.Start();
                            }
                            return;
                        }
                    }
                    EcellObject p = m_current.Copy();
                    EcellData data = new EcellData(name, new EcellValue(0.0),
                            "Process:" + m_current.key + ":" + name);
                    data.M_isGettable = true;
                    data.M_isLoadable = true;
                    data.M_isLogable = true;
                    data.M_isLogger = false;
                    data.M_isSavable = true;
                    data.M_isSettable = true;
                    p.M_value.Add(data);
                    m_dgv.Rows[e.RowIndex].Cells[1].Tag = data;
                    try
                    {
                        m_isChanging = true;
                        m_dManager.DataChanged(m_current.modelID,
                            m_current.key,
                            m_current.type,
                            p);
                        m_isChanging = false;
                        m_dgv.Rows[e.RowIndex].Cells[1].Value = 0.0;
                    }
                    catch (Exception ex)
                    {
                        m_isChanging = false;
                        ex.ToString();
                        String errmes = m_resources.GetString("ErrChanged");
                        MessageBox.Show(errmes + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    String errmes = m_resources.GetString("NoProp");
                    MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    try
                    {
                        m_dgv.Rows.RemoveAt(e.RowIndex);
                    }
                    catch (Exception ex)
                    {
                                m_editRow = e.RowIndex;
                                ex.ToString();
                                m_deletetime.Enabled = true;
                                m_deletetime.Start();
                    }
                }
                return;
            }
            if (tag.M_name.Equals("ID"))
            {
                String tmpID = editCell.Value.ToString();
                if (m_current.type.Equals("System"))
                {
                    if (Util.IsNGforSystemFullID(tmpID))
                    {
                        editCell.Value = m_current.key;
                        String errmes = m_resources.GetString("ErrID");
                        MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                }
                else
                {
                    if (Util.IsNGforComponentFullID(tmpID))
                    {
                        editCell.Value = m_current.key;
                        String errmes = m_resources.GetString("ErrID");
                        MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (editCell.Equals(m_current.key)) return;

                EcellObject p = m_current.Copy();
                p.key = tmpID;
                try
                {
                    m_isChanging = true;
                    m_dManager.DataChanged(
                        m_current.modelID,
                        m_current.key,
                        m_current.type,
                        p);
                    m_isChanging = false;
                }
                catch (Exception ex)
                {
                    m_isChanging = false;
                    ex.ToString();
                    String errmes = m_resources.GetString("ErrChanged");
                    MessageBox.Show(errmes + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (tag.M_name.Equals("ClassName"))
            {
                //SelectedIndexChangedイベントハンドラを削除
                if (this.m_ComboControl != null)
                {
                    this.m_ComboControl.SelectedIndexChanged -=
                        new EventHandler(DgvSelectedIndexChanged);
                    this.m_ComboControl = null;
                }
            }
            else if (tag.M_name.Equals("Size"))
            {
                String data = "";
                if (editCell.Value != null) data = editCell.Value.ToString();
                if (data.Equals(""))
                {
                    if (m_current.M_instances != null)
                    {
                        foreach (EcellObject o in m_current.M_instances)
                        {
                            if (o.key.EndsWith(":SIZE"))
                            {
                                m_current.M_instances.Remove(o);
                                m_dManager.DataDelete(o.modelID, o.key, o.type);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    bool isHit = false;
                    if (m_current.M_instances != null)
                    {
                        foreach (EcellObject o in m_current.M_instances)
                        {
                            if (o.key.EndsWith(":SIZE"))
                            {
                                foreach (EcellData d in o.M_value)
                                {
                                    if (d.M_name.EndsWith("Value"))
                                    {
                                        if (data.Equals(d.M_value.ToString())) break;
                                        EcellData p = d.Copy();
                                        p.M_value = new EcellValue(Convert.ToDouble(data));
                                        o.M_value.Remove(d);
                                        o.M_value.Add(p);
                                        m_isChanging = true;
                                        m_dManager.DataChanged(
                                            o.modelID,
                                            o.key,
                                            o.type,
                                            o);
                                        m_isChanging = false;
                                        break;
                                    }
                                }
                                isHit = true;
                                break;
                            }
                        }
                    }
                    if (isHit == false)
                    {
                        Dictionary<string, EcellData> plist = DataManager.GetVariableProperty();
                        List<EcellData> dlist = new List<EcellData>();
                        foreach (string pname in plist.Keys)
                        {
                            if (pname.Equals("Value"))
                            {
                                EcellData d = plist[pname];
                                d.M_value = new EcellValue(Convert.ToDouble(data));
                                dlist.Add(d);
                            }
                            else
                            {
                                dlist.Add(plist[pname]);
                            }
                        }
                        EcellObject obj = EcellObject.CreateObject(m_current.modelID,
                            m_current.key + ":SIZE", "Variable", "Variable", dlist);
                        List<EcellObject> rList = new List<EcellObject>();
                        rList.Add(obj);
                        m_dManager.DataAdd(rList);
                        if (m_current.M_instances == null)
                            m_current.M_instances = new List<EcellObject>();
                        m_current.M_instances.Add(obj);
                    }
                }
            }
            else
            {
                if (editCell.Value == null) return;
                String data = editCell.Value.ToString();
                EcellObject p = m_current.Copy();
                foreach (EcellData d in p.M_value)
                {
                    if (d.M_name.Equals(tag.M_name))
                    {
                        try
                        {
                            if (d.M_value.IsInt())
                                d.M_value = new EcellValue(Convert.ToInt32(data));
                            else if (d.M_value.IsDouble())
                                d.M_value = new EcellValue(Convert.ToDouble(data));
                            else
                                d.M_value = new EcellValue(data);
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            String errmes = m_resources.GetString("ErrFormat");
                            MessageBox.Show(errmes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                try
                {
                    m_isChanging = true;
                    m_dManager.DataChanged(
                        m_current.modelID,
                        m_current.key,
                        m_current.type,
                        p);
                    m_isChanging = false;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    m_isChanging = false;
                    String errmes = m_resources.GetString("ErrChanged");
                    MessageBox.Show(errmes + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        /// <summary>
        /// Event when user change the selected item of DataGridViewComboBoxCell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvSelectedIndexChanged(object sender, EventArgs e)
        {
            //選択されたアイテムを表示
            DataGridViewComboBoxEditingControl cb =
                (DataGridViewComboBoxEditingControl)sender;
            String cname = cb.SelectedItem.ToString();
            if (cname.Equals(m_current.classname)) return;

            List<EcellData> propList = new List<EcellData>();
            Dictionary<String, EcellData> propDict = DataManager.GetProcessProperty(cname);
            foreach (EcellData d in m_current.M_value)
            {
                if (!propDict.ContainsKey(d.M_name))
                {
                    continue;
                }
                propDict[d.M_name].M_value = d.M_value;
            }
            foreach (EcellData d in propDict.Values)
            {
                propList.Add(d);
            }
            EcellObject obj = EcellObject.CreateObject(m_current.modelID,
                m_current.key,
                m_current.type,
                cname,
                propList);
            try
            {
                m_isChanging = true;
                m_dManager.DataChanged(
                    m_current.modelID,
                    m_current.key,
                    m_current.type,
                    obj);
                m_isChanging = false;
                if (DataManager.IsEnableAddProperty(cname))
                {
                    m_dgv.AllowUserToAddRows = true;
                }
                else
                {
                    m_dgv.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                m_isChanging = false;
                ex.ToString();
                String errmes = m_resources.GetString("ErrChanged");
                MessageBox.Show(errmes + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion
    }
}
