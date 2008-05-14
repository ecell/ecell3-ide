using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace EcellLib.Plugin
{
    public interface IDockOwner
    {
        DockPanel DockPanel { get; }
    }
}
