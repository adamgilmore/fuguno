namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using System.Web.Http;

    public class IterationInfoController : ApiController
    {
        public IterationInfo Get(string server, string collection, string project, string rootIterationPath)
        {
            var service = new IterationInfoService(server, collection, project, rootIterationPath);
            return service.GetCurrentIterationInfo();
        }
    }
}