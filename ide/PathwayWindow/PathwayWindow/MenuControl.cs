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
using EcellLib.PathwayWindow.UIComponent;
using System.ComponentModel;
using EcellLib.PathwayWindow.Resources;
using UMD.HCIL.Piccolo.Event;
using EcellLib.PathwayWindow.Handler;
using System.Drawing;
using System.Drawing.Drawing2D;
using EcellLib.PathwayWindow.Nodes;
using System.Diagnostics;
using EcellLib.PathwayWindow.SVG;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// MenuControl for PathwayWindow
    /// </summary>
    public class MenuControl
    {
        #region Static readonly fields
        #region CanvasPopUpMenu
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for ID
        /// </summary>
        public const string CanvasMenuID = "CanvasMenuID";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for delete
        /// </summary>
        public const string CanvasMenuDelete = "CanvasMenuDelete";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for copy
        /// </summary>
        public const string CanvasMenuCopy = "CanvasMenuCopy";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for cut
        /// </summary>
        public const string CanvasMenuCut = "CanvasMenuCut";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for paste
        /// </summary>
        public const string CanvasMenuPaste = "CanvasMenuPaste";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for delete
        /// </summary>
        public const string CanvasMenuMerge = "CanvasMenuMerge";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Set Layout
        /// </summary>
        public const string CanvasMenuLayout = "CanvasMenuLayout";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Change Layer
        /// </summary>
        public const string CanvasMenuChangeLayer = "LayerMenuChange";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Create Layer
        /// </summary>
        public const string CanvasMenuCreateLayer = "LayerMenuCreate";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Layer Move To Front
        /// </summary>
        public const string CanvasMenuMoveFront = "LayerMenuMoveFront";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Layer Move To Front
        /// </summary>
        public const string CanvasMenuMoveBack = "LayerMenuMoveBack";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for rightArrow
        /// </summary>
        public const string CanvasMenuRightArrow = "CanvasMenuRightArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for leftArrow
        /// </summary>
        public const string CanvasMenuLeftArrow = "CanvasMenuLeftArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for bidirArrow
        /// </summary>
        public const string CanvasMenuBidirArrow = "CanvasMenuBidirArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for constantLine
        /// </summary>
        public const string CanvasMenuConstantLine = "CanvasMenuConstantLine";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Create Logger
        /// </summary>
        public const string CanvasMenuCreateLogger = "CanvasMenuCreateLogger";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for delete Logger
        /// </summary>
        public const string CanvasMenuDeleteLogger = "CanvasMenuDeleteLogger";
        /// <summary>
        /// Key definition of m_cMenuDict for separator1
        /// </summary>
        public const string CanvasMenuSeparator1 = "CanvasMenuSeparator1";
        /// <summary>
        /// Key definition of m_cMenuDict for separator2
        /// </summary>
        public const string CanvasMenuSeparator2 = "CanvasMenuSeparator2";
        /// <summary>
        /// Key definition of m_cMenuDict for separator3
        /// </summary>
        public const string CanvasMenuSeparator3 = "CanvasMenuSeparator3";
        /// <summary>
        /// Key definition of m_cMenuDict for separator4
        /// </summary>
        public const string CanvasMenuSeparator4 = "CanvasMenuSeparator4";
        /// <summary>
        /// Key definition of m_cMenuDict for separator5
        /// </summary>
        public const string CanvasMenuSeparator5 = "CanvasMenuSeparator5";
        #endregion

        #region ToolBarMenu
        /// <summary>
        /// Key definition of MessageResPathway for ShowID
        /// </summary>
        private const string MenuItemSetup = "MenuItemSetup";
        /// <summary>
        /// Key definition of MessageResPathway for ToolTipShowID
        /// </summary>
        private const string MenuToolTipSetup = "MenuToolTipSetup";
        /// <summary>
        /// Key definition of MessageResPathway for ShowID
        /// </summary>
        private const string MenuItemShowID = "MenuItemShowID";
        /// <summary>
        /// Key definition of MessageResPathway for ToolTipShowID
        /// </summary>
        private const string MenuToolTipShowID = "MenuToolTipShowID";
        /// <summary>
        /// Key definition of MessageResPathway for ViewMode
        /// </summary>
        private const string MenuItemViewMode = "MenuItemViewMode";
        /// <summary>
        /// Key definition of MessageResPathway for ToolTipViewMode
        /// </summary>
        private const string MenuToolTipViewMode = "MenuToolTipViewMode";
        /// <summary>
        /// Key definition of MessageResPathway for MenuItemLayout
        /// </summary>
        private const string MenuItemLayout = "MenuItemLayout";
        /// <summary>
        /// Key definition of MessageResPathway for MenuItemLayout
        /// </summary>
        private const string MenuItemEdit = "MenuItemEdit";
        /// <summary>
        /// Key definition of MessageResPathway for MenuItemLayout
        /// </summary>
        private const string MenuItemView = "MenuItemView";
        #endregion

        #region ToolButton
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonAddConstant
        /// </summary>
        private const string ToolButtonAddConstant = "ToolButtonAddConstant";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonAddMutualReaction
        /// </summary>
        private const string ToolButtonAddMutualReaction = "ToolButtonAddMutualReaction";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonAddOnewayReaction
        /// </summary>
        private const string ToolButtonAddOnewayReaction = "ToolButtonAddOnewayReaction";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonCreateProcess
        /// </summary>
        private const string ToolButtonCreateProcess = "ToolButtonCreateProcess";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonCreateSystem
        /// </summary>
        private const string ToolButtonCreateSystem = "ToolButtonCreateSystem";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonCreateVariable
        /// </summary>
        private const string ToolButtonCreateVariable = "ToolButtonCreateVariable";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonMoveCanvas
        /// </summary>
        private const string ToolButtonMoveCanvas = "ToolButtonMoveCanvas";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonSelectMode
        /// </summary>
        private const string ToolButtonSelectMode = "ToolButtonSelectMode";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonZoomIn
        /// </summary>
        private const string ToolButtonZoomIn = "ToolButtonZoomIn";
        /// <summary>
        /// Key definition of MessageResPathway for ToolButtonZoomOut
        /// </summary>
        private const string ToolButtonZoomOut = "ToolButtonZoomOut";
        #endregion
        #endregion
        
        #region Fields
        /// <summary>
        /// The PathwayView, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        private PathwayControl m_con;

        /// <summary>
        /// A list of toolbox buttons.
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
        private List<ToolStripItem> m_buttonList;

        /// <summary>
        /// Dictionary for Eventhandlers.
        /// </summary>
        private Dictionary<string, Handle> m_handleDict = new Dictionary<string, Handle>();

        /// <summary>
        /// A list for layout algorithms, which implement ILayoutAlgorithm.
        /// </summary>
        private List<ILayoutAlgorithm> m_layoutList = new List<ILayoutAlgorithm>();

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_buttonList.
        /// </summary>
        public List<ToolStripItem> ToolButtonList
        {
            get { return m_buttonList; }
            set { m_buttonList = value; }
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
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public MenuControl(PathwayControl control)
        {
            m_con = control;
            m_resources = control.Resources;
            m_layoutList = m_con.Window.GetLayoutAlgorithms();
            m_menuLayoutList = CreateLayoutMenus();
            m_menuList = CreateMenuItems();
            m_buttonList = CreateToolButtonItems();
            m_popupMenu = CreatePopUpMenus();
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
            ToolStripItem idShow = new ToolStripMenuItem(CanvasMenuID);
            idShow.Name = CanvasMenuID;
            nodeMenu.Items.Add(idShow);
            m_popMenuDict.Add(CanvasMenuID, idShow);

            ToolStripSeparator separator1 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator1);
            m_popMenuDict.Add(CanvasMenuSeparator1, separator1);

            // Add Line Changer
            ToolStripItem rightArrow = new ToolStripMenuItem(m_resources.GetString(CanvasMenuRightArrow), PathwayResource.arrow_long_right_w);
            rightArrow.Name = CanvasMenuRightArrow;
            rightArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(rightArrow);
            m_popMenuDict.Add(CanvasMenuRightArrow, rightArrow);

            ToolStripItem leftArrow = new ToolStripMenuItem(m_resources.GetString(CanvasMenuLeftArrow), PathwayResource.arrow_long_left_w);
            leftArrow.Name = CanvasMenuLeftArrow;
            leftArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(leftArrow);
            m_popMenuDict.Add(CanvasMenuLeftArrow, leftArrow);

            ToolStripItem bidirArrow = new ToolStripMenuItem(m_resources.GetString(CanvasMenuBidirArrow), PathwayResource.arrow_long_bidir_w);
            bidirArrow.Name = CanvasMenuBidirArrow;
            bidirArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(bidirArrow);
            m_popMenuDict.Add(CanvasMenuBidirArrow, bidirArrow);

            ToolStripItem constant = new ToolStripMenuItem(m_resources.GetString(CanvasMenuConstantLine), PathwayResource.ten);
            constant.Name = CanvasMenuConstantLine;
            constant.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(constant);
            m_popMenuDict.Add(CanvasMenuConstantLine, constant);

            ToolStripSeparator separator3 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator3);
            m_popMenuDict.Add(CanvasMenuSeparator3, separator3);

            // Add Edit menus
            ToolStripItem cut = new ToolStripMenuItem(m_resources.GetString(CanvasMenuCut));
            cut.Click += new EventHandler(CutClick);
            nodeMenu.Items.Add(cut);
            m_popMenuDict.Add(CanvasMenuCut, cut);

            ToolStripItem copy = new ToolStripMenuItem(m_resources.GetString(CanvasMenuCopy));
            copy.Click += new EventHandler(CopyClick);
            nodeMenu.Items.Add(copy);
            m_popMenuDict.Add(CanvasMenuCopy, copy);

            ToolStripItem paste = new ToolStripMenuItem(m_resources.GetString(CanvasMenuPaste));
            paste.Click += new EventHandler(PasteClick);
            nodeMenu.Items.Add(paste);
            m_popMenuDict.Add(CanvasMenuPaste, paste);

            ToolStripItem delete = new ToolStripMenuItem(m_resources.GetString(CanvasMenuDelete));
            delete.Click += new EventHandler(DeleteClick);
            nodeMenu.Items.Add(delete);
            m_popMenuDict.Add(CanvasMenuDelete, delete);

            ToolStripItem merge = new ToolStripMenuItem(m_resources.GetString(CanvasMenuMerge));
            merge.Click += new EventHandler(MergeClick);
            nodeMenu.Items.Add(merge);
            m_popMenuDict.Add(CanvasMenuMerge, merge);

            ToolStripSeparator separator4 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator4);
            m_popMenuDict.Add(CanvasMenuSeparator4, separator4);

            // Add Layer Menu
            ToolStripItem changeLayer = new ToolStripMenuItem(m_resources.GetString(CanvasMenuChangeLayer));
            //changeLayer.Click += new EventHandler(m_con.ChangeLeyerClick);
            nodeMenu.Items.Add(changeLayer);
            m_popMenuDict.Add(CanvasMenuChangeLayer, changeLayer);

            ToolStripItem moveFront = new ToolStripMenuItem(m_resources.GetString(CanvasMenuMoveFront));
            moveFront.Click += new EventHandler(MoveToFrontClick);
            nodeMenu.Items.Add(moveFront);
            m_popMenuDict.Add(CanvasMenuMoveFront, moveFront);

            ToolStripItem moveBack = new ToolStripMenuItem(m_resources.GetString(CanvasMenuMoveBack));
            moveBack.Click += new EventHandler(MoveToBackClick);
            nodeMenu.Items.Add(moveBack);
            m_popMenuDict.Add(CanvasMenuMoveBack, moveBack);

            ToolStripSeparator separator5 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator5);
            m_popMenuDict.Add(CanvasMenuSeparator5, separator5);

            // Add LayoutMenu
            ToolStripMenuItem layout = new ToolStripMenuItem(m_resources.GetString(CanvasMenuLayout));
            layout.Name = CanvasMenuLayout;
            layout.DropDownItems.AddRange(CreateLayoutMenus().ToArray());
            nodeMenu.Items.Add(layout);
            m_popMenuDict.Add(CanvasMenuLayout, layout);

            ToolStripSeparator separator2 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator2);
            m_popMenuDict.Add(CanvasMenuSeparator2, separator2);

            // Create Logger
            ToolStripMenuItem createLogger = new ToolStripMenuItem(m_resources.GetString(CanvasMenuCreateLogger));
            nodeMenu.Items.Add(createLogger);
            m_popMenuDict.Add(CanvasMenuCreateLogger, createLogger);

            // Delete Logger
            ToolStripMenuItem deleteLogger = new ToolStripMenuItem(m_resources.GetString(CanvasMenuDeleteLogger));
            nodeMenu.Items.Add(deleteLogger);
            m_popMenuDict.Add(CanvasMenuDeleteLogger, deleteLogger);

