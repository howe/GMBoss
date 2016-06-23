using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GMBoss.Rudp
{
    public class HelpUtils
    {
        readonly static Dictionary<int, RudpClient> s_Clients = new Dictionary<int, RudpClient>(512);
        readonly static long ExpireSecond = 60 * 60 * 10000;                    //转换为Tick的单位
        readonly static object s_SyncRoot = new object();

        public static bool TryGetClient(IPEndPoint endPoint, out RudpClient client)
        {
            lock (s_SyncRoot)
            {
                var hashCode = endPoint.GetHashCode();

                if (s_Clients.ContainsKey(hashCode))
                {
                    client = s_Clients[hashCode];
                    return true;
                }

                client = null;
                return false;
            }
        }

        public static void AddClient(RudpClient client)
        {
            lock (s_SyncRoot)
            {
                s_Clients.Add(client.RemoteEP.GetHashCode(), client);
            }
        }

        public static void ClearExpire()
        {
            var rmKeys = new List<int>(16);

            lock (s_SyncRoot)
            {
                foreach (var kp in s_Clients)
                {
                    if (DateTime.Now.Ticks - kp.Value.CreateTick > ExpireSecond)
                    {
                        rmKeys.Add(kp.Key);
                    }
                }

                foreach (var r in rmKeys)
                {
                    s_Clients.Remove(r);
                }
            }
            rmKeys.Clear();
        }

        public static int GetUsePort(int startPort)
        {
            var ipGp = IPGlobalProperties.GetIPGlobalProperties();
            var endPoints = ipGp.GetActiveUdpListeners();
            bool isUsed;

            while (startPort < IPEndPoint.MaxPort)
            {
                isUsed = false;

                foreach (var ep in endPoints)
                {
                    if (ep.Port == startPort)
                    {
                        isUsed = true;
                        break;
                    }
                }

                if (isUsed)
                    startPort++;
                else
                    break;
            }
            return startPort;
        }

        public static string[] GetIP()
        {
            var adds = Dns.GetHostAddresses(Dns.GetHostName());
            var ips = new List<string>(8);

            foreach (var add in adds)
            {
                if (add.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(add.ToString());
                }
            }

            return ips.ToArray();
        }
    }
}