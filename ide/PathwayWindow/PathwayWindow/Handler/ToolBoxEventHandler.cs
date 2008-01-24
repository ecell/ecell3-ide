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
using System.Windows.Forms;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.Resources;
using System.Drawing;

namespace EcellLib.PathwayWindow.Handler
{
    public class ToolBoxEventHandler : PPathwayInputEventHandler
    {
        #region Fields
        /// <summary>
        /// Current canvas.
        /// </summary>
        CanvasControl m_canvas;
        /// <summary>
        /// flag of EventHandler state.
        /// </summary>
        bool m_flag = false;
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
        public ToolBoxEventHandler(PathwayControl control)
        {
            base.m_con = control;
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Event on mouse move.
        /// Move template object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(object sender, PInputEventArgs e)
        {
            base.OnMouseMove(sender, e);

            if (!m_flag || m_canvas == null || e.Canvas != m_canvas.PathwayCanvas)
                return;
            if (m_object == null)
                return;
            m_canvas.ControlLayer.AddChild(m_object);
            m_object.CenterPointF = e.Position;
        }

        /// <summary>
        /// Event on mouse up.
        /// Check EventHandler status and Set/Reset EventHandler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);

            if (!m_flag)
            {
                // Set EventHandler for current canvas.
                if (m_con.ActiveCanvas == null)
                    return;
                if (!(e.Canvas is PToolBoxCanvas))
                    return;
                SetEventHandler((PToolBoxCanvas)e.Canvas);
            }
            else if (e.Canvas == m_canvas.PathwayCanvas)
            {
                // Add new object and reset EventHandler.
                if(e.Button == MouseButtons.Left)
                    AddObject(e);
                ResetEventHandler();
            }
            else
            {
                // Reset EventHandler.
                ResetEventHandler();
                if (!(e.Canvas is PToolBoxCanvas))
                    return;
                SetEventHandler((PToolBoxCanvas)e.Canvas);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return base.DoesAcceptEvent(e) || e.Button == MouseButtons.Right;
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
            EcellObject eo = m_con.CreateDefaultObject(m_canvas.ModelID, systemKey, type, false);
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
            m_object.Refresh();
            m_flag = true;
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
            if(m_object.Parent != null)
                m_object.Parent.RemoveChild(m_object);

            m_canvas = null;
            m_object = null;
            m_flag = false;
            // Set Icon
            //m_con.ToolBox.Icon = (Icon)PathwayResource.arrow;
            //m_con.PathwayView.Icon = (Icon)PathwayResource.arrow;
        }
        #endregion
    }
}
