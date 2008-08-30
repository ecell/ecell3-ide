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
        /// Variable Reference List.
        /// </summary>
        public String m_refStr = null;
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
        /// Row index edited now.
        /// </summary>
        private int m_editRow = -1;
        /// <summary>
        /// StepperID of current object.
        /// </summary>
        private String m_stepperID = "";

        private List<string> m_nonDataProps;

        private List<string> m_procList = null;

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
            if (m_current is EcellText)
                return;
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
            if (m_propDic == null || m_current.Type != Constants.xpathProcess
                || m_propDic.ContainsKey(d.Name))
            {
                c1.Style.BackColor = Color.LightGray;
                c1.Style.SelectionBackColor = Color.LightGray;
                c1.ReadOnly = true;
            }
            else
            {
                c1.ReadOnly = false;
            }

            if (d.Value == null) return null;

            if (d.Name == Constants.xpathExpression)
            {
                c2 = new DataGridViewOutOfPlaceEditableCell();
                c2.Value = d.Value.CastToString();
                ((DataGridViewOutOfPlaceEditableCell)c2).OnOutOfPlaceEditRequested =
                    delegate(DataGridViewOutOfPlaceEditableCell c)
                    {
                        string retval = ShowFormulatorDialog(c.Value == null ? "" : c.Value.ToString());
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
                foreach (EcellObject obj in m_env.DataManager.GetStepper(null, m_current.ModelID))
                {
                    ((DataGridViewComboBoxCell)c2).Items.Add(obj.Key);
                }
                c2.Value = d.Value.ToString();
                m_stepperIDComboBox = (DataGridViewComboBoxCell)c2;
            }
            else
            {
                c2 = new DataGridViewTextBoxCell();
                c2.Value = d.Value.ToString();
            }
            r.Cells.Add(c2);
            m_dgv.Rows.Add(r);

            if (d.Settable)
            {
                c2.ReadOnly = false;
            }
            else
            {
                c2.ReadOnly = true;
                c2.Style.ForeColor = SystemColors.GrayText;
            }
            c2.Tag = d;

            return r;
        }

        DataGridViewRow AddNonDataProperty(string propName, string propValue, bool readOnly)
        {
            DataGridViewRow r = new DataGridViewRow();
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            DataGridViewCell c2 = null;
            r.Cells.Add(c1);
            c1.Style.BackColor = Color.LightGray;
            c1.Style.SelectionBackColor = Color.LightGray;
            c1.ReadOnly = true;
            c1.Value = propName;

            if (propName == Constants.xpathClassName)
            {
                if (m_current.Type == Constants.xpathSystem ||
                    m_current.Type == Constants.xpathVariable)
                {
                    c2 = new DataGridViewComboBoxCell();
                    ((DataGridViewComboBoxCell)c2).Items.Add(propValue);
                    c2.Value = propValue;
                }
                else if (m_current.Type == Constants.xpathText)
                {
                    c2 = new DataGridViewTextBoxCell();
                    c2.Value = "Text";
                    readOnly = true; // forcefully marked as readonly
                }
                else
                {
                    c2 = new DataGridViewComboBoxCell();
                    bool isHit = false;
                    foreach (string pName in m_procList)
                    {
                        ((DataGridViewComboBoxCell)c2).Items.Add(pName);
                        if (pName == propValue)
                            isHit = true;
                    }

                    if (!isHit)
                    {
                        ((DataGridViewComboBoxCell)c2).Items.Add(propValue);
                    }
                    c2.Value = propValue;
                }
            }
            else
            {
                c2 = new DataGridViewTextBoxCell();
                c2.Value = propValue;
            }
            r.Cells.Add(c2);
            c2.Tag = propName;
            c2.ReadOnly = readOnly;
            if (readOnly)
                c2.Style.ForeColor = SystemColors.GrayText;

            m_dgv.Rows.Add(r);
            m_nonDataProps.Add(propName);
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

        private void ReloadProperties()
        {
            m_propDic = null;
            m_dgv.Rows.Clear();
            if (m_current == null)
                return;

            string localID = null;
            string parentSystemPath;
            if (m_current.Type == Constants.xpathSystem)
                Util.SplitSystemPath(m_current.Key, out parentSystemPath, out localID);
            else
                Util.ParseEntityKey(m_current.Key, out parentSystemPath, out localID);

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

            foreach (EcellData d in m_current.Value)
            {
                if (d.Name == Constants.xpathSize)
                    continue;

                AddProperty(d, m_current.Type);
                if (d.Name == EcellProcess.VARIABLEREFERENCELIST)
                    m_refStr = d.Value.ToString();
            }

            if (m_current.Type == Constants.xpathSystem)
            {
                EcellData dSize = new EcellData();
                dSize.Name = Constants.xpathSize;
                dSize.Settable = true;
                dSize.Value = new EcellValue(0.1);
                if (m_current.Children != null)
                {
                    foreach (EcellObject o in m_current.Children)
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
                AddProperty(dSize, m_current.Type);
            }

            if (m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
            {
                UpdatePropForSimulation();
            }

            label1.Text = Util.BuildFullID(m_current.Type, parentSystemPath, localID);
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
                return;
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
                m_procList = m_env.DataManager.GetProcessList();
                if (m_type == ProjectStatus.Running || m_type == ProjectStatus.Suspended || m_type == ProjectStatus.Stepping)
                {
                    m_time.Enabled = false;
                    m_time.Stop();
                    ResetProperty();
                }
            }
            else if (type == ProjectStatus.Uninitialized)
            {
                m_dgv.Enabled = false;
                m_procList = null;
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

            if (e.Button == MouseButtons.Left)
            {
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
            else if (e.Button == MouseButtons.Right)
            {
                m_dgv[1, hti.RowIndex].Selected = true;
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

            DataGridViewCell editCell = m_dgv[e.ColumnIndex, e.RowIndex];
            if (e.ColumnIndex == 0)
            {
                // user-defined property
                string propName = e.Value.ToString();
                try
                {
                    if (propName == editCell.Value)
                        return;

                    if (m_current.GetEcellData(propName) != null
                            || m_nonDataProps.Contains(propName))
                        throw new Exception(MessageResources.ErrSameProp);

                    DataGridViewCell valueCell = m_dgv[1, e.RowIndex];

                    string superSystemPath, localID;
                    if (m_current.Type == Constants.xpathSystem)
                        Util.SplitSystemPath(m_current.Key, out superSystemPath, out localID);
                    else
                        Util.ParseEntityKey(m_current.Key, out superSystemPath, out localID);

                    if (valueCell.Tag == null)
                    {
                        if (valueCell.Value == "")
                            valueCell.Value = "0.0";
                    }
                    else
                    {
                        for (int i = m_current.Value.Count; --i >= 0; )
                        {
                            if (m_current.Value[i].Name == (string)editCell.Value)
                            {
                                m_current.Value.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    EcellData data = new EcellData(
                        propName,
                        new EcellValue(Convert.ToDouble(valueCell.Value)),
                        Util.BuildFullPN(
                            m_current.Type,
                            superSystemPath,
                            localID,
                            propName));
                    m_current.AddValue(data);
                    valueCell.Tag = data;
                }
                catch (Exception ex)
                {
                    e.Value = editCell.Value;
                    Trace.WriteLine(ex);
                    Util.ShowErrorDialog(ex.Message);
                }
            }
            else if (e.ColumnIndex == 1)
            {
                try
                {
                    if (editCell.Tag is EcellData)
                    {
                        EcellData tag = editCell.Tag as EcellData;
                        if (tag.Name == Constants.xpathSize)
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
                                            if (d.Value.IsDouble || d.Value.IsInt)
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
                    else if (editCell.Tag is string)
                    {
                        string propName = editCell.Tag as string;
                        if (propName == "ID")
                        {
                            String tmpID = e.Value.ToString();
                            if (tmpID.Equals(m_current.Key))
                                return;
                            if (Util.IsReservedID(tmpID) || Util.IsNGforID(tmpID))
                            {
                                throw new Exception(MessageResources.ErrID);
                            }

                            EcellObject p = m_current.Copy();
                            if (p.Type == Constants.xpathSystem)
                                p.Key = p.ParentSystemID + Constants.delimiterPath + tmpID;
                            else
                                p.Key = p.ParentSystemID + Constants.delimiterColon + tmpID;
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
        }
        #endregion

        #region IDataHandler ÉÅÉìÉo

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

        private void defineANewPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = m_dgv.RowTemplate.Clone() as DataGridViewRow;
            DataGridViewCell nameCell = nameColumn.CellTemplate.Clone() as DataGridViewCell;
            DataGridViewCell valueCell = valueColumn.CellTemplate.Clone() as DataGridViewCell;
            row.Cells.Add(nameCell);
            row.Cells.Add(valueCell);

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

            string superSystemPath, localID;
            if (m_current.Type == Constants.xpathSystem)
                Util.SplitSystemPath(m_current.Key, out superSystemPath, out localID);
            else
                Util.ParseEntityKey(m_current.Key, out superSystemPath, out localID);

            string propName = s_newPropPrefix + newPropIndex;
            string propValue = "0.0";

            EcellData data = new EcellData(
                propName,
                new EcellValue(Convert.ToDouble(propValue)),
                Util.BuildFullPN(
                    m_current.Type,
                    superSystemPath,
                    localID,
                    propName));
            m_current.AddValue(data);

            nameCell.Value = propName;
            valueCell.Value = propValue;
            valueCell.Tag = data;

            m_dgv.Rows.Add(row);
            valueCell.Selected = true;
        }

        private void deleteThisPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_propDic == null)
                return;

            DataGridViewRow row = m_dgv.CurrentRow;
            EcellData data = row.Cells[1].Tag as EcellData;

            if (!m_propDic.ContainsKey(data.Name))
            {
                EcellObject p = m_current.Copy();
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
                m_dgv.Rows.RemoveAt(row.Index);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            object tag = m_dgv.CurrentRow.Cells[1].Tag;
            deleteThisPropertyToolStripMenuItem.Enabled =
                m_propDic != null && tag is EcellData &&
                !m_propDic.ContainsKey(((EcellData)tag).Name);
        }

        private void m_dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewCell cell = m_dgv[1, m_dgv.CurrentCell.RowIndex];
            if (cell.Tag is string)
            {
                if ((string)cell.Tag == "ClassName" && cell is DataGridViewComboBoxCell)
                {
                    DataGridViewComboBoxEditingControl combo = (DataGridViewComboBoxEditingControl)e.Control;
                    EventHandler hdlr = null;
                    hdlr = delegate(object o, EventArgs _e)
                    {
                        string newClassName = (string)((DataGridViewComboBoxEditingControl)o).SelectedItem;
                        List<EcellData> props = new List<EcellData>();
                        foreach (KeyValuePair<string, EcellData> pair in m_env.DataManager.GetProcessProperty(newClassName))
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
                                val = m_current.GetEcellData(Constants.xpathVRL);
                            }
                            props.Add(val);
                        }
                        EcellObject obj = EcellObject.CreateObject(
                            m_current.ModelID, m_current.Key, m_current.Type,
                            newClassName, props);
                        NotifyDataChanged(obj.ModelID, obj.Key, obj);
                        m_current = obj;
                        ReloadProperties();
                        combo.SelectedIndexChanged -= hdlr;
                    };
                    combo.SelectedIndexChanged += hdlr;
                }
            }
        }
    }
}
