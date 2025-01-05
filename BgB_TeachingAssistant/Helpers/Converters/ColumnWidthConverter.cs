using System;
using System.Globalization;
using System.Windows.Data;

namespace BgB_TeachingAssistant.Helpers.Converters
{
    public class ColumnWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Check for null or invalid values
            if (values.Length < 2 || values[0] == null || values[1] == null)
                return double.NaN;

            // Safely cast the values
            if (values[0] is double totalWidth && values[1] is int columnCount)
            {
                if (columnCount == 0)
                    return double.NaN; // Avoid division by zero

                return totalWidth / columnCount; // Divide available width evenly
            }

            // Fallback in case of invalid types
            return double.NaN;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}