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
using System.Windows.Forms;
using EcellLib.Objects;

namespace EcellLib
{
    /// <summary>
    /// Class to manage the common function.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Additional plugin directories to be searched on startup.
        /// XXX: this should not be in Util class.
        /// </summary>
        private static List<string> extraPluginDirs = new List<string>();

        /// <summary>
        /// Additional DM directories to be searched on startup.
        /// XXX: this should not be in Util class.
        /// </summary>
        private static List<string> extraDMDirs = new List<string>();

        /// <summary>
        /// Whether to include default plugin / DM paths.
        /// </summary>
        private static bool noDefaultPaths;

        /// <summary>
        /// Get the analysis directory from register.
        /// </summary>
        /// <returns></returns>
        static public string GetAnalysisDir()
        {
            return GetRegistryValue(Constants.registryAnalysisDirKey);
        }

        /// <summary>
        /// Get the language from register.
        /// </summary>
        /// <returns></returns>
        static public string GetLang()
        {
            return GetRegistryValue(Constants.registryLang);
        }

        /// <summary>
        /// Get the directory of window setting..
        /// </summary>
        /// <returns>the directory path.</returns>
        static public string GetWindowSettingDir()
        {
            return GetRegistryValue(Constants.registryWinSetDir);
        }

        /// <summary>
        /// Get the working directory from register.
        /// </summary>
        /// <returns>the working directory.</returns>
        static public string GetBaseDir()
        {
            return GetRegistryValue(Constants.registryBaseDirKey);
        }

        /// <summary>
        /// Get the DM direcory from register.
        /// </summary>
        /// <returns>DM directory.</returns>
        static public string[] GetDMDirs(String currentProjectPath)
        {
            List<string> dmDirs = new List<string>();
            List<string> candidates = new List<string>();
            if (currentProjectPath != null)
            {
                if (currentProjectPath.EndsWith(Convert.ToString( Path.DirectorySeparatorChar)))
                {
                    candidates.Add(currentProjectPath + Constants.DMDirName);
                }
                else 
                {
                candidates.Add(currentProjectPath + Path.DirectorySeparatorChar + Constants.DMDirName);
                }
            }
            candidates.AddRange(extraDMDirs);
            if (!noDefaultPaths)
                candidates.Add(GetRegistryValue(Constants.registryDMDirKey));
            foreach (string dmDir in candidates)
            {
                if (Directory.Exists(dmDir))
                    dmDirs.Add(dmDir);
            }
            return dmDirs.ToArray();
        }

