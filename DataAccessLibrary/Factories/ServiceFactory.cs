using Microsoft.Extensions.DependencyInjection; // Add this using directive
using Bgb_DataAccessLibrary.Data.DataServices; // Ensure this is correct
using Bgb_DataAccessLibrary.Models.Interfaces;
using Bgb_DataAccessLibrary.Factories; // Ensure this is correct
using System;
using System.Reflection;


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

        //public void ConfigureServicesFor(object viewModel)
        //{
        //    if (viewModel is IPageViewModel pageViewModel)
        //    {
        //        string viewModelName = pageViewModel.Name;

        //        switch (viewModelName)
        //        {
        //            case "Test1":
        //                //if (viewModelName == "Test1")
        //                if (viewModelName == "Test1")
        //                {
        //                    pageViewModel.DataService = _serviceProvider.GetService<IDataServiceTestClass>();
        //                }
        //                break;

        //            // Add more cases for other view models
        //            default:
        //                throw new Exception($"ViewModel with Name '{viewModelName}' not recognized.");
        //        }
        //    }
        //    else { Console.WriteLine("viewModel is not a IPageViewModel"); }
        //}
        public void ConfigureServicesFor(object viewModel)
        {
            if (viewModel is IPageViewModel pageViewModel)
            {
                var viewModelType = viewModel.GetType();

                // Iterate through the properties of the view model
                foreach (var property in viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    // Check if the property type is an interface and can be resolved by the service provider
                    if (property.CanWrite && property.PropertyType.IsInterface)
                    {
                        // Attempt to resolve the service from the service provider
                        var service = _serviceProvider.GetService(property.PropertyType);

                        if (service != null)
                        {
                            // Inject the service into the view model property
                            property.SetValue(viewModel, service);
                            // Confirmation message to the console
                            Console.WriteLine($"Injected {service.GetType().Name} into {viewModel.GetType().Name}.{property.Name}");
                        }
                        else
                        {
                            Console.WriteLine($"Service for {property.PropertyType.Name} not found.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("viewModel is not a IPageViewModel");
            }
        }
        public GeneralDataService CreateGeneralDataService() =>
            _serviceProvider.GetRequiredService<GeneralDataService>();

        public StudentProfileDataService CreateStudentProfileDataService() =>
            _serviceProvider.GetRequiredService<StudentProfileDataService>();

        // Other methods...
    }
}



