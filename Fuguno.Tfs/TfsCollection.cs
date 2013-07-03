namespace Fuguno.Tfs
{
    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.Client;
    using System;
    using System.Collections.Generic;

    internal class TfsCollection
    {
        private TfsTeamProjectCollection _tpc;
        private IBuildServer _buildServer;

        public TfsCollection(string tfsServerUri, string tfsCollectionName)
        {
            Uri tfsProjectCollectionUri = GetTfsProjectCollectionUri(tfsServerUri, tfsCollectionName);
            _tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(tfsProjectCollectionUri);
        }

        public IBuildServer BuildServer
        {
            get { return GetService<IBuildServer>(ref _buildServer); }
        }

        private T GetService<T>(ref T prop)
        {
            if (prop == null)
                prop = _tpc.GetService<T>();
            return prop;
        }

        private Uri GetTfsProjectCollectionUri(string serverUrl, string collectionName)
        {
            UriBuilder tfsProjectCollectionUriBuilder = new UriBuilder(serverUrl);
            List<string> paths = new List<string>(tfsProjectCollectionUriBuilder.Path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            paths.Add(collectionName);
            tfsProjectCollectionUriBuilder.Path = string.Join("/", paths);
            return tfsProjectCollectionUriBuilder.Uri;
        }
    }
}