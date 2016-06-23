using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Update
{
    public partial class MainForm : Form
    {
        bool m_IsFail;
        string m_KillName;
        readonly Downloader m_Downloader = new Downloader();

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private string GetMD5(string path)
        {
            Stream stream = null;

            try
            {
                var md5 = MD5CryptoServiceProvider.Create();
                stream = File.Open(path, FileMode.Open, FileAccess.Read);
                var buffer = md5.ComputeHash(stream);
                var sb = new StringBuilder(64);

                foreach (var b in buffer)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
            finally
            {
                stream.Close();
            }
        }

        private void Downloader_Complete(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(delegate (int state)
                {
                    allBar.Value = allBar.Maximum;
                    allLab.Text = "更新完成,稍后重启程序!";
                    fileBar.Value = 0;
                    fileLab.Text = string.Empty;
                    timer1.Enabled = true;
                }), 0);
            }
            else
            {
                allBar.Value = allBar.Maximum;
                allLab.Text = "更新完成,稍后重启程序!";
                fileBar.Value = 0;
                fileLab.Text = string.Empty;
                timer1.Enabled = true;
            }
        }

        private void Downloader_Progress(object sender, EventArgs e)
        {
            var task = (sender as DownloadTask);

            if (!task.IsFailTask)               //不是失败下载任务
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<int>(delegate (int state)
                    {
                        allBar.Value = m_Downloader.DownloadRate;
                    }), 0);
                }
                else
                {
                    allBar.Value = m_Downloader.DownloadRate;
                }
            }
        }

        private void Task_Complete(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(delegate (int state)
                {
                    var task = (sender as DownloadTask);

                    task.Complete -= Task_Complete;
                    task.Progress -= Task_Progress;
                    task.Error -= Task_Error;
                    fileBar.Value = fileBar.Maximum;
                }), 0);
            }
            else
            {
                var task = (sender as DownloadTask);

                task.Complete -= Task_Complete;
                task.Progress -= Task_Progress;
                task.Error -= Task_Error;
                fileBar.Value = fileBar.Maximum;
            }
        }

        private void Task_Progress(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(delegate (int state)
                {
                    var task = (sender as DownloadTask);

                    fileBar.Value = task.DownloadRate;
                    fileLab.Text = "正在下载:" + task.SavePath;
                    allLab.Text = "正在更新:" + task.SavePath;
                }), 0);
            }
            else
            {
                var task = (sender as DownloadTask);

                fileBar.Value = task.DownloadRate;
                fileLab.Text = "正在下载:" + task.SavePath;
                allLab.Text = "正在更新:" + task.SavePath;
            }
        }

        private void Task_Error(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(delegate (int state)
                {
                    var task = (sender as DownloadTask);

                    task.Complete -= Task_Complete;
                    task.Progress -= Task_Progress;
                    task.Error -= Task_Error;
                    fileLab.Text = "下载文件失败:" + (sender as DownloadTask).SavePath;
                    fileLab.ForeColor = Color.Red;
                    allLab.Text = "更新失败,请重试...";
                    allLab.ForeColor = Color.Red;
                    m_IsFail = true; m_Downloader.IsAbort = true;        //终止下载
                }), 0);
            }
            else
            {
                var task = (sender as DownloadTask);

                task.Complete -= Task_Complete;
                task.Progress -= Task_Progress;
                task.Error -= Task_Error;
                fileLab.Text = "下载文件失败:" + (sender as DownloadTask).SavePath;
                fileLab.ForeColor = Color.Red;
                allLab.Text = "更新失败,请重试...";
                allLab.ForeColor = Color.Red;
                m_IsFail = true; m_Downloader.IsAbort = true;            //终止下载
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Process.Start($@"{Application.StartupPath}\{m_KillName}");
            Application.Exit(); Environment.Exit(0);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_IsFail) e.Cancel = true;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var cmds = Environment.CommandLine
                .Replace(Application.ExecutablePath, "").Split(' ');

            if (cmds.Length < 3)            //参数小于3
            {
                MessageBox.Show("无效操作!", "更新");
                Environment.Exit(0);
                return;
            }

            fileLab.Text = "正在获取更新文件...";
            m_KillName = cmds[1];

            string processName = cmds[1].Substring(0, cmds[1].IndexOf('.'));

            foreach (var p in Process.GetProcessesByName(processName))
            {
                p.Kill();                   //结束相关进程
            }

            var client = new WebClient();
            string ret = null;

            for (var i = 0; i < 3 && string.IsNullOrEmpty(ret); i++)
            {
                try
                {
                    ret = client.DownloadString(cmds[2]);
                }
                catch { ret = null; }
            }

            if (string.IsNullOrEmpty(ret))  //获取更新失败
            {
                fileLab.Text = "获取更新文件失败...请关闭重试!";
                fileLab.ForeColor = Color.Red;
                m_IsFail = true;
                return;
            }

            var fileInfos = ret.Split(new string[] { "\r\n" },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var f in fileInfos)
            {
                var p = f.Split('|');  //path|md5|url|size|...
                var path = Application.StartupPath + p[0];

                if (path.Equals(Application.ExecutablePath,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!File.Exists(path) || !GetMD5(path).Equals(p[1]))
                {
                    var task = Downloader.GetTask(p[2], path, int.Parse(p[3]));

                    task.Progress += Task_Progress;
                    task.Complete += Task_Complete;
                    task.Error += Task_Error;
                    m_Downloader.Enqueue(task);
                }
            }

            if (m_Downloader.TaskCount > 0)
            {
                m_Downloader.Progress += Downloader_Progress;
                m_Downloader.Complete += Downloader_Complete;
                m_Downloader.Download();
            }
            else
            {
                Downloader_Complete(null, null);
            }
        }
    }
}