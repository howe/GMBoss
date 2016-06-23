namespace Update
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.fileBar = new System.Windows.Forms.ProgressBar();
            this.allBar = new System.Windows.Forms.ProgressBar();
            this.fileLab = new System.Windows.Forms.Label();
            this.allLab = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // fileBar
            // 
            this.fileBar.Location = new System.Drawing.Point(22, 41);
            this.fileBar.Name = "fileBar";
            this.fileBar.Size = new System.Drawing.Size(376, 23);
            this.fileBar.TabIndex = 0;
            // 
            // allBar
            // 
            this.allBar.Location = new System.Drawing.Point(22, 99);
            this.allBar.Name = "allBar";
            this.allBar.Size = new System.Drawing.Size(376, 23);
            this.allBar.TabIndex = 1;
            // 
            // fileLab
            // 
            this.fileLab.AutoSize = true;
            this.fileLab.Location = new System.Drawing.Point(20, 26);
            this.fileLab.Name = "fileLab";
            this.fileLab.Size = new System.Drawing.Size(0, 12);
            this.fileLab.TabIndex = 2;
            // 
            // allLab
            // 
            this.allLab.AutoSize = true;
            this.allLab.Location = new System.Drawing.Point(20, 84);
            this.allLab.Name = "allLab";
            this.allLab.Size = new System.Drawing.Size(0, 12);
            this.allLab.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 151);
            this.Controls.Add(this.allLab);
            this.Controls.Add(this.fileLab);
            this.Controls.Add(this.allBar);
            this.Controls.Add(this.fileBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网吧竞技大师更新程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar fileBar;
        private System.Windows.Forms.ProgressBar allBar;
        private System.Windows.Forms.Label fileLab;
        private System.Windows.Forms.Label allLab;
        private System.Windows.Forms.Timer timer1;
    }
}

