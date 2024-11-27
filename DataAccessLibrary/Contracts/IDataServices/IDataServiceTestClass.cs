using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;

namespace Bgb_DataAccessLibrary.Contracts
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