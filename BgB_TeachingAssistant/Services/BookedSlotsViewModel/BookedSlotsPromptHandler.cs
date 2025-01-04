using System.Collections.ObjectModel;
using System.Text;
using Bgb_DataAccessLibrary.Contracts.IServices.IBookedSlotsViewModel;
using Bgb_DataAccessLibrary.Contracts.IServices.IDialog;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace BgB_TeachingAssistant.Services.BookedSlotsViewModel
{
    public class BookedSlotsPromptHandler : IBookedSlotsPromptHandler
    {
        private readonly IPromptService _promptService;
        public string Changes { get; set; }
        public BookedSlotsPromptHandler(IPromptService promptService)
        {
            _promptService = promptService;
        }

        public bool SavePromptUserChoice(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timeTableDataBackup)
        {
            Changes = GenerateChangeDetails(changes, timeTableDataBackup);
            return _promptService.ShowOkCancelPrompt(
                "Confirm Save",
                $"Are you sure you want to save these changes?\n\n{Changes}");
        }

        public bool RevertPromptUserChoice(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timeTableDataBackup)
        {
            Changes = GenerateChangeDetails(changes, timeTableDataBackup);
            return _promptService.ShowOkCancelPrompt(
                "Confirm Revert",
                "Are you sure you want to revert changes?");
        }

        public string GenerateChangeDetails(List<SlotEntry> changes, ObservableCollection<TimeTableRow> timeTableDataBackup)
        {
            if (changes == null || !changes.Any())
                return "No changes detected.";

            if (timeTableDataBackup == null)
                throw new ArgumentNullException(nameof(timeTableDataBackup), "TimeTable data backup is required.");

            var details = new StringBuilder();

            foreach (var slot in changes)
            {
                var originalSlot = timeTableDataBackup
                    .SelectMany(row => new[] { row.Montag, row.Dienstag, row.Mittwoch, row.Donnerstag, row.Freitag, row.Samstag, row.Sonntag })
                    .FirstOrDefault(s => s?.SlotID == slot.SlotID);

                if (originalSlot != null)
                {
                    details.AppendLine(
                        $"{slot.WeekdayName}, {slot.Time}:\t[{originalSlot.Name}]    ->    [{slot.Name}]");
                }
            }
            return details.ToString();
        }
    }
}