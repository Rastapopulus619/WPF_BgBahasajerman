namespace Bgb_DataAccessLibrary.Contracts.IModels.IStudentModels;

public interface IStudentModel
{
    int StudentID { get; set; }
    int StudentNumber { get; set; }
    string Name { get; set; }
    string Title { get; set; }
}