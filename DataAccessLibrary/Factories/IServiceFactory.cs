using Bgb_DataAccessLibrary.Data.DataServices;

namespace Bgb_DataAccessLibrary.Factories
{
    public interface IServiceFactory
    {
        T GetService<T>();  // For generic retrieval by type
        void ConfigureServicesFor(object viewModel); // Method to configure services for specific view models

        GeneralDataService CreateGeneralDataService();
        StudentProfileDataService CreateStudentProfileDataService();

        // Other services...
    }
}
