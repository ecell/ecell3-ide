//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Environment Application package
//
//                Copyright (C) 1996-2008 Keio University
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Ecell.Reporting;
using Ecell.Logging;

namespace Ecell
{
    /// <summary>
    /// Class to compile the source of DM.
    /// </summary>
    public class DMCompiler
    {
        #region Fields
        /// <summary>
        /// source file path.
        /// </summary>
        private string m_sourceFile;
        /// <summary>
        /// output file path.
        /// </summary>
        private string m_outputFile;
        /// <summary>
        /// DM file path.
        /// </summary>
        private string m_dmFile;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public DMCompiler()
        {
            m_sourceFile = null;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// get / set the source of DM.
        /// </summary>
        public string SourceFile
        {
            set { this.m_sourceFile = value; }
        }

        /// <summary>
        /// get / set the output file.
        /// </summary>
        public string OutputFile
        {
            get { return this.m_outputFile; }
            set { this.m_outputFile = value; }
        }
        /// <summary>
        /// get / set the dm file.
        /// </summary>
        public string DMFile
        {
            get { return this.m_dmFile; }
            set { this.m_dmFile = value; }
        }
        #endregion

        /// <summary>
        /// Compile the source of DM.
        /// </summary>
        /// <param name="env">Application Environmant object.</param>
        public void Compile(ApplicationEnvironment env)
        {
            // Check error.
            if (m_sourceFile == null || !File.Exists(m_sourceFile))
                return;
            string stageHome = System.Environment.GetEnvironmentVariable("ECELL_STAGING_HOME");
            if (string.IsNullOrEmpty(stageHome))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNotInstall,
                    new object[] { "E-Cell SDK" }));
                return;
            }
            // Set up compile environment.
            string VS80 = System.Environment.GetEnvironmentVariable("VS80COMNTOOLS");
            if (string.IsNullOrEmpty(VS80))
            {
                Util.ShowErrorDialog(string.Format(MessageResources.ErrNotInstall,
                    new object[] { "Visual Studio" }));
                return;
            }
            string groupname = Constants.groupCompile + ":" + m_sourceFile;
            int maxCount = 10;
            int count = 0;
            ReportingSession rs = null;
            while (rs == null)
            {
                try
                {
                    rs = env.ReportManager.GetReportingSession(groupname);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(100);
                    if (maxCount < count)
                    {
                        Util.ShowErrorDialog(string.Format(MessageResources.ErrCompile, new object[] { m_sourceFile }));
                        return;
                    }
                    count++;
                }
            }

            // Compile
            using (rs)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "cmd.exe";
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WorkingDirectory = Path.GetDirectoryName(m_sourceFile);
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardInput = true;

                string arch1 = "Win32";
                string arch2 = "X86";
                string arch3 = "x86";
                if (IntPtr.Size == 8)
                {
                    arch1 = "X64";
                    arch2 = "X64";
                    arch3 = "amd64";
                }

                Process p = Process.Start(psi);
                p.StandardInput.WriteLine("call \"" + VS80 + "..\\..\\VC\\vcvarsall.bat\" " + arch3);

                string opt = "cl.exe /O2 /GL /I \"{0}\\{3}\\Release\\include\" /I \"{0}\\{3}\\Release\\include\\ecell-3.2\" /I \"{0}\\{3}\\Release\\include\\ecell-3.2\\libecs\" /I \"{0}\\{3}\\Release\\include\\ecell-3.2\\libemc\" /D \"WIN32\" /D\"NODEBUG\" /D \"_WINDOWS\" /D \"_USRDLL\" /D \"GSL_DLL\" /D \"__STDC__=1\" /D \"_WINDLL\" /D \"_WIN32_WINNT=0x500\" /D \"_SECURE_SCL=0\" /D \"_MBCS\" /FD /EHsc /MD /W3 /nologo /Wp64 /Zi /TP /errorReport:prompt \"{1}\" /link /OUT:\"{2}\" /LIBPATH:\"{0}\\{3}\\Release\\lib\" /INCREMENTAL:NO /NOLOGO  /DLL /MANIFEST /MANIFESTFILE:\"{2}.intermediate.manifest \" /DEBUG /SUBSYSTEM:WINDOWS /OPT:REF /OPT:ICF /LTCG /MACHINE:{4} ecs.lib  kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib";
                string cmd = string.Format(opt, new object[] {
                    stageHome, m_sourceFile, m_outputFile, arch1, arch2
                    });

