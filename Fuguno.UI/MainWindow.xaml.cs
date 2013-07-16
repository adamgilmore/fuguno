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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBuildInfoService _buildInfoService;
        IIterationInfoService _iterationInfoService;
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

                var iterationInfo = _iterationInfoService.GetCurrentIterationInfo();

                IterationInfoTextBlock.Dispatcher.Invoke(new Action(delegate()
                {
                    IterationInfoTextBlock.Text = string.Format("{0} {1} days left", iterationInfo.Name, (iterationInfo.EndDate - DateTime.Now.Date).TotalDays);
                }));

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
