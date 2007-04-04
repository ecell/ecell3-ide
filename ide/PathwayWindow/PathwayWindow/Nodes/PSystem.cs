using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace EcellLib.PathwayWindow
{
    public class PSystem : PPathwayObject
    {
        #region Fields
        protected float m_systemWidth = 0;
        protected float m_systemHeight = 0;
        /// <summary>
        /// Indicate whether this PSystem is valid or not.
        /// </summary>
        protected bool m_valid = true;

        protected Brush m_backBrush;
        #endregion

        #region Accessors
        public float SystemWidth
        {
            get { return m_systemWidth; }
            set { m_systemWidth = value; }
        }
        public float SystemHeight
        {
            get { return m_systemHeight; }
            set { m_systemHeight = value; }
        }
        public virtual bool Valid
        {
            get { return m_valid; }
            set { m_valid = value; }
        }
        public virtual Brush BackgroundBrush
        {
            get { return m_backBrush; }
            set { m_backBrush = value; }
        }
        #endregion

        public virtual void SetRect(float x, float y, float width, float height)
        {
            base.X = x;
            base.Y = y;
            m_systemWidth = width;
            m_systemHeight = height;
        }
        public virtual void UpdateResizeHandlePositions()
        {
        }
        public override void Delete()
        {
        }
        public override bool HighLighted(bool highlight)
        {
            return true;
        }
        public override void Initialize()
        {
        }
        public override void DataChanged(EcellObject ecellObj)
        {
        }
        public override void DataDeleted()
        {
        }
        public override void SelectChanged()
        {
        }
        public override void Start()
        {
        }
        public override void Change()
        {
        }
        public override void Stop()
        {
        }
        public override void End()
        {
        }
        public override List<PathwayElement> GetElements()
        {
            return new List<PathwayElement>();
        }
        public override PPathwayObject CreateNewObject()
        {
            return new PSystem();
        }

        /// <summary>
        /// Check if this PSystem's region overlaps given rectangle
        /// </summary>
        /// <param name="rect">RectangleF to be checked</param>
        /// <returns>True if each rectangle overlaps other rectangle
        /// (doesn't contain whole rectangle)</returns>
        public virtual bool Overlaps(RectangleF rect)
        {
            RectangleF ownRect = base.GlobalBounds;
            bool isOverlapping = false;
            bool cornerInsideExists = false;
            bool cornerOutsideExists = false;

            if (ownRect.Contains(rect.X,rect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (ownRect.Contains(rect.X + rect.Width, rect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (ownRect.Contains(rect.X + rect.Width, rect.Y + rect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (ownRect.Contains(rect.X, rect.Y + rect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            isOverlapping = cornerInsideExists && cornerOutsideExists;
            cornerInsideExists = false;
            cornerOutsideExists = false;

            if (rect.Contains(ownRect.X, ownRect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if(rect.Contains(ownRect.X + ownRect.Width, ownRect.Y))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if(rect.Contains(ownRect.X + ownRect.Width, ownRect.Y + ownRect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            if (rect.Contains(ownRect.X, ownRect.Y + ownRect.Height))
                cornerInsideExists = true;
            else
                cornerOutsideExists = true;

            return isOverlapping || (cornerInsideExists && cornerOutsideExists);
        }

    }
}