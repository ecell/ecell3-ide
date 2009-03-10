using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.IDE.Plugins.PathwayWindow.Nodes
{
    class PPathwayAlias : PPathwayObject
    {
        PPathwayVariable m_variable = null;
        /// <summary>
        /// 
        /// </summary>
        public PPathwayAlias()
        {
        }

        public override PPathwayObject CreateNewObject()
        {
            return new PPathwayAlias();
        }

        public override void OnMouseDown(UMD.HCIL.Piccolo.Event.PInputEventArgs e)
        {
            m_variable.OnMouseDown(e);
        }
    }
}
