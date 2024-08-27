using Bgb_DataAccessLibrary.Data.DataServices;
using Bgb_DataAccessLibrary.Databases;
using Bgb_DataAccessLibrary.Logger;
using Bgb_DataAccessLibrary.QueryLoaders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    public static void RegisterDataServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add your bundled services here
        services.AddTransient<GeneralDataService>();
        services.AddTransient<StudentProfileDataService>();
    }
}
                        
                        