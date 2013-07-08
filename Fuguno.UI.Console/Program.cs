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

            while (true)
            {
                var buildInfo = buildInfoService.GetLatestBuildInfo(ConfigurationManager.AppSettings["TfsBuildDefinitionName"]);
                Console.WriteLine("{0} {1} {2} {3} {4}mins {5}",
                    buildInfo.BuildNumber, 
                    buildInfo.Status, 
                    buildInfo.RequestedFor,
                    buildInfo.StartTime, 
                    buildInfo.ElapsedTime.Minutes, 
                    FormatTestPassRate(buildInfo.TestRunInfos));

                Thread.Sleep(5000);
            }
        }

        private static string  FormatTestPassRate(List<TestRunInfo> testRunInfos)
        {
            string testPassRate = "No tests";

            if (testRunInfos != null && testRunInfos.Count > 0)
            {
                int testPassCount = 0;
                int testTotalCount = 0;
                foreach (var testRunInfo in testRunInfos)
                {
                    testPassCount += testRunInfo.Passed;
                    testTotalCount += testRunInfo.Total;
                }

                testPassRate = string.Format("{0:P0}", (decimal)testPassCount / (decimal)testTotalCount);
            }

            return testPassRate;
        }
    }
}
