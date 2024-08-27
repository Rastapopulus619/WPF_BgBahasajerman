using Bgb_DataAccessLibrary.Factories;

namespace BgB_TeachingAssistant.ViewModels
{
    public class BaseViewModel : ObservableObject, IPageViewModel
    {
        protected readonly IServiceFactory ServiceFactory;

        // Constructor for injecting the factory
        public BaseViewModel(IServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }

        // Implement the Name property from IPageViewModel
        public virtual string Name { get; }
    }
}