                p.StandardInput.WriteLine(cmd);
                p.StandardInput.WriteLine("exit");
                p.StandardInput.Close();

                string mes = p.StandardOutput.ReadToEnd();
                env.Console.Write(mes);
                if (mes.Contains(" error"))
                {
                    string[] ele = mes.Split(new char[] { '\n' });
                    for (int i = 0; i < ele.Length; i++)
                    {
                        if (ele[i].Contains(" error"))
                        {
                            rs.Add(new CompileReport(MessageType.Error, ele[i], groupname));
                            env.LogManager.Append(new ApplicationLogEntry(MessageType.Error, ele[i], this));
                        }
                    }

                    string errmes = string.Format(MessageResources.ErrCompile, new object[] { m_sourceFile });
                    Util.ShowErrorDialog(errmes);
                    p.StandardOutput.Close();
                    p.WaitForExit();
                    p.Close();
                    return;
                }

                p.StandardOutput.Close();
                p.WaitForExit();
                p.Close();

                p = Process.Start(psi);
                p.StandardInput.WriteLine("call \"" + VS80 + "\\vsvars32.bat\"");

                string mopt = "mt.exe /outputresource:\"{0};#2\" /manifest \"{0}.intermediate.manifest\" /nologo";
                cmd = string.Format(mopt, m_outputFile);
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.WriteLine("exit");
                p.StandardInput.Close();


                mes = p.StandardOutput.ReadToEnd();
                env.Console.WriteLine(mes);
                Console.WriteLine(mes);

                // error
                if (mes.Contains(" error"))
                {
                    string[] ele = mes.Split(new char[] { '\n' });
                    for (int i = 0; i < ele.Length; i++)
                    {
                        if (ele[i].Contains(" error"))
                        {
                            rs.Add(new CompileReport(MessageType.Error, ele[i], groupname));
                            env.LogManager.Append(new ApplicationLogEntry(MessageType.Error, ele[i], this));
                        }
                    }
                    string errmes = string.Format(MessageResources.ErrCompile, new object[] { m_sourceFile });
                    Util.ShowErrorDialog(errmes);
                    p.StandardOutput.Close();
                    p.WaitForExit();
                    p.Close();
                    return;
                }

                p.StandardOutput.Close();
                p.WaitForExit();
                p.Close();

                // Reload DMs.
                try
                {
                    env.DataManager.CurrentProject.UnloadSimulator();
                    File.Move(OutputFile, DMFile);
                    Util.ShowNoticeDialog(string.Format(MessageResources.InfoCompile, 
                        Path.GetFileNameWithoutExtension(DMFile)));
                    env.Console.WriteLine(string.Format(MessageResources.InfoCompile,
                        Path.GetFileNameWithoutExtension(DMFile)));
                    env.Console.Flush();
                    env.DataManager.CurrentProject.ReloadSimulator();
                }
                catch (Exception)
                {
                    // 移動先のDMがロードされているため移動できなかった。
                    // よってこの例外は無視するものとする。
                    Util.ShowNoticeDialog(string.Format(MessageResources.WarnMoveDM,
                        DMFile, OutputFile));
                    env.Console.WriteLine(string.Format(MessageResources.WarnMoveDM,
                        DMFile, OutputFile));
                    env.Console.Flush();
                }
            }
        
        }

        /// <summary>
        /// Comile the source of DM.
        /// </summary>
        /// <param name="fileName">the file name of source.</param>
        /// <param name="env">Application Environment object.</param>
        public static void Compile(string fileName, ApplicationEnvironment env)
        {
            if (fileName == null || !File.Exists(fileName))
                return;
            DMCompiler cm = new DMCompiler();
            cm.SourceFile = fileName;
            string outdir = Path.Combine(Path.GetDirectoryName(fileName), Constants.TmpDirName);
            if (!Directory.Exists(outdir))
                Directory.CreateDirectory(outdir);
            string outfile = Path.Combine(outdir, Path.GetFileNameWithoutExtension(fileName) + Constants.FileExtDM);
            string dmfile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + Constants.FileExtDM);
            cm.OutputFile = outfile;
            cm.DMFile = dmfile;

            cm.Compile(env);            
        }
    }
}
