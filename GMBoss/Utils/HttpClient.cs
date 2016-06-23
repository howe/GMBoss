using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GMBoss.Utils
{
    class Query : Dictionary<string, string>
    {
        public Query Go(string key, string value)
        {
            this.Add(key, value);
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(256);

            sb.Append('?');
            foreach (var kp in this)
            {
                sb.Append(kp.Key).Append('=')
                    .Append(kp.Value)
                    .Append('&');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }

    class SortQuery : SortedDictionary<string, string>
    {
        public SortQuery Go(string key, string value)
        {
            this.Add(key, value);
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(256);

            sb.Append('?');
            foreach (var kp in this)
            {
                sb.Append(kp.Key).Append('=')
                    .Append(kp.Value)
                    .Append('&');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }

    static class HttpClient
    {
        static readonly string hostName = HostInfo.GetHostName();
        static readonly string uuid = HostInfo.GetCpuID();
        static readonly string mac = HostInfo.GetMac();
        static readonly string netVersion = Environment.Version.ToString();
        static readonly string osName = HostInfo.GetOsName();

        static HttpClient()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 512;
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
        }

        static bool RemoteCertificateValidationCallback(
            Object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        static HttpWebRequest GetRequest(string url)
        {
            var result = WebRequest.Create(url) as HttpWebRequest;

            result.Headers["shopId"] = ClientInfo.NowClient.ShopID.ToString();
            result.Headers["userId"] = ClientInfo.NowClient.UserID;
            result.Headers["token"] = ClientInfo.NowClient.Token;

            result.Headers["hostName"] = hostName;
            result.Headers["uuid"] = uuid;
            result.Headers["mac"] = mac;

            result.Headers["cv"] = ClientInfo.Version.ToString();
            result.Headers["netversion"] = netVersion;
            result.Headers["osversion"] = osName;

            result.ContentType = "application/text;";
            //result.KeepAlive = false;                 //减少握手
            return result;
        }

        public static string Get(string url, string query = null)
        {
            HttpWebResponse response = null;
            string result = null;
            var request = GetRequest(url + query);

            request.Method = "GET";

            try
            {
                response = request.GetResponse() as HttpWebResponse;
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }

        public static string Get(string url, Query query)
        {
            return Get(url, query.ToString());
        }

        public static string Get(string url, SortQuery query)
        {
            return Get(url, query.ToString());
        }

        public static string Post(string url, string body)
        {
            HttpWebResponse response = null;
            string result = null;
            var buffer = Encoding.UTF8.GetBytes(body);
            var request = GetRequest(url);

            request.Method = "POST";

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(buffer, 0, buffer.Length);
                }

                response = request.GetResponse() as HttpWebResponse;
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }

        public static string Post(string url, Query query)
        {
            return Post(url, query.ToString());
        }

        public static string Post(string url, SortQuery query)
        {
            return Post(url, query.ToString());
        }
    }
}