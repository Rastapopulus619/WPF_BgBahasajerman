using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Models.Interfaces;

namespace Bgb_DataAccessLibrary.Models.Interfaces
{
    public interface IPageViewModel
    {
            string Name { get; }
            public IDataServiceTestClass DataService { get; set; }
    }
}
