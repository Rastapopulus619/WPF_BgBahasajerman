
namespace Bgb_DataAccessLibrary.Models.Interfaces
{
    public interface IPageDescriptor
    {
        string Icon { get; set; }
        string Name { get; set; }
        Type ViewModelType { get; set; }
    }
}