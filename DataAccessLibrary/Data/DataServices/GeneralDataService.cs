using Bgb_DataAccessLibrary.Models.StudentModels;
using System.Data;
using Bgb_DataAccessLibrary.Helpers.Conversion;
using Bgb_DataAccessLibrary.Events;
using Bgb_DataAccessLibrary.Contracts;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public class GeneralDataService : IGeneralDataService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        private readonly IQueryExecutor _queryExecutor;
        private readonly IEventAggregator _eventAggregator;


        public GeneralDataService(IDataAccess dataAccess, IQueryLoader queryLoader, IQueryExecutor queryExecutor, IEventAggregator eventAggregator)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            _queryExecutor = queryExecutor;
            _eventAggregator = eventAggregator;
        }

        public async Task<List<StudentModel>> GetStudentsAsync()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            return (await _dataAccess.QueryAsync<StudentModel>(query)).ToList();
        }
        public async Task<List<string>> GetStudentNamesAsync()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            var students = await _dataAccess.QueryAsync<StudentModel>(query);
            return students.Select(s => s.Name).ToList();
        }
        public async Task populateStudentPicker()
        {
            DataTable dt = await _queryExecutor.ExecuteQueryAsDataTableAsync("Get_AllStudents_IDsAndNames");
            StudentDataConverter studentDataConverter = new StudentDataConverter();
            List<IStudentPickerStudentModel> studentList = studentDataConverter.ConvertDataTableToStudentDTOList(dt);
            _eventAggregator.Publish(new PopulateStudentPickerEvent(studentList));

        }

    }
}