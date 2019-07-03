﻿using System;
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


                // DemoServer Activity
                var LastNewMatchmakingDemo = ActivityChecker.LastNewMatchmakingDemo();
                if (DateTime.Now - LastNewMatchmakingDemo > TimeSpan.FromHours(4))
                {
                    sendWarning = true;
                    warningMsg += "There has not been a single new matchmaking match in the database since " + LastNewMatchmakingDemo.ToString() + ".\n";
                }

                // FaceitMatchGatherer Activity
                var LastNewFaceitDemo = ActivityChecker.LastNewFaceitDemo();
                if (DateTime.Now - LastNewFaceitDemo > TimeSpan.FromHours(4))
                {
                    sendWarning = true;
                    warningMsg += "There has not been a single new faceit match in the database since " + LastNewFaceitDemo.ToString() + ".\n";
                }

                // DemoAnalyzer Activity
                var matchesWaitingForDemoAnalyzer = ActivityChecker.MatchesWaitingForDemoAnalyzer();
                if (matchesWaitingForDemoAnalyzer > 5)
                {
                    sendWarning = true;
                    warningMsg += "There are " + matchesWaitingForDemoAnalyzer + " waiting to be analyzed by DemoAnalyzer.\n";
                }

                // DemoAnalyzer Functionality
                var demoDownloaderOrAnalyzerFailQuota = ActivityChecker.DemoDownloaderOrAnalyzerFailQuota(20);
                if (demoDownloaderOrAnalyzerFailQuota > 0.2)
                {
                    sendWarning = true;
                    warningMsg += "Of the last " + 20 + " matches, DemoAnalyzer or DemoDownloader failed " + demoDownloaderOrAnalyzerFailQuota * 100 + "%.\n";
                }

                // DemoAnalyzer Activity
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
                    Log.WriteLine("At least one check failed. Sending report via whatsapp.");
                    Log.Write(warningMsg);
                    WhatsappMessager.SendWhatsAppMessage(warningMsg);
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