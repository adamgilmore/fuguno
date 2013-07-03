namespace Fuguno.Tfs
{
    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.TestManagement.Client;
    using System;
    using System.Collections.Generic;

    public class BuildInfoService : IBuildInfoService
    {
        private TfsCollection _tfsCollection;
        private string _tfsProjectName;

        private IBuildServer _buildServer;
        private ITestManagementService _testManagementService;

        public BuildInfoService(string tfsServerUri, string tfsCollectionName, string tfsProjectName)
        {
            _tfsProjectName = tfsProjectName;
            _tfsCollection = new TfsCollection(tfsServerUri, tfsCollectionName);
            _buildServer = _tfsCollection.BuildServer;
            _testManagementService = _tfsCollection.TestManagementService;
        }

        public BuildInfo GetLatestBuildInfo(string buildDefinitionName)
        {
            BuildInfo buildInfo = null;

            var buildDefinitionSpec = _buildServer.CreateBuildDetailSpec(_tfsProjectName, buildDefinitionName);
            buildDefinitionSpec.MaxBuildsPerDefinition = 1;
            buildDefinitionSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

            var buildDetails = _buildServer.QueryBuilds(buildDefinitionSpec);
            if (buildDetails == null || buildDetails.Builds == null || buildDetails.Builds.Length == 0)
            {
                throw new ApplicationException(string.Format("Build Definition '{0}' not found", buildDefinitionName));
            }

            var buildDetail = buildDetails.Builds[0];

            if (buildDetail != null)
            {
                buildInfo = new BuildInfo()
                {
                    BuildNumber = buildDetail.BuildNumber,
                    Status = buildDetail.Status.ToString(),
                    StartTime = buildDetail.StartTime,
                    LastChangeTime = buildDetail.LastChangedOn,
                    FinishTime = buildDetail.FinishTime,
                    RequestedBy = buildDetail.RequestedBy,
                    RequestedFor = buildDetail.RequestedFor
                };

                buildInfo.TestRunInfos = GetTestRunInfos(buildDetail.Uri);
            }
            
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
    }
}