        /// <summary>
        /// Get common document directory.
        /// </summary>
        /// <returns>directory path.</returns>
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
        static public string[] GetPluginDirs()
        {
            List<string> pluginDirs = new List<string>();

            {
                foreach (string pluginDir in extraPluginDirs)
                {
                    if (Directory.Exists(pluginDir))
                        pluginDirs.Add(pluginDir);
                }
            }

            if (!noDefaultPaths)
            {

                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser;
                    {
                        Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(Constants.registryEnvKey);
                        if (subkey != null)
                        {
                            string pluginDir = (string)subkey.GetValue(Constants.registryPluginDirKey);
                            if (pluginDir != null && Directory.Exists(pluginDir))
                                pluginDirs.Add(pluginDir);
                            subkey.Close();
                        }
                    }
                    {
                        Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(Constants.registrySWKey);
                        if (subkey != null)
                        {
                            string pluginDir = (string)subkey.GetValue(Constants.registryPluginDirKey);
                            if (pluginDir != null && Directory.Exists(pluginDir))
                                pluginDirs.Add(pluginDir);
                            subkey.Close();
                        }
                    }
                }

                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
                    {
                        Microsoft.Win32.RegistryKey subkey = key.OpenSubKey(Constants.registrySWKey);
                        if (subkey != null)
                        {
                            string pluginDir = (string)subkey.GetValue(Constants.registryPluginDirKey);
                            if (pluginDir != null && Directory.Exists(pluginDir))
                                pluginDirs.Add(pluginDir);
                            subkey.Close();
                        }
                    }
                }
            }

            return pluginDirs.ToArray();
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
        /// Check whether id contains the string except for '\', '/', '$', '~' or '%'.
        /// </summary>
        /// <param name="l_key">id</param>
        /// <returns>if contain, return true.</returns>
        static public bool IsNGforIDonWindows(string l_key)
        {
            for (int i = 0; i < l_key.Length; i++)
            {
                if (l_key[i] == '\\' || l_key[i] == '/'
                    || l_key[i] == ':' || l_key[i] == ';'
                    || l_key[i] == '*' || l_key[i] == '?'
                    || l_key[i] == '|' || l_key[i] == '\"'
                    || l_key[i] == '<' || l_key[i] == '>'
                    || l_key[i] == '~')
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether this object have the logging data.
        /// </summary>
        /// <param name="obj">the checked object.</param>
        /// <returns>if the object have the logging data, return true.</returns>
        static public bool IsLogged(EcellObject obj)
        {
            bool isLog = false;
            foreach (EcellData d in obj.Value)
            {
                if (d.Logged)
                {
                    isLog = true;
                    break;
                }
            }
            return isLog;
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
            String topDir = Path.GetTempPath() + "\\E-Cell IDE";
            if (!Directory.Exists(topDir))
            {
                Directory.CreateDirectory(topDir);
            }
            return topDir;
//            return GetRegistryValue(Constants.registryTmpDirKey);
        }

        /// <summary>
        /// Get the user directory.
        /// </summary>
        /// <returns></returns>
        public static string GetUserDir()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "E-Cell IDE");
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
                l_subkey = l_key.OpenSubKey(Constants.registryEnvKey);
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
                l_subkey = l_key.OpenSubKey(Constants.registrySWKey);
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
                l_subkey = l_key.OpenSubKey(Constants.registrySWKey);
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
                l_subkey = l_key.OpenSubKey(Constants.registrySWKey, true);
                /*
                l_currentDir = (string)l_subkey.GetValue(Constants.registryBaseDirKey);
                if (l_currentDir == null)
                {
                    RegistrySecurity s = l_subkey.GetAccessControl();
                    
                    l_subkey.CreateSubKey(Constants.registryBaseDirKey);
                }*/
                l_subkey.SetValue(Constants.registryLang, l_lang);
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
        /// Convert from name to full path.
        /// </summary>
        /// <param name="key">system name.</param>
        /// <param name="prop">property name.</param>
        /// <returns>full path.</returns>
        static public string ConvertSystemEntityPath(string key, string prop)
        {
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
                l_subkey = l_key.OpenSubKey(Constants.registryEnvKey, true);
                /*
                l_currentDir = (string)l_subkey.GetValue(Constants.registryBaseDirKey);
                if (l_currentDir == null)
                {
                    RegistrySecurity s = l_subkey.GetAccessControl();
                    
                    l_subkey.CreateSubKey(Constants.registryBaseDirKey);
                }*/
                l_subkey.SetValue(Constants.registryBaseDirKey, l_basedir);
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
        /// Convert the file name that decide in E-Cell Core from entity key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetOutputFileName(string key)
        {
            string fileName = key.Replace(":", "_"); ;
            return fileName.Replace("/", "_") + ".csv";
        }

        /// <summary>
        /// Add the specified directory to the plug-in search path list returned by GetPluginDirs()
        /// </summary>
        /// <param name="pluginDir">the plugin directory to include</param>
        public static void AddPluginDir(string pluginDir)
        {
            extraPluginDirs.Add(pluginDir);
        }

        /// <summary>
        /// Add the specified directory to the DM search path list returned by GetDMDirs()
        /// </summary>
        /// <param name="dmDir">the dm directory to include</param>
        public static void AddDMDir(string dmDir)
        {
            extraDMDirs.Add(dmDir);
        }

        /// <summary>
        /// Call this method to prevent PluginManage from loading plugins from default locations.
        /// </summary>
        public static void OmitDefaultPaths()
        {
            noDefaultPaths = true;
        }

        /// <summary>
        /// Build the full path from the information of components.
        /// </summary>
        /// <param name="type">the data type.</param>
        /// <param name="systemPath">the system directory path.</param>
        /// <param name="localID">the local ID.</param>
        /// <returns>The full ID.</returns>
        public static string BuildFullID(string type, string systemPath, string localID)
        {
            return type + Constants.delimiterColon + systemPath + Constants.delimiterColon + localID;
        }

        /// <summary>
        /// Build the full PN from the information of components.
        /// </summary>
        /// <param name="type">the data type.</param>
        /// <param name="systemPath">the system directory path.</param>
        /// <param name="localID">the local ID.</param>
        /// <param name="propName">the property name.</param>
        /// <returns>the full PN.</returns>
        public static string BuildFullPN(string type, string systemPath, string localID, string propName)
        {
            return BuildFullID(type, systemPath, localID) + Constants.delimiterColon + propName;
        }

        /// <summary>
        /// Strip the string with white spaces.
        /// </summary>
        /// <param name="val">the string data before split.</param>
        /// <returns>the string data after split.</returns>
        public static string StripWhitespaces(string val)
        {
            int startIdx = 0, len = val.Length, endIdx = len;

            for (; startIdx < len; ++startIdx)
            {
                if (!Char.IsWhiteSpace(val[startIdx]))
                    break;
            }

            while (--endIdx >= 0)
            {
                if (!Char.IsWhiteSpace(val[endIdx]))
                    break;
            }

            return val.Substring(startIdx, endIdx - startIdx + 1);
        }

        /// <summary>
        /// Copy Directory.
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (!System.IO.Directory.Exists(destDirName))
            {
                System.IO.Directory.CreateDirectory(destDirName);
                System.IO.File.SetAttributes(destDirName,
                    System.IO.File.GetAttributes(sourceDirName));
            }
            if (destDirName[destDirName.Length - 1] !=
                    System.IO.Path.DirectorySeparatorChar)
                destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;

            string[] files = System.IO.Directory.GetFiles(sourceDirName);
            foreach (string file in files)
                System.IO.File.Copy(file,
                    destDirName + System.IO.Path.GetFileName(file), true);

            string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
                CopyDirectory(dir, destDirName + System.IO.Path.GetFileName(dir));
        }


        /// <summary>
        /// Get the name and the parent path from the full object path.
        /// </summary>
        /// <param name="fullPath">the input full path.</param>
        /// <param name="path">the parent path.</param>
        /// <returns>the name of object.</returns>
        public static string GetNameFromPath(string fullPath, ref string path)
        {
            string result = "";
            string[] elements;
            if (fullPath.Contains(":"))
            {
                elements = fullPath.Split(new char[] { ':' });
                path = elements[0];
                result = elements[elements.Length - 1];
            }
            else
            {
                if (fullPath == "/")
                {
                    path = "/";
                    result = "/";
                }
                else
                {
                    elements = fullPath.Split(new char[] { '/' });
                    for (int i = 1; i < elements.Length - 1; i++)
                    {
                        path = path + "/" + elements[i];
                    }
                    result = elements[elements.Length - 1];
                }
            }
            return result;
        }

        /// <summary>
        /// Get key moved to another system.
        /// </summary>
        /// <param name="originalKey"></param>
        /// <param name="originalSystemKey"></param>
        /// <param name="newSystemKey"></param>
        /// <returns></returns>
        public static string GetMovedKey(string originalKey, string originalSystemKey, string newSystemKey)
        {
            if (null == originalKey || null == originalSystemKey || null == newSystemKey)
                return null;
            string newKey;
            if (originalSystemKey.Equals("/") && !newSystemKey.Equals("/"))
                newKey = newSystemKey + originalKey.Substring(1);
            else if (!originalSystemKey.Equals("/") && newSystemKey.Equals("/"))
                newKey = originalKey.Replace(originalSystemKey, "/");
            else
                newKey = originalKey.Replace(originalSystemKey, newSystemKey);
            return newKey.Replace("//", "/");
        }

    }
}
