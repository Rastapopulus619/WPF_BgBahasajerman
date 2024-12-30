using System.Windows;
using System.Windows.Controls;

namespace BgB_TeachingAssistant.Helpers.AttachedProperties
{
    public static class BookedSlotsVisibilityBehavior
    {
        public static bool GetIsContentVisible(DependencyObject obj) =>
            (bool)obj.GetValue(IsContentVisibleProperty);

        public static void SetIsContentVisible(DependencyObject obj, bool value) =>
            obj.SetValue(IsContentVisibleProperty, value);

        public static readonly DependencyProperty IsContentVisibleProperty =
            DependencyProperty.RegisterAttached(
                "IsContentVisible",
                typeof(bool),
                typeof(BookedSlotsVisibilityBehavior),
                new PropertyMetadata(true, OnIsContentVisibleChanged));

        private static void OnIsContentVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                textBlock.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}