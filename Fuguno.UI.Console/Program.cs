namespace Fuguno.UI.Console
{
    using Fuguno.Tfs;
    using System.Configuration;
    using System.Threading;
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            // Read config settings
            var tfsServerUri = ConfigurationManager.AppSettings["TfsServerUri"];
            var tfsCollectionName = ConfigurationManager.AppSettings["TfsCollectionName"];
            var tfsProjectName = ConfigurationManager.AppSettings["TfsProjectName"];
            var tfsRootIterationPath = ConfigurationManager.AppSettings["TfsRootIterationPath"];
            var tfsBuildDefinitonNames = ConfigurationManager.AppSettings["TfsBuildDefinitionNames"];
            var tfsWorkItemStatsWorkItemType = ConfigurationManager.AppSettings["TfsWorkItemStatsWorkItemType"];
            var tfsWorkItemStatsState = ConfigurationManager.AppSettings["TfsWorkItemStatsState"];
            var tfsWorkItemStatsAreaPaths = ConfigurationManager.AppSettings["TfsWorkItemStatsAreaPaths"];

            var areaPaths = tfsWorkItemStatsAreaPaths.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var workItemStatsService = new WorkItemStatsService(tfsServerUri, tfsCollectionName);

            // Get work item stats - active bugs by priority
            Console.WriteLine("Active Bugs by Priority");
            Console.WriteLine("=======================");
            var activeBugsByPriority = workItemStatsService.GetWorkItemCountByPriority(tfsWorkItemStatsWorkItemType, tfsWorkItemStatsState, areaPaths);
            for (var i = 0; i < activeBugsByPriority.Series[0].Data.Count; ++i)
            {
                Console.WriteLine("{0} {1}", activeBugsByPriority.Ticks[i], activeBugsByPriority.Series[0].Data[i]);
            }

            Console.WriteLine();

            // Get work item stats - active bugs by assigned to 
            Console.WriteLine("Active Bugs by Assignment");
            Console.WriteLine("=========================");
            var activeBugsByAssignedTo = workItemStatsService.GetWorkItemCountByAssignedTo(tfsWorkItemStatsWorkItemType, tfsWorkItemStatsState, areaPaths);
            for (var i = 0; i < activeBugsByAssignedTo.Ticks.Count; ++i)
            {
                Console.Write(activeBugsByAssignedTo.Ticks[i] + " ");

                foreach (var series in activeBugsByAssignedTo.Series)
                {
                    Console.Write(string.Format("{0}={1} ", series.Label, series.Data[i]));
                }

                Console.WriteLine();
            }
            
            // Get bug jail info
            var bugJailInfoService = new BugJailInfoService(tfsServerUri, tfsCollectionName);

            Console.WriteLine("Bug Jail");
            Console.WriteLine("=========================");
            var bugJailInfos = bugJailInfoService.GetBugJail(areaPaths);
            foreach (var bugJailInfo in bugJailInfos)
            {
                Console.WriteLine("{0} {1}", bugJailInfo.Name, bugJailInfo.ImageUrl);
            }

            Console.WriteLine();

            // Get iteration info - sprint and days remaining
            Console.WriteLine("Iteration info");
            Console.WriteLine("==============");
            var iterationInfoService = new IterationInfoService(tfsServerUri, tfsCollectionName, tfsProjectName, tfsRootIterationPath);
            var iterationInfo = iterationInfoService.GetCurrentIterationInfo();

            Console.WriteLine("{0} {1} {2:d} {3:d}", iterationInfo.Name, iterationInfo.Path, iterationInfo.StartDate, iterationInfo.EndDate);
            Console.WriteLine();

            // Get build infos
            Console.WriteLine("Build info");
            Console.WriteLine("==========");
            var buildInfoService = new BuildInfoService(tfsServerUri, tfsCollectionName, tfsProjectName);
            var buildDefinitonNames = tfsBuildDefinitonNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            while (true)
            {
                foreach (var buildDefinitionName in buildDefinitonNames)
                {
                    var buildInfo = buildInfoService.GetLatestBuildInfo(buildDefinitionName.Trim());
                    Console.WriteLine("{0} {1} {2} {3} {4}mins {5}",
                        buildInfo.BuildNumber,
                        buildInfo.Status,
                        buildInfo.RequestedFor,
                        buildInfo.StartTime,
                        buildInfo.ElapsedTime == null ? "-" : buildInfo.ElapsedTime.Value.Minutes.ToString(),
                        string.Format("{0:p}", ((double)buildInfo.TotalTestPassedCount / (double)buildInfo.TotalTestCount)));
                }
            }
        }
    }
}
