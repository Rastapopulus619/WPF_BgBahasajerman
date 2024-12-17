using System;
using System.Globalization;
using System.Windows.Data;

namespace BgB_TeachingAssistant.Helpers.Converters
{
    public class RowHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Check for null or insufficient values
            if (values.Length < 2 || values[0] == null || values[1] == null)
                return double.NaN; // Default to auto row height

            // Safely cast the values
            if (values[0] is double totalHeight && values[1] is int rowCount)
            {
                if (rowCount == 0)
                    return double.NaN; // Avoid division by zero

                double headerHeight = 30; // Approximate height of the DataGrid header
                return (totalHeight - headerHeight) / rowCount;
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
