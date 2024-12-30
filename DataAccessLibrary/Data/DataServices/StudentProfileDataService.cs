using Bgb_DataAccessLibrary.Contracts.IDataAccess;
using Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryLoaders;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public class StudentProfileDataService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        public StudentProfileDataService(IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
        }


    }
}
