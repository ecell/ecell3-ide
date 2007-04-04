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
// edited by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;
using EcellLib.PathwayWindow.Element;

namespace EcellLib.PathwayWindow.Node
{
    public class PEcellSystem : PSystem
    {
        #region Static readonly fields
        /// <summary>
        /// default length of x.
        /// </summary>
        public static readonly float MIN_X_LENGTH = 80;
        /// <summary>
        /// default length of y.
        /// </summary>
        public static readonly float MIN_Y_LENGTH = 80;
        #endregion

        #region Fields
        /// <summary>
        /// the flag whether this system is changed.
        /// </summary>
        protected bool m_isChanged = true;
        /// <summary>
        /// graphics of system.
        /// </summary>
        protected Graphics m_g;
        /// <summary>
        /// position of x before moving.
        /// </summary>
        protected float m_prevX = 0;
        /// <summary>
        /// position of y before moving.
        /// </summary>
        protected float m_prevY = 0;
        /// <summary>
        /// GraphicPath of background.
        /// </summary>
        protected GraphicsPath m_backGp;
        /// <summary>
        /// GraohicsPath of outline,
        /// </summary>
        protected GraphicsPath m_outlineGp;
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the flag whether display this system with highlight.
        /// </summary>
        public override bool IsHighLighted
        {
            get { return this.m_isSelected; }
            set
            {
                this.m_isSelected = value;
                if (value)
                {
                    this.Brush = m_highLightBrush;
                    UpdateResizeHandlePositions();
                }
                else
                {
                    this.Brush = m_normalBrush;
                    if(m_pathwayView != null)
                    {
                        //HideResizeHandles();
                    }
                }
                m_isChanged = true;
            }
        }

        /// <summary>
        /// get/set the flag whether this system is valid.
        /// </summary>
        public override bool IsInvalid
        {
            get { return this.m_isInvalid; }
            set
            {
                this.m_isInvalid = value;
                if (value)
                {
                    this.Brush = m_invalidBrush;
                    UpdateResizeHandlePositions();
                }
                else if (m_isSelected)
                    this.Brush = m_highLightBrush;
                else
                    this.Brush = m_normalBrush;
            }
        }
        
        /// <summary>
        /// get/set Pen.
        /// </summary>
        public override Pen Pen
        {
            get
            {
                return base.Pen;
            }
            set
            {
                base.pen = value;
                this.Repaint();
            }
        }
        
        /// <summary>
        /// get/set the width of this system.
        /// </summary>
        public override float SystemWidth
        {
            get { return m_systemWidth; }
            set
            {
                m_systemWidth = value;
                m_isChanged = true;
            }
        }

        /// <summary>
        /// get/set the height of this system.
        /// </summary>
        public override float SystemHeight
        {
            get { return m_systemHeight; }
            set
            {
                m_systemHeight = value;
                m_isChanged = true;
            }
        }

        /// <summary>
        /// get the position of center for the text.
        /// </summary>
        public float TextCenterX
        {
            get
            {
                return this.X + this.OffsetToLayer.X + Width / 2f;
            }
        }

        /// <summary>
        /// get the position of center for the text.
        /// </summary>
        public float TextCenterY
        {
            get
            {
                return this.Y + this.OffsetToLayer.Y + this.Height - this.Element.TextLowerMargin;
            }
        }

        /// <summary>
        /// get/set the flag whether this system is valid.
        /// </summary>
        public override bool Valid
        {
            get
            {
                return base.Valid;
            }
            set
            {
                base.Valid = value;
                if (base.Valid)
                {
                    if (this.m_isSelected)
                        this.Brush = m_highLightBrush;
                    else
                        this.Brush = m_normalBrush;
                }
                else
                    this.Brush = m_invalidBrush;
                m_isChanged = true;
            }
        }
        #endregion

