using GMBoss.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace GMBoss.Utils
{
    class ClientInfo
    {
        static readonly ClientInfo s_NowClient;

        int m_ShopID;
        string m_UserID;
        string m_Token;
        string m_Name;

        static ClientInfo()
        {
            var shopID = ConfigHelper.Get("NetBar", "ShopID");

            if (!string.IsNullOrEmpty(shopID))
            {
                s_NowClient = new ClientInfo()
                {
                    m_ShopID = int.Parse(shopID),
                    m_UserID = ConfigHelper.Get("NetBar", "UserID"),
                    m_Token = ConfigHelper.Get("NetBar", "Token"),
                    m_Name = ConfigHelper.Get("NetBar", "Name")
                };
            }
            else
            {
                s_NowClient = new ClientInfo();
            }
        }

        public int ShopID
        {
            get { return m_ShopID; }
            set
            {
                m_ShopID = value;
                ConfigHelper.Set("NetBar", "ShopID", value.ToString());
            }
        }

        public string UserID
        {
            get { return m_UserID; }
            set
            {
                m_UserID = value;
                ConfigHelper.Set("NetBar", "UserID", value);
            }
        }

        public string Token
        {
            get { return m_Token; }
            set
            {
                m_Token = value;
                ConfigHelper.Set("NetBar", "Token", value);
            }
        }

        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                ConfigHelper.Set("NetBar", "Name", value);
            }
        }

        public static ClientInfo NowClient
        {
            get { return s_NowClient; }
        }

        public static int Version
        {
            get { return 203; }
        }
    }

    static class HostInfo
    {
        public static string GetHostName()
        {
            return Environment.MachineName;
        }

        public static string GetMac()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (interfaces != null && interfaces.Length > 0)
                return interfaces[0].GetPhysicalAddress()
                    .ToString().Replace(":", "");
            else
                return "unknownmac";
        }

        public static string GetCpuID()
        {
            try
            {
                var mc = new ManagementClass("Win32_Processor");
                var moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    return mo.Properties["ProcessorId"].Value.ToString();
                }
            }
            catch (Exception ex)
            {

                Loger.Log("GetCpuID Error."+ex.Message);
            }
            
            return "unknowncpu";
        }

        public static string GetOsName()
        {
            if (IsWin81()) return "Win8.1";

            if (IsWin10()) return "Win10";

            var version = Environment.OSVersion.Version;

            if (version.Major == 6)
            {
                switch (version.Minor)
                {
                    case 0:
                        return "WinVista";

                    case 1:
                        return "Win7";

                    case 2:
                        return "Win8";
                }
            }
            else if (version.Major == 5)
            {
                switch (version.Minor)
                {
                    case 0:
                        return "Win2000";

                    case 1:
                        return "WinXP";

                    case 2:
                        return "Win2003";
                }
            }
            return "unknown";
        }

        public static bool IsWin81()
        {
            var product = (string)RegHelper.Get(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                "ProductName");

            return product.IndexOf("8.1") != -1;
        }

        public static bool IsWin10()
        {
            var product = (string)RegHelper.Get(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                "ProductName");

            return product.IndexOf("10") != -1;
        }
    }
}