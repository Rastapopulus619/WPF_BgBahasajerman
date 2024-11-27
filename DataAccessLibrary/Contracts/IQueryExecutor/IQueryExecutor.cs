using System.Data;

namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IQueryExecutor
    {
        Task<T> ExecuteQuerySingleAsync<T>(string queryName, object parameters = null);
        Task<List<T>> ExecuteQueryAsync<T>(string queryName, object parameters = null);
        Task<DataTable> ExecuteQueryAsDataTableAsync(string queryName, object parameters = null);
        Task<int> ExecuteCommandAsync(string queryName, object parameters = null);
    }
}