        /// <summary>
        /// create object of PPathwayObject.
        /// </summary>
        /// <returns>PPathwayObject.</returns>
        public override PPathwayObject CreateNewObject()
        {
            return new PEcellSystem();
        }

        public override void AddChild(PNode child)
        {
            base.AddChild(child);
        }

        /// <summary>
        /// constructor for PEcellSystem.
        /// </summary>
        public PEcellSystem()
        {
        }

        /// <summary>
        /// convert the position at canvas to the position of system.
        /// </summary>
        /// <param name="canvasPos">the position at canvas.</param>
        /// <returns>the position of system.</returns>
        public virtual PointF CanvasPos2SystemPos(PointF canvasPos)
        {      
            canvasPos.X -= base.OffsetX;
            canvasPos.Y -= base.OffsetY;
            if (base.Parent is PLayer)
                return canvasPos;
            PNode dummyParent = null;
            do
            {
                if (dummyParent == null)
                    dummyParent = base.Parent;
                else
                    dummyParent = dummyParent.Parent;
                canvasPos.X -= dummyParent.OffsetX;
                canvasPos.Y -= dummyParent.OffsetY;
            } while (!(dummyParent.Parent is PLayer));
            return canvasPos;
        }

        /// <summary>
        /// convert the position of system to the position at canvas.
        /// </summary>
        /// <param name="systemPos">the position of system.</param>
        /// <returns>the position at canvas.</returns>
        public virtual PointF SystemPos2CanvasPos(PointF systemPos)
        {
            PNode dummyParent = null;
            systemPos.X += base.OffsetX;
            systemPos.Y += base.OffsetY;
            do
            {
                if (dummyParent == null)
                    dummyParent = this.Parent;
                else
                    dummyParent = dummyParent.Parent;
                systemPos.X += dummyParent.OffsetX;
                systemPos.Y += dummyParent.OffsetY;
            } while (!(dummyParent.Parent is PLayer));
            return systemPos;
        }

        /// <summary>
        /// set the rectangle of system at canvas.
        /// </summary>
        /// <param name="x">the position of system.</param>
        /// <param name="y">the position of system.</param>
        /// <param name="width">the width of system.</param>
        /// <param name="height">the height of system.</param>
        public override void SetRect(float x, float y, float width, float height)
        {
            base.SetRect(x, y, width, height);
            Reset();
        }

        /// <summary>
        /// reset the view object of system in canvas.
        /// </summary>
        public override void Reset()
        {
            float prevX = X;
            float prevY = Y;
            
            base.Reset();
            float thickness = SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS;
            float outerDiameter = SystemElement.OUTER_RADIUS * 2;
            float innerDiameter = SystemElement.INNER_RADIUS * 2;
            float horizontalRectWidth = m_systemWidth - 2f * SystemElement.OUTER_RADIUS;
            float verticalRectHeight = m_systemHeight - 2f * SystemElement.OUTER_RADIUS;
            GraphicsPath gp = new GraphicsPath();
            gp.AddPie(0, 0, outerDiameter, outerDiameter, 180, 90);
            gp.AddPie(thickness, thickness, innerDiameter, innerDiameter, 180, 90);
            gp.AddRectangle(new RectangleF(SystemElement.OUTER_RADIUS, 0, m_systemWidth - outerDiameter, thickness));
            gp.AddPie(m_systemWidth - outerDiameter, 0, outerDiameter, outerDiameter, 270, 90);
            gp.AddPie(m_systemWidth - outerDiameter + thickness, thickness,innerDiameter,innerDiameter,270,90);
            gp.AddRectangle(new RectangleF(m_systemWidth - thickness, SystemElement.OUTER_RADIUS, thickness, verticalRectHeight));
            gp.AddPie(m_systemWidth - outerDiameter, m_systemHeight - outerDiameter, outerDiameter, outerDiameter, 0, 90);
            gp.AddPie(m_systemWidth - outerDiameter + thickness,
                      m_systemHeight - outerDiameter + thickness, innerDiameter, innerDiameter, 0, 90);
            gp.AddRectangle(new RectangleF(SystemElement.OUTER_RADIUS, m_systemHeight - thickness, horizontalRectWidth, thickness));
            gp.AddPie(0, m_systemHeight - outerDiameter, outerDiameter, outerDiameter, 90, 90);
            gp.AddPie(thickness, m_systemHeight - outerDiameter + thickness, innerDiameter, innerDiameter, 90, 90);
            gp.AddRectangle(new RectangleF(0,SystemElement.OUTER_RADIUS,thickness,verticalRectHeight));
            
            AddPath(gp,false);
            X = prevX;
            Y = prevY;

            SetGraphicsPath();
        }

