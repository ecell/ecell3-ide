using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Ecell.IDE.MainWindow.COM
{
    static class COMUtils
    {
        public static void RegisterLocalServer(Guid clsID, string executable, string progID, string viProgID)
        {
            RegistryKey rkeyRoot = Registry.ClassesRoot.OpenSubKey("CLSID", true);
            using (rkeyRoot)
            {
                RegistryKey rkey = rkeyRoot.CreateSubKey("{" + clsID.ToString() + "}");
                using (rkey) RegisterLocalServer(rkey, clsID, executable, progID, viProgID);
            }
        }

        private static void RegisterLocalServer(RegistryKey rkey, Guid clsID, string executable, string progID, string viProgID)
        {
            RegistryKey inprocHandler32Key = rkey.CreateSubKey("InprocHandler32");
            using (inprocHandler32Key) inprocHandler32Key.SetValue(null, "ole32.dll");
            RegistryKey localServerKey = rkey.CreateSubKey("LocalServer32");
            using (localServerKey) localServerKey.SetValue(null, executable);
            RegistryKey progIDKey = rkey.CreateSubKey("ProgID");
            using (progIDKey) progIDKey.SetValue(null, progID);
            if (viProgID != null)
            {
                RegistryKey viProgIDKey = rkey.CreateSubKey("VersionIndependentProgID");
                using (viProgIDKey)
                {
                    viProgIDKey.SetValue(null, viProgID);
                }
            }
        }

        public static void RegisterLocalServer(Guid clsID, string executable, string progID, string viProgID, Guid typelibGuid)
        {
            RegistryKey rkeyRoot = Registry.ClassesRoot.OpenSubKey("CLSID", true);
            using (rkeyRoot)
            {
                RegistryKey rkey = rkeyRoot.CreateSubKey("{" + clsID.ToString() + "}");
                using (rkey)
                {
                    RegisterLocalServer(rkey, clsID, executable, progID, viProgID);
                    RegistryKey typeLibGuidKey = rkey.CreateSubKey("TypeLib");
                    using (typeLibGuidKey)
                    {
                        typeLibGuidKey.SetValue(null, typeLibGuidKey.ToString());
                    }
                }
            }
        }

        public static void RegisterProgID(string progid, Guid clsID)
        {
            RegistryKey rkey = Registry.ClassesRoot.CreateSubKey(progid);
            using (rkey)
            {
                rkey.CreateSubKey("CLSID").SetValue(null, "{" + clsID.ToString() + "}");
            }
        }

        public static Guid GetGuidOf(Type t)
        {
            return new Guid((
                (GuidAttribute)t.GetCustomAttributes(
                    typeof(GuidAttribute), false)[0]).Value);
        }

        public static Guid GetGuidOf(Assembly a)
        {
            return new Guid((
                (GuidAttribute)a.GetCustomAttributes(
                    typeof(GuidAttribute), false)[0]).Value);
        }
    }
}
