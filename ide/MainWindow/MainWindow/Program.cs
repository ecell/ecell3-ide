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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Resources;
using System.Threading;
using System.Reflection;

namespace EcellLib.MainWindow
{
    class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            String lang = Util.GetLang();
            if (lang != null && (lang.ToUpper() == "DEFAULT" || lang.ToUpper() == "EN_US"))
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-us", true);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Splash frmSplash = new Splash();
            ApplicationContext me = new ApplicationContext();

            EventHandler onIdle = null;
            onIdle = delegate(object sender, EventArgs e)
            {
                Application.Idle -= onIdle;
                MainWindow frmMainWnd = new MainWindow();

                me.MainForm = frmMainWnd;
                frmMainWnd.Show();
                frmSplash.Close();
            };

            frmSplash.Show();
            Application.Idle += onIdle;
            Application.Run(me);
        }
    }
}