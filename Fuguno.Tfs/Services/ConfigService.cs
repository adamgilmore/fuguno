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
            var names = new List<string>();

            var teams = Helpers.GetTeams(_collection, _tfsProjectName);
            foreach (var team in teams)
            {
                names.Add(team.Name);
            }

            names.Sort();

            return names;
        }

        public IEnumerable<string> GetBuildDefinitionNames()
        {
            var names = new List<string>();

            var buildDefinitions = _collection.BuildServer.QueryBuildDefinitions(_tfsProjectName);
            foreach (var buildDefinition in buildDefinitions)
            {
                names.Add(buildDefinition.Name);
            }

            names.Sort();

            return names;
        }
    }
}
