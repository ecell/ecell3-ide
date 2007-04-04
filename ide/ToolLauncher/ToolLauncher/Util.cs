using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EcellLib;

namespace ToolLauncher
{
    public class Util
    {
        private static Util instance = null;
        private string initPathBoost = "";
        private string initPathEcell = "";
        private string initPathGSL = "";
        private string initPathVC = "";
        private string initEnvironment = "";
        public const string s_defaultKeyBoost = "BRD";
        public const string s_defaultKeyCD = "CD";
        public const string s_defaultKeyDebug = "DEBUG";
        public const string s_defaultKeyEcell = "ERD";
        public const string s_defaultKeyGSL = "GRD";
        public const string s_defaultKeyVC = "VCRD";
        public const string s_defaultPathBoost = "C:\\Boost";
        public const string s_defaultPathEcell = "C:\\E-Cell";
        public const string s_defaultPathGSL = "C:\\GSL";
        public const string s_defaultPathVC = "C:\\Program Files\\Microsoft Visual Studio 8\\VC";
        public const string s_initEnvironment = "toollauncher_environment.ini";
        public const string s_ironPythonDir = "IRONPYTHONSTARTUP";
        public const string s_signHyphen = "-";
        public const string s_signSpace = " ";
        public const string s_signDQ = "\"";
        public const string s_convertBat = "ConvertEntityIntoDll.bat";
        public const string s_FlagError = "[ERROR]: ";
        public const string s_FilePattern = "*.cpp";
        public const string s_toolPath = "\\..\\toollauncher";

        private Util()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.initEnvironment = EcellLib.Util.GetBaseDir() + EcellLib.Util.s_delimiterPath + s_initEnvironment;
            if (File.Exists(this.initEnvironment))
            {
                StreamReader sr = null;
                try
                {
                    string line = null;
                    sr = new StreamReader(initEnvironment, Encoding.GetEncoding("Shift_JIS"));
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf(s_defaultKeyBoost) == 0)
                        {
                            this.initPathBoost = line.Split('=')[1];
                            Util.Trim(ref this.initPathBoost);
                        }
                        else if (line.IndexOf(s_defaultKeyEcell) == 0)
                        {
                            this.initPathEcell = line.Split('=')[1];
                            Util.Trim(ref this.initPathEcell);
                        }
                        else if (line.IndexOf(s_defaultKeyGSL) == 0)
                        {
                            this.initPathGSL = line.Split('=')[1];
                            Util.Trim(ref this.initPathGSL);
                        }
                        else if (line.IndexOf(s_defaultKeyVC) == 0)
                        {
                            this.initPathVC = line.Split('=')[1];
                            Util.Trim(ref this.initPathVC);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ex.ToString();
                    this.initPathBoost = s_defaultPathBoost;
                    this.initPathEcell = s_defaultPathEcell;
                    this.initPathGSL = s_defaultPathGSL;
                    this.initPathVC = s_defaultPathVC;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                        sr = null;
                    }
                }
            }
            else
            {
                this.initPathBoost = s_defaultPathBoost;
                this.initPathEcell = s_defaultPathEcell;
                this.initPathGSL = s_defaultPathGSL;
                this.initPathVC = s_defaultPathVC;
            }
        }

        public string RootPathBoost
        {
            get { return this.initPathBoost; }
        }

        public string RootPathEcell
        {
            get { return this.initPathEcell; }
        }

        public string RootPathGSL
        {
            get { return this.initPathGSL; }
        }

        public string RootPathVC
        {
            get { return this.initPathVC; }
        }

        public static Util GetInstance()
        {
            if (instance == null)
            {
                instance = new Util();
            }
            return instance;
        }

        public void OverwriteInitFile(string pathBoost, string pathEcell, string pathGSL, string pathVC)
        {
            this.initPathBoost = pathBoost;
            this.initPathEcell = pathEcell;
            this.initPathGSL = pathGSL;
            this.initPathVC = pathVC;
            String tmpFile = this.initEnvironment + ".tmp";
            if (File.Exists(tmpFile))
            {
                File.Delete(tmpFile);
            }
            StreamWriter sw = null;
            bool successFlag = true;
            try
            {
                sw = new StreamWriter(tmpFile, false, Encoding.GetEncoding("Shift_JIS"));
                sw.WriteLine(s_defaultKeyBoost + " = " + this.initPathBoost);
                sw.WriteLine(s_defaultKeyEcell + " = " + this.initPathEcell);
                sw.WriteLine(s_defaultKeyGSL + " = " + this.initPathGSL);
                sw.WriteLine(s_defaultKeyVC + " = " + this.initPathVC);
                if (File.Exists(this.initEnvironment))
                {
                    StreamReader sr = null;
                    try
                    {
                        string line = null;
                        sr = new StreamReader(this.initEnvironment, Encoding.GetEncoding("Shift_JIS"));
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.IndexOf(s_defaultKeyBoost) != 0 && line.IndexOf(s_defaultKeyEcell) != 0
                                && line.IndexOf(s_defaultKeyGSL) != 0 && line.IndexOf(s_defaultKeyVC) != 0)
                            {
                                sw.WriteLine(line);
                            }
                        }
                    }
                    finally
                    {
                        if (sr != null)
                        {
                            sr.Close();
                            sr = null;
                        }
                    }
                    File.Delete(this.initEnvironment);
                }
            }
            catch (Exception swex)
            {
                swex.ToString();
                successFlag = false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                if (successFlag)
                {
                    File.Move(tmpFile, this.initEnvironment);
                }
                else
                {
                    File.Delete(tmpFile);
                }
            }
        }

        public static void Trim(ref string str)
        {
            while (str.IndexOf(s_signSpace) == 0)
            {
                str = str.Substring(1);
            }
            while (str.LastIndexOf(s_signSpace) == (str.Length - 1))
            {
                str = str.Substring(0, str.Length - 2);
            }
        }
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
