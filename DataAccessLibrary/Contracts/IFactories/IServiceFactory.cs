
namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IServiceFactory
    {
        T GetService<T>();  // For generic retrieval by type
        object GetService(Type serviceType); // Add this line
        void ConfigureServicesFor(object viewModel); // Method to configure services for specific view models

        //GeneralDataService CreateGeneralDataService();
        //StudentProfileDataService CreateStudentProfileDataService();

        // Other services...
    }
}
