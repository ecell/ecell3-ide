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
// written by Sachio Nohara <nohara@cbo.mss.co.jp>,
// MITSUBISHI SPACE SOFTWARE CO.,LTD.
//

using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Resources;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

using Ecell.Logging;
using Ecell.Plugin;

namespace Ecell.IDE
{
    class Program
    {
        static bool s_noSplash = false;
        enum OptionKind
        {
            PluginDirectory
        };

        [DllImport("kernel32")]
        static extern bool AllocConsole();

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string[] fileList = parseArguments(args);

            Util.InitialLanguage();

            ApplicationEnvironment env = new ApplicationEnvironment();

            Splash frmSplash = new Splash(env);

            if (!s_noSplash)
                frmSplash.Show();
            MainWindow.MainWindow window = null;
            EventHandler onIdle = null;
            onIdle = delegate(object sender, EventArgs ev)
            {
                Application.Idle -= onIdle;
                IEcellPlugin mainWnd = env.PluginManager.RegisterPlugin(
                    typeof(Ecell.IDE.MainWindow.MainWindow));
                window = (MainWindow.MainWindow)mainWnd;
                env.PluginManager.ChangeStatus(ProjectStatus.Uninitialized);
                ((Form)mainWnd).Show();

                if (!s_noSplash)
                    frmSplash.Close();

                foreach (string fPath in fileList)
                {
                    if (fPath.EndsWith(Constants.FileExtEML) || fPath.EndsWith(Constants.fileProjectXML))
                    {
                        env.DataManager.LoadProject(fPath);
                    }
                    else
                    {
                        //window.LoadUserActionFile(fPath);
                    }
                }
            };
            Application.Idle += onIdle;
            Application.Run(window);
        }

        /// <summary>
        /// Parses the argument list, configures the application and
        /// returns the non-parameter portion of it.
        /// </summary>
        /// <param name="args">list of arguments passed to Main() function.</param>
        /// <returns></returns>
        private static string[] parseArguments(string[] args)
        {
            List<string> nonParamArgs = new List<string>();
            foreach (string arg in args)
            {
                if (arg[0] == '/')
                {
                    if (arg.StartsWith("/PLUGINDIR:"))
                    {
                        Util.AddPluginDir(Path.GetFullPath(arg.Substring("/PLUGINDIR:".Length)));
                    }
                    else if (arg.StartsWith("/DMDIR:"))
                    {
                        Util.AddDMDir(Path.GetFullPath(arg.Substring("/DMDIR:".Length)));
                    }
                    else if (arg.StartsWith("/WINDOWSETTINGSDIR:"))
                    {
                        Util.SetWindowSettingDir(Path.GetFullPath(arg.Substring("/WINDOWSETTINGSDIR:".Length)));
                    }
                    else if (arg == "/NODEFAULTS")
                    {
                        Util.OmitDefaultPaths();
                    }
                    else if (arg == "/NOSPLASH")
                    {
                        s_noSplash = true;
                    }
                    else if (arg == "/TRACE")
                    {
                        AllocConsole();
                        Trace.Listeners.Add(new ConsoleTraceListener());
                    }
                }
                else
                {
                    nonParamArgs.Add(arg);
                }
            }
            return nonParamArgs.ToArray();
        }

    }
}