using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Ecell.IDE.MainWindow.COM
{
    [ComVisible(true)]
    [Guid("7889510F-5394-4c03-8792-1F091536F990")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface IAutomationServerObject
    {
        void LoadProjectIE(string projectID, string filepath);
        void Test(string test);
    }
}
