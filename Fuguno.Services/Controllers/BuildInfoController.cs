namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using System.Web.Http;

    public class BuildInfoController : ApiController
    {
        public BuildInfo Get(string buildDefinitionName)
        {
            var parameterMap = Helpers.GetConnectionParameters();

            var service = new BuildInfoService(parameterMap["Server"], parameterMap["Collection"], parameterMap["Project"]);
            return service.GetLatestBuildInfo(buildDefinitionName);
        }
    }
}
