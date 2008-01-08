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

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// Handler class for creating nodes (variables, process).
    /// </summary>
    public class CreateNodeMouseHandler : PPathwayInputEventHandler
    {
        #region Fields
        /// <summary>
        /// Object template.
        /// </summary>
        private PPathwayObject m_template;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">Control to diaplay the Matir2 .</param>
        /// <param name="control">Control to diaplay the Matir2 .</param>
        public CreateNodeMouseHandler(PathwayControl control, ComponentType cType)
        {
            this.m_con = control;
            this.m_template = m_con.ComponentManager.CreateTemplate(cType);
            m_template.Pickable = false;
        }
        #endregion

        #region EventHandlers
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

            if (!(e.PickedNode is PCamera))
                return;

            CanvasControl canvas = m_con.CanvasDictionary[e.Canvas.Name];
            string system = canvas.GetSurroundingSystemKey(e.Position);

            if (string.IsNullOrEmpty(system))
            {
                MessageBox.Show(m_resources.GetString("ErrOutRoot"),
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            // Create EcellObject.
            string type = ComponentManager.GetTypeString(m_template.Setting.ComponentType);
            EcellObject eo = m_con.CreateDefaultObject(canvas.ModelID, system, type, false);
            eo.X = m_template.X;
            eo.Y = m_template.Y;
            eo.Width = m_template.Width;
            eo.Height = m_template.Height;
            
            m_con.NotifyDataAdd(eo, true);
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
            m_template.Parent.RemoveChild(m_template);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set object template.
        /// </summary>
        private void SetTemplate(PInputEventArgs e)
        {
            CanvasControl canvas = m_con.CanvasDictionary[e.Canvas.Name];
            if (canvas == null)
                return;
            canvas.ControlLayer.AddChild(m_template);
            m_template.CenterPointF = e.Position;
        }

        /// <summary>
        /// Create new EcellObject.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //private EcellObject CreateNewObject(ComponentType cType)
        //{
        //    Dictionary<string, EcellData> dict = null;
        //    string type = ComponentManager.GetTypeString(m_template.Setting.ComponentType);
        //    string tmpId = m_canvas.GetTemporaryID(type, m_surSystem);
        //    string className = null;
        //    // Get ECellDatas.
        //    if (cType == ComponentType.Process)
        //    {
        //        className = "ExpressionFluxProcess";
        //        dict = m_dManager.GetProcessProperty(className);
        //    }
        //    else if (cType == ComponentType.Variable)
        //    {
        //        className = type;
        //        dict = m_dManager.GetVariableProperty();
        //    }
        //    // Change to List.
        //    List<EcellData> list = new List<EcellData>();
        //    foreach (EcellData d in dict.Values)
        //        list.Add(d);
            
        //    // Get EcellObject
        //    EcellObject eo = EcellObject.CreateObject(m_canvas.ModelID, tmpId, type, className, list);

        //    return eo;
        //}
        
        #endregion
    }
}