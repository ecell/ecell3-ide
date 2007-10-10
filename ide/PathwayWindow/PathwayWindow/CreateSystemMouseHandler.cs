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
using PathwayWindow;
using System.Windows.Forms;
using System.Threading;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.Piccolo.Util;
using EcellLib.PathwayWindow.Nodes;
using EcellLib.PathwayWindow.UIComponent;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// For creating a system.
    /// When [Create System] menu (rectangle on it) is selected in the toolbar, mouse event will be
    /// dealed with this handler.
    /// </summary>
    class CreateSystemMouseHandler : PBasicInputEventHandler
    {
        #region Fields
        protected PathwayView m_view;

        protected CanvasView m_set;

        /// <summary>
        /// PropertyEditor. By using this, parameters for new object will be input.
        /// </summary>
        //protected PropertyEditor m_editor;

        protected PointF m_startPoint;
        protected PPath m_selectedPath;

        /// <summary>
        /// Rectangle, surrounded by mouse.
        /// </summary>
        protected RectangleF m_rect;

        /// <summary>
        /// A system, which surrounds the position where the mouse was pressed down.
        /// </summary>
        protected string m_surSystem;

        /// <summary>
        /// m_invalidPen is used for writing rectangle in which new system will be created.
        /// If surrounding area is too small to create new system, this pen will be used.
        /// Otherwise, m_validPen will be used.
        /// </summary>
        protected Pen m_invalidPen = new Pen(Brushes.Black,1);

        /// <summary>
        /// m_validPen is used for writing rectangle in which new system will be created.
        /// If surrounding area has enough size for creating new system, this pen will be used.
        /// Otherwise, m_invalidPen will be used.
        /// </summary>
        protected Pen m_validPen = new Pen(Brushes.Blue, 5);

        /// <summary>
        /// m_overlapPen is used for writing rectangle in which new system will be created.
        /// If surrounding area has enough size for creating new system but is overlapping other
        /// systems, m_overlapPen will be used.
        /// </summary>
        protected Pen m_overlapPen = new Pen(Brushes.Red, 5);

        /// <summary>
        /// minimum width and height of System
        /// </summary>
        protected float m_minSystemWidth = 40;
        protected float m_minSystemHeight = 40;

        /// <summary>
        /// ResourceManager for PathwayWindow.
        /// </summary>
        ComponentResourceManager m_resources = new ComponentResourceManager(typeof(MessageResPathway));


        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        public CreateSystemMouseHandler(PathwayView view)
        {
            this.m_view = view;
        }

        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return e.Button != MouseButtons.Right;
        }

        bool m_isNode = false;
        /// <summary>
        /// Called when the mouse is down on the pathway canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {            
            base.OnMouseDown(sender, e);

            if (e.PickedNode is PCamera)
            {
                m_surSystem = m_view.CanvasDictionary[e.Canvas.Name].GetSurroundingSystemKey(e.Position);

                if (string.IsNullOrEmpty(m_surSystem))
                {
                    MessageBox.Show(m_resources.GetString("ErrOutRoot"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                m_startPoint = e.Position;
                m_selectedPath = new PPath();
                e.Canvas.Layer.AddChild(m_selectedPath);
                m_set = m_view.CanvasDictionary[e.Canvas.Name];
                if (m_set != null)
                    m_set.ResetSelectedObjects();
                m_isNode = false;
            }
            else
            {
                m_isNode = true;
            }
        }
        
        /// <summary>
        /// Called when the mouse is being dragged
        /// </summary>
        /// <param name="sender">event sender. maybe, PLayer</param>
        /// <param name="e">event information</param>
        public override void OnMouseDrag(object sender, PInputEventArgs e)
        {
            if (m_isNode) return;
            base.OnMouseDrag(sender, e);

            if (m_selectedPath == null)
                return;

            m_selectedPath.Reset();
            
            RectangleF rect = PathUtil.GetRectangle(m_startPoint, e.Position);
            m_selectedPath.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);
            if( m_minSystemWidth > rect.Width || m_minSystemHeight > rect.Height)
            {
                // When mouse surrounding region is smaller than minimum.
                m_selectedPath.Pen = m_invalidPen;
            }
            else if (m_view.CanvasDictionary[e.Canvas.Name].DoesSystemOverlaps(rect))
            {
                // When mouse surrounding region overlaps other system
                m_selectedPath.Pen = m_overlapPen;
                m_selectedPath.CloseFigure();
                m_selectedPath.AddLine(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                m_selectedPath.CloseFigure();
                m_selectedPath.AddLine(rect.X, rect.Y + rect.Height, rect.X + rect.Width, rect.Y);
            }
            else
            {
                // When a system could be created successfully if mouse is up.
                m_selectedPath.Pen = m_validPen;
            }
            
            PNodeList newlySelectedList = new PNodeList();
            m_view.CanvasDictionary[((PCamera)sender).Canvas.Name].ResetSelectedObjects();
            foreach(PLayer layer in m_view.CanvasDictionary[e.Canvas.Name].Layers.Values)
            {
                PNodeList list = new PNodeList();
                layer.FindIntersectingNodes(rect, list);
                newlySelectedList.AddRange(list);
            }

            foreach(PNode node in newlySelectedList)
            {
                if(node is PPathwayNode)
                {
                    m_view.CanvasDictionary[((PCamera)sender).Canvas.Name].AddSelectedNode((PPathwayNode)node, false);
                }
            }
        }

        public override void OnMouseUp(object sender, PInputEventArgs e)
        {
            if (m_isNode) return;
            base.OnMouseUp(sender, e);

            if (m_startPoint == PointF.Empty || m_selectedPath == null)
            {
                m_startPoint = PointF.Empty;
                return;
            }

            m_selectedPath.Reset();
            if(m_selectedPath != null && m_selectedPath.Parent != null)
            {
                m_selectedPath.Parent.RemoveChild(m_selectedPath);
            }
            m_rect = PathUtil.GetRectangle(m_startPoint, e.Position);

            if (m_rect.Width >= 40 && m_rect.Height >= 40)
            {
                if (m_view.CanvasDictionary[e.Canvas.Name].DoesSystemOverlaps(m_rect))
                {
                    MessageBox.Show(m_resources.GetString("ErrOverSystem"),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Stop);
                    return;
                }

                List<EcellObject> tmpList = DataManager.GetDataManager().GetData(m_view.Window.ModelID, m_surSystem);
                EcellObject m_currentObj = null;
                if (tmpList.Count > 0) m_currentObj = tmpList[0];
                
                String tmpID = DataManager.GetDataManager().GetTemporaryID(m_currentObj.modelID,
                    "System", m_currentObj.key);

                Dictionary<string, EcellData> dict = DataManager.GetSystemProperty();
                List<EcellData> dataList = new List<EcellData>();
                foreach (EcellData d in dict.Values)
                {
                    dataList.Add(d);
                }

                List<PPathwayObject> newlySelectedList = new List<PPathwayObject>();

                foreach (PLayer layer in m_set.Layers.Values)
                {
                    PNodeList list = new PNodeList();
                    layer.FindIntersectingNodes(m_rect, list);
                    foreach (PNode node in list)
                        if (node is PPathwayObject)
                            newlySelectedList.Add((PPathwayObject)node);
                }

                EcellObject eo = EcellObject.CreateObject(m_currentObj.modelID, tmpID, "System", "System", dataList);

                eo.X = m_rect.X;
                eo.Y = m_rect.Y;
                eo.Width = m_rect.Width;
                eo.Height = m_rect.Height;

                m_view.NotifyDataAdd(eo, true);
                
                foreach (PPathwayObject node in newlySelectedList)
                {
                    m_set.TransferNodeToByResize(eo.key, node, true);
                }                
            }
            else
            {
                m_set.ResetSelectedObjects();
            }
            m_startPoint = PointF.Empty;
        }

    }

}