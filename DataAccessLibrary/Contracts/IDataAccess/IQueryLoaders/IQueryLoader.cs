namespace Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryLoaders
{
    public interface IQueryLoader
    {
        string GetQuery(string queryName);
    }
}