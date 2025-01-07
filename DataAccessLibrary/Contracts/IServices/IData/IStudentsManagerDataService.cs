using Bgb_DataAccessLibrary.Contracts.IModels.IDisplayModels;
using System.Collections.ObjectModel;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IData;

public interface IStudentsManagerDataService
{
    /// <summary>
    /// Fetches all available study statuses from the database.
    /// </summary>
    /// <returns>An ObservableCollection of study status strings.</returns>
    Task<ObservableCollection<string>> GetStudyStatuses();

    /// <summary>
    /// Fetches all student data from the database.
    /// </summary>
    /// <returns>An ObservableCollection of IStudentManagerDisplayStudentModel instances.</returns>
    Task<ObservableCollection<IStudentManagerDisplayStudentModel>> GetAllStudentDisplayDataAsync();

    /// <summary>
    /// Fetches a single student's data by their StudentID.
    /// </summary>
    /// <param name="studentID">The ID of the student.</param>
    /// <returns>An instance of IStudentManagerDisplayStudentModel with the student's details.</returns>
    Task<IStudentManagerDisplayStudentModel> GetStudentDisplayDataByStudentID(int studentID);
}