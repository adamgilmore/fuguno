using Microsoft.TeamFoundation.Build.Client;
using System;
namespace Fuguno.Tfs
{
    public class BuildInfoService : IBuildInfoService
    {
        public BuildInfo GetLatestBuildInfo(string tfsServerUri, string tfsCollectionName, string tfsProjectName, string buildDefinitionName)
        {
            BuildInfo buildInfo = null;

            TfsCollection tfsCollection = new TfsCollection(tfsServerUri, tfsCollectionName);
            IBuildServer buildServer = tfsCollection.BuildServer;

            var buildDefinitionSpec = buildServer.CreateBuildDetailSpec(tfsProjectName, buildDefinitionName);
            buildDefinitionSpec.MaxBuildsPerDefinition = 1;
            buildDefinitionSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

            var buildDetails = buildServer.QueryBuilds(buildDefinitionSpec);
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
            }
            
            return buildInfo;
        }
    }
}
