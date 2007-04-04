using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.AboutWindow
{
    public partial class PluginDriver : Form
    {
        public PluginDriver()
        {
            InitializeComponent();
                        
            PluginBase pb = new AboutWindow();
            
            //Menu��������B
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();
            
            foreach(ToolStripMenuItem menuItem in menuList)
            {
                menuToolStripMenuItem.DropDownItems.Add(menuItem);
            }

            //ToolBox��������B
            List<ToolStripItem> toolList = pb.GetToolBarMenuStripItems();

            foreach(ToolStripButton button in toolList )
            {
                toolStrip1.Items.Add(button);
            }

            //UserControl��������B
            List<UserControl> windowList = pb.GetWindowsForms();
            foreach(UserControl control in windowList)
            {
                panel2.Controls.Add(control);
            }

            MessageBox.Show("plugin name: " + pb.GetPluginName());
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