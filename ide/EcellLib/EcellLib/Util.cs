//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2006 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
//
//END_HEADER
//
// written by Takeshi Yuasa <yuasa@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.AccessControl;
using System.Threading;

namespace EcellLib
{
    /// <summary>
    /// Class to manage the common function.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Reserved extension for DM path.
        /// </summary>
        public const string s_defaultDMPath = "dm";
        /// <summary>
        /// Reserved extension for library.
        /// </summary>
        public const string s_dmFileExtension = ".dll";
        /// <summary>
        /// Reserved char for colon.
        /// </summary>
        public const string s_delimiterColon = ":";
        /// <summary>
        /// Reserved char for equal.
        /// </summary>
        public const string s_delimiterEqual = "=";
        /// <summary>
        /// Reserved char for Hyphen.
        /// </summary>
        public const string s_delimiterHyphen = "-";
        /// <summary>
        /// Reserved char for Path.
        /// </summary>
        public const string s_delimiterPath = "/";
        /// <summary>
        /// Reserved char for period.
        /// </summary>
        public const string s_delimiterPeriod = ".";
        /// <summary>
        /// Reserved char for semi colon.
        /// </summary>
        public const string s_delimiterSemiColon = ";";
        /// <summary>
        /// Reserved char for sharp.
        /// </summary>
        public const string s_delimiterSharp = "#";
        /// <summary>
        /// Reserved char for space.
        /// </summary>
        public const string s_delimiterSpace = " ";
        /// <summary>
        /// Reserved char for delimiter.
        /// </summary>
        public const string s_delimiterTab = "\t";
        /// <summary>
        /// Reserved char for ubderbar,
        /// </summary>
        public const string s_delimiterUnderbar = "_";
        /// <summary>
        /// Reserved char for wild card.
        /// </summary>
        public const string s_delimiterWildcard = "*";
        /// <summary>
        /// Reserved the name of file..
        /// </summary>
        public const string s_fileProject = "project.info";
        /// <summary>
        /// Reserved the header of average.
        /// </summary>
        public const string s_headerAverage = "avg";
        /// <summary>
        /// Reserved the number of header.
        /// </summary>
        public const string s_headerColumn = "5";
        /// <summary>
        /// Reserved the header of data.
        /// </summary>
        public const string s_headerData = "DATA";
        /// <summary>
        /// Reserved the header of label.
        /// </summary>
        public const string s_headerLabel = "LABEL";
        /// <summary>
        /// Reserved the header of  max.
        /// </summary>
        public const string s_headerMaximum = "Max";
        /// <summary>
        /// Reserved the header of min.
        /// </summary>
        public const string s_headerMinimum = "Min";
        /// <summary>
        /// Reserved the header of note.
        /// </summary>
        public const string s_headerNote = "NOTE";
        /// <summary>
        /// Reserved the header of Size.
        /// </summary>
        public const string s_headerSize = "SIZE";
        /// <summary>
        /// Reserved the header of Time.
        /// </summary>
        public const string s_headerTime = "t";
        /// <summary>
        /// Reserved the header of tolerable.
        /// </summary>
        public const string s_headerTolerable = "Tolerable";
        /// <summary>
        /// Reserved the header of value.
        /// </summary>
        public const string s_headerValue = "value";
        /// <summary>
        /// Reserved XML path name for DefalutParameter.
        /// </summary>
        public const string s_parameterKey = "DefaultParameter";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE ANALYSIS".
        /// </summary>
        public const string s_registryAnalysisDirKey = "E-CELL IDE ANALYSIS";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE BASE".
        /// </summary>
        public const string s_registryBaseDirKey = "E-CELL IDE BASE";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE DM".
        /// </summary>
        public const string s_registryDMDirKey = "E-CELL IDE DM";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE PLUGIN".
        /// </summary>
        public const string s_registryPluginDirKey = "E-CELL IDE PLUGIN";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE LANG".
        /// </summary>
        public const string s_registryLang = "E-CELL IDE LANG";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE STATICDEBUG PLUGIN".
        /// </summary>
        public const string s_registryStaticDebugDirKey = "E-CELL IDE STATICDEBUG PLUGIN";
        /// <summary>
        /// Reserved XML path name for name of "E-CELL IDE TMP".
        /// </summary>
        public const string s_registryTmpDirKey = "E-CELL IDE TMP";
        /// <summary>
        /// Reserved XML path name for key of environment.
        /// </summary>
        public const string s_registryEnvKey = "Environment";
        /// <summary>
        /// Reserved XML path name for key of Software.
        /// </summary>
        public const string s_registrySWKey = "software\\KeioUniv\\E-Cell IDE";
        /// <summary>
        /// Reserved XML path name for key of Software.
        /// </summary>
        public const string s_registrySW2Key = "software\\KeioUniv";
        /// <summary>
        /// Reserved XML path name for comment.
        /// </summary>
        public const string s_textComment = "Commnet";
        /// <summary>
        /// Reserved XML path name for default stepper.
        /// </summary>
        public const string s_textKey = "DefaultStepper";
        /// <summary>
        /// Reserved XML path name for parameter of simulation.
        /// </summary>
        public const string s_textParameter = "SimulationParameter";
        /// <summary>
        /// Reserved XML path name for the type of double.
        /// </summary>
        public const string s_typeDouble = "double";
        /// <summary>
        /// Reserved XML path name for the type of int.
        /// </summary>
        public const string s_typeInt = "int";
        /// <summary>
        /// Reserved XML path name for the type of list.
        /// </summary>
        public const string s_typeList = "list";
        /// <summary>
        /// Reserved XML path name for string.
        /// </summary>
        public const string s_typeString = "string";
        /// <summary>
        /// Reserved XML path name for action.
        /// </summary>
        public const string s_xpathAction = "Action";
        /// <summary>
        /// Reserved XML path name for activity.
        /// </summary>
        public const string s_xpathActivity = "Activity";
        /// <summary>
        /// Reserved XML path name for class.
        /// </summary>
        public const string s_xpathClass = "class";
        /// <summary>
        /// Reserved XML path name for class name.
        /// </summary>
        public const string s_xpathClassName = "ClassName";
        /// <summary>
        /// Reserved XML path name for ECD.
        /// </summary>
        public const string s_xpathEcd = "ecd";
        /// <summary>
        /// Reserved XML path name for CSV.
        /// </summary>
        public const string s_xpathCsv = "csv";
        /// <summary>
        /// Reserved XML path name for EML.
        /// </summary>
        public const string s_xpathEml = "eml";
        /// <summary>
        /// Reserved XML path name for Expression.
        /// </summary>
        public const string s_xpathExpression = "Expression";
        /// <summary>
        /// Reserved XML path name for FireMethod.
        /// </summary>
        public const string s_xpathFireMethod = "FireMethod";
        /// <summary>
        /// Reserved XML path name for Fixed.
        /// </summary>
        public const string s_xpathFixed = "Fixed";
        /// <summary>
        /// Reserved XML path name for ID.
        /// </summary>
        public const string s_xpathID = "ID";
        /// <summary>
        /// Reserved XML path name for initial condition.
        /// </summary>
        public const string s_xpathInitialCondition = "InitialCondition";
        /// <summary>
        /// Reserved XML path name for Interval.
        /// </summary>
        public const string s_xpathInterval = "Interval";
        /// <summary>
        /// Reserved XML path name for IsEpsilonChecked.
        /// </summary>
        public const string s_xpathIsEpsilonChecked = "IsEpsilonChecked";
        /// <summary>
        /// Reserved XML path name for key.
        /// </summary>
        public const string s_xpathKey = "Key";
        /// <summary>
        /// Reserved XML path name for LoggerPolicy.
        /// </summary>
        public const string s_xpathLoggerPolicy = "LoggerPolicy";
        /// <summary>
        /// Reserved XML path name for Model.
        /// </summary>
        public const string s_xpathModel = "Model";
        /// <summary>
        /// Reserved XML path name for MolarConc.
        /// </summary>
        public const string s_xpathMolarConc = "MolarConc";
        /// <summary>
        /// Reserved XML path name for Name.
        /// </summary>
        public const string s_xpathName = "Name";
        /// <summary>
        /// Reserved XML path name for NumberConc.
        /// </summary>
        public const string s_xpathNumberConc = "NumberConc";
        /// <summary>
        /// Reserved XML path name for Prm.
        /// </summary>
        public const string s_xpathPrm = "Prm";
        /// <summary>
        /// Reserved XML path name for Process.
        /// </summary>
        public const string s_xpathProcess = "Process";
        /// <summary>
        /// Reserved XML path name for Project.
        /// </summary>
        public const string s_xpathProject = "Project";
        /// <summary>
        /// Reserved XML path name for Property.
        /// </summary>
        public const string s_xpathProperty = "property";
        /// <summary>
        /// Reserved XML path name for Result.
        /// </summary>
        public const string s_xpathResult = "Result";
        /// <summary>
        /// Reserved XML path name for Size.
        /// </summary>
        public const string s_xpathSize = "Size";
        /// <summary>
        /// Reserved XML path name for Simulation.
        /// </summary>
        public const string s_xpathSimulation = "Simulation";
        /// <summary>
        /// Reserved XML path name for Space.
        /// </summary>
        public const string s_xpathSpace = "Space";
        /// <summary>
        /// Reserved XML path name for Step.
        /// </summary>
        public const string s_xpathStep = "Step";
        /// <summary>
        /// Reserved XML path name for StepInterval.
        /// </summary>
        public const string s_xpathStepInterval = "StepInterval";
        /// <summary>
        /// Reserved XML path name for Stepper.
        /// </summary>
        public const string s_xpathStepper = "Stepper";
        /// <summary>
        /// Reserved XML path name for System.
        /// </summary>
        public const string s_xpathSystem = "System";
        /// <summary>
        /// Reserved XML path name for Value.
        /// </summary>
        public const string s_xpathValue = "Value";
        /// <summary>
        /// Reserved XML path name for Variable.
        /// </summary>
        public const string s_xpathVariable = "Variable";
        /// <summary>
        /// Reserved XML path name for VariableReferenceList.
        /// </summary>
        public const string s_xpathVRL = "VariableReferenceList";
        /// <summary>
        /// Reserved XML path name for xml.
        /// </summary>
        public const string s_xpathXml = "xml";
        
