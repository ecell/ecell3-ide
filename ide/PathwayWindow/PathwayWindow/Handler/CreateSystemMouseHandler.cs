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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Nodes;
using Ecell.Objects;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Nodes;

namespace Ecell.IDE.Plugins.PathwayWindow.Handler
{
    /// <summary>
    /// For creating a system.
    /// When [Create System] menu (rectangle on it) is selected in the toolbar, mouse event will be
    /// dealed with this handler.
    /// </summary>
    class CreateSystemMouseHandler : PPathwayInputEventHandler
    {
        #region Fields
        /// <summary>
        /// 
        /// </summary>
        protected CanvasControl m_canvas;
        
        /// <summary>
        /// 
        /// </summary>
        protected PointF m_startPoint;

        /// <summary>
        /// 
        /// </summary>
        protected PPath m_selectedPath;

        /// <summary>
        /// A system, which surrounds the position where the mouse was pressed down.
        /// </summary>
        protected string m_surSystem;

        /// <summary>
        /// m_invalidPen is used for writing rectangle in which new system will be created.
        /// If surrounding area is too small to create new system, this pen will be used.
        /// Otherwise, m_validPen will be used.
        /// </summary>
        private readonly Pen InvalidPen = new Pen(Brushes.Black, 1);

        /// <summary>
        /// m_validPen is used for writing rectangle in which new system will be created.
        /// If surrounding area has enough size for creating new system, this pen will be used.
        /// Otherwise, m_invalidPen will be used.
        /// </summary>
        private readonly Pen ValidPen = new Pen(Brushes.Blue, 5);

        /// <summary>
        /// m_overlapPen is used for writing rectangle in which new system will be created.
        /// If surrounding area has enough size for creating new system but is overlapping other
        /// systems, m_overlapPen will be used.
        /// </summary>
        private readonly Pen OverlapPen = new Pen(Brushes.Red, 5);

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control">PathwayControl instance</param>
        public CreateSystemMouseHandler(PathwayControl control)
            : base(control)
        {
            m_selectedPath = new PPath();
        }

        public override void Initialize()
        {
            base.Initialize();
            if (m_con.Canvas == null)
                return;
            m_con.Canvas.PCanvas.Cursor = Cursors.Cross;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            if (m_con.Canvas == null)
                return;
            m_con.Canvas.PCanvas.Cursor = Cursors.Arrow;
            m_selectedPath.Reset();
            m_startPoint = PointF.Empty;
            m_surSystem = null;
        }

        /// <summary>
        /// Get the flag whether system accept this action.
        /// </summary>
        /// <param name="e">PInputEventArgs</param>
        /// <returns>The judgement whether this action is acceped.</returns>
        public override bool DoesAcceptEvent(PInputEventArgs e)
        {
            return true;
        }

        /// <summary>
        /// Called when the mouse is down on the pathway canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {            
            base.OnMouseDown(sender, e);
            // if Button != MouseButtons.Left, return.
            if (e.Button != MouseButtons.Left)
                return;
            // if PickedNode 
            if (e.PickedNode is PPathwayObject || e.PickedNode is PPathwayEdge)
                return;

            m_canvas = m_con.Canvas;
            m_selectedPath.Reset();

            if (e.PickedNode is PCamera)
            {
                m_surSystem = m_canvas.GetSurroundingSystemKey(e.Position);

                if (string.IsNullOrEmpty(m_surSystem))
                {
                    Util.ShowErrorDialog(MessageResources.ErrOutRoot);
                    return;
                }

                m_startPoint = e.Position;
                e.Canvas.Layer.AddChild(m_selectedPath);
            }
        }
        
        /// <summary>
        /// Called when the mouse is being dragged
        /// </summary>
        /// <param name="sender">event sender. maybe, PLayer</param>
        /// <param name="e">event information</param>
        public override void OnMouseDrag(object sender, PInputEventArgs e)
        {
            base.OnMouseDrag(sender, e);
            if (string.IsNullOrEmpty(m_surSystem))
                return;

            m_selectedPath.Reset();
            
            RectangleF rect = PathUtil.GetRectangle(m_startPoint, e.Position);
            m_selectedPath.AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);
            // Set pen.
            if (rect.Width < PPathwaySystem.MIN_WIDTH || rect.Height < PPathwaySystem.MIN_HEIGHT)
            {
                // When mouse surrounding region is smaller than minimum.
                m_selectedPath.Pen = InvalidPen;
            }
            else if (m_canvas.DoesSystemOverlaps(rect))
            {
                // When mouse surrounding region overlaps other system
                m_selectedPath.Pen = OverlapPen;
                m_selectedPath.CloseFigure();
                m_selectedPath.AddLine(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                m_selectedPath.CloseFigure();
                m_selectedPath.AddLine(rect.X, rect.Y + rect.Height, rect.X + rect.Width, rect.Y);
            }
            else
            {
                // When a system could be created successfully if mouse is up.
                m_selectedPath.Pen = ValidPen;
            }

            // Set Highlight.
            m_canvas.ResetSelect();
            List<PPathwayObject> newlySelectedList = m_canvas.GetSurroundedObject(rect);
            foreach (PPathwayObject node in newlySelectedList)
                m_canvas.AddSelect(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(object sender, PInputEventArgs e)
        {
            base.OnMouseUp(sender, e);
            if (string.IsNullOrEmpty(m_surSystem))
                return;

            m_selectedPath.RemoveFromParent();
            m_selectedPath.Reset();

            RectangleF rect = PathUtil.GetRectangle(m_startPoint, e.Position);
            // Check size
            if (rect.Width <= PPathwaySystem.MIN_WIDTH || rect.Height <= PPathwaySystem.MIN_HEIGHT)
                return;
            // Check overlap
            if (m_canvas.DoesSystemOverlaps(rect))
            {
                Util.ShowErrorDialog(MessageResources.ErrOverSystem);
                Reset();
                return;
            }
            // Check Aliases
            foreach (PPathwayVariable variable in m_canvas.Variables.Values)
            {
                if(variable.Aliases.Count <= 0)
                    continue;
                bool contein = rect.Contains(variable.CenterPointF);
                foreach(PPathwayAlias alias in variable.Aliases)
                {
                    if(rect.Contains(alias.CenterPointF) == contein)
                        continue;

                    Util.ShowErrorDialog(MessageResources.ErrOutSystemAlias);
                    return;
                }
            }


            EcellObject eo = m_con.CreateDefaultObject(m_canvas.ModelID, m_surSystem, EcellObject.SYSTEM);

            eo.X = rect.X;
            eo.Y = rect.Y;
            eo.Width = rect.Width;
            eo.Height = rect.Height;
            eo.IsLayouted = true;
            m_con.NotifyDataAdd(eo, false);

            m_canvas.ResetSelect();
            // Add system.
            // Move Children.
            m_canvas.NotifyMoveObjects(false);

            // Reset System.
            PPathwayObject obj = m_canvas.GetObject(eo.Key, eo.Type);
            if (obj != null)
            {
                m_con.NotifyDataChanged(obj, true);
                m_con.Canvas.NotifySelectChanged(obj);
            }
            m_con.Menu.ResetEventHandler();
            Reset();
        }
    }
}
