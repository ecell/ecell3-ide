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
using EllLib.PathwayWindow;
using PathwayWindow;
using PathwayWindow.UIComponent;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// PathwayView plays a role of View part of MVC-model.
    /// </summary>
    public class PathwayView
    {
        #region Enumeration
        /// <summary>
        /// Direction of scrolling the canvas.
        /// </summary>
        public enum Direction { Vertical, Horizontal };
        #endregion

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

        /// <summary>
        /// A list of handlers for creating objects.
        /// </summary>
        List<PBasicInputEventHandler> m_objectHandlerList = new List<PBasicInputEventHandler>();

        /// <summary>
        /// A list of handlers for a canvas.
        /// </summary>
        List<PBasicInputEventHandler> m_canvasHandlerList = new List<PBasicInputEventHandler>();

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
        private int m_checkedComponent;

        /// <summary>
        /// Every time when m_dgv.CurrentCellDirtyStateChanged event occurs, 
        /// m_dgv_CurrentCellDirtyStateChanged delegate will be called twice.
        /// This flag is used for neglecting one of two delagate calling.
        /// </summary>
        private bool m_dirtyEventProcessed = false;
        #endregion

        #region Accessors
        /// <summary>
        ///  get/set Dctionary of CanvasViewComponentSet.
        /// </summary>
        public Dictionary<string, CanvasView> CanvasDictionary
        {
            get { return m_canvasDict; }
        }

        public CanvasView ActiveCanvas
        {
            get { return m_canvasDict[m_activeCanvasID]; }
        }

        /// <summary>
        /// get/set the number of checked component.
        /// </summary>
        public int CheckedComponent
        {
            get { return m_checkedComponent; }
            set { m_checkedComponent = value; }
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
        /// get the list of string for canvas of Process.
        /// </summary>
        public Dictionary<string, string> KeyProcessCanvas
        {
            get { return this.m_keyProCanvasDict; }
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

            ContextMenuStrip nodeMenu = new ContextMenuStrip();
            ToolStripItem delete = new ToolStripMenuItem("Delete");
            delete.Click += new EventHandler(delete_Click);
            nodeMenu.Items.AddRange(new ToolStripItem[]
                                    {
                                        delete
                                    });
            m_tabControl.ContextMenuStrip = nodeMenu;

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

            GroupBox layerGB = new GroupBox();
            layerGB.Dock = DockStyle.Fill;
            layerGB.Text = "Layer";
            layerGB.Controls.Add(m_dgv);

            lowerSplitCon.Panel2.Controls.Add(layerGB);

            // Preparing handler list.
            m_canvasHandlerList.Add( new DefaultMouseHandler(this));
            m_canvasHandlerList.Add( new PPanEventHandler() );
            m_canvasHandlerList.Add(new CreateReactionMouseHandler(this));
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
        //  you must edit this function.
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

        public void AddNewObj(string canvasName,
                              string systemName,
                              ComponentType cType,
                              ComponentSetting cs,
                              string key,
                              bool hasCoords,
                              float x,
                              float y,
                              float width,
                              float height,
                              bool needToNotify,
                              EcellObject eo)
        {
            if (string.IsNullOrEmpty(key))
                throw new PathwayException("key is not set!");

            RegisterObj(cType, key, canvasName);
            if (needToNotify)
            {
                if (eo == null)
                    throw new PathwayException("If you want to notify the DataManager of a new object, you must set eo argument of AddNewObj");

                NotifyDataAdd(eo);
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
                        if(!needToNotify)
                            ((AttributeElement)element).Value = m_pathwayWindow.GetEcellData(key, type, "Value");
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
                    if(!needToNotify)
                        ((ProcessElement)element).SetEdgesByStr(m_pathwayWindow.GetEcellData(key, type, "VariableReferenceList"));
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
                        ((SystemElement)element).Width = PEcellSystem.MIN_X_LENGTH;
                        ((SystemElement)element).Height = PEcellSystem.MIN_Y_LENGTH;
                    }

                    break;
            }

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
                    canvas.AddSystem(key, se, null, systemList);
                }
                else
                {
                    PPathwayObject obj = cs.CreateNewComponent(x, y, 0, 0, this);
                    ((PPathwayNode)obj).Element = (NodeElement)element;
                    ((PPathwayNode)obj).ShowingID = m_showingId;
                    obj.MouseDown += new PInputEventHandler(NodeSelected);
                    ((PPathwayNode)obj).Handler4Line = new PInputEventHandler(LineSelected);

                    string layer = null;
                    if (m_dgv.SelectedRows.Count != 0)
                        layer = (string)m_dgv[m_dgv.Columns["Name"].Index, m_dgv.SelectedRows[0].Index].Value;
                    m_canvasDict[canvasName].AddNewObj(layer, systemName, obj, hasCoords, false);
                }
            }
            else
                throw new PathwayException("CanvasID isn't set for " + key);
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
                        throw new DuplicateKeyException("Variable:" + key + " has already existed");
                    else
                    {
                        m_keyVarCanvasDict.Add(key, canvasName);
                        return true;
                    }

                case ComponentType.Process:
                    if (m_keyProCanvasDict.ContainsKey(key))
                        throw new DuplicateKeyException("Process:" + key + " has already existed");
                    else
                    {
                        m_keyProCanvasDict.Add(key, canvasName);
                        return true;
                    }

                case ComponentType.System:
                    if (m_keySysCanvasDict.ContainsKey(key))
                        throw new DuplicateKeyException("System:" + key + " has already existed");
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
        /// system change the listener for event/
        /// </summary>
        /// <param name="sender">ToolBoxMenuButton.</param>
        /// <param name="e">EventArgs.</param>
        public void ButtonStateChanged(object sender, EventArgs e)
        {            
            if (CheckedComponent >= 0)
                RemoveInputEventListener(m_objectHandlerList[CheckedComponent]);
            else if (CheckedComponent == -1)
                RemoveInputEventListener(m_canvasHandlerList[0]);
            else if (CheckedComponent == -2)
                RemoveInputEventListener(m_canvasHandlerList[1]);
            else if (CheckedComponent == -3)
                RemoveInputEventListener(m_canvasHandlerList[2]);
            else if (CheckedComponent == -4)
                RemoveInputEventListener(m_canvasHandlerList[2]);
            
            CheckedComponent = (int)((ToolStripButton)sender).Tag;
            
            foreach (ToolStripButton button in m_buttonList)
            {
                if ((int)button.Tag != CheckedComponent)
                {
                    button.Checked = false;
                }
            }

            m_splitCon.Cursor = Cursors.Arrow;
            if (CheckedComponent >= 0)
            {
                AddInputEventListener(m_objectHandlerList[CheckedComponent]);
                SetRefreshOverview(false);
            }
            else if (CheckedComponent == -1)
            {
                AddInputEventListener(m_canvasHandlerList[0]);
                SetRefreshOverview(false);
            }
            else if (CheckedComponent == -2)
            {
                m_splitCon.Cursor = Cursors.Hand;
                AddInputEventListener(m_canvasHandlerList[1]);
                SetRefreshOverview(true);
            }
            else if (CheckedComponent == -3)
            {
                AddInputEventListener(m_canvasHandlerList[2]);
                SetRefreshOverview(false);
            }
            else if (CheckedComponent == -4)
            {
                AddInputEventListener(m_canvasHandlerList[2]);
                SetRefreshOverview(false);
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
                m_overCanvas = m_canvasDict[name].OverviewCanvas;
                m_overviewGB.Controls.Add(m_overCanvas);

                m_canvasDict[name].UpdateShowButton(m_lowerPanelShown);
            }
        }

        /// <summary>
        /// When binding data in DataGridView,
        /// ...
        /// </summary>
        /// <param name="sender">DataGridView.</param>
        /// <param name="e">DataGridViewBindingComplete.</param>
        void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (((DataGridView)sender).Columns.Contains("Show"))
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
            UpdateShowButton();
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

            CheckedComponent = 0;

            ToolStripButton handButton = new ToolStripButton();
            handButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            handButton.Name = "SelectMode";
            handButton.Image = Resource1.hand;
            handButton.Text = "";
            handButton.CheckOnClick = true;
            handButton.ToolTipText = "MoveCanvas";
            handButton.Tag = -2;
            handButton.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(handButton);
            m_buttonList.Add(handButton);

            ToolStripButton button0 = new ToolStripButton();
            button0.ImageTransparentColor = System.Drawing.Color.Magenta;
            button0.Name = "SelectMode";
            button0.Image = Resource1.arrow;
            /*
            Graphics gra0 = Graphics.FromImage(button0.Image);
            Point[] arrowPoints = new Point[] { new Point(10,10),
                                               new Point(180,500),
                                               new Point(250,300),
                                               new Point(550,600),
                                               new Point(620,570),
                                               new Point(320,270),
                                               new Point(480,200),
                                               new Point(10,10)};
            gra0.FillPolygon(Brushes.White, arrowPoints);
            gra0.DrawLines(new Pen(Brushes.Black, 48), arrowPoints);*/
            //button0.Size = new System.Drawing.Size(640, 640);
            button0.Text = "";
            button0.CheckOnClick = true;
            button0.ToolTipText = "SelectMode";
            button0.Tag = -1;
            button0.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(button0);
            m_buttonList.Add(button0);

            ToolStripSeparator sep = new ToolStripSeparator();
            sep.Tag = 7;
            list.Add(sep);

            ToolStripButton arrowButton = new ToolStripButton();
            arrowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            arrowButton.Name = "reaction";
            arrowButton.Image = Resource1.arrow_long_right_w;
            arrowButton.Text = "";
            arrowButton.CheckOnClick = true;
            arrowButton.ToolTipText = "Add Reaction";
            arrowButton.Tag = -3;
            arrowButton.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(arrowButton);
            m_buttonList.Add(arrowButton);

            ToolStripButton constButton = new ToolStripButton();
            constButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            constButton.Name = "constant";
            constButton.Image = Resource1.ten;
            constButton.Text = "";
            constButton.CheckOnClick = true;
            constButton.ToolTipText = "Add Constant";
            constButton.Tag = -4;
            constButton.Click += new EventHandler(this.ButtonStateChanged);
            list.Add(constButton);
            m_buttonList.Add(constButton);


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
                    m_objectHandlerList.Add(new CreateSystemMouseHandler(this));
                }
                else
                {
                    GraphicsPath gp = cs.TransformedPath;
                    gra.FillPath(cs.NormalBrush, gp);
                    gra.DrawPath(new Pen(Brushes.Black, 16), gp);
                    m_objectHandlerList.Add(new CreateNodeMouseHandler(this));
                }
                button.Size = new System.Drawing.Size(256, 256);
                button.Text = "";
                button.CheckOnClick = true;
                button.ToolTipText = cs.Name;
                button.Tag = count++;
                button.Click += new EventHandler(this.ButtonStateChanged);
                list.Add(button);
                m_buttonList.Add(button);
            }

            return list;
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
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
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>        
        public void DataDelete(string key, ComponentType type)
        {
            switch(type)
            {
                case ComponentType.System:
                    if (m_keySysCanvasDict.ContainsKey(key))
                    {
                        if (m_canvasDict.ContainsKey(m_keySysCanvasDict[key]))
                            m_canvasDict[m_keySysCanvasDict[key]].DataDelete(key, ComponentType.System);
                        m_keySysCanvasDict.Remove(key);
                    }
                    this.DeleteFromRegistoryUnder(key);
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
                if(delete.StartsWith(system))
                    deleteList.Add(delete);
            foreach(string delete in deleteList)
                m_keyVarCanvasDict.Remove(delete);

            // Delete processes.
            deleteList = new List<string>();
            foreach(string delete in m_keyProCanvasDict.Keys)
                if (delete.StartsWith(system))
                    deleteList.Add(delete);
            foreach(string delete in deleteList)
                m_keyProCanvasDict.Remove(delete);

            // Delete systems.
            deleteList = new List<string>();
            foreach (string delete in m_keySysCanvasDict.Keys)
                if (delete.StartsWith(system))
                    deleteList.Add(delete);
            foreach (string delete in deleteList)
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
        /// <param name="modelID">Selected the model ID.</param>
        /// <param name="key">Selected the ID.</param>
        /// <param name="key">Selected the data type.</param>
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
        #endregion

        #region Methods to notify from view to controller (PathwayWindow)
        /// <summary>
        /// get data from DataManager by using the key and the type.
        /// </summary>
        /// <param name="key">the key of object.</param>
        /// <param name="type">the type of object.</param>
        /// <returns>EcellObject.</returns>
        public EcellObject GetData(string key, string type)
        {
            return m_pathwayWindow.GetData(key, type);
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
        /// Notify DataAdd event to outside.
        /// </summary>
        /// <param name="canvasName">Canvas name exsited object.</param>
        /// <param name="dict">added objects.</param>
        /*
        public void NotifyDataAdd(string canvasName, Dictionary<string, EcellObject> dict)
        {
            List<EcellObject> list = new List<EcellObject>();
            foreach(KeyValuePair<string, EcellObject> pair in dict)
            {
                if(pair.Value.type.Equals(SYSTEM_STRING))
                {
                    if (m_keySysCanvasDict.ContainsKey(pair.Key))
                    {
                        throw new DuplicateKeyException("System:" + pair.key);
                    }
                    else
                    {
                        m_keySysCanvasDict.Add(pair.Key,canvasName);
                        list.Add(pair.Value);
                    }
                }
                else if(pair.Value.type.Equals(VARIABLE_STRING))
                    {
                    if (m_keyVarCanvasDict.ContainsKey(pair.Key))
                    {
                        throw new DuplicateKeyException();
                    }
                    else
                    {
                        m_keyVarCanvasDict.Add(pair.Key,canvasName);
                        list.Add(pair.Value);
                    }
                }
                else if(pair.Value.type.Equals(PROCESS_STRING))
                {
                    if (m_keySysCanvasDict.ContainsKey(pair.Key))
                    {
                        throw new DuplicateKeyException();
                    }
                    else
                    {
                        m_keyProCanvasDict.Add(pair.Key, canvasName);
                        list.Add(pair.Value);
                    }
                }
            }
            if (m_pathwayWindow != null)
                m_pathwayWindow.NotifyDataAdd(list);
        }*/

        /// <summary>
        /// Notify DataChanged event to outside (PathwayView -> PathwayWindow -> DataManager)
        /// </summary>
        /// <param name="oldKey">the key before adding.</param>
        /// <param name="newKey">the key after adding.</param>
        /// <param name="type">type of EcellObject</param>
        /// <param name="isExternal">If true, notification will go to PathwayWindow.
        /// If false, notification will not go to PathwayWindow
        /// </param>
        public void NotifyDataChanged(string oldKey, string newKey, string type, Boolean isExternal)
        {
            if(PathwayView.SYSTEM_STRING.Equals(type))
            {
                if(m_keySysCanvasDict.ContainsKey(oldKey))
                {
                    string canvasId = m_keySysCanvasDict[oldKey];
                    m_keySysCanvasDict.Remove(oldKey);
                    m_keySysCanvasDict.Add(newKey, canvasId);
                    if(isExternal)
                        m_pathwayWindow.NotifyDataChanged(oldKey, newKey, type);
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
                        m_pathwayWindow.NotifyDataChanged(oldKey, newKey, type);
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
                        m_pathwayWindow.NotifyDataChanged(oldKey, newKey, type);
                }
            }
        }

        /// <summary>
        /// Inform the changing of EcellObject in PathwayEditor to DataManager.
        /// </summary>
        /// <param name="proKey"></param>
        /// <param name="varKey"></param>
        /// <param name="coefficient"></param>
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
        /// Move the display rectangle of PathwayEditor with mouse.
        /// </summary>
        /// <param name="direction">direction of moving.</param>
        /// <param name="delta">delta of moving.</param>
        public void PanCanvas(PathwayView.Direction direction, int delta)
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
        /// Update the checkbox in DataGridView.
        /// </summary>
        public void UpdateShowButton()
        {
            if (m_canvasDict != null)
            {
                foreach (CanvasView set in m_canvasDict.Values)
                {
                    set.UpdateShowButton(m_lowerPanelShown);
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
                MessageBox.Show("Please select at least one node for using " + algorithm.GetName() + " layout",
                                "Layout Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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
                foreach (NodeElement ne in nodeElements)
                {
                    string unique = ne.Key + ":" + ne.Type;
                    if (selectedKeys.ContainsKey(unique))
                    {
                        selectedKeys[unique].Element = ne;
                        PointF newPos = new PointF(ne.X, ne.Y);
                        string surSystem = this.ActiveCanvas.GetSurroundingSystem(newPos, null);
                        this.ActiveCanvas.TransferNodeTo(surSystem, selectedKeys[unique]);
                    }
                }
            }
        }

        /// <summary>
        /// Replace the contents with new PathwayElements.
        /// </summary>
        /// <param name="elements">The contents of the pathway window will be
        /// replaced with these elements</param>
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
                    new CanvasView(this, ce.CanvasID, m_lowerPanelShown, REDUCTION_SCALE, ShowButtonClick);

                m_tabControl.Controls.Add(set.TabPage);
                m_activeCanvasID = ce.CanvasID;
                m_canvasDict.Add(ce.CanvasID, set);
                m_layerDs.Tables.Add(set.LayerTable);

                set.UpdateOverview();
                set.UpdateShowButton(m_lowerPanelShown);

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
        #endregion

        /// <summary>
        /// the event sequence of click the check box of show or no show.
        /// </summary>
        /// <param name="sender">DataGridViewCheckBox.</param>
        /// <param name="e">PInputEventArgs.</param>
        void ShowButtonClick(object sender, PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.Default;
            m_lowerPanelShown = !m_lowerPanelShown;

            if(m_lowerPanelShown)
            {
                m_splitCon.Panel2Collapsed = false;
            }
            else
            {
                m_splitCon.Panel2Collapsed = true;
            }
            foreach (CanvasView set in m_canvasDict.Values)
            {
                set.ChangeShowButton(m_lowerPanelShown);
            }
        }

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
            UpdateShowButton();
        }

        #region Event delegate from PPathwayObject
        /// <summary>
        /// the event sequence of selecting the PNode of system in PathwayEditor.
        /// </summary>
        /// <param name="sender">PPathwaySystem.</param>
        /// <param name="e">PInputEventArgs.</param>
        public void SystemSelected(object sender, PInputEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.PickedNode == sender)
            {
                this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
                this.CanvasDictionary[e.Canvas.Name].AddSelectedSystem(((PPathwayObject)sender).Name);
            }
            else
            {
                this.CanvasDictionary[e.Canvas.Name].NodeMenu.Tag = e.PickedNode;
                foreach (ToolStripItem item in this.CanvasDictionary[e.Canvas.Name].NodeMenu.Items)
                {
                    if ("Delete".Equals(item.Text))
                    {
                        item.Tag = e.PickedNode;
                    }
                }
            }
        }

        /// <summary>
        /// the event sequence of selecting the PNode of process or variable in PathwayEditor.
        /// </summary>
        /// <param name="sender">PEcellVariavle of PEcellProcess.</param>
        /// <param name="e">PInputEventArgs.</param>
        public void NodeSelected(object sender, PInputEventArgs e)
        {
            if (e.PickedNode is PPathwayNode)
                ((PPathwayNode)e.PickedNode).ValidateEdges();

            if (e.Button == MouseButtons.Left)
            {
                PPathwayNode pnode = (PPathwayNode)e.PickedNode;

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
            else
            {
                this.CanvasDictionary[e.Canvas.Name].NodeMenu.Tag = e.PickedNode;
                foreach (ToolStripItem item in this.CanvasDictionary[e.Canvas.Name].NodeMenu.Items)
                {
                    if ("Delete".Equals(item.Text))
                    {
                        item.Tag = e.PickedNode;
                    }
                }
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
                this.CanvasDictionary[e.Canvas.Name].NodeMenu.Tag = e.PickedNode;
            }

            this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
            this.CanvasDictionary[e.Canvas.Name].AddSelectedLine(line);
        }

        public void NodeMoved(object sender, PInputEventArgs e)
        {
            if (!(e.PickedNode is PPathwayNode))
            {
                return;
            }
        }

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