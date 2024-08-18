namespace Bgb_DataAccessLibrary.Databases
{
    public interface IDataAccess
    {
        Task<int> ExecuteAsync(string sql, object parameters = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);

        // Optional: Add more methods based on specific needs
    }
}