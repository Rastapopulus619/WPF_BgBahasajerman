using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Contracts;
using Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryExecutor;
using Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryLoaders;
using Bgb_DataAccessLibrary.Contracts.IDataAccess;
using Bgb_DataAccessLibrary.Contracts.IModels.IDisplayModels;
using Bgb_DataAccessLibrary.Contracts.IServices.IData;
using Bgb_DataAccessLibrary.Models.DTOs.StudentModelDTOs;
using Mysqlx.Session;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;
using System.Collections.ObjectModel;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public class StudentsManagerDataService : IStudentsManagerDataService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        private readonly IQueryExecutor _queryExecutor;

        public StudentsManagerDataService(IDataAccess dataAccess, IQueryLoader queryLoader, IQueryExecutor queryExecutor)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            _queryExecutor = queryExecutor;
        }

        public async Task<ObservableCollection<string>> GetStudyStatuses()
        {
            // Execute the query to get all student data
            DataTable dataTable = await _queryExecutor.ExecuteQueryAsDataTableAsync("GetStudyStatuses");
            ObservableCollection<string> studyStatuses = new();

            foreach (DataRow row in dataTable.Rows)
            {
                studyStatuses.Add(row.Field<string>("StudyStatus"));
            }

            return studyStatuses;
        }

        public async Task<ObservableCollection<IStudentManagerDisplayStudentModel>> GetAllStudentDisplayDataAsync()
        {
            // Execute the query to get all student data
            DataTable dataTable = await _queryExecutor.ExecuteQueryAsDataTableAsync("GetAllStudentsManagerData");

            // Convert DataTable rows to a collection of IStudentManagerDisplayStudentModel
            var students = new ObservableCollection<IStudentManagerDisplayStudentModel>(
                dataTable.AsEnumerable().Select(row => new StudentManagerDisplayStudentModel
                {
                    StudentID = row.Field<int>("StudentID"),
                    StudentNumber = row.Field<int>("StudentNumber"),
                    Name = row.Field<string>("Name") ?? "N/A",
                    Title = row.Field<string>("Title") ?? "N/A",
                    Goal = row.Field<string>("Goal") ?? "N/A",
                    Level = row.Field<string>("Level") ?? "N/A",
                    FirstLesson = row.Field<DateTime?>("FirstLesson") ?? DateTime.MinValue,
                    LessonsTotal = row.Field<int?>("LessonsTotal") ?? 0,
                    LastLesson = row.Field<DateTime?>("LastLesson") ?? DateTime.MinValue,
                    StudyStatus = row.Field<string>("StudyStatus") ?? "Unknown",
                    CityOfResidence = row.Field<string>("CityOfResidence") ?? "N/A",
                    CountryOfResidence = row.Field<string>("CountryOfResidence") ?? "N/A",
                    ProvinceOfResidence = row.Field<string>("ProvinceOfResidence") ?? "N/A",
                    AddressField = row.Field<string>("AddressField") ?? "N/A",
                    Email1 = row.Field<string>("Email1") ?? "N/A",
                    Email2 = row.Field<string>("Email2") ?? "N/A",
                    Tel1 = row.Field<string>("Tel1") ?? "N/A",
                    Tel2 = row.Field<string>("Tel2") ?? "N/A",
                    TelOrtu1 = row.Field<string>("TelOrtu1") ?? "N/A",
                    TelOrtu2 = row.Field<string>("TelOrtu2") ?? "N/A",
                    Age = row.Field<int?>("Age") ?? 0,
                    Gender = row.Field<string>("Gender") ?? "Unknown",
                    CountryOfOrigin = row.Field<string>("CountryOfOrigin") ?? "N/A",
                    ProvinceOfOrigin = row.Field<string>("ProvinceOfOrigin") ?? "N/A",
                    CityOfOrigin = row.Field<string>("CityOfOrigin") ?? "N/A"
                }));

            return students;
        }

        public async Task<IStudentManagerDisplayStudentModel> GetStudentDisplayDataByStudentID(int studentID)
        {
            // Parameter for the query
            var parameter = new { StudentID = studentID };

            // Execute the query and get the DataTable
            DataTable dataTable = await _queryExecutor.ExecuteQueryAsDataTableAsync("GetStudentsManagerDataByStudentID", parameter);

            // Check if we got a result
            if (dataTable.Rows.Count == 0)
                throw new InvalidOperationException($"No student found with StudentID: {studentID}");

            // Fetch the first row (assuming one result per StudentID)
            DataRow row = dataTable.Rows[0];

            // Create a new instance of StudentManagerDisplayStudentModel and map the data
            var studentDisplayData = new StudentManagerDisplayStudentModel
            {
                StudentID = row.Field<int>("StudentID"),
                StudentNumber = row.Field<int>("StudentNumber"),
                Name = row.Field<string>("Name") ?? "N/A",
                Title = row.Field<string>("Title") ?? "N/A",
                Goal = row.Field<string>("Goal") ?? "N/A",
                Level = row.Field<string>("Level") ?? "N/A",
                FirstLesson = row.Field<DateTime?>("FirstLesson") ?? DateTime.MinValue,
                LessonsTotal = row.Field<int?>("LessonsTotal") ?? 0,
                LastLesson = row.Field<DateTime?>("LastLesson") ?? DateTime.MinValue,
                StudyStatus = row.Field<string>("StudyStatus") ?? "Unknown",
                CityOfResidence = row.Field<string>("CityOfResidence") ?? "N/A",
                CountryOfResidence = row.Field<string>("CountryOfResidence") ?? "N/A",
                ProvinceOfResidence = row.Field<string>("ProvinceOfResidence") ?? "N/A",
                AddressField = row.Field<string>("AddressField") ?? "N/A",
                Email1 = row.Field<string>("Email1") ?? "N/A",
                Email2 = row.Field<string>("Email2") ?? "N/A",
                Tel1 = row.Field<string>("Tel1") ?? "N/A",
                Tel2 = row.Field<string>("Tel2") ?? "N/A",
                TelOrtu1 = row.Field<string>("TelOrtu1") ?? "N/A",
                TelOrtu2 = row.Field<string>("TelOrtu2") ?? "N/A",
                Age = row.Field<int?>("Age") ?? 0,
                Gender = row.Field<string>("Gender") ?? "Unknown",
                CountryOfOrigin = row.Field<string>("CountryOfOrigin") ?? "N/A",
                ProvinceOfOrigin = row.Field<string>("ProvinceOfOrigin") ?? "N/A",
                CityOfOrigin = row.Field<string>("CityOfOrigin") ?? "N/A"
            };

            return studentDisplayData;
        }
        //continue here:
        // add dataService to DI container
        // replace model with DI fetched model to keep it modular
        // implement in the viewModel to fetch studentData like this
        // delete the old attempts with DTOs!
    }
}
