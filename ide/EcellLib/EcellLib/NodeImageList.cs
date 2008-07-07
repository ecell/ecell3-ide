using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ecell
{
    /// <summary>
    /// NodeImageComponent
    /// </summary>
    public partial class NodeImageComponent : Component
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NodeImageComponent()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public NodeImageComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        /// <summary>
        /// ImageList of TreeNode 
        /// </summary>
        public ImageList ImageList
        {
            get { return imageList1; }
        }
    }
}
