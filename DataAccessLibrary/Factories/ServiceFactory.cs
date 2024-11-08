using Microsoft.Extensions.DependencyInjection; // Add this using directive
using Bgb_DataAccessLibrary.Data.DataServices; // Ensure this is correct
using Bgb_DataAccessLibrary.Models.Interfaces;
using Bgb_DataAccessLibrary.Factories; // Ensure this is correct


namespace Bgb_DataAccessLibrary.Factories
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public void ConfigureServicesFor(object viewModel)
        {
            if (viewModel is IPageViewModel pageViewModel)
            {
                string viewModelName = pageViewModel.Name;

                switch (viewModelName)
                {
                    case "Test1":
                        if (viewModelName == "Test1")
                        {
                            pageViewModel.DataService = _serviceProvider.GetService<IDataServiceTestClass>();
                        }
                        break;

                    // Add more cases for other view models
                    default:
                        throw new Exception($"ViewModel with Name '{viewModelName}' not recognized.");
                }
            }
            else { Console.WriteLine("viewModel is not a IPageViewModel"); }
        }
        public GeneralDataService CreateGeneralDataService() =>
            _serviceProvider.GetRequiredService<GeneralDataService>();

        public StudentProfileDataService CreateStudentProfileDataService() =>
            _serviceProvider.GetRequiredService<StudentProfileDataService>();

        // Other methods...
    }
}



