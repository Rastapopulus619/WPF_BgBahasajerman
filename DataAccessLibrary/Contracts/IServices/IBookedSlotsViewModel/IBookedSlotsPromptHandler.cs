using System.Collections.ObjectModel;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel
{
    public interface IBookedSlotsPromptHandler
    {
        bool SavePromptUserChoice(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timeTableDataBackup);
        bool RevertPromptUserChoice(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timeTableDataBackup);
        string GenerateChangeDetails(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timeTableDataBackup);
        string Changes { get; set; }
    }
}