        /// <summary>
        /// get the list of element in system.
        /// </summary>
        /// <returns></returns>
        public override List<PathwayElement> GetElements()
        {
            List<PathwayElement> elementList = new List<PathwayElement>();
            
            /*SystemElement sysEle = new SystemElement();
            if(this.ECellObject != null)
            {
                sysEle.ModelID = this.m_ecellObj.modelID;
                sysEle.Key = this.m_ecellObj.key;
            }
            
            sysEle.Type = "system";
            sysEle.X = this.X + this.OffsetX;
            sysEle.Y = this.Y + this.OffsetY;
            Console.WriteLine("sysEle.X:" + sysEle.X + ", sysEle.Y:" + sysEle.Y);
            sysEle.Width = this.Width;
            sysEle.Height = this.Height;
            sysEle.CsId = this.m_csId;
            */
            foreach(PPathwayObject obj in this.ChildObjectList)
            {
                elementList.AddRange(obj.GetElements());
            }

            return elementList;
        }
        
        /// <summary>
        /// event on paint this object.
        /// </summary>
        /// <param name="paintContext">PPaintContext</param>
        protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
        {
            if(m_prevX != (this.X + this.OffsetX) || m_prevY != (this.Y + this.OffsetY))
            {
                m_isChanged = true;
            }
            
            if (m_isChanged)
            {
                this.SetGraphicsPath();
                m_isChanged = false;
            }
            Brush b = this.Brush;
            if (b != null)
                paintContext.Graphics.FillPath(b, base.path);
            
            if(m_backBrush != null)
                paintContext.Graphics.FillPath(m_backBrush, m_backGp);
            
            Pen p = null;
            if (IsHighLighted)
                p = new Pen(Brushes.DarkOrange, 1);
            else
                p = new Pen(Brushes.Blue, 1);
            paintContext.Graphics.DrawPath( p, m_outlineGp );            
            
        }

        /// <summary>
        /// event on double click this system.
        /// if there are system outside this system,
        /// it fire event at outside system.
        /// </summary>
        /// <param name="e"></param>
        public override void OnDoubleClick(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            if (!(e.PickedNode is PEcellSystem)) return;
            PEcellSystem p = (PEcellSystem)e.PickedNode;
            if (!p.Element.Key.Equals(Element.Key)) return;

            EcellObject obj = m_set.View.GetData(Element.Key, "System");

            PropertyEditor editor = new PropertyEditor();
            editor.layoutPanel.SuspendLayout();
            editor.SetCurrentObject(obj);
            editor.SetDataType(obj.type);
            editor.button1.Click += new EventHandler(editor.UpdateProperty);
            editor.LayoutPropertyEditor();
            editor.layoutPanel.ResumeLayout(false);
            editor.ShowDialog();
        }

