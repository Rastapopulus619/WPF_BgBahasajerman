using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BgB_TeachingAssistant.ViewModels
{
    public class PackageViewModel : BaseViewModel
    {
        public override string Name => "Student";
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;

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
        public PackageViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            PopulateStudentList();

        }

        private async void PopulateStudentList()
        {
            try
            {
                // Retrieve all student names

                // with dynamic type:
                //var result = _dataAccess.QueryAsync<dynamic>(_queryLoader.GetQuery("GetStudentList")).Result.ToList();

                // Convert the result to a list of strings
                //List<string> studentNames = result
                //    .Select(item => (string)item.Name)  // Assuming the dynamic object has a property called StudentName
                //    .ToList();


                // Retrieve all student names
                List<StudentModel> studentList = await GetStudentsAsync();

                // Convert the list of students to a list of names
                List<string> studentNames = studentList.Select(s => s.Name).ToList();

                // Set the StudentNames property to update the ComboBox
                StudentNames = new ObservableCollection<string>(studentNames);


                //MessageBox.Show("Student list loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving student list: {ex.Message}");
            }

        }
        private async Task<List<StudentModel>> GetStudentsAsync()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            return (await _dataAccess.QueryAsync<StudentModel>(query)).ToList();
        }
    }
}
