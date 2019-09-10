using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMonitorer
{
    static class ActivityChecker
    {
        public enum DemoStatus
        {
            NEW = 0,
            //DOWNLOADED = 1,
            DownloadFinished = 2,
            DemoAnalyzerFinished = 3,
            DemoDownloaderOrAnalyzerFailed = 4,
            PyAnalyzerFinished = 5,
            PyAnalyzerFailed = 6,
        }


        public static DateTime LastNewMatchmakingDemo()
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {               
                return dbContext.DemoStats
                    .Where(x=> x.Source == "Valve")
                    .Select(x => x.MatchDate)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
            }
        }

        public static DateTime LastNewFaceitDemo()
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                return dbContext.DemoStats
                    .Where(x => (x.MatchStats.Select(y => y.Source).SingleOrDefault() ?? "") == "Faceit")
                    .Select(x => x.MatchDate)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
            }
        }

        public static DateTime LastFaceitCheck()
        {
            using (UserDataDataClassesDataContext dbContext
                = new UserDataDataClassesDataContext())
            {
                return dbContext.AspNetUsers
                    .Where(x=>x.FaceItLastCheck != null)
                    .Select(x => (DateTime) x.FaceItLastCheck)
                    .OrderByDescending(x => x)
                    .First();
            }
        }

        public static float DemoDownloaderOrAnalyzerFailQuota(int recentMatches)
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                var statusList = dbContext.DemoStats
                    .OrderByDescending(x => x.MatchDate)
                    .Select(x => x.Status)
                    .Take(recentMatches)
                    .ToList();

                return statusList.Count(x => x == (short) DemoStatus.DemoDownloaderOrAnalyzerFailed) / statusList.Count;
            }
        }

        public static float PyAnalyzerFailQuota(int recentMatches)
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                var statusList = dbContext.DemoStats
                    .OrderByDescending(x => x.MatchDate)
                    .Select(x => x.Status)
                    .Take(recentMatches)
                    .ToList();

                return statusList.Count(x => x == (short)DemoStatus.PyAnalyzerFailed) / statusList.Count;
            }
        }

        public static int MatchesWaitingForDemoDownloader()
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                return dbContext.DemoStats
                    .Where(x => x.Status == 1)
                    .Count();
            }
        }

        public static int MatchesWaitingForDemoAnalyzer()
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                return dbContext.DemoStats
                    .Where(x => x.Status == 2)
                    .Count();
            }
        }

        public static int MatchesWaitingForPyAnalyzer()
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                return dbContext.DemoStats
                    .Where(x => x.Status == 3)
                    .Count();
            }
        }

        public static bool ProcessIsRunning(string processName)
        {
            return Process.GetProcesses().Any(x => x.ProcessName == processName);
        }

    }
}
