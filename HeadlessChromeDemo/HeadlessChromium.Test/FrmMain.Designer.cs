namespace HeadlessChromium.Test
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_chromiumTest = new System.Windows.Forms.Button();
            this.rTxt_log = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_chromiumTest_Encapsulated = new System.Windows.Forms.Button();
            this.btn_nativeChromeTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_chromiumTest
            // 
            this.btn_chromiumTest.Location = new System.Drawing.Point(13, 14);
            this.btn_chromiumTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_chromiumTest.Name = "btn_chromiumTest";
            this.btn_chromiumTest.Size = new System.Drawing.Size(142, 43);
            this.btn_chromiumTest.TabIndex = 0;
            this.btn_chromiumTest.Text = "无头浏览器测试";
            this.btn_chromiumTest.UseVisualStyleBackColor = true;
            this.btn_chromiumTest.Click += new System.EventHandler(this.btn_chromiumTest_Click);
            // 
            // rTxt_log
            // 
            this.rTxt_log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rTxt_log.Location = new System.Drawing.Point(13, 103);
            this.rTxt_log.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rTxt_log.Name = "rTxt_log";
            this.rTxt_log.ReadOnly = true;
            this.rTxt_log.Size = new System.Drawing.Size(558, 295);
            this.rTxt_log.TabIndex = 1;
            this.rTxt_log.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(13, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log：";
            // 
            // btn_chromiumTest_Encapsulated
            // 
            this.btn_chromiumTest_Encapsulated.Location = new System.Drawing.Point(163, 14);
            this.btn_chromiumTest_Encapsulated.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_chromiumTest_Encapsulated.Name = "btn_chromiumTest_Encapsulated";
            this.btn_chromiumTest_Encapsulated.Size = new System.Drawing.Size(208, 43);
            this.btn_chromiumTest_Encapsulated.TabIndex = 3;
            this.btn_chromiumTest_Encapsulated.Text = "无头浏览器测试（封装版）";
            this.btn_chromiumTest_Encapsulated.UseVisualStyleBackColor = true;
            this.btn_chromiumTest_Encapsulated.Click += new System.EventHandler(this.btn_chromiumTest_Encapsulated_Click);
            // 
            // btn_nativeChromeTest
            // 
            this.btn_nativeChromeTest.Location = new System.Drawing.Point(379, 14);
            this.btn_nativeChromeTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_nativeChromeTest.Name = "btn_nativeChromeTest";
            this.btn_nativeChromeTest.Size = new System.Drawing.Size(192, 43);
            this.btn_nativeChromeTest.TabIndex = 4;
            this.btn_nativeChromeTest.Text = "本地 Chrome 浏览器测试";
            this.btn_nativeChromeTest.UseVisualStyleBackColor = true;
            this.btn_nativeChromeTest.Click += new System.EventHandler(this.btn_nativeChromeTest_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.btn_nativeChromeTest);
            this.Controls.Add(this.btn_chromiumTest_Encapsulated);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rTxt_log);
            this.Controls.Add(this.btn_chromiumTest);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chromium 测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_chromiumTest;
        private System.Windows.Forms.RichTextBox rTxt_log;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_chromiumTest_Encapsulated;
        private System.Windows.Forms.Button btn_nativeChromeTest;
    }
}

