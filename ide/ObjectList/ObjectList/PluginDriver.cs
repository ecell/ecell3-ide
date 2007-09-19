using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

using EcellLib;

namespace EcellLib.ObjectList
{
    /// <summary>
    /// Driver class to test this plugin.
    /// </summary>
    public partial class PluginDriver : Form
    {
        private PrintDocument printDocument1 = null;

        private PluginBase pb = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PluginDriver()
        {
            InitializeComponent();

            pb = new ObjectList();
            
            // Menu��������B
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();
            if (menuList != null)
            {
                foreach (ToolStripMenuItem menuItem in menuList)
                {
                    menuToolStripMenuItem.DropDownItems.Add(menuItem);
                }
            }

            // ToolBox��������B
            List<ToolStripItem> toolList = pb.GetToolBarMenuStripItems();
            if (toolList != null)
            {
                foreach (ToolStripItem button in toolList)
                {
                    toolStrip1.Items.Add(button);
                }
            }

            // UserControl��������B
            List<DockContent> windowList = pb.GetWindowsForms();
            foreach (DockContent win in windowList)
            {
                foreach(Control con in win.Controls)
                    panel2.Controls.Add(con);
            }

            // �v�����g�@�\�̐ݒ������B
            printDocument1 = new System.Drawing.Printing.PrintDocument();

            //this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            
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

        private void ecellObjectBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void �R�s�[CToolStripButton_Click(object sender, EventArgs e)
        {

        }
        /*
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bitmap = pb.Print();

            // ����y�[�W�̕`����s��
            e.Graphics.DrawImage(bitmap, new Point(0, 0));

            bitmap.Dispose();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;

            DialogResult result = printDialog1.ShowDialog();

            // If the result is OK then print the document.
            if (result == DialogResult.OK)
            {
                printDocument1.Print();
            }

        }*/

    }
}