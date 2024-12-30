using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IServices
{
    internal interface IBookedSlotsPromptHandler
    {
        public interface IBookedSlotsPromptHandler
        {
            bool ShowSavePrompt(List<SlotEntry> changes);
            bool ShowRevertPrompt();
            string GenerateChangeDetails(List<SlotEntry> changes);
        }
    }
}
