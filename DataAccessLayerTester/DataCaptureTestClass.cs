// DataAccessTesterConsoleApp/MockDataAccess.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Contracts.ILogger;
using Bgb_DataAccessLibrary.DataAccess.Databases;

namespace DataAccessLayerTester
{
    public class MockLoggerService : ILoggerService
    {
        public void Log(string message) { }
        public void LogException(Exception ex) { }
    }
    public class MockDataAccess : MySqlDataAccess
    {
        public MockDataAccess(string connectionString) : base(connectionString, new MockLoggerService()) { }

        //public override Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        //{
        //    // Return mock data
        //    return Task.FromResult<IEnumerable<T>>(new List<T>());
        //}

        //public override Task<int> ExecuteAsync(string sql, object parameters = null)
        //{
        //    // Mock execution
        //    return Task.FromResult(1);
        //}
    }
}
