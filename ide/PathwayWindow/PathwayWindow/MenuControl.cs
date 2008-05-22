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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Handler;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.Graphic;
using EcellLib.PathwayWindow.Dialog;
using EcellLib.Layout;
using EcellLib.Objects;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// MenuControl for PathwayWindow
    /// </summary>
    public class MenuControl
    {
        #region Fields
        /// <summary>
        /// The PathwayView, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        private PathwayControl m_con;

        /// <summary>
        /// A list of menu items.
        /// </summary>
        private List<ToolStripMenuItem> m_menuList;

        /// <summary>
        /// A list for menu of layout algorithm, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ToolStripItem> m_menuLayoutList;

        /// <summary>
        /// ContextMenuStrip for PPathwayNode
        /// </summary>
        private ContextMenuStrip m_popupMenu;

        /// <summary>
        /// List of ToolStripMenuItems for ContextMenu
        /// </summary>
        private Dictionary<string, ToolStripItem> m_popMenuDict = new Dictionary<string, ToolStripItem>();

        /// <summary>
        /// A list of toolbox buttons.
        /// </summary>
        private ToolStrip m_buttonList;

        /// <summary>
        /// Indicate which pathway-related toolbar button is selected.
        /// </summary>
        private Handle m_handle;

        /// <summary>
        /// Indicate which pathway-related toolbar button is selected.
        /// </summary>
        private Handle m_defHandle;

        /// <summary>
        /// Dictionary for Eventhandlers.
        /// </summary>
        private Dictionary<string, Handle> m_handleDict = new Dictionary<string, Handle>();

        /// <summary>
        /// A list for layout algorithms, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ILayoutAlgorithm> m_layoutList = new List<ILayoutAlgorithm>();

        /// <summary>
        /// Default layout algorithm
        /// </summary>
        private ILayoutAlgorithm m_defaultLayoutAlgorithm;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_buttonList.
        /// </summary>
        public ToolStrip ToolButtons
        {
            get { return m_buttonList; }
            set { m_buttonList = value; }
        }

        /// <summary>
        /// get/set the number of checked component.
        /// </summary>
        public Handle Handle
        {
            get { return m_handle; }
            set { m_handle = value; }
        }

        /// <summary>
        /// Dictionary of EventHandlers
        /// </summary>
        public Dictionary<string, Handle> HandleDict
        {
            get { return m_handleDict; }
        }

        /// <summary>
        /// Accessor for m_cMenuDict.
        /// </summary>
        public Dictionary<string, ToolStripItem> PopupMenuDict
        {
            get { return m_popMenuDict; }
        }

        /// <summary>
        /// Accessor for m_nodeMenu.
        /// </summary>
        public ContextMenuStrip PopupMenu
        {
            get { return m_popupMenu; }
        }

        /// <summary>
        /// Accessor for m_menuList.
        /// </summary>
        public List<ToolStripMenuItem> ToolMenuList
        {
            get { return m_menuList; }
            set { m_menuList = value; }
        }

        /// <summary>
        /// Accessor for m_menuLayoutList.
        /// </summary>
        public List<ToolStripItem> LayoutMenus
        {
            get { return m_menuLayoutList; }
        }

        /// <summary>
        /// LayoutList
        /// </summary>
        public List<ILayoutAlgorithm> LayoutList
        {
            get { return m_layoutList; }
            set { m_layoutList = value; }
        }

        /// <summary>
        /// DefaultLayoutAlgorithm
        /// </summary>
        public ILayoutAlgorithm DefaultLayoutAlgorithm
        {
            get { return m_defaultLayoutAlgorithm; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public MenuControl(PathwayControl control)
        {
            m_con = control;
            m_layoutList = m_con.Window.GetLayoutAlgorithms();
            m_defaultLayoutAlgorithm = GetDefaultLayoutAlgorithm();
            m_menuLayoutList = CreateLayoutMenus();
            m_menuList = CreateMenuItems();
            m_buttonList = CreateToolButtons();
            m_popupMenu = CreatePopUpMenus();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ILayoutAlgorithm GetDefaultLayoutAlgorithm()
        {
            ILayoutAlgorithm la = null;
            foreach (ILayoutAlgorithm algo in m_layoutList)
            {
                if (algo.GetName() == "Grid")
                {
                    la = algo;
                }
            }
            return la;
        }
        #endregion

        #region Methods to Create Menus
        /// <summary>
        /// Create LayoutAlgorithm menus.
        /// </summary>
        private List<ToolStripItem> CreateLayoutMenus()
        {
            // Create menu.
            List<ToolStripItem> menuList = new List<ToolStripItem>();
            int count = 0;
            foreach (ILayoutAlgorithm algorithm in m_layoutList)
            {
                PathwayToolStripMenuItem layoutItem = new PathwayToolStripMenuItem();
                layoutItem.Text = algorithm.GetMenuText();
                layoutItem.ID = count;
                layoutItem.Visible = true;
                layoutItem.ToolTipText = algorithm.GetToolTipText();

                List<string> subCommands = algorithm.GetSubCommands();
                if (subCommands == null || subCommands.Count == 0)
                {
                    layoutItem.Click += new EventHandler(LayoutItem_Click);
                }
                else
                {
                    int subcount = 0;
                    foreach (string subCommandName in subCommands)
                    {
                        PathwayToolStripMenuItem layoutSubItem = new PathwayToolStripMenuItem();
                        layoutSubItem.Text = subCommandName;
                        layoutSubItem.ID = count;
                        layoutSubItem.SubID = subcount;
                        layoutSubItem.Visible = true;
                        layoutSubItem.Click += new EventHandler(LayoutItem_Click);
                        layoutItem.DropDownItems.Add(layoutSubItem);
                        subcount++;
                    }
                }
                menuList.Add(layoutItem);
                count++;
            }
            return menuList;
        }

        /// <summary>
        /// Create Popup Menus.
        /// </summary>
        ///<returns>ContextMenu.</returns>
        private ContextMenuStrip CreatePopUpMenus()
        {
            // Preparing a context menu.
            ContextMenuStrip nodeMenu = new ContextMenuStrip();

            // Add ID checker
            ToolStripItem idShow = new ToolStripMenuItem(MenuConstants.CanvasMenuID);
            idShow.Name = MenuConstants.CanvasMenuID;
            idShow.Click += new EventHandler(ShowPropertyDialogClick);
            nodeMenu.Items.Add(idShow);
            m_popMenuDict.Add(MenuConstants.CanvasMenuID, idShow);

            ToolStripSeparator separator1 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator1);
            m_popMenuDict.Add(MenuConstants.CanvasMenuSeparator1, separator1);

            // Add Line Changer
            ToolStripItem rightArrow = new ToolStripMenuItem(MessageResPathway.CanvasMenuRightArrow, PathwayResource.arrow_long_right_w);
            rightArrow.Name = MenuConstants.CanvasMenuRightArrow;
            rightArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(rightArrow);
            m_popMenuDict.Add(MenuConstants.CanvasMenuRightArrow, rightArrow);

            ToolStripItem leftArrow = new ToolStripMenuItem(MessageResPathway.CanvasMenuLeftArrow, PathwayResource.arrow_long_left_w);
            leftArrow.Name = MenuConstants.CanvasMenuLeftArrow;
            leftArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(leftArrow);
            m_popMenuDict.Add(MenuConstants.CanvasMenuLeftArrow, leftArrow);

            ToolStripItem bidirArrow = new ToolStripMenuItem(MessageResPathway.CanvasMenuBidirArrow, PathwayResource.arrow_long_bidir_w);
            bidirArrow.Name = MenuConstants.CanvasMenuBidirArrow;
            bidirArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(bidirArrow);
            m_popMenuDict.Add(MenuConstants.CanvasMenuBidirArrow, bidirArrow);

            ToolStripItem constant = new ToolStripMenuItem(MessageResPathway.CanvasMenuConstantLine, PathwayResource.ten);
            constant.Name = MenuConstants.CanvasMenuConstantLine;
            constant.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(constant);
            m_popMenuDict.Add(MenuConstants.CanvasMenuConstantLine, constant);

            ToolStripSeparator separator3 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator3);
            m_popMenuDict.Add(MenuConstants.CanvasMenuSeparator3, separator3);

            // Add Edit menus
            ToolStripItem cut = new ToolStripMenuItem(MessageResPathway.CanvasMenuCut);
            cut.Click += new EventHandler(CutClick);
            nodeMenu.Items.Add(cut);
            m_popMenuDict.Add(MenuConstants.CanvasMenuCut, cut);

            ToolStripItem copy = new ToolStripMenuItem(MessageResPathway.CanvasMenuCopy);
            copy.Click += new EventHandler(CopyClick);
            nodeMenu.Items.Add(copy);
            m_popMenuDict.Add(MenuConstants.CanvasMenuCopy, copy);

            ToolStripItem paste = new ToolStripMenuItem(MessageResPathway.CanvasMenuPaste);
            paste.Click += new EventHandler(PasteClick);
            nodeMenu.Items.Add(paste);
            m_popMenuDict.Add(MenuConstants.CanvasMenuPaste, paste);

            ToolStripItem delete = new ToolStripMenuItem(MessageResPathway.CanvasMenuDelete);
            delete.Click += new EventHandler(DeleteClick);
            nodeMenu.Items.Add(delete);
            m_popMenuDict.Add(MenuConstants.CanvasMenuDelete, delete);

            ToolStripItem merge = new ToolStripMenuItem(MessageResPathway.CanvasMenuMerge);
            merge.Click += new EventHandler(MergeClick);
            nodeMenu.Items.Add(merge);
            m_popMenuDict.Add(MenuConstants.CanvasMenuMerge, merge);

            ToolStripSeparator separator4 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator4);
            m_popMenuDict.Add(MenuConstants.CanvasMenuSeparator4, separator4);

            // Add Layer Menu
            ToolStripItem changeLayer = new ToolStripMenuItem(MessageResPathway.CanvasMenuChangeLayer);
            //changeLayer.Click += new EventHandler(m_con.ChangeLeyerClick);
            nodeMenu.Items.Add(changeLayer);
            m_popMenuDict.Add(MenuConstants.CanvasMenuChangeLayer, changeLayer);

            ToolStripItem moveFront = new ToolStripMenuItem(MessageResPathway.LayerMenuMoveFront);
            moveFront.Click += new EventHandler(MoveToFrontClick);
            nodeMenu.Items.Add(moveFront);
            m_popMenuDict.Add(MenuConstants.CanvasMenuMoveFront, moveFront);

            ToolStripItem moveBack = new ToolStripMenuItem(MessageResPathway.LayerMenuMoveBack);
            moveBack.Click += new EventHandler(MoveToBackClick);
            nodeMenu.Items.Add(moveBack);
            m_popMenuDict.Add(MenuConstants.CanvasMenuMoveBack, moveBack);

            ToolStripSeparator separator5 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator5);
            m_popMenuDict.Add(MenuConstants.CanvasMenuSeparator5, separator5);

            // Add LayoutMenu
            ToolStripMenuItem layout = new ToolStripMenuItem(MessageResPathway.CanvasMenuLayout);
            layout.Name = MenuConstants.CanvasMenuLayout;
            layout.DropDownItems.AddRange(CreateLayoutMenus().ToArray());
            nodeMenu.Items.Add(layout);
            m_popMenuDict.Add(MenuConstants.CanvasMenuLayout, layout);

            ToolStripSeparator separator2 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator2);
            m_popMenuDict.Add(MenuConstants.CanvasMenuSeparator2, separator2);

            // Create Logger
            ToolStripMenuItem createLogger = new ToolStripMenuItem(MessageResPathway.CanvasMenuCreateLogger);
            nodeMenu.Items.Add(createLogger);
            m_popMenuDict.Add(MenuConstants.CanvasMenuCreateLogger, createLogger);

            // Delete Logger
            ToolStripMenuItem deleteLogger = new ToolStripMenuItem(MessageResPathway.CanvasMenuDeleteLogger);
            nodeMenu.Items.Add(deleteLogger);
            m_popMenuDict.Add(MenuConstants.CanvasMenuDeleteLogger, deleteLogger);

