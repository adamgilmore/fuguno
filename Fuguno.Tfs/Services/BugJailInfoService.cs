namespace Fuguno.Tfs.Services
{
    using Fuguno.Tfs.Models;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Framework.Client;
    using Microsoft.TeamFoundation.Framework.Common;
    using Microsoft.TeamFoundation.Server;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public class BugJailInfoService
    {
        private const string BugJailInfoQueryTemplate = @"
            SELECT  [System.Id], 
                    [System.State],
                    [Microsoft.VSTS.Common.Priority], 
                    [System.AssignedTo] 
            FROM    WorkItems 
            WHERE   [System.WorkItemType] = 'Bug'";

        private string _projectName;
        private string _teamName;
        private TfsCollection _collection;
        private string _userImageUrlTemplate;

        public BugJailInfoService(string tfsServerUri, string tfsCollectionName, string tfsProjectName, string tfsTeamName, string userImageUrlTemplate)
        {
            _projectName = tfsProjectName;
            _teamName = tfsTeamName;
            _collection = new TfsCollection(tfsServerUri, tfsCollectionName);
            _userImageUrlTemplate = userImageUrlTemplate;
        }

        public IEnumerable<BugJailInfo> WhoIsInJail()
        {
            Trace.TraceInformation(string.Format("{0} -> WhoIsInJail", DateTime.Now));

            try
            {
                List<BugJailInfo> bugJailInfos = new List<BugJailInfo>();

                // get area paths for this team
                var areaPaths = Helpers.GetAreaPathsForTeam(_collection, _projectName, _teamName);

                // build query
                var queryText = new StringBuilder(string.Format(BugJailInfoQueryTemplate));
                queryText.Append(" AND [System.State] = 'Active'");
                queryText.Append(Helpers.CreateAreaPathQueryFragment(areaPaths));

                // run work item query
                var workItems = RunQuery(queryText.ToString());

                // get team members
                var teamConfiguration = Helpers.GetTeamConfiguration(_collection, _projectName, _teamName);
                var team = _collection.TeamService.ReadTeam(teamConfiguration.TeamId, null);
                var members = team.GetMembers(_collection.Collection, MembershipQuery.Expanded).Where(m => !m.IsContainer);
                foreach (var member in members)
                {
                    if (IsInActiveJail(member, workItems))
                    {
                        var userName = member.UniqueName.Split('\\')[1];
                        bugJailInfos.Add(new BugJailInfo() { Name = member.DisplayName, ImageUrl = string.Format(_userImageUrlTemplate, userName) });
                    }
                }

                //SDET - if (Resolved P0 age > 1d || (Active Test bugs > 6 && age > 5d) || (Resolved Test bugs > 6 && age > 5d)

                return bugJailInfos;
            }
            finally
            {
                Trace.TraceInformation(string.Format("{0} <- WhoIsInJail", DateTime.Now));
            }
        }

        private bool IsInActiveJail(TeamFoundationIdentity member, List<WorkItemInfo> workItemInfos)
        {
            var items = (
                from item in ( from item in workItemInfos where item.AssignedTo == member.DisplayName select item)
                group item by item.Priority into priorities
                select new { Key = priorities.Key, Count = priorities.Count() }
                ).ToDictionary(i => i.Key, i => i.Count);

            if (items.ContainsKey(0) && items[0] > 0)
                return true;
            if (items.ContainsKey(1) && items[1] > 3)
                return true;
            if (items.ContainsKey(2) && items[2] > 6)
                return true;

            return false;
        }

        private List<WorkItemInfo> RunQuery(string queryText)
        {
            var workItemInfos = new List<WorkItemInfo>();
            var workItems = _collection.WorkItemStore.Query(queryText.ToString());
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
