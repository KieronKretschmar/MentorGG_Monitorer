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
            while (true)
            {
                Log.WriteLine("Checking module activity.");


                var warningMsg = "Report: \n";
                var sendWarning = false;

                // DemoServer
                // DemoServer Activity
                var LastNewMatchmakingDemo = ActivityChecker.LastNewMatchmakingDemo();
                if (DateTime.Now - LastNewMatchmakingDemo > TimeSpan.FromHours(6))
                {
                    sendWarning = true;
                    warningMsg += "There has not been a single new matchmaking match in the database since " + LastNewMatchmakingDemo.ToString() + ".\n";
                }


                // DemoDownloader
                // DemoDownloader Activity
                var matchesWaitingForDemoDownloader = ActivityChecker.MatchesWaitingForDemoDownloader();
                if (matchesWaitingForDemoDownloader > 5)
                {
                    sendWarning = true;
                    warningMsg += "There are " + matchesWaitingForDemoDownloader + " waiting to be analyzed by DemoDownloader.\n";
                }

                // DemoDownloader Functionality
                var demoDownloaderFailQuota = ActivityChecker.DemoDownloaderFailQuota(50);
                if (demoDownloaderFailQuota > 0.3)
                {
                    sendWarning = true;
                    warningMsg += "Of the last " + 20 + " matches, DemoDownloader failed " + demoDownloaderFailQuota * 100 + "%.\n";
                }


                // FaceitMatchGatherer 
                // FaceitMatchGatherer Activity
                var LastFaceitCheck = ActivityChecker.LastFaceitCheck();
                if (DateTime.Now - LastFaceitCheck > TimeSpan.FromHours(1))
                {
                    sendWarning = true;
                    warningMsg += "There has not been a single check for new faceit matches since " + LastFaceitCheck.ToString() + ".\n";
                }

                // FaceitMatchGatherer Functionality
                var LastNewFaceitDemo = ActivityChecker.LastNewFaceitDemo();
                if (DateTime.Now - LastNewFaceitDemo > TimeSpan.FromHours(24))
                {
                    sendWarning = true;
                    warningMsg += "There has not been a single new faceit match in the database since " + LastNewFaceitDemo.ToString() + ".\n";
                }


                // DemoAnalyzer
                // DemoAnalyzer Activity
                var matchesWaitingForDemoAnalyzer = ActivityChecker.MatchesWaitingForDemoAnalyzer();
                if (matchesWaitingForDemoAnalyzer > 5)
                {
                    sendWarning = true;
                    warningMsg += "There are " + matchesWaitingForDemoAnalyzer + " waiting to be analyzed by DemoAnalyzer.\n";
                }

                // DemoAnalyzer Functionality
                var demoAnalyzerFailQuota = ActivityChecker.DemoAnalyzerFailQuota(20);
                if (demoAnalyzerFailQuota > 0.2)
                {
                    sendWarning = true;
                    warningMsg += "Of the last " + 20 + " matches, DemoAnalyzer failed " + demoAnalyzerFailQuota * 100 + "%.\n";
                }


                // PyAnalyzer                
                // PyAnalyzer Activity
                var matchesWaitingForPyAnalyzer = ActivityChecker.MatchesWaitingForPyAnalyzer();
                if (matchesWaitingForPyAnalyzer > 5)
                {
                    sendWarning = true;
                    warningMsg += "There are " + matchesWaitingForPyAnalyzer + " waiting to be analyzed by PyAnalyzer.\n";
                }
                // PyAnalyzer Functionality
                var pyAnalyzerFailQuota = ActivityChecker.PyAnalyzerFailQuota(20);
                if (pyAnalyzerFailQuota > 0.2)
                {
                    sendWarning = true;
                    warningMsg += "Of the last " + 20 + " matches, PyAnalyzer failed " + pyAnalyzerFailQuota * 100 + "%.\n";
                }




                if (sendWarning)
                {
                    Log.WriteLine("At least one check failed. Sending report via email.");
                    Log.Write(warningMsg);
                    Messager.SendMail("MentorMonitorer Report", warningMsg, "k.kretschmar@hotmail.de");
                }
                else
                {
                    Log.WriteLine("All checks passed.");

                }

                // Sleep 30 minutes
                Log.WriteLine("Sleeping for 30 minutes.");
                Thread.Sleep(30 * 60 * 1000);
            }

        }
    }
}
