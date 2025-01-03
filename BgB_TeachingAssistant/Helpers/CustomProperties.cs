using System.Windows;
using System.Windows.Controls;

namespace BgB_TeachingAssistant.Helpers
{
    public static class CustomProperties
    {
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(CustomProperties),
                //new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.Inherits)
                new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.None)
            );

        public static void SetCornerRadius(UIElement element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(UIElement element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.RegisterAttached(
                "Icon",
                typeof(string),
                typeof(CustomProperties),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits)
            );

        public static void SetIconPath(UIElement element, string value)
        {
            element.SetValue(IconPathProperty, value);
        }

        public static string GetIconPath(UIElement element)
        {
            return (string)element.GetValue(IconPathProperty);
        }
    }
}