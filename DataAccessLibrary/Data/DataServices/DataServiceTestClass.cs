using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary.QueryLoaders;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public class DataServiceTestClass : IDataServiceTestClass
    {
        //*****
        //normal communication
        //Define delegate signature/format
        //public delegate void DataProcessedEventHandler(string data);
        //public event DataProcessedEventHandler DataProcessed;

        //public void ProcessData()
        //{
        //    string processedData = "Processed data at " + DateTime.Now;

        //*****
        //normal communication
        //DataProcessed?.Invoke(processedData);
        //}
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;

        public DataServiceTestClass(IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
        }
        public async Task ProcessData(IEventAggregator eventAggregator, GeneralDataService dataService)
        {
            List<string> studentNames = await dataService.GetStudentNamesAsync();
            string processedData = "Processed data at " + DateTime.Now;
            eventAggregator.Publish(processedData);
        }
        //public async Task<string> ProcessData()
        //{
        //    GeneralDataService dataService = new GeneralDataService();
        //    List<string> studentNames = await GeneralDataService.dataService.GetStudentNamesAsync();
        //    return studentNames.FirstOrDefault();
        //}

        public async Task<List<string>> ProcessDataGetList()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            var students = await _dataAccess.QueryAsync<StudentModel>(query);
            return students.Select(s => s.Name).ToList();
        }

        public async Task<string> ProcessDataGetSingle()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            var students = await _dataAccess.QueryAsync<StudentModel>(query);
            return students.Select(s => s.Name).ToList().FirstOrDefault();
        }

    }
}
