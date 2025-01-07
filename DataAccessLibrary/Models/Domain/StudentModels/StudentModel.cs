using Bgb_DataAccessLibrary.Contracts.IModels.IDomain.IStudentModels;

namespace Bgb_DataAccessLibrary.Models.Domain.StudentModels
{
    public class StudentModel : IStudentModel
    {
        public int StudentID { get; set; }
        public int StudentNumber { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

}
