using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib.EntityListWindow
{
    /// <summary>
    /// Test Driver Program.
    /// </summary>
    public partial class PluginDriver : Form
    {
        PluginBase pb = null;
        private PrintDocument printDocument1 = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PluginDriver()
        {
            InitializeComponent();

            pb = new EntityListWindow();
            List<ToolStripMenuItem> menuList = pb.GetMenuStripItems();

            if (menuList != null)
            {
                foreach (ToolStripMenuItem menu in menuList)
                {
                    this.mainstrip.Items.AddRange(new ToolStripItem[] {
                        menu });
                }
            }

            List<UserControl> windowList = pb.GetWindowsForms();
            if (windowList != null)
            {
                foreach (UserControl control in windowList)
                {
                    control.Anchor =
                     (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
                    this.splitContainer1.Panel1.Controls.Add(control);
                }
                pb.SetPanel(this.splitContainer1.Panel1);
            }

            printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
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
            e.Graphics.DrawImage(bitmap, new Point(0, 0));
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
    }
}
