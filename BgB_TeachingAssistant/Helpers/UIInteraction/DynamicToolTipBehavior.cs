using System.Windows.Controls;
using System.Windows;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;
using Microsoft.Xaml.Behaviors;

namespace BgB_TeachingAssistant.Helpers.UIInteraction
{


    public class DynamicToolTipBehavior : Behavior<DataGridCell>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.DataContext is TimeTableRow row &&
                AssociatedObject.Column is DataGridColumn column)
            {
                // Resolve the property name from the column header
                string columnName = column.Header.ToString();
                var property = typeof(TimeTableRow).GetProperty(columnName);
                var value = property?.GetValue(row);

                // Set the ToolTip
                AssociatedObject.ToolTip = value?.ToString();
            }
        }
    }


}
