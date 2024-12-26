using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BgB_TeachingAssistant.Views
{
    /// <summary>
    /// Interaction logic for BookedSlotsView.xaml
    /// </summary>
    public partial class BookedSlotsView : UserControl
    {
        public BookedSlotsView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TimetableDataGrid.Focus();

        }

        // if the above doesn't work, try this:
        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Dispatcher.InvokeAsync(() => TimetableDataGrid.Focus(), System.Windows.Threading.DispatcherPriority.ContextIdle);
        //}

        private void StreamlineEditOnSelect(object sender, EventArgs e)
        {
            if (sender is not DataGrid dataGrid)
            {
                // Exit if sender is not a DataGrid
                return;
            }

            // Ensure the CurrentCell and its properties are valid
            if (dataGrid.CurrentCell == null || dataGrid.CurrentCell.Item == null || dataGrid.CurrentCell.Column == null)
            {
                // Exit gracefully if CurrentCell is invalid
                return;
            }

            try
            {
                // Automatically enter edit mode
                dataGrid.BeginEdit();

                // Get the current cell's content presenter
                var contentPresenter = dataGrid.CurrentCell.Column.GetCellContent(dataGrid.CurrentCell.Item) as ContentPresenter;

                if (contentPresenter != null)
                {
                    // Use VisualTreeHelper to find the TextBox or other editable element
                    var editingElement = FindChild<TextBox>(contentPresenter);

                    if (editingElement != null)
                    {
                        // Set focus to the editing element
                        editingElement.Focus();
                        editingElement.CaretIndex = editingElement.Text.Length;

                        // Optional: Auto-select all content
                        if(editingElement.Text == "-")
                        {
                            editingElement.SelectAll();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle gracefully to prevent crashes
                Console.WriteLine($"Error in StreamlineEditOnSelect: {ex.Message}");
            }
        }


        // Helper method to find a specific child element type in the visual tree
        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T desiredChild)
                {
                    return desiredChild;
                }

                var foundChild = FindChild<T>(child);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }

            return null;
        }


    }
}