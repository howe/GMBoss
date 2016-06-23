using System;
using System.Collections.Generic;
using System.Text;

namespace GMBoss.Log
{
    interface IWriter
    {
        void Log(string log);
    }
}