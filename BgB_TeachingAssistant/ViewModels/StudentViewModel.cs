using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary.QueryLoaders;
using BgB_TeachingAssistant.Commands;

namespace BgB_TeachingAssistant.ViewModels
{
    public class StudentViewModel : ObservableObject, IPageViewModel
    {
        public string Name => "Students";
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        public ICommand DanCukCommand { get; }
        public ICommand LoadStudentsCommand { get; }

        public StudentViewModel(IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            LoadStudentsCommand = new RelayCommand(async () => await LoadStudentsAsync());
            DanCukCommand = new RelayCommand(DanCukMethod);
        }

        private ObservableCollection<StudentModel> _students;
        public ObservableCollection<StudentModel> Students
        {
            get => _students;
            set => SetProperty(ref _students, value, nameof(Students));  // Provide the property name
        }

        private async Task LoadStudentsAsync()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            var students = await _dataAccess.QueryAsync<StudentModel>(query);
            Students = new ObservableCollection<StudentModel>(students.ToList());
        }
        private void DanCukMethod()
        {
            // Implement what you want to happen on the ButtonDanCuk click
        }
    }
}
