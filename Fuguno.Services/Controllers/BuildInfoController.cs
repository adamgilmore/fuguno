using Fuguno.Tfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fuguno.Services.Controllers
{
    public class BuildInfoController : ApiController
    {
        public BuildInfo Get(string server, string collection, string project, string buildDefinitionName)
        {
            var service = new BuildInfoService(server, collection, project);
            return service.GetLatestBuildInfo(buildDefinitionName);
        }
    }
}
