using Bgb_DataAccessLibrary.Models.StudentModels;

namespace Bgb_DataAccessLibrary.Data.DataServices
{
    public interface IGeneralDataService
    {
        Task<List<string>> GetStudentNamesAsync();
        Task<List<StudentModel>> GetStudentsAsync();
        Task populateStudentPicker();
    }
}