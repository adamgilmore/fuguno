using Fuguno.Tfs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fuguno.UI
{
    /// <summary>
    /// Interaction logic for BuildInfoWidget.xaml
    /// </summary>
    public partial class BuildInfoWidget : UserControl
    {
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(BuildInfoWidget));
        public static DependencyProperty BuildNumberProperty = DependencyProperty.Register("BuildNumber", typeof(string), typeof(BuildInfoWidget));
        public static DependencyProperty RequestedForProperty = DependencyProperty.Register("RequestedFor", typeof(string), typeof(BuildInfoWidget));
        public static DependencyProperty ElapsedTimeProperty = DependencyProperty.Register("ElapsedTime", typeof(TimeSpan?), typeof(BuildInfoWidget));
        public static DependencyProperty TestPassRateProperty = DependencyProperty.Register("TestPassRate", typeof(double?), typeof(BuildInfoWidget));

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public string BuildNumber
        {
            get { return (string)GetValue(BuildNumberProperty); }
            set { SetValue(BuildNumberProperty, value); }
        }

        public string RequestedFor
        {
            get { return (string)GetValue(RequestedForProperty); }
            set { SetValue(RequestedForProperty, value); }
        }

        public TimeSpan? ElapsedTime
        {
            get { return (TimeSpan?)GetValue(ElapsedTimeProperty); }
            set { SetValue(ElapsedTimeProperty, value); }
        }

        public double? TestPassRate
        {
            get { return (double?)GetValue(TestPassRateProperty); }
            set { SetValue(TestPassRateProperty, value); }
        }

        public BuildInfoWidget()
        {
            InitializeComponent();
        }
    }
}
