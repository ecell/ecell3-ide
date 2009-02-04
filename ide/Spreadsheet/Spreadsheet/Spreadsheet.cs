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
using System.Diagnostics;

using Ecell;
using Ecell.Plugin;
using Ecell.Objects;

using System.Drawing.Drawing2D;

namespace Ecell.IDE.Plugins.Spreadsheet
{
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

    /// <summary>
    /// Plugin class to display object by list.
    /// </summary>
    public class Spreadsheet : PluginBase, IRasterizable
    {
        #region Fields
        /// <summary>
        /// The flag whether the select change start in this plugin.
        /// </summary>
        private bool m_isSelected = false;
        private bool m_isSelectionChanged = false;
        private DataGridViewRow m_selectedRow = null;

        /// <summary>
        /// DataGridView to display the property of model.
        /// </summary>
        private DataGridView m_gridView;
        /// <summary>
        /// Popup menu.
        /// </summary>
        private ContextMenuStrip m_contextMenu;
        private EcellObject m_dragObject;
        /// <summary>
        /// Color of header.
        /// </summary>
        private Color m_headerColor = Color.Gray;
        private Color m_headerForeColor = Color.White;
        private Color m_systemColor = Color.FromArgb(0xff, 0xff, 0xd0);
        private Color m_processColor = Color.FromArgb(0xcc, 0xff, 0xcc);
        private Color m_variableColor = Color.FromArgb(0xcc, 0xcc, 0xff);
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

        #region Constructor
        /// <summary>
        /// Construcotor.
        /// </summary>
        public Spreadsheet()
        {
            m_time = new System.Windows.Forms.Timer();
            m_time.Enabled = false;
            m_time.Interval = 100;
            m_time.Tick += new EventHandler(FireTimer);
        }


        #endregion

        #region Initializer
        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            InitializeComponents();
        }
        #endregion

        /// <summary>
        /// Deconstructor for Spreadsheet.
        /// </summary>
        ~Spreadsheet()
        {
            m_gridView.Dispose();
        }

        /// <summary>
        /// Get the window form for Spreadsheet.
        /// </summary>
        /// <returns>UserControl</returns>        
        public override IEnumerable<EcellDockContent> GetWindowsForms()
        {
            EcellDockContent win = new EcellDockContent();
            m_gridView.Dock = DockStyle.Fill;
            win.Controls.Add(m_gridView);
            win.Name = "Spreadsheet";
            win.Text = MessageResources.Spreadsheet;
            win.Icon = MessageResources.objlist;
            win.TabText = win.Text;
            win.IsSavable = true;
            return new EcellDockContent[] { win };
        }

        void InitializeComponents()
        {
            m_gridView = new DataGridView();
            m_gridView.Dock = DockStyle.Fill;
            m_gridView.MultiSelect = true;
            m_gridView.AllowUserToAddRows = false;
            m_gridView.AllowUserToDeleteRows = false;
            m_gridView.AllowUserToResizeRows = false;
            m_gridView.RowHeadersVisible = true;
            m_gridView.ColumnHeadersVisible = true;
            m_gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_gridView.CellClick += new DataGridViewCellEventHandler(ClickObjectCell);
            m_gridView.SelectionChanged += new EventHandler(m_gridView_SelectionChanged);
            m_gridView.RowTemplate.DefaultHeaderCellType = typeof(DataGridViewNumberedRowHeaderCell);
            m_gridView.MouseMove += new MouseEventHandler(m_gridView_MouseMove);
            m_gridView.MouseDown += new MouseEventHandler(m_gridView_MouseDown);
            m_gridView.MouseUp += new MouseEventHandler(m_gridView_MouseUp);
            m_gridView.MouseLeave += new EventHandler(m_gridView_MouseLeave);
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

            m_contextMenu = new ContextMenuStrip();
            ToolStripMenuItem it = new ToolStripMenuItem();
            it.Text = MessageResources.SearchMenuText;
            it.ShortcutKeys = Keys.Control | Keys.F;
            it.Click += new EventHandler(ClickSearchMenu);

            m_contextMenu.Items.AddRange(new ToolStripItem[] { it });
        }

