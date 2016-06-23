using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GMBoss.Log
{
    class FileWriter : IWriter
    {
        readonly string LogDir = $"{Application.StartupPath}\\Log\\";
        readonly StreamWriter m_Writer;

        public FileWriter()
        {
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
            m_Writer = new StreamWriter($"{LogDir}{DateTime.Now.Ticks}.txt", true);
        }

        public void Log(string log)
        {
            lock (m_Writer)
            {
                m_Writer.WriteLine(log);
                m_Writer.Flush();
            }
        }
    }
}