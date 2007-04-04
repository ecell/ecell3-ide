using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace InstallSupport
{
    [RunInstaller(true)]
    public class ModifyRegistory : Installer
    {
        /// <summary>
        /// The key of the "Path"
        /// </summary>
        private const string s_registryKeyEnvironment = "Path";
        /// <summary>
        /// The key of the "Environment"
        /// </summary>
        private const string s_registryPathEnvironment
                = "SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment";
        /// <summary>
        /// The delimiter of the path
        /// </summary>
        private const string s_delimKey = ";";
        /// <summary>
        /// The added path
        /// </summary>
        private const string s_addedPathKey = "addedPath";
        /// <summary>
        /// The cleaned path
        /// </summary>
        private const string s_cleanedPathKey = "cleanedPath";

        /// <summary>
        /// Adds the path to the "Path" environment.
        /// </summary>
        /// <param name="l_addPath">The added path</param>
        private void AddRegistoryEnvironmentPath(string l_addPath)
        {
            // MessageBox.Show(l_addPath);
            RegistryKey l_regkey = Registry.LocalMachine.CreateSubKey(s_registryPathEnvironment);
            string l_regValue = Convert.ToString(l_regkey.GetValue(s_registryKeyEnvironment));
            string l_value = l_regValue;
            bool l_flag = false;
            if (l_regValue.IndexOf(s_delimKey) < 0)
            {
                if (l_regValue.Equals(l_addPath))
                {
                    l_flag = true;
                }
            }
            else
            {
                foreach (string l_path in l_regValue.Split(s_delimKey.ToCharArray()))
                {
                    if (l_path.Equals(l_addPath))
                    {
                        l_flag = true;
                        break;
                    }
                }
            }
            if (!l_flag)
            {
                if (l_regValue.LastIndexOf(s_delimKey) != (l_regValue.Length - 1))
                {
                    l_value = l_value + s_delimKey;
                }
                l_value = l_value + l_addPath;
                l_regkey.SetValue(s_registryKeyEnvironment, l_value);
            }
            l_regkey.Close();
        }

        /// <summary>
        /// Cleans the "Path" environment.
        /// </summary>
        /// <param name="l_cleanPath"></param>
        private void CleanRegistoryEnvironmentPath(string l_cleanPath)
        {
            // MessageBox.Show(l_cleanPath);
            RegistryKey l_regkey = Registry.LocalMachine.CreateSubKey(s_registryPathEnvironment);
            string l_regValue = Convert.ToString(l_regkey.GetValue(s_registryKeyEnvironment));
            string l_value = null;
            if (l_regValue.IndexOf(s_delimKey) < 0)
            {
                if (!l_regValue.Equals(l_cleanPath))
                {
                    l_value = l_regValue;
                }
            }
            else
            {
                foreach (string l_path in l_regValue.Split(s_delimKey.ToCharArray()))
                {
                    if (!l_path.Equals(l_cleanPath) && l_path.Length > 0)
                    {
                        l_value += l_path + s_delimKey;
                    }
                }
            }
            l_regkey.SetValue(s_registryKeyEnvironment, l_value);
            l_regkey.Close();
        }

        /// <summary>
        /// Executes when the S/W is installed.
        /// </summary>
        /// <param name="stateSaver">The "IDictionary" 4 the commit, the rollback and the uninstallation</param>
        public override void Install(IDictionary stateSaver)
        {
            try
            {
                base.Install(stateSaver);
                string addedPath = this.Context.Parameters[s_addedPathKey];
                if (addedPath.IndexOf(s_delimKey) < 0)
                {
                    this.AddRegistoryEnvironmentPath(addedPath);
                }
                else
                {
                    string[] addedPaths = addedPath.Split(s_delimKey.ToCharArray());
                    for (int i = 0; i < addedPaths.Length; i++)
                    {
                        this.AddRegistoryEnvironmentPath(addedPaths[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InstallException(ex.ToString());
            }
        }

        /// <summary>
        /// Executes when the S/W is uninstalled.
        /// </summary>
        /// <param name="savedState">The "IDictionary" 4 the installation</param>
        public override void Uninstall(IDictionary savedState)
        {
            try
            {
                base.Uninstall(savedState);
                string cleanedPath = this.Context.Parameters[s_cleanedPathKey];
                if (cleanedPath.IndexOf(s_delimKey) < 0)
                {
                    this.CleanRegistoryEnvironmentPath(cleanedPath);
                }
                else
                {
                    string[] cleanedPaths = cleanedPath.Split(s_delimKey.ToCharArray());
                    for (int i = 0; i < cleanedPaths.Length; i++)
                    {
                        this.CleanRegistoryEnvironmentPath(cleanedPaths[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InstallException(ex.ToString());
            }
        }
    }
}
