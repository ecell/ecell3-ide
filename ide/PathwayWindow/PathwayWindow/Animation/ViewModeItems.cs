using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ecell.IDE.Plugins.PathwayWindow.Dialog;

namespace Ecell.IDE.Plugins.PathwayWindow.Animation
{
    /// <summary>
    /// private class for AnimationSettingDialog
    /// </summary>
    internal class ViewModeItems : UserControl
    {
        private GroupBox groupBox;
        private PropertyBrushItem bgBrush;
        private PropertyBrushItem edgeBrush;
        private PropertyTextItem edgeWidth;

        private AnimationControl m_control;

        /// <summary>
        /// 
        /// </summary>
        public ViewModeItems()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public ViewModeItems(AnimationControl control)
        {
            m_control = control;
            InitializeComponent();

            //MessageResources.DialogTextBackgroundBrush
            //MessageResources.DialogTextEdgeBrush
            //MessageResources.DialogTextMaxEdgeWidth
            bgBrush.Brush = control.ViewBGBrush;
            edgeBrush.Brush = control.ViewEdgeBrush;
            edgeWidth.Text = control.MaxEdgeWidth.ToString();
        }

        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewModeItems));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.bgBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.edgeBrush = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyBrushItem();
            this.edgeWidth = new Ecell.IDE.Plugins.PathwayWindow.Dialog.PropertyTextItem();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.AccessibleDescription = null;
            this.groupBox.AccessibleName = null;
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.BackgroundImage = null;
            this.groupBox.Controls.Add(this.bgBrush);
            this.groupBox.Controls.Add(this.edgeBrush);
            this.groupBox.Controls.Add(this.edgeWidth);
            this.groupBox.Font = null;
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // bgBrush
            // 
            this.bgBrush.AccessibleDescription = null;
            this.bgBrush.AccessibleName = null;
            resources.ApplyResources(this.bgBrush, "bgBrush");
            this.bgBrush.BackgroundImage = null;
            this.bgBrush.Font = null;
            this.bgBrush.Name = "bgBrush";
            // 
            // edgeBrush
            // 
            this.edgeBrush.AccessibleDescription = null;
            this.edgeBrush.AccessibleName = null;
            resources.ApplyResources(this.edgeBrush, "edgeBrush");
            this.edgeBrush.BackgroundImage = null;
            this.edgeBrush.Font = null;
            this.edgeBrush.Name = "edgeBrush";
            // 
            // edgeWidth
            // 
            this.edgeWidth.AccessibleDescription = null;
            this.edgeWidth.AccessibleName = null;
            resources.ApplyResources(this.edgeWidth, "edgeWidth");
            this.edgeWidth.BackgroundImage = null;
            this.edgeWidth.Font = null;
            this.edgeWidth.Name = "edgeWidth";
            this.edgeWidth.Validating += new System.ComponentModel.CancelEventHandler(this.MaxEdgeWidthValidating);
            // 
            // ViewModeItems
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoSize = true;
            this.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right);
            this.BackgroundImage = null;
            this.Controls.Add(this.groupBox);
            this.Font = null;
            this.Name = "ViewModeItems";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }
        void MaxEdgeWidthValidating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string text = edgeWidth.Text;
            if (String.IsNullOrEmpty(text))
            {
                Util.ShowErrorDialog(String.Format(MessageResources.ErrNoInput, edgeWidth.LabelText));
                edgeWidth.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
            float dummy;
            if (!float.TryParse(text, out dummy) || dummy < 0)
            {
                Util.ShowErrorDialog(MessageResources.ErrInvalidValue);
                edgeWidth.Text = Convert.ToString(m_control.MaxEdgeWidth);
                e.Cancel = true;
                return;
            }
        }

        public void ApplyChanges()
        {
            m_control.ViewBGBrush = bgBrush.Brush;
            m_control.MaxEdgeWidth = float.Parse(edgeWidth.Text);
            m_control.ViewEdgeBrush = edgeBrush.Brush;
        }
    }
}
