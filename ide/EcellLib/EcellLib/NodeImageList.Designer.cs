namespace Ecell
{
    partial class NodeImageComponent
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NodeImageComponent));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Project");
            this.imageList1.Images.SetKeyName(1, "Model");
            this.imageList1.Images.SetKeyName(2, "System");
            this.imageList1.Images.SetKeyName(3, "Process");
            this.imageList1.Images.SetKeyName(4, "Variable");
            this.imageList1.Images.SetKeyName(5, "dm");
            this.imageList1.Images.SetKeyName(6, "Parameters");
            this.imageList1.Images.SetKeyName(7, "Log");

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
    }
}
