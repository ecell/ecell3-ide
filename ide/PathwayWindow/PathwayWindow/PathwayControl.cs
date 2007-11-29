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
using System.IO;
using System.ComponentModel;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// PathwayView plays a role of View part of MVC-model.
    /// </summary>
    public class PathwayControl
    {
        #region Static readonly fields
        #endregion

        #region Canvas Menu
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
        /// PathwayWindow.
        /// </summary>
        private PathwayWindow m_window;

        /// <summary>
        /// Dictionary for Eventhandlers.
        /// </summary>
        private Dictionary<int, PBasicInputEventHandler> m_handlerDict = new Dictionary<int, PBasicInputEventHandler>();

        /// <summary>
        /// A list of toolbox buttons.
        /// </summary>
        private List<ToolStripButton> m_buttonList = new List<ToolStripButton>();

        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        private Dictionary<string, ToolStripItem> m_cMenuDict = new Dictionary<string, ToolStripItem>();

        /// <summary>
        /// Dictionary for canvases.
        ///  key: canvas ID
        ///  value: CanvasViewComponentSet
        /// </summary>
        private Dictionary<string, CanvasControl> m_canvasDict;

        /// <summary>
        /// The CanvasID of currently active canvas.
        /// </summary>
        private string m_activeCanvasID;

        /// <summary>
        /// ComponentSettingsManager for creating Systems and Nodes
        /// </summary>
        private ComponentManager m_csManager;

        /// <summary>
        /// OverView interface.
        /// </summary>
        private PathwayView m_pathwayView;
        /// <summary>
        /// OverView interface.
        /// </summary>
        private OverView m_overView;
        /// <summary>
        /// LayerView interface.
        /// </summary>
        private LayerView m_layerView;

        /// <summary>
        /// ContextMenuStrip for PPathwayNode
        /// </summary>
        private ContextMenuStrip m_nodeMenu;

        /// <summary>
        /// List of PPathwayNode for copied object.
        /// </summary>
        private List<EcellObject> m_copiedNodes = new List<EcellObject>();

        /// <summary>
        /// Point of mouse cursor.
        /// </summary>
        private PointF m_mousePos;

        /// <summary>
        /// Point of mouse cursor.
        /// </summary>
        private PointF m_copyPos;

        /// <summary>
        /// Whether each node is showing it's ID or not;
        /// </summary>
        private bool m_showingId = true;

        /// <summary>
        /// Indicate which pathway-related toolbar button is selected.
        /// </summary>
        private Handle m_selectedHandle;

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
        public Dictionary<string, CanvasControl> CanvasDictionary
        {
            get { return m_canvasDict; }
        }

        /// <summary>
        /// Accessor for currently active canvas.
        /// </summary>
        public CanvasControl ActiveCanvas
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
        /// Accessor for m_overviewCanvas.
        /// </summary>
        public PathwayView PathwayView
        {
            get { return m_pathwayView; }
        }
        /// <summary>
        /// Accessor for m_overView.
        /// </summary>
        public OverView OverView
        {
            get { return m_overView; }
        }
        /// <summary>
        /// Accessor for m_layerView.
        /// </summary>
        public LayerView LayerView
        {
            get { return m_layerView; }
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
        /// get/set the CoponentSettingManager.
        /// </summary>
        public ComponentManager ComponentManager
        {
            get { return this.m_csManager; }
            set { this.m_csManager = value; }
        }

        /// <summary>
        /// Accessor for m_nodeMenu.
        /// </summary>
        public ContextMenuStrip NodeMenu
        {
            get { return m_nodeMenu; }
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
                foreach(CanvasControl set in m_canvasDict.Values)
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
            get { return this.m_window; }
            set { this.m_window = value; }
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

        /// <summary>
        /// Accessor for m_cMenuDict.
        /// </summary>
        public Dictionary<string, ToolStripItem> ContextMenuDict
        {
            get { return m_cMenuDict; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// the constructor for PathwayView.
        /// set the handler of event and user control.
        /// </summary>
        public PathwayControl(PathwayWindow window)
        {
            this.m_window = window;
            // Preparing Interfaces
            m_pathwayView = new PathwayView(this);
            m_overView = new OverView();
            m_layerView = new LayerView(this);
            // Create canvas.
            m_canvasDict = new Dictionary<string, CanvasControl>();
            m_nodeMenu = GetPopUpMenues();
            m_csManager = ComponentManager.LoadComponentSettings();
        }

        #endregion
        /// <summary>
        /// Create pathway canvas.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        public void CreateCanvas(string modelID)
        {
            // Clear current canvas (TODO: Remove when support multiple canvas).
            m_canvasDict = new Dictionary<string, CanvasControl>();

            // Create canvas
            CanvasControl canvas = new CanvasControl(this, modelID);
            m_activeCanvasID = modelID;
            m_canvasDict.Add(modelID, canvas);
            // Set Pathwayview
            m_pathwayView.Clear();
            m_pathwayView.TabControl.Controls.Add(canvas.TabPage);
            // Set Overview
            m_overView.SetCanvas(canvas.OverviewCanvas);
            canvas.UpdateOverview();
            // Set Layerview
            m_layerView.DataGridView.DataSource = canvas.LayerTable;
        }

        /// <summary>
        /// Add new object to this canvas.
        /// </summary>
        /// <param name="modelID">name of model</param>
        /// <param name="systemName">name of system</param>
        /// <param name="eo">EcellObject</param>
        /// <param name="isAnchor">True is default. If undo unit contains multiple actions,
        /// only the last action's isAnchor is true, the others' isAnchor is false</param>
        public void DataAdd(string modelID,
            string systemName,
            EcellObject eo,
            bool isAnchor)
        {
            // Null check.
            if (eo == null)
                throw new PathwayException(m_resources.GetString("ErrAddObjNot"));
            // Load new project
            if (eo.type.Equals(EcellObject.PROJECT))
            {
                this.Clear();
                return;
            }
            // create new canvas
            if (eo.type.Equals(EcellObject.MODEL))
            {
                this.CreateCanvas(eo.modelID);
                return;
            }
            // Error check.
            if (string.IsNullOrEmpty(eo.key))
                throw new PathwayException(m_resources.GetString("ErrKeyNot"));
            if (string.IsNullOrEmpty(modelID) || !m_canvasDict.ContainsKey(modelID))
                throw new PathwayException(m_resources.GetString("ErrNotSetCanvas") + eo.key);
            // Set ProjectData
            if (eo.key.EndsWith(":SIZE"))
                return;

            CanvasControl canvas = m_canvasDict[modelID];
            // Create PathwayObject and set to canvas.
            bool isPosSet = eo.IsPosSet;
            ComponentSetting cs = GetComponentSetting(eo.type);
            PPathwayObject obj = cs.CreateNewComponent(eo, this);
            if (eo is EcellSystem)
            {
                PPathwaySystem system = (PPathwaySystem)obj;
                system.Refresh();
                system.MouseDown += new PInputEventHandler(SystemSelected);
            }
            else
            {
                PPathwayNode node = (PPathwayNode)obj;
                node.ShowingID = m_showingId;
                node.MouseDown += new PInputEventHandler(NodeSelected);
                node.MouseEnter += new PInputEventHandler(NodeEntered);
                node.MouseLeave += new PInputEventHandler(NodeLeft);
                node.Handler4Line = new PInputEventHandler(LineSelected);
            }
            canvas.DataAdd(systemName, obj, isPosSet, false);
            NotifyDataChanged(eo.key, eo.key, obj, false, false);
        }

        /// <summary>
        /// Get ComponentSetting.
        /// </summary>
        /// <param name="type">ComponentType</param>
        /// <returns>ComponentSetting.</returns>
        private ComponentSetting GetComponentSetting(string type)
        {
            ComponentType cType = ComponentManager.ParseComponentKind(type);
            switch (cType)
            {
                case ComponentType.Process:
                    return m_csManager.DefaultProcessSetting;
                case ComponentType.System:
                    return m_csManager.DefaultSystemSetting;
                case ComponentType.Variable:
                    return m_csManager.DefaultVariableSetting;
            }
            return null;
        }
        /// <summary>
        /// Get Popup Menus.
        /// </summary>
        ///<returns>ContextMenu.</returns>
        public ContextMenuStrip GetPopUpMenues()
        {
            // Preparing a context menu.
            ContextMenuStrip nodeMenu = new ContextMenuStrip();
            nodeMenu.Closed += new ToolStripDropDownClosedEventHandler(m_nodeMenu_Closed);

            ToolStripItem idShow = new ToolStripMenuItem("tmp");
            idShow.Name = CANVAS_MENU_ID;
            nodeMenu.Items.Add(idShow);
            m_cMenuDict.Add(CANVAS_MENU_ID, idShow);

            ToolStripSeparator separator1 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator1);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR1, separator1);

            int count = 0;
            foreach (ILayoutAlgorithm algorithm in Window.LayoutAlgorithm)
            {
                ToolStripMenuItem algoItem = new ToolStripMenuItem(algorithm.GetMenuText());
                algoItem.Tag = count;
                algoItem.ToolTipText = algorithm.GetToolTipText();
                algoItem.Click += new EventHandler(Window.eachLayoutItem_Click);

                List<string> subCommands = algorithm.GetSubCommands();
                if (subCommands != null && subCommands.Count != 0)
                {
                    int subcount = 0;
                    foreach (string subCommandName in subCommands)
                    {
                        ToolStripMenuItem layoutSubItem = new ToolStripMenuItem();
                        layoutSubItem.Text = subCommandName;
                        layoutSubItem.Tag = count + "," + subcount;
                        layoutSubItem.Click += new EventHandler(Window.eachLayoutItem_Click);
                        algoItem.DropDownItems.Add(layoutSubItem);
                        subcount++;
                    }
                }

                nodeMenu.Items.Add(algoItem);

                count++;
            }

            ToolStripSeparator separator2 = new ToolStripSeparator();
            separator2.Tag = count;
            nodeMenu.Items.Add(separator2);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR2, separator2);

            ToolStripItem rightArrow = new ToolStripMenuItem("Process -> Variable", Resource1.arrow_long_right_w);
            rightArrow.Name = CANVAS_MENU_RIGHT_ARROW;
            rightArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(rightArrow);
            m_cMenuDict.Add(CANVAS_MENU_RIGHT_ARROW, rightArrow);

            ToolStripItem leftArrow = new ToolStripMenuItem("Process <- Variable", Resource1.arrow_long_left_w);
            leftArrow.Name = CANVAS_MENU_LEFT_ARROW;
            leftArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(leftArrow);
            m_cMenuDict.Add(CANVAS_MENU_LEFT_ARROW, leftArrow);

            ToolStripItem bidirArrow = new ToolStripMenuItem("Process <-> Variable", Resource1.arrow_long_bidir_w);
            bidirArrow.Name = CANVAS_MENU_BIDIR_ARROW;
            bidirArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(bidirArrow);
            m_cMenuDict.Add(CANVAS_MENU_BIDIR_ARROW, bidirArrow);

            ToolStripItem constant = new ToolStripMenuItem("Constant", Resource1.ten);
            constant.Name = CANVAS_MENU_CONSTANT_LINE;
            constant.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(constant);
            m_cMenuDict.Add(CANVAS_MENU_CONSTANT_LINE, constant);

            ToolStripSeparator separator3 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator3);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR3, separator3);

            ToolStripItem cut = new ToolStripMenuItem(m_resources.GetString("CutMenuText"));
            cut.Click += new EventHandler(this.CutClick);
            nodeMenu.Items.Add(cut);
            m_cMenuDict.Add(CANVAS_MENU_CUT, cut);

            ToolStripItem copy = new ToolStripMenuItem(m_resources.GetString("CopyMenuText"));
            copy.Click += new EventHandler(this.CopyClick);
            nodeMenu.Items.Add(copy);
            m_cMenuDict.Add(CANVAS_MENU_COPY, copy);

            ToolStripItem paste = new ToolStripMenuItem(m_resources.GetString("PasteMenuText"));
            paste.Click += new EventHandler(this.PasteClick);
            nodeMenu.Items.Add(paste);
            m_cMenuDict.Add(CANVAS_MENU_PASTE, paste);

            ToolStripItem delete = new ToolStripMenuItem(m_resources.GetString("DeleteMenuText"));
            delete.Text = m_resources.GetString("DeleteMenuText");
            delete.Click += new EventHandler(this.DeleteClick);
            nodeMenu.Items.Add(delete);
            m_cMenuDict.Add(CANVAS_MENU_DELETE, delete);

            ToolStripItem deleteWith = new ToolStripMenuItem(m_resources.GetString("MergeMenuText"));
            deleteWith.Click += new EventHandler(MergeClick);
            nodeMenu.Items.Add(deleteWith);
            m_cMenuDict.Add(CANVAS_MENU_DELETE_WITH, deleteWith);

            ToolStripSeparator separator4 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator4);
            m_cMenuDict.Add(CANVAS_MENU_SEPARATOR4, separator4);

            // Create Logger
            ToolStripMenuItem createLogger = new ToolStripMenuItem(m_resources.GetString("CreateLogMenuText"));
            nodeMenu.Items.Add(createLogger);
            m_cMenuDict.Add(CANVAS_MENU_CREATE_LOGGER, createLogger);

            // Delete Logger
            ToolStripMenuItem deleteLogger = new ToolStripMenuItem(m_resources.GetString("DeleteLogMenuText"));
            nodeMenu.Items.Add(deleteLogger);
            m_cMenuDict.Add(CANVAS_MENU_DELETE_LOGGER, deleteLogger);

