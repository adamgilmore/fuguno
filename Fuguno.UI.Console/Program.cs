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
            var iterationInfoService = new IterationInfoService(
                ConfigurationManager.AppSettings["TfsServerUri"],
                ConfigurationManager.AppSettings["TfsCollectionName"],
                ConfigurationManager.AppSettings["TfsProjectName"],
                ConfigurationManager.AppSettings["TfsRootIterationPath"]);

            var iterationInfo = iterationInfoService.GetCurrentIterationInfo();

            Console.WriteLine("{0} {1} {2:d} {3:d}", iterationInfo.Name, iterationInfo.Path, iterationInfo.StartDate, iterationInfo.EndDate);

            var buildInfoService = new BuildInfoService(
                ConfigurationManager.AppSettings["TfsServerUri"],
                ConfigurationManager.AppSettings["TfsCollectionName"],
                ConfigurationManager.AppSettings["TfsProjectName"]);

            var buildDefinitonNames = ConfigurationManager.AppSettings["TfsBuildDefinitionNames"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

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
                        buildInfo.ElapsedTime.Minutes,
                        string.Format("{0:p}", ((double)buildInfo.TotalTestPassedCount / (double)buildInfo.TotalTestCount)));
                }
            }
        }
    }
}
