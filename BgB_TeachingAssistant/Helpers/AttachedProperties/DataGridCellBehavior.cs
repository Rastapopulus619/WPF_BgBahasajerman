using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace BgB_TeachingAssistant.Helpers.AttachedProperties
{
    public static class DataGridCellBehavior
    {
        public static readonly DependencyProperty IsTriggerEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsTriggerEnabled",  // Name of the property
                typeof(bool),        // Property type
                typeof(DataGridCellBehavior), // Owner type
                new PropertyMetadata(false, OnIsTriggerEnabledChanged));


        public static void SetIsTriggerEnabled(DependencyObject obj, bool value)
        {
            Console.WriteLine($"SetIsTriggerEnabled called: {value}");
            obj.SetValue(IsTriggerEnabledProperty, value);
        }

        public static bool GetIsTriggerEnabled(DependencyObject obj)
        {
            var value = (bool)obj.GetValue(IsTriggerEnabledProperty);
            Console.WriteLine($"GetIsTriggerEnabled called: {value}");
            return value;
        }


        private static void OnIsTriggerEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridCell cell && e.NewValue is bool isEnabled)
            {
                Console.WriteLine($"IsTriggerEnabled changed: {isEnabled} for {cell.DataContext}");

                // Your dynamic styling logic
                var style = new Style(typeof(DataGridCell), cell.Style);

                if (isEnabled)
                {
                    style.Triggers.Add(new DataTrigger
                    {
                        Binding = new Binding("Level"),
                        Value = "A1",
                        Setters =
                        {
                            new Setter(Control.BackgroundProperty, Brushes.LightCoral),
                            new Setter(TextElement.ForegroundProperty, Brushes.Maroon)
                        }
                    });
                    style.Triggers.Add(new DataTrigger
                    {
                        Binding = new Binding("Level"),
                        Value = "A2",
                        Setters =
                        {
                            new Setter(Control.BackgroundProperty, Brushes.LightGreen),
                            new Setter(TextElement.ForegroundProperty, Brushes.DarkGreen)
                        }
                    });
                }

                cell.Style = style;
            }
            else
            {
                Console.WriteLine($"Unexpected DependencyObject type: {d.GetType().Name}");
            }
        }

    }
}