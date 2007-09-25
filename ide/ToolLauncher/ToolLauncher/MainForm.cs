using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ToolLauncher
{
    public partial class MainForm : Form
    {
        private Util util = null;
        private string defaultDir = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.Initialize();
        }

        private void Initialize()
        {
            this.util = Util.GetInstance();
            this.defaultDir
                = Path.GetDirectoryName(Environment.GetEnvironmentVariable(Util.s_ironPythonDir)) + Util.s_toolPath;
            // this.defaultDir = Directory.GetCurrentDirectory() + Util.s_toolPath;
        }

        #region Convert to Dll

        private bool isFolderConvert = true;

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripMenuItemProperty_Click(object sender, EventArgs e)
        {
            Form propertyForm = new PropertyForm();
            propertyForm.ShowDialog(this);
            propertyForm.Dispose();
        }

        private void buttonConvertFolder_Click(object sender, EventArgs e)
        {
            if (this.fbdConvert.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxConvert.Text = this.fbdConvert.SelectedPath;
                this.isFolderConvert = true;
            }
        }

        private void buttonConvertFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialogConvert.ShowDialog(this) == DialogResult.OK)
            {
                this.textBoxConvert.Text = this.openFileDialogConvert.FileName;
                this.isFolderConvert = false;
            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            if (this.textBoxConvert.Text == null || this.textBoxConvert.Text.Length <= 0)
            {
                this.AddText2MessageBox(Util.s_FlagError + "The file name is \"null\".");
            }
            if (this.isFolderConvert)
            {
                if (Directory.Exists(this.textBoxConvert.Text))
                {
                    foreach (string fileName in Directory.GetFiles(this.textBoxConvert.Text, Util.s_FilePattern))
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.Convert), fileName);
                        Application.DoEvents();
                    }
                }
                else
                {
                    this.AddText2MessageBox(
                            Util.s_FlagError + "Can't find the directory named [" + this.textBoxConvert.Text + "].");
                }
            }
            else
            {
                if (File.Exists(this.textBoxConvert.Text))
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.Convert), this.textBoxConvert.Text);
                }
                else
                {
                    this.AddText2MessageBox(
                            Util.s_FlagError + "Can't find the file named [" + this.textBoxConvert.Text + "].");
                }
            }
        }

        private void Convert(object fileName)
        {
            string args
                    = Util.s_signHyphen + Util.s_defaultKeyBoost + Util.s_signSpace
                            + Util.s_signDQ + this.util.RootPathBoost + Util.s_signDQ + Util.s_signSpace
                    + Util.s_signHyphen + Util.s_defaultKeyEcell + Util.s_signSpace
                            + Util.s_signDQ + this.util.RootPathEcell + Util.s_signDQ + Util.s_signSpace
                    + Util.s_signHyphen + Util.s_defaultKeyGSL + Util.s_signSpace
                            + Util.s_signDQ + this.util.RootPathGSL + Util.s_signDQ + Util.s_signSpace
                    + Util.s_signHyphen + Util.s_defaultKeyVC + Util.s_signSpace
                            + Util.s_signDQ + this.util.RootPathVC + Util.s_signDQ + Util.s_signSpace
                    + Util.s_signHyphen + Util.s_defaultKeyCD + Util.s_signSpace
                            + Util.s_signDQ + Directory.GetCurrentDirectory() + Util.s_signDQ + Util.s_signSpace
                    + Util.s_signHyphen + Util.s_defaultKeyDebug
                    + Util.s_signSpace + Util.s_signDQ + fileName + Util.s_signDQ;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Environment.GetEnvironmentVariable("ComSpec"); // cmd.exe
            psi.RedirectStandardInput = false;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.Arguments = @"/c " + this.defaultDir + "\\" + Util.s_convertBat + Util.s_signSpace + args;
            Process p = Process.Start(psi);
            this.AddText2MessageBox(p.StandardOutput.ReadToEnd());
            p.WaitForExit();
        }

        private void AddText2MessageBox(string line)
        {
            try
            {
                this.textBoxMessage.Text += line;
                this.textBoxMessage.Text += Environment.NewLine + Environment.NewLine;
                this.textBoxMessage.SelectionStart = this.textBoxMessage.Text.Length;
                this.textBoxMessage.ScrollToCaret();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