#if DEBUG
            ToolStripItem debug = new ToolStripMenuItem("Debug");
            debug.Click += new EventHandler(DebugClick);
            nodeMenu.Items.Add(debug);
#endif
            return nodeMenu;
        }

        /// <summary>
        /// Create Tool Menus.
        /// </summary>
        /// <returns></returns>
        private List<ToolStripMenuItem> CreateMenuItems()
        {
            List<ToolStripMenuItem> menuList = new List<ToolStripMenuItem>();

            // ExportGraphics menu.
            ToolStripMenuItem exportSVGItem = new ToolStripMenuItem();
            exportSVGItem.ToolTipText = MessageResPathway.MenuToolTipExportSVG;
            exportSVGItem.Text = MessageResPathway.MenuItemExportSVG;
            exportSVGItem.Click += new EventHandler(ExportSVG);

            ToolStripMenuItem exportMenu = new ToolStripMenuItem();
            exportMenu.DropDownItems.AddRange(new ToolStripItem[] { exportSVGItem });
            exportMenu.ToolTipText = MessageResPathway.MenuToolTipExport;
            exportMenu.Text = MessageResPathway.MenuItemExport;
            exportMenu.Name = MenuConstants.MenuItemExport;
            exportMenu.Tag = 17;

            ToolStripMenuItem fileMenu = new ToolStripMenuItem();
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { exportMenu });
            fileMenu.Text = MessageResPathway.MenuItemFile;
            fileMenu.Name = MenuConstants.MenuItemFile;

            menuList.Add(fileMenu);

            // Setup menu
            ToolStripMenuItem setupItem = new ToolStripMenuItem();
            setupItem.ToolTipText = MessageResPathway.MenuToolTipSetup;
            setupItem.Text = MessageResPathway.MenuItemSetup;
            setupItem.Click += new EventHandler(ShowDialogClick);

            ToolStripMenuItem setup = new ToolStripMenuItem();
            setup.DropDownItems.AddRange(new ToolStripItem[] { setupItem });
            setup.Text = MessageResPathway.MenuItemSetup;
            setup.Name = MenuConstants.MenuItemSetup;

            menuList.Add(setup);

            // View menu
            ToolStripMenuItem showIdItem = new ToolStripMenuItem();
            showIdItem.CheckOnClick = true;
            showIdItem.CheckState = CheckState.Checked;
            showIdItem.ToolTipText = MessageResPathway.MenuToolTipShowID;
            showIdItem.Text = MessageResPathway.MenuItemShowID;
            showIdItem.Click += new EventHandler(ShowIdClick);

            ToolStripMenuItem viewModeItem = new ToolStripMenuItem();
            viewModeItem.CheckOnClick = true;
            viewModeItem.CheckState = CheckState.Unchecked;
            viewModeItem.ToolTipText = MessageResPathway.MenuToolTipViewMode;
            viewModeItem.Text = MessageResPathway.MenuItemViewMode;
            viewModeItem.Click += new EventHandler(ViewModeClick);

            ToolStripMenuItem viewMenu = new ToolStripMenuItem();
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { showIdItem, viewModeItem });
            viewMenu.Text = MessageResPathway.MenuItemView;
            viewMenu.Name = MenuConstants.MenuItemView;

            menuList.Add(viewMenu);

            // Edit menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem();
            editMenu.Text = MessageResPathway.MenuItemEdit;
            editMenu.Name = MenuConstants.MenuItemEdit;

            ToolStripMenuItem deleteMenu = new ToolStripMenuItem();
            deleteMenu.Text = MessageResPathway.CanvasMenuDelete;
            deleteMenu.Name = MenuConstants.CanvasMenuDelete;
            deleteMenu.Click += new EventHandler(DeleteClick);
            deleteMenu.ShortcutKeys = Keys.Delete;
            deleteMenu.ShowShortcutKeys = true;

            ToolStripMenuItem cutMenu = new ToolStripMenuItem();
            cutMenu.Text = MessageResPathway.CanvasMenuCut;
            cutMenu.Name = MenuConstants.CanvasMenuCut;
            cutMenu.Click += new EventHandler(CutClick);
            cutMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            cutMenu.ShowShortcutKeys = true;

            ToolStripMenuItem copyMenu = new ToolStripMenuItem();
            copyMenu.Text = MessageResPathway.CanvasMenuCopy;
            copyMenu.Name = MenuConstants.CanvasMenuCopy;
            copyMenu.Click += new EventHandler(CopyClick);
            copyMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            copyMenu.ShowShortcutKeys = true;

            ToolStripMenuItem pasteMenu = new ToolStripMenuItem();
            pasteMenu.Text = MessageResPathway.CanvasMenuPaste;
            pasteMenu.Name = MenuConstants.CanvasMenuPaste;
            pasteMenu.Click += new EventHandler(PasteClick);
            pasteMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            pasteMenu.ShowShortcutKeys = true;

            editMenu.DropDownItems.AddRange(new ToolStripItem[] { cutMenu, copyMenu, pasteMenu, deleteMenu });
            menuList.Add(editMenu);

            // Layout menu
            ToolStripMenuItem layoutMenu = new ToolStripMenuItem();
            layoutMenu.Text = MessageResPathway.MenuItemLayout;
            layoutMenu.Name = MenuConstants.MenuItemLayout;
            layoutMenu.DropDownItems.AddRange(m_menuLayoutList.ToArray());
            menuList.Add(layoutMenu);

            return menuList;
        }

        /// <summary>
        /// Create ToolStripItems.
        /// </summary>
        /// <returns>the list of ToolStripItems.</returns>
        private ToolStrip CreateToolButtons()
        {
            ToolStrip list = new ToolStrip();

            // Used for ID of handle
            int handleCount = 0;

            PathwayToolStripButton handButton = new PathwayToolStripButton();
            handButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            handButton.Name = MenuConstants.ToolButtonMoveCanvas;
            handButton.Image = PathwayResource.move1;
            handButton.CheckOnClick = true;
            handButton.ToolTipText = MessageResPathway.ToolButtonMoveCanvas;
            handButton.Handle = new Handle(Mode.Pan, handleCount, new PPathwayPanEventHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonMoveCanvas, handButton.Handle);
            handButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(handButton);

            PathwayToolStripButton button0 = new PathwayToolStripButton();
            button0.ImageTransparentColor = System.Drawing.Color.Magenta;
            button0.Name = MenuConstants.ToolButtonSelectMode;
            button0.Image = PathwayResource.arrow;
            button0.CheckOnClick = true;
            button0.ToolTipText = MessageResPathway.ToolButtonSelectMode;
            button0.Handle = new Handle(Mode.Select, handleCount, new DefaultMouseHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonSelectMode, button0.Handle);
            button0.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(button0);

            ToolStripSeparator sep1 = new ToolStripSeparator();
            list.Items.Add(sep1);

            PathwayToolStripButton arrowButton = new PathwayToolStripButton();
            arrowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            arrowButton.Name = MenuConstants.ToolButtonAddOnewayReaction;
            arrowButton.Image = PathwayResource.arrow_long_right_w;
            arrowButton.CheckOnClick = true;
            arrowButton.ToolTipText = MessageResPathway.ToolButtonAddOnewayReaction;
            arrowButton.Handle = new Handle(Mode.CreateOneWayReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonAddOnewayReaction, arrowButton.Handle);
            arrowButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(arrowButton);

            PathwayToolStripButton bidirButton = new PathwayToolStripButton();
            bidirButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            bidirButton.Name = MenuConstants.ToolButtonAddMutualReaction;
            bidirButton.Image = PathwayResource.arrow_long_bidir_w;
            bidirButton.CheckOnClick = true;
            bidirButton.ToolTipText = MessageResPathway.ToolButtonAddMutualReaction;
            bidirButton.Handle = new Handle(Mode.CreateMutualReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonAddMutualReaction, bidirButton.Handle);
            bidirButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(bidirButton);

            PathwayToolStripButton constButton = new PathwayToolStripButton();
            constButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            constButton.Name = MenuConstants.ToolButtonAddConstant;
            constButton.Image = PathwayResource.ten;
            constButton.CheckOnClick = true;
            constButton.ToolTipText = MessageResPathway.ToolButtonAddConstant;
            constButton.Handle = new Handle(Mode.CreateConstant, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonAddConstant, constButton.Handle);
            constButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(constButton);

            foreach (ComponentSetting cs in m_con.ComponentManager.ComponentSettings)
            {
                PathwayToolStripButton button = new PathwayToolStripButton();
                button.ImageTransparentColor = Color.Magenta;
                button.ComponentSetting = cs;
                button.Name = cs.Name;
                button.Image = cs.IconImage;
                button.Size = new System.Drawing.Size(32, 32);
                button.CheckOnClick = true;
                if (cs.ComponentType == ComponentType.System)
                {
                    button.Handle = new Handle(Mode.CreateSystem, handleCount++, new CreateSystemMouseHandler(m_con), cs.ComponentType);
                    button.ToolTipText = MessageResPathway.ToolButtonCreateSystem;
                }
                else
                {
                    button.Handle = new Handle(Mode.CreateNode, handleCount++, new CreateNodeMouseHandler(m_con, cs), cs.ComponentType);
                    if (cs.ComponentType == ComponentType.Process)
                        button.ToolTipText = MessageResPathway.ToolButtonCreateProcess;
                    else if(cs.ComponentType == ComponentType.Variable)
                        button.ToolTipText = MessageResPathway.ToolButtonCreateVariable;
                }

                m_handleDict.Add(cs.Name, button.Handle);
                button.Click += new EventHandler(ButtonStateChanged);
                list.Items.Add(button);
            }

            ToolStripSeparator sep2 = new ToolStripSeparator();
            list.Items.Add(sep2);

            ToolStripButton zoominButton = new ToolStripButton();
            zoominButton.ImageTransparentColor = Color.Magenta;
            zoominButton.Name = MenuConstants.ToolButtonZoomIn;
            zoominButton.Image = (Image)TypeDescriptor.GetConverter(
                    PathwayResource.Icon_ZoomIn).ConvertTo(
                        PathwayResource.Icon_ZoomIn,
                        typeof(Image));
            zoominButton.CheckOnClick = false;
            zoominButton.ToolTipText = MessageResPathway.ToolButtonZoomIn;
            zoominButton.Tag = 2f;
            zoominButton.Click += new EventHandler(ZoomButton_Click);
            list.Items.Add(zoominButton);

            ToolStripButton zoomoutButton = new ToolStripButton();
            zoomoutButton.ImageTransparentColor = Color.Magenta;
            zoomoutButton.Name = MenuConstants.ToolButtonZoomOut;
            zoomoutButton.Image = (Image)TypeDescriptor.GetConverter(
                PathwayResource.Icon_ZoomOut).ConvertTo(
                    PathwayResource.Icon_ZoomOut,
                    typeof(Image));
            zoomoutButton.CheckOnClick = false;
            zoomoutButton.ToolTipText = MessageResPathway.ToolButtonZoomOut;
            zoomoutButton.Tag = 0.5f;
            zoomoutButton.Click += new EventHandler(ZoomButton_Click);
            list.Items.Add(zoomoutButton);

            // SelectMode is default.
            button0.Checked = true;
            m_handle = (Handle)button0.Handle;
            m_defHandle = m_handle;

            return list;
        }
        #endregion

        #region Internal Methods
        
        /// <summary>
        /// SetPopupMenus
        /// </summary>
        internal void SetPopupMenus()
        {
            // Set popup menu visibility flags.
            PNode node = m_con.Canvas.FocusNode;
            bool isPPathwayObject = (node is PPathwayObject);
            bool isPPathwayNode = (node is PPathwayNode);
            bool isPPathwaySystem = (node is PPathwaySystem);
            bool isPPathwayText = (node is PPathwayText);
            bool isRoot = false;
            bool isLine = (node is PPathwayLine);
            bool isCopiedObject = (m_con.CopiedNodes.Count > 0);
            bool isLayoutMenu = (m_menuLayoutList.Count > 0);

            // Set popup menu text.
            if (isPPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)node;
                m_popMenuDict[MenuConstants.CanvasMenuID].Text = obj.EcellObject.Key;
                SetLoggerMenu(obj);
                SetLayerManu(obj);
                if (isPPathwaySystem)
                {
                    m_popMenuDict[MenuConstants.CanvasMenuMerge].Text =
                        MessageResPathway.CanvasMenuMerge + "(" + obj.EcellObject.ParentSystemID + ")";
                }
                if (obj.EcellObject.Key.Equals("/"))
                    isRoot = true;
            }
            if (isLine)
            {
                PPathwayLine line = (PPathwayLine)node;
                SetLineMenu(line);
            }

            // Show ObjectID(key).
            m_popMenuDict[MenuConstants.CanvasMenuID].Visible = isPPathwayObject;
            m_popMenuDict[MenuConstants.CanvasMenuSeparator1].Visible = isPPathwayObject;
            // Show Line menus.
            m_popMenuDict[MenuConstants.CanvasMenuRightArrow].Visible = isLine;
            m_popMenuDict[MenuConstants.CanvasMenuLeftArrow].Visible = isLine;
            m_popMenuDict[MenuConstants.CanvasMenuBidirArrow].Visible = isLine;
            m_popMenuDict[MenuConstants.CanvasMenuConstantLine].Visible = isLine;
            m_popMenuDict[MenuConstants.CanvasMenuSeparator3].Visible = isLine;
            // Show Node / System edit menus.
            m_popMenuDict[MenuConstants.CanvasMenuCut].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuCopy].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuPaste].Visible = isCopiedObject;
            m_popMenuDict[MenuConstants.CanvasMenuDelete].Visible = (isPPathwayObject && !isRoot) || isLine || isPPathwayText;
            m_popMenuDict[MenuConstants.CanvasMenuMerge].Visible = isPPathwaySystem && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuSeparator4].Visible = isCopiedObject || (isPPathwayObject && !isRoot);
            // Show Layer menu.
            m_popMenuDict[MenuConstants.CanvasMenuChangeLayer].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuMoveFront].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuMoveBack].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuSeparator5].Visible = isPPathwayObject && !isRoot && !isPPathwayText;
            // Show Layout menus.
            m_popMenuDict[MenuConstants.CanvasMenuLayout].Visible = isLayoutMenu && !(isLine || isPPathwayText);
            m_popMenuDict[MenuConstants.CanvasMenuSeparator2].Visible = isLayoutMenu && !(isLine || isPPathwayText);
            // Show Logger menu.
            m_popMenuDict[MenuConstants.CanvasMenuCreateLogger].Visible = isPPathwayObject && !isPPathwayText;
            m_popMenuDict[MenuConstants.CanvasMenuDeleteLogger].Visible = isPPathwayObject && !isPPathwayText;
        }

        /// <summary>
        /// Set logger menu items.
        /// </summary>
        /// <param name="obj">Selected PPathwayObject.</param>
        private void SetLoggerMenu(PPathwayObject obj)
        {

            ToolStripMenuItem createLogger = (ToolStripMenuItem)m_popMenuDict[MenuConstants.CanvasMenuCreateLogger];
            ToolStripMenuItem deleteLogger = (ToolStripMenuItem)m_popMenuDict[MenuConstants.CanvasMenuDeleteLogger];
            createLogger.DropDown.Items.Clear();
            deleteLogger.DropDown.Items.Clear();

            if (obj.EcellObject == null || obj.EcellObject.ModelID == null)
                return;
            // set logger menu
            EcellObject eo = obj.EcellObject;
            foreach (EcellData data in eo.Value)
            {
                // Create "Create Logger" menu.
                if (!data.Logable)
                    continue;
                ToolStripItem createItem = new ToolStripMenuItem(data.Name);
                createItem.Text = data.Name;
                createItem.Click += new EventHandler(m_con.Menu.CreateLoggerClick);
                createLogger.DropDown.Items.Add(createItem);

                // create "Delete Logger" menu.
                if (!data.Logged)
                    continue;
                ToolStripItem deleteItem = new ToolStripMenuItem(data.Name);
                deleteItem.Text = data.Name;
                deleteItem.Click += new EventHandler(m_con.Menu.DeleteLoggerClick);
                deleteLogger.DropDown.Items.Add(deleteItem);
            }
            createLogger.Enabled = (createLogger.DropDown.Items.Count != 0);
            deleteLogger.Enabled = (deleteLogger.DropDown.Items.Count != 0);
        }

        /// <summary>
        /// Set layer menu items.
        /// </summary>
        /// <param name="obj"></param>
        private void SetLayerManu(PPathwayObject obj)
        {
            ToolStripMenuItem layerMenu = (ToolStripMenuItem)m_popMenuDict[MenuConstants.CanvasMenuChangeLayer];
            layerMenu.DropDown.Items.Clear();
            foreach (string layerName in m_con.Canvas.GetLayerNameList())
            {
                ToolStripMenuItem layerItem = new ToolStripMenuItem(layerName);
                layerItem.Checked = layerName.Equals(obj.Layer.Name);
                layerItem.Click += new EventHandler(m_con.Menu.ChangeLeyerClick);
                layerMenu.DropDown.Items.Add(layerItem);
            }
            ToolStripSeparator separator = new ToolStripSeparator();
            layerMenu.DropDown.Items.Add(separator);

            ToolStripMenuItem createNewLayer = new ToolStripMenuItem(MessageResPathway.LayerMenuCreate);
            createNewLayer.Click += new EventHandler(m_con.Menu.ChangeLeyerClick);
            layerMenu.DropDown.Items.Add(createNewLayer);
        }
        /// <summary>
        /// Set line menu.
        /// </summary>
        /// <param name="line"></param>
        private void SetLineMenu(PPathwayLine line)
        {
            m_popMenuDict[MenuConstants.CanvasMenuRightArrow].Enabled = !(line.Info.Direction == EdgeDirection.Outward);
            m_popMenuDict[MenuConstants.CanvasMenuLeftArrow].Enabled = !(line.Info.Direction == EdgeDirection.Inward);
            m_popMenuDict[MenuConstants.CanvasMenuBidirArrow].Enabled = !(line.Info.Direction == EdgeDirection.Bidirection);
            m_popMenuDict[MenuConstants.CanvasMenuConstantLine].Enabled = !(line.Info.Direction == EdgeDirection.None);
        }

        /// <summary>
        /// Set EventHandler.
        /// </summary>
        internal void SetDefaultEventHandler()
        {
            SetEventHandler(m_defHandle);
        }
        /// <summary>
        /// Set EventHandler.
        /// </summary>
        /// <param name="handle"></param>
        internal void SetEventHandler(Handle handle)
        {
            // Remove old EventHandler
            PBasicInputEventHandler handler = m_handle.EventHandler;
            ((IPathwayEventHandler)handler).Reset();
            RemoveInputEventListener(handler);

            // Set new EventHandler 
            m_handle = handle;
            handler = m_handle.EventHandler;
            foreach (ToolStripItem item in m_buttonList.Items)
            {
                if (!(item is PathwayToolStripButton))
                    continue;
                PathwayToolStripButton button = (PathwayToolStripButton)item;
                if (button.Handle == m_handle)
                    button.Checked = true;
                else
                    button.Checked = false;
            }
            ((IPathwayEventHandler)handler).Initialize();
            AddInputEventListener(handler);
            if (handler is DefaultMouseHandler
                || handler is PPathwayPanEventHandler)
                m_defHandle = handle;

            if (m_con.Canvas == null)
                return;
            m_con.Canvas.LineHandler.SetLineVisibility(false);
        }

        /// <summary>
        /// SetLineHandler
        /// </summary>
        /// <param name="node"></param>
        internal void SetCreateLineHandler(PPathwayNode node)
        {
            Handle handle = m_handleDict[MenuConstants.ToolButtonAddOnewayReaction];
            SetEventHandler(handle);
            CreateReactionMouseHandler handler = (CreateReactionMouseHandler)handle.EventHandler;
            handler.StartNode = node;
        }
        /// <summary>
        /// Add the selected EventHandler to event listener.
        /// </summary>
        /// <param name="handler">added EventHandler.</param>
        private void AddInputEventListener(PBasicInputEventHandler handler)
        {
            // Exception condition 
            if (m_con.Canvas == null)
                return;
            m_con.Canvas.PCanvas.AddInputEventListener(handler);
        }

        /// <summary>
        /// Delete the selected EventHandler from event listener.
        /// </summary>
        /// <param name="handler">deleted EventHandler.</param>
        private void RemoveInputEventListener(PBasicInputEventHandler handler)
        {
            // Exception condition 
            if (m_con.Canvas == null)
                return;

            m_con.Canvas.PCanvas.RemoveInputEventListener(handler);
        }

        #endregion

        #region EventHandlers for MenuClick
