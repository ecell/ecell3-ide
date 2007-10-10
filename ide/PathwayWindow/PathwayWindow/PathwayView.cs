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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
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
using EcellLib.PathwayWindow.Nodes;
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
        /// Default LayerID
        /// </summary>
        string m_defLayerId = "first";

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
        /// List of PPathwayNode for copied object.
        /// </summary>
        List<EcellObject> m_copiedNodes = new List<EcellObject>();

        /// <summary>
        /// Point of mouse cursor.
        /// </summary>
        PointF m_mousePos;

        /// <summary>
        /// Point of mouse cursor.
        /// </summary>
        PointF m_copyPos;

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
            get
            {
                if (m_activeCanvasID == null)
                    return null;
                else
                    return m_canvasDict[m_activeCanvasID];
            }
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
            get { return m_mousePos; }
            set { m_mousePos = value; }
        }
        /// <summary>
        /// Accessor for m_copiedNodes.
        /// </summary>
        public List<EcellObject> CopiedNodes
        {
            get { return m_copiedNodes; }
            set { m_copiedNodes = value; }
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
            m_tabControl.MouseEnter += new EventHandler(m_tabControl_OnMouseEnter);
            m_tabControl.MouseWheel += new MouseEventHandler(m_tabControl_OnMouseWheel);
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
        /// Create pathway canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateCanvas(string canvasID)
        {
            m_canvasDict = new Dictionary<string, CanvasView>();

            CanvasView canvas = new CanvasView(this, canvasID, REDUCTION_SCALE, null);
            m_activeCanvasID = canvasID;
            m_canvasDict.Add(canvasID, canvas);
            canvas.AddLayer(this.m_defLayerId);

            m_tabControl.Controls.Add(canvas.TabPage);
            m_layerDs.Tables.Clear();
            m_layerDs.Tables.Add(canvas.LayerTable);
            canvas.UpdateOverview();

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
                m_canvasDict[dataMember].OverView.Canvas.Refresh();
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
        /// <param name="eo">EcellObject</param>
        /// <param name="needToNotify">whether notification is needed or not</param>
        /// <param name="isAnchor">True is default. If undo unit contains multiple actions,
        /// only the last action's isAnchor is true, the others' isAnchor is false</param>
        /// <param name="valueStr">String for System label.</param>
        public void AddNewObj(string canvasName,
            string systemName,
            EcellObject eo,
            bool isAnchor)
        {
            ComponentType cType = ComponentSetting.ParseComponentKind(eo.type);
            ComponentSetting cs = GetComponentSetting(cType);
            // Error check.
            if (eo == null)
                throw new PathwayException(m_resources.GetString("ErrAddObjNot"));
            if (eo.key.EndsWith(":SIZE"))
                return;
            if (!RegisterObj(cType, eo.key, canvasName))
                return;
            if (string.IsNullOrEmpty(eo.key))
                throw new PathwayException(m_resources.GetString("ErrKeyNot"));
            if (string.IsNullOrEmpty(canvasName) || !m_canvasDict.ContainsKey(canvasName))
                throw new PathwayException(m_resources.GetString("ErrNotSetCanvas") + eo.key);

            CanvasView canvas = m_canvasDict[canvasName];
            PLayer layer = ActiveCanvas.Layers[this.m_defLayerId];


            if (eo is EcellSystem)
            {
                PPathwaySystem system = (PPathwaySystem)cs.CreateNewComponent(eo, this);
                system.Reset();
                system.MouseDown += new PInputEventHandler(SystemSelected);
                system.Layer = layer;
                system.Name = eo.key;
                canvas.AddNewObj(m_defLayerId, systemName, system, eo.IsPosSet, false);
                //canvas.AddChildToSelectedSystem(parentSystemId, system, hasCoords);
            }
            else
            {
                PPathwayObject obj = cs.CreateNewComponent(eo, this);
                obj.ShowingID = m_showingId;
                obj.MouseDown += new PInputEventHandler(NodeSelected);
                obj.MouseEnter += new PInputEventHandler(NodeEntered);
                obj.MouseLeave += new PInputEventHandler(NodeLeft);
                ((PPathwayNode)obj).Handler4Line = new PInputEventHandler(LineSelected);
                canvas.AddNewObj(m_defLayerId, systemName, obj, eo.IsPosSet, false);
            }
        }

        /// <summary>
        /// Get ComponentSetting.
        /// </summary>
        /// <param name="cType">ComponentType</param>
        private ComponentSetting GetComponentSetting(ComponentType cType)
        {
            switch (cType)
            {
                case ComponentType.Process:
                    return ComponentSettingsManager.DefaultProcessSetting;
                case ComponentType.System:
                    return ComponentSettingsManager.DefaultSystemSetting;
                case ComponentType.Variable:
                    return ComponentSettingsManager.DefaultVariableSetting;
            }
            return null;
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
                m_overCanvas = m_canvasDict[name].OverView.Canvas;
                m_overviewGB.Controls.Add(m_overCanvas);

            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">TabControl.</param>
        /// <param name="e">MouseEventArgs.</param>
        void m_tabControl_OnMouseWheel(object sender, MouseEventArgs e)
        {

            if (Control.ModifierKeys == Keys.Shift)
            {
                this.PanCanvas(Direction.Horizontal, e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Control || e.Button == MouseButtons.Right)
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
        void m_tabControl_OnMouseEnter(object sender, EventArgs e)
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
            if (this.CanvasDictionary == null)
                return;

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
                    if (data.key != null && !key.Equals(data.key))
                    {
                        string canvasId = m_keySysCanvasDict[key];
                        m_keySysCanvasDict.Remove(key);
                        m_keySysCanvasDict.Add(data.key, canvasId);
                    }
                    m_canvasDict[m_keySysCanvasDict[data.key]].DataChanged(key, data, type);
                    break;
                case ComponentType.Variable:
                    if (!m_keyVarCanvasDict.ContainsKey(key))
                        return;
                    if(data.key != null && !key.Equals(data.key))
                    {
                        string canvasId = m_keyVarCanvasDict[key];
                        m_keyVarCanvasDict.Remove(key);
                        m_keyVarCanvasDict.Add(data.key, canvasId);
                    }
                    m_canvasDict[m_keyVarCanvasDict[data.key]].DataChanged(key, data, type);
                    break;
                case ComponentType.Process:
                    if (!m_keyProCanvasDict.ContainsKey(key))
                        return;
                    if(data.key != null && !key.Equals(data.key))
                    {
                        string canvasId = m_keyProCanvasDict[key];
                        m_keyProCanvasDict.Remove(key);
                        m_keyProCanvasDict.Add(data.key, canvasId);
                    }
                    m_canvasDict[m_keyProCanvasDict[data.key]].DataChanged(key, data, type);
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
            ICollection<string> keyList = CopyList(m_keyVarCanvasDict.Keys);
            foreach (string key in keyList)
                if (PathUtil.IsUnder(system, key))
                {
                    ActiveCanvas.DataDelete(key, ComponentType.Variable);
                    m_keyVarCanvasDict.Remove(key);
                }
            
            // Delete processes.
            keyList = CopyList(m_keyProCanvasDict.Keys);
            foreach (string key in keyList)
                if (PathUtil.IsUnder(system, key))
                {
                    ActiveCanvas.DataDelete(key, ComponentType.Process);
                    m_keyProCanvasDict.Remove(key);
                }

            // Delete systems.
            keyList = CopyList(m_keySysCanvasDict.Keys);
            foreach (string key in keyList)
                if (PathUtil.IsUnder(system, key))
                    m_keySysCanvasDict.Remove(key);
        }

        private List<string> CopyList(ICollection<string> list)
        {
            string[] tmpArray = new string[list.Count];
            list.CopyTo(tmpArray, 0);
            List<string> newList = new List<string>(tmpArray);
            return newList;
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

            if(SYSTEM_STRING.Equals(eo.type))
                if (m_canvasDict[canvas].Systems.ContainsKey(eo.key))
                    m_canvasDict[canvas].Systems[eo.key].EcellObject = (EcellSystem)eo;

            else if(VARIABLE_STRING.Equals(eo.type))
                if (m_canvasDict[canvas].Variables.ContainsKey(eo.key))
                    m_canvasDict[canvas].Variables[eo.key].EcellObject = (EcellVariable)eo;

            else if(PROCESS_STRING.Equals(eo.type))
                if(m_canvasDict[canvas].Processes.ContainsKey(eo.key))
                    m_canvasDict[canvas].Processes[eo.key].EcellObject = (EcellProcess)eo;
            
        }
        #endregion

        #region Methods to notify from view to controller (PathwayWindow)
        /// <summary>
        /// Notify DataAdd event to outside.
        /// </summary>
        /// <param name="eo">Added EcellObject</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void NotifyDataAdd(EcellObject eo, bool isAnchor)
        {
            List<EcellObject> list = new List<EcellObject>();
            list.Add(eo);
            if (m_pathwayWindow != null)
                m_pathwayWindow.NotifyDataAdd(list, isAnchor);
        }

        /// <summary>
        /// Notify DataChanged event to outside (PathwayView -> PathwayWindow -> DataManager)
        /// To notify position or size change.
        /// </summary>
        /// <param name="oldKey">the key before adding.</param>
        /// <param name="newKey">the key after adding.</param>
        /// <param name="obj">x coordinate of object.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            string newKey,
            EcellObject eo,
            bool isAnchor)
        {
            m_pathwayWindow.NotifyDataChanged(oldKey, newKey, eo, isAnchor);
        }

        /// <summary>
        /// Notify DataChanged event to outside (PathwayView -> PathwayWindow -> DataManager)
        /// To notify position or size change.
        /// </summary>
        /// <param name="oldKey">the key before adding.</param>
        /// <param name="newKey">the key after adding.</param>
        /// <param name="obj">x coordinate of object.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            string newKey,
            PPathwayObject obj,
            bool isAnchor)
        {
            Dictionary<string, string> dic;
            if (PathwayView.SYSTEM_STRING.Equals(obj.EcellObject.type))
                dic = m_keySysCanvasDict;
            else if (PathwayView.VARIABLE_STRING.Equals(obj.EcellObject.type))
                dic = m_keyVarCanvasDict;
            else if (PathwayView.PROCESS_STRING.Equals(obj.EcellObject.type))
                dic = m_keyProCanvasDict;
            else
                return;
            if (!dic.ContainsKey(oldKey))
                return;
            string canvasId = dic[oldKey];
            dic.Remove(oldKey);
            dic.Add(newKey, canvasId);

            m_pathwayWindow.NotifyDataChanged(oldKey, newKey, obj.EcellObject, isAnchor);
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
        /// <param name="isAnchor">the type of deleted object.</param>
        public void NotifyDataDelete(string key, ComponentType type, bool isAnchor)
        {
            switch(type)
            {
                case ComponentType.System:
                    if (m_pathwayWindow != null)
                        m_pathwayWindow.NotifyDataDelete(key, SYSTEM_STRING, isAnchor);
                    if (m_keySysCanvasDict.ContainsKey(key))
                        m_keySysCanvasDict.Remove(key);
                    break;
                case ComponentType.Variable:
                    if (m_pathwayWindow != null)
                        m_pathwayWindow.NotifyDataDelete(key, VARIABLE_STRING, isAnchor);
                    if (m_keyVarCanvasDict.ContainsKey(key))
                        m_keyVarCanvasDict.Remove(key);
                    break;
                case ComponentType.Process:
                    if (m_pathwayWindow != null)
                        m_pathwayWindow.NotifyDataDelete(key, PROCESS_STRING, isAnchor);
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
                m_overviewGB.Controls.Remove(m_canvasDict[name].OverView);
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
            if(m_dgv.Visible)
            {
                m_layerDs.Tables.Clear();
                m_layerDs.Dispose();
                m_layerDs = new DataSet();
                m_layerDs.Tables.Add(m_layerDataTable);
                m_dgv.DataSource = m_layerDs;
            }
        }

        #region EventHandler
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

                if ((startNode is PPathwayProcess && e.PickedNode is PPathwayVariable)
                    || (startNode is PPathwayVariable && e.PickedNode is PPathwayProcess))
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

        /// <summary>
        /// Called when a copy menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyClick(object sender, EventArgs e)
        {
            if (this.ActiveCanvas == null)
                return;
            this.CopiedNodes.Clear();
            this.m_copyPos = this.MousePosition;

            this.CopiedNodes = SetCopyNodes(this.ActiveCanvas.SelectedNodes);
        }

        /// <summary>
        /// Called when a cut menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CutClick(object sender, EventArgs e)
        {
            if (this.ActiveCanvas == null)
                return;
            this.CopiedNodes.Clear();
            this.m_copyPos = this.MousePosition;

            this.CopiedNodes = SetCopyNodes(this.ActiveCanvas.SelectedNodes);

            int i = 0;
            bool isAnchor;
            foreach (EcellObject eo in this.CopiedNodes)
            {
                i++;
                isAnchor = (i == this.CopiedNodes.Count); 
                this.NotifyDataDelete(eo.key, ComponentSetting.ParseComponentKind(eo.type), isAnchor);
            }

        }

        /// <summary>
        /// Called when a paste menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PasteClick(object sender, EventArgs e)
        {
            if (this.CopiedNodes == null)
                return;
            // Get position diff
            PointF diff = GetDiff(this.MousePosition, this.m_copyPos);

            List<EcellObject> nodeList = CopyNodes(this.CopiedNodes);
            int i = 0;
            bool isAnchor;
            foreach (EcellObject eo in nodeList)
            {
                i++;
                isAnchor = (i == this.CopiedNodes.Count); 
                this.NotifyDataAdd(eo, isAnchor);
            }
        }

        private List<EcellObject> SetCopyNodes(List<EcellObject> nodeList)
        {
            List<EcellObject> copyNodes = new List<EcellObject>();
            //Copy Variavles
            foreach (EcellObject node in nodeList)
                if (node is EcellVariable)
                    copyNodes.Add(node.Copy());
            //Copy Processes
            foreach (EcellObject node in nodeList)
                if (node is EcellProcess)
                    copyNodes.Add(node.Copy());
            return copyNodes;
        }

        private List<EcellObject> CopyNodes(List<EcellObject> nodeList)
        {
            List<EcellObject> copiedNodes = new List<EcellObject>();
            Dictionary<string, string> varKeys = new Dictionary<string, string>();
            // Get position diff
            PointF diff = GetDiff(this.MousePosition, this.m_copyPos);
            // Get parent System
            EcellObject sys = this.ActiveCanvas.GetSurroundingSystem(this.MousePosition);
            DataManager m_dManager = DataManager.GetDataManager();
            // Set m_copiedNodes.
            if (nodeList != null)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    EcellObject node = nodeList[i];
                    //Create new EcellObject
                    EcellObject eo = node.Copy();
                    if (m_dManager.IsDataExists(eo.modelID, eo.key, eo.type))
                        eo.key = m_dManager.GetTemporaryID(eo.modelID, eo.type, sys.key);
                    eo.SetPosition( eo.X + diff.X, eo.Y + diff.Y);

                    // Check Position
                    if (!ActiveCanvas.CheckNodePosition(eo) )
                        eo.PointF = ActiveCanvas.GetVacantPoint(sys.key, eo.PointF);

                    copiedNodes.Add(eo);
                    // Set Variable name.
                    varKeys.Add(":" + node.key, ":" + eo.key);
                }
            }
            // Reset edges.
            foreach (EcellObject eo in copiedNodes)
            {
                if (eo.type == "Process")
                {
                    eo.GetEcellData(EcellProcess.VARIABLEREFERENCELIST).M_value = eo.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST).Copy();
                    foreach (EcellValue edge in eo.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST).CastToList())
                    {
                        foreach (EcellValue val in edge.CastToList())
                            if (varKeys.ContainsKey(val.ToString()))
                                val.M_value = varKeys[val.ToString()];
                    }
                }
            }
            return copiedNodes;
        }

        private PointF GetDiff(PointF posA, PointF posB)
        {
            PointF diff = new PointF(posA.X - posB.X, posA.Y - posB.Y);
            if (diff.X == 0 && diff.Y == 0)
            {
                diff.X = diff.X + 10;
                diff.Y = diff.Y + 10;
            }
            return diff;
        }

        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteClick(object sender, EventArgs e)
        {
            // Delete Selected Line
            Line line = (Line)ActiveCanvas.SelectedLine;
            if (line != null)
            {
                NotifyVariableReferenceChanged(
                    line.Info.ProcessKey,
                    line.Info.VariableKey,
                    RefChangeType.Delete,
                    0);
                ActiveCanvas.ResetSelectedLine();
            }
            // Delete Selected Nodes
            if (this.ActiveCanvas.SelectedNodes != null)
            {
                List<EcellObject> slist = new List<EcellObject>();
                foreach (EcellObject t in this.ActiveCanvas.SelectedNodes)
                    slist.Add(t);

                int i = 0;
                foreach (EcellObject deleteNode in slist)
                {
                    i++;
                    bool isAnchor = (i == slist.Count);
                    try
                    {
                        NotifyDataDelete(deleteNode.key,
                                         ComponentSetting.ParseComponentKind(deleteNode.type),
                                         isAnchor);
                    }
                    catch (IgnoreException)
                    {
                        return;
                    }
                }
            }
            // Delete Selected System
            if (ActiveCanvas.SelectedSystemName != null)
            {
                PPathwaySystem sys = (PPathwaySystem)ActiveCanvas.Systems[ActiveCanvas.SelectedSystemName];
                // Return if sys is null or root sys.
                if (string.IsNullOrEmpty(sys.Name))
                    return;
                if (sys.Name.Equals("/"))
                {
                    MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                // Delete sys.
                try
                {
                    NotifyDataDelete(sys.EcellObject.key, ComponentType.System, true);
                }
                catch (IgnoreException)
                {
                    return;
                }
                ActiveCanvas.ResetSelectedSystem();
            }
        }
        #endregion
    }
}