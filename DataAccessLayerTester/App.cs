using Bgb_DataAccessLibrary.Data.DataServices;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Contracts.IDatabases;
using Bgb_DataAccessLibrary.Contracts.IQueryExecutor;
using Bgb_DataAccessLibrary.Contracts.IQueryLoaders;
using Bgb_DataAccessLibrary.Contracts.IMessages;

namespace DataAccessLayerTester
{
    public class App
    {
        private readonly IMessages _messages;
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;
        private readonly IQueryExecutor _queryExecutor;
        public App(IMessages messages, IDataAccess dataAccess, IQueryLoader queryLoader, IQueryExecutor queryExecutor)
        {
            _messages = messages;
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
            _queryExecutor = queryExecutor;

            ShowMessages();
        }
        public void Run()
        {
            string queryTest = _queryLoader.GetQuery("GetStudentList");

            //var data = _dataAccess.QueryAsync<dynamic>("SELECT * FROM students").Result;
            var dataList1 = _dataAccess.QueryAsync<dynamic>(_queryLoader.GetQuery("GetStudentList")).Result.ToList();
            //var dataList = _dataAccess.QueryAsync<dynamic>("SELECT * FROM students").Result.ToList();
            Console.WriteLine("Trying to get data with stored queries: ");
            Console.WriteLine();
            Console.WriteLine($"ID: {dataList1[36].StudentID}, Number: {dataList1[36].StudentNumber}, Name: {dataList1[36].Name}, Title: {dataList1[36].Title ?? "No Title"}");
            //Console.WriteLine($"ID: {dataList[36].StudentID}, Number: {dataList[36].StudentNumber}, Name: {dataList[36].Name}, Title: {dataList[36].Title ?? "No Title"}");

            //foreach (var row in data)
            //{
            //    Console.WriteLine($"ID: {row.StudentID}, Number: {row.StudentNumber}, Name: {row.Name}, Title: {row.Title ?? "No Title"}");
            //}

            // This is using a strongly-typed class model to capture the data:

            //var dataWithModel = _dataAccess.QueryAsync<Student>("SELECT * FROM Students").Result;
            //var dataWithModelDataList = _dataAccess.QueryAsync<Student>("SELECT * FROM Students").Result.ToList();

            //Console.WriteLine($"ID: {dataWithModelDataList[36].StudentID}, Number: {dataWithModelDataList[36].StudentNumber}, Name: {dataWithModelDataList[36].Name}, Title: {dataWithModelDataList[36].Title ?? "No Title"}");

            //foreach (var student in dataWithModel)
            //{
            //    Console.WriteLine($"ID: {student.StudentID}, Number: {student.StudentNumber}, Name: {student.Name}, Title: {student.Title}");
            //}
            TestDataServiceAsync();
        Console.ReadLine();


        }

        
    public async Task TestDataServiceAsync()
        {
            DataServiceTestClass dataService = new DataServiceTestClass(_dataAccess, _queryLoader);
            // Await the asynchronous method
            string name = await dataService.GetStudentNameByStudentID(1.ToString());

            // Now `name` is the result of the async call, and you can use it as a string.
            Console.WriteLine($"Retrieved Name: {name}");
        }

    private void ShowMessages()
        {
            Console.WriteLine(_messages.SayHello());
            Console.WriteLine(_messages.SayGoodbye());
            Console.WriteLine();
        }
    }
}
