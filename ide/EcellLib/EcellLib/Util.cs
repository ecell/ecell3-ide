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
// modified by Chihiro Okada <c_okada@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using Ecell.Objects;
using Ecell.Exceptions;

namespace Ecell
{
    /// <summary>
    /// partial class for Util.
    /// method to control registory.
    /// </summary>
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
        /// idParser
        /// </summary>
        private static Regex idParser = new Regex("[A-Za-z_]+(\\d+)$");

        /// <summary>
        /// Check whether id contains the string except for letter, digit or '_'.
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>if contain, return true.</returns>
        static public bool IsNGforID(string key)
        {
            if (string.IsNullOrEmpty(key))
                return true;
            if (key.Length > 128)
                return true;
            if (key.Equals(Constants.delimiterPath))
                return false;

            for (int i = 0; i < key.Length; i++)
            {
                if (Char.IsLetterOrDigit(key[i]) || key[i] == '_')
                    continue;
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
            if (string.IsNullOrEmpty(key))
                return true;
            if (key.Length > 128)
                return true;

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

        /// <summary>
        /// Check if this key is Reserved or not.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>return true if key is reserved string.</returns>
        static public bool IsReservedID(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            string ID = key.ToUpper();
            if (ID.EndsWith(":SIZE") || ID.Equals("SIZE"))
                return true;
            return false;
        }

        /// <summary>
        /// Check if this type is NG or not.
        /// </summary>
        /// <param name="type">object type.</param>
        /// <returns>return true if type is not reserved type.</returns>
        public static bool IsNGforType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return true;
            else if (type.Equals(EcellObject.SYSTEM))
                return false;
            else if (type.Equals(EcellObject.PROCESS))
                return false;
            else if (type.Equals(EcellObject.VARIABLE))
                return false;
            else if (type.Equals(EcellObject.TEXT))
                return false;
            else if (type.Equals(EcellObject.STEPPER))
                return false;
            else if (type.Equals(EcellObject.MODEL))
                return false;
            else if (type.Equals(EcellObject.PROJECT))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Check if this fullPN is NG or not.
        /// </summary>
        /// <param name="fullPN">FullPN</param>
        /// <returns>return true if FullPN is NG.</returns>
        public static bool IsNGforFullPN(string fullPN)
        {
            if (string.IsNullOrEmpty(fullPN) || !fullPN.Contains(":"))
                return true;
            int i = fullPN.LastIndexOf(':');
            string fullID = fullPN.Substring(0, i);
            string param = fullPN.Substring(i + 1);
            if (IsNGforID(param))
                return true;
            return IsNGforFullID(fullID);
        }

        /// <summary>
        /// Check if this fullID is NG or not.
        /// </summary>
        /// <param name="fullID">FullID</param>
        /// <returns>return true if FullID is NG.</returns>
        public static bool IsNGforFullID(string fullID)
        {
            if (string.IsNullOrEmpty(fullID))
                return true;
            if (!fullID.Contains(Constants.delimiterColon))
                return true;
            if (fullID.Split(':').Length != 3)
                return true;

            string type = fullID.Split(':')[0];
            if (IsNGforType(type))
                return true;

            string key = fullID.Substring(type.Length + 1);
            if (type.Equals(EcellObject.SYSTEM) && key != ":/")
                return IsNGforSystemKey(GetSuperSystemPath(key));
            else if (type.Equals(EcellObject.PROCESS) || type.Equals(EcellObject.VARIABLE) || type.Equals(EcellObject.TEXT))
                return IsNGforEntityKey(key);
            else
                return false;
        }

        /// <summary>
        /// Check whether this id of system is NG.
        /// </summary>
        /// <param name="key">the system id.</param>
        /// <returns>correct is false.</returns>
        static public bool IsNGforSystemKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return true;
            if (key[0] != '/')
                return true;

            bool isDel = false;
            for (int i = 0; i < key.Length; i++)
            {
                if (!Char.IsLetterOrDigit(key[i])
                    && key[i] != '_'
                    && key[i] != '/')
                    return true;

                if (key[i] == '/')
                {
                    if (isDel == true) 
                        return true;
                    isDel = true;
                }
                else
                {
                    isDel = false;
                }
            }
            return false;
        }

        /// <summary>
        /// Check whether this id of component object(process or variable) is NG.
        /// </summary>
        /// <param name="key">the component id.</param>
        /// <returns>correct is false.</returns>
        static public bool IsNGforEntityKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return true;
            if (key[0] != '/')
                return true;
            if (!key.Contains(":"))
                return true;

            int i = key.LastIndexOf(Constants.delimiterColon);
            string systemPath = key.Substring(0, i);
            string localID = key.Substring(i + 1);

            if (IsNGforSystemKey(systemPath))
                return true;
            if (IsNGforID(localID))
                return true;

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
            if (IsNGforSystemKey(key))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "key"));
            if (IsNGforID(prop))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "prop"));

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
            if (IsNGforType(type))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "type"));
            if (IsNGforSystemKey(systemPath) && !localID.Equals(Constants.delimiterPath))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "systemPath"));
            if (IsNGforID(localID))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "localID"));

            // build Full Path.
            return type + Constants.delimiterColon + systemPath + Constants.delimiterColon + localID;
        }

        /// <summary>
        /// Build the full PN from the information of components.
        /// </summary>
        /// <param name="fullID">FullID</param>
        /// <param name="propName">property name.</param>
        /// <returns>build FullPN</returns>
        public static string BuildFullPN(string fullID, string propName)
        {
            if (IsNGforFullID(fullID))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "fullID"));
            if (IsNGforID(propName))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "propName"));
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
            string fullID = BuildFullID(type, systemPath, localID);
            return BuildFullPN(fullID, propName);
        }

        /// <summary>
        /// Get new Revision No.
        /// </summary>
        /// <param name="sourceDir">source directory.</param>
        /// <returns>the last revision.</returns>
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
        /// Get key moved to another system.
        /// </summary>
        /// <param name="originalKey">the original key.</param>
        /// <param name="originalSystemKey">the original system key.</param>
        /// <param name="newSystemKey">the new system key.</param>
        /// <returns>the new key.</returns>
        public static string GetMovedKey(string originalKey, string originalSystemKey, string newSystemKey)
        {
            if (IsNGforSystemKey(originalSystemKey))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "originalSystemKey"));
            if (IsNGforSystemKey(newSystemKey))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "newSystemKey"));
            if (IsNGforSystemKey(originalKey) && IsNGforEntityKey(originalKey))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "originalKey"));

            string newKey;
            if (originalSystemKey.Equals(Constants.delimiterPath) && newSystemKey.Equals(Constants.delimiterPath))
                newKey = originalKey;
            else if (originalSystemKey.Equals(Constants.delimiterPath) && !newSystemKey.Equals(Constants.delimiterPath))
                newKey = newSystemKey + originalKey.Replace("/:", ":");
            else if (!originalSystemKey.Equals(Constants.delimiterPath) && newSystemKey.Equals(Constants.delimiterPath))
                newKey = originalKey.Replace(originalSystemKey, Constants.delimiterPath);
            else
                newKey = newSystemKey + originalKey.Substring(originalSystemKey.Length).Replace("/:", ":");
            return newKey.Replace("//", "/");
        }

        /// <summary>
        /// Get new project name from source directory.
        /// </summary>
        /// <returns>new project name.</returns>
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

        /// <summary>
        /// Split Object Key to SystemPath and LocalID.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="systemPath">the system key</param>
        /// <param name="localID">the got local ID.</param>
        public static void ParseKey(string key, out string systemPath, out string localID)
        {
            if (string.IsNullOrEmpty(key))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "key"));
            if (!key.Contains(":"))
                ParseSystemKey(key, out systemPath, out localID);
            else
                ParseEntityKey(key, out systemPath, out localID);
        }

        /// <summary>
        /// Split Entity Key to SystemPath and LocalID.
        /// </summary>
        /// <param name="key">the key.</param>
        /// <param name="systemPath">the system key.</param>
        /// <param name="localID">the got local ID.</param>
        public static void ParseEntityKey(string key, out string systemPath, out string localID)
        {
            if (IsNGforEntityKey(key))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "key"));

            int i = key.LastIndexOf(Constants.delimiterColon);
            systemPath = key.Substring(0, i);
            localID = key.Substring(i + 1);
        }

        /// <summary>
        /// Split systemPath to ParentSystemPath and LocalID.
        /// </summary>
        /// <param name="systemKey">the system key.</param>
        /// <param name="parentSystemPath">the got parent system key.</param>
        /// <param name="localID">the got local ID.</param>
        public static void ParseSystemKey(string systemKey, out string parentSystemPath, out string localID)
        {
            if (IsNGforSystemKey(systemKey))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "systemKey"));
            if (systemKey.Equals("/"))
            {
                parentSystemPath = "";
                localID = Constants.delimiterPath;
                return;
            }
            int i = systemKey.LastIndexOf(Constants.delimiterPath);
            if (i == 0)
            {
                parentSystemPath = Constants.delimiterPath;
            }
            else
            {
                parentSystemPath = systemKey.Substring(0, i);
            }
            localID = systemKey.Substring(i + 1);

        }

        /// <summary>
        /// Parse FullID to Type, SystemPath and LocalID.
        /// </summary>
        /// <param name="fullID">FullID.</param>
        /// <param name="type">the got type.</param>
        /// <param name="systemPath">the got system key.</param>
        /// <param name="localID">the got local ID.</param>
        public static void ParseFullID(string fullID, out string type, out string systemPath, out string localID)
        {
            string key;
            ParseFullID(fullID, out type, out key);

            if (type.Equals(EcellObject.SYSTEM))
                ParseSystemKey(key, out systemPath, out localID);
            else
                ParseEntityKey(key, out systemPath, out localID);
        }

        /// <summary>
        /// Parse FullID to Type, SystemPath and LocalID.
        /// </summary>
        /// <param name="fullID">FullID</param>
        /// <param name="type">the got type.</param>
        /// <param name="key">the got key.</param>
        public static void ParseFullID(string fullID, out string type, out string key)
        {
            if (IsNGforFullID(fullID))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "fullID"));

            int i = fullID.IndexOf(Constants.delimiterColon);
            type = fullID.Substring(0, i);
            key = fullID.Substring(i + 1);

            if (key.Equals(":/"))
                key = Constants.delimiterPath;
            else if (type.Equals(EcellObject.SYSTEM))
            {
                key = key.Replace("/:", Constants.delimiterPath);
                key = key.Replace(Constants.delimiterColon, Constants.delimiterPath);
            }
            else if (type.Equals(EcellObject.STEPPER))
            {
                key = key.Substring(1);
            }
        }
        /// <summary>
        /// Parse FullPN to Type, SystemPath, LocalID and PropertyName.
        /// </summary>
        /// <param name="fullPN">Full Property Path.</param>
        /// <param name="type">Type of entity.</param>
        /// <param name="key">Key of entity</param>
        /// <param name="propName">PropertyName</param>
        public static void ParseFullPN(string fullPN, out string type, out string key, out string propName)
        {
            if (IsNGforFullPN(fullPN))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "fullPN"));
            int i = fullPN.LastIndexOf(':');
            propName = fullPN.Substring(i + 1);
            string fullID = fullPN.Substring(0,i);
            ParseFullID(fullID, out type, out key);
        }


        /// <summary>
        /// Get temporary id from localID.
        /// </summary>
        /// <param name="localID">local ID</param>
        /// <returns>the temporary ID.</returns>
        public static int ParseTemporaryID(string localID)
        {
            int i = 0;
            if (string.IsNullOrEmpty(localID))
                return i;

            Match match = idParser.Match(localID);
            if (match.Success)
            {
                string num = match.Groups[1].Value;
                i = int.Parse(num);
            }
            return i;
        }

        /// <summary>
        /// GetSuperSystemPath
        /// </summary>
        /// <param name="systemPath">the system path.</param>
        /// <returns>the parent system path</returns>
        public static string GetSuperSystemPath(string systemPath)
        {
            Regex postColonRegex = new Regex(":\\w*$");
            if (string.IsNullOrEmpty(systemPath) || systemPath.Equals(Constants.delimiterPath))
                return "";
            else if (systemPath.Contains(":"))
            {
                return postColonRegex.Replace(systemPath, "");
            }
            else
            {
                string parentSys;
                string localID;
                ParseSystemKey(systemPath, out parentSys, out localID);
                return parentSys;
            }
        }
        /// <summary>
        /// Normalize the system path.
        /// </summary>
        /// <param name="systemPath">the system path.</param>
        /// <param name="currentSystemPath">the current system path.</param>
        /// <returns>the normalized system path.</returns>
        public static string NormalizeSystemPath(string systemPath, string currentSystemPath)
        {
            if (string.IsNullOrEmpty(systemPath))
                return currentSystemPath;

            if (systemPath[0] == '/')
                return systemPath;

            if (string.IsNullOrEmpty(currentSystemPath) || currentSystemPath[0] != '/')
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "currentSystemPath"));
            //
            List<string> retval = new List<string>(currentSystemPath.Split('/'));
            retval.RemoveAt(0);
            //
            foreach (string comp in systemPath.Split('/'))
            {
                if (comp == "..")
                {
                    if (retval.Count > 0)
                        retval.RemoveAt(retval.Count - 1);
                }
                else if (comp != ".")
                {
                    retval.Add(comp);
                }
            }

            return "/" + string.Join("/", retval.ToArray());
        }
        /// <summary>
        /// Normalize VariableReference from EcellReference
        /// </summary>
        /// <param name="er">the reference object.</param>
        /// <param name="systemPath">the system path.</param>
        public static void NormalizeVariableReference(EcellReference er, string systemPath)
        {
            string path, localID;
            int i = er.Key.LastIndexOf(Constants.delimiterColon);
            path = er.Key.Substring(0, i);
            localID = er.Key.Substring(i + 1);
            er.Key = NormalizeSystemPath(path, systemPath) + Constants.delimiterColon + localID;
        }

        /// <summary>
        /// Is there any difference or not between two processes.
        /// </summary>
        /// <param name="oldObj">the old object.</param>
        /// <param name="newObj">the new object.</param>
        /// <returns>return true if VariableReference is changed.</returns>
        public static bool DoesVariableReferenceChange(EcellObject oldObj, EcellObject newObj)
        {
            if (oldObj == null || !(oldObj is EcellProcess))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "oldObj"));
            if (newObj == null || !(newObj is EcellProcess))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "newObj"));

            EcellValue varRef1 = oldObj.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST);
            EcellValue varRef2 = newObj.GetEcellValue(EcellProcess.VARIABLEREFERENCELIST);

            if (varRef1 == null && varRef2 == null)
                return false;
            else if (varRef1 == null)
                return true;
            else 
                return !varRef1.Equals(varRef2);
        }

        /// <summary>
        /// Is there any difference or not between two processes.
        /// </summary>
        /// <param name="oldObj">the old object.</param>
        /// <param name="newObj">the new object.</param>
        /// <returns>return true if Expression is changed.</returns>
        public static bool DoesExpressionChange(EcellObject oldObj, EcellObject newObj)
        {
            if (oldObj == null || !(oldObj is EcellProcess))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "oldObj"));
            if (newObj == null || !(newObj is EcellProcess))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "newObj"));

            EcellData d1 = oldObj.GetEcellData(Constants.xpathExpression);
            EcellData d2 = newObj.GetEcellData(Constants.xpathExpression);

            if (d1 == d2)
                return false;
            else if (d1 == null || d2 == null)
                return true;
            string e1 = d1.Value.ToString();
            string e2 = d2.Value.ToString();

            return !e1.Equals(e2);
        }

        /// <summary>
        /// Is there any difference or not between two processes.
        /// </summary>
        /// <param name="oldObj">the old object.</param>
        /// <param name="newObj">the new object.</param>
        /// <returns>return true if Activity is changed.</returns>
        public static bool DoesActivityChange(EcellObject oldObj, EcellObject newObj)
        {
            if (oldObj == null || !(oldObj is EcellProcess))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "oldObj"));
            if (newObj == null || !(newObj is EcellProcess))
                throw new EcellException(string.Format(MessageResources.ErrInvalidParam, "newObj"));

            EcellData d1 = oldObj.GetEcellData(Constants.xpathActivity);
            EcellData d2 = newObj.GetEcellData(Constants.xpathActivity);

            if (d1 == d2)
                return false;
            else if (d1 == null || d2 == null)
                return true;
            string e1 = d1.Value.ToString();
            string e2 = d2.Value.ToString();

            return !e1.Equals(e2);
        }

        /// <summary>
        /// Generate Random ID
        /// </summary>
        /// <param name="len">the id length.</param>
        /// <returns>the random ID.</returns>
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
    }
    /// <summary>
    /// partial class for Util.
    /// methods for language settings.
    /// </summary>
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
        /// <returns>get culture information</returns>
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
    /// partial class for Util.
    /// Methods to Show dialogs.
    /// </summary>
    public partial class Util
    {
        /// <summary>
        /// Show the error dialog.
        /// </summary>
        /// <param name="msg">the error message.</param>
        public static void ShowErrorDialog(string msg)
        {
            MessageBox.Show(msg, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show warining dialog.
        /// </summary>
        /// <param name="msg">the warning message.</param>
        public static void ShowWarningDialog(string msg)
        {
            MessageBox.Show(msg, "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Show notice dialog.
        /// </summary>
        /// <param name="msg">the notice message.</param>
        public static void ShowNoticeDialog(string msg)
        {
            MessageBox.Show(msg, "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Show yes or no dialog.
        /// </summary>
        /// <param name="msg">the confirm message.</param>
        /// <returns>return true if Yes is clicked.</returns>
        public static bool ShowYesNoDialog(string msg)
        {
            return MessageBox.Show(msg, "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.Yes;
        }


        /// <summary>
        /// Show yes, no or cancel dialog.
        /// </summary>
        /// <param name="msg">the confirm message.</param>
        /// <returns>yes is true, no is false and cancel throw Exception.</returns>
        public static bool ShowYesNoCancelDialog(string msg)
        {
            return ShowYesNoCancelDialog(msg, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Show yes, no or cancel dialog with the default button.
        /// </summary>
        /// <param name="msg">the confirm message.</param>
        /// <param name="defaultButton">the default selected button.</param>
        /// <returns>yes is true, no is false and cancel throw Exception.</returns>
        public static bool ShowYesNoCancelDialog(string msg, MessageBoxDefaultButton defaultButton)
        {
            switch (MessageBox.Show(msg, "Confirmation",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, defaultButton))
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
        /// Show ok or cancel button.
        /// </summary>
        /// <param name="msg">the confirm message.</param>
        /// <returns>return true if ok is clicked.</returns>
        public static bool ShowOKCancelDialog(string msg)
        {
            return MessageBox.Show(msg, "Confirmation",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                    DialogResult.OK;
        }
    }
    /// <summary>
    /// partial class for Util.
    /// Methods to control folders or files.
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

        /// <summary>
        /// Get window setting directory.
        /// </summary>
        private static string s_windowSettingDir = GetRegistryValue(Constants.registryWinSetDir);

        /// <summary>
        /// Get the temporary directory from register.
        /// </summary>
        /// <returns>the tmporary directory.</returns>
        public static string GetTmpDir()
        {
            string topDir = Path.GetTempPath() + Path.DirectorySeparatorChar + "E-Cell IDE";
            if (!Directory.Exists(topDir))
            {
                Directory.CreateDirectory(topDir);
            }
            return topDir;
        }

        /// <summary>
        /// Get the user directory.
        /// </summary>
        /// <returns>the user directory.</returns>
        public static string GetUserDir()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "E-Cell IDE");
        }

        /// <summary>
        /// Set the working directory to set directory.
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
        /// <returns>the analysis directory.</returns>
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

        /// <summary>
        /// Set the window setting directory.
        /// </summary>
        /// <param name="dir">the set directory.</param>
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
        /// Get the startup file.
        /// </summary>
        /// <returns>the startup file.</returns>
        public static string GetStartupFile()
        {
            return GetRegistryValue(Constants.registryStartup);
        }

        /// <summary>
        /// Get the DM direcory from register.
        /// </summary>
        /// <returns>the array of dm directory.</returns>
        public static string[] GetDMDirs()
        {
            return GetDMDirs(null);
        }

        /// <summary>
        /// Get the DM direcory from register.
        /// </summary>
        /// <returns>DM directory.</returns>
        public static string[] GetDMDirs(string currentProjectPath)
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
        /// Check whether SDK and Visual Studio is installed.
        /// </summary>
        /// <returns>return true if SDK is installed.</returns>
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
        /// Check whether the file is DM.
        /// </summary>
        /// <param name="fileName">the file name.</param>
        /// <returns>return true if the file is DM.</returns>
        static public bool IsDMFile(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            if (name.EndsWith(Constants.xpathProcess))
                return true;
            if (name.EndsWith(Constants.xpathStepper))
                return true;
            return false;
        }

        /// <summary>
        /// Get a list of Process Templates.
        /// </summary>
        /// <returns>the list of process templates.</returns>
        static public List<string> GetProcessTemplateList()
        {
            List<string> result = new List<string>();

            string confDir = Util.GetWindowSettingDir();
            string processDir = confDir + Constants.delimiterPath + Constants.xpathProcess;

            if (!Directory.Exists(processDir))
                return result;
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

        /// <summary>
        /// Get a Process Template.
        /// </summary>
        /// <param name="name">the template name.</param>
        /// <returns>the templete file.</returns>
        static public string GetProcessTemplate(string name)
        {
            string result = "";
            string line = "";
            string confDir = Util.GetWindowSettingDir();
            string processFile = confDir + Constants.delimiterPath + Constants.xpathProcess +
                Constants.delimiterPath + name;
            if (!File.Exists(processFile))
                return result;

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
        /// <param name="key">key.</param>
        /// <returns>the log file.</returns>
        public static string GetOutputFileName(string key)
        {
            string fileName = key.Replace("/", "_");
            fileName = fileName.Replace(":", "_");
//            fileName = fileName.Replace("__", "_");
            fileName = fileName + ".csv";
            return fileName;
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
        /// <param name="dir">the directory.</param>
        /// <returns>return true if this directory is ignore.</returns>
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
        /// Does project "projectName" already exist or not.
        /// </summary>
        /// <param name="projectName">the new project name.</param>
        /// <returns>return true if this project is exist.</returns>
        public static bool IsExistProject(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
                return false;

            string dir = Path.Combine(GetBaseDir(),projectName);
            return Directory.Exists(dir);
        }
        /// <summary>
        /// Copy File
        /// </summary>
        /// <param name="filename">the source file name.</param>
        /// <param name="targetDir">the target directory.</param>
        public static void CopyFile(string filename, string targetDir)
        {
            if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
                throw new EcellException(string.Format(MessageResources.ErrFindFile, filename));
            if (string.IsNullOrEmpty(targetDir) || !Directory.Exists(targetDir))
                throw new EcellException(string.Format(MessageResources.ErrFindDir, targetDir));
            File.Copy(filename, Path.Combine(targetDir, Path.GetFileName(filename)), true);
        }

        /// <summary>
        /// Copy Directory.
        /// </summary>
        /// <param name="sourceDir">the source directory.</param>
        /// <param name="targetDir">the target directory.</param>
        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            CopyDirectory(sourceDir, targetDir, false);
        }

        /// <summary>
        /// Copy Directory.
        /// </summary>
        /// <param name="sourceDir">the source directory.</param>
        /// <param name="targetDir">the target directory.</param>
        /// <param name="overwrite">the flag whether this file is override.</param>
        public static void CopyDirectory(string sourceDir, string targetDir, bool overwrite)
        {
            if (string.IsNullOrEmpty(sourceDir) || !Directory.Exists(sourceDir))
                throw new EcellException(string.Format(MessageResources.ErrFindDir, sourceDir));
            if (string.IsNullOrEmpty(targetDir))
                throw new EcellException(string.Format(MessageResources.ErrFindDir, targetDir));

            if (sourceDir.Equals(targetDir))
                targetDir = GetNewDir(targetDir);
            else if (Directory.Exists(targetDir) && !overwrite)
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
                File.Copy(file, file.Replace(sourceDir, targetDir), overwrite);
        }

        /// <summary>
        /// Get New Directory name.
        /// </summary>
        /// <param name="targetDir">the target directory.</param>
        /// <returns>the new directory path.</returns>
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
        /// Get new file name.
        /// </summary>
        /// <param name="targetFile">the target file.</param>
        /// <returns>the new file path.</returns>
        public static string GetNewFileName(string targetFile)
        {
            int revNo = 0;
            string newFile = "";
            do
            {
                revNo++;
                newFile = targetFile + revNo.ToString();
            } while (File.Exists(newFile));
            return newFile;
        }

        /// <summary>
        /// IsHidden
        /// </summary>
        /// <param name="dir">the target directory.</param>
        /// <returns>return true if this directory is hidden.</returns>
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
            //else if (j == 3) return Color.LightSalmon;
            //else if (j == 4) return Color.Gold;
            //else if (j == 5) return Color.LimeGreen;
            //else if (j == 6) return Color.Coral;
            //else if (j == 7) return Color.Navy;
            //else if (j == 8) return Color.Lime;
            //else if (j == 9) return Color.Purple;
            //else if (j == 10) return Color.SkyBlue;
            //else if (j == 11) return Color.Green;
            //else if (j == 12) return Color.Plum;
            //else if (j == 13) return Color.HotPink;
            //else if (j == 14) return Color.Orchid;
            //else if (j == 15) return Color.Tomato;
            //else if (j == 16) return Color.Orange;
            //else if (j == 17) return Color.Magenta;
            //else if (j == 18) return Color.Blue;
            //else if (j == 19) return Color.Red;
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
            //else if (j == 3) return Brushes.LightSalmon;
            //else if (j == 4) return Brushes.Gold;
            //else if (j == 5) return Brushes.LimeGreen;
            //else if (j == 6) return Brushes.Coral;
            //else if (j == 7) return Brushes.Navy;
            //else if (j == 8) return Brushes.Lime;
            //else if (j == 9) return Brushes.Purple;
            //else if (j == 10) return Brushes.SkyBlue;
            //else if (j == 11) return Brushes.Green;
            //else if (j == 12) return Brushes.Plum;
            //else if (j == 13) return Brushes.HotPink;
            //else if (j == 14) return Brushes.Orchid;
            //else if (j == 15) return Brushes.Tomato;
            //else if (j == 16) return Brushes.Orange;
            //else if (j == 17) return Brushes.Magenta;
            //else if (j == 18) return Brushes.Blue;
            //else if (j == 19) return Brushes.Red;
            else return Brushes.Black;
        }

        /// <summary>
        /// Get the line style from index.
        /// </summary>
        /// <param name="i">index.</param>
        /// <returns>line style.</returns>
        public static DashStyle GetLine(int i)
        {
            int j = i / 3; // GetColorBrush.
            if (j == 0) return DashStyle.Solid;
            else if (j == 1) return DashStyle.Dash;
            else if (j == 2) return DashStyle.DashDot;
            else if (j == 3) return DashStyle.Dot;
            else return DashStyle.DashDotDot;
        }

        /// <summary>
        /// Convert from ValueDataFormat to string.
        /// </summary>
        /// <param name="value">ValueDataFormat</param>
        /// <returns>the format string.</returns>
        public static string GetDisplayFormat(ValueDataFormat value)
        {
            switch (value)
            {
                case ValueDataFormat.Exponential1:
                    return "e1";
                case ValueDataFormat.Exponential2:
                    return "e2";
                case ValueDataFormat.Exponential3:
                    return "e3";
                case ValueDataFormat.Exponential4:
                    return "e4";
                case ValueDataFormat.Exponential5:
                    return "e5";
                default:
                    return "G";
            }
        }

        /// <summary>
        /// Convert Image to base64 string.
        /// </summary>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        public static string ImgToBase64(string imgFile)
        {
            string base64 = null;

            try
            {
                FileStream fst = new FileStream(imgFile, FileMode.Open, FileAccess.Read);
                byte[] buf = new byte[fst.Length];
                fst.Read(buf, 0, buf.Length);
                base64 = Convert.ToBase64String(buf);
                fst.Close();
            }
            catch (Exception e)
            {
                throw new EcellException("fail to convert image ", e);
            }
            return base64;
        }

        /// <summary>
        /// Convert base64 string to Image.
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Image Base64ToImage(string base64)
        {
            // Base64ToImage
            Image img = null;
            try
            {
                MemoryStream imgst = new MemoryStream();
                byte[] buf = System.Convert.FromBase64String(base64);
                imgst.Write(buf, 0, buf.Length);
                img = Image.FromStream(imgst);
            }
            catch (Exception e)
            {
                throw new EcellException("fail to convert image", e);
            }
            return img;
        }

        /// <summary>
        /// Cancel Exception
        /// </summary>
        public class CancelException : EcellException { }

    }
}
