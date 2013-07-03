
namespace Fuguno.Tfs
{
    public interface IBuildInfoService
    {
        BuildInfo GetLatestBuildInfo(string tfsServerUri, string tfsCollectionName, string tfsProjectName, string buildDefinitionName);
    }
}
