using System;
using System.Collections.Generic;
using System.Text;

namespace GMBoss.Log
{
    static class Loger
    {
        static readonly string s_Split = new string('=', 256);
        static readonly IWriter s_Writer;

        static Loger()
        {
#if DEBUG
            s_Writer = new DebugWriter();
#else
            s_Writer = new FileWriter();
#endif
        }

        public static void Log(string log)
        {
            s_Writer.Log($"{DateTime.Now}_{log}");
        }

        public static void Log(Exception e, object param)
        {
            s_Writer.Log($"{s_Split}\r\n{DateTime.Now}_Message:{e.Message}\r\nException:{e.GetType().FullName} StackTrace:{e.StackTrace} Param:{param}\r\n{s_Split}");
        }
    }
}