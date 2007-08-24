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
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Node;
using EcellLib.PathwayWindow.Element;
using PathwayWindow;
using PathwayWindow.UIComponent;
using System.IO;
using System.ComponentModel;

namespace EcellLib.PathwayWindow
{
    #region Enumeration
    /// <summary>
    /// Direction of scrolling the canvas.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Vertical direction
        /// </summary>
        Vertical,
        /// <summary>
        /// Horizontal direction
        /// </summary>
        Horizontal
    };

    /// <summary>
    /// Mode
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Select objects
        /// </summary>
        Select,
        /// <summary>
        /// Pan canvas
        /// </summary>
        Pan,
        /// <summary>
        /// Create reaction
        /// </summary>
        CreateOneWayReaction,
        /// <summary>
        /// Create mutual(Interactive) reaction
        /// </summary>
        CreateMutualReaction,
        /// <summary>
        /// Create constant
        /// </summary>
        CreateConstant,
        /// <summary>
        /// Create system
        /// </summary>
        CreateSystem,
        /// <summary>
        /// Create node
        /// </summary>
        CreateNode
    };
    #endregion

    /// <summary>
    /// PathwayView plays a role of View part of MVC-model.
    /// </summary>
    public class PathwayView
    {
        #region Static readonly fields
        /// <summary>
        /// Graphical content of m_canvas is scaled by m_reductionScale in overview canvas (m_overCanvas)
        /// </summary>
        private static readonly float REDUCTION_SCALE = 0.05f;

        /// <summary>
        /// Width of "Show" column of layer DataGridView.
        /// </summary>
        private readonly int LAYER_SHOWCOLUMN_WIDTH = 50;

        /// <summary>
        /// When E-Cell IDE classes(MainWindow, plugins) comminucates, Information of type of each E-Cell
        /// object is passed as string. Each E-cell objects are expressed as the following string.
        /// </summary>
        public static readonly string MODEL_STRING = "Model";

        /// <summary>
        /// When E-Cell IDE classes(MainWindow, plugins) comminucates, Information of type of each E-Cell
        /// object is passed as string. Each E-cell objects are expressed as the following string.
        /// </summary>
        public static readonly string SYSTEM_STRING = "System";

        /// <summary>
        /// When E-Cell IDE classes(MainWindow, plugins) comminucates, Information of type of each E-Cell
        /// object is passed as string. Each E-cell objects are expressed as the following string.
        /// </summary>
        public static readonly string VARIABLE_STRING = "Variable";

        /// <summary>
        /// When E-Cell IDE classes(MainWindow, plugins) comminucates, Information of type of each E-Cell
        /// object is passed as string. Each E-cell objects are expressed as the following string.
        /// </summary>
        public static readonly string PROCESS_STRING = "Process";

        /// <summary>
        /// When new system will be added by other plugin, it will be positioned this length away from
        /// parent system boundary.
        /// </summary>
        public static readonly float SYSTEM_SYSTEM_MARGIN = 60;
        #endregion

        #region Fields
        /// <summary>
        /// PathwayWindow.
        /// </summary>
        PathwayWindow m_pathwayWindow;

        /// <summary>
        /// A list of ComponentSettings.
        /// </summary>
        List<ComponentSetting> m_componentSettings;

        //List<PBasicInputEventHandler> m_objectHandlerList = new List<PBasicInputEventHandler>();
        
        //List<PBasicInputEventHandler> m_canvasHandlerList = new List<PBasicInputEventHandler>();

        Dictionary<int, PBasicInputEventHandler> m_handlerDict = new Dictionary<int, PBasicInputEventHandler>();

        /// <summary>
        /// A list of toolbox buttons.
        /// </summary>
        List<ToolStripButton> m_buttonList = new List<ToolStripButton>();

        /// <summary>
        /// Dictionary for canvases.
        ///  key: canvas ID
        ///  value: CanvasViewComponentSet
        /// </summary>
        Dictionary<string, CanvasView> m_canvasDict;

        /// <summary>
        /// The CanvasID of currently active canvas.
        /// </summary>
        string m_activeCanvasID;

        /// <summary>
        /// Having relation between Ecell key of a system and canvas ID;
        /// </summary>
        Dictionary<string, string> m_keySysCanvasDict = new Dictionary<string,string>();
        
        /// <summary>
        /// Having relation between Ecell key of a variable and canvas ID.
        /// </summary>
        Dictionary<string, string> m_keyVarCanvasDict = new Dictionary<string,string>();

        /// <summary>
        /// Having relation between Ecell key of a process and canvas ID.
        /// </summary>
        Dictionary<string, string> m_keyProCanvasDict = new Dictionary<string,string>();

        /// <summary>
        /// ComponentSettingsManager for creating Systems and Nodes
        /// </summary>
        ComponentSettingsManager m_csManager;

        /// <summary>
        /// SplitContainer splits main pathway area and other area (overview, layer control)
        /// </summary>
        SplitContainer m_splitCon;

        /// <summary>
        /// Tshi tabcontrol contains tab pages which represent different type of networks, such as 
        /// metabolic networks, genetic networks.
        /// </summary>
        TabControl m_tabControl;

        /// <summary>
        /// GroupBox for overview panel.
        /// </summary>
        GroupBox m_overviewGB;

        /// <summary>
        /// PCanvas for overview area
        /// </summary>
        PCanvas m_overCanvas;

        /// <summary>
        /// Point of mouse cursor.
        /// </summary>
        PointF m_pointF;

        /// <summary>
        /// PPath on m_overCanvas, which stands for a viewed area in m_canvas (Main canvas)
        /// </summary>
        PDisplayedArea m_displayedArea;

        /// <summary>
        /// DataGridView for layer control panel.
        /// </summary>
        DataGridView m_dgv;

        /// <summary>
        /// DataSet for layers.
        /// </summary>
        DataSet m_layerDs;

        /// <summary>
        /// DataTable for showing layer information table on a layer control panel (DataGridView)
        /// </summary>
        DataTable m_layerDataTable;

        /// <summary>
        /// Whether the lower panel (overview, layer control) is shown or not (collapsed).
        /// </summary>
        bool m_lowerPanelShown = true;

        /// <summary>
        /// Whether each node is showing it's ID or not;
        /// </summary>
        bool m_showingId = true;

        /// <summary>
        /// Indicate which pathway-related toolbar button is selected.
        /// </summary>
        private Handle m_selectedHandle;

        /// <summary>
        /// Every time when m_dgv.CurrentCellDirtyStateChanged event occurs, 
        /// m_dgv_CurrentCellDirtyStateChanged delegate will be called twice.
        /// This flag is used for neglecting one of two delagate calling.
        /// </summary>
        private bool m_dirtyEventProcessed = false;

        /// <summary>
        /// Whether PathwayView is freezed or not.
        /// </summary>
        private bool m_isFreezed = false;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
        #endregion

        #region Accessors
        /// <summary>
        ///  get/set Dctionary of CanvasViewComponentSet.
        /// </summary>
        public Dictionary<string, CanvasView> CanvasDictionary
        {
            get { return m_canvasDict; }
        }

        /// <summary>
        /// Accessor for currently active canvas.
        /// </summary>
        public CanvasView ActiveCanvas
        {
            get { return m_canvasDict[m_activeCanvasID]; }
        }

        /// <summary>
        /// get/set the number of checked component.
        /// </summary>
        public Handle SelectedHandle
        {
            get { return m_selectedHandle; }
            set { m_selectedHandle = value; }
        }

        /// <summary>
        /// get the setting of component.
        /// </summary>
        public List<ComponentSetting> ComponentSettings
        {
            get { return m_componentSettings; }
        }

        /// <summary>
        /// get/set the CoponentSettingManager.
        /// </summary>
        public ComponentSettingsManager ComponentSettingsManager
        {
            get { return this.m_csManager; }
            set { this.m_csManager = value; }
        }

        /// <summary>
        /// get the control for split.
        /// </summary>
        public Control Control
        {
            get { return m_splitCon; }
        }

        /// <summary>
        /// get/set the flag of showing id.
        /// </summary>
        public bool ShowingID
        {
            get { return m_showingId; }
            set
            {
                m_showingId = value;
                if (m_canvasDict == null) return;
                foreach(CanvasView set in m_canvasDict.Values)
                {
                    set.ShowingID = m_showingId;
                }
            }
        }

        /// <summary>
        ///  get/set PathwayWindow related this object.
        /// </summary>
        public PathwayWindow Window
        {
            get { return this.m_pathwayWindow; }
            set { this.m_pathwayWindow = value; }
        }

        /// <summary>
        ///  get TabControl related this object.
        /// </summary>
        public TabControl TabControl
        {
            get { return this.m_tabControl; }
        }

        /// <summary>
        /// get the list of string for canvas of Process.
        /// </summary>
        public Dictionary<string, string> KeyProcessCanvas
        {
            get { return this.m_keyProCanvasDict; }
        }

        /// <summary>
        /// get/set the number of checked component.
        /// </summary>
        public PointF MousePosition
        {
            get { return m_pointF; }
            set { m_pointF = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// the constructor for PathwayView.
        /// set the handler of event and user control.
        /// </summary>
        public PathwayView()
        {
            // Preparing a pathway panel
            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;
            m_tabControl.SelectedIndexChanged += new EventHandler(m_tabControl_SelectedIndexChanged);
            m_tabControl.MouseEnter += new EventHandler(m_tabControl_MouseEnter);
            m_tabControl.MouseWheel += new MouseEventHandler(m_tabControl_MouseWheel);
            /*
            ContextMenuStrip nodeMenu = new ContextMenuStrip();
            ToolStripItem delete = new ToolStripMenuItem("Delete");
            delete.Click += new EventHandler(delete_Click);
            nodeMenu.Items.AddRange(new ToolStripItem[]
                                    {
                                        delete
                                    });
            m_tabControl.ContextMenuStrip = nodeMenu;

             */ 
            m_splitCon = new SplitContainer();
            m_splitCon.Dock = DockStyle.Fill;
            m_splitCon.Orientation = Orientation.Horizontal;
            m_splitCon.BorderStyle = BorderStyle.Fixed3D;
            m_splitCon.SplitterDistance = 300;
            m_splitCon.SplitterMoved += new SplitterEventHandler(m_splitCon_SplitterMoved);
            m_splitCon.Panel1.Controls.Add(m_tabControl);

            SplitContainer lowerSplitCon = new SplitContainer();
            lowerSplitCon.Dock = DockStyle.Fill;
            lowerSplitCon.Orientation = Orientation.Vertical;
            lowerSplitCon.BorderStyle = BorderStyle.Fixed3D;
            lowerSplitCon.FixedPanel = FixedPanel.Panel1;
            lowerSplitCon.SplitterDistance = 200;
            lowerSplitCon.SplitterMoved += new SplitterEventHandler(m_splitCon_SplitterMoved);
            m_splitCon.Panel2.Controls.Add(lowerSplitCon);

            // Preparing an overview panel
            m_overviewGB = new GroupBox();
            m_overviewGB.Dock = DockStyle.Fill;
            m_overviewGB.Text = "Overview";
            m_displayedArea = new PDisplayedArea();
            m_overviewGB.Controls.Add(m_overCanvas);
            lowerSplitCon.Panel1.Controls.Add(m_overviewGB);

            // Preparing a layer control panel
            m_dgv = new DataGridView();
            m_layerDataTable = new DataTable();
            DataColumn showDc = new DataColumn("Show");
            showDc.DataType = typeof(bool);
            m_layerDataTable.Columns.Add(showDc);
            DataColumn nameDc = new DataColumn("Name");
            nameDc.DataType = typeof(string);
            m_layerDataTable.Columns.Add(nameDc);
            m_layerDs = new DataSet();
            m_layerDs.Tables.Add(m_layerDataTable);
            m_dgv.Dock = DockStyle.Fill;
            m_dgv.DataSource = m_layerDs;
            m_dgv.AllowUserToAddRows = false;
            m_dgv.AllowUserToDeleteRows = false;
            m_dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            m_dgv.MultiSelect = false;
            m_dgv.RowHeadersVisible = false;
            m_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            m_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            m_dgv.AllowUserToResizeRows = false;
            m_dgv.CurrentCellDirtyStateChanged += new EventHandler(m_dgv_CurrentCellDirtyStateChanged);
            m_dgv.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgv_DataBindingComplete);            
            m_dgv.VisibleChanged += new EventHandler(m_dgv_VisibleChanged);

            GroupBox layerGB = new GroupBox();
            layerGB.Dock = DockStyle.Fill;
            layerGB.Text = "Layer";
            layerGB.Controls.Add(m_dgv);

            lowerSplitCon.Panel2.Controls.Add(layerGB);

            // Preparing handler list.
            //m_canvasHandlerList.Add( new DefaultMouseHandler(this));
            //m_canvasHandlerList.Add( new PPanEventHandler() );
            //m_canvasHandlerList.Add(new CreateReactionMouseHandler(this));
        }

        void m_dgv_VisibleChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).Columns.Contains("Show") && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns["Show"].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns["Show"].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns["Show"].Frozen = true;
                ((DataGridView)sender).Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                ((DataGridView)sender).Columns["Name"].ReadOnly = true;
            }
        }

        #endregion

        /// <summary>
        /// copy selected object(EcellObject) to new object.
        /// </summary>
        /// <param name="originalKey">the key of selected object.</param>
        /// <param name="newKey">the key of new object.</param>
        public void CopyEcellObject(string originalKey, string newKey)
        {
            m_pathwayWindow.CopyEcellObject(originalKey, newKey);
        }

        /// <summary>
        /// not implement.
        /// when we introduce the multi model, 
        ///  you must edit this function.
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        void delete_Click(object sender, EventArgs e)
        {
            // not implement.
            // when we introduce the multi model, 
            // you must edit this function.
        }

        /// <summary>
        /// event sequence of changing the check of layer showing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {            
            if (!m_dirtyEventProcessed)
            {
                string dataMember = ((DataGridView)sender).DataMember;
                bool show = !(bool)((DataGridView)sender).CurrentRow.Cells["Show"].Value;
                string layerName = (string)((DataGridView)sender).CurrentRow.Cells["Name"].Value;             
                m_canvasDict[dataMember].Layers[layerName].Visible = show;
                m_canvasDict[dataMember].PathwayCanvas.Refresh();
                m_canvasDict[dataMember].OverviewCanvas.Refresh();
                m_canvasDict[dataMember].RefreshVisibility();
                m_dirtyEventProcessed = true;
            }
            else
            {
                m_dirtyEventProcessed = false;
            }
            ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        /// <summary>
        /// Add new object to this canvas.
        /// </summary>
        /// <param name="canvasName">name of canvas</param>
        /// <param name="systemName">name of system</param>
        /// <param name="cType">type of component</param>
        /// <param name="cs">ComponentSetting</param>
        /// <param name="key">key</param>
        /// <param name="hasCoords">whether an object has coordinates or not</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="needToNotify">whether notification is needed or not</param>
        /// <param name="eo">EcellObject</param>
        /// <param name="valueStr"></param>
        /// <param name="change"></param>
        public void AddNewObj(string canvasName,
            string systemName,
            ComponentType cType,
            ComponentSetting cs,
            string modelID,
            string key,
            bool hasCoords,
            float x,
            float y,
            float width,
            float height,
            bool needToNotify,
            EcellObject eo,
            string valueStr,
            bool change)
        {
            if (string.IsNullOrEmpty(key))
                throw new PathwayException(m_resources.GetString("ErrKeyNot"));

            if (!RegisterObj(cType, key, canvasName))
                return;

            if (needToNotify)
            {
                if (eo == null)
                    throw new PathwayException(m_resources.GetString("ErrAddObjNot"));

                try
                {
                    NotifyDataAdd(eo);
                }
                catch (IgnoreException)
                {
                    UnregisterObj(cType, key);
                    return;
                }
                catch (Exception e)
                {
                    UnregisterObj(cType, key);
                    MessageBox.Show(m_resources.GetString("ErrAddObj"), "Error\n" + e.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
                        
            ComponentElement element = null;
            string type = null;
            switch(cType)
            {
                case ComponentType.Variable:
                    type = VARIABLE_STRING;
                    if(key.EndsWith(":SIZE"))
                    {
                        element = new AttributeElement();
                        if (!needToNotify)
                        {
                            if (null == valueStr)
                                ((AttributeElement)element).Value = GetEcellData(key, type, "Value");
                            else
                                ((AttributeElement)element).Value = valueStr;
                        }
                    }
                    else
                    {
                        element = new VariableElement();
                        type = VARIABLE_STRING;
                        if(cs == null)
                            cs = ComponentSettingsManager.DefaultVariableSetting;
                    }
                    break;

                case ComponentType.Process:
                    element = new ProcessElement();
                    type = PROCESS_STRING;
                    if(cs == null)
                        cs = ComponentSettingsManager.DefaultProcessSetting;
                    if(!needToNotify && !change)
                        ((ProcessElement)element).SetEdgesByStr(GetEcellData(key, type, "VariableReferenceList"));
                    break;

                case ComponentType.System:
                    element = new SystemElement();
                    type = SYSTEM_STRING;
                    if (cs == null)
                        cs = ComponentSettingsManager.DefaultSystemSetting;
                    if(hasCoords)
                    {
                        ((SystemElement)element).Width = width;
                        ((SystemElement)element).Height = height;
                    }
                    else
                    {
                        ((SystemElement)element).Width = PEcellSystem.DEFAULT_WIDTH;
                        ((SystemElement)element).Height = PEcellSystem.DEFAULT_HEIGHT;
                    }

                    break;
            }

            element.ModelID = modelID;
            element.Key = key;
            element.CanvasID = canvasName;
            element.X = x;
            element.Y = y;
            element.Type = type;

            if (!string.IsNullOrEmpty(canvasName) && m_canvasDict.ContainsKey(canvasName))
            {
                if (element is AttributeElement)
                    m_canvasDict[canvasName].AddAttributeToSystem(((AttributeElement)element).TargetKey, (AttributeElement)element);
                else if(element is SystemElement)
                {
                    List<PEcellSystem> systemList = new List<PEcellSystem>();
                    SystemElement se = (SystemElement)element;
                    CanvasView canvas = m_canvasDict[canvasName];
                    foreach(PLayer layer in canvas.Layers.Values)
                    {
                        PEcellSystem system = (PEcellSystem)cs.CreateNewComponent(se.X, se.Y, se.Width, se.Height, this);
                        system.Reset();
                        system.Element = se;
                        system.MouseDown += new PInputEventHandler(SystemSelected);
                        systemList.Add(system);
                        system.Layer = layer;
                        system.Name = key;
                        systemName = system.Name;
                        string parentSystemId = PathUtil.GetParentSystemId(key);
                        if (string.IsNullOrEmpty(parentSystemId))
                            layer.AddChild(system);
                        else
                            canvas.AddNewObj(null, parentSystemId, system, hasCoords, false);
                            //canvas.AddChildToSelectedSystem(parentSystemId, system, hasCoords);
                    }
                    se.X = systemList[0].X;
                    se.Y = systemList[0].Y;
                    canvas.AddSystem(key, se, null, systemList);
                }
                else
                {
                    PPathwayObject obj = cs.CreateNewComponent(x, y, 0, 0, this);
                    ((PPathwayNode)obj).Element = (NodeElement)element;
                    ((PPathwayNode)obj).ShowingID = m_showingId;
                    obj.MouseDown += new PInputEventHandler(NodeSelected);
                    obj.MouseEnter += new PInputEventHandler(NodeEntered);
                    obj.MouseLeave += new PInputEventHandler(NodeLeft);
                    ((PPathwayNode)obj).Handler4Line = new PInputEventHandler(LineSelected);

                    string layer = null;
                    if (m_dgv.SelectedRows.Count != 0)
                        layer = (string)m_dgv[m_dgv.Columns["Name"].Index, m_dgv.SelectedRows[0].Index].Value;
                    m_canvasDict[canvasName].AddNewObj(layer, systemName, obj, hasCoords, false);
                }                
            }
            else
                throw new PathwayException(m_resources.GetString("ErrNotSetCanvas") + key);

            //m_pathwayWindow.NotifySelectChanged(key, type);
        }
        
        /// <summary>
        /// Add PPathwayObject to selected layer of PCanvas
        /// </summary>
        /// <param name="canvas">obj is add to this canvas</param>
        /// <param name="obj">obj is add to this canvas</param>
        public void AddChildToSelectedLayer(string canvas, PPathwayObject obj)
        {
            if (obj is PPathwayNode)
                ((PPathwayNode)obj).ShowingID = m_showingId;

            if(m_dgv.SelectedRows.Count != 0)
            {
                string layer = (string)m_dgv[m_dgv.Columns["Name"].Index, m_dgv.SelectedRows[0].Index].Value;
                m_canvasDict[canvas].AddChildToSelectedLayer(layer, obj);
            }
        }

        /// <summary>
        /// Add PPathwayObject to selected layer of PCanvas
        /// </summary>
        /// <param name="canvas">obj is add to this canvas</param>
        /// <param name="system">obj is add to this system</param>
        /// <param name="obj">obj is add to this canvas</param>
        public void AddChildToSelectedLayer(string canvas, string system, PPathwayObject obj)
        {
            if (obj is PPathwayNode)
                ((PPathwayNode)obj).ShowingID = m_showingId;

            if (m_dgv.SelectedRows.Count != 0)
            {
                string layer = (string)m_dgv[m_dgv.Columns["Name"].Index, m_dgv.SelectedRows[0].Index].Value;
                m_canvasDict[canvas].AddChildToSelectedLayer(layer, system, obj);
            }
        }

        /// <summary>
        /// Add PPathwayObject to top visible layer of PCanvas.
        /// </summary>
        /// <param name="canvas">obj is add to this canvas</param>
        /// <param name="obj">this obj is add</param>
        public void AddChildToTopVisibleLayer(string canvas, PPathwayObject obj)
        {
            m_canvasDict[canvas].AddChildToTopVisibleLayer(obj);
        }
        
        /// <summary>
        /// Add the selected EventHandler to event listener.
        /// </summary>
        /// <param name="handler">added EventHandler.</param>
        public void AddInputEventListener(PBasicInputEventHandler handler)
        {
            if(m_canvasDict != null)
            {
                foreach (CanvasView set in m_canvasDict.Values)
                {
                    set.PathwayCanvas.AddInputEventListener(handler);
                }
            }
        }

        /// <summary>
        /// Register a key to this PathwayView's dictionary.
        /// </summary>
        /// <param name="type">Object's type (Variable, Process, System)</param>
        /// <param name="key">ECell key</param>
        /// <param name="canvasName">
        /// true, if registered.
        /// false, if not registered because key is null or etc.
        /// </param>
        private bool RegisterObj(ComponentType type, string key, string canvasName)
        {
            if (key == null || canvasName == null)
                return false;

            switch (type)
            {
                case ComponentType.Variable:
                    if (m_keyVarCanvasDict.ContainsKey(key))
                        return false;
                    else
                    {
                        m_keyVarCanvasDict.Add(key, canvasName);
                        return true;
                    }

                case ComponentType.Process:
                    if (m_keyProCanvasDict.ContainsKey(key))
                        return false;
                    else
                    {
                        m_keyProCanvasDict.Add(key, canvasName);
                        return true;
                    }

                case ComponentType.System:
                    if (m_keySysCanvasDict.ContainsKey(key))
                        return false;
                    else
                    {
                        m_keySysCanvasDict.Add(key, canvasName);
                        return true;
                    }
            }
            return false;
        }

        /// <summary>
        /// Unregister a key from this PathwayView's dictionary.
        /// </summary>
        /// <param name="type">Object's type (Variable, Process, System)</param>
        /// <param name="key">ECell key</param>
        /// <returns>
        /// true, if unregistered.
        /// false, if not unregistered bacause key is null or the registory doesn't have the key.
        /// </returns>
        private bool UnregisterObj(ComponentType type, string key)
        {
            if (key == null)
                return false;

            switch (type)
            {
                case ComponentType.Variable:
                    if (m_keyVarCanvasDict.ContainsKey(key))
                    {
                        m_keyVarCanvasDict.Remove(key);
                        return true;
                    }
                    else
                        return false;
                case ComponentType.Process:
                    if (m_keyProCanvasDict.ContainsKey(key))
                    {
                        m_keyProCanvasDict.Remove(key);
                        return true;
                    }
                    else
                        return false;                    
                case ComponentType.System:
                    if (m_keySysCanvasDict.ContainsKey(key))
                    {
                        m_keySysCanvasDict.Remove(key);
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }

        /// <summary>
        /// Delete the selected EventHandler from event listener.
        /// </summary>
        /// <param name="handler">deleted EventHandler.</param>
        public void RemoveInputEventListener(PBasicInputEventHandler handler)
        {
            if(m_canvasDict != null)
            {
                foreach(CanvasView set in m_canvasDict.Values)
                {
                    set.PathwayCanvas.RemoveInputEventListener(handler);
                }
            }
        }

        #region Event delegate
        /// <summary>
        /// When select the button in ToolBox,
        /// system change the listener for event
        /// </summary>
        /// <param name="sender">ToolBoxMenuButton.</param>
        /// <param name="e">EventArgs.</param>
        public void ButtonStateChanged(object sender, EventArgs e)
        {
            /*
            if (CheckedComponent >= 0)
                RemoveInputEventListener(m_objectHandlerList[CheckedComponent]);
            else if (CheckedComponent == -1)
                RemoveInputEventListener(m_canvasHandlerList[0]);
            else if (CheckedComponent == -2)
                RemoveInputEventListener(m_canvasHandlerList[1]);
            else if (CheckedComponent == -3)
                RemoveInputEventListener(m_canvasHandlerList[2]);
            else if (CheckedComponent == -4)
                RemoveInputEventListener(m_canvasHandlerList[2]);*/

            RemoveInputEventListener(m_handlerDict[SelectedHandle.HandleID]);
                        
            SelectedHandle = (Handle)((ToolStripButton)sender).Tag;
            
            foreach (ToolStripButton button in m_buttonList)
            {
                if (button.Tag != SelectedHandle)
                {
                    button.Checked = false;
                }
            }

            AddInputEventListener(m_handlerDict[SelectedHandle.HandleID]);

            if (SelectedHandle.Mode == Mode.Pan)
            {
                m_splitCon.Cursor = new Cursor(new MemoryStream(Resource1.move));
                SetRefreshOverview(true);
                Freeze();
            }
            else
            {
                m_splitCon.Cursor = Cursors.Arrow;
                SetRefreshOverview(false);
                Unfreeze();
            }
            /*
            if (CheckedComponent >= 0)
            {
                AddInputEventListener(m_objectHandlerList[CheckedComponent]);
                SetRefreshOverview(false);
                Unfreeze();
            }
            else if (CheckedComponent == -1)
            {
                AddInputEventListener(m_canvasHandlerList[0]);
                SetRefreshOverview(false);
                Unfreeze();
            }
            else if (CheckedComponent == -2)
            {
                m_splitCon.Cursor = new Cursor( new MemoryStream( Resource1.move ));
                AddInputEventListener(m_canvasHandlerList[1]);
                SetRefreshOverview(true);
                Freeze();
            }
            else if (CheckedComponent == -3)
            {
                AddInputEventListener(m_canvasHandlerList[2]);
                SetRefreshOverview(false);
                Unfreeze();
            }
            else if (CheckedComponent == -4)
            {
                AddInputEventListener(m_canvasHandlerList[2]);
                SetRefreshOverview(false);
                Unfreeze();
            }*/
        }

        /// <summary>
        /// set whether refresh the overview.
        /// </summary>
        /// <param name="value"></param>
        void SetRefreshOverview(bool value)
        {
            if (m_canvasDict == null)
                return;

            foreach(CanvasView set in m_canvasDict.Values)
            {
                set.RefreshOverview = value;
            }
        }

        /// <summary>
        /// When change the focused tab control,
        /// system show the selected model in overview.
        /// </summary>
        /// <param name="sender">TabControl.</param>
        /// <param name="e">EventArgs.</param>
        void m_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((TabControl)sender).TabCount != 0)
            {
                string name = ((TabControl)sender).TabPages[((TabControl)sender).SelectedIndex].Name;

                m_dgv.DataMember = name;

                m_overviewGB.Controls.Remove(m_overCanvas);
                m_overCanvas = m_canvasDict[name].OverviewCanvas;
                m_overviewGB.Controls.Add(m_overCanvas);

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">TabControl.</param>
        /// <param name="e">MouseEventArgs.</param>
        void m_tabControl_MouseWheel(object sender, MouseEventArgs e)
        {

            if (Control.ModifierKeys == Keys.Shift)
            {
                this.PanCanvas(Direction.Horizontal, e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Control)
            {
                float zoom = (float)1.00 + (float)e.Delta / 1200;
                this.ActiveCanvas.Zoom(zoom);
            }
            else
            {
                this.PanCanvas(Direction.Vertical, e.Delta);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">TabControl.</param>
        /// <param name="e">EventArgs.</param>
        void m_tabControl_MouseEnter(object sender, EventArgs e)
        {
            this.TabControl.Focus();
        }
        
        /// <summary>
        /// When binding data in DataGridView,
        /// ...
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewBindingComplete.</param>
        void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (((DataGridView)sender).Columns.Contains("Show") && ((DataGridView)sender).Visible)
            {
                ((DataGridView)sender).Columns["Show"].Width = LAYER_SHOWCOLUMN_WIDTH;
                ((DataGridView)sender).Columns["Show"].Resizable = DataGridViewTriState.False;
                ((DataGridView)sender).Columns["Show"].Frozen = true;
                ((DataGridView)sender).Columns["Name"].SortMode = DataGridViewColumnSortMode.NotSortable;
                ((DataGridView)sender).Columns["Name"].ReadOnly = true;
            }
        }

        /// <summary>
        /// When move the splitter, system redraw the overview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">SplitterEventArgs.</param>
        void m_splitCon_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateOverview();
        }
        #endregion
        
        #region Methods from controller (PathwayWindow) to view        
        /// <summary>
        /// Get a list of ToolStripItems.
        /// </summary>
        /// <returns>the list of ToolStripItems.</returns>
        public List<ToolStripItem> GetToolBarMenuStripItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();

            // Used for ID of handle
            int handleCount = 0;

            ToolStripButton handButton = new ToolStripButton();
            handButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            handButton.Name = "MoveCanvas";
            handButton.Image = Resource1.move1;
            handButton.Text = "";
            handButton.CheckOnClick = true;
            handButton.ToolTipText = "MoveCanvas";
            handButton.Tag = new Handle(Mode.Pan, handleCount, -1);
            m_handlerDict.Add(handleCount++, new PPanEventHandler());
            handButton.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(handButton);
            m_buttonList.Add(handButton);

            ToolStripButton button0 = new ToolStripButton();
            button0.ImageTransparentColor = System.Drawing.Color.Magenta;
            button0.Name = "SelectMode";
            button0.Image = Resource1.arrow;
            button0.Text = "";
            button0.CheckOnClick = true;
            button0.ToolTipText = "SelectMode";
            button0.Tag = new Handle(Mode.Select, handleCount, -1);
            m_handlerDict.Add(handleCount++, new DefaultMouseHandler(this));
            button0.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(button0);
            m_buttonList.Add(button0);

            ToolStripSeparator sep = new ToolStripSeparator();
            sep.Tag = 7;
            list.Add(sep);

            ToolStripButton arrowButton = new ToolStripButton();
            arrowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            arrowButton.Name = "reactionOneway";
            arrowButton.Image = Resource1.arrow_long_right_w;
            arrowButton.Text = "";
            arrowButton.CheckOnClick = true;
            arrowButton.ToolTipText = "Add Mutual Reaction";
            arrowButton.Tag = new Handle(Mode.CreateOneWayReaction, handleCount, -1);
            m_handlerDict.Add(handleCount++, new CreateReactionMouseHandler(this));
            arrowButton.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(arrowButton);
            m_buttonList.Add(arrowButton);

            ToolStripButton bidirButton = new ToolStripButton();
            bidirButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            bidirButton.Name = "reactionMutual";
            bidirButton.Image = Resource1.arrow_long_bidir_w;
            bidirButton.Text = "";
            bidirButton.CheckOnClick = true;
            bidirButton.ToolTipText = "Add Oneway Reaction";
            bidirButton.Tag = new Handle(Mode.CreateMutualReaction, handleCount, -1);
            m_handlerDict.Add(handleCount++, new CreateReactionMouseHandler(this));
            bidirButton.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(bidirButton);
            m_buttonList.Add(bidirButton);

            ToolStripButton constButton = new ToolStripButton();
            constButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            constButton.Name = "constant";
            constButton.Image = Resource1.ten;
            constButton.Text = "";
            constButton.CheckOnClick = true;
            constButton.ToolTipText = "Add Constant";
            constButton.Tag = new Handle(Mode.CreateConstant, handleCount, -1);
            m_handlerDict.Add(handleCount++, new CreateReactionMouseHandler(this));
            constButton.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(constButton);
            m_buttonList.Add(constButton);

            ToolStripButton zoominButton = new ToolStripButton();            
            zoominButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoominButton.Name = "zoomin";
            zoominButton.Image = Resource1.zoom_in;
            zoominButton.Text = "";
            zoominButton.CheckOnClick = false;
            zoominButton.ToolTipText = "Zoom In";
            zoominButton.Tag = 2f;
            //m_handlerDict.Add(handleCount++, new CreateReactionMouseHandler(this));
            zoominButton.Click += new EventHandler(ZoomButton_Click);
            list.Add(zoominButton);
            m_buttonList.Add(zoominButton);

            ToolStripButton zoomoutButton = new ToolStripButton();
            zoomoutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoomoutButton.Name = "zoomout";
            zoomoutButton.Image = Resource1.zoom_out;
            zoomoutButton.Text = "";
            zoomoutButton.CheckOnClick = false;
            zoomoutButton.ToolTipText = "Zoom Out";
            zoomoutButton.Tag = 0.5f;
            //m_handlerDict.Add(handleCount++, new CreateReactionMouseHandler(this));
            zoomoutButton.Click += new EventHandler(ZoomButton_Click);
            list.Add(zoomoutButton);
            m_buttonList.Add(zoomoutButton);

            CreateSystemMouseHandler csmh = new CreateSystemMouseHandler(this);
            CreateNodeMouseHandler cnmh = new CreateNodeMouseHandler(this);

            int count = 0;

            foreach (ComponentSetting cs in m_componentSettings)
            {
                ToolStripButton button = new ToolStripButton();
                button.ImageTransparentColor = System.Drawing.Color.Magenta;
                button.Name = cs.Name;
                button.Image = new Bitmap(256, 256);
                Graphics gra = Graphics.FromImage(button.Image);
                if (cs.ComponentKind == ComponentType.System)
                {
                    Rectangle rect = new Rectangle(3, 3, 240, 240);
                    gra.DrawRectangle(new Pen(Brushes.Black, 16), rect);
                    //m_objectHandlerList.Add(new CreateSystemMouseHandler(this));
                    m_handlerDict.Add(handleCount, csmh);
                    button.Tag = new Handle(Mode.CreateSystem, handleCount++, count++);
                }
                else
                {
                    GraphicsPath gp = cs.TransformedPath;
                    gra.FillPath(cs.NormalBrush, gp);
                    gra.DrawPath(new Pen(Brushes.Black, 16), gp);
                    //m_objectHandlerList.Add(new CreateNodeMouseHandler(this));
                    m_handlerDict.Add(handleCount, cnmh);
                    button.Tag = new Handle(Mode.CreateNode, handleCount++, count++);
                }
                button.Size = new System.Drawing.Size(256, 256);
                button.Text = "";
                button.CheckOnClick = true;
                button.ToolTipText = cs.Name;
                
                button.Click += new EventHandler(this.ButtonStateChanged);
                list.Add(button);
                m_buttonList.Add(button);
            }

            // SelectMode is default.
            button0.Checked = true;
            SelectedHandle = (Handle)button0.Tag;

            return list;
        }

        void ZoomButton_Click(object sender, EventArgs e)
        {
            float rate = (float)((ToolStripButton)sender).Tag;
            
            foreach(CanvasView canvas in this.CanvasDictionary.Values)
            {
                canvas.Zoom(rate);
            }
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="key">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="data">Changed value of object.</param>
        public void DataChanged(string key, EcellObject data, ComponentType type)
        {
            if(string.IsNullOrEmpty(key))
                return;
            switch (type)
            {
                case ComponentType.System:
                    if (!m_keySysCanvasDict.ContainsKey(key))
                        return;
                    m_canvasDict[m_keySysCanvasDict[key]].DataChanged(key, data, type);
                    if (data.key != null && !key.Equals(data.key))
                    {
                        string canvasId = m_keySysCanvasDict[key];
                        m_keySysCanvasDict.Remove(key);
                        m_keySysCanvasDict.Add(data.key, canvasId);
                    }
                    break;
                case ComponentType.Variable:
                    if (!m_keyVarCanvasDict.ContainsKey(key))
                        return;
                    m_canvasDict[m_keyVarCanvasDict[key]].DataChanged(key, data, type);
                    if(data.key != null && !key.Equals(data.key))
                    {
                        string canvasId = m_keyVarCanvasDict[key];
                        m_keyVarCanvasDict.Remove(key);
                        m_keyVarCanvasDict.Add(data.key, canvasId);
                    }
                    break;
                case ComponentType.Process:
                    if (!m_keyProCanvasDict.ContainsKey(key))
                        return;
                    m_canvasDict[m_keyProCanvasDict[key]].DataChanged(key, data, type);
                    if(data.key != null && !key.Equals(data.key))
                    {
                        string canvasId = m_keyProCanvasDict[key];
                        m_keyProCanvasDict.Remove(key);
                        m_keyProCanvasDict.Add(data.key, canvasId);
                    }
                    break;
            }
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>        
        public void DataDelete(string key, ComponentType type)
        {
            switch(type)
            {
                case ComponentType.System:
                    this.DeleteFromRegistoryUnder(key);
                    if (m_keySysCanvasDict.ContainsKey(key))
                    {
                        if (m_canvasDict.ContainsKey(m_keySysCanvasDict[key]))
                            m_canvasDict[m_keySysCanvasDict[key]].DataDelete(key, ComponentType.System);
                        m_keySysCanvasDict.Remove(key);
                    }                    
                    break;
                case ComponentType.Variable:
                    if (m_keyVarCanvasDict.ContainsKey(key))
                    {
                        if (m_canvasDict.ContainsKey(m_keyVarCanvasDict[key]))
                            m_canvasDict[m_keyVarCanvasDict[key]].DataDelete(key, ComponentType.Variable);
                        else if (key.EndsWith(":SIZE"))
                        {
                            string[] list = key.Split(new char[] { ':' });
                            if (list.Length > 0 && m_keySysCanvasDict.ContainsKey(list[0]))
                            {
                                if (m_canvasDict.ContainsKey(m_keySysCanvasDict[list[0]]))
                                    m_canvasDict[m_keySysCanvasDict[list[0]]].DataDelete(key, ComponentType.Variable);
                            }
                        }
                        m_keyVarCanvasDict.Remove(key);
                    }
                    break;
                case ComponentType.Process:
                    if (m_keyProCanvasDict.ContainsKey(key))
                    {
                        if (m_canvasDict.ContainsKey(m_keyProCanvasDict[key]))
                            m_canvasDict[m_keyProCanvasDict[key]].DataDelete(key, ComponentType.Process);
                        m_keyProCanvasDict.Remove(key);
                    }
                    break;
            }
        }

        /// <summary>
        /// Delete objects under a given system from m_key****CanvasDict
        /// </summary>
        public void DeleteFromRegistoryUnder(string system)
        {
            if(string.IsNullOrEmpty(system))
                return;

            // Delete variables.
            List<string> deleteList = new List<string>();
            foreach(string delete in m_keyVarCanvasDict.Keys)
                if (delete.StartsWith(system + ":"))
                {
                    deleteList.Add(delete);
                    m_canvasDict[m_keyVarCanvasDict[delete]].DataDelete(delete, ComponentType.Variable);
                }
            foreach(string delete in deleteList)
                m_keyVarCanvasDict.Remove(delete);
            
            // Delete processes.
            deleteList = new List<string>();
            foreach(string delete in m_keyProCanvasDict.Keys)
                if (delete.StartsWith(system + ":"))
                {
                    deleteList.Add(delete);
                    m_canvasDict[m_keyProCanvasDict[delete]].DataDelete(delete, ComponentType.Process);
                }
            foreach(string delete in deleteList)
                m_keyProCanvasDict.Remove(delete);

            // Delete systems.
            deleteList = new List<string>();
            foreach(string delete in m_keySysCanvasDict.Keys)
                if (delete.StartsWith(system + "/"))
                    deleteList.Add(delete);
            foreach(string delete in deleteList)
                m_keySysCanvasDict.Remove(delete);
        }

        /// <summary>
        /// get bitmap image of this pathway.
        /// </summary>
        /// <returns>Bitmap of this pathway.</returns>
        public Bitmap Print()
        {
            if(m_canvasDict != null && m_canvasDict.Count != 0)
            {
                foreach(CanvasView set in m_canvasDict.Values)
                {
                    return set.ToImage();
                }
                return new Bitmap(1, 1);
            }
            else
                return new Bitmap(1, 1);
        }

        /// <summary>
        /// The event sequence on changing selected object at other plugin.
        /// </summary>
        /// <param name="key">Selected the ID.</param>
        /// <param name="type">Selected the data type.</param>
        public void SelectChanged(string key, ComponentType type)
        {
            switch(type)
            {
                case ComponentType.System:
                    if (m_keySysCanvasDict.ContainsKey(key))
                        if (m_canvasDict.ContainsKey(m_keySysCanvasDict[key]))
                            m_canvasDict[ m_keySysCanvasDict[key] ].SelectChanged(key, ComponentType.System);
                    break;
                case ComponentType.Variable:
                    if (m_keyVarCanvasDict.ContainsKey(key))
                        if (m_canvasDict.ContainsKey(m_keyVarCanvasDict[key]))
                            m_canvasDict[m_keyVarCanvasDict[key]].SelectChanged(key, ComponentType.Variable);
                    break;
                case ComponentType.Process:
                    if (m_keyProCanvasDict.ContainsKey(key))
                        if (m_canvasDict.ContainsKey(m_keyProCanvasDict[key]))
                            m_canvasDict[m_keyProCanvasDict[key]].SelectChanged(key, ComponentType.Process);
                    break;
            }
        }

        /// <summary>
        /// Set position of EcellObject.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="eo"></param>
        public void SetPosition(string canvas, EcellObject eo)
        {
            string systemName = PathUtil.GetParentSystemId(eo.key);

            bool isExist = false;

            if(SYSTEM_STRING.Equals(eo.type))
            {
                if (m_canvasDict[canvas].Systems.ContainsKey(eo.key))
                {
                    EcellLib.PathwayWindow.CanvasView.SystemContainer sysCon = m_canvasDict[canvas].Systems[eo.key];
                    eo.X = sysCon.EcellSystems[0].X;
                    eo.Y = sysCon.EcellSystems[0].Y;
                    eo.OffsetX = sysCon.EcellSystems[0].OffsetX;
                    eo.OffsetY = sysCon.EcellSystems[0].OffsetY;
                    eo.Width = sysCon.EcellSystems[0].Width;
                    eo.Height = sysCon.EcellSystems[0].Height;
                    isExist = true;
                }
            }
            else if(VARIABLE_STRING.Equals(eo.type))
            {
                if (m_canvasDict[canvas].Variables.ContainsKey(eo.key))
                {
                    PEcellVariable var = m_canvasDict[canvas].Variables[eo.key];
                    eo.X = var.X;
                    eo.Y = var.Y;
                    eo.OffsetX = var.OffsetX;
                    eo.OffsetY = var.OffsetY;
                    isExist = true;
                }
            }
            else if(PROCESS_STRING.Equals(eo.type))
            {
                if(m_canvasDict[canvas].Processes.ContainsKey(eo.key))
                {
                    PEcellProcess pro = m_canvasDict[canvas].Processes[eo.key];
                    eo.X = pro.X;
                    eo.Y = pro.Y;
                    eo.OffsetX = pro.OffsetX;
                    eo.OffsetY = pro.OffsetY;
                    isExist = true;
                }
            }
            
            /*
            if(!isExist)
            {
                PointF point = m_canvasDict[canvas].GetPosition(systemName);
                eo.X = point.X;
                eo.Y = point.Y;
            }*/
        }
        #endregion

        #region Methods to notify from view to controller (PathwayWindow)
        /// <summary>
        /// get data from DataManager by using the key and the type.
        /// </summary>
        /// <param name="key">the key of object.</param>
        /// <param name="type">the type of object.</param>
        /// <returns>EcellObject.</returns>
        public EcellObject GetEcellObject(string key, string type)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            DataManager dm = DataManager.GetDataManager();
            return dm.GetEcellObject(m_pathwayWindow.ModelID, key, type);
        }

        /// <summary>
        /// Call value of E-cell object.
        /// </summary>
        /// <param name="key">key of EcellObject</param>
        /// <param name="type">type of EcellObject</param>
        /// <param name="name">name of EcellData</param>
        /// <returns></returns>
        public string GetEcellData(string key, string type, string name)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(name))
                return null;

            DataManager dm = DataManager.GetDataManager();
            return dm.GetEcellData(this.m_pathwayWindow.ModelID, key, type, name);
        }

        /// <summary>
        /// Notify DataAdd event to outside.
        /// </summary>
        /// <param name="eo">Added EcellObject</param>
        public void NotifyDataAdd(EcellObject eo)
        {
            List<EcellObject> list = new List<EcellObject>();
            list.Add(eo);
            if (m_pathwayWindow != null)
                m_pathwayWindow.NotifyDataAdd(list);
        }

        /// <summary>
        /// Notify DataChanged event to outside (PathwayView -> PathwayWindow -> DataManager)
        /// </summary>
        /// <param name="oldKey">the key before adding.</param>
        /// <param name="newKey">the key after adding.</param>
        /// <param name="type">type of EcellObject</param>
        /// <param name="x">x coordinate of object.</param>
        /// <param name="y">y coordinate of object.</param>
        /// <param name="offsetx">x offset of object.</param>
        /// <param name="offsety">y offset of object.</param>
        /// <param name="width">width of object.</param>
        /// <param name="height">height of object.</param>
        /// <param name="isExternal">If true, notification will go to PathwayWindow.
        /// If false, notification will not go to PathwayWindow
        /// </param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            string newKey,
            string type,
            float x,
            float y,
            float offsetx,
            float offsety,
            float width,
            float height,
            Boolean isExternal,
            bool isAnchor)
        {
            if(PathwayView.SYSTEM_STRING.Equals(type))
            {
                if(m_keySysCanvasDict.ContainsKey(oldKey))
                {
                    string canvasId = m_keySysCanvasDict[oldKey];
                    m_keySysCanvasDict.Remove(oldKey);
                    m_keySysCanvasDict.Add(newKey, canvasId);
                    if(isExternal)
                        m_pathwayWindow.NotifyDataChanged(oldKey, newKey, type, x, y, offsetx, offsety, width, height, isAnchor);
                }
            }
            else if(PathwayView.VARIABLE_STRING.Equals(type))
            {
                // Update m_keyVarCanvasDict
                if (m_keyVarCanvasDict.ContainsKey(oldKey))
                {
                    string canvasId = m_keyVarCanvasDict[oldKey];
                    m_keyVarCanvasDict.Remove(oldKey);
                    m_keyVarCanvasDict.Add(newKey, canvasId);
                    if(isExternal)
                        m_pathwayWindow.NotifyDataChanged(oldKey, newKey, type, x, y, offsetx, offsety, width, height, isAnchor);
                }
            }
            else if (PathwayView.PROCESS_STRING.Equals(type))
            {
                // Update m_keyProCanvasDict
                if (m_keyProCanvasDict.ContainsKey(oldKey))
                {
                    string canvasId = m_keyProCanvasDict[oldKey];
                    m_keyProCanvasDict.Remove(oldKey);
                    m_keyProCanvasDict.Add(newKey, canvasId);
                    if(isExternal)
                        m_pathwayWindow.NotifyDataChanged(oldKey, newKey, type, x, y, offsetx, offsety, width, height, isAnchor);
                }
            }
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="proKey">key of process</param>
        /// <param name="varKey">key of variable</param>
        /// <param name="changeType">type of change</param>
        /// <param name="coefficient">coefficient of VariableReference</param>
        public void NotifyVariableReferenceChanged(string proKey, string varKey, RefChangeType changeType, int coefficient)
        {
            m_pathwayWindow.NotifyVariableReferenceChanged( proKey, varKey, changeType, coefficient );
        }

        /// <summary>
        /// Notify DataDelete event to outsite.
        /// </summary>
        /// <param name="key">the key of deleted object.</param>
        /// <param name="type">the type of deleted object.</param>
        public void NotifyDataDelete(string key, ComponentType type)
        {
            switch(type)
            {
                case ComponentType.System:
                    if (m_pathwayWindow != null)
                        m_pathwayWindow.NotifyDataDelete(key, SYSTEM_STRING);
                    if (m_keySysCanvasDict.ContainsKey(key))
                        m_keySysCanvasDict.Remove(key);
                    break;
                case ComponentType.Variable:
                    if (m_pathwayWindow != null)
                        m_pathwayWindow.NotifyDataDelete(key, VARIABLE_STRING);
                    if (m_keyVarCanvasDict.ContainsKey(key))
                        m_keyVarCanvasDict.Remove(key);
                    break;
                case ComponentType.Process:
                    if (m_pathwayWindow != null)
                        m_pathwayWindow.NotifyDataDelete(key, PROCESS_STRING);
                    if (m_keyProCanvasDict.ContainsKey(key))
                        m_keyProCanvasDict.Remove(key);
                    break;
            }            
        }

        /// <summary>
        /// Notify DataDelete event to outsite.
        /// </summary>
        /// <param name="key">the key of deleted object.</param>
        /// <param name="type">the type of deleted object.</param>
        public void NotifyDataMerge(string key, ComponentType type)
        {
            switch (type)
            {
                case ComponentType.System:
                    if (m_pathwayWindow != null)
                        m_pathwayWindow.NotifyDataMerge(key, SYSTEM_STRING);
                    if (m_keySysCanvasDict.ContainsKey(key))
                        m_keySysCanvasDict.Remove(key);
                    break;
            }
        }


        /// <summary>
        /// Notify SelectChanged event to outside.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void NotifySelectChanged(string key, string type)
        {
            if (m_pathwayWindow != null)
                m_pathwayWindow.NotifySelectChanged(key, type);
        }
        #endregion

        /// <summary>
        /// Whether this PathwayView contains an object or not.
        /// </summary>
        /// <returns></returns>
        public bool HasObject(ComponentType ct, string key)
        {
            switch(ct)
            {
                case ComponentType.System:
                    if (m_keySysCanvasDict.ContainsKey(key))
                        return true;
                    break;
                case ComponentType.Variable:
                    if (m_keyVarCanvasDict.ContainsKey(key))
                        return true;
                    break;
                case ComponentType.Process:
                    if (m_keyProCanvasDict.ContainsKey(key))
                        return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Freeze all objects to be unpickable.
        /// </summary>
        private void Freeze()
        {
            if (m_isFreezed)
                return;
            if(null != m_canvasDict)
            {
                foreach (CanvasView canvas in m_canvasDict.Values)
                    canvas.Freeze();
            }
            m_isFreezed = true;
        }

        /// <summary>
        /// Cancel freezed status.
        /// </summary>
        private void Unfreeze()
        {
            if (!m_isFreezed)
                return;
            if(null != m_canvasDict)
            {
                foreach (CanvasView canvas in m_canvasDict.Values)
                    canvas.Unfreeze();
            }
            m_isFreezed = false;
        }

        /// <summary>
        /// Move the display rectangle of PathwayEditor with mouse.
        /// </summary>
        /// <param name="direction">direction of moving.</param>
        /// <param name="delta">delta of moving.</param>
        public void PanCanvas(Direction direction, int delta)
        {
            foreach (CanvasView set in m_canvasDict.Values)
            {
                set.PanCanvas(direction, delta);
            }
        }

        /// <summary>
        /// set the list of ComponentSetting.
        /// </summary>
        /// <param name="list">list of ComponentSetting.</param>
        public void SetSettings(List<ComponentSetting> list)
        {
            m_componentSettings = list;
        }

        /// <summary>
        /// Update the overview.
        /// </summary>
        public void UpdateOverview()
        {
            if(m_canvasDict != null)
            {
                foreach(CanvasView set in m_canvasDict.Values)
                {
                    set.UpdateOverview();
                }
            }
        }

        /// <summary>
        /// Clears the contents of the pathway window, and Returns it to an
        /// initial state.
        /// </summary>
        public void Clear()
        {
            if (m_tabControl.SelectedIndex > -1)
            {
                string name = m_tabControl.TabPages[m_tabControl.SelectedIndex].Name;
                m_overviewGB.Controls.Remove(m_canvasDict[name].OverviewCanvas);
            }
            m_tabControl.TabPages.Clear();

            m_keySysCanvasDict = new Dictionary<string, string>();
            m_keyVarCanvasDict = new Dictionary<string, string>();
            m_keyProCanvasDict = new Dictionary<string, string>();
            if(m_canvasDict != null)
            {
                foreach(CanvasView set in m_canvasDict.Values)
                {
                    set.Dispose();
                }
            }
            m_canvasDict = null;
            m_layerDs.Tables.Remove(m_layerDataTable);
            m_layerDs.Dispose();
            m_layerDs = new DataSet();
            m_layerDs.Tables.Add(m_layerDataTable);
            m_dgv.DataSource = m_layerDs;
        }

        /// <summary>
        /// Collect PathwayElement indicated by a key
        /// </summary>
        /// <returns></returns>
        public PathwayElement GetElement(ComponentType ct, string key)
        {
            string canvasName = null;
            switch (ct)
            {
                case ComponentType.System:
                    canvasName = m_keySysCanvasDict[key];
                    break;
                case ComponentType.Variable:
                    canvasName = m_keyVarCanvasDict[key];
                    break;
                case ComponentType.Process:
                    canvasName = m_keyProCanvasDict[key];
                    break;
            }
            return m_canvasDict[canvasName].GetElement(ct, key);
        }

        /// <summary>
        /// Collects all PathwayElements, currently displayed in this pathway window
        /// </summary>
        /// <returns>all PathwayElements on this pathway window</returns>
        public List<PathwayElement> GetElements()
        {
            List<PathwayElement> returnList = new List<PathwayElement>();
            if (m_canvasDict != null)
            {
                foreach (CanvasView set in m_canvasDict.Values)
                {
                    returnList.AddRange(set.GetAllElements());
                }
            }

            return returnList;
        }

        /// <summary>
        /// Get keys of objects under a system.
        /// </summary>
        /// <param name="systemKey"></param>
        /// <returns></returns>
        public List<UniqueKey> GetKeysUnderSystem(string systemKey)
        {
            List<UniqueKey> returnList = new List<UniqueKey>();

            if (null == systemKey)
                return returnList;

            foreach (string key in m_keySysCanvasDict.Keys)            
                if (PathUtil.IsUnder(systemKey, key))
                    returnList.Add(new UniqueKey(ComponentType.System, key));
            
            foreach (string key in m_keyVarCanvasDict.Keys)
                if (PathUtil.IsUnder(systemKey, key))
                    returnList.Add(new UniqueKey(ComponentType.Variable, key));

            foreach (string key in m_keyProCanvasDict.Keys)
                if (PathUtil.IsUnder(systemKey, key))
                    returnList.Add(new UniqueKey(ComponentType.Process, key));
            return returnList;
        }

        /// <summary>
        /// Layout selected nodes on the currently active canvas.
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="commandNum"></param>
        public void LayoutSelected(ILayoutAlgorithm algorithm, int commandNum)
        {
            if (algorithm == null)
                return;

            // No nodes are selected, so exit.
            if (this.ActiveCanvas.SelectedNodes.Count == 0)
            {
                ComponentResourceManager crm = new ComponentResourceManager(typeof(MessageResPathway));
                MessageBox.Show(crm.GetString("MsgLayoutNoNode"),
                                "Layout Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            List<NodeElement> nodeElements = new List<NodeElement>();
            Dictionary<string, PPathwayNode> selectedKeys = new Dictionary<string, PPathwayNode>();
            string parentSystemId = null;

            foreach (PPathwayNode node in this.ActiveCanvas.SelectedNodes)
            {
                foreach (PathwayElement pe in node.GetElements())
                    if (pe is NodeElement)
                    {
                        if (parentSystemId == null)
                            parentSystemId = PathUtil.GetParentSystemId(((NodeElement)pe).Key);
                        selectedKeys.Add(((NodeElement)pe).Key + ":" + ((NodeElement)pe).Type, node);
                        break;
                    }
            }

            foreach (PathwayElement element in this.ActiveCanvas.GetAllElements())
            {
                if (element is NodeElement)
                {
                    NodeElement ne = (NodeElement)element;
                    if(!string.IsNullOrEmpty(parentSystemId) && parentSystemId.Equals(PathUtil.GetParentSystemId(ne.Key)))
                    {
                        string unique = ne.Key + ":" + ne.Type;
                        if (selectedKeys.ContainsKey(unique))
                        {
                            PointF offsetToL = ((PEcellSystem)selectedKeys[unique].ParentObject).OffsetToLayer;
                            ne.X += offsetToL.X;
                            ne.Y += offsetToL.Y;
                            ne.Fixed = false;
                        }
                        else
                            ne.Fixed = true;
                        nodeElements.Add(ne);
                    }
                }
            }

            if (algorithm.DoLayout(commandNum, false, null, nodeElements))
            {
                int total = nodeElements.Count;
                int i = 0;

                foreach (NodeElement ne in nodeElements)
                {
                    i++;
                    string unique = ne.Key + ":" + ne.Type;
                    if (selectedKeys.ContainsKey(unique))
                    {
                        selectedKeys[unique].Element = ne;
                        PointF newPos = new PointF(ne.X, ne.Y);
                        string surSystem = this.ActiveCanvas.GetSurroundingSystem(newPos, null);
                        this.ActiveCanvas.TransferNodeTo(surSystem, selectedKeys[unique], (i == total));
                    }
                }
            }
        }

        /// <summary>
        /// Replace the contents with new PathwayElements.
        /// </summary>
        /// <param name="elements">The contents of the pathway window will be
        /// replaced with these elements</param>
        /// <param name="doLayout">whether nodes should be layouted or not</param>
        /// <returns>Old PathwayElements which was replaced</returns>
        public List<PathwayElement> Replace(List<PathwayElement> elements, bool doLayout)
        {
            // Collect all PathwayElements.
            List<PathwayElement> returnList = GetElements();

            // Clear the pathway window.
            if (m_canvasDict != null && m_canvasDict.Count != 0)
                Clear();

            m_canvasDict = new Dictionary<string, CanvasView>();

            Dictionary<string, CanvasElement> canvasElements = new Dictionary<string, CanvasElement>();
            Dictionary<string, List<LayerElement>> layerElements = new Dictionary<string, List<LayerElement>>();
            Dictionary<string, List<SystemElement>> systemElements = new Dictionary<string, List<SystemElement>>();
            Dictionary<string, List<VariableElement>> variableElements = new Dictionary<string, List<VariableElement>>();
            Dictionary<string, List<ProcessElement>> processElements = new Dictionary<string, List<ProcessElement>>();
            Dictionary<string, List<AttributeElement>> attrElements = new Dictionary<string, List<AttributeElement>>();

            Dictionary<string, AttributeElement> attrDict = new Dictionary<string,AttributeElement>();

            foreach(PathwayElement element in elements)
            {
                switch(element.Element)
                {
                    case PathwayElement.ElementType.Canvas:
                        CanvasElement ce = (CanvasElement)element;
                        canvasElements.Add(ce.CanvasID,ce);
                        break;
                    case PathwayElement.ElementType.Layer:
                        LayerElement le = (LayerElement)element;
                        this.AddLayer(layerElements, le.CanvasID, le);
                        break;
                    case PathwayElement.ElementType.System:
                        SystemElement se = (SystemElement)element;
                        this.AddSystem(systemElements, se.CanvasID, se);
                        m_keySysCanvasDict.Add(se.Key, se.CanvasID);
                        break;
                    case PathwayElement.ElementType.Variable:
                        VariableElement ve = (VariableElement)element;
                        this.AddVariable(variableElements, ve.CanvasID, ve);
                        m_keyVarCanvasDict.Add(ve.Key, ve.CanvasID);
                        break;
                    case PathwayElement.ElementType.Process:
                        ProcessElement pe = (ProcessElement)element;
                        this.AddProcess(processElements, pe.CanvasID, pe);
                        m_keyProCanvasDict.Add(pe.Key, pe.CanvasID);
                        break;
                    case PathwayElement.ElementType.Attribute:
                        AttributeElement ae = (AttributeElement)element;
                        this.AddAttribute(attrElements, ae.CanvasID, ae);
                        m_keyVarCanvasDict.Add(ae.Key, ae.CanvasID);
                        attrDict.Add(ae.Key, ae);
                        break;
                    default:
                        break;
                }
            }
            
            foreach (CanvasElement ce in canvasElements.Values)
            {
                CanvasView set =
                    new CanvasView(this, ce.CanvasID, m_lowerPanelShown, REDUCTION_SCALE, null);

                m_tabControl.Controls.Add(set.TabPage);
                m_activeCanvasID = ce.CanvasID;
                m_canvasDict.Add(ce.CanvasID, set);
                m_layerDs.Tables.Add(set.LayerTable);

                set.UpdateOverview();

                if(doLayout)
                {
                    // Layouted automatically in default layout manner
                    List<SystemElement> layoutSystemElement = null;
                    List<NodeElement> layoutNodeElement = new List<NodeElement>();

                    if (systemElements.ContainsKey(ce.CanvasID))
                        layoutSystemElement = systemElements[ce.CanvasID];
                    if (variableElements.ContainsKey(ce.CanvasID))
                        foreach (VariableElement ve in variableElements[ce.CanvasID])
                            layoutNodeElement.Add(ve);
                    if (processElements.ContainsKey(ce.CanvasID))
                        foreach (ProcessElement pe in processElements[ce.CanvasID])
                            layoutNodeElement.Add(pe);
                    
                    m_pathwayWindow.DefaultLayoutAlgorithm.DoLayout(-1, true, layoutSystemElement, layoutNodeElement);
                }
            }
            
            foreach(List<LayerElement> listLe in layerElements.Values)
                foreach(LayerElement le in listLe)
                    m_canvasDict[le.CanvasID].AddLayer(le.LayerID);
            
            // Setting default contents for overview panel and layer control panel
            foreach (CanvasView set in m_canvasDict.Values)
            {
                m_overviewGB.Controls.Remove(m_overCanvas);
                m_overCanvas = set.OverviewCanvas;
                m_overviewGB.Controls.Add(m_overCanvas);
                m_dgv.DataMember = set.LayerTable.TableName;
                break;
            }
            
            // Locate each PEcellSystem on PCanvas
            foreach (CanvasView set in m_canvasDict.Values)
            {
                if(systemElements.ContainsKey(set.CanvasID))
                {
                    foreach(SystemElement sysEle in systemElements[set.CanvasID])
                    {
                        List<PEcellSystem> systemList = new List<PEcellSystem>();
                        
                        foreach(PLayer layer in this.CanvasDictionary[set.CanvasID].Layers.Values)
                        {
                            PEcellSystem system = this.CreateSystem(sysEle);
                            system.MouseDown += new PInputEventHandler(SystemSelected);
                            systemList.Add(system);
                            system.Layer = layer;
                            system.Name = sysEle.Key;

                            string parentSystemId = PathUtil.GetParentSystemId(system.Element.Key);
                            if (parentSystemId.Equals(""))
                                layer.AddChild(system);
                            else
                                this.CanvasDictionary[set.CanvasID].AddNewObj(null, parentSystemId, system, true, true);
                                //this.CanvasDictionary[set.CanvasID].AddChildToSelectedSystem(parentSystemId, system, true);
                        }
                        if (attrDict.ContainsKey(sysEle.Key + ":SIZE"))
                            set.AddSystem(sysEle.Key, sysEle, attrDict[sysEle.Key + ":SIZE"], systemList);
                        else
                            set.AddSystem(sysEle.Key, sysEle, null, systemList);
                    }
                }

                // Locate each PPathwayNode on PCanvas
                if (variableElements.ContainsKey(set.CanvasID))
                {
                    foreach (VariableElement varEle in variableElements[set.CanvasID])
                    {
                        PPathwayNode pnode = this.CreateVariable(varEle);
                        pnode.MouseDown += new PInputEventHandler(NodeSelected);
                        pnode.MouseEnter += new PInputEventHandler(NodeEntered);
                        pnode.MouseLeave += new PInputEventHandler(NodeLeft);
                        pnode.CanvasView = set;

                        if (pnode == null)
                            continue;
                        set.AddChildToSelectedLayer(varEle.LayerID, varEle.ParentSystemID, pnode);
                    }
                }
                if (processElements.ContainsKey(set.CanvasID))
                {
                    foreach (ProcessElement nodeEle in processElements[set.CanvasID])
                    {
                        PPathwayNode pnode = null;
                        pnode = this.CreateProcess(nodeEle);

                        pnode.MouseDown += new PInputEventHandler(NodeSelected);
                        pnode.MouseEnter += new PInputEventHandler(NodeEntered);
                        pnode.MouseLeave += new PInputEventHandler(NodeLeft);
                        pnode.Handler4Line = new PInputEventHandler(LineSelected);
                        pnode.CanvasView = set;

                        if (pnode == null)
                            continue;
                        set.AddChildToSelectedLayer(nodeEle.LayerID, nodeEle.ParentSystemID, pnode);
                        ((PEcellProcess)pnode).CreateEdges();
                        ((PEcellProcess)pnode).RefreshEdges();
                    }
                }
            }
            return returnList;
        }
        
        #region Methods for internal use
        /// <summary>
        /// add the layer to managed list.
        /// </summary>
        /// <param name="dict">dictionary of canvas.</param>
        /// <param name="canvasId">canvas id.</param>
        /// <param name="le">added layer.</param>
        private void AddLayer(Dictionary<string, List<LayerElement>> dict,
            string canvasId, LayerElement le)
        {
            if (dict.ContainsKey(canvasId))
                dict[canvasId].Add(le);
            else
            {
                List<LayerElement> list = new List<LayerElement>();
                list.Add(le);
                dict.Add(canvasId, list);
            }
        }

        /// <summary>
        /// add the system the managed list.
        /// </summary>
        /// <param name="dict">dictionary of canvas.</param>
        /// <param name="canvasId">canvas id.</param>
        /// <param name="se">the added system.</param>
        private void AddSystem(Dictionary<string, List<SystemElement>> dict, string canvasId, SystemElement se)
        {
            if (dict.ContainsKey(canvasId))
                dict[canvasId].Add(se);
            else
            {
                List<SystemElement> list = new List<SystemElement>();
                list.Add(se);
                dict.Add(canvasId, list);
            }
        }

        /// <summary>
        /// add the variable to managed list.
        /// </summary>
        /// <param name="dict">dictionary of canvas.</param>
        /// <param name="canvasId">canvas id.</param>
        /// <param name="ne">the added variable.</param>
        private void AddVariable(Dictionary<string, List<VariableElement>> dict, string canvasId, VariableElement ne)
        {
            if (dict.ContainsKey(canvasId))
                dict[canvasId].Add(ne);
            else
            {
                List<VariableElement> list = new List<VariableElement>();
                list.Add(ne);
                dict.Add(canvasId, list);
            }
        }

        /// <summary>
        /// add the process to managed list.
        /// </summary>
        /// <param name="dict">the dictionary of canvas.</param>
        /// <param name="canvasId">canvas id.</param>
        /// <param name="ne">the added process.</param>
        private void AddProcess(Dictionary<string, List<ProcessElement>> dict, string canvasId, ProcessElement ne)
        {
            if (dict.ContainsKey(canvasId))
            {
                dict[canvasId].Add(ne);
            }
            else
            {
                List<ProcessElement> list = new List<ProcessElement>();
                list.Add(ne);
                dict.Add(canvasId, list);
            }
        }

        /// <summary>
        /// add the attribute to managed list.
        /// </summary>
        /// <param name="dict">the dictionary of canvas.</param>
        /// <param name="canvasId">canvas id.</param>
        /// <param name="ae">the added attribute.</param>
        private void AddAttribute(Dictionary<string, List<AttributeElement>> dict, string canvasId, AttributeElement ae)
        {
            if (dict.ContainsKey(canvasId))
                dict[canvasId].Add(ae);
            else
            {
                List<AttributeElement> list = new List<AttributeElement>();
                list.Add(ae);
                dict.Add(canvasId, list);
            }
        }

        /// <summary>
        /// create PNode of system using the information of element.
        /// </summary>
        /// <param name="sysEle">element.</param>
        /// <returns>PNode of system(PEcellSystem).</returns>
        private PEcellSystem CreateSystem(SystemElement sysEle)
        {
            if(sysEle == null || m_csManager == null || m_csManager.DefaultSystemSetting == null)
                return null;
            PEcellSystem returnSystem = (PEcellSystem)m_csManager.DefaultSystemSetting.CreateNewComponent(sysEle.X, sysEle.Y, sysEle.Width, sysEle.Height, this);
            returnSystem.OffsetX = sysEle.OffsetX;
            returnSystem.OffsetY = sysEle.OffsetY;
            returnSystem.Element = sysEle;
            returnSystem.Reset();
            return returnSystem;
        }

        /// <summary>
        /// create PNode of process using the information of element.
        /// </summary>
        /// <param name="nodeEle">element.</param>
        /// <returns>PNode of process(PEcellProcess).</returns>
        private PEcellProcess CreateProcess(ProcessElement nodeEle)
        {
            if (nodeEle == null || m_csManager == null | m_csManager.DefaultProcessSetting == null)
                return null;
            PEcellProcess returnProcess = (PEcellProcess)m_csManager.ProcessSettings[nodeEle.CsId].CreateNewComponent(nodeEle.X, nodeEle.Y, 0, 0, this);
            returnProcess.Element = nodeEle;
            return returnProcess;
        }

        /// <summary>
        /// create PNode of variable using the information of element.
        /// </summary>
        /// <param name="nodeEle">element of variable.</param>
        /// <returns>PNode of variable(PEcellVariable)</returns>
        private PEcellVariable CreateVariable(VariableElement nodeEle)
        {
            if (nodeEle == null || m_csManager == null | m_csManager.DefaultVariableSetting == null)
                return null;
            PEcellVariable returnVariable = (PEcellVariable)m_csManager.VariableSettings[nodeEle.CsId].CreateNewComponent(nodeEle.X, nodeEle.Y, 0, 0, this);
            returnVariable.Element = nodeEle;
            return returnVariable;
        }

        /// <summary>
        /// Create edge from PEcellVariable.
        /// </summary>
        public void CreateEdge(PEcellProcess process, PEcellVariable variable, int coefficient)
        {
            CreateEdge(process, variable.Element.Key, coefficient);
        }

        /// <summary>
        /// Create edge from variable key.
        /// </summary>
        public void CreateEdge(PEcellProcess process, string variableKey, int coefficient)
        {
            DataManager dm = DataManager.GetDataManager();
            EcellObject obj = dm.GetEcellObject(process.Element.ModelID, process.Element.Key, process.Element.Type);
            foreach (EcellData d in obj.M_value)
            {
                if (!d.M_name.Equals("VariableReferenceList"))
                    continue;

                List<EcellReference> list =
                    EcellReference.ConvertString(d.M_value.ToString());

                // If this process and variable are connected in the same direction, nothing will be done.
                if (PathUtil.CheckReferenceListContainsEntity(list, variableKey, coefficient))
                {
                    MessageBox.Show(m_resources.GetString("ErrAlrConnect"),
                     "Notice",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Exclamation);
                    return;
                }

                String refStr = "(";
                int i = 0;
                foreach (EcellReference r in list)
                {
                    if (i == 0) refStr = refStr + r.ToString();
                    else refStr = refStr + ", " + r.ToString();
                    i++;
                }

                String n = "";
                String pre = "";
                if (coefficient == 0)
                {
                    pre = "C";
                }
                else if (coefficient == -1)
                {
                    pre = "S";
                }
                else 
                {
                    pre = "P";
                }

                int k = 0;
                while (true)
                {
                    bool ishit = false;
                    n = pre + k;
                    foreach (EcellReference r in list)
                    {
                        if (r.name == n)
                        {
                            k++;
                            ishit = true;
                            continue;
                        }
                    }
                    if (ishit == false) break;
                }

                EcellReference eref = new EcellReference();
                eref.name = n;
                eref.fullID = ":" + variableKey;
                eref.coefficient = coefficient;
                eref.isAccessor = 1;

                if (i == 0) refStr = refStr + eref.ToString();
                else refStr = refStr + ", " + eref.ToString();
                refStr = refStr + ")";

                d.M_value = EcellValue.ToVariableReferenceList(refStr);

                dm.DataChanged(obj.modelID, obj.key, obj.type, obj);
                return;
            }

        }
        /// <summary>
        /// Create edge from variable key.
        /// </summary>
        public void ClearEdges(PEcellProcess process)
        {
            DataManager dm = DataManager.GetDataManager();
            EcellObject obj = dm.GetEcellObject(process.Element.ModelID, process.Element.Key, process.Element.Type);
            foreach (EcellData d in obj.M_value)
            {
                if (!d.M_name.Equals("VariableReferenceList"))
                    continue;

                d.M_value = EcellValue.ToVariableReferenceList("()");

                dm.DataChanged(obj.modelID, obj.key, obj.type, obj);
                return;
            }
        }

        #endregion

        /// <summary>
        /// Called when UserControl is resized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SizeChanged(object sender, EventArgs e)
        {
            if(m_dgv != null && m_dgv.Columns.Contains("Show"))
                m_dgv.Columns["Show"].Width = LAYER_SHOWCOLUMN_WIDTH;
            UpdateOverview();
        }

        #region Event delegate from PPathwayObject
        /// <summary>
        /// the event sequence of selecting the PNode of system in PathwayEditor.
        /// </summary>
        /// <param name="sender">PPathwaySystem</param>
        /// <param name="e">PInputEventArgs</param>
        public void SystemSelected(object sender, PInputEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.PickedNode == sender)
            {
                this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
                this.CanvasDictionary[e.Canvas.Name].AddSelectedSystem(((PPathwayObject)sender).Name);
            }
            else
            {
                this.CanvasDictionary[e.Canvas.Name].ClickedNode = e.PickedNode;
                foreach (String iName in CanvasDictionary[e.Canvas.Name].ContextMenuDict.Keys)
                {
                    if (iName.StartsWith("delete"))
                    {
                        CanvasDictionary[e.Canvas.Name].ContextMenuDict[iName].Tag = e.PickedNode;
                    }
                }
/*
                foreach (ToolStripItem item in this.CanvasDictionary[e.Canvas.Name].NodeMenu.Items)
                {
                    if ("Delete".Equals(item.Text))
                    {
                        item.Tag = e.PickedNode;
                    }
                }
 */
            }
        }

        /// <summary>
        /// the event sequence of selecting the PNode of process or variable in PathwayEditor.
        /// </summary>
        /// <param name="sender">PEcellVariavle of PEcellProcess.</param>
        /// <param name="e">PInputEventArgs.</param>
        public void NodeSelected(object sender, PInputEventArgs e)
        {
            //if (e.PickedNode is PPathwayNode)
            //    ((PPathwayNode)e.PickedNode).ValidateEdges();
            //if (e.Button == MouseButtons.Left)
            //{
                PPathwayNode pnode = (PPathwayNode)e.PickedNode;

                if (SelectedHandle.Mode == Mode.CreateOneWayReaction
                    || SelectedHandle.Mode == Mode.CreateMutualReaction
                    || SelectedHandle.Mode == Mode.CreateConstant)
                {
                    this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
                    this.CanvasDictionary[e.Canvas.Name].AddNodeToBeConnected((PPathwayNode)sender);
                }
                else
                {
                    if (!pnode.IsHighLighted)
                    {
                        if (e.Modifiers == Keys.Shift)
                        {
                            this.CanvasDictionary[e.Canvas.Name].AddSelectedNode((PPathwayNode)sender, true);
                        }
                        else
                        {
                            this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
                            this.CanvasDictionary[e.Canvas.Name].AddSelectedNode((PPathwayNode)sender, true);
                        }
                    }
                }
            //}
            //else
            //{
                this.CanvasDictionary[e.Canvas.Name].ClickedNode = e.PickedNode;
                foreach (String iName in CanvasDictionary[e.Canvas.Name].ContextMenuDict.Keys)
                {
                    if (iName.StartsWith("delete"))
                    {
                        CanvasDictionary[e.Canvas.Name].ContextMenuDict[iName].Tag = e.PickedNode;
                    }
                }
/*                foreach (ToolStripItem item in this.CanvasDictionary[e.Canvas.Name].NodeMenu.Items)
                {
                    if ("Delete".Equals(item.Text))
                    {
                        item.Tag = e.PickedNode;
                    }
                }
 */
            //}
        }

        /// <summary>
        /// Called when the mouse enters a node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NodeEntered(object sender, PInputEventArgs e)
        {
            if (SelectedHandle.Mode == Mode.CreateOneWayReaction ||
                SelectedHandle.Mode == Mode.CreateMutualReaction ||
                SelectedHandle.Mode == Mode.CreateConstant)
            {
                PPathwayNode startNode = m_canvasDict[e.Canvas.Name].NodeToBeReconnected;
                if (null == startNode)
                    return;

                if ((startNode is PEcellProcess && e.PickedNode is PEcellVariable)
                    || (startNode is PEcellVariable && e.PickedNode is PEcellProcess))
                {
                    PPathwayNode endNode = e.PickedNode as PPathwayNode;
                    if (null != endNode)
                        endNode.IsMouseOn = true;
                }
            }
        }

        /// <summary>
        /// Called when the mouse leaves a node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NodeLeft(object sender, PInputEventArgs e)
        {
            if (SelectedHandle.Mode == Mode.CreateOneWayReaction ||
                SelectedHandle.Mode == Mode.CreateMutualReaction ||
                SelectedHandle.Mode == Mode.CreateConstant)
            {
                PPathwayNode endNode = e.PickedNode as PPathwayNode;
                if (null != endNode)
                    endNode.IsMouseOn = false;
            }
        }

        /// <summary>
        /// Called when a line is selected on the PathwayWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LineSelected(object sender, PInputEventArgs e)
        {
            if (!(e.PickedNode is Line))
                return;

            Line line = (Line)e.PickedNode;

            if (e.Button == MouseButtons.Right)
            {
                this.CanvasDictionary[e.Canvas.Name].ClickedNode = e.PickedNode;
            }

            this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
            this.CanvasDictionary[e.Canvas.Name].AddSelectedLine(line);
        }

        /// <summary>
        /// Called when a node moves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NodeMoved(object sender, PInputEventArgs e)
        {
            if (!(e.PickedNode is PPathwayNode))
            {
                return;
            }
        }

        /// <summary>
        /// Called when node is unselected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NodeUnselected(object sender, PInputEventArgs e)
        {
            if (!(e.PickedNode is PPathwayNode))
            {
                return;
            }
        }

        #endregion
    }
}