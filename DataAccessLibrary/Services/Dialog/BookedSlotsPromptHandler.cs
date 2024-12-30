using System.Text;
using Bgb_DataAccessLibrary.Contracts.IServices;
using Bgb_DataAccessLibrary.Contracts.IServices.IDialog;
using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Services.Dialog
{
    public class BookedSlotsPromptHandler : IBookedSlotsPromptHandler
    {
        private readonly IPromptService _promptService;
        public BookedSlotsPromptHandler(IPromptService promptService)
        {
            _promptService = promptService;
        }

        public bool ShowSavePrompt(List<SlotEntry> changes)
        {
            var changeDetails = GenerateChangeDetails(changes);
            return _promptService.ShowOkCancelPrompt(
                "Confirm Save",
                $"Are you sure you want to save these changes?\n\n{changeDetails}");
        }

        public bool ShowRevertPrompt()
        {
            return _promptService.ShowOkCancelPrompt(
                "Confirm Revert",
                "Are you sure you want to revert changes?");
        }

        public string GenerateChangeDetails(List<SlotEntry> changes)
        {
            if (changes == null || !changes.Any())
                return "No changes detected.";

            var details = new StringBuilder();
            foreach (var slot in changes)
            {
                details.AppendLine($"{slot.WeekdayName}, {slot.Time}: [{slot.Name}]");
            }
            return details.ToString();
        }
    }
}