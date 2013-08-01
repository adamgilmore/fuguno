namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs.Services;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Http;

    public class BoardConfigController : ApiController
    {
        // GET api/boardconfig/teamnames
        [HttpGet]
        public IEnumerable<string> TeamNames()
        {
            var parameterMap = Helpers.GetConnectionParameters();

            var service = new ConfigService(parameterMap["Server"], parameterMap["Collection"], parameterMap["Project"]);
            return service.GetTeamNames();
        }

        // GET api/boardconfig/teamnames
        [HttpGet]
        public IEnumerable<string> BuildDefinitionNames()
        {
            var parameterMap = Helpers.GetConnectionParameters();

            var service = new ConfigService(parameterMap["Server"], parameterMap["Collection"], parameterMap["Project"]);
            return service.GetBuildDefinitionNames();
        }
    }
}
