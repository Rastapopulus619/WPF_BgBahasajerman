using Bgb_DataAccessLibrary.Contracts.IModels.IDisplayModels;
using Bgb_DataAccessLibrary.Contracts.IServices.IData;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;
using BgB_TeachingAssistant.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class StudentsManagerViewModel : ViewModelBase
    {
        public override string Name => "StudentsManager";

        private ObservableCollection<StudentModel> _students;
        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value, nameof(Students)); // Provide the property name
        }

        private ObservableCollection<IStudentManagerDisplayStudentModel> _studentsCompleteData;
        public ObservableCollection<IStudentManagerDisplayStudentModel> StudentsCompleteData
        {
            get => _studentsCompleteData;
            set => SetProperty(ref _studentsCompleteData, value, nameof(StudentsCompleteData)); // Provide the property name
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

        private StudentModel _selectedStudent;

        public StudentModel SelectedStudent // has two-way binding with the DataGrid
        {
            get => _selectedStudent;
            set
            {
                if (SetProperty(ref _selectedStudent, value))
                {
                    OnSelectedStudentChanged();
                }
            }
        }
        private IStudentManagerDisplayStudentModel _selectedStudentDataBackup;
        private IStudentManagerDisplayStudentModel _selectedStudentData;

        public IStudentManagerDisplayStudentModel SelectedStudentData
        {
            get => _selectedStudentData;
            set
            {
                if (SetProperty(ref _selectedStudentData, value))
                {
                    if (_selectedStudentData != null && StudyStatuses != null)
                    {
                        // Attempt to find a matching StudyStatus
                        var matchingStatus = StudyStatuses.FirstOrDefault(status => status == _selectedStudentData.StudyStatus);

                        if (matchingStatus != null)
                        {
                            SelectedStudyStatus = matchingStatus;
                        }
                        else
                        {
                            // Throw an exception if no match is found
                            throw new InvalidOperationException($"The StudyStatus '{_selectedStudentData.StudyStatus}' does not exist in the available StudyStatuses.");
                        }
                    }
                    else
                    {
                        SelectedStudyStatus = null; // Reset or handle appropriately
                    }

                    // Perform additional logic if needed
                }
            }
        }

        private ObservableCollection<string> _studyStatuses = new ObservableCollection<string>();
        public ObservableCollection<string> StudyStatuses
        {
            get => _studyStatuses;
            set => SetProperty(ref _studyStatuses, value); // SetProperty comes from your ViewModelBase
        }
        private string _selectedStudyStatus;

        public string SelectedStudyStatus
        {
            get => _selectedStudyStatus;
            set
            {
                if (SetProperty(ref _selectedStudyStatus, value))
                {
                    SelectedStudentData.StudyStatus = _selectedStudyStatus;
                    LogSelectedStudentData();
                }
            }
        }
        public IGeneralDataService GeneralDataService { get; set; }
        public IStudentsManagerDataService StudentsManagerDataService { get; set; }

        public StudentsManagerViewModel(IServiceFactory serviceFactory) : base(serviceFactory)
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

            await InitializeStudyStatuses();
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
        public async Task InitializeStudyStatuses()
        {
            try
            {
                // Clear the collection before adding new items
                StudyStatuses.Clear();

                // Fetch data from the database
                StudyStatuses = await StudentsManagerDataService.GetStudyStatuses();
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., log or show error message
                Console.WriteLine($"Error fetching StudyStatuses: {ex.Message}");
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

        // Optional: Logic to handle when the selection changes
        private async Task OnSelectedStudentChanged()
        {
            // Example logic, e.g., logging or triggering some action
            Console.WriteLine($"Selected Student: {_selectedStudent?.Name}");

            SelectedStudentData = await StudentsManagerDataService.GetStudentDisplayDataByStudentID(_selectedStudent.StudentID);

            LogSelectedStudentData();
        }
        private void LogSelectedStudentData()
        {
            // Log some of the fetched data to the console
            Console.WriteLine($"Student ID: {SelectedStudentData.StudentID}");
            Console.WriteLine($"Student Number: {SelectedStudentData.StudentNumber}");
            Console.WriteLine($"Title: {SelectedStudentData.Title}");
            Console.WriteLine($"Name: {SelectedStudentData.Name}");
            Console.WriteLine($"Study Status: {SelectedStudentData.StudyStatus}");
            Console.WriteLine($"Goal: {SelectedStudentData.Goal}");
            Console.WriteLine($"Level: {SelectedStudentData.Level}");
            Console.WriteLine($"First Lesson: {SelectedStudentData.FirstLesson}");
            Console.WriteLine($"Lessons Total: {SelectedStudentData.LessonsTotal}");
            Console.WriteLine($"Last Lesson: {SelectedStudentData.LastLesson}");
            Console.WriteLine($"City of Residence: {SelectedStudentData.CityOfResidence}");
        }
    }
}
