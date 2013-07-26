namespace Fuguno.Tfs
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IWorkItemStatsService
    {
        WorkItemStats GetWorkItemCountByAssignedTo(string workItemType, string state, string[] areaPaths);
        WorkItemStats GetWorkItemCountByPriority(string workItemType, string state, string[] areaPaths);
    }
}
