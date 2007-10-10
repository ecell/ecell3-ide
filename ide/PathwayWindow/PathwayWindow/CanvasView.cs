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
using PathwayWindow;
using PathwayWindow.UIComponent;
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
        public static readonly string CANVAS_MENU_DELETE = "Delete";

        /// <summary>
        /// Key definition of m_cMenuDict for copy
        /// </summary>
        public static readonly string CANVAS_MENU_COPY = "Copy";

        /// <summary>
        /// Key definition of m_cMenuDict for cut
        /// </summary>
        public static readonly string CANVAS_MENU_CUT = "Cut";

        /// <summary>
        /// Key definition of m_cMenuDict for paste
        /// </summary>
        public static readonly string CANVAS_MENU_PASTE = "Paste";

        /// <summary>
        /// Key definition of m_cMenuDict for delete
        /// </summary>
        public static readonly string CANVAS_MENU_DELETE_WITH = "deletewith";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_CREATE_LOGGER = "Create Logger";

        /// <summary>
        /// Key definition of m_cMenuDict for delete Logger
        /// </summary>
        public static readonly string CANVAS_MENU_DELETE_LOGGER = "Delete Logger";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_SIZE = "Size";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_ACTIVITY = "Activity";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_MOLAR_ACTIVITY = "Molar Activity";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_DIF_COEFF = "Diffusion Coeff";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_MOL_CONC = "Molar Conc";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_NUM_CONC = "Number Conc";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_VALUE = "Value";

        /// <summary>
        /// Key definition of m_cMenuDict for Create Logger
        /// </summary>
        public static readonly string CANVAS_MENU_LOGGER_VELOCITY = "Velocity";

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
        protected PathwayView m_view;

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
        /// the canvas of overview.
        /// </summary>
        protected OverView m_overview;

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
        List<EcellObject> m_selectedNodes = new List<EcellObject>();

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
        ElementType m_reconnectNodeType;

        /// <summary>
        /// Clicked PathwayObject.
        /// </summary>
        PNode m_clickedNode = null;

        /// <summary>
        /// Stack for nodes under the mouse.
        /// this will be used to reconnect edge.
        /// </summary>
        Stack<EcellObject> m_nodesUnderMouse = new Stack<EcellObject>();
        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_pathwayView.
        /// </summary>
        public PathwayView PathwayView
        {
            get { return m_view; }
            set { m_view = value; }
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
            get { return m_pathwayCanvas; }
        }

        /// <summary>
        /// Accessor for m_selectedNodes.
        /// </summary>
        public List<EcellObject> SelectedNodes
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
        /// Accessor for m_overviewCanvas.
        /// </summary>
        public OverView OverView
        {
            get { return m_overview; }
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
        /// <param name="overviewScale">scale of overview.</param>
        /// <param name="handler">EventHandler of PathwayView.</param>
        public CanvasView(PathwayView view,
            string name,
            float overviewScale,
            PInputEventHandler handler)
        {
            m_view = view;
            m_canvasId = name;

            // Preparing TabPage
            m_pathwayTabPage = new TabPage(name);
            m_pathwayTabPage.Name = name;
            m_pathwayTabPage.AutoScroll = true;

            m_pathwayCanvas = new PathwayCanvas(this);

            m_pathwayCanvas.RemoveInputEventListener(m_pathwayCanvas.PanEventHandler);
            m_pathwayCanvas.RemoveInputEventListener(m_pathwayCanvas.ZoomEventHandler);
            //m_pathwayCanvas.AddInputEventListener(new PPathwayZoomEventHandler(m_pathwayView));
            m_pathwayCanvas.AddInputEventListener(new DefaultMouseHandler(m_view));
            m_pathwayCanvas.Dock = DockStyle.Fill;
            //m_pathwayCanvas.MouseDown += new MouseEventHandler(m_pathwayCanvas_MouseDown);
            //m_pathwayCanvas.MouseMove += new MouseEventHandler(m_pathwayCanvas_MouseMove);
            m_pathwayCanvas.Name = name;
            //m_pathwayCanvas.Camera.Scale = DEFAULT_CAMERA_SCALE;
            m_pathwayCanvas.Camera.ScaleViewBy(0.7f);

            PScrollableControl scrolCtrl = new PScrollableControl(m_pathwayCanvas);
            scrolCtrl.Layout += new LayoutEventHandler(scrolCtrl_Layout);
            scrolCtrl.Dock = DockStyle.Fill;
            m_pathwayTabPage.Controls.Add(scrolCtrl);

            // Preparing overview
            m_area = new PDisplayedArea();
            m_overview = new OverView(overviewScale,
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

            ToolStripSeparator separator1 = new ToolStripSeparator();
            m_nodeMenu.Items.Add(separator1);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR1, separator1);

            int count = 0;
            foreach (ILayoutAlgorithm algorithm in m_view.Window.LayoutAlgorithm)
            {
                ToolStripMenuItem algoItem = new ToolStripMenuItem(algorithm.GetMenuText());
                algoItem.Tag = count;
                algoItem.ToolTipText = algorithm.GetToolTipText();
                algoItem.Click += new EventHandler(m_view.Window.eachLayoutItem_Click);

                List<string> subCommands = algorithm.GetSubCommands();
                if (subCommands != null && subCommands.Count != 0)
                {
                    int subcount = 0;
                    foreach (string subCommandName in subCommands)
                    {
                        ToolStripMenuItem layoutSubItem = new ToolStripMenuItem();
                        layoutSubItem.Text = subCommandName;
                        layoutSubItem.Tag = count + "," + subcount;
                        layoutSubItem.Click += new EventHandler(m_view.Window.eachLayoutItem_Click);
                        algoItem.DropDownItems.Add(layoutSubItem);
                        subcount++;
                    }
                }

                m_nodeMenu.Items.Add(algoItem);

                count++;
            }

            ToolStripSeparator separator2 = new ToolStripSeparator();
            separator2.Tag = count;
            m_nodeMenu.Items.Add(separator2);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR2, separator2);

            ToolStripItem rightArrow = new ToolStripMenuItem("Process -> Variable", Resource1.arrow_long_right_w);
            rightArrow.Name = CANVAS_MENU_RIGHT_ARROW;
            rightArrow.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(rightArrow);
            m_cMenuDict.Add(CANVAS_MENU_RIGHT_ARROW, rightArrow);

            ToolStripItem leftArrow = new ToolStripMenuItem("Process <- Variable", Resource1.arrow_long_left_w);
            leftArrow.Name = CANVAS_MENU_LEFT_ARROW;
            leftArrow.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(leftArrow);
            m_cMenuDict.Add(CANVAS_MENU_LEFT_ARROW, leftArrow);

            ToolStripItem bidirArrow = new ToolStripMenuItem("Process <-> Variable", Resource1.arrow_long_bidir_w);
            bidirArrow.Name = CANVAS_MENU_BIDIR_ARROW;
            bidirArrow.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(bidirArrow);
            m_cMenuDict.Add(CANVAS_MENU_BIDIR_ARROW, bidirArrow);

            ToolStripItem constant = new ToolStripMenuItem("Constant", Resource1.ten);
            constant.Name = CANVAS_MENU_CONSTANT_LINE;
            constant.Click += new EventHandler(ChangeLineClick);
            m_nodeMenu.Items.Add(constant);
            m_cMenuDict.Add(CANVAS_MENU_CONSTANT_LINE, constant);

            ToolStripSeparator separator3 = new ToolStripSeparator();
            m_nodeMenu.Items.Add(separator3);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR3, separator3);

            ToolStripItem cut = new ToolStripMenuItem(m_resources.GetString("CutMenuText"));
            cut.Click += new EventHandler(this.m_view.CutClick);
            m_nodeMenu.Items.Add(cut);
            m_cMenuDict.Add(CANVAS_MENU_CUT, cut);

            ToolStripItem copy = new ToolStripMenuItem(m_resources.GetString("CopyMenuText"));
            copy.Click += new EventHandler(this.m_view.CopyClick);
            m_nodeMenu.Items.Add(copy);
            m_cMenuDict.Add(CANVAS_MENU_COPY, copy);

            ToolStripItem paste = new ToolStripMenuItem(m_resources.GetString("PasteMenuText"));
            paste.Click += new EventHandler(this.m_view.PasteClick);
            m_nodeMenu.Items.Add(paste);
            m_cMenuDict.Add(CANVAS_MENU_PASTE, paste);

            ToolStripItem delete = new ToolStripMenuItem(m_resources.GetString("DeleteMenuText"));
            delete.Text = m_resources.GetString("DeleteMenuText");
            delete.Click += new EventHandler(this.m_view.DeleteClick);
            m_nodeMenu.Items.Add(delete);
            m_cMenuDict.Add(CANVAS_MENU_DELETE, delete);

            ToolStripItem deleteWith = new ToolStripMenuItem(m_resources.GetString("MergeMenuText"));
            deleteWith.Click += new EventHandler(MergeClick);
            m_nodeMenu.Items.Add(deleteWith);
            m_cMenuDict.Add(CANVAS_MENU_DELETE_WITH, deleteWith);

            ToolStripSeparator separator4 = new ToolStripSeparator();
            m_nodeMenu.Items.Add(separator4);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR4, separator4);

            // Create Logger
            ToolStripMenuItem createLogger = new ToolStripMenuItem(m_resources.GetString("CreateLogMenuText"));
            m_nodeMenu.Items.Add(createLogger);
            m_cMenuDict.Add(CANVAS_MENU_CREATE_LOGGER, createLogger);

            // Delete Logger
            ToolStripMenuItem deleteLogger = new ToolStripMenuItem(m_resources.GetString("DeleteLogMenuText"));
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
            m_resizeHandles[0].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNWSE);
            m_resizeHandles[0].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[0].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[0].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeNW);
            m_resizeHandles[0].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[1].Tag = ResizeHandleDragHandler.MovingRestriction.Vertical;
            m_resizeHandles[1].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNS);
            m_resizeHandles[1].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[1].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[1].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeN);
            m_resizeHandles[1].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[2].Tag = ResizeHandleDragHandler.MovingRestriction.NoRestriction;
            m_resizeHandles[2].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNESW);
            m_resizeHandles[2].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[2].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[2].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeNE);
            m_resizeHandles[2].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[3].Tag = ResizeHandleDragHandler.MovingRestriction.Horizontal;
            m_resizeHandles[3].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeWE);
            m_resizeHandles[3].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[3].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[3].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeE);
            m_resizeHandles[3].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[4].Tag = ResizeHandleDragHandler.MovingRestriction.NoRestriction;
            m_resizeHandles[4].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNWSE);
            m_resizeHandles[4].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[4].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[4].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeSE);
            m_resizeHandles[4].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[5].Tag = ResizeHandleDragHandler.MovingRestriction.Vertical;
            m_resizeHandles[5].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNS);
            m_resizeHandles[5].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[5].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[5].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeS);
            m_resizeHandles[5].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[6].Tag = ResizeHandleDragHandler.MovingRestriction.NoRestriction;
            m_resizeHandles[6].MouseEnter += new PInputEventHandler(PPathwaySystem_CursorSizeNESW);
            m_resizeHandles[6].MouseLeave += new PInputEventHandler(PPathwaySystem_MouseLeave);
            m_resizeHandles[6].MouseDown += new PInputEventHandler(PPathwaySystem_MouseDown);
            m_resizeHandles[6].MouseDrag += new PInputEventHandler(PPathwaySystem_ResizeSW);
            m_resizeHandles[6].MouseUp += new PInputEventHandler(PPathwaySystem_MouseUp);

            m_resizeHandles[7].Tag = ResizeHandleDragHandler.MovingRestriction.Horizontal;
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
            m_lineHandle4V.Tag = ElementType.Variable;
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
            m_lineHandle4P.Tag = ElementType.Process;
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
        }

        /// <summary>
        /// Called when the mouse is down on m_lineHandle.
        /// Start to reconnecting edge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_lineHandle_MouseDown(object sender, PInputEventArgs e)
        {
            m_reconnectNodeType = (ElementType)e.PickedNode.Tag;
            if (m_reconnectNodeType == ElementType.Process && null != m_ctrlLayer)
            {
                m_ctrlLayer.RemoveChild(m_lineHandle4P);
            }
            else if (m_reconnectNodeType == ElementType.Variable && null != m_ctrlLayer)
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

            m_selectedLine.DrawLine();
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
                EcellObject eo = m_nodesUnderMouse.Pop();
                if (eo is EcellProcess && m_reconnectNodeType == ElementType.Process)
                {
                    m_view.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_view.NotifyVariableReferenceChanged(eo.key, m_vOnLinesEnd, RefChangeType.BiDir, 0);
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
                        m_view.NotifyVariableReferenceChanged(
                            eo.key,
                            m_vOnLinesEnd,
                            RefChangeType.SingleDir,
                            coefficient);
                    }
                    ResetSelectedLine();
                }
                else if (eo is EcellVariable && m_reconnectNodeType == ElementType.Variable)
                {
                    m_view.NotifyVariableReferenceChanged(m_pOnLinesEnd, m_vOnLinesEnd, RefChangeType.Delete, 0);
                    if (m_selectedLine.Info.Direction == EdgeDirection.Bidirection)
                    {
                        m_view.NotifyVariableReferenceChanged(m_pOnLinesEnd, eo.key, RefChangeType.BiDir, 0);
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
                        m_view.NotifyVariableReferenceChanged(
                            m_pOnLinesEnd,
                            eo.key,
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

                m_view.NotifyVariableReferenceChanged(
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
            DialogResult result = MessageBox.Show(m_resources.GetString("ConfirmMerge"),
                "Merge",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Cancel)
                return;

            /* 20070629 delete by sachiboo. 
                        //PPathwayObject obj = (PPathwayObject)ClickedNode;
            */

            Object obj = ((ToolStripItem)sender).Tag;
            if (obj is PPathwaySystem)
            {
                PPathwaySystem deleteSystem = (PPathwaySystem)obj;
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

                try
                {
                    m_view.NotifyDataMerge(deleteSystem.EcellObject.key, ComponentType.System);
                }
                catch (IgnoreException)
                {
                    return;
                }
                if (deleteSystem.IsHighLighted)
                    ResetSelectedSystem();

            }
            ((ToolStripMenuItem)sender).Tag = null;
        }

        /// <summary>
        /// Called when a create logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateLoggerClick(object sender, EventArgs e)
        {
            string logger = ((ToolStripItem)sender).Text;

            if (ClickedNode is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)ClickedNode;
                EcellObject ecellobj = obj.EcellObject;
                Debug.WriteLine("Create " + obj.EcellObject.type + " Logger:" + obj.EcellObject.key);

                // set logger
                foreach (EcellData d in ecellobj.M_value)
                {
                    if (logger.Equals(d.M_name))
                    {
                        d.M_isLogger = true;
                        PluginManager.GetPluginManager().LoggerAdd(
                            ecellobj.modelID,
                            ecellobj.key,
                            ecellobj.type,
                            d.M_entityPath);
                    }
                }
                // modify changes
                m_view.NotifyDataChanged(ecellobj.key,ecellobj.key,ecellobj,true);
            }

        }

        /// <summary>
        /// Called when a delete logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteLoggerClick(object sender, EventArgs e)
        {
            string logger = ((ToolStripItem)sender).Text;

            if (ClickedNode is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)ClickedNode;
                EcellObject ecellobj = obj.EcellObject;
                Debug.WriteLine("Delete " + obj.EcellObject.type + " Logger:" + obj.EcellObject.key);

                // delete logger
                foreach (EcellData d in ecellobj.M_value)
                    if (logger.Equals(d.M_name))
                        d.M_isLogger = false;
                // modify changes
                m_view.NotifyDataChanged(ecellobj.key, ecellobj.key, ecellobj, true);
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
            if (ClickedNode is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)ClickedNode;
                MessageBox.Show("Name:" + obj.Name + "\nX:" + obj.X + "\nY:" + obj.Y
                    + "\nOffsetX:" + obj.OffsetX + "\nOffsetY:" + obj.OffsetY + "\nToString()"
                    + obj.ToString());
            }
            else
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                PointF point = new PointF();
                Debug.WriteLine(this.TabPage.Parent.ToString());
                point.X = item.Owner.Left;
                point.Y = item.Owner.Top;
                MessageBox.Show("Left:" + point.X + "Top:" + point.Y);

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
        void PPathwaySystem_MouseUpNew(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (m_selectedSystemName == null)
                return;
            // Get selected system
            PPathwaySystem system = m_systems[m_selectedSystemName];

            // If selected system overlaps another one, cancel change.
            if (this.DoesSystemOverlaps(system.GlobalBounds, system.EcellObject.key))
            {
                system.ReturnToMemorizedPosition();
                this.ValidateSystem(system);
                system.Reset();
                return;
            }

            // Change contained system

            // Change uncontained system

            // Check each node and change parent system

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
            PPathwaySystem system = m_systems[m_selectedSystemName];

            // If selected system overlaps another, reset system region.
            if (this.DoesSystemOverlaps(system.GlobalBounds, system.EcellObject.key))
            {
                system.ReturnToMemorizedPosition();
                this.ValidateSystem(system);
                system.Reset();
            }
            else
            {
                system.Reset();

                PNodeList currentSystemChildren = new PNodeList();
                List<PNode> tmpList = new List<PNode>();
                foreach (PLayer layer in this.Layers.Values)
                {
                    PNodeList nodeList = new PNodeList();
                    layer.FindIntersectingNodes(system.Rect, nodeList);
                    foreach (PNode p in nodeList)
                    {
                        if (!currentSystemChildren.Contains(p))
                            currentSystemChildren.Add(p);
                        if (p is PPathwaySystem && p != system)
                        {
                            PNodeList tmpNodeList = new PNodeList();
                            layer.FindIntersectingNodes(((PPathwaySystem)p).Rect, tmpNodeList);
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
                        currentDict.Add(((PPathwayNode)child).EcellObject.type + ":" + ((PPathwayNode)child).EcellObject.key, child);
                    }
                    else if (child is PPathwaySystem)
                    {
                        if (!m_selectedSystemName.Equals(((PPathwaySystem)child).EcellObject.key))
                            currentDict.Add(((PPathwaySystem)child).EcellObject.type + ":" + ((PPathwaySystem)child).EcellObject.key, child);
                    }
                }

                Dictionary<string, PNode> beforeDict = new Dictionary<string, PNode>();
                foreach (PNode child in m_systemChildrenBeforeDrag)
                {
                    string key = "";
                    if (child is PPathwayNode)
                    {
                        PPathwayNode childNode = ((PPathwayNode)child);
                        key = childNode.EcellObject.key;
                        beforeDict.Add(childNode.EcellObject.type + ":" + key, child);
                    }
                    else if (child is PPathwaySystem)
                    {
                        key = ((PPathwaySystem)child).EcellObject.key;
                        beforeDict.Add(((PPathwaySystem)child).EcellObject.type + ":" + key, child);
                    }
                }

                // If ID duplication could occurred, system resizing will be aborted
                Dictionary<string, PPathwayNode> proNameDict = new Dictionary<string, PPathwayNode>();
                Dictionary<string, PPathwayNode> varNameDict = new Dictionary<string, PPathwayNode>();
                foreach (KeyValuePair<string, PNode> node in currentDict)
                {
                    string name = PathUtil.RemovePath(node.Key);

                    if (node.Value is PPathwayVariable)
                    {
                        if (varNameDict.ContainsKey(name))
                        {
                            // Resizing is aborted
                            system.ReturnToMemorizedPosition();
                            this.ValidateSystem(system);
                            system.RefreshText();
                            system.Reset();

                            UpdateResizeHandlePositions();
                            ResetSelectedObjects();
                            ClearSurroundState();
                            MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            varNameDict.Add(name, null);
                        }
                    }
                    else if (node.Value is PPathwayProcess)
                    {
                        if (proNameDict.ContainsKey(name))
                        {
                            // Resizing is aborted
                            system.ReturnToMemorizedPosition();
                            this.ValidateSystem(system);
                            system.RefreshText();
                            system.Reset();

                            UpdateResizeHandlePositions();
                            ResetSelectedObjects();
                            ClearSurroundState();
                            MessageBox.Show(m_resources.GetString("ErrSameObj"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            proNameDict.Add(name, null);
                        }
                    }
                }

                foreach (string key in beforeDict.Keys)
                {
                    if (!currentDict.ContainsKey(key))
                    {
                        bool isDuplicate = false;
                        String[] sp = key.Split(new char[] { ':' });
                        if (key.StartsWith("System"))
                        {
                            String name = sp[1];
                            sp = sp[1].Split(new char[] { '/' });
                            PPathwayObject p = system.ParentObject;
                            if (p == null)
                                continue;
                            String newkey = ((PPathwaySystem)p).EcellObject.key + "/" + sp[sp.Length - 1];
                            if (m_view.HasObject(ComponentType.System, newkey))
                                isDuplicate = true;

                        }
                        else
                        {
                            String data = sp[1] + ":" + sp[2];
                            PPathwayObject p = system.ParentObject;
                            if (p == null)
                                continue;
                            String newkey = ((PPathwaySystem)p).EcellObject.key + ":" + sp[2];
                            ComponentType ct = ComponentSetting.ParseComponentKind(sp[0]);
                            if (m_view.HasObject(ct, newkey))
                                isDuplicate = true;
                        }

                        if (isDuplicate)
                        {
                            // Resizing is aborted
                            system.ReturnToMemorizedPosition();
                            this.ValidateSystem(system);
                            system.RefreshText();
                            system.Reset();

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

                    String[] sp = key.Split(new char[] { ':' });
                    if (key.StartsWith("System"))
                    {
                        String prevkey = sp[1];
                        sp = sp[1].Split(new char[] { '/' });
                        PPathwayObject p = system.ParentObject;
                        String newkey = m_selectedSystemName + "/" + sp[sp.Length - 1];
                        EcellObject obj = Systems[prevkey].EcellObject;
                        obj.key = newkey;

                        String tmp = m_selectedSystemName;
                        this.TransferSystemTo(m_selectedSystemName, prevkey, true);
                        m_selectedSystemName = tmp;
                    }
                    else
                    {
                        string prevkey = sp[1] + ":" + sp[2];
                        PPathwayObject obj;
                        if (sp[0].Equals("Process"))
                            obj = Processes[prevkey];
                        else
                            obj = Variables[prevkey];

                        if (obj == null) continue;
                        this.TransferNodeToByResize(m_selectedSystemName, obj, false);
                    }
                }

                foreach (string key in beforeDict.Keys)
                {
                    if (currentDict.ContainsKey(key)) continue;

                    if (m_selectedSystemName.Equals("/"))
                    {
                        system.ReturnToMemorizedPosition();
                        this.ValidateSystem(system);
                        system.RefreshText();
                        system.Reset();
                        UpdateResizeHandlePositions();
                        ResetSelectedObjects();
                        ClearSurroundState();

                        return;
                    }

                    String[] sp = key.Split(new char[] { ':' });
                    if (key.StartsWith("System"))
                    {
                        string prevkey = sp[1];
                        sp = sp[1].Split(new char[] { '/' });
                        PPathwayObject p = system.ParentObject;
                        EcellObject obj = Systems[prevkey].EcellObject;

                        String tmp = m_selectedSystemName;
                        this.TransferSystemTo(p.EcellObject.key, prevkey, true);
                        m_selectedSystemName = tmp;
                    }
                    else
                    {
                        String prevkey = sp[1] + ":" + sp[2];
                        PPathwayObject p = system.ParentObject;

                        PPathwayObject obj;
                        if (sp[0].Equals("Process"))
                            obj = Processes[prevkey];
                        else
                            obj = Variables[prevkey];

                        if (obj == null) continue;
                        this.TransferNodeToByResize(p.EcellObject.key, obj, false);
                    }
                }
                // Fire DataChanged for child in system.!
                m_systemChildrenBeforeDrag = null;
            }
            m_view.NotifyDataChanged(
                system.EcellObject.key,
                system.EcellObject.key,
                system.EcellObject,
                true);

            system.RefreshText();

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
            system.MemorizeCurrentPosition();
            PointF offsetToL = system.Offset;
            PointF lP = new PointF(system.X + offsetToL.X, system.Y + offsetToL.Y);
            m_upperLeftPoint = lP;
            m_upperRightPoint = new PointF(m_upperLeftPoint.X + system.Width, m_upperLeftPoint.Y);
            m_lowerRightPoint = new PointF(m_upperLeftPoint.X + system.Width,
                                            m_upperLeftPoint.Y + system.Height);
            m_lowerLeftPoint = new PointF(m_upperLeftPoint.X, m_upperLeftPoint.Y + system.Height);

            m_systemChildrenBeforeDrag = new PNodeList();
            m_systemChildrenBeforeDrag.AddRange(system.ChildrenReference);
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
            EcellObject eo = system.EcellObject;

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = m_lowerRightPoint.X - X;
            float height = m_lowerRightPoint.Y - Y;
            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                eo.X = X;
                eo.Y = Y;
                eo.Width = width;
                eo.Height = height;
                system.RefreshText();
                PointF offsetToL = system.Offset;

                system.X = eo.X - offsetToL.X;
                system.Y = eo.Y - offsetToL.Y;
                system.Width = eo.Width;
                system.Height = eo.Height;
                this.ValidateSystem(system);
                system.Reset();
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
            EcellObject eo = system.EcellObject;

            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float height = m_lowerRightPoint.Y - Y;

            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                eo.Y = Y;
                eo.Height = height;
                system.RefreshText();
                PointF offsetToL = system.Offset;
                system.Y = eo.Y - offsetToL.Y;
                system.Height = eo.Height;
                this.ValidateSystem(system);
                system.Reset();
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
            EcellObject eo = system.EcellObject;

            float Y = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                               - eo.X - system.Offset.X;
            float height = m_lowerLeftPoint.Y - Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                eo.Y = Y;
                eo.Width = width;
                eo.Height = height;
                system.RefreshText();
                PointF offsetToL = system.Offset;
                system.Y = eo.Y - offsetToL.Y;
                system.Width = eo.Width;
                system.Height = eo.Height;
                this.ValidateSystem(system);
                system.Reset();
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
            EcellObject eo = system.EcellObject;

            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                              - eo.X - system.Offset.X;
            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                eo.Width = width;
                system.RefreshText();
                system.Width = eo.Width;
                this.ValidateSystem(system);
                system.Reset();
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
            EcellObject eo = system.EcellObject;

            float width = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                               - eo.X - system.Offset.X;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                                - eo.Y - system.Offset.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                eo.Width = width;
                eo.Height = height;
                system.RefreshText();
                PointF offsetToL = system.Offset;
                system.Width = eo.Width;
                system.Height = eo.Height;
                this.ValidateSystem(system);
                system.Reset();
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
            EcellObject eo = system.EcellObject;

            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                                 - eo.Y - system.Offset.Y;

            if (height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                eo.Height = height;
                system.RefreshText();

                system.Height = eo.Height;
                this.ValidateSystem(system);
                system.Reset();
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
            EcellObject eo = system.EcellObject;

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = m_upperRightPoint.X - e.PickedNode.X - e.PickedNode.OffsetX - m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS;
            float height = e.PickedNode.Y + e.PickedNode.OffsetY + m_resizeHandleHalfWidth + PPathwaySystem.HALF_THICKNESS
                               - eo.Y - system.Offset.Y;

            if (width > PPathwaySystem.MIN_X_LENGTH && height > PPathwaySystem.MIN_Y_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                eo.X = X;
                eo.Width = width;
                eo.Height = height;
                system.RefreshText();
                PointF offsetToL = system.Offset;
                system.X = eo.X - offsetToL.X;
                system.Width = eo.Width;
                system.Height = eo.Height;
                this.ValidateSystem(system);
                system.Reset();

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
            EcellObject sysEle = system.EcellObject;

            float X = e.PickedNode.X + e.PickedNode.OffsetX + m_resizeHandleHalfWidth - PPathwaySystem.HALF_THICKNESS;
            float width = m_lowerRightPoint.X - X;

            if (width > PPathwaySystem.MIN_X_LENGTH)
            {
                ((ResizeHandle)e.PickedNode).FreeMoveRestriction();
                sysEle.X = X;
                sysEle.Width = width;
                system.RefreshText();
                PointF offsetToL = system.Offset;
                system.X = sysEle.X - offsetToL.X;
                system.Width = sysEle.Width;
                this.ValidateSystem(system);
                system.Reset();
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
            if (this.DoesSystemOverlaps(system.Rect))
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
        /// </summary>
        public void NotifySelectChanged(string key, string type)
        {
            if (m_view != null)
                m_view.NotifySelectChanged(key, type);
        }

        /// <summary>
        /// Notify this canvas that the mouse is on it.
        /// </summary>
        /// <param name="element">mouse is on this node</param>
        public void NotifyMouseEnter(EcellObject eo)
        {
            if (m_isReconnectMode)
                m_nodesUnderMouse.Push(eo);
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
        /// Transfer selected objects from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName"></param>
        public void TransferSelectedTo(string systemName)
        {
            foreach (EcellObject node in SelectedNodes)
            {
                TransferNodeTo(systemName, node, true, true);
            }
            ResetSelectedObjects();
        }

        /// <summary>
        /// Transfer an object from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName">The name of the system to which object is transfered. If null, obj is
        /// transfered to layer itself</param>
        /// <param name="obj">transfered object</param>
        /// <param name="toBeNotified">Whether this needs to be notified or not</param>
        /// <param name="isAnchor">Whether this action is an anchor or not</param>
        public void TransferNodeTo(string systemName, EcellObject eo, bool toBeNotified, bool isAnchor)
        {
            // Set new system.
            string oldKey = eo.key;
            eo.parentSystemID = systemName;

            if (eo is EcellVariable)
            {
                PPathwayVariable var = Variables[oldKey];
                m_variables.Remove(oldKey);
                m_variables.Add(eo.key, var);
            }
            else if (eo is EcellProcess)
            {
                PPathwayProcess pro = Processes[oldKey];
                m_processes.Remove(oldKey);
                m_processes.Add(eo.key, pro);
            }
            if (toBeNotified)
            {
                m_view.NotifyDataChanged(
                    oldKey,
                    eo.key,
                    eo,
                    isAnchor);
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
                m_variables.Remove(var.EcellObject.key);
                string newKey = systemName + ":" + PathUtil.RemovePath(var.EcellObject.key);
                string oldKey = var.EcellObject.key;
                var.EcellObject.key = newKey;
                m_variables.Add(var.EcellObject.key, var);
                var.Refresh();
                if (!oldKey.Equals(newKey))
                    m_view.NotifyDataChanged(
                        oldKey,
                        newKey,
                        obj.EcellObject,
                        isAnchor);
            }
            else if (obj is PPathwayProcess)
            {
                PPathwayProcess pro = (PPathwayProcess)obj;
                m_processes.Remove(pro.EcellObject.key);
                string newKey = systemName + ":" + PathUtil.RemovePath(pro.EcellObject.key);
                string oldKey = pro.EcellObject.key;
                pro.EcellObject.key = newKey;
                m_processes.Add(pro.EcellObject.key, pro);
                pro.Refresh();
                if (!oldKey.Equals(newKey))
                    m_view.NotifyDataChanged(
                        oldKey,
                        newKey,
                        obj.EcellObject,
                        isAnchor);
            }
        }

        /// <summary>
        /// Transfer an system from one PEcellSystem/Layer to PEcellSystem/Layer.
        /// </summary>
        /// <param name="systemName">The name of the system to which a system will be transfered. If null, obj is
        /// transfered to layer itself</param>
        /// <param name="oldKey">old key of a system to be transfered</param>
        /// <param name="isExternal">Whether the change will go outside</param>
        public void TransferSystemTo(string systemName, string oldKey, bool isExternal)
        {
            if (String.IsNullOrEmpty(systemName))
                return;
            string newKey;
            ResetSelectedObjects();
            if (systemName.Equals("/"))
                newKey = systemName + PathUtil.RemovePath(oldKey);
            else
                newKey = systemName + "/" + PathUtil.RemovePath(oldKey);

            PPathwaySystem system = m_systems[oldKey];
            PPathwaySystem parentSys = m_systems[systemName];
            system.Parent.RemoveChild(system);
            parentSys.AddChild(system);

            if (isExternal)
            {
                m_view.NotifyDataChanged(
                    oldKey,
                    newKey,
                    system.EcellObject,
                    true);
            }

            system.EcellObject.key = newKey;
            system.Name = system.Name.Replace(oldKey, newKey);

            m_systems.Add(newKey, system);
            m_systems.Remove(oldKey);

            foreach (string key in this.GetAllSystemUnder(oldKey))
            {
                PPathwaySystem childSys = m_systems[key];
                m_systems.Remove(key);
                string newSysKey = key.Replace(oldKey, newKey);
                childSys.Name = newSysKey;
                childSys.EcellObject.key = newSysKey;
                m_systems.Add(newSysKey, childSys);
            }

            foreach (string key in this.GetAllVariableUnder(oldKey))
            {
                PPathwayVariable pev = m_variables[key];
                m_variables.Remove(key);
                string newVarKey = key.Replace(oldKey, newKey);
                pev.Name = newVarKey;
                m_variables.Add(newVarKey, pev);
            }

            foreach (string key in this.GetAllProcessUnder(oldKey))
            {
                PPathwayProcess pep = m_processes[key];
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
        public bool DoesSystemOverlaps(RectangleF rect, string excludeName)
        {
            bool isOverlaping = false;
            foreach (PPathwaySystem system in m_systems.Values)
                if (system.Overlaps(rect) && !system.EcellObject.key.Equals(excludeName))
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

            if (systemName == null || systemName.Equals("") )
            {
                obj.Layer = Layers[layer];
                Layers[layer].AddChild(obj);
            }
            else
            {
                PPathwaySystem system = m_systems[systemName];

                // If obj hasn't coordinate, it will be settled. 
                if (!hasCoords)
                {
                    if (obj is PPathwayNode)
                    {
                        // Get a vacant point where an incoming node should be placed.
                        PointF vacantPoint = GetVacantPoint(systemName);
                        obj.X = vacantPoint.X - obj.Width;
                        obj.Y = vacantPoint.Y - obj.Height;
                    }
                    else if(obj is PPathwaySystem)
                    {
                        float maxX = 0f;
                        float maxY = 0f;
                        float x = 0f;
                        float y = 0f;

                        foreach (PPathwayObject ppo in system.ChildObjectList)
                        {
                            if (ppo is PPathwayObject)
                            {
                                x = ppo.X + ppo.OffsetX + ppo.Width;
                                y = ppo.Y + ppo.OffsetY + ppo.Height;

                            }
                            if (maxX < x)
                                maxX = x;
                            if (maxY < y)
                                maxY = y;
                        }
                        // Set obj's coordinate
                        obj.X = system.X + system.Offset.X + maxX + PPathwaySystem.SYSTEM_MARGIN;
                        obj.Y = system.Y + system.Offset.Y + PPathwaySystem.SYSTEM_MARGIN;
                        obj.Width = PPathwaySystem.DEFAULT_WIDTH;
                        obj.Height = PPathwaySystem.DEFAULT_HEIGHT;

                        system.MakeSpace(obj);
                    }
                }

                // Set obj to appropriate layer.
                if (system.Layer == Layers[layer])
                {
                    obj.Layer = Layers[layer];
                    obj.RefreshText();

                    system.AddChild(obj);
                    obj.ParentObject = system;
                }
                if (obj is PPathwayProcess)
                    ((PPathwayProcess)obj).CreateEdges();
            }
            //this.m_view.NotifyDataChanged(obj.EcellObject.key, obj.EcellObject.key, obj, true);

        }

        /// <summary>
        /// add child object to the selected layer.
        /// </summary>
        /// <param name="layer">selected layer.</param>
        /// <param name="obj">add object.</param>
        public void AddChildToSelectedLayer(string layer, PPathwayObject obj)
        {
            RegisterObjToSet(obj);
            obj.Layer = Layers[layer];
            Layers[layer].AddChild(obj);
        }

        /// <summary>
        /// add the child object to the selected layer.
        /// </summary>
        /// <param name="layer">the selected layer.</param>
        /// <param name="systemName">system name of the added object.</param>
        /// <param name="obj">the added object.</param>
        /// <value
        public void AddChildToSelectedLayer(string layer, string systemName, PPathwayObject obj)
        {
            RegisterObjToSet(obj);
            if (systemName == null)
            {
                obj.Layer = Layers[layer];
                Layers[layer].AddChild(obj);
            }
            else
            {
                foreach (PPathwaySystem system in m_systems.Values)
                {
                    if (system.Layer == Layers[layer])
                    {
                        obj.Layer = Layers[layer];
                        system.AddChild(obj);
                        if (obj is PPathwayNode)
                            obj.ParentObject = system;
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
            if (obj.Layer == null)
            {
                PPathwaySystem system = m_systems[systemName];
                obj.Layer = system.Layer;
                if (!isCoordSet)
                {
                    obj.X = system.X + ((system.Width > 80) ? 80 : system.Width / 2) - obj.Width;
                    obj.Y = system.Y + ((system.Height > 80) ? 80 : system.Height / 2) - obj.Height;
                    if (obj is PPathwayNode)
                    {
                        obj.EcellObject.X = obj.X;
                        obj.EcellObject.Y = obj.Y;
                        ((PPathwayNode)obj).RefreshText();
                    }
                }

                system.AddChild(obj);
                if (obj is PPathwayNode)
                    ((PPathwayNode)obj).ParentObject = system;
            }
            else
            {
                foreach (PPathwaySystem system in m_systems.Values)
                {
                    if (system.Layer == obj.Layer)
                    {
                        if (!isCoordSet)
                        {
                            obj.X = system.X + ((system.Width > 80) ? 80 : system.Width / 2) - obj.Width;
                            obj.Y = system.Y + ((system.Height > 80) ? 80 : system.Height / 2) - obj.Height;
                            obj.EcellObject.X = obj.X;
                            obj.EcellObject.Y = obj.Y;
                            if (obj is PPathwayNode)
                                ((PPathwayNode)obj).RefreshText();

                        }
                        PointF offsetToL = system.Offset;
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
            foreach (DataRow row in m_table.Rows)
            {
                if ((bool)row[COLUMN_NAME4SHOW] == true)
                {
                    Layers[(string)row[COLUMN_NAME4NAME]].AddChild(obj);
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

            m_pathwayCanvas.Root.AddChild(0, layer);
            m_pathwayCanvas.Camera.AddLayer(0, layer);

            DataRow dr = m_table.NewRow();
            dr[COLUMN_NAME4SHOW] = true;
            dr[COLUMN_NAME4NAME] = name;
            m_table.Rows.Add(dr);

            m_overview.AddObservedLayer(layer);

            Layers.Add(name, layer);
            m_ctrlLayer.MoveToFront();

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
            RectangleF rootRect = m_systems["/"].Rect;
            if (rootRect.IntersectsWith(rectF) || rootRect.Contains(rectF))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Get a temporary key of EcellObject.
        /// </summary>
        /// <param name="type">The data type of EcellObject.</param>
        /// <param name="key">The ID of parent system.</param>
        /// <returns>"TemporaryID"</returns> 
        public string GetTemporaryID(string type, string systemID)
        {
            return this.PathwayView.Window.GetTemporaryID(type, systemID);
        }
        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">PointF</param>
        /// <returns>EcellObject</returns>
        public EcellObject GetSurroundingSystem(PointF point)
        {
            return GetSurroundingSystem(point, null);
        }
        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <param name="excludedSystem">If this parameter is set, this system is excluded from searching</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public EcellObject GetSurroundingSystem(PointF point, string excludedSystem)
        {
            EcellObject obj = this.Systems["/"].EcellObject;

            foreach (PPathwaySystem sys in this.Systems.Values)
                if (sys.Rect.Contains(point) && !sys.EcellObject.key.Equals(excludedSystem))
                    obj = sys.EcellObject;
            return obj;
        }

        /// <summary>
        /// Return a system which surrounds a given point.
        /// If more than one system surround a given point, smallest system will be returned.
        /// </summary>
        /// <param name="point">A system surrounds this point will be returned.</param>
        /// <param name="excludedSystem">If this parameter is set, this system is excluded from searching</param>
        /// <returns>Surrounding system name. Null will be returned if there is no surround system.</returns>
        public string GetSurroundingSystemKey(PointF point)
        {
            return GetSurroundingSystem(point).key;
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
            return GetSurroundingSystem(point).key;
        }

        /// <summary>
        /// Notify this set that one PPathwayNode is selected
        /// </summary>
        /// <param name="obj">Newly selected object</param>
        /// <param name="toBeNotified">Whether selection must be notified to Ecell-Core or not.</param>
        public void AddSelectedNode(PPathwayNode obj, bool toBeNotified)
        {
            SelectedNodes.Add(obj.EcellObject);
            foreach (EcellObject eo in SelectedNodes)
            {
                if (toBeNotified)
                    m_view.NotifySelectChanged(eo.key, eo.type);
            }
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
            m_view.NotifySelectChanged(systemName, type);
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

            line.DrawLine();
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
                if (null != m_line4reconnect.Parent)
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
        public List<string> GetAllSystemUnder(string systemKey)
        {
            List<string> returnList = new List<string>();
            foreach (string key in m_systems.Keys)
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
            foreach (string key in m_variables.Keys)
            {
                if (key.StartsWith(systemKey) && !key.Equals(systemKey))
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
            foreach (string key in m_processes.Keys)
            {
                if (key.StartsWith(systemKey) && !key.Equals(systemKey))
                    returnList.Add(key);
            }
            return returnList;
        }

        /// <summary>
        /// Get all EcellObjects of this object.
        /// </summary>
        /// <param name="systemKey"></param>
        /// <param name="systemKey"></param>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public PPathwayObject GetSelectedObject(string key, string type)
        {
            if (type.Equals(EcellObject.SYSTEM))
                return Systems[key];
            if (type.Equals(EcellObject.PROCESS))
                return Processes[key];
            if (type.Equals(EcellObject.VARIABLE))
                return Variables[key];
            return null;
        }
        /// <summary>
        /// Get all EcellObjects of this object.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<EcellObject> GetAllObjects()
        {
            List<EcellObject> returnList = new List<EcellObject>();
            returnList.AddRange(GetSystemList());
            returnList.AddRange(GetNodeList());

            return returnList;
        }
        /// <summary>
        /// Get all EcellSystems of this object.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<EcellObject> GetSystemList()
        {
            List<EcellObject> returnList = new List<EcellObject>();
            foreach (PPathwaySystem system in this.Systems.Values)
                returnList.Add(system.EcellObject);

            return returnList;
        }
        /// <summary>
        /// Get all EcellProcesss of this object.
        /// </summary>
        /// <returns>A list which contains all PathwayElements of this object</returns>
        public List<EcellObject> GetNodeList()
        {
            List<EcellObject> returnList = new List<EcellObject>();
            foreach (PPathwayVariable variable in this.Variables.Values)
                returnList.Add(variable.EcellObject);
            foreach (PPathwayProcess process in this.Processes.Values)
                returnList.Add(process.EcellObject);

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
                m_pathwayCanvas.Camera.TranslateViewBy(0, delta);
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

            switch (type)
            {
                case ComponentType.System:
                    if (!m_systems.ContainsKey(key))
                        return;
                    PPathwaySystem system = m_systems[key];
                    //m_systems.Remove(key);

                    if (!key.Equals(data.key))
                    {
                        TransferSystemTo( data.parentSystemID, key, false);
                        m_systems.Remove(key);
                        m_systems.Add(data.key, system);

                    }

                    system.EcellObject = (EcellSystem)data;
                    system.Name = data.key;
                    system.Width = data.Width;
                    system.Height = data.Height;
                    system.Reset();

                    UpdateResizeHandlePositions();
                    break;
                case ComponentType.Variable:
                    if (!m_variables.ContainsKey(key))
                        return;
                    PPathwayVariable var = m_variables[key];
                    var.EcellObject = (EcellVariable)data;
                    var.Name = PathUtil.RemovePath(data.key);
                    var.Refresh();

                    m_variables.Remove(key);
                    m_variables.Add(data.key, var);
                    break;
                case ComponentType.Process:
                    if (!m_processes.ContainsKey(key))
                        return;
                    PPathwayProcess pro = m_processes[key];
                    pro.EcellObject = (EcellProcess)data;
                    pro.RefreshEdges();

                    // switch keys of dictionary
                    m_processes.Remove(key);
                    pro.Name = PathUtil.RemovePath(data.key);
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
                    if (!m_systems.ContainsKey(key))
                        break;

                    PPathwaySystem system = m_systems[key];
                    system.Parent.RemoveChild(system);
                    int ind = m_ctrlLayer.IndexOfChild(system);
                    m_ctrlLayer.RemoveChild(ind);

                    if (m_selectedSystemName != null && key.Equals(m_selectedSystemName))
                    {
                        HideResizeHandles();
                        m_selectedSystemName = null;
                    }
                    m_systems.Remove(key);

                    break;
                case ComponentType.Variable:
                    if (!m_variables.ContainsKey(key))
                        break;
                    PPathwayVariable v = m_variables[key];
                    v.NotifyRemoveRelatedVariable();
                    v.Parent.RemoveChild(v);
                    v.Reset();
                    m_variables.Remove(key);
                    break;
                case ComponentType.Process:
                    if (!m_processes.ContainsKey(key))
                        break;
                    PPathwayProcess p = m_processes[key];
                    p.NotifyRemoveRelatedProcess();
                    p.DeleteEdges();
                    p.Parent.RemoveChild(p);
                    p.Reset();
                    m_processes.Remove(key);
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
                        m_pathwayCanvas.Camera.AnimateViewToCenterBounds(focusSystem.FullBounds,
                                                                             true,
                                                                             CAMERA_ANIM_DURATION);

                        UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
                    }
                    break;
                case ComponentType.Variable:
                    bool isAlreadySelected = false;
                    foreach (EcellObject selectNode in SelectedNodes)
                    {
                        if (key.Equals(selectNode.key) && selectNode is EcellVariable)
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
                            m_pathwayCanvas.Camera.AnimateViewToCenterBounds(PathUtil.GetFocusBound(focusNode.FullBounds, LEAST_FOCUS_SIZE),
                                                                             true,
                                                                             CAMERA_ANIM_DURATION);
                            UpdateOverviewAfterTime(CAMERA_ANIM_DURATION + 150);
                        }
                    }
                    break;
                case ComponentType.Process:
                    bool isProAlreadySelected = false;
                    foreach (EcellObject selectNode in SelectedNodes)
                    {
                        if (key.Equals(selectNode.key) && selectNode is EcellProcess)
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
                        m_pathwayCanvas.Camera.AnimateViewToCenterBounds(PathUtil.GetFocusBound(focusNode.FullBounds, LEAST_FOCUS_SIZE),
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
            RectangleF localF = m_pathwayCanvas.Camera.Bounds;
            RectangleF recf = m_pathwayCanvas.Camera.ViewBounds;
            PMatrix matrix = m_pathwayCanvas.Camera.ViewMatrix;
            m_area.Reset();
            m_area.Offset = ZERO_POINT;
            m_area.AddRectangle(recf.X, recf.Y, recf.Width, recf.Height);
            m_overview.UpdateTransparent();
            m_overview.Canvas.Refresh();
        }

        /// <summary>
        /// Zoom in/out this canvas.
        /// </summary>
        /// <param name="rate"></param>
        public void Zoom(float rate)
        {
            float currentScale = this.PathwayCanvas.Camera.ViewScale;
            float newScale = currentScale * rate;

            if (newScale < PPathwayZoomEventHandler.MIN_SCALE)
            {
                rate = PPathwayZoomEventHandler.MIN_SCALE / currentScale;
            }

            if (PPathwayZoomEventHandler.MAX_SCALE < newScale)
            {
                rate = PPathwayZoomEventHandler.MAX_SCALE / currentScale;
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

            if (m_pathwayCanvas != null)
                m_pathwayCanvas.Dispose();

            if (m_overview != null)
                m_overview.Dispose();
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
            if (SelectedNodes.Count == 0)
            {
                return;
            }
            PNodeList spareList = new PNodeList();
            foreach (EcellObject obj in SelectedNodes)
                GetSelectedObject(obj.key, obj.type).IsHighLighted = false;

            lock (this)
            {
                SelectedNodes.Clear();
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
            m_selectedLine.DrawLine();
        }

        /// <summary>
        /// Reset selected line
        /// </summary>
        public void ResetSelectedLine()
        {
            m_isReconnectMode = false;

            m_nodesUnderMouse.Clear();

            if (null != m_selectedLine)
            {
                m_selectedLine.Visible = true;
            }

            m_selectedLine = null;
            m_vOnLinesEnd = null;
            m_pOnLinesEnd = null;

            if (null != m_lineHandle4V && null != m_lineHandle4V.Parent)
            {
                m_ctrlLayer.RemoveChild(m_lineHandle4V);
            }
            if (null != m_lineHandle4P && null != m_lineHandle4P.Parent)
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
            m_pathwayCanvas.Refresh();
        }

        /// <summary>
        /// Return true if EcellSystem contains a point.
        /// </summary>
        /// <param name="sysKey">string</param>
        /// <param name="point">PointF</param>
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
            foreach (PPathwaySystem sys in this.Systems.Values)
                if (sys.EcellObject.key.Equals(sysKey) && sys.Rect.Contains(point) && sys.Rect.Contains(center))
                    sysContains = true;
                else if (sys.EcellObject.key.StartsWith(sysKey) && sys.Rect.Contains(point) && sys.Rect.Contains(center))
                    childContains = true;
            return sysContains && !childContains;
        }

        /// <summary>
        /// Return nearest vacant point of EcellSystem.
        /// </summary>
        /// <param name="sys">EcellObject of parent system.</param>
        /// <param name="obj">EcellObject of moved node</param>
        public PointF GetVacantPoint(string sysKey)
        {
            EcellObject sys = Systems[sysKey].EcellObject;
            Random hRandom = new Random();
            PointF basePos = new PointF(sys.X + (float)hRandom.Next((int)sys.Width), sys.Y + (float)hRandom.Next((int)sys.Height));
            return GetVacantPoint(sysKey, basePos);
        }

        /// <summary>
        /// Return nearest vacant point of EcellSystem.
        /// </summary>
        /// <param name="sys">EcellObject of parent system.</param>
        /// <param name="obj">EcellObject of moved node</param>
        public PointF GetVacantPoint(string sysKey, PointF pos)
        {
            PPathwaySystem sys = Systems[sysKey];
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
    #region Enums
    /// <summary>
    /// Type of element.
    /// In constructor of subclasses, one of this type will be set to m_elementType.
    /// </summary>
    public enum ElementType
    {
        /// <summary>
        /// type of pathway canvas
        /// </summary>
        Canvas,
        /// <summary>
        /// type of layer
        /// </summary>
        Layer,
        /// <summary>
        /// type of system
        /// </summary>
        System,
        /// <summary>
        /// type of variable
        /// </summary>
        Variable,
        /// <summary>
        /// type of process
        /// </summary>
        Process,
        /// <summary>
        /// type of attribute
        /// </summary>
        Attribute
    };
    #endregion
}