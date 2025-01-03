using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Contracts.IDataAccess;
using Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryExecutor;
using Bgb_DataAccessLibrary.Contracts.IDataAccess.IQueryLoaders;
using Bgb_DataAccessLibrary.Contracts.IHelpers.ITimeTableHelpers;
using Bgb_DataAccessLibrary.Contracts.IMessages;
using Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.ITimeTableDTOs;
using Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel;
using Bgb_DataAccessLibrary.Contracts.IServices.ICommunication.IEventAggregators;
using Bgb_DataAccessLibrary.Contracts.IServices.IData;
using Bgb_DataAccessLibrary.Contracts.IServices.IDialog;
using Bgb_DataAccessLibrary.Contracts.IServices.ILogging;
using Bgb_DataAccessLibrary.Contracts.IServices.INavigation;
using Bgb_DataAccessLibrary.Contracts.IServices.IResources;
using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.DataAccess.Databases;
using Bgb_DataAccessLibrary.DataAccess.QueryExecutor;
using Bgb_DataAccessLibrary.DataAccess.QueryLoaders;
using Bgb_DataAccessLibrary.Events;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Helpers.TimeTableHelpers;
using Bgb_DataAccessLibrary.Logger;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;
using Bgb_DataAccessLibrary.Services.Communication.EventAggregators;
using BgB_TeachingAssistant.Services;
using BgB_TeachingAssistant.Services.BookedSlotsViewModel;
using BgB_TeachingAssistant.Services.Dialog;
using BgB_TeachingAssistant.Services.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BgB_TeachingAssistant
{
    public static class ServiceRegistration
    {
        public static void RegisterAllServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register configuration
            services.AddSingleton(configuration);

            // Register core services
            RegisterCoreServices(services, configuration);

            // Register data services
            RegisterDataServices(services);

            // Register navigation services
            RegisterNavigationServices(services);

            // Register other application services
            RegisterOtherServices(services);

            // Register Events
            RegisterEvents(services);
        }

        private static void RegisterCoreServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register logging services
            services.AddSingleton<ILoggerService>(sp =>
                new LoggerService(@"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\BgB_TeachingAssistant\Logs"));

            // Register database and query-related services
            services.AddSingleton<IDataAccess, MySqlDataAccess>(sp =>
                new MySqlDataAccess(configuration.GetConnectionString("MySQL"), sp.GetRequiredService<ILoggerService>()));

            services.AddSingleton<IQueryLoader, Bgb_QueryLoader>(sp =>
                new Bgb_QueryLoader(@"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\DataAccessLibrary\DataAccess\BgB_Queries\"));

            services.AddTransient<IQueryExecutor, QueryExecutor>();

            services.AddSingleton<IEventAggregator, EventAggregator>();
        }

        private static void RegisterDataServices(IServiceCollection services)
        {
            // Register data services
            services.AddTransient<IGeneralDataService, GeneralDataService>();
            services.AddTransient<StudentProfileDataService>();
            services.AddTransient<IDataServiceTestClass, DataServiceTestClass>();
            services.AddTransient<IBookedSlotsDataService, BookedSlotsDataService>();

        }

        private static void RegisterNavigationServices(IServiceCollection services)
        {
            services.AddTransient<PackageNavigationService>();
            services.AddSingleton<INavigationService, Bgb_DataAccessLibrary.Services.Navigation.NavigationService>();
            services.AddSingleton<IServiceFactory, ServiceFactory>();
        }

        private static void RegisterOtherServices(IServiceCollection services)
        {

            //TimeTableServices
            services.AddTransient<ITimeTableDataHelper, TimeTableDataHelper>();
            services.AddTransient<ISlotEntrySubscriptionManager, SlotEntrySubscriptionManager>();
            services.AddTransient<ISlotEntryValidator, SlotEntryValidator>();
            services.AddTransient<ITimeTableSaveStateUpdater, TimeTableSaveStateUpdater>();


            //PromptServices
            services.AddSingleton<IPromptService, PromptService>();

            //BookedSlotsViewModelServices
            services.AddTransient<IBookedSlotsPromptHandler, BookedSlotsPromptHandler>();
            services.AddTransient<IBookedSlotsInitializer, BookedSlotsInitializer>();


            // Register ResourceDictionaryLoader as a singleton
            services.AddSingleton<IResourceDictionaryLoader>(sp =>
            {
                var resourcesPath = "C:\\Programmieren\\ProgrammingProjects\\WPF\\WPF_BgBahasajerman\\BgB_TeachingAssistant\\Views\\Resources";
                return new ResourceDictionaryLoader(resourcesPath);
            });            // Register testing service
            services.AddTransient<IMessages, Messages>();

            // Optionally, add other dependencies here as needed
        }
        private static void RegisterEvents(IServiceCollection services)
        {
            // Register testing service
            services.AddTransient<IStudentNameByIDEvent, StudentNameByIDEvent>();
            services.AddTransient<IPopulateStudentPickerEvent, PopulateStudentPickerEvent>();

            // Register other event interfaces similarly
            // Register other event interfaces similarly
            // Register other event interfaces similarly
        }
    }
}