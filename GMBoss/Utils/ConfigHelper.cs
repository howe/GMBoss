using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GMBoss.Utils
{
    static class ConfigHelper
    {
        static readonly string IniPath = $"{Application.StartupPath}\\Config.ini";

        [DllImport("Kernel32.dll")]
        static extern uint GetPrivateProfileString(string app, string key, string defStr,
            StringBuilder retVal, uint size, string path);

        [DllImport("Kernel32.dll")]
        static extern uint WritePrivateProfileString(string app, string key, string value, string path);

        public static string Get(string app, string key)
        {
            var sb = new StringBuilder(1024);

            GetPrivateProfileString(app, key, null, sb, (uint)sb.Capacity, IniPath);
            return sb.ToString();
        }

        public static void Set(string app, string key, string value)
        {
            WritePrivateProfileString(app, key, value, IniPath);
        }
    }
}