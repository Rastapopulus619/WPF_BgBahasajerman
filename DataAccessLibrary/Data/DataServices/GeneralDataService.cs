using Bgb_DataAccessLibrary.QueryExecutor;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary.QueryLoaders;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public class GeneralDataService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;

        public GeneralDataService(IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
        }

        private async Task<List<StudentModel>> GetStudentsAsync()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            return (await _dataAccess.QueryAsync<StudentModel>(query)).ToList();
        }
    }
}