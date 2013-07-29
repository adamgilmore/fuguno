namespace Fuguno.UI
{
    using System.Configuration;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.Navigate(ConfigurationManager.AppSettings["Url"]);
        }
    }
}
