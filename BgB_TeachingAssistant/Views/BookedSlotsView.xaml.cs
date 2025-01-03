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

            if (DataContext is BookedSlotsViewModel viewModel)
            {
                // Pass resource dictionary to the ViewModel
                var mergedResources = Resources.MergedDictionaries;
                viewModel.ResourceDictionaries = new List<ResourceDictionary>(mergedResources);
            }
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
        }

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
                // TimetableDataGrid.UpdateLayout();

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

                Style originalStyle = new Style();
                if (DataContext is BookedSlotsViewModel viewModel)
                {
                    // Store the original XAML-defined style
                    originalStyle = viewModel.DefaultCellStyle;
                }

                if (showLevelsEnabled && slotEntry != null)
                {
                    // Clone the original style
                    var updatedStyle = new Style(typeof(DataGridCell), originalStyle);

                    // Apply styles dynamically based on the Level property #359635
                    switch (slotEntry.Level)
                    {
                        case "A1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B3535")))); // red on gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "A2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#359635")))); // green on gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "B1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#927A07")))); // yellow on gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "B2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(95, 95, 85)))); // Slightly yellowish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "C1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(95, 85, 85)))); // Slightly pinkish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "C2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(105, 95, 105)))); // Slightly purplish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Gespräch":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#273DD0")))); // blue on grey
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Prüfungstraining A1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(95, 95, 95)))); // Slightly reddish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Prüfungstraining A2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(85, 95, 85)))); // Slightly greenish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Prüfungstraining B1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(85, 85, 95)))); // Slightly bluish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Prüfungstraining B2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(95, 95, 85)))); // Slightly yellowish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Prüfungstraining C1":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(95, 85, 85)))); // Slightly pinkish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Prüfungstraining C2":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(105, 95, 105)))); // Slightly purplish gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        case "Prüfungstraining DaF":
                            updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(95, 95, 95)))); // Neutral gray
                            updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
                            break;
                        default:
                            // updatedStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Color.FromRgb(53, 53, 53)))); // Default dark gray
                            // updatedStyle.Setters.Add(new Setter(TextElement.ForegroundProperty, Brushes.White));
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

        private bool isUsingDefaultStyle = true; // Track current style

        private void ToggleMittwochStylesButton_Click(object sender, RoutedEventArgs e)
        {
            // Load styles for TextBlock
            var defaultTextBlockStyle = LoadControlStyle("ValidationDependentCellStyle");
            var alternateTextBlockStyle = LoadControlStyle("AlternateTextBlockStyle");

            // Load styles for TextBox
            var defaultTextBoxStyle = LoadControlStyle("DataGridEditableTextBoxStyle");
            var alternateTextBoxStyle = LoadControlStyle("AlternateTextBoxStyle");

            if (defaultTextBlockStyle == null || alternateTextBlockStyle == null ||
                defaultTextBoxStyle == null || alternateTextBoxStyle == null)
            {
                Console.WriteLine("Error: Required styles not loaded.");
                return;
            }

            // Determine new styles based on toggle state
            var newTextBlockStyle = isUsingDefaultStyle ? alternateTextBlockStyle : defaultTextBlockStyle;
            var newTextBoxStyle = isUsingDefaultStyle ? alternateTextBoxStyle : defaultTextBoxStyle;

            isUsingDefaultStyle = !isUsingDefaultStyle;

            // Update ViewModel properties
            if (DataContext is BookedSlotsViewModel viewModel)
            {
                viewModel.MittwochTextBlockStyle = newTextBlockStyle; // Update TextBlock style
                viewModel.MittwochEditingStyle = newTextBoxStyle;    // Update TextBox style
            }

            TimetableDataGrid.Dispatcher.Invoke(() =>
            {
                TimetableDataGrid.UpdateLayout();
            }, System.Windows.Threading.DispatcherPriority.Render);

            Console.WriteLine($"Switched to TextBlock style: {GetStyleInfo(newTextBlockStyle)}");
            Console.WriteLine($"Switched to TextBox editing style: {GetStyleInfo(newTextBoxStyle)}");
        }





        private void ApplyStylesToCell(DataGridCell cell, Style textBlockStyle, Style textBoxStyle)
        {
            if (cell.Column is DataGridTemplateColumn column)
            {
                // Modify the CellTemplate (TextBlock)
                if (column.CellTemplate != null)
                {
                    var textBlock = FindChild<TextBlock>(cell.Content as ContentPresenter);
                    if (textBlock != null)
                    {
                        textBlock.Style = textBlockStyle;
                        Console.WriteLine($"Applied style to TextBlock: {GetStyleInfo(textBlockStyle)}");
                    }
                }

                // Modify the CellEditingTemplate (TextBox)
                if (column.CellEditingTemplate != null)
                {
                    var editingTemplate = column.CellEditingTemplate.LoadContent() as TextBox;
                    if (editingTemplate != null)
                    {
                        editingTemplate.Style = textBoxStyle;
                        Console.WriteLine($"Applied style to TextBox (edit mode): {GetStyleInfo(textBoxStyle)}");
                    }
                }
            }
        }




        private string GetStyleInfo(Style style)
        {
            if (style == null) return "None";

            // Search for the style key in the local ResourceDictionary
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Views/Resources/Styles/DataGrid/DataGridCellStyles.xaml", UriKind.Relative)
            };

            foreach (var key in resourceDictionary.Keys)
            {
                if (resourceDictionary[key] == style)
                {
                    return key.ToString();
                }
            }

            return $"TargetType: {style.TargetType.Name}";
        }

        private Style LoadControlStyle(string styleKey)
        {
            try
            {
                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("Views/Resources/Styles/DataGrid/DataGridControlStyles.xaml", UriKind.Relative)
                };

                return resourceDictionary[styleKey] as Style ?? throw new Exception($"Style '{styleKey}' not found in DataGridControlStyles.xaml.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading control style '{styleKey}': {ex.Message}");
                return null; // Return null if the style can't be loaded
            }
        }


    }
}