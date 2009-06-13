using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ecell.Objects;

namespace Ecell.IDE.Plugins.ProjectExplorer
{
    /// <summary>
    /// Input name dialog.
    /// </summary>
    public partial class InputNameDialog : Form
    {
        #region Fields
        /// <summary>
        /// ProjectExplore object.
        /// </summary>
        private ProjectExplorer m_owner;
        /// <summary>
        /// The flag whether this dialog is stepper.
        /// </summary>
        private bool m_isStepper = false;
        #endregion

        /// <summary>
        /// Constructors.
        /// </summary>
        public InputNameDialog(bool isStepper)
        {
            InitializeComponent();
            m_isStepper = isStepper;
            if (m_isStepper)
                this.Text = MessageResources.NameNewStepper;
        }

        #region Accessors
        /// <summary>
        /// get / set owner object.
        /// </summary>
        public ProjectExplorer OwnerForm
        {
            get { return this.m_owner; }
            set { this.m_owner = value; }
        }
        /// <summary>
        /// get / set the input text.
        /// </summary>
        public String InputText
        {
            get { return NameTextBox.Text; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Show event for this dialog.
        /// </summary>
        /// <param name="sender">InputNameDialog.</param>
        /// <param name="e">EventArgs.</param>
        private void ShownForm(object sender, EventArgs e)
        {
            NameTextBox.Focus();
        }
        /// <summary>
        /// Close event for this dialog.
        /// </summary>
        /// <param name="sender">InputNameDialog.</param>
        /// <param name="e">FormClosingEventArgs.</param>
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
                if (m_isStepper)
                {
                    string name = NameTextBox.Text;
                    string modelID = m_owner.DataManager.CurrentProject.Model.ModelID;
                    List<EcellObject> list = m_owner.DataManager.GetStepper(modelID);
                    foreach (EcellObject obj in list)
                    {
                        if (obj.Key.Equals(name))
                        {
                            Util.ShowErrorDialog(MessageResources.ErrAlreadyExistStepper);
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else
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
        /// <summary>
        /// Validating text box.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">CancelEventArgs</param>
        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {

        }
        #endregion
    }
}