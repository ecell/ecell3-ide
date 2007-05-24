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
using System.Collections.Generic;
using System.Text;
using System.Security.AccessControl;

namespace EcellLib
{
    public class Util
    {
        public const string s_defaultDMPath = "dm";
        public const string s_dmFileExtension = ".dll";
        public const string s_delimiterColon = ":";
        public const string s_delimiterEqual = "=";
        public const string s_delimiterHyphen = "-";
        public const string s_delimiterPath = "/";
        public const string s_delimiterPeriod = ".";
        public const string s_delimiterSemiColon = ";";
        public const string s_delimiterSharp = "#";
        public const string s_delimiterSpace = " ";
        public const string s_delimiterTab = "\t";
        public const string s_delimiterUnderbar = "_";
        public const string s_delimiterWildcard = "*";
        public const string s_fileProject = "project.info";
        public const string s_headerAverage = "avg";
        public const string s_headerColumn = "5";
        public const string s_headerData = "DATA";
        public const string s_headerLabel = "LABEL";
        public const string s_headerMaximum = "Max";
        public const string s_headerMinimum = "Min";
        public const string s_headerNote = "NOTE";
        public const string s_headerSize = "SIZE";
        public const string s_headerTime = "t";
        public const string s_headerTolerable = "Tolerable";
        public const string s_headerValue = "value";
        public const string s_parameterKey = "DefaultParameter";
        public const string s_registryAnalysisDirKey = "E-CELL IDE ANALYSIS";
        public const string s_registryBaseDirKey = "E-CELL IDE BASE";
        public const string s_registryDMDirKey = "E-CELL IDE DM";
        public const string s_registryPluginDirKey = "E-CELL IDE PLUGIN";
        public const string s_registryStaticDebugDirKey = "E-CELL IDE STATICDEBUG PLUGIN";
        public const string s_registryTmpDirKey = "E-CELL IDE TMP";
        public const string s_registryEnvKey = "Environment";
        public const string s_registrySWKey = "software\\KeioUniv\\E-Cell IDE";
        public const string s_registrySW2Key = "software\\KeioUniv";
        public const string s_textComment = "Commnet";
        public const string s_textKey = "DefaultStepper";
        public const string s_textParameter = "SimulationParameter";
        public const string s_typeDouble = "double";
        public const string s_typeInt = "int";
        public const string s_typeList = "list";
        public const string s_typeString = "string";
        public const string s_xpathAction = "Action";
        public const string s_xpathActivity = "Activity";
        public const string s_xpathClass = "class";
        public const string s_xpathClassName = "ClassName";
        public const string s_xpathEcd = "ecd";
        public const string s_xpathEml = "eml";
        public const string s_xpathExpression = "Expression";
        public const string s_xpathFireMethod = "FireMethod";
        public const string s_xpathFixed = "Fixed";
        public const string s_xpathID = "ID";
        public const string s_xpathInitialCondition = "InitialCondition";
        public const string s_xpathInterval = "Interval";
        public const string s_xpathIsEpsilonChecked = "IsEpsilonChecked";
        public const string s_xpathKey = "Key";
        public const string s_xpathLoggerPolicy = "LoggerPolicy";
        public const string s_xpathModel = "Model";
        public const string s_xpathMolarConc = "MolarConc";
        public const string s_xpathName = "Name";
        public const string s_xpathNumberConc = "NumberConc";
        public const string s_xpathPrm = "Prm";
        public const string s_xpathProcess = "Process";
        public const string s_xpathProject = "Project";
        public const string s_xpathProperty = "property";
        public const string s_xpathResult = "Result";
        public const string s_xpathSize = "Size";
        public const string s_xpathSimulation = "Simulation";
        public const string s_xpathSpace = "Space";
        public const string s_xpathStep = "Step";
        public const string s_xpathStepInterval = "StepInterval";
        public const string s_xpathStepper = "Stepper";
        public const string s_xpathSystem = "System";
        public const string s_xpathValue = "Value";
        public const string s_xpathVariable = "Variable";
        public const string s_xpathVRL = "VariableReferenceList";
        public const string s_xpathXml = "xml";
        //
        public const int NOTLOAD = 0;
        public const int LOADED = 1;
        public const int RUNNING = 2;
        public const int SUSPEND = 3;
        public const int STEP = 4;

        public const int ANALYSIS_YET = 0;
        public const int ANALYSIS_DO = 1;
        public const int ANALYSIS_DONE = 2;

        static public string GetAnalysisDir()
        {
            return GetRegistryValue(s_registryAnalysisDirKey);
        }

        static public string GetBaseDir()
        {
            return GetRegistryValue(s_registryBaseDirKey);
        }

        static public string GetDMDir()
        {
            return GetRegistryValue(s_registryDMDirKey);
        }

        static public string GetPluginDir()
        {
            return GetRegistryValue(s_registryPluginDirKey);
        }

        static public bool IsNG(string l_key)
        {
            for (int i = 0; i < l_key.Length; i++)
            {
                if (Char.IsLetterOrDigit(l_key[i]) ||
                    l_key[i] == '_') continue;
                return true;
            }
            return false;
        }

        static public string GetTmpDir()
        {
            return GetRegistryValue(s_registryTmpDirKey);
        }

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
