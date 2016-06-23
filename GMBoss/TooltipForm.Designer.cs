namespace GMBoss
{
    partial class TooltipForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TooltipForm));
            this.headLab = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.PictureBox();
            this.icon = new System.Windows.Forms.PictureBox();
            this.contentLab = new System.Windows.Forms.LinkLabel();
            this.linkText = new System.Windows.Forms.Label();
            this.titleLab = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // headLab
            // 
            this.headLab.BackColor = System.Drawing.Color.Transparent;
            this.headLab.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.headLab.Location = new System.Drawing.Point(30, 6);
            this.headLab.Name = "headLab";
            this.headLab.Size = new System.Drawing.Size(56, 17);
            this.headLab.TabIndex = 0;
            this.headLab.Text = "系统消息";
            this.headLab.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.BurlyWood;
            this.panel1.Controls.Add(this.closeButton);
            this.panel1.Controls.Add(this.icon);
            this.panel1.Controls.Add(this.headLab);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 30);
            this.panel1.TabIndex = 3;
            // 
            // closeButton
            // 
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.Image = ((System.Drawing.Image)(resources.GetObject("closeButton.Image")));
            this.closeButton.Location = new System.Drawing.Point(270, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(30, 30);
            this.closeButton.TabIndex = 4;
            this.closeButton.TabStop = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // icon
            // 
            this.icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.icon.Image = ((System.Drawing.Image)(resources.GetObject("icon.Image")));
            this.icon.Location = new System.Drawing.Point(11, 5);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(18, 18);
            this.icon.TabIndex = 3;
            this.icon.TabStop = false;
            // 
            // contentLab
            // 
            this.contentLab.ActiveLinkColor = System.Drawing.Color.Moccasin;
            this.contentLab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentLab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.contentLab.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.contentLab.ForeColor = System.Drawing.Color.Gray;
            this.contentLab.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.contentLab.LinkColor = System.Drawing.Color.DimGray;
            this.contentLab.Location = new System.Drawing.Point(14, 72);
            this.contentLab.Name = "contentLab";
            this.contentLab.Size = new System.Drawing.Size(274, 91);
            this.contentLab.TabIndex = 7;
            this.contentLab.TabStop = true;
            this.contentLab.Text = "消息内容";
            this.contentLab.UseCompatibleTextRendering = true;
            // 
            // linkText
            // 
            this.linkText.AutoSize = true;
            this.linkText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkText.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkText.ForeColor = System.Drawing.SystemColors.Highlight;
            this.linkText.Location = new System.Drawing.Point(231, 179);
            this.linkText.Name = "linkText";
            this.linkText.Size = new System.Drawing.Size(57, 12);
            this.linkText.TabIndex = 6;
            this.linkText.Text = "查看详情";
            this.linkText.Click += new System.EventHandler(this.linkText_Click);
            // 
            // titleLab
            // 
            this.titleLab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLab.Cursor = System.Windows.Forms.Cursors.Hand;
            this.titleLab.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLab.Location = new System.Drawing.Point(12, 37);
            this.titleLab.Name = "titleLab";
            this.titleLab.Size = new System.Drawing.Size(276, 24);
            this.titleLab.TabIndex = 5;
            this.titleLab.Text = "消息标题";
            this.titleLab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TooltipForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.ControlBox = false;
            this.Controls.Add(this.contentLab);
            this.Controls.Add(this.linkText);
            this.Controls.Add(this.titleLab);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TooltipForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TooltipForm";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.closeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label headLab;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox closeButton;
        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.LinkLabel contentLab;
        private System.Windows.Forms.Label linkText;
        private System.Windows.Forms.Label titleLab;
    }
}