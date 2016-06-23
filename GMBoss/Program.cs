using GMBoss.Log;
using GMBoss.Model;
using GMBoss.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace GMBoss
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            Application.ThreadException += ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LoginForm());
        }

        public static void Check()
        {
            RegHelper.SetSelfAutoRun();

            Loger.Log($"GMBoss {ClientInfo.Version} start.");

            if (Process.GetProcessesByName("gmboss").Length > 1)
            {
                Loger.Log("Duplicated process. Exit.");
                Application.Exit(); Environment.Exit(0);
            }

            string ret = null;
            for (var i = 0; i < 3 && string.IsNullOrEmpty(ret); i++)
            {
                Loger.Log($"load version.txt count {i}.");
#if DEBUG
                ret = HttpClient.Get("http://beta.wbmgmt.youzijie.com/boss/checkClient");
#else
                ret = HttpClient.Get("http://wbmgmt.youzijie.com/boss/checkClient");
#endif
            }

            if (!string.IsNullOrEmpty(ret))
            {
                var rsp = CheckClientResp.fromXML(ret);

                if (rsp != null)
                {
                    if (rsp.latesVersion > ClientInfo.Version
                        || rsp.forceUpdate)
                    {
                        var exePath = Application.ExecutablePath;

                        Loger.Log($"from {ClientInfo.Version} update to {rsp.latesVersion}");
                        Process.Start(Application.StartupPath + @"\update.exe",
                            $"{exePath.Substring(exePath.LastIndexOf("\\") + 1)} {rsp.latesVersionUrl}");
                        Application.Exit(); Environment.Exit(0);
                    }
                    else if (!rsp.needLogin)
                    {
                        Application.OpenForms["LoginForm"].Hide();
                        MainForm.NewShow();
                    }
                }
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Loger.Log(e.ExceptionObject as Exception, $"Domain:{e.IsTerminating}");
        }

        private static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Loger.Log(e.Exception, "Thread");
        }
    }
}