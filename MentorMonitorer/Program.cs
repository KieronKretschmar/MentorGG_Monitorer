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
            
            List<string> ProcessesToCheck = new List<string>() {
                "DemoAnalyzer",
                "DemoDownloaderUnzipper",
                "python",
                "SteamInfoGatherer",
                "FaceItMatchGatherer",
                "SteamworksConnection"
            };

            while (true)
            {

                var processRunningReport = CheckProcessesAreRunning(ProcessesToCheck);

                if (processRunningReport.SendReport)
                {
                    Log.WriteLine("At least one process is not running. Sending report via whatsapp.");
                    Log.Write(processRunningReport.Message);
                    MailMessager.SendMail("MentorMonitorer Process Not Running Report", processRunningReport.Message, "k.kretschmar@hotmail.de");
                    //WhatsappMessager.SendWhatsAppMessage(processRunningReport.Message);
                }
                else
                {
                    Log.WriteLine("All Processes are running.");

                }


                var activityReport = CheckActivity();

                if (activityReport.SendReport)
                {
                    Log.WriteLine("At least one check failed. Sending report via email.");
                    Log.Write(activityReport.Message);
                    MailMessager.SendMail("MentorMonitorer Report", activityReport.Message, "k.kretschmar@hotmail.de");
                }
                else
                {
                    Log.WriteLine("All Activity checks passed.");
                }

                // Sleep 15 minutes
                Log.WriteLine("Sleeping for 10 minutes.");
                Thread.Sleep(10 * 60 * 1000);
            }

        }

        public static Report CheckActivity()
        {
            var report = new Report();
            // Diagnostics to be sent via mail
            var message = "Report: \n";
            var sendReport = false;

            // DemoServer
            // DemoServer Activity
            var LastNewMatchmakingDemo = ActivityChecker.LastNewMatchmakingDemo();
            if (DateTime.Now - LastNewMatchmakingDemo > TimeSpan.FromHours(6))
            {
                sendReport = true;
                message += "There has not been a single new matchmaking match in the database since " + LastNewMatchmakingDemo.ToString() + ".\n";
            }


            // DemoDownloader
            // DemoDownloader Activity
            var matchesWaitingForDemoDownloader = ActivityChecker.MatchesWaitingForDemoDownloader();
            if (matchesWaitingForDemoDownloader > 5)
            {
                sendReport = true;
                message += "There are " + matchesWaitingForDemoDownloader + " waiting to be analyzed by DemoDownloader.\n";
            }

            // DemoDownloader Functionality
            var demoDownloaderFailQuota = ActivityChecker.DemoDownloaderFailQuota(20);
            if (demoDownloaderFailQuota > 0.3)
            {
                sendReport = true;
                message += "Of the last " + 20 + " matches, DemoAnalyzer or DemoDownloader failed " + demoDownloaderFailQuota * 100 + "%.\n";
            }


            // FaceitMatchGatherer 
            // FaceitMatchGatherer Activity
            var LastFaceitCheck = ActivityChecker.LastFaceitCheck();
            if (DateTime.Now - LastFaceitCheck > TimeSpan.FromHours(1))
            {
                sendReport = true;
                message += "There has not been a single check for new faceit matches since " + LastFaceitCheck.ToString() + ".\n";
            }

            // FaceitMatchGatherer Functionality
            var LastNewFaceitDemo = ActivityChecker.LastNewFaceitDemo();
            if (DateTime.Now - LastNewFaceitDemo > TimeSpan.FromHours(24))
            {
                sendReport = true;
                message += "There has not been a single new faceit match in the database since " + LastNewFaceitDemo.ToString() + ".\n";
            }


            // DemoAnalyzer
            // DemoAnalyzer Activity
            var matchesWaitingForDemoAnalyzer = ActivityChecker.MatchesWaitingForDemoAnalyzer();
            if (matchesWaitingForDemoAnalyzer > 5)
            {
                sendReport = true;
                message += "There are " + matchesWaitingForDemoAnalyzer + " waiting to be analyzed by DemoAnalyzer.\n";
            }


            // PyAnalyzer                
            // PyAnalyzer Activity
            var matchesWaitingForPyAnalyzer = ActivityChecker.MatchesWaitingForPyAnalyzer();
            if (matchesWaitingForPyAnalyzer > 5)
            {
                sendReport = true;
                message += "There are " + matchesWaitingForPyAnalyzer + " waiting to be analyzed by PyAnalyzer.\n";
            }
            // PyAnalyzer Functionality
            var pyAnalyzerFailQuota = ActivityChecker.PyAnalyzerFailQuota(20);
            if (pyAnalyzerFailQuota > 0.2)
            {
                sendReport = true;
                message += "Of the last " + 20 + " matches, PyAnalyzer failed " + pyAnalyzerFailQuota * 100 + "%.\n";
            }

            return new Report
            {
                SendReport = sendReport,
                Message = message,
            };
        }
            
        public static Report CheckProcessesAreRunning(List<string> processesToCheck)
        {
            Log.WriteLine("Checking module activity.");

            // Diagnostics to be sent via mobile
            var message = "Report: \n";
            var sendReport = false;

            foreach (var processName in processesToCheck)
            {
                if (!ActivityChecker.ProcessIsRunning(processName))
                {
                    sendReport = true;
                    message += processName + " is not Running.\n";
                }
            }

            return new Report
            {
                SendReport = sendReport,
                Message = message,
            };
        }

        public struct Report
        {
            public bool SendReport { get; set; }
            public string Message { get; set; }
        }
    }
}
