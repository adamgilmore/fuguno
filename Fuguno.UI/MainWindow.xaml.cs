namespace Fuguno.UI
{
    using Fuguno.Tfs;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading;
    using System.Timers;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBuildInfoService _buildInfoService;
        string _buildDefinitionName;
        long _refreshInteralMilliseconds;
        BackgroundWorker _worker;

        public MainWindow()
        {
            InitializeComponent();

            _buildInfoService = new BuildInfoService(
                ConfigurationManager.AppSettings["TfsServerUri"],
                ConfigurationManager.AppSettings["TfsCollectionName"],
                ConfigurationManager.AppSettings["TfsProjectName"]);

            _buildDefinitionName = ConfigurationManager.AppSettings["TfsBuildDefinitionName"];

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

                Debug.WriteLine("{0} Enter", DateTime.Now);

                var buildInfo = _buildInfoService.GetLatestBuildInfo(_buildDefinitionName);

                BuildInfoWidget.Dispatcher.Invoke(new Action(delegate()
                {
                    Brush backgroundBrush = null;
                    switch (buildInfo.Status)
                    {
                        case "None":
                        case "NotStarted":
                            backgroundBrush = new SolidColorBrush(Colors.DarkGray);
                            break;
                        case "InProgress":
                            backgroundBrush = new SolidColorBrush(Colors.LightGray);
                            break;
                        case "Succeeded":
                            backgroundBrush = new SolidColorBrush(Colors.Green);
                            break;
                        case "PartiallySucceeded":
                            backgroundBrush = new SolidColorBrush(Colors.Orange);
                            break;
                        case "Failed":
                            backgroundBrush = new SolidColorBrush(Colors.Red);
                            break;
                        case "Stopped":
                            backgroundBrush = new SolidColorBrush(Colors.Yellow);
                            break;
                    }

                    BuildInfoWidget.Text = string.Format("{0} {1} {2} {3}mins",
                    buildInfo.BuildNumber,
                    buildInfo.Status,
                    buildInfo.RequestedFor,
                    buildInfo.ElapsedTime.Minutes);

                    BuildInfoWidget.Background = backgroundBrush;
                }));

                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds < _refreshInteralMilliseconds)
                    Thread.Sleep((int)(_refreshInteralMilliseconds - stopwatch.ElapsedMilliseconds));
            }            
        }
    }
}
