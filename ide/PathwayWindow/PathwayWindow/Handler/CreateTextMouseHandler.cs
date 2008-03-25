using System;
using System.Collections.Generic;
using System.Text;
using EcellLib.PathwayWindow.Nodes;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo;

namespace EcellLib.PathwayWindow.Handler
{
    /// <summary>
    /// CreateTextMouseHandler
    /// </summary>
    public class CreateTextMouseHandler : PPathwayInputEventHandler
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="control"></param>
        public CreateTextMouseHandler(PathwayControl control)
        {
            m_con = control;
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

            CanvasControl canvas = m_con.Canvas;
            PPathwayText text = new PPathwayText(canvas);
            text.X = e.Position.X;
            text.Y = e.Position.Y;
            canvas.AddText(text);
        }
    }
}
