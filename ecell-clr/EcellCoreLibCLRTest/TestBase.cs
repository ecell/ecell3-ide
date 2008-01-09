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
            return System.Environment.GetEnvironmentVariable("_STG_HOME");
        }

        protected static string GetDMDirectory()
        {
            return Path.Combine(GetStagingHomeDirectory(), "lib\\ecell-3.1\\dms");
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
