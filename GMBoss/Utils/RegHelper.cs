using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GMBoss.Utils
{
    static class RegHelper
    {
        static readonly string RegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static bool SetSelfAutoRun()
        {
#if !DEBUG
            try
            {
                var exePath = Application.ExecutablePath;
                var fileName = exePath.Substring(exePath.LastIndexOf(@"\") + 1);
                var regKey = Registry.LocalMachine.CreateSubKey(RegPath);

                regKey.SetValue(fileName.Substring(0, fileName.IndexOf('.')), exePath);
                regKey.Close();
                return true;
            }
            catch (Exception) { return false; }
#else
            return true;
#endif
        }

        public static object Get(string path, string key)
        {
            return Registry.GetValue(path, key, null);
        }

        public static void Set(string path, string key, string value)
        {
            Registry.SetValue(path, key, value);
        }
    }
}