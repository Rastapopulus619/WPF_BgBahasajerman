using BgB_TeachingAssistant.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

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
        private Style LoadStyle(string styleKey)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Views/Resources/Styles/DataGrid/DataGridCellStyles.xaml", UriKind.Relative)
            };
            return (Style)resourceDictionary[styleKey];
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BookedSlotsViewModel viewModel)
            {
                // Load styles from the local ResourceDictionary
                var defaultStyle = LoadStyle("DayCellStyle");
                var alternateStyle = LoadStyle("AlternateDayCellStyle");

                // Pass styles to ViewModel
                viewModel.DefaultCellStyle = defaultStyle;
                viewModel.AlternateCellStyle = alternateStyle;

                // Initialize the current style
                viewModel.CurrentCellStyle = defaultStyle;
            }
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












        // Button Click Event Handler
        private void ToggleShowLevelsButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is BookedSlotsViewModel viewModel)
            {
                // Toggle the ShowLevelsEnabled property
                viewModel.ShowLevelsEnabled = !viewModel.ShowLevelsEnabled;

                // Force layout updates
                TimetableDataGrid.UpdateLayout();

                foreach (var item in TimetableDataGrid.Items)
                {
                    var row = TimetableDataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        foreach (var column in TimetableDataGrid.Columns)
                        {
                            var cell = GetCell(row, column);
                            if (cell != null)
                            {
                                UpdateCellStyle(cell, column, viewModel.ShowLevelsEnabled);
                            }
                        }
                    }
                }

                // Force the DataGrid to refresh styles
                TimetableDataGrid.Dispatcher.Invoke(() =>
                {
                    TimetableDataGrid.UpdateLayout();
                }, System.Windows.Threading.DispatcherPriority.Render);
            }
        }


        // Update Cell Style Dynamically
        private void UpdateCellStyle(DataGridCell cell, DataGridColumn column, bool showLevelsEnabled)
        {
            Console.WriteLine($"Cell DataContext: {cell.DataContext}");

            if (cell.DataContext is TimeTableRow row)
            {
                // Get the SlotEntry object corresponding to the column
                SlotEntry? slotEntry = column.Header switch
                {
                    "Montag" => row.Montag,
                    "Dienstag" => row.Dienstag,
                    "Mittwoch" => row.Mittwoch,
                    "Donnerstag" => row.Donnerstag,
                    "Freitag" => row.Freitag,
                    "Samstag" => row.Samstag,
                    "Sonntag" => row.Sonntag,
                    _ => null
                };

                // Store the original XAML-defined style
                var originalStyle = LoadStyle("DayCellStyle");

                if (showLevelsEnabled && slotEntry != null)
                {
                    // Clone the original style
                    var updatedStyle = new Style(typeof(DataGridCell), originalStyle);

                    // Apply styles dynamically based on the Level property
                    switch (slotEntry.Level)
                    {
                        case "A1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightCoral));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Maroon));
                            break;
                        case "A2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightGreen));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.DarkGreen));
                            break;
                        case "B1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightBlue));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.DarkBlue));
                            break;
                        case "B2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightYellow));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Orange));
                            break;
                        case "C1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightPink));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.DarkRed));
                            break;
                        case "C2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Plum));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Purple));
                            break;
                        case "Gespräch":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightGray));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Gray));
                            break;
                        case "Prüfungstraining A1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightCoral));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Maroon));
                            break;
                        case "Prüfungstraining A2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightGreen));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.DarkGreen));
                            break;
                        case "Prüfungstraining B1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightBlue));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.DarkBlue));
                            break;
                        case "Prüfungstraining B2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightYellow));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Orange));
                            break;
                        case "Prüfungstraining C1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightPink));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.DarkRed));
                            break;
                        case "Prüfungstraining C2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Plum));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Purple));
                            break;
                        case "Prüfungstraining DaF":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.LightGray));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Black));
                            break;
                        default:
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, Brushes.Transparent));
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.Black));
                            break;
                    }

                    // Apply the dynamically updated style
                    cell.Style = updatedStyle;
                }
                else
                {
                    // Revert to the original XAML-defined style
                    cell.Style = originalStyle;
                    Console.WriteLine($"Reverted to original style for column {column.Header}");
                }
            }
        }








        // Get DataGridCell for a Row and Column
        private DataGridCell GetCell(DataGridRow row, DataGridColumn column)
        {
            var presenter = FindVisualChild<DataGridCellsPresenter>(row);
            if (presenter != null)
            {
                var cell = presenter.ItemContainerGenerator.ContainerFromIndex(column.DisplayIndex) as DataGridCell;
                Console.WriteLine(cell == null ? "Cell not found" : $"Cell found for column: {column.Header}");
                return cell;
            }
            Console.WriteLine("Presenter not found for row");
            return null;
        }


        // Find a Visual Child in the Visual Tree
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                {
                    return tChild;
                }

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

    }
}