namespace ToolLauncher
{
    /// <summary>
    /// PropertyForm class.
    /// </summary>
    partial class PropertyForm
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
            this.tabPage00 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelVC1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonVC = new System.Windows.Forms.Button();
            this.textBoxVC = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelVC0 = new System.Windows.Forms.TableLayoutPanel();
            this.labelVC = new System.Windows.Forms.Label();
            this.tableLayoutParnelGSL2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGSL = new System.Windows.Forms.Button();
            this.textBoxGSL = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelGSL0 = new System.Windows.Forms.TableLayoutPanel();
            this.labelGSL = new System.Windows.Forms.Label();
            this.tableLayoutPanelBoost1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonBoost = new System.Windows.Forms.Button();
            this.textBoxBoost = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelBoost0 = new System.Windows.Forms.TableLayoutPanel();
            this.labelBoost = new System.Windows.Forms.Label();
            this.tableLayoutPanelEcell1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonEcell = new System.Windows.Forms.Button();
            this.textBoxEcell = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelEcell0 = new System.Windows.Forms.TableLayoutPanel();
            this.labelEcell = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.fbdEcell = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.fbdBoost = new System.Windows.Forms.FolderBrowserDialog();
            this.fbdGSL = new System.Windows.Forms.FolderBrowserDialog();
            this.fbdVC = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabPage00.SuspendLayout();
            this.tableLayoutPanelVC1.SuspendLayout();
            this.tableLayoutPanelVC0.SuspendLayout();
            this.tableLayoutParnelGSL2.SuspendLayout();
            this.tableLayoutPanelGSL0.SuspendLayout();
            this.tableLayoutPanelBoost1.SuspendLayout();
            this.tableLayoutPanelBoost0.SuspendLayout();
            this.tableLayoutPanelEcell1.SuspendLayout();
            this.tableLayoutPanelEcell0.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage00
            // 
            this.tabPage00.Controls.Add(this.tableLayoutPanelVC1);
            this.tabPage00.Controls.Add(this.tableLayoutPanelVC0);
            this.tabPage00.Controls.Add(this.tableLayoutParnelGSL2);
            this.tabPage00.Controls.Add(this.tableLayoutPanelGSL0);
            this.tabPage00.Controls.Add(this.tableLayoutPanelBoost1);
            this.tabPage00.Controls.Add(this.tableLayoutPanelBoost0);
            this.tabPage00.Controls.Add(this.tableLayoutPanelEcell1);
            this.tabPage00.Controls.Add(this.tableLayoutPanelEcell0);
            this.tabPage00.Location = new System.Drawing.Point(4, 21);
            this.tabPage00.Name = "tabPage00";
            this.tabPage00.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage00.Size = new System.Drawing.Size(462, 225);
            this.tabPage00.TabIndex = 0;
            this.tabPage00.Text = "Environment";
            this.tabPage00.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelVC1
            // 
            this.tableLayoutPanelVC1.ColumnCount = 2;
            this.tableLayoutPanelVC1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.44588F));
            this.tableLayoutPanelVC1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.55411F));
            this.tableLayoutPanelVC1.Controls.Add(this.buttonVC, 1, 0);
            this.tableLayoutPanelVC1.Controls.Add(this.textBoxVC, 0, 0);
            this.tableLayoutPanelVC1.Location = new System.Drawing.Point(0, 194);
            this.tableLayoutPanelVC1.Name = "tableLayoutPanelVC1";
            this.tableLayoutPanelVC1.RowCount = 1;
            this.tableLayoutPanelVC1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelVC1.Size = new System.Drawing.Size(462, 29);
            this.tableLayoutPanelVC1.TabIndex = 7;
            // 
            // buttonVC
            // 
            this.buttonVC.Location = new System.Drawing.Point(407, 3);
            this.buttonVC.Name = "buttonVC";
            this.buttonVC.Size = new System.Drawing.Size(52, 23);
            this.buttonVC.TabIndex = 1;
            this.buttonVC.Text = "Folder";
            this.buttonVC.UseVisualStyleBackColor = true;
            this.buttonVC.Click += new System.EventHandler(this.buttonVC_Click);
            // 
            // textBoxVC
            // 
            this.textBoxVC.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxVC.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxVC.Location = new System.Drawing.Point(21, 4);
            this.textBoxVC.Name = "textBoxVC";
            this.textBoxVC.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxVC.Size = new System.Drawing.Size(380, 20);
            this.textBoxVC.TabIndex = 0;
            // 
            // tableLayoutPanelVC0
            // 
            this.tableLayoutPanelVC0.ColumnCount = 2;
            this.tableLayoutPanelVC0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelVC0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 318F));
            this.tableLayoutPanelVC0.Controls.Add(this.labelVC, 0, 0);
            this.tableLayoutPanelVC0.Location = new System.Drawing.Point(0, 168);
            this.tableLayoutPanelVC0.Name = "tableLayoutPanelVC0";
            this.tableLayoutPanelVC0.RowCount = 2;
            this.tableLayoutPanelVC0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelVC0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelVC0.Size = new System.Drawing.Size(462, 25);
            this.tableLayoutPanelVC0.TabIndex = 6;
            // 
            // labelVC
            // 
            this.labelVC.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelVC.Location = new System.Drawing.Point(3, 0);
            this.labelVC.Name = "labelVC";
            this.labelVC.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelVC.Size = new System.Drawing.Size(136, 23);
            this.labelVC.TabIndex = 0;
            this.labelVC.Text = "/VC++ Root Dir :";
            // 
            // tableLayoutParnelGSL2
            // 
            this.tableLayoutParnelGSL2.ColumnCount = 2;
            this.tableLayoutParnelGSL2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.44588F));
            this.tableLayoutParnelGSL2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.55411F));
            this.tableLayoutParnelGSL2.Controls.Add(this.buttonGSL, 1, 0);
            this.tableLayoutParnelGSL2.Controls.Add(this.textBoxGSL, 0, 0);
            this.tableLayoutParnelGSL2.Location = new System.Drawing.Point(0, 138);
            this.tableLayoutParnelGSL2.Name = "tableLayoutParnelGSL2";
            this.tableLayoutParnelGSL2.RowCount = 1;
            this.tableLayoutParnelGSL2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutParnelGSL2.Size = new System.Drawing.Size(462, 29);
            this.tableLayoutParnelGSL2.TabIndex = 5;
            // 
            // buttonGSL
            // 
            this.buttonGSL.Location = new System.Drawing.Point(407, 3);
            this.buttonGSL.Name = "buttonGSL";
            this.buttonGSL.Size = new System.Drawing.Size(52, 23);
            this.buttonGSL.TabIndex = 1;
            this.buttonGSL.Text = "Folder";
            this.buttonGSL.UseVisualStyleBackColor = true;
            this.buttonGSL.Click += new System.EventHandler(this.buttonGSL_Click);
            // 
            // textBoxGSL
            // 
            this.textBoxGSL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxGSL.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxGSL.Location = new System.Drawing.Point(21, 4);
            this.textBoxGSL.Name = "textBoxGSL";
            this.textBoxGSL.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxGSL.Size = new System.Drawing.Size(380, 20);
            this.textBoxGSL.TabIndex = 0;
            // 
            // tableLayoutPanelGSL0
            // 
            this.tableLayoutPanelGSL0.ColumnCount = 2;
            this.tableLayoutPanelGSL0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelGSL0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 318F));
            this.tableLayoutPanelGSL0.Controls.Add(this.labelGSL, 0, 0);
            this.tableLayoutPanelGSL0.Location = new System.Drawing.Point(0, 112);
            this.tableLayoutPanelGSL0.Name = "tableLayoutPanelGSL0";
            this.tableLayoutPanelGSL0.RowCount = 2;
            this.tableLayoutPanelGSL0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelGSL0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelGSL0.Size = new System.Drawing.Size(462, 25);
            this.tableLayoutPanelGSL0.TabIndex = 4;
            // 
            // labelGSL
            // 
            this.labelGSL.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelGSL.Location = new System.Drawing.Point(3, 0);
            this.labelGSL.Name = "labelGSL";
            this.labelGSL.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelGSL.Size = new System.Drawing.Size(136, 23);
            this.labelGSL.TabIndex = 0;
            this.labelGSL.Text = "/GSL Root Dir :";
            // 
            // tableLayoutPanelBoost1
            // 
            this.tableLayoutPanelBoost1.ColumnCount = 2;
            this.tableLayoutPanelBoost1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.44588F));
            this.tableLayoutPanelBoost1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.55411F));
            this.tableLayoutPanelBoost1.Controls.Add(this.buttonBoost, 1, 0);
            this.tableLayoutPanelBoost1.Controls.Add(this.textBoxBoost, 0, 0);
            this.tableLayoutPanelBoost1.Location = new System.Drawing.Point(0, 82);
            this.tableLayoutPanelBoost1.Name = "tableLayoutPanelBoost1";
            this.tableLayoutPanelBoost1.RowCount = 1;
            this.tableLayoutPanelBoost1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBoost1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelBoost1.Size = new System.Drawing.Size(462, 29);
            this.tableLayoutPanelBoost1.TabIndex = 3;
            // 
            // buttonBoost
            // 
            this.buttonBoost.Location = new System.Drawing.Point(407, 3);
            this.buttonBoost.Name = "buttonBoost";
            this.buttonBoost.Size = new System.Drawing.Size(52, 23);
            this.buttonBoost.TabIndex = 1;
            this.buttonBoost.Text = "Folder";
            this.buttonBoost.UseVisualStyleBackColor = true;
            this.buttonBoost.Click += new System.EventHandler(this.buttonBoost_Click);
            // 
            // textBoxBoost
            // 
            this.textBoxBoost.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxBoost.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxBoost.Location = new System.Drawing.Point(21, 4);
            this.textBoxBoost.Name = "textBoxBoost";
            this.textBoxBoost.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxBoost.Size = new System.Drawing.Size(380, 20);
            this.textBoxBoost.TabIndex = 0;
            // 
            // tableLayoutPanelBoost0
            // 
            this.tableLayoutPanelBoost0.ColumnCount = 2;
            this.tableLayoutPanelBoost0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBoost0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 318F));
            this.tableLayoutPanelBoost0.Controls.Add(this.labelBoost, 0, 0);
            this.tableLayoutPanelBoost0.Location = new System.Drawing.Point(0, 56);
            this.tableLayoutPanelBoost0.Name = "tableLayoutPanelBoost0";
            this.tableLayoutPanelBoost0.RowCount = 2;
            this.tableLayoutPanelBoost0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelBoost0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelBoost0.Size = new System.Drawing.Size(462, 25);
            this.tableLayoutPanelBoost0.TabIndex = 2;
            // 
            // labelBoost
            // 
            this.labelBoost.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelBoost.Location = new System.Drawing.Point(3, 0);
            this.labelBoost.Name = "labelBoost";
            this.labelBoost.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelBoost.Size = new System.Drawing.Size(136, 23);
            this.labelBoost.TabIndex = 0;
            this.labelBoost.Text = "/Boost Root Dir :";
            // 
            // tableLayoutPanelEcell1
            // 
            this.tableLayoutPanelEcell1.ColumnCount = 2;
            this.tableLayoutPanelEcell1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.44588F));
            this.tableLayoutPanelEcell1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.55411F));
            this.tableLayoutPanelEcell1.Controls.Add(this.buttonEcell, 1, 0);
            this.tableLayoutPanelEcell1.Controls.Add(this.textBoxEcell, 0, 0);
            this.tableLayoutPanelEcell1.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanelEcell1.Name = "tableLayoutPanelEcell1";
            this.tableLayoutPanelEcell1.RowCount = 1;
            this.tableLayoutPanelEcell1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEcell1.Size = new System.Drawing.Size(462, 29);
            this.tableLayoutPanelEcell1.TabIndex = 1;
            // 
            // buttonEcell
            // 
            this.buttonEcell.Location = new System.Drawing.Point(407, 3);
            this.buttonEcell.Name = "buttonEcell";
            this.buttonEcell.Size = new System.Drawing.Size(52, 23);
            this.buttonEcell.TabIndex = 1;
            this.buttonEcell.Text = "Folder";
            this.buttonEcell.UseVisualStyleBackColor = true;
            this.buttonEcell.Click += new System.EventHandler(this.buttonEcell_Click);
            // 
            // textBoxEcell
            // 
            this.textBoxEcell.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBoxEcell.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxEcell.Location = new System.Drawing.Point(21, 4);
            this.textBoxEcell.Name = "textBoxEcell";
            this.textBoxEcell.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxEcell.Size = new System.Drawing.Size(380, 20);
            this.textBoxEcell.TabIndex = 0;
            // 
            // tableLayoutPanelEcell0
            // 
            this.tableLayoutPanelEcell0.ColumnCount = 2;
            this.tableLayoutPanelEcell0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEcell0.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 318F));
            this.tableLayoutPanelEcell0.Controls.Add(this.labelEcell, 0, 0);
            this.tableLayoutPanelEcell0.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEcell0.Name = "tableLayoutPanelEcell0";
            this.tableLayoutPanelEcell0.RowCount = 2;
            this.tableLayoutPanelEcell0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelEcell0.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanelEcell0.Size = new System.Drawing.Size(462, 25);
            this.tableLayoutPanelEcell0.TabIndex = 0;
            // 
            // labelEcell
            // 
            this.labelEcell.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEcell.Location = new System.Drawing.Point(3, 0);
            this.labelEcell.Name = "labelEcell";
            this.labelEcell.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelEcell.Size = new System.Drawing.Size(136, 23);
            this.labelEcell.TabIndex = 0;
            this.labelEcell.Text = "/E-Cell Root Dir :";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage00);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(470, 250);
            this.tabControl.TabIndex = 0;
            // 
            // fbdEcell
            // 
            this.fbdEcell.Description = "Please select a folder.";
            this.fbdEcell.SelectedPath = "C:\\";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.44588F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.55411F));
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(177, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 23);
            this.button1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 0;
            // 
            // fbdBoost
            // 
            this.fbdBoost.Description = "Please select a folder.";
            this.fbdBoost.SelectedPath = "C:\\";
            // 
            // fbdGSL
            // 
            this.fbdGSL.Description = "Please select a folder.";
            this.fbdGSL.SelectedPath = "C:\\";
            // 
            // fbdVC
            // 
            this.fbdVC.Description = "Please select a folder.";
            this.fbdVC.SelectedPath = "C:\\";
            // 
            // buttonDefault
            // 
            this.buttonDefault.Location = new System.Drawing.Point(243, 268);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(75, 23);
            this.buttonDefault.TabIndex = 1;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(324, 268);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(405, 268);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // PropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 316);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.tabControl);
            this.Name = "PropertyForm";
            this.Text = "Property";
            this.tabPage00.ResumeLayout(false);
            this.tableLayoutPanelVC1.ResumeLayout(false);
            this.tableLayoutPanelVC1.PerformLayout();
            this.tableLayoutPanelVC0.ResumeLayout(false);
            this.tableLayoutParnelGSL2.ResumeLayout(false);
            this.tableLayoutParnelGSL2.PerformLayout();
            this.tableLayoutPanelGSL0.ResumeLayout(false);
            this.tableLayoutPanelBoost1.ResumeLayout(false);
            this.tableLayoutPanelBoost1.PerformLayout();
            this.tableLayoutPanelBoost0.ResumeLayout(false);
            this.tableLayoutPanelEcell1.ResumeLayout(false);
            this.tableLayoutPanelEcell1.PerformLayout();
            this.tableLayoutPanelEcell0.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //
        // 
        //
        private System.Windows.Forms.FolderBrowserDialog fbdBoost;
        private System.Windows.Forms.FolderBrowserDialog fbdEcell;
        private System.Windows.Forms.FolderBrowserDialog fbdGSL;
        private System.Windows.Forms.FolderBrowserDialog fbdVC;
        //
        //
        //
        private System.Windows.Forms.TabPage tabPage00;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEcell0;
        private System.Windows.Forms.Label labelEcell;
        private System.Windows.Forms.Button buttonEcell;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEcell1;
        private System.Windows.Forms.TextBox textBoxEcell;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBoost0;
        private System.Windows.Forms.Label labelBoost;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBoost1;
        private System.Windows.Forms.Button buttonBoost;
        private System.Windows.Forms.TextBox textBoxBoost;
        private System.Windows.Forms.TableLayoutPanel tableLayoutParnelGSL2;
        private System.Windows.Forms.Button buttonGSL;
        private System.Windows.Forms.TextBox textBoxGSL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGSL0;
        private System.Windows.Forms.Label labelGSL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelVC1;
        private System.Windows.Forms.Button buttonVC;
        private System.Windows.Forms.TextBox textBoxVC;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelVC0;
        private System.Windows.Forms.Label labelVC;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
