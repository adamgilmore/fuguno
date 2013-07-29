namespace Fuguno.Services.Controllers
{
    using Fuguno.Tfs;
    using System.Collections.Generic;
    using System.Web.Http;

    public class WorkItemStatsController : ApiController
    {
        [HttpGet, ActionName("assignedto")]
        public WorkItemStats GetWorkItemCountByAssignedTo(string connectionName, string projectName, string teamName, string workItemType, string state)
        {
            var parameterMap = Helpers.GetConnectionParameters(connectionName);
            var service = new WorkItemStatsService(parameterMap["Server"], parameterMap["Collection"]);
            return service.GetWorkItemCountByAssignedTo(projectName, teamName, workItemType, state);

        }

        [HttpGet, ActionName("priority")]
        public WorkItemStats GetWorkItemCountByPriority(string connectionName, string projectName, string teamName, string workItemType, string state)
        {
            var parameterMap = Helpers.GetConnectionParameters(connectionName);
            var service = new WorkItemStatsService(parameterMap["Server"], parameterMap["Collection"]);
            return service.GetWorkItemCountByPriority(projectName, teamName, workItemType, state);
        }
    }
}
