using Bgb_DataAccessLibrary.Contracts;

namespace Bgb_DataAccessLibrary.Models.StudentModels
{
    public class StudentPickerStudentModel : IStudentPickerStudentModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
    }
}
