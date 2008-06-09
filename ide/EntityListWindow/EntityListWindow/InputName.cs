using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace EcellLib.EntityListWindow
{
    /// <summary>
    /// Form to input the new DM name.
    /// </summary>
    public partial class InputName : Form
    {
        #region Fields
        /// <summary>
        /// The path of dm directory for the current project.
        /// </summary>
        private string m_dir;
        /// <summary>
        /// The current selected node.
        /// </summary>
        private TreeNode m_node;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="dmDir">The path of dm directory.</param>
        /// <param name="node">The current selected node.</param>
        public InputName(string dmDir, TreeNode node)
        {
            InitializeComponent();
            m_dir = dmDir;
            m_node = node;
        }
        #endregion

        #region Events
        /// <summary>
        /// The event sequence when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void INCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The event sequence when the create button is clicked.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">EventArgs.</param>
        private void INNewButton_Click(object sender, EventArgs e)
        {
            String name = INTextBox.Text;
            if (String.IsNullOrEmpty(name)) return;
            if (!name.EndsWith(Constants.xpathProcess) && !name.EndsWith(Constants.xpathStepper))
            {
                Util.ShowWarningDialog(MessageResEntList.WarnDMName);
                return;
            }

            try
            {
                string filename = Path.Combine(m_dir, name);
                filename = filename + Constants.FileExtSource;
                File.Create(filename);

                TreeNode dNode = new TreeNode(name);
                dNode.Tag = new TagData("", "", Constants.xpathDM);
                m_node.Nodes.Add(dNode);

                this.Close();
            }
            catch (Exception)
            {
                Util.ShowErrorDialog(string.Format(MessageResEntList.ErrCreateFile,
                    new object[] { name }));                
            }
        }
        #endregion
    }
}