using GMBoss.Model;
using GMBoss.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace GMBoss
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            MciPlayer.SetMaxVolume(this.Handle);
        }

        string GetMD5(string text)
        {
            var md5 = MD5CryptoServiceProvider.Create();
            var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            var sb = new StringBuilder(64);

            foreach (var b in buffer)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            codePic.ImageLocation = "http://wbmgmt.youzijie.com/boss/getValidateImg?uuid=" + HostInfo.GetCpuID();
#if DEBUG
            idBox.Text = "csjwk-yzj";
            pwdBox.Text = "123456";
#endif
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            Program.Check();
        }

        private void codePic_Click(object sender, EventArgs e)
        {
            codePic.ImageLocation = codePic.ImageLocation;
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (idBox.Text.Length == 0)
            {
                MessageBox.Show("请输入账号。", "提示");
                idBox.Focus();
                return;
            }

            if (pwdBox.Text.Length == 0)
            {
                MessageBox.Show("请输入密码。", "提示");
                pwdBox.Focus();
                return;
            }

            if (codeBox.Text.Length == 0)
            {
                MessageBox.Show("请输入验证码。", "提示");
                codeBox.Focus();
                return;
            }

            try
            {
                this.Enabled = false;
                var ret = HttpClient.Get("http://wbmgmt.youzijie.com/boss/login", new Query().Go("userId", idBox.Text)
                    .Go("password", GetMD5(pwdBox.Text)).Go("code", codeBox.Text));

                if (string.IsNullOrEmpty(ret))
                {
                    MessageBox.Show("网络请求异常, 请检查本机网络连接状况后重试", "网络请求异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    codePic_Click(null, null);
                    return;
                }

                var rsp = LoginResp.fromXML(ret);
                if (rsp == null)
                {
                    MessageBox.Show("服务器返回异常, 请更新客户端版本", "服务器返回异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    codePic_Click(null, null);
                    return;
                }

                if (rsp.ret != 0 || string.IsNullOrEmpty(rsp.userId) || string.IsNullOrEmpty(rsp.shopName)
                    || rsp.shopId < 1)
                {
                    MessageBox.Show(rsp.message, "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    codePic_Click(null, null);
                    return;
                }

                ClientInfo.NowClient.UserID = rsp.userId;
                ClientInfo.NowClient.ShopID = rsp.shopId;
                ClientInfo.NowClient.Token = rsp.token;
                ClientInfo.NowClient.Name = rsp.shopName;

                MainForm.NewShow(rsp.shopName, rsp.shopId, rsp.userId);
                this.Hide();
            }
            finally
            {
                this.Enabled = true;
            }
        }
    }
}