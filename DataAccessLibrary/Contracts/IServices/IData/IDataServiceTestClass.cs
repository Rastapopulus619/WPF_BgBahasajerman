using Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventAggregators;
using Bgb_DataAccessLibrary.Data.DataServices;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IData
{
    public interface IDataServiceTestClass
    {
        public Task ProcessData(IEventAggregator eventAggregator, GeneralDataService dataService);
        public Task<List<string>> ProcessDataGetList();
        public Task<string> ProcessDataGetSingle();
        //public Task<string> GetStudentNameByStudentID(string studentID);
        public Task GetStudentNameByStudentID(string studentID);
    }
}