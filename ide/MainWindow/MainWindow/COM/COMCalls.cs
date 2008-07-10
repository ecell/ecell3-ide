using System;
using System.Runtime.InteropServices;

namespace Ecell.IDE.COM
{
    static class COMCalls
    {
        [DllImport("ole32.dll")]
        public static extern int CoRegisterClassObject(
            [In] ref Guid rclsid,
            IntPtr pUnk,
            uint dwClsContext, uint flags, out uint lpdwRegister);

        public const uint CLSCTX_LOCAL_SERVER = 0x00004;
        public const uint CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000;
        public const uint CLSCTX_ACTIVATE_32_BIT_SERVER = 0x40000;
        public const uint CLSCTX_ACTIVATE_64_BIT_SERVER = 0x80000;
        public const uint REGCLS_SINGLEUSE = 0x00000;
        public const uint REGCLS_MULTIPLEUSE = 0x00001;
    }
}
