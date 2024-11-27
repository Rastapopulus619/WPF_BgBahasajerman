namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IPageDescriptor
    {
        string Icon { get; set; }
        string Name { get; set; }
        Type ViewModelType { get; set; }
    }
}