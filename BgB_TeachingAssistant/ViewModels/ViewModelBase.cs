using Bgb_DataAccessLibrary.Factories;

namespace BgB_TeachingAssistant.ViewModels
{
    public class ViewModelBase : ObservableObject, IPageViewModel
    {
        protected readonly IServiceFactory ServiceFactory;

        // Constructor for injecting the factory
        public ViewModelBase(IServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }

        // Implement the Name property from IPageViewModel
        public virtual string Name { get; }

    }
}
