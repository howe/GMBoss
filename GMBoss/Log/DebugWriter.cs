using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace GMBoss.Log
{
    class DebugWriter : IWriter
    {
        public void Log(string log)
        {
            Debug.WriteLine(log);
        }
    }
}