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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.IDE.Plugins.PathwayWindow.UIComponent;
using Ecell.Objects;
using Ecell.IDE.Plugins.PathwayWindow.Components;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// Handler class for creating nodes (variables, process).
    /// </summary>
    public class CreateNodeMouseHandler : PPathwayInputEventHandler
    {
        #region Fields
        private ComponentSetting m_cs = null;
        /// <summary>
        /// Object template.
        /// </summary>
        private PPathwayObject m_template = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">PathwayControl instance.</param>
        /// <param name="cs">ComponentSetting</param>
        public CreateNodeMouseHandler(PathwayControl control, ComponentSetting cs)
            : base(control)
        {
            this.m_cs = cs;
            this.m_template = m_cs.CreateTemplate();
            this.m_template.Pickable = false;
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Get the flag whether system accept this events.
        /// </summary>
        /// <param name="e">Target events.</param>
        /// <returns>The judgement whether this event is accepted.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return true;
        }        

        /// <summary>
        /// Called when the mouse is move on the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(object sender, PInputEventArgs e)
        {
            base.OnMouseMove(sender, e);
            SetTemplate(e);
        }

        /// <summary>
        /// Called when the mouse is down on the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);

            // Cancel
            if (!(e.PickedNode is PCamera))
                return;
            if (e.Button == MouseButtons.Right)
            {
                m_con.Menu.ResetEventHandler();
                return;
            }

            // Get parent system.
            CanvasControl canvas = m_con.Canvas;
            string system = canvas.GetSurroundingSystemKey(e.Position);
            if (m_template is PPathwayText)
            {
                system = Constants.delimiterPath;
            }
            else if (string.IsNullOrEmpty(system))
            {
                Util.ShowErrorDialog(MessageResources.ErrOutRoot);
                return;
            }

            // Create EcellObject.
            EcellObject eo = m_con.CreateDefaultObject(canvas.ModelID, system, m_cs.Type);
            eo.X = m_template.X;
            eo.Y = m_template.Y;
            eo.Width = m_template.Width;
            eo.Height = m_template.Height;

            m_con.NotifyDataAdd(eo, true);
            PPathwayObject obj = m_con.Canvas.GetObject(eo.Key, eo.Type);
            if(obj != null)
                m_con.Canvas.NotifySelectChanged(obj);
            m_con.Menu.ResetEventHandler();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Method to initialize EventHandler status.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Reset();
        }

        /// <summary>
        /// Method to reset EventHandler status.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            if (m_template.Parent == null)
                return;
            m_template.RemoveFromParent();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set object template.
        /// </summary>
        private void SetTemplate(PInputEventArgs e)
        {
            CanvasControl canvas = m_con.Canvas;
            if (canvas == null)
                return;
            canvas.ControlLayer.AddChild(m_template);
            m_template.CenterPointF = e.Position;
            m_template.Pickable = false;
            m_template.RefreshView();
            m_template.Visible = true;
        }
        #endregion
    }
}
