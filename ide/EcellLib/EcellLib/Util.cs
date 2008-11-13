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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Win32;
using Ecell.Objects;

namespace Ecell
{
    public partial class Util
    {
        /// <summary>
        /// Get the set value from register
        /// </summary>
        /// <param name="intendedKey">registry key.</param>
        /// <returns>the value.</returns>
        static private string GetRegistryValue(string intendedKey)
        {
            string currentDir = null;
            RegistryKey key = Registry.CurrentUser;
            RegistryKey subkey = null;
            try
            {
                // Get Environment parameter.
                subkey = key.OpenSubKey(Constants.registryEnvKey);
                currentDir = (string)subkey.GetValue(intendedKey);
                if (currentDir != null)
                    return currentDir;

                // Get Software parameter.
                subkey = key.OpenSubKey(Constants.registrySWKey);
                if (subkey != null)
                {
                    currentDir = (string)subkey.GetValue(intendedKey);
                    if (currentDir != null)
                        return currentDir;
                }

                // Get Local parameter.
                key = Registry.LocalMachine;
                subkey = key.OpenSubKey(Constants.registrySWKey);
                if (subkey != null)
                {
                    currentDir = (string)subkey.GetValue(intendedKey);
                }
                return currentDir;
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
                if (subkey != null)
                {
                    subkey.Close();
                }
            }
        }
    }

