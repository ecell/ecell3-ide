using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ecell.IDE.Plugins.ProjectExplorer
{
    public partial class InputNameDialog : Form
    {
        private ProjectExplorer m_owner;

        public InputNameDialog()
        {
            InitializeComponent();
        }

        public ProjectExplorer Owner
        {
            get { return this.m_owner; }
            set { this.m_owner = value; }
        }

        public String InputText
        {
            get { return NameTextBox.Text; }
        }

        private void ShownForm(object sender, EventArgs e)
        {
            NameTextBox.Focus();
        }

        private void ClosingInputDialogForm(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
                return;

            if (String.IsNullOrEmpty(NameTextBox.Text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, MessageResources.NameName));
                e.Cancel = true;
                return;
            }

            if (m_owner != null)
            {
                List<string> data = m_owner.DataManager.GetSimulationParameterIDs();
                if (data.Contains(NameTextBox.Text))
                {
                    Util.ShowErrorDialog(MessageResources.ErrAlreadyExist);
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}