using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.QueryLoaders;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerTester
{
    public class App
    {
        private readonly IMessages _messages;
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;

        public App(IMessages messages, IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            _messages = messages;
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;

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


            Console.ReadLine();


        }
        private void ShowMessages()
        {
            Console.WriteLine(_messages.SayHello());
            Console.WriteLine(_messages.SayGoodbye());
            Console.WriteLine();
        }
    }
}
