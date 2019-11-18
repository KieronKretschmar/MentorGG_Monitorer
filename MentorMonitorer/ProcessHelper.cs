using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMonitorer
{
    public static class ProcessHelper
    {
        public static void RestartProcess(string name, string executablePath)
        {
            // Stop process if running
            var process = Process.GetProcessesByName(name).FirstOrDefault();
            if(process != null)
            {
                process.Kill();
            }

            // Start process
            var startInfo = new ProcessStartInfo();

            startInfo.WorkingDirectory = Directory.GetParent(executablePath).ToString();
            startInfo.FileName = Path.GetFileName(executablePath);
            Process proc = Process.Start(startInfo);
        }


        public static bool ProcessIsRunning(string processName)
        {
            return Process.GetProcesses().Any(x => x.ProcessName == processName);
        }
    }
}
