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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.Resources;
using System.Drawing;

namespace EcellLib.PathwayWindow.Handler
{
    public class ToolBoxDragHandler : PDragSequenceEventHandler
    {
        #region Fields
        /// <summary>
        /// PathwayControl
        /// </summary>
        PathwayControl m_con;
        /// <summary>
        /// Current canvas.
        /// </summary>
        CanvasControl m_canvas;
        /// <summary>
        /// Template object.
        /// </summary>
        PPathwayObject m_object = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public ToolBoxDragHandler(PathwayControl control)
        {
            m_con = control;
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Event on mouse move.
        /// Move template object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnDrag(object sender, PInputEventArgs e)
        {
            base.OnDrag(sender, e);

            if (m_canvas == null)
                return;
            if (m_object == null)
                return;

            m_object.CenterPointF = e.Position;
        }
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);
        }
        protected override void OnStartDrag(object sender, PInputEventArgs e)
        {
            base.OnStartDrag(sender, e);

            // Set EventHandler for current canvas.
            if (m_con.ActiveCanvas == null)
                return;
            if (!(e.Canvas is PToolBoxCanvas))
                return;
            SetEventHandler((PToolBoxCanvas)e.Canvas);
        }

        /// <summary>
        /// Event on mouse up.
        /// Check EventHandler status and Set/Reset EventHandler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEndDrag(object sender, PInputEventArgs e)
        {
            base.OnEndDrag(sender, e);
            if (m_canvas == null)
                return;

            if (e.Canvas == m_canvas.PathwayCanvas)
            {
                // Add new object and reset EventHandler.
                AddObject(e);
            }
            ResetEventHandler();
       }
        #endregion

        #region private Methods
        /// <summary>
        /// Add new Object.
        /// </summary>
        /// <param name="e"></param>
        private void AddObject(PInputEventArgs e)
        {
            string systemKey = m_canvas.GetSurroundingSystemKey(e.Position);
            string type = GetType(m_object);
            DataManager dm = m_canvas.PathwayControl.Window.DataManager;
            EcellObject eo = dm.CreateDefaultObject(m_canvas.ModelID, systemKey, type, false);
            eo.X = m_object.X;
            eo.Y = m_object.Y;
            eo.Width = m_object.Width;
            eo.Height = m_object.Height;
            m_canvas.PathwayControl.NotifyDataAdd(eo, true);
        }

        /// <summary>
        /// get object type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetType(PPathwayObject obj)
        {
            if (obj is PPathwaySystem)
                return EcellObject.SYSTEM;
            else if (obj is PPathwayProcess)
                return EcellObject.PROCESS;
            else if (obj is PPathwayVariable)
                return EcellObject.VARIABLE;
            else
                return null;
        }
        /// <summary>
        /// Initialise EventHandler.
        /// </summary>
        /// <param name="canvas"></param>
        private void SetEventHandler(PToolBoxCanvas canvas)
        {
            m_canvas = m_con.ActiveCanvas;
            m_canvas.PathwayCanvas.AddInputEventListener(this);
            m_object = canvas.Setting.CreateTemplate();
            m_object.Pickable = false;
            m_canvas.ControlLayer.AddChild(m_object);
            // Set Icon
            //m_con.ToolBox.Icon = (Icon)PathwayResource.hand;
            //m_con.PathwayView.Icon = (Icon)PathwayResource.hand;
        }

        /// <summary>
        /// Reset EventHandler
        /// </summary>
        private void ResetEventHandler()
        {
            m_canvas.PathwayCanvas.RemoveInputEventListener(this);
            m_canvas.ControlLayer.RemoveChild(m_object);
            m_canvas = null;
            m_object = null;
            // Set Icon
            //m_con.ToolBox.Icon = (Icon)PathwayResource.arrow;
            //m_con.PathwayView.Icon = (Icon)PathwayResource.arrow;
        }
        #endregion
    }
}