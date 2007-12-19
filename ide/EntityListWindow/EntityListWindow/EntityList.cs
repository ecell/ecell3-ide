using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace EcellLib
{
    public partial class EntityList : EcellDockContent
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityList()
        {
            base.m_isSavable = true;
            InitializeComponent();
            
        }

    }
}
