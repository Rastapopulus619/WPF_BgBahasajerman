using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.QueryLoaders;
using Bgb_DataAccessLibrary.QueryExecutor;
using BgB_TeachingAssistant.ViewModels;
using BgB_TeachingAssistant.Views;
using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Logger;
using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Factories;

namespace BgB_TeachingAssistant
{
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }
        private IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Register IConfiguration
                    services.AddSingleton<IConfiguration>(context.Configuration);

                    // Register your services
                    services
                        .AddSingleton<ILoggerService>(sp => new LoggerService(@"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\BgB_TeachingAssistant\Logs"))
                        .AddSingleton<IDataAccess, MySqlDataAccess>(sp => new MySqlDataAccess(context.Configuration.GetConnectionString("MySQL"), sp.GetRequiredService<ILoggerService>()))
                        .AddSingleton<IQueryLoader, Bgb_QueryLoader>(sp => new Bgb_QueryLoader(@"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\DataAccessLibrary\BgB_Queries"))
                        //.AddTransient<IDataAccess, MySqlDataAccess>() // Use passing IConfiguration automatically and fetching connectionString in the DataAccess class
                        //.AddTransient<IDataAccess, MySqlDataAccess>(sp => new MySqlDataAccess(sp.GetRequiredService<IConfiguration>().GetConnectionString("MySQL")))

                        .AddTransient<IQueryExecutor, QueryExecutor>()
                        .AddTransient<IMessages, Messages>();

                    //.AddTransient<GeneralDataService>()
                    //.AddTransient<StudentProfileDataService>()

                    // Register additional services from ServiceRegistration
                    ServiceRegistration.RegisterDataServices(services, context.Configuration);
                    services.AddSingleton<IServiceFactory, ServiceFactory>();

                    services
                    // Register view models
                        .AddTransient<ApplicationViewModel>()
                        .AddTransient<DashboardViewModel>()
                        .AddTransient<StudentViewModel>()
                        .AddTransient<PackageViewModel>()

                        // Register MainWindow
                        .AddSingleton<ApplicationView>();
                        //.AddSingleton<MainWindow>();

                        
                });

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Resolve your main window (or other services) here
            var appView = _host.Services.GetRequiredService<ApplicationView>();
            // Show the ApplicationView
            appView.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
