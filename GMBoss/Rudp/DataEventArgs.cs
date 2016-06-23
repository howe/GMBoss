using System;

namespace GMBoss.Rudp
{
    public class DataEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}