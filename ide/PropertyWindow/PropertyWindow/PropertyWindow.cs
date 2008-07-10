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
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;

using Ecell.UI.Components;
using Ecell.Plugin;
using Ecell.Objects;

namespace Ecell.IDE.Plugins.PropertyWindow
{
    /// <summary>
    /// The Plugin Class to show property of object.
    /// </summary>
    public class PropertyWindow : PluginBase
    {
        #region Fields
        /// <summary>
        /// The displayed object.
        /// </summary>
        private EcellObject m_current = null;
        /// <summary>
        /// dgv (DataGridView) is property window grid.
        /// </summary>
        private DataGridView m_dgv = null;
        /// <summary>
        /// Window to set the list of variable.
        /// </summary>
        private VariableReferenceEditDialog m_win;
        /// <summary>
        /// Window to set the expression of process.
        /// </summary>
        private FormulatorDialog m_fwin;
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
        Timer m_time;
        /// <summary>
        /// Timer to delete the property.
        /// </summary>
        Timer m_deletetime;
        /// <summary>
        /// Current status of project.
        /// </summary>
        private ProjectStatus m_type = ProjectStatus.Uninitialized;
        /// <summary>
        /// Expression.
        /// </summary>
        private String m_expression = null;
        /// <summary>
        /// Dictionary of property of displayed object.
        /// </summary>
        private Dictionary<String, EcellData> m_propDic;
        /// <summary>
        /// Flag whether this properties is changing.
        /// </summary>
        private bool m_isChanging = false;
        /// <summary>
        /// Row index edited now.
        /// </summary>
        private int m_editRow = -1;
        /// <summary>
        /// StepperID of current object.
        /// </summary>
        private String m_stepperID = "";
        /// <summary>
        /// StepperID ComboBox of current object.
        /// </summary>
        private DataGridViewComboBoxCell m_stepperIDComboBox = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyWindow()
        {
            m_dgv = new DataGridView();
            m_dgv.Dock = DockStyle.Fill;
            m_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            m_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            m_dgv.AllowUserToAddRows = false;
            m_dgv.AllowUserToDeleteRows = false;
            m_dgv.AllowUserToResizeRows = false;
            m_dgv.RowHeadersVisible = true;
            m_dgv.RowTemplate.Height = 21;

            DataGridViewTextBoxColumn textName = new DataGridViewTextBoxColumn();
            textName.HeaderText = "Name";
            textName.Name = "NameColumn";
            textName.ReadOnly = false;

            DataGridViewTextBoxColumn textValue = new DataGridViewTextBoxColumn();
            textValue.HeaderText = "Value";
            textValue.Name = "ValueColumn";
            textValue.ReadOnly = true;

            m_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    textName, textValue});

            m_dgv.MouseDown += new MouseEventHandler(MouseDownOnDataGrid);
            m_dgv.UserDeletingRow += new DataGridViewRowCancelEventHandler(DeleteRowByUser);
            m_dgv.CellClick += new DataGridViewCellEventHandler(ClickCell);
            m_dgv.CellEndEdit += new DataGridViewCellEventHandler(ChangeProperty);
            m_dgv.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(ShowEditingControl);
            m_dgv.MouseLeave += new EventHandler(LeaveMouse);

            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(FireTimer);

