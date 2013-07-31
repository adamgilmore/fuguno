namespace Fuguno.Services.Controllers.Api
{
    using Fuguno.Tfs;
    using Fuguno.Tfs.Models;
    using Fuguno.Tfs.Services;
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