        /// <summary>
        /// set GraphicsPath.
        /// </summary>
        public void SetGraphicsPath()
        {
            float thickness = SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS;
            float outerDiameter = SystemElement.OUTER_RADIUS * 2;
            float innerDiameter = SystemElement.INNER_RADIUS * 2;
            float horizontalRectWidth = m_systemWidth - 2f * SystemElement.OUTER_RADIUS;
            float verticalRectHeight = m_systemHeight - 2f * SystemElement.OUTER_RADIUS;


//            if (base.m_backBrush != null)
//            {
                m_backGp = new GraphicsPath();
                m_backGp.FillMode = FillMode.Alternate;
                m_backGp.AddRectangle(new RectangleF(X + thickness,
                                                   Y + thickness,
                                                   m_systemWidth - 2 * thickness,
                                                   m_systemHeight - 2 * thickness));
                m_backGp.AddRectangle(new RectangleF(X + thickness,
                                                   Y + thickness,
                                                   SystemElement.INNER_RADIUS,
                                                   SystemElement.INNER_RADIUS));
                m_backGp.AddRectangle(new RectangleF(X + m_systemWidth - SystemElement.OUTER_RADIUS,
                                                   Y + thickness,
                                                   SystemElement.INNER_RADIUS,
                                                   SystemElement.INNER_RADIUS));
                m_backGp.AddRectangle(new RectangleF(X + m_systemWidth - SystemElement.OUTER_RADIUS,
                                                   Y + m_systemHeight - SystemElement.OUTER_RADIUS,
                                                   SystemElement.INNER_RADIUS,
                                                   SystemElement.INNER_RADIUS));
                m_backGp.AddRectangle(new RectangleF(X + thickness,
                                                   Y + m_systemHeight - SystemElement.OUTER_RADIUS,
                                                   SystemElement.INNER_RADIUS,
                                                   SystemElement.INNER_RADIUS));
                m_backGp.AddPie(X + thickness, Y + thickness, innerDiameter, innerDiameter, 180, 90);
                m_backGp.AddPie(X + m_systemWidth - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS, Y + thickness,
                               innerDiameter, innerDiameter, 270, 90);
                m_backGp.AddPie(X + m_systemWidth - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS, 
                                Y + m_systemHeight - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS,
                                innerDiameter, innerDiameter, 0, 90);
                m_backGp.AddPie(X + thickness, Y + m_systemHeight - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS,
                               innerDiameter, innerDiameter, 90, 90);
                
//            }

            m_outlineGp = new GraphicsPath();
            m_outlineGp.FillMode = FillMode.Alternate;
            m_outlineGp.AddArc(X, Y, outerDiameter, outerDiameter, 180, 90);
            m_outlineGp.AddArc(X + m_systemWidth - outerDiameter, Y, outerDiameter, outerDiameter, 270, 90);
            m_outlineGp.AddArc(X + m_systemWidth - outerDiameter, Y + m_systemHeight - outerDiameter,
                      outerDiameter, outerDiameter, 0, 90);
            m_outlineGp.AddArc(X, Y + m_systemHeight - outerDiameter, outerDiameter, outerDiameter, 90, 90);
            m_outlineGp.AddLine(X, Y + SystemElement.OUTER_RADIUS, X, Y + m_systemHeight - SystemElement.OUTER_RADIUS);
            m_outlineGp.CloseFigure();
            m_outlineGp.AddArc(X + thickness, Y + thickness, innerDiameter, innerDiameter, 180, 90);
            m_outlineGp.AddArc(X + m_systemWidth - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS,
                      Y + thickness, innerDiameter, innerDiameter, 270, 90);
            m_outlineGp.AddArc(X + m_systemWidth - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS,
                      Y + m_systemHeight - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS,
                      innerDiameter, innerDiameter, 0, 90);
            m_outlineGp.AddArc(X + thickness, Y + m_systemHeight - SystemElement.OUTER_RADIUS - SystemElement.INNER_RADIUS, innerDiameter, innerDiameter, 90, 90);
            m_outlineGp.AddLine(X + thickness, Y + SystemElement.OUTER_RADIUS, X + thickness, Y + m_systemHeight - SystemElement.OUTER_RADIUS);            
        }
    }    
}