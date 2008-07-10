using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Ecell.IDE.COM
{
    [ComVisible(true)]
    [Guid("7889510F-5394-4c03-8792-1F091536F990")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface IApplication
    {
        void LoadProjectIE(string projectID, string filepath);
    }
}
