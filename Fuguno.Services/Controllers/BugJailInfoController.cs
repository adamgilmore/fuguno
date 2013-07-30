namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Http;

    public class BugJailInfoController : ApiController
    {
        public IEnumerable<BugJailInfo> GetAll(string connectionName, string teamName)
        {
            var parameterMap = Helpers.GetConnectionParameters(connectionName);

            var userImageUrlTemplate = ConfigurationManager.AppSettings["UserImageUrlTemplate"];

            var service = new BugJailInfoService(parameterMap["Server"], parameterMap["Collection"], parameterMap["Project"], teamName, userImageUrlTemplate);
            return service.WhoIsInJail();
        }
    }
}
