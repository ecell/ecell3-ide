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
// written by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System.Drawing;
using Ecell.IDE.Plugins.PathwayWindow.Components;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo;

namespace Ecell.IDE.Plugins.PathwayWindow.UIComponent
{
    /// <summary>
    /// UI class for PathwayWindow.
    /// </summary>
    public partial class PToolBoxCanvas : PCanvas
    {
        #region Fields
        PPathwayObject m_object = null;
        ComponentSetting m_setting = null;
        
        #endregion

        #region Accessors
        /// <summary>
        /// 
        /// </summary>
        public PPathwayObject PPathwayObject
        {
            get { return m_object; }
            set { m_object = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ComponentSetting Setting
        {
            get { return m_setting; }
            set
            {
                m_setting = value;
                SetTemplate(value);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public PToolBoxCanvas()
        {
            PPathwayLayer layer = new PPathwayLayer();
            base.Root.AddChild(layer);
            base.Camera.AddLayer(layer);
            base.Camera.ScaleViewBy(0.7f);
            base.Camera.Pickable = false;
            base.RemoveInputEventListener(base.PanEventHandler);
            base.RemoveInputEventListener(base.ZoomEventHandler);
            base.BackColor = Color.Silver;
            base.AllowDrop = false;
            base.GridFitText = false;
        }
        #endregion

        #region private Methods
        private void SetTemplate(ComponentSetting setting)
        {
            if (setting == null)
                return;
            RectangleF bounds = base.Camera.ViewBounds;
            PointF center = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
            m_object = setting.CreateTemplate();
            m_object.Pickable = false;
            m_object.CenterPointF = center;
            m_object.RefreshView();
            base.Layer.AddChild(m_object);
        }
        #endregion
    }
}
