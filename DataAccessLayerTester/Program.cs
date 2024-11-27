// DataAccessTesterConsoleApp/Program.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bgb_DataAccessLibrary;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.QueryLoaders;
using Bgb_DataAccessLibrary.QueryExecutor;
using Org.BouncyCastle.Asn1;
using System;
using System.Threading.Tasks;
using DataAccessLayerTester;
using Bgb_DataAccessLibrary.Logger;
using Bgb_DataAccessLibrary.Contracts.IDatabases;
using Bgb_DataAccessLibrary.Contracts.ILogger;
using Bgb_DataAccessLibrary.Contracts.IQueryExecutor;
using Bgb_DataAccessLibrary.Contracts.IQueryLoaders;
using Bgb_DataAccessLibrary.Contracts.IMessages;

namespace DataAccessLayerTester
{
    class Program
    {
        public static IConfiguration Configuration;
        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                services.GetRequiredService<App>().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occurred: {ex.Message}");
                Console.ReadLine();
            }
        }
        static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IConfiguration>(context.Configuration);

                services
                        .AddTransient<App>()
                        .AddSingleton<ILoggerService>(sp => new LoggerService(@"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\DataAccessLayerTester\Logs"))
                        .AddTransient<IMessages, Messages>()
                        //.AddTransient<IDataAccess, MySqlDataAccess>() // use passing IConfiguration automatically and fetching connectionString in the DataAccess class
                        //.AddTransient<IDataAccess, MySqlDataAccess>(sp => new MySqlDataAccess(sp.GetRequiredService<IConfiguration>().GetConnectionString("MySQL")))
                        .AddSingleton<IDataAccess, MySqlDataAccess>(sp => new MySqlDataAccess(context.Configuration.GetConnectionString("MySQL"), sp.GetRequiredService<ILoggerService>()))
                        .AddSingleton<IQueryLoader, Bgb_QueryLoader>(sp => new Bgb_QueryLoader(@"C:\Programmieren\ProgrammingProjects\WPF\WPF_BgBahasajerman\DataAccessLibrary\BgB_Queries"))
                        .AddTransient<IQueryExecutor, QueryExecutor>();
            });

        /*
        * .AddTransient<IDataAccess, MySqlDataAccess>(): 
        * 
        * This option automatically uses the IConfiguration 
        * provided by the DI container, simplifying the registration. 
        * It's a good choice if you're sticking 
        * with dependency injection for configuration management.
        * 
        * *** for lightweight simple usage of one connection ***
        * .AddTransient<IDataAccess, MySqlDataAccess>(sp => new MySqlDataAccess(sp.GetRequiredService<IConfiguration>().GetConnectionString("MySQL"))): 
        * 
        * This provides more control by explicitly passing the connection string, 
        * which could be useful in scenarios where you might want to manipulate 
        * or validate the connection string before passing it.
        * 
        * **
        * ****** for lightweight simple usage of one connection ******
        * **
        * 
        * .AddSingleton<IDataAccess, MySqlDataAccess>(sp => new MySqlDataAccess(context.Configuration.GetConnectionString("MySQL"))): 
        * 
        * This is similar to the second option but registers the IDataAccess as a singleton,
        * meaning the same instance is reused throughout the application's lifetime.
        * This can be beneficial if the IDataAccess object is expensive to create
        * or if you need to maintain a consistent state."
        * 
        * 
        */


    }
}