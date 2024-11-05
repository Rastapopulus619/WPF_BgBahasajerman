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

                    ServiceRegistration.RegisterAllServices(services, context.Configuration);

                    ViewModelRegistration.RegisterViewModels(services);

                    // Register MainWindow
                    services.AddSingleton<ApplicationView>();
                        
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
