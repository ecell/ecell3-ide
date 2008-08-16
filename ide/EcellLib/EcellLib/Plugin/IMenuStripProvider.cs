using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ecell.Plugin
{
    public interface IMenuStripProvider
    {
        /// <summary>
        /// Get menustrips for each plugin.
        /// </summary>
        /// <returns>null.</returns>
        IEnumerable<ToolStripMenuItem> GetMenuStripItems();
    }
}
