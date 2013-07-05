
namespace Fuguno.Tfs
{
    using System.Collections.Generic;

    public interface IBuildInfoService
    {
        BuildInfo GetLatestBuildInfo(string buildDefinitionName);
        IEnumerable<BuildInfo> GetLatestBuildInfos(IEnumerable<string> buildDefinitionNames);
    }
}
