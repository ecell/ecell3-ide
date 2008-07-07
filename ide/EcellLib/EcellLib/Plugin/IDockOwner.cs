using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace Ecell.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDockOwner
    {
        /// <summary>
        /// 
        /// </summary>
        DockPanel DockPanel { get; }
    }
}
