namespace Fuguno.Tfs
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IWorkItemStatsService
    {
        IEnumerable<WorkItemStat> GetWorkItemCountByAssignedTo(string workItemType, string state, string[] areaPaths);
        IEnumerable<WorkItemStat> GetWorkItemCountByPriority(string workItemType, string state, string[] areaPaths);
    }
}
