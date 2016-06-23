using GMBoss.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMBoss
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar.Value * 100 != MciPlayer.Volume)       //第一次不播放语音
            {
                MciPlayer.Volume = trackBar.Value * 100;
                MciPlayer.Play($@"{Application.StartupPath}\tempFiles\tip.mp3");
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            trackBar.Value = MciPlayer.Volume / 100;
        }
    }
}