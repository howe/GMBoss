using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace GMBoss.Utils
{
    class DownloadThread
    {
        volatile bool m_IsComplete;
        volatile int m_RestartCount;

        public bool IsComplete
        {
            get { return m_IsComplete; }
        }

        public int Offset
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        public int TotalBytes
        {
            get;
            set;
        }

        public byte[] Buffer
        {
            get;
            set;
        }

        public DownloadTask Task
        {
            get;
            set;
        }

        public void Download()
        {
            m_IsComplete = false;
            Position = 0;
            ThreadPool.QueueUserWorkItem(Saveing);
        }

        private void Saveing(object state)
        {
            HttpWebResponse response = null;
            var totalBytes = 0L;
            var request = WebRequest.Create(Task.Url) as HttpWebRequest;

            request.Method = "GET";
            request.AddRange(Offset, Offset + TotalBytes);

            try
            {
                response = request.GetResponse() as HttpWebResponse;
                var count = 0;

                using (var stream = response.GetResponseStream())
                {
                    while ((count = stream.Read(Buffer, 0, Buffer.Length)) > 0
                        && !Task.IsFailTask)
                    {
                        lock (Task.FileStream)
                        {
                            Task.FileStream.Seek(Offset + Position, SeekOrigin.Begin);
                            Task.FileStream.Write(Buffer, 0, count);
                            Position += count;
                        }

                        totalBytes += count;
                        Task.AddBytes(count);
                    }

                    m_IsComplete = true;
                }

                Task.Refresh();
            }
            catch
            {
                Task.AddBytes(-(int)totalBytes);                //将下载字节数复原
                totalBytes = 0;

                if (m_RestartCount++ < 5)                       //最多重试5次
                {
                    Thread.Sleep(6000);                         //等待6秒
                    this.Download();                            //重试
                }
                else
                {
                    Task.Refresh(true);                         //触发Error事件
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }
    }

    class DownloadTask
    {
        public event EventHandler Complete;
        public event EventHandler Progress;
        public event EventHandler Error;
        volatile bool m_IsFailTask;
        long m_DownloadTotal;

        public bool IsFailTask
        {
            get { return m_IsFailTask; }
        }

        public bool IsComplete
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public long FileSize
        {
            get;
            set;
        }

        public string SavePath
        {
            get;
            set;
        }

        public string TempPath
        {
            get;
            set;
        }

        public Stream FileStream
        {
            get;
            set;
        }

        public DownloadThread[] Downloads
        {
            get;
            set;
        }

        public long DownloadTotal
        {
            get { return m_DownloadTotal; }
        }

        public int DownloadRate
        {
            get { return (int)((double)DownloadTotal / FileSize * 100); }
        }

        public object Tag
        {
            get;
            set;
        }

        public void AddBytes(int count)
        {
            Interlocked.Add(ref m_DownloadTotal, count);
            if (Progress != null) Progress(this, new EventArgs());
        }

        public void Begin()
        {
            var splitSize = 1048576;
            var dir = Directory.GetParent(TempPath).FullName;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            FileStream = File.Create(TempPath);

            if (FileSize > splitSize)
            {
                var tCount = FileSize / splitSize;

                if (tCount > 5)
                    tCount = 5;

                Downloads = new DownloadThread[tCount];
                for (var i = 0; i < Downloads.Length; i++)
                {
                    Downloads[i] = new DownloadThread()
                    {
                        Offset = (int)((FileSize / tCount) * i),
                        TotalBytes = (int)((i != tCount - 1) ? (FileSize / tCount) : FileSize - (FileSize / tCount) * i),
                        Buffer = new byte[8192],
                        Task = this,
                    };
                }

                for (var i = 0; i < Downloads.Length; i++)
                {
                    Downloads[i].Download();
                }
            }
            else
            {
                var t = new DownloadThread()
                {
                    Offset = 0,
                    TotalBytes = (int)FileSize,
                    Buffer = new byte[8192],
                    Task = this
                };

                Downloads = new DownloadThread[1] { t };
                t.Download();
            }
        }

        public void Refresh(bool isError = false)
        {
            if (isError)                        //触发下载错误
            {
                m_IsFailTask = true;
                if (Error != null) Error(this, new EventArgs());
            }
            else
            {
                lock (this)
                {
                    if (IsComplete) return;

                    foreach (var t in Downloads)
                    {
                        if (!t.IsComplete) return;
                    }

                    IsComplete = true;
                    FileStream.Close();

                    if (File.Exists(SavePath))
                        File.Delete(SavePath);

                    File.Move(TempPath, SavePath);
                    if (Complete != null) Complete(this, new EventArgs());
                }
            }
        }
    }

    class Downloader
    {
        Queue<DownloadTask> m_Tasks = new Queue<DownloadTask>(16);
        int m_TempCount;                        //下载文件总数
        public event EventHandler Complete;
        public event EventHandler Progress;

        public int DownloadRate
        {
            get { return (int)((m_TempCount - m_Tasks.Count) / (double)m_TempCount * 100); }
        }

        public int TaskCount
        {
            get { return m_Tasks.Count; }
        }

        public bool IsAbort
        {
            get;
            set;
        }

        public static long GetFileSize(string url)
        {
            HttpWebResponse response = null;
            long result = -1L;
            var request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = "GET";

            try
            {
                response = request.GetResponse() as HttpWebResponse;
                result = response.ContentLength;
            }
            catch (Exception)
            {
                result = -1L;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }

        public static DownloadTask GetTask(string url, string savePath, long size = -1)
        {
            if (size == -1)
                size = GetFileSize(url);

            if (size == -1)
                throw new Exception("get file size fail!");

            return new DownloadTask()
            {
                Url = url,
                FileSize = size,
                SavePath = savePath,
                TempPath = savePath + ".tmp",
            };
        }

        public DownloadTask Enqueue(DownloadTask task)
        {
            task.Complete += Task_Complete;
            task.Error += Task_Error;
            m_Tasks.Enqueue(task);
            return task;
        }

        public DownloadTask Enqueue(string url, string savePath, long size = -1)
        {
            return this.Enqueue(GetTask(url, savePath, size));
        }

        public void Download()
        {
            m_Tasks.Peek().Begin();
            m_TempCount = m_Tasks.Count;
        }

        private void Task_Complete(object sender, EventArgs e)
        {
            var task = m_Tasks.Dequeue();

            if (Progress != null)
                Progress(task, new EventArgs());

            task.Complete -= Task_Complete;
            task.Error -= Task_Error;

            if (m_Tasks.Count == 0)                 //下载完成
            {
                if (Complete != null)
                    Complete(this, new EventArgs());
            }
            else
            {
                this.Download();
            }
        }

        private void Task_Error(object sender, EventArgs e)
        {
            var task = m_Tasks.Dequeue();

            task.Complete -= Task_Complete;
            task.Error -= Task_Error;
            m_TempCount--;

            if (Progress != null)
                Progress(task, new EventArgs());

            if (m_Tasks.Count > 0 && !IsAbort)
            {
                this.Download();
            }
        }
    }
}