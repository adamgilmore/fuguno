
namespace Fuguno.Tfs
{
    using System.Collections.Generic;

    public interface IBuildInfoService
    {
        BuildInfo GetLatestBuildInfo(string buildDefinitionName);
    }
}
