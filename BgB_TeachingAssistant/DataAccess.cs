using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace BgB_TeachingAssistant
{
    public class DataAccess
    {
        private readonly string _connectionString;
        public DataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySQL");
            //Checking whether the connection is fetched from appsetings.json
            //MessageBox.Show($"Connection String: {_connectionString}"); // Debug output
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