        /// <summary>
        /// Project status (this program is loaded no yet,)
        /// </summary>
        public const int NOTLOAD = 0;
        /// <summary>
        /// Projct status (this projgram is loaded).
        /// </summary>
        public const int LOADED = 1;
        /// <summary>
        /// Project status (this project is running).
        /// </summary>
        public const int RUNNING = 2;
        /// <summary>
        /// Project status (this project is suspend).
        /// </summary>
        public const int SUSPEND = 3;
        /// <summary>
        /// Project status (this project is in step).
        /// </summary>
        public const int STEP = 4;

        /// <summary>
        /// Analysis status (this analysis is run no yet.
        /// </summary>
        public const int ANALYSIS_YET = 0;
        /// <summary>
        /// Analysis status (this analysis is running).
        /// </summary>
        public const int ANALYSIS_DO = 1;
        /// <summary>
        /// Analysis status (this analysis is already done).
        /// </summary>
        public const int ANALYSIS_DONE = 2;

        /// <summary>
        /// Get the analysis directory from register.
        /// </summary>
        /// <returns></returns>
        static public string GetAnalysisDir()
        {
            return GetRegistryValue(s_registryAnalysisDirKey);
        }

        /// <summary>
        /// Get the language from register.
        /// </summary>
        /// <returns></returns>
        static public string GetLang()
        {
            return GetRegistryValue(s_registryLang);
        }

