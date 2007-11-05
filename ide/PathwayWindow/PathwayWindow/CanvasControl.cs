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
        /// used for painting the background of show button. For viewing objects under it, this
        /// color has alpha value
        /// </summary>
        private static readonly SolidBrush SHOW_BTN_BG_BRUSH
             = new SolidBrush(Color.FromArgb(48, Color.Gray));

        /// <summary>
        /// used for painting the arrow of show button. For viewing objects under it, this color
        /// has alpha value.
        /// </summary>
        private static readonly SolidBrush SHOW_BTN_ARROW_BRUSH
            = new SolidBrush(Color.FromArgb(200, Color.Black));

        /// <summary>
        /// used for painting the arrow of show button. For viewing objects under it, this color
        /// has alpha value.
        /// </summary>
        private static readonly SolidBrush SHOW_BTN_SHADOW_BRUSH
             = new SolidBrush(Color.FromArgb(200, Color.Black));

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
        /// DataTable for DataGridView displayed layer list.
        /// </summary>
        protected DataTable m_table;

        /// <summary>
        /// The dictionary for all layers.
        /// </summary>
        protected Dictionary<string, PLayer> m_layers;

        /// <summary>
        /// The dictionary for all systems on this canvas.
        /// </summary>
        protected Dictionary<string, PPathwaySystem> m_systems = new Dictionary<string, PPathwaySystem>();

        /// <summary>
        /// The dictionary for all variables on this canvas.
        /// </summary>
        protected Dictionary<string, PPathwayVariable> m_variables = new Dictionary<string, PPathwayVariable>();

        /// <summary>
        /// The dictionary for all processes on this canvas.
        /// </summary>
        protected Dictionary<string, PPathwayProcess> m_processes = new Dictionary<string, PPathwayProcess>();

        /// <summary>
        /// PLayer for control use.
        /// For example, resize handlers for PEcellSystem.
        /// </summary>
        protected PLayer m_ctrlLayer;

        /// <summary>
        /// ContextMenuStrip for PPathwayNode
        /// </summary>
        private ContextMenuStrip m_nodeMenu;

        /// <summary>
        /// Whether each node is showing it's ID or not;
        /// </summary>
        protected bool m_showingId = true;

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
        /// PPathwayObject, which is to be connected.
        /// </summary>
        PPathwayNode m_nodeToBeConnected;

        /// <summary>
        /// List of PNodes, which are currently surrounded by the system.
        /// </summary>
        PNodeList m_surroundedBySystem = null;

        /// <summary>
        /// m_resideHandles contains a list of ResizeHandle for resizing a system.
        /// </summary>
        protected PNodeList m_resizeHandles = new PNodeList();

        /// <summary>
        /// Half of width of a ResizeHandle
        /// </summary>
        protected float m_resizeHandleHalfWidth = 10;

        /// <summary>
        /// Used to save upper left point of a system
        /// </summary>
        protected PointF m_upperLeftPoint;

        /// <summary>
        /// Used to save upper right point of a system
        /// </summary>
        protected PointF m_upperRightPoint;

        /// <summary>
        /// Used to save lower right point of a system
        /// </summary>
        protected PointF m_lowerRightPoint;

        /// <summary>
        /// Used to save lower left point of a system
        /// </summary>
        protected PointF m_lowerLeftPoint;

        /////// To handle an edge to reconnect
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
        }

        /// <summary>
        /// Accessor for m_nodeMenu.
        /// </summary>
        public ContextMenuStrip NodeMenu
        {
            get { return m_nodeMenu; }
            set { this.m_nodeMenu = value; }
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
        public PNode SelectedLine
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
        public Dictionary<string, PLayer> Layers
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
        public Dictionary<string, PPathwaySystem> Systems
        {
            get { return m_systems; }
        }

        /// <summary>
        /// Accessor for m_variables.
        /// </summary>
        public Dictionary<string, PPathwayVariable> Variables
        {
            get { return m_variables; }
        }

        /// <summary>
        /// Accessor for m_processes.
        /// </summary>
        public Dictionary<string, PPathwayProcess> Processes
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
                foreach (PPathwayNode pnode in m_variables.Values)
                {
                    pnode.ShowingID = m_showingId;
                }
                foreach (PPathwayNode pnode in m_processes.Values)
                {
                    pnode.ShowingID = m_showingId;
                }
                foreach (PPathwaySystem system in m_systems.Values)
                {
                    if (m_showingId)
                        system.Text.Visible = true;
                    else
                        system.Text.Visible = false;
                }
            }
        }

        /// <summary>
        /// Accessor for m_line4reconnect.
        /// </summary>
        public Line Line4Reconnect
        {
            get { return m_line4reconnect; }
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
            m_pCanvas.RemoveInputEventListener(m_pCanvas.PanEventHandler);
            m_pCanvas.RemoveInputEventListener(m_pCanvas.ZoomEventHandler);
            m_pCanvas.AddInputEventListener(new DefaultMouseHandler(m_con));
            m_pCanvas.Dock = DockStyle.Fill;
            m_pCanvas.Name = modelID;
            m_pCanvas.Camera.ScaleViewBy(0.7f);

            PScrollableControl scrolCtrl = new PScrollableControl(m_pCanvas);
            scrolCtrl.Layout += new LayoutEventHandler(scrolCtrl_Layout);
            scrolCtrl.Dock = DockStyle.Fill;
            m_pathwayTabPage.Controls.Add(scrolCtrl);

            // Preparing overview
            m_overviewCanvas = new OverviewCanvas(m_pCanvas.Layer,
                                                  m_pCanvas.Camera);
            m_pCanvas.Camera.RemoveLayer(m_pCanvas.Layer);

            // Preparing DataTable
            m_table = new DataTable(modelID);
            DataColumn dc = new DataColumn(COLUMN_NAME4SHOW);
            dc.DataType = typeof(bool);
            m_table.Columns.Add(dc);
            DataColumn dc2 = new DataColumn(COLUMN_NAME4NAME);
            dc2.DataType = typeof(string);
            m_table.Columns.Add(dc2);
            // Preparing layer list
            m_layers = new Dictionary<string, PLayer>();

            // Preparing control layer
            m_ctrlLayer = new PLayer();
            m_ctrlLayer.AddInputEventListener(new ResizeHandleDragHandler(this));
            m_pCanvas.Root.AddChild(m_ctrlLayer);
            m_pCanvas.Camera.AddLayer(m_ctrlLayer);

            // Preparing context menus.
            m_pCanvas.ContextMenuStrip = m_con.NodeMenu;

            // Preparing system handlers
            for (int m = 0; m < 8; m++)
            {
                ResizeHandle handle = new ResizeHandle();
                handle.Brush = Brushes.DarkOrange;
                handle.Pen = new Pen(Brushes.DarkOliveGreen, 1);

                handle.AddRectangle(-1 * m_resizeHandleHalfWidth,
                                    -1 * m_resizeHandleHalfWidth,
                                    m_resizeHandleHalfWidth * 2f,
                                    m_resizeHandleHalfWidth * 2f);
                m_resizeHandles.Add(handle);
            }

            m_resizeHandles[0].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[0].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNWSE);
            m_resizeHandles[0].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[0].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[0].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeNW);
            m_resizeHandles[0].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[1].Tag = MovingRestriction.Vertical;
            m_resizeHandles[1].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNS);
            m_resizeHandles[1].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[1].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[1].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeN);
            m_resizeHandles[1].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[2].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[2].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNESW);
            m_resizeHandles[2].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[2].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[2].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeNE);
            m_resizeHandles[2].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[3].Tag = MovingRestriction.Horizontal;
            m_resizeHandles[3].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeWE);
            m_resizeHandles[3].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[3].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[3].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeE);
            m_resizeHandles[3].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[4].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[4].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNWSE);
            m_resizeHandles[4].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[4].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[4].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeSE);
            m_resizeHandles[4].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[5].Tag = MovingRestriction.Vertical;
            m_resizeHandles[5].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNS);
            m_resizeHandles[5].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[5].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[5].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeS);
            m_resizeHandles[5].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[6].Tag = MovingRestriction.NoRestriction;
            m_resizeHandles[6].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNESW);
            m_resizeHandles[6].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[6].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[6].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeSW);
            m_resizeHandles[6].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[7].Tag = MovingRestriction.Horizontal;
            m_resizeHandles[7].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeWE);
            m_resizeHandles[7].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[7].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[7].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeW);
            m_resizeHandles[7].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

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

            //m_pathwayCanvas.AddInputEventListener(new MouseDownHandler(m_pathwayView));
        }

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

        #region Methods for System
        /// <summary>
        /// Highlights objects currently surrounded by the selected system.
        /// </summary>
        private void RefreshSurroundState()
        {
            if (m_selectedSystemName == null)
                return;
            ClearSurroundState();
            m_surroundedBySystem = new PNodeList();
            PPathwaySystem topSystem = m_systems[m_selectedSystemName];
            foreach (PLayer layer in Layers.Values)
            {
                PNodeList list = new PNodeList();
                layer.FindIntersectingNodes(topSystem.Rect, list);
                m_surroundedBySystem.AddRange(list);
            }
            foreach (PNode node in m_surroundedBySystem)
            {
                if (node is PPathwayObject)
                    ((PPathwayObject)node).IsHighLighted = true;
            }
        }

        /// <summary>
        /// Turn off highlight for previously surrounded by system objects, and clear resources for managing
        /// surrounding state.
        /// </summary>
        private void ClearSurroundState()
        {
            if (m_surroundedBySystem == null)
                return;
            foreach (PNode node in m_surroundedBySystem)
            {
                if (node is PPathwayObject)
                    ((PPathwayObject)node).IsHighLighted = false;
            }
            m_surroundedBySystem = null;
        }

        /// <summary>
        /// Called when the mouse is up on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_MouseUp(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            // Get selected system
            string systemKey = m_selectedSystemName;
            PPathwaySystem system = m_systems[systemKey];

            // If selected system overlaps another, reset system region.
            if (this.DoesSystemOverlaps(system.GlobalBounds, systemKey))
            {
                ResetSystemResize(system);
                return;
            }
            system.Refresh();

            List<PPathwayObject> objList = GetAllObjects();
            // Select PathwayObjects being moved into current system.
            Dictionary<string, PPathwayObject> currentDict = new Dictionary<string, PPathwayObject>();
            // Select PathwayObjects being moved to upper system.
            Dictionary<string, PPathwayObject> beforeDict = new Dictionary<string, PPathwayObject>();
            foreach (PPathwayObject obj in objList)
            {
                if (system.Rect.Contains(obj.Rect))
                {
                    if (!obj.EcellObject.parentSystemID.StartsWith(systemKey) && !obj.EcellObject.key.Equals(systemKey))
                        currentDict.Add(obj.EcellObject.type + ":" + obj.EcellObject.key, obj);
                }
                else
                {
                    if (obj.EcellObject.parentSystemID.StartsWith(systemKey) && !obj.EcellObject.key.Equals(systemKey))
                        beforeDict.Add(obj.EcellObject.type + ":" + obj.EcellObject.key, obj);
                }
            }

            // If ID duplication could occurred, system resizing will be aborted
            foreach (PPathwayObject obj in currentDict.Values)
            {
                // Check duplicated object.
                if (obj is PPathwaySystem && !m_systems.ContainsKey(systemKey + "/" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayProcess && !m_processes.ContainsKey(systemKey + ":" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayVariable && !m_variables.ContainsKey(systemKey + ":" + obj.EcellObject.name))
                    continue;
                // If duplicated object exists.
                ResetSystemResize(system);
                MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string parentKey = system.EcellObject.parentSystemID;
            foreach (PPathwayObject obj in beforeDict.Values)
            {
                // Check duplicated object.
                if (obj is PPathwaySystem && !m_systems.ContainsKey(parentKey + "/" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayProcess && !m_processes.ContainsKey(parentKey + ":" + obj.EcellObject.name))
                    continue;
                else if (obj is PPathwayVariable && !m_variables.ContainsKey(parentKey + ":" + obj.EcellObject.name))
                    continue;
                // If duplicated object exists.
                ResetSystemResize(system);
                MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Move objects.
            foreach (PPathwayObject obj in currentDict.Values)
            {
                string oldKey = obj.EcellObject.key;
                string oldSyskey = obj.EcellObject.parentSystemID;
                string newKey = null;
                if (obj is PPathwaySystem)
                    newKey = systemKey + "/" + obj.EcellObject.name;
                else
                    newKey = systemKey + ":" + obj.EcellObject.name;
                // Set node change
                this.m_con.NotifyDataChanged(oldKey, newKey, obj, true, true);
            }
            foreach (PPathwayObject obj in beforeDict.Values)
            {
                string oldKey = obj.EcellObject.key;
                string newKey = null;
                if (obj is PPathwaySystem)
                    newKey = parentKey + "/" + obj.EcellObject.name;
                else
                    newKey = parentKey + ":" + obj.EcellObject.name;
                // Set node change
                this.m_con.NotifyDataChanged(oldKey, newKey, obj, true, true);
            }

            // Fire DataChanged for child in system.!
            UpdateResizeHandlePositions();
            ResetSelectedObjects();
            ClearSurroundState();

            // Update systems
            m_con.NotifyDataChanged(
                system.EcellObject.key,
                system.EcellObject.key,
                system,
                true,
                true);
        }

        void ResetSystemResize(PPathwaySystem system)
        {
            // Resizing is aborted
            system.ResetPosition();
            system.Refresh();
            this.ValidateSystem(system);
            UpdateResizeHandlePositions();
            ResetSelectedObjects();
            ClearSurroundState();
        }

        /// <summary>
        /// Called when the mouse is down on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_MouseDown(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            PPathwaySystem system = m_systems[m_selectedSystemName];
            system.MemorizePosition();
            PointF offsetToL = system.Offset;
            PointF lP = new PointF(system.X + offsetToL.X, system.Y + offsetToL.Y);
            m_upperLeftPoint = lP;
            m_upperRightPoint = new PointF(m_upperLeftPoint.X + system.Width, m_upperLeftPoint.Y);
            m_lowerRightPoint = new PointF(m_upperLeftPoint.X + system.Width, m_upperLeftPoint.Y + system.Height);
            m_lowerLeftPoint = new PointF(m_upperLeftPoint.X, m_upperLeftPoint.Y + system.Height);
        }

        /// <summary>
        /// Called when the NorthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeNW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = m_lowerRightPoint.X - X;
            float height = m_lowerRightPoint.Y - Y;
            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                system.X = X;
                system.Y = Y;
                system.Width = width;
                system.Height = height;

                this.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                }
            }
        }

        /// <summary>
        /// Called when the North resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeN(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float height = m_lowerRightPoint.Y - Y;

            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.Y = Y - offsetToL.Y;
                system.Height = height;
                this.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
            }
        }

        /// <summary>
        /// Called when the NorthEast resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeNE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                               - system.X - system.Offset.X;
            float height = m_lowerLeftPoint.Y - Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.Y = Y - offsetToL.Y;
                system.Width = width;
                system.Height = height;
                this.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();

                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                }
            }
        }

        /// <summary>
        /// Called when the East resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                              - system.X - system.Offset.X;
            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                system.Width = width;
                this.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
            }
        }

        /// <summary>
        /// Called when the SouthEast resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeSE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                               - system.X - system.Offset.X;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                                - system.Y - system.Offset.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.Width = width;
                system.Height = height;
                this.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();

                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                }
            }
        }

        /// <summary>
        /// Called when the South resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeS(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                                 - system.Y - system.Offset.Y;

            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                system.Height = height;
                this.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
            }
        }

        /// <summary>
        /// Called when the SouthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeSW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = m_upperRightPoint.X - e.PickedNode.X - e.PickedNode.OffsetX - m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                               - system.Y - system.Offset.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.X = X - offsetToL.X;
                system.Width = width;
                system.Height = height;
                this.ValidateSystem(system);
                system.Refresh();

                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();

                if (width <= PPathwaySystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                }
                if (height <= PPathwaySystem.MIN_Y_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                }
            }
        }

        /// <summary>
        /// Called when the West resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_ResizeW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            PPathwaySystem system = m_systems[m_selectedSystemName];

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = m_lowerRightPoint.X - X;

            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                PointF offsetToL = system.Offset;
                system.X = X - offsetToL.X;
                system.Width = width;
                this.ValidateSystem(system);
                system.Refresh();
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
            }
        }

        /// <summary>
        /// Called when the mouse is off a resize handle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_MouseLeave(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_CursorSizeNWSE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNWSE;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_CursorSizeNS(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNS;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_CursorSizeNESW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNESW;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PPathwaySystem_CursorSizeWE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeWE;
        }
        #endregion

        /// <summary>
        /// Validate a system. According to result, system.Valid will be changed.
        /// </summary>
        /// <param name="system">PEcellSystem to be validated</param>
        protected void ValidateSystem(PPathwaySystem system)
        {
            if (this.DoesSystemOverlaps(system.Rect, system.EcellObject.key))
                system.Valid = false;
            else
                system.Valid = true;
        }

        /// <summary>
        /// Show resize handles for resizing system.
        /// </summary>
        protected void ShowResizeHandles()
        {
            foreach (PNode node in m_resizeHandles)
            {
                m_ctrlLayer.AddChild(node);
            }
        }

        /// <summary>
        /// Reset reside handles' positions.
        /// </summary>
        public void UpdateResizeHandlePositions()
        {
            if (m_selectedSystemName == null || !m_systems.ContainsKey(m_selectedSystemName))
                return;

            PPathwaySystem system = m_systems[m_selectedSystemName];

            PointF gP = new PointF(system.X + system.OffsetX, system.Y + system.OffsetY);

            PPathwaySystem parentSystem = system;

            while (parentSystem.Parent is PPathwaySystem)
            {
                parentSystem = (PPathwaySystem)parentSystem.Parent;
                parentSystem.LocalToParent(gP);
                gP.X = gP.X + parentSystem.OffsetX;
                gP.Y = gP.Y + parentSystem.OffsetY;
            }

            float halfThickness = PPathwaySystem.HALF_THICKNESS;
            m_resizeHandles[0].SetOffset(gP.X + halfThickness, gP.Y + halfThickness);
            m_resizeHandles[1].SetOffset(gP.X + system.Width / 2f, gP.Y + halfThickness);
            m_resizeHandles[2].SetOffset(gP.X + system.Width - halfThickness, gP.Y + halfThickness);
            m_resizeHandles[3].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height / 2f);
            m_resizeHandles[4].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height - halfThickness);
            m_resizeHandles[5].SetOffset(gP.X + system.Width / 2f, gP.Y + system.Height - halfThickness);
            m_resizeHandles[6].SetOffset(gP.X + halfThickness, gP.Y + system.Height - halfThickness);
            m_resizeHandles[7].SetOffset(gP.X + halfThickness, gP.Y + system.Height / 2f);
        }

        /// <summary>
        /// Reset reside handles' positions except one fixedHandle
        /// </summary>
        /// <param name="fixedHandle">this ResizeHandle must not be updated</param>
        public void UpdateResizeHandlePositions(PNode fixedHandle)
        {
            if (m_selectedSystemName == null)
                return;

            PPathwaySystem system = m_systems[m_selectedSystemName];

            PointF gP = new PointF(system.X + system.OffsetX, system.Y + system.OffsetY);

            PPathwaySystem parentSystem = system;

            while (parentSystem.Parent is PPathwaySystem)
            {
                parentSystem = (PPathwaySystem)parentSystem.Parent;
                parentSystem.LocalToParent(gP);
                gP.X = gP.X + parentSystem.OffsetX;
                gP.Y = gP.Y + parentSystem.OffsetY;
            }

            float halfOuterRadius = PPathwaySystem.OUTER_RADIUS / 2f;
            float halfThickness = (PPathwaySystem.OUTER_RADIUS - PPathwaySystem.INNER_RADIUS) / 2;
            if (m_resizeHandles[0] != fixedHandle)
                m_resizeHandles[0].SetOffset(gP.X + halfThickness, gP.Y + halfThickness);
            if (m_resizeHandles[1] != fixedHandle)
                m_resizeHandles[1].SetOffset(gP.X + system.Width / 2f, gP.Y + halfThickness);
            if (m_resizeHandles[2] != fixedHandle)
                m_resizeHandles[2].SetOffset(gP.X + system.Width - halfThickness, gP.Y + halfThickness);
            if (m_resizeHandles[3] != fixedHandle)
                m_resizeHandles[3].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height / 2f);
            if (m_resizeHandles[4] != fixedHandle)
                m_resizeHandles[4].SetOffset(gP.X + system.Width - halfThickness, gP.Y + system.Height - halfThickness);
            if (m_resizeHandles[5] != fixedHandle)
                m_resizeHandles[5].SetOffset(gP.X + system.Width / 2f, gP.Y + system.Height - halfThickness);
            if (m_resizeHandles[6] != fixedHandle)
                m_resizeHandles[6].SetOffset(gP.X + halfThickness, gP.Y + system.Height - halfThickness);
            if (m_resizeHandles[7] != fixedHandle)
                m_resizeHandles[7].SetOffset(gP.X + halfThickness, gP.Y + system.Height / 2f);
        }

        /// <summary>
        /// Notify SelectChanged event to outside.
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        /// <param name="isSelect">the flag whether this object is selected.</param>
        /// </summary>
        public void NotifySelectChanged(string key, string type, bool isSelect)
        {
            if (m_con != null)
                m_con.Window.NotifySelectChanged(this.m_modelId, key, type, isSelect);
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
        /// Transfer an object from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName">The name of the system to which object is transfered. If null, obj is
        /// transfered to layer itself</param>
        /// <param name="obj">transfered object</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void TransferNodeToByResize(string systemName, PPathwayObject obj, bool isAnchor)
        {
            // The case that obj is transfered to PEcellSystem.
            PPathwaySystem system = m_systems[systemName];
            if (system.Layer == obj.Layer && system != obj)
            {
                PPathwayObject po = (PPathwayObject)obj.Parent;
                po.RemoveChild(po.IndexOfChild(obj));
                system.AddChild(obj);
                if (system.IndexOfChild(po) < 0)
                {
                    obj.OffsetX -= system.OffsetX;
                    obj.OffsetY -= system.OffsetY;
                }
                else
                {
                    obj.OffsetX += po.OffsetX;
                    obj.OffsetY += po.OffsetY;
                }
                obj.X += obj.OffsetX;
                obj.Y += obj.OffsetY;
                obj.OffsetX = 0;
                obj.OffsetY = 0;
                if (obj is PPathwayNode)
                {
                    ((PPathwayNode)obj).ParentObject = system;
                }
            }
            if (obj is PPathwayVariable)
            {
                PPathwayVariable var = (PPathwayVariable)obj;
                string newKey = systemName + ":" + var.EcellObject.name;
                string oldKey = var.EcellObject.key;
                if (!oldKey.Equals(newKey))
                    m_con.NotifyDataChanged(
                        oldKey,
                        newKey,
                        obj,
                        true,
                        isAnchor);
            }
            else if (obj is PPathwayProcess)
            {
                PPathwayProcess pro = (PPathwayProcess)obj;
                string newKey = systemName + ":" + pro.EcellObject.name;
                string oldKey = pro.EcellObject.key;
                if (!oldKey.Equals(newKey))
                    m_con.NotifyDataChanged(
                        oldKey,
                        newKey,
                        obj,
                        true,
                        isAnchor);
            }
        }

        /// <summary>
        /// Hide resize handles for resizing system.
        /// </summary>
        protected void HideResizeHandles()
        {
            foreach (PNode node in m_resizeHandles)
                if (node.Parent == m_ctrlLayer)
                    m_ctrlLayer.RemoveChild(node);
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
        /// <param name="excludeName">The system with this name will NOT be taken into account</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
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
        /// <param name="layer"></param>
        /// <param name="systemName"></param>
        /// <param name="obj"></param>
        /// <param name="hasCoords"></param>
        /// <param name="isFirst"></param>
        public void AddNewObj(string layer, string systemName, PPathwayObject obj, bool hasCoords, bool isFirst)
        {
            ResetSelectedObjects();
            if (layer == null && Layers.Count == 1)
                foreach (string key in Layers.Keys)
                    layer = key;

            if (obj is PPathwayNode)
                ((PPathwayNode)obj).ShowingID = this.m_showingId;
            RegisterObjToSet(obj);
            // Set Root System
            if (systemName == null || systemName.Equals("") )
            {
                obj.Layer = Layers[layer];
                Layers[layer].AddChild(obj);
                return;
            }

            // Set Child object.
            PPathwaySystem system = m_systems[systemName];
            // If obj hasn't coordinate, it will be settled. 
            if (!hasCoords)
            {
                if (obj is PPathwayNode)
                    obj.PointF = GetVacantPoint(systemName);
                else if(obj is PPathwaySystem)
                {
                    float maxX = system.X + system.OffsetX;
                    float x = 0f;

                    foreach (PNode ppo in system.ChildrenReference)
                    {
                        if (ppo is PPathwayObject)
                        {
                            x = ppo.X + ppo.OffsetX + ppo.Width;
                        }
                        if (maxX < x)
                            maxX = x;
                    }
                    // Set obj's coordinate
                    obj.X = maxX + PPathwaySystem.SYSTEM_MARGIN;
                    obj.Y = system.Y + system.Offset.Y + PPathwaySystem.SYSTEM_MARGIN;
                    obj.Width = PPathwaySystem.DEFAULT_WIDTH;
                    obj.Height = PPathwaySystem.DEFAULT_HEIGHT;

                    system.MakeSpace(obj);
                }
            }
            // Set to parent object.
            system.AddChild(obj);
            obj.ParentObject = system;
            obj.RefreshText();

            if (obj is PPathwayProcess)
                ((PPathwayProcess)obj).RefreshEdges();
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

        /// <summary>
        /// add the selected layer.
        /// </summary>
        /// <param name="name">the added layer.</param>
        public void AddLayer(string name)
        {
            PLayer layer = new PLayer();
            layer.AddInputEventListener(new NodeDragHandler(this, m_systems));
            layer.Visible = true;

            m_pCanvas.Root.AddChild(0, layer);
            m_pCanvas.Camera.AddLayer(0, layer);

            DataRow dr = m_table.NewRow();
            dr[COLUMN_NAME4SHOW] = true;
            dr[COLUMN_NAME4NAME] = name;
            m_table.Rows.Add(dr);

            //m_con.OverView.AddLayer(layer);
            m_overviewCanvas.AddObservedLayer(layer);

            Layers.Add(name, layer);
            ControlLayer.MoveToFront();

        }

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
        /// Notify this set that one PPathwayNode is selected
        /// </summary>
        /// <param name="obj">Newly selected object</param>
        /// <param name="toBeNotified">Whether selection must be notified to Ecell-Core or not.</param>
        public void AddSelectedNode(PPathwayNode obj, bool toBeNotified)
        {
            m_selectedNodes.Add(obj);
            obj.IsHighLighted = true;
            if (toBeNotified)
                NotifySelectChanged(obj.EcellObject.key, obj.EcellObject.type, true);
        }

        /// <summary>
        /// Notify this set that one PEcellSystem is selected.
        /// </summary>
        /// <param name="systemName">the name of selected system.</param>
        public void AddSelectedSystem(string systemName)
        {
            m_selectedSystemName = systemName;
            if (m_systems.ContainsKey(systemName))
                m_systems[systemName].IsHighLighted = true;
            ShowResizeHandles();
            UpdateResizeHandlePositions();
            string type = m_systems[systemName].EcellObject.type;
            NotifySelectChanged(systemName, type, true);
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
            else if (m_line4reconnect.Parent != null)
            {
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
            foreach (PPathwaySystem sysCon in m_systems.Values)
                sysCon.Freeze();
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
            obj.ParentObject.RemoveChild(obj);
            obj.ParentObject = m_systems[sysKey];
            obj.ParentObject.AddChild(obj);

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

        private void RemoveObject(PPathwayObject obj)
        {
            if (obj == null)
                return;

            obj.Text.RemoveFromParent();
            obj.Parent.RemoveChild(obj);
        }

        private void RemoveNodeUnder(string sysKey)
        {
            foreach (PPathwayObject obj in GetNodeList())
                if (obj.EcellObject.key.StartsWith(sysKey) && !obj.EcellObject.key.Equals(sysKey) )
                    DataDelete(obj.EcellObject.key, obj.EcellObject.type);
        }

        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void SelectChanged(string key, ComponentType type)
        {
            switch (type)
            {
                case ComponentType.System:
                    if (m_selectedSystemName != null && m_selectedSystemName.Equals(key))
                        return;
                    this.ResetSelectedObjects();
                    this.AddSelectedSystem(key);
                    PPathwaySystem focusSystem = m_systems[key];
                    if (null != focusSystem)
                    {
                        m_pCanvas.Camera.AnimateViewToCenterBounds(focusSystem.FullBounds,
                                                                             true,
                                                                             CAMERA_ANIM_DURATION);

                        UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
                    }
                    break;
                case ComponentType.Variable:
                    bool isAlreadySelected = false;
                    foreach (PPathwayObject selectNode in m_selectedNodes)
                    {
                        if (key.Equals(selectNode.EcellObject.key) && selectNode is PPathwayVariable)
                        {
                            isAlreadySelected = true;
                            break;
                        }
                    }
                    if (!isAlreadySelected)
                    {
                        this.ResetSelectedObjects();
                        if (m_variables.ContainsKey(key))
                        {
                            PPathwayNode focusNode = (PPathwayNode)m_variables[key];
                            this.AddSelectedNode(focusNode, false);
                            m_pCanvas.Camera.AnimateViewToCenterBounds(PathUtil.GetFocusBound(focusNode.FullBounds, LEAST_FOCUS_SIZE),
                                                                             true,
                                                                             CAMERA_ANIM_DURATION);
                            UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
                        }
                    }
                    break;
                case ComponentType.Process:
                    bool isProAlreadySelected = false;
                    foreach (PPathwayObject selectNode in m_selectedNodes)
                    {
                        if (key.Equals(selectNode.EcellObject.key) && selectNode is PPathwayProcess)
                        {
                            isProAlreadySelected = true;
                            break;
                        }
                    }
                    if (!isProAlreadySelected)
                    {
                        this.ResetSelectedObjects();
                        PPathwayNode focusNode = (PPathwayNode)m_processes[key];
                        this.AddSelectedNode(focusNode, false);
                        m_pCanvas.Camera.AnimateViewToCenterBounds(PathUtil.GetFocusBound(focusNode.FullBounds, LEAST_FOCUS_SIZE),
                                                                         true,
                                                                         CAMERA_ANIM_DURATION);
                        UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
                    }
                    break;
            }
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
            //m_con.OverView.DisplayedArea.Reset();
            //m_con.OverView.DisplayedArea.Offset = PointF.Empty;
            //m_con.OverView.DisplayedArea.AddRectangle(recf.X, recf.Y, recf.Width, recf.Height);
            //m_con.OverView.UpdateTransparent();
            //m_con.OverView.Canvas.Refresh();
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
            HideResizeHandles();
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

            if (m_selectedLine != null)
            {
                m_processes[m_selectedLine.Info.ProcessKey].Refresh();
                m_selectedLine = null;
            }

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
            if (isAnyVisible)
                m_ctrlLayer.Visible = true;
            else
                m_ctrlLayer.Visible = false;
            m_pCanvas.Refresh();
        }

        /// <summary>
        /// Return true if EcellSystem contains a point.
        /// </summary>
        /// <param name="eo">EcellSystem object.</param>
        /// <returns>bool</returns>
        public bool CheckNodePosition(EcellObject eo)
        {
            return CheckNodePosition(eo.parentSystemID, eo.PointF);
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