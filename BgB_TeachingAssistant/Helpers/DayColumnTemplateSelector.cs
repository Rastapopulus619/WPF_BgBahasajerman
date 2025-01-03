using System.Windows;
using System.Windows.Controls;

namespace BgB_TeachingAssistant.Helpers
{
    public class DayColumnTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReadOnlyTemplate { get; set; }
        public DataTemplate EditableTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var cell = container as FrameworkElement;
            var isEditing = (cell?.TemplatedParent as DataGridCell)?.IsEditing ?? false;

            return isEditing ? EditableTemplate : ReadOnlyTemplate;
        }
    }
}