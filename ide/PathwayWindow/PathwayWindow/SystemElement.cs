using System;
using System.Collections.Generic;
using System.Text;
using EcellLib.PathwayWindow;

namespace EcellLib.PathwayWindow
{
    [Serializable]
    public class SystemElement : ComponentElement
    {
        #region Fields
        protected float m_x;
        protected float m_y;
        protected float m_width;
        protected float m_height;
        #endregion

        #region Accessors
        
        public float X
        {
            get { return m_x; }
            set { m_x = value; }
        }
        public float Y
        {
            get { return m_y; }
            set { m_y = value; }
        }
        public float Width
        {
            get { return m_width; }
            set { m_width = value; }
        }
        public float Height
        {
            get { return m_height; }
            set { m_height = value; }
        }
        
        #endregion

        public SystemElement()
        {
            base.m_elementType = PathwayElement.ElementType.System;
            this.m_type = "system";
        }

        public override string ToString()
        {
            string returnStr = base.ToString();

            returnStr += " [CanvasID = " + m_canvasId;
            returnStr += ", LayerID = " + m_layerId;
            returnStr += ", ModelID = " + m_modelId;
            returnStr += ", Key = " + m_key;
            returnStr += ", Type = " + m_type;
            returnStr += ", X = " + m_x;
            returnStr += ", Y = " + m_y;
            returnStr += ", Width = " + m_width;
            returnStr += ", Height = " + m_height;
            returnStr += ", CsId = " + m_csId;
            returnStr += ", Optional = " + m_optional + "]";

            return returnStr;
        }
    }
}