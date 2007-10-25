using System;
using System.Collections.Generic;
using System.Text;
using UMD.HCIL.Piccolo.Event;

namespace EcellLib.PathwayWindow
{
    /// <summary>
    /// Handleer for mouse down.
    /// </summary>
    public class MouseDownHandler : PBasicInputEventHandler
    {
        /// <summary>
        /// The PathwayView instance
        /// </summary>
        private PathwayControl m_con;

        /// <summary>
        /// The contructor
        /// </summary>
        public MouseDownHandler(PathwayControl control)
        {
            m_con = control;
        }

        /// <summary>
        /// Called when the mouse is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);
            if(e.Button == System.Windows.Forms.MouseButtons.Left && m_con != null)
            {
                m_con.CanvasDictionary[e.Canvas.Name].ResetSelectedObjects();
            }
        }
    }
}
