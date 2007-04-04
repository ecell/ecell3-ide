using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo;

namespace EcellLib.PathwayWindow
{
    public class PEcellSystem : PSystem
    {
        #region Fields
        protected float m_outerRadius = 20f;
        protected float m_innerRadius = 10f;

        protected string m_csId;

        protected Brush m_normalBrush = Brushes.LightBlue;
        protected Brush m_highLightBrush = Brushes.Gold;
        protected Brush m_invalidBrush = Brushes.Red;

        protected PNodeList m_resizeHandles = new PNodeList();
        protected float m_halfThickness = 0;
        protected float m_resizeHandleWidth = 20;

        protected PointF m_mouseDownPos;

        protected PointF upperLeftPoint;
        protected PointF upperRightPoint;
        protected PointF lowerRightPoint;
        protected PointF lowerLeftPoint;
        #endregion

        #region Accessors
        public string CsID
        {
            get { return m_csId; }
            set { m_csId = value; }
        }
        public float HalfThickness
        {
            get {
                if(m_halfThickness == 0)
                    m_halfThickness = (m_outerRadius - m_innerRadius) / 2f;
                return m_halfThickness;
            }
        }
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
                    //ShowResizeHandles();
                }
                else
                {
                    this.Brush = m_normalBrush;
                    if(m_pathwayView != null)
                    {
                        //HideResizeHandles();
                    }

                }
            }
        }
        public override bool Valid
        {
            get
            {
                return base.Valid;
            }
            set
            {
                base.Valid = value;
                if(base.Valid)
                {
                    if (this.m_isSelected)
                        this.Brush = m_highLightBrush;
                    else
                        this.Brush = m_normalBrush;
                }
                else
                    this.Brush = m_invalidBrush;
            }
        }
        #endregion

        public override PPathwayObject CreateNewObject()
        {
            return new PEcellSystem();
        }
        public PEcellSystem()
        {
        }
        public override void SetRect(float x, float y, float width, float height)
        {
            base.SetRect(x, y, width, height);
            Reset();
        }
        public override void Reset()
        {
            float prevX = X;
            float prevY = Y;
            
            base.Reset();
            float thickness = m_outerRadius - m_innerRadius;
            float outerDiameter = m_outerRadius * 2;
            float innerDiameter = m_innerRadius * 2;
            float horizontalRectWidth = m_systemWidth - 2f * m_outerRadius;
            float verticalRectHeight = m_systemHeight - 2f * m_outerRadius;
            GraphicsPath gp = new GraphicsPath();
            gp.AddPie(0, 0, outerDiameter, outerDiameter, 180, 90);
            gp.AddPie(thickness, thickness, innerDiameter, innerDiameter, 180, 90);
            gp.AddRectangle(new RectangleF(m_outerRadius, 0, m_systemWidth - outerDiameter, thickness));
            gp.AddPie(m_systemWidth - outerDiameter, 0, outerDiameter, outerDiameter, 270, 90);
            gp.AddPie(m_systemWidth - outerDiameter + thickness, thickness,innerDiameter,innerDiameter,270,90);
            gp.AddRectangle(new RectangleF(m_systemWidth - thickness, m_outerRadius, thickness, verticalRectHeight));
            gp.AddPie(m_systemWidth - outerDiameter, m_systemHeight - outerDiameter, outerDiameter, outerDiameter, 0, 90);
            gp.AddPie(m_systemWidth - outerDiameter + thickness,
                      m_systemHeight - outerDiameter + thickness, innerDiameter, innerDiameter, 0, 90);
            gp.AddRectangle(new RectangleF(m_outerRadius, m_systemHeight - thickness, horizontalRectWidth, thickness));
            gp.AddPie(0, m_systemHeight - outerDiameter, outerDiameter, outerDiameter, 90, 90);
            gp.AddPie(thickness, m_systemHeight - outerDiameter + thickness, innerDiameter, innerDiameter, 90, 90);
            gp.AddRectangle(new RectangleF(0,m_outerRadius,thickness,verticalRectHeight));
            
            AddPath(gp,false);

            X = prevX;
            Y = prevY;
        }
        public override void OnMouseLeave(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            //base.OnMouseLeave(e);
            e.Canvas.Cursor = Cursors.Default;
        }
        public override void OnMouseEnter(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseEnter(e);
            //e.Canvas.Cursor = Cursors.SizeNWSE;
        }
        public override List<PathwayElement> GetElements()
        {
            List<PathwayElement> elementList = new List<PathwayElement>();
            SystemElement sysEle = new SystemElement();
            if(this.ECellObject != null)
            {
                sysEle.ModelID = this.m_ecellObj.modelID;
                sysEle.Key = this.m_ecellObj.key;
            }
            sysEle.Type = "system";
            sysEle.X = this.X + this.OffsetX;
            sysEle.Y = this.Y + this.OffsetY;
            sysEle.Width = this.Width;
            sysEle.Height = this.Height;
            sysEle.CsId = this.m_csId;

            foreach(PPathwayObject obj in this.ChildObjectList)
            {
                elementList.AddRange(obj.GetElements());
            }

            return elementList;
        }
        protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
        {
            Brush b = this.Brush;
            Graphics g = paintContext.Graphics;

            if (b != null)
            {
                g.FillPath(b, base.path);
            }

            float thickness = m_outerRadius - m_innerRadius;
            float outerDiameter = m_outerRadius * 2;
            float innerDiameter = m_innerRadius * 2;
            float horizontalRectWidth = m_systemWidth - 2f * m_outerRadius;
            float verticalRectHeight = m_systemHeight - 2f * m_outerRadius;
            
            Pen p = null;
            if (IsHighLighted)
                p = new Pen(Brushes.DarkOrange, 1);
            else
                p = new Pen(Brushes.Blue, 1);
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(X, Y, outerDiameter, outerDiameter, 180, 90);
            gp.AddArc(X + m_systemWidth - outerDiameter, Y, outerDiameter, outerDiameter, 270, 90);
            gp.AddArc(X + m_systemWidth - outerDiameter, Y + m_systemHeight - outerDiameter,
                      outerDiameter, outerDiameter, 0, 90);
            gp.AddArc(X, Y + m_systemHeight - outerDiameter, outerDiameter, outerDiameter, 90, 90);
            gp.AddLine(X, Y + m_outerRadius, X, Y+ m_systemHeight - m_outerRadius);
            g.DrawPath(p, gp);
            gp.Reset();
            gp.AddArc(X + thickness, Y + thickness, innerDiameter, innerDiameter, 180, 90 );
            gp.AddArc(X + m_systemWidth - m_outerRadius - m_innerRadius,
                      Y + thickness, innerDiameter, innerDiameter, 270, 90);
            gp.AddArc(X + m_systemWidth - m_outerRadius - m_innerRadius,
                      Y + m_systemHeight - m_outerRadius - m_innerRadius,
                      innerDiameter, innerDiameter, 0, 90);
            gp.AddArc(X + thickness, Y + m_systemHeight - m_outerRadius - m_innerRadius, innerDiameter, innerDiameter, 90, 90);
            gp.AddLine(X + thickness, Y + m_outerRadius, X + thickness, Y + m_systemHeight - m_outerRadius);
            g.DrawPath(p, gp);

            if(base.m_backBrush != null)
            {
                GraphicsPath backGp = new GraphicsPath();
                
                backGp.AddRectangle(new RectangleF(X + m_outerRadius - m_innerRadius,
                                                   Y + m_outerRadius - m_innerRadius,
                                                   m_systemWidth - 2 * thickness,
                                                   m_systemHeight - 2 * thickness));
                g.FillPath(m_backBrush, backGp);
            }
        }
        
    }

    
}