using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EcellLib
{
    public partial class AddProperyDialog : Form
    {
        private String m_resultName = null;

        public AddProperyDialog()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddPropertyApplyButton_Click(object sender, EventArgs e)
        {
            m_resultName = PropertyTextBox.Text;
            this.Close();
        }

        public String ShowPropertyDialog()
        {
            this.ShowDialog();
            return m_resultName;
        }
    }
}