        /// <summary>
        /// Get the working directory from register.
        /// </summary>
        /// <returns>the working directory.</returns>
        static public string GetBaseDir()
        {
            return GetRegistryValue(s_registryBaseDirKey);
        }

        /// <summary>
        /// Get the DM directory for DM from register.
        /// </summary>
        /// <param name="prjID">project ID.</param>
        /// <returns>the DM directory.</returns>
        static public string GetProjectDMDir(string prjID)
        {
            string baseDir = GetBaseDir();
            if (Directory.Exists(baseDir + "\\" + prjID + "\\dm"))
            {
                return baseDir + "\\" + prjID + "\\dm";
            }
            return null;
        }

        /// <summary>
        /// Get the DM direcory from register.
        /// </summary>
        /// <param name="m_currentProjectID">loading project.</param>
        /// <returns>DM directory.</returns>
        static public string GetDMDir(string m_currentProjectID)
        {
            return GetDMDir() + ";" + Util.GetProjectDMDir(m_currentProjectID);
        }

        /// <summary>
        /// Get the DM direcory from register.
        /// </summary>
        /// <returns>DM directory.</returns>
        static public string GetDMDir()
        {
//            return GetRegistryValue(s_registryDMDirKey) + ";" + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\My E-Cell Projects\\sample\\dm";
            return GetRegistryValue(s_registryDMDirKey) + ";" + GetCommonDocumentDir() + "\\My E-Cell Projects\\sample\\dm";
        }

