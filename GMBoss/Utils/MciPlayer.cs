using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GMBoss.Utils
{
    static class MciPlayer
    {
        static readonly string MciDevice = "Yzj_PlayDevice";
        static int s_Volume = int.Parse(ConfigHelper.Get("MCI", "Volume"));

        [DllImport("Winmm.dll", EntryPoint = "mciSendString")]
        static extern uint MciSendString(string cmd, string retStr,
            uint retLen, uint callback);

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000; //静音
        private const int APPCOMMAND_VOLUME_UP = 0x0a0000; //100
        private const int APPCOMMAND_VOLUME_DOWN = 0x090000; //0
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 播放音量
        /// </summary>
        public static int Volume
        {
            get
            {
                return s_Volume;
            }
            set
            {
                s_Volume = value;
                ConfigHelper.Set("MCI", "Volume", value.ToString());
            }
        }

        public static void Play(string path)
        {
            Close();            //播放先关闭
            MciSendString("open \"" + path + $"\" alias {MciDevice}", null, 0, 0);
            MciSendString($"setaudio {MciDevice} volume to {s_Volume}", null, 0, 0);
            MciSendString($"play {MciDevice}", null, 0, 0);
        }

        public static void Close()
        {
            MciSendString($"close {MciDevice}", null, 0, 0);
        }

        public static void SetMaxVolume(IntPtr intptr)
        {
            for (int i = 0; i < 55; i++)
            {
                SendMessageW(intptr, WM_APPCOMMAND, intptr, (IntPtr)APPCOMMAND_VOLUME_UP);
            }
        }

        public static void SetMinVolume()
        {
            for (int i = 0; i < 55; i++)
            {
                SendMessageW(IntPtr.Zero, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)APPCOMMAND_VOLUME_DOWN);
            }
        }
    }
}