    /// <summary>
    /// Class to manage the common function.
    /// </summary>
    public partial class Util
    {
        /// <summary>
        /// Check whether id contains the string except for letter, digit or '_'.
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>if contain, return true.</returns>
        static public bool IsNGforID(string key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                if (Char.IsLetterOrDigit(key[i]) ||
                    key[i] == '_') continue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether id contains the string except for '\', '/', '$', '~' or '%'.
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>if contain, return true.</returns>
        static public bool IsNGforIDonWindows(string key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] == '\\' || key[i] == '/'
                    || key[i] == ':' || key[i] == ';'
                    || key[i] == '*' || key[i] == '?'
                    || key[i] == '|' || key[i] == '\"'
                    || key[i] == '<' || key[i] == '>'
                    || key[i] == '~')
                return true;
            }
            return false;
        }

        static public bool IsReservedID(string key)
        {
            string ID = key.ToUpper();
            if (ID.EndsWith(":SIZE") || ID.Equals("SIZE"))
                return true;
            return false;
        }

        /// <summary>
        /// Check whether this id of system is NG.
        /// </summary>
        /// <param name="key">the system id.</param>
        /// <returns>correct is false.</returns>
        static public bool IsNGforSystemFullID(string key)
        {
            int delCount = 0;
            bool isDel = false;
            for (int i = 0; i < key.Length; i++)
            {
                if (!Char.IsLetterOrDigit(key[i]) &&
                    key[i] != '_' &&
                    key[i] != '/' &&
                    key[i] != ':') return true;
                if (key[i] == '/')
                {
                    if (isDel == true) return true;
                    isDel = true;
                }
                else
                {
                    isDel = false;
                }
                if (key[i] == ':') delCount++;
            }
            if (delCount > 0) return true;
            return false;
        }

        /// <summary>
        /// Check whether this id of component object(process or variable) is NG.
        /// </summary>
        /// <param name="key">the component id.</param>
        /// <returns>correct is false.</returns>
        static public bool IsNGforComponentFullID(string key)
        {
            int delCount = 0;
            bool isDel = false;
            for (int i = 0; i < key.Length; i++)
            {
                if (!Char.IsLetterOrDigit(key[i]) &&
                    key[i] != '_' &&
                    key[i] != '/' &&
                    key[i] != ':') return true;
                if (key[i] == '/')
                {
                    if (isDel == true) return true;
                    isDel = true;
                }
                else
                {
                    isDel = false;
                }
                if (key[i] == ':') delCount++;
            }
            if (delCount > 1) return true;
            if (delCount <= 0) return true;
            return false;
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

        public static string BuildFullPN(string fullID, string propName)
        {
            return fullID + Constants.delimiterColon + propName;
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
            return BuildFullPN(BuildFullID(type, systemPath, localID), propName);
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
        /// Copy File
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="targetDir"></param>
        public static void CopyFile(string filename, string targetDir)
        {
            File.Copy(filename, Path.Combine(targetDir, Path.GetFileName(filename)), true);
        }

        /// <summary>
        /// Copy Directory.
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            if (sourceDir.Equals(targetDir))
                targetDir = GetNewDir(targetDir);
            else if (Directory.Exists(targetDir))
                targetDir = GetNewDir(targetDir);

            // List up directories and files.
            string[] dirs = System.IO.Directory.GetDirectories(sourceDir, "*.*", SearchOption.AllDirectories);
            string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

            // Create directory if necessary.
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                File.SetAttributes(targetDir, File.GetAttributes(sourceDir));
            }
            // Copy directories.
            foreach (string dir in dirs)
                Directory.CreateDirectory(dir.Replace(sourceDir, targetDir));
            // Copy Files.
            foreach (string file in files)
                File.Copy(file, file.Replace(sourceDir, targetDir));
        }

        /// <summary>
        /// Get New Directory name.
        /// </summary>
        /// <param name="targetDir"></param>
        /// <returns></returns>
        public static string GetNewDir(string targetDir)
        {
            int revNo = 0;
            string newDir = "";
            do
            {
                revNo++;
                newDir = targetDir + revNo.ToString();
            } while (Directory.Exists(newDir));
            return newDir;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <returns></returns>
        public static string GetRevNo(string sourceDir)
        {
            int revNo = 0;
            string revision = "";
            do
            {
                revNo++;
                revision = "Revision" + revNo.ToString();
            } while (Directory.Exists(Path.Combine(sourceDir, revision)));
            return revision;
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

        public static string GetNewProjectName()
        {
            string baseDir = Util.GetBaseDir();
            string preName = "project";
            int i = 1;
            for (;;)
            {
                string prjName = preName + i;
                string prjPath = Path.Combine(baseDir, prjName);
                if (!Directory.Exists(prjPath) &&
                    !File.Exists(prjPath))
                    return prjName;

                i++;
            }
        }

        public static void ParseEntityKey(string str, out string systemPath, out string localID)
        {
            int idx = str.LastIndexOf(Constants.delimiterColon);
            if (idx < 0)
                throw new ApplicationException("Malformed entity key: " + str);
            systemPath = str.Substring(0, idx);
            localID = str.Substring(idx + 1);
        }

        public static void SplitSystemPath(string str, out string parentSystemPath, out string localID)
        {
            int idx = str.LastIndexOf(Constants.delimiterPath);
            if (idx < 0)
            {
                parentSystemPath = null;
                localID = Constants.delimiterPath.ToString();
                return;
            }
            if (idx == 0)
            {
                if (str.Length == 1)
                {
                    parentSystemPath = "";
                    localID = "/";
                    return;
                }
                parentSystemPath = "/";
            }
            else
            {
                parentSystemPath = str.Substring(0, idx);
            }
            localID = str.Substring(idx + 1);
        }

        public static void ParseFullID(string str, out string type, out string systemPath, out string localID)
        {
            string[] parts = str.Split(Constants.delimiterColon.ToCharArray(), 3);
            if (parts.Length != 3)
                throw new ApplicationException("Malformed FullID: " + str);
            type = parts[0];
            systemPath = parts[1];
            localID = parts[2];
        }

        public static void ParseFullPN(string str, out string type, out string systemPath, out string localID, out string propName)
        {
            string[] parts = str.Split(Constants.delimiterColon.ToCharArray(), 4);
            if (parts.Length != 4)
                throw new ApplicationException("Malformed FullID: " + str);
            type = parts[0];
            systemPath = parts[1];
            localID = parts[2];
            propName = parts[3];
        }

        public static string GetSuperSystemPath(string systemPath)
        {
            Regex postColonRegex = new Regex(":\\w*$");
            Regex postSlashRegex = new Regex("/\\w*$");
            if (systemPath == null || systemPath.Equals("") || systemPath.Equals("/"))
                return "";
            else if (systemPath.Contains(":"))
            {
                return postColonRegex.Replace(systemPath, "");
            }
            else
            {
                string retval = postSlashRegex.Replace(systemPath, "");
                if (retval.Equals(""))
                    return "/";
                else
                    return retval;
            }
        }

        public static string NormalizeSystemPath(string systemPath, string currentSystemPath)
        {
            if (systemPath[0] == '/')
                return systemPath;

            if (systemPath.Length == 0)
                return currentSystemPath;

            Debug.Assert(currentSystemPath != null && currentSystemPath[0] == '/');

            List<string> retval = new List<string>(currentSystemPath.Split('/'));

            foreach (string comp in systemPath.Split('/'))
            {
                if (comp == "..")
                {
                    if (retval.Count > 1)
                        retval.RemoveAt(retval.Count - 1);
                }
                else if (comp != ".")
                {
                    retval.Add(comp);
                }
            }

            return string.Join("/", retval.ToArray());
        }

        public static EcellValue NormalizeVariableReference(EcellValue val, string systemPath)
        {
            if (!val.IsList)
                throw new ArgumentException();
            ArrayList newComps = new ArrayList();
            foreach (object i in (IEnumerable)val.Value)
                newComps.Add(i);
            if (!(newComps[1] is string))
                throw new ArgumentException();
            string entityType, path, localID;
            ParseFullID((string)newComps[1], out entityType, out path, out localID);
            newComps[1] = new EcellValue(
                BuildFullID(entityType, NormalizeSystemPath(path, systemPath), localID));
            return new EcellValue(newComps);
        }

        public static string GenerateRandomID(int len)
        {
            StringBuilder sb = new StringBuilder();
            string usableCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random r = new Random();
            for (int i = 0; i < len; ++i)
            {
                sb.Append(usableCharacters[r.Next(0, usableCharacters.Length)]);
            }
            return sb.ToString();
        }

        public static bool Contains(System.Collections.IEnumerable i, object o)
        {
            foreach (object _o in i)
            {
                if (_o == o)
                    return true;
            }
            return false;
        }
    }

    public partial class Util
    {
        /// <summary>
        /// Set the language of application when application is start.
        /// </summary>
        static public void InitialLanguage()
        {
            CultureInfo lang = Util.GetLanguage();
            if (lang != null && lang != CultureInfo.InvariantCulture)
            {
                Thread.CurrentThread.CurrentUICulture = lang;
            }
        }

        /// <summary>
        /// Get the language from register.
        /// </summary>
        /// <returns></returns>
        static public CultureInfo GetLanguage()
        {
            string isoTwoLetterLangCode = GetRegistryValue(Constants.registryLang);
            if (isoTwoLetterLangCode != null)
            {
                isoTwoLetterLangCode = isoTwoLetterLangCode.Replace('_', '-');
                if (isoTwoLetterLangCode != CultureInfo.InvariantCulture.TwoLetterISOLanguageName)
                {
                    try
                    {
                        return CultureInfo.GetCultureInfo(isoTwoLetterLangCode);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e);
                    }
                }
            }
            return CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Set language for E-Cell IDE.
        /// </summary>
        /// <param name="lang">language.</param>
        static public void SetLanguage(CultureInfo lang)
        {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey subkey = key.CreateSubKey(Constants.registrySWKey);
            using (subkey) subkey.SetValue(Constants.registryLang, lang.TwoLetterISOLanguageName);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public partial class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowErrorDialog(string msg)
        {
            MessageBox.Show(msg, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowWarningDialog(string msg)
        {
            MessageBox.Show(msg, "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowNoticeDialog(string msg)
        {
            MessageBox.Show(msg, "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ShowYesNoDialog(string msg)
        {
            return MessageBox.Show(msg, "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes;
        }
        /// <summary>
        /// 
        /// </summary>
        public class CancelException : Exception {}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ShowYesNoCancelDialog(string msg)
        {
            switch (MessageBox.Show(msg, "Confirmation",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    return true;
                case DialogResult.No:
                    return false;
                default:
                    throw new CancelException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ShowOKCancelDialog(string msg)
        {
            return MessageBox.Show(msg, "Confirmation",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                    DialogResult.OK;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public partial class Util
    {
        /// <summary>
        /// Additional plugin directories to be searched on startup.
        /// XXX: this should not be in Util class.
        /// </summary>
        private static List<string> s_extraPluginDirs = new List<string>();

        /// <summary>
        /// Additional DM directories to be searched on startup.
        /// XXX: this should not be in Util class.
        /// </summary>
        private static List<string> s_extraDMDirs = new List<string>();

        /// <summary>
        /// Whether to include default plugin / DM paths.
        /// </summary>
        private static bool s_noDefaultPaths;

        private static string s_windowSettingDir = GetRegistryValue(Constants.registryWinSetDir);

        /// <summary>
        /// Get the temporary directory from register.
        /// </summary>
        /// <returns></returns>
        public static string GetTmpDir()
        {
            String topDir = Path.GetTempPath() + Path.DirectorySeparatorChar + "E-Cell IDE";
            if (!Directory.Exists(topDir))
            {
                Directory.CreateDirectory(topDir);
            }
            return topDir;
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
        /// Set the working directory to set directiroy.
        /// </summary>
        /// <param name="basedir">set directory.</param>
        public static void SetBaseDir(string basedir)
        {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey subkey = null;
            try
            {
                subkey = key.OpenSubKey(Constants.registryEnvKey, true);
                subkey.SetValue(Constants.registryBaseDirKey, basedir);
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
                if (subkey != null)
                {
                    subkey.Close();
                }
            }
        }

        /// <summary>
        /// Call this method to prevent PluginManager from loading plugins from default locations.
        /// </summary>
        public static void OmitDefaultPaths()
        {
            s_noDefaultPaths = true;
        }

        /// <summary>
        /// Get the analysis directory from register.
        /// </summary>
        /// <returns></returns>
        public static string GetAnalysisDir()
        {
            return GetRegistryValue(Constants.registryAnalysisDirKey);
        }

        /// <summary>
        /// Get the directory of window setting..
        /// </summary>
        /// <returns>the directory path.</returns>
        public static string GetWindowSettingDir()
        {
            return s_windowSettingDir;
        }

        public static void SetWindowSettingDir(string dir)
        {
            s_windowSettingDir = dir;
        }

        /// <summary>
        /// Get the working directory from register.
        /// </summary>
        /// <returns>the working directory.</returns>
        public static string GetBaseDir()
        {
            string path = GetRegistryValue(Constants.registryBaseDirKey);
            if (path == null)
            {
                path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path = Path.Combine(path, "My E-Cell Projects");
                SetBaseDir(path);
            }
            return path;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetStartupFile()
        {
            return GetRegistryValue(Constants.registryStartup);
        }

        /// <summary>
        /// Get the DM direcory from register.
        /// </summary>
        /// <returns>DM directory.</returns>
        public static string[] GetDMDirs(String currentProjectPath)
        {
            List<string> dmDirs = new List<string>();
            List<string> candidates = new List<string>();
            if (currentProjectPath != null)
            {
                candidates.Add(Path.Combine(currentProjectPath, Constants.DMDirName));
            }
            candidates.AddRange(s_extraDMDirs);
            if (!s_noDefaultPaths)
                candidates.Add(GetRegistryValue(Constants.registryDMDirKey));
            foreach (string dmDir in candidates)
            {
                if (Directory.Exists(dmDir))
                    dmDirs.Add(dmDir);
            }
            return dmDirs.ToArray();
        }

        /// <summary>
        /// Get the DM dictionary.
        /// </summary>
        /// <param name="dmDir"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetDmDic(string dmDir)
        {
            Dictionary<string, List<string>> dmDic = new Dictionary<string, List<string>>();
            dmDic = new Dictionary<string, List<string>>();
            // 4 Process
            dmDic.Add(Constants.xpathProcess, new List<string>());
            // 4 Stepper
            dmDic.Add(Constants.xpathStepper, new List<string>());
            // 4 System
            List<string> systemList = new List<string>();
            systemList.Add(Constants.xpathSystem);
            dmDic.Add(Constants.xpathSystem, systemList);
            // 4 Variable
            List<string> variableList = new List<string>();
            variableList.Add(Constants.xpathVariable);
            dmDic.Add(Constants.xpathVariable, variableList);
            //
            // Searches the DM paths
            //
            string[] dmPathArray = GetDMDirs(dmDir);
            if (dmPathArray == null)
            {
                throw new Exception("ErrFindDmDir");
            }
            foreach (string dmPath in dmPathArray)
            {
                if (!Directory.Exists(dmPath))
                {
                    continue;
                }
                // 4 Process
                string[] processDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathProcess + Constants.FileExtDM
                    );
                foreach (string processDM in processDMArray)
                {
                    dmDic[Constants.xpathProcess].Add(Path.GetFileNameWithoutExtension(processDM));
                }
                // 4 Stepper
                string[] stepperDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathStepper + Constants.FileExtDM
                    );
                foreach (string stepperDM in stepperDMArray)
                {
                    dmDic[Constants.xpathStepper].Add(Path.GetFileNameWithoutExtension(stepperDM));
                }
                // 4 System
                string[] systemDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathSystem + Constants.FileExtDM
                    );
                foreach (string systemDM in systemDMArray)
                {
                    dmDic[Constants.xpathSystem].Add(Path.GetFileNameWithoutExtension(systemDM));
                }
                // 4 Variable
                string[] variableDMArray = Directory.GetFiles(
                    dmPath,
                    Constants.delimiterWildcard + Constants.xpathVariable + Constants.FileExtDM
                    );
                foreach (string variableDM in variableDMArray)
                {
                    dmDic[Constants.xpathVariable].Add(Path.GetFileNameWithoutExtension(variableDM));
                }
            }
            return dmDic;
        }

        /// <summary>
        /// Add the specified directory to the DM search path list returned by GetDMDirs()
        /// </summary>
        /// <param name="dmDir">the dm directory to include</param>
        public static void AddDMDir(string dmDir)
        {
            s_extraDMDirs.Add(dmDir);
        }


        /// <summary>
        /// Get common document directory.
        /// </summary>
        /// <returns>directory path.</returns>
        static public string GetCommonDocumentDir()
        {
            string currentDir = null;
            RegistryKey key = Registry.CurrentUser;
            RegistryKey subkey = null;
            try
            {
                key = Registry.LocalMachine;
                subkey = key.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders");
                if (subkey != null)
                {
                    currentDir = (string)subkey.GetValue("Common Documents");
                }
                return currentDir;
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
                if (subkey != null)
                {
                    subkey.Close();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public bool IsInstalledSDK()
        {
            string stageHome = System.Environment.GetEnvironmentVariable("ECELL_STAGING_HOME");
            if (String.IsNullOrEmpty(stageHome))
            {
                return false;
            }
            string VS80 = System.Environment.GetEnvironmentVariable("VS80COMNTOOLS");
            if (string.IsNullOrEmpty(VS80))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public bool IsDMFile(string fileName)
        {
            String name = Path.GetFileNameWithoutExtension(fileName);
            if (name.EndsWith(Constants.xpathProcess))
                return true;
            if (name.EndsWith(Constants.xpathStepper))
                return true;
            return false;
        }

        static public List<string> GetProcessTemplateList()
        {
            List<string> result = new List<string>();

            string confDir = Util.GetWindowSettingDir();
            string processDir = confDir + Constants.delimiterPath + Constants.xpathProcess;

            if (!Directory.Exists(processDir)) return result;
            string[] files = Directory.GetFiles(processDir);
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].EndsWith(Constants.xpathProcess) &&
                    !files[i].EndsWith(Constants.xpathStepper))
                    continue;
                string name = Path.GetFileName(files[i]);
                result.Add(name);
            }
            return result;
        }

        static public string GetProcessTemplate(string name)
        {
            string result = "";
            string line = "";
            string confDir = Util.GetWindowSettingDir();
            string processFile = confDir + Constants.delimiterPath + Constants.xpathProcess +
                Constants.delimiterPath + name;
            if (!File.Exists(processFile)) return result;

            TextReader l_reader = new StreamReader(processFile);
            while ((line = l_reader.ReadLine()) != null)
            {
                result += line + "\n";
            }
            l_reader.Close();

            return result;
        }

        /// <summary>
        /// Get the plugin directory from register.
        /// </summary>
        /// <returns>plugin directory.</returns>
        static public string[] GetPluginDirs()
        {
            List<string> pluginDirs = new List<string>();

            {
                foreach (string pluginDir in s_extraPluginDirs)
                {
                    if (Directory.Exists(pluginDir))
                        pluginDirs.Add(pluginDir);
                }
            }

            if (!s_noDefaultPaths)
            {

                {
                    RegistryKey key = Registry.CurrentUser;
                    {
                        RegistryKey subkey = key.OpenSubKey(Constants.registryEnvKey);
                        if (subkey != null)
                        {
                            string pluginDir = (string)subkey.GetValue(Constants.registryPluginDirKey);
                            if (pluginDir != null && Directory.Exists(pluginDir) && !pluginDirs.Contains(pluginDir))
                                pluginDirs.Add(pluginDir);
                            subkey.Close();
                        }
                    }
                    {
                        RegistryKey subkey = key.OpenSubKey(Constants.registrySWKey);
                        if (subkey != null)
                        {
                            string pluginDir = (string)subkey.GetValue(Constants.registryPluginDirKey);
                            if (pluginDir != null && Directory.Exists(pluginDir) && !pluginDirs.Contains(pluginDir))
                                pluginDirs.Add(pluginDir);
                            subkey.Close();
                        }
                    }
                }

                {
                    RegistryKey key = Registry.LocalMachine;
                    {
                        RegistryKey subkey = key.OpenSubKey(Constants.registrySWKey);
                        if (subkey != null)
                        {
                            string pluginDir = (string)subkey.GetValue(Constants.registryPluginDirKey);
                            if (pluginDir != null && Directory.Exists(pluginDir) && !pluginDirs.Contains(pluginDir))
                                pluginDirs.Add(pluginDir);
                            subkey.Close();
                        }
                    }
                }
            }

            return pluginDirs.ToArray();
        }

        /// <summary>
        /// Add the specified directory to the plug-in search path list returned by GetPluginDirs()
        /// </summary>
        /// <param name="pluginDir">the plugin directory to include</param>
        public static void AddPluginDir(string pluginDir)
        {
            s_extraPluginDirs.Add(pluginDir);
        }

        /// <summary>
        /// Returns the "bin" directory
        /// </summary>
        public static string GetBinDir()
        {
            return Path.GetDirectoryName(typeof(Util).Assembly.Location);
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
        /// Ignored directory names.
        /// </summary>
        public static string[] IgnoredDirList = {
            "Model",
            "Simulation",
            "Parameters",
            Constants.DMDirName
        };

        /// <summary>
        /// IsIgnoredDir
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsIgnoredDir(string dir)
        {
            string name = Path.GetFileNameWithoutExtension(dir);
            bool ignored = false;
            foreach (string ignoredDir in IgnoredDirList)
            {
                if (ignoredDir.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    ignored = true;
                    break;
                }
            }
            return ignored;
        }
        /// <summary>
        /// IsHidden
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool IsHidden(string dir)
        {
            FileAttributes fas = File.GetAttributes(dir);
            return ((fas & FileAttributes.Hidden) == FileAttributes.Hidden);
        }

        /// <summary>
        /// Get the object of color.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>color object.</returns>
        public static Color GetColor(int i)
        {
            int j = i % 3;
            if (j == 0) return Color.OrangeRed;
            else if (j == 1) return Color.LightSkyBlue;
            else if (j == 2) return Color.LightGreen;
            else if (j == 3) return Color.LightSalmon;
            else if (j == 4) return Color.Gold;
            else if (j == 5) return Color.LimeGreen;
            else if (j == 6) return Color.Coral;
            else if (j == 7) return Color.Navy;
            else if (j == 8) return Color.Lime;
            else if (j == 9) return Color.Purple;
            else if (j == 10) return Color.SkyBlue;
            else if (j == 11) return Color.Green;
            else if (j == 12) return Color.Plum;
            else if (j == 13) return Color.HotPink;
            else if (j == 14) return Color.Orchid;
            else if (j == 15) return Color.Tomato;
            else if (j == 16) return Color.Orange;
            else if (j == 17) return Color.Magenta;
            else if (j == 18) return Color.Blue;
            else if (j == 19) return Color.Red;
            else return Color.Black;
        }

        /// <summary>
        /// Get the brush of color.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>Brush.</returns>
        public static Brush GetColorBlush(int i)
        {
            int j = i % 3;
            if (j == 0) return Brushes.OrangeRed;
            else if (j == 1) return Brushes.LightSkyBlue;
            else if (j == 2) return Brushes.LightGreen;
            else if (j == 3) return Brushes.LightSalmon;
            else if (j == 4) return Brushes.Gold;
            else if (j == 5) return Brushes.LimeGreen;
            else if (j == 6) return Brushes.Coral;
            else if (j == 7) return Brushes.Navy;
            else if (j == 8) return Brushes.Lime;
            else if (j == 9) return Brushes.Purple;
            else if (j == 10) return Brushes.SkyBlue;
            else if (j == 11) return Brushes.Green;
            else if (j == 12) return Brushes.Plum;
            else if (j == 13) return Brushes.HotPink;
            else if (j == 14) return Brushes.Orchid;
            else if (j == 15) return Brushes.Tomato;
            else if (j == 16) return Brushes.Orange;
            else if (j == 17) return Brushes.Magenta;
            else if (j == 18) return Brushes.Blue;
            else if (j == 19) return Brushes.Red;
            else return Brushes.Black;
        }
    }
}
