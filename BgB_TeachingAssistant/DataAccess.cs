using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace BgB_TeachingAssistant
{
    public class DataAccess
    {
        private readonly string _connectionString;
        private readonly Dictionary<string, string> _queries = new Dictionary<string, string>();

        public DataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySQL");
            LoadQueries();
        }
        private void LoadQueries()
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "BgB_Queries");
            LoadQueriesFromDirectory(basePath);
        }
        private void LoadQueriesFromDirectory(string directoryPath)
        {
            var sqlFiles = Directory.GetFiles(directoryPath, "*.sql", SearchOption.AllDirectories);

            foreach (var file in sqlFiles)
            {
                var key = Path.GetFileNameWithoutExtension(file);
                var query = File.ReadAllText(file);
                if (!_queries.ContainsKey(key))
                {
                    _queries.Add(key, query);
                }
                else
                {
                    // Handle duplicate keys if necessary
                    // For example, log a warning or throw an exception
                }
            }
        }
        public string GetQuery(string queryName)
        {
            return _queries.ContainsKey(queryName) ? _queries[queryName] : null;
        }

        public IDbConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public int GetStudentID(string studentName)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(_connectionString))
                {
                    var output = connection.Query<int>("SELECT StudentID FROM students WHERE Name = @Name", new { Name = studentName }).FirstOrDefault();
                    return output;
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                return -1; // Return an appropriate default value or handle accordingly
            }
        }

        private void LogException(Exception ex)
        {
            string logDirectory = @"C:\Programmieren\bgbahasajermanApp_Combo\WPF_BgBahasajerman\ErrorLogs";

            // Ensure the directory exists
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string logFilePath = Path.Combine(logDirectory, "log.txt");
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {ex.Message}{Environment.NewLine}");
        }
    }
}
