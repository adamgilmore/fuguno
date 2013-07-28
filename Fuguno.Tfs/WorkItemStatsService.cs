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

        public WorkItemStats GetWorkItemCountByAssignedTo(string workItemType, string state, string[] areaPaths)
        {
            Trace.TraceInformation(string.Format("{0} -> GetWorkItemCountByAssignedTo", DateTime.Now));

            try
            {
                // build query
                var queryText = new StringBuilder(string.Format(WorkItemCountQueryTemplate, workItemType));
                queryText.AppendFormat(" AND State = '{0}'", state);
                queryText.Append(Helpers.CreateAreaPathQueryFragment(areaPaths));

                // run query
                var workItemInfos = RunQuery(queryText.ToString());

                var workItemStats = new WorkItemStats();

                // create a series for each distinct priority
                var priorities = (from item in workItemInfos orderby item.Priority descending select item.Priority).Distinct().ToList();
                foreach (var priority in priorities)
                {
                    workItemStats.Series.Add(new WorkItemStatsSeries() { Label = string.Format("P{0}", priority) });
                }

                // for each person, add the counts of each priority to the correct data series
                List<string> people = (from item in workItemInfos orderby item.AssignedTo ascending select item.AssignedTo).Distinct().ToList();
                for (var i = 0; i < people.Count; ++i)
                {
                    workItemStats.Ticks.Add(people[i]);


                    for (var j = 0; j < priorities.Count; ++j )
                    {
                        workItemStats.Series[j].Data.Add((from item in workItemInfos where item.AssignedTo == people[i] && item.Priority == priorities[j] select item).Count());
                    }
                }

                return workItemStats;
            }
            finally
            {
                Trace.TraceInformation(string.Format("{0} <- GetWorkItemCountByAssignedTo", DateTime.Now));
            }
        }

        public WorkItemStats GetWorkItemCountByPriority(string workItemType, string state, string[] areaPaths)
        {
            Trace.TraceInformation(string.Format("{0} -> GetWorkItemCountByPriority", DateTime.Now));

            try
            {
                // build query
                var queryText = new StringBuilder(string.Format(WorkItemCountQueryTemplate, workItemType));
                queryText.AppendFormat(" AND State = '{0}'", state);
                queryText.Append(Helpers.CreateAreaPathQueryFragment(areaPaths));

                // run query
                var workItemInfos = RunQuery(queryText.ToString());

                // group results
                var workItemStats = new WorkItemStats();
                var workItemStatsSeries = new WorkItemStatsSeries();

                var priorities = (from item in workItemInfos orderby item.Priority ascending select item.Priority).Distinct();
                foreach (var priority in priorities)
                {
                    workItemStats.Ticks.Add(string.Format("P{0}", priority));
                    workItemStatsSeries.Data.Add((from item in workItemInfos where item.Priority == priority select item).Count());
                }

                workItemStats.Series.Add(workItemStatsSeries);

                return workItemStats;
            }
            finally
            {
                Trace.TraceInformation(string.Format("{0} <- GetWorkItemCountByPriority", DateTime.Now));
            }
        }

        private List<WorkItemInfo> RunQuery(string queryText)
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
    }
}
