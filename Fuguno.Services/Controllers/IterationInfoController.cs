namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using System.Web.Http;

    public class IterationInfoController : ApiController
    {
        public IterationInfo Get(string connectionName, string rootIterationPath)
        {
            var parameterMap = Helpers.GetConnectionParameters(connectionName);

            var service = new IterationInfoService(parameterMap["Server"], parameterMap["Collection"], parameterMap["Project"], rootIterationPath);
            return service.GetCurrentIterationInfo();
        }
    }
}