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
using System.Drawing.Imaging;
using System.IO;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using Ecell.Objects;
using Ecell.Plugin;
using Ecell.IDE.Plugins.PathwayWindow.Handler;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Graphic;
using Ecell.IDE.Plugins.PathwayWindow.Components;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;

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
        /// 
        /// </summary>
        private IContainer components;
        private ToolStrip toolButton;
        private PathwayToolStripButton toolButtonHand;
        private PathwayToolStripButton toolButtonSelect;
        private ToolStripSeparator toolButtonSeparator1;
        private ToolStripButton toolButtonOverview;
        private ToolStripButton toolButtonAnimation;
        private ToolStripButton toolButtonZoomin;
        private ToolStripButton toolButtonZoomout;
        private ToolStripComboBox toolButtonZoomRate;
        private ToolStripSeparator toolButtonSeparator2;
        private PathwayToolStripButton toolButtonArrow;
        private PathwayToolStripButton toolButtonBidirArrow;
        private PathwayToolStripButton toolButtonConst;

        private ToolStrip toolMenu;
        private ToolStripMenuItem toolMenuExport;
        private ToolStripMenuItem MenuItemFile;
        private ToolStripMenuItem toolMenuFocusMode;
        private ToolStripMenuItem toolMenuShowID;
        private ToolStripMenuItem toolMenuAnimation;
        private ToolStripMenuItem MenuItemView;
        private ToolStripMenuItem MenuItemEdit;
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
        private ToolStripMenuItem toolStripAlias;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem toolStripChangeLayer;
        private ToolStripMenuItem toolStripTextAlign;
        private ToolStripMenuItem toolStripAlignLeft;
        private ToolStripMenuItem toolStripAlignCenter;
        private ToolStripMenuItem toolStripAlignRight;
        private ToolStripMenuItem toolStripMoveFront;
        private ToolStripMenuItem toolStripMoveBack;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripOneWayArrow;
        private ToolStripMenuItem toolStripProperty;

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
                menuList.Add(MenuItemFile);
                menuList.Add(MenuItemView);
                menuList.Add(MenuItemEdit);
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
            : this()
        {
            m_con = control;
            m_con.ProjectStatusChange += new EventHandler(OnProjectStatusChange);
            commonMenu.Environment = m_con.Window.Environment;
            CreateToolButtons();
        }

        /// <summary>
        /// Set Menu abailability.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnProjectStatusChange(object sender, EventArgs e)
        {
            bool menuFlag = m_con.ProjectStatus == ProjectStatus.Loaded;

            toolMenuExport.Enabled = menuFlag;

            toolMenuCut.Enabled = menuFlag;
            toolMenuCopy.Enabled = menuFlag;
            toolMenuPaste.Enabled = menuFlag;
            toolMenuDelete.Enabled = menuFlag;

            toolButtonAnimation.Enabled = menuFlag;
            toolMenuAnimation.Enabled = menuFlag;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuControl));
            this.popupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripIdShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripOneWayArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAnotherArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripBidirArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripConstant = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDeleteArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.commonMenu = new Ecell.IDE.CommonContextMenu();
            this.toolStripCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAlias = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripChangeLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextAlign = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAlignLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAlignCenter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAlignRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMoveFront = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMoveBack = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripProperty = new System.Windows.Forms.ToolStripMenuItem();
            this.toolButton = new System.Windows.Forms.ToolStrip();
            this.toolButtonHand = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonSelect = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonOverview = new System.Windows.Forms.ToolStripButton();
            this.toolButtonAnimation = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomin = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomout = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomRate = new System.Windows.Forms.ToolStripComboBox();
            this.toolButtonSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonArrow = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonBidirArrow = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolButtonConst = new Ecell.IDE.Plugins.PathwayWindow.UIComponent.PathwayToolStripButton();
            this.toolMenu = new System.Windows.Forms.ToolStrip();
            this.MenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuFocusMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuShowID = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuAnimation = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.popupMenu.SuspendLayout();
            this.toolButton.SuspendLayout();
            this.toolMenu.SuspendLayout();
            // 
            // popupMenu
            // 
            this.popupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripIdShow,
            this.toolStripSeparator1,
            this.toolStripOneWayArrow,
            this.toolStripAnotherArrow,
            this.toolStripBidirArrow,
            this.toolStripConstant,
            this.toolStripDeleteArrow,
            this.commonMenu.addToolStripMenuItem,
            this.toolStripCut,
            this.toolStripCopy,
            this.toolStripPaste,
            this.toolStripDelete,
            this.commonMenu.mergeSystemToolStripMenuItem,
            this.toolStripAlias,
            this.toolStripSeparator2,
            this.toolStripTextAlign,
            this.toolStripChangeLayer,
            this.toolStripMoveFront,
            this.toolStripMoveBack,
            this.toolStripSeparator3,
            this.commonMenu.loggingToolStripMenuItem,
            this.commonMenu.observedToolStripMenuItem,
            this.commonMenu.parameterToolStripMenuItem,
            this.commonMenu.propertyToolStripMenuItem});
            this.popupMenu.Name = "popupMenu";
            this.popupMenu.Size = new System.Drawing.Size(272, 462);
            // 
            // toolStripIdShow
            // 
            this.toolStripIdShow.Name = "toolStripIdShow";
            this.toolStripIdShow.Size = new System.Drawing.Size(271, 22);
            this.toolStripIdShow.Enabled = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(268, 6);
            // 
            // toolStripOneWayArrow
            // 
            this.toolStripOneWayArrow.Name = "toolStripOneWayArrow";
            this.toolStripOneWayArrow.Size = new System.Drawing.Size(271, 22);
            this.toolStripOneWayArrow.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuOnewayArrow;
            this.toolStripOneWayArrow.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripAnotherArrow
            // 
            this.toolStripAnotherArrow.Name = "toolStripAnotherArrow";
            this.toolStripAnotherArrow.Size = new System.Drawing.Size(271, 22);
            this.toolStripAnotherArrow.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuAnotherArrow;
            this.toolStripAnotherArrow.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripBidirArrow
            // 
            this.toolStripBidirArrow.Name = "toolStripBidirArrow";
            this.toolStripBidirArrow.Size = new System.Drawing.Size(271, 22);
            this.toolStripBidirArrow.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuBidirArrow;
            this.toolStripBidirArrow.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripConstant
            // 
            this.toolStripConstant.Name = "toolStripConstant";
            this.toolStripConstant.Size = new System.Drawing.Size(271, 22);
            this.toolStripConstant.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuConstantLine;
            this.toolStripConstant.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // toolStripDeleteArrow
            // 
            this.toolStripDeleteArrow.Name = "toolStripDeleteArrow";
            this.toolStripDeleteArrow.Size = new System.Drawing.Size(271, 22);
            this.toolStripDeleteArrow.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuDelete;
            this.toolStripDeleteArrow.Click += new System.EventHandler(this.ChangeLineClick);
            // 
            // commonMenu
            // 
            this.commonMenu.Environment = null;
            this.commonMenu.Object = null;
            // 
            // toolStripCut
            // 
            this.toolStripCut.Name = "toolStripCut";
            this.toolStripCut.Size = new System.Drawing.Size(271, 22);
            this.toolStripCut.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuCut;
            this.toolStripCut.Click += new System.EventHandler(this.CutClick);
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.Size = new System.Drawing.Size(271, 22);
            this.toolStripCopy.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuCopy;
            this.toolStripCopy.Click += new System.EventHandler(this.CopyClick);
            // 
            // toolStripPaste
            // 
            this.toolStripPaste.Name = "toolStripPaste";
            this.toolStripPaste.Size = new System.Drawing.Size(271, 22);
            this.toolStripPaste.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuPaste;
            this.toolStripPaste.Click += new System.EventHandler(this.PasteClick);
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Size = new System.Drawing.Size(271, 22);
            this.toolStripDelete.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuDelete;
            this.toolStripDelete.Click += new System.EventHandler(this.DeleteClick);
            // 
            // toolStripAlias
            // 
            this.toolStripAlias.Name = "toolStripAlias";
            this.toolStripAlias.Size = new System.Drawing.Size(271, 22);
            this.toolStripAlias.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuAlias;
            this.toolStripAlias.Click += new System.EventHandler(this.CreateAliasClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(268, 6);
            // 
            // toolStripChangeLayer
            // 
            this.toolStripChangeLayer.Name = "toolStripChangeLayer";
            this.toolStripChangeLayer.Size = new System.Drawing.Size(271, 22);
            this.toolStripChangeLayer.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuChangeLayer;
            // 
            // toolStripTextAlign
            // 
            this.toolStripTextAlign.Name = "toolStripTextAlign";
            this.toolStripTextAlign.Size = new System.Drawing.Size(271, 22);
            this.toolStripTextAlign.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuTextAlign;
            this.toolStripTextAlign.DropDown.Items.AddRange(new ToolStripItem[] {
                this.toolStripAlignLeft,
                this.toolStripAlignCenter,
                this.toolStripAlignRight});
            // 
            // toolStripAlignLeft
            // 
            this.toolStripAlignLeft.Name = "toolStripAlignLeft";
            this.toolStripAlignLeft.Size = new System.Drawing.Size(271, 22);
            this.toolStripAlignLeft.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuAlignLeft;
            this.toolStripAlignLeft.Tag = StringAlignment.Near;
            this.toolStripAlignLeft.Click += new EventHandler(TextAlign_Click);
            // 
            // toolStripAlignCenter
            // 
            this.toolStripAlignCenter.Name = "toolStripAlignCenter";
            this.toolStripAlignCenter.Size = new System.Drawing.Size(271, 22);
            this.toolStripAlignCenter.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuAlignCenter;
            this.toolStripAlignCenter.Tag = StringAlignment.Center;
            this.toolStripAlignCenter.Click += new EventHandler(TextAlign_Click);
            // 
            // toolStripAlignRight
            // 
            this.toolStripAlignRight.Name = "toolStripAlignRight";
            this.toolStripAlignRight.Size = new System.Drawing.Size(271, 22);
            this.toolStripAlignRight.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.CanvasMenuAlignRight;
            this.toolStripAlignRight.Tag = StringAlignment.Far;
            this.toolStripAlignRight.Click += new EventHandler(TextAlign_Click);
            // 
            // toolStripMoveFront
            // 
            this.toolStripMoveFront.Name = "toolStripMoveFront";
            this.toolStripMoveFront.Size = new System.Drawing.Size(271, 22);
            this.toolStripMoveFront.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuMoveFront;
            this.toolStripMoveFront.Click += new System.EventHandler(this.MoveToFrontClick);
            // 
            // toolStripMoveBack
            // 
            this.toolStripMoveBack.Name = "toolStripMoveBack";
            this.toolStripMoveBack.Size = new System.Drawing.Size(271, 22);
            this.toolStripMoveBack.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.LayerMenuMoveBack;
            this.toolStripMoveBack.Click += new System.EventHandler(this.MoveToBackClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(268, 6);
            // 
            // toolStripProperty
            // 
            this.toolStripProperty.Name = "toolStripProperty";
            this.toolStripProperty.Size = new System.Drawing.Size(182, 22);
            this.toolStripProperty.Text = "toolStripMenuItem1";
            // 
            // toolButton
            // 
            this.toolButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonHand,
            this.toolButtonSelect,
            this.toolButtonSeparator1,
            this.toolButtonOverview,
            this.toolButtonAnimation,
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
            this.toolButtonHand.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipMoveCanvas;
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
            this.toolButtonSelect.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipSelectMode;
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
            this.toolButtonOverview.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipOverview;
            this.toolButtonOverview.Click += new System.EventHandler(this.overviewButton_CheckedChanged);
            // 
            // toolButtonViewMode
            // 
            this.toolButtonAnimation.Checked = true;
            this.toolButtonAnimation.CheckOnClick = true;
            this.toolButtonAnimation.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonViewMode.Image")));
            this.toolButtonAnimation.Name = "toolButtonViewMode";
            this.toolButtonAnimation.Size = new System.Drawing.Size(23, 20);
            this.toolButtonAnimation.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipAnimation;
            this.toolButtonAnimation.Click += new System.EventHandler(this.ViewModeButtonClick);
            // 
            // toolButtonZoomin
            // 
            this.toolButtonZoomin.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonZoomin.Image")));
            this.toolButtonZoomin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonZoomin.Name = "toolButtonZoomin";
            this.toolButtonZoomin.Size = new System.Drawing.Size(23, 20);
            this.toolButtonZoomin.Tag = true;
            this.toolButtonZoomin.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipZoomIn;
            this.toolButtonZoomin.Click += new System.EventHandler(this.ZoomButton_Click);
            // 
            // toolButtonZoomout
            // 
            this.toolButtonZoomout.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonZoomout.Image")));
            this.toolButtonZoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonZoomout.Name = "toolButtonZoomout";
            this.toolButtonZoomout.Size = new System.Drawing.Size(23, 20);
            this.toolButtonZoomout.Tag = false;
            this.toolButtonZoomout.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipZoomOut;
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
            this.toolButtonArrow.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipAddOnewayReaction;
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
            this.toolButtonBidirArrow.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipAddMutualReaction;
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
            this.toolButtonConst.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.ButtonToolTipAddConstant;
            this.toolButtonConst.Click += new System.EventHandler(this.ButtonStateChanged);
            // 
            // toolMenu
            // 
            this.toolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemFile,
            this.MenuItemView,
            this.MenuItemEdit});
            this.toolMenu.Location = new System.Drawing.Point(0, 0);
            this.toolMenu.Name = "toolMenu";
            this.toolMenu.Size = new System.Drawing.Size(100, 25);
            this.toolMenu.TabIndex = 0;
            // 
            // MenuItemFile
            // 
            this.MenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuExport});
            this.MenuItemFile.Name = "MenuItemFile";
            this.MenuItemFile.Size = new System.Drawing.Size(36, 25);
            this.MenuItemFile.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemFile;
            // 
            // toolMenuExport
            // 
            this.toolMenuExport.Name = "toolMenuExport";
            this.toolMenuExport.Size = new System.Drawing.Size(187, 22);
            this.toolMenuExport.Tag = 17;
            this.toolMenuExport.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemExport;
            this.toolMenuExport.Click += new System.EventHandler(this.ExportImage);
            // 
            // MenuItemView
            // 
            this.MenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuFocusMode,
            this.toolMenuShowID,
            this.toolMenuAnimation});
            this.MenuItemView.Name = "MenuItemView";
            this.MenuItemView.Size = new System.Drawing.Size(42, 25);
            this.MenuItemView.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemView;
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
            // toolMenuAnimation
            // 
            this.toolMenuAnimation.Checked = true;
            this.toolMenuAnimation.CheckOnClick = true;
            this.toolMenuAnimation.Name = "toolMenuAnimation";
            this.toolMenuAnimation.Size = new System.Drawing.Size(169, 22);
            this.toolMenuAnimation.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemAnimation;
            this.toolMenuAnimation.ToolTipText = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuToolTipAnimation;
            this.toolMenuAnimation.Click += new System.EventHandler(this.ViewModeItemClick);
            // 
            // MenuItemEdit
            // 
            this.MenuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuCut,
            this.toolMenuCopy,
            this.toolMenuPaste,
            this.toolMenuDelete});
            this.MenuItemEdit.Name = "MenuItemEdit";
            this.MenuItemEdit.Size = new System.Drawing.Size(37, 25);
            this.MenuItemEdit.Text = global::Ecell.IDE.Plugins.PathwayWindow.MessageResources.MenuItemEdit;
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
                if (cs.Type == EcellObject.SYSTEM)
                {
                    button.Handle = new Handle(Mode.CreateSystem, new CreateSystemMouseHandler(m_con));
                    button.ToolTipText = MessageResources.ButtonToolTipCreateSystem;
                }
                else
                {
                    button.Handle = new Handle(Mode.CreateNode, new CreateNodeMouseHandler(m_con, cs));
                    if (cs.Type == EcellObject.PROCESS)
                        button.ToolTipText = MessageResources.ButtonToolTipCreateProcess;
                    else if (cs.Type == EcellObject.VARIABLE)
                        button.ToolTipText = MessageResources.ButtonToolTipCreateVariable;
                    else if (cs.Type == EcellObject.TEXT)
                        button.ToolTipText = MessageResources.ButtonToolTipCreateText;
                }

                button.Click += new EventHandler(ButtonStateChanged);
                toolButton.Items.Add(button);
            }

            // SelectMode is default.
            toolButtonSelect.Checked = true;
            m_handle = toolButtonSelect.Handle;
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
            bool isPPathwayNode = (node is PPathwayEntity);
            bool isPPathwayVariable = (node is PPathwayVariable);
            bool isPPathwayAlias = (node is PPathwayAlias);
            bool isPPathwaySystem = (node is PPathwaySystem);
            bool isPPathwayText = (node is PPathwayText);
            bool isRoot = false;
            bool isLine = (node is PPathwayLine);
            bool isOneway = true;
            bool isEffector = false;
            bool isCopiedObject = (m_con.CopiedNodes.Count > 0);
            bool isInsideRoot = m_con.Canvas.Systems[Constants.delimiterPath].Rect.Contains(m_con.MousePosition);

            // Set Popup menu visibility.
            if (isNull || (!isNull && node.Offset == PointF.Empty))
            {
                m_con.Canvas.PCanvas.ContextMenuStrip = this.PopupMenu;
            }
            else
            {
                m_con.Canvas.PCanvas.ContextMenuStrip = null;
            }


            // Set popup menu text.
            if (isPPathwayObject)
            {
                EcellObject obj = ((PPathwayObject)node).EcellObject;
                commonMenu.Object = obj;
                toolStripIdShow.Text = obj.FullID;
                SetLayerManu(obj);
                if (obj.Key.Equals(Constants.delimiterPath))
                    isRoot = true;
            }
            if (isLine)
            {
                PPathwayLine line = (PPathwayLine)node;
                SetLineMenu(line);
                isOneway = line.Info.Coefficient != 0;
                isEffector = line.Info.Direction == EdgeDirection.None;
            }
            if (isPPathwayText)
            {
                SetTextAlignmenu((PPathwayText)node);
            }
            // Show ObjectID(key).
            toolStripIdShow.Visible = isPPathwayObject;
            toolStripSeparator1.Visible = isPPathwayObject;
            // Show Line menus.
            toolStripOneWayArrow.Visible = isLine && !isOneway;
            toolStripAnotherArrow.Visible = isLine && isOneway;
            toolStripBidirArrow.Visible = isLine && (isOneway || isEffector);
            toolStripConstant.Visible = isLine && !isEffector;
            toolStripDeleteArrow.Visible = isLine;
            // Show Node / System edit menus.
            toolStripCut.Visible = isPPathwayObject && !isRoot;
            toolStripCopy.Visible = isPPathwayObject && !isRoot;
            toolStripPaste.Visible = isCopiedObject && isInsideRoot;
            toolStripDelete.Visible = (isPPathwayObject && !isRoot) || isPPathwayText;
            toolStripAlias.Visible = isPPathwayVariable;
            toolStripSeparator2.Visible = isPPathwayObject && !isRoot;
            // Set Text menu.
            toolStripTextAlign.Visible = isPPathwayText;
            // Show Layer menu.
            toolStripChangeLayer.Visible = isPPathwayObject && !isRoot;
            toolStripMoveFront.Visible = isPPathwayObject && !isRoot;
            toolStripMoveBack.Visible = isPPathwayObject && !isRoot;
            toolStripSeparator3.Visible = isPPathwayObject && !isRoot && !isPPathwayText;
            // Show Logger menu.
            commonMenu.addToolStripMenuItem.Visible = isPPathwaySystem;
            commonMenu.mergeSystemToolStripMenuItem.Visible = isPPathwaySystem && !isRoot;
            commonMenu.loggingToolStripMenuItem.Visible = isPPathwayObject && !isPPathwayText;
            commonMenu.observedToolStripMenuItem.Visible = isPPathwayObject && !isPPathwayText;
            commonMenu.parameterToolStripMenuItem.Visible = isPPathwayObject && !isPPathwayText;
            commonMenu.propertyToolStripMenuItem.Visible = isPPathwayObject;
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
        }

        /// <summary>
        /// Set text menu items.
        /// </summary>
        /// <param name="text"></param>
        private void SetTextAlignmenu(PPathwayText text)
        {
            StringAlignment align = text.PText.TextAlignment;
            this.toolStripAlignLeft.Enabled = align != StringAlignment.Near;
            this.toolStripAlignCenter.Enabled = align != StringAlignment.Center;
            this.toolStripAlignRight.Enabled = align != StringAlignment.Far;

            this.toolStripAlignLeft.Checked = align == StringAlignment.Near;
            this.toolStripAlignCenter.Checked = align == StringAlignment.Center;
            this.toolStripAlignRight.Checked = align == StringAlignment.Far;

        }

        /// <summary>
        /// Set line menu.
        /// </summary>
        /// <param name="line"></param>
        private void SetLineMenu(PPathwayLine line)
        {
            EdgeDirection direction = line.Info.Direction;

            toolStripAnotherArrow.Enabled = direction == EdgeDirection.Inward || direction == EdgeDirection.Outward;
            toolStripOneWayArrow.Enabled = direction == EdgeDirection.None || direction == EdgeDirection.Bidirection;
            toolStripBidirArrow.Enabled = direction != EdgeDirection.Bidirection;
            toolStripConstant.Enabled = direction != EdgeDirection.None;
        }

        /// <summary>
        /// Set EventHandler.
        /// </summary>
        internal void ResetEventHandler()
        {
            SetEventHandler(toolButtonSelect.Handle);
        }
        /// <summary>
        /// Set moving hand.
        /// </summary>
        internal void SetHandEventHandler()
        {
            SetEventHandler(toolButtonHand.Handle);
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
            handler = handle.EventHandler;
            foreach (ToolStripItem item in toolButton.Items)
            {
                if (!(item is PathwayToolStripButton))
                    continue;
                PathwayToolStripButton button = (PathwayToolStripButton)item;
                if (button.Handle == handle)
                    button.Checked = true;
                else
                    button.Checked = false;
            }

            if (m_con.Canvas == null)
                return;

            ((IPathwayEventHandler)handler).Initialize();
            AddInputEventListener(handler);
            m_con.Canvas.LineHandler.SetLineVisibility(false);
        }

        /// <summary>
        /// SetLineHandler
        /// </summary>
        /// <param name="node"></param>
        internal void SetCreateLineHandler(PPathwayEntity node)
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateAliasClick(object sender, EventArgs e)
        {
            // Check active canvas.
            CanvasControl canvas = m_con.Canvas;
            PPathwayVariable var = (PPathwayVariable)canvas.FocusNode;
            EcellVariable variable = (EcellVariable)var.EcellObject;
            PathUtil.SetLayout(variable, var);
            List<EcellLayout> aliases = variable.Aliases;
            aliases.Add(new EcellLayout(m_con.MousePosition));
            variable.Aliases = aliases;
            m_con.NotifyDataChanged(variable.Key, variable, true, true);
        }

        /// <summary>
        /// Called when a delete menu of the context menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender, EventArgs e)
        {
            if (m_con.Canvas == null || !m_con.Canvas.PCanvas.Focused)
                return;
            m_con.DeteleNodes();
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

            try
            {
                // Delete old edge.
                bool isDelete = (item.Text == MessageResources.CanvasMenuDelete);
                m_con.NotifyVariableReferenceChanged(
                    line.Info.ProcessKey,
                    line.Info.VariableKey,
                    RefChangeType.Delete,
                    0,
                    isDelete);
                if (isDelete)
                    return;

                // Create new edgeInfo.
                RefChangeType changeType = RefChangeType.SingleDir;
                int coefficient = 0;
                // Anothor Direction.
                if (item.Text == MessageResources.CanvasMenuAnotherArrow)
                {
                    changeType = RefChangeType.SingleDir;
                    if (line.Info.Direction == EdgeDirection.Inward)
                        coefficient = 1;
                    else
                        coefficient = -1;
                }
                // Bidir
                else if (item.Text == MessageResources.CanvasMenuBidirArrow)
                {
                    changeType = RefChangeType.BiDir;
                    coefficient = 0;
                }
                // Oneway
                else if (item.Text == MessageResources.CanvasMenuOnewayArrow)
                {
                    changeType = RefChangeType.SingleDir;
                    coefficient = 1;
                }
                // Effector
                else
                {
                    changeType = RefChangeType.SingleDir;
                    coefficient = 0;
                }
                // Throw DataChange Event.
                m_con.NotifyVariableReferenceChanged(
                    line.Info.ProcessKey,
                    line.Info.VariableKey,
                    changeType,
                    coefficient,
                    true);
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(MessageResources.ErrCreateEdge);
            }
        }

        /// <summary>
        /// Set Text Alignment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextAlign_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            StringAlignment align = (StringAlignment)menu.Tag;

            // Get new layer name.
            PPathwayText text = (PPathwayText)m_con.Canvas.FocusNode;
            text.PText.TextAlignment = align;
            text.NotifyDataChanged();
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
            string name = name = menu.Text;

            // Change layer of selected objects.
            PPathwayLayer layer = canvas.GetLayer(name);
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
            PPathwayObject obj = (PPathwayObject)m_con.Canvas.FocusNode;

            bool flag = false;
            foreach (PNode node in obj.Layer.GetNodes())
            {
                if (flag)
                {
                    obj.MoveInFrontOf(node);
                }
                if (node == obj)
                    flag = true;
            }
        }

        /// <summary>
        /// Layer move to back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveToBackClick(object sender, EventArgs e)
        {
            PPathwayObject obj = (PPathwayObject)m_con.Canvas.FocusNode;

            bool flag = true;
            foreach (PNode node in obj.Layer.GetNodes())
            {
                if (flag)
                {
                    obj.MoveInFrontOf(node);
                }
                if (node == obj)
                    flag = false;
            }
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
            if (m_con.Canvas == null || !m_con.Canvas.PCanvas.Focused)
                return;
            m_con.CutNodes();
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
            PropertyDialog dialog = new PropertyDialog(m_con.Window.PluginManager.GetPropertySettings());
            using (dialog)
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                dialog.ApplyChanges();
                m_con.ResetObjectSettings();
            }
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
            m_con.FocusMode = (item.CheckState == CheckState.Checked);
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
            SetAnimation(toolMenuAnimation.Checked);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewModeButtonClick(object sender, EventArgs e)
        {
            SetAnimation(toolButtonAnimation.Checked);
        }

        internal void SetAnimation(bool viewMode)
        {
            toolButtonAnimation.Checked = viewMode;
            toolMenuAnimation.Checked = viewMode;
            m_con.IsAnimation = viewMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        internal void SetAnimationEnabled(bool enabled)
        {
            toolButtonAnimation.Enabled = enabled;
            toolMenuAnimation.Enabled = enabled;
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
                sfd.Filter = Constants.FilterSVGFile + "|" + Constants.FilterImageFile;
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
            PointF offset = new PointF();
            if (e.KeyCode == Keys.Left)
                offset.X = offset.X - moveLength;
            if (e.KeyCode == Keys.Right)
                offset.X = offset.X + moveLength;
            if (e.KeyCode == Keys.Up)
                offset.Y = offset.Y - moveLength;
            if (e.KeyCode == Keys.Down)
                offset.Y = offset.Y + moveLength;

            if (offset == PointF.Empty)
                return;

            m_con.Canvas.MoveSelectedObjects(offset);
            m_con.Canvas.NotifyMoveObjects(true);
        }

        #endregion

    }
}
