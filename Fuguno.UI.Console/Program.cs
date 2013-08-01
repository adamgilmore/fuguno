namespace Fuguno.UI.Console
{
    using Fuguno.Tfs;
    using System.Configuration;
    using System.Threading;
    using System;
    using System.Collections.Generic;
    using Fuguno.Tfs.Services;

    class Program
    {
        static void Main(string[] args)
        {
            // Read config settings
            var tfsServerUri = ConfigurationManager.AppSettings["TfsServerUri"];
            var tfsCollectionName = ConfigurationManager.AppSettings["TfsCollectionName"];
            var tfsProjectName = ConfigurationManager.AppSettings["TfsProjectName"];
            var tfsTeamName = ConfigurationManager.AppSettings["TfsTeamName"];
            var tfsRootIterationPath = ConfigurationManager.AppSettings["TfsRootIterationPath"];
            var tfsBuildDefinitonNames = ConfigurationManager.AppSettings["TfsBuildDefinitionNames"];
            var tfsWorkItemStatsWorkItemType = ConfigurationManager.AppSettings["TfsWorkItemStatsWorkItemType"];
            var tfsWorkItemStatsState = ConfigurationManager.AppSettings["TfsWorkItemStatsState"];
            var userImageUrlTemplate = ConfigurationManager.AppSettings["UserImageUrlTemplate"];

            // Get build definitions
            var configService = new ConfigService(tfsServerUri, tfsCollectionName, tfsProjectName);

            Console.WriteLine("Build Definitions");
            Console.WriteLine("=================");
            var buildDefinitionNames = configService.GetBuildDefinitionNames();
            foreach (var buildDefinitionName in buildDefinitionNames)
            {
                Console.WriteLine(buildDefinitionName);
            }

            Console.WriteLine();

            // Get team names

            Console.WriteLine("Team Names");
            Console.WriteLine("==========");
            var teamNames = configService.GetTeamNames();
            foreach (var teamName in teamNames)
            {
                Console.WriteLine(teamName);
            }

            // Get bug jail info
            var bugJailInfoService = new BugJailInfoService(tfsServerUri, tfsCollectionName, tfsProjectName, tfsTeamName, userImageUrlTemplate);

            Console.WriteLine("Bug Jail");
            Console.WriteLine("========");
            var bugJailInfos = bugJailInfoService.WhoIsInJail();
            foreach (var bugJailInfo in bugJailInfos)
            {
                Console.WriteLine("{0} {1}", bugJailInfo.Name, bugJailInfo.ImageUrl);
            }

            Console.WriteLine();

            // Get work item stats - active bugs by priority
            var workItemStatsService = new WorkItemStatsService(tfsServerUri, tfsCollectionName);

            Console.WriteLine("Active Bugs by Priority");
            Console.WriteLine("=======================");
            var activeBugsByPriority = workItemStatsService.GetWorkItemCountByPriority(tfsProjectName, tfsTeamName, tfsWorkItemStatsWorkItemType, tfsWorkItemStatsState);
            for (var i = 0; i < activeBugsByPriority.Series[0].Data.Count; ++i)
            {
                Console.WriteLine("{0} {1}", activeBugsByPriority.Ticks[i], activeBugsByPriority.Series[0].Data[i]);
            }

            Console.WriteLine();

            // Get work item stats - active bugs by assigned to 
            Console.WriteLine("Active Bugs by Assignment");
            Console.WriteLine("=========================");
            var activeBugsByAssignedTo = workItemStatsService.GetWorkItemCountByAssignedTo(tfsProjectName, tfsTeamName, tfsWorkItemStatsWorkItemType, tfsWorkItemStatsState);
            for (var i = 0; i < activeBugsByAssignedTo.Ticks.Count; ++i)
            {
                Console.Write(activeBugsByAssignedTo.Ticks[i] + " ");

                foreach (var series in activeBugsByAssignedTo.Series)
                {
                    Console.Write(string.Format("{0}={1} ", series.Label, series.Data[i]));
                }

                Console.WriteLine();
            }
            
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
