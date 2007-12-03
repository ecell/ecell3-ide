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
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.UIComponent;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Handler class for creating nodes (variables, process).
    /// </summary>
    public class CreateNodeMouseHandler : PBasicInputEventHandler
    {
        /// <summary>
        /// PathwayView
        /// </summary>
        protected PathwayControl m_con;

        /// <summary>
        /// PropertyEditor. By using this, parameters for new object will be input.
        /// </summary>
        protected PropertyEditor m_editor;

        /// <summary>
        /// CanvasViewComponentSet, on which a new object will be created.
        /// </summary>
        protected CanvasControl m_canvas;

        /// <summary>
        /// Where mouse was pressed down on pathwaycanvas.
        /// </summary>
        protected PointF m_downPos;

        /// <summary>
        /// A system, which surrounds the position where the mouse was pressed down.
        /// </summary>
        protected string m_surSystem;
        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));

        /// <summary>
        /// DataManage instance associated to this object.
        /// </summary>
        DataManager m_dManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dManager">A DataManager instance to associate.</param>
        /// <param name="control">Control to diaplay the Matir2 .</param>
        public CreateNodeMouseHandler(PathwayControl control)
        {
            this.m_con = control;
            this.m_dManager = control.Window.DataManager;
        }

        /// <summary>
        /// Return whether this handler accepts a certain event or not.
        /// </summary>
        /// <param name="e">checked event</param>
        /// <returns>true if this handler accepts an event, false otherwise</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.Button != MouseButtons.Right;
        }

        /// <summary>
        /// Called when the mouse is down on the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);

            if (!(e.PickedNode is PCamera))
                return;

            m_canvas = m_con.CanvasDictionary[e.Canvas.Name];
            m_downPos = e.Position;
            m_surSystem = m_con.CanvasDictionary[e.Canvas.Name].GetSurroundingSystemKey(e.Position);

            if (string.IsNullOrEmpty(m_surSystem))
            {
                MessageBox.Show(m_resources.GetString("ErrOutRoot"),
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            // Create EcellObject.
            EcellObject eo = CreateNewObject(m_con.SelectedHandle.CsID);
            eo.X = m_downPos.X;
            eo.Y = m_downPos.Y;
            eo.Width = PPathwayNode.DEFAULT_WIDTH;
            eo.Height = PPathwayNode.DEFAULT_HEIGHT;

            m_con.NotifyDataAdd(eo, true);
        }
        /// <summary>
        /// Create new EcellObject.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private EcellObject CreateNewObject(ComponentType cType)
        {
            Dictionary<string, EcellData> dict = null;
            string type = ComponentManager.GetTypeString(cType);
            string tmpId = m_canvas.GetTemporaryID(type, m_surSystem);
            string className = null;
            // Get ECellDatas.
            if (cType == ComponentType.Process)
            {
                className = "ExpressionFluxProcess";
                dict = m_dManager.GetProcessProperty(className);
            }
            else if (cType == ComponentType.Variable)
            {
                className = type;
                dict = m_dManager.GetVariableProperty();
            }
            // Change to List.
            List<EcellData> list = new List<EcellData>();
            foreach (EcellData d in dict.Values)
                list.Add(d);
            
            // Get EcellObject
            EcellObject eo = EcellObject.CreateObject(m_canvas.ModelID, tmpId, type, className, list);

            return eo;
        }
    }
}