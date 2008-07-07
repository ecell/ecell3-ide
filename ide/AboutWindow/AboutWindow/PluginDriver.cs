using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ecell.Plugin;

namespace Ecell.IDE.Plugins.AboutWindow
{
    /// <summary>
    /// Test Driver Program.
    /// </summary>
    public partial class PluginDriver : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PluginDriver()
        {
            InitializeComponent();

            IEcellPlugin pb = new AboutWindow();
            
            //Menu��������B
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();
            
            foreach(ToolStripMenuItem menuItem in menuList)
            {
                menuToolStripMenuItem.DropDownItems.Add(menuItem);
            }

            //ToolBox������
            ToolStrip toolList = pb.GetToolBarMenuStrip();
            Util.ShowNoticeDialog("plugin name: " + pb.GetPluginName());
        }
          

        /// <summary>
        /// �A�v���P�[�V�����̃��C�� �G���g�� �|�C���g�ł��B
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PluginDriver());
        }
    }
}