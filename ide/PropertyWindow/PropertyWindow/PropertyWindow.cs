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

namespace Ecell.IDE.Plugins.PropertyWindow
{
    public partial class PropertyWindow : EcellDockContent, IEcellPlugin, IDataHandler, IDockContentProvider
    {
        #region Fields
        /// <summary>
        /// The displayed object.
        /// </summary>
        private EcellObject m_current = null;
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

        /// <summary>
        /// The application environment associated to this object.
        /// </summary>
        protected ApplicationEnvironment m_env = null;
        #endregion

        /// <summary>
        /// The application environment associated to this plugin
        /// </summary>
        public ApplicationEnvironment Environment
        {
            get { return m_env; }
            set { m_env = value; }
        }

        public PropertyWindow()
        {
            InitializeComponent();

            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(FireTimer);

            m_deletetime = new System.Windows.Forms.Timer();
            m_deletetime.Enabled = false;
            m_deletetime.Interval = 100;
            m_deletetime.Tick += new EventHandler(DeleteTimerFire);
        }

        /// <summary>
        /// Change the property of data. 
        /// </summary>
        /// <param name="modelID">the modelID of object changed property.</param>
        /// <param name="key">the key of object changed property.</param>
        /// <param name="obj">the object changed property.</param>
        private void NotifyDataChanged(string modelID, string key, EcellObject obj)
        {
            m_isChanging = true;
            try
            {
                m_env.DataManager.DataChanged(modelID, key, obj.Type, obj);
                m_current = obj;
            }
            finally
            {
                m_isChanging = false;
            }
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
                    if (d.Name == m_dgv[0, i].Value.ToString())
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
            m_isChanging = true;
            try
            {
                if (sysObj.Children != null)
                {
                    foreach (EcellObject o in sysObj.Children)
                    {
                        if (o.Key.EndsWith(":SIZE"))
                        {
                            EcellObject no = o.Copy();
                            no.GetEcellData("Value").Value = new EcellValue(Convert.ToDouble(data));
                            m_env.DataManager.DataChanged(o.ModelID, o.Key, o.Type, no);
                            return;
                        }
                    }
                }

                {
                    Dictionary<string, EcellData> plist = m_env.DataManager.GetVariableProperty();
                    List<EcellData> dlist = new List<EcellData>();
                    foreach (string pname in plist.Keys)
                    {
                        if (pname == Constants.xpathValue)
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
                        sysObj.Key + Constants.delimiterColon + "SIZE", Constants.xpathVariable,
                        Constants.xpathVariable, dlist);
                    List<EcellObject> rList = new List<EcellObject>();
                    rList.Add(obj);
                    m_env.DataManager.DataAdd(rList);
                    if (sysObj.Children == null)
                        sysObj.Children = new List<EcellObject>();
                    sysObj.Children.Add(obj);
                }
            }
            finally
            {
                m_isChanging = false;
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
                List<EcellReference> tmpList = EcellReference.ConvertString(m_refStr);
                foreach (EcellReference r in tmpList)
                {
                    list.Add(r.Name + ".MolarConc");
                }
                foreach (EcellReference r in tmpList)
                {
                    list.Add(r.Name + ".Value");
                }
                fwin.AddReserveString(list);

                fwin.ImportFormulate(formula);
                if (fwin.ShowDialog() != DialogResult.OK)
                    return null;

                return fwin.ExportFormulate();
            }
        }

        /// <summary>
        /// Update the property value with simulation.
        /// </summary>
        void UpdatePropForSimulation()
        {
            double l_time = m_env.DataManager.GetCurrentSimulationTime();
            if (l_time == 0.0) return;
            if (m_current == null || m_current.Value == null) return;
            foreach (EcellData d in m_current.Value)
            {
                EcellValue e = m_env.DataManager.GetEntityProperty(d.EntityPath);
                foreach (DataGridViewRow r in m_dgv.Rows)
                {
                    if ((string)r.Cells[0].Value == d.Name)
                    {
                        if (d.Gettable && (d.Value.IsDouble))
                            r.Cells[1].Value = e.ToString();
                        if (d.Name == "FullID")
                            r.Cells[1].ReadOnly = true;
                        break;
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
            return m_env.DataManager.GetEcellObject(modelID, key, type);
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
            if (d.Name == Constants.xpathClassName)
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
                    List<string> procList = m_env.DataManager.GetProcessList();
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
                    if (m_env.DataManager.IsEnableAddProperty(d.Value.ToString()))
                    {
                        m_dgv.AllowUserToAddRows = true;
                        m_dgv.AllowUserToDeleteRows = true;
                        m_propDic = m_env.DataManager.GetProcessProperty(d.Value.ToString());
                    }
                    else
                    {
                        m_dgv.AllowUserToAddRows = false;
                        m_dgv.AllowUserToDeleteRows = false;
                    }
                }
            }
            else if (d.Name == Constants.xpathExpression)
            {
                c2 = new DataGridViewOutOfPlaceEditableCell();
                c2.Value = d.Value.CastToString();
                ((DataGridViewOutOfPlaceEditableCell)c2).OnOutOfPlaceEditRequested =
                    delegate(DataGridViewOutOfPlaceEditableCell c)
                    {
                        string retval = ShowFormulatorDialog(c.Value == null ? "": c.Value.ToString());
                        if (retval != null)
                        {
                            c.Value = retval;
                            return true;
                        }
                        return false;
                    };
            }
            else if (d.Name == Constants.xpathVRL)
            {
                c2 = new DataGridViewLinkCell();
                c2.Value = MessageResources.LabelEdit;
            }
            else if (d.Name == Constants.xpathStepperID)
            {
                c2 = new DataGridViewComboBoxCell();
                List<EcellObject> slist;
                slist = m_env.DataManager.GetStepper(null, m_current.ModelID);
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
                c2.Style.ForeColor = Color.Silver;
            }
            c2.Tag = d;

            return r;
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
            ChangeObject(modelID, key, type);
        }

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
            string localID = null;
            string parentSystemPath;
            if (type == Constants.xpathSystem)
                Util.SplitSystemPath(key, out parentSystemPath, out localID);
            else
                Util.ParseEntityKey(key, out parentSystemPath, out localID);
            dKey.Value = new EcellValue(localID);
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
                if (d.Name == Constants.xpathSize)
                    continue;

                AddProperty(d, type);
                if (d.Name == EcellProcess.VARIABLEREFERENCELIST)
                    m_refStr = d.Value.ToString();
            }

