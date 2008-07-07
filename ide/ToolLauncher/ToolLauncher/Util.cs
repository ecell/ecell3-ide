using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ecell;

namespace ToolLauncher
{
    /// <summary>
    /// ToolLauncher util function.
    /// </summary>
    public class Util
    {
        private static Util instance = null;
        private string initPathBoost = "";
        private string initPathEcell = "";
        private string initPathGSL = "";
        private string initPathVC = "";
        private string initEnvironment = "";

        private Util()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.initEnvironment = Ecell.Util.GetBaseDir() + Ecell.Constants.delimiterPath + Constants.initEnvironment;
            if (File.Exists(this.initEnvironment))
            {
                StreamReader sr = null;
                try
                {
                    string line = null;
                    sr = new StreamReader(initEnvironment, Encoding.GetEncoding("Shift_JIS"));
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf(Constants.defaultKeyBoost) == 0)
                        {
                            this.initPathBoost = line.Split('=')[1];
                            Util.Trim(ref this.initPathBoost);
                        }
                        else if (line.IndexOf(Constants.defaultKeyEcell) == 0)
                        {
                            this.initPathEcell = line.Split('=')[1];
                            Util.Trim(ref this.initPathEcell);
                        }
                        else if (line.IndexOf(Constants.defaultKeyGSL) == 0)
                        {
                            this.initPathGSL = line.Split('=')[1];
                            Util.Trim(ref this.initPathGSL);
                        }
                        else if (line.IndexOf(Constants.defaultKeyVC) == 0)
                        {
                            this.initPathVC = line.Split('=')[1];
                            Util.Trim(ref this.initPathVC);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ex.ToString();
                    this.initPathBoost = Constants.defaultPathBoost;
                    this.initPathEcell = Constants.defaultPathEcell;
                    this.initPathGSL = Constants.defaultPathGSL;
                    this.initPathVC = Constants.defaultPathVC;
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
                this.initPathBoost = Constants.defaultPathBoost;
                this.initPathEcell = Constants.defaultPathEcell;
                this.initPathGSL = Constants.defaultPathGSL;
                this.initPathVC = Constants.defaultPathVC;
            }
        }

        /// <summary>
        /// get boost path.
        /// </summary>
        public string RootPathBoost
        {
            get { return this.initPathBoost; }
        }

        /// <summary>
        /// get E-Cell path.
        /// </summary>
        public string RootPathEcell
        {
            get { return this.initPathEcell; }
        }

        /// <summary>
        /// get GSL path.
        /// </summary>
        public string RootPathGSL
        {
            get { return this.initPathGSL; }
        }

        /// <summary>
        /// get Visual Studio path.
        /// </summary>
        public string RootPathVC
        {
            get { return this.initPathVC; }
        }

        /// <summary>
        /// Get Util object. singleton pattern.
        /// </summary>
        /// <returns></returns>
        public static Util GetInstance()
        {
            if (instance == null)
            {
                instance = new Util();
            }
            return instance;
        }

        /// <summary>
        /// Over write the init file.
        /// </summary>
        /// <param name="pathBoost">boost path.</param>
        /// <param name="pathEcell">e-cell path.</param>
        /// <param name="pathGSL">gsl path.</param>
        /// <param name="pathVC">visual studio path.</param>
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
                sw.WriteLine(Constants.defaultKeyBoost + " = " + this.initPathBoost);
                sw.WriteLine(Constants.defaultKeyEcell + " = " + this.initPathEcell);
                sw.WriteLine(Constants.defaultKeyGSL + " = " + this.initPathGSL);
                sw.WriteLine(Constants.defaultKeyVC + " = " + this.initPathVC);
                if (File.Exists(this.initEnvironment))
                {
                    StreamReader sr = null;
                    try
                    {
                        string line = null;
                        sr = new StreamReader(this.initEnvironment, Encoding.GetEncoding("Shift_JIS"));
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.IndexOf(Constants.defaultKeyBoost) != 0 && line.IndexOf(Constants.defaultKeyEcell) != 0
                                && line.IndexOf(Constants.defaultKeyGSL) != 0 && line.IndexOf(Constants.defaultKeyVC) != 0)
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

        /// <summary>
        /// Trim the string by space character.
        /// </summary>
        /// <param name="str">input string.</param>
        public static void Trim(ref string str)
        {
            while (str.IndexOf(Constants.signSpace) == 0)
            {
                str = str.Substring(1);
            }
            while (str.LastIndexOf(Constants.signSpace) == (str.Length - 1))
            {
                str = str.Substring(0, str.Length - 2);
            }
        }
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
