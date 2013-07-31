namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using Fuguno.Tfs.Models;
    using Fuguno.Tfs.Services;
    using System.Collections.Generic;
    using System.Web.Http;

    public class WorkItemStatsController : ApiController
    {
        [HttpGet, ActionName("assignedto")]
        public WorkItemStats GetWorkItemCountByAssignedTo(string teamName, string workItemType, string state)
        {
            var parameterMap = Helpers.GetConnectionParameters();
            var service = new WorkItemStatsService(parameterMap["Server"], parameterMap["Collection"]);
            return service.GetWorkItemCountByAssignedTo(parameterMap["Project"], teamName, workItemType, state);

        }

        [HttpGet, ActionName("priority")]
        public WorkItemStats GetWorkItemCountByPriority(string teamName, string workItemType, string state)
        {
            var parameterMap = Helpers.GetConnectionParameters();
            var service = new WorkItemStatsService(parameterMap["Server"], parameterMap["Collection"]);
            return service.GetWorkItemCountByPriority(parameterMap["Project"], teamName, workItemType, state);
        }
    }
}
