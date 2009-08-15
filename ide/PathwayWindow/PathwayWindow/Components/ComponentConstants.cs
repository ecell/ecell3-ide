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
    internal class ComponentConstants
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
        /// The default name of stepper.
        /// </summary>
        public const string NameOfDefaultStepper = "DefaultStepper";

        #endregion

        #region XMLPath
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFileName = "ComponentSettings.xml";
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
        public const string xPathIsStencil = "IsStencil";
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
        /// <summary>
        /// 
        /// </summary>
        public const string xPathIconImage = "IconImage";
        #endregion
    }
}
