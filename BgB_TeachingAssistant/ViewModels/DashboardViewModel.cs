using Bgb_DataAccessLibrary.Factories;

namespace BgB_TeachingAssistant.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        // Constructor for injecting the factory
        public DashboardViewModel(IServiceFactory serviceFactory)
            : base(serviceFactory)
        {
        }

        // Override the Name property from IPageViewModel
        public override string Name => "Dashboard";
    }
}
