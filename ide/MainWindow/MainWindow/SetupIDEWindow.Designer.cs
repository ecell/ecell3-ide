namespace EcellLib.MainWindow
{
    partial class SetupIDEWindow
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

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupIDEWindow));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.langGroupBox = new System.Windows.Forms.GroupBox();
            this.jpRadioButton = new System.Windows.Forms.RadioButton();
            this.enRadioButton = new System.Windows.Forms.RadioButton();
            this.autoRadioButton = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.SIOKButton = new System.Windows.Forms.Button();
            this.SICancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.langGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.langGroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // langGroupBox
            // 
            this.langGroupBox.Controls.Add(this.jpRadioButton);
            this.langGroupBox.Controls.Add(this.enRadioButton);
            this.langGroupBox.Controls.Add(this.autoRadioButton);
            resources.ApplyResources(this.langGroupBox, "langGroupBox");
            this.langGroupBox.Name = "langGroupBox";
            this.langGroupBox.TabStop = false;
            // 
            // jpRadioButton
            // 
            resources.ApplyResources(this.jpRadioButton, "jpRadioButton");
            this.jpRadioButton.Name = "jpRadioButton";
            this.jpRadioButton.TabStop = true;
            this.jpRadioButton.UseVisualStyleBackColor = true;
            // 
            // enRadioButton
            // 
            resources.ApplyResources(this.enRadioButton, "enRadioButton");
            this.enRadioButton.Name = "enRadioButton";
            this.enRadioButton.TabStop = true;
            this.enRadioButton.UseVisualStyleBackColor = true;
            // 
            // autoRadioButton
            // 
            resources.ApplyResources(this.autoRadioButton, "autoRadioButton");
            this.autoRadioButton.Name = "autoRadioButton";
            this.autoRadioButton.TabStop = true;
            this.autoRadioButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.SIOKButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.SICancelButton, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // SIOKButton
            // 
            resources.ApplyResources(this.SIOKButton, "SIOKButton");
            this.SIOKButton.Name = "SIOKButton";
            this.SIOKButton.UseVisualStyleBackColor = true;
            this.SIOKButton.Click += new System.EventHandler(this.OKButtonClick);
            // 
            // SICancelButton
            // 
            resources.ApplyResources(this.SICancelButton, "SICancelButton");
            this.SICancelButton.Name = "SICancelButton";
            this.SICancelButton.UseVisualStyleBackColor = true;
            this.SICancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // SetupIDEWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SetupIDEWindow";
            this.Shown += new System.EventHandler(this.SetupWindowIDEWindowShown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.langGroupBox.ResumeLayout(false);
            this.langGroupBox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox langGroupBox;
        public System.Windows.Forms.RadioButton jpRadioButton;
        public System.Windows.Forms.RadioButton enRadioButton;
        public System.Windows.Forms.RadioButton autoRadioButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.Button SIOKButton;
        public System.Windows.Forms.Button SICancelButton;
    }
}