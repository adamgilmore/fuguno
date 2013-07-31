namespace Fuguno.Tfs.Services
{
    using System.Collections.Generic;
    using System.Linq;

    public class ConfigService
    {
        private TfsCollection _collection;
        private string _tfsProjectName;

        public ConfigService(string tfsServerUri, string tfsCollectionName, string tfsProjectName)
        {
            _collection = new TfsCollection(tfsServerUri, tfsCollectionName);
            _tfsProjectName = tfsProjectName;
        }

        public IEnumerable<string> GetTeamNames()
        {
            var teamNames = new List<string>();

            var teams = Helpers.GetTeams(_collection, _tfsProjectName);
            foreach (var team in teams)
            {
                teamNames.Add(team.Name);
            }

            teamNames.Sort();

            return teamNames;
        }
    }
}
