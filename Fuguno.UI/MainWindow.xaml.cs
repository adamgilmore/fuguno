namespace Fuguno.UI
{
    using Fuguno.Tfs;
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
        List<string> _buildDefinitionNames = new List<string>();
        long _refreshInteralMilliseconds;
        BackgroundWorker _worker;

        ObservableCollection<BuildInfo> _buildInfos = new ObservableCollection<BuildInfo>();

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

            var names = ConfigurationManager.AppSettings["TfsBuildDefinitionNames"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var name in names)
            {
                _buildDefinitionNames.Add(name.Trim());
            }

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

                var buildInfos = _buildInfoService.GetLatestBuildInfos(_buildDefinitionNames);

                BuildInfosListBox.Dispatcher.Invoke(new Action(delegate()
                {
                    _buildInfos.Clear();
                    foreach (var buildInfo in buildInfos)
                    {
                        _buildInfos.Add(buildInfo);
                    }

                    BuildInfosListBox.ItemsSource = _buildInfos;
                }));

                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds < _refreshInteralMilliseconds)
                    Thread.Sleep((int)(_refreshInteralMilliseconds - stopwatch.ElapsedMilliseconds));
            }            
        }
    }
}
