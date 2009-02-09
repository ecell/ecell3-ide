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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.IDE.Plugins.PathwayWindow
{

    /// <summary>
    /// ComponentConstants
    /// </summary>
    internal class PathwayConstants
    {
        #region ComponentConstants
        /// <summary>
        /// The default name of system.
        /// </summary>
        public const string NameOfDefaultSystem = "DefaultSystem";
        /// <summary>
        /// The default name of process.
        /// </summary>
        public const string NameOfDefaultProcess = "DefaultProcess";
        /// <summary>
        /// The default name of variable.
        /// </summary>
        public const string NameOfDefaultVariable = "DefaultVariable";
        /// <summary>
        /// The default name of text.
        /// </summary>
        public const string NameOfDefaultText = "DefaultText";
        /// <summary>
        /// Class name of PPathwaySystem.
        /// </summary>
        public const string ClassPPathwaySystem = "PPathwaySystem";
        /// <summary>
        /// Class name of PPathwayProcess.
        /// </summary>
        public const string ClassPPathwayProcess = "PPathwayProcess";
        /// <summary>
        /// Class name of PPathwayVariable.
        /// </summary>
        public const string ClassPPathwayVariable = "PPathwayVariable";
        /// <summary>
        /// Class name of PPathwayText.
        /// </summary>
        public const string ClassPPathwayText = "PPathwayText";
        #endregion

        #region Window Constants
        /// <summary>
        /// 
        /// </summary>
        public const string WindowLayer = "WindowLayer";
        /// <summary>
        /// 
        /// </summary>
        public const string WindowOverview = "WindowOverview";
        /// <summary>
        /// 
        /// </summary>
        public const string WindowPathway = "WindowPathway";
        /// <summary>
        /// 
        /// </summary>
        public const string WindowToolbox = "WindowToolbox";

        #endregion

        #region XMLPath
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFileHeader1 = "PathwayWindow configuration file.";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFileHeader2 = "Automatically generated file. DO NOT modify!";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathApplication = "Application";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathApplicationVersion = "Version";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFileVersion = "FileVersion";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathConfigFileVersion = "ConfigFileVersion";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathVersion = "1.0";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathComponentList = "ComponentList";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathComponent = "Component";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathName = "Name";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathType = "Type";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathClass = "Class";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathModelID = "ModelID";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathKey = "Key";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathIsDafault = "IsDefault";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathMode = "Mode";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathEdit = "Edit";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathGradation = "Gradation";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathSize = "Size";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFigure = "Figure";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathTextBrush = "TextBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathLineBrush = "LineBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFillBrush = "FillBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathCenterBrush = "CenterBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathIsGradation = "IsGradation";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathIconFile = "IconFile";
        #endregion
    }

    /// <summary>
    /// Menu constants
    /// </summary>
    internal class MenuConstants: Ecell.MenuConstants
    {
        #region Menu Constants
        #region CanvasPopUpMenu
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for ID
        /// </summary>
        internal const string CanvasMenuID = "CanvasMenuID";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for delete
        /// </summary>
        internal const string CanvasMenuDelete = "CanvasMenuDelete";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for copy
        /// </summary>
        internal const string CanvasMenuCopy = "CanvasMenuCopy";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for cut
        /// </summary>
        internal const string CanvasMenuCut = "CanvasMenuCut";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for paste
        /// </summary>
        internal const string CanvasMenuPaste = "CanvasMenuPaste";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for delete
        /// </summary>
        internal const string CanvasMenuMerge = "CanvasMenuMerge";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for delete
        /// </summary>
        internal const string CanvasMenuAlias = "CanvasMenuAlias";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for Set Layout
        /// </summary>
        internal const string CanvasMenuLayout = "CanvasMenuLayout";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for Change Layer
        /// </summary>
        internal const string CanvasMenuChangeLayer = "LayerMenuChange";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for Create Layer
        /// </summary>
        internal const string CanvasMenuCreateLayer = "LayerMenuCreate";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for Layer Move To Front
        /// </summary>
        internal const string CanvasMenuMoveFront = "LayerMenuMoveFront";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for Layer Move To Front
        /// </summary>
        internal const string CanvasMenuMoveBack = "LayerMenuMoveBack";
        ///// <summary>
        ///// Key definition of m_cMenuDict and MessageResources for rightArrow
        ///// </summary>
        //internal const string CanvasMenuRightArrow = "CanvasMenuRightArrow";
        ///// <summary>
        ///// Key definition of m_cMenuDict and MessageResources for leftArrow
        ///// </summary>
        //internal const string CanvasMenuLeftArrow = "CanvasMenuLeftArrow";
        internal const string CanvasMenuAnotherArrow = "CanvasMenuAnotherArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for bidirArrow
        /// </summary>
        internal const string CanvasMenuBidirArrow = "CanvasMenuBidirArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for rightArrow
        /// </summary>
        internal const string CanvasMenuDeleteArrow = "CanvasMenuDeleteArrow";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for constantLine
        /// </summary>
        internal const string CanvasMenuConstantLine = "CanvasMenuConstantLine";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for Create Logger
        /// </summary>
        internal const string CanvasMenuCreateLogger = "CanvasMenuCreateLogger";
        /// <summary>
        /// Key definition of m_cMenuDict and MessageResources for delete Logger
        /// </summary>
        internal const string CanvasMenuDeleteLogger = "CanvasMenuDeleteLogger";
        /// <summary>
        /// Key definition of m_cMenuDict for separator1
        /// </summary>
        internal const string CanvasMenuSeparator1 = "CanvasMenuSeparator1";
        /// <summary>
        /// Key definition of m_cMenuDict for separator2
        /// </summary>
        internal const string CanvasMenuSeparator2 = "CanvasMenuSeparator2";
        /// <summary>
        /// Key definition of m_cMenuDict for separator3
        /// </summary>
        internal const string CanvasMenuSeparator3 = "CanvasMenuSeparator3";
        /// <summary>
        /// Key definition of m_cMenuDict for separator4
        /// </summary>
        internal const string CanvasMenuSeparator4 = "CanvasMenuSeparator4";
        /// <summary>
        /// Key definition of m_cMenuDict for separator5
        /// </summary>
        internal const string CanvasMenuSeparator5 = "CanvasMenuSeparator5";
        #endregion

        #region ToolBarMenu
        /// <summary>
        /// Key definition of MessageResources for Export
        /// </summary>
        internal const string MenuItemExport = "MenuItemExport";
        /// <summary>
        /// Key definition of MessageResources for ToolTipExport
        /// </summary>
        internal const string MenuToolTipExport = "MenuToolTipExport";
        /// <summary>
        /// Key definition of MessageResources for ExportSVG
        /// </summary>
        internal const string MenuItemExportSVG = "MenuItemExportSVG";
        /// <summary>
        /// Key definition of MessageResources for ToolTipExportSVG
        /// </summary>
        internal const string MenuToolTipExportSVG = "MenuToolTipExportSVG";
        /// <summary>
        /// Key definition of MessageResources for ToolTipShowID
        /// </summary>
        internal const string MenuToolTipSetup = "MenuToolTipSetup";
        /// <summary>
        /// Key definition of MessageResources for ShowID
        /// </summary>
        internal const string MenuItemShowID = "MenuItemShowID";
        /// <summary>
        /// Key definition of MessageResources for ToolTipShowID
        /// </summary>
        internal const string MenuToolTipShowID = "MenuToolTipShowID";
        /// <summary>
        /// Key definition of MessageResources for ViewMode
        /// </summary>
        internal const string MenuItemViewMode = "MenuItemViewMode";
        /// <summary>
        /// Key definition of MessageResources for ToolTipViewMode
        /// </summary>
        internal const string MenuToolTipViewMode = "MenuToolTipViewMode";
        #endregion

        #region ToolButton
        /// <summary>
        /// Key definition of MessageResources for ToolButtonAddConstant
        /// </summary>
        internal const string ToolButtonAddConstant = "ToolButtonAddConstant";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonAddMutualReaction
        /// </summary>
        internal const string ToolButtonAddMutualReaction = "ToolButtonAddMutualReaction";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonAddOnewayReaction
        /// </summary>
        internal const string ToolButtonAddOnewayReaction = "ToolButtonAddOnewayReaction";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonCreateText
        /// </summary>
        internal const string ToolButtonCreateText = "ToolButtonCreateText";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonCreateProcess
        /// </summary>
        internal const string ToolButtonCreateProcess = "ToolButtonCreateProcess";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonCreateSystem
        /// </summary>
        internal const string ToolButtonCreateSystem = "ToolButtonCreateSystem";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonCreateVariable
        /// </summary>
        internal const string ToolButtonCreateVariable = "ToolButtonCreateVariable";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonMoveCanvas
        /// </summary>
        internal const string ToolButtonMoveCanvas = "ToolButtonMoveCanvas";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonSelectMode
        /// </summary>
        internal const string ToolButtonSelectMode = "ToolButtonSelectMode";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonOverview
        /// </summary>
        internal const string ToolButtonOverview = "ToolButtonOverview";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonViewMode
        /// </summary>
        internal const string ToolButtonViewMode = "ToolButtonViewMode";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonZoomIn
        /// </summary>
        internal const string ToolButtonZoomIn = "ToolButtonZoomIn";
        /// <summary>
        /// Key definition of MessageResources for ToolButtonZoomOut
        /// </summary>
        internal const string ToolButtonZoomOut = "ToolButtonZoomOut";
        #endregion
        #endregion
    }
}
