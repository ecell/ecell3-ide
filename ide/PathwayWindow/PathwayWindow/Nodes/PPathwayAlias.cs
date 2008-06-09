using System;
using System.Collections.Generic;
using System.Text;

namespace EcellLib.PathwayWindow.Nodes
{
    class PPathwayAlias : PPathwayObject
    {
        PPathwayVariable m_variable = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        public PPathwayAlias(PPathwayVariable variable)
        {
            m_variable = variable;
            m_canvas = variable.Canvas;
        }

        public override PPathwayObject CreateNewObject()
        {
            return null;
        }

        public override void OnMouseDown(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            m_variable.OnMouseDown(e);
        }

        internal void NotifyDataChanged()
        {
            m_ecellObj.X = this.X + this.OffsetX;
            m_ecellObj.Y = this.Y + this.OffsetY;
            m_variable.Canvas.Control.NotifyDataChanged(m_ecellObj.Key, m_variable.EcellObject, true, true);
        }
    }
}