            if (type == Constants.xpathSystem)
            {
                EcellData dSize = new EcellData();
                dSize.Name = Constants.xpathSize;
                dSize.Settable = true;
                dSize.Value = new EcellValue(0.1);
                if (obj.Children != null)
                {
                    foreach (EcellObject o in obj.Children)
                    {
                        if (o.Key.EndsWith(":SIZE"))
                        {
                            foreach (EcellData d in o.Value)
                            {
                                if (d.Name == "Value")
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

            label1.Text = Util.BuildFullID(type, parentSystemPath, localID);
        }

        /// <summary>
        /// The event process when user add the object to the selected objects.
        /// </summary>
        /// <param name="modelID">ModelID of object added to selected objects.</param>
        /// <param name="key">ID of object added to selected objects.</param>
        /// <param name="type">Type of object added to selected objects.</param>
        public void AddSelect(string modelID, string key, string type)
        {
            ChangeObject(modelID, key, type);
        }

        /// <summary>
        /// The event sequence to add the object at other plugin.
        /// </summary>
        /// <param name="data">The value of the adding object.</param>
        public void DataAdd(List<EcellObject> data)
        {
            if (data == null) return;
            foreach (EcellObject obj in data)
            {
                if (obj.Type == Constants.xpathStepper)
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
        public void DataChanged(string modelID, string key, string type, EcellObject data)
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
        public void DataDelete(string modelID, string key, string type)
        {
            if (m_current == null) return;
            if (type == Constants.xpathStepper)
            {
                if (m_stepperIDComboBox == null) return;
                if (m_stepperIDComboBox.Items.Contains(key))
                {
                    m_stepperIDComboBox.Items.Remove(key);
                }
            }
            if (modelID == m_current.ModelID && key == m_current.Key && type == m_current.Type)
            {
                Clear();
            }
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
            m_stepperIDComboBox = null;
        }

        /// <summary>
        ///  When change system status, change menu enable/disable.
        /// </summary>
        /// <param name="type">System status.</param>
        public void ChangeStatus(ProjectStatus type)
        {
            if (type == ProjectStatus.Running)
            {
                m_dgv.Enabled = false;
                m_time.Enabled = true;
                m_time.Start();
            }
            else if (type == ProjectStatus.Suspended)
            {
                m_dgv.Enabled = true;
                m_time.Enabled = false;
                m_time.Stop();
                UpdatePropForSimulation();
            }
            else if (type == ProjectStatus.Loaded)
            {
                m_dgv.Enabled = true;
                if (m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
                {
                    m_time.Enabled = false;
                    m_time.Stop();
                    ResetProperty();
                }
            }
            else if (type == ProjectStatus.Stepping)
            {
                m_dgv.Enabled = true;
                UpdatePropForSimulation();
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
                    if (d.Name == name)
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
                if (d.Name != s)
                    continue;
                if (!d.Logable && !d.Settable)
                    break;
                if (!d.Value.IsDouble)
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
            if (d.Name == Constants.xpathClassName)
            {
                this.m_ComboControl.SelectedIndexChanged += new EventHandler(ChangeSelectedIndexOfProcessClass);
            }
            else
            {
                this.m_ComboControl.SelectedIndexChanged += new EventHandler(ChangeSelectedIndexOfStepperID);
            }
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
            List<EcellReference> list = EcellReference.ConvertFromVarRefList(((EcellData)c.Tag).Value);
            VariableReferenceEditDialog win = new VariableReferenceEditDialog(m_env.DataManager, m_env.PluginManager, list);
            using (win)
            {
                if (win.ShowDialog() != DialogResult.OK)
                    return;
                EcellObject eo = m_current.Copy();
                EcellData nd = eo.GetEcellData(((EcellData)c.Tag).Name);
                nd.Value = EcellReference.ConvertToVarRefList(EcellReference.ConvertString(win.ReferenceString));
                c.Tag = nd;
                m_current = eo;
                try
                {
                    NotifyDataChanged(eo.ModelID, eo.Key, eo);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    Util.ShowErrorDialog(ex.Message);
                }
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
        private void ChangeProperty(object sender, DataGridViewCellParsingEventArgs e)
        {
            e.ParsingApplied = true;
            if (m_current == null)
                return;
            if (e.ColumnIndex <= 0)
                return;
            if (m_env.PluginManager.Status == ProjectStatus.Running)
                return;
            DataGridViewCell editCell = m_dgv[e.ColumnIndex, e.RowIndex];
            EcellData tag = editCell.Tag as EcellData;
            Debug.Assert(tag != null);
            try
            {
                if (tag.Name == "ID")
                {
                    String tmpID = e.Value.ToString();
                    if (e.Value == m_current.Key)
                        return;

                    EcellObject p = m_current.Copy();
                    if (p.Type == Constants.xpathSystem)
                        p.Key = p.ParentSystemID + Constants.delimiterPath + tmpID;
                    else
                        p.Key = p.ParentSystemID + Constants.delimiterColon + tmpID;
                    NotifyDataChanged(m_current.ModelID, m_current.Key, p);
                }
                else if (tag.Name == Constants.xpathClassName)
                {
                    if (this.m_ComboControl != null)
                    {
                        this.m_ComboControl.SelectedIndexChanged -=
                            new EventHandler(ChangeSelectedIndexOfProcessClass);
                        this.m_ComboControl = null;
                    }
                }
                else if (tag.Name == Constants.xpathStepperID)
                {
                    if (this.m_ComboControl != null)
                    {
                        this.m_ComboControl.SelectedIndexChanged -=
                            new EventHandler(ChangeSelectedIndexOfStepperID);
                        this.m_ComboControl = null;
                    }
                }
                else if (tag.Name == Constants.xpathSize)
                {
                    UpdateSize(m_current, e.Value.ToString());
                }
                else
                {
                    String data = e.Value.ToString();
                    if (m_env.PluginManager.Status == ProjectStatus.Running
                        || m_env.PluginManager.Status == ProjectStatus.Suspended)
                    {
                        m_env.DataManager.SetEntityProperty(tag.EntityPath, data);
                        UpdatePropForSimulation(); // for calculated properties such as MolarConc, etc.
                    }
                    else
                    {
                        EcellObject p = m_current.Copy();
                        foreach (EcellData d in p.Value)
                        {
                            if (d.Name == tag.Name)
                            {
                                try
                                {
                                    if (d.Value.IsInt)
                                        d.Value = new EcellValue(Convert.ToInt32(data));
                                    else if (d.Value.IsDouble)
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
                        NotifyDataChanged(m_current.ModelID, m_current.Key, p);
                    }
                }
            }
            catch (Exception ex)
            {
                e.Value = editCell.Value;
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
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
            if (cname == m_current.Classname)
                return;

            List<EcellData> propList = new List<EcellData>();
            Dictionary<String, EcellData> propDict = m_env.DataManager.GetProcessProperty(cname);
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
                if (m_env.DataManager.IsEnableAddProperty(cname))
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
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
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
            if (cname == m_stepperID)
                return;

            foreach (EcellData d in m_current.Value)
            {
                if (d.Name == Constants.xpathStepperID)
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
                Trace.WriteLine(ex);
                Util.ShowErrorDialog(ex.Message);
            }
        }
        #endregion

        #region IDataHandler メンバ

        public void AdvancedTime(double time)
        {
        }

        public void ChangeUndoStatus(UndoStatus status)
        {
        }

        public void LoggerAdd(string modelID, string type, string key, string path)
        {
        }

        public void ParameterAdd(string projectID, string parameterID)
        {
        }

        public void ParameterDelete(string projectID, string parameterID)
        {
        }

        public void ParameterSet(string projectID, string parameterID)
        {
        }

        public void RemoveObservedData(EcellObservedData data)
        {
        }

        public void RemoveParameterData(EcellParameterData data)
        {
        }

        public void RemoveSelect(string modelID, string key, string type)
        {
        }

        public void ResetSelect()
        {
        }

        public void SaveModel(string modelID, string directory)
        {
        }

        public void SetObservedData(EcellObservedData data)
        {
        }

        public void SetParameterData(EcellParameterData data)
        {
        }

        public void SetPosition(EcellObject data)
        {
        }
        #endregion

        public Dictionary<string, Delegate> GetPublicDelegate()
        {
            return null;
        }

        public void Initialize()
        {
        }

        public IEnumerable<EcellDockContent> GetWindowsForms()
        {
            return new EcellDockContent[] { this };
        }
    }
}
