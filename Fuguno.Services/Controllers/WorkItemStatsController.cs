namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using System.Collections.Generic;
    using System.Web.Http;

    public class WorkItemStatsController : ApiController
    {
        [HttpGet, ActionName("assignedto")]
        public IEnumerable<WorkItemStat> GetWorkItemCountByAssignedTo(string server, string collection, string workItemType, string state, string areaPaths)
        {
            var areaPathsArray = areaPaths.Split(new char[] { ',' });

            var service = new WorkItemStatsService(server, collection);
            return service.GetWorkItemCountByAssignedTo(workItemType, state, areaPathsArray);

        }

        [HttpGet, ActionName("priority")]
        public IEnumerable<WorkItemStat> GetWorkItemCountByPriority(string server, string collection, string workItemType, string state, string areaPaths)
        {
            var areaPathsArray = areaPaths.Split(new char[] { ',' });

            var service = new WorkItemStatsService(server, collection);
            return service.GetWorkItemCountByPriority(workItemType, state, areaPathsArray);
        }
    }
}
