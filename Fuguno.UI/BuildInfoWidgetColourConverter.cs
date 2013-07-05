namespace Fuguno.UI
{
    using System;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(string), typeof(Brush))]
    public class BuildInfoWidgetColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string status = (string)value;

            switch (status)
            {
                case "NotStarted":
                    return new SolidColorBrush(Colors.DarkGray);
                case "InProgress":
                    return new SolidColorBrush(Colors.LightGray);
                case "Succeeded":
                    return new SolidColorBrush(Colors.Green);
                case "PartiallySucceeded":
                    return new SolidColorBrush(Colors.Orange);
                case "Failed":
                    return new SolidColorBrush(Colors.Red);
                case "Stopped":
                    return new SolidColorBrush(Colors.Yellow);
                default:
                    return new SolidColorBrush(Colors.Black);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
