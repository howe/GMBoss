namespace GMBoss
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
            this.logBox = new System.Windows.Forms.TextBox();
            this.shTimer = new System.Windows.Forms.Timer(this.components);
            this.bottomBar = new System.Windows.Forms.StatusStrip();
            this.versionLab = new System.Windows.Forms.ToolStripStatusLabel();
            this.sLab = new System.Windows.Forms.ToolStripStatusLabel();
            this.shLab = new System.Windows.Forms.ToolStripStatusLabel();
            this.nameLab = new System.Windows.Forms.Label();
            this.idLab = new System.Windows.Forms.Label();
            this.userLab = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.backstageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.configMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.testBtn = new System.Windows.Forms.Button();
            this.bottomBar.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.SystemColors.Window;
            this.logBox.Location = new System.Drawing.Point(12, 84);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(332, 97);
            this.logBox.TabIndex = 0;
            this.logBox.DoubleClick += new System.EventHandler(this.logBox_DoubleClick);
            // 
            // shTimer
            // 
            this.shTimer.Enabled = true;
            this.shTimer.Interval = 1000;
            this.shTimer.Tick += new System.EventHandler(this.shTimer_Tick);
            // 
            // bottomBar
            // 
            this.bottomBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionLab,
            this.sLab,
            this.shLab});
            this.bottomBar.Location = new System.Drawing.Point(0, 184);
            this.bottomBar.Name = "bottomBar";
            this.bottomBar.Size = new System.Drawing.Size(356, 22);
            this.bottomBar.SizingGrip = false;
            this.bottomBar.TabIndex = 2;
            this.bottomBar.Text = "statusStrip1";
            // 
            // versionLab
            // 
            this.versionLab.Name = "versionLab";
            this.versionLab.Size = new System.Drawing.Size(116, 17);
            // 
            // sLab
            // 
            this.sLab.Name = "sLab";
            this.sLab.Size = new System.Drawing.Size(11, 17);
            this.sLab.Text = "|";
            // 
            // shLab
            // 
            this.shLab.ForeColor = System.Drawing.Color.Green;
            this.shLab.Name = "shLab";
            this.shLab.Size = new System.Drawing.Size(0, 17);
            // 
            // nameLab
            // 
            this.nameLab.AutoSize = true;
            this.nameLab.Location = new System.Drawing.Point(12, 11);
            this.nameLab.Name = "nameLab";
            this.nameLab.Size = new System.Drawing.Size(41, 12);
            this.nameLab.TabIndex = 3;
            this.nameLab.Text = "label1";
            // 
            // idLab
            // 
            this.idLab.AutoSize = true;
            this.idLab.Location = new System.Drawing.Point(12, 35);
            this.idLab.Name = "idLab";
            this.idLab.Size = new System.Drawing.Size(41, 12);
            this.idLab.TabIndex = 4;
            this.idLab.Text = "label1";
            // 
            // userLab
            // 
            this.userLab.AutoSize = true;
            this.userLab.Location = new System.Drawing.Point(12, 59);
            this.userLab.Name = "userLab";
            this.userLab.Size = new System.Drawing.Size(41, 12);
            this.userLab.TabIndex = 5;
            this.userLab.Text = "label2";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "网吧竞技大师辅助端（收银机）";
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Text = "网吧竞技大师辅助端（收银机）";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMenu,
            this.backstageMenu,
            this.configMenu,
            this.exitMenu});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(133, 92);
            this.contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_ItemClicked);
            // 
            // showMenu
            // 
            this.showMenu.Name = "showMenu";
            this.showMenu.Size = new System.Drawing.Size(132, 22);
            this.showMenu.Text = "主窗口(&M)";
            // 
            // backstageMenu
            // 
            this.backstageMenu.Name = "backstageMenu";
            this.backstageMenu.Size = new System.Drawing.Size(132, 22);
            this.backstageMenu.Text = "管理(&B)";
            // 
            // configMenu
            // 
            this.configMenu.Name = "configMenu";
            this.configMenu.Size = new System.Drawing.Size(132, 22);
            this.configMenu.Text = "设置(&C)";
            // 
            // exitMenu
            // 
            this.exitMenu.Name = "exitMenu";
            this.exitMenu.Size = new System.Drawing.Size(132, 22);
            this.exitMenu.Text = "退出(&E)";
            // 
            // testBtn
            // 
            this.testBtn.Location = new System.Drawing.Point(266, 11);
            this.testBtn.Name = "testBtn";
            this.testBtn.Size = new System.Drawing.Size(78, 23);
            this.testBtn.TabIndex = 6;
            this.testBtn.Text = "声音测试";
            this.testBtn.UseVisualStyleBackColor = true;
            this.testBtn.Click += new System.EventHandler(this.testBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 206);
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.testBtn);
            this.Controls.Add(this.userLab);
            this.Controls.Add(this.idLab);
            this.Controls.Add(this.nameLab);
            this.Controls.Add(this.bottomBar);
            this.Controls.Add(this.logBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网吧竞技大师辅助端（收银机）";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.bottomBar.ResumeLayout(false);
            this.bottomBar.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Timer shTimer;
        private System.Windows.Forms.StatusStrip bottomBar;
        private System.Windows.Forms.ToolStripStatusLabel shLab;
        private System.Windows.Forms.Label nameLab;
        private System.Windows.Forms.Label idLab;
        private System.Windows.Forms.Label userLab;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showMenu;
        private System.Windows.Forms.ToolStripMenuItem backstageMenu;
        private System.Windows.Forms.ToolStripMenuItem configMenu;
        private System.Windows.Forms.ToolStripMenuItem exitMenu;
        private System.Windows.Forms.Button testBtn;
        private System.Windows.Forms.ToolStripStatusLabel versionLab;
        private System.Windows.Forms.ToolStripStatusLabel sLab;
    }
}