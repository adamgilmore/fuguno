namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using Fuguno.Tfs.Models;
    using Fuguno.Tfs.Services;
    using System.Configuration;
    using System.Web.Http;

    public class IterationInfoController : ApiController
    {
        public IterationInfo Get()
        {
            var parameterMap = Helpers.GetConnectionParameters();
            var rootIterationPath = ConfigurationManager.AppSettings["RootIterationPath"];

            var service = new IterationInfoService(parameterMap["Server"], parameterMap["Collection"], parameterMap["Project"], rootIterationPath);
            return service.GetCurrentIterationInfo();
        }
    }
}