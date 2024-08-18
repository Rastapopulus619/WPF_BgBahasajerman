namespace Bgb_DataAccessLibrary.QueryExecutor
{
    public interface IQueryExecutor
    {
        Task<int> ExecuteCommandAsync(string queryName, object parameters = null);
        Task<List<T>> ExecuteQueryAsync<T>(string queryName, object parameters = null);
        Task<T> ExecuteQuerySingleAsync<T>(string queryName, object parameters = null);
    }
}