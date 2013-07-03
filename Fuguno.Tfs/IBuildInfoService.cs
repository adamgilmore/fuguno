
namespace Fuguno.Tfs
{
    public interface IBuildInfoService
    {
        BuildInfo GetLatestBuildInfo(string buildDefinitionName);
    }
}
