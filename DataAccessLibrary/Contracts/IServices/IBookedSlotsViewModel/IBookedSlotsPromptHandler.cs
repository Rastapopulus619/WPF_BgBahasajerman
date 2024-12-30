using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel
{
    public interface IBookedSlotsPromptHandler
    {
        bool SavePromptUserChoice(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timetableDataBackup);
        bool RevertPromptUserChoice(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timetableDataBackup);
        string GenerateChangeDetails(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timetableDataBackup);
        string Changes { get; set; }
    }
}
