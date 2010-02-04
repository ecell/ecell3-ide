using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace EcellCoreLibCLRTest
{
    public class TestBase
    {
        Random r;

        protected TestBase()
        {
            r = new Random();
        }

        protected static string GetStagingHomeDirectory()
        {
            string path = System.Environment.GetEnvironmentVariable("ECELL_STAGING_HOME");
            return path;
        }

        protected static string GetDMDirectory()
        {
            string path = Path.Combine(GetStagingHomeDirectory(), "Win32\\Release\\lib\\ecell-3.2\\dms");
            return path;
        }

        protected double GenerateRandomDouble()
        {
            return r.NextDouble();
        }

        protected string GenerateRandomID()
        {
            const string idchars = "0123456789abcdefghjijklmnopqrtuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 16; ++i)
            {
                sb.Append(idchars[r.Next(idchars.Length)]);
            }
            return sb.ToString();
        }
    }
}
