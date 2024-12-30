using Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.ITimeTableDTOs;

namespace Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs
{
    public class TimeTableRow : ITimeTableRow
    {
        public string Zeiten { get; set; }    // Time column

        public SlotEntry Montag { get; set; } = new SlotEntry();
        public SlotEntry Dienstag { get; set; } = new SlotEntry();
        public SlotEntry Mittwoch { get; set; } = new SlotEntry();
        public SlotEntry Donnerstag { get; set; } = new SlotEntry();
        public SlotEntry Freitag { get; set; } = new SlotEntry();
        public SlotEntry Samstag { get; set; } = new SlotEntry();
        public SlotEntry Sonntag { get; set; } = new SlotEntry();
    }
}
