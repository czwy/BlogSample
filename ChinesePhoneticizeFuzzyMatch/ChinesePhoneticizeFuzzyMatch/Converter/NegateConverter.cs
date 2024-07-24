using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChinesePhoneticizeFuzzyMatch.Converter
{
    public class NegateConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            else if (value is System.Windows.Visibility)
                return (System.Windows.Visibility)value != System.Windows.Visibility.Visible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType.Name == "Boolean")
                return !(bool)value;
            else if (targetType.Name == "Visibility")
                return (System.Windows.Visibility)value != System.Windows.Visibility.Visible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            return DependencyProperty.UnsetValue;
        }
    }
}
