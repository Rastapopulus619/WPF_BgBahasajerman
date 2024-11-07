using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Factories;
using Bgb_DataAccessLibrary.Logger;
using Bgb_DataAccessLibrary.QueryExecutor;
using Bgb_DataAccessLibrary.QueryLoaders;
using Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators;
using BgB_TeachingAssistant.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            new Bgb_QueryLoader(@"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\DataAccessLibrary\BgB_Queries"));

        services.AddTransient<IQueryExecutor, QueryExecutor>();

        services.AddSingleton<IEventAggregator, EventAggregator>();
    }

    private static void RegisterDataServices(IServiceCollection services)
    {
        // Register data services
        services.AddTransient<GeneralDataService>();
        services.AddTransient<StudentProfileDataService>();
    }

    private static void RegisterNavigationServices(IServiceCollection services)
    {
        // Register navigation services
        services.AddTransient<PackageNavigationService>();
        services.AddSingleton<IServiceFactory, ServiceFactory>();
    }

    private static void RegisterOtherServices(IServiceCollection services)
    {
        // Register testing service
        services.AddTransient<IMessages, Messages>();

        services.AddTransient<IDataServiceTestClass, DataServiceTestClass>();
        // Optionally, add other dependencies here as needed
    }
}

