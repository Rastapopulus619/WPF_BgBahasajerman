using Bgb_DataAccessLibrary.Models.Domain.StudentModels;
using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Contracts.IModels.IStudentModels;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel
{
    public interface IBookedSlotsInitializer
    {
        Task<ObservableCollection<TimeTableRow>> FetchTimeTableDataAsync();
        Task<ObservableCollection<IStudentModel>> FetchStudentListAsync();
    }

}
