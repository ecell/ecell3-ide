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
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Handler;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.PiccoloX.Components;
using WeifenLuo.WinFormsUI.Docking;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// This class manages resources related to one canvas.
    ///  ex) PathwayCanvas, Layer, etc.
    /// </summary>
    public class CanvasControl : IDisposable
    {
        #region Static readonly fields
        /// <summary>
        /// Least canvas size when a node is focused.
        /// </summary>
        protected static readonly float LEAST_FOCUS_SIZE = 500f;

        /// <summary>
        /// Duration for camera centering animation when a node is selected.
        /// this will be used for the argument of PCamera.AnimateViewToCenterBounds()
        /// </summary>
        protected static readonly int CAMERA_ANIM_DURATION = 700;

        /// <summary>
        /// Minimum scale.
        /// </summary>
        public static readonly float MIN_SCALE = .1f;

        /// <summary>
        /// Maximum scale
        /// </summary>
        public static readonly float MAX_SCALE = 5;

        /// <summary>
        /// Used to draw line to reconnect.
        /// </summary>
        private static readonly Pen LINE_THIN_PEN = new Pen(new SolidBrush(Color.FromArgb(200, Color.Orange)), 2);

        /// <summary>
        /// radius of a line handle
        /// </summary>
        private static readonly float LINE_HANDLE_RADIUS = 5;

        /// <summary>
        /// Name of DataColumn for setting layer visibilities (check box)
        /// </summary>
        private static readonly string COLUMN_NAME4SHOW = "Show";

        /// <summary>
        /// Name of DataColumn for indicating layer names (string)
        /// </summary>
        private static readonly string COLUMN_NAME4NAME = "Name";
        #endregion

        #region Fields
        /// <summary>
        /// The PathwayView, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        protected PathwayControl m_con;

        /// <summary>
        /// The unique ID of this canvas.
        /// </summary>
        protected string m_modelId;

        /// <summary>
        /// Whether each node is showing it's ID or not;
        /// </summary>
        protected bool m_showingId = true;

        /// <summary>
        /// Tab page for this canvas.
        /// </summary>
        protected TabPage m_pathwayTabPage;

        /// <summary>
        /// PCanvas for pathways.
        /// </summary>
        protected PathwayCanvas m_pCanvas;

        /// <summary>
        /// the canvas of overview.
        /// </summary>
        protected OverviewCanvas m_overviewCanvas;

        /// <summary>
        /// The dictionary for all systems on this canvas.
        /// </summary>
        protected SortedDictionary<string, PPathwaySystem> m_systems = new SortedDictionary<string, PPathwaySystem>();

        /// <summary>
        /// The dictionary for all variables on this canvas.
        /// </summary>
        protected SortedDictionary<string, PPathwayVariable> m_variables = new SortedDictionary<string, PPathwayVariable>();

        /// <summary>
        /// The dictionary for all processes on this canvas.
        /// </summary>
        protected SortedDictionary<string, PPathwayProcess> m_processes = new SortedDictionary<string, PPathwayProcess>();

        /// <summary>
        /// DataTable for DataGridView displayed layer list.
        /// </summary>
        protected DataTable m_table;

        /// <summary>
        /// The dictionary for all layers.
        /// </summary>
        protected Dictionary<string, PPathwayLayer> m_layers;

        /// <summary>
        /// Default LayerID
        /// </summary>
        protected string m_defLayerID = "Layer0";

        /// <summary>
        /// PLayer for control use.
        /// For example, resize handlers for PEcellSystem.
        /// </summary>
        protected PLayer m_ctrlLayer;

        /// <summary>
        /// List of PPathwayNode for selected object.
        /// </summary>
        List<PPathwayObject> m_selectedNodes = new List<PPathwayObject>();

        /// <summary>
        /// selected line
        /// </summary>
        Line m_selectedLine = null;

        /// <summary>
        /// The name of system currently selected on the canvas.
        /// </summary>
        string m_selectedSystemName;

        /// <summary>
        /// SelectChange flag.
        /// </summary>
        bool m_isSelectChanged = false;

        /// <summary>
        /// ResizeHandler for resizing a system.
        /// </summary>
        protected ResizeHandler m_resizeHandler;

        /// <summary>
        /// PPathwayObject, which is to be connected.
        /// </summary>
        PPathwayNode m_nodeToBeConnected;

        /// <summary>
        /// To handle an edge to reconnect
        /// </summary>
        bool m_isReconnectMode = false;

        /// <summary>
        /// The key of variable at the end of an edge
        /// </summary>
        string m_vOnLinesEnd = null;

        /// <summary>
        /// Line handle on the end for a variable
        /// </summary>
        PPath m_lineHandle4V = null;

        /// <summary>
        /// The key of process at the end of an edge
        /// </summary>
        string m_pOnLinesEnd = null;

        /// <summary>
        /// Line handle on the end for a process
        /// </summary>
        PPath m_lineHandle4P = null;

        /// <summary>
        /// Line for reconnecting.
        /// When a line owned by PEcellProcess is selected, this line will be hidden.
        /// Then m_line4reconnect will appear.
        /// </summary>
        Line m_line4reconnect = null;

        /// <summary>
        /// Variable or Process.
        /// </summary>
        ComponentType m_cType;

        /// <summary>
        /// Clicked PathwayObject.
        /// </summary>
        PNode m_clickedNode = null;

        /// <summary>
        /// Stack for nodes under the mouse.
        /// this will be used to reconnect edge.
        /// </summary>
        Stack<PPathwayObject> m_nodesUnderMouse = new Stack<PPathwayObject>();

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_pathwayView.
        /// </summary>
        public PathwayControl PathwayControl
        {
            get { return m_con; }
            set { m_con = value; }
        }

        /// <summary>
        /// Accessor for m_canvasId.
        /// </summary>
        public string ModelID
        {
            get { return m_modelId; }
            set { m_modelId = value; }
        }

        /// <summary>
        /// Accessor for m_ctrlLayer.
        /// </summary>
        public PLayer ControlLayer
        {
            get { return m_ctrlLayer; }
            set { this.m_ctrlLayer = value; }
        }

        /// <summary>
        /// Accessor for m_clickedNode.
        /// </summary>
        public PNode ClickedNode
        {
            get { return m_clickedNode; }
            set { this.m_clickedNode = value; }
        }

        /// <summary>
        /// Accessor for node to be reconnected.
        /// </summary>
        public PPathwayNode NodeToBeReconnected
        {
            get { return m_nodeToBeConnected; }
        }

        /// <summary>
        /// Accessor for m_pathwayTabPages.
        /// </summary>
        public TabPage TabPage
        {
            get { return m_pathwayTabPage; }
        }

        /// <summary>
        /// Accessor for m_pathwayCanvas.
        /// </summary>
        public PCanvas PathwayCanvas
        {
            get { return m_pCanvas; }
        }

        /// <summary>
        /// Accessor for m_overviewCanvas.
        /// </summary>
        public PCanvas OverviewCanvas
        {
            get { return m_overviewCanvas; }
        }

        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public List<PPathwayObject> SelectedNodes
        {
            get { return m_selectedNodes; }
        }

        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public Line SelectedLine
        {
            get { return m_selectedLine; }
        }

        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public string SelectedSystemName
        {
            get { return m_selectedSystemName; }
        }

        /// <summary>
        /// Accessor for m_layers.
        /// </summary>
        public Dictionary<string, PPathwayLayer> Layers
        {
            get { return m_layers; }
        }

        /// <summary>
        /// Accessor for m_table.
        /// </summary>
        public DataTable LayerTable
        {
            get { return m_table; }
        }

        /// <summary>
        /// Accessor for m_systems.
        /// </summary>
        public SortedDictionary<string, PPathwaySystem> Systems
        {
            get { return m_systems; }
        }

        /// <summary>
        /// Accessor for m_variables.
        /// </summary>
        public SortedDictionary<string, PPathwayVariable> Variables
        {
            get { return m_variables; }
        }

        /// <summary>
        /// Accessor for m_processes.
        /// </summary>
        public SortedDictionary<string, PPathwayProcess> Processes
        {
            get { return m_processes; }
        }

        /// <summary>
        /// Accessor for m_showingId.
        /// </summary>
        public bool ShowingID
        {
            get { return m_showingId; }
            set
            {
                m_showingId = value;
                foreach (PPathwayNode node in m_variables.Values)
                    node.ShowingID = m_showingId;
                foreach (PPathwayNode node in m_processes.Values)
                    node.ShowingID = m_showingId;
                foreach (PPathwaySystem system in m_systems.Values)
                    system.ShowingID = m_showingId;
            }
        }

        /// <summary>
        /// Accessor for m_line4reconnect.
        /// </summary>
        public Line Line4Reconnect
        {
            get { return m_line4reconnect; }
        }

        public ResizeHandler ResizeHandler
        {
            get { return m_resizeHandler; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// the constructor with initial parameters.
        /// </summary>
        /// <param name="view">PathwayView.</param>
        /// <param name="modelID">Model id.</param>
        public CanvasControl(PathwayControl view,
            string modelID)
        {
            m_con = view;
            m_modelId = modelID;

            // Preparing TabPage
            m_pathwayTabPage = new TabPage(modelID);
            m_pathwayTabPage.Name = modelID;
            m_pathwayTabPage.AutoScroll = true;

            // Preparing PathwayCanvas
            m_pCanvas = new PathwayCanvas(this);

            // Preparing overview
            m_overviewCanvas = new OverviewCanvas(m_pCanvas.Layer,
                                                  m_pCanvas.Camera);
            m_pCanvas.Camera.RemoveLayer(m_pCanvas.Layer);

            PScrollableControl scrolCtrl = new PScrollableControl(m_pCanvas);
            scrolCtrl.Layout += new LayoutEventHandler(scrolCtrl_Layout);
            scrolCtrl.Dock = DockStyle.Fill;
            m_pathwayTabPage.Controls.Add(scrolCtrl);

            // Preparing DataTable
            m_table = new DataTable(modelID);
            DataColumn dc = new DataColumn(COLUMN_NAME4SHOW);
            dc.DataType = typeof(bool);
            m_table.Columns.Add(dc);
            DataColumn dc2 = new DataColumn(COLUMN_NAME4NAME);
            dc2.DataType = typeof(string);
            m_table.Columns.Add(dc2);
            // Preparing layer list
            m_layers = new Dictionary<string, PPathwayLayer>();

            // Preparing control layer
            m_ctrlLayer = new PLayer();
            m_ctrlLayer.AddInputEventListener(new ResizeHandleDragHandler(this));
            m_pCanvas.Root.AddChild(m_ctrlLayer);
            m_pCanvas.Camera.AddLayer(m_ctrlLayer);

            // Preparing system ResizeHandlers
            m_resizeHandler = new ResizeHandler(this);

            // Prepare line handles
            m_lineHandle4V = new PPath();
            m_lineHandle4V.Brush = new SolidBrush(Color.FromArgb(125, Color.Orange));
            m_lineHandle4V.Pen = new Pen(Brushes.DarkCyan, 1);
            m_lineHandle4V.AddEllipse(
                0,
                0,
                2 * LINE_HANDLE_RADIUS,
                2 * LINE_HANDLE_RADIUS);
            m_lineHandle4V.Tag = ComponentType.Variable;
            m_lineHandle4V.MouseDown += new PInputEventHandler(m_lineHandle_MouseDown);
            m_lineHandle4V.MouseDrag += new PInputEventHandler(m_lineHandle_MouseDrag);
            m_lineHandle4V.MouseUp += new PInputEventHandler(m_lineHandle_MouseUp);

            m_lineHandle4P = new PPath();
            m_lineHandle4P.Brush = new SolidBrush(Color.FromArgb(125, Color.Orange));
            m_lineHandle4P.Pen = new Pen(Brushes.DarkCyan, 1);
            m_lineHandle4P.AddEllipse(
                0,
                0,
                2 * LINE_HANDLE_RADIUS,
                2 * LINE_HANDLE_RADIUS);
            m_lineHandle4P.Tag = ComponentType.Process;
            m_lineHandle4P.MouseDown += new PInputEventHandler(m_lineHandle_MouseDown);
            m_lineHandle4P.MouseDrag += new PInputEventHandler(m_lineHandle_MouseDrag);
            m_lineHandle4P.MouseUp += new PInputEventHandler(m_lineHandle_MouseUp);

            m_line4reconnect = new Line();
            m_line4reconnect.Brush = new SolidBrush(Color.FromArgb(200, Color.Orange));
            m_line4reconnect.Pen = LINE_THIN_PEN;
            m_line4reconnect.Pickable = false;
        }
        #endregion

        #region EventHandlers
        void scrolCtrl_Layout(object sender, LayoutEventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse is down on m_lineHandle.
        /// Start to reconnecting edge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseDown(object sender, PInputEventArgs e)
        {
            m_cType = (ComponentType)e.PickedNode.Tag;
            if (m_cType == ComponentType.Process && null != m_ctrlLayer)
            {
                m_ctrlLayer.RemoveChild(m_lineHandle4P);
            }
            else if (m_cType == ComponentType.Variable && null != m_ctrlLayer)
            {
                m_ctrlLayer.RemoveChild(m_lineHandle4V);
            }
        }

        /// <summary>
        /// Called when m_lineHandle is being dragged.
        /// reconnecting line is redrawn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseDrag(object sender, PInputEventArgs e)
        {
            if (null == m_line4reconnect || null == m_selectedLine)
            {
                return;
            }
            m_line4reconnect.Reset();
            PointF ppoint = new PointF(
                LINE_HANDLE_RADIUS + m_lineHandle4P.X + m_lineHandle4P.OffsetX,
                LINE_HANDLE_RADIUS + m_lineHandle4P.Y + m_lineHandle4P.OffsetY);
            PointF vpoint = new PointF(
                LINE_HANDLE_RADIUS + m_lineHandle4V.X + m_lineHandle4V.OffsetX,
                LINE_HANDLE_RADIUS + m_lineHandle4V.Y + m_lineHandle4V.OffsetY);
            m_line4reconnect.ProPoint = ppoint;
            m_line4reconnect.VarPoint = vpoint;
            m_line4reconnect.DrawLine();
        }

        /// <summary>
        /// Called when the mouse is up on m_lineHandle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseUp(object sender, PInputEventArgs e)
        {
            if (m_nodesUnderMouse.Count != 0 && null != m_selectedLine)
            {
                PPathwayObject obj = m_nodesUnderMouse.Pop();
                if (obj is PPathwayProcess && m_cType == ComponentType.Process)
                {
                    m_con.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_con.NotifyVariableReferenceChanged(obj.EcellObject.key, m_vOnLinesEnd, RefChangeType.BiDir, 0);
                    }
                    else
                    {
                        int coefficient = m_selectedLine.Info.Coefficient;
                        m_con.NotifyVariableReferenceChanged(
                            obj.EcellObject.key,
                            m_vOnLinesEnd,
                            RefChangeType.SingleDir,
                            coefficient);
                    }
                    ResetSelectedLine();
                }
                else if (obj is PPathwayVariable && m_cType == ComponentType.Variable)
                {
                    m_con.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_con.NotifyVariableReferenceChanged(m_pOnLinesEnd, obj.EcellObject.key, RefChangeType.BiDir, 0);
                    }
                    else
                    {
                        int coefficient = m_selectedLine.Info.Coefficient;
                        m_con.NotifyVariableReferenceChanged(
                            m_pOnLinesEnd,
                            obj.EcellObject.key,
                            RefChangeType.SingleDir,
                            coefficient);
                    }
                    ResetSelectedLine();
                }
            }
            ResetLinePosition();
        }
        #endregion

        /// <summary>
        /// Notify SelectChanged event to outside.
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        /// </summary>
        public void NotifySelectChanged(string key, string type)
        {
            m_isSelectChanged = true;
            if (m_con != null)
                m_con.Window.NotifySelectChanged(this.m_modelId, key, type);
        }

        /// <summary>
        /// Notify SelectChanged event to outside.
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        /// <param name="isSelect">the flag whether this object is selected.</param>
        /// </summary>
        public void NotifyAddSelect(string key, string type, bool isSelect)
        {
            if (m_con != null)
                m_con.Window.NotifyAddSelect(this.m_modelId, key, type, isSelect);
        }

        /// <summary>
        /// Notify this canvas that the mouse is on it.
        /// </summary>
        /// <param name="obj">mouse is on this node</param>
        public void NotifyMouseEnter(PPathwayObject obj)
        {
            if (m_isReconnectMode)
                m_nodesUnderMouse.Push(obj);
        }

        /// <summary>
        /// Notify this canvas that the mouse is not out of it.
        /// </summary>
        public void NotifyMouseLeave()
        {
            if (m_isReconnectMode)
            {
                try
                {
                    m_nodesUnderMouse.Pop();
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        /// <summary>
        /// Check if any system of this canvas overlaps given rectangle.
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemOverlaps(RectangleF rect)
        {
            return DoesSystemOverlaps(rect, null);
        }

        /// <summary>
        /// Check if any system of this canvas (exclude specified system) overlaps given rectangle.
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <param name="excludeSystem">The name of the system to ignore in the check.</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, false otherwise</returns>
        public bool DoesSystemOverlaps(RectangleF rect, string excludeSystem)
        {
            bool isOverlaping = false;
            foreach (PPathwaySystem system in m_systems.Values)
                if (system.Overlaps(rect) && !system.EcellObject.key.Equals(excludeSystem))
                    isOverlaping = true;

            return isOverlaping;
        }

        /// <summary>
        /// Add PPathwayObject to this canvas.
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="obj"></param>
        /// <param name="hasCoords"></param>
        /// <param name="isFirst"></param>
        public void DataAdd(string systemName, PPathwayObject obj, bool hasCoords, bool isFirst)
        {
            // Set Layer
            SetLayer(obj);

            RegisterObjToSet(obj);
            if (obj is PPathwayNode)
                ((PPathwayNode)obj).ShowingID = this.m_showingId;
            // Set Root System
            if (systemName == null || systemName.Equals("") )
            {
                if (!hasCoords)
                    SetSystemSize(obj);
                obj.Layer.AddChild(obj);
                return;
            }

            // Set Child object.
            PPathwaySystem system = m_systems[systemName];
            obj.ParentObject = system;
            // If obj hasn't coordinate, it will be settled. 
            if (obj is PPathwayNode && !system.Rect.Contains(obj.PointF))
                obj.PointF = GetVacantPoint(systemName);
            if (obj is PPathwaySystem && !hasCoords)
            {
                float maxX = system.X + system.OffsetX;
                float x = 0f;
                List<PPathwayObject> list = GetAllObjectUnder(system.EcellObject.key);
                foreach (PPathwayObject ppo in list)
                {
                    if (ppo == obj)
                        continue;
                    x = ppo.X + ppo.OffsetX + ppo.Width;
                    if (maxX < x)
                        maxX = x;
                }
                // Set obj's coordinate
                obj.X = maxX + PPathwaySystem.SYSTEM_MARGIN;
                obj.Y = system.Y + system.Offset.Y + PPathwaySystem.SYSTEM_MARGIN;
                SetSystemSize(obj);
                system.MakeSpace(obj);
            }
            obj.Refresh();
        }
        /// <summary>
        /// Set Layer.
        /// </summary>
        /// <param name="obj"></param>
        public void SetLayer(PPathwayObject obj)
        {
            string layerID = obj.EcellObject.LayerID;
            if (obj.EcellObject.key.Equals("/") && (layerID != null && !layerID.Equals("")))
            {
                m_defLayerID = layerID;
            }
            else if (layerID == null || layerID.Equals(""))
            {
                layerID = m_defLayerID;
            }
            if (!m_layers.ContainsKey(layerID))
            {
                AddLayer(layerID);
            }
            obj.Layer = m_layers[layerID];
            obj.Layer.AddChild(obj);
        }

        /// <summary>
        /// Set the system size.
        /// </summary>
        /// <param name="system">The system object.</param>
        private void SetSystemSize(PPathwayObject system)
        {
            int length = 200 * (int)Math.Sqrt(system.EcellObject.Children.Count);
            if (length > PPathwaySystem.DEFAULT_WIDTH)
            {
                system.Width = length;
                system.Height = length;
            }
            else
            {
                system.Width = PPathwaySystem.DEFAULT_WIDTH;
                system.Height = PPathwaySystem.DEFAULT_HEIGHT;
            }

        }
        /// <summary>
        /// register the object to this set.
        /// </summary>
        /// <param name="obj">the registered object.</param>
        public void RegisterObjToSet(PPathwayObject obj)
        {
            if (obj is PPathwaySystem)
            {
                PPathwaySystem system = (PPathwaySystem)obj;
                m_systems.Add(system.EcellObject.key, system);
            }
            else if (obj is PPathwayVariable)
            {
                PPathwayVariable node = (PPathwayVariable)obj;
                m_variables.Add(node.EcellObject.key, node);
            }
            else if (obj is PPathwayProcess)
            {
                PPathwayProcess node = (PPathwayProcess)obj;
                m_processes.Add(node.EcellObject.key, node);
            }
            if (obj.CanvasControl == null)
                obj.CanvasControl = this;
        }

        #region Methods to control Layer.
        /// <summary>
        /// add the selected layer.
        /// </summary>
        /// <param name="name">the added layer.</param>
        public void AddLayer(string name)
        {
            // Error check.
            if (name == null || name.Equals(""))
                return;
            if (m_layers.ContainsKey(name))
            {
                MessageBox.Show(name + m_resources.GetString("ErrAlrExist"));
                return;
            }
            PPathwayLayer layer = new PPathwayLayer(name);
            layer.AddInputEventListener(new NodeDragHandler(this));

            m_pCanvas.Root.AddChild(0, layer);
            m_pCanvas.Camera.AddLayer(0, layer);
            m_overviewCanvas.AddObservedLayer(layer);
            m_layers.Add(layer.Name, layer);

            RefreshLayerTable();
            m_ctrlLayer.MoveToFront();
        }
        /// <summary>
        /// Refresh Layer table.
        /// </summary>
        private void RefreshLayerTable()
        {
            m_table.Clear();
            foreach (KeyValuePair<string, PPathwayLayer> set in m_layers)
            {
                DataRow dr = m_table.NewRow();
                dr[COLUMN_NAME4SHOW] = set.Value.Visible;
                dr[COLUMN_NAME4NAME] = set.Key;
                m_table.Rows.Add(dr);
            }
        }

        /// <summary>
        /// Delete selected layer.
        /// </summary>
        /// <param name="name"></param>
        public void DeleteLayer(string name)
        {
            if (name.Equals(m_defLayerID))
            {
                MessageBox.Show(m_resources.GetString("ErrDelRoot"));
                return;
            }
            PPathwayLayer layer = m_layers[name];
            m_layers.Remove(name);
            m_overviewCanvas.RemoveObservedLayer(layer);
            m_pCanvas.Camera.RemoveLayer(layer);
            m_pCanvas.Root.RemoveChild(layer);

            RefreshLayerTable();

            // Delete Nodes under this layer
            List<PPathwayObject> list = layer.NodeList;
            int i = 0;
            foreach (PPathwayObject obj in list)
            {
                i++;
                m_con.NotifyDataDelete(obj, (i == list.Count));
            }
        }

        /// <summary>
        /// Rename selected layer.
        /// </summary>
        /// <param name="name"></param>
        public void RenameLayer(string oldName, string newName)
        {
            PPathwayLayer layer = m_layers[oldName];
            layer.Name = newName;
            m_layers.Remove(oldName);
            m_layers.Add(newName, layer);
            // Change Nodes under this layer
            List<PPathwayObject> list = layer.NodeList;
            int i = 0;
            foreach (PPathwayObject obj in list)
            {
                i++;
                m_con.NotifyDataChanged(
                    obj.EcellObject.key,
                    obj.EcellObject.key,
                    obj,
                    true,
                    (i == list.Count));
            }

            RefreshLayerTable();
        }

        /// <summary>
        /// Set Layer Visibility.
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isShowing"></param>
        public void ChangeLayerVisibility(string layerName, bool isShown)
        {
            PPathwayLayer layer = m_layers[layerName];
            if (layer == null)
                return;
            // Set Visibility.
            layer.Visible = isShown;
            RefreshVisibility();
            m_con.OverView.Canvas.Refresh();
        }

        /// <summary>
        /// Change Process Visibility.
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isShowing"></param>
        public void ChangeViewMode(bool isShown)
        {
            foreach (PPathwayProcess process in m_processes.Values)
            {
                process.ViewMode = isShown;
            }
        }

        /// <summary>
        /// Get a list of layers.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetLayerNameList()
        {
            List<string> list = new List<string>();
            foreach (PPathwayLayer layer in m_layers.Values)
                list.Add(layer.Name);

            return list;
        }

        /// <summary>
        /// Merge two layers
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        internal void MergeLayer(string oldName, string newName)
        {
            if (oldName.Equals(newName))
                return;

            PPathwayLayer oldlayer = m_layers[oldName];
            PPathwayLayer newlayer = m_layers[newName];
            // Change Nodes under this layer
            List<PPathwayObject> list = oldlayer.NodeList;
            int i = 0;
            foreach (PPathwayObject obj in list)
            {
                i++;
                obj.Layer = newlayer;
                m_con.NotifyDataChanged(
                    obj.EcellObject.key,
                    obj.EcellObject.key,
                    obj,
                    true,
                    (i == list.Count));
            }
            m_layers.Remove(oldName);
            RefreshLayerTable();
        }
        #endregion

        /// <summary>
        /// Get a temporary key of EcellObject.
        /// </summary>
        /// <param name="type">The data type of EcellObject.</param>
        /// <param name="systemID">The ID of parent system.</param>
        /// <returns>"TemporaryID"</returns> 
        public string GetTemporaryID(string type, string systemID)
        {
            return this.PathwayControl.Window.GetTemporaryID(m_modelId, type, systemID);
        }
        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <param name="excludedSystem">If this parameter is set, this system is excluded from searching</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public PPathwayObject GetSurroundingSystem(PointF point, string excludedSystem)
        {
            PPathwayObject obj = null;

            foreach (PPathwaySystem sys in this.m_systems.Values)
                if (sys.Rect.Contains(point) && !sys.EcellObject.key.Equals(excludedSystem))
                    obj = sys;
            return obj;
        }

        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public string GetSurroundingSystemKey(PointF point)
        {
            return GetSurroundingSystemKey(point, null);
        }
        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <param name="excludedSystem">If this parameter is set, this system is excluded from searching</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public string GetSurroundingSystemKey(PointF point, string excludedSystem)
        {
            PPathwayObject obj = GetSurroundingSystem(point, excludedSystem);
            if (obj == null)
                return null;
            return obj.EcellObject.key;
        }

        /// <summary>
        /// AddSelect PPathwayNode
        /// </summary>
        /// <param name="obj">Newly selected object</param>
        public void AddSelectedNode(PPathwayNode obj)
        {
            m_selectedNodes.Add(obj);
            obj.IsHighLighted = true;
        }

        /// <summary>
        /// Notify this set that one PEcellSystem is selected.
        /// </summary>
        /// <param name="systemName">the name of selected system.</param>
        public void AddSelectedSystem(string systemName)
        {
            m_selectedSystemName = systemName;
            if (!m_systems.ContainsKey(systemName))
                return;
            m_systems[systemName].IsHighLighted = true;
            m_resizeHandler.ShowResizeHandles();
        }

        /// <summary>
        /// Select this line on this canvas.
        /// </summary>
        /// <param name="line"></param>
        public void AddSelectedLine(Line line)
        {
            if (line == null)
                return;

            m_selectedLine = line;

            m_nodesUnderMouse.Clear();
            m_isReconnectMode = true;

            m_vOnLinesEnd = m_selectedLine.Info.VariableKey;
            m_pOnLinesEnd = m_selectedLine.Info.ProcessKey;

            // Prepare line handles
            m_lineHandle4V.Offset = PointF.Empty;
            m_lineHandle4P.Offset = PointF.Empty;

            m_lineHandle4V.X = m_selectedLine.VarPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4V.Y = m_selectedLine.VarPoint.Y - LINE_HANDLE_RADIUS;

            m_lineHandle4P.X = m_selectedLine.ProPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4P.Y = m_selectedLine.ProPoint.Y - LINE_HANDLE_RADIUS;

            m_ctrlLayer.AddChild(m_lineHandle4V);
            m_ctrlLayer.AddChild(m_lineHandle4P);

            // Create Reconnect line
            m_line4reconnect.Reset();
            m_line4reconnect.Pen = LINE_THIN_PEN;
            m_line4reconnect.Info.Direction = m_selectedLine.Info.Direction;
            m_line4reconnect.Info.TypeOfLine = m_selectedLine.Info.TypeOfLine;
            m_line4reconnect.VarPoint = m_selectedLine.VarPoint;
            m_line4reconnect.ProPoint = m_selectedLine.ProPoint;
            m_line4reconnect.DrawLine();

            SetLineVisibility(true);
        }

        /// <summary>
        /// Show/Hide line4reconnect.
        /// </summary>
        /// <param name="visible">visibility</param>
        public void SetLineVisibility(bool visible)
        {
            if (m_line4reconnect == null)
                return;

            if (visible)
            {
                m_ctrlLayer.AddChild(m_line4reconnect);
                m_line4reconnect.Visible = true;
            }
            else
            {
                if (m_line4reconnect.Parent != null)
                    m_ctrlLayer.RemoveChild(m_line4reconnect);
                m_line4reconnect.Visible = false;
                m_line4reconnect.Reset();
            }
        }

        /// <summary>
        /// Add node, which is to be connected
        /// </summary>
        /// <param name="obj">node which is to be connected</param>
        public void AddNodeToBeConnected(PPathwayNode obj)
        {
            if (null != m_nodeToBeConnected)
                m_nodeToBeConnected.IsToBeConnected = false;
            obj.IsToBeConnected = true;
            m_nodeToBeConnected = obj;
        }

        /// <summary>
        /// Freeze objects on this canvas.
        /// </summary>
        public void Freeze()
        {
            foreach (PPathwaySystem system in m_systems.Values)
                system.Freeze();
            foreach (PPathwayVariable var in m_variables.Values)
                var.Freeze();
            foreach (PPathwayProcess pro in m_processes.Values)
                pro.Freeze();
        }

        /// <summary>
        /// Get a key list of systems under a given system.
        /// </summary>
        /// <param name="systemKey"></param>
        /// <returns>list of Ecell key of systems</returns>
        public List<PPathwayObject> GetAllObjectUnder(string systemKey)
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            foreach (PPathwayObject obj in this.GetAllObjects())
            {
                if (obj.EcellObject.key.StartsWith(systemKey) && !obj.EcellObject.key.Equals(systemKey))
                    returnList.Add(obj);
            }
            return returnList;
        }

        /// <summary>
        /// Get all EcellObjects of this object.
        /// </summary>
        /// <param name="key">The key of selected object.</param>
        /// <param name="type">The type of selected object.</param>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public PPathwayObject GetSelectedObject(string key, string type)
        {
            if (key == null || type == null)
                return null;
            if (type.Equals(EcellObject.SYSTEM) && m_systems.ContainsKey(key))
                return m_systems[key];
            if (type.Equals(EcellObject.PROCESS) && m_processes.ContainsKey(key))
                return m_processes[key];
            if (type.Equals(EcellObject.VARIABLE) && m_variables.ContainsKey(key))
                return m_variables[key];
            return null;
        }
        /// <summary>
        /// Get all EcellObjects of this object.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<PPathwayObject> GetAllObjects()
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            returnList.AddRange(GetSystemList());
            returnList.AddRange(GetNodeList());

            return returnList;
        }
        /// <summary>
        /// Get all EcellSystems of this object.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<PPathwayObject> GetSystemList()
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            foreach (PPathwaySystem system in this.m_systems.Values)
                returnList.Add(system);

            return returnList;
        }
        /// <summary>
        /// Get all EcellProcesss of this object.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<PPathwayObject> GetNodeList()
        {
            List<PPathwayObject> returnList = new List<PPathwayObject>();
            foreach (PPathwayProcess process in this.m_processes.Values)
                returnList.Add(process);
            foreach (PPathwayVariable variable in this.m_variables.Values)
                returnList.Add(variable);

            return returnList;
        }

        #region From PathwayView
        /// <summary>
        /// change the display rectangles of PathwayEditor.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="delta"></param>
        public void PanCanvas(Direction direction, int delta)
        {
            if (direction == Direction.Vertical)
                m_pCanvas.Camera.TranslateViewBy(0, delta);
            else
                m_pCanvas.Camera.TranslateViewBy(delta, 0);
            this.UpdateOverview();
        }

        /// <summary>
        /// get bitmap image projected PathwayEditor.
        /// </summary>
        /// <returns></returns>
        public Bitmap ToImage()
        {
            return new Bitmap(m_pCanvas.Layer[0].ToImage());
        }

        /// <summary>
        /// Transfer the EcellObject from the old key to the new key.
        /// </summary>
        /// <param name="oldkey">The old key.</param>
        /// <param name="newkey">The new key.</param>
        /// <param name="obj">The transfered EcellObject.</param>
        public void TransferObject(string oldkey, string newkey, PPathwayObject obj)
        {
            if (obj is PPathwaySystem)
            {
                if (!m_systems.ContainsKey(oldkey))
                    return;
                m_systems.Remove(oldkey);
                m_systems.Add(newkey, (PPathwaySystem)obj);
                m_systems[newkey].Refresh();
            }
            else if (obj is PPathwayVariable)
            {
                if (!m_variables.ContainsKey(oldkey))
                    return;
                m_variables.Remove(oldkey);
                m_variables.Add(newkey, (PPathwayVariable)obj);
                m_variables[newkey].Refresh();
            }
            else if (obj is PPathwayProcess)
            {
                if (!m_processes.ContainsKey(oldkey))
                    return;
                m_processes.Remove(oldkey);
                m_processes.Add(newkey, (PPathwayProcess)obj);
                m_processes[newkey].Refresh();
            }
            string sysKey = PathUtil.GetParentSystemId(newkey);
            obj.ParentObject = m_systems[sysKey];

        }
        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="oldKey">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="obj">Changed value of object.</param>
        public void DataChanged(string oldKey, string newKey, PPathwayObject obj)
        {
            if (!oldKey.Equals(newKey))
                TransferObject(oldKey, newKey, obj);

            // Set Layer
            SetLayer(obj);
        }
        /// <summary>
        /// event sequence of deleting the object.
        /// </summary>
        /// <param name="key">the key of deleted object.</param>
        /// <param name="type">the type of deleted object.</param>
        public void DataDelete(string key, string type)
        {
            PPathwayObject obj = GetSelectedObject(key, type);
            if (obj == null)
                return;

            ResetSelectedObjects();

            if (obj is PPathwaySystem)
            {
                RemoveNodeUnder(key);
                m_systems.Remove(key);
            }
            else if (obj is PPathwayProcess)
            {
                ((PPathwayProcess)obj).Delete();
                m_processes.Remove(key);
            }
            else if (obj is PPathwayVariable)
            {
                ((PPathwayVariable)obj).NotifyRemoveToRelatedProcess();
                m_variables.Remove(key);
            }
            RemoveObject(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveObject(PPathwayObject obj)
        {
            if (obj == null)
                return;
            obj.Text.RemoveFromParent();
            obj.Parent.RemoveChild(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysKey"></param>
        private void RemoveNodeUnder(string sysKey)
        {
            foreach (PPathwayObject obj in GetAllObjectUnder(sysKey))
                DataDelete(obj.EcellObject.key, obj.EcellObject.type);
        }

        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void AddSelect(string key, string type)
        {
            PPathwayObject obj = GetSelectedObject(key, type);
            if (obj == null)
                return;

            if (type.Equals(EcellObject.SYSTEM))
            {
                this.ResetSelectedObjects();
                this.AddSelectedSystem(key);
            }
            if (type.Equals(EcellObject.PROCESS) || type.Equals(EcellObject.VARIABLE))
            {
                PPathwayNode focusNode = (PPathwayNode)obj;
                this.AddSelectedNode(focusNode);
            }
        }
        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void SelectChanged(string key, string type)
        {
            // Error check.
            if (key == null || type == null)
                return;
            PPathwayObject obj = GetSelectedObject(key, type);
            if (obj == null)
                return;
            // Set select change.
            this.ResetSelectedObjects();
            RectangleF centerBounds = new RectangleF();
            switch (type)
            {
                case EcellObject.SYSTEM:
                    centerBounds = obj.FullBounds;
                    this.AddSelectedSystem(key);
                    break;
                case EcellObject.VARIABLE: case EcellObject.PROCESS:
                    centerBounds = PathUtil.GetFocusBound(obj.FullBounds, LEAST_FOCUS_SIZE);
                    this.AddSelectedNode((PPathwayNode)obj);
                    break;
            }

            // Exit if the event came from this plugin.
            if (m_isSelectChanged)
            {
                m_isSelectChanged = false;
                return;
            }
            // Move camera view.
            m_pCanvas.Camera.AnimateViewToCenterBounds(centerBounds,
                                             true,
                                             CAMERA_ANIM_DURATION);
            UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
        }
        #endregion

        /// <summary>
        /// Cancel freeze status of this canvas.
        /// </summary>
        public void Unfreeze()
        {
            foreach (PPathwaySystem system in m_systems.Values)
                system.Unfreeze();
            foreach (PPathwayVariable var in m_variables.Values)
                var.Unfreeze();
            foreach (PPathwayProcess pro in m_processes.Values)
                pro.Unfreeze();
        }

        /// <summary>
        /// Call the UpdateOverview() method after a certain time passed
        /// </summary>
        /// <param name="miliSec">Called after this time passed</param>
        public void UpdateOverviewAfterTime(int miliSec)
        {
            Timer timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = miliSec;
            timer.Start();
        }

        /// <summary>
        /// This timer delegate is called for updating overview after object-moving anime has finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            this.UpdateOverview();
            ((Timer)sender).Stop();
            ((Timer)sender).Dispose();
        }

        /// <summary>
        /// redraw the overview.
        /// </summary>
        public void UpdateOverview()
        {
            RectangleF rect = m_pCanvas.Camera.ViewBounds;
            m_overviewCanvas.UpdateOverview(rect);
        }

        /// <summary>
        /// Zoom in/out this canvas.
        /// </summary>
        /// <param name="rate"></param>
        public void Zoom(float rate)
        {
            float currentScale = this.PathwayCanvas.Camera.ViewScale;
            float newScale = currentScale * rate;

            if (newScale < MIN_SCALE)
            {
                rate = MIN_SCALE / currentScale;
            }

            if (MAX_SCALE < newScale)
            {
                rate = MAX_SCALE / currentScale;
            }

            float zoomX = this.PathwayCanvas.Camera.X + this.PathwayCanvas.Camera.OffsetX + (this.PathwayCanvas.Camera.Width / 2);
            float zoomY = this.PathwayCanvas.Camera.Y + this.PathwayCanvas.Camera.OffsetY + (this.PathwayCanvas.Camera.Height / 2);
            this.PathwayCanvas.Camera.ScaleViewBy(rate, zoomX, zoomY);
            UpdateOverview();
        }

        /// <summary>
        /// Release all the unmanaged resources in this object
        /// </summary>
        public void Dispose()
        {
            if (m_pathwayTabPage != null)
                m_pathwayTabPage.Dispose();

            if (m_pCanvas != null)
                m_pCanvas.Dispose();

            if (m_overviewCanvas != null)
                m_overviewCanvas.Dispose();
        }

        /// <summary>
        /// reset the seleceted object.
        /// </summary>
        public void ResetSelectedSystem()
        {
            if (m_selectedSystemName != null && m_systems.ContainsKey(m_selectedSystemName))
            {
                m_systems[m_selectedSystemName].IsHighLighted = false;
            }
            m_selectedSystemName = null;
            m_resizeHandler.HideResizeHandles();
        }

        /// <summary>
        /// reset the seletect nodes.
        /// </summary>
        public void ResetSelectedNodes()
        {
            if (m_selectedNodes.Count == 0)
                return;
            foreach (PPathwayObject obj in m_selectedNodes)
                GetSelectedObject(obj.EcellObject.key, obj.EcellObject.type).IsHighLighted = false;
            lock (this)
                m_selectedNodes.Clear();
        }

        /// <summary>
        /// Reset node to be connected to normal state.
        /// </summary>
        public void ResetNodeToBeConnected()
        {
            if (null != m_nodeToBeConnected)
                m_nodeToBeConnected.IsToBeConnected = false;
            m_nodeToBeConnected = null;
        }

        /// <summary>
        /// Reset a reconnecting line.
        /// </summary>
        public void ResetLinePosition()
        {
            if (null == m_selectedLine)
                return;
            PointF varPoint = new PointF(m_selectedLine.VarPoint.X, m_selectedLine.VarPoint.Y);
            PointF proPoint = new PointF(m_selectedLine.ProPoint.X, m_selectedLine.ProPoint.Y);

            m_ctrlLayer.AddChild(m_lineHandle4V);
            m_ctrlLayer.AddChild(m_lineHandle4P);

            m_lineHandle4V.Offset = PointF.Empty;
            m_lineHandle4V.X = varPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4V.Y = varPoint.Y - LINE_HANDLE_RADIUS;

            m_lineHandle4P.Offset = PointF.Empty;
            m_lineHandle4P.X = proPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4P.Y = proPoint.Y - LINE_HANDLE_RADIUS;

            // Create line
            m_line4reconnect.Reset();
            m_processes[m_selectedLine.Info.ProcessKey].Refresh();
        }

        /// <summary>
        /// Reset selected line
        /// </summary>
        public void ResetSelectedLine()
        {
            if (m_selectedLine == null)
                return;

            m_isReconnectMode = false;
            m_nodesUnderMouse.Clear();
            m_processes[m_selectedLine.Info.ProcessKey].Refresh();
            m_selectedLine = null;

            m_vOnLinesEnd = null;
            m_pOnLinesEnd = null;

            if (m_lineHandle4V != null && m_lineHandle4V.Parent != null)
                m_ctrlLayer.RemoveChild(m_lineHandle4V);
            if (m_lineHandle4P != null && m_lineHandle4P.Parent != null)
                m_ctrlLayer.RemoveChild(m_lineHandle4P);

            SetLineVisibility(false);
        }

        /// <summary>
        /// reset the selected object(system and node).
        /// </summary>
        public void ResetSelectedObjects()
        {
            ResetSelectedSystem();
            ResetSelectedNodes();
            ResetSelectedLine();
        }

        /// <summary>
        /// Change visibility of control layer, according to visibility of object layers.
        /// If any object layer is visible, control layer is visible.
        /// Otherwise, a control layer is not visible
        /// </summary>
        public void RefreshVisibility()
        {
            bool isAnyVisible = false;
            foreach (PLayer layer in Layers.Values)
            {
                if (layer.Visible)
                {
                    isAnyVisible = true;
                    break;
                }
            }
            m_ctrlLayer.Visible = isAnyVisible;
            m_pCanvas.Refresh();
        }

        /// <summary>
        /// Return true if EcellSystem contains a point.
        /// </summary>
        /// <param name="sysKey">string</param>
        /// <param name="point">PointF</param>
        /// <returns>bool</returns>
        public bool CheckNodePosition(string sysKey, PointF point)
        {
            PointF center = new PointF(point.X + PPathwayNode.DEFAULT_WIDTH / 2, point.Y + PPathwayNode.DEFAULT_HEIGHT / 2);
            bool sysContains = false;
            bool childContains = false;
            foreach (PPathwaySystem sys in this.m_systems.Values)
                if (sys.EcellObject.key.Equals(sysKey) && sys.Rect.Contains(point) && sys.Rect.Contains(center))
                    sysContains = true;
                else if (sys.EcellObject.key.StartsWith(sysKey) && (sys.Rect.Contains(point) || sys.Rect.Contains(center) ) )
                    childContains = true;
            return sysContains && !childContains;
        }

        /// <summary>
        /// Return nearest vacant point of EcellSystem.
        /// </summary>
        /// <param name="sysKey">Key of system.</param>
        ///<returns>Point.</returns>
        public PointF GetVacantPoint(string sysKey)
        {
            PPathwaySystem sys = m_systems[sysKey];
            Random hRandom = new Random();
            PointF basePos = new PointF(
                (float)hRandom.Next((int)sys.X, (int)(sys.X + sys.Width)),
                (float)hRandom.Next((int)sys.Y, (int)(sys.Y + sys.Height)) );
            return GetVacantPoint(sysKey, basePos);
        }

        /// <summary>
        /// Return nearest vacant point of EcellSystem.
        /// </summary>
        /// <param name="sysKey">The key of system.</param>
        /// <param name="pos">Target position.</param>
        /// <returns>Point.</returns>
        public PointF GetVacantPoint(string sysKey, PointF pos)
        {
            PPathwaySystem sys = m_systems[sysKey];
            PointF newPos = new PointF(pos.X, pos.Y);
            double rad = Math.PI * 0.25f;
            float r = 0f;

            do
            {
                // Check 
                if (CheckNodePosition(sysKey, newPos))
                    break;

                r += 1f;
                newPos = new PointF(pos.X + r * (float)Math.Cos(rad * r), pos.Y + r * (float)Math.Sin(rad * r));

            } while (r < sys.Width || r < sys.Height);
            return newPos;
        }
    }
}