#if DEBUG
            ToolStripItem debug = new ToolStripMenuItem("Debug");
            debug.Click += new EventHandler(DebugClick);
            nodeMenu.Items.Add(debug);
            //m_cMenuDict.Add(CANVAS_MENU_DELETE, delete);
#endif
            return nodeMenu;
        }

        /// <summary>
        /// Set logger menu items.
        /// </summary>
        /// <param name="obj">Selected PPathwayObject.</param>
        public void SetMenuLogger(PPathwayObject obj)
        {
            ContextMenuDict[PathwayControl.CANVAS_MENU_ID].Text = obj.EcellObject.key;

            ToolStripMenuItem createLogger = (ToolStripMenuItem)ContextMenuDict[PathwayControl.CANVAS_MENU_CREATE_LOGGER];
            ToolStripMenuItem deleteLogger = (ToolStripMenuItem)ContextMenuDict[PathwayControl.CANVAS_MENU_DELETE_LOGGER];
            createLogger.DropDown.Items.Clear();
            deleteLogger.DropDown.Items.Clear();

            if (obj.EcellObject == null || obj.EcellObject.modelID == null)
                return;

            EcellObject ecellobj = obj.EcellObject;
            if (ecellobj == null)
                return;
            // set logger menu
            foreach (EcellData d in ecellobj.Value)
            {
                // Create "Create Logger" menu.
                if (!d.Logable)
                    continue;
                ToolStripItem createItem = new ToolStripMenuItem(d.Name);
                createItem.Text = d.Name;
                createItem.Click += new EventHandler(CreateLoggerClick);
                createLogger.DropDown.Items.Add(createItem);

                // create "Delete Logger" menu.
                if (!d.Logged)
                    continue;
                ToolStripItem deleteItem = new ToolStripMenuItem(d.Name);
                deleteItem.Text = d.Name;
                deleteItem.Click += new EventHandler(DeleteLoggerClick);
                deleteLogger.DropDown.Items.Add(deleteItem);
            }
            createLogger.Enabled = (createLogger.DropDown.Items.Count != 0);
            deleteLogger.Enabled = (deleteLogger.DropDown.Items.Count != 0);
        }
        /// <summary>
        /// Add the selected EventHandler to event listener.
        /// </summary>
        /// <param name="handler">added EventHandler.</param>
        public void AddInputEventListener(PBasicInputEventHandler handler)
        {
            // Exception condition 
            if (m_canvasDict == null)
                return;

            foreach (CanvasControl set in m_canvasDict.Values)
                set.PathwayCanvas.AddInputEventListener(handler);
        }

        /// <summary>
        /// Delete the selected EventHandler from event listener.
        /// </summary>
        /// <param name="handler">deleted EventHandler.</param>
        public void RemoveInputEventListener(PBasicInputEventHandler handler)
        {
            // Exception condition 
            if (m_canvasDict == null)
                return;

            foreach(CanvasControl set in m_canvasDict.Values)
                set.PathwayCanvas.RemoveInputEventListener(handler);
        }

        #region Event delegate
