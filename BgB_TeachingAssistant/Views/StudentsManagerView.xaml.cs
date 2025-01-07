using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BgB_TeachingAssistant.Views
{
    public partial class StudentsManagerView : UserControl
    {
        public StudentsManagerView()
        {
            InitializeComponent();
        }
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            // Ensure DataGrid is not null
            if (dataGrid != null)
            {
                // Check if ItemsSource implements INotifyCollectionChanged
                if (dataGrid.ItemsSource is INotifyCollectionChanged collection)
                {
                    collection.CollectionChanged += (s, args) =>
                    {
                        // Trigger scrolling when items are loaded
                        ScrollToLastItem(dataGrid);
                    };
                }
                else
                {
                    // Fallback: Trigger scrolling when data binding is complete
                    dataGrid.Dispatcher.InvokeAsync(() =>
                    {
                        ScrollToLastItem(dataGrid);
                    }, System.Windows.Threading.DispatcherPriority.DataBind);
                }
            }
        }
        private void DataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            // Ensure the sender is a DataGrid
            if (sender is DataGrid dataGrid && dataGrid.Items.Count > 0)
            {
                // Scroll to the last item
                var lastItem = dataGrid.Items[dataGrid.Items.Count - 1];
                dataGrid.ScrollIntoView(lastItem);
            }
        }


        private void ScrollToLastItem(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count > 0)
            {
                // Scroll to the last item in the DataGrid
                var lastItem = dataGrid.Items[dataGrid.Items.Count - 1];
                dataGrid.ScrollIntoView(lastItem);
            }
        }


    }
}
