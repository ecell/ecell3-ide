using System;
using System.Runtime.InteropServices;

namespace Ecell.IDE.COM
{
    [Guid("E03C55ED-65CB-4c8b-BA04-B535BB559F44")]
    class ApplicationClassFactory : IClassFactory
    {
        private IApplication m_aso;
        private uint m_classObjectID = 0;

        [DllImport("ole32.dll")]
        static extern int CoRevokeClassObject(uint dwRegister);

        public uint ClassObjectID
        {
            get { return m_classObjectID; }
            internal set { m_classObjectID = value; }
        }

        public IApplication ApplicationObject
        {
            get { return m_aso; }
            set { m_aso = value; }
        }

        public ApplicationClassFactory()
        {
        }

        ~ApplicationClassFactory()
        {
            CoRevokeClassObject(ClassObjectID);
        }

        public void CreateInstance(
            [In] IntPtr pUnkOuter,
            [In] ref Guid iid,
            out IntPtr instance)
        {
            if (COMUtils.GetGuidOf(typeof(IApplication)) == iid)
            {
                instance = Marshal.GetComInterfaceForObject(
                    m_aso, typeof(IApplication));
            }
            else if (COMUtils.GetGuidOf(typeof(IUnknown)) == iid)
            {
                instance = Marshal.GetIUnknownForObject(m_aso);
            }
            else
            {
                instance = IntPtr.Zero;
                throw Marshal.GetExceptionForHR((int)HRESULT.E_NOINTERFACE);
            }
        }

        public void LockServer(bool fLock)
        {
            // do nothing
        }
    }
}
