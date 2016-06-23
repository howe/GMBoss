using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GMBoss.Rudp
{
    public class RudpClient : IDisposable
    {
        public event EventHandler<DataEventArgs> Data;      //接受数据事件

        SortedDictionary<int, Msg> m_RMsg;
        SortedDictionary<int, Msg> m_SMsg;
        object m_SyncRoot = new object();
        UdpClient m_Client;
        int m_RID; int m_SID;
        IPEndPoint m_RemoteEP;

        public IPEndPoint RemoteEP
        {
            get { return m_RemoteEP; }
        }

        internal long CreateTick
        {
            get;
            set;
        }

        public RudpClient GetRemote(string host, int port)
        {
            return GetRemote(new IPEndPoint(IPAddress.Parse(host), port));
        }

        public RudpClient GetRemote(IPEndPoint endPoint)
        {
            RudpClient client = null;

            if (!HelpUtils.TryGetClient(endPoint, out client))
            {
                client = new RudpClient()
                {
                    CreateTick = DateTime.Now.Ticks,
                    m_RMsg = new SortedDictionary<int, Msg>(),
                    m_SMsg = new SortedDictionary<int, Msg>(),
                    m_RemoteEP = endPoint,
                    m_Client = m_Client,
                    m_RID = 1,
                    m_SID = 1
                };

                HelpUtils.AddClient(client);
            }
            return client;
        }

        public void Bind(IPEndPoint endPoint)
        {
            if (isDisposed) throw new ObjectDisposedException(this.GetType().FullName);

            m_Client = new UdpClient(endPoint);
            BeginReceive();
        }

        public void Bind(string host, int port)
        {
            if (isDisposed) throw new ObjectDisposedException(this.GetType().FullName);

            Bind(new IPEndPoint(IPAddress.Parse(host), port));
        }

        public void Close()
        {
            if (isDisposed) throw new ObjectDisposedException(this.GetType().FullName);

            if (m_RemoteEP == null)
            {
                m_Client.Close();
                Data = null;
            }
            else
            {
                m_RMsg.Clear();
                m_SMsg.Clear();
            }
        }

        public void Reset()
        {
            if (m_RemoteEP != null)
            {
                lock (m_SyncRoot)
                {
                    m_RMsg.Clear();
                    m_SMsg.Clear();
                    m_RID = 1; m_SID = 1;
                }
            }
        }

        public void Send(byte[] data)
        {
            if (isDisposed) throw new ObjectDisposedException(this.GetType().FullName);
            if (m_RemoteEP == null) throw new Exception("you need use GetRemote send to.");
            if (data.Length > 510) throw new Exception("data max length is 510.");

            var send = new byte[2 + data.Length];

            lock (m_SyncRoot)
            {
                var id = m_SID;

                send[0] = (byte)(id & 0x000000FF);
                send[1] = (byte)(id >> 8);

                m_SMsg.Add(id, new Msg()
                {
                    ID = id,
                    Data = send
                });

                m_SID = (id == short.MaxValue ? 1 : id + 1);
            }

            Buffer.BlockCopy(data, 0, send, 2, data.Length);
            BeginSend(send);
        }

        public void SendReset()
        {
            BeginSend(BitConverter.GetBytes((1 << 16) + 1));
        }

        void Confirm(short id)
        {
            BeginSend(BitConverter.GetBytes(id));
        }

        void BeginSend(byte[] data)
        {
            try
            {
                m_Client.BeginSend(data, data.Length,
                    m_RemoteEP, EndSend, null);
            }
            catch (Exception e)
            {
                //
            }
        }

        void EndSend(IAsyncResult ar)
        {
            try
            {
                m_Client.EndSend(ar);
            }
            catch (Exception e)
            {
                //
            }
        }

        void BeginReceive()
        {
            try
            {
                m_Client.BeginReceive(EndReceive, null);
            }
            catch (Exception e)
            {
                //
            }
        }

        void EndReceive(IAsyncResult ar)
        {
            IPEndPoint remoteEP = null;
            var isReceive = true;
            byte[] data = null;

            try
            {
                data = m_Client.EndReceive(ar, ref remoteEP);

                switch (data.Length)
                {
                    case 0:
                        isReceive = false;
                        break;

                    case 2:
                        {
                            var id = BitConverter.ToInt16(data, 0);
                            var client = GetRemote(remoteEP);

                            client.ClientConfirm(id);
                            break;
                        }

                    case 4:
                        {
                            var client = GetRemote(remoteEP);
                            var rs = BitConverter.ToInt32(data, 0);
                            var rid = rs >> 16;
                            var sid = rs & 0x0000FFFF;

                            lock (m_SyncRoot)
                            {
                                client.m_RID = rid;
                                client.m_SID = sid;
                                client.m_RMsg.Clear();
                                client.m_SMsg.Clear();
                            }
                            break;
                        }

                    default:
                        {
                            var id = BitConverter.ToInt16(data, 0);
                            var receive = new byte[data.Length - 2];
                            var client = GetRemote(remoteEP);

                            Buffer.BlockCopy(data, 2, receive, 0, receive.Length);
                            client.ClientReceive(this, id, receive);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                if (data == null) isReceive = false;
            }
            finally
            {
                if (isReceive) BeginReceive();
            }
        }

        void ClientConfirm(short id)
        {
            byte[] send = null;

            lock (m_SyncRoot)
            {
                if (m_SMsg.ContainsKey(Math.Abs(id)))
                {
                    if (id > 0)
                    {
                        m_SMsg.Remove(id);
                    }
                    else
                    {
                        send = m_SMsg[-id].Data;
                    }
                }
            }

            if (send != null) BeginSend(send);
        }

        void ClientReceive(RudpClient parent, short id, byte[] receive)
        {
            Confirm(id);                                //确认接受

            lock (m_SyncRoot)
            {
                if (id == m_RID)
                {
                    id++;

                    if (parent.Data != null)
                    {
                        parent.Data(this, new DataEventArgs() { Data = receive });
                    }

                    while (m_RMsg.ContainsKey(id))
                    {
                        var msg = m_RMsg[id++];

                        if (parent.Data != null)
                            parent.Data(this, new DataEventArgs() { Data = msg.Data });
                    }

                    m_RID = (short)(id == short.MinValue ? 1 : id);
                }
                else if (id > m_RID)                    //提前收到
                {
                    Confirm((short)-m_RID);             //要求重发

                    m_RMsg.Add(id, new Msg()
                    {
                        ID = id,
                        Data = receive,
                    });
                }
            }
        }

        #region IDisposable Support
        private bool isDisposed = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    this.Close();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                m_SMsg = null;
                m_RMsg = null;
                m_Client = null;
                m_RemoteEP = null;
                isDisposed = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~RudpClient() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}