        static public string GetCommonDocumentDir()
        {
            string l_currentDir = null;
            Microsoft.Win32.RegistryKey l_key = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey l_subkey = null;
            try
            {
                l_key = Microsoft.Win32.Registry.LocalMachine;
                l_subkey = l_key.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders");
                if (l_subkey != null)
                {
                    l_currentDir = (string)l_subkey.GetValue("Common Documents");
                }
                return l_currentDir;
            }
            finally
            {
                if (l_key != null)
                {
                    l_key.Close();
                }
                if (l_subkey != null)
                {
                    l_subkey.Close();
                }
            }
        }

        /// <summary>
        /// Get the plugin directory from register.
        /// </summary>
        /// <returns>plugin directory.</returns>
        static public string GetPluginDir()
        {
            return GetRegistryValue(s_registryPluginDirKey);
        }

        /// <summary>
        /// Check whether id contains the string except for letter, digit or '_'.
        /// </summary>
        /// <param name="l_key">id</param>
        /// <returns>if contain, return true.</returns>
        static public bool IsNGforID(string l_key)
        {
            for (int i = 0; i < l_key.Length; i++)
            {
                if (Char.IsLetterOrDigit(l_key[i]) ||
                    l_key[i] == '_') continue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether this id of system is NG.
        /// </summary>
        /// <param name="l_key">the system id.</param>
        /// <returns>correct is false.</returns>
        static public bool IsNGforSystemFullID(string l_key)
        {
            int delCount = 0;
            bool isDel = false;
            for (int i = 0; i < l_key.Length; i++)
            {
                if (!Char.IsLetterOrDigit(l_key[i]) &&
                    l_key[i] != '_' &&
                    l_key[i] != '/' &&
                    l_key[i] != ':') return true;
                if (l_key[i] == '/')
                {
                    if (isDel == true) return true;
                    isDel = true;
                }
                else
                {
                    isDel = false;
                }
                if (l_key[i] == ':') delCount++;
            }
            if (delCount > 0) return true;
            return false;
        }

        /// <summary>
        /// Check whether this id of component object(process or variable) is NG.
        /// </summary>
        /// <param name="l_key">the component id.</param>
        /// <returns>correct is false.</returns>
        static public bool IsNGforComponentFullID(string l_key)
        {
            int delCount = 0;
            bool isDel = false;
            for (int i = 0; i < l_key.Length; i++)
            {
                if (!Char.IsLetterOrDigit(l_key[i]) &&
                    l_key[i] != '_' &&
                    l_key[i] != '/' &&
                    l_key[i] != ':') return true;
                if (l_key[i] == '/')
                {
                    if (isDel == true) return true;
                    isDel = true;
                }
                else
                {
                    isDel = false;
                }
                if (l_key[i] == ':') delCount++;
            }
            if (delCount > 1) return true;
            if (delCount <= 0) return true;
            return false;
        }

        /// <summary>
        /// Get the temporary directory from register.
        /// </summary>
        /// <returns></returns>
        static public string GetTmpDir()
        {
            String topDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\E-Cell IDE";
            if (!Directory.Exists(topDir))
            {
                Directory.CreateDirectory(topDir);
            }
            return topDir;
//            return GetRegistryValue(s_registryTmpDirKey);
        }

        /// <summary>
        /// Get the set value from register
        /// </summary>
        /// <param name="l_intendedKey">registry key.</param>
        /// <returns>the value.</returns>
        static private string GetRegistryValue(string l_intendedKey)
        {
            string l_currentDir = null;
            Microsoft.Win32.RegistryKey l_key = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey l_subkey = null;
            try
            {
                l_subkey = l_key.OpenSubKey(s_registryEnvKey);
                l_currentDir = (string)l_subkey.GetValue(l_intendedKey);
                if (l_currentDir != null)
                    return l_currentDir;
            }
            finally
            {
                if (l_subkey != null)
                {
                    l_subkey.Close();
                }
            }
            try
            {
                l_subkey = l_key.OpenSubKey(s_registrySWKey);
                if (l_subkey != null)
                {
                    l_currentDir = (string)l_subkey.GetValue(l_intendedKey);
                    if (l_currentDir != null)
                        return l_currentDir;
                }
            }
            finally
            {
                if (l_key != null)
                {
                    l_key.Close();
                }
                if (l_subkey != null)
                {
                    l_subkey.Close();
                }
            }
            try
            {
                l_key = Microsoft.Win32.Registry.LocalMachine;
                l_subkey = l_key.OpenSubKey(s_registrySWKey);
                if (l_subkey != null)
                {
                    l_currentDir = (string)l_subkey.GetValue(l_intendedKey);
                }
                return l_currentDir;
            }
            finally
            {
                if (l_key != null)
                {
                    l_key.Close();
                }
                if (l_subkey != null)
                {
                    l_subkey.Close();
                }
            }
        }

        /// <summary>
        /// Set the language of application when application is start.
        /// </summary>
        static public void InitialLanguage()
        {
            String lang = Util.GetLang();
            if (lang == null)
            {
                // nothing
            }
            else if (lang.ToUpper() == "EN_US")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us", true);
            }
            else if (lang.ToUpper() == "JA")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ja", true);
            }
        }

