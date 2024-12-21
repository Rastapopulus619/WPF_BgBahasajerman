using System.Windows;
using System.Windows.Controls;

namespace BgB_TeachingAssistant.Helpers.Converters
{
    public static class TextBoxHelper
    {
        public static void FocusAndSelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll(); // Select all text
                textBox.Focus();     // Set focus to the TextBox
            }
        }
    }
}
