using Bgb_DataAccessLibrary.Models.DTOs.TimeTableDTOs;

namespace Bgb_DataAccessLibrary.Contracts.IModels.IDTOs.ITimeTableDTOs;

public interface ITimeTableRow
{
    string Zeiten { get; set; } // Time column
    SlotEntry Montag { get; set; }
    SlotEntry Dienstag { get; set; }
    SlotEntry Mittwoch { get; set; }
    SlotEntry Donnerstag { get; set; }
    SlotEntry Freitag { get; set; }
    SlotEntry Samstag { get; set; }
    SlotEntry Sonntag { get; set; }
}