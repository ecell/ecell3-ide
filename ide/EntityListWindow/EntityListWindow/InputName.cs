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
        private string m_dir;
        private TreeNode m_node;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with the initial parameters.
        /// </summary>
        /// <param name="dmDir"></param>
        /// <param name="node"></param>
        public InputName(string dmDir, TreeNode node)
        {
            InitializeComponent();
            m_dir = dmDir;
            m_node = node;
        }
        #endregion

        #region Events
        private void INCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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