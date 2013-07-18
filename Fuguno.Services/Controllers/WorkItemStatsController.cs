namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using System.Collections.Generic;
    using System.Web.Http;

    public class WorkItemStatsController : ApiController
    {
        [HttpGet, ActionName("assignedto")]
        public IEnumerable<WorkItemStat> GetWorkItemCountByAssignedTo(string connectionName, string workItemType, string state, string areaPaths)
        {
            var parameterMap = Helpers.GetConnectionParameters(connectionName);

            var areaPathsArray = areaPaths.Split(new char[] { ',' });

            var service = new WorkItemStatsService(parameterMap["Server"], parameterMap["Collection"]);
            return service.GetWorkItemCountByAssignedTo(workItemType, state, areaPathsArray);

        }

        [HttpGet, ActionName("priority")]
        public IEnumerable<WorkItemStat> GetWorkItemCountByPriority(string connectionName, string workItemType, string state, string areaPaths)
        {
            var parameterMap = Helpers.GetConnectionParameters(connectionName);

            var areaPathsArray = areaPaths.Split(new char[] { ',' });

            var service = new WorkItemStatsService(parameterMap["Server"], parameterMap["Collection"]);
            return service.GetWorkItemCountByPriority(workItemType, state, areaPathsArray);
        }
    }
}
