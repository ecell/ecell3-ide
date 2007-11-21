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
    /// <summary>
    /// ToolLauncher Main Form.
    /// </summary>
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
                = Path.GetDirectoryName(Environment.GetEnvironmentVariable(Constants.ironPythonDir)) + Constants.toolPath;
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
                this.AddText2MessageBox(Constants.FlagError + "The file name is \"null\".");
            }
            if (this.isFolderConvert)
            {
                if (Directory.Exists(this.textBoxConvert.Text))
                {
                    foreach (string fileName in Directory.GetFiles(this.textBoxConvert.Text, Constants.FilePattern))
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.Convert), fileName);
                        Application.DoEvents();
                    }
                }
                else
                {
                    this.AddText2MessageBox(
                            Constants.FlagError + "Can't find the directory named [" + this.textBoxConvert.Text + "].");
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
                            Constants.FlagError + "Can't find the file named [" + this.textBoxConvert.Text + "].");
                }
            }
        }

        private void Convert(object fileName)
        {
            string args
                    = Constants.signHyphen + Constants.defaultKeyBoost + Constants.signSpace
                            + Constants.signDQ + this.util.RootPathBoost + Constants.signDQ + Constants.signSpace
                    + Constants.signHyphen + Constants.defaultKeyEcell + Constants.signSpace
                            + Constants.signDQ + this.util.RootPathEcell + Constants.signDQ + Constants.signSpace
                    + Constants.signHyphen + Constants.defaultKeyGSL + Constants.signSpace
                            + Constants.signDQ + this.util.RootPathGSL + Constants.signDQ + Constants.signSpace
                    + Constants.signHyphen + Constants.defaultKeyVC + Constants.signSpace
                            + Constants.signDQ + this.util.RootPathVC + Constants.signDQ + Constants.signSpace
                    + Constants.signHyphen + Constants.defaultKeyCD + Constants.signSpace
                            + Constants.signDQ + Directory.GetCurrentDirectory() + Constants.signDQ + Constants.signSpace
                    + Constants.signHyphen + Constants.defaultKeyDebug
                    + Constants.signSpace + Constants.signDQ + fileName + Constants.signDQ;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Environment.GetEnvironmentVariable("ComSpec"); // cmd.exe
            psi.RedirectStandardInput = false;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.Arguments = @"/c " + this.defaultDir + "\\" + Constants.convertBat + Constants.signSpace + args;
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
