using System;
using System.Collections.Generic;
using System.Linq;
using static Bgb_DataAccessLibrary.Helpers.ExtensionMethods.StringExtensionMethods;
using Bgb_DataAccessLibrary.Models.Interfaces;
using static Bgb_DataAccessLibrary.Logger.DependencyInjectionLogger;
using Bgb_DataAccessLibrary.Logger;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BgB_TeachingAssistant.Views;

namespace BgB_TeachingAssistant
{
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }
        private IHost _host;

        public App()
        {
            // Build the host with the custom LoggingServiceProviderFactory
            _host = CreateHostBuilder().Build();

            // Note: LoggingServiceProviderFactory handles logging of service registrations

            // Log the PageDescriptors
            LogPageDescriptors(_host.Services);
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

                    // Register all services
                    ServiceRegistration.RegisterAllServices(services, context.Configuration);

                    // Register ViewModels
                    ViewModelRegistration.RegisterViewModels(services);

                    LogStartup(services);


                    // Register MainWindow
                    services.AddSingleton<ApplicationView>();
                });

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Start the host
            await _host.StartAsync();

            // Log the startup completion
            ColorizeLine(("[App] Application started.",ConsoleColor.Blue));

            // Resolve and show the main application view
            var appView = _host.Services.GetRequiredService<ApplicationView>();
            DependencyInjectionLogger.LogResolution(typeof(ApplicationView), appView);

            appView.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            // Log the application shutdown
            ColorizeLine(("[App] Application shutting down...", ConsoleColor.Blue));

            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }

        private void LogPageDescriptors(IServiceProvider serviceProvider)
        {
            var pageDescriptors = serviceProvider.GetService<IEnumerable<IPageDescriptor>>();
            if (pageDescriptors != null)
            {
                Console.WriteLine("=== Registered Page Descriptors ===");
                foreach (var descriptor in pageDescriptors)
                {
                    ColorizeLine(
                    ("Reg. ", ConsoleColor.Magenta),
                    ($"{descriptor.Name} -> ", ConsoleColor.DarkGray),
                    ($"{descriptor.ViewModelType.Name}", ConsoleColor.White));

                    //Console.WriteLine($"Name: {descriptor.Name}, ViewModelType: {descriptor.ViewModelType.Name}");
                }
                Console.WriteLine("===================================");
            }
        }
    }
}
