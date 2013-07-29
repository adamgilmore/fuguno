namespace Fuguno.Tfs
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IWorkItemStatsService
    {
        WorkItemStats GetWorkItemCountByAssignedTo(string projectName, string teamName, string workItemType, string state);
        WorkItemStats GetWorkItemCountByPriority(string projectName, string teamName, string workItemType, string state);
    }
}
