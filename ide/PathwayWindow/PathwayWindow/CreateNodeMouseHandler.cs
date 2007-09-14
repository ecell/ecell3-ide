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
using EcellLib.PathwayWindow.Node;
using EcellLib.PathwayWindow.UIComponent;
using EcellLib.PathwayWindow.Element;

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
        protected PathwayView m_view;

        /// <summary>
        /// PropertyEditor. By using this, parameters for new object will be input.
        /// </summary>
        protected PropertyEditor m_editor;

        /// <summary>
        /// CanvasViewComponentSet, on which a new object will be created.
        /// </summary>
        protected CanvasView m_set;

        /// <summary>
        /// The name of the canvas, on which a new object will be created.
        /// </summary>
        protected string m_canvasName;

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
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public CreateNodeMouseHandler(PathwayView view)
        {
            this.m_view = view;
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

            if (e.PickedNode is PCamera)
            {
                m_set = m_view.CanvasDictionary[e.Canvas.Name];
                m_canvasName = ((PCamera)sender).Canvas.Name;
                m_downPos = e.Position;
                m_surSystem = m_view.CanvasDictionary[e.Canvas.Name].GetSurroundingSystem(e.Position, null);

                if (string.IsNullOrEmpty(m_surSystem))
                {
                    MessageBox.Show(m_resources.GetString("ErrOutRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                //List<EcellObject> tmpList = DataManager.GetDataManager().GetData(m_view.Window.ModelID, m_surSystem);
                
                DataManager dm = DataManager.GetDataManager();

                if (m_view.ComponentSettings[m_view.SelectedHandle.CsID].ComponentKind == ComponentType.Process)
                {
                    string tmpId = dm.GetTemporaryID(m_view.Window.ModelID, "Process", m_surSystem);
                    Dictionary<string, EcellData> dict = DataManager.GetProcessProperty(dm.CurrentProjectID, "ExpressionFluxProcess");
                    List<EcellData> list = new List<EcellData>();
                    foreach (EcellData d in dict.Values)
                    {
                        list.Add(d);
                    }
                    
                    EcellObject eo = EcellObject.CreateObject(m_view.Window.ModelID, tmpId,
                    "Process", "ExpressionFluxProcess", list);
                    eo.X = m_downPos.X;
                    eo.Y = m_downPos.Y;

                    ComponentSetting cs = m_view.ComponentSettings[m_view.SelectedHandle.CsID];

                    m_view.AddNewObj(m_canvasName, m_surSystem, ComponentType.Process, cs, eo, true, true, null);
                }
                else
                {
                    string tmpId = dm.GetTemporaryID(m_view.Window.ModelID, "Variable", m_surSystem);
                    Dictionary<string, EcellData> dict = DataManager.GetVariableProperty();
                    List<EcellData> list = new List<EcellData>();
                    foreach (EcellData d in dict.Values)
                        list.Add(d);

                    EcellObject eo = EcellObject.CreateObject(m_view.Window.ModelID, tmpId,
                    "Variable", "Variable", list);
                    eo.X = m_downPos.X;
                    eo.Y = m_downPos.Y;

                    ComponentSetting cs = m_view.ComponentSettings[m_view.SelectedHandle.CsID];

                    m_view.AddNewObj(m_canvasName, m_surSystem, ComponentType.Variable, cs, eo, true, true, null);
                }
            }
        }
    }
}