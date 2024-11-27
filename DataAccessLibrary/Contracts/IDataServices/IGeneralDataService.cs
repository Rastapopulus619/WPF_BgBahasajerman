using Bgb_DataAccessLibrary.Models.StudentModels;

namespace Bgb_DataAccessLibrary.Contracts
{
    public interface IGeneralDataService
    {
        Task<List<string>> GetStudentNamesAsync();
        Task<List<StudentModel>> GetStudentsAsync();
        Task populateStudentPicker();
    }
}