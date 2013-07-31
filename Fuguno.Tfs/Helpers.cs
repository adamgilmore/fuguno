namespace Fuguno.Tfs
{
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System;
    using Microsoft.TeamFoundation.ProcessConfiguration.Client;
    using Microsoft.TeamFoundation.Client;

    internal static class Helpers
    {
        internal static string CreateAreaPathQueryFragment(string[] areaPaths)
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

        internal static string[] GetAreaPathsForTeam(TfsCollection collection, string projectName, string teamName)
        {
            var areaPaths = new List<string>();

            var teamConfiguration = GetTeamConfiguration(collection, projectName, teamName);
            foreach (var teamFieldValue in teamConfiguration.TeamSettings.TeamFieldValues)
            {
                areaPaths.Add(teamFieldValue.Value);
            }

            return areaPaths.ToArray();
        }

        internal static string GetCurrentIterationPathForTeam(TfsCollection collection, string projectName, string teamName)
        {
            var teamConfiguration = GetTeamConfiguration(collection, projectName, teamName);
            return teamConfiguration.TeamSettings.CurrentIterationPath;
        }


        internal static TeamConfiguration GetTeamConfiguration(TfsCollection collection, string projectName, string teamName)
        {
            var teams = GetTeams(collection, projectName);
            var team = teams.Where(t => t.Name == teamName).First();
            var teamConfiguration = collection.TeamSettings.GetTeamConfigurations(new Guid[] { team.Identity.TeamFoundationId }).First();
            return teamConfiguration;
        }

        internal static IEnumerable<TeamFoundationTeam> GetTeams(TfsCollection collection, string projectName)
        {
            var projectInfo = collection.CommonStructureService.GetProjectFromName(projectName);
            var teams = collection.TeamService.QueryTeams(projectInfo.Uri);
            return teams;
        }
    }


}