#if DEBUG
        /// <summary>
        /// Called when a debug menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugClick(object sender, EventArgs e)
        {
            if (m_con.Canvas.FocusNode is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)m_con.Canvas.FocusNode;
                MessageBox.Show(
                    "Name:" + obj.EcellObject.Key
                    + "\nLayer:" + obj.EcellObject.LayerID
                    + "\nX:" + obj.X + "\nY:" + obj.Y
                    + "\nWidth:" + obj.Width + "\nHeight:" + obj.Height
                    + "\nOffsetX:" + obj.OffsetX + "\nOffsetY:" + obj.OffsetY 
                    + "\nToString():" + obj.ToString());
            }
            else
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                Util.ShowNoticeDialog("X:" + m_con.MousePosition.X + "Y:" + m_con.MousePosition.Y);
            }
        }
#endif
        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MergeClick(object sender, EventArgs e)
        {
            // Check exception.
            CanvasControl canvas = m_con.Canvas;
            if (canvas == null || canvas.SelectedSystem == null)
                return;

            PPathwaySystem system = canvas.SelectedSystem;
            m_con.Window.NotifyDataMerge(system.EcellObject.ModelID, system.EcellObject.Key);
            if (system.IsHighLighted)
                canvas.ResetSelectedSystem();
        }

        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender, EventArgs e)
        {
            // Check active canvas.
            CanvasControl canvas = m_con.Canvas;
            if (canvas == null)
                return;
            // Delete Selected Text
            if (canvas.FocusNode is PPathwayText)
            {
                m_con.NotifyDataDelete(((PPathwayText)canvas.FocusNode).EcellObject, true);
                return;
            }
            // Delete Selected Line
            PPathwayLine line = canvas.LineHandler.SelectedLine;
            if (line != null)
            {
                m_con.NotifyVariableReferenceChanged(
                    line.Info.ProcessKey,
                    line.Info.VariableKey,
                    RefChangeType.Delete,
                    0,
                    true);
                canvas.ResetSelectedLine();
            }
            // Delete Selected Nodes
            if (canvas.SelectedNodes != null)
            {
                List<EcellObject> slist = new List<EcellObject>();
                foreach (PPathwayObject node in canvas.SelectedNodes)
                    slist.Add(node.EcellObject);

                int i = 0;
                foreach (EcellObject deleteNode in slist)
                {
                    i++;
                    bool isAnchor = (i == slist.Count);
                    m_con.NotifyDataDelete(deleteNode, isAnchor);
                }
            }
            // Delete Selected System
            PPathwaySystem system = canvas.SelectedSystem;
            if (system != null)
            {
                // Return if system is null or root.
                if (string.IsNullOrEmpty(system.EcellObject.Key))
                    return;
                // Delete sys.
                m_con.NotifyDataDelete(system.EcellObject, true);
                canvas.ResetSelectedSystem();
            }
        }

        /// <summary>
        /// Called when a create logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateLoggerClick(object sender, EventArgs e)
        {
            if (!(m_con.Canvas.FocusNode is PPathwayObject))
                return;

            PPathwayObject obj = (PPathwayObject)m_con.Canvas.FocusNode;
            string logger = ((ToolStripItem)sender).Text;
            Debug.WriteLine("Create " + obj.EcellObject.Type + " Logger:" + obj.EcellObject.Key);
            // set logger
            m_con.NotifyLoggerChanged(obj, logger, true);
        }

        /// <summary>
        /// Called when a delete logger menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteLoggerClick(object sender, EventArgs e)
        {
            if (!(m_con.Canvas.FocusNode is PPathwayObject))
                return;
            string logger = ((ToolStripItem)sender).Text;

            PPathwayObject obj = (PPathwayObject)m_con.Canvas.FocusNode;
            Debug.WriteLine("Delete " + obj.EcellObject.Type + " Logger:" + obj.EcellObject.Key);
            // delete logger
            m_con.NotifyLoggerChanged(obj, logger, false);
        }

        /// <summary>
        /// Called when one of layout menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutItem_Click(object sender, EventArgs e)
        {
            if (!(sender is PathwayToolStripMenuItem))
                return;
            PathwayToolStripMenuItem item = (PathwayToolStripMenuItem)sender;
            int layoutIdx = item.ID;
            int subIdx = item.SubID;
            ILayoutAlgorithm algorithm = m_layoutList[layoutIdx];
            m_con.DoLayout(algorithm, subIdx, true);
        }

        /// <summary>
        /// Called when a change line menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeLineClick(object sender, EventArgs e)
        {
            // Selected MenuItem.
            if (!(sender is ToolStripItem))
                return;
            ToolStripItem item = (ToolStripItem)sender;
            // Selected Canvas
            CanvasControl canvas = m_con.Canvas;
            if (canvas == null)
                return;
            // Selected Line
            PPathwayLine line = canvas.LineHandler.SelectedLine;
            if (line == null)
                return;
            canvas.ResetSelectedLine();

            // Change edgeInfo.
            RefChangeType changeType = RefChangeType.SingleDir;
            int coefficient = 0;
            if (item.Name == MenuConstants.CanvasMenuRightArrow)
            {
                changeType = RefChangeType.SingleDir;
                coefficient = 1;
            }
            else if (item.Name == MenuConstants.CanvasMenuLeftArrow)
            {
                changeType = RefChangeType.SingleDir;
                coefficient = -1;
            }
            else if (item.Name == MenuConstants.CanvasMenuBidirArrow)
            {
                changeType = RefChangeType.BiDir;
                coefficient = 0;
            }
            else
            {
                changeType = RefChangeType.SingleDir;
                coefficient = 0;
            }
            m_con.NotifyVariableReferenceChanged(
                line.Info.ProcessKey,
                line.Info.VariableKey,
                changeType,
                coefficient,
                true);
        }

        /// <summary>
        /// Change the Layer of Selected Objects.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeLeyerClick(object sender, EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            CanvasControl canvas = m_con.Canvas;
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            // Get new layer name.
            string name;
            if (menu.Text.Equals(MessageResPathway.LayerMenuCreate))
            {
                // Select Layer
                List<string> layerList = canvas.GetLayerNameList();
                string title = MessageResPathway.LayerMenuCreate;
                name = SelectBoxDialog.Show(title, title, layerList);
                if (name == null || name.Equals(""))
                    return;
                if (!canvas.Layers.ContainsKey(name))
                    canvas.AddLayer(name);
            }
            else
            {
                name = menu.Text;
            }

            // Change layer of selected objects.
            PPathwayLayer layer = canvas.Layers[name];
            List<PPathwayObject> objList = canvas.SelectedNodes;
            if (canvas.SelectedSystem != null)
                objList.Add(canvas.SelectedSystem);
            int i = 0;
            foreach (PPathwayObject obj in objList)
            {
                obj.Layer = layer;
                i++;
                m_con.NotifyDataChanged(
                    obj.EcellObject.Key,
                    obj.EcellObject.Key,
                    obj,
                    true,
                    (i == objList.Count));
            }
        }

        /// <summary>
        /// Layer move to front.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveToFrontClick(object sender, EventArgs e)
        {
            CanvasControl canvas = m_con.Canvas;
            PPathwayObject obj = (PPathwayObject)canvas.FocusNode;
            canvas.LayerMoveToFront(obj.Layer);
        }

        /// <summary>
        /// Layer move to back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveToBackClick(object sender, EventArgs e)
        {
            CanvasControl canvas = m_con.Canvas;
            PPathwayObject obj = (PPathwayObject)canvas.FocusNode;
            canvas.LayerMoveToBack(obj.Layer);
        }

        /// <summary>
        /// Called when a copy menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyClick(object sender, EventArgs e)
        {
            m_con.CopyNodes();
        }

        /// <summary>
        /// Called when a cut menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutClick(object sender, EventArgs e)
        {
            m_con.CopyNodes();

            int i = 0;
            bool isAnchor;
            foreach (EcellObject eo in m_con.CopiedNodes)
            {
                i++;
                isAnchor = (i == m_con.CopiedNodes.Count);
                m_con.NotifyDataDelete(eo, isAnchor);
            }

        }

        /// <summary>
        /// Called when a paste menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteClick(object sender, EventArgs e)
        {
            m_con.PasteNodes();
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ShowDialogClick(object sender, EventArgs e)
        {
            PropertyDialog dialog = new PropertyDialog();
            dialog.Text = "PathwaySettings";
            PropertyDialogTabPage componentPage = m_con.ComponentManager.CreateTabPage();
            PropertyDialogTabPage animationPage = m_con.Animation.CreateTabPage();
            dialog.TabControl.Controls.Add(animationPage);
            dialog.TabControl.Controls.Add(componentPage);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                componentPage.ApplyChange();
                animationPage.ApplyChange();
                m_con.ResetObjectSettings();
            }
            dialog.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPropertyDialogClick(object sender, EventArgs e)
        {
            PPathwayObject obj = (PPathwayObject)m_con.Canvas.FocusNode;
            EcellObject eo = m_con.Window.GetEcellObject(obj.EcellObject);
            PropertyEditor.Show(m_con.Window.DataManager, m_con.Window.PluginManager, eo);
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ShowIdClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.CheckState == CheckState.Checked)
                m_con.ShowingID = true;
            else
                m_con.ShowingID = false;
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ViewModeClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            m_con.ViewMode = item.Checked;
        }

        /// <summary>
        /// When select the button in ToolBox,
        /// system change the listener for event
        /// </summary>
        /// <param name="sender">ToolBoxMenuButton.</param>
        /// <param name="e">EventArgs.</param>
        private void ButtonStateChanged(object sender, EventArgs e)
        {
            if (!(sender is PathwayToolStripButton))
                return;
            PathwayToolStripButton selectedButton = (PathwayToolStripButton)sender;
            SetEventHandler(selectedButton.Handle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomButton_Click(object sender, EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            m_con.Canvas.Zoom((float)((ToolStripButton)sender).Tag);
        }
        /// <summary>
        /// Export SVG format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportSVG(object sender, EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "SVG File|*.svg";
            sfd.CheckPathExists = true;
            sfd.CreatePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Save window settings.
                GraphicsExporter.ExportSVG(m_con.Canvas, sfd.FileName);
            }
        }
        #endregion

    }
}
