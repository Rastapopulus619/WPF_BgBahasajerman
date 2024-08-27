using Bgb_DataAccessLibrary.Data.DataServices;
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
        public override string Name => "Package";
        private readonly GeneralDataService _generalDataService;

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
            _generalDataService = serviceFactory.CreateGeneralDataService();
            LoadStudentList();
        }

        private async void LoadStudentList()
        {
            try
            {
                // Retrieve and set student names
                var studentNames = await _generalDataService.GetStudentNamesAsync();
                StudentNames = new ObservableCollection<string>(studentNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving student list: {ex.Message}");
            }
        }
        //private async Task<List<StudentModel>> GetStudentsAsync()
        //{
        //    var query = _queryLoader.GetQuery("GetStudentList");
        //    return (await _dataAccess.QueryAsync<StudentModel>(query)).ToList();
        //}
    }
}