            m_deletetime = new System.Windows.Forms.Timer();
            m_deletetime.Enabled = false;
            m_deletetime.Interval = 100;
            m_deletetime.Tick += new EventHandler(DeleteTimerFire);
        }
        #endregion

        /// <summary>
        /// Change the property of data. 
        /// </summary>
        /// <param name="modelID">the modelID of object changed property.</param>
        /// <param name="key">the key of object changed property.</param>
        /// <param name="obj">the object changed property.</param>
        private void NotifyDataChanged(string modelID, string key, EcellObject obj)
        {
            m_isChanging = true;
            m_dManager.DataChanged(modelID, key, obj.Type, obj);
            m_current = obj;
            m_isChanging = false;
        }

        /// <summary>
        /// Display the property of current object.
        /// </summary>
        private void ResetProperty()
        {
            if (m_current == null) return;
            foreach (EcellData d in m_current.Value)
            {
                if (!d.Value.IsDouble() &&
                    !d.Value.IsInt())
                    continue;
                for (int i = 0; i < m_dgv.Rows.Count; i++)
                {
                    if (m_dgv.Rows[i].IsNewRow) continue;
                    if (m_dgv[0, i].Value == null) continue;
                    if (d.Name.Equals(m_dgv[0, i].Value.ToString()))
                    {
                        m_dgv[1, i].Value = d.Value.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Update the size value.
        /// If system do not have the size object, system create the size object.
        /// </summary>
        /// <param name="sysObj">system object.</param>
        /// <param name="data">size value.</param>
        private void UpdateSize(EcellObject sysObj, string data)
        {
            if (data.Equals(""))
            {
                if (sysObj.Children == null) return;
                foreach (EcellObject o in sysObj.Children)
                {
                    if (o.Key.EndsWith(":SIZE"))
                    {
                        sysObj.Children.Remove(o);
                        m_dManager.DataDelete(o.ModelID, o.Key, o.Type);
                        break;
                    }
                }
            }
            else
            {
                bool isHit = false;
                if (sysObj.Children != null)
                {
                    foreach (EcellObject o in sysObj.Children)
                    {
                        if (!o.Key.EndsWith(":SIZE")) continue;
                        foreach (EcellData d in o.Value)
                        {
                            if (!d.Name.EndsWith(Constants.xpathValue)) continue;
                            if (data.Equals(d.Value.ToString())) break;

                            EcellData p = d.Copy();
                            p.Value = new EcellValue(Convert.ToDouble(data));
                            o.Value.Remove(d);
                            o.Value.Add(p);
                            m_dManager.DataChanged(
                                          o.ModelID,
                                          o.Key,
                                          o.Type,
                                          o);
                            break;
                        }
                        isHit = true;
                        break;
                    }
                }
                if (isHit == false)
                {
                    Dictionary<string, EcellData> plist = m_dManager.GetVariableProperty();
                    List<EcellData> dlist = new List<EcellData>();
                    foreach (string pname in plist.Keys)
                    {
                        if (pname.Equals(Constants.xpathValue))
                        {
                            EcellData d = plist[pname];
                            d.Value = new EcellValue(Convert.ToDouble(data));
                            dlist.Add(d);
                        }
                        else
                        {
                            dlist.Add(plist[pname]);
                        }
                    }
                    EcellObject obj = EcellObject.CreateObject(sysObj.ModelID,
                        sysObj.Key + ":SIZE", Constants.xpathVariable,
                        Constants.xpathVariable, dlist);
                    List<EcellObject> rList = new List<EcellObject>();
                    rList.Add(obj);
                    m_dManager.DataAdd(rList);
                    if (sysObj.Children == null)
                        sysObj.Children = new List<EcellObject>();
                    sysObj.Children.Add(obj);
                }
            }
        }

        /// <summary>
        /// Event of clicking the formulator button.
        /// Show the window to edit the formulator.
        /// </summary>
        public void ShowFormulatorWindow()
        {
            m_fwin = new FormulatorDialog();
            m_cnt = new FormulatorControl();
            m_fwin.tableLayoutPanel.Controls.Add(m_cnt, 0, 0);
            m_cnt.Dock = DockStyle.Fill;

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
            List<EcellReference> tmpList = EcellReference.ConvertString(m_refStr);
            foreach (EcellReference r in tmpList)
            {
                list.Add(r.Name + ".MolarConc");
            }
            foreach (EcellReference r in tmpList)
            {
                list.Add(r.Name + ".Value");
            }
            m_cnt.AddReserveString(list);


            m_cnt.ImportFormulate(m_expression);
            m_fwin.FApplyButton.Click += new EventHandler(UpdateFormulator);
            m_fwin.FCloseButton.Click += new EventHandler(m_fwin.CancelButtonClick);

            m_fwin.ShowDialog();
        }

        /// <summary>
        /// Update the property value with simulation.
        /// </summary>
        void UpdatePropForSimulation()
        {
            double l_time = m_dManager.GetCurrentSimulationTime();
            if (l_time == 0.0) return;
            if (m_current == null || m_current.Value == null) return;
            foreach (EcellData d in m_current.Value)
            {
                if (d.Gettable && (d.Value.IsDouble()))
                {
                    EcellValue e = m_dManager.GetEntityProperty(d.EntityPath);
                    foreach (DataGridViewRow r in m_dgv.Rows)
                    {
                        if (r.Cells[0].Value.Equals(d.Name))
                        {
                            r.Cells[1].Value = e.ToString();
                            break;
                        }
                    }
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
            return m_dManager.GetEcellObject(modelID, key, type);
        }

        /// <summary>
        /// Add the property to PropertyWindow.
        /// </summary>
        /// <param name="d">EcellData of property.</param>
        /// <param name="type">Type of property.</param>
        /// <returns>Row in DataGridView.</returns>
        DataGridViewRow AddProperty(EcellData d, string type)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            DataGridViewCell c2;
            c1.Value = d.Name;
            r.Cells.Add(c1);

            if (d.Value == null) return null;
            if (d.Name.Equals(Constants.xpathClassName))
            {
                c2 = new DataGridViewComboBoxCell();
                if (type == Constants.xpathSystem ||
                    type == Constants.xpathVariable ||
                    type == Constants.xpathText)
                {
                    ((DataGridViewComboBoxCell)c2).Items.Add(type);
                    c2.Value = type;
                    m_dgv.AllowUserToAddRows = false;
                    m_dgv.AllowUserToDeleteRows = false;
                }
                else
                {
                    List<string> procList = m_dManager.GetProcessList();
                    bool isHit = false;
                    foreach (string pName in procList)
                    {
                        ((DataGridViewComboBoxCell)c2).Items.Add(pName);
                        if (pName == d.Value.ToString()) isHit = true;
                    }

                    if (!isHit)
                    {
                        ((DataGridViewComboBoxCell)c2).Items.Add(d.Value.ToString());
                    }
                    c2.Value = d.Value.ToString();
                    if (m_dManager.IsEnableAddProperty(d.Value.ToString()))
                    {
                        m_dgv.AllowUserToAddRows = true;
                        m_dgv.AllowUserToDeleteRows = true;
                        m_propDic = m_dManager.GetProcessProperty(d.Value.ToString());
                    }
                    else
                    {
                        m_dgv.AllowUserToAddRows = false;
                        m_dgv.AllowUserToDeleteRows = false;
                    }
                }
            }
            else if (d.Name.Equals(Constants.xpathExpression))
            {
                c2 = new DataGridViewButtonCell();
                c2.Value = "...";
            }
            else if (d.Name.Equals(Constants.xpathVRL))
            {
                c2 = new DataGridViewButtonCell();
                c2.Value = "Edit Variable Reference ...";
            }
            else if (d.Name.Equals(Constants.xpathStepperID))
            {
                c2 = new DataGridViewComboBoxCell();
                List<EcellObject> slist;
                slist = m_dManager.GetStepper(null, m_current.ModelID);
                foreach (EcellObject obj in slist)
                {
                    ((DataGridViewComboBoxCell)c2).Items.AddRange(new object[] { obj.Key });
                }
                m_stepperID = d.Value.ToString();
                m_stepperID = m_stepperID.Replace("(", "");
                m_stepperID = m_stepperID.Replace(")", "");
                m_stepperID = m_stepperID.Replace("\"", "");

                c2.Value = m_stepperID;
                m_stepperIDComboBox = (DataGridViewComboBoxCell)(c2.Clone());
            }
            else
            {
                c2 = new DataGridViewTextBoxCell();
                c2.Value = d.Value.ToString();
            }
            r.Cells.Add(c2);
            m_dgv.Rows.Add(r);

            c1.ReadOnly = true;
            if (d.Settable)
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
        /// Get the window form for PropertyWindow.
        /// </summary>
        /// <returns>UserControl</returns>
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            EcellDockContent dock = new EcellDockContent();
            dock.Name = "PropertyWindow";
            dock.Text = MessageResources.PropertyWindow;
            dock.Icon = MessageResources.proper;         
            dock.TabText = dock.Text;
            dock.Controls.Add(m_dgv);
            dock.IsSavable = true;
            return new EcellDockContent[] { dock };
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            ChangeObject(modelID, key, type);
        }

        /// <summary>
        /// Change the displayed object.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        private void ChangeObject(string modelID, string key, string type)
        {
            // When called with illegal arguments, this method will do nothing;
            if (String.IsNullOrEmpty(modelID) || String.IsNullOrEmpty(key))
            {
                return;
            }
            Clear();
            EcellObject obj = GetData(modelID, key, type);
            if (obj == null) return;
            EcellData dModelID = new EcellData();
            dModelID.Name = "ModelID";
            dModelID.Value = new EcellValue(modelID);
            dModelID.Settable = false;
            AddProperty(dModelID, type);

            EcellData dKey = new EcellData();
            dKey.Name = "ID";
            dKey.Value = new EcellValue(key);
            dKey.Settable = true;
            AddProperty(dKey, type);

            EcellData dClass = new EcellData();
            dClass.Name = "ClassName";
            dClass.Value = new EcellValue(obj.Classname);
            dClass.Settable = true;
            AddProperty(dClass, type);
            m_current = obj;

            foreach (EcellData d in obj.Value)
            {
                if (d.Name.Equals(Constants.xpathSize))
                    continue;

                AddProperty(d, type);
                if (d.Name.Equals(EcellProcess.VARIABLEREFERENCELIST))
                    m_refStr = d.Value.ToString();
                if (d.Name.Equals(EcellProcess.EXPRESSION))
                    m_expression = d.Value.ToString();
            }
            if (type == Constants.xpathSystem)
            {
                EcellData dSize = new EcellData();
                dSize.Name = Constants.xpathSize;
                dSize.Settable = true;
                dSize.Value = new EcellValue("");
                if (obj.Children != null)
                {
                    foreach (EcellObject o in obj.Children)
                    {
                        if (o.Key.EndsWith(":SIZE"))
                        {
                            foreach (EcellData d in o.Value)
                            {
                                if (d.EntityPath.EndsWith(":Value"))
                                {
                                    dSize.Value = new EcellValue(d.Value.CastToDouble());
                                }
                            }
                        }
                    }
                }
                AddProperty(dSize, type);
            }

            if (m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
            {
                UpdatePropForSimulation();
            }
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public override void AddSelect(string modelID, string key, string type)
        {
            ChangeObject(modelID, key, type);
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            if (data == null) return;
            foreach (EcellObject obj in data)
            {
                if (obj.Type.Equals(Constants.xpathStepper))
                {
                    if (m_stepperIDComboBox == null) return;
                    m_stepperIDComboBox.Items.AddRange(new object[] { obj.Key });
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
        public override void DataChanged(string modelID, string key, string type, EcellObject data)
        {
            if (m_current == null) return;
            if (m_isChanging == true) return;
            ChangeObject(data.ModelID, data.Key, data.Type);
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            if (m_current == null) return;
            if (type.Equals(Constants.xpathStepper))
            {
                if (m_stepperIDComboBox == null) return;
                if (m_stepperIDComboBox.Items.Contains(key))
                {
                    m_stepperIDComboBox.Items.Remove(key);
                }
            }
            if (modelID.Equals(m_current.ModelID) &&
                key.Equals(m_current.Key) &&
                type.Equals(m_current.Type))
            {
                Clear();
            }
        }

        /// <summary>
        /// The event sequence on closing project.
        /// </summary>
        public override void Clear()
        {
            m_current = null;
            m_dgv.Rows.Clear();
            m_dgv.AllowUserToAddRows = false;
            m_dgv.AllowUserToDeleteRows = false;
            m_stepperIDComboBox = null;
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public override void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Running)
            {
                m_time.Enabled = true;
                m_time.Start();
            }
            else if (type == ProjectStatus.Suspended)
            {
                m_time.Enabled = false;
                m_time.Stop();
                UpdatePropForSimulation();
            }
            else if ((m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping) &&
                type == ProjectStatus.Loaded)
            {
                m_time.Enabled = false;
                m_time.Stop();
                ResetProperty();
            }
            else if (type == ProjectStatus.Stepping)
            {
                UpdatePropForSimulation();
            }
            m_type = type;
        }

        /// <summary>
        /// Get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>The bitmap data of plugin.</returns>
        public override Bitmap Print(string name)
        {
            Bitmap bitmap = new Bitmap(m_dgv.Width, m_dgv.Height);
            m_dgv.DrawToBitmap(bitmap, m_dgv.ClientRectangle);
            return bitmap;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"PropertyWindow"</returns>
        public override string GetPluginName()
        {
            return "PropertyWindow";
        }

        /// <summary>
        /// Get the version of this plugin.
        /// </summary>
        /// <returns>version string.</returns>
        public override String GetVersionString()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Check whether this plugin can print display image.
        /// </summary>
        /// <returns>true</returns>
        public override IEnumerable<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            names.Add(MessageResources.PropertyWindow);
            return names;
        }
        #endregion


        #region Events
        /// <summary>
        /// Event when user delete the row.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewRowCancelEventArgs.</param>
        private void DeleteRowByUser(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (m_propDic == null) return;
            string name = m_dgv.Rows[e.Row.Index].Cells[0].Value.ToString();

            if (m_propDic.ContainsKey(name)) // be disable to delete.
            {
                e.Cancel = true;
            }
            else
            {
                EcellObject p = m_current.Copy();
                foreach (EcellData d in p.Value)
                {
                    if (d.Name.Equals(name))
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
                    ex.ToString();
                    m_isChanging = false;
                }
            }
        }

        /// <summary>
        /// Event when the mouse is down in row.
        /// This data of row is used for Drag and Drop.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">MouseEventArgs.</param>
        private void MouseDownOnDataGrid(object sender, MouseEventArgs e)
        {
            DataGridView v = sender as DataGridView;
            if (e.Button != MouseButtons.Left)
                return;

            DataGridView.HitTestInfo hti = v.HitTest(e.X, e.Y);
            if (hti.ColumnIndex > 0) return;
            if (hti.RowIndex <= 0) return;
            string s = v[0, hti.RowIndex].Value as string;
            if (s == null) return;
            foreach (EcellData d in m_current.Value)
            {
                if (!d.Name.Equals(s))
                    continue;
                if (!d.Logable && !d.Settable)
                    break;
                if (!d.Value.IsDouble())
                    break;
                EcellDragObject dobj = new EcellDragObject(m_current.ModelID,
                    m_current.Key,
                    m_current.Type,
                    d.EntityPath,
                    d.Settable,
                    d.Logable);

                v.DoDragDrop(dobj, DragDropEffects.Move | DragDropEffects.Copy);
                return;
            }
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
            m_deletetime.Stop();
        }

        /// <summary>
        /// Set the controller when the edited cell is DataGridViewComboBoxCell.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewEditingControlShowingEventArgs</param>
        void ShowEditingControl(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!(e.Control is DataGridViewComboBoxEditingControl))
                return;
            if (!(m_dgv.CurrentCell is DataGridViewComboBoxCell))
                return;

            this.m_ComboControl = (DataGridViewComboBoxEditingControl)e.Control;
            EcellData d = m_dgv.CurrentCell.Tag as EcellData;
            if (d.Name.Equals(Constants.xpathClassName))
            {
                this.m_ComboControl.SelectedIndexChanged += new EventHandler(ChangeSelectedIndexOfProcessClass);
            }
            else
            {
                this.m_ComboControl.SelectedIndexChanged += new EventHandler(ChangeSelectedIndexOfStepperID);
            }
        }

        /// <summary>
        /// Event when the button in VariableReferenceList is clicked.
        /// Set the list of Variable Reference.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        void ApplyVarRefButton(object sender, EventArgs e)
        {
            String refStr = m_win.GetVarReference();
            if (refStr == null || m_refStr.Equals(refStr))
                return;

            EcellObject obj = m_current.Copy();
            obj.GetEcellData(EcellProcess.VARIABLEREFERENCELIST).Value =
                EcellValue.ToVariableReferenceList(refStr);

            m_win.Close();
            try
            {
                NotifyDataChanged(m_current.ModelID, m_current.Key, obj);
                m_refStr = refStr;
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                m_isChanging = false;
                return;
            }
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
            foreach (EcellData d in p.Value)
            {
                if (d.Name.Equals(Constants.xpathExpression))
                {
                    d.Value = new EcellValue(tmp);
                }
            }
            try
            {
                NotifyDataChanged(m_current.ModelID, m_current.Key, p);
                m_expression = tmp;
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
                m_isChanging = false;
            }

            m_fwin.Close();
            m_fwin.Dispose();
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
        void ClickCell(object sender, DataGridViewCellEventArgs e)
        {
            int rIndex = e.RowIndex;
            int cIndex = e.ColumnIndex;
            if (cIndex < 0) return;
            if (rIndex < 0) return;

            DataGridViewCell c = m_dgv.Rows[rIndex].Cells[cIndex] as DataGridViewCell;
            if (c == null) return;
            if (c is DataGridViewTextBoxCell) return;
            if (c.Value == null) return;

            if (c.Value.Equals("..."))
            {
                ShowFormulatorWindow();
            }
            else if (c.Value.Equals("Edit Variable Reference ..."))
            {
                m_win = new VariableReferenceEditDialog(m_dManager, m_pManager);
                m_win.AddVarButton.Click += new EventHandler(m_win.AddVarReference);
                m_win.DeleteVarButton.Click += new EventHandler(m_win.DeleteVarReference);
                m_win.VRCloseButton.Click += new EventHandler(m_win.CloseVarReference);
                m_win.VRApplyButton.Click += new EventHandler(ApplyVarRefButton);

                List<EcellReference> list = EcellReference.ConvertString(m_refStr);
                foreach (EcellReference v in list)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    bool isAccessor = false;
                    if (v.IsAccessor == 1)
                        isAccessor = true;
                    m_win.dgv.Rows.Add(new object[] { v.Name, v.FullID, v.Coefficient, isAccessor });
                }
                m_win.ShowDialog();
            }
            else
            {
                // nothing
            }
        }

        /// <summary>
        /// Start the delete timer.
        /// </summary>
        /// <param name="index">the row index to delete.</param>
        private void StartDeleteTimer(int index)
        {
            m_editRow = index;
            m_deletetime.Enabled = true;
            m_deletetime.Start();
        }

        /// <summary>
        /// Event when the value of cell is changed.
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewCellEventArgs.</param>
        private void ChangeProperty(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell editCell = m_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (editCell == null) return;
            EcellData tag = editCell.Tag as EcellData;
            if (tag == null)
            {
                if (e.ColumnIndex == 0)
                {
                    if (editCell.Value == null)
                        return;
                    string name = editCell.Value.ToString();
                    for (int i = 0; i < m_dgv.Rows.Count; i++)
                    {
                        if (e.RowIndex == i)
                            continue;
                        if (m_dgv[0, i].Value == null)
                            continue;
                        if (name.Equals(m_dgv[0, i].Value.ToString()))
                        {
                            Util.ShowErrorDialog(MessageResources.ErrSameProp);
                            try
                            {
                                m_dgv.Rows.RemoveAt(e.RowIndex);
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                                StartDeleteTimer(e.RowIndex);
                            }
                            return;
                        }
                    }
                    EcellObject p = m_current.Copy();
                    EcellData data = new EcellData(name, new EcellValue(0.0),
                            "Process:" + m_current.Key + ":" + name);
                    data.Gettable = true;
                    data.Loadable = true;
                    data.Logable = true;
                    data.Logged = false;
                    data.Saveable = true;
                    data.Settable = true;
                    p.Value.Add(data);
                    m_dgv.Rows[e.RowIndex].Cells[1].Tag = data;
                    try
                    {
                        NotifyDataChanged(m_current.ModelID, m_current.Key, p);
                        m_dgv.Rows[e.RowIndex].Cells[1].Value = 0.0;
                    }
                    catch (Exception ex)
                    {
                        m_isChanging = false;
                        Util.ShowErrorDialog(ex.Message);
                    }
                }
                else
                {
                    Util.ShowErrorDialog(MessageResources.ErrNoProp);

                    try
                    {
                        m_dgv.Rows.RemoveAt(e.RowIndex);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        StartDeleteTimer(e.RowIndex);
                    }
                }
                return;
            }
            if (tag.Name.Equals("ID"))
            {
                String tmpID = editCell.Value.ToString();
                if ((m_current.Type == Constants.xpathSystem && Util.IsNGforSystemFullID(tmpID)) ||
                    (m_current.Type != Constants.xpathSystem && Util.IsNGforComponentFullID(tmpID)))
                {
                    editCell.Value = m_current.Key;
                    Util.ShowErrorDialog(MessageResources.ErrID);

                    return;
                }
                if (editCell.Equals(m_current.Key))
                    return;

                EcellObject p = m_current.Copy();
                p.Key = tmpID;
                try
                {
                    NotifyDataChanged(m_current.ModelID, m_current.Key, p);
                }
                catch (Exception ex)
                {
                    m_isChanging = false;
                    Util.ShowErrorDialog(ex.Message);
                    return;
                }
            }
            else if (tag.Name.Equals(Constants.xpathClassName))
            {
                if (this.m_ComboControl != null)
                {
                    this.m_ComboControl.SelectedIndexChanged -=
                        new EventHandler(ChangeSelectedIndexOfProcessClass);
                    this.m_ComboControl = null;
                }
            }
            else if (tag.Name.Equals(Constants.xpathStepperID))
            {
                if (this.m_ComboControl != null)
                {
                    this.m_ComboControl.SelectedIndexChanged -=
                        new EventHandler(ChangeSelectedIndexOfStepperID);
                    this.m_ComboControl = null;
                }
            }
            else if (tag.Name.Equals(Constants.xpathSize))
            {
                try
                {
                    String data = "";
                    if (editCell.Value != null)
                        data = editCell.Value.ToString();
                    m_isChanging = true;
                    UpdateSize(m_current, data);
                    m_isChanging = false;
                }
                catch (Exception ex)
                {
                    m_isChanging = false;
                    Util.ShowErrorDialog(ex.Message);
                    return;
                }
            }
            else
            {
                if (editCell.Value == null)
                    return;
                String data = editCell.Value.ToString();
                EcellObject p = m_current.Copy();
                foreach (EcellData d in p.Value)
                {
                    if (d.Name.Equals(tag.Name))
                    {
                        try
                        {
                            if (d.Value.IsInt())
                                d.Value = new EcellValue(Convert.ToInt32(data));
                            else if (d.Value.IsDouble())
                                d.Value = new EcellValue(Convert.ToDouble(data));
                            else
                                d.Value = new EcellValue(data);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                            Util.ShowErrorDialog(MessageResources.ErrFormat);
                            return;
                        }
                    }
                }
                try
                {
                    NotifyDataChanged(m_current.ModelID, m_current.Key, p);
                }
                catch (Exception ex)
                {
                    m_isChanging = false;
                    Util.ShowErrorDialog(ex.Message);
                }
            }
        }

        /// <summary>
        /// Event when user change the selected item of DataGridViewComboBoxCell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeSelectedIndexOfProcessClass(object sender, EventArgs e)
        {
            //選択されたアイテムを表示
            DataGridViewComboBoxEditingControl cb =
                (DataGridViewComboBoxEditingControl)sender;
            String tagName = cb.Tag as string;
            String cname = cb.SelectedItem.ToString();
            if (cname.Equals(m_current.Classname)) return;

            List<EcellData> propList = new List<EcellData>();
            Dictionary<String, EcellData> propDict = m_dManager.GetProcessProperty(cname);
            foreach (EcellData d in m_current.Value)
            {
                if (!propDict.ContainsKey(d.Name))
                {
                    continue;
                }
                if (!propDict[d.Name].Settable)
                    continue;
                propDict[d.Name].Value = d.Value;
            }
            foreach (EcellData d in propDict.Values)
            {
                propList.Add(d);
            }
            EcellObject obj = EcellObject.CreateObject(m_current.ModelID,
                m_current.Key,
                m_current.Type,
                cname,
                propList);
            obj.X = m_current.X;
            obj.Y = m_current.Y;
            obj.OffsetX = m_current.OffsetX;
            obj.OffsetY = m_current.OffsetY;
            obj.Height = m_current.Height;
            obj.Width = m_current.Width;
            try
            {
                NotifyDataChanged(m_current.ModelID, m_current.Key, obj);
                if (m_dManager.IsEnableAddProperty(cname))
                {
                    m_dgv.AllowUserToAddRows = true;
                }
                else
                {
                    m_dgv.AllowUserToAddRows = false;
                }
                SelectChanged(m_current.ModelID, m_current.Key, m_current.Type);
            }
            catch (Exception ex)
            {
                m_isChanging = false;
                Util.ShowErrorDialog(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Event when user change the selected item of DataGridViewComboBoxCell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeSelectedIndexOfStepperID(object sender, EventArgs e)
        {
            //選択されたアイテムを表示
            DataGridViewComboBoxEditingControl cb =
                (DataGridViewComboBoxEditingControl)sender;
            String cname = cb.SelectedItem.ToString();
            if (cname.Equals(m_stepperID))
                return;

            foreach (EcellData d in m_current.Value)
            {
                if (d.Name.Equals(Constants.xpathStepperID))
                {
                    d.Value = new EcellValue(cname);
                }
            }
            EcellObject obj = EcellObject.CreateObject(m_current.ModelID,
                m_current.Key,
                m_current.Type,
                m_current.Classname,
                m_current.Value);
            obj.X = m_current.X;
            obj.Y = m_current.Y;
            obj.OffsetX = m_current.OffsetX;
            obj.OffsetY = m_current.OffsetY;
            obj.Height = m_current.Height;
            obj.Width = m_current.Width;
            try
            {
                NotifyDataChanged(m_current.ModelID, m_current.Key, obj);
            }
            catch (Exception ex)
            {
                m_isChanging = false;
                Util.ShowErrorDialog(ex.Message);
                return;
            }
        }
        #endregion
    }
}
