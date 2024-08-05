using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BgB_TeachingAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; }

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Access the connection string
            var connectionString = Configuration.GetConnectionString("MySQL");
            // Use the connection string to set up your database connection
        }
    }
}
