using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMonitorer
{
    /// <summary>
    /// Set Twilio data as environment variable, 
    /// see https://www.twilio.com/docs/usage/secure-credentials and https://www.twilio.com/blog/2017/01/how-to-set-environment-variables.html
    /// </summary>
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
                    .Where(x=>(x.MatchStats.Select(y=>y.Source).SingleOrDefault() ?? "") == "Valve")
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

        public static DateTime DemoAnalyzerLastActivity()
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                return dbContext.DemoStats
                    .Where(x => x.Status == 3 || x.Status == 4)
                    .Select(x => x.MatchDate)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
            }
        }

        public static DateTime PyAnalyzerLastActivity()
        {
            using (DemoAnalyzerDataClassesDataContext dbContext
                = new DemoAnalyzerDataClassesDataContext())
            {
                return dbContext.DemoStats
                    .Where(x => x.Status == 5 || x.Status == 6)
                    .Select(x => x.MatchDate)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
            }
        }

    }
}
