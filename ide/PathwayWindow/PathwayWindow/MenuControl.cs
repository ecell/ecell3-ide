﻿//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.IDE.Plugins.PathwayWindow.Dialog;
using Ecell.Objects;
using System.Drawing.Imaging;
using System.IO;

namespace Ecell.IDE.Plugins.PathwayWindow
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
        /// 
        /// </summary>
        private ToolStripComboBox m_zoomRate;

        private ToolStripMenuItem viewModeItem;
        private ToolStripButton viewModeButton;
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
        /// ZoomRate
        /// </summary>
        public ToolStripComboBox ZoomRate
        {
            get { return m_zoomRate; }
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
            m_menuList = CreateToolMenus();
            m_buttonList = CreateToolButtons();
            m_popupMenu = CreatePopUpMenus();
        }
        #endregion

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
            ToolStripItem anotherArrow = new ToolStripMenuItem(MessageResources.CanvasMenuAnotherArrow);
            anotherArrow.Name = MenuConstants.CanvasMenuAnotherArrow;
            anotherArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(anotherArrow);
            m_popMenuDict.Add(MenuConstants.CanvasMenuAnotherArrow, anotherArrow);


            ToolStripItem bidirArrow = new ToolStripMenuItem(MessageResources.CanvasMenuBidirArrow);
            bidirArrow.Name = MenuConstants.CanvasMenuBidirArrow;
            bidirArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(bidirArrow);
            m_popMenuDict.Add(MenuConstants.CanvasMenuBidirArrow, bidirArrow);

            ToolStripItem constant = new ToolStripMenuItem(MessageResources.CanvasMenuConstantLine);
            constant.Name = MenuConstants.CanvasMenuConstantLine;
            constant.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(constant);
            m_popMenuDict.Add(MenuConstants.CanvasMenuConstantLine, constant);

            ToolStripItem deleteArrow = new ToolStripMenuItem(MessageResources.CanvasMenuDelete);
            deleteArrow.Name = MenuConstants.CanvasMenuDeleteArrow;
            deleteArrow.Click += new EventHandler(ChangeLineClick);
            nodeMenu.Items.Add(deleteArrow);
            m_popMenuDict.Add(MenuConstants.CanvasMenuDeleteArrow, deleteArrow);

            // Add Edit menus
            ToolStripItem cut = new ToolStripMenuItem(MessageResources.CanvasMenuCut);
            cut.Click += new EventHandler(CutClick);
            nodeMenu.Items.Add(cut);
            m_popMenuDict.Add(MenuConstants.CanvasMenuCut, cut);

            ToolStripItem copy = new ToolStripMenuItem(MessageResources.CanvasMenuCopy);
            copy.Click += new EventHandler(CopyClick);
            nodeMenu.Items.Add(copy);
            m_popMenuDict.Add(MenuConstants.CanvasMenuCopy, copy);

            ToolStripItem paste = new ToolStripMenuItem(MessageResources.CanvasMenuPaste);
            paste.Click += new EventHandler(PasteClick);
            nodeMenu.Items.Add(paste);
            m_popMenuDict.Add(MenuConstants.CanvasMenuPaste, paste);

            ToolStripItem delete = new ToolStripMenuItem(MessageResources.CanvasMenuDelete);
            delete.Click += new EventHandler(DeleteClick);
            nodeMenu.Items.Add(delete);
            m_popMenuDict.Add(MenuConstants.CanvasMenuDelete, delete);

            ToolStripItem merge = new ToolStripMenuItem(MessageResources.CanvasMenuMerge);
            merge.Click += new EventHandler(MergeClick);
            nodeMenu.Items.Add(merge);
            m_popMenuDict.Add(MenuConstants.CanvasMenuMerge, merge);

            ToolStripItem alias = new ToolStripMenuItem(MessageResources.CanvasMenuAlias);
            alias.Click += new EventHandler(CreateAliasClick);
            nodeMenu.Items.Add(alias);
            m_popMenuDict.Add(MenuConstants.CanvasMenuAlias, alias);

            ToolStripSeparator separator4 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator4);
            m_popMenuDict.Add(MenuConstants.CanvasMenuSeparator4, separator4);

            // Add Layer Menu
            ToolStripItem changeLayer = new ToolStripMenuItem(MessageResources.CanvasMenuChangeLayer);
            //changeLayer.Click += new EventHandler(m_con.ChangeLeyerClick);
            nodeMenu.Items.Add(changeLayer);
            m_popMenuDict.Add(MenuConstants.CanvasMenuChangeLayer, changeLayer);

            ToolStripItem moveFront = new ToolStripMenuItem(MessageResources.LayerMenuMoveFront);
            moveFront.Click += new EventHandler(MoveToFrontClick);
            nodeMenu.Items.Add(moveFront);
            m_popMenuDict.Add(MenuConstants.CanvasMenuMoveFront, moveFront);

            ToolStripItem moveBack = new ToolStripMenuItem(MessageResources.LayerMenuMoveBack);
            moveBack.Click += new EventHandler(MoveToBackClick);
            nodeMenu.Items.Add(moveBack);
            m_popMenuDict.Add(MenuConstants.CanvasMenuMoveBack, moveBack);

            ToolStripSeparator separator5 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator5);
            m_popMenuDict.Add(MenuConstants.CanvasMenuSeparator5, separator5);

            // Create Logger
            ToolStripMenuItem createLogger = new ToolStripMenuItem(MessageResources.CanvasMenuCreateLogger);
            nodeMenu.Items.Add(createLogger);
            m_popMenuDict.Add(MenuConstants.CanvasMenuCreateLogger, createLogger);

            // Delete Logger
            ToolStripMenuItem deleteLogger = new ToolStripMenuItem(MessageResources.CanvasMenuDeleteLogger);
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
        private List<ToolStripMenuItem> CreateToolMenus()
        {
            List<ToolStripMenuItem> menuList = new List<ToolStripMenuItem>();

            ToolStripMenuItem exportMenu = new ToolStripMenuItem();
            exportMenu.ToolTipText = MessageResources.MenuToolTipExport;
            exportMenu.Text = MessageResources.MenuItemExport;
            exportMenu.Name = MenuConstants.MenuItemExport;
            exportMenu.Click += new EventHandler(ExportImage);
            exportMenu.Tag = 17;

            ToolStripMenuItem fileMenu = new ToolStripMenuItem();
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { exportMenu });
            fileMenu.Text = MessageResources.MenuItemFile;
            fileMenu.Name = MenuConstants.MenuItemFile;

            menuList.Add(fileMenu);

            // Setup menu
            ToolStripMenuItem setupItem = new ToolStripMenuItem();
            setupItem.ToolTipText = MessageResources.MenuToolTipSetup;
            setupItem.Text = MessageResources.MenuItemSetup;
            setupItem.Click += new EventHandler(ShowDialogClick);

            ToolStripMenuItem setup = new ToolStripMenuItem();
            setup.DropDownItems.AddRange(new ToolStripItem[] { setupItem });
            setup.Text = MessageResources.MenuItemSetup;
            setup.Name = MenuConstants.MenuItemSetup;

            menuList.Add(setup);

            // View menu
            ToolStripMenuItem focusModeItem = new ToolStripMenuItem();
            focusModeItem.CheckOnClick = true;
            focusModeItem.CheckState = CheckState.Checked;
            focusModeItem.ToolTipText = MessageResources.MenuToolTipFocus;
            focusModeItem.Text = MessageResources.MenuItemFocus;
            focusModeItem.Click += new EventHandler(ChangeFocusMode);

            ToolStripMenuItem showIdItem = new ToolStripMenuItem();
            showIdItem.CheckOnClick = true;
            showIdItem.CheckState = CheckState.Checked;
            showIdItem.ToolTipText = MessageResources.MenuToolTipShowID;
            showIdItem.Text = MessageResources.MenuItemShowID;
            showIdItem.Click += new EventHandler(ShowIdClick);

            viewModeItem = new ToolStripMenuItem();
            viewModeItem.CheckOnClick = true;
            viewModeItem.CheckState = CheckState.Unchecked;
            viewModeItem.ToolTipText = MessageResources.MenuToolTipViewMode;
            viewModeItem.Text = MessageResources.MenuItemViewMode;
            viewModeItem.Click += new EventHandler(ViewModeItemClick);

            ToolStripMenuItem viewMenu = new ToolStripMenuItem();
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { focusModeItem, showIdItem, viewModeItem });
            viewMenu.Text = MessageResources.MenuItemView;
            viewMenu.Name = MenuConstants.MenuItemView;

            menuList.Add(viewMenu);

            // Edit menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem();
            editMenu.Text = MessageResources.MenuItemEdit;
            editMenu.Name = MenuConstants.MenuItemEdit;

            ToolStripMenuItem deleteMenu = new ToolStripMenuItem();
            deleteMenu.Text = MessageResources.CanvasMenuDelete;
            deleteMenu.Name = MenuConstants.CanvasMenuDelete;
            deleteMenu.Click += new EventHandler(DeleteClick);
            deleteMenu.ShortcutKeys = Keys.Delete;
            deleteMenu.ShowShortcutKeys = true;

            ToolStripMenuItem cutMenu = new ToolStripMenuItem();
            cutMenu.Text = MessageResources.CanvasMenuCut;
            cutMenu.Name = MenuConstants.CanvasMenuCut;
            cutMenu.Click += new EventHandler(CutClick);
            cutMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            cutMenu.ShowShortcutKeys = true;

            ToolStripMenuItem copyMenu = new ToolStripMenuItem();
            copyMenu.Text = MessageResources.CanvasMenuCopy;
            copyMenu.Name = MenuConstants.CanvasMenuCopy;
            copyMenu.Click += new EventHandler(CopyClick);
            copyMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            copyMenu.ShowShortcutKeys = true;

            ToolStripMenuItem pasteMenu = new ToolStripMenuItem();
            pasteMenu.Text = MessageResources.CanvasMenuPaste;
            pasteMenu.Name = MenuConstants.CanvasMenuPaste;
            pasteMenu.Click += new EventHandler(PasteClick);
            pasteMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            pasteMenu.ShowShortcutKeys = true;

            editMenu.DropDownItems.AddRange(new ToolStripItem[] { cutMenu, copyMenu, pasteMenu, deleteMenu });
            menuList.Add(editMenu);

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
            handButton.ToolTipText = MessageResources.ToolButtonMoveCanvas;
            handButton.Handle = new Handle(Mode.Pan, handleCount, new PPathwayPanEventHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonMoveCanvas, handButton.Handle);
            handButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(handButton);

            PathwayToolStripButton selectButton = new PathwayToolStripButton();
            selectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            selectButton.Name = MenuConstants.ToolButtonSelectMode;
            selectButton.Image = PathwayResource.arrow;
            selectButton.CheckOnClick = true;
            selectButton.ToolTipText = MessageResources.ToolButtonSelectMode;
            selectButton.Handle = new Handle(Mode.Select, handleCount, new DefaultMouseHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonSelectMode, selectButton.Handle);
            selectButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(selectButton);

            ToolStripSeparator sep1 = new ToolStripSeparator();
            list.Items.Add(sep1);

            PathwayToolStripButton arrowButton = new PathwayToolStripButton();
            arrowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            arrowButton.Name = MenuConstants.ToolButtonAddOnewayReaction;
            arrowButton.Image = PathwayResource.arrow_long_right_w;
            arrowButton.CheckOnClick = true;
            arrowButton.ToolTipText = MessageResources.ToolButtonAddOnewayReaction;
            arrowButton.Handle = new Handle(Mode.CreateOneWayReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonAddOnewayReaction, arrowButton.Handle);
            arrowButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(arrowButton);

            PathwayToolStripButton bidirButton = new PathwayToolStripButton();
            bidirButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            bidirButton.Name = MenuConstants.ToolButtonAddMutualReaction;
            bidirButton.Image = PathwayResource.arrow_long_bidir_w;
            bidirButton.CheckOnClick = true;
            bidirButton.ToolTipText = MessageResources.ToolButtonAddMutualReaction;
            bidirButton.Handle = new Handle(Mode.CreateMutualReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add(MenuConstants.ToolButtonAddMutualReaction, bidirButton.Handle);
            bidirButton.Click += new EventHandler(ButtonStateChanged);
            list.Items.Add(bidirButton);

            PathwayToolStripButton constButton = new PathwayToolStripButton();
            constButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            constButton.Name = MenuConstants.ToolButtonAddConstant;
            constButton.Image = PathwayResource.ten;
            constButton.CheckOnClick = true;
            constButton.ToolTipText = MessageResources.ToolButtonAddConstant;
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
                    button.ToolTipText = MessageResources.ToolButtonCreateSystem;
                }
                else
                {
                    button.Handle = new Handle(Mode.CreateNode, handleCount++, new CreateNodeMouseHandler(m_con, cs), cs.ComponentType);
                    if (cs.ComponentType == ComponentType.Process)
                        button.ToolTipText = MessageResources.ToolButtonCreateProcess;
                    else if (cs.ComponentType == ComponentType.Variable)
                        button.ToolTipText = MessageResources.ToolButtonCreateVariable;
                    else if (cs.ComponentType == ComponentType.Text)
                        button.ToolTipText = MessageResources.ToolButtonCreateText;
                }

                m_handleDict.Add(cs.Name, button.Handle);
                button.Click += new EventHandler(ButtonStateChanged);
                list.Items.Add(button);
            }

            ToolStripSeparator sep2 = new ToolStripSeparator();
            list.Items.Add(sep2);

            ToolStripButton overviewButton = new ToolStripButton();
            overviewButton.Name = MenuConstants.ToolButtonOverview;
            overviewButton.ToolTipText = MessageResources.MenuToolTipOverview;
            overviewButton.CheckOnClick = true;
            overviewButton.CheckState = CheckState.Checked;
            overviewButton.Click += new EventHandler(overviewButton_CheckedChanged);
            overviewButton.Image = (Image)TypeDescriptor.GetConverter(
                    PathwayResource.Icon_PathwayView).ConvertTo(
                        PathwayResource.Icon_OverView,
                        typeof(Image));
            list.Items.Add(overviewButton);

            viewModeButton = new ToolStripButton();
            viewModeButton.Name = MenuConstants.ToolButtonViewMode;
            viewModeButton.ToolTipText = MessageResources.MenuToolTipViewMode;
            viewModeButton.CheckOnClick = true;
            viewModeButton.Click += new EventHandler(ViewModeButtonClick);
            viewModeButton.Image = (Image)TypeDescriptor.GetConverter(
                    PathwayResource.Icon_PathwayView).ConvertTo(
                        PathwayResource.Icon_PathwayView,
                        typeof(Image));
            list.Items.Add(viewModeButton);

            ToolStripButton zoominButton = new ToolStripButton();
            zoominButton.ImageTransparentColor = Color.Magenta;
            zoominButton.Name = MenuConstants.ToolButtonZoomIn;
            zoominButton.Image = (Image)TypeDescriptor.GetConverter(
                    PathwayResource.Icon_ZoomIn).ConvertTo(
                        PathwayResource.Icon_ZoomIn,
                        typeof(Image));
            zoominButton.CheckOnClick = false;
            zoominButton.ToolTipText = MessageResources.ToolButtonZoomIn;
            zoominButton.Tag = true;
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
            zoomoutButton.ToolTipText = MessageResources.ToolButtonZoomOut;
            zoomoutButton.Tag = false;
            zoomoutButton.Click += new EventHandler(ZoomButton_Click);
            list.Items.Add(zoomoutButton);

            m_zoomRate = new ToolStripComboBox();
            object[] arr = { "400%", "300%", "200%", "150%", "125%", "100%", "80%", "60%", "40%", "30%", "20%" };
            m_zoomRate.Text = "70%";
            m_zoomRate.MaxLength = 5;
            m_zoomRate.Width = 20;
            m_zoomRate.ComboBox.Width = 15;
            m_zoomRate.Items.AddRange(arr);
            m_zoomRate.KeyDown += new KeyEventHandler(ZoomRate_KeyDown);
            m_zoomRate.SelectedIndexChanged += new EventHandler(ZoomRate_SelectedIndexChanged);
            list.Items.Add(m_zoomRate);
            // SelectMode is default.
            selectButton.Checked = true;
            m_handle = (Handle)selectButton.Handle;
            m_defHandle = m_handle;

            return list;
        }

        #region Internal Methods
        
        /// <summary>
        /// SetPopupMenus
        /// </summary>
        internal void SetPopupMenus()
        {
            // Set popup menu visibility flags.
            PNode node = m_con.Canvas.FocusNode;
            bool isNull = (node == null);
            bool isPPathwayObject = (node is PPathwayObject);
            bool isPPathwayNode = (node is PPathwayNode);
            bool isPPathwayVariable = false;// (node is PPathwayVariable);
            bool isPPathwayAlias = (node is PPathwayAlias);
            bool isPPathwaySystem = (node is PPathwaySystem);
            bool isPPathwayText = (node is PPathwayText);
            bool isRoot = false;
            bool isLine = (node is PPathwayLine);
            bool isCopiedObject = (m_con.CopiedNodes.Count > 0);
            bool isInsideRoot = m_con.Canvas.Systems["/"].Rect.Contains(m_con.MousePosition);
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
                        MessageResources.CanvasMenuMerge + "(" + obj.EcellObject.ParentSystemID + ")";
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
            m_popMenuDict[MenuConstants.CanvasMenuAnotherArrow].Visible = isLine;
            m_popMenuDict[MenuConstants.CanvasMenuBidirArrow].Visible = isLine;
            m_popMenuDict[MenuConstants.CanvasMenuConstantLine].Visible = isLine;
            m_popMenuDict[MenuConstants.CanvasMenuDeleteArrow].Visible = isLine;
            // Show Node / System edit menus.
            m_popMenuDict[MenuConstants.CanvasMenuCut].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuCopy].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuPaste].Visible = isCopiedObject && isInsideRoot;
            m_popMenuDict[MenuConstants.CanvasMenuDelete].Visible = (isPPathwayObject && !isRoot) || isPPathwayText;
            m_popMenuDict[MenuConstants.CanvasMenuMerge].Visible = isPPathwaySystem && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuAlias].Visible = isPPathwayVariable;
            m_popMenuDict[MenuConstants.CanvasMenuSeparator4].Visible = isPPathwayObject && !isRoot;
            // Show Layer menu.
            m_popMenuDict[MenuConstants.CanvasMenuChangeLayer].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuMoveFront].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuMoveBack].Visible = isPPathwayObject && !isRoot;
            m_popMenuDict[MenuConstants.CanvasMenuSeparator5].Visible = isPPathwayObject && !isRoot && !isPPathwayText;
            // Show Logger menu.
            m_popMenuDict[MenuConstants.CanvasMenuCreateLogger].Visible = isPPathwayObject && !isPPathwayText;
            m_popMenuDict[MenuConstants.CanvasMenuDeleteLogger].Visible = false; //isPPathwayObject && !isPPathwayText;
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
                layerItem.Enabled = !layerItem.Checked;
                layerItem.Click += new EventHandler(m_con.Menu.ChangeLeyerClick);
                layerMenu.DropDown.Items.Add(layerItem);
            }
            //ToolStripSeparator separator = new ToolStripSeparator();
            //layerMenu.DropDown.Items.Add(separator);

            //ToolStripMenuItem createNewLayer = new ToolStripMenuItem(MessageResources.LayerMenuCreate);
            //createNewLayer.Click += new EventHandler(m_con.Menu.ChangeLeyerClick);
            //layerMenu.DropDown.Items.Add(createNewLayer);
        }
        /// <summary>
        /// Set line menu.
        /// </summary>
        /// <param name="line"></param>
        private void SetLineMenu(PPathwayLine line)
        {
            if (line.Info.Direction == EdgeDirection.Bidirection)
            {
                m_popMenuDict[MenuConstants.CanvasMenuAnotherArrow].Enabled = false;
                m_popMenuDict[MenuConstants.CanvasMenuBidirArrow].Enabled = false;
                m_popMenuDict[MenuConstants.CanvasMenuConstantLine].Enabled = true;
            }
            else if (line.Info.Direction == EdgeDirection.None)
            {
                m_popMenuDict[MenuConstants.CanvasMenuAnotherArrow].Enabled = false;
                m_popMenuDict[MenuConstants.CanvasMenuBidirArrow].Enabled = true;
                m_popMenuDict[MenuConstants.CanvasMenuConstantLine].Enabled = false;
            }
            else
            {
                m_popMenuDict[MenuConstants.CanvasMenuAnotherArrow].Enabled = true;
                m_popMenuDict[MenuConstants.CanvasMenuBidirArrow].Enabled = true;
                m_popMenuDict[MenuConstants.CanvasMenuConstantLine].Enabled = true;
            }
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

            if (m_con.Canvas == null)
                return;

            ((IPathwayEventHandler)handler).Initialize();
            AddInputEventListener(handler);
            if (handler is DefaultMouseHandler
                || handler is PPathwayPanEventHandler)
                m_defHandle = handle;
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

        private void CreateAliasClick(object sender, EventArgs e)
        {
            // Check active canvas.
            CanvasControl canvas = m_con.Canvas;
            PPathwayVariable var = (PPathwayVariable)canvas.FocusNode;
            EcellObject eo = var.EcellObject;
            EcellObject alias = EcellObject.CreateObject(eo.ModelID, eo.Key, eo.Type, eo.Classname, null);
            alias.SetPosition(eo);
            alias.PointF = m_con.MousePosition;
            eo.Children.Add(alias);
            m_con.NotifyDataChanged(eo.Key, eo, true, true);
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

            if (!m_con.Canvas.PCanvas.Focused)
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
                    line.Info.Coefficient,
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

            // Delete old edge.
            m_con.NotifyVariableReferenceChanged(
                line.Info.ProcessKey,
                line.Info.VariableKey,
                RefChangeType.Delete,
                0,
                false);
            if (item.Name == MenuConstants.CanvasMenuDeleteArrow)
                return;
            // Create new edgeInfo.
            RefChangeType changeType = RefChangeType.SingleDir;
            int coefficient = 0;
            if (item.Name == MenuConstants.CanvasMenuAnotherArrow)
            {
                changeType = RefChangeType.SingleDir;
                if (line.Info.Direction == EdgeDirection.Inward)
                    coefficient = 1;
                else
                    coefficient = -1;
            }
            //if (item.Name == MenuConstants.CanvasMenuRightArrow)
            //{
            //    changeType = RefChangeType.SingleDir;
            //    coefficient = 1;
            //}
            //else if (item.Name == MenuConstants.CanvasMenuLeftArrow)
            //{
            //    changeType = RefChangeType.SingleDir;
            //    coefficient = -1;
            //}
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
            PPathwayObject node = (PPathwayObject)canvas.FocusNode;
            string name = node.EcellObject.Layer;
            if (menu.Text.Equals(MessageResources.LayerMenuCreate))
            {
                // Select Layer
                List<string> layerList = canvas.GetLayerNameList();
                string title = MessageResources.LayerMenuCreate;
                name = SelectBoxDialog.Show(title, title, name, layerList);
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
            canvas.OverviewCanvas.Refresh();
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
            canvas.OverviewCanvas.Refresh();
        }

        /// <summary>
        /// Called when a copy menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyClick(object sender, EventArgs e)
        {
            if (!m_con.Canvas.PCanvas.Focused)
                return;
            m_con.CopyNodes();
        }

        /// <summary>
        /// Called when a cut menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutClick(object sender, EventArgs e)
        {
            if (!m_con.Canvas.PCanvas.Focused)
                return;
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
            if (!m_con.Canvas.PCanvas.Focused)
                return;
            try
            {
                m_con.PasteNodes();
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog(ex.Message);
            }
        }

        /// <summary>
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ShowDialogClick(object sender, EventArgs e)
        {
            PropertyDialog dialog = new PropertyDialog();
            using (dialog)
            {
                dialog.Text = MessageResources.DialogTextPathwaySetting;
                PropertyDialogTabPage componentPage = m_con.ComponentManager.CreateTabPage();
                PropertyDialogTabPage animationPage = m_con.Animation.CreateTabPage();
                dialog.TabControl.Controls.Add(animationPage);
                dialog.TabControl.Controls.Add(componentPage);
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                componentPage.ApplyChange();
                animationPage.ApplyChange();
                m_con.ResetObjectSettings();
            }
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

        private void ChangeFocusMode(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.CheckState == CheckState.Checked)
                m_con.FocusMode = true;
            else
                m_con.FocusMode = false;
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
        /// the event sequence of clicking the menu of [View]->[Show Id]
        /// </summary>
        /// <param name="sender">MenuStripItem.</param>
        /// <param name="e">EventArgs.</param>
        private void ViewModeItemClick(object sender, EventArgs e)
        {
            SetViewMode(viewModeItem.Checked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewModeButtonClick(object sender, EventArgs e)
        {
            SetViewMode(viewModeButton.Checked);
        }

        private void SetViewMode(bool viewMode)
        {
            viewModeButton.Checked = viewMode;
            viewModeItem.Checked = viewMode;
            m_con.ViewMode = viewMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void overviewButton_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripButton item = (ToolStripButton)sender;
            m_con.PathwayView.OverviewVisibility = item.Checked;
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
            float current = m_con.Canvas.PCanvas.Camera.ViewScale * 100f;
            bool extend = (bool)((ToolStripButton)sender).Tag;
            float zoomRate = GetZoomRate(current, extend);
            Zoom(zoomRate);
        }

        private float GetZoomRate(float current, bool extend)
        {
            float newRate = current;
            if (extend)
            {
                foreach (object obj in ZoomRate.Items)
                {
                    string str = (string)obj;
                    float f = float.Parse(str.Replace("%",""));
                    if(f > current+1)
                        newRate = f;
                }
            }
            else
            {
                foreach (object obj in ZoomRate.Items)
                {
                    string str = (string)obj;
                    float f = float.Parse(str.Replace("%", ""));
                    if (f < current-1)
                    {
                        newRate = f;
                        break;
                    }
                }
            }
            return newRate / current;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            if (e.KeyCode != Keys.Enter)
                return;
            float zoomRate;
            try
            {
                zoomRate = float.Parse(m_zoomRate.Text.Replace("%", "")) / m_con.Canvas.PCanvas.Camera.ViewScale / 100f;
            }
            catch
            {
                zoomRate = m_con.Canvas.PCanvas.Camera.ViewScale * 100f;
                m_zoomRate.Text = zoomRate.ToString("###") + "%";
                return;
            }
            Zoom(zoomRate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ZoomRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            string rate = m_zoomRate.Items[m_zoomRate.SelectedIndex].ToString().Replace("%","");
            float zoomRate = float.Parse(rate) / m_con.Canvas.PCanvas.Camera.ViewScale / 100f;
            Zoom(zoomRate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoomRate"></param>
        public void Zoom(float zoomRate)
        {
            if (m_con.Canvas == null)
                return;
            
            m_con.Canvas.Zoom(zoomRate);
            float rate = m_con.Canvas.PCanvas.Camera.ViewScale * 100f;
            m_zoomRate.Text = rate.ToString("###") + "%";
        }

        /// <summary>
        /// ExportImage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportImage(object sender, EventArgs e)
        {
            if (m_con.Canvas == null)
                return;
            SaveFileDialog sfd = new SaveFileDialog();
            using (sfd)
            {
                sfd.Filter = "SVG File|*.svg|" + Constants.FilterImageFile;
                sfd.CheckPathExists = true;
                sfd.FileName = m_con.Canvas.ModelID;
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                if (string.IsNullOrEmpty(sfd.FileName))
                    return;

                // save SVG image
                if (sfd.FileName.EndsWith(Constants.FileExtSVG))
                {
                    GraphicsExporter.ExportSVG(m_con.Canvas, sfd.FileName);
                    return;
                }

                // Save Other format.
                Image image = m_con.Canvas.ToImage();
                ImageFormat format = GetImageFormat(sfd.FileName);
                image.Save(sfd.FileName, format);
            }
        }

        private static ImageFormat GetImageFormat(string filename)
        {
            string ext = Path.GetExtension(filename);
            if (ext == Constants.FileExtBMP)
                return ImageFormat.Bmp;
            else if (ext == Constants.FileExtPNG)
                return ImageFormat.Png;
            else if (ext == Constants.FileExtJPG)
                return ImageFormat.Jpeg;
            else if (ext == Constants.FileExtGIF)
                return ImageFormat.Gif;
            else
                return ImageFormat.Bmp;
        }
        #endregion
        #region EventHandler for PPathwayCanvas

        /// <summary>
        /// Event on mose wheel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            CanvasControl canvas = m_con.Canvas;
            if (canvas == null)
                return;

            if (Control.ModifierKeys == Keys.Shift)
            {
                canvas.PanCanvas(Direction.Horizontal, e.Delta);
            }
            else if (Control.ModifierKeys == Keys.Control || e.Button == MouseButtons.Right)
            {
                float zoomRate = (float)1.00 + (float)e.Delta / 1200;
                Zoom(zoomRate);
            }
            else
            {
                canvas.PanCanvas(Direction.Vertical, e.Delta);
            }
        }

        /// <summary>
        /// Event on mouse down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Canvas_MouseDown(object sender, MouseEventArgs e)
        {

        }

        internal void Canvas_KeyPress(object sender, KeyPressEventArgs e)
        {
            CanvasControl canvas = m_con.Canvas;
            if (canvas == null)
                return;

            if (e.KeyChar == (char)Keys.Right)
            {
            }
            else if (e.KeyChar == (char)Keys.Left)
            {
            }
            else if (e.KeyChar == (char)Keys.Up)
            {
            }
            else if (e.KeyChar == (char)Keys.Down)
            {
            }
        }
        #endregion
    }
}
