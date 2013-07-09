namespace Fuguno.Tfs
{
    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.TestManagement.Client;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class BuildInfoService : IBuildInfoService
    {
        private string _tfsProjectName;

        private IBuildServer _buildServer;
        private ITestManagementService _testManagementService;

        public BuildInfoService(string tfsServerUri, string tfsCollectionName, string tfsProjectName)
        {
            _tfsProjectName = tfsProjectName;
            var tfsCollection = new TfsCollection(tfsServerUri, tfsCollectionName);
            _buildServer = tfsCollection.BuildServer;
            _testManagementService = tfsCollection.TestManagementService;
        }

        public IEnumerable<BuildInfo> GetLatestBuildInfos(IEnumerable<string> buildDefinitionNames)
        {
            Trace.TraceInformation(string.Format("{0} -> GetLatestBuildInfos", DateTime.Now));

            List<BuildInfo> buildInfos = new List<BuildInfo>();

            foreach (var buildDefinitionName in buildDefinitionNames)
            {
                buildInfos.Add(GetLatestBuildInfo(buildDefinitionName));
            }

            Trace.TraceInformation(string.Format("{0} <- GetLatestBuildInfos", DateTime.Now));

            return buildInfos;
        }

        public BuildInfo GetLatestBuildInfo(string buildDefinitionName)
        {
            Trace.TraceInformation(string.Format("{0} -> GetLatestBuildInfo buildDefinitionName={1}", DateTime.Now, buildDefinitionName));

            BuildInfo buildInfo = null;

            var buildDefinitionSpec = _buildServer.CreateBuildDetailSpec(_tfsProjectName, buildDefinitionName);
            buildDefinitionSpec.InformationTypes = null;
            buildDefinitionSpec.MaxBuildsPerDefinition = 1;
            buildDefinitionSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

            var buildDetails = _buildServer.QueryBuilds(buildDefinitionSpec);
            if (buildDetails == null || buildDetails.Builds == null || buildDetails.Builds.Length == 0)
            {
                throw new ApplicationException(string.Format("Build Definition '{0}' not found", buildDefinitionName));
            }

            var buildDetail = buildDetails.Builds[0]; // we only requested 1 so take the first
            if (buildDetail != null)
            {
                buildInfo = new BuildInfo()
                {
                    Name = buildDefinitionName,
                    BuildNumber = buildDetail.BuildNumber,
                    Status = buildDetail.Status.ToString(),
                    StartTime = buildDetail.StartTime,
                    LastChangeTime = buildDetail.LastChangedOn,
                    FinishTime = buildDetail.FinishTime,
                    RequestedBy = buildDetail.RequestedBy,
                    RequestedFor = buildDetail.RequestedFor
                };

                buildInfo.TestRunInfos = GetTestRunInfos(buildDetail.Uri);

                long totalTestCount, totalTestPassedCount, totalTestFailedCount, totalTestInconclusiveCount;
                CalculateTotalTestCounts(buildInfo.TestRunInfos, out totalTestCount, out totalTestPassedCount, out totalTestFailedCount, out totalTestInconclusiveCount);

                buildInfo.TotalTestCount = totalTestCount;
                buildInfo.TotalTestPassedCount = totalTestPassedCount;
                buildInfo.TotalTestFailedCount = totalTestFailedCount;
                buildInfo.TotalTestInconclusiveCount = totalTestInconclusiveCount;
            }

            Trace.TraceInformation(string.Format("{0} <- GetLatestBuildInfo buildDefinitionName={1}", DateTime.Now, buildDefinitionName));
            
            return buildInfo;
        }

        private List<TestRunInfo> GetTestRunInfos(Uri buildUri)
        {
            var testRunInfos = new List<TestRunInfo>();

            var testProject = _testManagementService.GetTeamProject(_tfsProjectName);
            var testRuns = testProject.TestRuns.ByBuild(buildUri);
            foreach (var testRun in testRuns)
            {
                testRunInfos.Add(new TestRunInfo()
                {
                    Completed = testRun.Statistics.CompletedTests,
                    Failed = testRun.Statistics.FailedTests,
                    Inconclusive = testRun.Statistics.InconclusiveTests,
                    Passed = testRun.Statistics.PassedTests,
                    Total = testRun.Statistics.TotalTests
                });
            }

            return testRunInfos;
        }

        private static void CalculateTotalTestCounts(List<TestRunInfo> testRunInfos, out long totalTestCount, out long totalPassedCount, out long totalFailedCount, out long totalInconclusiveCount)
        {
            totalTestCount = 0;
            totalPassedCount = 0;
            totalFailedCount = 0;
            totalInconclusiveCount = 0;

            if (testRunInfos != null && testRunInfos.Count > 0)
            {
                foreach (var testRunInfo in testRunInfos)
                {
                    totalTestCount += testRunInfo.Completed;
                    totalPassedCount += testRunInfo.Passed;
                    totalFailedCount += testRunInfo.Failed;
                    totalInconclusiveCount += testRunInfo.Inconclusive;
                }
            }
        }
    }
}
