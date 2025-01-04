using Bgb_DataAccessLibrary.Contracts.IServices.IData;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;
using BgB_TeachingAssistant.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class StudentViewModel : ViewModelBase
    { 
        public override string Name => "Student";
        public IGeneralDataService GeneralDataService { get; set; }
        public ICommand DanCukCommand { get; }
        public ICommand LoadStudentsCommand { get; }


        private ObservableCollection<StudentModel> _students;
        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value, nameof(Students));  // Provide the property name
        }

        private ObservableCollection<string> _studentNames;
        public ObservableCollection<string> StudentNames
        {
            get => _studentNames;
            set => SetProperty(ref _studentNames, value, nameof(StudentNames));
        }
        private ICollectionView _filteredItems;
        public ICollectionView FilteredItems
        {
            get => _filteredItems;
            set => SetProperty(ref _filteredItems, value);
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (SetProperty(ref _filterText, value))
                {
                    FilteredItems.Refresh();
                }
            }
        }
        public StudentViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            ServiceFactory.ConfigureServicesFor(this);

            // Trigger asynchronous initialization from the constructor
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // Initialize the Students collection
            Students = new ObservableCollection<StudentModel>();

            // Retrieve data and populate the collection
            await LoadStudentsAsync();

            // Set up the filtered view
            FilteredItems = CollectionViewSource.GetDefaultView(Students);
            FilteredItems.Filter = FilterLogic;
        }

        private async Task LoadStudentsAsync()
        {
            try
            {
                // Retrieve students using the data service
                var students = await GeneralDataService.GetStudentsAsync();

                // Update the existing Students collection
                if (Students != null)
                {
                    Students.Clear();
                    foreach (var student in students)
                    {
                        Students.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional)
                Console.WriteLine($"Error loading students: {ex.Message}");
            }
        }

        private bool FilterLogic(object item)
        {
            if (item is StudentModel student)
            {
                return string.IsNullOrEmpty(FilterText) ||
                       student.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private async void DanCukMethod()
            {
                try
                {
                    // Retrieve and set student names
                    var studentNames = await GeneralDataService.GetStudentNamesAsync();
                    Console.WriteLine($"first value in student list: {studentNames[0]}");
                    //MessageBox.Show($"first value in student list: {studentNames[0]}");
                    StudentNames = new ObservableCollection<string>(studentNames);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving student list: {ex.Message}");
                }
            }
    }
}
