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
    public class MenuControl : Component
    {
        #region Fields
        /// <summary>
        /// The PathwayView, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        private PathwayControl m_con;

        /// <summary>
        /// Indicate which pathway-related toolbar button is selected.
        /// </summary>
        private Handle m_handle;

        /// <summary>
        /// Indicate which pathway-related toolbar button is selected.
        /// </summary>
        private Handle m_defHandle;

        /// <summary>
        /// 
        /// </summary>
        private IContainer components;
        private ToolStrip toolButton;
        private PathwayToolStripButton toolButtonHand;
        private PathwayToolStripButton toolButtonSelect;
        private ToolStripSeparator toolButtonSeparator1;
        private ToolStripButton toolButtonOverview;
        private ToolStripButton toolButtonViewMode;
        private ToolStripButton toolButtonZoomin;
        private ToolStripButton toolButtonZoomout;
        private ToolStripComboBox toolButtonZoomRate;
        private ToolStripSeparator toolButtonSeparator2;
        private PathwayToolStripButton toolButtonArrow;
        private PathwayToolStripButton toolButtonBidirArrow;
        private PathwayToolStripButton toolButtonConst;

        private ToolStrip toolMenu;
        private ToolStripMenuItem toolMenuExport;
        private ToolStripMenuItem toolMenuFile;
        private ToolStripMenuItem toolMenuSetupItem;
        private ToolStripMenuItem toolMenuSetup;
        private ToolStripMenuItem toolMenuFocusMode;
        private ToolStripMenuItem toolMenuShowID;
        private ToolStripMenuItem toolMenuViewMode;
        private ToolStripMenuItem toolMenuView;
        private ToolStripMenuItem toolMenuEdit;
        private ToolStripMenuItem toolMenuDelete;
        private ToolStripMenuItem toolMenuCut;
        private ToolStripMenuItem toolMenuCopy;
        private ToolStripMenuItem toolMenuPaste;

        private ContextMenuStrip popupMenu;
        private ToolStripMenuItem toolStripIdShow;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripAnotherArrow;
        private ToolStripMenuItem toolStripBidirArrow;
        private ToolStripMenuItem toolStripConstant;
        private ToolStripMenuItem toolStripDeleteArrow;
        private ToolStripMenuItem toolStripCut;
        private ToolStripMenuItem toolStripCopy;
        private ToolStripMenuItem toolStripPaste;
        private ToolStripMenuItem toolStripDelete;
        private ToolStripMenuItem toolStripMerge;
        private ToolStripMenuItem toolStripAlias;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem toolStripChangeLayer;
        private ToolStripMenuItem toolStripMoveFront;
        private ToolStripMenuItem toolStripMoveBack;
        private ToolStripSeparator toolStripSeparator3;

        private CommonContextMenu commonMenu;
        #endregion

        #region Accessors
        /// <summary>
        /// Accessor for m_buttonList.
        /// </summary>
        public ToolStrip ToolButtons
        {
            get { return toolButton; }
        }

        /// <summary>
        /// Accessor for m_nodeMenu.
        /// </summary>
        public ContextMenuStrip PopupMenu
        {
            get { return popupMenu; }
        }

        /// <summary>
        /// Accessor for m_menuList.
        /// </summary>
        public List<ToolStripMenuItem> ToolMenuList
        {
            get 
            {
                List<ToolStripMenuItem> menuList = new List<ToolStripMenuItem>();
                menuList.Add(toolMenuFile);
                menuList.Add(toolMenuSetup);
                menuList.Add(toolMenuView);
                menuList.Add(toolMenuEdit);
                return menuList;
            }
        }

        /// <summary>
        /// get/set the number of checked component.
        /// </summary>
        public Handle Handle
        {
            get { return m_handle; }
        }

        /// <summary>
        /// ZoomRate
        /// </summary>
        public ToolStripComboBox ZoomRate
        {
            get { return toolButtonZoomRate; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public MenuControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public MenuControl(PathwayControl control)
        {
            m_con = control;
            InitializeComponent();
            commonMenu.Environment = m_con.Window.Environment;
            popupMenu.Items.AddRange(new ToolStripItem[] {
                commonMenu.addToolStripMenuItem,
                commonMenu.loggingToolStripMenuItem,
                commonMenu.observedToolStripMenuItem,
                commonMenu.parameterToolStripMenuItem});
            CreateToolButtons();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuControl));
            this.popupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripIdShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripAnotherArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripBidirArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripConstant = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDeleteArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMerge = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAlias = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripChangeLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMoveFront = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMoveBack = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButton = new System.Windows.Forms.ToolStrip();
            this.toolButtonHand = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonSelect = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonOverview = new System.Windows.Forms.ToolStripButton();
            this.toolButtonViewMode = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomin = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomout = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomRate = new System.Windows.Forms.ToolStripComboBox();
            this.toolButtonSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonArrow = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonBidirArrow = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonConst = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolMenu = new System.Windows.Forms.ToolStrip();
            this.toolMenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuSetupItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuFocusMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuShowID = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuViewMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.commonMenu = new Ecell.IDE.CommonContextMenu(this.components);
            this.popupMenu.SuspendLayout();
            this.toolButton.SuspendLayout();
            this.toolMenu.SuspendLayout();
            // 
            // popupMenu
            // 
            this.popupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripIdShow,
            this.toolStripSeparator1,
            this.toolStripAnotherArrow,
            this.toolStripBidirArrow,
            this.toolStripConstant,
            this.toolStripDeleteArrow,
            this.toolStripCut,
            this.toolStripCopy,
            this.toolStripPaste,
            this.toolStripDelete,
            this.toolStripMerge,
            this.toolStripAlias,
            this.toolStripSeparator2,
            this.toolStripChangeLayer,
            this.toolStripMoveFront,
            this.toolStripMoveBack,
            this.toolStripSeparator3});
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.Size = new System.Drawing.Size(218, 380);
            // 
            // toolStripIdShow
            // 
            this.toolStripIdShow.Name = "toolStripIdShow";
            this.toolStripIdShow.Size = new System.Drawing.Size(217, 22);
            this.toolStripIdShow.Click += new System.EventHandler(this.ShowPropertyDialogClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(214, 6);
            // 
            // toolStripAnotherArrow
            // 
            this.toolStripAnotherArrow.Name = "toolStripAnotherArrow";
            this.toolStripAnotherArrow.Size = new System.Drawing.Size(217, 22);
            this.toolStripAnotherArrow.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuAnotherArrow;
            this.toolStripAnotherArrow.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripBidirArrow
            // 
            this.toolStripBidirArrow.Name = "toolStripBidirArrow";
            this.toolStripBidirArrow.Size = new System.Drawing.Size(217, 22);
            this.toolStripBidirArrow.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuBidirArrow;
            this.toolStripBidirArrow.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripConstant
            // 
            this.toolStripConstant.Name = "toolStripConstant";
            this.toolStripConstant.Size = new System.Drawing.Size(217, 22);
            this.toolStripConstant.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuConstantLine;
            this.toolStripConstant.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripDeleteArrow
            // 
            this.toolStripDeleteArrow.Name = "toolStripDeleteArrow";
            this.toolStripDeleteArrow.Size = new System.Drawing.Size(217, 22);
            this.toolStripDeleteArrow.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuDelete;
            this.toolStripDeleteArrow.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripCut
            // 
            this.toolStripCut.Name = "toolStripCut";
            this.toolStripCut.Size = new System.Drawing.Size(217, 22);
            this.toolStripCut.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuCut;
            this.toolStripCut.Click += new System.EventHandler(this.CutClick);
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.Size = new System.Drawing.Size(217, 22);
            this.toolStripCopy.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuCopy;
            this.toolStripCopy.Click += new System.EventHandler(this.CopyClick);
            // 
            // toolStripPaste
            // 
            this.toolStripPaste.Name = "toolStripPaste";
            this.toolStripPaste.Size = new System.Drawing.Size(217, 22);
            this.toolStripPaste.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuPaste;
            this.toolStripPaste.Click += new System.EventHandler(this.PasteClick);
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Size = new System.Drawing.Size(217, 22);
            this.toolStripDelete.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuDelete;
            this.toolStripDelete.Click += new System.EventHandler(this.DeleteClick);
            // 
            // toolStripMerge
            // 
            this.toolStripMerge.Name = "toolStripMerge";
            this.toolStripMerge.Size = new System.Drawing.Size(217, 22);
            this.toolStripMerge.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuMerge;
            this.toolStripMerge.Click += new System.EventHandler(this.MergeClick);
            // 
            // toolStripAlias
            // 
            this.toolStripAlias.Name = "toolStripAlias";
            this.toolStripAlias.Size = new System.Drawing.Size(217, 22);
            this.toolStripAlias.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuAlias;
            this.toolStripAlias.Click += new System.EventHandler(this.CreateAliasClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(214, 6);
            // 
            // toolStripChangeLayer
            // 
            this.toolStripChangeLayer.Name = "toolStripChangeLayer";
            this.toolStripChangeLayer.Size = new System.Drawing.Size(217, 22);
            this.toolStripChangeLayer.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuChangeLayer;
            // 
            // toolStripMoveFront
            // 
            this.toolStripMoveFront.Name = "toolStripMoveFront";
            this.toolStripMoveFront.Size = new System.Drawing.Size(217, 22);
            this.toolStripMoveFront.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuMoveFront;
            this.toolStripMoveFront.Click += new System.EventHandler(this.MoveToFrontClick);
            // 
            // toolStripMoveBack
            // 
            this.toolStripMoveBack.Name = "toolStripMoveBack";
            this.toolStripMoveBack.Size = new System.Drawing.Size(217, 22);
            this.toolStripMoveBack.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuMoveBack;
            this.toolStripMoveBack.Click += new System.EventHandler(this.MoveToBackClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // toolButton
            // 
            this.toolButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonHand,
            this.toolButtonSelect,
            this.toolButtonSeparator1,
            this.toolButtonOverview,
            this.toolButtonViewMode,
            this.toolButtonZoomin,
            this.toolButtonZoomout,
            this.toolButtonZoomRate,
            this.toolButtonSeparator2,
            this.toolButtonArrow,
            this.toolButtonBidirArrow,
            this.toolButtonConst});
            this.toolButton.Location = new System.Drawing.Point(0, 0);
            this.toolButton.Name = "toolButton";
            this.toolButton.Size = new System.Drawing.Size(100, 25);
            this.toolButton.TabIndex = 0;
            this.toolButton.Text = "toolStrip1";
            // 
            // toolButtonHand
            // 
            this.toolButtonHand.CheckOnClick = true;
            this.toolButtonHand.ComponentSetting = null;
            this.toolButtonHand.Handle = null;
            this.toolButtonHand.Image = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.move1;
            this.toolButtonHand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonHand.Name = "toolButtonHand";
            this.toolButtonHand.Size = new System.Drawing.Size(23, 22);
            this.toolButtonHand.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ToolButtonMoveCanvas;
            this.toolButtonHand.Click += new System.EventHandler(this.ButtonStateChanged);
            // 
            // toolButtonSelect
            // 
            this.toolButtonSelect.CheckOnClick = true;
            this.toolButtonSelect.ComponentSetting = null;
            this.toolButtonSelect.Handle = null;
            this.toolButtonSelect.Image = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.arrow;
            this.toolButtonSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonSelect.Name = "toolButtonSelect";
            this.toolButtonSelect.Size = new System.Drawing.Size(23, 22);
            this.toolButtonSelect.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ToolButtonSelectMode;
            this.toolButtonSelect.Click += new System.EventHandler(this.ButtonStateChanged);
            // 
            // toolButtonSeparator1
            // 
            this.toolButtonSeparator1.Name = "toolButtonSeparator1";
            this.toolButtonSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolButtonOverview
            // 
            this.toolButtonOverview.Checked = true;
            this.toolButtonOverview.CheckOnClick = true;
            this.toolButtonOverview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolButtonOverview.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonOverview.Image")));
            this.toolButtonOverview.Name = "toolButtonOverview";
            this.toolButtonOverview.Size = new System.Drawing.Size(23, 22);
            this.toolButtonOverview.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipOverview;
            this.toolButtonOverview.Click += new System.EventHandler(this.overviewButton_CheckedChanged);
            // 
            // toolButtonViewMode
            // 
            this.toolButtonViewMode.CheckOnClick = true;
            this.toolButtonViewMode.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonViewMode.Image")));
            this.toolButtonViewMode.Name = "toolButtonViewMode";
            this.toolButtonViewMode.Size = new System.Drawing.Size(23, 20);
            this.toolButtonViewMode.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipViewMode;
            this.toolButtonViewMode.Click += new System.EventHandler(this.ViewModeButtonClick);
            // 
            // toolButtonZoomin
            // 
            this.toolButtonZoomin.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonZoomin.Image")));
            this.toolButtonZoomin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonZoomin.Name = "toolButtonZoomin";
            this.toolButtonZoomin.Size = new System.Drawing.Size(23, 20);
            this.toolButtonZoomin.Tag = true;
            this.toolButtonZoomin.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ToolButtonZoomIn;
            this.toolButtonZoomin.Click += new System.EventHandler(this.ZoomButton_Click);
            // 
            // toolButtonZoomout
            // 
            this.toolButtonZoomout.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonZoomout.Image")));
            this.toolButtonZoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonZoomout.Name = "toolButtonZoomout";
            this.toolButtonZoomout.Size = new System.Drawing.Size(23, 20);
            this.toolButtonZoomout.Tag = false;
            this.toolButtonZoomout.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ToolButtonZoomOut;
            this.toolButtonZoomout.Click += new System.EventHandler(this.ZoomButton_Click);
            // 
            // toolButtonZoomRate
            // 
            this.toolButtonZoomRate.Items.AddRange(new object[] {
            "400%",
            "300%",
            "200%",
            "150%",
            "125%",
            "100%",
            "80%",
            "60%",
            "40%",
            "30%",
            "20%"});
            this.toolButtonZoomRate.MaxLength = 5;
            this.toolButtonZoomRate.Name = "toolButtonZoomRate";
            this.toolButtonZoomRate.Size = new System.Drawing.Size(75, 20);
            this.toolButtonZoomRate.Text = "70%";
            this.toolButtonZoomRate.SelectedIndexChanged += new System.EventHandler(this.ZoomRate_SelectedIndexChanged);
            this.toolButtonZoomRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ZoomRate_KeyDown);
            // 
            // toolButtonSeparator2
            // 
            this.toolButtonSeparator2.Name = "toolButtonSeparator2";
            this.toolButtonSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // toolButtonArrow
            // 
            this.toolButtonArrow.CheckOnClick = true;
            this.toolButtonArrow.ComponentSetting = null;
            this.toolButtonArrow.Handle = null;
            this.toolButtonArrow.Image = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.arrow_long_right_w;
            this.toolButtonArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonArrow.Name = "toolButtonArrow";
            this.toolButtonArrow.Size = new System.Drawing.Size(23, 20);
            this.toolButtonArrow.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ToolButtonAddOnewayReaction;
            this.toolButtonArrow.Click += new System.EventHandler(this.ButtonStateChanged);
            // 
            // toolButtonBidirArrow
            // 
            this.toolButtonBidirArrow.CheckOnClick = true;
            this.toolButtonBidirArrow.ComponentSetting = null;
            this.toolButtonBidirArrow.Handle = null;
            this.toolButtonBidirArrow.Image = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.arrow_long_bidir_w;
            this.toolButtonBidirArrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonBidirArrow.Name = "toolButtonBidirArrow";
            this.toolButtonBidirArrow.Size = new System.Drawing.Size(23, 20);
            this.toolButtonBidirArrow.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ToolButtonAddMutualReaction;
            this.toolButtonBidirArrow.Click += new System.EventHandler(this.ButtonStateChanged);
            // 
            // toolButtonConst
            // 
            this.toolButtonConst.CheckOnClick = true;
            this.toolButtonConst.ComponentSetting = null;
            this.toolButtonConst.Handle = null;
            this.toolButtonConst.Image = global::Ecell.IDE.Plugins.PathwayWindow.PathwayResource.ten;
            this.toolButtonConst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonConst.Name = "toolButtonConst";
            this.toolButtonConst.Size = new System.Drawing.Size(23, 20);
            this.toolButtonConst.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ToolButtonAddConstant;
            this.toolButtonConst.Click += new System.EventHandler(this.ButtonStateChanged);
            // 
            // toolMenu
            // 
            this.toolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuFile,
            this.toolMenuSetup,
            this.toolMenuView,
            this.toolMenuEdit});
            this.toolMenu.Location = new System.Drawing.Point(0, 0);
            this.toolMenu.Name = "toolMenu";
            this.toolMenu.Size = new System.Drawing.Size(100, 25);
            this.toolMenu.TabIndex = 0;
            // 
            // toolMenuFile
            // 
            this.toolMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuExport});
            this.toolMenuFile.Name = "MenuItemFile";
            this.toolMenuFile.Size = new System.Drawing.Size(36, 25);
            this.toolMenuFile.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemFile;
            // 
            // toolMenuExport
            // 
            this.toolMenuExport.Name = "toolMenuExport";
            this.toolMenuExport.Size = new System.Drawing.Size(159, 22);
            this.toolMenuExport.Tag = 17;
            this.toolMenuExport.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemExport;
            this.toolMenuExport.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipExport;
            this.toolMenuExport.Click += new System.EventHandler(this.ExportImage);
            // 
            // toolMenuSetup
            // 
            this.toolMenuSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuSetupItem});
            this.toolMenuSetup.Name = "MenuItemSetup";
            this.toolMenuSetup.Size = new System.Drawing.Size(110, 25);
            this.toolMenuSetup.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemSetup;
            // 
            // toolMenuSetupItem
            // 
            this.toolMenuSetupItem.Name = "toolMenuSetupItem";
            this.toolMenuSetupItem.Size = new System.Drawing.Size(163, 22);
            this.toolMenuSetupItem.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemSetup;
            this.toolMenuSetupItem.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipSetup;
            this.toolMenuSetupItem.Click += new System.EventHandler(this.ShowDialogClick);
            // 
            // toolMenuView
            // 
            this.toolMenuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuFocusMode,
            this.toolMenuShowID,
            this.toolMenuViewMode});
            this.toolMenuView.Name = "MenuItemView";
            this.toolMenuView.Size = new System.Drawing.Size(42, 25);
            this.toolMenuView.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemView;
            // 
            // toolMenuFocusMode
            // 
            this.toolMenuFocusMode.Checked = true;
            this.toolMenuFocusMode.CheckOnClick = true;
            this.toolMenuFocusMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolMenuFocusMode.Name = "toolMenuFocusMode";
            this.toolMenuFocusMode.Size = new System.Drawing.Size(169, 22);
            this.toolMenuFocusMode.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemFocus;
            this.toolMenuFocusMode.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipFocus;
            this.toolMenuFocusMode.Click += new System.EventHandler(this.ChangeFocusMode);
            // 
            // toolMenuShowID
            // 
            this.toolMenuShowID.Checked = true;
            this.toolMenuShowID.CheckOnClick = true;
            this.toolMenuShowID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolMenuShowID.Name = "toolMenuShowID";
            this.toolMenuShowID.Size = new System.Drawing.Size(169, 22);
            this.toolMenuShowID.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemShowID;
            this.toolMenuShowID.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipShowID;
            this.toolMenuShowID.Click += new System.EventHandler(this.ShowIdClick);
            // 
            // toolMenuViewMode
            // 
            this.toolMenuViewMode.CheckOnClick = true;
            this.toolMenuViewMode.Name = "toolMenuViewMode";
            this.toolMenuViewMode.Size = new System.Drawing.Size(169, 22);
            this.toolMenuViewMode.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemViewMode;
            this.toolMenuViewMode.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipViewMode;
            this.toolMenuViewMode.Click += new System.EventHandler(this.ViewModeItemClick);
            // 
            // toolMenuEdit
            // 
            this.toolMenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuCut,
            this.toolMenuCopy,
            this.toolMenuPaste,
            this.toolMenuDelete});
            this.toolMenuEdit.Name = "MenuItemEdit";
            this.toolMenuEdit.Size = new System.Drawing.Size(37, 25);
            this.toolMenuEdit.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemEdit;
            // 
            // toolMenuCut
            // 
            this.toolMenuCut.Name = "toolMenuCut";
            this.toolMenuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolMenuCut.Size = new System.Drawing.Size(137, 22);
            this.toolMenuCut.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuCut;
            this.toolMenuCut.Click += new System.EventHandler(this.CutClick);
            // 
            // toolMenuCopy
            // 
            this.toolMenuCopy.Name = "toolMenuCopy";
            this.toolMenuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolMenuCopy.Size = new System.Drawing.Size(137, 22);
            this.toolMenuCopy.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuCopy;
            this.toolMenuCopy.Click += new System.EventHandler(this.CopyClick);
            // 
            // toolMenuPaste
            // 
            this.toolMenuPaste.Name = "toolMenuPaste";
            this.toolMenuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolMenuPaste.Size = new System.Drawing.Size(137, 22);
            this.toolMenuPaste.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuPaste;
            this.toolMenuPaste.Click += new System.EventHandler(this.PasteClick);
            // 
            // toolMenuDelete
            // 
            this.toolMenuDelete.Name = "toolMenuDelete";
            this.toolMenuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolMenuDelete.Size = new System.Drawing.Size(137, 22);
            this.toolMenuDelete.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuDelete;
            this.toolMenuDelete.Click += new System.EventHandler(this.DeleteClick);
            // 
            // commonMenu
            // 
            this.commonMenu.Environment = null;
            this.commonMenu.Object = null;
            this.popupMenu.ResumeLayout(false);
            this.toolButton.ResumeLayout(false);
            this.toolButton.PerformLayout();
            this.toolMenu.ResumeLayout(false);
            this.toolMenu.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Create ToolStripItems.
        /// </summary>
        /// <returns>the list of ToolStripItems.</returns>
        private void CreateToolButtons()
        {
            // Used for ID of handle
            toolButtonHand.Handle = new Handle(Mode.Pan, new PPathwayPanEventHandler(m_con));
            toolButtonSelect.Handle = new Handle(Mode.Select, new DefaultMouseHandler(m_con));
            toolButtonArrow.Handle = new Handle(Mode.CreateOneWayReaction, new CreateReactionMouseHandler(m_con));
            toolButtonBidirArrow.Handle = new Handle(Mode.CreateMutualReaction, new CreateReactionMouseHandler(m_con));
            toolButtonConst.Handle = new Handle(Mode.CreateConstant, new CreateReactionMouseHandler(m_con));

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
                    button.Handle = new Handle(Mode.CreateSystem, new CreateSystemMouseHandler(m_con), cs.ComponentType);
                    button.ToolTipText = MessageResources.ToolButtonCreateSystem;
                }
                else
                {
                    button.Handle = new Handle(Mode.CreateNode, new CreateNodeMouseHandler(m_con, cs), cs.ComponentType);
                    if (cs.ComponentType == ComponentType.Process)
                        button.ToolTipText = MessageResources.ToolButtonCreateProcess;
                    else if (cs.ComponentType == ComponentType.Variable)
                        button.ToolTipText = MessageResources.ToolButtonCreateVariable;
                    else if (cs.ComponentType == ComponentType.Text)
                        button.ToolTipText = MessageResources.ToolButtonCreateText;
                }

                button.Click += new EventHandler(ButtonStateChanged);
                toolButton.Items.Add(button);
            }

            // SelectMode is default.
            toolButtonSelect.Checked = true;
            m_handle = toolButtonSelect.Handle;
            m_defHandle = m_handle;
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
                EcellObject obj = ((PPathwayObject)node).EcellObject;
                commonMenu.Object = obj;
                toolStripIdShow.Text = obj.Key;
                SetLayerManu(obj);
                if (isPPathwaySystem)
                {
                    toolStripMerge.Text =
                        MessageResources.CanvasMenuMerge + "(" + obj.ParentSystemID + ")";
                }
                if (obj.Key.Equals("/"))
                    isRoot = true;
            }
            if (isLine)
            {
                PPathwayLine line = (PPathwayLine)node;
                SetLineMenu(line);
            }
            // Show ObjectID(key).
            toolStripIdShow.Visible = isPPathwayObject;
            toolStripSeparator1.Visible = isPPathwayObject;
            // Show Line menus.
            toolStripAnotherArrow.Visible = isLine;
            toolStripBidirArrow.Visible = isLine;
            toolStripConstant.Visible = isLine;
            toolStripDeleteArrow.Visible = isLine;
            // Show Node / System edit menus.
            toolStripCut.Visible = isPPathwayObject && !isRoot;
            toolStripCopy.Visible = isPPathwayObject && !isRoot;
            toolStripPaste.Visible = isCopiedObject && isInsideRoot;
            toolStripDelete.Visible = (isPPathwayObject && !isRoot) || isPPathwayText;
            toolStripMerge.Visible = isPPathwaySystem && !isRoot;
            toolStripAlias.Visible = isPPathwayVariable;
            toolStripSeparator2.Visible = isPPathwayObject && !isRoot;
            // Show Layer menu.
            toolStripChangeLayer.Visible = isPPathwayObject && !isRoot;
            toolStripMoveFront.Visible = isPPathwayObject && !isRoot;
            toolStripMoveBack.Visible = isPPathwayObject && !isRoot;
            toolStripSeparator3.Visible = isPPathwayObject && !isRoot && !isPPathwayText;
            // Show Logger menu.
            commonMenu.addToolStripMenuItem.Visible = isPPathwaySystem;
            commonMenu.loggingToolStripMenuItem.Visible = isPPathwayObject;
            commonMenu.observedToolStripMenuItem.Visible = isPPathwayObject;
            commonMenu.parameterToolStripMenuItem.Visible = isPPathwayObject;
        }

        /// <summary>
        /// Set layer menu items.
        /// </summary>
        /// <param name="obj"></param>
        private void SetLayerManu(EcellObject obj)
        {
            ToolStripMenuItem layerMenu = toolStripChangeLayer;
            layerMenu.DropDown.Items.Clear();
            foreach (string layerName in m_con.Canvas.GetLayerNameList())
            {
                ToolStripMenuItem layerItem = new ToolStripMenuItem(layerName);
                layerItem.Checked = layerName.Equals(obj.Layer);
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
                toolStripAnotherArrow.Enabled = false;
                toolStripBidirArrow.Enabled = false;
                toolStripConstant.Enabled = true;
            }
            else if (line.Info.Direction == EdgeDirection.None)
            {
                toolStripAnotherArrow.Enabled = false;
                toolStripBidirArrow.Enabled = true;
                toolStripConstant.Enabled = false;
            }
            else
            {
                toolStripAnotherArrow.Enabled = true;
                toolStripBidirArrow.Enabled = true;
                toolStripConstant.Enabled = true;
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
            foreach (ToolStripItem item in toolButton.Items)
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
            Handle handle = toolButtonArrow.Handle;
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
            if (canvas == null || commonMenu.Object == null)
                return;

            EcellObject system = commonMenu.Object;
            m_con.Window.NotifyDataMerge(system.ModelID, system.Key);
            canvas.NotifyResetSelect();
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
            int i = 0;
            foreach (PPathwayObject obj in objList)
            {
                obj.Layer = layer;
                i++;
                m_con.NotifyDataChanged(
                    obj,
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
                PropertyDialogTabPage pathwayPage = m_con.Animation.PathwayDialogTabPage;
                PropertyDialogTabPage animationPage = m_con.Animation.AnimationDialogTabPage;
                PropertyDialogTabPage componentPage = m_con.ComponentManager.DialogTabPage;
                dialog.TabControl.Controls.Add(pathwayPage);
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
            SetViewMode(toolMenuViewMode.Checked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewModeButtonClick(object sender, EventArgs e)
        {
            SetViewMode(toolButtonViewMode.Checked);
        }

        private void SetViewMode(bool viewMode)
        {
            toolButtonViewMode.Checked = viewMode;
            toolMenuViewMode.Checked = viewMode;
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
                zoomRate = float.Parse(toolButtonZoomRate.Text.Replace("%", "")) / m_con.Canvas.PCanvas.Camera.ViewScale / 100f;
            }
            catch
            {
                zoomRate = m_con.Canvas.PCanvas.Camera.ViewScale * 100f;
                toolButtonZoomRate.Text = zoomRate.ToString("###") + "%";
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
            string rate = toolButtonZoomRate.Items[toolButtonZoomRate.SelectedIndex].ToString().Replace("%","");
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
            toolButtonZoomRate.Text = rate.ToString("###") + "%";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Canvas_OnKeyPress(object sender, KeyPressEventArgs e)
        {
            string key = "OnKeyPress = " + e.KeyChar + "\n" + (Control.ModifierKeys == Keys.ControlKey);

            MessageBox.Show(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (!m_con.Canvas.PCanvas.Focused)
                return;
            float moveLength = 10;
            if (Control.ModifierKeys == Keys.Control)
                moveLength = 1;
            PointF delta = new PointF();
            if (e.KeyCode == Keys.Left)
                delta.X = delta.X - moveLength;
            if (e.KeyCode == Keys.Right)
                delta.X = delta.X + moveLength;
            if (e.KeyCode == Keys.Up)
                delta.Y = delta.Y - moveLength;
            if (e.KeyCode == Keys.Down)
                delta.Y = delta.Y + moveLength;


            if (delta == PointF.Empty)
                return;
            foreach (PPathwayObject obj in m_con.Canvas.SelectedNodes)
            {
                obj.MovePosition(delta);
            }
            m_con.Canvas.NotifyMoveObjects();
        }

        #endregion

    }
}
