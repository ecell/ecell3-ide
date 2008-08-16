using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ecell.Plugin
{
    public interface IToolStripProvider
    {
        /// <summary>
        /// Get toolbar buttons for each plugin.
        /// </summary>
        /// <returns>null</returns>
        ToolStrip GetToolBarMenuStrip();
    }
}
