using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Ecell.Plugin
{
    public interface IRootMenuItemProvider
    {
        ToolStripMenuItem GetRootMenuItem(string name);
    }
}
