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
// edited by Chihiro Okada <okada@cbo.mss.co.jp>,
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
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using EcellLib.PathwayWindow.Node;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Element;
using EcellLib.PathwayWindow.Handler;
using PathwayWindow;
using PathwayWindow.UIComponent;
using UMD.HCIL.PiccoloX.Components;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// This class manages resources related to one canvas.
    ///  ex) PathwayCanvas, Layer, etc.
    /// </summary>
    public class CanvasView : IDisposable
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
        /// Used for setting node's offset to point (0,0)
        /// </summary>
        private static readonly PointF ZERO_POINT = new PointF(0, 0);

        /// <summary>
        /// Name of DataColumn for setting layer visibilities (check box)
        /// </summary>
        private static readonly string COLUMN_NAME4SHOW = "Show";

        /// <summary>
        /// Name of DataColumn for indicating layer names (string)
        /// </summary>
        private static readonly string COLUMN_NAME4NAME = "Name";

        /// <summary>
        /// Key definition of m_cMenuDict for delete
        /// </summary>
        public static readonly string CANVAS_MENU_DELETE = "delete";

        /// <summary>
        /// Key definition of m_cMenuDict for delete
        /// </summary>
        public static readonly string CANVAS_MENU_DELETE_WITH = "deletewith";

        /// <summary>
        /// Key definition of m_cMenuDict for delete
        /// </summary>
        public static readonly string CANVAS_MENU_CREATE_LOGGER = "create logger";

        /// <summary>
        /// Key definition of m_cMenuDict for delete
        /// </summary>
        public static readonly string CANVAS_MENU_DELETE_LOGGER = "delete logger";

        /// <summary>
        /// Key definition of m_cMenuDict for separator1
        /// </summary>
        public static readonly string CANVAS_MENU_SEPARATOR1 = "separator1";

        /// <summary>
        /// Key definition of m_cMenuDict for separator2
        /// </summary>
        public static readonly string CANVAS_MENU_SEPARATOR2 = "separator2";

        /// <summary>
        /// Key definition of m_cMenuDict for separator3
        /// </summary>
        public static readonly string CANVAS_MENU_SEPARATOR3 = "separator3";

        /// <summary>
        /// Key definition of m_cMenuDict for separator4
        /// </summary>
        public static readonly string CANVAS_MENU_SEPARATOR4 = "separator4";

        /// <summary>
        /// Key definition of m_cMenuDict for rightArrow
        /// </summary>
        public static readonly string CANVAS_MENU_RIGHT_ARROW = "rightArrow";

        /// <summary>
        /// Key definition of m_cMenuDict for leftArrow
        /// </summary>
        public static readonly string CANVAS_MENU_LEFT_ARROW = "leftArrow";

        /// <summary>
        /// Key definition of m_cMenuDict for bidirArrow
        /// </summary>
        public static readonly string CANVAS_MENU_BIDIR_ARROW = "bidirArrow";

        /// <summary>
        /// Key definition of m_cMenuDict for ID
        /// </summary>
        public static readonly string CANVAS_MENU_ID = "id";

        /// <summary>
        /// Key definition of m_cMenuDict for constantLine
        /// </summary>
        public static readonly string CANVAS_MENU_CONSTANT_LINE = "constantLine";
        #endregion

        #region Fields
        /// <summary>
        /// The PathwayView, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        protected PathwayView m_pathwayView;

        /// <summary>
        /// The unique ID of this canvas.
        /// </summary>
        protected string m_canvasId;

        /// <summary>
        /// Tab page for this canvas.
        /// </summary>
        protected TabPage m_pathwayTabPage;

        /// <summary>
        /// PCanvas for pathways.
        /// </summary>
        protected PathwayCanvas m_pathwayCanvas;

        /// <summary>
        /// A button shown on left-lower corner of a main pathway display
        /// for showing overview and layer control
        /// </summary>
        protected ShowBtnDownward m_showBtnDownward;

        /// <summary>
        /// A button shown on left-lower corner of a main pathway display 
        /// for hiding overview and layer control
        /// </summary>
        protected ShowBtnUpward m_showBtnUpward;

        /// <summary>
        /// the canvas of overview.
        /// </summary>
        protected OverviewCanvas m_overviewCanvas;

        /// <summary>
        /// Display rectangles using overview.
        /// </summary>
        protected PDisplayedArea m_area;

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
        protected Dictionary<string, SystemContainer> m_systems = new Dictionary<string,SystemContainer>();

        /// <summary>
        /// The dictionary for all variables on this canvas.
        /// </summary>
        protected Dictionary<string, PEcellVariable> m_variables = new Dictionary<string, PEcellVariable>();

        /// <summary>
        /// The dictionary for all processes on this canvas.
        /// </summary>
        protected Dictionary<string, PEcellProcess> m_processes = new Dictionary<string, PEcellProcess>();
                
        /// <summary>
        /// PLayer for control use.
        /// For example, resize handlers for PEcellSystem.
        /// </summary>
        protected PLayer m_ctrlLayer;

        /// <summary>
        /// Width of a show button.
        /// </summary>
        float m_showBtnWidth = 15;

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
        List<PPathwayNode> m_selectedNodes = new List<PPathwayNode>();

        /// <summary>
        /// selected line
        /// </summary>
        Line m_selectedLine = null;

        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        Dictionary<string, ToolStripItem> m_cMenuDict = new Dictionary<string, ToolStripItem>();

        /// <summary>
        /// The name of system currently selected on the canvas.
        /// </summary>
        string m_selectedSystemName;

        /// <summary>
        /// PPathwayObject, which is to be connected.
        /// </summary>
        PPathwayNode m_nodeToBeConnected;

        /// <summary>
        /// List of PNodes which belong to a system before the system resizing started.
        /// </summary>
        PNodeList m_systemChildrenBeforeDrag = null;

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

        /// <summary>
        /// Whether an overview should be refreshed or not
        /// </summary>
        protected bool m_isRefreshOverview = false;

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
        PPath m_line4reconnect = null;

        /// <summary>
        /// Variable or Process.
        /// </summary>
        PathwayElement.ElementType m_reconnectNodeType;

        /// <summary>
        /// Stack for nodes under the mouse.
        /// this will be used to reconnect edge.
        /// </summary>
        Stack<NodeElement> m_nodesUnderMouse = new Stack<NodeElement>();
        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_pathwayView.
        /// </summary>
        public PathwayView View
        {
            get { return m_pathwayView; }
            set { m_pathwayView = value; }
        }

        /// <summary>
        /// Accessor for m_canvasId.
        /// </summary>
        public string CanvasID
        {
            get { return m_canvasId; }
            set { m_canvasId = value; }
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
            get { return m_pathwayCanvas; }
        }

        /// <summary>
        /// Accessor for m_showBtnDownward.
        /// </summary>
        public ShowBtnDownward ShowBtnDownward
        {
            get { return m_showBtnDownward; }
        }

        /// <summary>
        /// Accessor for m_showBtnUpward.
        /// </summary>
        public ShowBtnUpward ShowBtnUpward
        {
            get { return m_showBtnUpward; }
        }

        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public List<PPathwayNode> SelectedNodes
        {
            get { return m_selectedNodes; }
        }

        /// <summary>
        /// Accessor for m_overviewCanvas.
        /// </summary>
        public PCanvas OverviewCanvas
        {
            get { return m_overviewCanvas; }
        }

        /// <summary>
        /// Accessor for m_area.
        /// </summary>
        public PDisplayedArea DisplayedArea
        {
            get { return m_area; }
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
        public Dictionary<string, SystemContainer> Systems
        {
            get { return m_systems; }
        }

        /// <summary>
        /// Accessor for m_variables.
        /// </summary>
        public Dictionary<string, PEcellVariable> Variables
        {
            get { return m_variables; }
        }

        /// <summary>
        /// Accessor for m_processes.
        /// </summary>
        public Dictionary<string, PEcellProcess> Processes
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
                foreach(PPathwayNode pnode in m_variables.Values)
                {
                    pnode.ShowingID = m_showingId;
                }
                foreach (PPathwayNode pnode in m_processes.Values)
                {
                    pnode.ShowingID = m_showingId;
                }
                foreach(SystemContainer system in m_systems.Values)
                {
                    if (m_showingId)
                        system.Text.Visible = true;
                    else
                        system.Text.Visible = false;
                }
            }
        }

        /// <summary>
        /// Accessor for m_isRefreshOverview.
        /// </summary>
        public bool RefreshOverview
        {
            get { return m_isRefreshOverview; }
            set
            {
                m_isRefreshOverview = value;
            }
        }

        /// <summary>
        /// Accessor for m_cMenuDict.
        /// </summary>
        public Dictionary<string, ToolStripItem> ContextMenuDict
        {
            get { return m_cMenuDict; }
        }

        /// <summary>
        /// Accessor for m_line4reconnect.
        /// </summary>
        public PPath Line4Reconnect
        {
            get { return m_line4reconnect; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// the constructor with initial parameters.
        /// </summary>
        /// <param name="view">PathwayView.</param>
        /// <param name="name">canvas id.</param>
        /// <param name="lowerPanelShown">whether show lower panel.</param>
        /// <param name="overviewScale">scale of overview.</param>
        /// <param name="handler">EventHandler of PathwayView.</param>
        public CanvasView(PathwayView view, string name, 
            bool lowerPanelShown, float overviewScale, 
            PInputEventHandler handler)
        {
            m_pathwayView = view;
            m_canvasId = name;

            // Preparing show button for showing/hiding overview panel and layer control panel
            m_showBtnDownward = new ShowBtnDownward(new Pen(Brushes.Black),
                                                    SHOW_BTN_BG_BRUSH,
                                                    SHOW_BTN_ARROW_BRUSH,
                                                    SHOW_BTN_SHADOW_BRUSH);
            m_showBtnDownward.Click += handler;
            m_showBtnUpward = new ShowBtnUpward(new Pen(Brushes.Black),
                                                    SHOW_BTN_BG_BRUSH,
                                                    SHOW_BTN_ARROW_BRUSH,
                                                    SHOW_BTN_SHADOW_BRUSH);
            m_showBtnUpward.Click += handler;
            
            // Preparing TabPage
            m_pathwayTabPage = new TabPage(name);
            m_pathwayTabPage.Name = name;
            m_pathwayTabPage.AutoScroll = true;

            m_pathwayCanvas = new PathwayCanvas(this);
            
            m_pathwayCanvas.RemoveInputEventListener(m_pathwayCanvas.PanEventHandler);
            m_pathwayCanvas.RemoveInputEventListener(m_pathwayCanvas.ZoomEventHandler);
            m_pathwayCanvas.AddInputEventListener(new PPathwayZoomEventHandler(m_pathwayView));
            m_pathwayCanvas.Dock = DockStyle.Fill;
            //m_pathwayCanvas.MouseDown += new MouseEventHandler(m_pathwayCanvas_MouseDown);
            //m_pathwayCanvas.MouseMove += new MouseEventHandler(m_pathwayCanvas_MouseMove);
            m_pathwayCanvas.Camera.AddChild(m_showBtnDownward);
            m_pathwayCanvas.Name = name;
            //m_pathwayCanvas.Camera.Scale = DEFAULT_CAMERA_SCALE;
            m_pathwayCanvas.Camera.ScaleViewBy(0.7f);
            if (lowerPanelShown)
                m_showBtnDownward.Visible = true;
            else
                m_showBtnUpward.Visible = true;

            PScrollableControl scrolCtrl = new PScrollableControl(m_pathwayCanvas);
            scrolCtrl.Layout += new LayoutEventHandler(scrolCtrl_Layout);
            scrolCtrl.Dock = DockStyle.Fill;
            m_pathwayTabPage.Controls.Add(scrolCtrl);
            //m_pathwayTabPage.Controls.Add(m_pathwayCanvas);

            // Preparing overview
            m_area = new PDisplayedArea();
            m_overviewCanvas = new OverviewCanvas(overviewScale,
                                                  m_pathwayCanvas.Layer,
                                                  m_pathwayCanvas.Camera,
                                                  m_area);

            m_pathwayCanvas.Camera.RemoveLayer(m_pathwayCanvas.Layer);
            
            // Preparing DataTable
            m_table = new DataTable(name);
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
            m_pathwayCanvas.Root.AddChild(m_ctrlLayer);
            m_pathwayCanvas.Camera.AddLayer(m_ctrlLayer);

            // Preparing a context menu.
            m_nodeMenu = new ContextMenuStrip();
            m_nodeMenu.Closed += new ToolStripDropDownClosedEventHandler(m_nodeMenu_Closed);

            ToolStripItem idShow = new ToolStripMenuItem("tmp");
            idShow.Name = CANVAS_MENU_ID;
            m_nodeMenu.Items.Add(idShow);
            m_cMenuDict.Add(CANVAS_MENU_ID, idShow);

            ToolStripSeparator separator3 = new ToolStripSeparator();
            m_nodeMenu.Items.Add(separator3);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR3, separator3);

            int count = 0;
            foreach(ILayoutAlgorithm algorithm in m_pathwayView.Window.LayoutAlgorithm)
            {
                ToolStripMenuItem algoItem = new ToolStripMenuItem(algorithm.GetMenuText());
                algoItem.Tag = count;
                algoItem.ToolTipText = algorithm.GetToolTipText();
                algoItem.Click += new EventHandler(m_pathwayView.Window.eachLayoutItem_Click);

                List<string> subCommands = algorithm.GetSubCommands();
                if (subCommands != null && subCommands.Count != 0)
                {
                    int subcount = 0;
                    foreach(string subCommandName in subCommands)
                    {
                        ToolStripMenuItem layoutSubItem = new ToolStripMenuItem();
                        layoutSubItem.Text = subCommandName;
                        layoutSubItem.Tag = count + "," + subcount;
                        layoutSubItem.Click += new EventHandler(m_pathwayView.Window.eachLayoutItem_Click);
                        algoItem.DropDownItems.Add(layoutSubItem);
                        subcount++;
                    }
                }

                m_nodeMenu.Items.Add(algoItem);

                count++;
            }

            ToolStripSeparator separator1 = new ToolStripSeparator();
            m_nodeMenu.Items.Add(separator1);
            m_cMenuDict.Add( CANVAS_MENU_SEPARATOR1, separator1 );

            ToolStripItem rightArrow = new ToolStripMenuItem("Process -> Variable", Resource1.arrow_long_right_w);
            rightArrow.Name = CANVAS_MENU_RIGHT_ARROW;
            rightArrow.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(rightArrow);
            m_cMenuDict.Add( CANVAS_MENU_RIGHT_ARROW, rightArrow );

            ToolStripItem leftArrow = new ToolStripMenuItem("Process <- Variable", Resource1.arrow_long_left_w);
            leftArrow.Name = CANVAS_MENU_LEFT_ARROW;
            leftArrow.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(leftArrow);
            m_cMenuDict.Add( CANVAS_MENU_LEFT_ARROW, leftArrow );

            ToolStripItem bidirArrow = new ToolStripMenuItem("Process <-> Variable", Resource1.arrow_long_bidir_w);
            bidirArrow.Name = CANVAS_MENU_BIDIR_ARROW;
            bidirArrow.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(bidirArrow);
            m_cMenuDict.Add( CANVAS_MENU_BIDIR_ARROW, bidirArrow );

            ToolStripItem constant = new ToolStripMenuItem("Constant", Resource1.ten);
            constant.Name = CANVAS_MENU_CONSTANT_LINE;
            constant.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(constant);
            m_cMenuDict.Add( CANVAS_MENU_CONSTANT_LINE, constant );

            ToolStripSeparator separator2 = new ToolStripSeparator();
            m_nodeMenu.Items.Add(separator2);
            m_cMenuDict.Add( CANVAS_MENU_SEPARATOR2, separator2);

            ToolStripItem delete = new ToolStripMenuItem(m_resources.GetString("DeleteMenuText"));
            delete.Click += new EventHandler(DeleteClick);
            m_nodeMenu.Items.Add(delete);
            m_cMenuDict.Add(CANVAS_MENU_DELETE, delete);

            ToolStripItem deleteWith = new ToolStripMenuItem(m_resources.GetString("MergeMenuText"));
            deleteWith.Click += new EventHandler(MergeClick);
            m_nodeMenu.Items.Add(deleteWith);
            m_cMenuDict.Add(CANVAS_MENU_DELETE_WITH, deleteWith);

            ToolStripSeparator separator4 = new ToolStripSeparator();
            m_nodeMenu.Items.Add(separator4);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR4, separator4);
            
            // Create EntityListLogger
            ToolStripItem createLogger = new ToolStripMenuItem(m_resources.GetString("CreateLogMenuText"));
            createLogger.Click += new EventHandler(CreateLoggerClick);
            m_nodeMenu.Items.Add(createLogger);
            m_cMenuDict.Add(CANVAS_MENU_CREATE_LOGGER, createLogger);

            // Delete EntityListLogger
            ToolStripItem deleteLogger = new ToolStripMenuItem(m_resources.GetString("DeleteLogMenuText"));
            deleteLogger.Click += new EventHandler(DeleteLoggerClick);
            m_nodeMenu.Items.Add(deleteLogger);
            m_cMenuDict.Add(CANVAS_MENU_DELETE_LOGGER, deleteLogger);
            
#if DEBUG
            ToolStripItem debug = new ToolStripMenuItem("Debug");
            debug.Click += new EventHandler(DebugClick);
            m_nodeMenu.Items.Add(debug);
            //m_cMenuDict.Add(CANVAS_MENU_DELETE, delete);
#endif
            m_pathwayCanvas.ContextMenuStrip = m_nodeMenu;

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
            
            m_resizeHandles[0].Tag = ResizeHandleDragHandler.MovingRestriction.NoRestriction;
            m_resizeHandles[0].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeNWSE);
            m_resizeHandles[0].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[0].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[0].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeNW);
            m_resizeHandles[0].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            m_resizeHandles[1].Tag = ResizeHandleDragHandler.MovingRestriction.Vertical;
            m_resizeHandles[1].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeNS);
            m_resizeHandles[1].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[1].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[1].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeN);
            m_resizeHandles[1].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            m_resizeHandles[2].Tag = ResizeHandleDragHandler.MovingRestriction.NoRestriction;
            m_resizeHandles[2].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeNESW);
            m_resizeHandles[2].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[2].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[2].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeNE);
            m_resizeHandles[2].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            m_resizeHandles[3].Tag = ResizeHandleDragHandler.MovingRestriction.Horizontal;
            m_resizeHandles[3].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeWE);
            m_resizeHandles[3].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[3].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[3].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeE);
            m_resizeHandles[3].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            m_resizeHandles[4].Tag = ResizeHandleDragHandler.MovingRestriction.NoRestriction;
            m_resizeHandles[4].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeNWSE);
            m_resizeHandles[4].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[4].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[4].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeSE);
            m_resizeHandles[4].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            m_resizeHandles[5].Tag = ResizeHandleDragHandler.MovingRestriction.Vertical;
            m_resizeHandles[5].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeNS);
            m_resizeHandles[5].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[5].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[5].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeS);
            m_resizeHandles[5].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            m_resizeHandles[6].Tag = ResizeHandleDragHandler.MovingRestriction.NoRestriction;
            m_resizeHandles[6].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeNESW);
            m_resizeHandles[6].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[6].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[6].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeSW);
            m_resizeHandles[6].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            m_resizeHandles[7].Tag = ResizeHandleDragHandler.MovingRestriction.Horizontal;
            m_resizeHandles[7].MouseEnter += new PInputEventHandler(PEcellSystem_CursorSizeWE);
            m_resizeHandles[7].MouseLeave += new PInputEventHandler(PEcellSystem_MouseLeave);
            m_resizeHandles[7].MouseDown += new PInputEventHandler(PEcellSystem_MouseDown);
            m_resizeHandles[7].MouseDrag += new PInputEventHandler(PEcellSystem_ResizeW);
            m_resizeHandles[7].MouseUp += new PInputEventHandler(PEcellSystem_MouseUp);

            // Prepare line handles
            m_lineHandle4V = new PPath();
            m_lineHandle4V.Brush = new SolidBrush(Color.FromArgb(125, Color.Orange));
            m_lineHandle4V.Pen = new Pen(Brushes.DarkCyan, 1);
            m_lineHandle4V.AddEllipse(
                0,
                0,
                2 * LINE_HANDLE_RADIUS,
                2 * LINE_HANDLE_RADIUS);
            m_lineHandle4V.Tag = PathwayElement.ElementType.Variable;
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
            m_lineHandle4P.Tag = PathwayElement.ElementType.Process;
            m_lineHandle4P.MouseDown += new PInputEventHandler(m_lineHandle_MouseDown);
            m_lineHandle4P.MouseDrag += new PInputEventHandler(m_lineHandle_MouseDrag);
            m_lineHandle4P.MouseUp += new PInputEventHandler(m_lineHandle_MouseUp);

            m_line4reconnect = new PPath();
            m_line4reconnect.Brush = new SolidBrush(Color.FromArgb(200, Color.Orange));
            m_line4reconnect.Pen = LINE_THIN_PEN;
            m_line4reconnect.Pickable = false;

            //m_pathwayCanvas.AddInputEventListener(new MouseDownHandler(m_pathwayView));
        }

        void scrolCtrl_Layout(object sender, LayoutEventArgs e)
        {
            m_pathwayView.UpdateShowButton();
        }

        /// <summary>
        /// Called when the mouse is down on m_lineHandle.
        /// Start to reconnecting edge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseDown(object sender, PInputEventArgs e)
        {
            m_reconnectNodeType = (PathwayElement.ElementType)e.PickedNode.Tag;
            if(m_reconnectNodeType == PathwayElement.ElementType.Process && null != m_ctrlLayer)
            {
                m_ctrlLayer.RemoveChild(m_lineHandle4P);
            }
            else if(m_reconnectNodeType == PathwayElement.ElementType.Variable && null != m_ctrlLayer)
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
            if(null == m_line4reconnect || null == m_selectedLine)
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

            switch (m_selectedLine.Info.TypeOfLine)
            {
                case LineType.Solid:
                    m_line4reconnect.AddLine(vpoint.X, vpoint.Y, ppoint.X, ppoint.Y);
                    break;
                case LineType.Dashed:
                    PEcellProcess.AddDashedLine(m_line4reconnect, vpoint.X, vpoint.Y, ppoint.X, ppoint.Y);
                    break;
                case LineType.Unknown:
                    m_line4reconnect.AddLine(vpoint.X, vpoint.Y, ppoint.X, ppoint.Y);
                    break;
            }
            
            switch (m_selectedLine.Info.Direction)
            {
                case EdgeDirection.Bidirection:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(ppoint, vpoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(vpoint, ppoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.Inward:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(ppoint, vpoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.Outward:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(vpoint, ppoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.None:
                    break;
            }
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
                NodeElement element = m_nodesUnderMouse.Pop();
                if(element is ProcessElement && m_reconnectNodeType == PathwayElement.ElementType.Process)
                {                    
                    m_pathwayView.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_pathwayView.NotifyVariableReferenceChanged(element.Key, m_vOnLinesEnd, RefChangeType.BiDir, 0);
                    }
                    else
                    {
                        int coefficient = 0;
                        switch (m_selectedLine.Info.Direction)
                        {
                            case EdgeDirection.Inward:
                                coefficient = -1;
                                break;
                            case EdgeDirection.None:
                                coefficient = 0;
                                break;
                            case EdgeDirection.Outward:
                                coefficient = 1;
                                break;
                        }
                        m_pathwayView.NotifyVariableReferenceChanged(
                            element.Key,
                            m_vOnLinesEnd,
                            RefChangeType.SingleDir,
                            coefficient);
                    }
                    ResetSelectedLine();
                }
                else if(element is VariableElement && m_reconnectNodeType  == PathwayElement.ElementType.Variable)
                {                    
                    m_pathwayView.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_pathwayView.NotifyVariableReferenceChanged(m_pOnLinesEnd, element.Key, RefChangeType.BiDir, 0);
                    }
                    else
                    {
                        int coefficient = 0;
                        switch (m_selectedLine.Info.Direction)
                        {
                            case EdgeDirection.Inward:
                                coefficient = -1;
                                break;
                            case EdgeDirection.None:
                                coefficient = 0;
                                break;
                            case EdgeDirection.Outward:
                                coefficient = 1;
                                break;
                        }
                        m_pathwayView.NotifyVariableReferenceChanged(
                            m_pOnLinesEnd,
                            element.Key,
                            RefChangeType.SingleDir,
                            coefficient);
                    }
                    ResetSelectedLine();
                }
            }
            ResetLinePosition();
        }

        /// <summary>
        /// Called when m_nodeMenu is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_nodeMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (sender is ContextMenuStrip)
            {
                ((ContextMenuStrip)sender).Tag = null;
            }
        }
        #endregion

        /// <summary>
        /// Called when the mouse is down on a canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        void m_pathwayCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                m_pathwayCanvas.ContextMenuStrip = m_nodeMenu;
            }
        }

        /// <summary>
        /// event sequence of moving the display rectangles.
        /// </summary>
        /// <param name="sender">Canvas.</param>
        /// <param name="e">MouseEventArgs.</param>
        void m_pathwayCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isRefreshOverview)
                this.UpdateOverview();
            if(e.Button == MouseButtons.Right)
            {
                m_pathwayCanvas.ContextMenuStrip = null;                
            }
        }*/

        /// <summary>
        /// Called when a change line menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChangeLineClick(object sender, EventArgs e)
        {
            if (sender is ToolStripItem)
            {
                ToolStripItem item = (ToolStripItem)sender;

                EdgeInfo edgeInfo = ((Line)item.Tag).Info;

                RefChangeType changeType
                    = RefChangeType.SingleDir;
                int coefficient = 0;

                if (item.Name == CANVAS_MENU_RIGHT_ARROW)
                {
                    changeType = RefChangeType.SingleDir;
                    coefficient = 1;
                }
                else if (item.Name == CANVAS_MENU_LEFT_ARROW)
                {
                    changeType = RefChangeType.SingleDir;
                    coefficient = -1;
                }
                else if (item.Name == CANVAS_MENU_BIDIR_ARROW)
                {
                    changeType = RefChangeType.BiDir;
                    coefficient = 0;
                }
                else
                {
                    changeType = RefChangeType.SingleDir;
                    coefficient = 0;
                }

                m_pathwayView.NotifyVariableReferenceChanged(
                    edgeInfo.ProcessKey,
                    edgeInfo.VariableKey,
                    changeType,
                    coefficient);
            }
        }

        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MergeClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(m_resources.GetString("ConfirmDelete"),
                "Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Cancel)
                return;

            /* 20070629 delete by sachiboo. 
                        //PPathwayObject obj = (PPathwayObject)m_nodeMenu.Tag;
                        Object obj = ((ToolStripItem)sender).Tag;
            */

            Object obj = ((ToolStripItem)sender).Tag;
            if (obj is PEcellSystem)
                {
                    PEcellSystem deleteSystem = (PEcellSystem)obj;
                    if (string.IsNullOrEmpty(deleteSystem.Name))
                        return;
                    if (deleteSystem.Name.Equals("/"))
                    {
                        MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }

                    List<string> list = this.GetAllSystemUnder(deleteSystem.Element.Key);

                    try
                    {
                        m_pathwayView.NotifyDataMerge(deleteSystem.Element.Key, ComponentType.System);
                    }
                    catch (IgnoreException)
                    {
                        return;
                    }

                    foreach (string under in list)
                    {
                        PText sysText = m_systems[under].Text;
                        sysText.Parent.RemoveChild(sysText);
                    }

                    if (((PPathwayObject)obj).IsHighLighted)
                    {
                        HideResizeHandles();
                        m_selectedSystemName = null;
                    }
                }
                else if (obj is Line)
                {
                    m_pathwayView.NotifyVariableReferenceChanged(
                        ((Line)obj).Info.ProcessKey,
                        ((Line)obj).Info.VariableKey,
                        RefChangeType.Delete,
                        0);
                    ResetSelectedLine();
                }
            ((ToolStripMenuItem)sender).Tag = null;
        }

        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(m_resources.GetString("ConfirmDelete"),
                "Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Cancel)
                return;

            /* 20070629 delete by sachiboo. 
                        //PPathwayObject obj = (PPathwayObject)m_nodeMenu.Tag;
                        Object obj = ((ToolStripItem)sender).Tag;
            */


            if (this.SelectedNodes != null)
            {
                List<PPathwayNode> slist = new List<PPathwayNode>();
                foreach (PPathwayNode t in this.SelectedNodes)
                {
                    slist.Add(t);
                }

                foreach (PPathwayNode obj1 in slist)
                {
                    if (obj1 is PPathwayNode)
                    {
                        PPathwayNode deleteNode = (PPathwayNode)obj1;
                        try
                        {
                            if (deleteNode is PEcellVariable)
                            {
                                m_pathwayView.NotifyDataDelete(deleteNode.Element.Key, ComponentType.Variable);
                            }
                            else if (deleteNode is PEcellProcess)
                            {
                                m_pathwayView.NotifyDataDelete(deleteNode.Element.Key, ComponentType.Process);
                            }
                        }
                        catch (IgnoreException)
                        {
                            return;
                        }
                        if (((PPathwayObject)obj1).Parent != null)
                            ((PPathwayObject)obj1).Parent.RemoveChild((PPathwayObject)obj1);
                    }
                }
            }
            Object obj = ((ToolStripItem)sender).Tag;
            if (obj is PEcellSystem)
            {
                PEcellSystem deleteSystem = (PEcellSystem)obj;
                if (string.IsNullOrEmpty(deleteSystem.Name))
                    return;
                if (deleteSystem.Name.Equals("/"))
                {
                    MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                List<string> list = this.GetAllSystemUnder(deleteSystem.Element.Key);

                try
                {
                    m_pathwayView.NotifyDataDelete(deleteSystem.Element.Key, ComponentType.System);
                }
                catch (IgnoreException)
                {
                    return;
                }

                foreach (string under in list)
                {
                    PText sysText = m_systems[under].Text;
                    sysText.Parent.RemoveChild(sysText);
                }

                if (((PPathwayObject)obj).IsHighLighted)
                {
                    HideResizeHandles();
                    m_selectedSystemName = null;
                }
            }
            else if (obj is Line)
            {
                m_pathwayView.NotifyVariableReferenceChanged(
                    ((Line)obj).Info.ProcessKey,
                    ((Line)obj).Info.VariableKey,
                    RefChangeType.Delete,
                    0);
                ResetSelectedLine();
            }
            ((ToolStripMenuItem)sender).Tag = null;
        }

        /// <summary>
        /// Called when a create logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateLoggerClick(object sender, EventArgs e)
        {
            string logger = "";
            EcellObject ecellobj = null;
            if (m_cMenuDict[CANVAS_MENU_DELETE].Tag is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)m_cMenuDict[CANVAS_MENU_DELETE].Tag;
                // Variable
                if (obj is PEcellVariable)
                {
                    PEcellVariable val = (PEcellVariable)obj;
                    Debug.WriteLine("Create Variable Logger:" + val.Element.Key);
                    logger = "Value";
                    // get EcellObject
                    ecellobj = m_pathwayView.GetData(val.Element.Key, val.Element.Type);
                }
                // Process
                else if (obj is PEcellProcess)
                {
                    PEcellProcess proc = (PEcellProcess)obj;
                    Debug.WriteLine("Create Process Logger:" + proc.Element.Key);
                    logger = "Activity";
                    // get EcellObject
                    ecellobj = m_pathwayView.GetData(proc.Element.Key, proc.Element.Type);
                }
                // Process
                else if (obj is PEcellSystem)
                {
                    PEcellSystem sys = (PEcellSystem)obj;
                    Debug.WriteLine("Create System Logger:" + sys.Element.Key);
                    logger = "Size";
                    // get EcellObject
                    ecellobj = m_pathwayView.GetData(sys.Element.Key, sys.Element.Type);
                }
                // exit if ecellobj is null.
                if (ecellobj == null)
                {
                    return;
                }

                // set logger
                foreach (EcellData d in ecellobj.M_value)
                {
                    if (logger.Equals(d.M_name))
                    {
                        PluginManager.GetPluginManager().LoggerAdd(
                                        ecellobj.modelID,
                                        ecellobj.key,
                                        ecellobj.type,
                                        d.M_entityPath);
                        d.M_isLogger = true;
                    }
                }
                // modify changes
                DataManager.GetDataManager().DataChanged(
                                ecellobj.modelID,
                                ecellobj.key,
                                ecellobj.type,
                                ecellobj);

            }
            else
            {
                Debug.WriteLine("Not PPathwayObject:" + m_cMenuDict[CANVAS_MENU_DELETE].Tag.ToString());
            }

        }

        /// <summary>
        /// Called when a delete logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteLoggerClick(object sender, EventArgs e)
        {
            string logger = "";
            EcellObject ecellobj = null;

            if (m_cMenuDict[CANVAS_MENU_DELETE].Tag is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)m_cMenuDict[CANVAS_MENU_DELETE].Tag;
                // Variable
                if (obj is PEcellVariable)
                {
                    PEcellVariable val = (PEcellVariable)obj;
                    Debug.WriteLine("Create Variable Logger:" + val.Element.Key);
                    logger = "Value";
                    // get EcellObject
                    ecellobj = m_pathwayView.GetData(val.Element.Key, val.Element.Type);
                }
                // Process
                else if (obj is PEcellProcess)
                {
                    PEcellProcess proc = (PEcellProcess)obj;
                    Debug.WriteLine("Create Process Logger:" + proc.Element.Key);
                    logger = "Activity";
                    // get EcellObject
                    ecellobj = m_pathwayView.GetData(proc.Element.Key, proc.Element.Type);
                }
                // Process
                else if (obj is PEcellSystem)
                {
                    PEcellSystem sys = (PEcellSystem)obj;
                    Debug.WriteLine("Create System Logger:" + sys.Element.Key);
                    logger = "Size";
                    // get EcellObject
                    ecellobj = m_pathwayView.GetData(sys.Element.Key, sys.Element.Type);
                }
                // exit if ecellobj is null.
                if (ecellobj == null)
                {
                    return;
                }

                // delete logger
                foreach (EcellData d in ecellobj.M_value)
                {
                    if (logger.Equals(d.M_name))
                    {
                        d.M_isLogger = false;
                    }
                }
                // modify changes
                DataManager.GetDataManager().DataChanged(
                                ecellobj.modelID,
                                ecellobj.key,
                                ecellobj.type,
                                ecellobj);

            }
            else
            {
                Debug.WriteLine("Not PPathwayObject:" + m_cMenuDict[CANVAS_MENU_DELETE].Tag.ToString());
            }
        }