#if DEBUG
            ToolStripItem debug = new ToolStripMenuItem("Debug");
            debug.Click += new EventHandler(m_con.DebugClick);
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

            // Setup menu
            ToolStripMenuItem setupItem = new ToolStripMenuItem();
            setupItem.ToolTipText = m_resources.GetString(MenuToolTipSetup);
            setupItem.Text = m_resources.GetString(MenuItemSetup);
            setupItem.Click += new EventHandler(ShowDialogClick);

            ToolStripMenuItem setupMenu = new ToolStripMenuItem();
            setupMenu.DropDownItems.AddRange(new ToolStripItem[] { setupItem });
            setupMenu.Text = m_resources.GetString(MenuItemSetup);
            setupMenu.Name = MenuItemSetup;

            menuList.Add(setupMenu);

            // View menu
            ToolStripMenuItem showIdItem = new ToolStripMenuItem();
            showIdItem.CheckOnClick = true;
            showIdItem.CheckState = CheckState.Checked;
            showIdItem.ToolTipText = m_resources.GetString(MenuToolTipShowID);
            showIdItem.Text = m_resources.GetString(MenuItemShowID);
            showIdItem.Click += new EventHandler(ShowIdClick);

            ToolStripMenuItem viewModeItem = new ToolStripMenuItem();
            viewModeItem.CheckOnClick = true;
            viewModeItem.CheckState = CheckState.Unchecked;
            viewModeItem.ToolTipText = m_resources.GetString(MenuToolTipViewMode);
            viewModeItem.Text = m_resources.GetString(MenuItemViewMode);
            viewModeItem.Click += new EventHandler(ViewModeClick);

            ToolStripMenuItem viewMenu = new ToolStripMenuItem();
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { showIdItem, viewModeItem });
            viewMenu.Text = m_resources.GetString(MenuItemView);
            viewMenu.Name = MenuItemView;

            menuList.Add(viewMenu);

            // Edit menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem();
            editMenu.Text = m_resources.GetString(MenuItemEdit);
            editMenu.Name = MenuItemEdit;

            ToolStripMenuItem deleteMenu = new ToolStripMenuItem();
            deleteMenu.Text = m_resources.GetString(CanvasMenuDelete);
            deleteMenu.Name = CanvasMenuDelete;
            deleteMenu.Click += new EventHandler(DeleteClick);
            deleteMenu.ShortcutKeys = Keys.Delete;
            deleteMenu.ShowShortcutKeys = true;

            ToolStripMenuItem cutMenu = new ToolStripMenuItem();
            cutMenu.Text = m_resources.GetString(CanvasMenuCut);
            cutMenu.Name = CanvasMenuCut;
            cutMenu.Click += new EventHandler(CutClick);
            cutMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            cutMenu.ShowShortcutKeys = true;

            ToolStripMenuItem copyMenu = new ToolStripMenuItem();
            copyMenu.Text = m_resources.GetString(CanvasMenuCopy);
            copyMenu.Name = CanvasMenuCopy;
            copyMenu.Click += new EventHandler(CopyClick);
            copyMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            copyMenu.ShowShortcutKeys = true;

            ToolStripMenuItem pasteMenu = new ToolStripMenuItem();
            pasteMenu.Text = m_resources.GetString(CanvasMenuPaste);
            pasteMenu.Name = CanvasMenuPaste;
            pasteMenu.Click += new EventHandler(PasteClick);
            pasteMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            pasteMenu.ShowShortcutKeys = true;

            editMenu.DropDownItems.AddRange(new ToolStripItem[] { cutMenu, copyMenu, pasteMenu, deleteMenu });
            menuList.Add(editMenu);

            // Layout menu
            ToolStripMenuItem layoutMenu = new ToolStripMenuItem();
            layoutMenu.Text = m_resources.GetString(MenuItemLayout);
            layoutMenu.Name = MenuItemLayout;
            layoutMenu.DropDownItems.AddRange(m_menuLayoutList.ToArray());
            menuList.Add(layoutMenu);

            return menuList;
        }

        /// <summary>
        /// Create ToolStripItems.
        /// </summary>
        /// <returns>the list of ToolStripItems.</returns>
        private List<ToolStripItem> CreateToolButtonItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();

            // Used for ID of handle
            int handleCount = 0;

            PathwayToolStripButton handButton = new PathwayToolStripButton();
            handButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            handButton.Name = ToolButtonMoveCanvas;
            handButton.Image = PathwayResource.move1;
            handButton.CheckOnClick = true;
            handButton.ToolTipText = m_resources.GetString(ToolButtonMoveCanvas);
            handButton.Handle = new Handle(Mode.Pan, handleCount, new PPanEventHandler());
            m_handleDict.Add(ToolButtonMoveCanvas, handButton.Handle);
            handButton.Click += new EventHandler(ButtonStateChanged);
            list.Add(handButton);

            PathwayToolStripButton button0 = new PathwayToolStripButton();
            button0.ImageTransparentColor = System.Drawing.Color.Magenta;
            button0.Name = ToolButtonSelectMode;
            button0.Image = PathwayResource.arrow;
            button0.CheckOnClick = true;
            button0.ToolTipText = m_resources.GetString(ToolButtonSelectMode);
            button0.Handle = new Handle(Mode.Select, handleCount, new DefaultMouseHandler(m_con));
            m_handleDict.Add(ToolButtonSelectMode, button0.Handle);
            button0.Click += new EventHandler(ButtonStateChanged);
            list.Add(button0);

            ToolStripSeparator sep = new ToolStripSeparator();
            list.Add(sep);

            PathwayToolStripButton arrowButton = new PathwayToolStripButton();
            arrowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            arrowButton.Name = ToolButtonAddOnewayReaction;
            arrowButton.Image = PathwayResource.arrow_long_right_w;
            arrowButton.CheckOnClick = true;
            arrowButton.ToolTipText = m_resources.GetString(ToolButtonAddOnewayReaction);
            arrowButton.Handle = new Handle(Mode.CreateOneWayReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(ToolButtonAddOnewayReaction, arrowButton.Handle);
            arrowButton.Click += new EventHandler(ButtonStateChanged);
            list.Add(arrowButton);

            PathwayToolStripButton bidirButton = new PathwayToolStripButton();
            bidirButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            bidirButton.Name = ToolButtonAddMutualReaction;
            bidirButton.Image = PathwayResource.arrow_long_bidir_w;
            bidirButton.CheckOnClick = true;
            bidirButton.ToolTipText = m_resources.GetString(ToolButtonAddMutualReaction);
            bidirButton.Handle = new Handle(Mode.CreateMutualReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(ToolButtonAddMutualReaction, bidirButton.Handle);
            bidirButton.Click += new EventHandler(ButtonStateChanged);
            list.Add(bidirButton);

            PathwayToolStripButton constButton = new PathwayToolStripButton();
            constButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            constButton.Name = ToolButtonAddConstant;
            constButton.Image = PathwayResource.ten;
            constButton.CheckOnClick = true;
            constButton.ToolTipText = m_resources.GetString(ToolButtonAddConstant);
            constButton.Handle = new Handle(Mode.CreateConstant, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(ToolButtonAddConstant, constButton.Handle);
            constButton.Click += new EventHandler(ButtonStateChanged);
            list.Add(constButton);

            foreach (ComponentSetting cs in m_con.ComponentManager.ComponentSettings)
            {
                PathwayToolStripButton button = new PathwayToolStripButton();
                button.ImageTransparentColor = System.Drawing.Color.Magenta;
                button.ComponentSetting = cs;
                button.Name = cs.Name;
                button.Image = cs.IconImage;
                button.Size = new System.Drawing.Size(32, 32);
                button.CheckOnClick = true;
                if (cs.ComponentType == ComponentType.System)
                {
                    button.Handle = new Handle(Mode.CreateSystem, handleCount++, new CreateSystemMouseHandler(m_con), cs.ComponentType);
                    button.ToolTipText = m_resources.GetString(ToolButtonCreateSystem);
                }
                else
                {
                    button.Handle = new Handle(Mode.CreateNode, handleCount++, new CreateNodeMouseHandler(m_con, cs), cs.ComponentType);
                    if (cs.ComponentType == ComponentType.Process)
                        button.ToolTipText = m_resources.GetString(ToolButtonCreateProcess);
                    else if(cs.ComponentType == ComponentType.Variable)
                        button.ToolTipText = m_resources.GetString(ToolButtonCreateVariable);
                }

                m_handleDict.Add(cs.Name, button.Handle);
                button.Click += new EventHandler(ButtonStateChanged);
                list.Add(button);
            }

            PathwayToolStripButton zoominButton = new PathwayToolStripButton();
            zoominButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoominButton.Name = ToolButtonZoomIn;
            zoominButton.Image = PathwayResource.zoom_in;
            zoominButton.CheckOnClick = false;
            zoominButton.ToolTipText = m_resources.GetString(ToolButtonZoomIn);
            zoominButton.Handle = new Handle(Mode.CreateConstant, handleCount, 2f);
            zoominButton.Click += new EventHandler(ZoomButton_Click);
            list.Add(zoominButton);

            PathwayToolStripButton zoomoutButton = new PathwayToolStripButton();
            zoomoutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoomoutButton.Name = ToolButtonZoomOut;
            zoomoutButton.Image = PathwayResource.zoom_out;
            zoomoutButton.CheckOnClick = false;
            zoomoutButton.ToolTipText = m_resources.GetString(ToolButtonZoomOut);
            zoomoutButton.Handle = new Handle(Mode.CreateConstant, handleCount, 0.5f);
            zoomoutButton.Click += new EventHandler(ZoomButton_Click);
            list.Add(zoomoutButton);

            // SelectMode is default.
            button0.Checked = true;
            m_con.SelectedHandle = (Handle)button0.Handle;

            return list;
        }
        #endregion

        #region EventHandlers for MenuClick
#if DEBUG
        /// <summary>
        /// Called when a debug menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DebugClick(object sender, EventArgs e)
        {
            if (ActiveCanvas.FocusNode is PPathwayObject)
            {
                PPathwayObject obj = (PPathwayObject)m_con.CanvasControl.FocusNode;
                MessageBox.Show(
                    "Name:" + obj.EcellObject.key
                    + "\nLayer:" + obj.EcellObject.LayerID
                    + "\nX:" + obj.X + "\nY:" + obj.Y
                    + "\nWidth:" + obj.Width + "\nHeight:" + obj.Height
                    + "\nOffsetX:" + obj.OffsetX + "\nOffsetY:" + obj.OffsetY 
                    + "\nToString():" + obj.ToString());
            }
            else
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                MessageBox.Show("X:" + m_mousePos.X + "Y:" + m_mousePos.Y);
            }
        }
#endif
        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MergeClick(object sender, EventArgs e)
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
        public void DeleteClick(object sender, EventArgs e)
        {
            // Check active canvas.
            CanvasControl canvas = m_con.Canvas;
            if (canvas == null)
                return;

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
                if (system.EcellObject.Key.Equals("/"))
                {
                    MessageBox.Show(m_resources.GetString("ErrDelRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
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
        public void CreateLoggerClick(object sender, EventArgs e)
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
        public void DeleteLoggerClick(object sender, EventArgs e)
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
        public void LayoutItem_Click(object sender, EventArgs e)
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
        public void ChangeLineClick(object sender, EventArgs e)
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
            if (item.Name == MenuControl.CanvasMenuRightArrow)
            {
                changeType = RefChangeType.SingleDir;
                coefficient = 1;
            }
            else if (item.Name == MenuControl.CanvasMenuLeftArrow)
            {
                changeType = RefChangeType.SingleDir;
                coefficient = -1;
            }
            else if (item.Name == MenuControl.CanvasMenuBidirArrow)
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
        public void ChangeLeyerClick(object sender, EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            CanvasControl canvas = m_con.Canvas;
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            // Get new layer name.
            string name;
            if (menu.Text.Equals(m_resources.GetString(MenuControl.CanvasMenuCreateLayer)))
            {
                // Select Layer
                List<string> layerList = canvas.GetLayerNameList();
                name = SelectBoxDialog.Show(m_resources.GetString(MenuControl.CanvasMenuCreateLayer), m_resources.GetString(MenuControl.CanvasMenuCreateLayer), layerList);
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
        public void MoveToFrontClick(object sender, EventArgs e)
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
        public void MoveToBackClick(object sender, EventArgs e)
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
        public void CopyClick(object sender, EventArgs e)
        {
            m_con.CopyNodes();
        }

        /// <summary>
        /// Called when a cut menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CutClick(object sender, EventArgs e)
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
        public void PasteClick(object sender, EventArgs e)
        {
            m_con.PasteNodes();
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        public void ShowDialogClick(object sender, EventArgs e)
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
            }
            dialog.Dispose();

            CanvasControl canvas = m_con.Canvas;
            if (canvas == null)
                return;
            canvas.ResetObjectSettings();
            GraphicsExporter.ExportSVG(canvas, Util.GetUserDir() + "/" + canvas.ModelID + ".svg");
            m_con.SetNodeIcons();
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        public void ShowIdClick(object sender, EventArgs e)
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
        public void ViewModeClick(object sender, EventArgs e)
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
        public void ButtonStateChanged(object sender, EventArgs e)
        {
            if (!(sender is PathwayToolStripButton))
                return;
            PathwayToolStripButton selectedButton = (PathwayToolStripButton)sender;
            m_con.SetEventHandler(selectedButton.Handle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ZoomButton_Click(object sender, EventArgs e)
        {
            if (!(sender is PathwayToolStripButton))
                return;
            PathwayToolStripButton button = (PathwayToolStripButton)sender;
            float rate = button.Handle.ZoomingRate;
            if (m_con.Canvas == null)
                return;
            m_con.Canvas.Zoom(rate);
        }
        #endregion

    }
}
