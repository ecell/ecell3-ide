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
        public static readonly string CanvasMenuID = "CanvasMenuID";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for delete
        /// </summary>
        public static readonly string CanvasMenuDelete = "CanvasMenuDelete";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for copy
        /// </summary>
        public static readonly string CanvasMenuCopy = "CanvasMenuCopy";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for cut
        /// </summary>
        public static readonly string CanvasMenuCut = "CanvasMenuCut";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for paste
        /// </summary>
        public static readonly string CanvasMenuPaste = "CanvasMenuPaste";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for delete
        /// </summary>
        public static readonly string CanvasMenuMerge = "CanvasMenuMerge";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Set Layout
        /// </summary>
        public static readonly string CanvasMenuLayout = "CanvasMenuLayout";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Change Layer
        /// </summary>
        public static readonly string CanvasMenuChangeLayer = "CanvasMenuChangeLayer";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for rightArrow
        /// </summary>
        public static readonly string CanvasMenuRightArrow = "CanvasMenuRightArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for leftArrow
        /// </summary>
        public static readonly string CanvasMenuLeftArrow = "CanvasMenuLeftArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for bidirArrow
        /// </summary>
        public static readonly string CanvasMenuBidirArrow = "CanvasMenuBidirArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for constantLine
        /// </summary>
        public static readonly string CanvasMenuConstantLine = "CanvasMenuConstantLine";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for Create Logger
        /// </summary>
        public static readonly string CanvasMenuCreateLogger = "CanvasMenuCreateLogger";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResPathway for delete Logger
        /// </summary>
        public static readonly string CanvasMenuDeleteLogger = "CanvasMenuDeleteLogger";
        /// <summary>
        /// Key definition of m_cMenuDict for separator1
        /// </summary>
        public static readonly string CanvasMenuSeparator1 = "CanvasMenuSeparator1";
        /// <summary>
        /// Key definition of m_cMenuDict for separator2
        /// </summary>
        public static readonly string CanvasMenuSeparator2 = "CanvasMenuSeparator2";
        /// <summary>
        /// Key definition of m_cMenuDict for separator3
        /// </summary>
        public static readonly string CanvasMenuSeparator3 = "CanvasMenuSeparator3";
        /// <summary>
        /// Key definition of m_cMenuDict for separator4
        /// </summary>
        public static readonly string CanvasMenuSeparator4 = "CanvasMenuSeparator4";
        /// <summary>
        /// Key definition of m_cMenuDict for separator5
        /// </summary>
        public static readonly string CanvasMenuSeparator5 = "CanvasMenuSeparator5";
        #endregion

        #region ToolBarMenu
        /// <summary>
        /// Key definition of MessageResPathway for ShowID
        /// </summary>
        private static readonly string MenuItemSetup = "MenuItemSetup";
        /// <summary>
        /// Key definition of MessageResPathway for ToolTipShowID
        /// </summary>
        private static readonly string MenuToolTipSetup = "MenuToolTipSetup";
        /// <summary>
        /// Key definition of MessageResPathway for ShowID
        /// </summary>
        private static readonly string MenuItemShowID = "MenuItemShowID";
        /// <summary>
        /// Key definition of MessageResPathway for ToolTipShowID
        /// </summary>
        private static readonly string MenuToolTipShowID = "MenuToolTipShowID";
        /// <summary>
        /// Key definition of MessageResPathway for ViewMode
        /// </summary>
        private static readonly string MenuItemViewMode = "MenuItemViewMode";
        /// <summary>
        /// Key definition of MessageResPathway for ToolTipViewMode
        /// </summary>
        private static readonly string MenuToolTipViewMode = "MenuToolTipViewMode";
        /// <summary>
        /// Key definition of MessageResPathway for MenuItemLayout
        /// </summary>
        private static readonly string MenuItemLayout = "MenuItemLayout";
        /// <summary>
        /// Key definition of MessageResPathway for MenuItemLayout
        /// </summary>
        private static readonly string MenuItemEdit = "MenuItemEdit";
        /// <summary>
        /// Key definition of MessageResPathway for MenuItemLayout
        /// </summary>
        private static readonly string MenuItemView = "MenuItemView";
        #endregion
        #endregion
        
        #region Fields
        /// <summary>
        /// The PathwayView, from which this class gets messages from the E-cell core and through which this class
        /// sends messages to the E-cell core.
        /// </summary>
        protected PathwayControl m_con;

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
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));
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

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public MenuControl(PathwayControl control)
        {
            m_con = control;
            m_menuLayoutList = CreateLayoutMenus();
            m_menuList = CreateToolStripMenuItems();
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
            foreach (ILayoutAlgorithm algorithm in m_con.LayoutList)
            {
                PathwayToolStripMenuItem layoutItem = new PathwayToolStripMenuItem();
                layoutItem.Text = algorithm.GetMenuText();
                layoutItem.ID = count;
                layoutItem.Visible = true;
                layoutItem.ToolTipText = algorithm.GetToolTipText();

                List<string> subCommands = algorithm.GetSubCommands();
                if (subCommands == null || subCommands.Count == 0)
                {
                    layoutItem.Click += new EventHandler(m_con.LayoutItem_Click);
                }
                else
                {
                    int subcount = 0;
                    foreach (string subCommandName in subCommands)
                    {
                        PathwayToolStripMenuItem layoutSubItem = new PathwayToolStripMenuItem();
                        layoutSubItem.Text = subCommandName;
                        layoutSubItem.SubID = subcount;
                        layoutSubItem.Visible = true;
                        layoutSubItem.Click += new EventHandler(m_con.LayoutItem_Click);
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
            rightArrow.Click += new EventHandler(m_con.ChangeLineClick);
            nodeMenu.Items.Add(rightArrow);
            m_popMenuDict.Add(CanvasMenuRightArrow, rightArrow);

            ToolStripItem leftArrow = new ToolStripMenuItem(m_resources.GetString(CanvasMenuLeftArrow), PathwayResource.arrow_long_left_w);
            leftArrow.Name = CanvasMenuLeftArrow;
            leftArrow.Click += new EventHandler(m_con.ChangeLineClick);
            nodeMenu.Items.Add(leftArrow);
            m_popMenuDict.Add(CanvasMenuLeftArrow, leftArrow);

            ToolStripItem bidirArrow = new ToolStripMenuItem(m_resources.GetString(CanvasMenuBidirArrow), PathwayResource.arrow_long_bidir_w);
            bidirArrow.Name = CanvasMenuBidirArrow;
            bidirArrow.Click += new EventHandler(m_con.ChangeLineClick);
            nodeMenu.Items.Add(bidirArrow);
            m_popMenuDict.Add(CanvasMenuBidirArrow, bidirArrow);

            ToolStripItem constant = new ToolStripMenuItem(m_resources.GetString(CanvasMenuConstantLine), PathwayResource.ten);
            constant.Name = CanvasMenuConstantLine;
            constant.Click += new EventHandler(m_con.ChangeLineClick);
            nodeMenu.Items.Add(constant);
            m_popMenuDict.Add(CanvasMenuConstantLine, constant);

            ToolStripSeparator separator3 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator3);
            m_popMenuDict.Add(CanvasMenuSeparator3, separator3);

            // Add Edit menus
            ToolStripItem cut = new ToolStripMenuItem(m_resources.GetString(CanvasMenuCut));
            cut.Click += new EventHandler(m_con.CutClick);
            nodeMenu.Items.Add(cut);
            m_popMenuDict.Add(CanvasMenuCut, cut);

            ToolStripItem copy = new ToolStripMenuItem(m_resources.GetString(CanvasMenuCopy));
            copy.Click += new EventHandler(m_con.CopyClick);
            nodeMenu.Items.Add(copy);
            m_popMenuDict.Add(CanvasMenuCopy, copy);

            ToolStripItem paste = new ToolStripMenuItem(m_resources.GetString(CanvasMenuPaste));
            paste.Click += new EventHandler(m_con.PasteClick);
            nodeMenu.Items.Add(paste);
            m_popMenuDict.Add(CanvasMenuPaste, paste);

            ToolStripItem delete = new ToolStripMenuItem(m_resources.GetString(CanvasMenuDelete));
            delete.Click += new EventHandler(m_con.DeleteClick);
            nodeMenu.Items.Add(delete);
            m_popMenuDict.Add(CanvasMenuDelete, delete);

            ToolStripItem merge = new ToolStripMenuItem(m_resources.GetString(CanvasMenuMerge));
            merge.Click += new EventHandler(m_con.MergeClick);
            nodeMenu.Items.Add(merge);
            m_popMenuDict.Add(CanvasMenuMerge, merge);

            ToolStripSeparator separator4 = new ToolStripSeparator();
            nodeMenu.Items.Add(separator4);
            m_popMenuDict.Add(CanvasMenuSeparator4, separator4);

            // Layer changer
            ToolStripItem changeLayer = new ToolStripMenuItem(m_resources.GetString(CanvasMenuChangeLayer));
            changeLayer.Click += new EventHandler(m_con.ChangeLeyerClick);
            nodeMenu.Items.Add(changeLayer);
            m_popMenuDict.Add(CanvasMenuChangeLayer, changeLayer);

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
        private List<ToolStripMenuItem> CreateToolStripMenuItems()
        {
            List<ToolStripMenuItem> menuList = new List<ToolStripMenuItem>();

            // Setup menu
            ToolStripMenuItem setupItem = new ToolStripMenuItem();
            setupItem.ToolTipText = m_resources.GetString(MenuToolTipSetup);
            setupItem.Text = m_resources.GetString(MenuItemSetup);
            setupItem.Click += new EventHandler(m_con.ShowDialogClick);

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
            showIdItem.Click += new EventHandler(m_con.ShowIdClick);

            ToolStripMenuItem viewModeItem = new ToolStripMenuItem();
            viewModeItem.CheckOnClick = true;
            viewModeItem.CheckState = CheckState.Unchecked;
            viewModeItem.ToolTipText = m_resources.GetString(MenuToolTipViewMode);
            viewModeItem.Text = m_resources.GetString(MenuItemViewMode);
            viewModeItem.Click += new EventHandler(m_con.ViewModeClick);

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
            deleteMenu.Click += new EventHandler(m_con.DeleteClick);
            deleteMenu.ShortcutKeys = Keys.Delete;
            deleteMenu.ShowShortcutKeys = true;

            ToolStripMenuItem cutMenu = new ToolStripMenuItem();
            cutMenu.Text = m_resources.GetString(CanvasMenuCut);
            cutMenu.Name = CanvasMenuCut;
            cutMenu.Click += new EventHandler(m_con.CutClick);
            cutMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            cutMenu.ShowShortcutKeys = true;

            ToolStripMenuItem copyMenu = new ToolStripMenuItem();
            copyMenu.Text = m_resources.GetString(CanvasMenuCopy);
            copyMenu.Name = CanvasMenuCopy;
            copyMenu.Click += new EventHandler(m_con.CopyClick);
            copyMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            copyMenu.ShowShortcutKeys = true;

            ToolStripMenuItem pasteMenu = new ToolStripMenuItem();
            pasteMenu.Text = m_resources.GetString(CanvasMenuPaste);
            pasteMenu.Name = CanvasMenuPaste;
            pasteMenu.Click += new EventHandler(m_con.PasteClick);
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
            handButton.Name = "MoveCanvas";
            handButton.Image = PathwayResource.move1;
            handButton.Text = "";
            handButton.CheckOnClick = true;
            handButton.ToolTipText = "MoveCanvas";
            handButton.Handle = new Handle(Mode.Pan, handleCount, new PPanEventHandler());
            m_handleDict.Add("MoveCanvas", handButton.Handle);
            handButton.Click += new EventHandler(m_con.ButtonStateChanged);
            list.Add(handButton);

            PathwayToolStripButton button0 = new PathwayToolStripButton();
            button0.ImageTransparentColor = System.Drawing.Color.Magenta;
            button0.Name = "SelectMode";
            button0.Image = PathwayResource.arrow;
            button0.Text = "";
            button0.CheckOnClick = true;
            button0.ToolTipText = "SelectMode";
            button0.Handle = new Handle(Mode.Select, handleCount, new DefaultMouseHandler(m_con));
            m_handleDict.Add("SelectMode", button0.Handle);
            button0.Click += new EventHandler(m_con.ButtonStateChanged);
            list.Add(button0);

            ToolStripSeparator sep = new ToolStripSeparator();
            list.Add(sep);

            PathwayToolStripButton arrowButton = new PathwayToolStripButton();
            arrowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            arrowButton.Name = "reactionOneway";
            arrowButton.Image = PathwayResource.arrow_long_right_w;
            arrowButton.Text = "";
            arrowButton.CheckOnClick = true;
            arrowButton.ToolTipText = "Add Oneway Reaction";
            arrowButton.Handle = new Handle(Mode.CreateOneWayReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add("reactionOneway", arrowButton.Handle);
            arrowButton.Click += new EventHandler(m_con.ButtonStateChanged);
            list.Add(arrowButton);

            PathwayToolStripButton bidirButton = new PathwayToolStripButton();
            bidirButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            bidirButton.Name = "reactionMutual";
            bidirButton.Image = PathwayResource.arrow_long_bidir_w;
            bidirButton.Text = "";
            bidirButton.CheckOnClick = true;
            bidirButton.ToolTipText = "Add Mutual Reaction";
            bidirButton.Handle = new Handle(Mode.CreateMutualReaction, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add("reactionMutual", bidirButton.Handle);
            bidirButton.Click += new EventHandler(m_con.ButtonStateChanged);
            list.Add(bidirButton);

            PathwayToolStripButton constButton = new PathwayToolStripButton();
            constButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            constButton.Name = "constant";
            constButton.Image = PathwayResource.ten;
            constButton.Text = "";
            constButton.CheckOnClick = true;
            constButton.ToolTipText = "Add Constant";
            constButton.Handle = new Handle(Mode.CreateConstant, handleCount, new CreateReactionMouseHandler(m_con));
            m_handleDict.Add("constant", constButton.Handle);
            constButton.Click += new EventHandler(m_con.ButtonStateChanged);
            list.Add(constButton);

            foreach (ComponentSetting cs in m_con.ComponentManager.ComponentSettings)
            {
                PathwayToolStripButton button = new PathwayToolStripButton();
                button.ImageTransparentColor = System.Drawing.Color.Magenta;
                button.Name = cs.Name;
                button.Image = new Bitmap(256, 256);
                Graphics gra = Graphics.FromImage(button.Image);
                if (cs.ComponentType == ComponentType.System)
                {
                    Rectangle rect = new Rectangle(3, 3, 240, 240);
                    gra.DrawRectangle(new Pen(Brushes.Black, 16), rect);
                    button.Handle = new Handle(Mode.CreateSystem, handleCount++, new CreateSystemMouseHandler(m_con), cs.ComponentType);
                }
                else
                {
                    GraphicsPath gp = cs.TransformedPath;
                    gra.FillPath(cs.FillBrush, gp);
                    gra.DrawPath(new Pen(Brushes.Black, 16), gp);
                    button.Handle = new Handle(Mode.CreateNode, handleCount++, new CreateNodeMouseHandler(m_con, cs.ComponentType), cs.ComponentType);
                }
                button.Size = new System.Drawing.Size(256, 256);
                button.Text = "";
                button.CheckOnClick = true;
                button.ToolTipText = cs.Name;

                m_handleDict.Add(cs.Name, button.Handle);
                button.Click += new EventHandler(m_con.ButtonStateChanged);
                list.Add(button);
            }

            PathwayToolStripButton zoominButton = new PathwayToolStripButton();
            zoominButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoominButton.Name = "zoomin";
            zoominButton.Image = PathwayResource.zoom_in;
            zoominButton.Text = "";
            zoominButton.CheckOnClick = false;
            zoominButton.ToolTipText = "Zoom In";
            zoominButton.Handle = new Handle(Mode.CreateConstant, handleCount, 2f);
            zoominButton.Click += new EventHandler(m_con.ZoomButton_Click);
            list.Add(zoominButton);

            PathwayToolStripButton zoomoutButton = new PathwayToolStripButton();
            zoomoutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoomoutButton.Name = "zoomout";
            zoomoutButton.Image = PathwayResource.zoom_out;
            zoomoutButton.Text = "";
            zoomoutButton.CheckOnClick = false;
            zoomoutButton.ToolTipText = "Zoom Out";
            zoomoutButton.Handle = new Handle(Mode.CreateConstant, handleCount, 0.5f);
            zoomoutButton.Click += new EventHandler(m_con.ZoomButton_Click);
            list.Add(zoomoutButton);

            // SelectMode is default.
            button0.Checked = true;
            m_con.SelectedHandle = (Handle)button0.Handle;

            return list;
        }
        #endregion
    }
}
