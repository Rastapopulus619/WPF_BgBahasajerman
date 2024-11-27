using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Logger;
using Bgb_DataAccessLibrary.Contracts;

namespace Bgb_DataAccessLibrary.Databases
{
    public class MySqlDataAccess : IDataAccess
    {
        private readonly string _connectionString;
        private readonly ILoggerService _logger;

        // Constructor for use with DI
        public MySqlDataAccess(IConfiguration config, ILoggerService logger)
        {
            _connectionString = config.GetConnectionString("MySQL");
            _logger = logger;
        }

        // Constructor for manual instantiation
        public MySqlDataAccess(string connectionString, ILoggerService logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(_connectionString))
                {
                    return await connection.QueryAsync<T>(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw; // rethrow the exception after logging
            }
        }
        public async Task<DataTable> GetDataTableAsync(string sql, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(_connectionString))
                {
                    var dataTable = new DataTable();
                    var reader = await connection.ExecuteReaderAsync(sql, parameters);
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw;
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            try
            {
                using (IDbConnection connection = new MySqlConnection(_connectionString))
                {
                    return await connection.ExecuteAsync(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw; // rethrow the exception after logging
            }
        }
    }
}