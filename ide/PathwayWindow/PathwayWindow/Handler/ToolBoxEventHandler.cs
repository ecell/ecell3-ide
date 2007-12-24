using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.PathwayWindow.Handler
{
    class ToolBoxEventHandler : PPathwayInputEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public ToolBoxEventHandler(PathwayControl control)
        {
            base.m_con = control;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(object sender, UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            base.OnMouseDown(sender, e);
            MessageBox.Show("I'm sorry to tell you that \nthis function is under construction.");
        }
    }
}