        /// <summary>
        /// get bitmap that converts display image on this plugin.
        /// </summary>
        /// <returns>bitmap data</returns>
        public Bitmap Print(string names)
        {
            Bitmap b = new Bitmap(m_gridView.Width, m_gridView.Height);
            m_gridView.DrawToBitmap(b, m_gridView.ClientRectangle);

            return b;
        }

        /// <summary>
        /// Get the name of this plugin.
        /// </summary>
        /// <returns>"Spreadsheet"</returns>
        public override string GetPluginName()
        {
            return "Spreadsheet";
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
        public IEnumerable<string> GetEnablePrintNames()
        {
            List<string> names = new List<string>();
            names.Add(MessageResources.Spreadsheet);
            return names;
        }

        /// <summary>
        /// The property of object is updated when simulation is runniing.
        /// </summary>
        private void UpdatePropForSimulation()
        {
            double ctime = DataManager.GetCurrentSimulationTime();
            if (ctime == 0.0)
                return;

            try
            {
                foreach (string entPath in m_propDic.Keys)
                {
                    EcellValue v = DataManager.GetEntityProperty(entPath);
                    if (v == null) continue;
                    m_propDic[entPath].Value = (string)v;
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
        /// The property of object is reset when simulation is stopped.
        /// </summary>
        private void ResetPropForSimulation()
        {
            List<EcellObject> list = DataManager.GetData(m_currentModelID, null);
            Clear();
            DataAdd(list);
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
            m_gridView.Rows.Add(rs);
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
            m_gridView.Rows.Insert(index, rs);
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
            m_gridView.Rows.Insert(index, rs);
        }

        /// <summary>
        /// Add the object to DataGridView.
        /// </summary>
        /// <param name="data">The added object.</param>
        public override void DataAdd(List<EcellObject> data)
        {
            if (data == null)
                return;
            foreach (EcellObject obj in data)
            {
                DataAdd(obj);
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
                return data != null ? (string)data.Value: "";
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
                return data.SizeInVolume.ToString();
            }
            else
            {
                foreach (EcellData d in obj.Value)
                {
                    if (name.Equals(d.Name))
                        return (string)d.Value;
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
                for (int j = 0; j < s_columnChars.Length; j++)
                {
                    if (!((string)m_gridView[j, i].Value).Contains(text))
                        continue;

                    ResetSelect();
                    if (i < 0 || i > m_gridView.RowCount - 1)
                        continue;
                    if (!m_gridView.Rows[i].Visible || m_gridView.Rows[i].Frozen)
                        continue;
                    m_gridView.Rows[i].Selected = true;
                    EcellObject obj = m_gridView.Rows[i].Tag as EcellObject;
                    if (obj == null)
                        return;
                    PluginManager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
                    return;
                }
            }
        }

        /// <summary>
        /// Event when the project is closed.
        /// </summary>
        public override void Clear()
        {
            m_gridView.Rows.Clear();
            m_propDic.Clear();
            CreateSystemHeader();
            CreateVariableHeader();
            CreateProcessHeader();
            m_currentModelID = null;
            m_lastSelected = null;
        }

        /// <summary>
        /// Event when the selected object is added.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="key">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public override void AddSelect(string modelID, string key, string type)
        {
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null)
            {
                m_isSelected = true;
                row.Selected = true;
                m_isSelected = false;
                if (m_gridView.FirstDisplayedScrollingRowIndex >= 0)
                    m_gridView.FirstDisplayedScrollingRowIndex = row.Index;
            }
        }

        /// <summary>
        /// Event when the selected object is removed.
        /// </summary>
        /// <param name="modelID">ModelID of the removed object.</param>
        /// <param name="key">ID of the removed object.</param>
        /// <param name="type">Type of the removed object.</param>
        public override void RemoveSelect(string modelID, string key, string type)
        {
            DataGridViewRow row = SearchIndex(type, key);
            if (row != null)
                row.Selected = false;
        }

        /// <summary>
        /// Event when object is selected.
        /// </summary>
        /// <param name="modelID">ModelID of the selected object.</param>
        /// <param name="key">ID of the selected object.</param>
        /// <param name="type">Type of the selected object.</param>
        public override void SelectChanged(string modelID, string key, string type)
        {
            if (m_isSelected)
                return;
            m_gridView.ClearSelection();
            m_selectedRow = null;
            AddSelect(modelID, key, type);
        }

        /// <summary>
        /// Event when the property of object is changed.
        /// </summary>
        /// <param name="modelID">ModelID of the changed object.</param>
        /// <param name="key">ID of the changed object.</param>
        /// <param name="type">Type of the changed object.</param>
        /// <param name="obj">The changed object.</param>
        public override void DataChanged(string modelID, string key, string type, EcellObject obj)
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
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public override void DataDelete(string modelID, string key, string type)
        {
            DataDelete(modelID, key, type, true);
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
                        if (((string)m_gridView[s_ID, i].Value).StartsWith(id))
                        {
                            DeleteDictionary(i);
                            m_isSelected = true;
                            m_gridView.Rows.RemoveAt(i);
                            m_isSelected = false;
                        }
                }
            }
            else
            {
                DeleteDictionary(ind);
                m_isSelected = true;
                m_gridView.Rows.RemoveAt(ind);
                m_isSelected = false;
            }
        }

        /// <summary>
        /// Event when the status of system is changed.
        /// </summary>
        /// <param name="status">the changed status.</param>
        public override void ChangeStatus(ProjectStatus status)
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

        #region Events
        /// <summary>
        /// Event when search button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void ClickSearchMenu(object sender, EventArgs e)
        {
            SearchInstanceDialog win = new SearchInstanceDialog();
            using (win)
            {
                if (win.ShowDialog() == DialogResult.OK)
                {
                    string searchText = win.SearchText;
                    SearchInstance(searchText);
                }
            }
        }

        void m_gridView_SelectionChanged(object sender, EventArgs e)
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
        void ClickObjectCell(object sender, DataGridViewCellEventArgs e)
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
                    PluginManager.SelectChanged(obj.ModelID, obj.Key, obj.Type);
                }
                else
                {
                    PluginManager.AddSelect(obj.ModelID, obj.Key, obj.Type);
                }
            }
            else
            {
                PluginManager.RemoveSelect(obj.ModelID, obj.Key, obj.Type);                                
            }
            m_isSelected = false;
            m_selectedRow = null;
        }
        /// <summary>
        /// ResetSelect
        /// </summary>
        public override void ResetSelect()
        {
            m_gridView.ClearSelection();
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

        void m_gridView_MouseLeave(object sender, EventArgs e)
        {
            m_isSelected = false;
        }

        void m_gridView_MouseUp(object sender, MouseEventArgs e)
        {
            m_isSelected = false;
        }

        private DataGridViewRow m_lastSelected = null;
        void m_gridView_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hti = m_gridView.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                if (hti.RowIndex < 0)
                    return;
                DataGridViewRow r = m_gridView.Rows[hti.RowIndex];
                if (Control.ModifierKeys != Keys.Shift)
                {
                    m_selectedRow = r;
                    m_lastSelected = r;
                }
                else
                {
                    if (m_lastSelected != null)
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
                            if (obj == null) continue;
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
                }
                m_dragObject = r.Tag as EcellObject;
            }
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


        void m_gridView_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;

            if (m_dragObject == null) return;
            EnterDragMode();
            m_dragObject = null;
        }
        #endregion
    }
}
