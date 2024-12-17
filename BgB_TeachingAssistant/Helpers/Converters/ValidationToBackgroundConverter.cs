using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BgB_TeachingAssistant.Helpers.Converters
{
    public class ValidationToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isValid = value is bool valid && valid;
            return isValid ? Brushes.Transparent : Brushes.LightCoral;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
