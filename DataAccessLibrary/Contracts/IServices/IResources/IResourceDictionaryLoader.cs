namespace Bgb_DataAccessLibrary.Contracts.IServices.IResources
{
    public interface IResourceDictionaryLoader
    {
        IReadOnlyDictionary<string, object> GetMergedResources();
    }
}