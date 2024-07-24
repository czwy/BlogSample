using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChinesePhoneticizeFuzzyMatch.Converter
{
    public class Int2VisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return ((int)value) > 0 ? Visibility.Visible : Visibility.Collapsed;
            else
                return (int)value > int.Parse(parameter.ToString()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
