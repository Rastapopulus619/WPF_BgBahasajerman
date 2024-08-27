using Microsoft.Extensions.DependencyInjection; // Add this using directive
using Bgb_DataAccessLibrary.Data.DataServices; // Ensure this is correct
using Bgb_DataAccessLibrary.Factories; // Ensure this is correct


namespace Bgb_DataAccessLibrary.Factories
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public GeneralDataService CreateGeneralDataService() =>
            _serviceProvider.GetRequiredService<GeneralDataService>();

        public StudentProfileDataService CreateStudentProfileDataService() =>
            _serviceProvider.GetRequiredService<StudentProfileDataService>();

        // Other methods...
    }
}



