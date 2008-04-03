using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EcellLib.Plugin;

namespace EcellLib.AboutWindow
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
            
            //Menuを加える。
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();
            
            foreach(ToolStripMenuItem menuItem in menuList)
            {
                menuToolStripMenuItem.DropDownItems.Add(menuItem);
            }

            //ToolBoxを加え
            List<ToolStripItem> toolList = pb.GetToolBarMenuStripItems();

            if (toolList != null)
            {
                foreach (ToolStripButton button in toolList)
                {
                    toolStrip1.Items.Add(button);
                }
            }

            MessageBox.Show("plugin name: " + pb.GetPluginName());
        }
          

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
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