using Bgb_DataAccessLibrary.Models.Domain.StudentModels;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IData
{
    public interface IGeneralDataService
    {
        Task<List<string>> GetStudentNamesAsync();
        Task<List<StudentModel>> GetStudentsAsync();
        Task populateStudentPicker();
    }
}