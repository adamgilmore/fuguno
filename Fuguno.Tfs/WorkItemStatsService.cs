namespace Fuguno.Tfs
{
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

    public class WorkItemStatsService : IWorkItemStatsService
    {
        private const string WorkItemCountQueryTemplate = @"
            SELECT  [System.Id], 
                    [Microsoft.VSTS.Common.Priority], 
                    [Microsoft.VSTS.Common.Severity], 
                    [System.AssignedTo] 
            FROM    WorkItems 
            WHERE   [System.WorkItemType] = '{0}'";

        private WorkItemStore _workItemStore;

        public WorkItemStatsService(string tfsServerUri, string tfsCollectionName)
        {
            var tfsCollection = new TfsCollection(tfsServerUri, tfsCollectionName);
            _workItemStore = tfsCollection.WorkItemStore;
        }

        public IEnumerable<WorkItemStat> GetWorkItemCountByAssignedTo(string workItemType, string state, string[] areaPaths)
        {
            Trace.TraceInformation(string.Format("{0} -> GetWorkItemCountByAssignedTo", DateTime.Now));

            try
            {
                // build query
                var queryText = new StringBuilder(string.Format(WorkItemCountQueryTemplate, workItemType));
                queryText.AppendFormat(" AND State = '{0}'", state);
                queryText.Append(CreateAreaPathQueryFragment(areaPaths));

                // run query
                var workItemInfos = RunQuery(queryText);

                // group results
                return workItemInfos
                    .GroupBy(workItemInfo => workItemInfo.AssignedTo)
                    .Select(list => new WorkItemStat() { Key = list.Key, Count = list.Count() })
                    .OrderBy(stat => stat.Count);
            }
            finally
            {
                Trace.TraceInformation(string.Format("{0} <- GetWorkItemCountByAssignedTo", DateTime.Now));
            }
        }

        public IEnumerable<WorkItemStat> GetWorkItemCountByPriority(string workItemType, string state, string[] areaPaths)
        {
            Trace.TraceInformation(string.Format("{0} -> GetWorkItemCountByPriority", DateTime.Now));

            try
            {
                // build query
                var queryText = new StringBuilder(string.Format(WorkItemCountQueryTemplate, workItemType));
                queryText.AppendFormat(" AND State = '{0}'", state);
                queryText.Append(CreateAreaPathQueryFragment(areaPaths));

                // run query
                var workItemInfos = RunQuery(queryText);

                // group results
                return workItemInfos
                    .GroupBy(workItemInfo => workItemInfo.Priority)
                    .Select(list => new WorkItemStat() { Key = list.Key.ToString(), Count = list.Count() })
                    .OrderBy(stat => stat.Key);
            }
            finally
            {
                Trace.TraceInformation(string.Format("{0} <- GetWorkItemCountByPriority", DateTime.Now));
            }
        }

        private List<WorkItemInfo> RunQuery(StringBuilder queryText)
        {
            var workItemInfos = new List<WorkItemInfo>();
            var workItems = _workItemStore.Query(queryText.ToString());
            foreach (WorkItem workItem in workItems)
            {
                workItemInfos.Add(new WorkItemInfo()
                {
                    Id = workItem.Id,
                    Priority = Convert.ToInt32(workItem.Fields["Microsoft.VSTS.Common.Priority"].Value),
                    Severity = Convert.ToString(workItem.Fields["Microsoft.VSTS.Common.Severity"].Value),
                    AssignedTo = Convert.ToString(workItem.Fields["System.AssignedTo"].Value)
                });
            }
            return workItemInfos;
        }

        private static string CreateAreaPathQueryFragment(string[] areaPaths)
        {
            var queryText = new StringBuilder();

            if (areaPaths != null & areaPaths.Length > 0)
            {
                queryText.Append(" AND ");

                if (areaPaths.Length > 1)
                    queryText.Append("(");

                bool prefixWithOr = false;
                foreach (var areaPath in areaPaths)
                {
                    if (prefixWithOr == true)
                        queryText.Append(" OR ");
                    else
                        prefixWithOr = true;
                    queryText.AppendFormat("[Area Path] under '{0}'", areaPath.Trim());
                }

                if (areaPaths.Length > 1)
                    queryText.Append(")");
            }

            return queryText.ToString();
        }
    }
}
