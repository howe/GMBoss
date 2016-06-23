using GMBoss.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMBoss
{
    public partial class TooltipForm : Form
    {
        public TooltipForm()
        {
            InitializeComponent();
        }

        public static void NewShow(string title, string content)
        {
            var tooltip = new TooltipForm();

            tooltip.headLab.Text = title;
            tooltip.contentLab.Text = content;
            tooltip.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - tooltip.Width,
                Screen.PrimaryScreen.WorkingArea.Height - tooltip.Height);
            tooltip.Show();
        }

        private void linkText_Click(object sender, EventArgs e)
        {
            string url = $"http://wbmgmt.youzijie.com/boss/gotoMgmt/{ClientInfo.NowClient.ShopID}/{ClientInfo.NowClient.UserID}/{ClientInfo.NowClient.Token}";
            Process.Start(url);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}