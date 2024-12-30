using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Contracts.IModels.IStudentModels;
using Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel;
using Bgb_DataAccessLibrary.Contracts.IServices.IData;
using Bgb_DataAccessLibrary.Models.Domain.StudentModels;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

public class BookedSlotsInitializer : IBookedSlotsInitializer
{
    private readonly IBookedSlotsDataService _dataService;

    public BookedSlotsInitializer(IBookedSlotsDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task<ObservableCollection<TimeTableRow>> FetchTimeTableDataAsync()
    {
        try
        {
            return new ObservableCollection<TimeTableRow>(await _dataService.GetBookedSlotsAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching timetable data: {ex.Message}");
            throw;
        }
    }

    public async Task<ObservableCollection<IStudentModel>> FetchStudentListAsync()
    {
        try
        {
            return new ObservableCollection<IStudentModel>(await _dataService.GetStudentsAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching student list: {ex.Message}");
            throw;
        }
    }
}