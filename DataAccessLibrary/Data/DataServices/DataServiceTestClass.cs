﻿using Bgb_DataAccessLibrary.Events;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Contracts;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public class DataServiceTestClass : IDataServiceTestClass
    {
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        private readonly IQueryExecutor _queryExecutor;
        private readonly IEventAggregator _eventAggregator;

        public DataServiceTestClass(IDataAccess dataAccess, IQueryLoader queryLoader, IQueryExecutor queryExecutor, IEventAggregator eventAggregator)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            _queryExecutor = queryExecutor;
            _eventAggregator = eventAggregator;
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

        //public async Task<string> GetStudentNameByStudentID(string studentID)
        public async Task GetStudentNameByStudentID(string studentID)
        {
            string studentName = await _queryExecutor.ExecuteQuerySingleAsync<string>("GetStudentNameByStudentID", new { StudentID = studentID });

            // Publish the event with the strongly-typed event class
            _eventAggregator.Publish(new StudentNameByIDEvent(studentName));
            //return studentName;
        }


    }
}
