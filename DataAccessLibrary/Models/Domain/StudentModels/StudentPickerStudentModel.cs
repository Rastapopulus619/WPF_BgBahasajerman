using Bgb_DataAccessLibrary.Contracts;
using Bgb_DataAccessLibrary.Contracts.IModels.IDomain.IStudentModels;

namespace Bgb_DataAccessLibrary.Models.Domain.StudentModels
{
    public class StudentPickerStudentModel : IStudentPickerStudentModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
    }
}
