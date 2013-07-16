namespace Fuguno.Tfs
{
    using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;

    internal class TfsCollection
    {
        private TfsTeamProjectCollection _tpc;
        private IBuildServer _buildServer;
        private ITestManagementService _testManagementService;
        private ICommonStructureService4 _commonStructureService;
        private WorkItemStore _workItemStore;

        public TfsCollection(string tfsServerUri, string tfsCollectionName)
        {
            Uri tfsProjectCollectionUri = GetTfsProjectCollectionUri(tfsServerUri, tfsCollectionName);
            _tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(tfsProjectCollectionUri);
        }

        public IBuildServer BuildServer
        {
            get { return GetService<IBuildServer>(ref _buildServer); }
        }

        public ITestManagementService TestManagementService
        {
            get { return GetService<ITestManagementService>(ref _testManagementService); }
        }

        public ICommonStructureService4 CommonStructureService
        {
            get { return GetService<ICommonStructureService4>(ref _commonStructureService); }
        }

        public WorkItemStore WorkItemStore
        {
            get { return GetService<WorkItemStore>(ref _workItemStore); }
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