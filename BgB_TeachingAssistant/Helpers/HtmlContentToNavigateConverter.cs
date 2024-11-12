using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace BgB_TeachingAssistant.Views
{
    public class HtmlContentToNavigateConverter : IValueConverter
    {
        private const string TemporaryDirectory = @"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\BgB_TeachingAssistant\HtmlContent\TemporaryFiles\";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string htmlContent)
            {
                // Ensure the directory exists
                if (!Directory.Exists(TemporaryDirectory))
                {
                    Directory.CreateDirectory(TemporaryDirectory);
                }

                // Generate a unique file name based on the current timestamp
                string fileName = "temp_" + DateTime.Now.Ticks + ".html";
                string filePath = Path.Combine(TemporaryDirectory, fileName);

                try
                {
                    // Write the HTML content to the temporary file
                    File.WriteAllText(filePath, htmlContent);

                    // Return the Uri of the newly created file
                    return new Uri(filePath);
                }
                catch (Exception ex)
                {
                    // Handle any errors that might occur during file creation
                    Console.WriteLine($"Error writing HTML to temporary file: {ex.Message}");
                    return null;
                }
            }

            return null; // Return null if the value is not a valid HTML string
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
