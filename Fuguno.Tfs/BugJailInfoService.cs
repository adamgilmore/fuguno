namespace Fuguno.Tfs
{
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Framework.Common;
    using Microsoft.TeamFoundation.Server;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public class BugJailInfoService : IBugJailInfoService
    {
        private const string BugJailInfoQueryTemplate = @"
            SELECT  [System.Id], 
                    [System.State],
                    [Microsoft.VSTS.Common.Priority], 
                    [System.AssignedTo] 
            FROM    WorkItems 
            WHERE   [System.WorkItemType] = 'Bug'";

        private string _projectName;
        private TfsCollection _collection;

        public BugJailInfoService(string tfsServerUri, string tfsCollectionName, string tfsProjectName)
        {
            _projectName = tfsProjectName;
            _collection = new TfsCollection(tfsServerUri, tfsCollectionName);
        }

        public IEnumerable<BugJailInfo> GetBugJail(string[] areaPaths)
        {
            Trace.TraceInformation(string.Format("{0} -> GetBugJail", DateTime.Now));

            try
            {
                var projectInfo = _collection.CommonStructureService.GetProjectFromName(_projectName);
                var teams = _collection.TeamService.QueryTeams(projectInfo.Uri);
                foreach (var team in teams)
                {
                    var members = team.GetMembers(_collection.Collection, MembershipQuery.Expanded);
                    var users = members.Where(m => !m.IsContainer);
                }

                // build query
                var queryText = new StringBuilder(string.Format(BugJailInfoQueryTemplate));
                queryText.Append(" AND [System.State] = 'Active'");
                queryText.Append(Helpers.CreateAreaPathQueryFragment(areaPaths));

                // run query
                var workItems = _collection.WorkItemStore.Query(queryText.ToString());
                foreach (WorkItem workItem in workItems)
                {
                }

                //SDE - if (Active P0 > 0 || Active P1 > 3 || Active P2 > 6 || Active P3 > 6)
                //SDET - if (Resolved P0 age > 1d || (Active Test bugs > 6 && age > 5d) || (Resolved Test bugs > 6 && age > 5d)
                //PM - ?

                // group results
                //return bugJailInfos
                //    .GroupBy(bugJailInfo => bugJailInfo.Name)
                //    .Select(list => new BugJailInfo() { Name = list.Name })
                //    .OrderBy(item => item.Name);
                return new List<BugJailInfo>();
            }
            finally
            {
                Trace.TraceInformation(string.Format("{0} <- GetBugJail", DateTime.Now));
            }
        }
    }
}
