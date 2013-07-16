namespace Fuguno.UI
{
    using Fuguno.Tfs;
    using Fuguno.UI.Helpers;
    using Fuguno.UI.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls.DataVisualization.Charting;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBuildInfoService _buildInfoService;
        IIterationInfoService _iterationInfoService;
        IWorkItemStatsService _workItemStatsService;
        long _refreshInteralMilliseconds;
        BackgroundWorker _worker;

        ObservableCollectionWithItemNotify<BuildInfoViewModel> _buildInfoViewModels = new ObservableCollectionWithItemNotify<BuildInfoViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            _iterationInfoService = new IterationInfoService(
                ConfigurationManager.AppSettings["TfsServerUri"],
                ConfigurationManager.AppSettings["TfsCollectionName"],
                ConfigurationManager.AppSettings["TfsProjectName"],
                ConfigurationManager.AppSettings["TfsRootIterationPath"]);

            _buildInfoService = new BuildInfoService(
                ConfigurationManager.AppSettings["TfsServerUri"],
                ConfigurationManager.AppSettings["TfsCollectionName"],
                ConfigurationManager.AppSettings["TfsProjectName"]);

            _workItemStatsService = new WorkItemStatsService(
                ConfigurationManager.AppSettings["TfsServerUri"],
                ConfigurationManager.AppSettings["TfsCollectionName"]);


            var buildDefinitonNames = ConfigurationManager.AppSettings["TfsBuildDefinitionNames"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var buildDefinitionName in buildDefinitonNames)
            {
                var viewModel = new BuildInfoViewModel(buildDefinitionName.Trim(), true);
                _buildInfoViewModels.Add(viewModel);
            }

            BuildInfosListBox.ItemsSource = _buildInfoViewModels;

            _refreshInteralMilliseconds = Convert.ToInt32(ConfigurationManager.AppSettings["RefreshIntervalSeconds"]) * 1000;

            _worker = new BackgroundWorker();
            _worker.DoWork += WorkerStart;
            _worker.RunWorkerAsync();
        }


        void WorkerStart(object sender, DoWorkEventArgs e)
        {
            while(true) 
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Work Item stats
                var tfsWorkItemStatsWorkItemType = ConfigurationManager.AppSettings["TfsWorkItemStatsWorkItemType"];
                var tfsWorkItemStatsState = ConfigurationManager.AppSettings["TfsWorkItemStatsState"];
                var tfsWorkItemStatsAreaPaths = ConfigurationManager.AppSettings["TfsWorkItemStatsAreaPaths"];
                var areaPaths = tfsWorkItemStatsAreaPaths.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var activeBugsByPriority = _workItemStatsService.GetWorkItemCountByPriority(tfsWorkItemStatsWorkItemType, tfsWorkItemStatsState, areaPaths);

                BugPriorityChart.Dispatcher.Invoke(new Action(delegate()
                {
                    ((ColumnSeries)BugPriorityChart.Series[0]).ItemsSource = activeBugsByPriority;
                }));

                var activeBugsByAssignedTo = _workItemStatsService.GetWorkItemCountByAssignedTo(tfsWorkItemStatsWorkItemType, tfsWorkItemStatsState, areaPaths);

                BugAssignedToChart.Dispatcher.Invoke(new Action(delegate()
                {
                    ((BarSeries)BugAssignedToChart.Series[0]).ItemsSource = activeBugsByAssignedTo;
                }));

                // Iteration Info
                var iterationInfo = _iterationInfoService.GetCurrentIterationInfo();

                IterationInfoTextBlock.Dispatcher.Invoke(new Action(delegate()
                {
                    IterationInfoTextBlock.Text = string.Format("{0} {1} days left", iterationInfo.Name, (iterationInfo.EndDate - DateTime.Now.Date).TotalDays);
                }));

                // Build Info
                foreach (var buildInfoViewModel in _buildInfoViewModels)
                {
                    var buildInfo = _buildInfoService.GetLatestBuildInfo(buildInfoViewModel.Name);

                    BuildInfosListBox.Dispatcher.Invoke(new Action(delegate()
                    {
                        buildInfoViewModel.BuildNumber = buildInfo.BuildNumber;
                        buildInfoViewModel.RequestedFor = buildInfo.RequestedFor;
                        buildInfoViewModel.Status = buildInfo.Status;
                        buildInfoViewModel.StartTime = buildInfo.StartTime;
                        buildInfoViewModel.FinishTime = buildInfo.FinishTime;
                        buildInfoViewModel.LastChangeTime = buildInfo.LastChangeTime;
                        buildInfoViewModel.TotalTestCount = buildInfo.TotalTestCount;
                        buildInfoViewModel.TotalTestPassedCount = buildInfo.TotalTestPassedCount;
                        buildInfoViewModel.IsLoading = false;
                    }));
                }

                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds < _refreshInteralMilliseconds)
                    Thread.Sleep((int)(_refreshInteralMilliseconds - stopwatch.ElapsedMilliseconds));
            }            
        }
    }
}
