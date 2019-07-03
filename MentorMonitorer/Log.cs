//////////////////////////
/// 
/// Copyright 2019, Johannes Gocke
/// johannes_gocke@hotmail.de
/// 
/// You must not use this code for anything without consent by Johannes Gocke
/// 
//////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMonitorer
{
    static class Log
    {
        public static void Write(string message)
        {
            Console.Write("\r" + DateTime.Now + ": " + message);
        }

        public static void WriteLine(long message)
        {
            WriteLine(message.ToString());
        }

        public static void WriteLine(string message)
        {
            Console.WriteLine("\r" + DateTime.Now + ": " + message);
        }
    }
}
