using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Ecell.Plugin
{
    public interface IRootMenuProvider
    {
        ToolStripMenuItem GetRootMenuItem(string name);
    }
}
