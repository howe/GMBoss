namespace GMBoss
{
    partial class LoginForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.idBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pwdBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.codePic = new System.Windows.Forms.PictureBox();
            this.loginBtn = new System.Windows.Forms.Button();
            this.codeBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.codePic)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户名：";
            // 
            // idBox
            // 
            this.idBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.idBox.Location = new System.Drawing.Point(161, 32);
            this.idBox.MaxLength = 20;
            this.idBox.Name = "idBox";
            this.idBox.Size = new System.Drawing.Size(131, 23);
            this.idBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码：";
            // 
            // pwdBox
            // 
            this.pwdBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pwdBox.Location = new System.Drawing.Point(161, 71);
            this.pwdBox.MaxLength = 20;
            this.pwdBox.Name = "pwdBox";
            this.pwdBox.PasswordChar = '*';
            this.pwdBox.Size = new System.Drawing.Size(131, 23);
            this.pwdBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "验证码：";
            // 
            // codePic
            // 
            this.codePic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.codePic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.codePic.Location = new System.Drawing.Point(161, 138);
            this.codePic.Name = "codePic";
            this.codePic.Size = new System.Drawing.Size(131, 31);
            this.codePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.codePic.TabIndex = 5;
            this.codePic.TabStop = false;
            this.codePic.Click += new System.EventHandler(this.codePic_Click);
            // 
            // loginBtn
            // 
            this.loginBtn.Location = new System.Drawing.Point(101, 187);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(191, 42);
            this.loginBtn.TabIndex = 6;
            this.loginBtn.Text = "登陆";
            this.loginBtn.UseVisualStyleBackColor = true;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // codeBox
            // 
            this.codeBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.codeBox.Location = new System.Drawing.Point(161, 110);
            this.codeBox.MaxLength = 4;
            this.codeBox.Name = "codeBox";
            this.codeBox.Size = new System.Drawing.Size(131, 23);
            this.codeBox.TabIndex = 5;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.loginBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 255);
            this.Controls.Add(this.codeBox);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.codePic);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pwdBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.idBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登陆";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.codePic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox idBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox pwdBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox codePic;
        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.TextBox codeBox;
    }
}