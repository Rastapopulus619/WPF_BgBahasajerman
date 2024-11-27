using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Bgb_DataAccessLibrary.Contracts;
using Bgb_DataAccessLibrary.QueryLoaders;

namespace Bgb_DataAccessLibrary.QueryExecutor
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IDataAccess _dataAccess;
        private readonly IQueryLoader _queryLoader;

        public QueryExecutor(IDataAccess dataAccess, IQueryLoader queryLoader)
        {
            _dataAccess = dataAccess;
            _queryLoader = queryLoader;
        }
        public async Task<T> ExecuteQuerySingleAsync<T>(string queryName, object parameters = null)
        {
            string query = _queryLoader.GetQuery(queryName);
            var result = await _dataAccess.QueryAsync<T>(query, parameters);
            return result.SingleOrDefault();
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string queryName, object parameters = null)
        {
            string query = _queryLoader.GetQuery(queryName);
            var result = await _dataAccess.QueryAsync<T>(query, parameters);
            return result.ToList();
        }
        public async Task<DataTable> ExecuteQueryAsDataTableAsync(string queryName, object parameters = null)
        {
            string query = _queryLoader.GetQuery(queryName);
            return await _dataAccess.GetDataTableAsync(query, parameters);
        }

        public async Task<int> ExecuteCommandAsync(string queryName, object parameters = null)
        {
            string query = _queryLoader.GetQuery(queryName);
            return await _dataAccess.ExecuteAsync(query, parameters);
        }
    }
}