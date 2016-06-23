using GMBoss.Log;
using GMBoss.Model;
using GMBoss.Rudp;
using GMBoss.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GMBoss
{
    public partial class MainForm : Form
    {
        readonly List<Downloader> m_Downloaders = new List<Downloader>(8);
        readonly Queue<VoiceFilesResp.VoiceFile> m_PlayFiles =
            new Queue<VoiceFilesResp.VoiceFile>(16);
        readonly RudpClient m_UdpClient = new RudpClient();
        readonly string ActionChars = @"-\|/";
        ConfigForm m_Config;
        int m_AIdx = 0;

        class HostIps
        {
            public string[] Ips { get; set; }

            public int Port { get; set; }
        }

        public class CallParam
        {
            public MainForm MainForm { get; set; }

            public string IP { get; set; }

            public string HostName { get; set; }

            public string Content { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Icon = this.Icon;
        }

        public static void NewShow(string name, int shopID, string userID)
        {
            var mainForm = new MainForm();

            mainForm.nameLab.Text = "网吧: " + name;
            mainForm.idLab.Text = "编号: " + shopID.ToString();
            mainForm.userLab.Text = "账号: " + userID;
            mainForm.Show();
        }

        public static void NewShow()
        {
            var mainForm = new MainForm();

            mainForm.nameLab.Text = "网吧: " + ClientInfo.NowClient.Name;
            mainForm.idLab.Text = "编号: " + ClientInfo.NowClient.ShopID;
            mainForm.userLab.Text = "账号: " + ClientInfo.NowClient.UserID;
            mainForm.Show();
        }

        public void ClearLog()
        {
            logBox.Clear();
        }

        public void Log(string log)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(delegate (string l)
                {
                    NLog(l);
                }), log);
            }
            else
            {
                NLog(log);
            }
        }

        private void NLog(string log)
        {
            if (logBox.Lines.Length >= 100)
                ClearLog();

            logBox.AppendText(log + Environment.NewLine);
        }

        public void Play(string path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(delegate (string p)
                {
                    MciPlayer.Play(path);
                }), path);
            }
            else
            {
                MciPlayer.Play(path);
            }
        }

        public void ShowTooltip(string title, string content)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(delegate (string p)
                {
                    TooltipForm.NewShow(title, content);
                }), string.Empty);
            }
            else
            {
                TooltipForm.NewShow(title, content);
            }
        }

        private void CloseApp()
        {
            notifyIcon.Visible = false;
            Application.Exit();
            Environment.Exit(0);
        }

        private void logBox_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否清除Log？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                ClearLog();
        }

        private void shTimer_Tick(object sender, EventArgs e)
        {
            shLab.Text = $"与服务器已连接...{ActionChars[m_AIdx++]}";
            if (m_AIdx == ActionChars.Length) m_AIdx = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            versionLab.Text = $"  Version:{GMBoss.Utils.ClientInfo.Version} 正式版";
            notifyIcon.ShowBalloonTip(30000, "启动成功", "网吧竞技大师实时语音播报已启动",
                ToolTipIcon.Info);

            var ips = new HostIps()         //上报的IP信息
            {
                Ips = HelpUtils.GetIP(),
                Port = HelpUtils.GetUsePort(19051)
            };

            m_UdpClient.Bind("0.0.0.0", ips.Port);
            m_UdpClient.Data += M_UdpClient_Data;
            ThreadPool.QueueUserWorkItem(GetVoiceFiles, ips);
            this.Hide();
        }

        private void M_UdpClient_Data(object sender, DataEventArgs e)
        {
            var cmd = Encoding.UTF8.GetString(e.Data).Split(' ');
            var client = sender as RudpClient;

            switch (cmd[0].ToLower())
            {
                case "hello":               //机器探测收银机IP
                    {
                        client.Send(Encoding.UTF8.GetBytes("hello_ok"));
                        break;
                    }

                case "call":                //呼叫网管
                    {
                        client.Send(Encoding.UTF8.GetBytes("call_ok"));
                        ThreadPool.QueueUserWorkItem(OnClientCall,
                            new CallParam()
                            {
                                MainForm = this,
                                IP = client.RemoteEP.Address.ToString(),
                                HostName = Encoding.UTF8.GetString(Convert.FromBase64String(cmd[1])),
                                Content = Encoding.UTF8.GetString(Convert.FromBase64String(cmd[2]))
                            });
                        break;
                    }
            }
        }

        private void GetVoiceFiles(object state)
        {
#if DEBUG
            var url = "http://beta.wbmgmt.youzijie.com/boss/queryVoices";
#else
            var url = "http://wbmgmt.youzijie.com/boss/queryVoices";
#endif

            var dir = Application.StartupPath + @"\tempFiles\";

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            Log("连接服务器成功...");
            ThreadPool.QueueUserWorkItem(PlayVoiceFiles);

            bool isPostIps = false;
            var ips = state as HostIps;
            var newUrl = $"{url}?BossIp={string.Join("|", ips.Ips)}&BossPort={ips.Port}";

            while (true)
            {
                try
                {
                    var ret = HttpClient.Get(!isPostIps ? newUrl : url);

                    if (string.IsNullOrEmpty(ret))
                    {
                        Loger.Log("ret is null or empty.");
                        continue;
                    }

                    var rsp = VoiceFilesResp.fromXML(ret);

                    if (rsp == null)
                    {
                        Loger.Log("CheckClientResp is null.");
                        continue;
                    }

                    if (rsp.ret == 999) //登陆Token失效
                    {
                        MessageBox.Show(rsp.message, "GMBoss 登录失效", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Loger.Log("Token expired exit code. Exit.");
                        this.CloseApp();
                        return;
                    }

                    //if (rsp.ret == 998) //版本强制更新
                    //{
                    //    MessageBox.Show(rsp.message, "GMBoss 已发布最新版本，请更新。", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    this.CloseApp();
                    //    return;
                    //}

                    if (rsp.latesVersion > ClientInfo.Version
                        || rsp.forceUpdate)
                    {
                        var exePath = Application.ExecutablePath;

                        Loger.Log($"from {ClientInfo.Version} update to {rsp.latesVersion}");
                        Process.Start(Application.StartupPath + @"\update.exe",
                            $"{exePath.Substring(exePath.LastIndexOf("\\") + 1)} {rsp.latesVersionUrl}");
                        Application.Exit(); Environment.Exit(0);
                    }

                    if (rsp.ret == 996) //上报日志数据
                    {
                        //上报日志操作
                        Loger.Log("need update to server is log.");
                        continue;
                    }

                    if (!isPostIps)     //上报IP完成
                    {
                        isPostIps = true;
                    }

                    if (rsp.ret == 0 && rsp.files.Count > 0)
                    {
                        var downloader = new Downloader();

                        m_Downloaders.Add(downloader);
                        foreach (var vf in rsp.files)
                        {
                            var filePath = $"{dir}{Guid.NewGuid().ToString()}.mp3";
                            var task = downloader.Enqueue(vf.url, filePath);

                            task.Tag = vf;
                        }
                        downloader.Complete += Downloader_Complete;
                        downloader.Progress += Downloader_Progress;
                        downloader.Download();
                    }
                }
                catch (Exception e)
                {
                    Loger.Log(e, new StackTrace());
                }
                finally
                {
                    Thread.Sleep(30000);
                }
            }
        }

        private void PlayVoiceFiles(object state)
        {
            VoiceFilesResp.VoiceFile vf = null;

            Log("实时语音播报已启动...");

            while (true)
            {
                try
                {
                    lock (m_PlayFiles)
                    {
                        if (m_PlayFiles.Count > 0)
                            vf = m_PlayFiles.Peek();
                        else
                            vf = null;
                    }

                    if (vf != null)
                    {
                        vf.times--;
                        this.Play(vf.url);
                        Thread.Sleep(++vf.length * 1000);

                        if (vf.times == 0)
                        {
                            File.Delete(vf.url);
                            Log($"已播放：{vf.url}");
                            lock (m_PlayFiles) { m_PlayFiles.Dequeue(); }
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch
                {
                    Loger.Log("播放音频文件出错.");
                }
            }
        }

        private void OnClientCall(object state)
        {
            var param = state as CallParam;
            var dir = Application.StartupPath + @"\tempFiles\";

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

#if DEBUG
            var url = "http://beta.wbmgmt.youzijie.com/boss/callWebMaster";
#else
            var url = "http://wbmgmt.youzijie.com/boss/callWebMaster";
#endif

            var ret = HttpClient.Get(url, new Query().Go("ip", param.IP)
                .Go("hostName", param.HostName)
                .Go("content", param.Content));

            if (string.IsNullOrEmpty(ret))
            {
                Loger.Log("ret is null or empty.");
                return;
            }

            var rsp = VoiceFilesResp.fromXML(ret);

            if (rsp == null)
            {
                Loger.Log("CheckClientResp is null.");
                return;
            }

            if (rsp.ret == 0 && rsp.files.Count > 0)
            {
                var downloader = new Downloader();

                m_Downloaders.Add(downloader);
                foreach (var vf in rsp.files)
                {
                    var filePath = $"{dir}{Guid.NewGuid().ToString()}.mp3";
                    var task = downloader.Enqueue(vf.url, filePath);

                    task.Tag = vf;
                    ShowTooltip(vf.title, vf.content);
                }
                downloader.Complete += Downloader_Complete;
                downloader.Progress += Downloader_Progress;
                downloader.Download();
            }
        }

        private void Downloader_Progress(object sender, EventArgs e)
        {
            var task = sender as DownloadTask;
            var vf = task.Tag as VoiceFilesResp.VoiceFile;

            if (!task.IsFailTask)
            {
                lock (m_PlayFiles)
                {
                    vf.url = task.SavePath;
                    m_PlayFiles.Enqueue(vf);
                }
                Loger.Log(task.SavePath + " 下载完成.");
            }
            else
            {
                Log(task.SavePath + " 下载失败.");
                Loger.Log(task.SavePath + " 下载失败.");
            }
        }

        private void Downloader_Complete(object sender, EventArgs e)
        {
            var downloader = sender as Downloader;

            downloader.Complete -= Downloader_Complete;
            downloader.Progress -= Downloader_Progress;
            m_Downloaders.Remove(downloader);
            Loger.Log("本次所有音频文件下载完成.");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == showMenu)
            {
                this.Show();
            }
            else if (e.ClickedItem == backstageMenu)
            {
                string url = $"http://wbmgmt.youzijie.com/boss/gotoMgmt/{ClientInfo.NowClient.ShopID}/{ClientInfo.NowClient.UserID}/{ClientInfo.NowClient.Token}";
                Process.Start(url);
            }
            else if (e.ClickedItem == configMenu)
            {
                if (m_Config == null)
                    m_Config = new ConfigForm();

                m_Config.Show();
            }
            else if (e.ClickedItem == exitMenu)
            {
                this.CloseApp();
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
            }
        }

        private void testBtn_Click(object sender, EventArgs e)
        {
            MciPlayer.Play($@"{Application.StartupPath}\tempFiles\tip.mp3");
        }
    }
}