using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Threading;

namespace MentorMonitorer
{
    class Program
    {

        static void Main(string[] args)
        {
            
            List<string> ProcessesToCheck = new List<string>() { "DemoAnalyzer", "DemoDownloaderUnzipper", "python", "SteamInfoGatherer", "FaceItMatchGatherer" };

            while (true)
            {

                CheckProcessesAreRunning(ProcessesToCheck);

                CheckActivity();

                // Sleep 30 minutes
                Log.WriteLine("Sleeping for 30 minutes.");
                Thread.Sleep(30 * 60 * 1000);
            }

        }

        public static void CheckActivity()
        {
            // Diagnostics to be sent via mail
            var emailMsg = "Report: \n";
            var sendEmailWarning = false;

            // DemoServer
            // DemoServer Activity
            var LastNewMatchmakingDemo = ActivityChecker.LastNewMatchmakingDemo();
            if (DateTime.Now - LastNewMatchmakingDemo > TimeSpan.FromHours(6))
            {
                sendEmailWarning = true;
                emailMsg += "There has not been a single new matchmaking match in the database since " + LastNewMatchmakingDemo.ToString() + ".\n";
            }


            // DemoDownloader
            // DemoDownloader Activity
            var matchesWaitingForDemoDownloader = ActivityChecker.MatchesWaitingForDemoDownloader();
            if (matchesWaitingForDemoDownloader > 5)
            {
                sendEmailWarning = true;
                emailMsg += "There are " + matchesWaitingForDemoDownloader + " waiting to be analyzed by DemoDownloader.\n";
            }


            // FaceitMatchGatherer 
            // FaceitMatchGatherer Activity
            var LastFaceitCheck = ActivityChecker.LastFaceitCheck();
            if (DateTime.Now - LastFaceitCheck > TimeSpan.FromHours(1))
            {
                sendEmailWarning = true;
                emailMsg += "There has not been a single check for new faceit matches since " + LastFaceitCheck.ToString() + ".\n";
            }

            // FaceitMatchGatherer Functionality
            var LastNewFaceitDemo = ActivityChecker.LastNewFaceitDemo();
            if (DateTime.Now - LastNewFaceitDemo > TimeSpan.FromHours(24))
            {
                sendEmailWarning = true;
                emailMsg += "There has not been a single new faceit match in the database since " + LastNewFaceitDemo.ToString() + ".\n";
            }


            // DemoAnalyzer
            // DemoAnalyzer Activity
            var matchesWaitingForDemoAnalyzer = ActivityChecker.MatchesWaitingForDemoAnalyzer();
            if (matchesWaitingForDemoAnalyzer > 5)
            {
                sendEmailWarning = true;
                emailMsg += "There are " + matchesWaitingForDemoAnalyzer + " waiting to be analyzed by DemoAnalyzer.\n";
            }

            // DemoAnalyzer Functionality
            var demoDownloaderOrAnalyzerFailQuota = ActivityChecker.DemoDownloaderOrAnalyzerFailQuota(20);
            if (demoDownloaderOrAnalyzerFailQuota > 0.2)
            {
                sendEmailWarning = true;
                emailMsg += "Of the last " + 20 + " matches, DemoAnalyzer or DemoDownloader failed " + demoDownloaderOrAnalyzerFailQuota * 100 + "%.\n";
            }


            // PyAnalyzer                
            // PyAnalyzer Activity
            var matchesWaitingForPyAnalyzer = ActivityChecker.MatchesWaitingForPyAnalyzer();
            if (matchesWaitingForPyAnalyzer > 5)
            {
                sendEmailWarning = true;
                emailMsg += "There are " + matchesWaitingForPyAnalyzer + " waiting to be analyzed by PyAnalyzer.\n";
            }
            // PyAnalyzer Functionality
            var pyAnalyzerFailQuota = ActivityChecker.PyAnalyzerFailQuota(20);
            if (pyAnalyzerFailQuota > 0.2)
            {
                sendEmailWarning = true;
                emailMsg += "Of the last " + 20 + " matches, PyAnalyzer failed " + pyAnalyzerFailQuota * 100 + "%.\n";
            }




            if (sendEmailWarning)
            {
                Log.WriteLine("At least one check failed. Sending report via email.");
                Log.Write(emailMsg);
                MailMessager.SendMail("MentorMonitorer Report", emailMsg, "k.kretschmar@hotmail.de");
            }
            else
            {
                Log.WriteLine("All checks passed.");
            }
        }
            
        public static void CheckProcessesAreRunning(List<string> processesToCheck)
        {
            Log.WriteLine("Checking module activity.");

            // Diagnostics to be sent via mobile
            var mobileMsg = "Report: \n";
            var sendMobileWarning = false;

            foreach (var processName in processesToCheck)
            {
                if (!ActivityChecker.ProcessIsRunning(processName))
                {
                    sendMobileWarning = true;
                    mobileMsg += processName + " is not Running.\n";
                }
            }


            if (sendMobileWarning)
            {
                Log.WriteLine("At least one process is not running. Sending report via whatsapp.");
                Log.Write(mobileMsg);
                WhatsappMessager.SendWhatsAppMessage(mobileMsg);
            }
            else
            {
                Log.WriteLine("All Processes are running.");

            }
        }
    }
}
