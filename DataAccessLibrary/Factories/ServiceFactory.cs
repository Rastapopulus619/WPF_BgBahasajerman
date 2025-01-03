using Microsoft.Extensions.DependencyInjection;
using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Logger;
using System;
using System.Reflection;
using Bgb_DataAccessLibrary.Helpers.ExtensionMethods;
using Bgb_DataAccessLibrary.Contracts;
using Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventAggregators;
using Bgb_DataAccessLibrary.Contracts.IServices.IResources;

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
            var service = _serviceProvider.GetService<T>();
            DependencyInjectionLogger.LogResolution(typeof(T), service);
            return service;
        }

        public object GetService(Type serviceType)
        {
            var service = _serviceProvider.GetService(serviceType);
            DependencyInjectionLogger.LogResolution(serviceType, service);
            return service;
        }
        public void ConfigureServicesFor(object viewModel)
        {
            if (viewModel is IPageViewModel pageViewModel)
            {
                var viewModelType = viewModel.GetType();
                string viewModelHashCode = viewModel.GetHashCode().ToString();

                // Iterate through the properties of the view model
                foreach (var property in viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    // Check if the property type is an interface and can be resolved by the service provider
                    if (property.CanWrite && property.PropertyType.IsInterface &&
                        property.PropertyType != typeof(IEventAggregator) &&
                        property.PropertyType != typeof(IServiceFactory))
                    {
                        // Attempt to resolve the service from the service provider
                        var service = _serviceProvider.GetService(property.PropertyType);

                        // Log the resolution
                        DependencyInjectionLogger.LogResolution(property.PropertyType, service);

                        if (service != null)
                        {
                            // Inject the service into the view model property
                            property.SetValue(viewModel, service);

                            // Confirmation message to the console
                            $"[SF] Injected: ".ColorizeMulti(ConsoleColor.DarkMagenta)
                                .Append($"{ service.GetType().Name}",ConsoleColor.Yellow).Append($" -> ({viewModelHashCode}) {viewModel.GetType().Name}.",ConsoleColor.DarkGray).Append($"{property.Name}",ConsoleColor.Yellow).WriteLine();
                        }
                        else
                        {
                            $"Service for {property.PropertyType.Name} not found.".Colorize(ConsoleColor.DarkGray);
                        }
                    }
                }
            }
            else
            {
                "viewModel is not a IPageViewModel".Colorize(ConsoleColor.Red);
            }

        }
        public void ConfigureViewModelBaseServices(IViewModelBase viewModel)
        {
                var viewModelType = viewModel.GetType();
                string viewModelHashCode = viewModel.GetHashCode().ToString();

                foreach (var property in viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (property.CanWrite && (property.PropertyType == typeof(IServiceFactory) || property.PropertyType == typeof(IEventAggregator) || property.PropertyType == typeof(IResourceDictionaryLoader)))
                {
                        var service = _serviceProvider.GetService(property.PropertyType);

                        DependencyInjectionLogger.LogResolution(property.PropertyType, service);

                        if (service != null)
                        {
                            property.SetValue(viewModel, service);

                            $"[SF] Injected: ".ColorizeMulti(ConsoleColor.DarkMagenta)
                                .Append($"{service.GetType().Name}", ConsoleColor.Yellow)
                                .Append($" -> ({viewModelHashCode}) {viewModel.GetType().Name}.", ConsoleColor.DarkGray)
                                .Append($"{property.Name}", ConsoleColor.Yellow)
                                .WriteLine();
                        }
                        else
                        {
                            $"Service for {property.PropertyType.Name} not found.".Colorize(ConsoleColor.DarkGray);
                        }
                    }
                }
            }
        

        // Manual methods to create logic to fetch certain instances from the DI under specified conditions:

        //public GeneralDataService CreateGeneralDataService() =>
        //    _serviceProvider.GetRequiredService<GeneralDataService>();

        //public StudentProfileDataService CreateStudentProfileDataService() =>
        //    _serviceProvider.GetRequiredService<StudentProfileDataService>();

        // Other methods...
    }

}


