using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.Simulation
{
    public partial class PluginDriver : Form
    {
        PluginBase pb = null;

        public PluginDriver()
        {
            InitializeComponent();

            pb = new Simulation();
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();

            foreach (ToolStripMenuItem menu in menuList)
            {
                this.mainstrip.Items.AddRange(new ToolStripItem[] {
                menu});
            }

            List<ToolStripItem> toolList = pb.GetToolBarMenuStripItems();
            foreach (ToolStripItem tool in toolList)
            {
                this.toolstrip.Items.AddRange(new ToolStripItem[] {
                    tool});
            }

        }

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PluginDriver());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

    }
}