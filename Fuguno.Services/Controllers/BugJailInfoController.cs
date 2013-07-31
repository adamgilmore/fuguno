namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using Fuguno.Tfs.Models;
    using Fuguno.Tfs.Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Http;

    public class BugJailInfoController : ApiController
    {
        public IEnumerable<BugJailInfo> GetAll(string teamName)
        {
            var parameterMap = Helpers.GetConnectionParameters();

            var userImageUrlTemplate = ConfigurationManager.AppSettings["UserImageUrlTemplate"];

            var service = new BugJailInfoService(parameterMap["Server"], parameterMap["Collection"], parameterMap["Project"], teamName, userImageUrlTemplate);
            return service.WhoIsInJail();
        }
    }
}
