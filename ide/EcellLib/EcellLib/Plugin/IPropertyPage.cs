using System;
using System.Collections.Generic;
using System.Text;

namespace Ecell.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyItem
    {
        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize();

        /// <summary>
        /// ApplyChange
        /// </summary>
        void ApplyChange();

        /// <summary>
        /// PropertyDialogClosing
        /// </summary>
        void PropertyDialogClosing();
    }
}
