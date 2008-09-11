using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ecell.Objects
{
    /// <summary>
    /// EcellLayout
    /// This class contains 4 float variable and 1 string variable which show the layout of EcellObject.
    /// </summary>
    [Serializable]
    public struct EcellLayout : ICloneable
    {
        #region Fields
        /// <summary>
        /// RectangleF
        /// </summary>
        private RectangleF m_rect;
        /// <summary>
        /// Offset
        /// </summary>
        private PointF m_offset;
        /// <summary>
        /// Layer
        /// </summary>
        private string m_layer;
        /// <summary>
        /// Default layer name.
        /// </summary>
        public static readonly string DefaultLayer = "Layer0";
    		 
	    #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        public EcellLayout(PointF location)
        {
            m_rect = RectangleF.Empty;
            m_offset = PointF.Empty;
            m_layer = DefaultLayer;
            m_rect.Location = location;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        public EcellLayout(RectangleF rect)
        {
            m_rect = rect;
            m_offset = PointF.Empty;
            m_layer = DefaultLayer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public EcellLayout(float x, float y, float width, float height)
        {
            m_rect = new RectangleF(x, y, width, height);
            m_offset = new PointF();
            m_layer = DefaultLayer;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get/set the layer property.
        /// </summary>
        public string Layer
        {
            get { return m_layer; }
            set { m_layer = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public RectangleF Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        /// <summary>
        /// PointF
        /// </summary>
        public PointF Location
        {
            get { return m_rect.Location; }
            set { m_rect.Location = value; }
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public float X
        {
            get { return m_rect.X; }
            set { m_rect.X = value; }
        }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public float Y
        {
            get { return m_rect.Y; }
            set { m_rect.Y = value; }
        }

        /// <summary>
        /// Size
        /// </summary>
        public SizeF Size
        {
            get { return m_rect.Size; }
            set { m_rect.Size = value; }
        }

        /// <summary>
        /// Width
        /// </summary>
        public float Width
        {
            get { return m_rect.Width; }
            set { m_rect.Width = value; }
        }

        /// <summary>
        /// Height
        /// </summary>
        public float Height
        {
            get { return m_rect.Height; }
            set { m_rect.Height = value; }
        }

        /// <summary>
        /// Offset
        /// </summary>
        public PointF Offset
        {
            get { return m_offset; }
            set { m_offset = value; }
        }

        /// <summary>
        /// X offset
        /// </summary>
        public float OffsetX
        {
            get { return m_offset.X; }
            set { m_offset.X = value; }
        }

        /// <summary>
        /// Y offset
        /// </summary>
        public float OffsetY
        {
            get { return m_offset.Y; }
            set { m_offset.Y = value; }
        }

        /// <summary>
        /// Accessor for Center.
        /// </summary>
        public PointF Center
        {
            get { return new PointF(CenterX, CenterY); }
            set
            {
                CenterX = value.X;
                CenterY = value.Y;
            }
        }

        /// <summary>
        /// Accessor for CenterX.
        /// </summary>
        public float CenterX
        {
            get { return m_rect.X + m_rect.Width / 2f; }
            set { m_rect.X = value - m_rect.Width / 2f; }
        }

        /// <summary>
        /// Accessor for CenterY.
        /// </summary>
        public float CenterY
        {
            get { return m_rect.Y + m_rect.Height / 2f; }
            set { m_rect.Y = value - m_rect.Height / 2f; }
        }

        /// <summary>
        /// Top
        /// </summary>
        public float Top 
        {
            get { return m_rect.Top;}
        }

        /// <summary>
        /// Bottom
        /// </summary>
        public float Bottom 
        {
            get { return m_rect.Bottom; } 
        }

        /// <summary>
        /// Left
        /// </summary>
        public float Left 
        {
            get { return m_rect.Left; }
        }

        /// <summary>
        /// Right
        /// </summary>
        public float Right 
        { 
            get { return m_rect.Right; } 
        }
	    #endregion

        #region Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool Contains(PointF pt)
        {
            return m_rect.Contains(pt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool Contains(RectangleF rect)
        {
            return m_rect.Contains(rect);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        { 
            if(!(obj is EcellLayout))
                return false;
            EcellLayout layout = (EcellLayout)obj;
            return (layout.Rect == m_rect) && (layout.Offset == m_offset) && (layout.Layer == m_layer);
        }
        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Get information of this struct.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Width + ", " + Height + ", " + OffsetX + ", " + OffsetY + ", " + Layer + ")";
        }
	    #endregion

        #region ICloneable メンバ
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public EcellLayout Clone()
        {
            EcellLayout layout = new EcellLayout();
            layout.Rect = m_rect;
            layout.Offset = m_offset;
            layout.Layer = m_layer;
            return layout;
        }
        #endregion
    }
}
