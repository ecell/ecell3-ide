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

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// PathwayDialogConstant
    /// </summary>
    internal class AnimationConstants
    {
        #region Constants for dialog text.
        /// <summary>
        /// DialogTextAnimationSetting
        /// </summary>
        public const string DialogTextAnimationSetting = "DialogTextAnimationSetting";
        /// <summary>
        /// DialogTextBackgroundBrush
        /// </summary>
        public const string DialogTextBackgroundBrush = "DialogTextBackgroundBrush";
        /// <summary>
        /// DialogTextEdgeBrush
        /// </summary>
        public const string DialogTextEdgeBrush = "DialogTextEdgeBrush";
        /// <summary>
        /// DialogTextEditMode
        /// </summary>
        public const string DialogTextEditMode = "DialogTextEditMode";
        /// <summary>
        /// DialogTextEdgeWidth
        /// </summary>
        public const string DialogTextEdgeWidth = "DialogTextEdgeWidth";
        /// <summary>
        /// DialogTextMaxEdgeWidth
        /// </summary>
        public const string DialogTextMaxEdgeWidth = "DialogTextMaxEdgeWidth";
        /// <summary>
        /// DialogTextNGBrush
        /// </summary>
        public const string DialogTextNGBrush = "DialogTextNGBrush";
        /// <summary>
        /// DialogTextNormalEdge
        /// </summary>
        public const string DialogTextNormalEdge = "DialogTextNormalEdge";
        /// <summary>
        /// DialogTextThresholdHigh
        /// </summary>
        public const string DialogTextThresholdHigh = "DialogTextThresholdHigh";
        /// <summary>
        /// DialogTextThresholdLow
        /// </summary>
        public const string DialogTextThresholdLow = "DialogTextThresholdLow";
        /// <summary>
        /// DialogTextViewMode
        /// </summary>
        public const string DialogTextViewMode = "DialogTextViewMode";
        /// <summary>
        /// DialogTextViewMode
        /// </summary>
        public const string DialogTextPropertyBrush = "DialogTextPropertyBrush";
        /// <summary>
        /// DialogTextLogarithmic
        /// </summary>
        public const string DialogTextLogarithmic = "DialogTextLogarithmic";
        #endregion

        #region Constants for XML.
        /// <summary>
        /// 
        /// </summary>
        public const string xPathFileName = "AnimationSettings.xml";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathAnimationSettings = "AnimationSettings";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathVersion = "1.0";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathAutoThreshold = "AutoThreshold";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathThresholdHigh = "ThresholdHigh";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathThresholdLow = "ThresholdLow";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathNormalEdgeWidth = "NormalEdgeWidth";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathMaxEdgeWidth = "MaxEdgeWidth";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathEditBGBrush = "EditBGBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathEditEdgeBrush = "EditEdgeBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathViewBGBrush = "ViewBGBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathViewEdgeBrush = "ViewEdgeBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathLowEdgeBrush = "LowEdgeBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathHighEdgeBrush = "HighEdgeBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathNGEdgeBrush = "NGEdgeBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathPropertyBrush = "PropertyBrush";
        /// <summary>
        /// 
        /// </summary>
        public const string xPathIsLogarithmic = "IsLogarithmic";
        #endregion
    }
}
