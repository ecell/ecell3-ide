using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.PropertyWindow
{
    public partial class PluginDriver : Form
    {
        private PrintDocument printDocument1 = null;

        private PluginBase pb = null;

        public PluginDriver()
        {
            
            InitializeComponent();
            
            pb = new PropertyWindow();
            
            // Menuを加える。
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();
            
            foreach(ToolStripMenuItem menuItem in menuList)
            {
                menuToolStripMenuItem.DropDownItems.Add(menuItem);
            }

            // ToolBoxを加える。
            List<ToolStripItem> toolList = pb.GetToolBarMenuStripItems();

            foreach(ToolStripItem button in toolList )
            {
                toolStrip1.Items.Add(button);
            }

            // UserControlを加える。
            List<UserControl> windowList = pb.GetWindowsForms();
            foreach(UserControl control in windowList)
            {
                panel2.Controls.Add(control);
            }

            // プリント機能の設定をする。
            printDocument1 = new System.Drawing.Printing.PrintDocument();

            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bitmap = pb.Print();

            // 印刷ページの描画を行う
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

        }

        private void selectInstance1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pb.SelectChanged("gen","/Japan/Osaka/Takatsuki", "System");
        }

        private void selectInstance2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pb.SelectChanged("gen", "/Japan/Osaka/Umeda", "System");
        }

        private void selectNullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pb.Clear();
        }

    }
}