        /// <summary>
        /// Set language for E-Cell IDE.
        /// </summary>
        /// <param name="l_lang">language.</param>
        static public void SetLanguage(string l_lang)
        {
            Microsoft.Win32.RegistryKey l_key = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey l_subkey = null;
            try
            {
                l_subkey = l_key.OpenSubKey(s_registrySWKey, true);
                /*
                l_currentDir = (string)l_subkey.GetValue(s_registryBaseDirKey);
                if (l_currentDir == null)
                {
                    RegistrySecurity s = l_subkey.GetAccessControl();
                    
                    l_subkey.CreateSubKey(s_registryBaseDirKey);
                }*/
                l_subkey.SetValue(s_registryLang, l_lang);
            }
            finally
            {
                if (l_key != null)
                {
                    l_key.Close();
                }
                if (l_subkey != null)
                {
                    l_subkey.Close();
                }
            }
        }

        static public string ConvertSystemEntityPath(string key, string prop)
        {
            string result = "";
            string dir = "";
            string id = "";

            if (key.Equals("/"))
            {
                dir = "";
                id = "/";
            }
            else
            {
                int i =  key.LastIndexOf("/");
                if (i == 0)
                {
                    dir = "/";
                    id = key.Substring(1);
                }
                else
                {
                    dir = key.Substring(0, i);
                    id = key.Substring(i + 1);
                }
            }
            return "System:" + dir + ":" + id + ":" + prop;
        }

        /// <summary>
        /// Set the working directory to set directiroy.
        /// </summary>
        /// <param name="l_basedir">set directory.</param>
        static public void SetBaseDir(string l_basedir)
        {
//            string l_currentDir = null;
            Microsoft.Win32.RegistryKey l_key = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey l_subkey = null;
            try
            {
                l_subkey = l_key.OpenSubKey(s_registryEnvKey, true);
                /*
                l_currentDir = (string)l_subkey.GetValue(s_registryBaseDirKey);
                if (l_currentDir == null)
                {
                    RegistrySecurity s = l_subkey.GetAccessControl();
                    
                    l_subkey.CreateSubKey(s_registryBaseDirKey);
                }*/
                l_subkey.SetValue(s_registryBaseDirKey, l_basedir);
            }
            finally
            {
                if (l_key != null)
                {
                    l_key.Close();
                }
                if (l_subkey != null)
                {
                    l_subkey.Close();
                }
            }
        }
    }
}
