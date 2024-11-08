﻿using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Models.StudentModels;
using Bgb_DataAccessLibrary.QueryExecutor;
using Bgb_DataAccessLibrary.QueryLoaders;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using MySqlX.XDevAPI.Common;
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
        private readonly IQueryExecutor _queryExecutor;

        public DataServiceTestClass(IDataAccess dataAccess, IQueryLoader queryLoader, IQueryExecutor queryExecutor)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            _queryExecutor = queryExecutor;
        }
        public async Task ProcessData(IEventAggregator eventAggregator, GeneralDataService dataService)
        {
            List<string> studentNames = await dataService.GetStudentNamesAsync();
            string processedData = "Processed data at " + DateTime.Now;
            eventAggregator.Publish(processedData);
        }
        public async Task<string> ProcessDataGetSingle()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            var students = await _dataAccess.QueryAsync<StudentModel>(query);
            return students.SingleOrDefault().Name;
        }

        public async Task<List<string>> ProcessDataGetList()
        {
            var query = _queryLoader.GetQuery("GetStudentList");
            var students = await _dataAccess.QueryAsync<StudentModel>(query);
            return students.Select(s => s.Name).ToList();
        }

        public async Task<string> GetStudentNameByStudentID(string studentID)
        {
            return await _queryExecutor.ExecuteQuerySingleAsync<string>("GetStudentNameByStudentID", new { StudentID = studentID });
            //var query = _queryLoader.GetQuery("GetStudentList");
            //var students = await _dataAccess.QueryAsync<StudentModel>(query);
            //return students.Select(s => s.Name).ToList().FirstOrDefault();
        }

    }
}