#if DEBUG
        /// <summary>
        /// Called when a debug menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebugClick(object sender, EventArgs e)
        {
            if (m_cMenuDict[CANVAS_MENU_DELETE].Tag is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)m_cMenuDict[CANVAS_MENU_DELETE].Tag;
                MessageBox.Show("Name:" + obj.Name + "\nX:" + obj.X + "\nY:" + obj.Y
                    + "\nOffsetX:" + obj.OffsetX + "\nOffsetY:" + obj.OffsetY + "\nToString()"
                    + obj.ToString());
            }
        }
#endif

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
            PEcellSystem topSystem = m_systems[m_selectedSystemName].EcellSystems[0];
            foreach(PLayer layer in Layers.Values)
            {
                PNodeList list = new PNodeList();
                layer.FindIntersectingNodes(topSystem.Rect, list);
                m_surroundedBySystem.AddRange(list);
            }
            foreach(PNode node in m_surroundedBySystem)
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
            foreach(PNode node in m_surroundedBySystem)
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
        void PEcellSystem_MouseUp(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            PEcellSystem system = m_systems[m_selectedSystemName].EcellSystems[0];
            if (this.DoesSystemOverlaps(system.GlobalBounds, system.Element.Key))
            {
                foreach (PEcellSystem eachSys in m_systems[m_selectedSystemName].EcellSystems)
                {
                    eachSys.ReturnToMemorizedPosition();
                    this.ValidateSystem(eachSys);
                    eachSys.Reset();
                }
            }
            else
            {
                foreach (PEcellSystem eachSys in m_systems[m_selectedSystemName].EcellSystems)
                    eachSys.Reset();
                
                PNodeList currentSystemChildren = new PNodeList();
                PEcellSystem topSystem = m_systems[m_selectedSystemName].EcellSystems[0];
                List<PNode> tmpList = new List<PNode>();
                foreach (PLayer layer in this.Layers.Values)
                {
                    PNodeList nodeList = new PNodeList();
                    layer.FindIntersectingNodes(topSystem.Rect, nodeList);
                    foreach (PNode p in nodeList)
                    {
                        if (!currentSystemChildren.Contains(p))
                            currentSystemChildren.Add(p);
                        if (p is PEcellSystem && p != topSystem)
                        {
                            PNodeList tmpNodeList = new PNodeList();
                            layer.FindIntersectingNodes(((PEcellSystem)p).Rect, tmpNodeList);
                            foreach (PNode pn in tmpNodeList)
                            {
                                if (pn != p && !tmpList.Contains(pn)) tmpList.Add(pn);
                            }
                        }
                    }
                }

                Dictionary<string, PNode> currentDict = new Dictionary<string, PNode>();
                foreach (PNode child in currentSystemChildren)
                {
                    if (tmpList.Contains(child)) continue;
                    if (child is PPathwayNode)
                    {
                        currentDict.Add( ((PPathwayNode)child).Element.Type + ":" + ((PPathwayNode)child).Element.Key, child);
                    }
                    else if (child is PEcellSystem)
                    {
                        if (!m_selectedSystemName.Equals(((PEcellSystem)child).Element.Key))
                            currentDict.Add( ((PEcellSystem)child).Element.Type + ":" + ((PEcellSystem)child).Element.Key, child);
                    }
                }

                Dictionary<string, PNode> beforeDict = new Dictionary<string, PNode>();
                foreach (PNode child in m_systemChildrenBeforeDrag)
                {
                    string key = "";
                    if (child is PPathwayNode)
                    {
                        PPathwayNode childNode = ((PPathwayNode)child);
                        key = childNode.Element.Key;
                        beforeDict.Add(childNode.Element.Type + ":" + key, child);
                    }
                    else if (child is PEcellSystem)
                    {
                        key = ((PEcellSystem)child).Element.Key;
                        beforeDict.Add(((PEcellSystem)child).Element.Type + ":" + key, child);
                    }
                }

                // If ID duplication could occurred, system resizing will be aborted
                Dictionary<string, PPathwayNode> nameDict = new Dictionary<string, PPathwayNode>();
                foreach (string key in currentDict.Keys)
                {
                    string name = PathUtil.RemovePath(key);
                    if (nameDict.ContainsKey(name))
                    {
                        // Resizing is aborted
                        foreach (PEcellSystem eachSys in m_systems[m_selectedSystemName].EcellSystems)
                        {
                            eachSys.ReturnToMemorizedPosition();
                            this.ValidateSystem(eachSys);
                            eachSys.Reset();
                        }
                        m_systems[m_selectedSystemName].UpdateText();

                        UpdateResizeHandlePositions();
                        ResetSelectedObjects();
                        ClearSurroundState();
                        MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        nameDict.Add(name, null);
                    }
                }

                foreach (string key in beforeDict.Keys)
                {
                    if(!currentDict.ContainsKey(key))
                    {
                        bool isDuplicate = false;
                        String[] sp = key.Split(new char[] { ':' });
                        if (key.StartsWith("System"))
                        {
                            String name = sp[1];
                            sp = sp[1].Split(new char[] { '/' });
                            PPathwayObject p = system.ParentObject;
                            String newkey = ((PEcellSystem)p).Element.Key + "/" + sp[sp.Length - 1];
                            if (m_pathwayView.HasObject(ComponentType.System, newkey))
                                isDuplicate = true;
                            
                        }
                        else
                        {
                            String data = sp[1] + ":" + sp[2];
                            PPathwayObject p = system.ParentObject;
                            String newkey = ((PEcellSystem)p).Element.Key + ":" + sp[2];
                            ComponentType ct;
                            if (key.StartsWith("Process"))
                                ct = ComponentType.Process;
                            else
                                ct = ComponentType.Variable;
                            if (m_pathwayView.HasObject(ct, newkey))
                                isDuplicate = true;
                            
                        }

                        if (isDuplicate)
                        {
                            // Resizing is aborted
                            foreach (PEcellSystem eachSys in m_systems[m_selectedSystemName].EcellSystems)
                            {
                                eachSys.ReturnToMemorizedPosition();
                                this.ValidateSystem(eachSys);
                                eachSys.Reset();
                            }
                            m_systems[m_selectedSystemName].UpdateText();

                            UpdateResizeHandlePositions();
                            ResetSelectedObjects();
                            ClearSurroundState();
                            MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                foreach (String key in currentDict.Keys)
                {
                    if (beforeDict.ContainsKey(key)) continue;

                    String[] sp = key.Split(new char[] {':'});
                    if (key.StartsWith("System"))
                    {
                        String name = sp[1];
                        sp = sp[1].Split(new char[] { '/' });
                        PPathwayObject p = system.ParentObject;
                        String newkey = m_selectedSystemName + "/" + sp[sp.Length - 1];
                        EcellObject obj = m_pathwayView.GetData(name, "System");
                        String prevkey = obj.key;
                        obj.key = newkey;

                        String tmp = m_selectedSystemName;
                        this.TransferSystemTo(m_selectedSystemName, prevkey);
                        m_selectedSystemName = tmp;
                    }
                    else
                    {
                        String data = sp[1] + ":" + sp[2];
                        String newkey = m_selectedSystemName + ":" + sp[2];
                        EcellObject obj = m_pathwayView.GetData(data, sp[0]);
                        if (obj == null) continue;
                        String prevkey = obj.key;
                        obj.key = newkey;

                        if (key.StartsWith("Process"))
                        {
                            this.TransferNodeToByResize(m_selectedSystemName, m_processes[prevkey]);
                            m_processes[newkey].RefreshEdges();
                        }
                        else
                        {
                            this.TransferNodeToByResize(m_selectedSystemName, m_variables[prevkey]);
                            m_variables[newkey].Refresh();
                        }

                        DataManager.GetDataManager().DataChanged(
                            obj.modelID, prevkey, obj.type, obj);
                    }
                }

                foreach (String key in beforeDict.Keys)
                {
                    if (currentDict.ContainsKey(key)) continue;

                    if (m_selectedSystemName.Equals("/"))
                    {
                        foreach (PEcellSystem eachSys in m_systems[m_selectedSystemName].EcellSystems)
                        {
                            eachSys.ReturnToMemorizedPosition();
                            this.ValidateSystem(eachSys);
                            eachSys.Reset();
                        }
                        m_systems[m_selectedSystemName].UpdateText();
                        UpdateResizeHandlePositions();
                        ResetSelectedObjects();
                        ClearSurroundState();

                        return;
                    }

                    String[] sp = key.Split(new char[] {':'});
                    if (key.StartsWith("System"))
                    {
                        String name = sp[1];
                        sp = sp[1].Split(new char[] { '/' });
                        PPathwayObject p = system.ParentObject;
                        String newkey =  ((PEcellSystem)p).Element.Key + "/" + sp[sp.Length - 1];
                        EcellObject obj = m_pathwayView.GetData(name, "System");
                        String prevkey = name;
                        obj.key = newkey;

                        String tmp = m_selectedSystemName;
                        this.TransferSystemTo(((PEcellSystem)p).Element.Key, prevkey);
                        m_selectedSystemName = tmp;
                    }
                    else
                    {
                        String data = sp[1] + ":" + sp[2];
                        PPathwayObject p = system.ParentObject;
                        String newkey = ((PEcellSystem)p).Element.Key + ":" + sp[2];
                        EcellObject obj = m_pathwayView.GetData(data, sp[0]);
                        String prevkey = data;
                        obj.key = newkey;

                        if (key.StartsWith("Process"))
                        {
                            this.TransferNodeToByResize(((PEcellSystem)p).Element.Key, m_processes[prevkey]);
                            m_processes[newkey].RefreshEdges();
                        }
                        else
                        {
                            this.TransferNodeToByResize(((PEcellSystem)p).Element.Key, m_variables[prevkey]);
                            m_variables[newkey].Refresh();
                        }
                        
                        DataManager.GetDataManager().DataChanged(
                            obj.modelID, prevkey, obj.type, obj);
                    }
                }
                // Fire DataChanged for child in system.!
                m_systemChildrenBeforeDrag = null;
            }
            m_systems[m_selectedSystemName].UpdateText();
            
            UpdateResizeHandlePositions();
            ResetSelectedObjects();
            ClearSurroundState();
        }

        /// <summary>
        /// Called when the mouse is down on one of resize handles for a system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PEcellSystem_MouseDown(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            PEcellSystem system = m_systems[m_selectedSystemName].EcellSystems[0];
            system.MemorizeCurrentPosition();
            PointF offsetToL = m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer;
            PointF lP = new PointF(system.X + offsetToL.X, system.Y + offsetToL.Y);
            m_upperLeftPoint = lP;
            m_upperRightPoint = new PointF(m_upperLeftPoint.X + system.Width, m_upperLeftPoint.Y);
            m_lowerRightPoint = new PointF(m_upperLeftPoint.X + system.Width,
                                            m_upperLeftPoint.Y + system.Height);
            m_lowerLeftPoint = new PointF(m_upperLeftPoint.X, m_upperLeftPoint.Y + system.Height);

            m_systemChildrenBeforeDrag = new PNodeList();

            foreach (PEcellSystem oneSystem in m_systems[m_selectedSystemName].EcellSystems)
                m_systemChildrenBeforeDrag.AddRange(oneSystem.ChildrenReference);
        }

        /// <summary>
        /// Called when the NorthWest resize handle is being dragged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PEcellSystem_ResizeNW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;
            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - sysEle.HalfThickness;
            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - sysEle.HalfThickness;
            float width = m_lowerRightPoint.X - X;
            float height = m_lowerRightPoint.Y - Y;
            if (width > PEcellSystem.MIN_X_LENGTH && height > PEcellSystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.X = X;
                sysEle.Y = Y;
                sysEle.Width = width;
                sysEle.Height = height;
                m_systems[m_selectedSystemName].UpdateText();
                PointF offsetToL = m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer;
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.X = sysEle.X - offsetToL.X;
                    system.Y = sysEle.Y - offsetToL.Y;
                    system.Width = sysEle.Width;
                    system.Height = sysEle.Height;
                    //system.SystemWidth = sysEle.Width;
                    //system.SystemHeight = sysEle.Height;
                    this.ValidateSystem(system);
                    system.Reset();
                }
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                if (width <= PEcellSystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
                }
                if (height <= PEcellSystem.MIN_Y_LENGTH)
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
        void PEcellSystem_ResizeN(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;
            
            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - sysEle.HalfThickness;
            float height = m_lowerRightPoint.Y - Y;

            if (height > PEcellSystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.Y = Y;
                sysEle.Height = height;
                m_systems[m_selectedSystemName].UpdateText();
                PointF offsetToL = m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer;
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.Y = sysEle.Y - offsetToL.Y;
                    system.Height = sysEle.Height;
                    this.ValidateSystem(system);
                    system.Reset();
                }
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
        void PEcellSystem_ResizeNE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;

            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - sysEle.HalfThickness;
            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + sysEle.HalfThickness
                               - sysEle.X - m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer.X;
            float height = m_lowerLeftPoint.Y - Y;

            if (width > PEcellSystem.MIN_X_LENGTH && height > PEcellSystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.Y = Y;
                sysEle.Width = width;
                sysEle.Height = height;
                m_systems[m_selectedSystemName].UpdateText();
                PointF offsetToL = m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer;
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.Y = sysEle.Y - offsetToL.Y;
                    system.Width = sysEle.Width;
                    system.Height = sysEle.Height;
                    //system.SystemWidth = sysEle.Width;
                    //system.SystemHeight = sysEle.Height;
                    this.ValidateSystem(system);
                    system.Reset();
                }
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();

                if (width <= PEcellSystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();
                }
                if (height <= PEcellSystem.MIN_Y_LENGTH)
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
        void PEcellSystem_ResizeE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;

            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + sysEle.HalfThickness
                              - sysEle.X - m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer.X;
            if (width > PEcellSystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.Width = width;
                m_systems[m_selectedSystemName].UpdateText();
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.Width = sysEle.Width;
                    //system.SystemWidth = sysEle.Width;
                    this.ValidateSystem(system);
                    system.Reset();
                }
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
        void PEcellSystem_ResizeSE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;

            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + sysEle.HalfThickness
                               - sysEle.X - m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer.X;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + sysEle.HalfThickness
                                - sysEle.Y - m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer.Y;

            if (width > PEcellSystem.MIN_X_LENGTH && height > PEcellSystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.Width = width;
                sysEle.Height = height;
                m_systems[m_selectedSystemName].UpdateText();
                PointF offsetToL = m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer;
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.Width = sysEle.Width;
                    system.Height = sysEle.Height;
                    //system.SystemWidth = sysEle.Width;
                    //system.SystemHeight = sysEle.Height;
                    this.ValidateSystem(system);
                    system.Reset();
                }
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXMinus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();

                if (width <= PEcellSystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                }
                if (height <= PEcellSystem.MIN_Y_LENGTH)
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
        void PEcellSystem_ResizeS(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;

            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + sysEle.HalfThickness
                                 - sysEle.Y - m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer.Y;

            if (height > PEcellSystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.Height = height;
                m_systems[m_selectedSystemName].UpdateText();

                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.Height = sysEle.Height;
                    this.ValidateSystem(system);
                    system.Reset();
                }
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
        void PEcellSystem_ResizeSW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - sysEle.HalfThickness;
            float width = m_upperRightPoint.X - e.PickedNode.X - e.PickedNode.OffsetX - m_resizeHandleHalfWidth + sysEle.HalfThickness;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + sysEle.HalfThickness
                               - sysEle.Y - m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer.Y;

            if (width > PEcellSystem.MIN_X_LENGTH && height > PEcellSystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.X = X;
                sysEle.Width = width;
                sysEle.Height = height;
                m_systems[m_selectedSystemName].UpdateText();
                PointF offsetToL = m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer;
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.X = sysEle.X - offsetToL.X;
                    system.Width = sysEle.Width;
                    system.Height = sysEle.Height;
                    //system.SystemWidth = sysEle.Width;
                    //system.SystemHeight = sysEle.Height;
                    this.ValidateSystem(system);
                    system.Reset();
                }
                UpdateResizeHandlePositions(e.PickedNode);
            }
            else
            {
                ((ResizeHandle)e.PickedNode).ProhibitMovingToXPlus();
                ((ResizeHandle)e.PickedNode).ProhibitMovingToYMinus();

                if (width <= PEcellSystem.MIN_X_LENGTH)
                {
                    ((ResizeHandle)e.PickedNode).ProhibitMovingToYPlus();
                }
                if (height <= PEcellSystem.MIN_Y_LENGTH)
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
        void PEcellSystem_ResizeW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            RefreshSurroundState();
            SystemElement sysEle = m_systems[m_selectedSystemName].Element;

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - sysEle.HalfThickness;
            float width = m_lowerRightPoint.X - X;

            if (width > PEcellSystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.X = X;
                sysEle.Width = width;
                m_systems[m_selectedSystemName].UpdateText();
                PointF offsetToL = m_systems[m_selectedSystemName].EcellSystems[0].OffsetToLayer;
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.X = sysEle.X - offsetToL.X;
                    system.Width = sysEle.Width;
                    //system.SystemWidth = sysEle.Width;
                    this.ValidateSystem(system);
                    system.Reset();
                }
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
        void PEcellSystem_MouseLeave(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PEcellSystem_CursorSizeNWSE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNWSE;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PEcellSystem_CursorSizeNS(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNS;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PEcellSystem_CursorSizeNESW(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeNESW;
        }

        /// <summary>
        /// Called for changing the mouse figure on a resize handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PEcellSystem_CursorSizeWE(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            e.Canvas.Cursor = Cursors.SizeWE;
        }
        #endregion

        /// <summary>
        /// Validate a system. According to result, system.Valid will be changed.
        /// </summary>
        /// <param name="system">PEcellSystem to be validated</param>
        protected void ValidateSystem(PEcellSystem system)
        {
            if (this.DoesSystemOverlaps(system))
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

            PEcellSystem system = m_systems[m_selectedSystemName].EcellSystems[0];

            PointF gP = new PointF(system.X + system.OffsetX, system.Y + system.OffsetY);

            PEcellSystem parentSystem = system;

            while(parentSystem.Parent is PEcellSystem)
            {
                parentSystem = (PEcellSystem)parentSystem.Parent;
                parentSystem.LocalToParent(gP);
                gP.X = gP.X + parentSystem.OffsetX;
                gP.Y = gP.Y + parentSystem.OffsetY;
            }

            float halfOuterRadius = SystemElement.OUTER_RADIUS / 2f;
            float halfThickness = (SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS) / 2;
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

            PEcellSystem system = m_systems[m_selectedSystemName].EcellSystems[0];

            PointF gP = new PointF(system.X + system.OffsetX, system.Y + system.OffsetY);

            PEcellSystem parentSystem = system;

            while (parentSystem.Parent is PEcellSystem)
            {
                parentSystem = (PEcellSystem)parentSystem.Parent;
                parentSystem.LocalToParent(gP);
                gP.X = gP.X + parentSystem.OffsetX;
                gP.Y = gP.Y + parentSystem.OffsetY;
            }

            float halfOuterRadius = SystemElement.OUTER_RADIUS / 2f;
            float halfThickness = (SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS) / 2;
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
        /// </summary>
        public void NotifySelectChanged(string key, string type)
        {
            if (m_pathwayView != null)
                m_pathwayView.NotifySelectChanged(key, type);
        }

        /// <summary>
        /// Notify this canvas that the mouse is on it.
        /// </summary>
        /// <param name="element">mouse is on this node</param>
        public void NotifyMouseEnter(NodeElement element)
        {
            if(m_isReconnectMode)            
                m_nodesUnderMouse.Push(element);
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
                catch(InvalidOperationException)
                {
                }
            }
        }

        /// <summary>
        /// Set background color of pathway view to default color;
        /// </summary>
        protected void SetBackToDefault()
        {
            foreach(SystemContainer sysCon in m_systems.Values)
            {
                foreach(PEcellSystem system in sysCon.EcellSystems)
                {
                    system.BackgroundBrush = null;
                }
            }
            m_pathwayCanvas.BackColor = Color.White;
        }

        /// <summary>
        /// Set background color of pathway view without given systemName to shading color
        /// </summary>
        /// <param name="systemName">Only this system's color remain default state.</param>
        protected void SetShadeWithoutSystem(string systemName)
        {
            if (systemName != null && m_systems.ContainsKey(systemName))
            {
                m_pathwayCanvas.BackColor = Color.Silver;
                m_systems[systemName].EcellSystems[m_systems[systemName].EcellSystems.Count - 1].BackgroundBrush = Brushes.White;

                foreach(KeyValuePair<string, SystemContainer> pair in m_systems)
                {
                    if(!pair.Key.Equals(systemName))
                    {
                        pair.Value.EcellSystems[m_systems[pair.Key].EcellSystems.Count - 1].BackgroundBrush = Brushes.Silver;
                    }
                }
            }
            else
            {
                m_pathwayCanvas.BackColor = Color.White;
                foreach (SystemContainer sysCon in m_systems.Values)
                {
                    sysCon.EcellSystems[sysCon.EcellSystems.Count - 1].BackgroundBrush = Brushes.Silver;
                }
            }
        }
        
        /// <summary>
        /// Transfer selected objects from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName"></param>
        public void TransferSelectedTo(string systemName)
        {
            foreach(PPathwayNode node in m_selectedNodes)
            {
                TransferNodeTo(systemName, node);
            }
            ResetSelectedObjects();
        }

        /// <summary>
        /// Transfer an object from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName">The name of the system to which object is transfered. If null, obj is
        /// transfered to layer itself</param>
        /// <param name="obj">transfered object</param>
        public void TransferNodeTo(string systemName, PPathwayObject obj)
        {
            // The case that obj is transfered to PEcellSystem.
            foreach(PEcellSystem system in m_systems[systemName].EcellSystems)
            {
                if (system.Layer == obj.Layer && system != obj)
                {
                    PointF offset = system.OffsetToLayer;
                    
                    obj.X -= offset.X;
                    obj.Y -= offset.Y;
                    
                    system.AddChild(obj);
                    if(obj is PPathwayNode)
                    {
                        ((PPathwayNode)obj).ParentObject = system;
                    }
                }
            }
            if(obj is PEcellVariable)
            {
                PEcellVariable var = (PEcellVariable)obj;
                m_variables.Remove(var.Element.Key);
                string newKey = systemName + ":" + PathUtil.RemovePath(var.Element.Key);
                string oldKey = var.Element.Key;
                var.Element.Key = newKey;
                m_variables.Add(var.Element.Key, var);
                var.RefreshText();
                if (!oldKey.Equals(newKey))
                    m_pathwayView.NotifyDataChanged(oldKey, newKey, PathwayView.VARIABLE_STRING, true);
                var.Refresh();
            }
            else if(obj is PEcellProcess)
            {
                PEcellProcess pro = (PEcellProcess)obj;
                m_processes.Remove(pro.Element.Key);
                string newKey = systemName + ":" + PathUtil.RemovePath(pro.Element.Key);
                string oldKey = pro.Element.Key;
                pro.Element.Key = newKey;
                m_processes.Add(pro.Element.Key, pro);
                pro.RefreshText();
                if (!oldKey.Equals(newKey))
                    m_pathwayView.NotifyDataChanged(oldKey, newKey, PathwayView.PROCESS_STRING, true);
                pro.Refresh();
            }
        }


        /// <summary>
        /// Transfer an object from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName">The name of the system to which object is transfered. If null, obj is
        /// transfered to layer itself</param>
        /// <param name="obj">transfered object</param>
        public void TransferNodeToByResize(string systemName, PPathwayObject obj)
        {
            // The case that obj is transfered to PEcellSystem.
            foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
            {
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
            }
            if (obj is PEcellVariable)
            {
                PEcellVariable var = (PEcellVariable)obj;
                m_variables.Remove(var.Element.Key);
                string newKey = systemName + ":" + PathUtil.RemovePath(var.Element.Key);
                string oldKey = var.Element.Key;
                var.Element.Key = newKey;
                m_variables.Add(var.Element.Key, var);
                var.RefreshText();
                if (!oldKey.Equals(newKey))
                    m_pathwayView.NotifyDataChanged(oldKey, newKey, PathwayView.VARIABLE_STRING, true);
            }
            else if (obj is PEcellProcess)
            {
                PEcellProcess pro = (PEcellProcess)obj;
                m_processes.Remove(pro.Element.Key);
                string newKey = systemName + ":" + PathUtil.RemovePath(pro.Element.Key);
                string oldKey = pro.Element.Key;
                pro.Element.Key = newKey;
                m_processes.Add(pro.Element.Key, pro);
                pro.RefreshText();
                if (!oldKey.Equals(newKey))
                    m_pathwayView.NotifyDataChanged(oldKey, newKey, PathwayView.PROCESS_STRING, true);
            }
        }

        /// <summary>
        /// Transfer an system from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName">The name of the system to which a system will be transfered. If null, obj is
        /// transfered to layer itself</param>
        /// <param name="oldKey">old key of a system to be transfered</param>
        public void TransferSystemTo(string systemName, string oldKey)
        {
            if(String.IsNullOrEmpty(systemName))
                return;
            string newKey;
            ResetSelectedObjects();
            if (systemName.Equals("/"))
            {
                newKey = systemName + PathUtil.RemovePath(oldKey);
            }
            else
            {
                newKey = systemName + "/" + PathUtil.RemovePath(oldKey);
            }

            SystemContainer oldSysCon = m_systems[oldKey];
            SystemContainer parentSysCon = m_systems[systemName];

            foreach(PEcellSystem pes in oldSysCon.EcellSystems)
            {
                pes.Parent.RemoveChild(pes);
                foreach (PEcellSystem ppes in parentSysCon.EcellSystems)
                    if (ppes.Layer == pes.Layer)
                        ppes.AddChild(pes);
            }

            m_pathwayView.NotifyDataChanged(oldKey, newKey, PathwayView.SYSTEM_STRING, true);

            oldSysCon.Element.Key = newKey;
            for (int k = 0; k < oldSysCon.EcellSystems.Count; k++)
            {
                String name = oldSysCon.EcellSystems[k].Name;
                name = name.Replace(oldKey, newKey);
                oldSysCon.EcellSystems[k].Name = name;
                oldSysCon.EcellSystems[k].Element.Key = name;
            }
            SystemContainer newSysCon = new SystemContainer(oldSysCon.EcellSystems, oldSysCon.Element);
            newSysCon.Text = oldSysCon.Text;
            newSysCon.Attribute = oldSysCon.Attribute;
            m_systems.Add(newKey, newSysCon);
            m_systems.Remove(oldKey);

            foreach (string key in this.GetAllSystemUnder(oldKey))
            {
                SystemContainer sysCon = m_systems[key];
                m_systems.Remove(key);
                string newSysKey = key.Replace(oldKey, newKey);
                foreach (PEcellSystem system in sysCon.EcellSystems)
                    system.Name = newSysKey;
                sysCon.Element.Key = newSysKey;
                m_systems.Add(newSysKey, sysCon);
            }

            foreach (string key in this.GetAllVariableUnder(oldKey))
            {
                PEcellVariable pev = m_variables[key];
                m_variables.Remove(key);
                string newVarKey = key.Replace(oldKey, newKey);
                pev.Name = newVarKey;
                m_variables.Add(newVarKey, pev);
            }

            foreach (string key in this.GetAllProcessUnder(oldKey))
            {
                PEcellProcess pep = m_processes[key];
                m_processes.Remove(key);
                string newProKey = key.Replace(oldKey, newKey);
                pep.Name = newProKey;
                m_processes.Add(newProKey, pep);
            }
        }

        /// <summary>
        /// Hide resize handles for resizing system.
        /// </summary>
        protected void HideResizeHandles()
        {
            foreach(PNode node in m_resizeHandles)
                if(node.Parent == m_ctrlLayer)
                    m_ctrlLayer.RemoveChild(node);
        }

        /// <summary>
        /// Check if any system of this canvas overlaps given rectangle.
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemOverlaps(RectangleF rect)
        {
            bool isOverlaping = false;
            foreach(SystemContainer sysCon in m_systems.Values)
            {
                if (sysCon.EcellSystems[0].Overlaps(rect))
                {
                    isOverlaping = true;
                }
            }
            return isOverlaping;
        }

        /// <summary>
        /// Check if any system of this canvas (exclude specified system) overlaps given rectangle.
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <param name="excludeName">The system with this name will NOT be taken into account</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemOverlaps(RectangleF rect, string excludeName)
        {
            bool isOverlaping = false;
            foreach (SystemContainer sysCon in m_systems.Values)
            {
                if (!sysCon.Element.Key.Equals(excludeName) && sysCon.EcellSystems[0].Overlaps(rect))
                {
                    isOverlaping = true;
                }
            }
            return isOverlaping;
        }

        /// <summary>
        /// Check if any system of this canvas overlaps given system.
        /// </summary>
        /// <param name="system">system, to be checked</param>
        /// <returns>True if there is a system which overlaps rectangle of argument, otherwise false</returns>
        public bool DoesSystemOverlaps(PEcellSystem system)
        {
            bool isOverlaping = false;
            foreach (SystemContainer sysCon in m_systems.Values)
            {
                if (!sysCon.EcellSystems.Contains(system) && sysCon.EcellSystems[0].Overlaps(system.GlobalFullBounds))
                {
                    isOverlaping = true;
                }
            }
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
            if (layer == null && m_layers.Count == 1)
                foreach (string key in m_layers.Keys)
                    layer = key;

            if (obj is PPathwayNode)
                ((PPathwayNode)obj).ShowingID = this.m_showingId;
            RegisterObjToSet(obj);
            
            if (systemName == null)
            {
                obj.Layer = m_layers[layer];
                m_layers[layer].AddChild(obj);
            }
            else
            {
                // If obj hasn't coordinate, it will be settled. 
                if(!hasCoords)
                {
                    if (obj is PPathwayNode)
                    {
                        // Get a vacant point where an incoming node should be placed.
                        List<PPathwayNode> nodeList = new List<PPathwayNode>();
                        foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
                            foreach (PPathwayObject ppo in system.ChildObjectList)
                                if (ppo is PPathwayNode)
                                    nodeList.Add((PPathwayNode)ppo);
                        PEcellSystem topSystem = m_systems[systemName].EcellSystems[0];
                        RectangleF sysRect = new RectangleF(
                            topSystem.X + topSystem.OffsetToLayer.X,
                            topSystem.Y + topSystem.OffsetToLayer.Y,
                            topSystem.Width,
                            topSystem.Height);
                        List<RectangleF> excludeRectList = new List<RectangleF>();
                        List<PPathwayObject> childList = topSystem.ChildObjectList;
                        foreach (PPathwayObject child in childList)
                        {
                            if (child is PEcellSystem)
                            {
                                excludeRectList.Add(new RectangleF(
                                    child.X + ((PEcellSystem)child).OffsetToLayer.X,
                                    child.Y + ((PEcellSystem)child).OffsetToLayer.Y,
                                    ((PEcellSystem)child).Width,
                                    ((PEcellSystem)child).Height));
                            }
                        }
                        PointF vacantPoint = GetVacantPoint(sysRect, excludeRectList, nodeList);
                        obj.X = vacantPoint.X - obj.Width;
                        obj.Y = vacantPoint.Y - obj.Height;
                    }
                    else
                    {
                        float maxX = 0f;
                        float maxY = 0f;
                        float systemX = 0f;
                        float systemY = 0f;
                        float systemW = 0f;
                        float systemH = 0f;
                        float systemOffsetToLayerX = 0f;
                        float systemOffsetToLayerY = 0f;

                        foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
                        {
                            systemX = system.X;
                            systemY = system.Y;
                            systemW = system.Width;
                            systemH = system.Height;
                            PointF OffsetToLayer = system.OffsetToLayer;
                            systemOffsetToLayerX = OffsetToLayer.X;
                            systemOffsetToLayerY = OffsetToLayer.Y;

                            foreach (PPathwayObject ppo in system.ChildObjectList)
                            {
                                float x = 0;
                                float y = 0;
                                if (ppo is PPathwayNode)
                                {
                                    x = ppo.X + ppo.OffsetX;
                                    y = ppo.Y + ppo.OffsetY;
                                }
                                else if (ppo is PEcellSystem)
                                {
                                    PEcellSystem pes = (PEcellSystem)ppo;
                                    x = pes.X + pes.OffsetX + pes.Width;
                                    y = pes.Y + pes.OffsetY + pes.Height;

                                }
                                if (maxX < x)
                                    maxX = x;
                                if (maxY < y)
                                    maxY = y;
                            }
                        }

                        // Calculate size needed for parent system to house this system.
                        float needX = maxX + PEcellSystem.DEFAULT_WIDTH + 2 * PathwayView.SYSTEM_SYSTEM_MARGIN;
                        float needY = maxY;
                        if (needY < PEcellSystem.DEFAULT_HEIGHT + 2 * PathwayView.SYSTEM_SYSTEM_MARGIN)
                            needY = PEcellSystem.DEFAULT_HEIGHT + 2 * PathwayView.SYSTEM_SYSTEM_MARGIN;

                        RectangleF request4XDir = RectangleF.Empty;
                        RectangleF request4YDir = RectangleF.Empty;

                        if (systemW < needX)
                            request4XDir = new RectangleF(systemX + systemW, systemY, needX - systemW, needY);
                        
                        if (systemH < needY)
                            request4YDir = new RectangleF(systemX, systemY + systemH, systemW, needY - systemH);

                        foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
                            system.MakeSpace(request4XDir, request4YDir);

                        foreach (SystemContainer sysCon in m_systems.Values)
                        {
                            foreach (PEcellSystem system in sysCon.EcellSystems)
                                system.Reset();
                            sysCon.UpdateText();
                        }

                        // Set obj's coordinate
                        obj.X = systemX + systemOffsetToLayerX + maxX + PathwayView.SYSTEM_SYSTEM_MARGIN;
                        obj.Y = systemY + systemOffsetToLayerY + PathwayView.SYSTEM_SYSTEM_MARGIN;
                    }
                }

                // Set obj to appropriate layer.
                foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
                {
                    if (system.Layer == m_layers[layer])
                    {
                        obj.Layer = m_layers[layer];
                        PointF offsetToL = system.OffsetToLayer;

                        if (!isFirst)
                        {
                            obj.X -= offsetToL.X;
                            obj.Y -= offsetToL.Y;
                        }
                        
                        if (obj is PPathwayNode)
                        {
                            

                            ((PPathwayNode)obj).Element.X = obj.X;
                            ((PPathwayNode)obj).Element.Y = obj.Y;
                            ((PPathwayNode)obj).RefreshText();                                                        
                        }
                        system.AddChild(obj);
                        if (obj is PPathwayObject)
                            ((PPathwayObject)obj).ParentObject = system;
                    }
                    if (obj is PEcellProcess)
                        ((PEcellProcess)obj).CreateEdges();
                }
            }
        }

        /// <summary>
        /// add child object to the selected layer.
        /// </summary>
        /// <param name="layer">selected layer.</param>
        /// <param name="obj">add object.</param>
        public void AddChildToSelectedLayer(string layer, PPathwayObject obj)
        {
            RegisterObjToSet(obj);
            obj.Layer = m_layers[layer];
            m_layers[layer].AddChild(obj);
        }

        /// <summary>
        /// add the child object to the selected layer.
        /// </summary>
        /// <param name="layer">the selected layer.</param>
        /// <param name="systemName">system name of the added object.</param>
        /// <param name="obj">the added object.</param>
        public void AddChildToSelectedLayer(string layer, string systemName, PPathwayObject obj)
        {
            RegisterObjToSet(obj);
            if (systemName == null)
            {
                obj.Layer = m_layers[layer];
                m_layers[layer].AddChild(obj);
            }
            else
            {
                foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
                {
                    if (system.Layer == m_layers[layer])
                    {
                        obj.Layer = m_layers[layer];
                        system.AddChild(obj);
                        if (obj is PPathwayNode)
                            ((PPathwayNode)obj).ParentObject = system;
                    }
                }
            }
        }

        /// <summary>
        /// add the child object to the selected system.
        /// </summary>
        /// <param name="systemName">the name of selected system.</param>
        /// <param name="obj">the added object.</param>
        /// <param name="isCoordSet">position offset.</param>
        public void AddChildToSelectedSystem(string systemName, PPathwayObject obj, bool isCoordSet)
        {
            RegisterObjToSet(obj);
            if(obj.Layer == null)
            {
                PEcellSystem system = m_systems[systemName].EcellSystems[0];
                obj.Layer = system.Layer;
                if(!isCoordSet)
                {
                    obj.X = system.X + ((system.Width > 80) ? 80 : system.Width / 2) - obj.Width;
                    obj.Y = system.Y + ((system.Height > 80) ? 80 : system.Height / 2) - obj.Height;
                    if (obj is PPathwayNode)
                    {
                        ((PPathwayNode)obj).Element.X = obj.X;
                        ((PPathwayNode)obj).Element.Y = obj.Y;
                        ((PPathwayNode)obj).RefreshText();
                    }
                    else if(obj is PSystem)
                    {
                    }
                }
                
                system.AddChild(obj);
                if (obj is PPathwayNode)
                    ((PPathwayNode)obj).ParentObject = system;
            }
            else
            {
                foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
                {
                    if (system.Layer == obj.Layer)
                    {
                        if (!isCoordSet)
                        {
                            obj.X = system.X + ((system.Width > 80) ? 80 : system.Width / 2) - obj.Width;
                            obj.Y = system.Y + ((system.Height > 80) ? 80 : system.Height / 2) - obj.Height;
                            if (obj is PPathwayNode)
                            {
                                ((PPathwayNode)obj).Element.X = obj.X;
                                ((PPathwayNode)obj).Element.Y = obj.Y;
                                ((PPathwayNode)obj).RefreshText();
                            }
                            else if(obj is PSystem)
                            {
                                ((PSystem)obj).Element.X = obj.X;
                                ((PSystem)obj).Element.Y = obj.Y;
                            }
                        }
                        PointF offsetToL = system.OffsetToLayer;
                        obj.OffsetBy(-1 * offsetToL.X, -1 * offsetToL.Y);                        
                        system.AddChild(obj);
                        obj.ParentObject = system;
                    }
                }
            }
            if (obj is PPathwayNode)
                ((PPathwayNode)obj).ShowingID = this.m_showingId;
        }

        /// <summary>
        /// Add PPathwayObject to top visible layer.
        /// </summary>
        /// <param name="obj"></param>
        public void AddChildToTopVisibleLayer(PPathwayObject obj)
        {
            RegisterObjToSet(obj);
            foreach(DataRow row in m_table.Rows)
            {
                if ((bool)row[COLUMN_NAME4SHOW] == true)
                {
                    m_layers[(string)row[COLUMN_NAME4NAME]].AddChild(obj);
                    break;
                }
            }
        }

        /// <summary>
        /// register the object to this set.
        /// </summary>
        /// <param name="obj">the registered object.</param>
        public void RegisterObjToSet(PPathwayObject obj)
        {
            if(obj is PEcellVariable)
            {
                PEcellVariable node = (PEcellVariable)obj;
                m_variables.Add(node.Element.Key, node);
            }
            else if(obj is PEcellProcess)
            {
                PEcellProcess node = (PEcellProcess)obj;
                m_processes.Add(node.Element.Key, node);
            }
            if (obj.CanvasView == null)
                obj.CanvasView = this;
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

            m_pathwayCanvas.Root.AddChild(0,layer);
            m_pathwayCanvas.Camera.AddLayer(0,layer);

            DataRow dr = m_table.NewRow();
            dr[COLUMN_NAME4SHOW] = true;
            dr[COLUMN_NAME4NAME] = name;
            m_table.Rows.Add(dr);

            m_overviewCanvas.AddObservedLayer(layer);

            m_layers.Add(name,layer);
            m_ctrlLayer.MoveToFront();
           
        }

        /// <summary>
        /// Add an attribute to a system by using AttributeElement
        /// </summary>
        /// <param name="systemName">Added to this system</param>
        /// <param name="attrElement">this AttributeElement will be added</param>
        public void AddAttributeToSystem(string systemName, AttributeElement attrElement)
        {
            if (string.IsNullOrEmpty(systemName) || !m_systems.ContainsKey(systemName))
                return;
            SystemContainer sysCon = m_systems[systemName];
            if (sysCon == null)
                return;
            if (sysCon.Text != null)
            {
                PText prevText = sysCon.Text;
                if (m_ctrlLayer.ChildrenReference.Contains(prevText))
                    m_ctrlLayer.RemoveChild(prevText);
            }

            PText text = new PText();
            sysCon.Text = text;
            text.Pickable = false;
            text.Font = new Font("Gothics", 12, System.Drawing.FontStyle.Regular);
            m_ctrlLayer.AddChild(text);
            sysCon.Attribute = attrElement;
            sysCon.UpdateText();
        }

        /// <summary>
        /// Remove attributes from system.
        /// </summary>
        /// <param name="systemName">Attribute will be removed from this system</param>
        public void RemoveAttributeFromSystem(string systemName)
        {
            if (string.IsNullOrEmpty(systemName) || !m_systems.ContainsKey(systemName))
                return;
            SystemContainer sysCon = m_systems[systemName];
            if (sysCon == null)
                return;
            sysCon.Attribute = null;
            sysCon.UpdateText();
        }

        /// <summary>
        /// add system to this set.
        /// </summary>
        /// <param name="systemName">system name.</param>
        /// <param name="systemElement">element.</param>
        /// <param name="attrElement">attribute element.</param>
        /// <param name="systemList">child system.</param>
        public void AddSystem(string systemName, SystemElement systemElement, 
            AttributeElement attrElement, List<PEcellSystem> systemList)
        {
            SystemContainer sysCon = new SystemContainer(systemList, systemElement);
            m_systems.Add(systemName, sysCon);
            this.AddAttributeToSystem(systemName, attrElement);
            foreach (PEcellSystem p in systemList)
            {
                if (p.CanvasView == null)
                    p.CanvasView = this;
            }
        }

        /// <summary>
        /// get the list of system by using system name.
        /// </summary>
        /// <param name="systemName">system name.</param>
        /// <returns>the list of system.</returns>
        public List<PEcellSystem> GetSystem(string systemName)
        {
            return m_systems[systemName].EcellSystems;
        }

        /// <summary>
        /// Whether given rectangle is inside the root system or not.
        /// </summary>
        /// <param name="rectF">a rectangle to be checked.</param>
        /// <returns>True, if the given rectangle is inside the root system.
        ///          False, if the given rectangle is outside the root system.
        /// </returns>
        public bool IsInsideRoot(RectangleF rectF)
        {
            RectangleF rootRect = m_systems["/"].EcellSystems[0].Rect;
            PointF offsetToL = m_systems["/"].EcellSystems[0].OffsetToLayer;
            rootRect.Offset(offsetToL);
            if (rootRect.IntersectsWith(rectF) || rootRect.Contains(rectF))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <param name="excludedSystem">If this parameter is set, this system is excluded from searching</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public string GetSurroundingSystem(PointF point, string excludedSystem)
        {
            string leastSurSystemName = null;
            float minArea = 0f;
            foreach(KeyValuePair<string,SystemContainer> systemPair in m_systems)
            {
                if (!String.IsNullOrEmpty(excludedSystem) && excludedSystem.Equals(systemPair.Key))
                    continue;
                
                if (systemPair.Value.EcellSystems[0].GlobalBounds.Contains(point))
                {
                    RectangleF presentRect = systemPair.Value.EcellSystems[0].GlobalBounds;
                    float area = presentRect.Width * presentRect.Height;

                    if (leastSurSystemName == null)
                    {
                        leastSurSystemName = systemPair.Key;
                        minArea = area;
                    }
                    else
                    {
                        if(area < minArea)
                        {
                            leastSurSystemName = systemPair.Key;
                            minArea = area;
                        }
                    }
                }
            }

            if (leastSurSystemName != null)
                return leastSurSystemName;
            else
                return null;
        }

        /// <summary>
        /// Notify this set that one PPathwayNode is selected
        /// </summary>
        /// <param name="obj">Newly selected object</param>
        /// <param name="toBeNotified">Whether selection must be notified to Ecell-Core or not.</param>
        public void AddSelectedNode(PPathwayNode obj, bool toBeNotified)
        {
            m_selectedNodes.Add(obj);
            foreach (PPathwayObject eachObj in m_selectedNodes)
            {
                eachObj.IsHighLighted = true;
            }
            if(toBeNotified)
                m_pathwayView.NotifySelectChanged(obj.Element.Key, obj.Element.Type);
        }

        /// <summary>
        /// Notify this set that one PEcellSystem is selected.
        /// </summary>
        /// <param name="systemName">the name of selected system.</param>
        public void AddSelectedSystem(string systemName)
        {
            m_selectedSystemName = systemName;
            if(m_systems.ContainsKey(systemName))
            {
                foreach (PEcellSystem system in m_systems[systemName].EcellSystems)
                    system.IsHighLighted = true;
            }
            ShowResizeHandles();
            UpdateResizeHandlePositions();
            string type = m_systems[systemName].Element.Type;
            m_pathwayView.NotifySelectChanged(systemName, type);
        }

        /// <summary>
        /// Select this line on this canvas.
        /// </summary>
        /// <param name="line"></param>
        public void AddSelectedLine(Line line)
        {
            m_isReconnectMode = true;

            m_nodesUnderMouse.Clear();

            line.Visible = false;

            m_selectedLine = line;
            m_vOnLinesEnd = line.Info.VariableKey;
            m_pOnLinesEnd = line.Info.ProcessKey;

            // Prepare line handles
            m_lineHandle4V.Offset = PointF.Empty;
            m_lineHandle4P.Offset = PointF.Empty;

            m_lineHandle4V.X = line.VarPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4V.Y = line.VarPoint.Y - LINE_HANDLE_RADIUS;

            m_lineHandle4P.X = line.ProPoint.X - LINE_HANDLE_RADIUS;
            m_lineHandle4P.Y = line.ProPoint.Y - LINE_HANDLE_RADIUS;

            m_ctrlLayer.AddChild(m_lineHandle4V);
            m_ctrlLayer.AddChild(m_lineHandle4P);

            // Create line
            m_line4reconnect.Pen = LINE_THIN_PEN;
            m_line4reconnect.Reset();
            switch (line.Info.TypeOfLine)
            {
                case LineType.Solid:
                    m_line4reconnect.AddLine(line.VarPoint.X, line.VarPoint.Y, line.ProPoint.X, line.ProPoint.Y);
                    break;
                case LineType.Dashed:
                    PEcellProcess.AddDashedLine(m_line4reconnect, line.VarPoint.X, line.VarPoint.Y, line.ProPoint.X, line.ProPoint.Y);
                    break;
                case LineType.Unknown:
                    m_line4reconnect.AddLine(line.VarPoint.X, line.VarPoint.Y, line.ProPoint.X, line.ProPoint.Y);
                    break;
            }

            switch (line.Info.Direction)
            {
                case EdgeDirection.Bidirection:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(line.ProPoint, line.VarPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(line.VarPoint, line.ProPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.Inward:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(line.ProPoint, line.VarPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.Outward:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(line.VarPoint, line.ProPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.None:
                    break;
            }
            SetLineVisibility(true);
        }

        /// <summary>
        /// Show/Hide line4reconnect.
        /// </summary>
        /// <param name="visible">visibility</param>
        public void SetLineVisibility(bool visible)
        {
            if (null == m_line4reconnect)
                return;

            if (visible)
                m_ctrlLayer.AddChild(m_line4reconnect);
            else
            {
                if(null != m_line4reconnect.Parent)
                    m_ctrlLayer.RemoveChild(m_line4reconnect);
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
            foreach (SystemContainer sysCon in m_systems.Values)
                sysCon.Freeze();
            foreach (PEcellVariable var in m_variables.Values)
                var.Freeze();
            foreach (PEcellProcess pro in m_processes.Values)
                pro.Freeze();            
        }

        /// <summary>
        /// Get PathwayElement indicated by a key.
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public PathwayElement GetElement(ComponentType ct, string key)
        {
            if(null != key && key.EndsWith(":SIZE"))
            {
                return m_systems[PathUtil.GetParentSystemId(key)].Attribute;
            }

            switch(ct)
            {
                case ComponentType.System:
                    return m_systems[key].Element;                    
                case ComponentType.Variable:
                    return m_variables[key].Element;
                case ComponentType.Process:
                    return m_processes[key].Element;
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Get a key list of systems under a given system.
        /// </summary>
        /// <param name="systemKey"></param>
        /// <returns>list of Ecell key of systems</returns>
        public List<string> GetAllSystemUnder(string systemKey)
        {
            List<string> returnList = new List<string>();
            foreach(string key in m_systems.Keys)
            {
                if (key.StartsWith(systemKey + "/") && !key.Equals(systemKey))
                    returnList.Add(key);
            }
            return returnList;
        }

        /// <summary>
        /// Get a key list of variables under a given system.
        /// </summary>
        /// <param name="systemKey"></param>
        /// <returns>list of Ecell key of variables</returns>
        public List<string> GetAllVariableUnder(string systemKey)
        {
            List<string> returnList = new List<string>();
            foreach(string key in m_variables.Keys)
            {
                if(key.StartsWith(systemKey) && !key.Equals(systemKey))
                    returnList.Add(key);
            }
            return returnList;
        }

        /// <summary>
        /// Get a key list of processes under a given system.
        /// </summary>
        /// <param name="systemKey"></param>
        /// <returns>list of Ecell key of processes</returns>
        public List<String> GetAllProcessUnder(string systemKey)
        {
            List<string> returnList = new List<string>();
            foreach(string key in m_processes.Keys)
            {
                if(key.StartsWith(systemKey) && !key.Equals(systemKey))
                    returnList.Add(key);
            }
            return returnList;
        }

        /// <summary>
        /// Get all PathwayElements of this object.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<PathwayElement> GetAllElements()
        {
            List<PathwayElement> returnList = new List<PathwayElement>();
            
            // Create CanvasElement from canvas information
            CanvasElement canvasElement = new CanvasElement();
            canvasElement.CanvasID = m_canvasId;
            canvasElement.HasGrid = false;
            canvasElement.OffsetX = m_pathwayCanvas.Camera.OffsetX;
            canvasElement.OffsetY = m_pathwayCanvas.Camera.OffsetY;
            canvasElement.Status = "default";
            returnList.Add(canvasElement);
            
            foreach(SystemContainer sysCon in m_systems.Values)
            {
                returnList.Add( sysCon.Element );
            }

            // Create LayerElement from layer information
            int zorder = 0;
            foreach(KeyValuePair<string,PLayer> layer in m_layers)
            {
                LayerElement layerElement = new LayerElement();
                
                layerElement.CanvasID = m_canvasId;
                layerElement.LayerID = layer.Key;
                layerElement.ZOrder = zorder;
                returnList.Add(layerElement);
                zorder++;

                foreach(PNode node in layer.Value.ChildrenReference)
                {
                    if (node is PPathwayObject)
                    {
                        foreach(PathwayElement pathEle in ((PPathwayObject)node).GetElements())
                        {
                            if(pathEle is ComponentElement)
                            {
                                ((ComponentElement)pathEle).CanvasID = m_canvasId;
                                ((ComponentElement)pathEle).LayerID = layer.Key;
                            }
                            returnList.Add(pathEle);
                        }
                    }
                }
            }
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
                m_pathwayCanvas.Camera.TranslateViewBy(0,delta);
            else
                m_pathwayCanvas.Camera.TranslateViewBy(delta, 0);
            this.UpdateOverview();
        }

        /// <summary>
        /// get bitmap image projected PathwayEditor.
        /// </summary>
        /// <returns></returns>
        public Bitmap ToImage()
        {
            return new Bitmap(m_pathwayCanvas.Layer[0].ToImage());
        }

        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of object before chage.</param>
        /// <param name="data">the type of object before change.</param>
        /// <param name="type">the changed object.</param>
        public void DataChanged(string key, EcellObject data, ComponentType type)
        {
            if (key == null || data.key == null)
                return;

            switch(type)
            {
                case ComponentType.System:
                    if (!m_systems.ContainsKey(key))
                        return;
                    SystemContainer sysCon = m_systems[key];
                    m_systems.Remove(key);
                    sysCon.Element.Key = data.key;
                    foreach(PEcellSystem sys in sysCon.EcellSystems)
                    {
                        sys.Element.Key = data.key;
                        sys.Name = data.key;
                    }
                    sysCon.UpdateText();
                    m_systems.Add(data.key, sysCon);
                    break;
                case ComponentType.Variable:
                    if (!m_variables.ContainsKey(key))
                        return;
                    PEcellVariable var = m_variables[key];
                    m_variables.Remove(key);
                    var.Element.Key = data.key;
                    var.Name = PathUtil.RemovePath(data.key);
                    var.RefreshText();
                    m_variables.Add(data.key, var);
                    var.ReconstructEdges();
                    break;
                case ComponentType.Process:                    
                    if (!m_processes.ContainsKey(key))
                        return;
                    PEcellProcess pro = m_processes[key];
                    string vrl = null;
                    foreach (EcellData ed in data.M_value)
                    {
                        if (ed.M_name.Equals("VariableReferenceList"))
                        {
                            vrl = ed.M_value.ToString();
                        }
                    }
                    ProcessElement pe = (ProcessElement)pro.Element;
                    pe.SetEdgesByStr(vrl);
                    pro.DeleteEdges();
                    pro.CreateEdges();
                    
                    // switch keys of dictionary
                    m_processes.Remove(key);
                    pro.Element.Key = data.key;
                    pro.Name = PathUtil.RemovePath(data.key);
                    pro.RefreshText();
                    m_processes.Add(data.key, pro);
                    break;
            }
        }

        /// <summary>
        /// event sequence of deleting the object.
        /// </summary>
        /// <param name="key">the key of deleted object.</param>
        /// <param name="type">the type of deleted object.</param>
        public void DataDelete(string key, ComponentType type)
        {
            ResetSelectedObjects();
            switch (type)
            {
                case ComponentType.System:
                    foreach (PEcellSystem system in m_systems[key].EcellSystems)
                        system.Parent.RemoveChild(system);
                    
                    SystemContainer con = m_systems[key];
                    int ind = m_ctrlLayer.IndexOfChild(con.Text);
                    m_ctrlLayer.RemoveChild(ind);

                    m_systems.Remove(key);

                    if (m_selectedSystemName != null && key.Equals(m_selectedSystemName))
                    {
                        HideResizeHandles();
                        m_selectedSystemName = null;
                    }
                    List<string> deleteList = new List<string>();
                    foreach(KeyValuePair<string, SystemContainer> pair in m_systems)
                    {
                        if (pair.Key.Equals(key) || pair.Key.StartsWith(key + "/"))
                        {
                            PText idTxt = pair.Value.Text;
                            idTxt.Parent.RemoveChild(idTxt);
                            deleteList.Add(pair.Key);
                        }
                    }

                    foreach(string delete in deleteList)
                    {
                        m_systems.Remove(delete);
                    }

                    break;
                case ComponentType.Variable:
                    if (m_variables.ContainsKey(key))
                    {
                        PEcellVariable v = m_variables[key];
                        v.NotifyRemoveRelatedVariable();
                        v.Parent.RemoveChild(v);
                        v.Reset();
                        m_variables.Remove(key);
                    }
                    else if (key.EndsWith("SIZE"))
                    {
                        this.RemoveAttributeFromSystem(PathUtil.GetParentSystemId(key));
                    }
                    break;
                case ComponentType.Process:
                    if (m_processes.ContainsKey(key))
                    {
                        PEcellProcess p = m_processes[key];
                        p.NotifyRemoveRelatedProcess();
                        p.DeleteEdges();
                        p.Parent.RemoveChild(p);
                        p.Reset();
                        m_processes.Remove(key);
                    }
                    break;
            }            
        }

        /// <summary>
        /// event sequence of changing the information of object.
        /// </summary>
        /// <param name="key">the key of selected object.</param>
        /// <param name="type">the type of selected object.</param>
        public void SelectChanged(string key, ComponentType type)
        {
            switch(type)
            {
                case ComponentType.System:
                    if (m_selectedSystemName != null && m_selectedSystemName.Equals(key))
                        return;
                    this.ResetSelectedObjects();
                    this.AddSelectedSystem(key);
                    PEcellSystem focusSystem = (PEcellSystem)m_systems[key].EcellSystems[0];
                    if(null != focusSystem)
                    {
                        m_pathwayCanvas.Camera.AnimateViewToCenterBounds(focusSystem.FullBounds,
                                                                             true,
                                                                             CAMERA_ANIM_DURATION);

                        UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
                    }
                    break;
                case ComponentType.Variable:
                    bool isAlreadySelected = false;
                    foreach (PPathwayNode selectNode in m_selectedNodes)
                    {
                        if (key.Equals(selectNode.Element.Key) && selectNode is PEcellVariable)
                        {
                            isAlreadySelected = true;
                            break;
                        }
                    }
                    if (!isAlreadySelected)
                    {
                        this.ResetSelectedObjects();
                        if(m_variables.ContainsKey(key))
                        {
                            PPathwayNode focusNode = (PPathwayNode)m_variables[key];
                            this.AddSelectedNode(focusNode, false);
                            m_pathwayCanvas.Camera.AnimateViewToCenterBounds(PathUtil.GetFocusBound(focusNode.FullBounds, LEAST_FOCUS_SIZE),
                                                                             true,
                                                                             CAMERA_ANIM_DURATION);
                            UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
                        }
                    }
                    break;
                case ComponentType.Process:
                    bool isProAlreadySelected = false;
                    foreach (PPathwayNode selectNode in m_selectedNodes)
                    {
                        if (key.Equals(selectNode.Element.Key) && selectNode is PEcellProcess)
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
                        m_pathwayCanvas.Camera.AnimateViewToCenterBounds(PathUtil.GetFocusBound( focusNode.FullBounds, LEAST_FOCUS_SIZE ),
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
            foreach (SystemContainer sysCon in m_systems.Values)
                sysCon.UnFreeze();
            foreach (PEcellVariable var in m_variables.Values)
                var.Unfreeze();
            foreach (PEcellProcess pro in m_processes.Values)
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
            RectangleF localF = m_pathwayCanvas.Camera.Bounds;
            RectangleF recf = m_pathwayCanvas.Camera.ViewBounds;
            PMatrix matrix = m_pathwayCanvas.Camera.ViewMatrix;
            m_area.Reset();
            m_area.Offset = ZERO_POINT;
            m_area.AddRectangle(recf.X, recf.Y, recf.Width, recf.Height);
            m_overviewCanvas.UpdateTransparent();
            m_overviewCanvas.Refresh();
        }

        /// <summary>
        /// change position of button that is whether overview show of no show.
        /// </summary>
        /// <param name="lowerPanelShown">the  flag whether overview show.</param>
        public void UpdateShowButton(bool lowerPanelShown)
        {
            RectangleF rect = m_pathwayCanvas.Camera.Bounds;
            if (lowerPanelShown)
                m_showBtnDownward.Offset = new PointF(rect.X, rect.Y + rect.Height - m_showBtnWidth - 1);
            else
                m_showBtnUpward.Offset = new PointF(rect.X, rect.Y + rect.Height - m_showBtnWidth - 1);
        }

        /// <summary>
        /// change whether show lower overview.
        /// </summary>
        /// <param name="lowerPanelShown">the flag whether show the lower overview.</param>
        public void ChangeShowButton(bool lowerPanelShown)
        {
            if(lowerPanelShown)
            {
                m_pathwayCanvas.Camera.RemoveChild(m_showBtnUpward);
                m_pathwayCanvas.Camera.AddChild(m_showBtnDownward);
            }
            else
            {
                m_pathwayCanvas.Camera.RemoveChild(m_showBtnDownward);
                m_pathwayCanvas.Camera.AddChild(m_showBtnUpward);
            }
            UpdateShowButton(lowerPanelShown);
        }

        /// <summary>
        /// Release all the unmanaged resources in this object
        /// </summary>
        public void Dispose()
        {
            if (m_pathwayTabPage != null)
                m_pathwayTabPage.Dispose();

            if (m_pathwayCanvas != null)
                m_pathwayCanvas.Dispose();

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
                foreach (PEcellSystem system in m_systems[m_selectedSystemName].EcellSystems)
                {
                    system.IsHighLighted = false;
                }
                m_selectedSystemName = null;
            }
            HideResizeHandles();
        }

        /// <summary>
        /// reset the seletect nodes.
        /// </summary>
        public void ResetSelectedNodes()
        {
            if (m_selectedNodes.Count == 0)
            {
                return;
            }
            PNodeList spareList = new PNodeList();
            foreach (PPathwayObject obj in m_selectedNodes)
            {
                obj.IsHighLighted = false;
            }
            lock (this)
            {
                if (m_selectedNodes.Count != 0)
                    m_selectedNodes.RemoveRange(0, m_selectedNodes.Count);
            }
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
            if(null == m_selectedLine)
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
            switch (m_selectedLine.Info.TypeOfLine)
            {
                case LineType.Solid:
                    m_line4reconnect.AddLine(varPoint.X, varPoint.Y, proPoint.X, proPoint.Y);
                    break;
                case LineType.Dashed:
                    PEcellProcess.AddDashedLine(m_line4reconnect, varPoint.X, varPoint.Y, proPoint.X, proPoint.Y);
                    break;
                case LineType.Unknown:
                    m_line4reconnect.AddLine(varPoint.X, varPoint.Y, proPoint.X, proPoint.Y);
                    break;
            }

            switch (m_selectedLine.Info.Direction)
            {
                case EdgeDirection.Bidirection:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(proPoint, varPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(varPoint, proPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.Inward:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(proPoint, varPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.Outward:
                    m_line4reconnect.AddPolygon(PathUtil.GetArrowPoints(varPoint, proPoint, PEcellProcess.ARROW_RADIAN_A, PEcellProcess.ARROW_RADIAN_B, PEcellProcess.ARROW_LENGTH));
                    break;
                case EdgeDirection.None:
                    break;
            }
        }

        /// <summary>
        /// Reset selected line
        /// </summary>
        public void ResetSelectedLine()
        {
            m_isReconnectMode = false;

            m_nodesUnderMouse.Clear();

            if(null != m_selectedLine)
            {
                m_selectedLine.Visible = true;
            }

            m_selectedLine = null;
            m_vOnLinesEnd = null;
            m_pOnLinesEnd = null;

            if(null != m_lineHandle4V && null != m_lineHandle4V.Parent)
            {
                m_ctrlLayer.RemoveChild(m_lineHandle4V);
            }
            if(null != m_lineHandle4P && null != m_lineHandle4P.Parent)
            {
                m_ctrlLayer.RemoveChild(m_lineHandle4P);
            }
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
            foreach(PLayer layer in m_layers.Values)
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
            m_pathwayCanvas.Refresh();
        }

        #region Private methods
        /// <summary>
        /// get a vacant point of a system for newly coming entity.
        /// </summary>
        /// <param name="wholeSpace"></param>
        /// <param name="excludeRectList"></param>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        private PointF GetVacantPoint(RectangleF wholeSpace, List<RectangleF> excludeRectList, List<PPathwayNode> nodeList)
        {
            

            // Set margin. nodes can't be placed there.
            RectangleF spaceWithoutMargin = new RectangleF();
            spaceWithoutMargin.X = (float)(wholeSpace.X + wholeSpace.Width * 0.1);
            spaceWithoutMargin.Width = (float)(wholeSpace.Width * 0.8);
            spaceWithoutMargin.Y = (float)(wholeSpace.Y + wholeSpace.Height * 0.1);
            spaceWithoutMargin.Height = (float)(wholeSpace.Height * 0.8);

            // List of point of coordinate of each node of nodeList
            List<PointF> pointList = new List<PointF>();

            foreach(PPathwayNode node in nodeList)
            {
                PointF point = new PointF(node.X + node.OffsetX, node.Y + node.OffsetY);
                pointList.Add(point);
            }
                        
            int numOfSampling = 20;
            bool isSufficient = false; // Whether at least one valid point was found or not.

            PointF vacantPoint = PointF.Empty;
            float maxSumOfDistance = 0; // sum of distances between a vacant point and each node points.
            Random rand = new Random();

            // Repeat sampling for numOfSampling times, and settle most suitable vacant point
            for ( int i = 0; i < numOfSampling || !isSufficient; i++ )
            {
                RectangleF space = (i < numOfSampling) ? spaceWithoutMargin : wholeSpace;

                PointF vacantPointCand
                    = new PointF(space.X + (float)rand.NextDouble() * space.Width,
                    space.Y + (float)rand.NextDouble() * space.Height);

                if (!this.DoRectsContainAPoint(vacantPointCand, excludeRectList))
                    isSufficient = true;
                else
                    continue;

                float sumOfDistance = 0;

                // Add distance between vacantPointCand and bound of space.
                sumOfDistance += (vacantPointCand.X - wholeSpace.X < wholeSpace.Right - vacantPointCand.X)
                    ? vacantPointCand.X - wholeSpace.X : wholeSpace.Right - vacantPointCand.X;
                sumOfDistance += (vacantPointCand.Y - wholeSpace.Y < wholeSpace.Bottom - vacantPointCand.Y)
                    ? vacantPointCand.Y - wholeSpace.Y : wholeSpace.Bottom - vacantPointCand.Y;
                
                foreach(PointF point in pointList)
                {
                    sumOfDistance += PathUtil.GetDistance(point, vacantPointCand);
                }

                if(maxSumOfDistance < sumOfDistance)
                {
                    maxSumOfDistance = sumOfDistance;
                    vacantPoint = vacantPointCand;
                }
            }

            return vacantPoint;
        }

        /// <summary>
        /// Return whether a list of rectangle contains a point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rectList"></param>
        /// <returns>true if at least one rectangle of list contains a point, otherwise return false.</returns>
        private bool DoRectsContainAPoint(PointF point, List<RectangleF> rectList)
        {
            foreach(RectangleF rect in rectList)
            {
                if (rect.Contains(point))
                    return true;
            }
            return false;
        }
        #endregion
        
        class NodeDragHandler : PDragEventHandler
        {
            #region Fields
            /// <summary>
            /// the related CanvasViewCompomnetSet.
            /// </summary>
            private CanvasView m_set;

            /// <summary>
            /// PComposite to move selected nodes together.
            /// </summary>
            private PComposite m_composite;

            /// <summary>
            /// Point, where the mouse is down.
            /// </summary>
            private PointF m_downPosition;

            /// <summary>
            /// dictionary of ssystem container that key is name.
            /// </summary>
            private Dictionary<string, SystemContainer> m_dict;

            /// <summary>
            /// Edges will be refreshed every time when this process has moved by this distance.
            /// </summary>
            private static readonly float m_refreshDistance = 4;
        
            /// <summary>
            /// the delta of moving.
            /// </summary>
            private SizeF m_movingDelta = new SizeF(0, 0);

            /// <summary>
            /// the list of SystemContainer.
            /// </summary>
            private List<SystemContainer> m_sysConList = new List<SystemContainer>();

            /// <summary>
            /// the list of PEcellSystem.
            /// </summary>
            private List<PEcellSystem> m_systemList;
            /// <summary>
            /// ResourceManager for PathwayWindow.
            /// </summary>
            ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));


            #endregion

            /// <summary>
            /// constructor with initial parameters.
            /// </summary>
            /// <param name="set">component set.</param>
            /// <param name="dict">dictionary of system.</param>
            public NodeDragHandler(CanvasView set, Dictionary<string,SystemContainer> dict)
            {
                m_set = set;
                m_dict = dict;
            }

            /// <summary>
            /// check whether this event is mouse event.
            /// </summary>
            /// <param name="e">event.</param>
            /// <returns>mouse event is true.</returns>
            public override bool DoesAcceptEvent(PInputEventArgs e)
            {
                return e.IsMouseEvent
                   && (e.Button == MouseButtons.Left || e.IsMouseEnterOrMouseLeave);
            }
            
            /// <summary>
            /// event on down the mouse button in canvas.
            /// </summary>
            /// <param name="sender">PathwayView.</param>
            /// <param name="e">PInputEventArgs.</param>
            public override void OnMouseDown(object sender, PInputEventArgs e)
            {
                base.OnMouseDown(sender, e);
                
                if (e.PickedNode is PEcellProcess)
                    ((PEcellProcess)e.PickedNode).ValidateEdges();

                if (e.PickedNode is PPathwayNode)
                {
                    PPathwayNode pnode = (PPathwayNode)e.PickedNode;
                    pnode.MemorizeCurrentPosition();
                    pnode.CancelAllParentOffsets();
                    m_set.ControlLayer.AddChild(pnode);

                    m_downPosition = new PointF(e.Position.X, e.Position.Y);
                    m_composite = new PComposite();
                    m_set.ControlLayer.AddChild(m_composite);
                    foreach (PPathwayObject obj in m_set.SelectedNodes)
                    {
                        if (obj != e.PickedNode)
                        {
                            obj.MemorizeCurrentPosition();
                            obj.CancelAllParentOffsets();
                            m_composite.AddChild(obj);                            
                        }
                    }
                }
                else if(e.PickedNode is PEcellSystem)
                {
                    ((PEcellSystem)e.PickedNode).MemorizeCurrentPosition();
                }

                e.Canvas.BackColor = Color.Silver;
                m_set.SetBackToDefault();
                m_set.SetShadeWithoutSystem(m_set.GetSurroundingSystem(e.Position, null));
            }

            /// <summary>
            /// event on start to drag PNode.
            /// </summary>
            /// <param name="sender">PathwayView.</param>
            /// <param name="e">PInputEventArgs.</param>
            protected override void OnStartDrag(object sender, PInputEventArgs e)
            {
                base.OnStartDrag(sender, e);
                if (e.PickedNode is PEcellSystem)
                {
                    SystemContainer system = m_dict[((PEcellSystem)e.PickedNode).Name];
                    m_systemList = m_set.GetSystem(system.Element.Key);
                    foreach (KeyValuePair<string, SystemContainer> sysPair in m_dict)
                    {
                        if (system.Element.CheckRelation(sysPair.Key) == SystemElement.Relation.Inferior)
                        {
                            m_sysConList.Add(sysPair.Value);
                        }
                    }
                }
                e.Handled = true;
                if (!(e.PickedNode is PEcellComposite) ||
                    e.PickedNode.ChildrenCount != 1 ||
                    !(e.PickedNode.ChildrenReference[0] is PSystem))
                {
                    e.PickedNode.MoveToFront();
                }
            }

            /// <summary>
            /// event on drag PNode.
            /// </summary>
            /// <param name="sender">PathwayView.</param>
            /// <param name="e">PInputEventArgs.</param>
            protected override void OnDrag(object sender, PInputEventArgs e)
            {
                base.OnDrag(sender, e);
                e.Canvas.BackColor = Color.Silver;
                m_set.SetBackToDefault();
                m_set.SetShadeWithoutSystem( m_set.GetSurroundingSystem(e.Position, null) );
                if(e.PickedNode is PEcellSystem)
                {
                    PEcellSystem picked = (PEcellSystem)e.PickedNode;
                    SystemElement element = m_dict[picked.Name].Element;
                    element.X = e.PickedNode.X;
                    element.Y = e.PickedNode.Y;
                    element.OffsetX = e.PickedNode.OffsetX;
                    element.OffsetY = e.PickedNode.OffsetY;
                    m_dict[picked.Name].UpdateText();
                    foreach(PEcellSystem system in m_systemList)
                    {
                        system.X = e.PickedNode.X;
                        system.Y = e.PickedNode.Y;
                        system.Offset = e.PickedNode.Offset;
                        system.NotifyMovement();
                    }
                    foreach(SystemContainer sysCon in m_sysConList)
                    {
                        sysCon.UpdateText();
                    }
                    // Change color if the system overlaps other system
                    PointF offset = picked.OffsetToLayer;
                    RectangleF rectF = new RectangleF(picked.X + offset.X,
                                                      picked.Y + offset.Y,
                                                      picked.Width,
                                                      picked.Height);
                    if (m_set.DoesSystemOverlaps(picked.GlobalBounds, picked.Element.Key)
                        || !m_set.IsInsideRoot(rectF))
                        picked.IsInvalid = true;
                    else
                        picked.IsInvalid = false;
                    m_set.UpdateResizeHandlePositions();

                    picked.MoveStart();
                }
                else if(e.PickedNode is PPathwayNode)
                {
                    m_composite.OffsetX += e.Delta.Width;
                    m_composite.OffsetY += e.Delta.Height;

                    m_movingDelta += e.CanvasDelta;

                    if ((Math.Abs(m_movingDelta.Width) + Math.Abs(m_movingDelta.Height)) > m_refreshDistance)
                    {
                        m_movingDelta = new SizeF(0, 0);
                        foreach (PPathwayNode node in m_set.SelectedNodes)
                            node.Refresh();
                    }
                }
            }

            /// <summary>
            /// event on end to drag PNode in PathwayView.
            /// </summary>
            /// <param name="sender">Pathwayview.</param>
            /// <param name="e">PInputEventArgs.</param>
            protected override void OnEndDrag(object sender, PInputEventArgs e)
            {
                base.OnEndDrag(sender, e);
                
                if(e.PickedNode is PPathwayNode)
                {
                    PPathwayNode pnode = (PPathwayNode)e.PickedNode;
                    pnode.OffsetX = 0; pnode.OffsetY = 0;
                    //PointF cp = 
                    //    m_set.Systems[PathUtil.GetParentSystemId(pnode.Element.Key)].EcellSystems[0].SystemPos2CanvasPos(new PointF(pnode.X, pnode.Y));
                    //pnode.X = cp.X;
                    //pnode.Y = cp.Y;
                    ReturnToSystem(pnode, m_downPosition, e.Position);

                    PNodeList togetherList = new PNodeList();

                    foreach (PNode together in m_composite.ChildrenReference)
                    {
                        togetherList.Add(together);
                    }

                    foreach (PNode together in togetherList)
                    {
                        PPathwayNode ptogether = (PPathwayNode)together;
                        ReturnToSystem(ptogether,
                                       new PointF(ptogether.X, ptogether.Y),
                                       new PointF(ptogether.X + m_composite.OffsetX, ptogether.Y + m_composite.OffsetY));
                    }
                    if (m_set.ControlLayer.ChildrenReference.Contains(m_composite))
                    {
                        m_set.ControlLayer.RemoveChild(m_composite);
                        m_composite = null;
                    }
                }
                else if(e.PickedNode is PEcellSystem && !(((PEcellSystem)e.PickedNode).Parent is PLayer))
                {
                    PEcellSystem picked = (PEcellSystem)e.PickedNode;
                    PointF offset = picked.OffsetToLayer;
                    RectangleF rectF = new RectangleF(picked.X + offset.X,
                                                      picked.Y + offset.Y,
                                                      picked.Width,
                                                      picked.Height);

                    if(m_set.DoesSystemOverlaps(picked.GlobalBounds, picked.Element.Key)
                        || !m_set.IsInsideRoot(rectF))
                    {
                        picked.ReturnToMemorizedPosition();
                        m_set.m_systems[picked.Name].UpdateText();
                        m_set.UpdateResizeHandlePositions();
                        picked.IsInvalid = false;
                    }
                    else
                    {
                        string oldSystemName = ((PEcellSystem)e.PickedNode).Name;
                        string surSys = m_set.GetSurroundingSystem(e.Position, oldSystemName);
                        string newSys = null;
                        if (surSys.Equals("/"))
                            newSys = "/" + PathUtil.RemovePath(oldSystemName);
                        else
                            newSys = surSys + "/" + PathUtil.RemovePath(oldSystemName);
                        if (!oldSystemName.Equals(newSys) && m_set.Systems.ContainsKey(newSys))
                        {
                            MessageBox.Show(newSys + m_resources.GetString("ErrAlrExist") , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            picked.ReturnToMemorizedPosition();
                            m_set.m_systems[picked.Name].UpdateText();
                            m_set.UpdateResizeHandlePositions();
                            picked.IsInvalid = false;
                        }
                        else
                        {
                            if (surSys == null || !surSys.Equals(PathUtil.GetParentSystemId(oldSystemName)))
                                m_set.TransferSystemTo(surSys, oldSystemName);
                        }
                    }
                }
                if(m_sysConList.Count != 0)
                    m_sysConList.RemoveRange(0,m_sysConList.Count);
                m_set.SetBackToDefault();
                m_set.UpdateOverview();
                if (e.PickedNode is PEcellSystem)
                {
                    PEcellSystem picked = (PEcellSystem)e.PickedNode;
                    picked.MoveEnd();
                }
            }

            private void ReturnToSystem(PPathwayNode node, PointF oldPosition, PointF newPosition)
            {
                string oldSystem = PathUtil.GetParentSystemId(((PPathwayNode)node).Element.Key);

                if (m_set.GetSurroundingSystem(newPosition, null) != null)
                {
                    string newSystem = m_set.GetSurroundingSystem(newPosition, null);
                    
                    node.X += newPosition.X - oldPosition.X;
                    node.Y += newPosition.Y - oldPosition.Y;

                    if (newSystem.Equals(oldSystem))
                    {
                        m_set.TransferNodeTo(m_set.GetSurroundingSystem(newPosition, null), (PPathwayObject)node);
                    }
                    else if (node is PEcellVariable)
                    {
                        string nodeName = PathUtil.RemovePath(((PEcellVariable)node).Element.Key);
                        if (m_set.Variables.ContainsKey(newSystem + ":" + nodeName))
                        {
                            ((PPathwayNode)node).ParentObject.AddChild(node);
                            ((PPathwayNode)node).ReturnToMemorizedPosition();
                            MessageBox.Show(nodeName + m_resources.GetString("ErrAlrExist"),
                                            "Error", MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                        else
                        {
                            m_set.TransferNodeTo(m_set.GetSurroundingSystem(newPosition, null), (PPathwayObject)node);
                        }
                    }
                    else if (node is PEcellProcess)
                    {
                        string nodeName = PathUtil.RemovePath(((PEcellProcess)node).Element.Key);
                        if (m_set.Processes.ContainsKey(newSystem + ":" + nodeName))
                        {
                            ((PPathwayNode)node).ParentObject.AddChild(node);
                            ((PPathwayNode)node).ReturnToMemorizedPosition();
                            MessageBox.Show(nodeName + m_resources.GetString("ErrAlrExist"),
                                            "Error", MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                        else
                        {
                            m_set.TransferNodeTo(m_set.GetSurroundingSystem(newPosition, null), (PPathwayObject)node);
                        }
                    }
                }
                else
                {
                    m_set.TransferNodeTo(oldSystem, (PPathwayObject)node);
                    ((PPathwayNode)node).ReturnToMemorizedPosition();
                }
            }
        }

        /// <summary>
        /// Utility class for collecting all objects relating a certain system.
        /// By using this class, it becomes easier to managing all resources about a system.
        /// </summary>
        public class SystemContainer
        {
            #region Fields
            /// <summary>
            /// Physically, PEcellSystem (a subclass of Piccolo PNode) of a system exists in
            /// every PLayer. If there are eight PLayers in a canvas, there are eight PEcellSystems.
            /// This list contains these PEcellSystem.
            /// </summary>
            private List<PEcellSystem> m_pEcellSystems;

            /// <summary>
            /// SystemElement of this system.
            /// </summary>
            private SystemElement m_systemElement;

            /// <summary>
            /// AttributeElement for this system.
            /// </summary>
            private AttributeElement m_attrElement;

            /// <summary>
            /// PText, which is for indicating the system name on pathway canvas.
            /// </summary>
            private PText m_pText;
            #endregion

            #region Accessors
            /// <summary>
            /// get/set the list of related system.
            /// </summary>
            public List<PEcellSystem> EcellSystems
            {
                get { return m_pEcellSystems; }
                set { m_pEcellSystems = value; }
            }

            /// <summary>
            /// get/set the related element in container.
            /// </summary>
            public SystemElement Element
            {
                get { return m_systemElement; }
                set { m_systemElement = value; }
            }

            /// <summary>
            /// get/set the attribute in container.
            /// </summary>
            public AttributeElement Attribute
            {
                get { return m_attrElement; }
                set { m_attrElement = value; }
            }

            /// <summary>
            /// get/set the text displayed on PathwayView.
            /// </summary>
            public PText Text
            {
                get { return m_pText; }
                set { m_pText = value; }
            }
            #endregion

            #region Constructors
            /// <summary>
            /// constructor with initial parameters.
            /// </summary>
            /// <param name="ecellSystems">the list of related systems.</param>
            /// <param name="element">the related element.</param>
            public SystemContainer(List<PEcellSystem> ecellSystems, SystemElement element)
            {
                m_pEcellSystems = ecellSystems;
                m_systemElement = element;
            }
            #endregion

            #region Methods
            /// <summary>
            /// Freeze this system.
            /// </summary>
            public void Freeze()
            {
                foreach(PEcellSystem system in this.EcellSystems)
                    system.Freeze();
            }
            /// <summary>
            /// Unfreeze this system.
            /// </summary>
            public void UnFreeze()
            {
                foreach (PEcellSystem system in this.EcellSystems)
                    system.Unfreeze();
            }
            /// <summary>
            /// create the text displayed on PathwayEditor.
            /// if this system has the variable of SIZE,
            /// it add the attribute of SIZE to the text.
            /// </summary>
            public void UpdateText()
            {
                string attribute = "";
                if (m_attrElement != null)
                    attribute = " (SIZE:" + m_attrElement.Value + ")";

                if (String.IsNullOrEmpty(m_systemElement.Name))
                    m_pText.Text = "/" + attribute;
                else
                    m_pText.Text = m_systemElement.Name + attribute;
                m_pText.CenterBoundsOnPoint(m_pEcellSystems[0].TextCenterX, m_pEcellSystems[0].TextCenterY);
            }
            #endregion
        }
    }
}