#if DEBUG
        /// <summary>
        /// Called when a debug menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DebugClick(object sender, EventArgs e)
        {
            if (ActiveCanvas.ClickedNode is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)ActiveCanvas.ClickedNode;
                MessageBox.Show(
                    "Name:" + obj.EcellObject.key
                    + "\nLayer:" + obj.EcellObject.LayerID
                    + "\nX:" + obj.X + "\nY:" + obj.Y
                    + "\nOffsetX:" + obj.OffsetX + "\nOffsetY:" + obj.OffsetY 
                    + "\nToString()" + obj.ToString());
            }
            else
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                PointF point = new PointF();
                point.X = item.Owner.Left;
                point.Y = item.Owner.Top;
                MessageBox.Show("Left:" + point.X + "Top:" + point.Y);

            }
        }
#endif
        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MergeClick(object sender, EventArgs e)
        {
            // Check exception.
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!(item.Tag is PPathwaySystem))
                return;
            PPathwaySystem system = (PPathwaySystem)item.Tag;
            if (system.EcellObject.key.Equals("/"))
            {
                MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            // Check Object duplication.
            bool isDuplicate = false;
            string sysKey = system.EcellObject.key;
            string parentSysKey = system.EcellObject.parentSystemID;
            foreach (PPathwayObject obj in ActiveCanvas.GetAllObjectUnder(sysKey))
            {
                string newKey = obj.EcellObject.key.Replace(sysKey,parentSysKey);
                if (ActiveCanvas.GetSelectedObject(newKey, obj.EcellObject.type) != null)
                {
                    isDuplicate = true;
                    MessageBox.Show(newKey + m_resources.GetString("ErrAlrExist"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            if (isDuplicate)
                return;

            // Confirm system merge.
            DialogResult result = MessageBox.Show(m_resources.GetString("ConfirmMerge"),
                "Merge",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Cancel)
                return;

            try
            {
                // Move systems and nodes under merged system.
                foreach (PPathwayObject obj in ActiveCanvas.GetAllObjectUnder(sysKey))
                {
                    string newKey = obj.EcellObject.key.Replace(sysKey, parentSysKey);
                    ActiveCanvas.TransferObject(newKey, obj.EcellObject.key, obj);
                }
                m_window.NotifyDataMerge(system.EcellObject.modelID, system.EcellObject.key);
            }
            catch (IgnoreException)
            {
                return;
            }
            if (system.IsHighLighted)
                ActiveCanvas.ResetSelectedSystem();

            ((ToolStripMenuItem)sender).Tag = null;
        }
        /// <summary>
        /// Called when a create logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateLoggerClick(object sender, EventArgs e)
        {
            if (!(ActiveCanvas.ClickedNode is PPathwayObject))
                return;
            string logger = ((ToolStripItem)sender).Text;

            PPathwayObject obj = (PPathwayObject)ActiveCanvas.ClickedNode;
            Debug.WriteLine("Create " + obj.EcellObject.type + " Logger:" + obj.EcellObject.key);
            // set logger
            NotifyLoggerChanged(obj, logger, true);
        }
        /// <summary>
        /// Called when a delete logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteLoggerClick(object sender, EventArgs e)
        {
            if (!(ActiveCanvas.ClickedNode is PPathwayObject))
                return;
            string logger = ((ToolStripItem)sender).Text;

            PPathwayObject obj = (PPathwayObject)ActiveCanvas.ClickedNode;
            Debug.WriteLine("Delete " + obj.EcellObject.type + " Logger:" + obj.EcellObject.key);
            // delete logger
            NotifyLoggerChanged(obj, logger, false);
        }

        /// <summary>
        /// Notify logger change.
        /// </summary>
        /// <param name="obj">The object to change the logger property.</param>
        /// <param name="logger">The logger entity.</param>
        /// <param name="isLogger">The flag whether the entity is logged.</param>
        private void NotifyLoggerChanged(PPathwayObject obj, string logger, bool isLogger)
        {
            EcellObject eo = m_window.GetEcellObject(obj.EcellObject.modelID, obj.EcellObject.key, obj.EcellObject.type);

            // set logger
            foreach (EcellData d in eo.Value)
            {
                if (!logger.Equals(d.Name))
                    continue;
                // Set isLogger
                d.Logged = isLogger;

                // If isLogger, 
                if (!isLogger)
                    continue;
                m_window.NotifyLoggerAdd(
                    eo.modelID,
                    eo.key,
                    eo.type,
                    d.EntityPath);
            }
            m_window.NotifyDataChanged(eo.key, eo.key, eo, true, true);
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
        /// <summary>
        /// Called when a change line menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeLineClick(object sender, EventArgs e)
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

                NotifyVariableReferenceChanged(
                    edgeInfo.ProcessKey,
                    edgeInfo.VariableKey,
                    changeType,
                    coefficient);
            }
        }
        /// <summary>
        /// When select the button in ToolBox,
        /// system change the listener for event
        /// </summary>
        /// <param name="sender">ToolBoxMenuButton.</param>
        /// <param name="e">EventArgs.</param>
        public void ButtonStateChanged(object sender, EventArgs e)
        {
            RemoveInputEventListener(m_handlerDict[m_selectedHandle.HandleID]);
            m_selectedHandle = (Handle)((ToolStripButton)sender).Tag;
            
            foreach (ToolStripButton button in m_buttonList)
            {
                if (button.Tag != m_selectedHandle)
                {
                    button.Checked = false;
                }
            }

            AddInputEventListener(m_handlerDict[m_selectedHandle.HandleID]);

            if (m_selectedHandle.Mode == Mode.Pan)
            {
                m_pathwayView.Cursor = new Cursor(new MemoryStream(Resource1.move));
                Freeze();
            }
            else
            {
                m_pathwayView.Cursor = Cursors.Arrow;
                Unfreeze();
            }
            if (ActiveCanvas == null)
                return;
            ActiveCanvas.ResetNodeToBeConnected();
            ActiveCanvas.SetLineVisibility(false);
        }
        #endregion
        
        #region Methods from PathwayWindow(Interface) to Controller        
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
            handButton.Tag = new Handle(Mode.Pan, handleCount);
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
            button0.Tag = new Handle(Mode.Select, handleCount);
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
            arrowButton.ToolTipText = "Add Oneway Reaction";
            arrowButton.Tag = new Handle(Mode.CreateOneWayReaction, handleCount);
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
            bidirButton.ToolTipText = "Add Mutual Reaction";
            bidirButton.Tag = new Handle(Mode.CreateMutualReaction, handleCount);
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
            constButton.Tag = new Handle(Mode.CreateConstant, handleCount);
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

            CreateSystemMouseHandler csmh = new CreateSystemMouseHandler(m_window.DataManager, this);
            CreateNodeMouseHandler cnmh = new CreateNodeMouseHandler(m_window.DataManager, this);

            foreach (ComponentSetting cs in m_csManager.ComponentSettings)
            {
                ToolStripButton button = new ToolStripButton();
                button.ImageTransparentColor = System.Drawing.Color.Magenta;
                button.Name = cs.Name;
                button.Image = new Bitmap(256, 256);
                Graphics gra = Graphics.FromImage(button.Image);
                if (cs.ComponentType == ComponentType.System)
                {
                    Rectangle rect = new Rectangle(3, 3, 240, 240);
                    gra.DrawRectangle(new Pen(Brushes.Black, 16), rect);
                    //m_objectHandlerList.Add(new CreateSystemMouseHandler(this));
                    m_handlerDict.Add(handleCount, csmh);
                    button.Tag = new Handle(Mode.CreateSystem, handleCount++, cs.ComponentType);
                }
                else
                {
                    GraphicsPath gp = cs.TransformedPath;
                    gra.FillPath(cs.NormalBrush, gp);
                    gra.DrawPath(new Pen(Brushes.Black, 16), gp);
                    //m_objectHandlerList.Add(new CreateNodeMouseHandler(this));
                    m_handlerDict.Add(handleCount, cnmh);
                    button.Tag = new Handle(Mode.CreateNode, handleCount++, cs.ComponentType);
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
            m_selectedHandle = (Handle)button0.Tag;

            return list;
        }

        void ZoomButton_Click(object sender, EventArgs e)
        {
            float rate = (float)((ToolStripButton)sender).Tag;
            if (this.CanvasDictionary == null)
                return;

            foreach(CanvasControl canvas in this.CanvasDictionary.Values)
            {
                canvas.Zoom(rate);
            }
        }

        /// <summary>
        /// get bitmap image of this pathway.
        /// </summary>
        /// <returns>Bitmap of this pathway.</returns>
        public Bitmap Print()
        {
            if(m_canvasDict != null && m_canvasDict.Count != 0)
            {
                foreach(CanvasControl set in m_canvasDict.Values)
                {
                    return set.ToImage();
                }
                return new Bitmap(1, 1);
            }
            else
                return new Bitmap(1, 1);
        }

        /// <summary>
        /// Set position of EcellObject.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <param name="eo">The EcellObject set in canvas.</param>
        public void SetPosition(string modelID, EcellObject eo)
        {
            if(EcellObject.SYSTEM.Equals(eo.type))
                if (m_canvasDict[modelID].Systems.ContainsKey(eo.key))
                    m_canvasDict[modelID].Systems[eo.key].EcellObject = (EcellSystem)eo;

            else if(EcellObject.VARIABLE.Equals(eo.type))
                if (m_canvasDict[modelID].Variables.ContainsKey(eo.key))
                    m_canvasDict[modelID].Variables[eo.key].EcellObject = (EcellVariable)eo;

            else if(EcellObject.PROCESS.Equals(eo.type))
                if(m_canvasDict[modelID].Processes.ContainsKey(eo.key))
                    m_canvasDict[modelID].Processes[eo.key].EcellObject = (EcellProcess)eo;

            if (eo.Children == null || eo.Children.Count == 0)
                return;
            foreach (EcellObject child in eo.Children)
                SetPosition(modelID, child);
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
            if (m_window != null)
                m_window.NotifyDataAdd(list, isAnchor);
        }

        /// <summary>
        /// Notify DataChanged event to outside (PathwayView -> PathwayWindow -> DataManager)
        /// To notify position or size change.
        /// </summary>
        /// <param name="oldKey">the key before adding.</param>
        /// <param name="newKey">the key after adding.</param>
        /// <param name="obj">x coordinate of object.</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        /// <param name="isAnchor">Whether this action is an anchor or not.</param>
        public void NotifyDataChanged(
            string oldKey,
            string newKey,
            PPathwayObject obj,
            bool isRecorded,
            bool isAnchor)
        {
            EcellObject eo = m_window.GetEcellObject(obj.EcellObject.modelID, oldKey, obj.EcellObject.type);
            if (eo == null)
                throw new Exception();
            eo.key = newKey;
            eo.LayerID = obj.Layer.Name;
            eo.X = obj.X;
            eo.Y = obj.Y;
            eo.Width = obj.Width;
            eo.Height = obj.Height;
            eo.OffsetX = obj.OffsetX;
            eo.OffsetY = obj.OffsetY;

            m_window.NotifyDataChanged(oldKey, newKey, eo, isRecorded, isAnchor);
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
            // Get EcellObject of identified process.
            EcellProcess ep = (EcellProcess)m_window.GetEcellObject(ActiveCanvas.ModelID, proKey, EcellObject.PROCESS);
            // End if obj is null.
            if (null == ep)
                return;
            // Get EcellReference List.
            List<EcellReference> refList = ep.ReferenceList;
            List<EcellReference> newList = new List<EcellReference>();
            EcellReference changedRef = null;

            foreach (EcellReference v in refList)
            {
                if (v.fullID.EndsWith(varKey))
                    changedRef = v;
                else
                    newList.Add(v);
            }

            if (changedRef != null && changeType != RefChangeType.Delete)
            {
                switch(changeType)
                {
                    case RefChangeType.SingleDir:
                        changedRef.coefficient = coefficient;
                        changedRef.name = PathUtil.GetNewReferenceName(newList, coefficient);
                        newList.Add(changedRef);
                        break;
                    case RefChangeType.BiDir:
                        EcellReference copyRef = PathUtil.CopyEcellReference(changedRef);
                        changedRef.coefficient = -1;
                        changedRef.name = PathUtil.GetNewReferenceName(newList, -1);
                        copyRef.coefficient = 1;
                        copyRef.name = PathUtil.GetNewReferenceName(newList, 1);
                        newList.Add(changedRef);
                        newList.Add(copyRef);
                        break;
                }
            }
            else if(changedRef == null)
            {
                switch(changeType)
                {
                    case RefChangeType.SingleDir:
                        EcellReference addRef = new EcellReference();
                        addRef.coefficient = coefficient;
                        addRef.Key = varKey;
                        addRef.name = PathUtil.GetNewReferenceName(newList, coefficient);
                        addRef.isAccessor = 1;
                        newList.Add(addRef);
                        break;
                    case RefChangeType.BiDir:
                        EcellReference addSRef = new EcellReference();
                        addSRef.coefficient = -1;
                        addSRef.Key = varKey;
                        addSRef.name = PathUtil.GetNewReferenceName(newList, -1);
                        addSRef.isAccessor = 1;
                        newList.Add(addSRef);

                        EcellReference addPRef = new EcellReference();
                        addPRef.coefficient = 1;
                        addPRef.Key = varKey;
                        addPRef.name = PathUtil.GetNewReferenceName(newList, 1);
                        addPRef.isAccessor = 1;
                        newList.Add(addPRef);
                        break;
                }
            }
            ep.ReferenceList = newList;
            m_window.NotifyDataChanged(ep.key, ep.key, ep, true, true);
        }

        /// <summary>
        /// Notify DataDelete event to outsite.
        /// </summary>
        /// <param name="eo">the deleted object.</param>
        /// <param name="isAnchor">the type of deleted object.</param>
        public void NotifyDataDelete(EcellObject eo, bool isAnchor)
        {
            m_window.NotifyDataDelete(eo.modelID, eo.key, eo.type, isAnchor);
        }
        #endregion

        /// <summary>
        /// Freeze all objects to be unpickable.
        /// </summary>
        private void Freeze()
        {
            if (m_isFreezed)
                return;
            if(null != m_canvasDict)
            {
                foreach (CanvasControl canvas in m_canvasDict.Values)
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
                foreach (CanvasControl canvas in m_canvasDict.Values)
                    canvas.Unfreeze();
            }
            m_isFreezed = false;
        }

        /// <summary>
        /// Clears the contents of the pathway window, and Returns it to an
        /// initial state.
        /// </summary>
        public void Clear()
        {
            // Reset Interfaces
            m_overView.Clear();
            m_pathwayView.TabControl.TabPages.Clear();
            m_layerView.DataGridView.DataSource = null;

            // Clear Canvas dictionary.
            if(m_canvasDict != null)
                foreach(CanvasControl set in m_canvasDict.Values)
                    set.Dispose();
            m_canvasDict = null;
        }

        #region EventHandler
       /// <summary>
        /// the event sequence of selecting the PNode of system in PathwayEditor.
        /// </summary>
        /// <param name="sender">PPathwaySystem</param>
        /// <param name="e">PInputEventArgs</param>
        public void SystemSelected(object sender, PInputEventArgs e)
        {
            CanvasControl canvas = this.CanvasDictionary[e.Canvas.Name];
            if (canvas == null)
                return;

            if (e.PickedNode == sender)
            {
                canvas.ResetSelectedObjects();
                canvas.AddSelectedSystem(((PPathwayObject)sender).EcellObject.key);
                canvas.ClickedNode = e.PickedNode;
            }
            else
            {
                foreach (String iName in ContextMenuDict.Keys)
                {
                    if (iName.StartsWith("delete"))
                    {
                        ContextMenuDict[iName].Tag = e.PickedNode;
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
            PPathwayNode pnode = (PPathwayNode)sender;

            if (m_selectedHandle.Mode == Mode.CreateOneWayReaction
                || m_selectedHandle.Mode == Mode.CreateMutualReaction
                || m_selectedHandle.Mode == Mode.CreateConstant)
            {
                this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
                this.CanvasDictionary[e.Canvas.Name].AddNodeToBeConnected(pnode);
            }
            else
            {
                if (!pnode.IsHighLighted)
                {
                    if (e.Modifiers == Keys.Shift)
                    {
                        this.CanvasDictionary[e.Canvas.Name].AddSelectedNode(pnode, true);
                    }
                    else
                    {
                        this.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
                        this.CanvasDictionary[e.Canvas.Name].NotifySelectChanged(pnode.EcellObject.key, pnode.EcellObject.type);
                    }
                }
            }
            this.CanvasDictionary[e.Canvas.Name].ClickedNode = pnode;
            foreach (String iName in ContextMenuDict.Keys)
            {
                if (iName.StartsWith("delete"))
                {
                    ContextMenuDict[iName].Tag = pnode;
                }
            }
        }

        /// <summary>
        /// Called when the mouse enters a node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NodeEntered(object sender, PInputEventArgs e)
        {
            if (m_selectedHandle.Mode == Mode.CreateOneWayReaction ||
                m_selectedHandle.Mode == Mode.CreateMutualReaction ||
                m_selectedHandle.Mode == Mode.CreateConstant)
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
            if (m_selectedHandle.Mode == Mode.CreateOneWayReaction ||
                m_selectedHandle.Mode == Mode.CreateMutualReaction ||
                m_selectedHandle.Mode == Mode.CreateConstant)
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

            CanvasControl canvas = this.CanvasDictionary[e.Canvas.Name];
            Line line = (Line)e.PickedNode;

            canvas.ResetSelectedObjects();
            canvas.AddSelectedLine(line);

            if (e.Button == MouseButtons.Right)
                canvas.ClickedNode = line;
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
                this.NotifyDataDelete(eo, isAnchor);
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

        private List<EcellObject> SetCopyNodes(List<PPathwayObject> nodeList)
        {
            List<EcellObject> copyNodes = new List<EcellObject>();
            //Copy Variavles
            foreach (PPathwayObject node in nodeList)
                if (node is PPathwayVariable)
                    copyNodes.Add(node.EcellObject.Copy());
            //Copy Processes
            foreach (PPathwayObject node in nodeList)
                if (node is PPathwayProcess)
                    copyNodes.Add(node.EcellObject.Copy());
            return copyNodes;
        }

        private List<EcellObject> CopyNodes(List<EcellObject> nodeList)
        {
            List<EcellObject> copiedNodes = new List<EcellObject>();
            Dictionary<string, string> varKeys = new Dictionary<string, string>();
            // Get position diff
            PointF diff = GetDiff(this.MousePosition, this.m_copyPos);
            // Get parent System
            string sysKey;
            // Set m_copiedNodes.
            if (nodeList != null)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    EcellObject node = nodeList[i];
                    //Create new EcellObject
                    EcellObject eo = node.Copy();
                    // Check parent system
                    eo.SetPosition(eo.X + diff.X, eo.Y + diff.Y);
                    sysKey = this.ActiveCanvas.GetSurroundingSystemKey(eo.PointF);
                    if( sysKey == null )
                        sysKey = this.ActiveCanvas.GetSurroundingSystemKey(this.MousePosition);
                    if (eo.parentSystemID != sysKey)
                        eo.parentSystemID = sysKey;
                    // Check Position
                    if (!ActiveCanvas.CheckNodePosition(eo))
                        eo.PointF = ActiveCanvas.GetVacantPoint(sysKey, eo.PointF);
                    // Check duplicated object.
                    if (ActiveCanvas.GetSelectedObject(eo.key, eo.type) != null)
                        eo.key = m_window.GetTemporaryID(eo.modelID, eo.type, sysKey);

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
                    eo.GetEcellData(EcellProcess.VARIABLEREFERENCELIST).Value = eo.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST).Copy();
                    foreach (EcellValue edge in eo.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST).CastToList())
                    {
                        foreach (EcellValue val in edge.CastToList())
                            if (varKeys.ContainsKey(val.ToString()))
                                val.Value = varKeys[val.ToString()];
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
            if (ActiveCanvas == null) return;
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
                foreach (PPathwayObject node in this.ActiveCanvas.SelectedNodes)
                    slist.Add(node.EcellObject);

                int i = 0;
                foreach (EcellObject deleteNode in slist)
                {
                    i++;
                    bool isAnchor = (i == slist.Count);
                    NotifyDataDelete(deleteNode, isAnchor);
                }
            }
            // Delete Selected System
            if (ActiveCanvas.SelectedSystemName != null)
            {
                PPathwaySystem system = ActiveCanvas.Systems[ActiveCanvas.SelectedSystemName];
                // Return if system is null or root.
                if (string.IsNullOrEmpty(system.EcellObject.key))
                    return;
                if (system.EcellObject.key.Equals("/"))
                {
                    MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                // Delete sys.
                NotifyDataDelete(system.EcellObject, true);
                ActiveCanvas.ResetSelectedSystem();
            }
        }

        private void DeleteSystemUnder(PPathwaySystem system)
        {
            foreach (PNode obj in system.ChildrenReference)
                if (obj is PPathwaySystem)
                    DeleteSystemUnder((PPathwaySystem)obj);

            NotifyDataDelete(system.EcellObject, true);
        }

        /// <summary>
        /// Do object layout.
        /// </summary>
        /// <param name="algorithm">ILayoutAlgorithm</param>
        /// <param name="subIdx">int</param>
        /// <param name="isRecorded">Whether to record this change.</param>
        public void DoLayout(ILayoutAlgorithm algorithm, int subIdx, bool isRecorded)
        {

            List<EcellObject> systemList = this.GetSystemList(ActiveCanvas.ModelID);
            List<EcellObject> nodeList = this.GetNodeList(ActiveCanvas.ModelID);

            // Check Selected nodes when the layout algorithm uses selected objects.
            if (algorithm.GetLayoutType() == LayoutType.Selected)
                foreach (EcellObject node in nodeList)
                    node.isFixed = this.ActiveCanvas.GetSelectedObject(node.key, node.type).IsHighLighted;

            try
            {
                algorithm.DoLayout(subIdx, false, systemList, nodeList);
            }
            catch (Exception)
            {
                MessageBox.Show(m_resources.GetString("ErrLayout"));
                return;
            }

            // Set Layout.
            foreach (EcellObject system in systemList)
                this.m_window.NotifyDataChanged(system.key, system.key, system, isRecorded, false);
            int i = 0;
            foreach (EcellObject node in nodeList)
            {
                node.isFixed = false;
                if(i != nodeList.Count)
                    this.m_window.NotifyDataChanged(node.key, node.key, node, isRecorded, false);
                else
                    this.m_window.NotifyDataChanged(node.key, node.key, node, isRecorded, true);
                i++;
            }
        }

        /// <summary>
        /// Get the list of system in the target mode.
        /// </summary>
        /// <param name="modelID">The model ID.</param>
        /// <returns>The list of system.</returns>
        public List<EcellObject> GetSystemList(string modelID)
        {
            List<EcellObject> systemList = new List<EcellObject>();
            foreach(PPathwayObject obj in m_canvasDict[modelID].GetSystemList())
                systemList.Add(m_window.GetEcellObject(modelID, obj.EcellObject.key, obj.EcellObject.type));
            return systemList;
        }

        /// <summary>
        /// Get the list of EcellObject in the target model.
        /// </summary>
        /// <param name="modelID">the model ID.</param>
        /// <returns>the list of EcellObject.</returns>
        public List<EcellObject> GetNodeList(string modelID)
        {
            List<EcellObject> nodeList = new List<EcellObject>();
            foreach (PPathwayObject obj in m_canvasDict[modelID].GetNodeList())
                nodeList.Add(m_window.GetEcellObject(modelID, obj.EcellObject.key, obj.EcellObject.type));

            return nodeList;
        }

        /// <summary>
        /// The event sequence on changing value of data at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID before value change.</param>
        /// <param name="oldKey">The ID before value change.</param>
        /// <param name="type">The data type before value change.</param>
        /// <param name="eo">Changed value of object.</param>
        public void DataChanged(string modelID, string oldKey, string type, EcellObject eo)
        {
            // Select Canvas
            CanvasControl canvas = m_canvasDict[modelID];
            // If case SystemSize
            if( oldKey.EndsWith(":SIZE") )
            {
                ChangeSystemSize(modelID, eo.parentSystemID, eo.GetEcellValue("Value").CastToDouble());
                return;
            }

            // Select changed object.
            PPathwayObject obj = canvas.GetSelectedObject(oldKey, type);
            if (obj == null)
                return;

            // Change data.
            obj.EcellObject = eo;
            canvas.DataChanged(oldKey, eo.key, obj);
        }

        private void ChangeSystemSize(string modelID, string sysKey, double size)
        {
            EcellObject eo = m_window.GetEcellObject(modelID, sysKey, EcellObject.SYSTEM);
            eo.GetEcellValue(EcellSystem.SIZE).Value = size;
            DataChanged(modelID, eo.key, eo.type, eo);
        }

        /// <summary>
        /// The event sequence on deleting the object at other plugin.
        /// </summary>
        /// <param name="modelID">The model ID of deleted object.</param>
        /// <param name="key">The ID of deleted object.</param>
        /// <param name="type">The object type of deleted object.</param>
        public void DataDelete(string modelID, string key, string type)
        {
            CanvasControl canvas = this.m_canvasDict[modelID];
            if (canvas == null)
                return;
            // If case SystemSize
            if (key.EndsWith(":SIZE"))
            {
                ChangeSystemSize(modelID, PathUtil.GetParentSystemId(key), 0.1d);
                return;
            }

            // Delete object.
            canvas.DataDelete(key, type);

        }

        #endregion
    }
}